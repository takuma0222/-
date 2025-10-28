Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

''' <summary>
''' 電子天秤シミュレータのメインフォーム
''' 3台の電子天秤をシミュレートし、TCP接続でコマンドを受信
''' </summary>
Public Class Form1
    ' UI コンポーネント
    Private lblTitle As Label
    Private lblBalance1 As Label
    Private txtWeight1 As TextBox
    Private btnSet1 As Button
    Private lblStatus1 As Label
    
    Private lblBalance2 As Label
    Private txtWeight2 As TextBox
    Private btnSet2 As Button
    Private lblStatus2 As Label
    
    Private lblBalance3 As Label
    Private txtWeight3 As TextBox
    Private btnSet3 As Button
    Private lblStatus3 As Label
    
    Private btnStartServer As Button
    Private btnStopServer As Button
    Private txtLog As TextBox
    Private btnClearLog As Button
    Private lblServerStatus As Label
    
    ' サーバー関連
    Private _server1 As TcpListener
    Private _server2 As TcpListener
    Private _server3 As TcpListener
    Private _isRunning As Boolean = False
    
    ' 重量値
    Private _weight1 As Double = 0.0
    Private _weight2 As Double = 0.0
    Private _weight3 As Double = 0.0
    
    ' ポート番号 (仮想COMポートとして動作)
    Private Const PORT1 As Integer = 9001  ' COM1 相当
    Private Const PORT2 As Integer = 9002  ' COM2 相当  
    Private Const PORT3 As Integer = 9003  ' COM3 相当
    
    Public Sub New()
        InitializeComponent()
        InitializeUI()
    End Sub
    
    ''' <summary>
    ''' UIコンポーネントを初期化
    ''' </summary>
    Private Sub InitializeUI()
        Me.Text = "電子天秤シミュレータ"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        
        ' タイトル
        lblTitle = New Label()
        lblTitle.Text = "電子天秤シミュレータ (3台)"
        lblTitle.Font = New Font("MS Gothic", 16, FontStyle.Bold)
        lblTitle.Location = New Point(20, 20)
        lblTitle.Size = New Size(400, 30)
        Me.Controls.Add(lblTitle)
        
        ' 天秤1
        CreateBalanceControls(1, 70, _weight1)
        
        ' 天秤2
        CreateBalanceControls(2, 120, _weight2)
        
        ' 天秤3
        CreateBalanceControls(3, 170, _weight3)
        
        ' サーバー制御
        btnStartServer = New Button()
        btnStartServer.Text = "サーバー開始"
        btnStartServer.Location = New Point(20, 230)
        btnStartServer.Size = New Size(120, 30)
        AddHandler btnStartServer.Click, AddressOf BtnStartServer_Click
        Me.Controls.Add(btnStartServer)
        
        btnStopServer = New Button()
        btnStopServer.Text = "サーバー停止"
        btnStopServer.Location = New Point(150, 230)
        btnStopServer.Size = New Size(120, 30)
        btnStopServer.Enabled = False
        AddHandler btnStopServer.Click, AddressOf BtnStopServer_Click
        Me.Controls.Add(btnStopServer)
        
        lblServerStatus = New Label()
        lblServerStatus.Text = "サーバー停止中"
        lblServerStatus.Location = New Point(290, 235)
        lblServerStatus.Size = New Size(200, 20)
        lblServerStatus.ForeColor = Color.Red
        Me.Controls.Add(lblServerStatus)
        
        ' ログ
        Dim lblLog As New Label()
        lblLog.Text = "通信ログ:"
        lblLog.Location = New Point(20, 280)
        lblLog.Size = New Size(100, 20)
        Me.Controls.Add(lblLog)
        
        txtLog = New TextBox()
        txtLog.Multiline = True
        txtLog.ScrollBars = ScrollBars.Vertical
        txtLog.ReadOnly = True
        txtLog.Location = New Point(20, 305)
        txtLog.Size = New Size(740, 200)
        txtLog.BackColor = Color.Black
        txtLog.ForeColor = Color.Lime
        txtLog.Font = New Font("Consolas", 9)
        Me.Controls.Add(txtLog)
        
        btnClearLog = New Button()
        btnClearLog.Text = "ログクリア"
        btnClearLog.Location = New Point(20, 520)
        btnClearLog.Size = New Size(100, 30)
        AddHandler btnClearLog.Click, AddressOf BtnClearLog_Click
        Me.Controls.Add(btnClearLog)
        
        LogMessage("シミュレータを初期化しました")
        LogMessage("ポート設定: Balance1=" & PORT1.ToString() & ", Balance2=" & PORT2.ToString() & ", Balance3=" & PORT3.ToString())
    End Sub
    
    ''' <summary>
    ''' 各天秤のUI コンポーネントを作成
    ''' </summary>
    Private Sub CreateBalanceControls(balanceNum As Integer, yPos As Integer, ByRef weight As Double)
        Dim lblBalance As New Label()
        lblBalance.Text = "天秤" & balanceNum.ToString() & " (Port " & (PORT1 + balanceNum - 1).ToString() & "):"
        lblBalance.Location = New Point(20, yPos)
        lblBalance.Size = New Size(150, 20)
        Me.Controls.Add(lblBalance)
        
        Dim txtWeight As New TextBox()
        txtWeight.Text = weight.ToString("F2")
        txtWeight.Location = New Point(180, yPos)
        txtWeight.Size = New Size(80, 20)
        txtWeight.Tag = balanceNum
        Me.Controls.Add(txtWeight)
        
        Dim lblUnit As New Label()
        lblUnit.Text = "g"
        lblUnit.Location = New Point(270, yPos)
        lblUnit.Size = New Size(20, 20)
        Me.Controls.Add(lblUnit)
        
        Dim btnSet As New Button()
        btnSet.Text = "設定"
        btnSet.Location = New Point(300, yPos - 2)
        btnSet.Size = New Size(60, 25)
        btnSet.Tag = balanceNum
        AddHandler btnSet.Click, AddressOf BtnSetWeight_Click
        Me.Controls.Add(btnSet)
        
        Dim lblStatus As New Label()
        lblStatus.Text = "待機中"
        lblStatus.Location = New Point(380, yPos)
        lblStatus.Size = New Size(200, 20)
        lblStatus.ForeColor = Color.Blue
        Me.Controls.Add(lblStatus)
        
        ' クラスフィールドに保存
        Select Case balanceNum
            Case 1
                Me.txtWeight1 = txtWeight
                Me.btnSet1 = btnSet
                Me.lblStatus1 = lblStatus
            Case 2
                Me.txtWeight2 = txtWeight
                Me.btnSet2 = btnSet
                Me.lblStatus2 = lblStatus
            Case 3
                Me.txtWeight3 = txtWeight
                Me.btnSet3 = btnSet
                Me.lblStatus3 = lblStatus
        End Select
    End Sub
    
    ''' <summary>
    ''' 重量設定ボタンのクリック処理
    ''' </summary>
    Private Sub BtnSetWeight_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim balanceNum As Integer = CType(btn.Tag, Integer)
        
        Dim txtWeight As TextBox = Nothing
        Select Case balanceNum
            Case 1
                txtWeight = txtWeight1
            Case 2
                txtWeight = txtWeight2
            Case 3
                txtWeight = txtWeight3
        End Select
        
        Dim newWeight As Double
        If Double.TryParse(txtWeight.Text, newWeight) Then
            Select Case balanceNum
                Case 1
                    _weight1 = newWeight
                Case 2
                    _weight2 = newWeight
                Case 3
                    _weight3 = newWeight
            End Select
            
            LogMessage("天秤" & balanceNum.ToString() & "の重量を" & newWeight.ToString("F2") & "gに設定しました")
        Else
            MessageBox.Show("有効な数値を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    
    ''' <summary>
    ''' サーバー開始ボタンのクリック処理
    ''' </summary>
    Private Sub BtnStartServer_Click(sender As Object, e As EventArgs)
        Try
            _server1 = New TcpListener(IPAddress.Any, PORT1)
            _server2 = New TcpListener(IPAddress.Any, PORT2)
            _server3 = New TcpListener(IPAddress.Any, PORT3)
            
            _server1.Start()
            _server2.Start()
            _server3.Start()
            
            _isRunning = True
            
            ' 各サーバーの接続待機を開始
            Task.Run(Sub() AcceptClients(_server1, 1))
            Task.Run(Sub() AcceptClients(_server2, 2))
            Task.Run(Sub() AcceptClients(_server3, 3))
            
            btnStartServer.Enabled = False
            btnStopServer.Enabled = True
            lblServerStatus.Text = "サーバー稼働中"
            lblServerStatus.ForeColor = Color.Green
            
            LogMessage("サーバーを開始しました")
            LogMessage("ポート " & PORT1.ToString() & ", " & PORT2.ToString() & ", " & PORT3.ToString() & " で接続待機中...")
            
        Catch ex As Exception
            MessageBox.Show("サーバー開始エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogMessage("サーバー開始エラー: " & ex.Message)
        End Try
    End Sub
    
    ''' <summary>
    ''' サーバー停止ボタンのクリック処理
    ''' </summary>
    Private Sub BtnStopServer_Click(sender As Object, e As EventArgs)
        Try
            _isRunning = False
            
            If _server1 IsNot Nothing Then
                _server1.Stop()
            End If
            If _server2 IsNot Nothing Then
                _server2.Stop()
            End If
            If _server3 IsNot Nothing Then
                _server3.Stop()
            End If
            
            btnStartServer.Enabled = True
            btnStopServer.Enabled = False
            lblServerStatus.Text = "サーバー停止中"
            lblServerStatus.ForeColor = Color.Red
            
            LogMessage("サーバーを停止しました")
            
        Catch ex As Exception
            MessageBox.Show("サーバー停止エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogMessage("サーバー停止エラー: " & ex.Message)
        End Try
    End Sub
    
    ''' <summary>
    ''' クライアント接続を待機
    ''' </summary>
    Private Sub AcceptClients(server As TcpListener, balanceNum As Integer)
        While _isRunning
            Try
                ' Pendingで接続があるかチェックしてからAccept
                If server.Pending() Then
                    Dim client As TcpClient = server.AcceptTcpClient()

                    Me.Invoke(Sub()
                                  UpdateStatus(balanceNum, "接続中", Color.Green)
                                  LogMessage("天秤" & balanceNum.ToString() & ": クライアント接続")
                              End Sub)

                    ' クライアント処理を別スレッドで実行
                    Task.Run(Sub() HandleClient(client, balanceNum))
                Else
                    ' 接続待ちの間は少し待機
                    Thread.Sleep(100)
                End If

            Catch ex As SocketException
                ' ソケットエラー（停止時など）は無視
                If _isRunning Then
                    Me.Invoke(Sub() LogMessage("天秤" & balanceNum.ToString() & ": ソケットエラー - " & ex.Message))
                End If
                Exit While
            Catch ex As Exception When _isRunning
                Me.Invoke(Sub() LogMessage("天秤" & balanceNum.ToString() & ": 接続エラー - " & ex.Message))
            End Try
        End While
    End Sub
    
    ''' <summary>
    ''' クライアントからの通信を処理
    ''' </summary>
    Private Sub HandleClient(client As TcpClient, balanceNum As Integer)
        Dim stream As NetworkStream = Nothing
        
        Try
            stream = client.GetStream()
            Dim buffer(1024) As Byte
            
            While client.Connected AndAlso _isRunning
                If stream.DataAvailable Then
                    Dim bytesRead As Integer = stream.Read(buffer, 0, buffer.Length)
                    
                    If bytesRead > 0 Then
                        Dim command As String = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim()
                        
                        Me.Invoke(Sub() LogMessage("天秤" & balanceNum.ToString() & ": 受信 '" & command & "'"))
                        
                        ' コマンド処理
                        Dim response As String = ProcessCommand(command, balanceNum)
                        
                        If Not String.IsNullOrEmpty(response) Then
                            Dim responseBytes As Byte() = Encoding.ASCII.GetBytes(response & vbCrLf)
                            stream.Write(responseBytes, 0, responseBytes.Length)
                            stream.Flush()
                            
                            Me.Invoke(Sub() LogMessage("天秤" & balanceNum.ToString() & ": 送信 '" & response & "'"))
                        End If
                    End If
                End If
                
                Thread.Sleep(100)
            End While
            
        Catch ex As Exception
            Me.Invoke(Sub() LogMessage("天秤" & balanceNum.ToString() & ": 通信エラー - " & ex.Message))
        Finally
            If stream IsNot Nothing Then
                stream.Close()
            End If
            If client IsNot Nothing Then
                client.Close()
            End If
            Me.Invoke(Sub()
                         UpdateStatus(balanceNum, "切断", Color.Red)
                         LogMessage("天秤" & (balanceNum).ToString() & ": クライアント切断")
                     End Sub)
        End Try
    End Sub
    
    ''' <summary>
    ''' 受信コマンドを処理してレスポンスを生成
    ''' </summary>
    Private Function ProcessCommand(command As String, balanceNum As Integer) As String
        Select Case command.ToUpper()
            Case "Q"  ' 重量問い合わせコマンド
                Dim weight As Double = GetWeight(balanceNum)
                ' EK-iシリーズの応答形式: "ST,GS,+0000.00g"
                Dim formattedWeight As String = If(weight >= 0, "+", "") & weight.ToString("0000.00")
                Return "ST,GS," & formattedWeight & "g"
            
            Case "Z", "ZERO"  ' ゼロ点調整（シミュレーションなので成功を返す）
                Return "ST,GS,Zero OK"
            
            Case "T", "TARE"  ' 風袋引き（シミュレーションなので成功を返す）
                Return "ST,GS,Tare OK"
            
            Case Else
                Return "ST,ES,Unknown Command"  ' エラー応答
        End Select
    End Function
    
    ''' <summary>
    ''' 指定された天秤の重量を取得
    ''' </summary>
    Private Function GetWeight(balanceNum As Integer) As Double
        Select Case balanceNum
            Case 1
                Return _weight1
            Case 2
                Return _weight2
            Case 3
                Return _weight3
            Case Else
                Return 0.0
        End Select
    End Function
    
    ''' <summary>
    ''' 天秤の状態を更新
    ''' </summary>
    Private Sub UpdateStatus(balanceNum As Integer, status As String, color As Color)
        Select Case balanceNum
            Case 1
                lblStatus1.Text = status
                lblStatus1.ForeColor = color
            Case 2
                lblStatus2.Text = status
                lblStatus2.ForeColor = color
            Case 3
                lblStatus3.Text = status
                lblStatus3.ForeColor = color
        End Select
    End Sub
    
    ''' <summary>
    ''' ログメッセージを追加
    ''' </summary>
    Private Sub LogMessage(message As String)
        If txtLog.InvokeRequired Then
            txtLog.Invoke(Sub() LogMessage(message))
        Else
            Dim timestamp As String = DateTime.Now.ToString("HH:mm:ss.fff")
            txtLog.AppendText("[{timestamp}] " & (message).ToString() & "" & vbCrLf)
            txtLog.ScrollToCaret()
        End If
    End Sub
    
    ''' <summary>
    ''' ログクリアボタンのクリック処理
    ''' </summary>
    Private Sub BtnClearLog_Click(sender As Object, e As EventArgs)
        txtLog.Clear()
        LogMessage("ログをクリアしました")
    End Sub
    
    ''' <summary>
    ''' フォームが閉じられる時の処理
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        _isRunning = False
        If _server1 IsNot Nothing Then
            _server1.Stop()
        End If
        If _server2 IsNot Nothing Then
            _server2.Stop()
        End If
        If _server3 IsNot Nothing Then
            _server3.Stop()
        End If
        MyBase.OnFormClosing(e)
    End Sub
End Class

