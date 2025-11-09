Imports System.Windows.Forms
Imports System.IO
Imports System.Diagnostics

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
    
    ' 照合状態管理（2段階照合用）
    Private _verificationStage As Integer = 0  ' 0:未開始, 1:投入前完了, 2:完了

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
    ''' 【第1段階】投入前10mmのみ表示
    ''' </summary>
    Private Sub DisplayPre10mmOnly(condition As MaterialCondition)
        ' 投入前10mmの必要枚数のみ表示
        lblPre10mmRequired.Text = condition.Pre10mm.ToString() & "個"
        
        ' 投入前10mmのその他の列はクリア（照合前 数は後で設定）
        lblPre10mmRemaining.Text = ""
        lblPre10mmSecured.Text = ""
        lblPre10mmUsed.Text = ""
        lblPre10mmJudgment.Text = ""
        
        ' 投入後の部材は全てクリア
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
        
        ' エッジガードと気泡緩衝材もクリア
        lblEdgeRequired.Text = ""
        lblBubbleRequired.Text = ""
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

        ' ロットNoを表示
        lblLotNoValue.Text = If(String.IsNullOrEmpty(condition.LotNo), "", condition.LotNo)

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
    ''' 照合時計測値を表示（AFTER列）し、不足枚数を計算して表示
    ''' </summary>
    Private Sub DisplayVerificationReadings(condition As CardCondition)
        Dim verificationReadings As Dictionary(Of String, Double) = _balanceManager.VerificationReadings
        Dim initialReadings As Dictionary(Of String, Double) = _balanceManager.InitialReadings

        ' 秤(9001)の値を投入前10mmと投入後10mmのAFTER列に表示し、不足枚数を計算
        If verificationReadings.ContainsKey("Pre_10mm") And initialReadings.ContainsKey("Pre_10mm") Then
            Dim afterValue As Double = verificationReadings("Pre_10mm")
            Dim beforeValue As Double = initialReadings("Pre_10mm")
            Dim used As Double = beforeValue - afterValue
            Dim shortage As Double = _currentMaterialCondition.Pre10mm - used
            
            ' 投入前10mm
            lblPre10mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPre10mmUsed.Text = shortage.ToString("F0") & "個"
            
            ' 投入後10mm
            lblPost10mmSecured.Text = afterValue.ToString("F0") & "個"
            Dim post10mmShortage As Double = _currentMaterialCondition.Post10mm - used
            lblPost10mmUsed.Text = post10mmShortage.ToString("F0") & "個"
        End If

        ' 秤(9002)の値を投入後1mmのAFTER列に表示し、不足枚数を計算
        If verificationReadings.ContainsKey("Post_1mm") And initialReadings.ContainsKey("Post_1mm") Then
            Dim afterValue As Double = verificationReadings("Post_1mm")
            Dim beforeValue As Double = initialReadings("Post_1mm")
            Dim used As Double = beforeValue - afterValue
            Dim shortage As Double = _currentMaterialCondition.Post1mm - used
            
            lblPost1mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPost1mmUsed.Text = shortage.ToString("F0") & "個"
        End If

        ' 秤(9003)の値を投入後5mmのAFTER列に表示し、不足枚数を計算
        If verificationReadings.ContainsKey("Post_5mm") And initialReadings.ContainsKey("Post_5mm") Then
            Dim afterValue As Double = verificationReadings("Post_5mm")
            Dim beforeValue As Double = initialReadings("Post_5mm")
            Dim used As Double = beforeValue - afterValue
            Dim shortage As Double = _currentMaterialCondition.Post5mm - used
            
            lblPost5mmSecured.Text = afterValue.ToString("F0") & "個"
            lblPost5mmUsed.Text = shortage.ToString("F0") & "個"
        End If
    End Sub

    ''' <summary>
    ''' 判定を表示（不足枚数が0ならOK、>=1なら「不足」、<=-1なら「過剰」）
    ''' </summary>
    Private Sub DisplayJudgment(condition As CardCondition)
        ' 使用部材条件が取得できている場合はそちらを使用
        If _currentMaterialCondition Is Nothing Then
            Return
        End If
        
        Dim pre10mmRequired As Integer = _currentMaterialCondition.Pre10mm
        Dim post1mmRequired As Integer = _currentMaterialCondition.Post1mm
        Dim post5mmRequired As Integer = _currentMaterialCondition.Post5mm
        Dim post10mmRequired As Integer = _currentMaterialCondition.Post10mm

        ' 投入前10mmの判定
        If Not String.IsNullOrEmpty(lblPre10mmUsed.Text) AndAlso lblPre10mmUsed.Text <> "不要" Then
            Dim shortage As Integer = Integer.Parse(lblPre10mmUsed.Text.Replace("個", ""))
            
            If shortage = 0 Then
                lblPre10mmJudgment.Text = "OK"
                lblPre10mmJudgment.ForeColor = Color.Green
            ElseIf shortage >= 1 Then
                lblPre10mmJudgment.Text = "不足"
                lblPre10mmJudgment.ForeColor = Color.Red
            Else ' shortage <= -1
                lblPre10mmJudgment.Text = "過剰"
                lblPre10mmJudgment.ForeColor = Color.Red
            End If
        End If

        ' 投入後1mmの判定
        If Not String.IsNullOrEmpty(lblPost1mmUsed.Text) AndAlso lblPost1mmUsed.Text <> "不要" Then
            Dim shortage As Integer = Integer.Parse(lblPost1mmUsed.Text.Replace("個", ""))
            
            If shortage = 0 Then
                lblPost1mmJudgment.Text = "OK"
                lblPost1mmJudgment.ForeColor = Color.Green
            ElseIf shortage >= 1 Then
                lblPost1mmJudgment.Text = "不足"
                lblPost1mmJudgment.ForeColor = Color.Red
            Else
                lblPost1mmJudgment.Text = "過剰"
                lblPost1mmJudgment.ForeColor = Color.Red
            End If
        End If

        ' 投入後5mmの判定
        If Not String.IsNullOrEmpty(lblPost5mmUsed.Text) AndAlso lblPost5mmUsed.Text <> "不要" Then
            Dim shortage As Integer = Integer.Parse(lblPost5mmUsed.Text.Replace("個", ""))
            
            If shortage = 0 Then
                lblPost5mmJudgment.Text = "OK"
                lblPost5mmJudgment.ForeColor = Color.Green
            ElseIf shortage >= 1 Then
                lblPost5mmJudgment.Text = "不足"
                lblPost5mmJudgment.ForeColor = Color.Red
            Else
                lblPost5mmJudgment.Text = "過剰"
                lblPost5mmJudgment.ForeColor = Color.Red
            End If
        End If
        
        ' 投入後10mmの判定
        If Not String.IsNullOrEmpty(lblPost10mmUsed.Text) AndAlso lblPost10mmUsed.Text <> "不要" Then
            Dim shortage As Integer = Integer.Parse(lblPost10mmUsed.Text.Replace("個", ""))
            
            If shortage = 0 Then
                lblPost10mmJudgment.Text = "OK"
                lblPost10mmJudgment.ForeColor = Color.Green
            ElseIf shortage >= 1 Then
                lblPost10mmJudgment.Text = "不足"
                lblPost10mmJudgment.ForeColor = Color.Red
            Else
                lblPost10mmJudgment.Text = "過剰"
                lblPost10mmJudgment.ForeColor = Color.Red
            End If
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
    ''' <summary>
    ''' 照合ボタンクリック処理（2段階照合対応）
    ''' </summary>
    Private Sub BtnVerify_Click(sender As Object, e As EventArgs)
        If _currentCondition Is Nothing OrElse _currentMaterialCondition Is Nothing Then
            Return
        End If

        Dim employeeNo As String = txtEmployeeNo.Text.Trim()

        ' 従業員Noチェック
        If employeeNo.Length <> 6 Then
            ShowMessage("従業員Noは6桁で入力してください", Color.Red)
            Return
        End If

        Try
            If _verificationStage = 0 Then
                ' 【第1段階】投入前10mmの照合
                PerformFirstStageVerification(employeeNo)
            ElseIf _verificationStage = 1 Then
                ' 【第2段階】投入後1mm, 5mm, 10mmの照合
                PerformSecondStageVerification(employeeNo)
            End If

        Catch ex As Exception
            ShowMessage("照合エラー:" & ex.Message.Substring(0, Math.Min(20, ex.Message.Length)), Color.Red)
            _logManager.WriteErrorLog("照合時エラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 【第1段階】投入前10mmの照合処理
    ''' </summary>
    Private Sub PerformFirstStageVerification(employeeNo As String)
        ' 時間計測開始
        Dim sw As New Stopwatch()
        sw.Start()
        
        ' 天秤1から再度取得
        Dim pre10mmAfter As Integer = _balanceManager.ReadBalance(0)
        
        sw.Stop()
        _logManager.WriteErrorLog($"[第1段階照合] 天秤1取得時間: {sw.ElapsedMilliseconds}ms")
        
        ' 照合前の値を取得（Remainingが照合前の列）
        Dim pre10mmBefore As Integer = Integer.Parse(lblPre10mmRemaining.Text.Replace("個", ""))
        
        ' 使用枚数（差分）を計算
        Dim pre10mmUsed As Integer = pre10mmBefore - pre10mmAfter
        
        ' 不足枚数を計算：不足枚数 = 必要枚数 - 使用枚数
        Dim pre10mmShortage As Integer = _currentMaterialCondition.Pre10mm - pre10mmUsed
        
        ' 照合後 数と不足枚数を表示（Securedが照合後の列）
        lblPre10mmSecured.Text = pre10mmAfter.ToString() & "個"
        lblPre10mmUsed.Text = pre10mmShortage.ToString() & "個"
        
        ' 判定：不足枚数が0ならOK、>=1なら「不足」、<=-1なら「過剰」
        Dim isOk As Boolean = (pre10mmShortage = 0)
        If isOk Then
            lblPre10mmJudgment.Text = "OK"
            lblPre10mmJudgment.ForeColor = Color.Green
        ElseIf pre10mmShortage >= 1 Then
            lblPre10mmJudgment.Text = "不足"
            lblPre10mmJudgment.ForeColor = Color.Red
        Else ' pre10mmShortage <= -1
            lblPre10mmJudgment.Text = "過剰"
            lblPre10mmJudgment.ForeColor = Color.Red
        End If
        
        If isOk Then
            ' 第1段階OK: 投入後部材の取得と表示
            ShowMessage("照合OK。プロトスに入れた後に移載してください", Color.Green)
            _verificationStage = 1
            
            ' 投入後部材の情報を取得・表示
            DisplayPostMaterialsForSecondStage()
            
            ' ログ出力（第1段階）
            _logManager.WriteErrorLog($"[第1段階OK] 従業員No:{employeeNo}, カードNo:{txtCardNo.Text.Trim()}, 投入前10mm使用:{pre10mmUsed}, 不足枚数:{pre10mmShortage}")
            
        Else
            ' 第1段階NG
            Dim ngMessage As String = If(pre10mmShortage >= 1, "不足", "過剰")
            ShowMessage($"{ngMessage}: 投入前10mm 必要{_currentMaterialCondition.Pre10mm}個 使用{pre10mmUsed}個 不足{pre10mmShortage}個", Color.Red)
            _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, $"NG:第1段階({ngMessage})")
        End If
    End Sub
    
    ''' <summary>
    ''' 【第2段階】投入後部材の表示と取得
    ''' </summary>
    Private Sub DisplayPostMaterialsForSecondStage()
        ' 投入後部材の必要枚数を表示
        lblPost1mmRequired.Text = _currentMaterialCondition.Post1mm.ToString() & "個"
        lblPost5mmRequired.Text = _currentMaterialCondition.Post5mm.ToString() & "個"
        lblPost10mmRequired.Text = _currentMaterialCondition.Post10mm.ToString() & "個"
        
        ' エッジガードと気泡緩衝材の必要枚数を表示
        If _currentCondition.EdgeGuard = 0 Then
            lblEdgeRequired.Text = "不要"
        Else
            lblEdgeRequired.Text = _currentCondition.EdgeGuard.ToString() & "個"
        End If
        
        If _currentCondition.BubbleInterference = 0 Then
            lblBubbleRequired.Text = "不要"
        Else
            lblBubbleRequired.Text = _currentCondition.BubbleInterference.ToString() & "個"
        End If
        
        ' 天秤から照合前の値を取得
        Dim post1mmCount As Integer = _balanceManager.ReadBalance(1)  ' 天秤2
        Dim post5mmCount As Integer = _balanceManager.ReadBalance(2)  ' 天秤3
        Dim post10mmCount As Integer = _balanceManager.ReadBalance(0) ' 天秤1
        
        ' 照合前 数を表示（Remainingが照合前の列）
        lblPost1mmRemaining.Text = post1mmCount.ToString() & "個"
        lblPost5mmRemaining.Text = post5mmCount.ToString() & "個"
        lblPost10mmRemaining.Text = post10mmCount.ToString() & "個"
        
        ' その他の列はクリア
        lblPost1mmSecured.Text = ""
        lblPost1mmUsed.Text = ""
        lblPost1mmJudgment.Text = ""
        lblPost5mmSecured.Text = ""
        lblPost5mmUsed.Text = ""
        lblPost5mmJudgment.Text = ""
        lblPost10mmSecured.Text = ""
        lblPost10mmUsed.Text = ""
        lblPost10mmJudgment.Text = ""
        
        ' 照合ボタンは有効のまま（第2段階用）
        btnVerify.Enabled = True
    End Sub

    ''' <summary>
    ''' 【第2段階】投入後1mm, 5mm, 10mmの照合処理
    ''' </summary>
    Private Sub PerformSecondStageVerification(employeeNo As String)
        ' 天秤から再度取得
        Dim post1mmAfter As Integer = _balanceManager.ReadBalance(1)  ' 天秤2
        Dim post5mmAfter As Integer = _balanceManager.ReadBalance(2)  ' 天秤3
        Dim post10mmAfter As Integer = _balanceManager.ReadBalance(0) ' 天秤1
        
        ' 照合前の値を取得（Remainingが照合前の列）
        Dim post1mmBefore As Integer = Integer.Parse(lblPost1mmRemaining.Text.Replace("個", ""))
        Dim post5mmBefore As Integer = Integer.Parse(lblPost5mmRemaining.Text.Replace("個", ""))
        Dim post10mmBefore As Integer = Integer.Parse(lblPost10mmRemaining.Text.Replace("個", ""))
        
        ' 使用枚数（差分）を計算
        Dim post1mmUsed As Integer = post1mmBefore - post1mmAfter
        Dim post5mmUsed As Integer = post5mmBefore - post5mmAfter
        Dim post10mmUsed As Integer = post10mmBefore - post10mmAfter

        ' 照合後 数と使用枚数を表示（Securedが照合後の列）
        lblPost1mmSecured.Text = post1mmAfter.ToString() & "個"
        lblPost1mmUsed.Text = post1mmUsed.ToString() & "個"
        lblPost5mmSecured.Text = post5mmAfter.ToString() & "個"
        lblPost5mmUsed.Text = post5mmUsed.ToString() & "個"
        lblPost10mmSecured.Text = post10mmAfter.ToString() & "個"
        lblPost10mmUsed.Text = post10mmUsed.ToString() & "個"
        
        ' 判定
        Dim post1mmOk As Boolean = (post1mmUsed = _currentMaterialCondition.Post1mm)
        Dim post5mmOk As Boolean = (post5mmUsed = _currentMaterialCondition.Post5mm)
        Dim post10mmOk As Boolean = (post10mmUsed = _currentMaterialCondition.Post10mm)
        
        lblPost1mmJudgment.Text = If(post1mmOk, "OK", "NG")
        lblPost1mmJudgment.ForeColor = If(post1mmOk, Color.Green, Color.Red)
        lblPost5mmJudgment.Text = If(post5mmOk, "OK", "NG")
        lblPost5mmJudgment.ForeColor = If(post5mmOk, Color.Green, Color.Red)
        lblPost10mmJudgment.Text = If(post10mmOk, "OK", "NG")
        lblPost10mmJudgment.ForeColor = If(post10mmOk, Color.Green, Color.Red)
        
        Dim allOk As Boolean = post1mmOk AndAlso post5mmOk AndAlso post10mmOk
        
        If allOk Then
            ' 第2段階OK: 完了
            ShowMessage("照合OK 次工程に払出してください。", Color.Green)
            _verificationStage = 2
            btnVerify.Enabled = False
            
            ' ログ出力（第2段階）
            _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "OK")
            
            ' 入力欄をクリアしてリセット
            RemoveHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
            RemoveHandler txtCardNo.TextChanged, AddressOf TxtCardNo_TextChanged
            txtEmployeeNo.Text = ""
            txtCardNo.Text = ""
            cmbLapThickness.SelectedIndex = -1
            AddHandler txtEmployeeNo.TextChanged, AddressOf TxtEmployeeNo_TextChanged
            AddHandler txtCardNo.TextChanged, AddressOf TxtCardNo_TextChanged
            txtEmployeeNo.Enabled = True
            txtCardNo.Enabled = False
            cmbLapThickness.Enabled = False
            txtEmployeeNo.Focus()
            _verificationStage = 0
            
        Else
            ' 第2段階NG
            Dim ngMessage As String = "NG: "
            If Not post1mmOk Then ngMessage &= $"1mm(必要{_currentMaterialCondition.Post1mm}≠使用{post1mmUsed}) "
            If Not post5mmOk Then ngMessage &= $"5mm(必要{_currentMaterialCondition.Post5mm}≠使用{post5mmUsed}) "
            If Not post10mmOk Then ngMessage &= $"10mm(必要{_currentMaterialCondition.Post10mm}≠使用{post10mmUsed}) "
            
            ShowMessage(ngMessage, Color.Red)
            _logManager.WriteInspectionLog(employeeNo, txtCardNo.Text.Trim(), _currentCondition, "NG:第2段階")
        End If
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
        If _currentMaterialCondition Is Nothing Then
            Return False
        End If
        
        Dim isValid As Boolean = True

        ' Pre_10mm
        If differences.ContainsKey("Pre_10mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Pre_10mm")))
            If diff <> _currentMaterialCondition.Pre10mm Then
                isValid = False
            End If
        Else
            isValid = False
        End If

        ' Post_1mm
        If differences.ContainsKey("Post_1mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_1mm")))
            If diff <> _currentMaterialCondition.Post1mm Then
                isValid = False
            End If
        Else
            isValid = False
        End If

        ' Post_5mm
        If differences.ContainsKey("Post_5mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_5mm")))
            If diff <> _currentMaterialCondition.Post5mm Then
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
        If _currentMaterialCondition Is Nothing Then
            Return "NG: MaterialCondition not found"
        End If
        
        Dim errors As New List(Of String)()

        If differences.ContainsKey("Pre_10mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Pre_10mm")))
            If diff <> _currentMaterialCondition.Pre10mm Then
                errors.Add("10mm:" & diff.ToString() & "≠" & _currentMaterialCondition.Pre10mm.ToString())
            End If
        End If

        If differences.ContainsKey("Post_1mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_1mm")))
            If diff <> _currentMaterialCondition.Post1mm Then
                errors.Add("1mm:" & diff.ToString() & "≠" & _currentMaterialCondition.Post1mm.ToString())
            End If
        End If

        If differences.ContainsKey("Post_5mm") Then
            Dim diff As Integer = CInt(Math.Round(differences("Post_5mm")))
            If diff <> _currentMaterialCondition.Post5mm Then
                errors.Add("5mm:" & diff.ToString() & "≠" & _currentMaterialCondition.Post5mm.ToString())
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
        lblLotNoValue.Text = ""
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
                _logManager.WriteErrorLog("従業員No検索: 検索中のため処理をスキップ")
                Return
            End If

            _isSearchingEmployee = True

            Try
                ' ローディング表示
                ShowMessage("読み込み中…", Color.Black)
                lblEmployeeNameValue.Text = "検索中..."

                _logManager.WriteErrorLog("従業員No検索開始: " & employeeNo)

                ' 非同期で従業員を検索
                Dim employeeName As String = Await _employeeLoader.SearchAsync(employeeNo)
                
                _logManager.WriteErrorLog("従業員No検索結果: " & If(employeeName, "該当なし"))
                _logManager.WriteErrorLog("従業員No検索結果: " & If(employeeName, "該当なし"))

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
    ''' カードNo入力時の処理
    ''' </summary>
    Private Sub TxtCardNo_TextChanged(sender As Object, e As EventArgs)
        Dim cardNo As String = txtCardNo.Text.Trim()

        ' 6桁入力完了時にLAP厚選択欄を活性化
        If cardNo.Length = 6 Then
            ' カード情報を取得（品名、枚数、所在などの表示用）
            _currentCondition = _cardLoader.GetCondition(cardNo)
            
            If _currentCondition Is Nothing Then
                ShowMessage("該当するカード情報がありません", Color.Red)
                ClearConditionLabels()
                lblCardNoDisplayValue.Text = ""
                lblLotNoValue.Text = ""
                lblProductNameValue.Text = ""
                lblQuantityValue.Text = ""
                lblLocationValue.Text = ""
                cmbLapThickness.Enabled = False
                cmbLapThickness.SelectedIndex = -1
                btnVerify.Enabled = False
                
                ' カードNo入力欄をクリア
                txtCardNo.Text = ""
                txtCardNo.Focus()
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
            lblLotNoValue.Text = ""
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

        ' 照合状態をリセット
        _verificationStage = 0

        ' 【第1段階】投入前10mmのみ表示
        DisplayPre10mmOnly(_currentMaterialCondition)

        ' カードNo入力欄を非活性化
        txtCardNo.Enabled = False

        ' 【第1段階】天秤1のみから計測
        Try
            ' 測定中メッセージを表示
            ShowMessage("秤の値測定中...", Color.Black)
            
            ' 時間計測開始
            Dim sw As New Stopwatch()
            sw.Start()
            
            ' 天秤1のみから投入前10mmを取得
            Dim pre10mmCount As Integer = _balanceManager.ReadBalance(0) ' 天秤1
            
            sw.Stop()
            _logManager.WriteErrorLog($"[LAP厚選択] 天秤1取得時間: {sw.ElapsedMilliseconds}ms")
            
            ' 投入前10mmの照合前 数を表示（Remainingが照合前の列）
            lblPre10mmRemaining.Text = pre10mmCount.ToString() & "個"
            
            ' 測定完了後のメッセージ
            ShowMessage("投入前10mmを確認して照合ボタンを押してください", Color.Green)
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
            lblLotNoValue.Text = ""
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
            lblLotNoValue.Text = ""
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

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

