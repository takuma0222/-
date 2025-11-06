Imports System.Windows.Forms

''' <summary>
''' メインフォーム
''' </summary>
Public Class MainForm
    Inherits Form

    ' サービス
    Private _appConfig As AppConfig
    Private _cardLoader As CardConditionLoader
    Private _materialLoader As MaterialConditionLoader
    Private _balanceManager As BalanceManager
    Private _logManager As LogManager
    Private _employeeLoader As EmployeeLoader
    Private _currentCondition As CardCondition
    Private _currentMaterialCondition As MaterialCondition
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
        AddHandler cmbLapThickness.SelectedIndexChanged, AddressOf CmbLapThickness_SelectedIndexChanged
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
            
            ' 使用部材条件読み込み
            Dim materialCsvPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.MaterialConditionCsvPath)
            _materialLoader = New MaterialConditionLoader(materialCsvPath)
            
            ' LAP厚プルダウンリストを設定
            cmbLapThickness.Items.Clear()
            For Each thickness In _appConfig.LapThicknessList
                cmbLapThickness.Items.Add(thickness)
            Next
            
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
        
        ' 【UI仕様変更】大文字英数字の入力を許可（大文字を小文字に変換しない）
        ' 旧処理：大文字を小文字に変換
        ' If Char.IsUpper(e.KeyChar) Then
        '     e.KeyChar = Char.ToLower(e.KeyChar)
        ' End If
    End Sub

    ''' <summary>
    ''' カードNo入力後の処理（フォーカス離脱時）
    ''' </summary>
    Private Sub TxtCardNo_Leave(sender As Object, e As EventArgs)
        ' TextChangedで処理されるため、特に追加処理は不要
    End Sub

    ''' <summary>
    ''' 条件をラベルに表示（旧メソッド - 互換性のため残す）
    ''' </summary>
    Private Sub DisplayCondition(condition As CardCondition)
        ' 必要枚数を表示
        lblPre10mmRequired.Text = condition.Pre10mm.ToString() & "個"
        lblPost1mmRequired.Text = condition.Post1mm.ToString() & "個"
        lblPost5mmRequired.Text = condition.Post5mm.ToString() & "個"
        lblPost10mmRequired.Text = condition.Post10mm.ToString() & "個"
        
        ' エッジガードと気泡緩衝材は0の場合"不要"を表示（他の列はDisplayInitialBalanceReadingsで設定）
        If condition.EdgeGuard = 0 Then
            lblEdgeRequired.Text = "不要"
        Else
            lblEdgeRequired.Text = condition.EdgeGuard.ToString() & "個"
        End If
        
        If condition.BubbleInterference = 0 Then
            lblBubbleRequired.Text = "不要"
        Else
            lblBubbleRequired.Text = condition.BubbleInterference.ToString("D2") & "個"
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
    ''' 使用部材条件を表示（MaterialConditionから）
    ''' </summary>
    Private Sub DisplayMaterialCondition(condition As MaterialCondition)
        ' 必要枚数を表示
        lblPre10mmRequired.Text = condition.Pre10mm.ToString() & "個"
        lblPost1mmRequired.Text = condition.Post1mm.ToString() & "個"
        lblPost5mmRequired.Text = condition.Post5mm.ToString() & "個"
        lblPost10mmRequired.Text = condition.Post10mm.ToString() & "個"

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
    End Sub

    ''' <summary>
    ''' エッジガードと気泡緩衝材の情報を表示（CardConditionから）
    ''' </summary>
    Private Sub DisplayEdgeAndBubbleInfo(condition As CardCondition)
        ' エッジガードと気泡緩衝材は0の場合"不要"を表示
        If condition.EdgeGuard = 0 Then
            lblEdgeRequired.Text = "不要"
        Else
            lblEdgeRequired.Text = condition.EdgeGuard.ToString() & "個"
        End If
        
        If condition.BubbleInterference = 0 Then
            lblBubbleRequired.Text = "不要"
        Else
            lblBubbleRequired.Text = condition.BubbleInterference.ToString("D2") & "個"
        End If

        ' エッジガードと気泡緩衝材の他の列をクリア
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
            lblPre10mmRemaining.Text = readings("Pre_10mm").ToString("F0") & "個"
            lblPost10mmRemaining.Text = readings("Pre_10mm").ToString("F0") & "個"
        End If

        ' 秤(9002)の値を投入後1mmに表示
        If readings.ContainsKey("Post_1mm") Then
            lblPost1mmRemaining.Text = readings("Post_1mm").ToString("F0") & "個"
        End If

        ' 秤(9003)の値を投入後5mmに表示
        If readings.ContainsKey("Post_5mm") Then
            lblPost5mmRemaining.Text = readings("Post_5mm").ToString("F0") & "個"
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
    ''' 照合時計測値を表示（AFTER列）し、過不足枚数を計算して表示
    ''' </summary>
    Private Sub DisplayVerificationReadings(condition As CardCondition)
        Dim verificationReadings As Dictionary(Of String, Double) = _balanceManager.VerificationReadings
        Dim initialReadings As Dictionary(Of String, Double) = _balanceManager.InitialReadings

        ' 使用部材条件が取得できている場合はそちらを使用
        Dim pre10mmRequired As Integer = If(_currentMaterialCondition IsNot Nothing, _currentMaterialCondition.Pre10mm, condition.Pre10mm)
        Dim post1mmRequired As Integer = If(_currentMaterialCondition IsNot Nothing, _currentMaterialCondition.Post1mm, condition.Post1mm)
        Dim post5mmRequired As Integer = If(_currentMaterialCondition IsNot Nothing, _currentMaterialCondition.Post5mm, condition.Post5mm)
        Dim post10mmRequired As Integer = If(_currentMaterialCondition IsNot Nothing, _currentMaterialCondition.Post10mm, condition.Post10mm)

        ' 秤(9001)の値を投入前10mmと投入後10mmのAFTER列に表示し、過不足枚数を計算
        If verificationReadings.ContainsKey("Pre_10mm") And initialReadings.ContainsKey("Pre_10mm") Then
            Dim afterValue As Double = verificationReadings("Pre_10mm")
            Dim beforeValue As Double = initialReadings("Pre_10mm")
            ' 新計算式: 必要枚数 - BEFORE - AFTER (正=過剰、負=不足、0=OK)
            Dim totalRequired As Integer = pre10mmRequired + post10mmRequired
            Dim differenceCount As Double = totalRequired - beforeValue - afterValue
            
            ' 投入前10mm
            lblPre10mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPre10mmUsed.Text = differenceCount.ToString("F0") & "個"
            
            ' 投入後10mm
            lblPost10mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPost10mmUsed.Text = differenceCount.ToString("F0") & "個"
        End If

        ' 秤(9002)の値を投入後1mmのAFTER列に表示し、過不足枚数を計算
        If verificationReadings.ContainsKey("Post_1mm") And initialReadings.ContainsKey("Post_1mm") Then
            Dim afterValue As Double = verificationReadings("Post_1mm")
            Dim beforeValue As Double = initialReadings("Post_1mm")
            ' 新計算式: 必要枚数 - BEFORE - AFTER (正=過剰、負=不足、0=OK)
            Dim differenceCount As Double = post1mmRequired - beforeValue - afterValue
            
            lblPost1mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPost1mmUsed.Text = differenceCount.ToString("F0") & "個"
        End If

        ' 秤(9003)の値を投入後5mmのAFTER列に表示し、過不足枚数を計算
        If verificationReadings.ContainsKey("Post_5mm") And initialReadings.ContainsKey("Post_5mm") Then
            Dim afterValue As Double = verificationReadings("Post_5mm")
            Dim beforeValue As Double = initialReadings("Post_5mm")
            ' 新計算式: 必要枚数 - BEFORE - AFTER (正=過剰、負=不足、0=OK)
            Dim differenceCount As Double = post5mmRequired - beforeValue - afterValue
            
            lblPost5mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPost5mmUsed.Text = differenceCount.ToString("F0") & "個"
        End If
    End Sub

    ''' <summary>
    ''' 過不足枚数から判定結果と色を取得
    ''' </summary>
    Private Function GetJudgmentResult(shortageCount As Double) As (text As String, color As Color)
        Dim roundedShortage As Integer = CInt(Math.Round(shortageCount))
        
        If roundedShortage = 0 Then
            Return ("OK", Color.Green)
        ElseIf roundedShortage >= 1 Then
            Return ("過剰", Color.Red)
        Else ' -1以下
            Return ("不足", Color.Red)
        End If
    End Function

    ''' <summary>
    ''' 判定を表示（過不足枚数に応じて判定を表示）
    ''' </summary>
    Private Sub DisplayJudgment(condition As CardCondition)
        ' 投入前10mmと投入後10mmの判定（同じ秤9001を使用、同じ過不足枚数）
        If Not String.IsNullOrEmpty(lblPre10mmUsed.Text) AndAlso lblPre10mmUsed.Text <> "不要" Then
            Dim shortageCount As Double = Double.Parse(lblPre10mmUsed.Text.Replace("個", ""))
            Dim result = GetJudgmentResult(shortageCount)
            
            ' 投入前10mmの判定
            lblPre10mmJudgment.Text = result.text
            lblPre10mmJudgment.ForeColor = result.color
            
            ' 投入後10mmの判定（同じ結果）
            lblPost10mmJudgment.Text = result.text
            lblPost10mmJudgment.ForeColor = result.color
        End If

        ' 投入後1mmの判定
        If Not String.IsNullOrEmpty(lblPost1mmUsed.Text) AndAlso lblPost1mmUsed.Text <> "不要" Then
            Dim shortageCount As Double = Double.Parse(lblPost1mmUsed.Text.Replace("個", ""))
            Dim result = GetJudgmentResult(shortageCount)
            
            lblPost1mmJudgment.Text = result.text
            lblPost1mmJudgment.ForeColor = result.color
        End If

        ' 投入後5mmの判定
        If Not String.IsNullOrEmpty(lblPost5mmUsed.Text) AndAlso lblPost5mmUsed.Text <> "不要" Then
            Dim shortageCount As Double = Double.Parse(lblPost5mmUsed.Text.Replace("個", ""))
            Dim result = GetJudgmentResult(shortageCount)
            
            lblPost5mmJudgment.Text = result.text
            lblPost5mmJudgment.ForeColor = result.color
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
                ShowMessage("照合OK", Color.Green)
                btnVerify.Enabled = False
                ' 照合OK時は両方の入力欄をクリアして活性化
                ' TextChangedイベントを一時的に無効化してメッセージ上書きを防ぐ
                RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                RemoveHandler txtCardNo.TextChanged, AddressOf TxtCardNo_TextChanged
                txtEmployeeNo.Text = ""
                txtCardNo.Text = ""
                cmbLapThickness.SelectedIndex = -1
                AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                AddHandler txtCardNo.TextChanged, AddressOf TxtCardNo_TextChanged
                txtEmployeeNo.Enabled = True
                txtCardNo.Enabled = True
                cmbLapThickness.Enabled = False
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
        txtEmployeeNo.Enabled = True  ' 従業員No入力欄を活性化
        txtCardNo.Text = ""
        txtCardNo.Enabled = False  ' カードNoを非活性化
        cmbLapThickness.SelectedIndex = -1  ' LAP厚をクリア
        cmbLapThickness.Enabled = False  ' LAP厚を非活性化
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
        _currentMaterialCondition = Nothing
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

                    ' 従業員No入力欄を非活性化
                    txtEmployeeNo.Enabled = False

                    ' カードNoを活性化してフォーカス移動
                    txtCardNo.Enabled = True
                    txtCardNo.Focus()
                    ShowMessage("カードNoを入力してください", Color.Black)
                Else
                    ' 該当なし：氏名表示欄に「ユーザなし」を表示
                    lblEmployeeNameValue.Text = "ユーザなし"
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
                lblEmployeeNameValue.Text = "ユーザなし"
                lblEmployeeNameValue.ForeColor = Color.Red
                ShowMessage("データ読み取りエラーが発生しました", Color.Red)
                _logManager.WriteErrorLog("従業員CSV未検出: " & ex.Message)

                txtCardNo.Enabled = False
                txtCardNo.Text = ""
                cmbLapThickness.Enabled = False
                cmbLapThickness.SelectedIndex = -1

                ' 入力欄をクリアしてフォーカスを戻す
                RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Text = ""
                AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
                txtEmployeeNo.Focus()

            Catch ex As Exception
                ' その他のエラー：エラー表示して入力欄をクリア
                lblEmployeeNameValue.Text = "ユーザなし"
                lblEmployeeNameValue.ForeColor = Color.Red
                ShowMessage("データ読み取りエラーが発生しました", Color.Red)
                _logManager.WriteErrorLog("従業員検索エラー: " & ex.Message & vbCrLf & ex.StackTrace)

                txtCardNo.Enabled = False
                txtCardNo.Text = ""
                cmbLapThickness.Enabled = False
                cmbLapThickness.SelectedIndex = -1

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
            cmbLapThickness.Enabled = False
            cmbLapThickness.SelectedIndex = -1
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
    ''' カードNo入力時の処理（6桁で自動実行しない）
    ''' </summary>
    Private Sub TxtCardNo_TextChanged(sender As Object, e As EventArgs)
        Dim cardNo As String = txtCardNo.Text.Trim()

        ' 6桁入力完了時にLAP厚選択欄を活性化
        If cardNo.Length = 6 Then
            ' カード情報を取得（品名、枚数、所在などの表示用）
            _currentCondition = _cardLoader.GetCondition(cardNo)
            
            If _currentCondition Is Nothing Then
                ShowMessage("条件なし", Color.Black)
                ClearConditionLabels()
                lblCardNoDisplayValue.Text = ""
                lblProductNameValue.Text = ""
                lblQuantityValue.Text = ""
                lblLocationValue.Text = ""
                cmbLapThickness.Enabled = False
                cmbLapThickness.SelectedIndex = -1
                btnVerify.Enabled = False
                Return
            End If
            
            ' カード情報を表示（品名、枚数、所在など）
            DisplayCardInfo(_currentCondition)
            
            ' LAP厚選択欄を活性化
            cmbLapThickness.Enabled = True
            cmbLapThickness.Focus()
            ShowMessage("LAP厚を選択してください", Color.Black)
        Else
            ' 6桁未満の場合はLAP厚選択欄を非活性化
            cmbLapThickness.Enabled = False
            cmbLapThickness.SelectedIndex = -1
            ClearConditionLabels()
            lblCardNoDisplayValue.Text = ""
            lblProductNameValue.Text = ""
            lblQuantityValue.Text = ""
            lblLocationValue.Text = ""
            btnVerify.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' LAP厚選択時の処理（データ取得を実施）
    ''' </summary>
    Private Sub CmbLapThickness_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cmbLapThickness.SelectedIndex = -1 OrElse _currentCondition Is Nothing Then
            Return
        End If

        Dim selectedLapThickness As String = cmbLapThickness.SelectedItem.ToString()

        ' 使用部材条件を枚数とLAP厚から取得
        _currentMaterialCondition = _materialLoader.GetCondition(_currentCondition.Quantity, selectedLapThickness)

        If _currentMaterialCondition Is Nothing Then
            ShowMessage("該当する使用部材条件がありません", Color.Red)
            ClearConditionLabels()
            btnVerify.Enabled = False
            Return
        End If

        ' 使用部材条件を表示
        DisplayMaterialCondition(_currentMaterialCondition)

        ' エッジガードと気泡緩衝材はカード情報から表示
        DisplayEdgeAndBubbleInfo(_currentCondition)

        ' カードNo入力欄を非活性化
        txtCardNo.Enabled = False

        ' 初回計測を実行
        Try
            ' 測定中メッセージを表示
            ShowMessage("秤の値測定中...", Color.Black)
            
            _balanceManager.PerformInitialReading()
            
            ' 天秤から取得した値を表示
            DisplayInitialBalanceReadings(_currentCondition)
            
            ' 測定完了後のメッセージ
            ShowMessage("各部材を必要枚数分準備してください", Color.Green)
            btnVerify.Enabled = True

        Catch ex As Exception
            ShowMessage("計測エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("初回計測エラー: " & ex.Message)
            btnVerify.Enabled = False
            
            ' エラー時はカードNo入力欄を再度活性化
            txtCardNo.Enabled = True
        End Try
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

            ' 【UI仕様変更】条件なしの場合もカードNo入力欄をクリアしない
            ' 旧処理：txtCardNo.Text = ""
            Return
        End If

        ' 条件を表示
        DisplayCondition(_currentCondition)

        ' カード情報を表示
        DisplayCardInfo(_currentCondition)

        ' 【UI仕様変更】カードNo入力欄を非活性化
        txtCardNo.Enabled = False

        ' 初回計測を実行
        Try
            ' 【UI仕様変更】測定中メッセージを表示
            ShowMessage("秤の値測定中...", Color.Black)
            
            _balanceManager.PerformInitialReading()
            
            ' 天秤から取得した値を表示
            DisplayInitialBalanceReadings(_currentCondition)
            
            ' 【UI仕様変更】測定完了後のメッセージを変更
            ShowMessage("各部材を必要枚数分準備してください", Color.Green)
            btnVerify.Enabled = True

            ' 【UI仕様変更】カードNo入力欄の内容を保持（クリアしない）
            ' 旧処理：txtCardNo.Text = ""
        Catch ex As Exception
            ShowMessage("計測エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("初回計測エラー: " & ex.Message)
            btnVerify.Enabled = False

            ' 【UI仕様変更】エラー時もカードNo入力欄の内容を保持（クリアしない）
            ' 旧処理：txtCardNo.Text = ""
            
            ' 【UI仕様変更】エラー時はカードNo入力欄を再度活性化
            txtCardNo.Enabled = True
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

