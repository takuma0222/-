''' <summary>
''' セッション状態を保持するクラス
''' </summary>
Public Class SessionState
    ''' <summary>
    ''' タイムスタンプ
    ''' </summary>
    Public Property Timestamp As DateTime
    
    ''' <summary>
    ''' 照合段階 (0=未開始, 1=第1段階完了, 2=完了)
    ''' </summary>
    Public Property Stage As Integer
    
    ''' <summary>
    ''' 従業員No
    ''' </summary>
    Public Property EmployeeNo As String
    
    ''' <summary>
    ''' 従業員名
    ''' </summary>
    Public Property EmployeeName As String
    
    ''' <summary>
    ''' カードNo
    ''' </summary>
    Public Property CardNo As String
    
    ''' <summary>
    ''' LAP厚
    ''' </summary>
    Public Property LapThickness As String
    
    ''' <summary>
    ''' 投入前10mm照合前の数（第1段階で固定）
    ''' </summary>
    Public Property Pre10mmBefore As Integer
    
    ''' <summary>
    ''' 投入前10mm照合後の数
    ''' </summary>
    Public Property Pre10mmAfter As Integer
    
    ''' <summary>
    ''' 投入前10mm過不足数
    ''' </summary>
    Public Property Pre10mmShortage As Integer
    
    ''' <summary>
    ''' 投入前10mm判定結果（OK/不足/過剰）
    ''' </summary>
    Public Property Pre10mmJudgment As String
    
    ''' <summary>
    ''' カード条件（JSON文字列として保存）
    ''' </summary>
    Public Property CardConditionJson As String
    
    ''' <summary>
    ''' 使用部材条件（JSON文字列として保存）
    ''' </summary>
    Public Property MaterialConditionJson As String
End Class
