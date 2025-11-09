''' <summary>
''' 棚保管情報モデル
''' </summary>
Public Class ShelfStorage
    ''' <summary>
    ''' 棚番号
    ''' </summary>
    Public Property ShelfNo As String
    
    ''' <summary>
    ''' カード番号
    ''' </summary>
    Public Property CardNo As String
    
    Public Sub New()
        ShelfNo = ""
        CardNo = ""
    End Sub
    
    Public Sub New(shelfNo As String, cardNo As String)
        Me.ShelfNo = shelfNo
        Me.CardNo = cardNo
    End Sub
End Class
