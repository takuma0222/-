Imports System.Windows.Forms

''' <summary>
''' 棚出庫画面
''' </summary>
Public Class ShelfRetrievalForm
    Inherits Form
    
    Private lblShelfInfo As Label
    Private lblCardNo As Label
    Private txtCardNo As TextBox
    Private btnExecute As Button
    Private btnCancel As Button
    
    Private _shelfStorage As ShelfStorage
    Private _shelfManager As ShelfStorageManager
    
    Public Property IsExecuted As Boolean = False
    
    Public Sub New(shelfStorage As ShelfStorage, shelfManager As ShelfStorageManager)
        _shelfStorage = shelfStorage
        _shelfManager = shelfManager
        
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        Me.Text = "棚出庫"
        Me.Size = New Drawing.Size(400, 250)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        
        ' 棚情報ラベル
        lblShelfInfo = New Label()
        lblShelfInfo.Text = $"棚番号: {_shelfStorage.ShelfNo}" & vbCrLf & $"カードNo: {_shelfStorage.CardNo}"
        lblShelfInfo.Font = New Drawing.Font("MS UI Gothic", 12.0F, Drawing.FontStyle.Bold)
        lblShelfInfo.Location = New Drawing.Point(20, 20)
        lblShelfInfo.Size = New Drawing.Size(360, 50)
        lblShelfInfo.TextAlign = Drawing.ContentAlignment.MiddleCenter
        Me.Controls.Add(lblShelfInfo)
        
        ' カードNoラベル
        lblCardNo = New Label()
        lblCardNo.Text = "カードNo:"
        lblCardNo.Font = New Drawing.Font("MS UI Gothic", 10.0F)
        lblCardNo.Location = New Drawing.Point(50, 90)
        lblCardNo.Size = New Drawing.Size(80, 25)
        lblCardNo.TextAlign = Drawing.ContentAlignment.MiddleRight
        Me.Controls.Add(lblCardNo)
        
        ' カードNo入力欄
        txtCardNo = New TextBox()
        txtCardNo.Font = New Drawing.Font("MS UI Gothic", 12.0F)
        txtCardNo.Location = New Drawing.Point(140, 90)
        txtCardNo.Size = New Drawing.Size(200, 30)
        txtCardNo.Text = _shelfStorage.CardNo
        txtCardNo.ReadOnly = True
        Me.Controls.Add(txtCardNo)
        
        ' 実行ボタン
        btnExecute = New Button()
        btnExecute.Text = "実行"
        btnExecute.Font = New Drawing.Font("MS UI Gothic", 11.0F, Drawing.FontStyle.Bold)
        btnExecute.Location = New Drawing.Point(80, 140)
        btnExecute.Size = New Drawing.Size(100, 40)
        AddHandler btnExecute.Click, AddressOf BtnExecute_Click
        Me.Controls.Add(btnExecute)
        
        ' キャンセルボタン
        btnCancel = New Button()
        btnCancel.Text = "キャンセル"
        btnCancel.Font = New Drawing.Font("MS UI Gothic", 11.0F)
        btnCancel.Location = New Drawing.Point(220, 140)
        btnCancel.Size = New Drawing.Size(100, 40)
        AddHandler btnCancel.Click, AddressOf BtnCancel_Click
        Me.Controls.Add(btnCancel)
    End Sub
    
    Private Sub BtnExecute_Click(sender As Object, e As EventArgs)
        Try
            ' 棚からカードを出庫
            _shelfManager.RemoveCard(_shelfStorage.ShelfNo)
            
            IsExecuted = True
            Me.DialogResult = DialogResult.OK
            Me.Close()
            
        Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs)
        IsExecuted = False
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
