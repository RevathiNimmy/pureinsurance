<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterfaceUW
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
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
	Public WithEvents chkSpreadAcrossInstalment As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeToInstalment As System.Windows.Forms.CheckBox
	Public WithEvents Instalment As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboTaxGroup As System.Windows.Forms.ComboBox
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents fraTax As System.Windows.Forms.GroupBox
	Public WithEvents chkApplyToCreditTransaction As System.Windows.Forms.CheckBox
	Public WithEvents OptRenewal As System.Windows.Forms.RadioButton
	Public WithEvents optAdditionalMTA As System.Windows.Forms.RadioButton
	Public WithEvents OptCancellation As System.Windows.Forms.RadioButton
	Public WithEvents optNewBusiness As System.Windows.Forms.RadioButton
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents OptReInstatement As System.Windows.Forms.RadioButton
	Public WithEvents OptReturnMTA As System.Windows.Forms.RadioButton
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents fraEffectiveTrans As System.Windows.Forms.GroupBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents txtRate As System.Windows.Forms.TextBox
	Public WithEvents OptPercentage As System.Windows.Forms.RadioButton
	Public WithEvents OptAmount As System.Windows.Forms.RadioButton
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblRate As System.Windows.Forms.Label
	Public WithEvents fraFeeAmount As System.Windows.Forms.GroupBox
	Public WithEvents optProduct As System.Windows.Forms.RadioButton
	Public WithEvents optRiskTypeGroup As System.Windows.Forms.RadioButton
	Public WithEvents optPerilGroup As System.Windows.Forms.RadioButton
	Public WithEvents cboRiskTypeGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboProduct As System.Windows.Forms.ComboBox
	Public WithEvents cboPerilGroup As System.Windows.Forms.ComboBox
	Public WithEvents lblFeeAppliesTo As System.Windows.Forms.Label
	Public WithEvents fraFeeAppliesTo As System.Windows.Forms.GroupBox
	Private WithEvents _TabMain_Tab1 As System.Windows.Forms.TabPage
	Public WithEvents TabMain_Tabs As System.Windows.Forms.TabControl.TabPageCollection
	Public WithEvents TabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Instalment = New System.Windows.Forms.GroupBox()
        Me.chkSpreadAcrossInstalment = New System.Windows.Forms.CheckBox()
        Me.chkIncludeToInstalment = New System.Windows.Forms.CheckBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.fraTax = New System.Windows.Forms.GroupBox()
        Me.cboTaxGroup = New System.Windows.Forms.ComboBox()
        Me.lblTaxGroup = New System.Windows.Forms.Label()
        Me.fraEffectiveTrans = New System.Windows.Forms.GroupBox()
        Me.chkOverrideFee = New System.Windows.Forms.CheckBox()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.chkApplyToCreditTransaction = New System.Windows.Forms.CheckBox()
        Me.OptRenewal = New System.Windows.Forms.RadioButton()
        Me.optAdditionalMTA = New System.Windows.Forms.RadioButton()
        Me.OptCancellation = New System.Windows.Forms.RadioButton()
        Me.optNewBusiness = New System.Windows.Forms.RadioButton()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.OptReInstatement = New System.Windows.Forms.RadioButton()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.OptReturnMTA = New System.Windows.Forms.RadioButton()
        Me.fraFeeAmount = New System.Windows.Forms.GroupBox()
        Me.chkUseWhenDeleted = New System.Windows.Forms.CheckBox()
        Me.chkApplyProRated = New System.Windows.Forms.CheckBox()
        Me.cboCurrency = New System.Windows.Forms.ComboBox()
        Me.txtRate = New System.Windows.Forms.TextBox()
        Me.OptPercentage = New System.Windows.Forms.RadioButton()
        Me.OptAmount = New System.Windows.Forms.RadioButton()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.fraFeeAppliesTo = New System.Windows.Forms.GroupBox()
        Me.fraCalculationBasis = New System.Windows.Forms.GroupBox()
        Me.optNetPremiumWithTax = New System.Windows.Forms.RadioButton()
        Me.optNetPremium = New System.Windows.Forms.RadioButton()
        Me.fraPaymentMethod = New System.Windows.Forms.GroupBox()
        Me.cboPaymentTerm = New System.Windows.Forms.ComboBox()
        Me.lblDebitOrderPaymentTerms = New System.Windows.Forms.Label()
        Me.cboMakeLiveOptions = New System.Windows.Forms.ComboBox()
        Me.optProduct = New System.Windows.Forms.RadioButton()
        Me.optRiskTypeGroup = New System.Windows.Forms.RadioButton()
        Me.optPerilGroup = New System.Windows.Forms.RadioButton()
        Me.cboRiskTypeGroup = New System.Windows.Forms.ComboBox()
        Me.cboProduct = New System.Windows.Forms.ComboBox()
        Me.cboPerilGroup = New System.Windows.Forms.ComboBox()
        Me.lblFeeAppliesTo = New System.Windows.Forms.Label()
        Me.TabMain = New System.Windows.Forms.TabControl()
        Me._TabMain_Tab1 = New System.Windows.Forms.TabPage()
        Me.Instalment.SuspendLayout()
        Me.fraTax.SuspendLayout()
        Me.fraEffectiveTrans.SuspendLayout()
        Me.fraFeeAmount.SuspendLayout()
        Me.fraFeeAppliesTo.SuspendLayout()
        Me.fraCalculationBasis.SuspendLayout()
        Me.fraPaymentMethod.SuspendLayout()
        Me.TabMain.SuspendLayout()
        Me._TabMain_Tab1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Instalment
        '
        Me.Instalment.BackColor = System.Drawing.SystemColors.Control
        Me.Instalment.Controls.Add(Me.chkSpreadAcrossInstalment)
        Me.Instalment.Controls.Add(Me.chkIncludeToInstalment)
        Me.Instalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Instalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Instalment.Location = New System.Drawing.Point(411, 316)
        Me.Instalment.Name = "Instalment"
        Me.Instalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Instalment.Size = New System.Drawing.Size(369, 81)
        Me.Instalment.TabIndex = 31
        Me.Instalment.TabStop = False
        Me.Instalment.Text = "Instalment"
        '
        'chkSpreadAcrossInstalment
        '
        Me.chkSpreadAcrossInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpreadAcrossInstalment.Checked = True
        Me.chkSpreadAcrossInstalment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSpreadAcrossInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpreadAcrossInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpreadAcrossInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpreadAcrossInstalment.Location = New System.Drawing.Point(24, 48)
        Me.chkSpreadAcrossInstalment.Name = "chkSpreadAcrossInstalment"
        Me.chkSpreadAcrossInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpreadAcrossInstalment.Size = New System.Drawing.Size(334, 25)
        Me.chkSpreadAcrossInstalment.TabIndex = 33
        Me.chkSpreadAcrossInstalment.Text = "Is Spread the Fee across Instalment"
        Me.chkSpreadAcrossInstalment.UseVisualStyleBackColor = False
        '
        'chkIncludeToInstalment
        '
        Me.chkIncludeToInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeToInstalment.Checked = True
        Me.chkIncludeToInstalment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncludeToInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeToInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeToInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeToInstalment.Location = New System.Drawing.Point(24, 16)
        Me.chkIncludeToInstalment.Name = "chkIncludeToInstalment"
        Me.chkIncludeToInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeToInstalment.Size = New System.Drawing.Size(281, 25)
        Me.chkIncludeToInstalment.TabIndex = 32
        Me.chkIncludeToInstalment.Text = "Is Include Fee in Instalment"
        Me.chkIncludeToInstalment.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(701, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 23
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(620, 440)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 22
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'fraTax
        '
        Me.fraTax.BackColor = System.Drawing.SystemColors.Control
        Me.fraTax.Controls.Add(Me.cboTaxGroup)
        Me.fraTax.Controls.Add(Me.lblTaxGroup)
        Me.fraTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTax.Location = New System.Drawing.Point(6, 316)
        Me.fraTax.Name = "fraTax"
        Me.fraTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTax.Size = New System.Drawing.Size(401, 81)
        Me.fraTax.TabIndex = 28
        Me.fraTax.TabStop = False
        Me.fraTax.Text = "Tax"
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxGroup.Location = New System.Drawing.Point(104, 22)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxGroup.Size = New System.Drawing.Size(249, 21)
        Me.cboTaxGroup.TabIndex = 21
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(24, 24)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(81, 17)
        Me.lblTaxGroup.TabIndex = 20
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'fraEffectiveTrans
        '
        Me.fraEffectiveTrans.BackColor = System.Drawing.SystemColors.Control
        Me.fraEffectiveTrans.Controls.Add(Me.chkOverrideFee)
        Me.fraEffectiveTrans.Controls.Add(Me.optAll)
        Me.fraEffectiveTrans.Controls.Add(Me.chkApplyToCreditTransaction)
        Me.fraEffectiveTrans.Controls.Add(Me.OptRenewal)
        Me.fraEffectiveTrans.Controls.Add(Me.optAdditionalMTA)
        Me.fraEffectiveTrans.Controls.Add(Me.OptCancellation)
        Me.fraEffectiveTrans.Controls.Add(Me.optNewBusiness)
        Me.fraEffectiveTrans.Controls.Add(Me.txtEffectiveDate)
        Me.fraEffectiveTrans.Controls.Add(Me.OptReInstatement)
        Me.fraEffectiveTrans.Controls.Add(Me.lblEffectiveDate)
        Me.fraEffectiveTrans.Controls.Add(Me.OptReturnMTA)
        Me.fraEffectiveTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEffectiveTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEffectiveTrans.Location = New System.Drawing.Point(6, 229)
        Me.fraEffectiveTrans.Name = "fraEffectiveTrans"
        Me.fraEffectiveTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEffectiveTrans.Size = New System.Drawing.Size(776, 86)
        Me.fraEffectiveTrans.TabIndex = 27
        Me.fraEffectiveTrans.TabStop = False
        Me.fraEffectiveTrans.Text = "Effective Transactions"
        '
        'chkOverrideFee
        '
        Me.chkOverrideFee.AutoSize = True
        Me.chkOverrideFee.Location = New System.Drawing.Point(583, 56)
        Me.chkOverrideFee.Name = "chkOverrideFee"
        Me.chkOverrideFee.Size = New System.Drawing.Size(127, 17)
        Me.chkOverrideFee.TabIndex = 21
        Me.chkOverrideFee.Text = "Override rate/amount"
        Me.chkOverrideFee.UseVisualStyleBackColor = True
        '
        'optAll
        '
        Me.optAll.BackColor = System.Drawing.SystemColors.Control
        Me.optAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAll.Location = New System.Drawing.Point(13, 28)
        Me.optAll.Name = "optAll"
        Me.optAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAll.Size = New System.Drawing.Size(114, 21)
        Me.optAll.TabIndex = 20
        Me.optAll.TabStop = True
        Me.optAll.Text = "All"
        Me.optAll.UseVisualStyleBackColor = False
        '
        'chkApplyToCreditTransaction
        '
        Me.chkApplyToCreditTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.chkApplyToCreditTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkApplyToCreditTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApplyToCreditTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkApplyToCreditTransaction.Location = New System.Drawing.Point(407, 56)
        Me.chkApplyToCreditTransaction.Name = "chkApplyToCreditTransaction"
        Me.chkApplyToCreditTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkApplyToCreditTransaction.Size = New System.Drawing.Size(193, 17)
        Me.chkApplyToCreditTransaction.TabIndex = 19
        Me.chkApplyToCreditTransaction.Text = "Apply to credit transaction"
        Me.chkApplyToCreditTransaction.UseVisualStyleBackColor = False
        '
        'OptRenewal
        '
        Me.OptRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.OptRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptRenewal.Location = New System.Drawing.Point(13, 56)
        Me.OptRenewal.Name = "OptRenewal"
        Me.OptRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptRenewal.Size = New System.Drawing.Size(114, 21)
        Me.OptRenewal.TabIndex = 14
        Me.OptRenewal.TabStop = True
        Me.OptRenewal.Text = "Renewal"
        Me.OptRenewal.UseVisualStyleBackColor = False
        '
        'optAdditionalMTA
        '
        Me.optAdditionalMTA.BackColor = System.Drawing.SystemColors.Control
        Me.optAdditionalMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAdditionalMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAdditionalMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAdditionalMTA.Location = New System.Drawing.Point(407, 23)
        Me.optAdditionalMTA.Name = "optAdditionalMTA"
        Me.optAdditionalMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAdditionalMTA.Size = New System.Drawing.Size(114, 21)
        Me.optAdditionalMTA.TabIndex = 13
        Me.optAdditionalMTA.TabStop = True
        Me.optAdditionalMTA.Text = "Additional MTA"
        Me.optAdditionalMTA.UseVisualStyleBackColor = False
        '
        'OptCancellation
        '
        Me.OptCancellation.BackColor = System.Drawing.SystemColors.Control
        Me.OptCancellation.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptCancellation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptCancellation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptCancellation.Location = New System.Drawing.Point(285, 24)
        Me.OptCancellation.Name = "OptCancellation"
        Me.OptCancellation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptCancellation.Size = New System.Drawing.Size(114, 21)
        Me.OptCancellation.TabIndex = 12
        Me.OptCancellation.TabStop = True
        Me.OptCancellation.Text = "Cancellation"
        Me.OptCancellation.UseVisualStyleBackColor = False
        '
        'optNewBusiness
        '
        Me.optNewBusiness.BackColor = System.Drawing.SystemColors.Control
        Me.optNewBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNewBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNewBusiness.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNewBusiness.Location = New System.Drawing.Point(133, 28)
        Me.optNewBusiness.Name = "optNewBusiness"
        Me.optNewBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNewBusiness.Size = New System.Drawing.Size(114, 21)
        Me.optNewBusiness.TabIndex = 11
        Me.optNewBusiness.TabStop = True
        Me.optNewBusiness.Text = "New Business"
        Me.optNewBusiness.UseVisualStyleBackColor = False
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(684, 24)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(81, 20)
        Me.txtEffectiveDate.TabIndex = 18
        '
        'OptReInstatement
        '
        Me.OptReInstatement.BackColor = System.Drawing.SystemColors.Control
        Me.OptReInstatement.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptReInstatement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptReInstatement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptReInstatement.Location = New System.Drawing.Point(133, 56)
        Me.OptReInstatement.Name = "OptReInstatement"
        Me.OptReInstatement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptReInstatement.Size = New System.Drawing.Size(114, 21)
        Me.OptReInstatement.TabIndex = 15
        Me.OptReInstatement.TabStop = True
        Me.OptReInstatement.Text = "Re-Instatement"
        Me.OptReInstatement.UseVisualStyleBackColor = False
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(580, 28)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(103, 13)
        Me.lblEffectiveDate.TabIndex = 17
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'OptReturnMTA
        '
        Me.OptReturnMTA.BackColor = System.Drawing.SystemColors.Control
        Me.OptReturnMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptReturnMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptReturnMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptReturnMTA.Location = New System.Drawing.Point(285, 53)
        Me.OptReturnMTA.Name = "OptReturnMTA"
        Me.OptReturnMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptReturnMTA.Size = New System.Drawing.Size(114, 21)
        Me.OptReturnMTA.TabIndex = 16
        Me.OptReturnMTA.TabStop = True
        Me.OptReturnMTA.Text = "Return MTA"
        Me.OptReturnMTA.UseVisualStyleBackColor = False
        '
        'fraFeeAmount
        '
        Me.fraFeeAmount.BackColor = System.Drawing.SystemColors.Control
        Me.fraFeeAmount.Controls.Add(Me.chkUseWhenDeleted)
        Me.fraFeeAmount.Controls.Add(Me.chkApplyProRated)
        Me.fraFeeAmount.Controls.Add(Me.cboCurrency)
        Me.fraFeeAmount.Controls.Add(Me.txtRate)
        Me.fraFeeAmount.Controls.Add(Me.OptPercentage)
        Me.fraFeeAmount.Controls.Add(Me.OptAmount)
        Me.fraFeeAmount.Controls.Add(Me.lblCurrency)
        Me.fraFeeAmount.Controls.Add(Me.lblRate)
        Me.fraFeeAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFeeAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFeeAmount.Location = New System.Drawing.Point(4, 142)
        Me.fraFeeAmount.Name = "fraFeeAmount"
        Me.fraFeeAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFeeAmount.Size = New System.Drawing.Size(776, 81)
        Me.fraFeeAmount.TabIndex = 26
        Me.fraFeeAmount.TabStop = False
        Me.fraFeeAmount.Text = "Fee amount"
        '
        'chkUseWhenDeleted
        '
        Me.chkUseWhenDeleted.AutoSize = True
        Me.chkUseWhenDeleted.Checked = True
        Me.chkUseWhenDeleted.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseWhenDeleted.Location = New System.Drawing.Point(434, 28)
        Me.chkUseWhenDeleted.Name = "chkUseWhenDeleted"
        Me.chkUseWhenDeleted.Size = New System.Drawing.Size(333, 17)
        Me.chkUseWhenDeleted.TabIndex = 12
        Me.chkUseWhenDeleted.Text = "Do you want to apply the fee to OOS transactions when deleted?"
        Me.chkUseWhenDeleted.UseVisualStyleBackColor = True
        '
        'chkApplyProRated
        '
        Me.chkApplyProRated.AutoSize = True
        Me.chkApplyProRated.Location = New System.Drawing.Point(240, 25)
        Me.chkApplyProRated.Name = "chkApplyProRated"
        Me.chkApplyProRated.Size = New System.Drawing.Size(104, 17)
        Me.chkApplyProRated.TabIndex = 11
        Me.chkApplyProRated.Text = "Apply Pro-rated?"
        Me.chkApplyProRated.UseVisualStyleBackColor = True
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(240, 48)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(193, 21)
        Me.cboCurrency.TabIndex = 10
        '
        'txtRate
        '
        Me.txtRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate.Location = New System.Drawing.Point(64, 48)
        Me.txtRate.MaxLength = 9
        Me.txtRate.Name = "txtRate"
        Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate.Size = New System.Drawing.Size(81, 20)
        Me.txtRate.TabIndex = 8
        '
        'OptPercentage
        '
        Me.OptPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.OptPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptPercentage.Location = New System.Drawing.Point(24, 24)
        Me.OptPercentage.Name = "OptPercentage"
        Me.OptPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptPercentage.Size = New System.Drawing.Size(105, 21)
        Me.OptPercentage.TabIndex = 5
        Me.OptPercentage.TabStop = True
        Me.OptPercentage.Text = "Percentage"
        Me.OptPercentage.UseVisualStyleBackColor = False
        '
        'OptAmount
        '
        Me.OptAmount.BackColor = System.Drawing.SystemColors.Control
        Me.OptAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptAmount.Location = New System.Drawing.Point(136, 24)
        Me.OptAmount.Name = "OptAmount"
        Me.OptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAmount.Size = New System.Drawing.Size(65, 21)
        Me.OptAmount.TabIndex = 6
        Me.OptAmount.TabStop = True
        Me.OptAmount.Text = "Value"
        Me.OptAmount.UseVisualStyleBackColor = False
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(168, 52)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(70, 13)
        Me.lblCurrency.TabIndex = 9
        Me.lblCurrency.Text = "Currency:"
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(24, 52)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(40, 13)
        Me.lblRate.TabIndex = 7
        Me.lblRate.Text = "Rate:"
        '
        'fraFeeAppliesTo
        '
        Me.fraFeeAppliesTo.BackColor = System.Drawing.SystemColors.Control
        Me.fraFeeAppliesTo.Controls.Add(Me.fraCalculationBasis)
        Me.fraFeeAppliesTo.Controls.Add(Me.fraPaymentMethod)
        Me.fraFeeAppliesTo.Controls.Add(Me.optProduct)
        Me.fraFeeAppliesTo.Controls.Add(Me.optRiskTypeGroup)
        Me.fraFeeAppliesTo.Controls.Add(Me.optPerilGroup)
        Me.fraFeeAppliesTo.Controls.Add(Me.cboRiskTypeGroup)
        Me.fraFeeAppliesTo.Controls.Add(Me.cboProduct)
        Me.fraFeeAppliesTo.Controls.Add(Me.cboPerilGroup)
        Me.fraFeeAppliesTo.Controls.Add(Me.lblFeeAppliesTo)
        Me.fraFeeAppliesTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFeeAppliesTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFeeAppliesTo.Location = New System.Drawing.Point(8, 23)
        Me.fraFeeAppliesTo.Name = "fraFeeAppliesTo"
        Me.fraFeeAppliesTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFeeAppliesTo.Size = New System.Drawing.Size(776, 136)
        Me.fraFeeAppliesTo.TabIndex = 25
        Me.fraFeeAppliesTo.TabStop = False
        Me.fraFeeAppliesTo.Text = "Fee applies to"
        '
        'fraCalculationBasis
        '
        Me.fraCalculationBasis.Controls.Add(Me.optNetPremiumWithTax)
        Me.fraCalculationBasis.Controls.Add(Me.optNetPremium)
        Me.fraCalculationBasis.Location = New System.Drawing.Point(27, 75)
        Me.fraCalculationBasis.Name = "fraCalculationBasis"
        Me.fraCalculationBasis.Size = New System.Drawing.Size(422, 52)
        Me.fraCalculationBasis.TabIndex = 32
        Me.fraCalculationBasis.TabStop = False
        Me.fraCalculationBasis.Text = "CalculationBasis"
        '
        'optNetPremiumWithTax
        '
        Me.optNetPremiumWithTax.AutoSize = True
        Me.optNetPremiumWithTax.Location = New System.Drawing.Point(172, 24)
        Me.optNetPremiumWithTax.Name = "optNetPremiumWithTax"
        Me.optNetPremiumWithTax.Size = New System.Drawing.Size(178, 17)
        Me.optNetPremiumWithTax.TabIndex = 1
        Me.optNetPremiumWithTax.TabStop = True
        Me.optNetPremiumWithTax.Text = "Net Premium + Tax On Premium "
        Me.optNetPremiumWithTax.UseVisualStyleBackColor = True
        '
        'optNetPremium
        '
        Me.optNetPremium.AutoSize = True
        Me.optNetPremium.Location = New System.Drawing.Point(11, 24)
        Me.optNetPremium.Name = "optNetPremium"
        Me.optNetPremium.Size = New System.Drawing.Size(85, 17)
        Me.optNetPremium.TabIndex = 0
        Me.optNetPremium.TabStop = True
        Me.optNetPremium.Text = "Net Premium"
        Me.optNetPremium.UseVisualStyleBackColor = True
        '
        'fraPaymentMethod
        '
        Me.fraPaymentMethod.Controls.Add(Me.cboPaymentTerm)
        Me.fraPaymentMethod.Controls.Add(Me.lblDebitOrderPaymentTerms)
        Me.fraPaymentMethod.Controls.Add(Me.cboMakeLiveOptions)
        Me.fraPaymentMethod.Location = New System.Drawing.Point(465, 19)
        Me.fraPaymentMethod.Name = "fraPaymentMethod"
        Me.fraPaymentMethod.Size = New System.Drawing.Size(300, 108)
        Me.fraPaymentMethod.TabIndex = 31
        Me.fraPaymentMethod.TabStop = False
        Me.fraPaymentMethod.Text = "Payment Method"
        '
        'cboPaymentTerm
        '
        Me.cboPaymentTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentTerm.FormattingEnabled = True
        Me.cboPaymentTerm.Location = New System.Drawing.Point(6, 74)
        Me.cboPaymentTerm.Name = "cboPaymentTerm"
        Me.cboPaymentTerm.Size = New System.Drawing.Size(276, 21)
        Me.cboPaymentTerm.TabIndex = 2
        '
        'lblDebitOrderPaymentTerms
        '
        Me.lblDebitOrderPaymentTerms.AutoSize = True
        Me.lblDebitOrderPaymentTerms.Location = New System.Drawing.Point(6, 50)
        Me.lblDebitOrderPaymentTerms.Name = "lblDebitOrderPaymentTerms"
        Me.lblDebitOrderPaymentTerms.Size = New System.Drawing.Size(140, 13)
        Me.lblDebitOrderPaymentTerms.TabIndex = 1
        Me.lblDebitOrderPaymentTerms.Text = "Debit Order Payment Terms:"
        '
        'cboMakeLiveOptions
        '
        Me.cboMakeLiveOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMakeLiveOptions.FormattingEnabled = True
        Me.cboMakeLiveOptions.Location = New System.Drawing.Point(7, 20)
        Me.cboMakeLiveOptions.Name = "cboMakeLiveOptions"
        Me.cboMakeLiveOptions.Size = New System.Drawing.Size(275, 21)
        Me.cboMakeLiveOptions.TabIndex = 0
        '
        'optProduct
        '
        Me.optProduct.BackColor = System.Drawing.SystemColors.Control
        Me.optProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.optProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optProduct.Location = New System.Drawing.Point(24, 24)
        Me.optProduct.Name = "optProduct"
        Me.optProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optProduct.Size = New System.Drawing.Size(73, 21)
        Me.optProduct.TabIndex = 0
        Me.optProduct.TabStop = True
        Me.optProduct.Text = "Product"
        Me.optProduct.UseVisualStyleBackColor = False
        '
        'optRiskTypeGroup
        '
        Me.optRiskTypeGroup.BackColor = System.Drawing.SystemColors.Control
        Me.optRiskTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.optRiskTypeGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optRiskTypeGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRiskTypeGroup.Location = New System.Drawing.Point(120, 24)
        Me.optRiskTypeGroup.Name = "optRiskTypeGroup"
        Me.optRiskTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optRiskTypeGroup.Size = New System.Drawing.Size(121, 21)
        Me.optRiskTypeGroup.TabIndex = 1
        Me.optRiskTypeGroup.TabStop = True
        Me.optRiskTypeGroup.Text = "Risk Type Group"
        Me.optRiskTypeGroup.UseVisualStyleBackColor = False
        '
        'optPerilGroup
        '
        Me.optPerilGroup.BackColor = System.Drawing.SystemColors.Control
        Me.optPerilGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPerilGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPerilGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPerilGroup.Location = New System.Drawing.Point(264, 24)
        Me.optPerilGroup.Name = "optPerilGroup"
        Me.optPerilGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPerilGroup.Size = New System.Drawing.Size(90, 21)
        Me.optPerilGroup.TabIndex = 2
        Me.optPerilGroup.TabStop = True
        Me.optPerilGroup.Text = "Peril Group"
        Me.optPerilGroup.UseVisualStyleBackColor = False
        '
        'cboRiskTypeGroup
        '
        Me.cboRiskTypeGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboRiskTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRiskTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRiskTypeGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRiskTypeGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRiskTypeGroup.Location = New System.Drawing.Point(200, 48)
        Me.cboRiskTypeGroup.Name = "cboRiskTypeGroup"
        Me.cboRiskTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRiskTypeGroup.Size = New System.Drawing.Size(249, 21)
        Me.cboRiskTypeGroup.TabIndex = 29
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(200, 48)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(249, 21)
        Me.cboProduct.TabIndex = 4
        '
        'cboPerilGroup
        '
        Me.cboPerilGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboPerilGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPerilGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPerilGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPerilGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPerilGroup.Location = New System.Drawing.Point(200, 48)
        Me.cboPerilGroup.Name = "cboPerilGroup"
        Me.cboPerilGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPerilGroup.Size = New System.Drawing.Size(249, 21)
        Me.cboPerilGroup.TabIndex = 30
        '
        'lblFeeAppliesTo
        '
        Me.lblFeeAppliesTo.AutoSize = True
        Me.lblFeeAppliesTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeAppliesTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeAppliesTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeAppliesTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeAppliesTo.Location = New System.Drawing.Point(24, 52)
        Me.lblFeeAppliesTo.Name = "lblFeeAppliesTo"
        Me.lblFeeAppliesTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeAppliesTo.Size = New System.Drawing.Size(177, 13)
        Me.lblFeeAppliesTo.TabIndex = 3
        Me.lblFeeAppliesTo.Text = "Product/Risk/Peril Group:"
        '
        'TabMain
        '
        Me.TabMain.Controls.Add(Me._TabMain_Tab1)
        Me.TabMain.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabMain.Location = New System.Drawing.Point(0, 0)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(793, 433)
        Me.TabMain.TabIndex = 24
        '
        '_TabMain_Tab1
        '
        Me._TabMain_Tab1.Controls.Add(Me.Instalment)
        Me._TabMain_Tab1.Controls.Add(Me.fraTax)
        Me._TabMain_Tab1.Controls.Add(Me.fraFeeAmount)
        Me._TabMain_Tab1.Controls.Add(Me.fraEffectiveTrans)
        Me._TabMain_Tab1.Location = New System.Drawing.Point(4, 23)
        Me._TabMain_Tab1.Name = "_TabMain_Tab1"
        Me._TabMain_Tab1.Size = New System.Drawing.Size(785, 406)
        Me._TabMain_Tab1.TabIndex = 0
        Me._TabMain_Tab1.Text = "1 - Fee Definition"
        '
        'frmInterfaceUW
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(796, 474)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraFeeAppliesTo)
        Me.Controls.Add(Me.TabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterfaceUW"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fee Maintenance"
        Me.Instalment.ResumeLayout(False)
        Me.fraTax.ResumeLayout(False)
        Me.fraEffectiveTrans.ResumeLayout(False)
        Me.fraEffectiveTrans.PerformLayout()
        Me.fraFeeAmount.ResumeLayout(False)
        Me.fraFeeAmount.PerformLayout()
        Me.fraFeeAppliesTo.ResumeLayout(False)
        Me.fraFeeAppliesTo.PerformLayout()
        Me.fraCalculationBasis.ResumeLayout(False)
        Me.fraCalculationBasis.PerformLayout()
        Me.fraPaymentMethod.ResumeLayout(False)
        Me.fraPaymentMethod.PerformLayout()
        Me.TabMain.ResumeLayout(False)
        Me._TabMain_Tab1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraPaymentMethod As System.Windows.Forms.GroupBox
    Friend WithEvents cboPaymentTerm As System.Windows.Forms.ComboBox
    Friend WithEvents lblDebitOrderPaymentTerms As System.Windows.Forms.Label
    Friend WithEvents cboMakeLiveOptions As System.Windows.Forms.ComboBox
    Friend WithEvents fraCalculationBasis As System.Windows.Forms.GroupBox
    Friend WithEvents optNetPremiumWithTax As System.Windows.Forms.RadioButton
    Friend WithEvents optNetPremium As System.Windows.Forms.RadioButton
    Public WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkApplyProRated As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideFee As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseWhenDeleted As System.Windows.Forms.CheckBox
#End Region
End Class