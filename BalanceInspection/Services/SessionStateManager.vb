Imports System.IO
Imports Newtonsoft.Json

''' <summary>
''' セッション状態を管理するクラス
''' </summary>
Public Class SessionStateManager
    Private _stateFilePath As String
    
    Public Sub New(stateFilePath As String)
        _stateFilePath = stateFilePath
    End Sub
    
    ''' <summary>
    ''' セッション状態を保存
    ''' </summary>
    Public Sub SaveState(state As SessionState)
        Try
            Dim json As String = JsonConvert.SerializeObject(state, Formatting.Indented)
            File.WriteAllText(_stateFilePath, json)
        Catch ex As Exception
            Throw New Exception("セッション状態の保存に失敗しました: " & ex.Message, ex)
        End Try
    End Sub
    
    ''' <summary>
    ''' セッション状態を読み込み
    ''' </summary>
    Public Function LoadState() As SessionState
        Try
            If Not File.Exists(_stateFilePath) Then
                Return Nothing
            End If
            
            Dim json As String = File.ReadAllText(_stateFilePath)
            Return JsonConvert.DeserializeObject(Of SessionState)(json)
        Catch ex As Exception
            ' ファイルが壊れている場合は削除
            DeleteState()
            Return Nothing
        End Try
    End Function
    
    ''' <summary>
    ''' セッション状態を削除
    ''' </summary>
    Public Sub DeleteState()
        Try
            If File.Exists(_stateFilePath) Then
                File.Delete(_stateFilePath)
            End If
        Catch ex As Exception
            ' 削除失敗は無視
        End Try
    End Sub
    
    ''' <summary>
    ''' セッション状態が存在するか確認
    ''' </summary>
    Public Function HasState() As Boolean
        Return File.Exists(_stateFilePath)
    End Function
End Class
