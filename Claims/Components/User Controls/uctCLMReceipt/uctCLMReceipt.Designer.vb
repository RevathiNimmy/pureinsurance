<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMReceipt
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializelblRecoveryFilter()
		InitializecboRecoveryFilter()
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
	Friend WithEvents txtPeril As System.Windows.Forms.TextBox
	Friend WithEvents txtRiskType As System.Windows.Forms.TextBox
	Friend WithEvents txtLossCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtLossDate As System.Windows.Forms.TextBox
	Friend WithEvents lblPeril As System.Windows.Forms.Label
	Friend WithEvents lblRiskType As System.Windows.Forms.Label
	Friend WithEvents lblLossCurrency As System.Windows.Forms.Label
	Friend WithEvents lblDateOfLoss As System.Windows.Forms.Label
	Friend WithEvents fraClaimInformation As System.Windows.Forms.GroupBox
	Friend WithEvents chkSettlement As System.Windows.Forms.CheckBox
	Friend WithEvents fraSettlement As System.Windows.Forms.GroupBox
	Friend WithEvents OptInsurer As System.Windows.Forms.RadioButton
	Friend WithEvents OptParty As System.Windows.Forms.RadioButton
	Friend WithEvents OptClient As System.Windows.Forms.RadioButton
	Friend WithEvents OptAgent As System.Windows.Forms.RadioButton
	Friend WithEvents OptClaimReceivable As System.Windows.Forms.RadioButton
	Friend WithEvents txtParty As System.Windows.Forms.TextBox
	Friend WithEvents cmdParty As System.Windows.Forms.Button
	Friend WithEvents lblParty As System.Windows.Forms.Label
	Friend WithEvents fraPayee As System.Windows.Forms.GroupBox
	Friend WithEvents txtReceivableTaxPercentage As System.Windows.Forms.TextBox
	Friend WithEvents chkTaxExempt As System.Windows.Forms.CheckBox
	Friend WithEvents lblReceivableTaxPercentage As System.Windows.Forms.Label
	Friend WithEvents fraReceivableTaxStatus As System.Windows.Forms.GroupBox
	Friend WithEvents txtITTaxNo As System.Windows.Forms.TextBox
	Friend WithEvents txtITPercentage As System.Windows.Forms.TextBox
	Friend WithEvents chkITDomiciled As System.Windows.Forms.CheckBox
	Friend WithEvents lblITTaxNo As System.Windows.Forms.Label
	Friend WithEvents lblITPercentage As System.Windows.Forms.Label
	Friend WithEvents fraInsuredTaxAdjustment As System.Windows.Forms.GroupBox
	Friend WithEvents cmdEditPayee As System.Windows.Forms.Button
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents _lvwRecovery_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwRecovery_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwRecovery As System.Windows.Forms.ListView
	Friend WithEvents fraRecovery As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents _lblRecoveryFilter_0 As System.Windows.Forms.Label
	Friend WithEvents _lvwCoinsurance_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurance_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurance_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCoinsurance_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwCoinsurance As System.Windows.Forms.ListView
	Friend WithEvents _cboRecoveryFilter_0 As System.Windows.Forms.ComboBox
	Friend WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents _lblRecoveryFilter_1 As System.Windows.Forms.Label
	Friend WithEvents _lvwReinsurance_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwReinsurance_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwReinsurance_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwReinsurance_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwReinsurance As System.Windows.Forms.ListView
	Friend WithEvents _cboRecoveryFilter_1 As System.Windows.Forms.ComboBox
	Friend WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
	Friend WithEvents txtGrossReceipt As System.Windows.Forms.TextBox
	Friend WithEvents txtTotalTax As System.Windows.Forms.TextBox
	Friend WithEvents txtNetReceipt As System.Windows.Forms.TextBox
	Friend WithEvents lblGrossReceipt As System.Windows.Forms.Label
	Friend WithEvents lblTotalTax As System.Windows.Forms.Label
	Friend WithEvents lblNetReceipt As System.Windows.Forms.Label
	Friend WithEvents fraReceiptSummary As System.Windows.Forms.GroupBox
	Friend WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Friend WithEvents txtMediaRef As System.Windows.Forms.TextBox
	Friend WithEvents txtBankName As System.Windows.Forms.TextBox
	Friend WithEvents txtBankCode As System.Windows.Forms.TextBox
	Friend WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Friend WithEvents cboCountry As System.Windows.Forms.ComboBox
	Friend WithEvents txtBankAccountNo As System.Windows.Forms.TextBox
	Friend WithEvents txtMediaType As System.Windows.Forms.TextBox
	Friend WithEvents txtCountry As System.Windows.Forms.TextBox
	Friend WithEvents lblPayeeName As System.Windows.Forms.Label
	Friend WithEvents lblMediaRef As System.Windows.Forms.Label
	Friend WithEvents lblBankName As System.Windows.Forms.Label
	Friend WithEvents lblBankCode As System.Windows.Forms.Label
	Friend WithEvents lblMediaType As System.Windows.Forms.Label
	Friend WithEvents lblCountry As System.Windows.Forms.Label
	Friend WithEvents lblBankAccountNo As System.Windows.Forms.Label
	Friend WithEvents fraReceiptDetails As System.Windows.Forms.GroupBox
	Friend WithEvents txtPayeeComments As System.Windows.Forms.TextBox
	Friend WithEvents fraReceiptComments As System.Windows.Forms.GroupBox
	Friend WithEvents lvwTaxesOnThisReceipt As System.Windows.Forms.ListView
	Friend WithEvents fraTaxesOnReceipt As System.Windows.Forms.GroupBox
	Friend WithEvents _SSTab1_TabPage3 As System.Windows.Forms.TabPage
	Friend WithEvents SSTab1 As System.Windows.Forms.TabControl
	Friend cboRecoveryFilter(1) As System.Windows.Forms.ComboBox
	Friend lblRecoveryFilter(1) As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraClaimInformation = New System.Windows.Forms.GroupBox
        Me.txtPeril = New System.Windows.Forms.TextBox
        Me.txtRiskType = New System.Windows.Forms.TextBox
        Me.txtLossCurrency = New System.Windows.Forms.TextBox
        Me.txtLossDate = New System.Windows.Forms.TextBox
        Me.lblPeril = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblLossCurrency = New System.Windows.Forms.Label
        Me.lblDateOfLoss = New System.Windows.Forms.Label
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraSettlement = New System.Windows.Forms.GroupBox
        Me.chkSettlement = New System.Windows.Forms.CheckBox
        Me.fraPayee = New System.Windows.Forms.GroupBox
        Me.OptInsurer = New System.Windows.Forms.RadioButton
        Me.OptParty = New System.Windows.Forms.RadioButton
        Me.OptClient = New System.Windows.Forms.RadioButton
        Me.OptAgent = New System.Windows.Forms.RadioButton
        Me.OptClaimReceivable = New System.Windows.Forms.RadioButton
        Me.txtParty = New System.Windows.Forms.TextBox
        Me.cmdParty = New System.Windows.Forms.Button
        Me.lblParty = New System.Windows.Forms.Label
        Me.fraReceivableTaxStatus = New System.Windows.Forms.GroupBox
        Me.txtReceivableTaxPercentage = New System.Windows.Forms.TextBox
        Me.chkTaxExempt = New System.Windows.Forms.CheckBox
        Me.lblReceivableTaxPercentage = New System.Windows.Forms.Label
        Me.fraInsuredTaxAdjustment = New System.Windows.Forms.GroupBox
        Me.txtITTaxNo = New System.Windows.Forms.TextBox
        Me.txtITPercentage = New System.Windows.Forms.TextBox
        Me.chkITDomiciled = New System.Windows.Forms.CheckBox
        Me.lblITTaxNo = New System.Windows.Forms.Label
        Me.lblITPercentage = New System.Windows.Forms.Label
        Me.fraRecovery = New System.Windows.Forms.GroupBox
        Me.cmdEditPayee = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwRecovery = New System.Windows.Forms.ListView
        Me._lvwRecovery_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwRecovery_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me._lblRecoveryFilter_0 = New System.Windows.Forms.Label
        Me.lvwCoinsurance = New System.Windows.Forms.ListView
        Me._lvwCoinsurance_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurance_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._cboRecoveryFilter_0 = New System.Windows.Forms.ComboBox
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me._lblRecoveryFilter_1 = New System.Windows.Forms.Label
        Me.lvwReinsurance = New System.Windows.Forms.ListView
        Me._lvwReinsurance_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwReinsurance_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._cboRecoveryFilter_1 = New System.Windows.Forms.ComboBox
        Me._SSTab1_TabPage3 = New System.Windows.Forms.TabPage
        Me.fraReceiptSummary = New System.Windows.Forms.GroupBox
        Me.txtGrossReceipt = New System.Windows.Forms.TextBox
        Me.txtTotalTax = New System.Windows.Forms.TextBox
        Me.txtNetReceipt = New System.Windows.Forms.TextBox
        Me.lblGrossReceipt = New System.Windows.Forms.Label
        Me.lblTotalTax = New System.Windows.Forms.Label
        Me.lblNetReceipt = New System.Windows.Forms.Label
        Me.fraReceiptDetails = New System.Windows.Forms.GroupBox
        Me.txtPayeeName = New System.Windows.Forms.TextBox
        Me.txtMediaRef = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtBankCode = New System.Windows.Forms.TextBox
        Me.cboMediaType = New System.Windows.Forms.ComboBox
        Me.cboCountry = New System.Windows.Forms.ComboBox
        Me.txtBankAccountNo = New System.Windows.Forms.TextBox
        Me.txtMediaType = New System.Windows.Forms.TextBox
        Me.txtCountry = New System.Windows.Forms.TextBox
        Me.lblPayeeName = New System.Windows.Forms.Label
        Me.lblMediaRef = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblBankCode = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblBankAccountNo = New System.Windows.Forms.Label
        Me.fraReceiptComments = New System.Windows.Forms.GroupBox
        Me.txtPayeeComments = New System.Windows.Forms.TextBox
        Me.fraTaxesOnReceipt = New System.Windows.Forms.GroupBox
        Me.lvwTaxesOnThisReceipt = New System.Windows.Forms.ListView
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraClaimInformation.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraSettlement.SuspendLayout()
        Me.fraPayee.SuspendLayout()
        Me.fraReceivableTaxStatus.SuspendLayout()
        Me.fraInsuredTaxAdjustment.SuspendLayout()
        Me.fraRecovery.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me._SSTab1_TabPage3.SuspendLayout()
        Me.fraReceiptSummary.SuspendLayout()
        Me.fraReceiptDetails.SuspendLayout()
        Me.fraReceiptComments.SuspendLayout()
        Me.fraTaxesOnReceipt.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraClaimInformation
        '
        Me.fraClaimInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimInformation.Controls.Add(Me.txtPeril)
        Me.fraClaimInformation.Controls.Add(Me.txtRiskType)
        Me.fraClaimInformation.Controls.Add(Me.txtLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.txtLossDate)
        Me.fraClaimInformation.Controls.Add(Me.lblPeril)
        Me.fraClaimInformation.Controls.Add(Me.lblRiskType)
        Me.fraClaimInformation.Controls.Add(Me.lblLossCurrency)
        Me.fraClaimInformation.Controls.Add(Me.lblDateOfLoss)
        Me.fraClaimInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimInformation.Location = New System.Drawing.Point(8, 8)
        Me.fraClaimInformation.Name = "fraClaimInformation"
        Me.fraClaimInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimInformation.Size = New System.Drawing.Size(857, 49)
        Me.fraClaimInformation.TabIndex = 57
        Me.fraClaimInformation.TabStop = False
        Me.fraClaimInformation.Text = "Claim Information"
        '
        'txtPeril
        '
        Me.txtPeril.AcceptsReturn = True
        Me.txtPeril.BackColor = System.Drawing.SystemColors.Control
        Me.txtPeril.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeril.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPeril.Location = New System.Drawing.Point(288, 16)
        Me.txtPeril.MaxLength = 0
        Me.txtPeril.Name = "txtPeril"
        Me.txtPeril.ReadOnly = True
        Me.txtPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPeril.Size = New System.Drawing.Size(161, 21)
        Me.txtPeril.TabIndex = 63
        Me.txtPeril.TabStop = False
        '
        'txtRiskType
        '
        Me.txtRiskType.AcceptsReturn = True
        Me.txtRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.txtRiskType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskType.Location = New System.Drawing.Point(80, 16)
        Me.txtRiskType.MaxLength = 0
        Me.txtRiskType.Name = "txtRiskType"
        Me.txtRiskType.ReadOnly = True
        Me.txtRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskType.Size = New System.Drawing.Size(169, 21)
        Me.txtRiskType.TabIndex = 60
        Me.txtRiskType.TabStop = False
        '
        'txtLossCurrency
        '
        Me.txtLossCurrency.AcceptsReturn = True
        Me.txtLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossCurrency.Location = New System.Drawing.Point(544, 16)
        Me.txtLossCurrency.MaxLength = 0
        Me.txtLossCurrency.Name = "txtLossCurrency"
        Me.txtLossCurrency.ReadOnly = True
        Me.txtLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossCurrency.Size = New System.Drawing.Size(137, 21)
        Me.txtLossCurrency.TabIndex = 59
        Me.txtLossCurrency.TabStop = False
        '
        'txtLossDate
        '
        Me.txtLossDate.AcceptsReturn = True
        Me.txtLossDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtLossDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLossDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLossDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLossDate.Location = New System.Drawing.Point(768, 16)
        Me.txtLossDate.MaxLength = 0
        Me.txtLossDate.Name = "txtLossDate"
        Me.txtLossDate.ReadOnly = True
        Me.txtLossDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLossDate.Size = New System.Drawing.Size(81, 21)
        Me.txtLossDate.TabIndex = 58
        Me.txtLossDate.TabStop = False
        Me.txtLossDate.Text = "21/01/2005"
        '
        'lblPeril
        '
        Me.lblPeril.AutoSize = True
        Me.lblPeril.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeril.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeril.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeril.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeril.Location = New System.Drawing.Point(257, 20)
        Me.lblPeril.Name = "lblPeril"
        Me.lblPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeril.Size = New System.Drawing.Size(30, 13)
        Me.lblPeril.TabIndex = 0
        Me.lblPeril.Text = "Peril:"
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
        Me.lblRiskType.TabIndex = 3
        Me.lblRiskType.Text = "Risk Type:"
        '
        'lblLossCurrency
        '
        Me.lblLossCurrency.AutoSize = True
        Me.lblLossCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossCurrency.Location = New System.Drawing.Point(456, 20)
        Me.lblLossCurrency.Name = "lblLossCurrency"
        Me.lblLossCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossCurrency.Size = New System.Drawing.Size(77, 13)
        Me.lblLossCurrency.TabIndex = 1
        Me.lblLossCurrency.Text = "Loss Currency:"
        '
        'lblDateOfLoss
        '
        Me.lblDateOfLoss.AutoSize = True
        Me.lblDateOfLoss.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateOfLoss.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOfLoss.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOfLoss.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOfLoss.Location = New System.Drawing.Point(688, 20)
        Me.lblDateOfLoss.Name = "lblDateOfLoss"
        Me.lblDateOfLoss.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOfLoss.Size = New System.Drawing.Size(72, 13)
        Me.lblDateOfLoss.TabIndex = 2
        Me.lblDateOfLoss.Text = "Date Of Loss:"
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage3)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(216, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 64)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(874, 411)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 4
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraSettlement)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraPayee)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraReceivableTaxStatus)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraInsuredTaxAdjustment)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraRecovery)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(866, 385)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 -  Recovery Amounts"
        '
        'fraSettlement
        '
        Me.fraSettlement.BackColor = System.Drawing.SystemColors.Control
        Me.fraSettlement.Controls.Add(Me.chkSettlement)
        Me.fraSettlement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSettlement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSettlement.Location = New System.Drawing.Point(672, 4)
        Me.fraSettlement.Name = "fraSettlement"
        Me.fraSettlement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSettlement.Size = New System.Drawing.Size(193, 49)
        Me.fraSettlement.TabIndex = 13
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
        Me.chkSettlement.TabIndex = 14
        Me.chkSettlement.Text = "Settlement"
        Me.chkSettlement.UseVisualStyleBackColor = False
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.OptInsurer)
        Me.fraPayee.Controls.Add(Me.OptParty)
        Me.fraPayee.Controls.Add(Me.OptClient)
        Me.fraPayee.Controls.Add(Me.OptAgent)
        Me.fraPayee.Controls.Add(Me.OptClaimReceivable)
        Me.fraPayee.Controls.Add(Me.txtParty)
        Me.fraPayee.Controls.Add(Me.cmdParty)
        Me.fraPayee.Controls.Add(Me.lblParty)
        Me.fraPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(8, 4)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(657, 49)
        Me.fraPayee.TabIndex = 5
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payer"
        '
        'OptInsurer
        '
        Me.OptInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.OptInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptInsurer.Location = New System.Drawing.Point(276, 20)
        Me.OptInsurer.Name = "OptInsurer"
        Me.OptInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptInsurer.Size = New System.Drawing.Size(65, 17)
        Me.OptInsurer.TabIndex = 69
        Me.OptInsurer.TabStop = True
        Me.OptInsurer.Text = "Insurer"
        Me.OptInsurer.UseVisualStyleBackColor = False
        '
        'OptParty
        '
        Me.OptParty.BackColor = System.Drawing.SystemColors.Control
        Me.OptParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptParty.Location = New System.Drawing.Point(137, 20)
        Me.OptParty.Name = "OptParty"
        Me.OptParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptParty.Size = New System.Drawing.Size(60, 21)
        Me.OptParty.TabIndex = 7
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
        Me.OptClient.Location = New System.Drawing.Point(348, 20)
        Me.OptClient.Name = "OptClient"
        Me.OptClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClient.Size = New System.Drawing.Size(65, 17)
        Me.OptClient.TabIndex = 9
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
        Me.OptAgent.Location = New System.Drawing.Point(204, 20)
        Me.OptAgent.Name = "OptAgent"
        Me.OptAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAgent.Size = New System.Drawing.Size(65, 17)
        Me.OptAgent.TabIndex = 8
        Me.OptAgent.TabStop = True
        Me.OptAgent.Text = "Agent"
        Me.OptAgent.UseVisualStyleBackColor = False
        '
        'OptClaimReceivable
        '
        Me.OptClaimReceivable.BackColor = System.Drawing.SystemColors.Control
        Me.OptClaimReceivable.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClaimReceivable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClaimReceivable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClaimReceivable.Location = New System.Drawing.Point(8, 20)
        Me.OptClaimReceivable.Name = "OptClaimReceivable"
        Me.OptClaimReceivable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClaimReceivable.Size = New System.Drawing.Size(121, 17)
        Me.OptClaimReceivable.TabIndex = 6
        Me.OptClaimReceivable.TabStop = True
        Me.OptClaimReceivable.Text = "Claim Receivable"
        Me.OptClaimReceivable.UseVisualStyleBackColor = False
        '
        'txtParty
        '
        Me.txtParty.AcceptsReturn = True
        Me.txtParty.BackColor = System.Drawing.SystemColors.Window
        Me.txtParty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParty.Location = New System.Drawing.Point(472, 16)
        Me.txtParty.MaxLength = 255
        Me.txtParty.Name = "txtParty"
        Me.txtParty.ReadOnly = True
        Me.txtParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParty.Size = New System.Drawing.Size(152, 21)
        Me.txtParty.TabIndex = 11
        '
        'cmdParty
        '
        Me.cmdParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParty.Location = New System.Drawing.Point(624, 16)
        Me.cmdParty.Name = "cmdParty"
        Me.cmdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParty.Size = New System.Drawing.Size(25, 21)
        Me.cmdParty.TabIndex = 12
        Me.cmdParty.Text = "..."
        Me.cmdParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdParty.UseVisualStyleBackColor = False
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(424, 20)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(34, 13)
        Me.lblParty.TabIndex = 10
        Me.lblParty.Text = "Party:"
        '
        'fraReceivableTaxStatus
        '
        Me.fraReceivableTaxStatus.BackColor = System.Drawing.SystemColors.Control
        Me.fraReceivableTaxStatus.Controls.Add(Me.txtReceivableTaxPercentage)
        Me.fraReceivableTaxStatus.Controls.Add(Me.chkTaxExempt)
        Me.fraReceivableTaxStatus.Controls.Add(Me.lblReceivableTaxPercentage)
        Me.fraReceivableTaxStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReceivableTaxStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReceivableTaxStatus.Location = New System.Drawing.Point(8, 53)
        Me.fraReceivableTaxStatus.Name = "fraReceivableTaxStatus"
        Me.fraReceivableTaxStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReceivableTaxStatus.Size = New System.Drawing.Size(315, 49)
        Me.fraReceivableTaxStatus.TabIndex = 15
        Me.fraReceivableTaxStatus.TabStop = False
        Me.fraReceivableTaxStatus.Text = "Receivable Tax Status"
        '
        'txtReceivableTaxPercentage
        '
        Me.txtReceivableTaxPercentage.AcceptsReturn = True
        Me.txtReceivableTaxPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtReceivableTaxPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReceivableTaxPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceivableTaxPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReceivableTaxPercentage.Location = New System.Drawing.Point(232, 16)
        Me.txtReceivableTaxPercentage.MaxLength = 5
        Me.txtReceivableTaxPercentage.Name = "txtReceivableTaxPercentage"
        Me.txtReceivableTaxPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReceivableTaxPercentage.Size = New System.Drawing.Size(49, 21)
        Me.txtReceivableTaxPercentage.TabIndex = 18
        '
        'chkTaxExempt
        '
        Me.chkTaxExempt.BackColor = System.Drawing.SystemColors.Control
        Me.chkTaxExempt.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTaxExempt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTaxExempt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTaxExempt.Location = New System.Drawing.Point(8, 20)
        Me.chkTaxExempt.Name = "chkTaxExempt"
        Me.chkTaxExempt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTaxExempt.Size = New System.Drawing.Size(113, 17)
        Me.chkTaxExempt.TabIndex = 16
        Me.chkTaxExempt.Text = "Tax Exempt"
        Me.chkTaxExempt.UseVisualStyleBackColor = False
        '
        'lblReceivableTaxPercentage
        '
        Me.lblReceivableTaxPercentage.AutoSize = True
        Me.lblReceivableTaxPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceivableTaxPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceivableTaxPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceivableTaxPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceivableTaxPercentage.Location = New System.Drawing.Point(128, 20)
        Me.lblReceivableTaxPercentage.Name = "lblReceivableTaxPercentage"
        Me.lblReceivableTaxPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceivableTaxPercentage.Size = New System.Drawing.Size(86, 13)
        Me.lblReceivableTaxPercentage.TabIndex = 17
        Me.lblReceivableTaxPercentage.Text = "Tax Percentage:"
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
        Me.fraInsuredTaxAdjustment.Location = New System.Drawing.Point(328, 53)
        Me.fraInsuredTaxAdjustment.Name = "fraInsuredTaxAdjustment"
        Me.fraInsuredTaxAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsuredTaxAdjustment.Size = New System.Drawing.Size(539, 49)
        Me.fraInsuredTaxAdjustment.TabIndex = 19
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
        Me.txtITTaxNo.Location = New System.Drawing.Point(326, 16)
        Me.txtITTaxNo.MaxLength = 30
        Me.txtITTaxNo.Name = "txtITTaxNo"
        Me.txtITTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtITTaxNo.Size = New System.Drawing.Size(203, 21)
        Me.txtITTaxNo.TabIndex = 24
        '
        'txtITPercentage
        '
        Me.txtITPercentage.AcceptsReturn = True
        Me.txtITPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtITPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtITPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtITPercentage.Location = New System.Drawing.Point(216, 16)
        Me.txtITPercentage.MaxLength = 5
        Me.txtITPercentage.Name = "txtITPercentage"
        Me.txtITPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtITPercentage.Size = New System.Drawing.Size(49, 21)
        Me.txtITPercentage.TabIndex = 22
        '
        'chkITDomiciled
        '
        Me.chkITDomiciled.BackColor = System.Drawing.SystemColors.Control
        Me.chkITDomiciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkITDomiciled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkITDomiciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkITDomiciled.Location = New System.Drawing.Point(16, 20)
        Me.chkITDomiciled.Name = "chkITDomiciled"
        Me.chkITDomiciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkITDomiciled.Size = New System.Drawing.Size(81, 17)
        Me.chkITDomiciled.TabIndex = 20
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
        Me.lblITTaxNo.Location = New System.Drawing.Point(274, 20)
        Me.lblITTaxNo.Name = "lblITTaxNo"
        Me.lblITTaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblITTaxNo.Size = New System.Drawing.Size(45, 13)
        Me.lblITTaxNo.TabIndex = 23
        Me.lblITTaxNo.Text = "Tax No:"
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
        Me.lblITPercentage.Size = New System.Drawing.Size(86, 13)
        Me.lblITPercentage.TabIndex = 21
        Me.lblITPercentage.Text = "Tax Percentage:"
        '
        'fraRecovery
        '
        Me.fraRecovery.BackColor = System.Drawing.SystemColors.Control
        Me.fraRecovery.Controls.Add(Me.cmdEditPayee)
        Me.fraRecovery.Controls.Add(Me.cmdDelete)
        Me.fraRecovery.Controls.Add(Me.cmdEdit)
        Me.fraRecovery.Controls.Add(Me.lvwRecovery)
        Me.fraRecovery.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRecovery.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRecovery.Location = New System.Drawing.Point(8, 101)
        Me.fraRecovery.Name = "fraRecovery"
        Me.fraRecovery.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRecovery.Size = New System.Drawing.Size(857, 273)
        Me.fraRecovery.TabIndex = 25
        Me.fraRecovery.TabStop = False
        Me.fraRecovery.Text = "Reserves Paid and cost to claim"
        '
        'cmdEditPayee
        '
        Me.cmdEditPayee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditPayee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditPayee.Enabled = False
        Me.cmdEditPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditPayee.Location = New System.Drawing.Point(8, 237)
        Me.cmdEditPayee.Name = "cmdEditPayee"
        Me.cmdEditPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditPayee.Size = New System.Drawing.Size(169, 21)
        Me.cmdEditPayee.TabIndex = 64
        Me.cmdEditPayee.Text = "Edit &Payee And Tax Details"
        Me.cmdEditPayee.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditPayee.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(776, 237)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 28
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(696, 237)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 27
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwRecovery
        '
        Me.lvwRecovery.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRecovery.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRecovery, "")
        Me.lvwRecovery.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRecovery_ColumnHeader_1, Me._lvwRecovery_ColumnHeader_2, Me._lvwRecovery_ColumnHeader_3, Me._lvwRecovery_ColumnHeader_4, Me._lvwRecovery_ColumnHeader_5, Me._lvwRecovery_ColumnHeader_6, Me._lvwRecovery_ColumnHeader_7, Me._lvwRecovery_ColumnHeader_8, Me._lvwRecovery_ColumnHeader_9, Me._lvwRecovery_ColumnHeader_10})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRecovery, True)
        Me.lvwRecovery.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRecovery.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRecovery.FullRowSelect = True
        Me.lvwRecovery.GridLines = True
        Me.lvwRecovery.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRecovery, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRecovery, "")
        Me.lvwRecovery.Location = New System.Drawing.Point(8, 16)
        Me.lvwRecovery.Name = "lvwRecovery"
        Me.lvwRecovery.Size = New System.Drawing.Size(840, 213)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRecovery, "")
        Me.listViewHelper1.SetSorted(Me.lvwRecovery, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRecovery, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRecovery, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRecovery.TabIndex = 26
        Me.lvwRecovery.UseCompatibleStateImageBehavior = False
        Me.lvwRecovery.View = System.Windows.Forms.View.Details
        '
        '_lvwRecovery_ColumnHeader_1
        '
        Me._lvwRecovery_ColumnHeader_1.Text = "Recovery Type"
        Me._lvwRecovery_ColumnHeader_1.Width = 97
        '
        '_lvwRecovery_ColumnHeader_2
        '
        Me._lvwRecovery_ColumnHeader_2.Text = "Recovery Party"
        Me._lvwRecovery_ColumnHeader_2.Width = 97
        '
        '_lvwRecovery_ColumnHeader_3
        '
        Me._lvwRecovery_ColumnHeader_3.Text = "Total Reserve"
        Me._lvwRecovery_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_3.Width = 97
        '
        '_lvwRecovery_ColumnHeader_4
        '
        Me._lvwRecovery_ColumnHeader_4.Text = "Recovered To Date"
        Me._lvwRecovery_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_4.Width = 97
        '
        '_lvwRecovery_ColumnHeader_5
        '
        Me._lvwRecovery_ColumnHeader_5.Text = "This Recovery (Loss)"
        Me._lvwRecovery_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_5.Width = 97
        '
        '_lvwRecovery_ColumnHeader_6
        '
        Me._lvwRecovery_ColumnHeader_6.Text = "Balance"
        Me._lvwRecovery_ColumnHeader_6.Width = 97
        '
        '_lvwRecovery_ColumnHeader_7
        '
        Me._lvwRecovery_ColumnHeader_7.Text = "Tax Band"
        Me._lvwRecovery_ColumnHeader_7.Width = 97
        '
        '_lvwRecovery_ColumnHeader_8
        '
        Me._lvwRecovery_ColumnHeader_8.Text = "Tax Amount"
        Me._lvwRecovery_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_8.Width = 97
        '
        '_lvwRecovery_ColumnHeader_9
        '
        Me._lvwRecovery_ColumnHeader_9.Text = "Net Receipt"
        Me._lvwRecovery_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRecovery_ColumnHeader_9.Width = 97
        '
        '_lvwRecovery_ColumnHeader_10
        '
        Me._lvwRecovery_ColumnHeader_10.Width = 97
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me._lblRecoveryFilter_0)
        Me._SSTab1_TabPage1.Controls.Add(Me.lvwCoinsurance)
        Me._SSTab1_TabPage1.Controls.Add(Me._cboRecoveryFilter_0)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(866, 385)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "2 - Coinsurance Recovery"
        '
        '_lblRecoveryFilter_0
        '
        Me._lblRecoveryFilter_0.AutoSize = True
        Me._lblRecoveryFilter_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryFilter_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryFilter_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryFilter_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryFilter_0.Location = New System.Drawing.Point(8, 16)
        Me._lblRecoveryFilter_0.Name = "_lblRecoveryFilter_0"
        Me._lblRecoveryFilter_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryFilter_0.Size = New System.Drawing.Size(83, 13)
        Me._lblRecoveryFilter_0.TabIndex = 66
        Me._lblRecoveryFilter_0.Text = "Recovery Type:"
        '
        'lvwCoinsurance
        '
        Me.lvwCoinsurance.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCoinsurance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCoinsurance, "")
        Me.lvwCoinsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurance_ColumnHeader_1, Me._lvwCoinsurance_ColumnHeader_2, Me._lvwCoinsurance_ColumnHeader_3, Me._lvwCoinsurance_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCoinsurance, False)
        Me.lvwCoinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurance.FullRowSelect = True
        Me.lvwCoinsurance.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCoinsurance, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCoinsurance, "")
        Me.lvwCoinsurance.Location = New System.Drawing.Point(8, 44)
        Me.lvwCoinsurance.Name = "lvwCoinsurance"
        Me.lvwCoinsurance.Size = New System.Drawing.Size(840, 333)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCoinsurance, "")
        Me.listViewHelper1.SetSorted(Me.lvwCoinsurance, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCoinsurance, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCoinsurance, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCoinsurance.TabIndex = 29
        Me.lvwCoinsurance.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurance.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurance_ColumnHeader_1
        '
        Me._lvwCoinsurance_ColumnHeader_1.Text = "Coinsurer"
        Me._lvwCoinsurance_ColumnHeader_1.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_2
        '
        Me._lvwCoinsurance_ColumnHeader_2.Text = "Share %"
        Me._lvwCoinsurance_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_2.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_3
        '
        Me._lvwCoinsurance_ColumnHeader_3.Text = "Recovery To Date"
        Me._lvwCoinsurance_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_3.Width = 97
        '
        '_lvwCoinsurance_ColumnHeader_4
        '
        Me._lvwCoinsurance_ColumnHeader_4.Text = "This Recovery"
        Me._lvwCoinsurance_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurance_ColumnHeader_4.Width = 97
        '
        '_cboRecoveryFilter_0
        '
        Me._cboRecoveryFilter_0.BackColor = System.Drawing.SystemColors.Window
        Me._cboRecoveryFilter_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboRecoveryFilter_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboRecoveryFilter_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboRecoveryFilter_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboRecoveryFilter_0.Location = New System.Drawing.Point(104, 12)
        Me._cboRecoveryFilter_0.Name = "_cboRecoveryFilter_0"
        Me._cboRecoveryFilter_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboRecoveryFilter_0.Size = New System.Drawing.Size(217, 21)
        Me._cboRecoveryFilter_0.TabIndex = 65
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me._lblRecoveryFilter_1)
        Me._SSTab1_TabPage2.Controls.Add(Me.lvwReinsurance)
        Me._SSTab1_TabPage2.Controls.Add(Me._cboRecoveryFilter_1)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(866, 385)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "3 - Reinsurance Recovery"
        '
        '_lblRecoveryFilter_1
        '
        Me._lblRecoveryFilter_1.AutoSize = True
        Me._lblRecoveryFilter_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblRecoveryFilter_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRecoveryFilter_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRecoveryFilter_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRecoveryFilter_1.Location = New System.Drawing.Point(8, 16)
        Me._lblRecoveryFilter_1.Name = "_lblRecoveryFilter_1"
        Me._lblRecoveryFilter_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRecoveryFilter_1.Size = New System.Drawing.Size(83, 13)
        Me._lblRecoveryFilter_1.TabIndex = 68
        Me._lblRecoveryFilter_1.Text = "Recovery Type:"
        '
        'lvwReinsurance
        '
        Me.lvwReinsurance.BackColor = System.Drawing.SystemColors.Window
        Me.lvwReinsurance.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwReinsurance, "")
        Me.lvwReinsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReinsurance_ColumnHeader_1, Me._lvwReinsurance_ColumnHeader_2, Me._lvwReinsurance_ColumnHeader_3, Me._lvwReinsurance_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReinsurance, False)
        Me.lvwReinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReinsurance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReinsurance.FullRowSelect = True
        Me.lvwReinsurance.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwReinsurance, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwReinsurance, "")
        Me.lvwReinsurance.Location = New System.Drawing.Point(8, 44)
        Me.lvwReinsurance.Name = "lvwReinsurance"
        Me.lvwReinsurance.Size = New System.Drawing.Size(840, 333)
        Me.listViewHelper1.SetSmallIcons(Me.lvwReinsurance, "")
        Me.listViewHelper1.SetSorted(Me.lvwReinsurance, False)
        Me.listViewHelper1.SetSortKey(Me.lvwReinsurance, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwReinsurance, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwReinsurance.TabIndex = 30
        Me.lvwReinsurance.UseCompatibleStateImageBehavior = False
        Me.lvwReinsurance.View = System.Windows.Forms.View.Details
        '
        '_lvwReinsurance_ColumnHeader_1
        '
        Me._lvwReinsurance_ColumnHeader_1.Text = "Reinsurer"
        Me._lvwReinsurance_ColumnHeader_1.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_2
        '
        Me._lvwReinsurance_ColumnHeader_2.Text = "Share %"
        Me._lvwReinsurance_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_2.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_3
        '
        Me._lvwReinsurance_ColumnHeader_3.Text = "Recovered To Date"
        Me._lvwReinsurance_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_3.Width = 97
        '
        '_lvwReinsurance_ColumnHeader_4
        '
        Me._lvwReinsurance_ColumnHeader_4.Text = "This Recovery"
        Me._lvwReinsurance_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwReinsurance_ColumnHeader_4.Width = 97
        '
        '_cboRecoveryFilter_1
        '
        Me._cboRecoveryFilter_1.BackColor = System.Drawing.SystemColors.Window
        Me._cboRecoveryFilter_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboRecoveryFilter_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboRecoveryFilter_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboRecoveryFilter_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboRecoveryFilter_1.Location = New System.Drawing.Point(104, 12)
        Me._cboRecoveryFilter_1.Name = "_cboRecoveryFilter_1"
        Me._cboRecoveryFilter_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboRecoveryFilter_1.Size = New System.Drawing.Size(217, 21)
        Me._cboRecoveryFilter_1.TabIndex = 67
        '
        '_SSTab1_TabPage3
        '
        Me._SSTab1_TabPage3.Controls.Add(Me.fraReceiptSummary)
        Me._SSTab1_TabPage3.Controls.Add(Me.fraReceiptDetails)
        Me._SSTab1_TabPage3.Controls.Add(Me.fraReceiptComments)
        Me._SSTab1_TabPage3.Controls.Add(Me.fraTaxesOnReceipt)
        Me._SSTab1_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage3.Name = "_SSTab1_TabPage3"
        Me._SSTab1_TabPage3.Size = New System.Drawing.Size(866, 385)
        Me._SSTab1_TabPage3.TabIndex = 3
        Me._SSTab1_TabPage3.Text = "4 - This Receipt"
        '
        'fraReceiptSummary
        '
        Me.fraReceiptSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraReceiptSummary.Controls.Add(Me.txtGrossReceipt)
        Me.fraReceiptSummary.Controls.Add(Me.txtTotalTax)
        Me.fraReceiptSummary.Controls.Add(Me.txtNetReceipt)
        Me.fraReceiptSummary.Controls.Add(Me.lblGrossReceipt)
        Me.fraReceiptSummary.Controls.Add(Me.lblTotalTax)
        Me.fraReceiptSummary.Controls.Add(Me.lblNetReceipt)
        Me.fraReceiptSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReceiptSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReceiptSummary.Location = New System.Drawing.Point(8, 12)
        Me.fraReceiptSummary.Name = "fraReceiptSummary"
        Me.fraReceiptSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReceiptSummary.Size = New System.Drawing.Size(321, 153)
        Me.fraReceiptSummary.TabIndex = 31
        Me.fraReceiptSummary.TabStop = False
        Me.fraReceiptSummary.Text = "This Receipt Summary"
        '
        'txtGrossReceipt
        '
        Me.txtGrossReceipt.AcceptsReturn = True
        Me.txtGrossReceipt.BackColor = System.Drawing.SystemColors.Window
        Me.txtGrossReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGrossReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossReceipt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGrossReceipt.Location = New System.Drawing.Point(120, 22)
        Me.txtGrossReceipt.MaxLength = 0
        Me.txtGrossReceipt.Name = "txtGrossReceipt"
        Me.txtGrossReceipt.ReadOnly = True
        Me.txtGrossReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGrossReceipt.Size = New System.Drawing.Size(185, 21)
        Me.txtGrossReceipt.TabIndex = 33
        Me.txtGrossReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalTax
        '
        Me.txtTotalTax.AcceptsReturn = True
        Me.txtTotalTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalTax.Location = New System.Drawing.Point(120, 55)
        Me.txtTotalTax.MaxLength = 0
        Me.txtTotalTax.Name = "txtTotalTax"
        Me.txtTotalTax.ReadOnly = True
        Me.txtTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalTax.Size = New System.Drawing.Size(185, 21)
        Me.txtTotalTax.TabIndex = 35
        Me.txtTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNetReceipt
        '
        Me.txtNetReceipt.AcceptsReturn = True
        Me.txtNetReceipt.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetReceipt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetReceipt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNetReceipt.Location = New System.Drawing.Point(120, 88)
        Me.txtNetReceipt.MaxLength = 0
        Me.txtNetReceipt.Name = "txtNetReceipt"
        Me.txtNetReceipt.ReadOnly = True
        Me.txtNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNetReceipt.Size = New System.Drawing.Size(185, 21)
        Me.txtNetReceipt.TabIndex = 37
        Me.txtNetReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblGrossReceipt
        '
        Me.lblGrossReceipt.AutoSize = True
        Me.lblGrossReceipt.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossReceipt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossReceipt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossReceipt.Location = New System.Drawing.Point(24, 26)
        Me.lblGrossReceipt.Name = "lblGrossReceipt"
        Me.lblGrossReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossReceipt.Size = New System.Drawing.Size(77, 13)
        Me.lblGrossReceipt.TabIndex = 32
        Me.lblGrossReceipt.Text = "Gross Receipt:"
        '
        'lblTotalTax
        '
        Me.lblTotalTax.AutoSize = True
        Me.lblTotalTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalTax.Location = New System.Drawing.Point(52, 59)
        Me.lblTotalTax.Name = "lblTotalTax"
        Me.lblTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalTax.Size = New System.Drawing.Size(55, 13)
        Me.lblTotalTax.TabIndex = 34
        Me.lblTotalTax.Text = "Total Tax:"
        '
        'lblNetReceipt
        '
        Me.lblNetReceipt.AutoSize = True
        Me.lblNetReceipt.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetReceipt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetReceipt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetReceipt.Location = New System.Drawing.Point(40, 92)
        Me.lblNetReceipt.Name = "lblNetReceipt"
        Me.lblNetReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetReceipt.Size = New System.Drawing.Size(67, 13)
        Me.lblNetReceipt.TabIndex = 36
        Me.lblNetReceipt.Text = "Net Receipt:"
        '
        'fraReceiptDetails
        '
        Me.fraReceiptDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraReceiptDetails.Controls.Add(Me.txtPayeeName)
        Me.fraReceiptDetails.Controls.Add(Me.txtMediaRef)
        Me.fraReceiptDetails.Controls.Add(Me.txtBankName)
        Me.fraReceiptDetails.Controls.Add(Me.txtBankCode)
        Me.fraReceiptDetails.Controls.Add(Me.cboMediaType)
        Me.fraReceiptDetails.Controls.Add(Me.cboCountry)
        Me.fraReceiptDetails.Controls.Add(Me.txtBankAccountNo)
        Me.fraReceiptDetails.Controls.Add(Me.txtMediaType)
        Me.fraReceiptDetails.Controls.Add(Me.txtCountry)
        Me.fraReceiptDetails.Controls.Add(Me.lblPayeeName)
        Me.fraReceiptDetails.Controls.Add(Me.lblMediaRef)
        Me.fraReceiptDetails.Controls.Add(Me.lblBankName)
        Me.fraReceiptDetails.Controls.Add(Me.lblBankCode)
        Me.fraReceiptDetails.Controls.Add(Me.lblMediaType)
        Me.fraReceiptDetails.Controls.Add(Me.lblCountry)
        Me.fraReceiptDetails.Controls.Add(Me.lblBankAccountNo)
        Me.fraReceiptDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReceiptDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReceiptDetails.Location = New System.Drawing.Point(8, 164)
        Me.fraReceiptDetails.Name = "fraReceiptDetails"
        Me.fraReceiptDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReceiptDetails.Size = New System.Drawing.Size(321, 220)
        Me.fraReceiptDetails.TabIndex = 40
        Me.fraReceiptDetails.TabStop = False
        Me.fraReceiptDetails.Text = "Receipt Details"
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
        Me.txtPayeeName.TabIndex = 44
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
        Me.txtMediaRef.TabIndex = 46
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
        Me.txtBankName.TabIndex = 48
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
        Me.txtBankCode.TabIndex = 52
        '
        'cboMediaType
        '
        Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMediaType.Location = New System.Drawing.Point(120, 24)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMediaType.Size = New System.Drawing.Size(185, 21)
        Me.cboMediaType.TabIndex = 42
        '
        'cboCountry
        '
        Me.cboCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cboCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCountry.Location = New System.Drawing.Point(120, 192)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCountry.Size = New System.Drawing.Size(185, 21)
        Me.cboCountry.TabIndex = 54
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
        Me.txtBankAccountNo.TabIndex = 50
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
        Me.txtMediaType.TabIndex = 61
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
        Me.txtCountry.TabIndex = 62
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
        Me.lblPayeeName.TabIndex = 43
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
        Me.lblMediaRef.TabIndex = 45
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
        Me.lblBankName.TabIndex = 47
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
        Me.lblBankCode.TabIndex = 51
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
        Me.lblMediaType.TabIndex = 41
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
        Me.lblCountry.TabIndex = 53
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
        Me.lblBankAccountNo.TabIndex = 49
        Me.lblBankAccountNo.Text = "Bank Account No:"
        '
        'fraReceiptComments
        '
        Me.fraReceiptComments.BackColor = System.Drawing.SystemColors.Control
        Me.fraReceiptComments.Controls.Add(Me.txtPayeeComments)
        Me.fraReceiptComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReceiptComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReceiptComments.Location = New System.Drawing.Point(336, 164)
        Me.fraReceiptComments.Name = "fraReceiptComments"
        Me.fraReceiptComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReceiptComments.Size = New System.Drawing.Size(529, 220)
        Me.fraReceiptComments.TabIndex = 55
        Me.fraReceiptComments.TabStop = False
        Me.fraReceiptComments.Text = "Receipt Comments"
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
        Me.txtPayeeComments.Size = New System.Drawing.Size(513, 171)
        Me.txtPayeeComments.TabIndex = 56
        '
        'fraTaxesOnReceipt
        '
        Me.fraTaxesOnReceipt.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaxesOnReceipt.Controls.Add(Me.lvwTaxesOnThisReceipt)
        Me.fraTaxesOnReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaxesOnReceipt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaxesOnReceipt.Location = New System.Drawing.Point(336, 12)
        Me.fraTaxesOnReceipt.Name = "fraTaxesOnReceipt"
        Me.fraTaxesOnReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaxesOnReceipt.Size = New System.Drawing.Size(529, 153)
        Me.fraTaxesOnReceipt.TabIndex = 38
        Me.fraTaxesOnReceipt.TabStop = False
        Me.fraTaxesOnReceipt.Text = "Taxes On This Receipt"
        '
        'lvwTaxesOnThisReceipt
        '
        Me.lvwTaxesOnThisReceipt.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTaxesOnThisReceipt, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTaxesOnThisReceipt, False)
        Me.lvwTaxesOnThisReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTaxesOnThisReceipt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTaxesOnThisReceipt.FullRowSelect = True
        Me.lvwTaxesOnThisReceipt.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTaxesOnThisReceipt, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTaxesOnThisReceipt, "")
        Me.lvwTaxesOnThisReceipt.Location = New System.Drawing.Point(8, 16)
        Me.lvwTaxesOnThisReceipt.Name = "lvwTaxesOnThisReceipt"
        Me.lvwTaxesOnThisReceipt.Size = New System.Drawing.Size(513, 124)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTaxesOnThisReceipt, "")
        Me.listViewHelper1.SetSorted(Me.lvwTaxesOnThisReceipt, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTaxesOnThisReceipt, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTaxesOnThisReceipt, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTaxesOnThisReceipt.TabIndex = 39
        Me.lvwTaxesOnThisReceipt.UseCompatibleStateImageBehavior = False
        Me.lvwTaxesOnThisReceipt.View = System.Windows.Forms.View.Details
        '
        'uctCLMReceipt
        '
        Me.Controls.Add(Me.fraClaimInformation)
        Me.Controls.Add(Me.SSTab1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctCLMReceipt"
        Me.Size = New System.Drawing.Size(880, 474)
        Me.fraClaimInformation.ResumeLayout(False)
        Me.fraClaimInformation.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraSettlement.ResumeLayout(False)
        Me.fraPayee.ResumeLayout(False)
        Me.fraPayee.PerformLayout()
        Me.fraReceivableTaxStatus.ResumeLayout(False)
        Me.fraReceivableTaxStatus.PerformLayout()
        Me.fraInsuredTaxAdjustment.ResumeLayout(False)
        Me.fraInsuredTaxAdjustment.PerformLayout()
        Me.fraRecovery.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me._SSTab1_TabPage1.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me._SSTab1_TabPage2.PerformLayout()
        Me._SSTab1_TabPage3.ResumeLayout(False)
        Me.fraReceiptSummary.ResumeLayout(False)
        Me.fraReceiptSummary.PerformLayout()
        Me.fraReceiptDetails.ResumeLayout(False)
        Me.fraReceiptDetails.PerformLayout()
        Me.fraReceiptComments.ResumeLayout(False)
        Me.fraTaxesOnReceipt.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializelblRecoveryFilter()
		Me.lblRecoveryFilter(1) = _lblRecoveryFilter_1
		Me.lblRecoveryFilter(0) = _lblRecoveryFilter_0
	End Sub
	Sub InitializecboRecoveryFilter()
		Me.cboRecoveryFilter(1) = _cboRecoveryFilter_1
		Me.cboRecoveryFilter(0) = _cboRecoveryFilter_0
    End Sub

    Private Sub UserControl_Initialize()

    End Sub
#End Region
End Class