''' <summary>
''' データプロバイダーの実装を切り替えるファクトリークラス
''' </summary>
Public Class DataProviderFactory
    ''' <summary>
    ''' データソースの種類
    ''' </summary>
    Public Enum DataSourceType
        CSV          ' CSVファイルから取得
        Socket       ' ソケット通信で取得
    End Enum
    
    Private _dataSourceType As DataSourceType
    Private _logManager As LogManager
    
    Public Sub New(dataSourceType As DataSourceType, logManager As LogManager)
        _dataSourceType = dataSourceType
        _logManager = logManager
    End Sub
    
    ''' <summary>
    ''' 従業員データプロバイダーを生成
    ''' </summary>
    Public Function CreateEmployeeDataProvider(Optional csvPath As String = "", Optional serverAddress As String = "", Optional serverPort As Integer = 0) As IEmployeeDataProvider
        Select Case _dataSourceType
            Case DataSourceType.CSV
                If String.IsNullOrEmpty(csvPath) Then
                    csvPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\employees.csv")
                End If
                Return New EmployeeLoader(csvPath, _logManager)
                
            Case DataSourceType.Socket
                If String.IsNullOrEmpty(serverAddress) OrElse serverPort = 0 Then
                    Throw New ArgumentException("ソケット通信にはサーバーアドレスとポートが必要です")
                End If
                Return New SocketEmployeeDataProvider(serverAddress, serverPort, _logManager)
                
            Case Else
                Throw New NotSupportedException("サポートされていないデータソースタイプです")
        End Select
    End Function
    
    ''' <summary>
    ''' カードデータプロバイダーを生成
    ''' </summary>
    Public Function CreateCardDataProvider(Optional csvPath As String = "", Optional serverAddress As String = "", Optional serverPort As Integer = 0) As ICardDataProvider
        Select Case _dataSourceType
            Case DataSourceType.CSV
                If String.IsNullOrEmpty(csvPath) Then
                    csvPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "card_conditions.csv")
                End If
                Return New CardConditionLoader(csvPath)
                
            Case DataSourceType.Socket
                If String.IsNullOrEmpty(serverAddress) OrElse serverPort = 0 Then
                    Throw New ArgumentException("ソケット通信にはサーバーアドレスとポートが必要です")
                End If
                Return New SocketCardDataProvider(serverAddress, serverPort, _logManager)
                
            Case Else
                Throw New NotSupportedException("サポートされていないデータソースタイプです")
        End Select
    End Function
End Class
