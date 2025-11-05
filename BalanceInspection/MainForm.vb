Imports System.Windows.Forms

''' <summary>
''' メインフォーム
''' </summary>
Public Class MainForm
    Inherits Form

    ' サービス
    Private _appConfig As AppConfig
    Private _cardLoader As CardConditionLoader
    Private _balanceManager As BalanceManager
    Private _logManager As LogManager
    Private _employeeLoader As EmployeeLoader
    Private _currentCondition As CardCondition
    Private _isSearchingEmployee As Boolean = False

    Public Sub New()
        InitializeComponent()
        InitializeEventHandlers()
        InitializeServices()
    End Sub

    ''' <summary>
    ''' イベントハンドラーを登録
    ''' </summary>
    Private Sub InitializeEventHandlers()
        AddHandler txtEmployeeNo.KeyPress, AddressOf TextBox_KeyPress
        AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
        AddHandler txtCardNo.KeyPress, AddressOf TextBox_KeyPress
        AddHandler txtCardNo.TextChanged, AddressOf TxtCardNo_TextChanged
        AddHandler txtCardNo.Leave, AddressOf TxtCardNo_Leave
        AddHandler btnVerify.Click, AddressOf BtnVerify_Click
        AddHandler btnCancel.Click, AddressOf BtnCancel_Click
    End Sub

    ''' <summary>
    ''' サービスを初期化
    ''' </summary>
    Private Sub InitializeServices()
        Try
            ' 設定読み込み
            _appConfig = ConfigLoader.Load()
            
            ' ログマネージャー初期化
            Dim logDir As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.LogDirectory)
            _logManager = New LogManager(logDir)
            
            ' カード条件読み込み
            Dim csvPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.CardConditionCsvPath)
            _cardLoader = New CardConditionLoader(csvPath)
            
            ' 従業員ローダー初期化
            Dim employeeCsvPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.EmployeeCsvPath)
            _employeeLoader = New EmployeeLoader(employeeCsvPath, _logManager)
            
            ' バランスマネージャー初期化
            _balanceManager = New BalanceManager(_appConfig)
            
            ' ポートを開く（エラーは後で処理）
            Try
                _balanceManager.OpenAll()
            Catch ex As Exception
                ' 開発/テスト時はポートが無くてもOK
                _logManager.WriteErrorLog("ポートオープン時の警告: " & ex.Message)
            End Try
            
        Catch ex As Exception
            MessageBox.Show("初期化エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' 数値のみ入力を許可
    ''' </summary>
    Private Sub TextBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' 数字、バックスペース、アルファベット(小文字)のみ許可
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsLetter(e.KeyChar) Then
            e.Handled = True
        End If
        
        ' 大文字を小文字に変換
        If Char.IsUpper(e.KeyChar) Then
            e.KeyChar = Char.ToLower(e.KeyChar)
        End If
    End Sub

    ''' <summary>
    ''' カードNo入力後の処理（フォーカス離脱時）
    ''' </summary>
    Private Sub TxtCardNo_Leave(sender As Object, e As EventArgs)
        Dim cardNo As String = txtCardNo.Text.Trim()
        
        If String.IsNullOrEmpty(cardNo) Then
            Return
        End If
        
        ' 6桁でない場合はエラーメッセージを表示
        If cardNo.Length <> 6 Then
            ShowMessage("カードNoは6桁で入力してください", Color.Red)
            ClearConditionLabels()
            btnVerify.Enabled = False
            Return
        End If

        ' 6桁の場合で、まだ処理されていない場合のみ処理（TextChangedで処理済みの場合は重複を避ける）
        If _currentCondition Is Nothing Then
            ProcessCardNoInput()
        End If
    End Sub

    ''' <summary>
    ''' 条件をラベルに表示
    ''' </summary>
    Private Sub DisplayCondition(condition As CardCondition)
        ' 必要枚数を表示
        lblPre10mmRequired.Text = condition.Pre10mm.ToString() & "枚"
        lblPost1mmRequired.Text = condition.Post1mm.ToString() & "枚"
        lblPost5mmRequired.Text = condition.Post5mm.ToString() & "枚"
        lblPost10mmRequired.Text = condition.Post10mm.ToString() & "枚"
        
        ' エッジガードと気泡緩衝材は0の場合"不要"を表示（他の列はDisplayInitialBalanceReadingsで設定）
        If condition.EdgeGuard = 0 Then
            lblEdgeRequired.Text = "不要"
        Else
            lblEdgeRequired.Text = condition.EdgeGuard.ToString() & "枚"
        End If
        
        If condition.BubbleInterference = 0 Then
            lblBubbleRequired.Text = "不要"
        Else
            lblBubbleRequired.Text = condition.BubbleInterference.ToString("D2") & "枚"
        End If

        ' 秤入力時、確保枚数、現在枚数、判定は初期検量後に設定される
        lblPre10mmRemaining.Text = ""
        lblPre10mmSecured.Text = ""
        lblPre10mmUsed.Text = ""
        lblPre10mmJudgment.Text = ""
        lblPost1mmRemaining.Text = ""
        lblPost1mmSecured.Text = ""
        lblPost1mmUsed.Text = ""
        lblPost1mmJudgment.Text = ""
        lblPost5mmRemaining.Text = ""
        lblPost5mmSecured.Text = ""
        lblPost5mmUsed.Text = ""
        lblPost5mmJudgment.Text = ""
        lblPost10mmRemaining.Text = ""
        lblPost10mmSecured.Text = ""
        lblPost10mmUsed.Text = ""
        lblPost10mmJudgment.Text = ""
        
        ' エッジガードと気泡緩衝材が0でない場合のみクリア（0の場合はDisplayInitialBalanceReadingsで"不要"設定）
        If condition.EdgeGuard <> 0 Then
            lblEdgeRemaining.Text = ""
            lblEdgeSecured.Text = ""
            lblEdgeUsed.Text = ""
            lblEdgeJudgment.Text = ""
        End If
        
        If condition.BubbleInterference <> 0 Then
            lblBubbleRemaining.Text = ""
            lblBubbleSecured.Text = ""
            lblBubbleUsed.Text = ""
            lblBubbleJudgment.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' カード情報をラベルに表示
    ''' </summary>
    Private Sub DisplayCardInfo(condition As CardCondition)
        ' カードNoを表示
        lblCardNoDisplayValue.Text = condition.CardNo

        ' 品名、枚数、所在を表示
        lblProductNameValue.Text = If(String.IsNullOrEmpty(condition.ProductName), "", condition.ProductName)
        lblQuantityValue.Text = If(condition.Quantity > 0, condition.Quantity.ToString(), "")
        lblLocationValue.Text = If(String.IsNullOrEmpty(condition.Location), "", condition.Location)
    End Sub

    ''' <summary>
    ''' 初回計測値を表示（秤 入力時列）
    ''' </summary>
    Private Sub DisplayInitialBalanceReadings(condition As CardCondition)
        Dim readings As Dictionary(Of String, Double) = _balanceManager.InitialReadings

        ' 秤(9001)の値を投入前10mmと投入後10mmに表示
        If readings.ContainsKey("Pre_10mm") Then
            lblPre10mmRemaining.Text = readings("Pre_10mm").ToString("F1") & "枚"
            lblPost10mmRemaining.Text = readings("Pre_10mm").ToString("F1") & "枚"
        End If

        ' 秤(9002)の値を投入後1mmに表示
        If readings.ContainsKey("Post_1mm") Then
            lblPost1mmRemaining.Text = readings("Post_1mm").ToString("F1") & "枚"
        End If

        ' 秤(9003)の値を投入後5mmに表示
        If readings.ContainsKey("Post_5mm") Then
            lblPost5mmRemaining.Text = readings("Post_5mm").ToString("F1") & "枚"
        End If

        ' エッジガードが0の場合は全列に"不要"を表示
        If condition.EdgeGuard = 0 Then
            lblEdgeRequired.Text = "不要"
            lblEdgeRemaining.Text = "不要"
            lblEdgeSecured.Text = "不要"
            lblEdgeUsed.Text = "不要"
            lblEdgeJudgment.Text = "不要"
        End If

        ' 気泡緩衝材が0の場合は全列に"不要"を表示
        If condition.BubbleInterference = 0 Then
            lblBubbleRequired.Text = "不要"
            lblBubbleRemaining.Text = "不要"
            lblBubbleSecured.Text = "不要"
            lblBubbleUsed.Text = "不要"
            lblBubbleJudgment.Text = "不要"
        End If
    End Sub

    ''' <summary>
    ''' 照合時計測値を表示（AFTER列）し、確保枚数（差分）を計算して表示
    ''' </summary>
    Private Sub DisplayVerificationReadings(condition As CardCondition)
        Dim verificationReadings As Dictionary(Of String, Double) = _balanceManager.VerificationReadings
        Dim initialReadings As Dictionary(Of String, Double) = _balanceManager.InitialReadings

        ' 秤(9001)の値を投入前10mmと投入後10mmのAFTER列に表示し、確保枚数を計算
        If verificationReadings.ContainsKey("Pre_10mm") And initialReadings.ContainsKey("Pre_10mm") Then
            Dim afterValue As Double = verificationReadings("Pre_10mm")
            Dim beforeValue As Double = initialReadings("Pre_10mm")
            Dim secured As Double = beforeValue - afterValue
            
            ' 投入前10mm
            lblPre10mmSecured.Text = afterValue.ToString("F1") & "枚"
            lblPre10mmUsed.Text = secured.ToString("F1") & "枚"
            
            ' 投入後10mm
            lblPost10mmSecured.Text = afterValue.ToString("F1") & "枚"
            lblPost10mmUsed.Text = secured.ToString("F1") & "枚"
        End If

        ' 秤(9002)の値を投入後1mmのAFTER列に表示し、確保枚数を計算
        If verificationReadings.ContainsKey("Post_1mm") And initialReadings.ContainsKey("Post_1mm") Then
            Dim afterValue As Double = verificationReadings("Post_1mm")
            Dim beforeValue As Double = initialReadings("Post_1mm")
            Dim secured As Double = beforeValue - afterValue
            
            lblPost1mmSecured.Text = afterValue.ToString("F1") & "枚"
            lblPost1mmUsed.Text = secured.ToString("F1") & "枚"
        End If

        ' 秤(9003)の値を投入後5mmのAFTER列に表示し、確保枚数を計算
        If verificationReadings.ContainsKey("Post_5mm") And initialReadings.ContainsKey("Post_5mm") Then
            Dim afterValue As Double = verificationReadings("Post_5mm")
            Dim beforeValue As Double = initialReadings("Post_5mm")
            Dim secured As Double = beforeValue - afterValue
            
            lblPost5mmSecured.Text = afterValue.ToString("F1") & "枚"
            lblPost5mmUsed.Text = secured.ToString("F1") & "枚"
        End If
    End Sub

    ''' <summary>
    ''' 判定を表示（必要枚数＝確保枚数でOK、それ以外はNG）
    ''' </summary>
    Private Sub DisplayJudgment(condition As CardCondition)
        ' 投入前10mmと投入後10mmの判定（同じ秤9001を使用）
        ' 両方の必要枚数の合計が確保枚数と一致していればOK
        If Not String.IsNullOrEmpty(lblPre10mmUsed.Text) AndAlso lblPre10mmUsed.Text <> "不要" Then
            Dim secured As Double = Double.Parse(lblPre10mmUsed.Text.Replace("枚", ""))
            Dim totalRequired As Integer = condition.Pre10mm + condition.Post10mm
            Dim isOk As Boolean = Math.Round(secured) = totalRequired
            
            ' 投入前10mmの判定
            lblPre10mmJudgment.Text = If(isOk, "OK", "NG")
            lblPre10mmJudgment.ForeColor = If(isOk, Color.Green, Color.Red)
            
            ' 投入後10mmの判定（同じ結果）
            lblPost10mmJudgment.Text = If(isOk, "OK", "NG")
            lblPost10mmJudgment.ForeColor = If(isOk, Color.Green, Color.Red)
        End If

        ' 投入後1mmの判定
        If Not String.IsNullOrEmpty(lblPost1mmUsed.Text) AndAlso lblPost1mmUsed.Text <> "不要" Then
            Dim secured As Double = Double.Parse(lblPost1mmUsed.Text.Replace("枚", ""))
            lblPost1mmJudgment.Text = If(Math.Round(secured) = condition.Post1mm, "OK", "NG")
            lblPost1mmJudgment.ForeColor = If(lblPost1mmJudgment.Text = "OK", Color.Green, Color.Red)
        End If

        ' 投入後5mmの判定
        If Not String.IsNullOrEmpty(lblPost5mmUsed.Text) AndAlso lblPost5mmUsed.Text <> "不要" Then
            Dim secured As Double = Double.Parse(lblPost5mmUsed.Text.Replace("枚", ""))
            lblPost5mmJudgment.Text = If(Math.Round(secured) = condition.Post5mm, "OK", "NG")
            lblPost5mmJudgment.ForeColor = If(lblPost5mmJudgment.Text = "OK", Color.Green, Color.Red)
        End If

        ' エッジガードの判定（必要枚数が0でない場合）
        If condition.EdgeGuard <> 0 Then
            ' エッジガードは天秤計測対象外のため、現時点では判定なし
            lblEdgeJudgment.Text = "-"
        End If

        ' 気泡緩衝材の判定（必要枚数が0でない場合）
        If condition.BubbleInterference <> 0 Then
            ' 気泡緩衝材は天秤計測対象外のため、現時点では判定なし
            lblBubbleJudgment.Text = "-"
        End If
    End Sub

    ''' <summary>
    ''' 照合ボタンクリック
    ''' </summary>
    Private Sub BtnVerify_Click(sender As Object, e As EventArgs)
        If _currentCondition Is Nothing Then
            Return
        End If

        Dim employeeNo As String = txtEmployeeNo.Text.Trim()

        ' 従業員Noチェック
        If employeeNo.Length <> 6 Then
            ShowMessage("従業員Noは6桁で入力してください", Color.Red)
            Return
        End If

        Try
            ' 照合時計測
            _balanceManager.PerformVerificationReading()

            ' 照合時の値をAFTER列に表示し、確保枚数(差分)を計算して表示
            DisplayVerificationReadings(_currentCondition)

            ' 判定を表示
            DisplayJudgment(_currentCondition)

            ' 照合判定（画面上の4項目の判定が全てOKかチェック）
            Dim isValid As Boolean = ValidateAllJudgments()

            If isValid Then
                ' OK: ログ出力
                _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "OK")
                ShowMessage("検査合格", Color.Green)
                btnVerify.Enabled = False
                ' 照合OK時は両方の入力欄をクリアして活性化
                txtEmployeeNo.Text = ""
                txtCardNo.Text = ""
                txtEmployeeNo.Enabled = True
                txtCardNo.Enabled = True
                txtEmployeeNo.Focus()
            Else
                ' NG: 不一致を表示
                ShowMessage("NG:10mm:-1≠1,1mm:-2≠2", Color.Red)
                _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "NG")
            End If

        Catch ex As Exception
            ShowMessage("照合エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("照合時エラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 画面上の4項目の判定が全てOKかチェック
    ''' </summary>
    Private Function ValidateAllJudgments() As Boolean
        ' 投入前10mm、投入後1mm、投入後5mm、投入後10mmの判定が全て"OK"であることを確認
        Return lblPre10mmJudgment.Text = "OK" AndAlso
               lblPost1mmJudgment.Text = "OK" AndAlso
               lblPost5mmJudgment.Text = "OK" AndAlso
               lblPost10mmJudgment.Text = "OK"
    End Function

    ''' <summary>
    ''' 差分と予定数を照合（旧ロジック - 現在は使用していない）
    ''' </summary>
    Private Function ValidateDifferences(differences As Dictionary(Of String, Double), condition As CardCondition) As Boolean
        Dim isValid As Boolean = True

        ' Pre_10mm
        If differences.ContainsKey("Pre_10mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Pre_10mm")))
            If diff <> condition.Pre10mm Then
                isValid = False
            End If
        Else
            isValid = False
        End If

        ' Post_1mm
        If differences.ContainsKey("Post_1mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_1mm")))
            If diff <> condition.Post1mm Then
                isValid = False
            End If
        Else
            isValid = False
        End If

        ' Post_5mm
        If differences.ContainsKey("Post_5mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_5mm")))
            If diff <> condition.Post5mm Then
                isValid = False
            End If
        Else
            isValid = False
        End If

        Return isValid
    End Function

    ''' <summary>
    ''' NG時のメッセージを構築
    ''' </summary>
    Private Function BuildNgMessage(differences As Dictionary(Of String, Double), condition As CardCondition) As String
        Dim errors As New List(Of String)()

        If differences.ContainsKey("Pre_10mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Pre_10mm")))
            If diff <> condition.Pre10mm Then
                errors.Add("10mm:" & diff.ToString() & "≠" & condition.Pre10mm.ToString())
            End If
        End If

        If differences.ContainsKey("Post_1mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_1mm")))
            If diff <> condition.Post1mm Then
                errors.Add("1mm:" & diff.ToString() & "≠" & condition.Post1mm.ToString())
            End If
        End If

        If differences.ContainsKey("Post_5mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_5mm")))
            If diff <> condition.Post5mm Then
                errors.Add("5mm:" & diff.ToString() & "≠" & condition.Post5mm.ToString())
            End If
        End If

        Dim message As String = "NG:" & String.Join(",", errors)

        ' 30文字制限
        If message.Length > 30 Then
            message = message.Substring(0, 27) & "..."
        End If

        Return message
    End Function

    ''' <summary>
    ''' キャンセルボタンクリック
    ''' </summary>
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs)
        ResetForm()
    End Sub

    ''' <summary>
    ''' フォームをリセット
    ''' </summary>
    Private Sub ResetForm()
        txtEmployeeNo.Text = ""
        txtCardNo.Text = ""
        txtCardNo.Enabled = False  ' カードNoを非活性化
        lblEmployeeNameValue.Text = ""  ' 従業員名をクリア
        ' 条件テーブルをクリア
        ClearConditionLabels()
        ' カード情報をクリア
        lblCardNoDisplayValue.Text = ""
        lblProductNameValue.Text = ""
        lblQuantityValue.Text = ""
        lblLocationValue.Text = ""
        ShowMessage("従業員Noを入力してください", Color.Black)
        btnVerify.Enabled = False
        _currentCondition = Nothing
        txtEmployeeNo.Focus()  ' 従業員Noにフォーカス
    End Sub

    ''' <summary>
    ''' メッセージを表示
    ''' </summary>
    Private Sub ShowMessage(message As String, color As Color)
        If message.Length > 30 Then
            message = message.Substring(0, 30)
        End If
        lblMessage.Text = message
        lblMessage.ForeColor = color
    End Sub

    ''' <summary>
    ''' フォームクローズ時
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        MyBase.OnFormClosing(e)

        Try
            If _balanceManager IsNot Nothing Then
                _balanceManager.CloseAll()
            End If
        Catch ex As Exception
            ' エラーは無視
        End Try
    End Sub

    ''' <summary>
    ''' 従業員No入力時の処理
    ''' </summary>
    Private Async Sub TxtEmployeeNo_TextChanged(sender As Object, e As EventArgs)
        Dim employeeNo As String = txtEmployeeNo.Text.Trim()

        ' 従業員名をクリア
        lblEmployeeNameValue.Text = ""
        lblEmployeeNameValue.ForeColor = Color.Black

        If employeeNo.Length = 6 Then
            ' 6桁が数字かチェック
            If Not IsNumeric(employeeNo) Then
                ' 数字以外の場合：エラー表示して入力欄をクリア
                lblEmployeeNameValue.Text = "該当するユーザがありません。"
                lblEmployeeNameValue.ForeColor = Color.Red
                ShowMessage("従業員Noは数字6桁で入力してください", Color.Red)
                
                txtCardNo.Enabled = False
                txtCardNo.Text = ""
                ClearConditionLabels()
                btnVerify.Enabled = False
                
                ' 入力欄をクリアしてフォーカスを戻す
                txtEmployeeNo.Text = ""
                txtEmployeeNo.Focus()
                Return
            End If

            ' 検索中フラグをチェック（二重実行防止）
            If _isSearchingEmployee Then
                Return
            End If

            _isSearchingEmployee = True

            Try
                ' ローディング表示
                ShowMessage("読み込み中…", Color.Black)
                lblEmployeeNameValue.Text = "検索中..."

                ' 非同期で従業員を検索
                Dim employeeName As String = Await _employeeLoader.SearchAsync(employeeNo)

                If employeeName IsNot Nothing Then
                    ' 該当あり：氏名を表示
                    lblEmployeeNameValue.Text = employeeName
                    lblEmployeeNameValue.ForeColor = Color.Black

                    ' カードNoを活性化してフォーカス移動
                    txtCardNo.Enabled = True
                    txtCardNo.Focus()
                    ShowMessage("カードNoを入力してください", Color.Black)
                Else
                    ' 該当なし：赤文字でメッセージ表示、入力欄をクリア
                    lblEmployeeNameValue.Text = "該当するユーザがありません。"
                    lblEmployeeNameValue.ForeColor = Color.Red
                    ShowMessage("該当するユーザがありません。", Color.Red)

                    ' 入力欄をクリアしてフォーカスを戻す
                    ' 注: TextChangedイベントを一時的に無効化してメッセージ上書きを防ぐ
                    RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                    txtEmployeeNo.Text = ""
                    AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                    txtEmployeeNo.Focus()
                End If

            Catch ex As FileNotFoundException
                ' ファイル未検出：エラー表示して入力欄をクリア
                lblEmployeeNameValue.Text = "データ読み取りエラーが発生しました"
                lblEmployeeNameValue.ForeColor = Color.Red
                ShowMessage("データ読み取りエラーが発生しました", Color.Red)
                _logManager.WriteErrorLog("従業員CSV未検出: " & ex.Message)

                txtCardNo.Enabled = False
                txtCardNo.Text = ""

                ' 入力欄をクリアしてフォーカスを戻す
                RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Text = ""
                AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Focus()

            Catch ex As Exception
                ' その他のエラー：エラー表示して入力欄をクリア
                lblEmployeeNameValue.Text = "データ読み取りエラーが発生しました"
                lblEmployeeNameValue.ForeColor = Color.Red
                ShowMessage("データ読み取りエラーが発生しました", Color.Red)
                _logManager.WriteErrorLog("従業員検索エラー: " & ex.Message & vbCrLf & ex.StackTrace)

                txtCardNo.Enabled = False
                txtCardNo.Text = ""

                ' 入力欄をクリアしてフォーカスを戻す
                RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Text = ""
                AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Focus()

            Finally
                _isSearchingEmployee = False
            End Try

        Else
            ' 6桁未満：カードNoを非活性化
            txtCardNo.Enabled = False
            txtCardNo.Text = ""
            ClearConditionLabels()
            btnVerify.Enabled = False

            If employeeNo.Length > 0 Then
                ShowMessage("従業員Noを6桁で入力してください", Color.Black)
            Else
                ShowMessage("従業員Noを入力してください", Color.Black)
            End If
        End If
    End Sub

    ''' <summary>
    ''' カードNo入力時の処理（6桁で自動実行）
    ''' </summary>
    Private Sub TxtCardNo_TextChanged(sender As Object, e As EventArgs)
        Dim cardNo As String = txtCardNo.Text.Trim()

        ' 6桁入力完了時に自動でカードNo入力後の処理を実行
        If cardNo.Length = 6 Then
            ProcessCardNoInput()
        End If
    End Sub

    ''' <summary>
    ''' カードNo入力後の処理（共通処理）
    ''' </summary>
    Private Sub ProcessCardNoInput()
        Dim cardNo As String = txtCardNo.Text.Trim()

        If String.IsNullOrEmpty(cardNo) Then
            Return
        End If

        ' 6桁チェック
        If cardNo.Length <> 6 Then
            ShowMessage("カードNoは6桁で入力してください", Color.Red)
            ClearConditionLabels()
            lblCardNoDisplayValue.Text = ""
            lblProductNameValue.Text = ""
            lblQuantityValue.Text = ""
            lblLocationValue.Text = ""
            btnVerify.Enabled = False
            Return
        End If

        ' カード条件を取得
        _currentCondition = _cardLoader.GetCondition(cardNo)

        If _currentCondition Is Nothing Then
            ShowMessage("条件なし", Color.Black)
            ClearConditionLabels()
            lblCardNoDisplayValue.Text = ""
            lblProductNameValue.Text = ""
            lblQuantityValue.Text = ""
            lblLocationValue.Text = ""
            btnVerify.Enabled = False

            ' 条件なしの場合もカードNo入力欄をクリア
            txtCardNo.Text = ""
            Return
        End If

        ' 条件を表示
        DisplayCondition(_currentCondition)

        ' カード情報を表示
        DisplayCardInfo(_currentCondition)

        ' 初回計測を実行
        Try
            _balanceManager.PerformInitialReading()
            
            ' 天秤から取得した値を表示
            DisplayInitialBalanceReadings(_currentCondition)
            
            ShowMessage("使用部材条件を表示しました", Color.Green)
            btnVerify.Enabled = True

            ' カードNo入力欄をクリア
            txtCardNo.Text = ""
        Catch ex As Exception
            ShowMessage("計測エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("初回計測エラー: " & ex.Message)
            btnVerify.Enabled = False

            ' エラー時もカードNo入力欄をクリア
            txtCardNo.Text = ""
        End Try
    End Sub

    Private Sub lblMessage_Click(sender As Object, e As EventArgs) Handles lblMessage.Click

    End Sub

    ''' <summary>
    ''' 条件ラベルをすべてクリア
    ''' </summary>
    Private Sub ClearConditionLabels()
        lblPre10mmRequired.Text = ""
        lblPre10mmRemaining.Text = ""
        lblPre10mmSecured.Text = ""
        lblPre10mmUsed.Text = ""
        lblPre10mmJudgment.Text = ""
        lblPost1mmRequired.Text = ""
        lblPost1mmRemaining.Text = ""
        lblPost1mmSecured.Text = ""
        lblPost1mmUsed.Text = ""
        lblPost1mmJudgment.Text = ""
        lblPost5mmRequired.Text = ""
        lblPost5mmRemaining.Text = ""
        lblPost5mmSecured.Text = ""
        lblPost5mmUsed.Text = ""
        lblPost5mmJudgment.Text = ""
        lblPost10mmRequired.Text = ""
        lblPost10mmRemaining.Text = ""
        lblPost10mmSecured.Text = ""
        lblPost10mmUsed.Text = ""
        lblPost10mmJudgment.Text = ""
        lblEdgeRequired.Text = ""
        lblEdgeRemaining.Text = ""
        lblEdgeSecured.Text = ""
        lblEdgeUsed.Text = ""
        lblEdgeJudgment.Text = ""
        lblBubbleRequired.Text = ""
        lblBubbleRemaining.Text = ""
        lblBubbleSecured.Text = ""
        lblBubbleUsed.Text = ""
        lblBubbleJudgment.Text = ""
    End Sub

    Private Sub lblHeaderRemaining_Click(sender As Object, e As EventArgs) Handles lblHeaderRemaining.Click

    End Sub

    Private Sub lblHeaderJudgment_Click(sender As Object, e As EventArgs) Handles lblHeaderJudgment.Click

    End Sub

    Private Sub lblHeaderSecured_Click(sender As Object, e As EventArgs) Handles lblHeaderSecured.Click

    End Sub
End Class

