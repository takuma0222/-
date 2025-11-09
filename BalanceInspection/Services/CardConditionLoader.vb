Imports System.IO
Imports System.Text

''' <summary>
''' カード条件CSVを読み込むクラス
''' </summary>
Public Class CardConditionLoader
    Private _conditions As Dictionary(Of String, CardCondition)
    Private _csvPath As String

    Public Sub New(csvPath As String)
        _csvPath = csvPath
        _conditions = New Dictionary(Of String, CardCondition)()
        LoadConditions()
    End Sub

    ''' <summary>
    ''' CSVファイルを読み込む
    ''' </summary>
    Private Sub LoadConditions()
        Try
            If Not File.Exists(_csvPath) Then
                ' サンプルCSVを作成
                CreateSampleCsv()
            End If

            Dim lines As String() = File.ReadAllLines(_csvPath, Encoding.UTF8)
            
            If lines.Length < 2 Then
                Throw New Exception("CSVファイルが空です")
            End If

            ' ヘッダー行をスキップ
            For i As Integer = 1 To lines.Length - 1
                Dim line As String = lines(i).Trim()
                If String.IsNullOrEmpty(line) Then
                    Continue For
                End If

                Dim parts As String() = line.Split(","c)
                If parts.Length < 7 Then
                    Continue For
                End If

                Dim condition As New CardCondition()
                condition.CardNo = parts(0).Trim()
                condition.LotNo = parts(1).Trim()
                
                Integer.TryParse(parts(2).Trim(), condition.EdgeGuard)
                Integer.TryParse(parts(3).Trim(), condition.BubbleInterference)
                
                condition.ProductName = parts(4).Trim()
                Integer.TryParse(parts(5).Trim(), condition.Quantity)
                condition.Location = parts(6).Trim()
                
                ' 工程は空文字列に設定（CSVに存在しない）
                condition.Process = ""

                _conditions(condition.CardNo) = condition
            Next
        Catch ex As Exception
            Throw New Exception("CSVファイルの読み込みに失敗しました: " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' サンプルCSVファイルを作成
    ''' </summary>
    Private Sub CreateSampleCsv()
        Dim sb As New StringBuilder()
        sb.AppendLine("CardNo,ロットNo,エッジガード,気泡緩衝材,品名,枚数,所在")
        sb.AppendLine("E00123,000001###,0,0,AAA,25,AAA")
        sb.AppendLine("E00124,000002###,1,3,BBB,25,BBB")
        sb.AppendLine("E00125,000003###,1,10,BBB,25,AAA")
        sb.AppendLine("E00126,000004###,1,10,CCC,25,AAA")
        sb.AppendLine("E00127,000005###,1,10,DDD,25,AAA")
        
        File.WriteAllText(_csvPath, sb.ToString(), New UTF8Encoding(True))
    End Sub

    ''' <summary>
    ''' カード番号から条件を取得
    ''' </summary>
    Public Function GetCondition(cardNo As String) As CardCondition
        If _conditions.ContainsKey(cardNo) Then
            Return _conditions(cardNo)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' すべての条件を再読み込み
    ''' </summary>
    Public Sub Reload()
        _conditions.Clear()
        LoadConditions()
    End Sub
End Class

