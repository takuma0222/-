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
        Me.pnlConditions = New System.Windows.Forms.Panel()
        Me.lblConditionsTitle = New System.Windows.Forms.Label()
        Me.lblPre10mmLabel = New System.Windows.Forms.Label()
        Me.lblPre10mmValue = New System.Windows.Forms.Label()
        Me.lblPost1mmLabel = New System.Windows.Forms.Label()
        Me.lblPost1mmValue = New System.Windows.Forms.Label()
        Me.lblPost5mmLabel = New System.Windows.Forms.Label()
        Me.lblPost5mmValue = New System.Windows.Forms.Label()
        Me.lblPost10mmLabel = New System.Windows.Forms.Label()
        Me.lblPost10mmValue = New System.Windows.Forms.Label()
        Me.lblEdgeLabel = New System.Windows.Forms.Label()
        Me.lblEdgeValue = New System.Windows.Forms.Label()
        Me.lblBubbleLabel = New System.Windows.Forms.Label()
        Me.lblBubbleValue = New System.Windows.Forms.Label()
        Me.pnlCardInfo = New System.Windows.Forms.Panel()
        Me.lblLocationValue = New System.Windows.Forms.Label()
        Me.lblLocationLabel = New System.Windows.Forms.Label()
        Me.lblQuantityValue = New System.Windows.Forms.Label()
        Me.lblQuantityLabel = New System.Windows.Forms.Label()
        Me.lblProductNameValue = New System.Windows.Forms.Label()
        Me.lblProductNameLabel = New System.Windows.Forms.Label()
        Me.lblCardNoDisplayValue = New System.Windows.Forms.Label()
        Me.lblCardNoDisplayLabel = New System.Windows.Forms.Label()
        Me.lblCardInfoTitle = New System.Windows.Forms.Label()
        Me.lblEmployeeNo = New System.Windows.Forms.Label()
        Me.lblCardNo = New System.Windows.Forms.Label()
        Me.pnlConditions.SuspendLayout()
        Me.pnlCardInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtEmployeeNo
        '
        Me.txtEmployeeNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.txtEmployeeNo.Location = New System.Drawing.Point(186, 117)
        Me.txtEmployeeNo.Margin = New System.Windows.Forms.Padding(4)
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
        Me.txtCardNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCardNo.MaxLength = 6
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(213, 31)
        Me.txtCardNo.TabIndex = 3
        '
        'btnVerify
        '
        Me.btnVerify.Enabled = False
        Me.btnVerify.Location = New System.Drawing.Point(1386, 108)
        Me.btnVerify.Margin = New System.Windows.Forms.Padding(4)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(114, 36)
        Me.btnVerify.TabIndex = 4
        Me.btnVerify.Text = "照合"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(1557, 108)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
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
        Me.lblMessage.Size = New System.Drawing.Size(1322, 48)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "従業員Noを入力してください"
        '
        'pnlConditions
        '
        Me.pnlConditions.BackColor = System.Drawing.Color.White
        Me.pnlConditions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlConditions.Controls.Add(Me.lblBubbleValue)
        Me.pnlConditions.Controls.Add(Me.lblBubbleLabel)
        Me.pnlConditions.Controls.Add(Me.lblEdgeValue)
        Me.pnlConditions.Controls.Add(Me.lblEdgeLabel)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmValue)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmLabel)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmValue)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmLabel)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmValue)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmLabel)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmValue)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmLabel)
        Me.pnlConditions.Controls.Add(Me.lblConditionsTitle)
        Me.pnlConditions.Location = New System.Drawing.Point(30, 430)
        Me.pnlConditions.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlConditions.Name = "pnlConditions"
        Me.pnlConditions.Size = New System.Drawing.Size(800, 278)
        Me.pnlConditions.TabIndex = 7
        '
        'lblConditionsTitle
        '
        Me.lblConditionsTitle.BackColor = System.Drawing.Color.DarkGray
        Me.lblConditionsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblConditionsTitle.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblConditionsTitle.ForeColor = System.Drawing.Color.White
        Me.lblConditionsTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblConditionsTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblConditionsTitle.Name = "lblConditionsTitle"
        Me.lblConditionsTitle.Size = New System.Drawing.Size(799, 39)
        Me.lblConditionsTitle.TabIndex = 0
        Me.lblConditionsTitle.Text = "使用部材条件"
        Me.lblConditionsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmLabel
        '
        Me.lblPre10mmLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblPre10mmLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPre10mmLabel.Location = New System.Drawing.Point(0, 39)
        Me.lblPre10mmLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmLabel.Name = "lblPre10mmLabel"
        Me.lblPre10mmLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblPre10mmLabel.TabIndex = 1
        Me.lblPre10mmLabel.Text = "投入前10mmクッション材"
        Me.lblPre10mmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmValue
        '
        Me.lblPre10mmValue.BackColor = System.Drawing.Color.White
        Me.lblPre10mmValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPre10mmValue.Location = New System.Drawing.Point(249, 39)
        Me.lblPre10mmValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmValue.Name = "lblPre10mmValue"
        Me.lblPre10mmValue.Size = New System.Drawing.Size(550, 39)
        Me.lblPre10mmValue.TabIndex = 2
        Me.lblPre10mmValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPre10mmValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'lblPost1mmLabel
        '
        Me.lblPost1mmLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblPost1mmLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost1mmLabel.Location = New System.Drawing.Point(0, 78)
        Me.lblPost1mmLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmLabel.Name = "lblPost1mmLabel"
        Me.lblPost1mmLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblPost1mmLabel.TabIndex = 3
        Me.lblPost1mmLabel.Text = "投入後1mmクッション材"
        Me.lblPost1mmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmValue
        '
        Me.lblPost1mmValue.BackColor = System.Drawing.Color.White
        Me.lblPost1mmValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost1mmValue.Location = New System.Drawing.Point(249, 78)
        Me.lblPost1mmValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmValue.Name = "lblPost1mmValue"
        Me.lblPost1mmValue.Size = New System.Drawing.Size(550, 39)
        Me.lblPost1mmValue.TabIndex = 4
        Me.lblPost1mmValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPost1mmValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'lblPost5mmLabel
        '
        Me.lblPost5mmLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblPost5mmLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost5mmLabel.Location = New System.Drawing.Point(0, 117)
        Me.lblPost5mmLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmLabel.Name = "lblPost5mmLabel"
        Me.lblPost5mmLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblPost5mmLabel.TabIndex = 5
        Me.lblPost5mmLabel.Text = "投入後5mmクッション材"
        Me.lblPost5mmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmValue
        '
        Me.lblPost5mmValue.BackColor = System.Drawing.Color.White
        Me.lblPost5mmValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost5mmValue.Location = New System.Drawing.Point(249, 117)
        Me.lblPost5mmValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmValue.Name = "lblPost5mmValue"
        Me.lblPost5mmValue.Size = New System.Drawing.Size(550, 39)
        Me.lblPost5mmValue.TabIndex = 6
        Me.lblPost5mmValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPost5mmValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'lblPost10mmLabel
        '
        Me.lblPost10mmLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblPost10mmLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost10mmLabel.Location = New System.Drawing.Point(0, 156)
        Me.lblPost10mmLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmLabel.Name = "lblPost10mmLabel"
        Me.lblPost10mmLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblPost10mmLabel.TabIndex = 7
        Me.lblPost10mmLabel.Text = "投入後10mmクッション材"
        Me.lblPost10mmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmValue
        '
        Me.lblPost10mmValue.BackColor = System.Drawing.Color.White
        Me.lblPost10mmValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblPost10mmValue.Location = New System.Drawing.Point(249, 156)
        Me.lblPost10mmValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmValue.Name = "lblPost10mmValue"
        Me.lblPost10mmValue.Size = New System.Drawing.Size(550, 39)
        Me.lblPost10mmValue.TabIndex = 8
        Me.lblPost10mmValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPost10mmValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'lblEdgeLabel
        '
        Me.lblEdgeLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblEdgeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblEdgeLabel.Location = New System.Drawing.Point(0, 195)
        Me.lblEdgeLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeLabel.Name = "lblEdgeLabel"
        Me.lblEdgeLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblEdgeLabel.TabIndex = 9
        Me.lblEdgeLabel.Text = "エッジガード"
        Me.lblEdgeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeValue
        '
        Me.lblEdgeValue.BackColor = System.Drawing.Color.White
        Me.lblEdgeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblEdgeValue.Location = New System.Drawing.Point(249, 195)
        Me.lblEdgeValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeValue.Name = "lblEdgeValue"
        Me.lblEdgeValue.Size = New System.Drawing.Size(550, 39)
        Me.lblEdgeValue.TabIndex = 10
        Me.lblEdgeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblEdgeValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'lblBubbleLabel
        '
        Me.lblBubbleLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblBubbleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblBubbleLabel.Location = New System.Drawing.Point(0, 234)
        Me.lblBubbleLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleLabel.Name = "lblBubbleLabel"
        Me.lblBubbleLabel.Size = New System.Drawing.Size(250, 39)
        Me.lblBubbleLabel.TabIndex = 11
        Me.lblBubbleLabel.Text = "気泡緩衝材"
        Me.lblBubbleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleValue
        '
        Me.lblBubbleValue.BackColor = System.Drawing.Color.White
        Me.lblBubbleValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblBubbleValue.Location = New System.Drawing.Point(249, 234)
        Me.lblBubbleValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleValue.Name = "lblBubbleValue"
        Me.lblBubbleValue.Size = New System.Drawing.Size(550, 39)
        Me.lblBubbleValue.TabIndex = 12
        Me.lblBubbleValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblBubbleValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        '
        'pnlCardInfo
        '
        Me.pnlCardInfo.BackColor = System.Drawing.Color.White
        Me.pnlCardInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCardInfo.Controls.Add(Me.lblLocationValue)
        Me.pnlCardInfo.Controls.Add(Me.lblLocationLabel)
        Me.pnlCardInfo.Controls.Add(Me.lblQuantityValue)
        Me.pnlCardInfo.Controls.Add(Me.lblQuantityLabel)
        Me.pnlCardInfo.Controls.Add(Me.lblProductNameValue)
        Me.pnlCardInfo.Controls.Add(Me.lblProductNameLabel)
        Me.pnlCardInfo.Controls.Add(Me.lblCardNoDisplayValue)
        Me.pnlCardInfo.Controls.Add(Me.lblCardNoDisplayLabel)
        Me.pnlCardInfo.Controls.Add(Me.lblCardInfoTitle)
        Me.pnlCardInfo.Location = New System.Drawing.Point(30, 210)
        Me.pnlCardInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCardInfo.Name = "pnlCardInfo"
        Me.pnlCardInfo.Size = New System.Drawing.Size(500, 199)
        Me.pnlCardInfo.TabIndex = 8
        '
        'lblLocationValue
        '
        Me.lblLocationValue.BackColor = System.Drawing.Color.White
        Me.lblLocationValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocationValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblLocationValue.Location = New System.Drawing.Point(149, 156)
        Me.lblLocationValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblLocationValue.Name = "lblLocationValue"
        Me.lblLocationValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.lblLocationValue.Size = New System.Drawing.Size(350, 39)
        Me.lblLocationValue.TabIndex = 5
        Me.lblLocationValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLocationLabel
        '
        Me.lblLocationLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblLocationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocationLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblLocationLabel.Location = New System.Drawing.Point(0, 156)
        Me.lblLocationLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblLocationLabel.Name = "lblLocationLabel"
        Me.lblLocationLabel.Size = New System.Drawing.Size(150, 39)
        Me.lblLocationLabel.TabIndex = 4
        Me.lblLocationLabel.Text = "所在"
        Me.lblLocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblQuantityValue
        '
        Me.lblQuantityValue.BackColor = System.Drawing.Color.White
        Me.lblQuantityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQuantityValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblQuantityValue.Location = New System.Drawing.Point(149, 117)
        Me.lblQuantityValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuantityValue.Name = "lblQuantityValue"
        Me.lblQuantityValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.lblQuantityValue.Size = New System.Drawing.Size(350, 39)
        Me.lblQuantityValue.TabIndex = 3
        Me.lblQuantityValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblQuantityLabel
        '
        Me.lblQuantityLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblQuantityLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQuantityLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblQuantityLabel.Location = New System.Drawing.Point(0, 117)
        Me.lblQuantityLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuantityLabel.Name = "lblQuantityLabel"
        Me.lblQuantityLabel.Size = New System.Drawing.Size(150, 39)
        Me.lblQuantityLabel.TabIndex = 2
        Me.lblQuantityLabel.Text = "枚数"
        Me.lblQuantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblProductNameValue
        '
        Me.lblProductNameValue.BackColor = System.Drawing.Color.White
        Me.lblProductNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblProductNameValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblProductNameValue.Location = New System.Drawing.Point(149, 78)
        Me.lblProductNameValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProductNameValue.Name = "lblProductNameValue"
        Me.lblProductNameValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.lblProductNameValue.Size = New System.Drawing.Size(350, 39)
        Me.lblProductNameValue.TabIndex = 1
        Me.lblProductNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProductNameLabel
        '
        Me.lblProductNameLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblProductNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblProductNameLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblProductNameLabel.Location = New System.Drawing.Point(0, 78)
        Me.lblProductNameLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProductNameLabel.Name = "lblProductNameLabel"
        Me.lblProductNameLabel.Size = New System.Drawing.Size(150, 39)
        Me.lblProductNameLabel.TabIndex = 0
        Me.lblProductNameLabel.Text = "品名"
        Me.lblProductNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCardNoDisplayValue
        '
        Me.lblCardNoDisplayValue.BackColor = System.Drawing.Color.White
        Me.lblCardNoDisplayValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCardNoDisplayValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNoDisplayValue.Location = New System.Drawing.Point(149, 39)
        Me.lblCardNoDisplayValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCardNoDisplayValue.Name = "lblCardNoDisplayValue"
        Me.lblCardNoDisplayValue.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.lblCardNoDisplayValue.Size = New System.Drawing.Size(350, 39)
        Me.lblCardNoDisplayValue.TabIndex = 8
        Me.lblCardNoDisplayValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCardNoDisplayLabel
        '
        Me.lblCardNoDisplayLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblCardNoDisplayLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCardNoDisplayLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNoDisplayLabel.Location = New System.Drawing.Point(0, 39)
        Me.lblCardNoDisplayLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCardNoDisplayLabel.Name = "lblCardNoDisplayLabel"
        Me.lblCardNoDisplayLabel.Size = New System.Drawing.Size(150, 39)
        Me.lblCardNoDisplayLabel.TabIndex = 7
        Me.lblCardNoDisplayLabel.Text = "カードNo"
        Me.lblCardNoDisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCardInfoTitle
        '
        Me.lblCardInfoTitle.BackColor = System.Drawing.Color.DarkGray
        Me.lblCardInfoTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCardInfoTitle.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblCardInfoTitle.ForeColor = System.Drawing.Color.White
        Me.lblCardInfoTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblCardInfoTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCardInfoTitle.Name = "lblCardInfoTitle"
        Me.lblCardInfoTitle.Size = New System.Drawing.Size(499, 39)
        Me.lblCardInfoTitle.TabIndex = 6
        Me.lblCardInfoTitle.Text = "カード情報"
        Me.lblCardInfoTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.Controls.Add(Me.pnlConditions)
        Me.Controls.Add(Me.pnlCardInfo)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.lblCardNo)
        Me.Controls.Add(Me.txtEmployeeNo)
        Me.Controls.Add(Me.lblEmployeeNo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "検品デスクトップアプリ"
        Me.pnlConditions.ResumeLayout(False)
        Me.pnlCardInfo.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeNo As TextBox
    Friend WithEvents txtCardNo As TextBox
    Friend WithEvents btnVerify As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents pnlConditions As Panel
    Friend WithEvents lblConditionsTitle As Label
    Friend WithEvents lblPre10mmLabel As Label
    Friend WithEvents lblPre10mmValue As Label
    Friend WithEvents lblPost1mmLabel As Label
    Friend WithEvents lblPost1mmValue As Label
    Friend WithEvents lblPost5mmLabel As Label
    Friend WithEvents lblPost5mmValue As Label
    Friend WithEvents lblPost10mmLabel As Label
    Friend WithEvents lblPost10mmValue As Label
    Friend WithEvents lblEdgeLabel As Label
    Friend WithEvents lblEdgeValue As Label
    Friend WithEvents lblBubbleLabel As Label
    Friend WithEvents lblBubbleValue As Label
    Friend WithEvents pnlCardInfo As Panel
    Friend WithEvents lblCardInfoTitle As Label
    Friend WithEvents lblCardNoDisplayLabel As Label
    Friend WithEvents lblCardNoDisplayValue As Label
    Friend WithEvents lblProductNameLabel As Label
    Friend WithEvents lblProductNameValue As Label
    Friend WithEvents lblQuantityLabel As Label
    Friend WithEvents lblQuantityValue As Label
    Friend WithEvents lblLocationLabel As Label
    Friend WithEvents lblLocationValue As Label
    Friend WithEvents lblEmployeeNo As Label
    Friend WithEvents lblCardNo As Label
End Class

