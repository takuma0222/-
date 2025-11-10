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
        
        ' イベントハンドラーを登録
        AddHandler btnExecute.Click, AddressOf BtnExecute_Click
        AddHandler btnCancel.Click, AddressOf BtnCancel_Click
        
        ' コンストラクタ後に動的な値を設定
        UpdateShelfInfo()
    End Sub
    
    Private Sub InitializeComponent()
        Me.lblShelfInfo = New System.Windows.Forms.Label()
        Me.lblCardNo = New System.Windows.Forms.Label()
        Me.txtCardNo = New System.Windows.Forms.TextBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblShelfInfo
        '
        Me.lblShelfInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblShelfInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblShelfInfo.Font = New System.Drawing.Font("MS UI Gothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblShelfInfo.Location = New System.Drawing.Point(20, 30)
        Me.lblShelfInfo.Name = "lblShelfInfo"
        Me.lblShelfInfo.Size = New System.Drawing.Size(460, 70)
        Me.lblShelfInfo.TabIndex = 0
        Me.lblShelfInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCardNo
        '
        Me.lblCardNo.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.lblCardNo.Location = New System.Drawing.Point(50, 130)
        Me.lblCardNo.Name = "lblCardNo"
        Me.lblCardNo.Size = New System.Drawing.Size(110, 35)
        Me.lblCardNo.TabIndex = 1
        Me.lblCardNo.Text = "カードNo:"
        Me.lblCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCardNo
        '
        Me.txtCardNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtCardNo.Font = New System.Drawing.Font("MS UI Gothic", 14.0!)
        Me.txtCardNo.Location = New System.Drawing.Point(170, 130)
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.ReadOnly = True
        Me.txtCardNo.Size = New System.Drawing.Size(250, 45)
        Me.txtCardNo.TabIndex = 2
        '
        'btnExecute
        '
        Me.btnExecute.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.btnExecute.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExecute.Font = New System.Drawing.Font("MS UI Gothic", 13.0!, System.Drawing.FontStyle.Bold)
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
        'ShelfRetrievalForm
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(792, 379)
        Me.Controls.Add(Me.lblShelfInfo)
        Me.Controls.Add(Me.lblCardNo)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.btnExecute)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShelfRetrievalForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "棚出庫"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    ''' <summary>
    ''' 棚情報を更新（動的な値を設定）
    ''' </summary>
    Private Sub UpdateShelfInfo()
        lblShelfInfo.Text = $"棚番号: {_shelfStorage.ShelfNo}" & vbCrLf & $"カードNo: {_shelfStorage.CardNo}"
        txtCardNo.Text = _shelfStorage.CardNo
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
