''' <summary>
''' アプリケーション設定を保持するクラス
''' </summary>
Public Class AppConfig
    ''' <summary>
    ''' ログ出力先ディレクトリ
    ''' </summary>
    Public Property LogDirectory As String = "logs"

    ''' <summary>
    ''' カード条件CSVファイルパス
    ''' </summary>
    Public Property CardConditionCsvPath As String = "card_conditions.csv"

    ''' <summary>
    ''' バランス設定リスト
    ''' </summary>
    Public Property Balances As List(Of BalanceConfig) = New List(Of BalanceConfig)()

    ''' <summary>
    ''' 通信タイムアウト（ミリ秒）
    ''' </summary>
    Public Property ReadTimeoutMs As Integer = 5000

    ''' <summary>
    ''' リトライ回数
    ''' </summary>
    Public Property MaxRetries As Integer = 3
End Class

