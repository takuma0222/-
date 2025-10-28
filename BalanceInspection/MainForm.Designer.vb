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
        Me.dgvCardInfo = New System.Windows.Forms.DataGridView()
        Me.ColProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColQuantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLocation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblEmployeeNo = New System.Windows.Forms.Label()
        Me.lblCardNo = New System.Windows.Forms.Label()
        CType(Me.dgvConditions, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCardInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtEmployeeNo
        '
        Me.txtEmployeeNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.txtEmployeeNo.Location = New System.Drawing.Point(186, 117)
        Me.txtEmployeeNo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtEmployeeNo.MaxLength = 6
        Me.txtEmployeeNo.Name = "txtEmployeeNo"
        Me.txtEmployeeNo.Size = New System.Drawing.Size(213, 31)
        Me.txtEmployeeNo.TabIndex = 1
        '
        'txtCardNo
        '
        Me.txtCardNo.Enabled = False
        Me.txtCardNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.txtCardNo.Location = New System.Drawing.Point(186, 161)
        Me.txtCardNo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtCardNo.MaxLength = 6
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(213, 31)
        Me.txtCardNo.TabIndex = 3
        '
        'btnVerify
        '
        Me.btnVerify.Enabled = False
        Me.btnVerify.Location = New System.Drawing.Point(1386, 108)
        Me.btnVerify.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(114, 36)
        Me.btnVerify.TabIndex = 4
        Me.btnVerify.Text = "照合"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(1557, 108)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(143, 36)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("MS UI Gothic", 18.0!)
        Me.lblMessage.ForeColor = System.Drawing.Color.Black
        Me.lblMessage.Location = New System.Drawing.Point(29, 58)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(1086, 48)
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
        Me.dgvConditions.Location = New System.Drawing.Point(614, 161)
        Me.dgvConditions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvConditions.MultiSelect = False
        Me.dgvConditions.Name = "dgvConditions"
        Me.dgvConditions.ReadOnly = True
        Me.dgvConditions.RowHeadersVisible = False
        Me.dgvConditions.RowHeadersWidth = 62
        Me.dgvConditions.RowTemplate.Height = 25
        Me.dgvConditions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvConditions.Size = New System.Drawing.Size(1086, 96)
        Me.dgvConditions.TabIndex = 7
        '
        'ColPre10mm
        '
        Me.ColPre10mm.HeaderText = "投入前10mmクッション材"
        Me.ColPre10mm.MinimumWidth = 8
        Me.ColPre10mm.Name = "ColPre10mm"
        Me.ColPre10mm.ReadOnly = True
        '
        'Col1mm
        '
        Me.Col1mm.HeaderText = "投入後1mmクッション材"
        Me.Col1mm.MinimumWidth = 8
        Me.Col1mm.Name = "Col1mm"
        Me.Col1mm.ReadOnly = True
        '
        'Col5mm
        '
        Me.Col5mm.HeaderText = "投入後5mmクッション材"
        Me.Col5mm.MinimumWidth = 8
        Me.Col5mm.Name = "Col5mm"
        Me.Col5mm.ReadOnly = True
        '
        'Col10mm
        '
        Me.Col10mm.HeaderText = "投入後10mmクッション材"
        Me.Col10mm.MinimumWidth = 8
        Me.Col10mm.Name = "Col10mm"
        Me.Col10mm.ReadOnly = True
        '
        'ColEdge
        '
        Me.ColEdge.HeaderText = "エッジガード"
        Me.ColEdge.MinimumWidth = 8
        Me.ColEdge.Name = "ColEdge"
        Me.ColEdge.ReadOnly = True
        '
        'ColBubble
        '
        Me.ColBubble.HeaderText = "気泡緩衝材"
        Me.ColBubble.MinimumWidth = 8
        Me.ColBubble.Name = "ColBubble"
        Me.ColBubble.ReadOnly = True
        '
        'dgvCardInfo
        '
        Me.dgvCardInfo.AllowUserToAddRows = False
        Me.dgvCardInfo.AllowUserToDeleteRows = False
        Me.dgvCardInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCardInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCardInfo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColProductName, Me.ColQuantity, Me.ColLocation})
        Me.dgvCardInfo.Location = New System.Drawing.Point(30, 210)
        Me.dgvCardInfo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvCardInfo.MultiSelect = False
        Me.dgvCardInfo.Name = "dgvCardInfo"
        Me.dgvCardInfo.ReadOnly = True
        Me.dgvCardInfo.RowHeadersVisible = False
        Me.dgvCardInfo.RowHeadersWidth = 62
        Me.dgvCardInfo.RowTemplate.Height = 25
        Me.dgvCardInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCardInfo.Size = New System.Drawing.Size(550, 96)
        Me.dgvCardInfo.TabIndex = 8
        '
        'ColProductName
        '
        Me.ColProductName.HeaderText = "品名"
        Me.ColProductName.MinimumWidth = 8
        Me.ColProductName.Name = "ColProductName"
        Me.ColProductName.ReadOnly = True
        '
        'ColQuantity
        '
        Me.ColQuantity.HeaderText = "枚数"
        Me.ColQuantity.MinimumWidth = 8
        Me.ColQuantity.Name = "ColQuantity"
        Me.ColQuantity.ReadOnly = True
        '
        'ColLocation
        '
        Me.ColLocation.HeaderText = "所在"
        Me.ColLocation.MinimumWidth = 8
        Me.ColLocation.Name = "ColLocation"
        Me.ColLocation.ReadOnly = True
        '
        'lblEmployeeNo
        '
        Me.lblEmployeeNo.AutoSize = True
        Me.lblEmployeeNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblEmployeeNo.Location = New System.Drawing.Point(30, 120)
        Me.lblEmployeeNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEmployeeNo.Name = "lblEmployeeNo"
        Me.lblEmployeeNo.Size = New System.Drawing.Size(114, 24)
        Me.lblEmployeeNo.TabIndex = 0
        Me.lblEmployeeNo.Text = "従業員No:"
        '
        'lblCardNo
        '
        Me.lblCardNo.AutoSize = True
        Me.lblCardNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNo.Location = New System.Drawing.Point(30, 161)
        Me.lblCardNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardNo.Name = "lblCardNo"
        Me.lblCardNo.Size = New System.Drawing.Size(96, 24)
        Me.lblCardNo.TabIndex = 2
        Me.lblCardNo.Text = "カードNo:"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1713, 922)
        Me.Controls.Add(Me.dgvCardInfo)
        Me.Controls.Add(Me.dgvConditions)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.lblCardNo)
        Me.Controls.Add(Me.txtEmployeeNo)
        Me.Controls.Add(Me.lblEmployeeNo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "検品デスクトップアプリ"
        CType(Me.dgvConditions, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCardInfo, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgvCardInfo As DataGridView
    Friend WithEvents ColProductName As DataGridViewTextBoxColumn
    Friend WithEvents ColQuantity As DataGridViewTextBoxColumn
    Friend WithEvents ColLocation As DataGridViewTextBoxColumn
    Friend WithEvents lblEmployeeNo As Label
    Friend WithEvents lblCardNo As Label
End Class

