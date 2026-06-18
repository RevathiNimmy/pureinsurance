<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMPayment1
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		SSTab1PreviousTab = SSTab1.SelectedIndex
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			UserControl_Terminate()
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents chkSettlement As System.Windows.Forms.CheckBox
	Friend WithEvents fraSettlement As System.Windows.Forms.GroupBox
	Friend WithEvents txtLossDate As System.Windows.Forms.TextBox
	Friend WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtRiskType As System.Windows.Forms.TextBox
	Friend WithEvents lblDateOfLoss As System.Windows.Forms.Label
	Friend WithEvents lblLossCurrency As System.Windows.Forms.Label
	Friend WithEvents lblRiskType As System.Windows.Forms.Label
	Friend WithEvents fraClaimInformation As System.Windows.Forms.GroupBox
	Friend WithEvents OptParty As System.Windows.Forms.RadioButton
	Friend WithEvents OptClient As System.Windows.Forms.RadioButton
	Friend WithEvents OptAgent As System.Windows.Forms.RadioButton
	Friend WithEvents OptClaimPayable As System.Windows.Forms.RadioButton
	Friend WithEvents txtParty As System.Windows.Forms.TextBox
	Friend WithEvents cmdParty As System.Windows.Forms.Button
	Friend WithEvents cboClaimPaymentTo As System.Windows.Forms.ComboBox
	Friend WithEvents txtClaimPaymentTo As System.Windows.Forms.TextBox
	Friend WithEvents lblPaymentTo As System.Windows.Forms.Label
	Friend WithEvents lblParty As System.Windows.Forms.Label
	Friend WithEvents fraPayee As System.Windows.Forms.GroupBox
	Friend WithEvents txtITTaxNo As System.Windows.Forms.TextBox
	Friend WithEvents txtITPercentage As System.Windows.Forms.TextBox
	Friend WithEvents chkITDomiciled As System.Windows.Forms.CheckBox
	Friend WithEvents lblITTaxNo As System.Windows.Forms.Label
	Friend WithEvents lblITPercentage As System.Windows.Forms.Label
	Friend WithEvents fraInsuredTaxAdjustment As System.Windows.Forms.GroupBox
	Friend WithEvents txtPTTaxNo As System.Windows.Forms.TextBox
	Friend WithEvents txtPTPercentage As System.Windows.Forms.TextBox
	Friend WithEvents chkPTDomiciled As System.Windows.Forms.CheckBox
	Friend WithEvents lblPTTaxNo As System.Windows.Forms.Label
	Friend WithEvents lblPTPercentage As System.Windows.Forms.Label
	Friend WithEvents fraPayeeTaxAdjustments As System.Windows.Forms.GroupBox
	Friend WithEvents txtSFPercentage As System.Windows.Forms.TextBox
	Friend WithEvents cboSafeHarbour As System.Windows.Forms.ComboBox
	Friend WithEvents txtSafeHarbour As System.Windows.Forms.TextBox
	Friend WithEvents lblSFPercentage As System.Windows.Forms.Label
	Friend WithEvents lblAgreement As System.Windows.Forms.Label
	Friend WithEvents fraSafeHarbour As System.Windows.Forms.GroupBox
	Friend WithEvents chkWHTExempt As System.Windows.Forms.CheckBox
	Friend WithEvents chkTaxExempt As System.Windows.Forms.CheckBox
	Friend WithEvents fraExemptions As System.Windows.Forms.GroupBox
	Friend WithEvents cmdPaymentLock As System.Windows.Forms.Button
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents cmdReserveEdit As System.Windows.Forms.Button
	Friend WithEvents cmdEditPayee As System.Windows.Forms.Button
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents lvwPaymentDetails As System.Windows.Forms.ListView
	Friend WithEvents cmdHistory As System.Windows.Forms.Button
	Friend WithEvents fraPaymentDetails As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents txtNetPayment As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalWHTax As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtGrossPayment As System.Windows.Forms.TextBox
	Friend WithEvents lblNetPayment As System.Windows.Forms.Label
	Friend WithEvents lblTotalWHTax As System.Windows.Forms.Label
	Friend WithEvents lblTotalTax As System.Windows.Forms.Label
	Friend WithEvents lblGrossPayment As System.Windows.Forms.Label
	Friend WithEvents fraThisPaymentSummary As System.Windows.Forms.GroupBox
	Friend WithEvents lblMediaRef As System.Windows.Forms.Label
	Friend WithEvents lblChequeDate As System.Windows.Forms.Label
	Friend WithEvents lblThirdPartyReference As System.Windows.Forms.Label
	Friend WithEvents lblBankAccountNo As System.Windows.Forms.Label
	Friend WithEvents lblMediaType As System.Windows.Forms.Label
	Friend WithEvents lblBankCode As System.Windows.Forms.Label
	Friend WithEvents lblBankName As System.Windows.Forms.Label
	Friend WithEvents lblPayeeName As System.Windows.Forms.Label
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents lblOurReference As System.Windows.Forms.Label
    Friend WithEvents dtpChequeDate As System.Windows.Forms.DateTimePicker
	Friend WithEvents uctPMAddressControl1 As PMAddressControl.uctPMAddressControl
	Friend WithEvents txtMediaRef As System.Windows.Forms.TextBox
	Friend WithEvents txtThirdPartyReference As System.Windows.Forms.TextBox
	Friend WithEvents txtBankAccountNo As System.Windows.Forms.TextBox
	Friend WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Friend WithEvents txtBankCode As System.Windows.Forms.TextBox
	Friend WithEvents txtBankName As System.Windows.Forms.TextBox
	Friend WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Friend WithEvents uctPartyBankCombo1 As uctPartyBank.uctPartyBankCombo
	Friend WithEvents txtOurReference As System.Windows.Forms.TextBox
	Friend WithEvents _SSTab2_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents txtPayeeComments As System.Windows.Forms.TextBox
	Friend WithEvents fraThisPaymentComments As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab2_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents SSTab2 As System.Windows.Forms.TabControl
	Friend WithEvents lvwTaxesOnThisPayment As System.Windows.Forms.ListView
	Friend WithEvents fraTaxesOnPayments As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents txtFormatPercentage As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalAllocated As System.Windows.Forms.TextBox
	Friend WithEvents txtPercentageAllocated As System.Windows.Forms.TextBox
	Friend WithEvents _lvwCoinsurers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurers_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwCoinsurers As System.Windows.Forms.ListView
	Friend WithEvents lblTotalAllocated As System.Windows.Forms.Label
	Friend WithEvents lblPercentageAllocated As System.Windows.Forms.Label
	Friend WithEvents fraCoinsurers As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
	Friend WithEvents SSTab1 As System.Windows.Forms.TabControl
    Private SSTab1PreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl()
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraSettlement = New System.Windows.Forms.GroupBox()
        Me.chkSettlement = New System.Windows.Forms.CheckBox()
        Me.fraClaimInformation = New System.Windows.Forms.GroupBox()
        Me.txtLossDate = New System.Windows.Forms.TextBox()
        Me.txtLossCurrency = New System.Windows.Forms.TextBox()
        Me.txtRiskType = New System.Windows.Forms.TextBox()
        Me.lblDateOfLoss = New System.Windows.Forms.Label()
        Me.lblLossCurrency = New System.Windows.Forms.Label()
        Me.lblRiskType = New System.Windows.Forms.Label()
        Me.fraPayee = New System.Windows.Forms.GroupBox()
        Me.chkIsExGratia = New System.Windows.Forms.CheckBox()
        Me.OptParty = New System.Windows.Forms.RadioButton()
        Me.OptClient = New System.Windows.Forms.RadioButton()
        Me.OptAgent = New System.Windows.Forms.RadioButton()
        Me.OptClaimPayable = New System.Windows.Forms.RadioButton()
        Me.txtParty = New System.Windows.Forms.TextBox()
        Me.cmdParty = New System.Windows.Forms.Button()
        Me.cboClaimPaymentTo = New System.Windows.Forms.ComboBox()
        Me.txtClaimPaymentTo = New System.Windows.Forms.TextBox()
        Me.lblPaymentTo = New System.Windows.Forms.Label()
        Me.lblParty = New System.Windows.Forms.Label()
        Me.fraInsuredTaxAdjustment = New System.Windows.Forms.GroupBox()
        Me.txtITTaxNo = New System.Windows.Forms.TextBox()
        Me.txtITPercentage = New System.Windows.Forms.TextBox()
        Me.chkITDomiciled = New System.Windows.Forms.CheckBox()
        Me.lblITTaxNo = New System.Windows.Forms.Label()
        Me.lblITPercentage = New System.Windows.Forms.Label()
        Me.fraPayeeTaxAdjustments = New System.Windows.Forms.GroupBox()
        Me.txtPTTaxNo = New System.Windows.Forms.TextBox()
        Me.txtPTPercentage = New System.Windows.Forms.TextBox()
        Me.chkPTDomiciled = New System.Windows.Forms.CheckBox()
        Me.lblPTTaxNo = New System.Windows.Forms.Label()
        Me.lblPTPercentage = New System.Windows.Forms.Label()
        Me.fraSafeHarbour = New System.Windows.Forms.GroupBox()
        Me.txtSFPercentage = New System.Windows.Forms.TextBox()
        Me.cboSafeHarbour = New System.Windows.Forms.ComboBox()
        Me.txtSafeHarbour = New System.Windows.Forms.TextBox()
        Me.lblSFPercentage = New System.Windows.Forms.Label()
        Me.lblAgreement = New System.Windows.Forms.Label()
        Me.fraExemptions = New System.Windows.Forms.GroupBox()
        Me.chkWHTExempt = New System.Windows.Forms.CheckBox()
        Me.chkTaxExempt = New System.Windows.Forms.CheckBox()
        Me.fraPaymentDetails = New System.Windows.Forms.GroupBox()
        Me.cmdPaymentLock = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdReserveEdit = New System.Windows.Forms.Button()
        Me.cmdEditPayee = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.lvwPaymentDetails = New System.Windows.Forms.ListView()
        Me.cmdHistory = New System.Windows.Forms.Button()
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraThisPaymentSummary = New System.Windows.Forms.GroupBox()
        Me.txtNetPayment = New System.Windows.Forms.TextBox()
        Me.txtTotalWHTax = New System.Windows.Forms.TextBox()
        Me.txtTotalTax = New System.Windows.Forms.TextBox()
        Me.txtGrossPayment = New System.Windows.Forms.TextBox()
        Me.lblNetPayment = New System.Windows.Forms.Label()
        Me.lblTotalWHTax = New System.Windows.Forms.Label()
        Me.lblTotalTax = New System.Windows.Forms.Label()
        Me.lblGrossPayment = New System.Windows.Forms.Label()
        Me.SSTab2 = New System.Windows.Forms.TabControl()
        Me._SSTab2_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblIBAN = New System.Windows.Forms.Label()
        Me.txtIBAN = New System.Windows.Forms.TextBox()
        Me.lblBIC = New System.Windows.Forms.Label()
        Me.txtBIC = New System.Windows.Forms.TextBox()
        Me.cboMediaType = New System.Windows.Forms.ComboBox()
        Me.lblMediaRef = New System.Windows.Forms.Label()
        Me.lblChequeDate = New System.Windows.Forms.Label()
        Me.lblThirdPartyReference = New System.Windows.Forms.Label()
        Me.lblBankAccountNo = New System.Windows.Forms.Label()
        Me.lblMediaType = New System.Windows.Forms.Label()
        Me.lblBankCode = New System.Windows.Forms.Label()
        Me.lblBankName = New System.Windows.Forms.Label()
        Me.lblPayeeName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblOurReference = New System.Windows.Forms.Label()
        Me.txtMediaType = New System.Windows.Forms.TextBox()
        Me.dtpChequeDate = New System.Windows.Forms.DateTimePicker()
        Me.uctPMAddressControl1 = New PMAddressControl.uctPMAddressControl
        Me.txtMediaRef = New System.Windows.Forms.TextBox()
        Me.txtThirdPartyReference = New System.Windows.Forms.TextBox()
        Me.txtBankAccountNo = New System.Windows.Forms.TextBox()
        Me.txtBankCode = New System.Windows.Forms.TextBox()
        Me.txtBankName = New System.Windows.Forms.TextBox()
        Me.txtPayeeName = New System.Windows.Forms.TextBox()
        Me.uctPartyBankCombo1 = New uctPartyBank.uctPartyBankCombo()
        Me.txtOurReference = New System.Windows.Forms.TextBox()
        Me._SSTab2_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraThisPaymentComments = New System.Windows.Forms.GroupBox()
        Me.txtPayeeComments = New System.Windows.Forms.TextBox()
        Me.fraTaxesOnPayments = New System.Windows.Forms.GroupBox()
        Me.lvwTaxesOnThisPayment = New System.Windows.Forms.ListView()
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraCoinsurers = New System.Windows.Forms.GroupBox()
        Me.txtFormatPercentage = New System.Windows.Forms.TextBox()
        Me.txtTotalAllocated = New System.Windows.Forms.TextBox()
        Me.txtPercentageAllocated = New System.Windows.Forms.TextBox()
        Me.lvwCoinsurers = New System.Windows.Forms.ListView()
        Me._lvwCoinsurers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCoinsurers_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCoinsurers_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCoinsurers_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblTotalAllocated = New System.Windows.Forms.Label()
        Me.lblPercentageAllocated = New System.Windows.Forms.Label()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraSettlement.SuspendLayout()
        Me.fraClaimInformation.SuspendLayout()
        Me.fraPayee.SuspendLayout()
        Me.fraInsuredTaxAdjustment.SuspendLayout()
        Me.fraPayeeTaxAdjustments.SuspendLayout()
        Me.fraSafeHarbour.SuspendLayout()
        Me.fraExemptions.SuspendLayout()
        Me.fraPaymentDetails.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.fraThisPaymentSummary.SuspendLayout()
        Me.SSTab2.SuspendLayout()
        Me._SSTab2_TabPage0.SuspendLayout()
        Me._SSTab2_TabPage1.SuspendLayout()
        Me.fraThisPaymentComments.SuspendLayout()
        Me.fraTaxesOnPayments.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.fraCoinsurers.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(289, 18)
        Me.SSTab1.Location = New System.Drawing.Point(0, 0)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(874, 571)
        Me.SSTab1.TabIndex = 51
        Me.SSTab1.TabStop = False
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraSettlement)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraClaimInformation)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPayee)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraInsuredTaxAdjustment)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPayeeTaxAdjustments)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraSafeHarbour)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraExemptions)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPaymentDetails)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(866, 545)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Payments"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
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
        Me.fraSettlement.Size = New System.Drawing.Size(191, 49)
        Me.fraSettlement.TabIndex = 37
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
        Me.chkSettlement.TabIndex = 38
        Me.chkSettlement.Text = "Settlement"
        Me.chkSettlement.UseVisualStyleBackColor = False
        '
        'fraClaimInformation
        '
        Me.fraClaimInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimInformation.Controls.Add(Me.txtLossDate)
        Me.fraClaimInformation.Controls.Add(Me.txtLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.txtRiskType)
        Me.fraClaimInformation.Controls.Add(Me.lblDateOfLoss)
        Me.fraClaimInformation.Controls.Add(Me.lblLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.lblRiskType)
        Me.fraClaimInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimInformation.Location = New System.Drawing.Point(6, 4)
        Me.fraClaimInformation.Name = "fraClaimInformation"
        Me.fraClaimInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimInformation.Size = New System.Drawing.Size(857, 49)
        Me.fraClaimInformation.TabIndex = 0
        Me.fraClaimInformation.TabStop = False
        Me.fraClaimInformation.Text = "Claim Information"
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
        Me.txtLossDate.Size = New System.Drawing.Size(193, 20)
        Me.txtLossDate.TabIndex = 6
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
        Me.txtLossCurrency.Size = New System.Drawing.Size(177, 20)
        Me.txtLossCurrency.TabIndex = 4
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
        Me.txtRiskType.Size = New System.Drawing.Size(169, 20)
        Me.txtRiskType.TabIndex = 2
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
        Me.lblDateOfLoss.TabIndex = 5
        Me.lblDateOfLoss.Text = "Date Of Loss:"
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
        Me.lblLossCurrency.TabIndex = 3
        Me.lblLossCurrency.Text = "Loss Currency:"
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
        Me.lblRiskType.TabIndex = 1
        Me.lblRiskType.Text = "Risk Type:"
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.chkIsExGratia)
        Me.fraPayee.Controls.Add(Me.OptParty)
        Me.fraPayee.Controls.Add(Me.OptClient)
        Me.fraPayee.Controls.Add(Me.OptAgent)
        Me.fraPayee.Controls.Add(Me.OptClaimPayable)
        Me.fraPayee.Controls.Add(Me.txtParty)
        Me.fraPayee.Controls.Add(Me.cmdParty)
        Me.fraPayee.Controls.Add(Me.cboClaimPaymentTo)
        Me.fraPayee.Controls.Add(Me.txtClaimPaymentTo)
        Me.fraPayee.Controls.Add(Me.lblPaymentTo)
        Me.fraPayee.Controls.Add(Me.lblParty)
        Me.fraPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(6, 53)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(857, 49)
        Me.fraPayee.TabIndex = 7
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payee"
        '
        'chkIsExGratia
        '
        Me.chkIsExGratia.AutoSize = True
        Me.chkIsExGratia.Location = New System.Drawing.Point(537, 20)
        Me.chkIsExGratia.Name = "chkIsExGratia"
        Me.chkIsExGratia.Size = New System.Drawing.Size(67, 17)
        Me.chkIsExGratia.TabIndex = 53
        Me.chkIsExGratia.Text = "Ex-gratia"
        Me.chkIsExGratia.UseVisualStyleBackColor = True
        '
        'OptParty
        '
        Me.OptParty.BackColor = System.Drawing.SystemColors.Control
        Me.OptParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptParty.Location = New System.Drawing.Point(361, 17)
        Me.OptParty.Name = "OptParty"
        Me.OptParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptParty.Size = New System.Drawing.Size(56, 23)
        Me.OptParty.TabIndex = 11
        Me.OptParty.TabStop = True
        Me.OptParty.Text = "Party"
        Me.OptParty.UseVisualStyleBackColor = False
        '
        'OptClient
        '
        Me.OptClient.BackColor = System.Drawing.SystemColors.Control
        Me.OptClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClient.Location = New System.Drawing.Point(472, 19)
        Me.OptClient.Name = "OptClient"
        Me.OptClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClient.Size = New System.Drawing.Size(57, 20)
        Me.OptClient.TabIndex = 13
        Me.OptClient.TabStop = True
        Me.OptClient.Text = "Client"
        Me.OptClient.UseVisualStyleBackColor = False
        '
        'OptAgent
        '
        Me.OptAgent.BackColor = System.Drawing.SystemColors.Control
        Me.OptAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptAgent.Location = New System.Drawing.Point(415, 17)
        Me.OptAgent.Name = "OptAgent"
        Me.OptAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAgent.Size = New System.Drawing.Size(59, 21)
        Me.OptAgent.TabIndex = 12
        Me.OptAgent.TabStop = True
        Me.OptAgent.Text = "Agent"
        Me.OptAgent.UseVisualStyleBackColor = False
        '
        'OptClaimPayable
        '
        Me.OptClaimPayable.BackColor = System.Drawing.SystemColors.Control
        Me.OptClaimPayable.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClaimPayable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClaimPayable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClaimPayable.Location = New System.Drawing.Point(264, 19)
        Me.OptClaimPayable.Name = "OptClaimPayable"
        Me.OptClaimPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClaimPayable.Size = New System.Drawing.Size(97, 20)
        Me.OptClaimPayable.TabIndex = 10
        Me.OptClaimPayable.TabStop = True
        Me.OptClaimPayable.Text = "Claim Payable"
        Me.OptClaimPayable.UseVisualStyleBackColor = False
        '
        'txtParty
        '
        Me.txtParty.AcceptsReturn = True
        Me.txtParty.BackColor = System.Drawing.SystemColors.Window
        Me.txtParty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParty.Location = New System.Drawing.Point(703, 18)
        Me.txtParty.MaxLength = 255
        Me.txtParty.Name = "txtParty"
        Me.txtParty.ReadOnly = True
        Me.txtParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParty.Size = New System.Drawing.Size(123, 20)
        Me.txtParty.TabIndex = 15
        '
        'cmdParty
        '
        Me.cmdParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParty.Location = New System.Drawing.Point(824, 18)
        Me.cmdParty.Name = "cmdParty"
        Me.cmdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParty.Size = New System.Drawing.Size(25, 21)
        Me.cmdParty.TabIndex = 16
        Me.cmdParty.Text = "..."
        Me.cmdParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdParty.UseVisualStyleBackColor = False
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
        Me.cboClaimPaymentTo.TabIndex = 9
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
        Me.txtClaimPaymentTo.Size = New System.Drawing.Size(161, 20)
        Me.txtClaimPaymentTo.TabIndex = 52
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
        Me.lblPaymentTo.TabIndex = 8
        Me.lblPaymentTo.Text = "Payment To:"
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(663, 22)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(34, 13)
        Me.lblParty.TabIndex = 14
        Me.lblParty.Text = "Party:"
        '
        'fraInsuredTaxAdjustment
        '
        Me.fraInsuredTaxAdjustment.BackColor = System.Drawing.SystemColors.Control
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.txtITTaxNo)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.txtITPercentage)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.chkITDomiciled)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.lblITTaxNo)
        Me.fraInsuredTaxAdjustment.Controls.Add(Me.lblITPercentage)
        Me.fraInsuredTaxAdjustment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInsuredTaxAdjustment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInsuredTaxAdjustment.Location = New System.Drawing.Point(6, 102)
        Me.fraInsuredTaxAdjustment.Name = "fraInsuredTaxAdjustment"
        Me.fraInsuredTaxAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsuredTaxAdjustment.Size = New System.Drawing.Size(427, 49)
        Me.fraInsuredTaxAdjustment.TabIndex = 17
        Me.fraInsuredTaxAdjustment.TabStop = False
        Me.fraInsuredTaxAdjustment.Text = "Insured Tax Adjustment"
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
        Me.txtITTaxNo.Size = New System.Drawing.Size(137, 20)
        Me.txtITTaxNo.TabIndex = 22
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
        Me.txtITPercentage.Size = New System.Drawing.Size(49, 20)
        Me.txtITPercentage.TabIndex = 20
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
        Me.chkITDomiciled.Size = New System.Drawing.Size(81, 23)
        Me.chkITDomiciled.TabIndex = 18
        Me.chkITDomiciled.Text = "Domiciled"
        Me.chkITDomiciled.UseVisualStyleBackColor = False
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
        Me.lblITTaxNo.TabIndex = 21
        Me.lblITTaxNo.Text = "Tax No:"
        '
        'lblITPercentage
        '
        Me.lblITPercentage.AutoSize = True
        Me.lblITPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblITPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblITPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblITPercentage.Location = New System.Drawing.Point(100, 21)
        Me.lblITPercentage.Name = "lblITPercentage"
        Me.lblITPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblITPercentage.Size = New System.Drawing.Size(65, 13)
        Me.lblITPercentage.TabIndex = 19
        Me.lblITPercentage.Text = "Percentage:"
        '
        'fraPayeeTaxAdjustments
        '
        Me.fraPayeeTaxAdjustments.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.txtPTTaxNo)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.txtPTPercentage)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.chkPTDomiciled)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.lblPTTaxNo)
        Me.fraPayeeTaxAdjustments.Controls.Add(Me.lblPTPercentage)
        Me.fraPayeeTaxAdjustments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayeeTaxAdjustments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayeeTaxAdjustments.Location = New System.Drawing.Point(440, 102)
        Me.fraPayeeTaxAdjustments.Name = "fraPayeeTaxAdjustments"
        Me.fraPayeeTaxAdjustments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayeeTaxAdjustments.Size = New System.Drawing.Size(423, 49)
        Me.fraPayeeTaxAdjustments.TabIndex = 23
        Me.fraPayeeTaxAdjustments.TabStop = False
        Me.fraPayeeTaxAdjustments.Text = "Payee Tax Adjustments"
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
        Me.txtPTTaxNo.Size = New System.Drawing.Size(137, 20)
        Me.txtPTTaxNo.TabIndex = 28
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
        Me.txtPTPercentage.Size = New System.Drawing.Size(49, 20)
        Me.txtPTPercentage.TabIndex = 26
        '
        'chkPTDomiciled
        '
        Me.chkPTDomiciled.BackColor = System.Drawing.SystemColors.Control
        Me.chkPTDomiciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPTDomiciled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPTDomiciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPTDomiciled.Location = New System.Drawing.Point(16, 18)
        Me.chkPTDomiciled.Name = "chkPTDomiciled"
        Me.chkPTDomiciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPTDomiciled.Size = New System.Drawing.Size(81, 19)
        Me.chkPTDomiciled.TabIndex = 24
        Me.chkPTDomiciled.Text = "Domiciled"
        Me.chkPTDomiciled.UseVisualStyleBackColor = False
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
        Me.lblPTTaxNo.TabIndex = 27
        Me.lblPTTaxNo.Text = "Tax No:"
        '
        'lblPTPercentage
        '
        Me.lblPTPercentage.AutoSize = True
        Me.lblPTPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPTPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPTPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPTPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPTPercentage.Location = New System.Drawing.Point(100, 20)
        Me.lblPTPercentage.Name = "lblPTPercentage"
        Me.lblPTPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPTPercentage.Size = New System.Drawing.Size(65, 13)
        Me.lblPTPercentage.TabIndex = 25
        Me.lblPTPercentage.Text = "Percentage:"
        '
        'fraSafeHarbour
        '
        Me.fraSafeHarbour.BackColor = System.Drawing.SystemColors.Control
        Me.fraSafeHarbour.Controls.Add(Me.txtSFPercentage)
        Me.fraSafeHarbour.Controls.Add(Me.cboSafeHarbour)
        Me.fraSafeHarbour.Controls.Add(Me.txtSafeHarbour)
        Me.fraSafeHarbour.Controls.Add(Me.lblSFPercentage)
        Me.fraSafeHarbour.Controls.Add(Me.lblAgreement)
        Me.fraSafeHarbour.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSafeHarbour.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSafeHarbour.Location = New System.Drawing.Point(6, 151)
        Me.fraSafeHarbour.Name = "fraSafeHarbour"
        Me.fraSafeHarbour.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSafeHarbour.Size = New System.Drawing.Size(425, 49)
        Me.fraSafeHarbour.TabIndex = 29
        Me.fraSafeHarbour.TabStop = False
        Me.fraSafeHarbour.Text = "Safe Harbour"
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
        Me.txtSFPercentage.Size = New System.Drawing.Size(49, 20)
        Me.txtSFPercentage.TabIndex = 33
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
        Me.cboSafeHarbour.TabIndex = 31
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
        Me.txtSafeHarbour.Size = New System.Drawing.Size(201, 20)
        Me.txtSafeHarbour.TabIndex = 53
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
        Me.lblSFPercentage.TabIndex = 32
        Me.lblSFPercentage.Text = "Percentage:"
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
        Me.lblAgreement.TabIndex = 30
        Me.lblAgreement.Text = "Agreement:"
        '
        'fraExemptions
        '
        Me.fraExemptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraExemptions.Controls.Add(Me.chkWHTExempt)
        Me.fraExemptions.Controls.Add(Me.chkTaxExempt)
        Me.fraExemptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraExemptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExemptions.Location = New System.Drawing.Point(440, 151)
        Me.fraExemptions.Name = "fraExemptions"
        Me.fraExemptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraExemptions.Size = New System.Drawing.Size(225, 49)
        Me.fraExemptions.TabIndex = 34
        Me.fraExemptions.TabStop = False
        Me.fraExemptions.Text = "Exemptions"
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
        Me.chkWHTExempt.TabIndex = 36
        Me.chkWHTExempt.Text = "WHT Exempt"
        Me.chkWHTExempt.UseVisualStyleBackColor = False
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
        Me.chkTaxExempt.TabIndex = 35
        Me.chkTaxExempt.Text = "Tax Exempt"
        Me.chkTaxExempt.UseVisualStyleBackColor = False
        '
        'fraPaymentDetails
        '
        Me.fraPaymentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentDetails.Controls.Add(Me.cmdPaymentLock)
        Me.fraPaymentDetails.Controls.Add(Me.cmdEdit)
        Me.fraPaymentDetails.Controls.Add(Me.cmdReserveEdit)
        Me.fraPaymentDetails.Controls.Add(Me.cmdEditPayee)
        Me.fraPaymentDetails.Controls.Add(Me.cmdDelete)
        Me.fraPaymentDetails.Controls.Add(Me.lvwPaymentDetails)
        Me.fraPaymentDetails.Controls.Add(Me.cmdHistory)
        Me.fraPaymentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentDetails.Location = New System.Drawing.Point(6, 200)
        Me.fraPaymentDetails.Name = "fraPaymentDetails"
        Me.fraPaymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentDetails.Size = New System.Drawing.Size(857, 230)
        Me.fraPaymentDetails.TabIndex = 39
        Me.fraPaymentDetails.TabStop = False
        Me.fraPaymentDetails.Text = "Payment Details"
        '
        'cmdPaymentLock
        '
        Me.cmdPaymentLock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPaymentLock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPaymentLock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPaymentLock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPaymentLock.Location = New System.Drawing.Point(368, 186)
        Me.cmdPaymentLock.Name = "cmdPaymentLock"
        Me.cmdPaymentLock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPaymentLock.Size = New System.Drawing.Size(97, 21)
        Me.cmdPaymentLock.TabIndex = 86
        Me.cmdPaymentLock.Text = "&Lock Payment"
        Me.cmdPaymentLock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPaymentLock.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(664, 186)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(105, 21)
        Me.cmdEdit.TabIndex = 85
        Me.cmdEdit.Text = "&Edit Payment"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdReserveEdit
        '
        Me.cmdReserveEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReserveEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReserveEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReserveEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReserveEdit.Location = New System.Drawing.Point(552, 186)
        Me.cmdReserveEdit.Name = "cmdReserveEdit"
        Me.cmdReserveEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReserveEdit.Size = New System.Drawing.Size(105, 21)
        Me.cmdReserveEdit.TabIndex = 84
        Me.cmdReserveEdit.Text = "Edit &Reserve"
        Me.cmdReserveEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReserveEdit.UseVisualStyleBackColor = False
        '
        'cmdEditPayee
        '
        Me.cmdEditPayee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditPayee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditPayee.Enabled = False
        Me.cmdEditPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditPayee.Location = New System.Drawing.Point(8, 186)
        Me.cmdEditPayee.Name = "cmdEditPayee"
        Me.cmdEditPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditPayee.Size = New System.Drawing.Size(169, 21)
        Me.cmdEditPayee.TabIndex = 55
        Me.cmdEditPayee.Text = "Edit Pa&yee And Tax Details"
        Me.cmdEditPayee.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditPayee.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(472, 186)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 21)
        Me.cmdDelete.TabIndex = 54
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
        Me.lvwPaymentDetails.GridLines = True
        Me.lvwPaymentDetails.HideSelection = False
        Me.lvwPaymentDetails.Location = New System.Drawing.Point(8, 16)
        Me.lvwPaymentDetails.Name = "lvwPaymentDetails"
        Me.lvwPaymentDetails.Size = New System.Drawing.Size(841, 167)
        Me.lvwPaymentDetails.TabIndex = 40
        Me.lvwPaymentDetails.UseCompatibleStateImageBehavior = False
        Me.lvwPaymentDetails.View = System.Windows.Forms.View.Details
        '
        'cmdHistory
        '
        Me.cmdHistory.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHistory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHistory.Location = New System.Drawing.Point(776, 186)
        Me.cmdHistory.Name = "cmdHistory"
        Me.cmdHistory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHistory.Size = New System.Drawing.Size(73, 21)
        Me.cmdHistory.TabIndex = 41
        Me.cmdHistory.Text = "&History"
        Me.cmdHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHistory.UseVisualStyleBackColor = False
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.fraThisPaymentSummary)
        Me._SSTab1_TabPage1.Controls.Add(Me.SSTab2)
        Me._SSTab1_TabPage1.Controls.Add(Me.fraTaxesOnPayments)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(866, 545)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "This Payment"
        Me._SSTab1_TabPage1.UseVisualStyleBackColor = True
        '
        'fraThisPaymentSummary
        '
        Me.fraThisPaymentSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraThisPaymentSummary.Controls.Add(Me.txtNetPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtTotalWHTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtTotalTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.txtGrossPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblNetPayment)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblTotalWHTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblTotalTax)
        Me.fraThisPaymentSummary.Controls.Add(Me.lblGrossPayment)
        Me.fraThisPaymentSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThisPaymentSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThisPaymentSummary.Location = New System.Drawing.Point(8, 4)
        Me.fraThisPaymentSummary.Name = "fraThisPaymentSummary"
        Me.fraThisPaymentSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThisPaymentSummary.Size = New System.Drawing.Size(321, 141)
        Me.fraThisPaymentSummary.TabIndex = 42
        Me.fraThisPaymentSummary.TabStop = False
        Me.fraThisPaymentSummary.Text = "This Payment Summary"
        '
        'txtNetPayment
        '
        Me.txtNetPayment.AcceptsReturn = True
        Me.txtNetPayment.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetPayment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNetPayment.Location = New System.Drawing.Point(120, 108)
        Me.txtNetPayment.MaxLength = 0
        Me.txtNetPayment.Name = "txtNetPayment"
        Me.txtNetPayment.ReadOnly = True
        Me.txtNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNetPayment.Size = New System.Drawing.Size(185, 20)
        Me.txtNetPayment.TabIndex = 50
        Me.txtNetPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalWHTax
        '
        Me.txtTotalWHTax.AcceptsReturn = True
        Me.txtTotalWHTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalWHTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalWHTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalWHTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalWHTax.Location = New System.Drawing.Point(120, 77)
        Me.txtTotalWHTax.MaxLength = 0
        Me.txtTotalWHTax.Name = "txtTotalWHTax"
        Me.txtTotalWHTax.ReadOnly = True
        Me.txtTotalWHTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalWHTax.Size = New System.Drawing.Size(185, 20)
        Me.txtTotalWHTax.TabIndex = 48
        Me.txtTotalWHTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalTax
        '
        Me.txtTotalTax.AcceptsReturn = True
        Me.txtTotalTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTax.Location = New System.Drawing.Point(120, 50)
        Me.txtTotalTax.MaxLength = 0
        Me.txtTotalTax.Name = "txtTotalTax"
        Me.txtTotalTax.ReadOnly = True
        Me.txtTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTax.Size = New System.Drawing.Size(185, 20)
        Me.txtTotalTax.TabIndex = 46
        Me.txtTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
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
        Me.txtGrossPayment.Size = New System.Drawing.Size(185, 20)
        Me.txtGrossPayment.TabIndex = 44
        Me.txtGrossPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNetPayment
        '
        Me.lblNetPayment.AutoSize = True
        Me.lblNetPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetPayment.Location = New System.Drawing.Point(32, 112)
        Me.lblNetPayment.Name = "lblNetPayment"
        Me.lblNetPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetPayment.Size = New System.Drawing.Size(71, 13)
        Me.lblNetPayment.TabIndex = 49
        Me.lblNetPayment.Text = "Net Payment:"
        '
        'lblTotalWHTax
        '
        Me.lblTotalWHTax.AutoSize = True
        Me.lblTotalWHTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalWHTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalWHTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalWHTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalWHTax.Location = New System.Drawing.Point(29, 81)
        Me.lblTotalWHTax.Name = "lblTotalWHTax"
        Me.lblTotalWHTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalWHTax.Size = New System.Drawing.Size(77, 13)
        Me.lblTotalWHTax.TabIndex = 47
        Me.lblTotalWHTax.Text = "Total WH Tax:"
        '
        'lblTotalTax
        '
        Me.lblTotalTax.AutoSize = True
        Me.lblTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalTax.Location = New System.Drawing.Point(52, 54)
        Me.lblTotalTax.Name = "lblTotalTax"
        Me.lblTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalTax.Size = New System.Drawing.Size(55, 13)
        Me.lblTotalTax.TabIndex = 45
        Me.lblTotalTax.Text = "Total Tax:"
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
        Me.lblGrossPayment.TabIndex = 43
        Me.lblGrossPayment.Text = "Gross Payment:"
        '
        'SSTab2
        '
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage0)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage1)
        Me.SSTab2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab2.ItemSize = New System.Drawing.Size(284, 18)
        Me.SSTab2.Location = New System.Drawing.Point(8, 150)
        Me.SSTab2.Multiline = True
        Me.SSTab2.Name = "SSTab2"
        Me.SSTab2.SelectedIndex = 0
        Me.SSTab2.Size = New System.Drawing.Size(861, 392)
        Me.SSTab2.TabIndex = 56
        '
        '_SSTab2_TabPage0
        '
        Me._SSTab2_TabPage0.Controls.Add(Me.lblIBAN)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtIBAN)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblBIC)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtBIC)
        Me._SSTab2_TabPage0.Controls.Add(Me.cboMediaType)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblMediaRef)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblChequeDate)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblThirdPartyReference)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblBankAccountNo)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblMediaType)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblBankCode)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblBankName)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblPayeeName)
        Me._SSTab2_TabPage0.Controls.Add(Me.Label1)
        Me._SSTab2_TabPage0.Controls.Add(Me.lblOurReference)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtMediaType)
        Me._SSTab2_TabPage0.Controls.Add(Me.dtpChequeDate)
        Me._SSTab2_TabPage0.Controls.Add(Me.uctPMAddressControl1)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtMediaRef)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtThirdPartyReference)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtBankAccountNo)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtBankCode)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtBankName)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtPayeeName)
        Me._SSTab2_TabPage0.Controls.Add(Me.uctPartyBankCombo1)
        Me._SSTab2_TabPage0.Controls.Add(Me.txtOurReference)
        Me._SSTab2_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage0.Name = "_SSTab2_TabPage0"
        Me._SSTab2_TabPage0.Size = New System.Drawing.Size(853, 366)
        Me._SSTab2_TabPage0.TabIndex = 0
        Me._SSTab2_TabPage0.Text = "Payment Details"
        '
        'lblIBAN
        '
        Me.lblIBAN.AutoSize = True
        Me.lblIBAN.BackColor = System.Drawing.SystemColors.Control
        Me.lblIBAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIBAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIBAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIBAN.Location = New System.Drawing.Point(329, 267)
        Me.lblIBAN.Name = "lblIBAN"
        Me.lblIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIBAN.Size = New System.Drawing.Size(35, 13)
        Me.lblIBAN.TabIndex = 96
        Me.lblIBAN.Text = "IBAN:"
        '
        'txtIBAN
        '
        Me.txtIBAN.AcceptsReturn = True
        Me.txtIBAN.BackColor = System.Drawing.SystemColors.Window
        Me.txtIBAN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIBAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIBAN.Location = New System.Drawing.Point(434, 264)
        Me.txtIBAN.MaxLength = 50
        Me.txtIBAN.Name = "txtIBAN"
        Me.txtIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIBAN.Size = New System.Drawing.Size(237, 20)
        Me.txtIBAN.TabIndex = 95
        '
        'lblBIC
        '
        Me.lblBIC.AutoSize = True
        Me.lblBIC.BackColor = System.Drawing.SystemColors.Control
        Me.lblBIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBIC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBIC.Location = New System.Drawing.Point(24, 264)
        Me.lblBIC.Name = "lblBIC"
        Me.lblBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBIC.Size = New System.Drawing.Size(27, 13)
        Me.lblBIC.TabIndex = 94
        Me.lblBIC.Text = "BIC:"
        '
        'txtBIC
        '
        Me.txtBIC.AcceptsReturn = True
        Me.txtBIC.BackColor = System.Drawing.SystemColors.Window
        Me.txtBIC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBIC.Location = New System.Drawing.Point(129, 261)
        Me.txtBIC.MaxLength = 50
        Me.txtBIC.Name = "txtBIC"
        Me.txtBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBIC.Size = New System.Drawing.Size(185, 20)
        Me.txtBIC.TabIndex = 93
        '
        'cboMediaType
        '
        Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMediaType.Location = New System.Drawing.Point(129, 37)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMediaType.Size = New System.Drawing.Size(185, 21)
        Me.cboMediaType.TabIndex = 66
        '
        'lblMediaRef
        '
        Me.lblMediaRef.AutoSize = True
        Me.lblMediaRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaRef.Location = New System.Drawing.Point(334, 41)
        Me.lblMediaRef.Name = "lblMediaRef"
        Me.lblMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaRef.Size = New System.Drawing.Size(59, 13)
        Me.lblMediaRef.TabIndex = 75
        Me.lblMediaRef.Text = "Media Ref:"
        '
        'lblChequeDate
        '
        Me.lblChequeDate.AutoSize = True
        Me.lblChequeDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblChequeDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChequeDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChequeDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChequeDate.Location = New System.Drawing.Point(49, 97)
        Me.lblChequeDate.Name = "lblChequeDate"
        Me.lblChequeDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChequeDate.Size = New System.Drawing.Size(77, 13)
        Me.lblChequeDate.TabIndex = 76
        Me.lblChequeDate.Text = "Payment Date:"
        '
        'lblThirdPartyReference
        '
        Me.lblThirdPartyReference.AutoSize = True
        Me.lblThirdPartyReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblThirdPartyReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThirdPartyReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThirdPartyReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblThirdPartyReference.Location = New System.Drawing.Point(30, 208)
        Me.lblThirdPartyReference.Name = "lblThirdPartyReference"
        Me.lblThirdPartyReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThirdPartyReference.Size = New System.Drawing.Size(87, 13)
        Me.lblThirdPartyReference.TabIndex = 77
        Me.lblThirdPartyReference.Text = "Their Reference:"
        '
        'lblBankAccountNo
        '
        Me.lblBankAccountNo.AutoSize = True
        Me.lblBankAccountNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccountNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccountNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccountNo.Location = New System.Drawing.Point(24, 180)
        Me.lblBankAccountNo.Name = "lblBankAccountNo"
        Me.lblBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccountNo.Size = New System.Drawing.Size(95, 13)
        Me.lblBankAccountNo.TabIndex = 78
        Me.lblBankAccountNo.Text = "Bank Account No:"
        '
        'lblMediaType
        '
        Me.lblMediaType.AutoSize = True
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(56, 41)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(66, 13)
        Me.lblMediaType.TabIndex = 79
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblBankCode
        '
        Me.lblBankCode.AutoSize = True
        Me.lblBankCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankCode.Location = New System.Drawing.Point(47, 152)
        Me.lblBankCode.Name = "lblBankCode"
        Me.lblBankCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankCode.Size = New System.Drawing.Size(72, 13)
        Me.lblBankCode.TabIndex = 80
        Me.lblBankCode.Text = "Branch Code:"
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(55, 124)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(66, 13)
        Me.lblBankName.TabIndex = 81
        Me.lblBankName.Text = "Bank Name:"
        '
        'lblPayeeName
        '
        Me.lblPayeeName.AutoSize = True
        Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeName.Location = New System.Drawing.Point(49, 69)
        Me.lblPayeeName.Name = "lblPayeeName"
        Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeName.Size = New System.Drawing.Size(71, 13)
        Me.lblPayeeName.TabIndex = 82
        Me.lblPayeeName.Text = "Payee Name:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(46, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 88
        Me.Label1.Text = "Account Type:"
        '
        'lblOurReference
        '
        Me.lblOurReference.AutoSize = True
        Me.lblOurReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblOurReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOurReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOurReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOurReference.Location = New System.Drawing.Point(32, 235)
        Me.lblOurReference.Name = "lblOurReference"
        Me.lblOurReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOurReference.Size = New System.Drawing.Size(80, 13)
        Me.lblOurReference.TabIndex = 91
        Me.lblOurReference.Text = "Our Reference:"
        '
        'txtMediaType
        '
        Me.txtMediaType.AcceptsReturn = True
        Me.txtMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaType.Location = New System.Drawing.Point(130, 37)
        Me.txtMediaType.MaxLength = 0
        Me.txtMediaType.Name = "txtMediaType"
        Me.txtMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaType.Size = New System.Drawing.Size(171, 20)
        Me.txtMediaType.TabIndex = 83
        '
        'dtpChequeDate
        '
        Me.dtpChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpChequeDate.Location = New System.Drawing.Point(129, 92)
        Me.dtpChequeDate.Name = "dtpChequeDate"
        Me.dtpChequeDate.Size = New System.Drawing.Size(185, 20)
        Me.dtpChequeDate.TabIndex = 69
        Me.dtpChequeDate.Enabled = True
        '
        'uctPMAddressControl1
        '
        Me.uctPMAddressControl1.AddressLine1 = ""
        Me.uctPMAddressControl1.AddressLine2 = ""
        Me.uctPMAddressControl1.AddressLine3 = ""
        Me.uctPMAddressControl1.AddressLine4 = ""
        Me.uctPMAddressControl1.Caption = ""
        Me.uctPMAddressControl1.CaptionAddress1 = "Address Line 1:"
        Me.uctPMAddressControl1.CaptionAddress2 = "Address Line 2:"
        Me.uctPMAddressControl1.CaptionAddress3 = "Address Line 3:"
        Me.uctPMAddressControl1.CaptionAddress4 = "Address Line 4:"
        Me.uctPMAddressControl1.CaptionCountry = "Country:"
        Me.uctPMAddressControl1.CaptionFontBoldAddress1 = False
        Me.uctPMAddressControl1.CaptionFontBoldPostCode = False
        Me.uctPMAddressControl1.CaptionPostCode = "Postcode:"
        Me.uctPMAddressControl1.ClearButtonCaption = "X"
        Me.uctPMAddressControl1.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.ClearButtonLeft = 7260
        Me.uctPMAddressControl1.ClearButtonWidth = 360
        Me.uctPMAddressControl1.CountryId = 0
        Me.uctPMAddressControl1.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.IsCountryRequired = 1
        Me.uctPMAddressControl1.IsPostCodeRequired = 1
        Me.uctPMAddressControl1.Location = New System.Drawing.Point(328, 75)
        Me.uctPMAddressControl1.Name = "uctPMAddressControl1"
        Me.uctPMAddressControl1.Organisation = ""
        Me.uctPMAddressControl1.PMAddressCnt = 0
        Me.uctPMAddressControl1.PMDatabaseID = 0
        Me.uctPMAddressControl1.PostCode = "__"
        Me.uctPMAddressControl1.QAS2PMAddress1 = "3,4,2,5,6"
        Me.uctPMAddressControl1.QAS2PMAddress2 = "8,7"
        Me.uctPMAddressControl1.QAS2PMAddress3 = "9"
        Me.uctPMAddressControl1.QAS2PMAddress4 = ""
        Me.uctPMAddressControl1.QASDatabaseID = 3
        Me.uctPMAddressControl1.SearchButtonCaption = ".."
        Me.uctPMAddressControl1.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.SearchButtonHeight = 285
        Me.uctPMAddressControl1.SearchButtonLeft = 6825
        Me.uctPMAddressControl1.SearchButtonTop = 1530
        Me.uctPMAddressControl1.SearchButtonWidth = 360
        Me.uctPMAddressControl1.Size = New System.Drawing.Size(520, 152)
        Me.uctPMAddressControl1.TabIndex = 74
        Me.uctPMAddressControl1.WarningMessage = ""
        '
        'txtMediaRef
        '
        Me.txtMediaRef.AcceptsReturn = True
        Me.txtMediaRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaRef.Location = New System.Drawing.Point(406, 37)
        Me.txtMediaRef.MaxLength = 100
        Me.txtMediaRef.Name = "txtMediaRef"
        Me.txtMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaRef.Size = New System.Drawing.Size(265, 20)
        Me.txtMediaRef.TabIndex = 67
        '
        'txtThirdPartyReference
        '
        Me.txtThirdPartyReference.AcceptsReturn = True
        Me.txtThirdPartyReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtThirdPartyReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThirdPartyReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThirdPartyReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtThirdPartyReference.Location = New System.Drawing.Point(129, 204)
        Me.txtThirdPartyReference.MaxLength = 0
        Me.txtThirdPartyReference.Name = "txtThirdPartyReference"
        Me.txtThirdPartyReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThirdPartyReference.Size = New System.Drawing.Size(185, 20)
        Me.txtThirdPartyReference.TabIndex = 73
        '
        'txtBankAccountNo
        '
        Me.txtBankAccountNo.AcceptsReturn = True
        Me.txtBankAccountNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAccountNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAccountNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAccountNo.Location = New System.Drawing.Point(129, 176)
        Me.txtBankAccountNo.MaxLength = 30
        Me.txtBankAccountNo.Name = "txtBankAccountNo"
        Me.txtBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAccountNo.Size = New System.Drawing.Size(185, 20)
        Me.txtBankAccountNo.TabIndex = 72
        '
        'txtBankCode
        '
        Me.txtBankCode.AcceptsReturn = True
        Me.txtBankCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankCode.Location = New System.Drawing.Point(129, 148)
        Me.txtBankCode.MaxLength = 8
        Me.txtBankCode.Name = "txtBankCode"
        Me.txtBankCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankCode.Size = New System.Drawing.Size(185, 20)
        Me.txtBankCode.TabIndex = 71
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(129, 120)
        Me.txtBankName.MaxLength = 255
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(185, 20)
        Me.txtBankName.TabIndex = 70
        '
        'txtPayeeName
        '
        Me.txtPayeeName.AcceptsReturn = True
        Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeName.Location = New System.Drawing.Point(129, 65)
        Me.txtPayeeName.MaxLength = 255
        Me.txtPayeeName.Name = "txtPayeeName"
        Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeName.Size = New System.Drawing.Size(185, 20)
        Me.txtPayeeName.TabIndex = 68
        '
        'uctPartyBankCombo1
        '
        Me.uctPartyBankCombo1.BankPaymentTypeCode = ""
        Me.uctPartyBankCombo1.EnableAdd = False
        Me.uctPartyBankCombo1.EnableEdit = False
        Me.uctPartyBankCombo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankCombo1.Location = New System.Drawing.Point(126, 5)
        Me.uctPartyBankCombo1.Name = "uctPartyBankCombo1"
        Me.uctPartyBankCombo1.PartyBankDetails = Nothing
        Me.uctPartyBankCombo1.PartyCnt = Nothing
        Me.uctPartyBankCombo1.SelectedPaymentID = 0
        Me.uctPartyBankCombo1.Size = New System.Drawing.Size(381, 25)
        Me.uctPartyBankCombo1.TabIndex = 87
        '
        'txtOurReference
        '
        Me.txtOurReference.AcceptsReturn = True
        Me.txtOurReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtOurReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOurReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOurReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOurReference.Location = New System.Drawing.Point(129, 235)
        Me.txtOurReference.MaxLength = 0
        Me.txtOurReference.Name = "txtOurReference"
        Me.txtOurReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOurReference.Size = New System.Drawing.Size(185, 20)
        Me.txtOurReference.TabIndex = 92
        '
        '_SSTab2_TabPage1
        '
        Me._SSTab2_TabPage1.Controls.Add(Me.fraThisPaymentComments)
        Me._SSTab2_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage1.Name = "_SSTab2_TabPage1"
        Me._SSTab2_TabPage1.Size = New System.Drawing.Size(853, 366)
        Me._SSTab2_TabPage1.TabIndex = 1
        Me._SSTab2_TabPage1.Text = "Comments"
        '
        'fraThisPaymentComments
        '
        Me.fraThisPaymentComments.BackColor = System.Drawing.SystemColors.Control
        Me.fraThisPaymentComments.Controls.Add(Me.txtPayeeComments)
        Me.fraThisPaymentComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraThisPaymentComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraThisPaymentComments.Location = New System.Drawing.Point(8, 4)
        Me.fraThisPaymentComments.Name = "fraThisPaymentComments"
        Me.fraThisPaymentComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraThisPaymentComments.Size = New System.Drawing.Size(833, 221)
        Me.fraThisPaymentComments.TabIndex = 57
        Me.fraThisPaymentComments.TabStop = False
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
        Me.txtPayeeComments.Size = New System.Drawing.Size(817, 194)
        Me.txtPayeeComments.TabIndex = 58
        '
        'fraTaxesOnPayments
        '
        Me.fraTaxesOnPayments.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaxesOnPayments.Controls.Add(Me.lvwTaxesOnThisPayment)
        Me.fraTaxesOnPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaxesOnPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaxesOnPayments.Location = New System.Drawing.Point(338, 4)
        Me.fraTaxesOnPayments.Name = "fraTaxesOnPayments"
        Me.fraTaxesOnPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaxesOnPayments.Size = New System.Drawing.Size(529, 141)
        Me.fraTaxesOnPayments.TabIndex = 89
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
        Me.lvwTaxesOnThisPayment.Size = New System.Drawing.Size(513, 113)
        Me.lvwTaxesOnThisPayment.TabIndex = 90
        Me.lvwTaxesOnThisPayment.UseCompatibleStateImageBehavior = False
        Me.lvwTaxesOnThisPayment.View = System.Windows.Forms.View.Details
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.fraCoinsurers)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(866, 545)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Co-Insurers"
        Me._SSTab1_TabPage2.UseVisualStyleBackColor = True
        '
        'fraCoinsurers
        '
        Me.fraCoinsurers.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoinsurers.Controls.Add(Me.txtFormatPercentage)
        Me.fraCoinsurers.Controls.Add(Me.txtTotalAllocated)
        Me.fraCoinsurers.Controls.Add(Me.txtPercentageAllocated)
        Me.fraCoinsurers.Controls.Add(Me.lvwCoinsurers)
        Me.fraCoinsurers.Controls.Add(Me.lblTotalAllocated)
        Me.fraCoinsurers.Controls.Add(Me.lblPercentageAllocated)
        Me.fraCoinsurers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoinsurers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoinsurers.Location = New System.Drawing.Point(8, 12)
        Me.fraCoinsurers.Name = "fraCoinsurers"
        Me.fraCoinsurers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoinsurers.Size = New System.Drawing.Size(857, 401)
        Me.fraCoinsurers.TabIndex = 59
        Me.fraCoinsurers.TabStop = False
        Me.fraCoinsurers.Text = "Co-Insurer Details"
        '
        'txtFormatPercentage
        '
        Me.txtFormatPercentage.AcceptsReturn = True
        Me.txtFormatPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatPercentage.Enabled = False
        Me.txtFormatPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatPercentage.Location = New System.Drawing.Point(416, 24)
        Me.txtFormatPercentage.MaxLength = 0
        Me.txtFormatPercentage.Name = "txtFormatPercentage"
        Me.txtFormatPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatPercentage.Size = New System.Drawing.Size(105, 20)
        Me.txtFormatPercentage.TabIndex = 64
        Me.txtFormatPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFormatPercentage.Visible = False
        '
        'txtTotalAllocated
        '
        Me.txtTotalAllocated.AcceptsReturn = True
        Me.txtTotalAllocated.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalAllocated.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalAllocated.Enabled = False
        Me.txtTotalAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalAllocated.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalAllocated.Location = New System.Drawing.Point(288, 24)
        Me.txtTotalAllocated.MaxLength = 0
        Me.txtTotalAllocated.Name = "txtTotalAllocated"
        Me.txtTotalAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalAllocated.Size = New System.Drawing.Size(105, 20)
        Me.txtTotalAllocated.TabIndex = 63
        Me.txtTotalAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPercentageAllocated
        '
        Me.txtPercentageAllocated.AcceptsReturn = True
        Me.txtPercentageAllocated.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentageAllocated.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentageAllocated.Enabled = False
        Me.txtPercentageAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentageAllocated.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentageAllocated.Location = New System.Drawing.Point(96, 24)
        Me.txtPercentageAllocated.MaxLength = 0
        Me.txtPercentageAllocated.Name = "txtPercentageAllocated"
        Me.txtPercentageAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentageAllocated.Size = New System.Drawing.Size(81, 20)
        Me.txtPercentageAllocated.TabIndex = 61
        Me.txtPercentageAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwCoinsurers
        '
        Me.lvwCoinsurers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCoinsurers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurers_ColumnHeader_1, Me._lvwCoinsurers_ColumnHeader_2, Me._lvwCoinsurers_ColumnHeader_3, Me._lvwCoinsurers_ColumnHeader_4})
        Me.lvwCoinsurers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurers.FullRowSelect = True
        Me.lvwCoinsurers.GridLines = True
        Me.lvwCoinsurers.HideSelection = False
        Me.lvwCoinsurers.Location = New System.Drawing.Point(8, 56)
        Me.lvwCoinsurers.Name = "lvwCoinsurers"
        Me.lvwCoinsurers.Size = New System.Drawing.Size(841, 337)
        Me.lvwCoinsurers.TabIndex = 65
        Me.lvwCoinsurers.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurers.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurers_ColumnHeader_1
        '
        Me._lvwCoinsurers_ColumnHeader_1.Text = "Insurer"
        Me._lvwCoinsurers_ColumnHeader_1.Width = 134
        '
        '_lvwCoinsurers_ColumnHeader_2
        '
        Me._lvwCoinsurers_ColumnHeader_2.Text = "% of Payment"
        Me._lvwCoinsurers_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_2.Width = 134
        '
        '_lvwCoinsurers_ColumnHeader_3
        '
        Me._lvwCoinsurers_ColumnHeader_3.Text = "Total Payment"
        Me._lvwCoinsurers_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_3.Width = 134
        '
        '_lvwCoinsurers_ColumnHeader_4
        '
        Me._lvwCoinsurers_ColumnHeader_4.Text = "Total This Payment Allocated"
        Me._lvwCoinsurers_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_4.Width = 201
        '
        'lblTotalAllocated
        '
        Me.lblTotalAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalAllocated.Location = New System.Drawing.Point(184, 24)
        Me.lblTotalAllocated.Name = "lblTotalAllocated"
        Me.lblTotalAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalAllocated.Size = New System.Drawing.Size(105, 25)
        Me.lblTotalAllocated.TabIndex = 62
        Me.lblTotalAllocated.Text = "Total Allocated:"
        '
        'lblPercentageAllocated
        '
        Me.lblPercentageAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentageAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentageAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentageAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentageAllocated.Location = New System.Drawing.Point(8, 24)
        Me.lblPercentageAllocated.Name = "lblPercentageAllocated"
        Me.lblPercentageAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentageAllocated.Size = New System.Drawing.Size(81, 25)
        Me.lblPercentageAllocated.TabIndex = 60
        Me.lblPercentageAllocated.Text = "% Allocated:"
        '
        'uctCLMPayment1
        '
        Me.Controls.Add(Me.SSTab1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMPayment1"
        Me.Size = New System.Drawing.Size(871, 800)
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraSettlement.ResumeLayout(False)
        Me.fraClaimInformation.ResumeLayout(False)
        Me.fraClaimInformation.PerformLayout()
        Me.fraPayee.ResumeLayout(False)
        Me.fraPayee.PerformLayout()
        Me.fraInsuredTaxAdjustment.ResumeLayout(False)
        Me.fraInsuredTaxAdjustment.PerformLayout()
        Me.fraPayeeTaxAdjustments.ResumeLayout(False)
        Me.fraPayeeTaxAdjustments.PerformLayout()
        Me.fraSafeHarbour.ResumeLayout(False)
        Me.fraSafeHarbour.PerformLayout()
        Me.fraExemptions.ResumeLayout(False)
        Me.fraPaymentDetails.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.fraThisPaymentSummary.ResumeLayout(False)
        Me.fraThisPaymentSummary.PerformLayout()
        Me.SSTab2.ResumeLayout(False)
        Me._SSTab2_TabPage0.ResumeLayout(False)
        Me._SSTab2_TabPage0.PerformLayout()
        Me._SSTab2_TabPage1.ResumeLayout(False)
        Me.fraThisPaymentComments.ResumeLayout(False)
        Me.fraThisPaymentComments.PerformLayout()
        Me.fraTaxesOnPayments.ResumeLayout(False)
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.fraCoinsurers.ResumeLayout(False)
        Me.fraCoinsurers.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtMediaType As System.Windows.Forms.TextBox
    Friend WithEvents chkIsExGratia As System.Windows.Forms.CheckBox
    Friend WithEvents lblBIC As System.Windows.Forms.Label
    Friend WithEvents txtBIC As System.Windows.Forms.TextBox
    Friend WithEvents lblIBAN As System.Windows.Forms.Label
    Friend WithEvents txtIBAN As System.Windows.Forms.TextBox
#End Region
#Region "Upgrade Support"
    <System.Runtime.InteropServices.ProgId("DataHasChangedEventArgs_NET.DataHasChangedEventArgs")> _
    Public NotInheritable Class DataHasChangedEventArgs
        Inherits System.EventArgs

        Public NewData(,) As Object
        Public Sub New(ByRef NewData(,) As Object)
            MyBase.New()
            Me.NewData = NewData
        End Sub
    End Class
#End Region
End Class
