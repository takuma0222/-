Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

''' <summary>
''' TCP通信を管理するクラス
''' </summary>
Public Class TcpCommunicationManager
    Private _client As TcpClient
    Private _stream As NetworkStream
    Private _config As BalanceConfig
    Private _readTimeoutMs As Integer
    Private _maxRetries As Integer
    
    Public ReadOnly Property LogicalName As String
        Get
            Return _config.LogicalName
        End Get
    End Property
    
    Public Sub New(config As BalanceConfig, readTimeoutMs As Integer, maxRetries As Integer)
        _config = config
        _readTimeoutMs = readTimeoutMs
        _maxRetries = maxRetries
    End Sub
    
    ''' <summary>
    ''' TCP接続を開く
    ''' </summary>
    Public Sub Open()
        Try
            If _client IsNot Nothing AndAlso _client.Connected Then
                _client.Close()
            End If
            
            _client = New TcpClient()
            _client.ReceiveTimeout = _readTimeoutMs
            _client.SendTimeout = _readTimeoutMs
            
            _client.Connect(_config.TcpAddress, _config.TcpPort)
            _stream = _client.GetStream()
            
        Catch ex As Exception
            Throw New Exception($"TCP接続 {_config.TcpAddress}:{_config.TcpPort} を開けませんでした: {ex.Message}", ex)
        End Try
    End Sub
    
    ''' <summary>
    ''' TCP接続を閉じる
    ''' </summary>
    Public Sub Close()
        Try
            _stream?.Close()
            _client?.Close()
        Catch ex As Exception
            ' 閉じる際のエラーは無視
        End Try
    End Sub
    
    ''' <summary>
    ''' 計測値を取得（TCP通信）
    ''' </summary>
    Public Function ReadValue() As Double
        Dim retryCount As Integer = 0
        
        While retryCount < _maxRetries
            Try
                ' 接続が確立されていない場合は接続する
                If _client Is Nothing OrElse Not _client.Connected Then
                    Open()
                End If
                
                ' 計測コマンド送信（EK-iシリーズのコマンド）
                Dim command As String = "Q" & vbCrLf
                Dim commandBytes As Byte() = Encoding.ASCII.GetBytes(command)
                _stream.Write(commandBytes, 0, commandBytes.Length)
                _stream.Flush()
                
                ' 応答待機
                Thread.Sleep(500)
                
                ' データ読み取り
                Dim buffer(1024) As Byte
                Dim bytesRead As Integer = _stream.Read(buffer, 0, buffer.Length)
                Dim response As String = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim()
                
                ' データをパース
                Dim value As Double = ParseWeight(response)
                Return value
                
            Catch ex As Exception
                retryCount += 1
                If retryCount >= _maxRetries Then
                    Throw New Exception($"TCP読み取りエラー: {_config.TcpAddress}:{_config.TcpPort} ({_config.LogicalName}) - {ex.Message}")
                End If
                Thread.Sleep(500)
            End Try
        End While
        
        Throw New Exception($"TCP計測値の取得に失敗しました: {_config.TcpAddress}:{_config.TcpPort}")
    End Function
    
    ''' <summary>
    ''' 指定されたタイムアウト時間でTCP計測値を取得
    ''' </summary>
    Public Function ReadValueWithTimeout(timeoutMs As Integer) As Double
        Dim originalTimeout As Integer = 0
        Dim retryCount As Integer = 0
        
        Try
            ' 接続が確立されていない場合は接続する
            If _client Is Nothing OrElse Not _client.Connected Then
                Open()
            End If
            
            ' 一時的にタイムアウトを変更
            originalTimeout = _client.ReceiveTimeout
            _client.ReceiveTimeout = timeoutMs
            
            While retryCount < _maxRetries
                Try
                    ' 計測コマンド送信
                    Dim command As String = "Q" & vbCrLf
                    Dim commandBytes As Byte() = Encoding.ASCII.GetBytes(command)
                    _stream.Write(commandBytes, 0, commandBytes.Length)
                    _stream.Flush()
                    
                    ' 応答待機
                    Thread.Sleep(500)
                    
                    ' データ読み取り
                    Dim buffer(1024) As Byte
                    Dim bytesRead As Integer = _stream.Read(buffer, 0, buffer.Length)
                    Dim response As String = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim()
                    
                    ' データをパース
                    Dim value As Double = ParseWeight(response)
                    Return value
                    
                Catch ex As Exception
                    retryCount += 1
                    If retryCount >= _maxRetries Then
                        Throw New Exception($"TCP初回計測タイムアウト({timeoutMs}ms): {_config.TcpAddress}:{_config.TcpPort} ({_config.LogicalName})")
                    End If
                    Thread.Sleep(500)
                End Try
            End While
            
            Throw New Exception($"TCP初回計測値の取得に失敗しました: {_config.TcpAddress}:{_config.TcpPort}")
            
        Finally
            ' タイムアウトを元に戻す
            If _client IsNot Nothing AndAlso _client.Connected Then
                _client.ReceiveTimeout = originalTimeout
            End If
        End Try
    End Function
    
    ''' <summary>
    ''' 応答文字列から重量を抽出
    ''' EK-iシリーズの応答形式: "ST,GS,+0000.00g" など
    ''' </summary>
    Private Function ParseWeight(response As String) As Double
        Try
            If String.IsNullOrEmpty(response) Then
                Throw New Exception("空の応答")
            End If
            
            ' EK-iシリーズの応答形式をパース
            ' 例: "ST,GS,+0123.45g"
            Dim parts As String() = response.Split(","c)
            
            If parts.Length >= 3 Then
                Dim weightPart As String = parts(2).Trim()
                
                ' "g" を除去
                If weightPart.EndsWith("g") Then
                    weightPart = weightPart.Substring(0, weightPart.Length - 1)
                End If
                
                ' 数値に変換
                Dim weight As Double
                If Double.TryParse(weightPart, weight) Then
                    Return weight
                Else
                    Throw New Exception($"重量データの変換に失敗: {weightPart}")
                End If
            Else
                Throw New Exception($"応答フォーマットが不正: {response}")
            End If
            
        Catch ex As Exception
            Throw New Exception($"重量データの解析に失敗: {response} - {ex.Message}")
        End Try
    End Function
    
    ''' <summary>
    ''' リソースの解放
    ''' </summary>
    Public Sub Dispose()
        Close()
    End Sub
End Class