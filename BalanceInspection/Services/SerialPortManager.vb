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
            Throw New Exception($"ポート {_config.PortName} を開けませんでした: {ex.Message}", ex)
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
                    Throw New Exception($"タイムアウト: {_config.PortName} ({_config.LogicalName})")
                End If
                Thread.Sleep(500)
            Catch ex As Exception
                retryCount += 1
                If retryCount >= _maxRetries Then
                    Throw New Exception($"読み取りエラー: {_config.PortName} ({_config.LogicalName}) - {ex.Message}")
                End If
                Thread.Sleep(500)
            End Try
        End While
        
        Throw New Exception($"計測値の取得に失敗しました: {_config.PortName}")
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
            
            ' ST,GS,+0000.00g 形式をパース
            Dim parts As String() = response.Split(","c)
            If parts.Length < 3 Then
                Throw New Exception($"不正な応答形式: {response}")
            End If
            
            ' 重量部分を抽出（3番目の要素）
            Dim weightStr As String = parts(2).Trim()
            
            ' 単位（g, kg など）を除去
            weightStr = System.Text.RegularExpressions.Regex.Replace(weightStr, "[a-zA-Z]", "")
            
            ' 数値に変換
            Dim weight As Double
            If Double.TryParse(weightStr, weight) Then
                Return weight
            Else
                Throw New Exception($"数値変換失敗: {weightStr}")
            End If
            
        Catch ex As Exception
            Throw New Exception($"応答のパースに失敗: {response} - {ex.Message}")
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
