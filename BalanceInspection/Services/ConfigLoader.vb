Imports System.IO
Imports Newtonsoft.Json

''' <summary>
''' 設定ファイル（JSON）を読み込むクラス
''' </summary>
Public Class ConfigLoader
    ''' <summary>
    ''' appsettings.jsonを読み込む
    ''' </summary>
    Public Shared Function Load() As AppConfig
        Try
            Dim configPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")
            
            If Not File.Exists(configPath) Then
                ' デフォルト設定を作成
                Dim defaultConfig As New AppConfig()
                defaultConfig.Balances.Add(New BalanceConfig() With {
                    .LogicalName = "Pre_10mm",
                    .PortName = "COM1",
                    .BaudRate = 9600,
                    .DataBits = 8,
                    .Parity = "None",
                    .StopBits = "One"
                })
                defaultConfig.Balances.Add(New BalanceConfig() With {
                    .LogicalName = "Post_1mm",
                    .PortName = "COM2",
                    .BaudRate = 9600,
                    .DataBits = 8,
                    .Parity = "None",
                    .StopBits = "One"
                })
                defaultConfig.Balances.Add(New BalanceConfig() With {
                    .LogicalName = "Post_5mm",
                    .PortName = "COM3",
                    .BaudRate = 9600,
                    .DataBits = 8,
                    .Parity = "None",
                    .StopBits = "One"
                })
                
                ' デフォルト設定を保存
                Dim jsonContent As String = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented)
                File.WriteAllText(configPath, jsonContent, System.Text.Encoding.UTF8)
                
                Return defaultConfig
            End If
            
            Dim json As String = File.ReadAllText(configPath, System.Text.Encoding.UTF8)
            Return JsonConvert.DeserializeObject(Of AppConfig)(json)
        Catch ex As Exception
            Throw New Exception("設定ファイルの読み込みに失敗しました: " & ex.Message, ex)
        End Try
    End Function
End Class
