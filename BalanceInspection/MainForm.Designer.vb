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
        Me.lblEmployeeNameValue = New System.Windows.Forms.Label()
        Me.txtCardNo = New System.Windows.Forms.TextBox()
        Me.cmbLapThickness = New System.Windows.Forms.ComboBox()
        Me.lblLapThickness = New System.Windows.Forms.Label()
        Me.btnVerify = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.pnlConditions = New System.Windows.Forms.Panel()
        Me.lblConditionsTitle = New System.Windows.Forms.Label()
        Me.lblHeaderMaterial = New System.Windows.Forms.Label()
        Me.lblHeaderRequired = New System.Windows.Forms.Label()
        Me.lblHeaderRemaining = New System.Windows.Forms.Label()
        Me.lblHeaderSecured = New System.Windows.Forms.Label()
        Me.lblHeaderUsed = New System.Windows.Forms.Label()
        Me.lblHeaderJudgment = New System.Windows.Forms.Label()
        Me.lblPre10mmName = New System.Windows.Forms.Label()
        Me.lblPre10mmRequired = New System.Windows.Forms.Label()
        Me.lblPre10mmRemaining = New System.Windows.Forms.Label()
        Me.lblPre10mmSecured = New System.Windows.Forms.Label()
        Me.lblPre10mmUsed = New System.Windows.Forms.Label()
        Me.lblPre10mmJudgment = New System.Windows.Forms.Label()
        Me.lblPost1mmName = New System.Windows.Forms.Label()
        Me.lblPost1mmRequired = New System.Windows.Forms.Label()
        Me.lblPost1mmRemaining = New System.Windows.Forms.Label()
        Me.lblPost1mmSecured = New System.Windows.Forms.Label()
        Me.lblPost1mmUsed = New System.Windows.Forms.Label()
        Me.lblPost1mmJudgment = New System.Windows.Forms.Label()
        Me.lblPost5mmName = New System.Windows.Forms.Label()
        Me.lblPost5mmRequired = New System.Windows.Forms.Label()
        Me.lblPost5mmRemaining = New System.Windows.Forms.Label()
        Me.lblPost5mmSecured = New System.Windows.Forms.Label()
        Me.lblPost5mmUsed = New System.Windows.Forms.Label()
        Me.lblPost5mmJudgment = New System.Windows.Forms.Label()
        Me.lblPost10mmName = New System.Windows.Forms.Label()
        Me.lblPost10mmRequired = New System.Windows.Forms.Label()
        Me.lblPost10mmRemaining = New System.Windows.Forms.Label()
        Me.lblPost10mmSecured = New System.Windows.Forms.Label()
        Me.lblPost10mmUsed = New System.Windows.Forms.Label()
        Me.lblPost10mmJudgment = New System.Windows.Forms.Label()
        Me.lblEdgeName = New System.Windows.Forms.Label()
        Me.lblEdgeRequired = New System.Windows.Forms.Label()
        Me.lblEdgeRemaining = New System.Windows.Forms.Label()
        Me.lblEdgeSecured = New System.Windows.Forms.Label()
        Me.lblEdgeUsed = New System.Windows.Forms.Label()
        Me.lblEdgeJudgment = New System.Windows.Forms.Label()
        Me.lblBubbleName = New System.Windows.Forms.Label()
        Me.lblBubbleRequired = New System.Windows.Forms.Label()
        Me.lblBubbleRemaining = New System.Windows.Forms.Label()
        Me.lblBubbleSecured = New System.Windows.Forms.Label()
        Me.lblBubbleUsed = New System.Windows.Forms.Label()
        Me.lblBubbleJudgment = New System.Windows.Forms.Label()
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
        Me.txtEmployeeNo.Location = New System.Drawing.Point(242, 156)
        Me.txtEmployeeNo.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.txtEmployeeNo.MaxLength = 6
        Me.txtEmployeeNo.Name = "txtEmployeeNo"
        Me.txtEmployeeNo.Size = New System.Drawing.Size(276, 39)
        Me.txtEmployeeNo.TabIndex = 1
        '
        'lblEmployeeNameValue
        '
        Me.lblEmployeeNameValue.AutoSize = True
        Me.lblEmployeeNameValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblEmployeeNameValue.Location = New System.Drawing.Point(546, 160)
        Me.lblEmployeeNameValue.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblEmployeeNameValue.Name = "lblEmployeeNameValue"
        Me.lblEmployeeNameValue.Size = New System.Drawing.Size(0, 33)
        Me.lblEmployeeNameValue.TabIndex = 7
        '
        'txtCardNo
        '
        Me.txtCardNo.Enabled = False
        Me.txtCardNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.txtCardNo.Location = New System.Drawing.Point(242, 215)
        Me.txtCardNo.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.txtCardNo.MaxLength = 6
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(276, 39)
        Me.txtCardNo.TabIndex = 3
        '
        'cmbLapThickness
        '
        Me.cmbLapThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLapThickness.Enabled = False
        Me.cmbLapThickness.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.cmbLapThickness.FormattingEnabled = True
        Me.cmbLapThickness.Location = New System.Drawing.Point(242, 273)
        Me.cmbLapThickness.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.cmbLapThickness.Name = "cmbLapThickness"
        Me.cmbLapThickness.Size = New System.Drawing.Size(276, 41)
        Me.cmbLapThickness.TabIndex = 4
        '
        'lblLapThickness
        '
        Me.lblLapThickness.AutoSize = True
        Me.lblLapThickness.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblLapThickness.Location = New System.Drawing.Point(39, 277)
        Me.lblLapThickness.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblLapThickness.Name = "lblLapThickness"
        Me.lblLapThickness.Size = New System.Drawing.Size(111, 33)
        Me.lblLapThickness.TabIndex = 9
        Me.lblLapThickness.Text = "LAP厚:"
        '
        'btnVerify
        '
        Me.btnVerify.Enabled = False
        Me.btnVerify.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.btnVerify.Location = New System.Drawing.Point(851, 452)
        Me.btnVerify.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(292, 138)
        Me.btnVerify.TabIndex = 5
        Me.btnVerify.Text = "照合"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.btnCancel.Location = New System.Drawing.Point(851, 156)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(292, 92)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("MS UI Gothic", 18.0!)
        Me.lblMessage.ForeColor = System.Drawing.Color.Black
        Me.lblMessage.Location = New System.Drawing.Point(38, 77)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(1719, 64)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "従業員Noを入力してください"
        '
        'pnlConditions
        '
        Me.pnlConditions.BackColor = System.Drawing.Color.White
        Me.pnlConditions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlConditions.Controls.Add(Me.lblConditionsTitle)
        Me.pnlConditions.Controls.Add(Me.lblHeaderMaterial)
        Me.pnlConditions.Controls.Add(Me.lblHeaderRequired)
        Me.pnlConditions.Controls.Add(Me.lblHeaderRemaining)
        Me.pnlConditions.Controls.Add(Me.lblHeaderSecured)
        Me.pnlConditions.Controls.Add(Me.lblHeaderUsed)
        Me.pnlConditions.Controls.Add(Me.lblHeaderJudgment)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmName)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmRequired)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmRemaining)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmSecured)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmUsed)
        Me.pnlConditions.Controls.Add(Me.lblPre10mmJudgment)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmName)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmRequired)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmRemaining)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmSecured)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmUsed)
        Me.pnlConditions.Controls.Add(Me.lblPost1mmJudgment)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmName)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmRequired)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmRemaining)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmSecured)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmUsed)
        Me.pnlConditions.Controls.Add(Me.lblPost5mmJudgment)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmName)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmRequired)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmRemaining)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmSecured)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmUsed)
        Me.pnlConditions.Controls.Add(Me.lblPost10mmJudgment)
        Me.pnlConditions.Controls.Add(Me.lblEdgeRemaining)
        Me.pnlConditions.Controls.Add(Me.lblEdgeSecured)
        Me.pnlConditions.Controls.Add(Me.lblEdgeUsed)
        Me.pnlConditions.Controls.Add(Me.lblEdgeJudgment)
        Me.pnlConditions.Controls.Add(Me.lblBubbleRemaining)
        Me.pnlConditions.Controls.Add(Me.lblBubbleSecured)
        Me.pnlConditions.Controls.Add(Me.lblBubbleUsed)
        Me.pnlConditions.Controls.Add(Me.lblBubbleJudgment)
        Me.pnlConditions.Location = New System.Drawing.Point(39, 640)
        Me.pnlConditions.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.pnlConditions.Name = "pnlConditions"
        Me.pnlConditions.Size = New System.Drawing.Size(1104, 312)
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
        Me.lblConditionsTitle.Size = New System.Drawing.Size(1103, 51)
        Me.lblConditionsTitle.TabIndex = 0
        Me.lblConditionsTitle.Text = "使用部材条件"
        Me.lblConditionsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderMaterial
        '
        Me.lblHeaderMaterial.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderMaterial.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderMaterial.Location = New System.Drawing.Point(0, 52)
        Me.lblHeaderMaterial.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderMaterial.Name = "lblHeaderMaterial"
        Me.lblHeaderMaterial.Size = New System.Drawing.Size(259, 51)
        Me.lblHeaderMaterial.TabIndex = 1
        Me.lblHeaderMaterial.Text = "部材名"
        Me.lblHeaderMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderRequired
        '
        Me.lblHeaderRequired.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderRequired.Location = New System.Drawing.Point(259, 52)
        Me.lblHeaderRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderRequired.Name = "lblHeaderRequired"
        Me.lblHeaderRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblHeaderRequired.TabIndex = 2
        Me.lblHeaderRequired.Text = "必要枚数"
        Me.lblHeaderRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderRemaining
        '
        Me.lblHeaderRemaining.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderRemaining.Location = New System.Drawing.Point(426, 52)
        Me.lblHeaderRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderRemaining.Name = "lblHeaderRemaining"
        Me.lblHeaderRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblHeaderRemaining.TabIndex = 3
        Me.lblHeaderRemaining.Text = "照合前　数"
        Me.lblHeaderRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderSecured
        '
        Me.lblHeaderSecured.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderSecured.Location = New System.Drawing.Point(594, 52)
        Me.lblHeaderSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderSecured.Name = "lblHeaderSecured"
        Me.lblHeaderSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblHeaderSecured.TabIndex = 4
        Me.lblHeaderSecured.Text = "照合後　数"
        Me.lblHeaderSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderUsed
        '
        Me.lblHeaderUsed.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderUsed.Location = New System.Drawing.Point(762, 52)
        Me.lblHeaderUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderUsed.Name = "lblHeaderUsed"
        Me.lblHeaderUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblHeaderUsed.TabIndex = 5
        Me.lblHeaderUsed.Text = "確保枚数"
        Me.lblHeaderUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHeaderJudgment
        '
        Me.lblHeaderJudgment.BackColor = System.Drawing.Color.LightGray
        Me.lblHeaderJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblHeaderJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeaderJudgment.Location = New System.Drawing.Point(929, 52)
        Me.lblHeaderJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblHeaderJudgment.Name = "lblHeaderJudgment"
        Me.lblHeaderJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblHeaderJudgment.TabIndex = 6
        Me.lblHeaderJudgment.Text = "判定"
        Me.lblHeaderJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmName
        '
        Me.lblPre10mmName.BackColor = System.Drawing.Color.White
        Me.lblPre10mmName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmName.Location = New System.Drawing.Point(0, 104)
        Me.lblPre10mmName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmName.Name = "lblPre10mmName"
        Me.lblPre10mmName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblPre10mmName.Size = New System.Drawing.Size(259, 51)
        Me.lblPre10mmName.TabIndex = 6
        Me.lblPre10mmName.Text = "投入前　10mm"
        Me.lblPre10mmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPre10mmRequired
        '
        Me.lblPre10mmRequired.BackColor = System.Drawing.Color.White
        Me.lblPre10mmRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmRequired.Location = New System.Drawing.Point(259, 104)
        Me.lblPre10mmRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmRequired.Name = "lblPre10mmRequired"
        Me.lblPre10mmRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblPre10mmRequired.TabIndex = 7
        Me.lblPre10mmRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmRemaining
        '
        Me.lblPre10mmRemaining.BackColor = System.Drawing.Color.White
        Me.lblPre10mmRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmRemaining.Location = New System.Drawing.Point(426, 104)
        Me.lblPre10mmRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmRemaining.Name = "lblPre10mmRemaining"
        Me.lblPre10mmRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblPre10mmRemaining.TabIndex = 8
        Me.lblPre10mmRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmSecured
        '
        Me.lblPre10mmSecured.BackColor = System.Drawing.Color.White
        Me.lblPre10mmSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmSecured.Location = New System.Drawing.Point(594, 104)
        Me.lblPre10mmSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmSecured.Name = "lblPre10mmSecured"
        Me.lblPre10mmSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblPre10mmSecured.TabIndex = 9
        Me.lblPre10mmSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmUsed
        '
        Me.lblPre10mmUsed.BackColor = System.Drawing.Color.White
        Me.lblPre10mmUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmUsed.Location = New System.Drawing.Point(762, 104)
        Me.lblPre10mmUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmUsed.Name = "lblPre10mmUsed"
        Me.lblPre10mmUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblPre10mmUsed.TabIndex = 10
        Me.lblPre10mmUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPre10mmJudgment
        '
        Me.lblPre10mmJudgment.BackColor = System.Drawing.Color.White
        Me.lblPre10mmJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPre10mmJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPre10mmJudgment.Location = New System.Drawing.Point(929, 104)
        Me.lblPre10mmJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPre10mmJudgment.Name = "lblPre10mmJudgment"
        Me.lblPre10mmJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblPre10mmJudgment.TabIndex = 11
        Me.lblPre10mmJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmName
        '
        Me.lblPost1mmName.BackColor = System.Drawing.Color.White
        Me.lblPost1mmName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmName.Location = New System.Drawing.Point(0, 156)
        Me.lblPost1mmName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmName.Name = "lblPost1mmName"
        Me.lblPost1mmName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblPost1mmName.Size = New System.Drawing.Size(259, 51)
        Me.lblPost1mmName.TabIndex = 11
        Me.lblPost1mmName.Text = "投入後　1mm"
        Me.lblPost1mmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPost1mmRequired
        '
        Me.lblPost1mmRequired.BackColor = System.Drawing.Color.White
        Me.lblPost1mmRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmRequired.Location = New System.Drawing.Point(259, 156)
        Me.lblPost1mmRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmRequired.Name = "lblPost1mmRequired"
        Me.lblPost1mmRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblPost1mmRequired.TabIndex = 12
        Me.lblPost1mmRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmRemaining
        '
        Me.lblPost1mmRemaining.BackColor = System.Drawing.Color.White
        Me.lblPost1mmRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmRemaining.Location = New System.Drawing.Point(426, 156)
        Me.lblPost1mmRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmRemaining.Name = "lblPost1mmRemaining"
        Me.lblPost1mmRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblPost1mmRemaining.TabIndex = 13
        Me.lblPost1mmRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmSecured
        '
        Me.lblPost1mmSecured.BackColor = System.Drawing.Color.White
        Me.lblPost1mmSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmSecured.Location = New System.Drawing.Point(594, 156)
        Me.lblPost1mmSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmSecured.Name = "lblPost1mmSecured"
        Me.lblPost1mmSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblPost1mmSecured.TabIndex = 14
        Me.lblPost1mmSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmUsed
        '
        Me.lblPost1mmUsed.BackColor = System.Drawing.Color.White
        Me.lblPost1mmUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmUsed.Location = New System.Drawing.Point(762, 156)
        Me.lblPost1mmUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmUsed.Name = "lblPost1mmUsed"
        Me.lblPost1mmUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblPost1mmUsed.TabIndex = 15
        Me.lblPost1mmUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost1mmJudgment
        '
        Me.lblPost1mmJudgment.BackColor = System.Drawing.Color.White
        Me.lblPost1mmJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost1mmJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost1mmJudgment.Location = New System.Drawing.Point(929, 156)
        Me.lblPost1mmJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost1mmJudgment.Name = "lblPost1mmJudgment"
        Me.lblPost1mmJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblPost1mmJudgment.TabIndex = 16
        Me.lblPost1mmJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmName
        '
        Me.lblPost5mmName.BackColor = System.Drawing.Color.White
        Me.lblPost5mmName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmName.Location = New System.Drawing.Point(0, 208)
        Me.lblPost5mmName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmName.Name = "lblPost5mmName"
        Me.lblPost5mmName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblPost5mmName.Size = New System.Drawing.Size(259, 51)
        Me.lblPost5mmName.TabIndex = 16
        Me.lblPost5mmName.Text = "投入後　5mm"
        Me.lblPost5mmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPost5mmRequired
        '
        Me.lblPost5mmRequired.BackColor = System.Drawing.Color.White
        Me.lblPost5mmRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmRequired.Location = New System.Drawing.Point(259, 208)
        Me.lblPost5mmRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmRequired.Name = "lblPost5mmRequired"
        Me.lblPost5mmRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblPost5mmRequired.TabIndex = 17
        Me.lblPost5mmRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmRemaining
        '
        Me.lblPost5mmRemaining.BackColor = System.Drawing.Color.White
        Me.lblPost5mmRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmRemaining.Location = New System.Drawing.Point(426, 208)
        Me.lblPost5mmRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmRemaining.Name = "lblPost5mmRemaining"
        Me.lblPost5mmRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblPost5mmRemaining.TabIndex = 18
        Me.lblPost5mmRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmSecured
        '
        Me.lblPost5mmSecured.BackColor = System.Drawing.Color.White
        Me.lblPost5mmSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmSecured.Location = New System.Drawing.Point(594, 208)
        Me.lblPost5mmSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmSecured.Name = "lblPost5mmSecured"
        Me.lblPost5mmSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblPost5mmSecured.TabIndex = 19
        Me.lblPost5mmSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmUsed
        '
        Me.lblPost5mmUsed.BackColor = System.Drawing.Color.White
        Me.lblPost5mmUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmUsed.Location = New System.Drawing.Point(762, 208)
        Me.lblPost5mmUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmUsed.Name = "lblPost5mmUsed"
        Me.lblPost5mmUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblPost5mmUsed.TabIndex = 20
        Me.lblPost5mmUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost5mmJudgment
        '
        Me.lblPost5mmJudgment.BackColor = System.Drawing.Color.White
        Me.lblPost5mmJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost5mmJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost5mmJudgment.Location = New System.Drawing.Point(929, 208)
        Me.lblPost5mmJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost5mmJudgment.Name = "lblPost5mmJudgment"
        Me.lblPost5mmJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblPost5mmJudgment.TabIndex = 21
        Me.lblPost5mmJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmName
        '
        Me.lblPost10mmName.BackColor = System.Drawing.Color.White
        Me.lblPost10mmName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmName.Location = New System.Drawing.Point(0, 260)
        Me.lblPost10mmName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmName.Name = "lblPost10mmName"
        Me.lblPost10mmName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblPost10mmName.Size = New System.Drawing.Size(259, 51)
        Me.lblPost10mmName.TabIndex = 22
        Me.lblPost10mmName.Text = "投入後　10mm"
        Me.lblPost10mmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPost10mmRequired
        '
        Me.lblPost10mmRequired.BackColor = System.Drawing.Color.White
        Me.lblPost10mmRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmRequired.Location = New System.Drawing.Point(259, 260)
        Me.lblPost10mmRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmRequired.Name = "lblPost10mmRequired"
        Me.lblPost10mmRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblPost10mmRequired.TabIndex = 23
        Me.lblPost10mmRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmRemaining
        '
        Me.lblPost10mmRemaining.BackColor = System.Drawing.Color.White
        Me.lblPost10mmRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmRemaining.Location = New System.Drawing.Point(426, 260)
        Me.lblPost10mmRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmRemaining.Name = "lblPost10mmRemaining"
        Me.lblPost10mmRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblPost10mmRemaining.TabIndex = 24
        Me.lblPost10mmRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmSecured
        '
        Me.lblPost10mmSecured.BackColor = System.Drawing.Color.White
        Me.lblPost10mmSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmSecured.Location = New System.Drawing.Point(594, 260)
        Me.lblPost10mmSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmSecured.Name = "lblPost10mmSecured"
        Me.lblPost10mmSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblPost10mmSecured.TabIndex = 25
        Me.lblPost10mmSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmUsed
        '
        Me.lblPost10mmUsed.BackColor = System.Drawing.Color.White
        Me.lblPost10mmUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmUsed.Location = New System.Drawing.Point(762, 260)
        Me.lblPost10mmUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmUsed.Name = "lblPost10mmUsed"
        Me.lblPost10mmUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblPost10mmUsed.TabIndex = 26
        Me.lblPost10mmUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPost10mmJudgment
        '
        Me.lblPost10mmJudgment.BackColor = System.Drawing.Color.White
        Me.lblPost10mmJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPost10mmJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblPost10mmJudgment.Location = New System.Drawing.Point(929, 260)
        Me.lblPost10mmJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPost10mmJudgment.Name = "lblPost10mmJudgment"
        Me.lblPost10mmJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblPost10mmJudgment.TabIndex = 27
        Me.lblPost10mmJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeName
        '
        Me.lblEdgeName.BackColor = System.Drawing.Color.White
        Me.lblEdgeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeName.Location = New System.Drawing.Point(39, 983)
        Me.lblEdgeName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeName.Name = "lblEdgeName"
        Me.lblEdgeName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblEdgeName.Size = New System.Drawing.Size(259, 51)
        Me.lblEdgeName.TabIndex = 28
        Me.lblEdgeName.Text = "エッジガード"
        Me.lblEdgeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEdgeRequired
        '
        Me.lblEdgeRequired.BackColor = System.Drawing.Color.White
        Me.lblEdgeRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeRequired.Location = New System.Drawing.Point(298, 983)
        Me.lblEdgeRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeRequired.Name = "lblEdgeRequired"
        Me.lblEdgeRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblEdgeRequired.TabIndex = 29
        Me.lblEdgeRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeRemaining
        '
        Me.lblEdgeRemaining.BackColor = System.Drawing.Color.White
        Me.lblEdgeRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeRemaining.Location = New System.Drawing.Point(426, 312)
        Me.lblEdgeRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeRemaining.Name = "lblEdgeRemaining"
        Me.lblEdgeRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblEdgeRemaining.TabIndex = 30
        Me.lblEdgeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeSecured
        '
        Me.lblEdgeSecured.BackColor = System.Drawing.Color.White
        Me.lblEdgeSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeSecured.Location = New System.Drawing.Point(594, 312)
        Me.lblEdgeSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeSecured.Name = "lblEdgeSecured"
        Me.lblEdgeSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblEdgeSecured.TabIndex = 31
        Me.lblEdgeSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeUsed
        '
        Me.lblEdgeUsed.BackColor = System.Drawing.Color.White
        Me.lblEdgeUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeUsed.Location = New System.Drawing.Point(762, 312)
        Me.lblEdgeUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeUsed.Name = "lblEdgeUsed"
        Me.lblEdgeUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblEdgeUsed.TabIndex = 32
        Me.lblEdgeUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEdgeJudgment
        '
        Me.lblEdgeJudgment.BackColor = System.Drawing.Color.White
        Me.lblEdgeJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEdgeJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblEdgeJudgment.Location = New System.Drawing.Point(929, 312)
        Me.lblEdgeJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblEdgeJudgment.Name = "lblEdgeJudgment"
        Me.lblEdgeJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblEdgeJudgment.TabIndex = 33
        Me.lblEdgeJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleName
        '
        Me.lblBubbleName.BackColor = System.Drawing.Color.White
        Me.lblBubbleName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleName.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleName.Location = New System.Drawing.Point(39, 1035)
        Me.lblBubbleName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleName.Name = "lblBubbleName"
        Me.lblBubbleName.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblBubbleName.Size = New System.Drawing.Size(259, 51)
        Me.lblBubbleName.TabIndex = 34
        Me.lblBubbleName.Text = "気泡緩衝材"
        Me.lblBubbleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBubbleRequired
        '
        Me.lblBubbleRequired.BackColor = System.Drawing.Color.White
        Me.lblBubbleRequired.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleRequired.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleRequired.Location = New System.Drawing.Point(298, 1035)
        Me.lblBubbleRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleRequired.Name = "lblBubbleRequired"
        Me.lblBubbleRequired.Size = New System.Drawing.Size(168, 51)
        Me.lblBubbleRequired.TabIndex = 35
        Me.lblBubbleRequired.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleRemaining
        '
        Me.lblBubbleRemaining.BackColor = System.Drawing.Color.White
        Me.lblBubbleRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleRemaining.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleRemaining.Location = New System.Drawing.Point(426, 364)
        Me.lblBubbleRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleRemaining.Name = "lblBubbleRemaining"
        Me.lblBubbleRemaining.Size = New System.Drawing.Size(168, 51)
        Me.lblBubbleRemaining.TabIndex = 36
        Me.lblBubbleRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleSecured
        '
        Me.lblBubbleSecured.BackColor = System.Drawing.Color.White
        Me.lblBubbleSecured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleSecured.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleSecured.Location = New System.Drawing.Point(594, 364)
        Me.lblBubbleSecured.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleSecured.Name = "lblBubbleSecured"
        Me.lblBubbleSecured.Size = New System.Drawing.Size(168, 51)
        Me.lblBubbleSecured.TabIndex = 37
        Me.lblBubbleSecured.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleUsed
        '
        Me.lblBubbleUsed.BackColor = System.Drawing.Color.White
        Me.lblBubbleUsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleUsed.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleUsed.Location = New System.Drawing.Point(762, 364)
        Me.lblBubbleUsed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleUsed.Name = "lblBubbleUsed"
        Me.lblBubbleUsed.Size = New System.Drawing.Size(168, 51)
        Me.lblBubbleUsed.TabIndex = 38
        Me.lblBubbleUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBubbleJudgment
        '
        Me.lblBubbleJudgment.BackColor = System.Drawing.Color.White
        Me.lblBubbleJudgment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBubbleJudgment.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.lblBubbleJudgment.Location = New System.Drawing.Point(929, 364)
        Me.lblBubbleJudgment.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBubbleJudgment.Name = "lblBubbleJudgment"
        Me.lblBubbleJudgment.Size = New System.Drawing.Size(174, 51)
        Me.lblBubbleJudgment.TabIndex = 39
        Me.lblBubbleJudgment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.pnlCardInfo.Location = New System.Drawing.Point(39, 347)
        Me.pnlCardInfo.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.pnlCardInfo.Name = "pnlCardInfo"
        Me.pnlCardInfo.Size = New System.Drawing.Size(649, 265)
        Me.pnlCardInfo.TabIndex = 8
        '
        'lblLocationValue
        '
        Me.lblLocationValue.BackColor = System.Drawing.Color.White
        Me.lblLocationValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocationValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblLocationValue.Location = New System.Drawing.Point(194, 208)
        Me.lblLocationValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblLocationValue.Name = "lblLocationValue"
        Me.lblLocationValue.Padding = New System.Windows.Forms.Padding(13, 0, 0, 0)
        Me.lblLocationValue.Size = New System.Drawing.Size(454, 51)
        Me.lblLocationValue.TabIndex = 5
        Me.lblLocationValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLocationLabel
        '
        Me.lblLocationLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblLocationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocationLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblLocationLabel.Location = New System.Drawing.Point(0, 208)
        Me.lblLocationLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblLocationLabel.Name = "lblLocationLabel"
        Me.lblLocationLabel.Size = New System.Drawing.Size(194, 51)
        Me.lblLocationLabel.TabIndex = 4
        Me.lblLocationLabel.Text = "工程"
        Me.lblLocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblQuantityValue
        '
        Me.lblQuantityValue.BackColor = System.Drawing.Color.White
        Me.lblQuantityValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQuantityValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblQuantityValue.Location = New System.Drawing.Point(194, 156)
        Me.lblQuantityValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuantityValue.Name = "lblQuantityValue"
        Me.lblQuantityValue.Padding = New System.Windows.Forms.Padding(13, 0, 0, 0)
        Me.lblQuantityValue.Size = New System.Drawing.Size(454, 51)
        Me.lblQuantityValue.TabIndex = 3
        Me.lblQuantityValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblQuantityLabel
        '
        Me.lblQuantityLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblQuantityLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQuantityLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblQuantityLabel.Location = New System.Drawing.Point(0, 156)
        Me.lblQuantityLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuantityLabel.Name = "lblQuantityLabel"
        Me.lblQuantityLabel.Size = New System.Drawing.Size(194, 51)
        Me.lblQuantityLabel.TabIndex = 2
        Me.lblQuantityLabel.Text = "枚数"
        Me.lblQuantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblProductNameValue
        '
        Me.lblProductNameValue.BackColor = System.Drawing.Color.White
        Me.lblProductNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblProductNameValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblProductNameValue.Location = New System.Drawing.Point(194, 104)
        Me.lblProductNameValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProductNameValue.Name = "lblProductNameValue"
        Me.lblProductNameValue.Padding = New System.Windows.Forms.Padding(13, 0, 0, 0)
        Me.lblProductNameValue.Size = New System.Drawing.Size(454, 51)
        Me.lblProductNameValue.TabIndex = 1
        Me.lblProductNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProductNameLabel
        '
        Me.lblProductNameLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblProductNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblProductNameLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblProductNameLabel.Location = New System.Drawing.Point(0, 104)
        Me.lblProductNameLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProductNameLabel.Name = "lblProductNameLabel"
        Me.lblProductNameLabel.Size = New System.Drawing.Size(194, 51)
        Me.lblProductNameLabel.TabIndex = 0
        Me.lblProductNameLabel.Text = "品名"
        Me.lblProductNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCardNoDisplayValue
        '
        Me.lblCardNoDisplayValue.BackColor = System.Drawing.Color.White
        Me.lblCardNoDisplayValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCardNoDisplayValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNoDisplayValue.Location = New System.Drawing.Point(194, 52)
        Me.lblCardNoDisplayValue.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCardNoDisplayValue.Name = "lblCardNoDisplayValue"
        Me.lblCardNoDisplayValue.Padding = New System.Windows.Forms.Padding(13, 0, 0, 0)
        Me.lblCardNoDisplayValue.Size = New System.Drawing.Size(454, 51)
        Me.lblCardNoDisplayValue.TabIndex = 8
        Me.lblCardNoDisplayValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCardNoDisplayLabel
        '
        Me.lblCardNoDisplayLabel.BackColor = System.Drawing.Color.LightGray
        Me.lblCardNoDisplayLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCardNoDisplayLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNoDisplayLabel.Location = New System.Drawing.Point(0, 52)
        Me.lblCardNoDisplayLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCardNoDisplayLabel.Name = "lblCardNoDisplayLabel"
        Me.lblCardNoDisplayLabel.Size = New System.Drawing.Size(194, 51)
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
        Me.lblCardInfoTitle.Size = New System.Drawing.Size(648, 51)
        Me.lblCardInfoTitle.TabIndex = 6
        Me.lblCardInfoTitle.Text = "カード情報"
        Me.lblCardInfoTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEmployeeNo
        '
        Me.lblEmployeeNo.AutoSize = True
        Me.lblEmployeeNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblEmployeeNo.Location = New System.Drawing.Point(39, 160)
        Me.lblEmployeeNo.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblEmployeeNo.Name = "lblEmployeeNo"
        Me.lblEmployeeNo.Size = New System.Drawing.Size(155, 33)
        Me.lblEmployeeNo.TabIndex = 0
        Me.lblEmployeeNo.Text = "従業員No:"
        '
        'lblCardNo
        '
        Me.lblCardNo.AutoSize = True
        Me.lblCardNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblCardNo.Location = New System.Drawing.Point(39, 215)
        Me.lblCardNo.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblCardNo.Name = "lblCardNo"
        Me.lblCardNo.Size = New System.Drawing.Size(131, 33)
        Me.lblCardNo.TabIndex = 2
        Me.lblCardNo.Text = "カードNo:"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(13.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1247, 1229)
        Me.Controls.Add(Me.pnlConditions)
        Me.Controls.Add(Me.pnlCardInfo)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.lblLapThickness)
        Me.Controls.Add(Me.cmbLapThickness)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.lblCardNo)
        Me.Controls.Add(Me.lblEmployeeNameValue)
        Me.Controls.Add(Me.txtEmployeeNo)
        Me.Controls.Add(Me.lblEmployeeNo)
        Me.Controls.Add(Me.lblBubbleRequired)
        Me.Controls.Add(Me.lblBubbleName)
        Me.Controls.Add(Me.lblEdgeRequired)
        Me.Controls.Add(Me.lblEdgeName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.MaximizeBox = True
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "検品デスクトップアプリ"
        Me.pnlConditions.ResumeLayout(False)
        Me.pnlCardInfo.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmployeeNo As TextBox
    Friend WithEvents lblEmployeeNameValue As Label
    Friend WithEvents txtCardNo As TextBox
    Friend WithEvents cmbLapThickness As ComboBox
    Friend WithEvents lblLapThickness As Label
    Friend WithEvents btnVerify As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents pnlConditions As Panel
    Friend WithEvents lblConditionsTitle As Label
    Friend WithEvents lblHeaderMaterial As Label
    Friend WithEvents lblHeaderRequired As Label
    Friend WithEvents lblHeaderRemaining As Label
    Friend WithEvents lblHeaderSecured As Label
    Friend WithEvents lblHeaderUsed As Label
    Friend WithEvents lblHeaderJudgment As Label
    Friend WithEvents lblPre10mmName As Label
    Friend WithEvents lblPre10mmRequired As Label
    Friend WithEvents lblPre10mmRemaining As Label
    Friend WithEvents lblPre10mmSecured As Label
    Friend WithEvents lblPre10mmUsed As Label
    Friend WithEvents lblPre10mmJudgment As Label
    Friend WithEvents lblPost1mmName As Label
    Friend WithEvents lblPost1mmRequired As Label
    Friend WithEvents lblPost1mmRemaining As Label
    Friend WithEvents lblPost1mmSecured As Label
    Friend WithEvents lblPost1mmUsed As Label
    Friend WithEvents lblPost1mmJudgment As Label
    Friend WithEvents lblPost5mmName As Label
    Friend WithEvents lblPost5mmRequired As Label
    Friend WithEvents lblPost5mmRemaining As Label
    Friend WithEvents lblPost5mmSecured As Label
    Friend WithEvents lblPost5mmUsed As Label
    Friend WithEvents lblPost5mmJudgment As Label
    Friend WithEvents lblPost10mmName As Label
    Friend WithEvents lblPost10mmRequired As Label
    Friend WithEvents lblPost10mmRemaining As Label
    Friend WithEvents lblPost10mmSecured As Label
    Friend WithEvents lblPost10mmUsed As Label
    Friend WithEvents lblPost10mmJudgment As Label
    Friend WithEvents lblEdgeName As Label
    Friend WithEvents lblEdgeRequired As Label
    Friend WithEvents lblEdgeRemaining As Label
    Friend WithEvents lblEdgeSecured As Label
    Friend WithEvents lblEdgeUsed As Label
    Friend WithEvents lblEdgeJudgment As Label
    Friend WithEvents lblBubbleName As Label
    Friend WithEvents lblBubbleRequired As Label
    Friend WithEvents lblBubbleRemaining As Label
    Friend WithEvents lblBubbleSecured As Label
    Friend WithEvents lblBubbleUsed As Label
    Friend WithEvents lblBubbleJudgment As Label
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

