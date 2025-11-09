''' <summary>
''' カード情報を提供するインターフェース
''' </summary>
Public Interface ICardDataProvider
    ''' <summary>
    ''' カード番号からカード情報を取得
    ''' </summary>
    ''' <param name="cardNo">カード番号</param>
    ''' <returns>カード情報。見つからない場合はNothing</returns>
    Function GetCardCondition(cardNo As String) As CardCondition
    
    ''' <summary>
    ''' データプロバイダーを初期化
    ''' </summary>
    Sub Initialize()
End Interface
