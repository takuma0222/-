Imports System.Collections.Generic

''' <summary>
''' 3台の天秤を管理するクラス
''' </summary>
Public Class BalanceManager
    Private _serialManagers As List(Of SerialPortManager)
    Private _initialReadings As Dictionary(Of String, Double)
    Private _verificationReadings As Dictionary(Of String, Double)

    Public Sub New(config As AppConfig)
        _serialManagers = New List(Of SerialPortManager)()
        _initialReadings = New Dictionary(Of String, Double)()
        _verificationReadings = New Dictionary(Of String, Double)()

        ' 各天秤のシリアルポートマネージャーを作成
        For Each balanceConfig As BalanceConfig In config.Balances
            Dim manager As New SerialPortManager(balanceConfig, config.ReadTimeoutMs, config.MaxRetries)
            _serialManagers.Add(manager)
        Next
    End Sub

    ''' <summary>
    ''' すべてのポートを開く
    ''' </summary>
    Public Sub OpenAll()
        Dim errors As New List(Of String)()
        
        For Each manager As SerialPortManager In _serialManagers
            Try
                manager.Open()
            Catch ex As Exception
                errors.Add($"{manager.LogicalName}: {ex.Message}")
            End Try
        Next
        
        If errors.Count > 0 Then
            Throw New Exception("ポートオープンエラー:" & vbCrLf & String.Join(vbCrLf, errors))
        End If
    End Sub

    ''' <summary>
    ''' すべてのポートを閉じる
    ''' </summary>
    Public Sub CloseAll()
        For Each manager As SerialPortManager In _serialManagers
            Try
                manager.Close()
            Catch ex As Exception
                ' エラーは無視
            End Try
        Next
    End Sub

    ''' <summary>
    ''' 初回計測（使用部材条件表示後）
    ''' </summary>
    Public Sub PerformInitialReading()
        _initialReadings.Clear()
        
        Dim errors As New List(Of String)()
        
        For Each manager As SerialPortManager In _serialManagers
            Try
                Dim value As Double = manager.ReadValue()
                _initialReadings(manager.LogicalName) = value
            Catch ex As Exception
                errors.Add($"{manager.LogicalName}: {ex.Message}")
            End Try
        Next
        
        If errors.Count > 0 Then
            Throw New Exception("初回計測エラー:" & vbCrLf & String.Join(vbCrLf, errors))
        End If
    End Sub

    ''' <summary>
    ''' 照合時計測（照合ボタン押下時）
    ''' </summary>
    Public Sub PerformVerificationReading()
        _verificationReadings.Clear()
        
        Dim errors As New List(Of String)()
        
        For Each manager As SerialPortManager In _serialManagers
            Try
                Dim value As Double = manager.ReadValue()
                _verificationReadings(manager.LogicalName) = value
            Catch ex As Exception
                errors.Add($"{manager.LogicalName}: {ex.Message}")
            End Try
        Next
        
        If errors.Count > 0 Then
            Throw New Exception("照合時計測エラー:" & vbCrLf & String.Join(vbCrLf, errors))
        End If
    End Sub

    ''' <summary>
    ''' 差分を計算（照合時 - 初回）
    ''' </summary>
    Public Function CalculateDifferences() As Dictionary(Of String, Double)
        Dim differences As New Dictionary(Of String, Double)()
        
        For Each logicalName As String In _initialReadings.Keys
            If _verificationReadings.ContainsKey(logicalName) Then
                Dim diff As Double = _verificationReadings(logicalName) - _initialReadings(logicalName)
                differences(logicalName) = diff
            End If
        Next
        
        Return differences
    End Function

    ''' <summary>
    ''' 初回計測値を取得
    ''' </summary>
    Public ReadOnly Property InitialReadings As Dictionary(Of String, Double)
        Get
            Return New Dictionary(Of String, Double)(_initialReadings)
        End Get
    End Property

    ''' <summary>
    ''' 照合時計測値を取得
    ''' </summary>
    Public ReadOnly Property VerificationReadings As Dictionary(Of String, Double)
        Get
            Return New Dictionary(Of String, Double)(_verificationReadings)
        End Get
    End Property
End Class
