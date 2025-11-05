''' <summary>
''' バランス設定を保持するクラス
''' </summary>
Public Class BalanceConfig
    ''' <summary>
    ''' 論理名（Pre_10mm, Post_1mm, Post_5mm など）
    ''' </summary>
    Public Property LogicalName As String

    ''' <summary>
    ''' COMポート名（例: COM1）
    ''' </summary>
    Public Property PortName As String

    ''' <summary>
    ''' ボーレート
    ''' </summary>
    Public Property BaudRate As Integer = 9600

    ''' <summary>
    ''' データビット
    ''' </summary>
    Public Property DataBits As Integer = 8

    ''' <summary>
    ''' パリティ（None, Odd, Even）
    ''' </summary>
    Public Property Parity As String = "None"

    ''' <summary>
    ''' ストップビット
    ''' </summary>
    Public Property StopBits As String = "One"

    ''' <summary>
    ''' 計測器ID（EK-iシリーズのID番号）
    ''' </summary>
    Public Property DeviceId As String = ""

    ''' <summary>
    ''' 接続タイプ（SerialPort または TCP）
    ''' </summary>
    Public Property ConnectionType As String = "SerialPort"

    ''' <summary>
    ''' TCP接続時のIPアドレス
    ''' </summary>
    Public Property TcpAddress As String = "127.0.0.1"

    ''' <summary>
    ''' TCP接続時のポート番号
    ''' </summary>
    Public Property TcpPort As Integer = 9001
End Class

