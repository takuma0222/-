Imports System.IO
Imports System.Text

''' <summary>
''' ログ出力を管理するクラス
''' </summary>
Public Class LogManager
    Private _logDirectory As String

    Public Sub New(logDirectory As String)
        _logDirectory = logDirectory
        
        ' ログディレクトリが存在しない場合は作成
        If Not Directory.Exists(_logDirectory) Then
            Directory.CreateDirectory(_logDirectory)
        End If
    End Sub

    ''' <summary>
    ''' 検査ログを出力（成功時）
    ''' </summary>
    Public Sub WriteInspectionLog(employeeNo As String, cardNo As String, condition As CardCondition, result As String)
        Try
            Dim logFileName As String = DateTime.Now.ToString("yyyyMMdd") & ".csv"
            Dim logPath As String = Path.Combine(_logDirectory, logFileName)
            
            Dim isNewFile As Boolean = Not File.Exists(logPath)
            
            Using writer As New StreamWriter(logPath, True, New UTF8Encoding(True))
                ' ヘッダー行を書き込み（新規ファイルの場合）
                If isNewFile Then
                    writer.WriteLine("Timestamp,EmployeeNo,CardNo,EdgeGuard,BubbleInterference,Result")
                End If
                
                ' データ行を書き込み
                Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                Dim logLine As String = timestamp & "," & employeeNo & "," & cardNo & "," & condition.EdgeGuard.ToString() & "," & condition.BubbleInterference.ToString() & "," & result.ToString()
                writer.WriteLine(logLine)
            End Using
            
        Catch ex As Exception
            Throw New Exception("ログ書き込みエラー: " & (ex.Message).ToString() & "", ex)
        End Try
    End Sub

    ''' <summary>
    ''' エラーログを出力
    ''' </summary>
    Public Sub WriteErrorLog(errorMessage As String)
        Try
            Dim errorDir As String = Path.Combine(_logDirectory, "error")
            If Not Directory.Exists(errorDir) Then
                Directory.CreateDirectory(errorDir)
            End If
            
            Dim errorFileName As String = DateTime.Now.ToString("yyyy-MM-dd") & ".log"
            Dim errorPath As String = Path.Combine(errorDir, errorFileName)
            
            Using writer As New StreamWriter(errorPath, True, New UTF8Encoding(True))
                Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                writer.WriteLine("[" & timestamp & "] " & errorMessage)
            End Using
            
        Catch ex As Exception
            ' エラーログの書き込み失敗は無視
        End Try
    End Sub
End Class

