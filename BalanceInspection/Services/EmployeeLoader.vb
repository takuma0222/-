Imports System.IO
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' 従業員データCSVを読み込むクラス
''' </summary>
Public Class EmployeeLoader
    Private _employees As Dictionary(Of String, String)
    Private _csvPath As String
    Private _isLoaded As Boolean = False

    Public Sub New(csvPath As String)
        _csvPath = csvPath
        _employees = New Dictionary(Of String, String)()
    End Sub

    ''' <summary>
    ''' CSVファイルを非同期で読み込む
    ''' </summary>
    Public Async Function LoadAsync() As Task
        Try
            If Not File.Exists(_csvPath) Then
                Throw New FileNotFoundException($"CSV file not found: {_csvPath}")
            End If

            ' 非同期でファイル読み込み
            Dim lines As String() = Await Task.Run(Function() File.ReadAllLines(_csvPath, Encoding.UTF8))
            
            If lines.Length < 2 Then
                Throw New Exception("CSVファイルが空です")
            End If

            _employees.Clear()

            ' ヘッダー行をスキップ
            For i As Integer = 1 To lines.Length - 1
                Dim line As String = lines(i).Trim()
                If String.IsNullOrEmpty(line) Then
                    Continue For
                End If

                Dim parts As String() = line.Split(","c)
                If parts.Length < 2 Then
                    Continue For
                End If

                Dim employeeNo As String = parts(0).Trim()
                Dim name As String = parts(1).Trim()

                ' 従業員NOは6桁の数字であることを確認
                If employeeNo.Length = 6 AndAlso IsNumeric(employeeNo) Then
                    _employees(employeeNo) = name
                End If
            Next

            _isLoaded = True

        Catch ex As Exception
            _isLoaded = False
            Throw New Exception("CSVファイルの読み込みに失敗しました: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' 従業員番号から氏名を非同期で取得
    ''' </summary>
    Public Async Function GetEmployeeNameAsync(employeeNo As String) As Task(Of String)
        ' 初回アクセス時または未ロード時に読み込み
        If Not _isLoaded Then
            Await LoadAsync()
        End If

        If _employees.ContainsKey(employeeNo) Then
            Return _employees(employeeNo)
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' すべての従業員データを再読み込み
    ''' </summary>
    Public Async Function ReloadAsync() As Task
        _employees.Clear()
        _isLoaded = False
        Await LoadAsync()
    End Function
End Class
