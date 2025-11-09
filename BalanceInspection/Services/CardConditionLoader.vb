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
                If parts.Length < 8 Then
                    Continue For
                End If

                Dim condition As New CardCondition()
                condition.CardNo = parts(0).Trim()
                condition.LotNo = parts(1).Trim()
                
                Integer.TryParse(parts(2).Trim(), condition.Pre10mm)
                Integer.TryParse(parts(3).Trim(), condition.Post1mm)
                Integer.TryParse(parts(4).Trim(), condition.Post5mm)
                Integer.TryParse(parts(5).Trim(), condition.Post10mm)
                Integer.TryParse(parts(6).Trim(), condition.EdgeGuard)
                Integer.TryParse(parts(7).Trim(), condition.BubbleInterference)

                ' 品名、枚数、所在、工程(追加のカラム)
                If parts.Length >= 9 Then
                    condition.ProductName = parts(8).Trim()
                End If
                If parts.Length >= 10 Then
                    Integer.TryParse(parts(9).Trim(), condition.Quantity)
                End If
                If parts.Length >= 11 Then
                    condition.Location = parts(10).Trim()
                End If
                If parts.Length >= 12 Then
                    condition.Process = parts(11).Trim()
                End If

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
        sb.AppendLine("CardNo,ロットNo,投入前10mmクッション材,投入後1mmクッション材,投入後5mmクッション材,投入後10mmクッション材,エッジガード,気泡緩衝材,品名,枚数,所在")
        sb.AppendLine("E00123,000001###,1,2,0,0,0,0,AAA,25,AAA")
        sb.AppendLine("E00124,000002###,1,1,2,1,1,3,BBB,25,BBB")
        sb.AppendLine("E00125,000003###,2,1,1,0,1,10,BBB,25,AAA")
        sb.AppendLine("E00126,000004###,2,1,1,0,1,10,CCC,25,AAA")
        sb.AppendLine("E00127,000005###,2,1,1,0,1,10,DDD,25,AAA")
        
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

