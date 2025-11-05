Imports System.IO.Ports
Imports System.Text
Imports System.Threading

''' <summary>
''' シリアルポート通信を管理するクラス
''' </summary>
Public Class SerialPortManager
    Private _port As SerialPort
    Private _config As BalanceConfig
    Private _readTimeoutMs As Integer
    Private _maxRetries As Integer

    Public Sub New(config As BalanceConfig, readTimeoutMs As Integer, maxRetries As Integer)
        _config = config
        _readTimeoutMs = readTimeoutMs
        _maxRetries = maxRetries
    End Sub

    ''' <summary>
    ''' ポートを開く
    ''' </summary>
    Public Sub Open()
        Try
            If _port IsNot Nothing AndAlso _port.IsOpen Then
                _port.Close()
            End If

            _port = New SerialPort()
            _port.PortName = _config.PortName
            _port.BaudRate = _config.BaudRate
            _port.DataBits = _config.DataBits
            
            ' パリティの設定
            Select Case _config.Parity.ToUpper()
                Case "NONE"
                    _port.Parity = Parity.None
                Case "ODD"
                    _port.Parity = Parity.Odd
                Case "EVEN"
                    _port.Parity = Parity.Even
                Case Else
                    _port.Parity = Parity.None
            End Select
            
            ' ストップビットの設定
            Select Case _config.StopBits.ToUpper()
                Case "ONE"
                    _port.StopBits = StopBits.One
                Case "TWO"
                    _port.StopBits = StopBits.Two
                Case Else
                    _port.StopBits = StopBits.One
            End Select
            
            _port.ReadTimeout = _readTimeoutMs
            _port.WriteTimeout = _readTimeoutMs
            _port.NewLine = vbCrLf
            
            _port.Open()
        Catch ex As Exception
            Throw New Exception("ポート {_config.PortName} を開けませんでした: " & (ex.Message).ToString() & "", ex)
        End Try
    End Sub

    ''' <summary>
    ''' ポートを閉じる
    ''' </summary>
    Public Sub Close()
        Try
            If _port IsNot Nothing AndAlso _port.IsOpen Then
                _port.Close()
            End If
        Catch ex As Exception
            ' 閉じる際のエラーは無視
        End Try
    End Sub

    ''' <summary>
    ''' 計測値を取得（コマンドモード）
    ''' </summary>
    Public Function ReadValue() As Double
        Dim retryCount As Integer = 0
        
        While retryCount < _maxRetries
            Try
                ' バッファをクリア
                If _port.IsOpen Then
                    _port.DiscardInBuffer()
                    _port.DiscardOutBuffer()
                End If
                
                ' 計測コマンド送信（EK-iシリーズのコマンド）
                _port.WriteLine("Q")
                
                ' 応答待機
                Thread.Sleep(500)
                
                ' データ読み取り
                Dim response As String = _port.ReadLine()
                
                ' データをパース
                Dim value As Double = ParseWeight(response)
                Return value
                
            Catch ex As TimeoutException
                retryCount += 1
                If retryCount >= _maxRetries Then
                    Throw New Exception("タイムアウト: {_config.PortName} (" & (_config.LogicalName).ToString() & ")")
                End If
                Thread.Sleep(500)
            Catch ex As Exception
                retryCount += 1
                If retryCount >= _maxRetries Then
                    Throw New Exception("読み取りエラー: {_config.PortName} ({_config.LogicalName}) - " & (ex.Message).ToString() & "")
                End If
                Thread.Sleep(500)
            End Try
        End While
        
        Throw New Exception("計測値の取得に失敗しました: " & (_config.PortName).ToString() & "")
    End Function

    ''' <summary>
    ''' 指定されたタイムアウト時間で計測値を取得（コマンドモード）
    ''' </summary>
    Public Function ReadValueWithTimeout(timeoutMs As Integer) As Double
        Dim originalTimeout As Integer = _port.ReadTimeout
        Dim retryCount As Integer = 0
        
        Try
            ' 一時的にタイムアウトを変更
            _port.ReadTimeout = timeoutMs
            
            While retryCount < _maxRetries
                Try
                    ' バッファをクリア
                    If _port.IsOpen Then
                        _port.DiscardInBuffer()
                        _port.DiscardOutBuffer()
                    End If
                    
                    ' 計測コマンド送信（EK-iシリーズのコマンド）
                    _port.WriteLine("Q")
                    
                    ' 応答待機
                    Thread.Sleep(500)
                    
                    ' データ読み取り
                    Dim response As String = _port.ReadLine()
                    
                    ' データをパース
                    Dim value As Double = ParseWeight(response)
                    Return value
                    
                Catch ex As TimeoutException
                    retryCount += 1
                    If retryCount >= _maxRetries Then
                        Throw New Exception("初回計測タイムアウト({timeoutMs}ms): {_config.PortName} (" & (_config.LogicalName).ToString() & ")")
                    End If
                    Thread.Sleep(500)
                Catch ex As Exception
                    retryCount += 1
                    If retryCount >= _maxRetries Then
                        Throw New Exception("初回計測読み取りエラー: {_config.PortName} ({_config.LogicalName}) - " & (ex.Message).ToString() & "")
                    End If
                    Thread.Sleep(500)
                End Try
            End While
            
            Throw New Exception("初回計測値の取得に失敗しました: " & (_config.PortName).ToString() & "")
            
        Finally
            ' タイムアウトを元に戻す
            If _port IsNot Nothing AndAlso _port.IsOpen Then
                _port.ReadTimeout = originalTimeout
            End If
        End Try
    End Function

    ''' <summary>
    ''' 応答文字列から計測値（個数）を抽出
    ''' 新しい応答形式: "ST,+00123456 PC"
    ''' </summary>
    Private Function ParseWeight(response As String) As Double
        Try
            If String.IsNullOrEmpty(response) Then
                Throw New Exception("空の応答")
            End If
            
            ' 新しい応答形式をパース
            ' 例: "ST,+00123456 PC"
            ' 先頭: "ST,"、符号: "+" or "-"、数値: 8桁（ゼロパディング）、末尾: " PC"（半角スペース + "PC"、固定3文字）
            
            ' "ST," で始まることを確認
            If Not response.StartsWith("ST,") Then
                Throw New Exception("応答フォーマットが不正（ST,で始まらない）: " & response)
            End If
            
            ' " PC" で終わることを確認
            If Not response.EndsWith(" PC") Then
                Throw New Exception("応答フォーマットが不正（ PCで終わらない）: " & response)
            End If
            
            ' "ST," の後から " PC" の前までを抽出
            Dim startIndex As Integer = 3 ' "ST," の長さ
            Dim endIndex As Integer = response.Length - 3 ' " PC" の長さ
            Dim countPart As String = response.Substring(startIndex, endIndex - startIndex)
            
            ' 符号を含む数値文字列（例: "+00123456" または "-00000100"）を整数値に変換
            Dim count As Double
            If Double.TryParse(countPart, count) Then
                Return count
            Else
                Throw New Exception("計測値データの変換に失敗: " & countPart)
            End If
            
        Catch ex As Exception
            Throw New Exception("計測値データの解析に失敗: " & response & " - " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' ポートが開いているか
    ''' </summary>
    Public ReadOnly Property IsOpen As Boolean
        Get
            Return _port IsNot Nothing AndAlso _port.IsOpen
        End Get
    End Property

    ''' <summary>
    ''' 論理名を取得
    ''' </summary>
    Public ReadOnly Property LogicalName As String
        Get
            Return _config.LogicalName
        End Get
    End Property
End Class

