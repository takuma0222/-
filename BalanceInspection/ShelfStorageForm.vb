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
        Me.Size = New Drawing.Size(400, 250)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        
        ' メッセージラベル
        lblMessage = New Label()
        lblMessage.Text = "入庫する棚番号を入れてください"
        lblMessage.Font = New Drawing.Font("MS UI Gothic", 12.0F, Drawing.FontStyle.Bold)
        lblMessage.Location = New Drawing.Point(20, 20)
        lblMessage.Size = New Drawing.Size(360, 30)
        lblMessage.TextAlign = Drawing.ContentAlignment.MiddleCenter
        Me.Controls.Add(lblMessage)
        
        ' 棚番号ラベル
        lblShelfNo = New Label()
        lblShelfNo.Text = "棚番号:"
        lblShelfNo.Font = New Drawing.Font("MS UI Gothic", 10.0F)
        lblShelfNo.Location = New Drawing.Point(50, 70)
        lblShelfNo.Size = New Drawing.Size(80, 25)
        lblShelfNo.TextAlign = Drawing.ContentAlignment.MiddleRight
        Me.Controls.Add(lblShelfNo)
        
        ' 棚番号入力欄
        txtShelfNo = New TextBox()
        txtShelfNo.Font = New Drawing.Font("MS UI Gothic", 12.0F)
        txtShelfNo.Location = New Drawing.Point(140, 70)
        txtShelfNo.Size = New Drawing.Size(200, 30)
        Me.Controls.Add(txtShelfNo)
        
        ' 実行ボタン
        btnExecute = New Button()
        btnExecute.Text = "実行"
        btnExecute.Font = New Drawing.Font("MS UI Gothic", 11.0F, Drawing.FontStyle.Bold)
        btnExecute.Location = New Drawing.Point(80, 130)
        btnExecute.Size = New Drawing.Size(100, 40)
        AddHandler btnExecute.Click, AddressOf BtnExecute_Click
        Me.Controls.Add(btnExecute)
        
        ' キャンセルボタン
        btnCancel = New Button()
        btnCancel.Text = "キャンセル"
        btnCancel.Font = New Drawing.Font("MS UI Gothic", 11.0F)
        btnCancel.Location = New Drawing.Point(220, 130)
        btnCancel.Size = New Drawing.Size(100, 40)
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
