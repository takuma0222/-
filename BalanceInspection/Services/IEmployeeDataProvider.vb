''' <summary>
''' 従業員情報を提供するインターフェース
''' </summary>
Public Interface IEmployeeDataProvider
    ''' <summary>
    ''' 従業員番号から従業員情報を取得
    ''' </summary>
    ''' <param name="employeeNo">従業員番号</param>
    ''' <returns>従業員名。見つからない場合はNothing</returns>
    Function GetEmployeeName(employeeNo As String) As String
    
    ''' <summary>
    ''' データプロバイダーを初期化
    ''' </summary>
    Sub Initialize()
End Interface
