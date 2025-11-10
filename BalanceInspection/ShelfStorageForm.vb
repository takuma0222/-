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
        
        ' イベントハンドラーを登録
        AddHandler btnExecute.Click, AddressOf BtnExecute_Click
        AddHandler btnCancel.Click, AddressOf BtnCancel_Click
        
        ' フォーカス設定
        txtShelfNo.Focus()
    End Sub
    
    Private Sub InitializeComponent()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblShelfNo = New System.Windows.Forms.Label()
        Me.txtShelfNo = New System.Windows.Forms.TextBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMessage.Font = New System.Drawing.Font("MS UI Gothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblMessage.Location = New System.Drawing.Point(20, 30)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(460, 50)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "入庫する棚番号を入れてください"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblShelfNo
        '
        Me.lblShelfNo.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.lblShelfNo.Location = New System.Drawing.Point(60, 120)
        Me.lblShelfNo.Name = "lblShelfNo"
        Me.lblShelfNo.Size = New System.Drawing.Size(100, 35)
        Me.lblShelfNo.TabIndex = 1
        Me.lblShelfNo.Text = "棚番号:"
        Me.lblShelfNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtShelfNo
        '
        Me.txtShelfNo.Font = New System.Drawing.Font("MS UI Gothic", 14.0!)
        Me.txtShelfNo.Location = New System.Drawing.Point(170, 120)
        Me.txtShelfNo.Name = "txtShelfNo"
        Me.txtShelfNo.Size = New System.Drawing.Size(250, 45)
        Me.txtShelfNo.TabIndex = 2
        '
        'btnExecute
        '
        Me.btnExecute.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.btnExecute.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExecute.Font = New System.Drawing.Font("MS UI Gothic", 13.0!, System.Drawing.FontStyle.Bold)
        Me.btnExecute.ForeColor = System.Drawing.Color.White
        Me.btnExecute.Location = New System.Drawing.Point(100, 190)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(130, 50)
        Me.btnExecute.TabIndex = 3
        Me.btnExecute.Text = "実行"
        Me.btnExecute.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.btnCancel.Location = New System.Drawing.Point(270, 190)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(130, 50)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'ShelfStorageForm
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(662, 387)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblShelfNo)
        Me.Controls.Add(Me.txtShelfNo)
        Me.Controls.Add(Me.btnExecute)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShelfStorageForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "棚入庫"
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
