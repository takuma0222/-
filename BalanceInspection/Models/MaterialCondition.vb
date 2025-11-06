''' <summary>
''' 使用部材条件を保持するクラス
''' </summary>
Public Class MaterialCondition
    ''' <summary>
    ''' 枚数
    ''' </summary>
    Public Property Quantity As Integer

    ''' <summary>
    ''' LAP厚
    ''' </summary>
    Public Property LapThickness As String

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
End Class
