<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctClaimPayment
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents cmdHistory As System.Windows.Forms.Button
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents lvwPaymentDetails As System.Windows.Forms.ListView
	Friend WithEvents fraPaymentDetails As System.Windows.Forms.GroupBox
	Friend WithEvents chkTaxExempt As System.Windows.Forms.CheckBox
	Friend WithEvents chkWHTExempt As System.Windows.Forms.CheckBox
	Friend WithEvents fraExemptions As System.Windows.Forms.GroupBox
	Friend WithEvents txtSafeHarbour As System.Windows.Forms.TextBox
	Friend WithEvents cboSafeHarbour As System.Windows.Forms.ComboBox
	Friend WithEvents txtSFPercentage As System.Windows.Forms.TextBox
	Friend WithEvents lblAgreement As System.Windows.Forms.Label
	Friend WithEvents lblSFPercentage As System.Windows.Forms.Label
	Friend WithEvents fraSafeHarbour As System.Windows.Forms.GroupBox
	Friend WithEvents chkPTDomiciled As System.Windows.Forms.CheckBox
	Friend WithEvents txtPTPercentage As System.Windows.Forms.TextBox
	Friend WithEvents txtPTTaxNo As System.Windows.Forms.TextBox
	Friend WithEvents lblPTPercentage As System.Windows.Forms.Label
	Friend WithEvents lblPTTaxNo As System.Windows.Forms.Label
	Friend WithEvents fraPayeeTaxAdjustments As System.Windows.Forms.GroupBox
	Friend WithEvents chkITDomiciled As System.Windows.Forms.CheckBox
	Friend WithEvents txtITPercentage As System.Windows.Forms.TextBox
	Friend WithEvents txtITTaxNo As System.Windows.Forms.TextBox
	Friend WithEvents lblITPercentage As System.Windows.Forms.Label
	Friend WithEvents lblITTaxNo As System.Windows.Forms.Label
	Friend WithEvents fraInsuredTaxAdjustment As System.Windows.Forms.GroupBox
	Friend WithEvents txtClaimPaymentTo As System.Windows.Forms.TextBox
	Friend WithEvents cboClaimPaymentTo As System.Windows.Forms.ComboBox
	Friend WithEvents cmdParty As System.Windows.Forms.Button
	Friend WithEvents txtParty As System.Windows.Forms.TextBox
	Friend WithEvents OptClaimPayable As System.Windows.Forms.RadioButton
	Friend WithEvents OptAgent As System.Windows.Forms.RadioButton
	Friend WithEvents OptClient As System.Windows.Forms.RadioButton
	Friend WithEvents OptParty As System.Windows.Forms.RadioButton
	Friend WithEvents lblParty As System.Windows.Forms.Label
	Friend WithEvents lblPaymentTo As System.Windows.Forms.Label
	Friend WithEvents fraPayee As System.Windows.Forms.GroupBox
	Friend WithEvents txtRiskType As System.Windows.Forms.TextBox
	Friend WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtLossDate As System.Windows.Forms.TextBox
	Friend WithEvents lblRiskType As System.Windows.Forms.Label
	Friend WithEvents lblLossCurrency As System.Windows.Forms.Label
	Friend WithEvents lblDateOfLoss As System.Windows.Forms.Label
	Friend WithEvents fraClaimInformation As System.Windows.Forms.GroupBox
	Friend WithEvents chkSettlement As System.Windows.Forms.CheckBox
	Friend WithEvents fraSettlement As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents txtGrossPayment As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalWHTax As System.Windows.Forms.TextBox
	Friend WithEvents txtNetPayment As System.Windows.Forms.TextBox
	Friend WithEvents lblGrossPayment As System.Windows.Forms.Label
	Friend WithEvents lblTotalTax As System.Windows.Forms.Label
	Friend WithEvents lblTotalWHTax As System.Windows.Forms.Label
	Friend WithEvents lblNetPayment As System.Windows.Forms.Label
	Friend WithEvents fraThisPaymentSummary As System.Windows.Forms.GroupBox
	Friend WithEvents txtCountry As System.Windows.Forms.TextBox
	Friend WithEvents txtMediaType As System.Windows.Forms.TextBox
	Friend WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Friend WithEvents txtMediaRef As System.Windows.Forms.TextBox
	Friend WithEvents txtBankName As System.Windows.Forms.TextBox
	Friend WithEvents txtBankCode As System.Windows.Forms.TextBox
	Friend WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Friend WithEvents cboCountry As System.Windows.Forms.ComboBox
	Friend WithEvents txtBankAccountNo As System.Windows.Forms.TextBox
	Friend WithEvents lblPayeeName As System.Windows.Forms.Label
	Friend WithEvents lblMediaRef As System.Windows.Forms.Label
	Friend WithEvents lblBankName As System.Windows.Forms.Label
	Friend WithEvents lblBankCode As System.Windows.Forms.Label
	Friend WithEvents lblMediaType As System.Windows.Forms.Label
	Friend WithEvents lblCountry As System.Windows.Forms.Label
	Friend WithEvents lblBankAccountNo As System.Windows.Forms.Label
	Friend WithEvents fraThisPaymentDetails As System.Windows.Forms.GroupBox
	Friend WithEvents txtPayeeComments As System.Windows.Forms.TextBox
	Friend WithEvents fraThisPaymentComments As System.Windows.Forms.GroupBox
	Friend WithEvents lvwTaxesOnThisPayment As System.Windows.Forms.ListView
	Friend WithEvents fraTaxesOnPayments As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents SSTab1 As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraPaymentDetails = New System.Windows.Forms.GroupBox
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdHistory = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.lvwPaymentDetails = New System.Windows.Forms.ListView
        Me.fraExemptions = New System.Windows.Forms.GroupBox
        Me.chkTaxExempt = New System.Windows.Forms.CheckBox
        Me.chkWHTExempt = New System.Windows.Forms.CheckBox
        Me.fraSafeHarbour = New System.Windows.Forms.GroupBox
        Me.txtSafeHarbour = New System.Windows.Forms.TextBox
        Me.cboSafeHarbour = New System.Windows.Forms.ComboBox
        Me.txtSFPercentage = New System.Windows.Forms.TextBox
        Me.lblAgreement = New System.Windows.Forms.Label
        Me.lblSFPercentage = New System.Windows.Forms.Label
        Me.fraPayeeTaxAdjustments = New System.Windows.Forms.GroupBox
        Me.chkPTDomiciled = New System.Windows.Forms.CheckBox
        Me.txtPTPercentage = New System.Windows.Forms.TextBox
        Me.txtPTTaxNo = New System.Windows.Forms.TextBox
        Me.lblPTPercentage = New System.Windows.Forms.Label
        Me.lblPTTaxNo = New System.Windows.Forms.Label
        Me.fraInsuredTaxAdjustment = New System.Windows.Forms.GroupBox
        Me.chkITDomiciled = New System.Windows.Forms.CheckBox
        Me.txtITPercentage = New System.Windows.Forms.TextBox
        Me.txtITTaxNo = New System.Windows.Forms.TextBox
        Me.lblITPercentage = New System.Windows.Forms.Label
        Me.lblITTaxNo = New System.Windows.Forms.Label
        Me.fraPayee = New System.Windows.Forms.GroupBox
        Me.txtClaimPaymentTo = New System.Windows.Forms.TextBox
        Me.cboClaimPaymentTo = New System.Windows.Forms.ComboBox
        Me.cmdParty = New System.Windows.Forms.Button
        Me.txtParty = New System.Windows.Forms.TextBox
        Me.OptClaimPayable = New System.Windows.Forms.RadioButton
        Me.OptAgent = New System.Windows.Forms.RadioButton
        Me.OptClient = New System.Windows.Forms.RadioButton
        Me.OptParty = New System.Windows.Forms.RadioButton
        Me.lblParty = New System.Windows.Forms.Label
        Me.lblPaymentTo = New System.Windows.Forms.Label
        Me.fraClaimInformation = New System.Windows.Forms.GroupBox
        Me.txtRiskType = New System.Windows.Forms.TextBox
        Me.txtLossCurrency = New System.Windows.Forms.TextBox
        Me.txtLossDate = New System.Windows.Forms.TextBox
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblDateOfLoss = New System.Windows.Forms.Label
        Me.fraSettlement = New System.Windows.Forms.GroupBox
        Me.chkSettlement = New System.Windows.Forms.CheckBox
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraThisPaymentSummary = New System.Windows.Forms.GroupBox
        Me.txtGrossPayment = New System.Windows.Forms.TextBox
        Me.txtTotalTax = New System.Windows.Forms.TextBox
        Me.txtTotalWHTax = New System.Windows.Forms.TextBox
        Me.txtNetPayment = New System.Windows.Forms.TextBox
        Me.lblGrossPayment = New System.Windows.Forms.Label
        Me.lblTotalTax = New System.Windows.Forms.Label
        Me.lblTotalWHTax = New System.Windows.Forms.Label
        Me.lblNetPayment = New System.Windows.Forms.Label
        Me.fraThisPaymentDetails = New System.Windows.Forms.GroupBox
        Me.txtCountry = New System.Windows.Forms.TextBox
        Me.txtMediaType = New System.Windows.Forms.TextBox
        Me.txtPayeeName = New System.Windows.Forms.TextBox
        Me.txtMediaRef = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtBankCode = New System.Windows.Forms.TextBox
        Me.cboMediaType = New System.Windows.Forms.ComboBox
        Me.cboCountry = New System.Windows.Forms.ComboBox
        Me.txtBankAccountNo = New System.Windows.Forms.TextBox
        Me.lblPayeeName = New System.Windows.Forms.Label
        Me.lblMediaRef = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblBankCode = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblBankAccountNo = New System.Windows.Forms.Label
        Me.fraThisPaymentComments = New System.Windows.Forms.GroupBox
        Me.txtPayeeComments = New System.Windows.Forms.TextBox
        Me.fraTaxesOnPayments = New System.Windows.Forms.GroupBox
        Me.lvwTaxesOnThisPayment = New System.Windows.Forms.ListView
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraPaymentDetails.SuspendLayout()
        Me.fraExemptions.SuspendLayout()
        Me.fraSafeHarbour.SuspendLayout()
        Me.fraPayeeTaxAdjustments.SuspendLayout()
        Me.fraInsuredTaxAdjustment.SuspendLayout()
        Me.fraPayee.SuspendLayout()
        Me.fraClaimInformation.SuspendLayout()
        Me.fraSettlement.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.fraThisPaymentSummary.SuspendLayout()
        Me.fraThisPaymentDetails.SuspendLayout()
        Me.fraThisPaymentComments.SuspendLayout()
        Me.fraTaxesOnPayments.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(289, 18)
        Me.SSTab1.Location = New System.Drawing.Point(0, 0)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(874, 445)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 0
        Me.SSTab1.TabStop = False
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPaymentDetails)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraExemptions)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraSafeHarbour)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPayeeTaxAdjustments)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraInsuredTaxAdjustment)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPayee)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraClaimInformation)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraSettlement)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(866, 419)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Payments"
        '
        'fraPaymentDetails
        '
        Me.fraPaymentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentDetails.Controls.Add(Me.cmdEdit)
        Me.fraPaymentDetails.Controls.Add(Me.cmdHistory)
        Me.fraPaymentDetails.Controls.Add(Me.cmdDelete)
        Me.fraPaymentDetails.Controls.Add(Me.lvwPaymentDetails)
        Me.fraPaymentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentDetails.Location = New System.Drawing.Point(8, 200)
        Me.fraPaymentDetails.Name = "fraPaymentDetails"
        Me.fraPaymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentDetails.Size = New System.Drawing.Size(857, 217)
        Me.fraPaymentDetails.TabIndex = 31
        Me.fraPaymentDetails.TabStop = False
        Me.fraPaymentDetails.Text = "Payment Details"
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(696, 192)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 21)
        Me.cmdEdit.TabIndex = 35
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdHistory
        '
        Me.cmdHistory.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHistory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHistory.Location = New System.Drawing.Point(776, 192)
        Me.cmdHistory.Name = "cmdHistory"
        Me.cmdHistory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHistory.Size = New System.Drawing.Size(73, 21)
        Me.cmdHistory.TabIndex = 34
        Me.cmdHistory.Text = "&History"
        Me.cmdHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHistory.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(616, 192)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 21)
        Me.cmdDelete.TabIndex = 32
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        Me.cmdDelete.Visible = False
        '
        'lvwPaymentDetails
        '
        Me.lvwPaymentDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPaymentDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPaymentDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPaymentDetails.FullRowSelect = True
        Me.lvwPaymentDetails.HideSelection = False
        Me.lvwPaymentDetails.Location = New System.Drawing.Point(8, 16)
        Me.lvwPaymentDetails.Name = "lvwPaymentDetails"
        Me.lvwPaymentDetails.Size = New System.Drawing.Size(841, 169)
        Me.lvwPaymentDetails.TabIndex = 33
        Me.lvwPaymentDetails.UseCompatibleStateImageBehavior = False
        Me.lvwPaymentDetails.View = System.Windows.Forms.View.Details
        '
        'fraExemptions
        '
        Me.fraExemptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraExemptions.Controls.Add(Me.chkTaxExempt)
        Me.fraExemptions.Controls.Add(Me.chkWHTExempt)
        Me.fraExemptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraExemptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExemptions.Location = New System.Drawing.Point(440, 151)
        Me.fraExemptions.Name = "fraExemptions"
        Me.fraExemptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraExemptions.Size = New System.Drawing.Size(225, 49)
        Me.fraExemptions.TabIndex = 36
        Me.fraExemptions.TabStop = False
        Me.fraExemptions.Text = "Exemptions"
        '
        'chkTaxExempt
        '
        Me.chkTaxExempt.BackColor = System.Drawing.SystemColors.Control
        Me.chkTaxExempt.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTaxExempt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTaxExempt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTaxExempt.Location = New System.Drawing.Point(16, 16)
        Me.chkTaxExempt.Name = "chkTaxExempt"
        Me.chkTaxExempt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTaxExempt.Size = New System.Drawing.Size(97, 21)
        Me.chkTaxExempt.TabIndex = 38
        Me.chkTaxExempt.Text = "Tax Exempt"
        Me.chkTaxExempt.UseVisualStyleBackColor = False
        '
        'chkWHTExempt
        '
        Me.chkWHTExempt.BackColor = System.Drawing.SystemColors.Control
        Me.chkWHTExempt.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWHTExempt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWHTExempt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWHTExempt.Location = New System.Drawing.Point(120, 16)
        Me.chkWHTExempt.Name = "chkWHTExempt"
        Me.chkWHTExempt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWHTExempt.Size = New System.Drawing.Size(97, 21)
        Me.chkWHTExempt.TabIndex = 37
        Me.chkWHTExempt.Text = "WHT Exempt"
        Me.chkWHTExempt.UseVisualStyleBackColor = False
        '
        'fraSafeHarbour
        '
        Me.fraSafeHarbour.BackColor = System.Drawing.SystemColors.Control
        Me.fraSafeHarbour.Controls.Add(Me.txtSafeHarbour)
        Me.fraSafeHarbour.Controls.Add(Me.cboSafeHarbour)
        Me.fraSafeHarbour.Controls.Add(Me.txtSFPercentage)
        Me.fraSafeHarbour.Controls.Add(Me.lblAgreement)
        Me.fraSafeHarbour.Controls.Add(Me.lblSFPercentage)
        Me.fraSafeHarbour.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSafeHarbour.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSafeHarbour.Location = New System.Drawing.Point(8, 151)
        Me.fraSafeHarbour.Name = "fraSafeHarbour"
        Me.fraSafeHarbour.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSafeHarbour.Size = New System.Drawing.Size(425, 49)
        Me.fraSafeHarbour.TabIndex = 39
        Me.fraSafeHarbour.TabStop = False
        Me.fraSafeHarbour.Text = "Safe Harbour"
        '
        'txtSafeHarbour
        '
        Me.txtSafeHarbour.AcceptsReturn = True
        Me.txtSafeHarbour.BackColor = System.Drawing.SystemColors.Window
        Me.txtSafeHarbour.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSafeHarbour.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSafeHarbour.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSafeHarbour.Location = New System.Drawing.Point(88, 16)
        Me.txtSafeHarbour.MaxLength = 0
        Me.txtSafeHarbour.Name = "txtSafeHarbour"
        Me.txtSafeHarbour.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSafeHarbour.Size = New System.Drawing.Size(201, 21)
        Me.txtSafeHarbour.TabIndex = 42
        '
        'cboSafeHarbour
        '
        Me.cboSafeHarbour.BackColor = System.Drawing.SystemColors.Window
        Me.cboSafeHarbour.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSafeHarbour.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSafeHarbour.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSafeHarbour.Location = New System.Drawing.Point(88, 16)
        Me.cboSafeHarbour.Name = "cboSafeHarbour"
        Me.cboSafeHarbour.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSafeHarbour.Size = New System.Drawing.Size(201, 21)
        Me.cboSafeHarbour.TabIndex = 41
        Me.cboSafeHarbour.Text = "Combo2"
        '
        'txtSFPercentage
        '
        Me.txtSFPercentage.AcceptsReturn = True
        Me.txtSFPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.txtSFPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSFPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSFPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSFPercentage.Location = New System.Drawing.Point(368, 16)
        Me.txtSFPercentage.MaxLength = 5
        Me.txtSFPercentage.Name = "txtSFPercentage"
        Me.txtSFPercentage.ReadOnly = True
        Me.txtSFPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSFPercentage.Size = New System.Drawing.Size(49, 21)
        Me.txtSFPercentage.TabIndex = 40
        '
        'lblAgreement
        '
        Me.lblAgreement.AutoSize = True
        Me.lblAgreement.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgreement.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgreement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgreement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgreement.Location = New System.Drawing.Point(16, 20)
        Me.lblAgreement.Name = "lblAgreement"
        Me.lblAgreement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgreement.Size = New System.Drawing.Size(61, 13)
        Me.lblAgreement.TabIndex = 44
        Me.lblAgreement.Text = "Agreement:"
        '
        'lblSFPercentage
        '
        Me.lblSFPercentage.AutoSize = True
        Me.lblSFPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblSFPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSFPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSFPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSFPercentage.Location = New System.Drawing.Point(296, 20)
        Me.lblSFPercentage.Name = "lblSFPercentage"
        Me.lblSFPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSFPercentage.Size = New System.Drawing.Size(65, 13)
        Me.lblSFPercentage.TabIndex = 43
        Me.lblSFPercentage.Text = "Percentage:"
        '
        'fraPayeeTaxAdjustments
        '
        Me.fraPayeeTaxAdjustments.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.chkPTDomiciled)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.txtPTPercentage)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.txtPTTaxNo)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.lblPTPercentage)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.lblPTTaxNo)
        Me.fraPayeeTaxAdjustments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayeeTaxAdjustments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayeeTaxAdjustments.Location = New System.Drawing.Point(440, 102)
        Me.fraPayeeTaxAdjustments.Name = "fraPayeeTaxAdjustments"
        Me.fraPayeeTaxAdjustments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayeeTaxAdjustments.Size = New System.Drawing.Size(427, 49)
        Me.fraPayeeTaxAdjustments.TabIndex = 45
        Me.fraPayeeTaxAdjustments.TabStop = False
        Me.fraPayeeTaxAdjustments.Text = "Payee Tax Adjustments"
        '
        'chkPTDomiciled
        '
        Me.chkPTDomiciled.BackColor = System.Drawing.SystemColors.Control
        Me.chkPTDomiciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPTDomiciled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPTDomiciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPTDomiciled.Location = New System.Drawing.Point(16, 16)
        Me.chkPTDomiciled.Name = "chkPTDomiciled"
        Me.chkPTDomiciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPTDomiciled.Size = New System.Drawing.Size(81, 21)
        Me.chkPTDomiciled.TabIndex = 48
        Me.chkPTDomiciled.Text = "Domiciled"
        Me.chkPTDomiciled.UseVisualStyleBackColor = False
        '
        'txtPTPercentage
        '
        Me.txtPTPercentage.AcceptsReturn = True
        Me.txtPTPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPTPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPTPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPTPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPTPercentage.Location = New System.Drawing.Point(176, 16)
        Me.txtPTPercentage.MaxLength = 5
        Me.txtPTPercentage.Name = "txtPTPercentage"
        Me.txtPTPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPTPercentage.Size = New System.Drawing.Size(49, 21)
        Me.txtPTPercentage.TabIndex = 47
        '
        'txtPTTaxNo
        '
        Me.txtPTTaxNo.AcceptsReturn = True
        Me.txtPTTaxNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPTTaxNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPTTaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPTTaxNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPTTaxNo.Location = New System.Drawing.Point(280, 16)
        Me.txtPTTaxNo.MaxLength = 30
        Me.txtPTTaxNo.Name = "txtPTTaxNo"
        Me.txtPTTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPTTaxNo.Size = New System.Drawing.Size(137, 21)
        Me.txtPTTaxNo.TabIndex = 46
        '
        'lblPTPercentage
        '
        Me.lblPTPercentage.AutoSize = True
        Me.lblPTPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPTPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPTPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPTPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPTPercentage.Location = New System.Drawing.Point(104, 20)
        Me.lblPTPercentage.Name = "lblPTPercentage"
        Me.lblPTPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPTPercentage.Size = New System.Drawing.Size(65, 13)
        Me.lblPTPercentage.TabIndex = 50
        Me.lblPTPercentage.Text = "Percentage:"
        '
        'lblPTTaxNo
        '
        Me.lblPTTaxNo.AutoSize = True
        Me.lblPTTaxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPTTaxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPTTaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPTTaxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPTTaxNo.Location = New System.Drawing.Point(232, 20)
        Me.lblPTTaxNo.Name = "lblPTTaxNo"
        Me.lblPTTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPTTaxNo.Size = New System.Drawing.Size(45, 13)
        Me.lblPTTaxNo.TabIndex = 49
        Me.lblPTTaxNo.Text = "Tax No:"
        '
        'fraInsuredTaxAdjustment
        '
        Me.fraInsuredTaxAdjustment.BackColor = System.Drawing.SystemColors.Control
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.chkITDomiciled)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.txtITPercentage)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.txtITTaxNo)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.lblITPercentage)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.lblITTaxNo)
        Me.fraInsuredTaxAdjustment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInsuredTaxAdjustment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInsuredTaxAdjustment.Location = New System.Drawing.Point(8, 102)
        Me.fraInsuredTaxAdjustment.Name = "fraInsuredTaxAdjustment"
        Me.fraInsuredTaxAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsuredTaxAdjustment.Size = New System.Drawing.Size(427, 49)
        Me.fraInsuredTaxAdjustment.TabIndex = 51
        Me.fraInsuredTaxAdjustment.TabStop = False
        Me.fraInsuredTaxAdjustment.Text = "Insured Tax Adjustment"
        '
        'chkITDomiciled
        '
        Me.chkITDomiciled.BackColor = System.Drawing.SystemColors.Control
        Me.chkITDomiciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkITDomiciled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkITDomiciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkITDomiciled.Location = New System.Drawing.Point(16, 16)
        Me.chkITDomiciled.Name = "chkITDomiciled"
        Me.chkITDomiciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkITDomiciled.Size = New System.Drawing.Size(81, 21)
        Me.chkITDomiciled.TabIndex = 54
        Me.chkITDomiciled.Text = "Domiciled"
        Me.chkITDomiciled.UseVisualStyleBackColor = False
        '
        'txtITPercentage
        '
        Me.txtITPercentage.AcceptsReturn = True
        Me.txtITPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtITPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtITPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtITPercentage.Location = New System.Drawing.Point(176, 16)
        Me.txtITPercentage.MaxLength = 5
        Me.txtITPercentage.Name = "txtITPercentage"
        Me.txtITPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtITPercentage.Size = New System.Drawing.Size(49, 21)
        Me.txtITPercentage.TabIndex = 53
        '
        'txtITTaxNo
        '
        Me.txtITTaxNo.AcceptsReturn = True
        Me.txtITTaxNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtITTaxNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtITTaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITTaxNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtITTaxNo.Location = New System.Drawing.Point(280, 16)
        Me.txtITTaxNo.MaxLength = 30
        Me.txtITTaxNo.Name = "txtITTaxNo"
        Me.txtITTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtITTaxNo.Size = New System.Drawing.Size(137, 21)
        Me.txtITTaxNo.TabIndex = 52
        '
        'lblITPercentage
        '
        Me.lblITPercentage.AutoSize = True
        Me.lblITPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblITPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblITPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblITPercentage.Location = New System.Drawing.Point(104, 20)
        Me.lblITPercentage.Name = "lblITPercentage"
        Me.lblITPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblITPercentage.Size = New System.Drawing.Size(65, 13)
        Me.lblITPercentage.TabIndex = 56
        Me.lblITPercentage.Text = "Percentage:"
        '
        'lblITTaxNo
        '
        Me.lblITTaxNo.AutoSize = True
        Me.lblITTaxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblITTaxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblITTaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITTaxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblITTaxNo.Location = New System.Drawing.Point(232, 20)
        Me.lblITTaxNo.Name = "lblITTaxNo"
        Me.lblITTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblITTaxNo.Size = New System.Drawing.Size(45, 13)
        Me.lblITTaxNo.TabIndex = 55
        Me.lblITTaxNo.Text = "Tax No:"
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.txtClaimPaymentTo)
        Me.fraPayee.Controls.Add(Me.cboClaimPaymentTo)
        Me.fraPayee.Controls.Add(Me.cmdParty)
        Me.fraPayee.Controls.Add(Me.txtParty)
        Me.fraPayee.Controls.Add(Me.OptClaimPayable)
        Me.fraPayee.Controls.Add(Me.OptAgent)
        Me.fraPayee.Controls.Add(Me.OptClient)
        Me.fraPayee.Controls.Add(Me.OptParty)
        Me.fraPayee.Controls.Add(Me.lblParty)
        Me.fraPayee.Controls.Add(Me.lblPaymentTo)
        Me.fraPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(8, 53)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(857, 49)
        Me.fraPayee.TabIndex = 57
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payee"
        '
        'txtClaimPaymentTo
        '
        Me.txtClaimPaymentTo.AcceptsReturn = True
        Me.txtClaimPaymentTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimPaymentTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimPaymentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimPaymentTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimPaymentTo.Location = New System.Drawing.Point(96, 16)
        Me.txtClaimPaymentTo.MaxLength = 0
        Me.txtClaimPaymentTo.Name = "txtClaimPaymentTo"
        Me.txtClaimPaymentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimPaymentTo.Size = New System.Drawing.Size(161, 21)
        Me.txtClaimPaymentTo.TabIndex = 65
        '
        'cboClaimPaymentTo
        '
        Me.cboClaimPaymentTo.BackColor = System.Drawing.SystemColors.Window
        Me.cboClaimPaymentTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClaimPaymentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClaimPaymentTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClaimPaymentTo.Location = New System.Drawing.Point(96, 16)
        Me.cboClaimPaymentTo.Name = "cboClaimPaymentTo"
        Me.cboClaimPaymentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClaimPaymentTo.Size = New System.Drawing.Size(161, 21)
        Me.cboClaimPaymentTo.TabIndex = 64
        Me.cboClaimPaymentTo.Text = "Combo1"
        '
        'cmdParty
        '
        Me.cmdParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParty.Location = New System.Drawing.Point(824, 16)
        Me.cmdParty.Name = "cmdParty"
        Me.cmdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParty.Size = New System.Drawing.Size(25, 21)
        Me.cmdParty.TabIndex = 63
        Me.cmdParty.Text = "..."
        Me.cmdParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdParty.UseVisualStyleBackColor = False
        '
        'txtParty
        '
        Me.txtParty.AcceptsReturn = True
        Me.txtParty.BackColor = System.Drawing.SystemColors.Window
        Me.txtParty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParty.Location = New System.Drawing.Point(672, 16)
        Me.txtParty.MaxLength = 255
        Me.txtParty.Name = "txtParty"
        Me.txtParty.ReadOnly = True
        Me.txtParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParty.Size = New System.Drawing.Size(152, 21)
        Me.txtParty.TabIndex = 62
        '
        'OptClaimPayable
        '
        Me.OptClaimPayable.BackColor = System.Drawing.SystemColors.Control
        Me.OptClaimPayable.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClaimPayable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClaimPayable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClaimPayable.Location = New System.Drawing.Point(272, 20)
        Me.OptClaimPayable.Name = "OptClaimPayable"
        Me.OptClaimPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClaimPayable.Size = New System.Drawing.Size(105, 13)
        Me.OptClaimPayable.TabIndex = 61
        Me.OptClaimPayable.TabStop = True
        Me.OptClaimPayable.Text = "Claim Payable"
        Me.OptClaimPayable.UseVisualStyleBackColor = False
        '
        'OptAgent
        '
        Me.OptAgent.BackColor = System.Drawing.SystemColors.Control
        Me.OptAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptAgent.Location = New System.Drawing.Point(465, 20)
        Me.OptAgent.Name = "OptAgent"
        Me.OptAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAgent.Size = New System.Drawing.Size(65, 13)
        Me.OptAgent.TabIndex = 60
        Me.OptAgent.TabStop = True
        Me.OptAgent.Text = "Agent"
        Me.OptAgent.UseVisualStyleBackColor = False
        '
        'OptClient
        '
        Me.OptClient.BackColor = System.Drawing.SystemColors.Control
        Me.OptClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClient.Location = New System.Drawing.Point(544, 20)
        Me.OptClient.Name = "OptClient"
        Me.OptClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClient.Size = New System.Drawing.Size(65, 13)
        Me.OptClient.TabIndex = 59
        Me.OptClient.TabStop = True
        Me.OptClient.Text = "Client"
        Me.OptClient.UseVisualStyleBackColor = False
        '
        'OptParty
        '
        Me.OptParty.BackColor = System.Drawing.SystemColors.Control
        Me.OptParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptParty.Location = New System.Drawing.Point(391, 20)
        Me.OptParty.Name = "OptParty"
        Me.OptParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptParty.Size = New System.Drawing.Size(60, 13)
        Me.OptParty.TabIndex = 58
        Me.OptParty.TabStop = True
        Me.OptParty.Text = "Party"
        Me.OptParty.UseVisualStyleBackColor = False
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(624, 20)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(34, 13)
        Me.lblParty.TabIndex = 67
        Me.lblParty.Text = "Party:"
        '
        'lblPaymentTo
        '
        Me.lblPaymentTo.AutoSize = True
        Me.lblPaymentTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentTo.Location = New System.Drawing.Point(16, 20)
        Me.lblPaymentTo.Name = "lblPaymentTo"
        Me.lblPaymentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentTo.Size = New System.Drawing.Size(67, 13)
        Me.lblPaymentTo.TabIndex = 66
        Me.lblPaymentTo.Text = "Payment To:"
        '
        'fraClaimInformation
        '
        Me.fraClaimInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimInformation.Controls.Add(Me.txtRiskType)
        Me.fraClaimInformation.Controls.Add(Me.txtLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.txtLossDate)
        Me.fraClaimInformation.Controls.Add(Me.lblRiskType)
        Me.fraClaimInformation.Controls.Add(Me.lblLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.lblDateOfLoss)
        Me.fraClaimInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimInformation.Location = New System.Drawing.Point(8, 4)
        Me.fraClaimInformation.Name = "fraClaimInformation"
        Me.fraClaimInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimInformation.Size = New System.Drawing.Size(857, 49)
        Me.fraClaimInformation.TabIndex = 68
        Me.fraClaimInformation.TabStop = False
        Me.fraClaimInformation.Text = "Claim Information"
        '
        'txtRiskType
        '
        Me.txtRiskType.AcceptsReturn = True
        Me.txtRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskType.Location = New System.Drawing.Point(88, 16)
        Me.txtRiskType.MaxLength = 0
        Me.txtRiskType.Name = "txtRiskType"
        Me.txtRiskType.ReadOnly = True
        Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskType.Size = New System.Drawing.Size(169, 21)
        Me.txtRiskType.TabIndex = 71
        '
        'txtLossCurrency
        '
        Me.txtLossCurrency.AcceptsReturn = True
        Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrency.Location = New System.Drawing.Point(368, 16)
        Me.txtLossCurrency.MaxLength = 0
        Me.txtLossCurrency.Name = "txtLossCurrency"
        Me.txtLossCurrency.ReadOnly = True
        Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrency.Size = New System.Drawing.Size(177, 21)
        Me.txtLossCurrency.TabIndex = 70
        '
        'txtLossDate
        '
        Me.txtLossDate.AcceptsReturn = True
        Me.txtLossDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossDate.Location = New System.Drawing.Point(656, 16)
        Me.txtLossDate.MaxLength = 0
        Me.txtLossDate.Name = "txtLossDate"
        Me.txtLossDate.ReadOnly = True
        Me.txtLossDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossDate.Size = New System.Drawing.Size(193, 21)
        Me.txtLossDate.TabIndex = 69
        '
        'lblRiskType
        '
        Me.lblRiskType.AutoSize = True
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(16, 20)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(58, 13)
        Me.lblRiskType.TabIndex = 74
        Me.lblRiskType.Text = "Risk Type:"
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.AutoSize = True
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(272, 20)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(77, 13)
        Me.lblLossCurrency.TabIndex = 73
        Me.lblLossCurrency.Text = "Loss Currency:"
        '
        'lblDateOfLoss
        '
        Me.lblDateOfLoss.AutoSize = True
        Me.lblDateOfLoss.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateOfLoss.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOfLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOfLoss.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOfLoss.Location = New System.Drawing.Point(568, 20)
        Me.lblDateOfLoss.Name = "lblDateOfLoss"
        Me.lblDateOfLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOfLoss.Size = New System.Drawing.Size(72, 13)
        Me.lblDateOfLoss.TabIndex = 72
        Me.lblDateOfLoss.Text = "Date Of Loss:"
        '
        'fraSettlement
        '
        Me.fraSettlement.BackColor = System.Drawing.SystemColors.Control
        Me.fraSettlement.Controls.Add(Me.chkSettlement)
        Me.fraSettlement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSettlement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSettlement.Location = New System.Drawing.Point(672, 151)
        Me.fraSettlement.Name = "fraSettlement"
        Me.fraSettlement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSettlement.Size = New System.Drawing.Size(193, 49)
        Me.fraSettlement.TabIndex = 75
        Me.fraSettlement.TabStop = False
        Me.fraSettlement.Text = "Settlement"
        '
        'chkSettlement
        '
        Me.chkSettlement.BackColor = System.Drawing.SystemColors.Control
        Me.chkSettlement.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSettlement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSettlement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSettlement.Location = New System.Drawing.Point(16, 16)
        Me.chkSettlement.Name = "chkSettlement"
        Me.chkSettlement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSettlement.Size = New System.Drawing.Size(105, 25)
        Me.chkSettlement.TabIndex = 76
        Me.chkSettlement.Text = "Settlement"
        Me.chkSettlement.UseVisualStyleBackColor = False
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.fraThisPaymentSummary)
        Me._SSTab1_TabPage1.Controls.Add(Me.fraThisPaymentDetails)
        Me._SSTab1_TabPage1.Controls.Add(Me.fraThisPaymentComments)
        Me._SSTab1_TabPage1.Controls.Add(Me.fraTaxesOnPayments)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(866, 419)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "This Payment"
        '
        'fraThisPaymentSummary
        '
        Me.fraThisPaymentSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraThisPaymentSummary.Controls.Add(Me.txtGrossPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtTotalTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtTotalWHTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtNetPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblGrossPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblTotalTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblTotalWHTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblNetPayment)
        Me.fraThisPaymentSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThisPaymentSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThisPaymentSummary.Location = New System.Drawing.Point(8, 4)
        Me.fraThisPaymentSummary.Name = "fraThisPaymentSummary"
        Me.fraThisPaymentSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThisPaymentSummary.Size = New System.Drawing.Size(321, 153)
        Me.fraThisPaymentSummary.TabIndex = 22
        Me.fraThisPaymentSummary.TabStop = False
        Me.fraThisPaymentSummary.Text = "This Payment Summary"
        '
        'txtGrossPayment
        '
        Me.txtGrossPayment.AcceptsReturn = True
        Me.txtGrossPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtGrossPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGrossPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGrossPayment.Location = New System.Drawing.Point(120, 22)
        Me.txtGrossPayment.MaxLength = 0
        Me.txtGrossPayment.Name = "txtGrossPayment"
        Me.txtGrossPayment.ReadOnly = True
        Me.txtGrossPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGrossPayment.Size = New System.Drawing.Size(185, 21)
        Me.txtGrossPayment.TabIndex = 26
        Me.txtGrossPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalTax
        '
        Me.txtTotalTax.AcceptsReturn = True
        Me.txtTotalTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTax.Location = New System.Drawing.Point(120, 54)
        Me.txtTotalTax.MaxLength = 0
        Me.txtTotalTax.Name = "txtTotalTax"
        Me.txtTotalTax.ReadOnly = True
        Me.txtTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTax.Size = New System.Drawing.Size(185, 21)
        Me.txtTotalTax.TabIndex = 25
        Me.txtTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalWHTax
        '
        Me.txtTotalWHTax.AcceptsReturn = True
        Me.txtTotalWHTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalWHTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalWHTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalWHTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalWHTax.Location = New System.Drawing.Point(120, 87)
        Me.txtTotalWHTax.MaxLength = 0
        Me.txtTotalWHTax.Name = "txtTotalWHTax"
        Me.txtTotalWHTax.ReadOnly = True
        Me.txtTotalWHTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalWHTax.Size = New System.Drawing.Size(185, 21)
        Me.txtTotalWHTax.TabIndex = 24
        Me.txtTotalWHTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNetPayment
        '
        Me.txtNetPayment.AcceptsReturn = True
        Me.txtNetPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNetPayment.Location = New System.Drawing.Point(120, 120)
        Me.txtNetPayment.MaxLength = 0
        Me.txtNetPayment.Name = "txtNetPayment"
        Me.txtNetPayment.ReadOnly = True
        Me.txtNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNetPayment.Size = New System.Drawing.Size(185, 21)
        Me.txtNetPayment.TabIndex = 23
        Me.txtNetPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblGrossPayment
        '
        Me.lblGrossPayment.AutoSize = True
        Me.lblGrossPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossPayment.Location = New System.Drawing.Point(18, 26)
        Me.lblGrossPayment.Name = "lblGrossPayment"
        Me.lblGrossPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossPayment.Size = New System.Drawing.Size(81, 13)
        Me.lblGrossPayment.TabIndex = 30
        Me.lblGrossPayment.Text = "Gross Payment:"
        '
        'lblTotalTax
        '
        Me.lblTotalTax.AutoSize = True
        Me.lblTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalTax.Location = New System.Drawing.Point(52, 58)
        Me.lblTotalTax.Name = "lblTotalTax"
        Me.lblTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalTax.Size = New System.Drawing.Size(55, 13)
        Me.lblTotalTax.TabIndex = 29
        Me.lblTotalTax.Text = "Total Tax:"
        '
        'lblTotalWHTax
        '
        Me.lblTotalWHTax.AutoSize = True
        Me.lblTotalWHTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalWHTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalWHTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalWHTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalWHTax.Location = New System.Drawing.Point(29, 91)
        Me.lblTotalWHTax.Name = "lblTotalWHTax"
        Me.lblTotalWHTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalWHTax.Size = New System.Drawing.Size(77, 13)
        Me.lblTotalWHTax.TabIndex = 28
        Me.lblTotalWHTax.Text = "Total WH Tax:"
        '
        'lblNetPayment
        '
        Me.lblNetPayment.AutoSize = True
        Me.lblNetPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetPayment.Location = New System.Drawing.Point(32, 124)
        Me.lblNetPayment.Name = "lblNetPayment"
        Me.lblNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetPayment.Size = New System.Drawing.Size(71, 13)
        Me.lblNetPayment.TabIndex = 27
        Me.lblNetPayment.Text = "Net Payment:"
        '
        'fraThisPaymentDetails
        '
        Me.fraThisPaymentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraThisPaymentDetails.Controls.Add(Me.txtCountry)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtMediaType)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtPayeeName)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtMediaRef)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtBankName)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtBankCode)
        Me.fraThisPaymentDetails.Controls.Add(Me.cboMediaType)
        Me.fraThisPaymentDetails.Controls.Add(Me.cboCountry)
        Me.fraThisPaymentDetails.Controls.Add(Me.txtBankAccountNo)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblPayeeName)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblMediaRef)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblBankName)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblBankCode)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblMediaType)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblCountry)
        Me.fraThisPaymentDetails.Controls.Add(Me.lblBankAccountNo)
        Me.fraThisPaymentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThisPaymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThisPaymentDetails.Location = New System.Drawing.Point(8, 156)
        Me.fraThisPaymentDetails.Name = "fraThisPaymentDetails"
        Me.fraThisPaymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThisPaymentDetails.Size = New System.Drawing.Size(321, 262)
        Me.fraThisPaymentDetails.TabIndex = 5
        Me.fraThisPaymentDetails.TabStop = False
        Me.fraThisPaymentDetails.Text = "Payment Details"
        '
        'txtCountry
        '
        Me.txtCountry.AcceptsReturn = True
        Me.txtCountry.BackColor = System.Drawing.SystemColors.Window
        Me.txtCountry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCountry.Location = New System.Drawing.Point(120, 192)
        Me.txtCountry.MaxLength = 0
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCountry.Size = New System.Drawing.Size(185, 21)
        Me.txtCountry.TabIndex = 14
        '
        'txtMediaType
        '
        Me.txtMediaType.AcceptsReturn = True
        Me.txtMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaType.Location = New System.Drawing.Point(120, 24)
        Me.txtMediaType.MaxLength = 0
        Me.txtMediaType.Name = "txtMediaType"
        Me.txtMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaType.Size = New System.Drawing.Size(185, 21)
        Me.txtMediaType.TabIndex = 13
        '
        'txtPayeeName
        '
        Me.txtPayeeName.AcceptsReturn = True
        Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeName.Location = New System.Drawing.Point(120, 52)
        Me.txtPayeeName.MaxLength = 255
        Me.txtPayeeName.Name = "txtPayeeName"
        Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeName.Size = New System.Drawing.Size(185, 21)
        Me.txtPayeeName.TabIndex = 12
        '
        'txtMediaRef
        '
        Me.txtMediaRef.AcceptsReturn = True
        Me.txtMediaRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaRef.Location = New System.Drawing.Point(120, 80)
        Me.txtMediaRef.MaxLength = 100
        Me.txtMediaRef.Name = "txtMediaRef"
        Me.txtMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaRef.Size = New System.Drawing.Size(185, 21)
        Me.txtMediaRef.TabIndex = 11
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(120, 108)
        Me.txtBankName.MaxLength = 255
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(185, 21)
        Me.txtBankName.TabIndex = 10
        '
        'txtBankCode
        '
        Me.txtBankCode.AcceptsReturn = True
        Me.txtBankCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankCode.Location = New System.Drawing.Point(120, 164)
        Me.txtBankCode.MaxLength = 8
        Me.txtBankCode.Name = "txtBankCode"
        Me.txtBankCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankCode.Size = New System.Drawing.Size(185, 21)
        Me.txtBankCode.TabIndex = 9
        '
        'cboMediaType
        '
        Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMediaType.Location = New System.Drawing.Point(120, 24)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMediaType.Size = New System.Drawing.Size(185, 21)
        Me.cboMediaType.TabIndex = 8
        Me.cboMediaType.Text = "Combo2"
        '
        'cboCountry
        '
        Me.cboCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cboCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCountry.Location = New System.Drawing.Point(120, 192)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCountry.Size = New System.Drawing.Size(185, 21)
        Me.cboCountry.TabIndex = 7
        Me.cboCountry.Text = "Combo3"
        '
        'txtBankAccountNo
        '
        Me.txtBankAccountNo.AcceptsReturn = True
        Me.txtBankAccountNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAccountNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAccountNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAccountNo.Location = New System.Drawing.Point(120, 136)
        Me.txtBankAccountNo.MaxLength = 30
        Me.txtBankAccountNo.Name = "txtBankAccountNo"
        Me.txtBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAccountNo.Size = New System.Drawing.Size(185, 21)
        Me.txtBankAccountNo.TabIndex = 6
        '
        'lblPayeeName
        '
        Me.lblPayeeName.AutoSize = True
        Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeName.Location = New System.Drawing.Point(38, 56)
        Me.lblPayeeName.Name = "lblPayeeName"
        Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeName.Size = New System.Drawing.Size(71, 13)
        Me.lblPayeeName.TabIndex = 21
        Me.lblPayeeName.Text = "Payee Name:"
        '
        'lblMediaRef
        '
        Me.lblMediaRef.AutoSize = True
        Me.lblMediaRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaRef.Location = New System.Drawing.Point(54, 84)
        Me.lblMediaRef.Name = "lblMediaRef"
        Me.lblMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaRef.Size = New System.Drawing.Size(59, 13)
        Me.lblMediaRef.TabIndex = 20
        Me.lblMediaRef.Text = "Media Ref:"
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(44, 112)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(66, 13)
        Me.lblBankName.TabIndex = 19
        Me.lblBankName.Text = "Bank Name:"
        '
        'lblBankCode
        '
        Me.lblBankCode.AutoSize = True
        Me.lblBankCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankCode.Location = New System.Drawing.Point(47, 168)
        Me.lblBankCode.Name = "lblBankCode"
        Me.lblBankCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankCode.Size = New System.Drawing.Size(63, 13)
        Me.lblBankCode.TabIndex = 18
        Me.lblBankCode.Text = "Bank Code:"
        '
        'lblMediaType
        '
        Me.lblMediaType.AutoSize = True
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(45, 28)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(66, 13)
        Me.lblMediaType.TabIndex = 17
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(64, 196)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(46, 13)
        Me.lblCountry.TabIndex = 16
        Me.lblCountry.Text = "Country:"
        '
        'lblBankAccountNo
        '
        Me.lblBankAccountNo.AutoSize = True
        Me.lblBankAccountNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccountNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccountNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccountNo.Location = New System.Drawing.Point(13, 140)
        Me.lblBankAccountNo.Name = "lblBankAccountNo"
        Me.lblBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccountNo.Size = New System.Drawing.Size(95, 13)
        Me.lblBankAccountNo.TabIndex = 15
        Me.lblBankAccountNo.Text = "Bank Account No:"
        '
        'fraThisPaymentComments
        '
        Me.fraThisPaymentComments.BackColor = System.Drawing.SystemColors.Control
        Me.fraThisPaymentComments.Controls.Add(Me.txtPayeeComments)
        Me.fraThisPaymentComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThisPaymentComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThisPaymentComments.Location = New System.Drawing.Point(336, 156)
        Me.fraThisPaymentComments.Name = "fraThisPaymentComments"
        Me.fraThisPaymentComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThisPaymentComments.Size = New System.Drawing.Size(529, 262)
        Me.fraThisPaymentComments.TabIndex = 3
        Me.fraThisPaymentComments.TabStop = False
        Me.fraThisPaymentComments.Text = "Payment Comments"
        '
        'txtPayeeComments
        '
        Me.txtPayeeComments.AcceptsReturn = True
        Me.txtPayeeComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeComments.Location = New System.Drawing.Point(8, 16)
        Me.txtPayeeComments.MaxLength = 255
        Me.txtPayeeComments.Multiline = True
        Me.txtPayeeComments.Name = "txtPayeeComments"
        Me.txtPayeeComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeComments.Size = New System.Drawing.Size(513, 225)
        Me.txtPayeeComments.TabIndex = 4
        '
        'fraTaxesOnPayments
        '
        Me.fraTaxesOnPayments.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaxesOnPayments.Controls.Add(Me.lvwTaxesOnThisPayment)
        Me.fraTaxesOnPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaxesOnPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaxesOnPayments.Location = New System.Drawing.Point(336, 4)
        Me.fraTaxesOnPayments.Name = "fraTaxesOnPayments"
        Me.fraTaxesOnPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaxesOnPayments.Size = New System.Drawing.Size(529, 153)
        Me.fraTaxesOnPayments.TabIndex = 1
        Me.fraTaxesOnPayments.TabStop = False
        Me.fraTaxesOnPayments.Text = "Taxes On This Payment"
        '
        'lvwTaxesOnThisPayment
        '
        Me.lvwTaxesOnThisPayment.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTaxesOnThisPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTaxesOnThisPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTaxesOnThisPayment.FullRowSelect = True
        Me.lvwTaxesOnThisPayment.GridLines = True
        Me.lvwTaxesOnThisPayment.Location = New System.Drawing.Point(8, 16)
        Me.lvwTaxesOnThisPayment.Name = "lvwTaxesOnThisPayment"
        Me.lvwTaxesOnThisPayment.Size = New System.Drawing.Size(513, 124)
        Me.lvwTaxesOnThisPayment.TabIndex = 2
        Me.lvwTaxesOnThisPayment.UseCompatibleStateImageBehavior = False
        Me.lvwTaxesOnThisPayment.View = System.Windows.Forms.View.Details
        '
        'uctClaimPayment
        '
        Me.Controls.Add(Me.SSTab1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctClaimPayment"
        Me.Size = New System.Drawing.Size(870, 442)
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraPaymentDetails.ResumeLayout(False)
        Me.fraExemptions.ResumeLayout(False)
        Me.fraSafeHarbour.ResumeLayout(False)
        Me.fraSafeHarbour.PerformLayout()
        Me.fraPayeeTaxAdjustments.ResumeLayout(False)
        Me.fraPayeeTaxAdjustments.PerformLayout()
        Me.fraInsuredTaxAdjustment.ResumeLayout(False)
        Me.fraInsuredTaxAdjustment.PerformLayout()
        Me.fraPayee.ResumeLayout(False)
        Me.fraPayee.PerformLayout()
        Me.fraClaimInformation.ResumeLayout(False)
        Me.fraClaimInformation.PerformLayout()
        Me.fraSettlement.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.fraThisPaymentSummary.ResumeLayout(False)
        Me.fraThisPaymentSummary.PerformLayout()
        Me.fraThisPaymentDetails.ResumeLayout(False)
        Me.fraThisPaymentDetails.PerformLayout()
        Me.fraThisPaymentComments.ResumeLayout(False)
        Me.fraTaxesOnPayments.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class