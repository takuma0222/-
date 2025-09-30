Imports System.IO.Ports
Imports System.Threading

''' <summary>
''' シリアルポート通信をシミュレートするクラス
''' 実際の検品アプリとの互換性のため、Named Pipe を使用してシリアルポート通信を模擬
''' </summary>
Public Class SerialPortSimulator
    Private _portName As String
    Private _isRunning As Boolean = False
    Private _currentWeight As Double = 0.0
    Private _balanceNumber As Integer
    Private _serverThread As Thread
    
    Public Event MessageReceived(message As String)
    Public Event WeightRequested(balanceNumber As Integer, weight As Double)
    
    Public Sub New(portName As String, balanceNumber As Integer)
        _portName = portName
        _balanceNumber = balanceNumber
    End Sub
    
    Public Sub SetWeight(weight As Double)
        _currentWeight = weight
    End Sub
    
    Public Sub StartSimulation()
        If _isRunning Then Return
        
        _isRunning = True
        _serverThread = New Thread(AddressOf SimulationLoop)
        _serverThread.IsBackground = True
        _serverThread.Start()
    End Sub
    
    Public Sub StopSimulation()
        _isRunning = False
        _serverThread?.Join(1000)
    End Sub
    
    Private Sub SimulationLoop()
        ' この部分は実際のシリアルポートエミュレーションのロジック
        ' 簡単なシミュレーションとして、定期的にイベントを発生
        
        While _isRunning
            Try
                Thread.Sleep(1000)
                
                ' 重量要求イベントを発生
                RaiseEvent WeightRequested(_balanceNumber, _currentWeight)
                
            Catch ex As ThreadAbortException
                Exit While
            Catch ex As Exception
                RaiseEvent MessageReceived($"シミュレーションエラー: {ex.Message}")
            End Try
        End While
    End Sub
End Class