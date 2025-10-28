<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtEmployeeNo = New System.Windows.Forms.TextBox()
        Me.txtCardNo = New System.Windows.Forms.TextBox()
        Me.btnVerify = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.dgvConditions = New System.Windows.Forms.DataGridView()
        Me.ColPre10mm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col1mm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col5mm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Col10mm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColEdge = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColBubble = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Dim lblEmployeeNo As System.Windows.Forms.Label = New System.Windows.Forms.Label()
        Dim lblCardNo As System.Windows.Forms.Label = New System.Windows.Forms.Label()
        CType(Me.dgvConditions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblEmployeeNo
        '
        lblEmployeeNo.AutoSize = True
        lblEmployeeNo.Location = New System.Drawing.Point(20, 20)
        lblEmployeeNo.Name = "lblEmployeeNo"
        lblEmployeeNo.Size = New System.Drawing.Size(100, 15)
        lblEmployeeNo.TabIndex = 0
        lblEmployeeNo.Text = "従業員No:"
        '
        'txtEmployeeNo
        '
        Me.txtEmployeeNo.Location = New System.Drawing.Point(130, 20)
        Me.txtEmployeeNo.MaxLength = 6
        Me.txtEmployeeNo.Name = "txtEmployeeNo"
        Me.txtEmployeeNo.Size = New System.Drawing.Size(150, 23)
        Me.txtEmployeeNo.TabIndex = 1
        '
        'lblCardNo
        '
        lblCardNo.AutoSize = True
        lblCardNo.Location = New System.Drawing.Point(300, 20)
        lblCardNo.Name = "lblCardNo"
        lblCardNo.Size = New System.Drawing.Size(100, 15)
        lblCardNo.TabIndex = 2
        lblCardNo.Text = "カードNo:"
        '
        'txtCardNo
        '
        Me.txtCardNo.Enabled = False
        Me.txtCardNo.Location = New System.Drawing.Point(410, 20)
        Me.txtCardNo.MaxLength = 6
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(150, 23)
        Me.txtCardNo.TabIndex = 3
        '
        'btnVerify
        '
        Me.btnVerify.Enabled = False
        Me.btnVerify.Location = New System.Drawing.Point(580, 15)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(80, 30)
        Me.btnVerify.TabIndex = 4
        Me.btnVerify.Text = "照合"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(680, 15)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblMessage.ForeColor = System.Drawing.Color.Black
        Me.lblMessage.Location = New System.Drawing.Point(20, 60)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(760, 40)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "従業員Noを入力してください"
        '
        'dgvConditions
        '
        Me.dgvConditions.AllowUserToAddRows = False
        Me.dgvConditions.AllowUserToDeleteRows = False
        Me.dgvConditions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvConditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConditions.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColPre10mm, Me.Col1mm, Me.Col5mm, Me.Col10mm, Me.ColEdge, Me.ColBubble})
        Me.dgvConditions.Location = New System.Drawing.Point(20, 110)
        Me.dgvConditions.MultiSelect = False
        Me.dgvConditions.Name = "dgvConditions"
        Me.dgvConditions.ReadOnly = True
        Me.dgvConditions.RowHeadersVisible = False
        Me.dgvConditions.RowTemplate.Height = 25
        Me.dgvConditions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvConditions.Size = New System.Drawing.Size(760, 80)
        Me.dgvConditions.TabIndex = 7
        '
        'ColPre10mm
        '
        Me.ColPre10mm.HeaderText = "投入前10mmクッション材"
        Me.ColPre10mm.Name = "ColPre10mm"
        Me.ColPre10mm.ReadOnly = True
        '
        'Col1mm
        '
        Me.Col1mm.HeaderText = "投入後1mmクッション材"
        Me.Col1mm.Name = "Col1mm"
        Me.Col1mm.ReadOnly = True
        '
        'Col5mm
        '
        Me.Col5mm.HeaderText = "投入後5mmクッション材"
        Me.Col5mm.Name = "Col5mm"
        Me.Col5mm.ReadOnly = True
        '
        'Col10mm
        '
        Me.Col10mm.HeaderText = "投入後10mmクッション材"
        Me.Col10mm.Name = "Col10mm"
        Me.Col10mm.ReadOnly = True
        '
        'ColEdge
        '
        Me.ColEdge.HeaderText = "エッジガード"
        Me.ColEdge.Name = "ColEdge"
        Me.ColEdge.ReadOnly = True
        '
        'ColBubble
        '
        Me.ColBubble.HeaderText = "気泡緩衝材"
        Me.ColBubble.Name = "ColBubble"
        Me.ColBubble.ReadOnly = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 500)
        Me.Controls.Add(Me.dgvConditions)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(lblCardNo)
        Me.Controls.Add(Me.txtEmployeeNo)
        Me.Controls.Add(lblEmployeeNo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "検品デスクトップアプリ"
        CType(Me.dgvConditions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeNo As TextBox
    Friend WithEvents txtCardNo As TextBox
    Friend WithEvents btnVerify As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents dgvConditions As DataGridView
    Friend WithEvents ColPre10mm As DataGridViewTextBoxColumn
    Friend WithEvents Col1mm As DataGridViewTextBoxColumn
    Friend WithEvents Col5mm As DataGridViewTextBoxColumn
    Friend WithEvents Col10mm As DataGridViewTextBoxColumn
    Friend WithEvents ColEdge As DataGridViewTextBoxColumn
    Friend WithEvents ColBubble As DataGridViewTextBoxColumn

End Class

