Imports System.Windows.Forms

''' <summary>
''' 棚入庫画面
''' </summary>
Public Class ShelfStorageForm
    Inherits Form
    
    Private lblMessage As Label
    Private lblShelfNo As Label
    Private txtShelfNo As TextBox
    Private btnExecute As Button
    Private btnCancel As Button
    
    Private _cardNo As String
    Private _shelfManager As ShelfStorageManager
    
    Public Property IsExecuted As Boolean = False
    Public Property SelectedShelfNo As String = ""
    
    Public Sub New(cardNo As String, shelfManager As ShelfStorageManager)
        _cardNo = cardNo
        _shelfManager = shelfManager
        
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        Me.Text = "棚入庫"
        Me.Size = New Drawing.Size(500, 300)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Drawing.Color.FromArgb(245, 245, 245)
        
        ' メッセージラベル
        lblMessage = New Label()
        lblMessage.Text = "入庫する棚番号を入れてください"
        lblMessage.Font = New Drawing.Font("MS UI Gothic", 14.0F, Drawing.FontStyle.Bold)
        lblMessage.Location = New Drawing.Point(20, 30)
        lblMessage.Size = New Drawing.Size(460, 50)
        lblMessage.TextAlign = Drawing.ContentAlignment.MiddleCenter
        lblMessage.BackColor = Drawing.Color.FromArgb(240, 248, 255)
        lblMessage.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(lblMessage)
        
        ' 棚番号ラベル
        lblShelfNo = New Label()
        lblShelfNo.Text = "棚番号:"
        lblShelfNo.Font = New Drawing.Font("MS UI Gothic", 13.0F, Drawing.FontStyle.Regular)
        lblShelfNo.Location = New Drawing.Point(60, 120)
        lblShelfNo.Size = New Drawing.Size(100, 35)
        lblShelfNo.TextAlign = Drawing.ContentAlignment.MiddleRight
        Me.Controls.Add(lblShelfNo)
        
        ' 棚番号入力欄
        txtShelfNo = New TextBox()
        txtShelfNo.Font = New Drawing.Font("MS UI Gothic", 14.0F)
        txtShelfNo.Location = New Drawing.Point(170, 120)
        txtShelfNo.Size = New Drawing.Size(250, 35)
        Me.Controls.Add(txtShelfNo)
        
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
        
        ' フォーカス設定
        txtShelfNo.Focus()
    End Sub
    
    Private Sub BtnExecute_Click(sender As Object, e As EventArgs)
        Dim shelfNo As String = txtShelfNo.Text.Trim()
        
        If String.IsNullOrEmpty(shelfNo) Then
            MessageBox.Show("棚番号を入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        
        ' 棚番号が1~10の範囲かチェック
        Dim shelfNumber As Integer
        If Not Integer.TryParse(shelfNo, shelfNumber) OrElse shelfNumber < 1 OrElse shelfNumber > 10 Then
            MessageBox.Show("棚番号は1~10の範囲で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        
        Try
            ' 棚にカードを入庫
            _shelfManager.StoreCard(shelfNo, _cardNo)
            
            SelectedShelfNo = shelfNo
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
