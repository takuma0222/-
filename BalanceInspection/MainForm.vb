Imports System.Windows.Forms

''' <summary>
''' メインフォーム
''' </summary>
Public Class MainForm
    Inherits Form

    ' UI コンポーネント
    Private txtEmployeeNo As TextBox
    Private txtCardNo As TextBox
    Private btnVerify As Button
    Private btnCancel As Button
    Private lblMessage As Label
    Private dgvConditions As DataGridView

    ' サービス
    Private _appConfig As AppConfig
    Private _cardLoader As CardConditionLoader
    Private _balanceManager As BalanceManager
    Private _logManager As LogManager
    Private _currentCondition As CardCondition

    Public Sub New()
        InitializeComponent()
        InitializeCustomComponents()
        InitializeServices()
    End Sub

    ''' <summary>
    ''' カスタムUIコンポーネントを初期化
    ''' </summary>
    Private Sub InitializeCustomComponents()
        Me.Text = "検品デスクトップアプリ"
        Me.Size = New Size(800, 500)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' ラベル: 従業員No
        Dim lblEmployeeNo As New Label()
        lblEmployeeNo.Text = "従業員No:"
        lblEmployeeNo.Location = New Point(20, 20)
        lblEmployeeNo.Size = New Size(100, 20)
        Me.Controls.Add(lblEmployeeNo)

        ' テキストボックス: 従業員No
        txtEmployeeNo = New TextBox()
        txtEmployeeNo.Location = New Point(130, 20)
        txtEmployeeNo.Size = New Size(150, 20)
        txtEmployeeNo.MaxLength = 6
        AddHandler txtEmployeeNo.KeyPress, AddressOf TextBox_KeyPress
        Me.Controls.Add(txtEmployeeNo)

        ' ラベル: カードNo
        Dim lblCardNo As New Label()
        lblCardNo.Text = "カードNo:"
        lblCardNo.Location = New Point(300, 20)
        lblCardNo.Size = New Size(100, 20)
        Me.Controls.Add(lblCardNo)

        ' テキストボックス: カードNo
        txtCardNo = New TextBox()
        txtCardNo.Location = New Point(410, 20)
        txtCardNo.Size = New Size(150, 20)
        txtCardNo.MaxLength = 6
        AddHandler txtCardNo.KeyPress, AddressOf TextBox_KeyPress
        AddHandler txtCardNo.Leave, AddressOf TxtCardNo_Leave
        Me.Controls.Add(txtCardNo)

        ' ボタン: 照合
        btnVerify = New Button()
        btnVerify.Text = "照合"
        btnVerify.Location = New Point(580, 15)
        btnVerify.Size = New Size(80, 30)
        btnVerify.Enabled = False
        AddHandler btnVerify.Click, AddressOf BtnVerify_Click
        Me.Controls.Add(btnVerify)

        ' ボタン: キャンセル
        btnCancel = New Button()
        btnCancel.Text = "キャンセル"
        btnCancel.Location = New Point(680, 15)
        btnCancel.Size = New Size(100, 30)
        AddHandler btnCancel.Click, AddressOf BtnCancel_Click
        Me.Controls.Add(btnCancel)

        ' ラベル: メッセージ
        lblMessage = New Label()
        lblMessage.Text = "従業員Noを入力してください"
        lblMessage.Location = New Point(20, 60)
        lblMessage.Size = New Size(760, 40)
        lblMessage.Font = New Font(lblMessage.Font.FontFamily, 10, FontStyle.Regular)
        lblMessage.ForeColor = Color.Black
        Me.Controls.Add(lblMessage)

        ' DataGridView: 使用部材条件
        dgvConditions = New DataGridView()
        dgvConditions.Location = New Point(20, 110)
        dgvConditions.Size = New Size(760, 80)
        dgvConditions.AllowUserToAddRows = False
        dgvConditions.AllowUserToDeleteRows = False
        dgvConditions.ReadOnly = True
        dgvConditions.RowHeadersVisible = False
        dgvConditions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvConditions.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvConditions.MultiSelect = False
        
        ' 列を追加
        dgvConditions.Columns.Add("Col1mm", "1mmクッション材")
        dgvConditions.Columns.Add("Col5mm", "5mmクッション材")
        dgvConditions.Columns.Add("Col10mm", "10mmクッション材")
        dgvConditions.Columns.Add("ColEdge", "エッジガード")
        dgvConditions.Columns.Add("ColBubble", "気泡緩衝材")
        
        Me.Controls.Add(dgvConditions)
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
                _logManager.WriteErrorLog($"ポートオープン時の警告: {ex.Message}")
            End Try
            
        Catch ex As Exception
            MessageBox.Show($"初期化エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    ''' カードNo入力後の処理
    ''' </summary>
    Private Sub TxtCardNo_Leave(sender As Object, e As EventArgs)
        Dim cardNo As String = txtCardNo.Text.Trim()
        
        If String.IsNullOrEmpty(cardNo) Then
            Return
        End If
        
        ' 6桁チェック
        If cardNo.Length <> 6 Then
            ShowMessage("カードNoは6桁で入力してください", Color.Red)
            dgvConditions.Rows.Clear()
            btnVerify.Enabled = False
            Return
        End If
        
        ' カード条件を取得
        _currentCondition = _cardLoader.GetCondition(cardNo)
        
        If _currentCondition Is Nothing Then
            ShowMessage("条件なし", Color.Black)
            dgvConditions.Rows.Clear()
            btnVerify.Enabled = False
            Return
        End If
        
        ' 条件を表示
        DisplayCondition(_currentCondition)
        
        ' 初回計測を実行
        Try
            _balanceManager.PerformInitialReading()
            ShowMessage("使用部材条件を表示しました", Color.Green)
            btnVerify.Enabled = True
        Catch ex As Exception
            ShowMessage("計測エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog($"初回計測エラー: {ex.Message}")
            btnVerify.Enabled = False
        End Try
    End Sub

    ''' <summary>
    ''' 条件をDataGridViewに表示
    ''' </summary>
    Private Sub DisplayCondition(condition As CardCondition)
        dgvConditions.Rows.Clear()
        dgvConditions.Rows.Add(
            condition.Post1mm.ToString(),
            condition.Post5mm.ToString(),
            condition.Pre10mm.ToString(),
            condition.EdgeGuard.ToString(),
            condition.BubbleInterference.ToString("D2")
        )
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
            Else
                ' NG: 不一致を表示
                Dim ngMessage As String = BuildNgMessage(differences, _currentCondition)
                ShowMessage(ngMessage, Color.Red)
                _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "NG")
            End If
            
        Catch ex As Exception
            ShowMessage("照合エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog($"照合時エラー: {ex.Message}")
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
                errors.Add($"10mm:{diff}≠{condition.Pre10mm}")
            End If
        End If
        
        If differences.ContainsKey("Post_1mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_1mm")))
            If diff <> condition.Post1mm Then
                errors.Add($"1mm:{diff}≠{condition.Post1mm}")
            End If
        End If
        
        If differences.ContainsKey("Post_5mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_5mm")))
            If diff <> condition.Post5mm Then
                errors.Add($"5mm:{diff}≠{condition.Post5mm}")
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
        dgvConditions.Rows.Clear()
        ShowMessage("従業員Noを入力してください", Color.Black)
        btnVerify.Enabled = False
        _currentCondition = Nothing
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
End Class
