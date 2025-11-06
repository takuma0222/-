Imports System.IO
Imports System.Text

''' <summary>
''' 使用部材条件CSVを読み込むクラス
''' </summary>
Public Class MaterialConditionLoader
    Private _conditions As List(Of MaterialCondition)
    Private _csvPath As String

    Public Sub New(csvPath As String)
        _csvPath = csvPath
        _conditions = New List(Of MaterialCondition)()
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
                If parts.Length < 6 Then
                    Continue For
                End If

                Dim condition As New MaterialCondition()
                Integer.TryParse(parts(0).Trim(), condition.Quantity)
                condition.LapThickness = parts(1).Trim()
                Integer.TryParse(parts(2).Trim(), condition.Pre10mm)
                Integer.TryParse(parts(3).Trim(), condition.Post1mm)
                Integer.TryParse(parts(4).Trim(), condition.Post5mm)
                Integer.TryParse(parts(5).Trim(), condition.Post10mm)

                _conditions.Add(condition)
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
        sb.AppendLine("枚数,LAP厚,投入前10mm,投入後1mm,投入後5mm,投入後10mm")
        sb.AppendLine("25,200μm,1,2,0,0")
        sb.AppendLine("25,250μm,1,1,2,1")
        sb.AppendLine("25,290μm,2,1,1,0")
        sb.AppendLine("25,300μm,1,2,1,1")
        sb.AppendLine("25,350μm,2,2,1,0")
        sb.AppendLine("50,200μm,2,3,1,1")
        sb.AppendLine("50,250μm,2,2,2,1")
        sb.AppendLine("50,290μm,3,2,1,1")
        sb.AppendLine("50,300μm,2,3,2,1")
        sb.AppendLine("50,350μm,3,3,1,1")
        
        File.WriteAllText(_csvPath, sb.ToString(), New UTF8Encoding(True))
    End Sub

    ''' <summary>
    ''' 枚数とLAP厚から条件を取得
    ''' </summary>
    Public Function GetCondition(quantity As Integer, lapThickness As String) As MaterialCondition
        For Each condition In _conditions
            If condition.Quantity = quantity AndAlso condition.LapThickness = lapThickness Then
                Return condition
            End If
        Next
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
