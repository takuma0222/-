''' <summary>
''' カード条件を保持するクラス
''' </summary>
Public Class CardCondition
    ''' <summary>
    ''' カード番号
    ''' </summary>
    Public Property CardNo As String

    ''' <summary>
    ''' ロット番号
    ''' </summary>
    Public Property LotNo As String

    ''' <summary>
    ''' 投入前10mmクッション材
    ''' </summary>
    Public Property Pre10mm As Integer

    ''' <summary>
    ''' 投入後1mmクッション材
    ''' </summary>
    Public Property Post1mm As Integer

    ''' <summary>
    ''' 投入後5mmクッション材
    ''' </summary>
    Public Property Post5mm As Integer

    ''' <summary>
    ''' 投入後10mmクッション材
    ''' </summary>
    Public Property Post10mm As Integer

    ''' <summary>
    ''' エッジガード
    ''' </summary>
    Public Property EdgeGuard As Integer

    ''' <summary>
    ''' 気泡緩衝材
    ''' </summary>
    Public Property BubbleInterference As Integer

    ''' <summary>
    ''' 品名
    ''' </summary>
    Public Property ProductName As String

    ''' <summary>
    ''' 枚数
    ''' </summary>
    Public Property Quantity As Integer

    ''' <summary>
    ''' 所在
    ''' </summary>
    Public Property Location As String

    ''' <summary>
    ''' 工程
    ''' </summary>
    Public Property Process As String
End Class

