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
    Private _currentCondition As CardCondition

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
            
            ' カード条件読み込み
            Dim csvPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.CardConditionCsvPath)
            _cardLoader = New CardConditionLoader(csvPath)
            
            ' バランスマネージャー初期化
            _balanceManager = New BalanceManager(_appConfig)
            
            ' ログマネージャー初期化
            Dim logDir As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.LogDirectory)
            _logManager = New LogManager(logDir)
            
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
            dgvConditions.Rows.Clear()
            btnVerify.Enabled = False
            Return
        End If
        
        ' 6桁の場合で、まだ処理されていない場合のみ処理（TextChangedで処理済みの場合は重複を避ける）
        If _currentCondition Is Nothing Then
            ProcessCardNoInput()
        End If
    End Sub

    ''' <summary>
    ''' 条件をDataGridViewに表示
    ''' </summary>
    Private Sub DisplayCondition(condition As CardCondition)
        dgvConditions.Rows.Clear()
        dgvConditions.Rows.Add(
            condition.Pre10mm.ToString(),
            condition.Post1mm.ToString(),
            condition.Post5mm.ToString(),
            condition.Post10mm.ToString(),
            condition.EdgeGuard.ToString(),
            condition.BubbleInterference.ToString("D2")
        )
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

            ' 差分計算
            Dim differences As Dictionary(Of String, Double) = _balanceManager.CalculateDifferences()

            ' 照合判定
            Dim isValid As Boolean = ValidateDifferences(differences, _currentCondition)

            If isValid Then
                ' OK: ログ出力
                _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "OK")
                ShowMessage("検査合格", Color.Green)
                btnVerify.Enabled = False
                ' 照合OK時はカードNoを非活性化して従業員Noにフォーカス
                txtCardNo.Enabled = False
                txtEmployeeNo.Focus()
            Else
                ' NG: 不一致を表示
                Dim ngMessage As String = BuildNgMessage(differences, _currentCondition)
                ShowMessage(ngMessage, Color.Red)
                _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "NG")
            End If

        Catch ex As Exception
            ShowMessage("照合エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("照合時エラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 差分と予定数を照合
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
        dgvConditions.Rows.Clear()
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
    Private Sub TxtEmployeeNo_TextChanged(sender As Object, e As EventArgs)
        Dim employeeNo As String = txtEmployeeNo.Text.Trim()

        If employeeNo.Length = 6 Then
            ' 6桁入力完了：カードNoを活性化してフォーカス移動
            txtCardNo.Enabled = True
            txtCardNo.Focus()
            ShowMessage("カードNoを入力してください", Color.Black)
        Else
            ' 6桁未満：カードNoを非活性化
            txtCardNo.Enabled = False
            txtCardNo.Text = ""
            dgvConditions.Rows.Clear()
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
            dgvConditions.Rows.Clear()
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
            dgvConditions.Rows.Clear()
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

    Private Sub dgvConditions_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvConditions.CellContentClick

    End Sub

    Private Sub lblMessage_Click(sender As Object, e As EventArgs) Handles lblMessage.Click

    End Sub
End Class

