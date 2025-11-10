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
        Me.Size = New Drawing.Size(500, 300)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Drawing.Color.FromArgb(245, 245, 245)
        
        ' 棚情報ラベル
        lblShelfInfo = New Label()
        lblShelfInfo.Text = $"棚番号: {_shelfStorage.ShelfNo}" & vbCrLf & $"カードNo: {_shelfStorage.CardNo}"
        lblShelfInfo.Font = New Drawing.Font("MS UI Gothic", 14.0F, Drawing.FontStyle.Bold)
        lblShelfInfo.Location = New Drawing.Point(20, 30)
        lblShelfInfo.Size = New Drawing.Size(460, 70)
        lblShelfInfo.TextAlign = Drawing.ContentAlignment.MiddleCenter
        lblShelfInfo.BackColor = Drawing.Color.FromArgb(240, 248, 255)
        lblShelfInfo.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(lblShelfInfo)
        
        ' カードNoラベル
        lblCardNo = New Label()
        lblCardNo.Text = "カードNo:"
        lblCardNo.Font = New Drawing.Font("MS UI Gothic", 13.0F, Drawing.FontStyle.Regular)
        lblCardNo.Location = New Drawing.Point(50, 130)
        lblCardNo.Size = New Drawing.Size(110, 35)
        lblCardNo.TextAlign = Drawing.ContentAlignment.MiddleRight
        Me.Controls.Add(lblCardNo)
        
        ' カードNo入力欄
        txtCardNo = New TextBox()
        txtCardNo.Font = New Drawing.Font("MS UI Gothic", 14.0F)
        txtCardNo.Location = New Drawing.Point(170, 130)
        txtCardNo.Size = New Drawing.Size(250, 35)
        txtCardNo.Text = _shelfStorage.CardNo
        txtCardNo.ReadOnly = True
        txtCardNo.BackColor = Drawing.Color.FromArgb(240, 240, 240)
        Me.Controls.Add(txtCardNo)
        
        ' 実行ボタン
        btnExecute = New Button()
        btnExecute.Text = "実行"
        btnExecute.Font = New Drawing.Font("MS UI Gothic", 13.0F, Drawing.FontStyle.Bold)
        btnExecute.Location = New Drawing.Point(100, 190)
        btnExecute.Size = New Drawing.Size(130, 50)
        btnExecute.BackColor = Drawing.Color.FromArgb(0, 120, 215)
        btnExecute.ForeColor = Drawing.Color.White
        btnExecute.FlatStyle = FlatStyle.Flat
        btnExecute.Cursor = Cursors.Hand
        AddHandler btnExecute.Click, AddressOf BtnExecute_Click
        Me.Controls.Add(btnExecute)
        
        ' キャンセルボタン
        btnCancel = New Button()
        btnCancel.Text = "キャンセル"
        btnCancel.Font = New Drawing.Font("MS UI Gothic", 13.0F, Drawing.FontStyle.Regular)
        btnCancel.Location = New Drawing.Point(270, 190)
        btnCancel.Size = New Drawing.Size(130, 50)
        btnCancel.BackColor = Drawing.Color.FromArgb(240, 240, 240)
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.Cursor = Cursors.Hand
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
