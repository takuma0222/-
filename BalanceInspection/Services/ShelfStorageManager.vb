Imports System.IO
Imports System.Text

''' <summary>
''' 棚保管情報を管理するクラス
''' </summary>
Public Class ShelfStorageManager
    Private _csvPath As String
    Private _storages As Dictionary(Of String, ShelfStorage) ' Key: 棚番号
    
    Public Sub New(csvPath As String)
        _csvPath = csvPath
        _storages = New Dictionary(Of String, ShelfStorage)()
        Initialize()
    End Sub
    
    ''' <summary>
    ''' 初期化（CSVファイルがなければ作成）
    ''' </summary>
    Private Sub Initialize()
        If Not File.Exists(_csvPath) Then
            CreateInitialCsv()
        End If
        LoadStorages()
    End Sub
    
    ''' <summary>
    ''' 初期CSVファイルを作成（棚1~10）
    ''' </summary>
    Private Sub CreateInitialCsv()
        Dim sb As New StringBuilder()
        sb.AppendLine("棚番号,カードNo")
        For i As Integer = 1 To 10
            sb.AppendLine(i.ToString() & ",")
        Next
        
        File.WriteAllText(_csvPath, sb.ToString(), New UTF8Encoding(True))
    End Sub
    
    ''' <summary>
    ''' CSVファイルから棚情報を読み込む
    ''' </summary>
    Private Sub LoadStorages()
        _storages.Clear()
        
        Dim lines As String() = File.ReadAllLines(_csvPath, Encoding.UTF8)
        
        If lines.Length < 2 Then
            Return
        End If
        
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
            
            Dim storage As New ShelfStorage()
            storage.ShelfNo = parts(0).Trim()
            storage.CardNo = If(parts.Length >= 2, parts(1).Trim(), "")
            
            _storages(storage.ShelfNo) = storage
        Next
    End Sub
    
    ''' <summary>
    ''' 棚情報をCSVファイルに保存
    ''' </summary>
    Private Sub SaveStorages()
        Dim sb As New StringBuilder()
        sb.AppendLine("棚番号,カードNo")
        
        ' 棚番号順にソートして保存
        Dim sortedStorages = _storages.Values.OrderBy(Function(s) Integer.Parse(s.ShelfNo)).ToList()
        
        For Each storage As ShelfStorage In sortedStorages
            sb.AppendLine(storage.ShelfNo & "," & storage.CardNo)
        Next
        
        File.WriteAllText(_csvPath, sb.ToString(), New UTF8Encoding(True))
    End Sub
    
    ''' <summary>
    ''' カード番号から棚番号を検索
    ''' </summary>
    Public Function FindShelfByCardNo(cardNo As String) As ShelfStorage
        ' 最新の状態を取得するため再読み込み
        LoadStorages()
        
        For Each storage As ShelfStorage In _storages.Values
            If Not String.IsNullOrEmpty(storage.CardNo) AndAlso storage.CardNo.Equals(cardNo, StringComparison.OrdinalIgnoreCase) Then
                Return storage
            End If
        Next
        Return Nothing
    End Function
    
    ''' <summary>
    ''' 棚にカードを入庫
    ''' </summary>
    Public Sub StoreCard(shelfNo As String, cardNo As String)
        ' 最新の状態を取得するため再読み込み
        LoadStorages()
        
        If Not _storages.ContainsKey(shelfNo) Then
            Throw New Exception($"棚番号{shelfNo}は存在しません")
        End If
        
        ' すでに別のカードが入庫されているかチェック
        If Not String.IsNullOrEmpty(_storages(shelfNo).CardNo) Then
            Throw New Exception($"棚番号{shelfNo}には既にカード{_storages(shelfNo).CardNo}が入庫されています")
        End If
        
        _storages(shelfNo).CardNo = cardNo
        SaveStorages()
    End Sub
    
    ''' <summary>
    ''' 棚からカードを出庫
    ''' </summary>
    Public Sub RemoveCard(shelfNo As String)
        ' 最新の状態を取得するため再読み込み
        LoadStorages()
        
        If Not _storages.ContainsKey(shelfNo) Then
            Throw New Exception($"棚番号{shelfNo}は存在しません")
        End If
        
        _storages(shelfNo).CardNo = ""
        SaveStorages()
    End Sub
    
    ''' <summary>
    ''' 棚が空いているかチェック
    ''' </summary>
    Public Function IsShelfEmpty(shelfNo As String) As Boolean
        If Not _storages.ContainsKey(shelfNo) Then
            Return False
        End If
        
        Return String.IsNullOrEmpty(_storages(shelfNo).CardNo)
    End Function
    
    ''' <summary>
    ''' すべての棚情報を取得
    ''' </summary>
    Public Function GetAllStorages() As List(Of ShelfStorage)
        Return _storages.Values.OrderBy(Function(s) Integer.Parse(s.ShelfNo)).ToList()
    End Function
End Class
