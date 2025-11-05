Imports System.IO
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' 従業員情報CSVを読み込むクラス
''' </summary>
Public Class EmployeeLoader
    Private _employees As Dictionary(Of String, String)
    Private _csvPath As String
    Private _logManager As LogManager

    Public Sub New(csvPath As String, logManager As LogManager)
        _csvPath = csvPath
        _logManager = logManager
        _employees = New Dictionary(Of String, String)()
    End Sub

    ''' <summary>
    ''' CSVファイルを非同期で読み込む
    ''' </summary>
    Public Async Function LoadAsync() As Task
        Try
            If Not File.Exists(_csvPath) Then
                _logManager.WriteErrorLog("CSV file not found: " & _csvPath)
                Throw New FileNotFoundException("従業員CSVファイルが見つかりません: " & _csvPath)
            End If

            Await Task.Run(Sub() LoadEmployeesFromFile())

        Catch ex As FileNotFoundException
            Throw
        Catch ex As Exception
            _logManager.WriteErrorLog("従業員CSV読み込みエラー: " & ex.Message & vbCrLf & ex.StackTrace)
            Throw New Exception("データ読み取りエラーが発生しました", ex)
        End Try
    End Function

    ''' <summary>
    ''' ファイルから従業員情報を読み込む（同期処理）
    ''' </summary>
    Private Sub LoadEmployeesFromFile()
        _employees.Clear()

        Dim lines As String() = File.ReadAllLines(_csvPath, Encoding.UTF8)

        If lines.Length = 0 Then
            Throw New Exception("CSVファイルが空です")
        End If

        ' 全行を処理（ヘッダー行がない前提、またはヘッダー行も処理）
        For Each line As String In lines
            Dim trimmedLine As String = line.Trim()
            If String.IsNullOrEmpty(trimmedLine) Then
                Continue For
            End If

            Dim parts As String() = trimmedLine.Split(","c)
            If parts.Length < 2 Then
                Continue For
            End If

            Dim employeeNo As String = parts(0).Trim()
            Dim employeeName As String = parts(1).Trim()

            ' 6桁の数字のみを有効な従業員NOとする
            If employeeNo.Length = 6 AndAlso IsNumeric(employeeNo) Then
                _employees(employeeNo) = employeeName
            End If
        Next
    End Sub

    ''' <summary>
    ''' 従業員NOから氏名を非同期で検索
    ''' </summary>
    Public Async Function SearchAsync(employeeNo As String) As Task(Of String)
        Try
            ' 必要に応じて最新データを読み込む（初回または更新時）
            If _employees.Count = 0 Then
                Await LoadAsync()
            End If

            ' 検索キーをログに記録（開発/運用ログとして利用）
            _logManager.WriteErrorLog("従業員NO検索: " & employeeNo)

            Return Await Task.Run(Function()
                If _employees.ContainsKey(employeeNo) Then
                    Return _employees(employeeNo)
                Else
                    Return Nothing
                End If
            End Function)

        Catch ex As Exception
            _logManager.WriteErrorLog("従業員NO検索エラー: " & ex.Message)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' すべての従業員情報を再読み込み
    ''' </summary>
    Public Async Function ReloadAsync() As Task
        _employees.Clear()
        Await LoadAsync()
    End Function
End Class
