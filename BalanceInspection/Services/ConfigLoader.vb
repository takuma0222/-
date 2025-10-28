Imports System.IO

''' <summary>
''' 設定ファイルを読み込むクラス
''' </summary>
Public Class ConfigLoader
    ''' <summary>
    ''' デフォルト設定を読み込む
    ''' </summary>
    Public Shared Function Load() As AppConfig
        Try
            ' デフォルト設定を作成（電子天秤シミュレータ対応）
            Dim defaultConfig As New AppConfig()
            defaultConfig.Balances.Add(New BalanceConfig() With {
                .LogicalName = "Pre_10mm",
                .ConnectionType = "TCP",
                .TcpAddress = "127.0.0.1",
                .TcpPort = 9001,
                .PortName = "SIM1",
                .BaudRate = 9600,
                .DataBits = 8,
                .Parity = "None",
                .StopBits = "One"
            })
            defaultConfig.Balances.Add(New BalanceConfig() With {
                .LogicalName = "Post_1mm",
                .ConnectionType = "TCP",
                .TcpAddress = "127.0.0.1",
                .TcpPort = 9002,
                .PortName = "SIM2",
                .BaudRate = 9600,
                .DataBits = 8,
                .Parity = "None",
                .StopBits = "One"
            })
            defaultConfig.Balances.Add(New BalanceConfig() With {
                .LogicalName = "Post_5mm",
                .ConnectionType = "TCP",
                .TcpAddress = "127.0.0.1",
                .TcpPort = 9003,
                .PortName = "SIM3",
                .BaudRate = 9600,
                .DataBits = 8,
                .Parity = "None",
                .StopBits = "One"
            })
            
            Return defaultConfig
        Catch ex As Exception
            Throw New Exception("設定ファイルの読み込みに失敗しました: " & ex.Message, ex)
        End Try
    End Function
End Class

