<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializetxtOpenClaim()
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
    Public WithEvents cmdChangeClientPolicy As System.Windows.Forms.Button
    Private WithEvents ClaimVersionsEvents As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripSeparator
    Private Shadows WithEvents Events As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents RiskDetails As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents InformationChecklist As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button8 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents Financial As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button10 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents DocArchive As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblClaimHandled As System.Windows.Forms.Label
    Public WithEvents lblClaimVersion As System.Windows.Forms.Label
    Public WithEvents lblAccountExec As System.Windows.Forms.Label
    Public WithEvents lblAccountHandler As System.Windows.Forms.Label
    Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Public WithEvents lblUnderwritingYearID As System.Windows.Forms.Label
    Public WithEvents lblLikelyToClaim As System.Windows.Forms.Label
    Public WithEvents lblInformation As System.Windows.Forms.Label
    Public WithEvents lblPrimaryCausationCode As System.Windows.Forms.Label
    Public WithEvents lblSecondaryCausationCode As System.Windows.Forms.Label
    Public WithEvents lblClaimNumber As System.Windows.Forms.Label
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents lblLossDate As System.Windows.Forms.Label
    Public WithEvents lblLossToDate As System.Windows.Forms.Label
    Public WithEvents lblTown As System.Windows.Forms.Label
    Public WithEvents lblLocation As System.Windows.Forms.Label
    Public WithEvents lblReportedDate As System.Windows.Forms.Label
    Public WithEvents lblReportedToDate As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblLastModified As System.Windows.Forms.Label
    Public WithEvents lblClaimStatusDate As System.Windows.Forms.Label
    Public WithEvents lblProgressStatus As System.Windows.Forms.Label
    Public WithEvents lblHandler As System.Windows.Forms.Label
    Public WithEvents lblCatastropheCode As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblCaseNumber As System.Windows.Forms.Label
    Private WithEvents _txtOpenClaim_43 As System.Windows.Forms.TextBox


    Public WithEvents txtCaseNumber As System.Windows.Forms.TextBox
    Public WithEvents chkClaimHandled_old As System.Windows.Forms.CheckBox
    Public WithEvents txtClaimVersion As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_45 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_44 As System.Windows.Forms.TextBox
    Public WithEvents cboUnderwritingYearID As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_10 As System.Windows.Forms.TextBox
    Public WithEvents chkInfoOnly As System.Windows.Forms.CheckBox
    Public WithEvents chkLikelyClaim As System.Windows.Forms.CheckBox
    Public WithEvents cboPrimaryCausationCode As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_0 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_1 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_2 As System.Windows.Forms.TextBox
    Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
    Public WithEvents cboRiskType As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_5 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_6 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_9 As System.Windows.Forms.TextBox
    Public WithEvents cboTown As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_4 As System.Windows.Forms.TextBox
    Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_7 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_8 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_12 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_13 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_11 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_3 As System.Windows.Forms.TextBox
    Public WithEvents cboProgressStatus As System.Windows.Forms.ComboBox
    Public WithEvents cboHandler As System.Windows.Forms.ComboBox
    Public WithEvents cboCatastropheCode As System.Windows.Forms.ComboBox
    Public WithEvents cboSecondaryCausationCode As System.Windows.Forms.ComboBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cboStandardExcess As System.Windows.Forms.ComboBox
    Private WithEvents _txtOpenClaim_41 As System.Windows.Forms.TextBox
    Public WithEvents lblStandardExcess As System.Windows.Forms.Label
    Public WithEvents lblNonStandardExcess As System.Windows.Forms.Label
    Public WithEvents fraDeductibles As System.Windows.Forms.GroupBox
    Public WithEvents cboAtFault As System.Windows.Forms.ComboBox
    Public WithEvents chkBonusAffected As System.Windows.Forms.CheckBox
    Public WithEvents lblAtFault As System.Windows.Forms.Label
    Public WithEvents fraNCDDetails As System.Windows.Forms.GroupBox
    Public WithEvents chkULR As System.Windows.Forms.CheckBox
    Private WithEvents _txtOpenClaim_38 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_39 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_40 As System.Windows.Forms.TextBox
    Public WithEvents chkSolicitorAppointed As System.Windows.Forms.CheckBox
    Public WithEvents lblRecoveryAgent As System.Windows.Forms.Label
    Public WithEvents lblSolicitorName As System.Windows.Forms.Label
    Public WithEvents lblLossDetails As System.Windows.Forms.Label
    Public WithEvents fraULRDetails As System.Windows.Forms.GroupBox
    Private WithEvents _txtOpenClaim_37 As System.Windows.Forms.TextBox
    Public WithEvents lblPreviousClaimDetails As System.Windows.Forms.Label
    Public WithEvents fraPreviousClaimDetails As System.Windows.Forms.GroupBox
    Private WithEvents _txtOpenClaim_34 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_35 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_36 As System.Windows.Forms.TextBox
    Public WithEvents chkPreviousClaim As System.Windows.Forms.CheckBox

    Public WithEvents ddEmployeeTitle As PMListMgrDropdown.uctDropdown
    Public WithEvents lblEmployeeTitle As System.Windows.Forms.Label
    Public WithEvents lblEmployeeForename As System.Windows.Forms.Label
    Public WithEvents lblEmployeeSurname As System.Windows.Forms.Label
    Public WithEvents lblLengthOfService As System.Windows.Forms.Label
    Public WithEvents fraEmployeeDetails As System.Windows.Forms.GroupBox
    Private WithEvents _txtOpenClaim_31 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_32 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_33 As System.Windows.Forms.TextBox


    Public WithEvents ddDriverTitle As PMListMgrDropdown.uctDropdown
    Public WithEvents lblDriverTitle As System.Windows.Forms.Label
    Public WithEvents lblDriverForename As System.Windows.Forms.Label
    Public WithEvents lblDriverSurname As System.Windows.Forms.Label
    Public WithEvents lblDatePassedTest As System.Windows.Forms.Label
    Public WithEvents fraInsuredDriverDetails As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents lblUserDefinedFieldE As System.Windows.Forms.Label
    Public WithEvents lblUserDefinedFieldD As System.Windows.Forms.Label
    Public WithEvents lblUserDefinedFieldC As System.Windows.Forms.Label
    Public WithEvents lblUserDefinedFieldB As System.Windows.Forms.Label
    Public WithEvents lblUserDefinedFieldA As System.Windows.Forms.Label
    Public WithEvents lblSubsidiaryCompany As System.Windows.Forms.Label
    Public WithEvents lblClientName As System.Windows.Forms.Label
    Public WithEvents lblTelePhoneNumberH As System.Windows.Forms.Label
    Public WithEvents lblTelePhoneNumberO As System.Windows.Forms.Label
    Public WithEvents lblFaxNumber As System.Windows.Forms.Label
    Public WithEvents lblMobileNumber As System.Windows.Forms.Label
    Public WithEvents lblEmailAddress As System.Windows.Forms.Label
    Public WithEvents lblVAT As System.Windows.Forms.Label
    Public WithEvents lblVATRegistartionNumber As System.Windows.Forms.Label
    Public WithEvents lblClientClaimNumber As System.Windows.Forms.Label
    Public WithEvents cboUDFA As uctGISUserDefLookupControl.cboGISLookup
    Public WithEvents cboUDFB As uctGISUserDefLookupControl.cboGISLookup
    Public WithEvents cboUDFC As uctGISUserDefLookupControl.cboGISLookup
    Public WithEvents cboUDFD As uctGISUserDefLookupControl.cboGISLookup
    Public WithEvents cboUDFE As uctGISUserDefLookupControl.cboGISLookup
    Private WithEvents _txtOpenClaim_42 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_18 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_23 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_22 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_21 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_20 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_19 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_17 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_16 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_15 As System.Windows.Forms.TextBox
    Public WithEvents CmdClient As System.Windows.Forms.Button
    Public WithEvents chkVATRegistered As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents lblInsurerName As System.Windows.Forms.Label
    Public WithEvents lblEmailIns As System.Windows.Forms.Label
    Public WithEvents lblContact As System.Windows.Forms.Label
    Public WithEvents lblIFaxNumber As System.Windows.Forms.Label
    Public WithEvents lblTelephoneNumber As System.Windows.Forms.Label
    Public WithEvents lblInsurerClaimNumber As System.Windows.Forms.Label
    Public WithEvents txtAllocated As System.Windows.Forms.TextBox
    Public WithEvents txtFormatPercent As System.Windows.Forms.TextBox
    Private WithEvents _lvwCoinsurers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCoinsurers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwCoinsurers As System.Windows.Forms.ListView
    Public WithEvents lblAllocated As System.Windows.Forms.Label
    Public WithEvents fraCoinsurers As System.Windows.Forms.GroupBox
    Private WithEvents _txtOpenClaim_25 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_26 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_27 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_28 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_29 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_30 As System.Windows.Forms.TextBox
    Private WithEvents _txtOpenClaim_24 As System.Windows.Forms.TextBox
    Public WithEvents cmdInsurer As System.Windows.Forms.Button
    Public WithEvents cmdInsurerDetails As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage


    Public WithEvents uctCLMListPaymentsC1 As uctCLMListPayments.uctCLMListPaymentsC
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public txtOpenClaim(45) As System.Windows.Forms.TextBox
    'Modified developer Guide no 32
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdChangeClientPolicy = New System.Windows.Forms.Button
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ClaimVersionsEvents = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripSeparator
        Me.Events = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripSeparator
        Me.RiskDetails = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me.InformationChecklist = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button8 = New System.Windows.Forms.ToolStripSeparator
        Me.Financial = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button10 = New System.Windows.Forms.ToolStripSeparator
        Me.DocArchive = New System.Windows.Forms.ToolStripButton
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.txtTPA = New System.Windows.Forms.TextBox
        Me.cmdTPA = New System.Windows.Forms.Button
        Me.lblClaimHandled = New System.Windows.Forms.Label
        Me.lblClaimVersion = New System.Windows.Forms.Label
        Me.lblAccountExec = New System.Windows.Forms.Label
        Me.lblAccountHandler = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblUnderwritingYearID = New System.Windows.Forms.Label
        Me.lblLikelyToClaim = New System.Windows.Forms.Label
        Me.lblInformation = New System.Windows.Forms.Label
        Me.lblPrimaryCausationCode = New System.Windows.Forms.Label
        Me.lblSecondaryCausationCode = New System.Windows.Forms.Label
        Me.lblClaimNumber = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblLossDate = New System.Windows.Forms.Label
        Me.lblLossToDate = New System.Windows.Forms.Label
        Me.lblTown = New System.Windows.Forms.Label
        Me.lblLocation = New System.Windows.Forms.Label
        Me.lblReportedDate = New System.Windows.Forms.Label
        Me.lblReportedToDate = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblLastModified = New System.Windows.Forms.Label
        Me.lblClaimStatusDate = New System.Windows.Forms.Label
        Me.lblProgressStatus = New System.Windows.Forms.Label
        Me.lblHandler = New System.Windows.Forms.Label
        Me.lblCatastropheCode = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblCaseNumber = New System.Windows.Forms.Label
        Me._txtOpenClaim_43 = New System.Windows.Forms.TextBox
        Me.chkClaimHandled = New GEMControlLib.YesNoCheck
        Me.txtCaseNumber = New System.Windows.Forms.TextBox
        Me.chkClaimHandled_old = New System.Windows.Forms.CheckBox
        Me.txtClaimVersion = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_45 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_44 = New System.Windows.Forms.TextBox
        Me.cboUnderwritingYearID = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_10 = New System.Windows.Forms.TextBox
        Me.chkInfoOnly = New System.Windows.Forms.CheckBox
        Me.chkLikelyClaim = New System.Windows.Forms.CheckBox
        Me.cboPrimaryCausationCode = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_0 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_1 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_2 = New System.Windows.Forms.TextBox
        Me.txtClaimNumber = New System.Windows.Forms.TextBox
        Me.cboRiskType = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_5 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_6 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_9 = New System.Windows.Forms.TextBox
        Me.cboTown = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_4 = New System.Windows.Forms.TextBox
        Me.cboCurrency = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_7 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_8 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_12 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_13 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_11 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_3 = New System.Windows.Forms.TextBox
        Me.cboProgressStatus = New System.Windows.Forms.ComboBox
        Me.cboHandler = New System.Windows.Forms.ComboBox
        Me.cboCatastropheCode = New System.Windows.Forms.ComboBox
        Me.cboSecondaryCausationCode = New System.Windows.Forms.ComboBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraDeductibles = New System.Windows.Forms.GroupBox
        Me.cboStandardExcess = New System.Windows.Forms.ComboBox
        Me._txtOpenClaim_41 = New System.Windows.Forms.TextBox
        Me.lblStandardExcess = New System.Windows.Forms.Label
        Me.lblNonStandardExcess = New System.Windows.Forms.Label
        Me.fraNCDDetails = New System.Windows.Forms.GroupBox
        Me.cboAtFault = New System.Windows.Forms.ComboBox
        Me.chkBonusAffected = New System.Windows.Forms.CheckBox
        Me.lblAtFault = New System.Windows.Forms.Label
        Me.fraULRDetails = New System.Windows.Forms.GroupBox
        Me.chkULR = New System.Windows.Forms.CheckBox
        Me._txtOpenClaim_38 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_39 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_40 = New System.Windows.Forms.TextBox
        Me.chkSolicitorAppointed = New System.Windows.Forms.CheckBox
        Me.lblRecoveryAgent = New System.Windows.Forms.Label
        Me.lblSolicitorName = New System.Windows.Forms.Label
        Me.lblLossDetails = New System.Windows.Forms.Label
        Me.fraEmployeeDetails = New System.Windows.Forms.GroupBox
        Me.fraPreviousClaimDetails = New System.Windows.Forms.GroupBox
        Me._txtOpenClaim_37 = New System.Windows.Forms.TextBox
        Me.lblPreviousClaimDetails = New System.Windows.Forms.Label
        Me._txtOpenClaim_34 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_35 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_36 = New System.Windows.Forms.TextBox
        Me.chkPreviousClaim = New System.Windows.Forms.CheckBox
        Me.ddEmployeeTitle = New PMListMgrDropdown.uctDropdown
        Me.lblEmployeeTitle = New System.Windows.Forms.Label
        Me.lblEmployeeForename = New System.Windows.Forms.Label
        Me.lblEmployeeSurname = New System.Windows.Forms.Label
        Me.lblLengthOfService = New System.Windows.Forms.Label
        Me.fraInsuredDriverDetails = New System.Windows.Forms.GroupBox
        Me._txtOpenClaim_31 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_32 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_33 = New System.Windows.Forms.TextBox
        Me.ddDriverTitle = New PMListMgrDropdown.uctDropdown
        Me.lblDriverTitle = New System.Windows.Forms.Label
        Me.lblDriverForename = New System.Windows.Forms.Label
        Me.lblDriverSurname = New System.Windows.Forms.Label
        Me.lblDatePassedTest = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.lblUserDefinedFieldE = New System.Windows.Forms.Label
        Me.lblUserDefinedFieldD = New System.Windows.Forms.Label
        Me.lblUserDefinedFieldC = New System.Windows.Forms.Label
        Me.lblUserDefinedFieldB = New System.Windows.Forms.Label
        Me.lblUserDefinedFieldA = New System.Windows.Forms.Label
        Me.lblSubsidiaryCompany = New System.Windows.Forms.Label
        Me.lblClientName = New System.Windows.Forms.Label
        Me.lblTelePhoneNumberH = New System.Windows.Forms.Label
        Me.lblTelePhoneNumberO = New System.Windows.Forms.Label
        Me.lblFaxNumber = New System.Windows.Forms.Label
        Me.lblMobileNumber = New System.Windows.Forms.Label
        Me.lblEmailAddress = New System.Windows.Forms.Label
        Me.lblVAT = New System.Windows.Forms.Label
        Me.lblVATRegistartionNumber = New System.Windows.Forms.Label
        Me.lblClientClaimNumber = New System.Windows.Forms.Label
        Me.cboUDFA = New uctGISUserDefLookupControl.cboGISLookup
        Me.cboUDFB = New uctGISUserDefLookupControl.cboGISLookup
        Me.cboUDFC = New uctGISUserDefLookupControl.cboGISLookup
        Me.cboUDFD = New uctGISUserDefLookupControl.cboGISLookup
        Me.cboUDFE = New uctGISUserDefLookupControl.cboGISLookup
        Me._txtOpenClaim_42 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_18 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_23 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_22 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_21 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_20 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_19 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_17 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_16 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_15 = New System.Windows.Forms.TextBox
        Me.CmdClient = New System.Windows.Forms.Button
        Me.chkVATRegistered = New System.Windows.Forms.CheckBox
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me.lblInsurerName = New System.Windows.Forms.Label
        Me.lblEmailIns = New System.Windows.Forms.Label
        Me.lblContact = New System.Windows.Forms.Label
        Me.lblIFaxNumber = New System.Windows.Forms.Label
        Me.lblTelephoneNumber = New System.Windows.Forms.Label
        Me.lblInsurerClaimNumber = New System.Windows.Forms.Label
        Me.fraCoinsurers = New System.Windows.Forms.GroupBox
        Me.txtAllocated = New System.Windows.Forms.TextBox
        Me.txtFormatPercent = New System.Windows.Forms.TextBox
        Me.lvwCoinsurers = New System.Windows.Forms.ListView
        Me._lvwCoinsurers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCoinsurers_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.lblAllocated = New System.Windows.Forms.Label
        Me._txtOpenClaim_25 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_26 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_27 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_28 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_29 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_30 = New System.Windows.Forms.TextBox
        Me._txtOpenClaim_24 = New System.Windows.Forms.TextBox
        Me.cmdInsurer = New System.Windows.Forms.Button
        Me.cmdInsurerDetails = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.uctCLMListPaymentsC1 = New uctCLMListPayments.uctCLMListPaymentsC
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraDeductibles.SuspendLayout()
        Me.fraNCDDetails.SuspendLayout()
        Me.fraULRDetails.SuspendLayout()
        Me.fraEmployeeDetails.SuspendLayout()
        Me.fraPreviousClaimDetails.SuspendLayout()
        Me.fraInsuredDriverDetails.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraCoinsurers.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdChangeClientPolicy
        '
        Me.cmdChangeClientPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangeClientPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangeClientPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangeClientPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangeClientPolicy.Location = New System.Drawing.Point(318, 460)
        Me.cmdChangeClientPolicy.Name = "cmdChangeClientPolicy"
        Me.cmdChangeClientPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangeClientPolicy.Size = New System.Drawing.Size(137, 22)
        Me.cmdChangeClientPolicy.TabIndex = 49
        Me.cmdChangeClientPolicy.Text = "Change Client/Policy"
        Me.cmdChangeClientPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangeClientPolicy.UseVisualStyleBackColor = False
        '
        'Toolbar1
        '
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClaimVersionsEvents, Me._Toolbar1_Button2, Me.Events, Me._Toolbar1_Button4, Me.RiskDetails, Me._Toolbar1_Button6, Me.InformationChecklist, Me._Toolbar1_Button8, Me.Financial, Me._Toolbar1_Button10, Me.DocArchive})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(659, 25)
        Me.Toolbar1.TabIndex = 55
        '
        'ClaimVersionsEvents
        '
        Me.ClaimVersionsEvents.AutoSize = False
        Me.ClaimVersionsEvents.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ClaimVersionsEvents.Name = "ClaimVersionsEvents"
        Me.ClaimVersionsEvents.Size = New System.Drawing.Size(24, 22)
        Me.ClaimVersionsEvents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ClaimVersionsEvents.ToolTipText = "Events recorded against all versions of this claim"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(6, 22)
        '
        'Events
        '
        Me.Events.AutoSize = False
        Me.Events.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Events.Name = "Events"
        Me.Events.Size = New System.Drawing.Size(24, 22)
        Me.Events.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Events.ToolTipText = "Events"
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(6, 22)
        '
        'RiskDetails
        '
        Me.RiskDetails.AutoSize = False
        Me.RiskDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.RiskDetails.Name = "RiskDetails"
        Me.RiskDetails.Size = New System.Drawing.Size(24, 22)
        Me.RiskDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.RiskDetails.ToolTipText = "Risk Details"
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(6, 22)
        '
        'InformationChecklist
        '
        Me.InformationChecklist.AutoSize = False
        Me.InformationChecklist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.InformationChecklist.Name = "InformationChecklist"
        Me.InformationChecklist.Size = New System.Drawing.Size(24, 22)
        Me.InformationChecklist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.InformationChecklist.ToolTipText = "Information Checklist"
        '
        '_Toolbar1_Button8
        '
        Me._Toolbar1_Button8.AutoSize = False
        Me._Toolbar1_Button8.Name = "_Toolbar1_Button8"
        Me._Toolbar1_Button8.Size = New System.Drawing.Size(6, 22)
        '
        'Financial
        '
        Me.Financial.AutoSize = False
        Me.Financial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.Financial.Name = "Financial"
        Me.Financial.Size = New System.Drawing.Size(24, 22)
        Me.Financial.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Financial.ToolTipText = "Financial Details"
        '
        '_Toolbar1_Button10
        '
        Me._Toolbar1_Button10.AutoSize = False
        Me._Toolbar1_Button10.Name = "_Toolbar1_Button10"
        Me._Toolbar1_Button10.Size = New System.Drawing.Size(6, 22)
        '
        'DocArchive
        '
        Me.DocArchive.AutoSize = False
        Me.DocArchive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.DocArchive.Name = "DocArchive"
        Me.DocArchive.Size = New System.Drawing.Size(24, 22)
        Me.DocArchive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.DocArchive.ToolTipText = "Document"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(551, 460)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 48
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(472, 460)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 47
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(101, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(639, 422)
        Me.tabMainTab.TabIndex = 56
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTPA)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdTPA)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimHandled)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountExec)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountHandler)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPolicyNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUnderwritingYearID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLikelyToClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInformation)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrimaryCausationCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSecondaryCausationCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLossDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLossToDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTown)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLocation)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReportedDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReportedToDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLastModified)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimStatusDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProgressStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblHandler)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCatastropheCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCaseNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_43)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkClaimHandled)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCaseNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkClaimHandled_old)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_45)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_44)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboUnderwritingYearID)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_10)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkInfoOnly)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkLikelyClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPrimaryCausationCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_5)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_6)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_9)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTown)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_4)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_7)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_8)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_12)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_13)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_11)
        Me._tabMainTab_TabPage0.Controls.Add(Me._txtOpenClaim_3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProgressStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboHandler)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCatastropheCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSecondaryCausationCode)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(631, 396)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Claim Details"
        '
        'txtTPA
        '
        Me.txtTPA.AcceptsReturn = True
        Me.txtTPA.BackColor = System.Drawing.SystemColors.Window
        Me.txtTPA.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTPA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTPA.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTPA.Location = New System.Drawing.Point(120, 87)
        Me.txtTPA.MaxLength = 0
        Me.txtTPA.Name = "txtTPA"
        Me.txtTPA.ReadOnly = True
        Me.txtTPA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTPA.Size = New System.Drawing.Size(177, 20)
        Me.txtTPA.TabIndex = 144
        '
        'cmdTPA
        '
        Me.cmdTPA.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTPA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTPA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTPA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTPA.Location = New System.Drawing.Point(3, 85)
        Me.cmdTPA.Name = "cmdTPA"
        Me.cmdTPA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTPA.Size = New System.Drawing.Size(103, 22)
        Me.cmdTPA.TabIndex = 143
        Me.cmdTPA.Text = "TPA..."
        Me.cmdTPA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdTPA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTPA.UseVisualStyleBackColor = False
        '
        'lblClaimHandled
        '
        Me.lblClaimHandled.AutoSize = True
        Me.lblClaimHandled.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimHandled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimHandled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimHandled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimHandled.Location = New System.Drawing.Point(3, 369)
        Me.lblClaimHandled.Name = "lblClaimHandled"
        Me.lblClaimHandled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimHandled.Size = New System.Drawing.Size(104, 13)
        Me.lblClaimHandled.TabIndex = 137
        Me.lblClaimHandled.Text = "Claim Handled:"
        '
        'lblClaimVersion
        '
        Me.lblClaimVersion.AutoSize = True
        Me.lblClaimVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimVersion.Location = New System.Drawing.Point(460, 361)
        Me.lblClaimVersion.Name = "lblClaimVersion"
        Me.lblClaimVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimVersion.Size = New System.Drawing.Size(73, 13)
        Me.lblClaimVersion.TabIndex = 138
        Me.lblClaimVersion.Text = "Claim Version:"
        '
        'lblAccountExec
        '
        Me.lblAccountExec.AutoSize = True
        Me.lblAccountExec.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountExec.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountExec.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountExec.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountExec.Location = New System.Drawing.Point(303, 65)
        Me.lblAccountExec.Name = "lblAccountExec"
        Me.lblAccountExec.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountExec.Size = New System.Drawing.Size(77, 13)
        Me.lblAccountExec.TabIndex = 139
        Me.lblAccountExec.Text = "Account Exec:"
        '
        'lblAccountHandler
        '
        Me.lblAccountHandler.AutoSize = True
        Me.lblAccountHandler.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountHandler.Location = New System.Drawing.Point(303, 40)
        Me.lblAccountHandler.Name = "lblAccountHandler"
        Me.lblAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountHandler.Size = New System.Drawing.Size(90, 13)
        Me.lblAccountHandler.TabIndex = 140
        Me.lblAccountHandler.Text = "Account Handler:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.AutoSize = True
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(303, 14)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(78, 13)
        Me.lblPolicyNumber.TabIndex = 141
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblUnderwritingYearID
        '
        Me.lblUnderwritingYearID.AutoSize = True
        Me.lblUnderwritingYearID.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnderwritingYearID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnderwritingYearID.Enabled = False
        Me.lblUnderwritingYearID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnderwritingYearID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnderwritingYearID.Location = New System.Drawing.Point(303, 286)
        Me.lblUnderwritingYearID.Name = "lblUnderwritingYearID"
        Me.lblUnderwritingYearID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnderwritingYearID.Size = New System.Drawing.Size(129, 13)
        Me.lblUnderwritingYearID.TabIndex = 37
        Me.lblUnderwritingYearID.Text = "Underwriting Year:"
        Me.lblUnderwritingYearID.Visible = False
        '
        'lblLikelyToClaim
        '
        Me.lblLikelyToClaim.AutoSize = True
        Me.lblLikelyToClaim.BackColor = System.Drawing.SystemColors.Control
        Me.lblLikelyToClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLikelyToClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLikelyToClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLikelyToClaim.Location = New System.Drawing.Point(312, 363)
        Me.lblLikelyToClaim.Name = "lblLikelyToClaim"
        Me.lblLikelyToClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLikelyToClaim.Size = New System.Drawing.Size(76, 13)
        Me.lblLikelyToClaim.TabIndex = 45
        Me.lblLikelyToClaim.Text = "Likely to claim:"
        '
        'lblInformation
        '
        Me.lblInformation.AutoSize = True
        Me.lblInformation.BackColor = System.Drawing.SystemColors.Control
        Me.lblInformation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInformation.Location = New System.Drawing.Point(320, 344)
        Me.lblInformation.Name = "lblInformation"
        Me.lblInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInformation.Size = New System.Drawing.Size(62, 13)
        Me.lblInformation.TabIndex = 43
        Me.lblInformation.Text = "Information:"
        '
        'lblPrimaryCausationCode
        '
        Me.lblPrimaryCausationCode.AutoSize = True
        Me.lblPrimaryCausationCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrimaryCausationCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrimaryCausationCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrimaryCausationCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrimaryCausationCode.Location = New System.Drawing.Point(3, 161)
        Me.lblPrimaryCausationCode.Name = "lblPrimaryCausationCode"
        Me.lblPrimaryCausationCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrimaryCausationCode.Size = New System.Drawing.Size(105, 13)
        Me.lblPrimaryCausationCode.TabIndex = 13
        Me.lblPrimaryCausationCode.Text = "Primary cause:"
        '
        'lblSecondaryCausationCode
        '
        Me.lblSecondaryCausationCode.AutoSize = True
        Me.lblSecondaryCausationCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecondaryCausationCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecondaryCausationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecondaryCausationCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecondaryCausationCode.Location = New System.Drawing.Point(303, 161)
        Me.lblSecondaryCausationCode.Name = "lblSecondaryCausationCode"
        Me.lblSecondaryCausationCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecondaryCausationCode.Size = New System.Drawing.Size(93, 13)
        Me.lblSecondaryCausationCode.TabIndex = 15
        Me.lblSecondaryCausationCode.Text = "Secondary cause:"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(3, 14)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(105, 19)
        Me.lblClaimNumber.TabIndex = 0
        Me.lblClaimNumber.Text = "Claim Number:"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(3, 317)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(92, 19)
        Me.lblRiskType.TabIndex = 39
        Me.lblRiskType.Text = "Risk type:"
        '
        'lblLossDate
        '
        Me.lblLossDate.AutoSize = True
        Me.lblLossDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossDate.Location = New System.Drawing.Point(3, 241)
        Me.lblLossDate.Name = "lblLossDate"
        Me.lblLossDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossDate.Size = New System.Drawing.Size(73, 13)
        Me.lblLossDate.TabIndex = 23
        Me.lblLossDate.Text = "Loss date:"
        '
        'lblLossToDate
        '
        Me.lblLossToDate.AutoSize = True
        Me.lblLossToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossToDate.Location = New System.Drawing.Point(303, 238)
        Me.lblLossToDate.Name = "lblLossToDate"
        Me.lblLossToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossToDate.Size = New System.Drawing.Size(68, 13)
        Me.lblLossToDate.TabIndex = 26
        Me.lblLossToDate.Text = "Loss to date:"
        '
        'lblTown
        '
        Me.lblTown.AutoSize = True
        Me.lblTown.BackColor = System.Drawing.SystemColors.Control
        Me.lblTown.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTown.Location = New System.Drawing.Point(3, 215)
        Me.lblTown.Name = "lblTown"
        Me.lblTown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTown.Size = New System.Drawing.Size(37, 13)
        Me.lblTown.TabIndex = 19
        Me.lblTown.Text = "Town:"
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.BackColor = System.Drawing.SystemColors.Control
        Me.lblLocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLocation.Location = New System.Drawing.Point(304, 189)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLocation.Size = New System.Drawing.Size(51, 13)
        Me.lblLocation.TabIndex = 21
        Me.lblLocation.Text = "Location:"
        '
        'lblReportedDate
        '
        Me.lblReportedDate.AutoSize = True
        Me.lblReportedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportedDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportedDate.Location = New System.Drawing.Point(3, 264)
        Me.lblReportedDate.Name = "lblReportedDate"
        Me.lblReportedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportedDate.Size = New System.Drawing.Size(103, 13)
        Me.lblReportedDate.TabIndex = 28
        Me.lblReportedDate.Text = "Reported date:"
        '
        'lblReportedToDate
        '
        Me.lblReportedToDate.AutoSize = True
        Me.lblReportedToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportedToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportedToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportedToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportedToDate.Location = New System.Drawing.Point(303, 260)
        Me.lblReportedToDate.Name = "lblReportedToDate"
        Me.lblReportedToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportedToDate.Size = New System.Drawing.Size(90, 13)
        Me.lblReportedToDate.TabIndex = 31
        Me.lblReportedToDate.Text = "Reported to date:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(3, 345)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(102, 13)
        Me.lblCurrency.TabIndex = 41
        Me.lblCurrency.Text = "Loss currency:"
        '
        'lblLastModified
        '
        Me.lblLastModified.AutoSize = True
        Me.lblLastModified.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastModified.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastModified.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastModified.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastModified.Location = New System.Drawing.Point(3, 289)
        Me.lblLastModified.Name = "lblLastModified"
        Me.lblLastModified.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastModified.Size = New System.Drawing.Size(96, 13)
        Me.lblLastModified.TabIndex = 34
        Me.lblLastModified.Text = "Last modified date:"
        '
        'lblClaimStatusDate
        '
        Me.lblClaimStatusDate.AutoSize = True
        Me.lblClaimStatusDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimStatusDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimStatusDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimStatusDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimStatusDate.Location = New System.Drawing.Point(303, 110)
        Me.lblClaimStatusDate.Name = "lblClaimStatusDate"
        Me.lblClaimStatusDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimStatusDate.Size = New System.Drawing.Size(95, 13)
        Me.lblClaimStatusDate.TabIndex = 8
        Me.lblClaimStatusDate.Text = "Claims status date:"
        '
        'lblProgressStatus
        '
        Me.lblProgressStatus.AutoSize = True
        Me.lblProgressStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProgressStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProgressStatus.Location = New System.Drawing.Point(3, 66)
        Me.lblProgressStatus.Name = "lblProgressStatus"
        Me.lblProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProgressStatus.Size = New System.Drawing.Size(113, 13)
        Me.lblProgressStatus.TabIndex = 4
        Me.lblProgressStatus.Text = "Progress status:"
        '
        'lblHandler
        '
        Me.lblHandler.AutoSize = True
        Me.lblHandler.BackColor = System.Drawing.SystemColors.Control
        Me.lblHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHandler.Location = New System.Drawing.Point(3, 40)
        Me.lblHandler.Name = "lblHandler"
        Me.lblHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHandler.Size = New System.Drawing.Size(101, 13)
        Me.lblHandler.TabIndex = 2
        Me.lblHandler.Text = "Claim handler:"
        '
        'lblCatastropheCode
        '
        Me.lblCatastropheCode.AutoSize = True
        Me.lblCatastropheCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCatastropheCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCatastropheCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCatastropheCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCatastropheCode.Location = New System.Drawing.Point(3, 188)
        Me.lblCatastropheCode.Name = "lblCatastropheCode"
        Me.lblCatastropheCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCatastropheCode.Size = New System.Drawing.Size(94, 13)
        Me.lblCatastropheCode.TabIndex = 17
        Me.lblCatastropheCode.Text = "Catastrophe code:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(3, 139)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(85, 13)
        Me.lblDescription.TabIndex = 11
        Me.lblDescription.Text = "Description:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(3, 113)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(71, 13)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Claims status:"
        '
        'lblCaseNumber
        '
        Me.lblCaseNumber.AutoSize = True
        Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseNumber.Location = New System.Drawing.Point(374, 14)
        Me.lblCaseNumber.Name = "lblCaseNumber"
        Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseNumber.Size = New System.Drawing.Size(34, 13)
        Me.lblCaseNumber.TabIndex = 136
        Me.lblCaseNumber.Text = "Case:"
        Me.lblCaseNumber.Visible = False
        '
        '_txtOpenClaim_43
        '
        Me._txtOpenClaim_43.AcceptsReturn = True
        Me._txtOpenClaim_43.BackColor = System.Drawing.SystemColors.Control
        Me._txtOpenClaim_43.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_43.Enabled = False
        Me._txtOpenClaim_43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_43.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_43.Location = New System.Drawing.Point(408, 12)
        Me._txtOpenClaim_43.MaxLength = 0
        Me._txtOpenClaim_43.Name = "_txtOpenClaim_43"
        Me._txtOpenClaim_43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_43.Size = New System.Drawing.Size(193, 20)
        Me._txtOpenClaim_43.TabIndex = 68
        Me._txtOpenClaim_43.TabStop = False
        '
        'chkClaimHandled
        '
        Me.chkClaimHandled.AutoCaption = True
        Me.chkClaimHandled.BackStyle = 0
        Me.chkClaimHandled.BorderStyle = 0
        Me.chkClaimHandled.Caption = ""
        Me.chkClaimHandled.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimHandled.Location = New System.Drawing.Point(130, 371)
        Me.chkClaimHandled.Name = "chkClaimHandled"
        Me.chkClaimHandled.Size = New System.Drawing.Size(17, 17)
        Me.chkClaimHandled.TabIndex = 58
        Me.chkClaimHandled.Value = 0
        Me.chkClaimHandled.WhatsThisHelpID = 0
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Enabled = False
        Me.txtCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(424, 12)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(177, 20)
        Me.txtCaseNumber.TabIndex = 57
        Me.txtCaseNumber.TabStop = False
        Me.txtCaseNumber.Visible = False
        '
        'chkClaimHandled_old
        '
        Me.chkClaimHandled_old.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimHandled_old.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimHandled_old.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimHandled_old.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimHandled_old.Location = New System.Drawing.Point(130, 369)
        Me.chkClaimHandled_old.Name = "chkClaimHandled_old"
        Me.chkClaimHandled_old.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimHandled_old.Size = New System.Drawing.Size(67, 19)
        Me.chkClaimHandled_old.TabIndex = 59
        Me.chkClaimHandled_old.UseVisualStyleBackColor = False
        Me.chkClaimHandled_old.Visible = False
        '
        'txtClaimVersion
        '
        Me.txtClaimVersion.AcceptsReturn = True
        Me.txtClaimVersion.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimVersion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimVersion.Enabled = False
        Me.txtClaimVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimVersion.Location = New System.Drawing.Point(554, 354)
        Me.txtClaimVersion.MaxLength = 0
        Me.txtClaimVersion.Name = "txtClaimVersion"
        Me.txtClaimVersion.ReadOnly = True
        Me.txtClaimVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimVersion.Size = New System.Drawing.Size(49, 20)
        Me.txtClaimVersion.TabIndex = 65
        Me.txtClaimVersion.TabStop = False
        Me.txtClaimVersion.Text = "1"
        '
        '_txtOpenClaim_45
        '
        Me._txtOpenClaim_45.AcceptsReturn = True
        Me._txtOpenClaim_45.BackColor = System.Drawing.SystemColors.Control
        Me._txtOpenClaim_45.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_45.Enabled = False
        Me._txtOpenClaim_45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_45.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_45.Location = New System.Drawing.Point(408, 63)
        Me._txtOpenClaim_45.MaxLength = 0
        Me._txtOpenClaim_45.Name = "_txtOpenClaim_45"
        Me._txtOpenClaim_45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_45.Size = New System.Drawing.Size(193, 20)
        Me._txtOpenClaim_45.TabIndex = 66
        Me._txtOpenClaim_45.TabStop = False
        '
        '_txtOpenClaim_44
        '
        Me._txtOpenClaim_44.AcceptsReturn = True
        Me._txtOpenClaim_44.BackColor = System.Drawing.SystemColors.Control
        Me._txtOpenClaim_44.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_44.Enabled = False
        Me._txtOpenClaim_44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_44.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_44.Location = New System.Drawing.Point(408, 38)
        Me._txtOpenClaim_44.MaxLength = 0
        Me._txtOpenClaim_44.Name = "_txtOpenClaim_44"
        Me._txtOpenClaim_44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_44.Size = New System.Drawing.Size(193, 20)
        Me._txtOpenClaim_44.TabIndex = 67
        Me._txtOpenClaim_44.TabStop = False
        '
        'cboUnderwritingYearID
        '
        Me.cboUnderwritingYearID.BackColor = System.Drawing.SystemColors.Window
        Me.cboUnderwritingYearID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUnderwritingYearID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUnderwritingYearID.Enabled = False
        Me.cboUnderwritingYearID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUnderwritingYearID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUnderwritingYearID.Location = New System.Drawing.Point(435, 286)
        Me.cboUnderwritingYearID.Name = "cboUnderwritingYearID"
        Me.cboUnderwritingYearID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUnderwritingYearID.Size = New System.Drawing.Size(177, 21)
        Me.cboUnderwritingYearID.Sorted = True
        Me.cboUnderwritingYearID.TabIndex = 38
        Me.cboUnderwritingYearID.Visible = False
        '
        '_txtOpenClaim_10
        '
        Me._txtOpenClaim_10.AcceptsReturn = True
        Me._txtOpenClaim_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_10.Location = New System.Drawing.Point(435, 260)
        Me._txtOpenClaim_10.MaxLength = 0
        Me._txtOpenClaim_10.Name = "_txtOpenClaim_10"
        Me._txtOpenClaim_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_10.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_10.TabIndex = 32
        '
        'chkInfoOnly
        '
        Me.chkInfoOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkInfoOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInfoOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInfoOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInfoOnly.Location = New System.Drawing.Point(426, 342)
        Me.chkInfoOnly.Name = "chkInfoOnly"
        Me.chkInfoOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInfoOnly.Size = New System.Drawing.Size(19, 19)
        Me.chkInfoOnly.TabIndex = 44
        Me.chkInfoOnly.UseVisualStyleBackColor = False
        '
        'chkLikelyClaim
        '
        Me.chkLikelyClaim.BackColor = System.Drawing.SystemColors.Control
        Me.chkLikelyClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLikelyClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLikelyClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLikelyClaim.Location = New System.Drawing.Point(426, 361)
        Me.chkLikelyClaim.Name = "chkLikelyClaim"
        Me.chkLikelyClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLikelyClaim.Size = New System.Drawing.Size(17, 19)
        Me.chkLikelyClaim.TabIndex = 46
        Me.chkLikelyClaim.UseVisualStyleBackColor = False
        '
        'cboPrimaryCausationCode
        '
        Me.cboPrimaryCausationCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboPrimaryCausationCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboPrimaryCausationCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboPrimaryCausationCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPrimaryCausationCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrimaryCausationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrimaryCausationCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPrimaryCausationCode.Location = New System.Drawing.Point(120, 162)
        Me.cboPrimaryCausationCode.Name = "cboPrimaryCausationCode"
        Me.cboPrimaryCausationCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPrimaryCausationCode.Size = New System.Drawing.Size(177, 21)
        Me.cboPrimaryCausationCode.Sorted = True
        Me.cboPrimaryCausationCode.TabIndex = 14
        '
        '_txtOpenClaim_0
        '
        Me._txtOpenClaim_0.AcceptsReturn = True
        Me._txtOpenClaim_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_0.Enabled = False
        Me._txtOpenClaim_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_0.Location = New System.Drawing.Point(120, 110)
        Me._txtOpenClaim_0.MaxLength = 50
        Me._txtOpenClaim_0.Name = "_txtOpenClaim_0"
        Me._txtOpenClaim_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_0.Size = New System.Drawing.Size(178, 20)
        Me._txtOpenClaim_0.TabIndex = 7
        '
        '_txtOpenClaim_1
        '
        Me._txtOpenClaim_1.AcceptsReturn = True
        Me._txtOpenClaim_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_1.Location = New System.Drawing.Point(435, 107)
        Me._txtOpenClaim_1.MaxLength = 0
        Me._txtOpenClaim_1.Name = "_txtOpenClaim_1"
        Me._txtOpenClaim_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_1.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_1.TabIndex = 9
        '
        '_txtOpenClaim_2
        '
        Me._txtOpenClaim_2.AcceptsReturn = True
        Me._txtOpenClaim_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_2.Enabled = False
        Me._txtOpenClaim_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_2.Location = New System.Drawing.Point(571, 107)
        Me._txtOpenClaim_2.MaxLength = 0
        Me._txtOpenClaim_2.Name = "_txtOpenClaim_2"
        Me._txtOpenClaim_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_2.Size = New System.Drawing.Size(41, 20)
        Me._txtOpenClaim_2.TabIndex = 10
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(120, 12)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(177, 20)
        Me.txtClaimNumber.TabIndex = 1
        '
        'cboRiskType
        '
        Me.cboRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRiskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRiskType.Location = New System.Drawing.Point(120, 317)
        Me.cboRiskType.Name = "cboRiskType"
        Me.cboRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRiskType.Size = New System.Drawing.Size(319, 21)
        'Me.cboRiskType.Sorted = True
        Me.cboRiskType.TabIndex = 40
        '
        '_txtOpenClaim_5
        '
        Me._txtOpenClaim_5.AcceptsReturn = True
        Me._txtOpenClaim_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_5.Location = New System.Drawing.Point(120, 238)
        Me._txtOpenClaim_5.MaxLength = 0
        Me._txtOpenClaim_5.Name = "_txtOpenClaim_5"
        Me._txtOpenClaim_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_5.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_5.TabIndex = 24
        '
        '_txtOpenClaim_6
        '
        Me._txtOpenClaim_6.AcceptsReturn = True
        Me._txtOpenClaim_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_6.Location = New System.Drawing.Point(257, 238)
        Me._txtOpenClaim_6.MaxLength = 0
        Me._txtOpenClaim_6.Name = "_txtOpenClaim_6"
        Me._txtOpenClaim_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_6.Size = New System.Drawing.Size(41, 20)
        Me._txtOpenClaim_6.TabIndex = 25
        '
        '_txtOpenClaim_9
        '
        Me._txtOpenClaim_9.AcceptsReturn = True
        Me._txtOpenClaim_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_9.Location = New System.Drawing.Point(435, 238)
        Me._txtOpenClaim_9.MaxLength = 0
        Me._txtOpenClaim_9.Name = "_txtOpenClaim_9"
        Me._txtOpenClaim_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_9.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_9.TabIndex = 27
        '
        'cboTown
        '
        Me.cboTown.BackColor = System.Drawing.SystemColors.Window
        Me.cboTown.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTown.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTown.Location = New System.Drawing.Point(120, 212)
        Me.cboTown.Name = "cboTown"
        Me.cboTown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTown.Size = New System.Drawing.Size(177, 21)
        Me.cboTown.Sorted = True
        Me.cboTown.TabIndex = 20
        '
        '_txtOpenClaim_4
        '
        Me._txtOpenClaim_4.AcceptsReturn = True
        Me._txtOpenClaim_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_4.Location = New System.Drawing.Point(436, 188)
        Me._txtOpenClaim_4.MaxLength = 50
        Me._txtOpenClaim_4.Name = "_txtOpenClaim_4"
        Me._txtOpenClaim_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_4.Size = New System.Drawing.Size(177, 20)
        Me._txtOpenClaim_4.TabIndex = 22
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(120, 344)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(177, 21)
        Me.cboCurrency.Sorted = True
        Me.cboCurrency.TabIndex = 42
        '
        '_txtOpenClaim_7
        '
        Me._txtOpenClaim_7.AcceptsReturn = True
        Me._txtOpenClaim_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_7.Location = New System.Drawing.Point(120, 260)
        Me._txtOpenClaim_7.MaxLength = 0
        Me._txtOpenClaim_7.Name = "_txtOpenClaim_7"
        Me._txtOpenClaim_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_7.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_7.TabIndex = 29
        '
        '_txtOpenClaim_8
        '
        Me._txtOpenClaim_8.AcceptsReturn = True
        Me._txtOpenClaim_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_8.Location = New System.Drawing.Point(257, 260)
        Me._txtOpenClaim_8.MaxLength = 0
        Me._txtOpenClaim_8.Name = "_txtOpenClaim_8"
        Me._txtOpenClaim_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_8.Size = New System.Drawing.Size(41, 20)
        Me._txtOpenClaim_8.TabIndex = 30
        '
        '_txtOpenClaim_12
        '
        Me._txtOpenClaim_12.AcceptsReturn = True
        Me._txtOpenClaim_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_12.Enabled = False
        Me._txtOpenClaim_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_12.Location = New System.Drawing.Point(120, 286)
        Me._txtOpenClaim_12.MaxLength = 0
        Me._txtOpenClaim_12.Name = "_txtOpenClaim_12"
        Me._txtOpenClaim_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_12.Size = New System.Drawing.Size(131, 20)
        Me._txtOpenClaim_12.TabIndex = 35
        '
        '_txtOpenClaim_13
        '
        Me._txtOpenClaim_13.AcceptsReturn = True
        Me._txtOpenClaim_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_13.Enabled = False
        Me._txtOpenClaim_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_13.Location = New System.Drawing.Point(257, 286)
        Me._txtOpenClaim_13.MaxLength = 0
        Me._txtOpenClaim_13.Name = "_txtOpenClaim_13"
        Me._txtOpenClaim_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_13.Size = New System.Drawing.Size(41, 20)
        Me._txtOpenClaim_13.TabIndex = 36
        '
        '_txtOpenClaim_11
        '
        Me._txtOpenClaim_11.AcceptsReturn = True
        Me._txtOpenClaim_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_11.Location = New System.Drawing.Point(572, 260)
        Me._txtOpenClaim_11.MaxLength = 0
        Me._txtOpenClaim_11.Name = "_txtOpenClaim_11"
        Me._txtOpenClaim_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_11.Size = New System.Drawing.Size(41, 20)
        Me._txtOpenClaim_11.TabIndex = 33
        '
        '_txtOpenClaim_3
        '
        Me._txtOpenClaim_3.AcceptsReturn = True
        Me._txtOpenClaim_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_3.Location = New System.Drawing.Point(120, 136)
        Me._txtOpenClaim_3.MaxLength = 1000
        Me._txtOpenClaim_3.Name = "_txtOpenClaim_3"
        Me._txtOpenClaim_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_3.Size = New System.Drawing.Size(480, 20)
        Me._txtOpenClaim_3.TabIndex = 12
        '
        'cboProgressStatus
        '
        Me.cboProgressStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboProgressStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboProgressStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProgressStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProgressStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProgressStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProgressStatus.Location = New System.Drawing.Point(120, 63)
        Me.cboProgressStatus.Name = "cboProgressStatus"
        Me.cboProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProgressStatus.Size = New System.Drawing.Size(177, 21)
        Me.cboProgressStatus.Sorted = True
        Me.cboProgressStatus.TabIndex = 5
        '
        'cboHandler
        '
        Me.cboHandler.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboHandler.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboHandler.BackColor = System.Drawing.SystemColors.Window
        Me.cboHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHandler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHandler.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHandler.Location = New System.Drawing.Point(120, 38)
        Me.cboHandler.Name = "cboHandler"
        Me.cboHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHandler.Size = New System.Drawing.Size(177, 21)
        Me.cboHandler.Sorted = True
        Me.cboHandler.TabIndex = 3
        '
        'cboCatastropheCode
        '
        Me.cboCatastropheCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboCatastropheCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCatastropheCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCatastropheCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCatastropheCode.Location = New System.Drawing.Point(120, 185)
        Me.cboCatastropheCode.Name = "cboCatastropheCode"
        Me.cboCatastropheCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCatastropheCode.Size = New System.Drawing.Size(177, 21)
        Me.cboCatastropheCode.Sorted = True
        Me.cboCatastropheCode.TabIndex = 18
        '
        'cboSecondaryCausationCode
        '
        Me.cboSecondaryCausationCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboSecondaryCausationCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSecondaryCausationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSecondaryCausationCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSecondaryCausationCode.Location = New System.Drawing.Point(436, 162)
        Me.cboSecondaryCausationCode.Name = "cboSecondaryCausationCode"
        Me.cboSecondaryCausationCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSecondaryCausationCode.Size = New System.Drawing.Size(177, 21)
        Me.cboSecondaryCausationCode.Sorted = True
        Me.cboSecondaryCausationCode.TabIndex = 16
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraDeductibles)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraNCDDetails)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraULRDetails)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraEmployeeDetails)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraInsuredDriverDetails)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(631, 396)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Additional Details"
        '
        'fraDeductibles
        '
        Me.fraDeductibles.BackColor = System.Drawing.SystemColors.Control
        Me.fraDeductibles.Controls.Add(Me.cboStandardExcess)
        Me.fraDeductibles.Controls.Add(Me._txtOpenClaim_41)
        Me.fraDeductibles.Controls.Add(Me.lblStandardExcess)
        Me.fraDeductibles.Controls.Add(Me.lblNonStandardExcess)
        Me.fraDeductibles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDeductibles.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDeductibles.Location = New System.Drawing.Point(312, 292)
        Me.fraDeductibles.Name = "fraDeductibles"
        Me.fraDeductibles.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDeductibles.Size = New System.Drawing.Size(289, 73)
        Me.fraDeductibles.TabIndex = 70
        Me.fraDeductibles.TabStop = False
        Me.fraDeductibles.Text = "Deductibles"
        '
        'cboStandardExcess
        '
        Me.cboStandardExcess.BackColor = System.Drawing.SystemColors.Window
        Me.cboStandardExcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStandardExcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStandardExcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStandardExcess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStandardExcess.Location = New System.Drawing.Point(144, 24)
        Me.cboStandardExcess.Name = "cboStandardExcess"
        Me.cboStandardExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStandardExcess.Size = New System.Drawing.Size(137, 21)
        Me.cboStandardExcess.TabIndex = 72
        '
        '_txtOpenClaim_41
        '
        Me._txtOpenClaim_41.AcceptsReturn = True
        Me._txtOpenClaim_41.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_41.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_41.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_41.Location = New System.Drawing.Point(144, 48)
        Me._txtOpenClaim_41.MaxLength = 50
        Me._txtOpenClaim_41.Name = "_txtOpenClaim_41"
        Me._txtOpenClaim_41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_41.Size = New System.Drawing.Size(137, 20)
        Me._txtOpenClaim_41.TabIndex = 71
        Me._txtOpenClaim_41.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblStandardExcess
        '
        Me.lblStandardExcess.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardExcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardExcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStandardExcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStandardExcess.Location = New System.Drawing.Point(8, 24)
        Me.lblStandardExcess.Name = "lblStandardExcess"
        Me.lblStandardExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStandardExcess.Size = New System.Drawing.Size(137, 17)
        Me.lblStandardExcess.TabIndex = 74
        Me.lblStandardExcess.Text = "Standard Excess:"
        '
        'lblNonStandardExcess
        '
        Me.lblNonStandardExcess.BackColor = System.Drawing.SystemColors.Control
        Me.lblNonStandardExcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNonStandardExcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNonStandardExcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNonStandardExcess.Location = New System.Drawing.Point(8, 48)
        Me.lblNonStandardExcess.Name = "lblNonStandardExcess"
        Me.lblNonStandardExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNonStandardExcess.Size = New System.Drawing.Size(137, 17)
        Me.lblNonStandardExcess.TabIndex = 73
        Me.lblNonStandardExcess.Text = "Non-Standard Excess:"
        '
        'fraNCDDetails
        '
        Me.fraNCDDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraNCDDetails.Controls.Add(Me.cboAtFault)
        Me.fraNCDDetails.Controls.Add(Me.chkBonusAffected)
        Me.fraNCDDetails.Controls.Add(Me.lblAtFault)
        Me.fraNCDDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNCDDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNCDDetails.Location = New System.Drawing.Point(312, 212)
        Me.fraNCDDetails.Name = "fraNCDDetails"
        Me.fraNCDDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNCDDetails.Size = New System.Drawing.Size(289, 73)
        Me.fraNCDDetails.TabIndex = 75
        Me.fraNCDDetails.TabStop = False
        Me.fraNCDDetails.Text = "NCD Details"
        '
        'cboAtFault
        '
        Me.cboAtFault.BackColor = System.Drawing.SystemColors.Window
        Me.cboAtFault.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAtFault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAtFault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAtFault.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAtFault.Location = New System.Drawing.Point(112, 24)
        Me.cboAtFault.Name = "cboAtFault"
        Me.cboAtFault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAtFault.Size = New System.Drawing.Size(169, 21)
        Me.cboAtFault.TabIndex = 77
        '
        'chkBonusAffected
        '
        Me.chkBonusAffected.BackColor = System.Drawing.SystemColors.Control
        Me.chkBonusAffected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkBonusAffected.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBonusAffected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBonusAffected.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBonusAffected.Location = New System.Drawing.Point(8, 48)
        Me.chkBonusAffected.Name = "chkBonusAffected"
        Me.chkBonusAffected.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBonusAffected.Size = New System.Drawing.Size(117, 13)
        Me.chkBonusAffected.TabIndex = 76
        Me.chkBonusAffected.Text = "Bonus Affected:"
        Me.chkBonusAffected.UseVisualStyleBackColor = False
        '
        'lblAtFault
        '
        Me.lblAtFault.BackColor = System.Drawing.SystemColors.Control
        Me.lblAtFault.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAtFault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAtFault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAtFault.Location = New System.Drawing.Point(8, 24)
        Me.lblAtFault.Name = "lblAtFault"
        Me.lblAtFault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAtFault.Size = New System.Drawing.Size(97, 17)
        Me.lblAtFault.TabIndex = 78
        Me.lblAtFault.Text = "At Fault:"
        '
        'fraULRDetails
        '
        Me.fraULRDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraULRDetails.Controls.Add(Me.chkULR)
        Me.fraULRDetails.Controls.Add(Me._txtOpenClaim_38)
        Me.fraULRDetails.Controls.Add(Me._txtOpenClaim_39)
        Me.fraULRDetails.Controls.Add(Me._txtOpenClaim_40)
        Me.fraULRDetails.Controls.Add(Me.chkSolicitorAppointed)
        Me.fraULRDetails.Controls.Add(Me.lblRecoveryAgent)
        Me.fraULRDetails.Controls.Add(Me.lblSolicitorName)
        Me.fraULRDetails.Controls.Add(Me.lblLossDetails)
        Me.fraULRDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraULRDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraULRDetails.Location = New System.Drawing.Point(312, 12)
        Me.fraULRDetails.Name = "fraULRDetails"
        Me.fraULRDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraULRDetails.Size = New System.Drawing.Size(289, 193)
        Me.fraULRDetails.TabIndex = 79
        Me.fraULRDetails.TabStop = False
        Me.fraULRDetails.Text = "Uninsured Loss Recovery Details"
        '
        'chkULR
        '
        Me.chkULR.BackColor = System.Drawing.SystemColors.Control
        Me.chkULR.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkULR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkULR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkULR.Location = New System.Drawing.Point(8, 24)
        Me.chkULR.Name = "chkULR"
        Me.chkULR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkULR.Size = New System.Drawing.Size(185, 13)
        Me.chkULR.TabIndex = 84
        Me.chkULR.Text = "Uninsured Loss Recovery"
        Me.chkULR.UseVisualStyleBackColor = False
        '
        '_txtOpenClaim_38
        '
        Me._txtOpenClaim_38.AcceptsReturn = True
        Me._txtOpenClaim_38.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_38.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_38.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_38.Location = New System.Drawing.Point(112, 48)
        Me._txtOpenClaim_38.MaxLength = 255
        Me._txtOpenClaim_38.Name = "_txtOpenClaim_38"
        Me._txtOpenClaim_38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_38.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_38.TabIndex = 83
        '
        '_txtOpenClaim_39
        '
        Me._txtOpenClaim_39.AcceptsReturn = True
        Me._txtOpenClaim_39.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_39.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_39.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_39.Location = New System.Drawing.Point(112, 104)
        Me._txtOpenClaim_39.MaxLength = 255
        Me._txtOpenClaim_39.Name = "_txtOpenClaim_39"
        Me._txtOpenClaim_39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_39.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_39.TabIndex = 82
        '
        '_txtOpenClaim_40
        '
        Me._txtOpenClaim_40.AcceptsReturn = True
        Me._txtOpenClaim_40.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_40.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_40.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_40.Location = New System.Drawing.Point(8, 144)
        Me._txtOpenClaim_40.MaxLength = 255
        Me._txtOpenClaim_40.Multiline = True
        Me._txtOpenClaim_40.Name = "_txtOpenClaim_40"
        Me._txtOpenClaim_40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_40.Size = New System.Drawing.Size(273, 45)
        Me._txtOpenClaim_40.TabIndex = 81
        '
        'chkSolicitorAppointed
        '
        Me.chkSolicitorAppointed.BackColor = System.Drawing.SystemColors.Control
        Me.chkSolicitorAppointed.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSolicitorAppointed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSolicitorAppointed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSolicitorAppointed.Location = New System.Drawing.Point(8, 80)
        Me.chkSolicitorAppointed.Name = "chkSolicitorAppointed"
        Me.chkSolicitorAppointed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSolicitorAppointed.Size = New System.Drawing.Size(137, 13)
        Me.chkSolicitorAppointed.TabIndex = 80
        Me.chkSolicitorAppointed.Text = "Solicitor Appointed"
        Me.chkSolicitorAppointed.UseVisualStyleBackColor = False
        '
        'lblRecoveryAgent
        '
        Me.lblRecoveryAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecoveryAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecoveryAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecoveryAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecoveryAgent.Location = New System.Drawing.Point(8, 48)
        Me.lblRecoveryAgent.Name = "lblRecoveryAgent"
        Me.lblRecoveryAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecoveryAgent.Size = New System.Drawing.Size(97, 17)
        Me.lblRecoveryAgent.TabIndex = 87
        Me.lblRecoveryAgent.Text = "Recovery Agent:"
        '
        'lblSolicitorName
        '
        Me.lblSolicitorName.BackColor = System.Drawing.SystemColors.Control
        Me.lblSolicitorName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSolicitorName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSolicitorName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSolicitorName.Location = New System.Drawing.Point(8, 104)
        Me.lblSolicitorName.Name = "lblSolicitorName"
        Me.lblSolicitorName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSolicitorName.Size = New System.Drawing.Size(97, 17)
        Me.lblSolicitorName.TabIndex = 86
        Me.lblSolicitorName.Text = "Solicitor Name:"
        '
        'lblLossDetails
        '
        Me.lblLossDetails.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossDetails.Location = New System.Drawing.Point(8, 128)
        Me.lblLossDetails.Name = "lblLossDetails"
        Me.lblLossDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossDetails.Size = New System.Drawing.Size(97, 17)
        Me.lblLossDetails.TabIndex = 85
        Me.lblLossDetails.Text = "Details:"
        '
        'fraEmployeeDetails
        '
        Me.fraEmployeeDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraEmployeeDetails.Controls.Add(Me.fraPreviousClaimDetails)
        Me.fraEmployeeDetails.Controls.Add(Me._txtOpenClaim_34)
        Me.fraEmployeeDetails.Controls.Add(Me._txtOpenClaim_35)
        Me.fraEmployeeDetails.Controls.Add(Me._txtOpenClaim_36)
        Me.fraEmployeeDetails.Controls.Add(Me.chkPreviousClaim)
        Me.fraEmployeeDetails.Controls.Add(Me.ddEmployeeTitle)
        Me.fraEmployeeDetails.Controls.Add(Me.lblEmployeeTitle)
        Me.fraEmployeeDetails.Controls.Add(Me.lblEmployeeForename)
        Me.fraEmployeeDetails.Controls.Add(Me.lblEmployeeSurname)
        Me.fraEmployeeDetails.Controls.Add(Me.lblLengthOfService)
        Me.fraEmployeeDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEmployeeDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEmployeeDetails.Location = New System.Drawing.Point(8, 148)
        Me.fraEmployeeDetails.Name = "fraEmployeeDetails"
        Me.fraEmployeeDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEmployeeDetails.Size = New System.Drawing.Size(297, 217)
        Me.fraEmployeeDetails.TabIndex = 88
        Me.fraEmployeeDetails.TabStop = False
        Me.fraEmployeeDetails.Text = "Employee Details"
        '
        'fraPreviousClaimDetails
        '
        Me.fraPreviousClaimDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreviousClaimDetails.Controls.Add(Me._txtOpenClaim_37)
        Me.fraPreviousClaimDetails.Controls.Add(Me.lblPreviousClaimDetails)
        Me.fraPreviousClaimDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPreviousClaimDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreviousClaimDetails.Location = New System.Drawing.Point(8, 120)
        Me.fraPreviousClaimDetails.Name = "fraPreviousClaimDetails"
        Me.fraPreviousClaimDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousClaimDetails.Size = New System.Drawing.Size(281, 89)
        Me.fraPreviousClaimDetails.TabIndex = 94
        Me.fraPreviousClaimDetails.TabStop = False
        '
        '_txtOpenClaim_37
        '
        Me._txtOpenClaim_37.AcceptsReturn = True
        Me._txtOpenClaim_37.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_37.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_37.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_37.Location = New System.Drawing.Point(8, 40)
        Me._txtOpenClaim_37.MaxLength = 255
        Me._txtOpenClaim_37.Multiline = True
        Me._txtOpenClaim_37.Name = "_txtOpenClaim_37"
        Me._txtOpenClaim_37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_37.Size = New System.Drawing.Size(265, 45)
        Me._txtOpenClaim_37.TabIndex = 95
        '
        'lblPreviousClaimDetails
        '
        Me.lblPreviousClaimDetails.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreviousClaimDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreviousClaimDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviousClaimDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreviousClaimDetails.Location = New System.Drawing.Point(8, 24)
        Me.lblPreviousClaimDetails.Name = "lblPreviousClaimDetails"
        Me.lblPreviousClaimDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreviousClaimDetails.Size = New System.Drawing.Size(73, 17)
        Me.lblPreviousClaimDetails.TabIndex = 96
        Me.lblPreviousClaimDetails.Text = "Details:"
        '
        '_txtOpenClaim_34
        '
        Me._txtOpenClaim_34.AcceptsReturn = True
        Me._txtOpenClaim_34.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_34.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_34.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_34.Location = New System.Drawing.Point(120, 48)
        Me._txtOpenClaim_34.MaxLength = 255
        Me._txtOpenClaim_34.Name = "_txtOpenClaim_34"
        Me._txtOpenClaim_34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_34.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_34.TabIndex = 92
        '
        '_txtOpenClaim_35
        '
        Me._txtOpenClaim_35.AcceptsReturn = True
        Me._txtOpenClaim_35.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_35.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_35.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_35.Location = New System.Drawing.Point(120, 72)
        Me._txtOpenClaim_35.MaxLength = 255
        Me._txtOpenClaim_35.Name = "_txtOpenClaim_35"
        Me._txtOpenClaim_35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_35.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_35.TabIndex = 91
        '
        '_txtOpenClaim_36
        '
        Me._txtOpenClaim_36.AcceptsReturn = True
        Me._txtOpenClaim_36.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_36.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_36.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_36.Location = New System.Drawing.Point(120, 96)
        Me._txtOpenClaim_36.MaxLength = 50
        Me._txtOpenClaim_36.Name = "_txtOpenClaim_36"
        Me._txtOpenClaim_36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_36.Size = New System.Drawing.Size(57, 20)
        Me._txtOpenClaim_36.TabIndex = 90
        Me._txtOpenClaim_36.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkPreviousClaim
        '
        Me.chkPreviousClaim.BackColor = System.Drawing.SystemColors.Control
        Me.chkPreviousClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPreviousClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreviousClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPreviousClaim.Location = New System.Drawing.Point(16, 120)
        Me.chkPreviousClaim.Name = "chkPreviousClaim"
        Me.chkPreviousClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPreviousClaim.Size = New System.Drawing.Size(109, 17)
        Me.chkPreviousClaim.TabIndex = 89
        Me.chkPreviousClaim.Text = "Previous Claim"
        Me.chkPreviousClaim.UseVisualStyleBackColor = False
        '
        'ddEmployeeTitle
        '
        Me.ddEmployeeTitle.AllowAbiCodeEntry = False
        Me.ddEmployeeTitle.AutoCompleteText = True
        Me.ddEmployeeTitle.DataModel = "GIIM"
        Me.ddEmployeeTitle.ListIndex = -1
        Me.ddEmployeeTitle.ListManager = Nothing
        Me.ddEmployeeTitle.Location = New System.Drawing.Point(120, 24)
        Me.ddEmployeeTitle.Login = False
        Me.ddEmployeeTitle.LongList = False
        Me.ddEmployeeTitle.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddEmployeeTitle.Name = "ddEmployeeTitle"
        Me.ddEmployeeTitle.PropertyId = "131085"
        Me.ddEmployeeTitle.ReadOnly_Renamed = False
        Me.ddEmployeeTitle.SelLength = 0
        Me.ddEmployeeTitle.SelStart = 0
        Me.ddEmployeeTitle.SelText = ""
        Me.ddEmployeeTitle.Size = New System.Drawing.Size(169, 21)
        Me.ddEmployeeTitle.TabIndex = 93
        Me.ddEmployeeTitle.ToolTipText = ""
        Me.ddEmployeeTitle.VehicleListId = ""
        Me.ddEmployeeTitle.VehicleMake = ""
        '
        'lblEmployeeTitle
        '
        Me.lblEmployeeTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmployeeTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmployeeTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployeeTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmployeeTitle.Location = New System.Drawing.Point(8, 24)
        Me.lblEmployeeTitle.Name = "lblEmployeeTitle"
        Me.lblEmployeeTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmployeeTitle.Size = New System.Drawing.Size(73, 17)
        Me.lblEmployeeTitle.TabIndex = 100
        Me.lblEmployeeTitle.Text = "Title:"
        '
        'lblEmployeeForename
        '
        Me.lblEmployeeForename.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmployeeForename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmployeeForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployeeForename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmployeeForename.Location = New System.Drawing.Point(8, 48)
        Me.lblEmployeeForename.Name = "lblEmployeeForename"
        Me.lblEmployeeForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmployeeForename.Size = New System.Drawing.Size(65, 17)
        Me.lblEmployeeForename.TabIndex = 99
        Me.lblEmployeeForename.Text = "Forename:"
        '
        'lblEmployeeSurname
        '
        Me.lblEmployeeSurname.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmployeeSurname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmployeeSurname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployeeSurname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmployeeSurname.Location = New System.Drawing.Point(8, 72)
        Me.lblEmployeeSurname.Name = "lblEmployeeSurname"
        Me.lblEmployeeSurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmployeeSurname.Size = New System.Drawing.Size(73, 17)
        Me.lblEmployeeSurname.TabIndex = 98
        Me.lblEmployeeSurname.Text = "Last name:"
        '
        'lblLengthOfService
        '
        Me.lblLengthOfService.BackColor = System.Drawing.SystemColors.Control
        Me.lblLengthOfService.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLengthOfService.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLengthOfService.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLengthOfService.Location = New System.Drawing.Point(8, 96)
        Me.lblLengthOfService.Name = "lblLengthOfService"
        Me.lblLengthOfService.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLengthOfService.Size = New System.Drawing.Size(105, 17)
        Me.lblLengthOfService.TabIndex = 97
        Me.lblLengthOfService.Text = "Length of Service:"
        '
        'fraInsuredDriverDetails
        '
        Me.fraInsuredDriverDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraInsuredDriverDetails.Controls.Add(Me._txtOpenClaim_31)
        Me.fraInsuredDriverDetails.Controls.Add(Me._txtOpenClaim_32)
        Me.fraInsuredDriverDetails.Controls.Add(Me._txtOpenClaim_33)
        Me.fraInsuredDriverDetails.Controls.Add(Me.ddDriverTitle)
        Me.fraInsuredDriverDetails.Controls.Add(Me.lblDriverTitle)
        Me.fraInsuredDriverDetails.Controls.Add(Me.lblDriverForename)
        Me.fraInsuredDriverDetails.Controls.Add(Me.lblDriverSurname)
        Me.fraInsuredDriverDetails.Controls.Add(Me.lblDatePassedTest)
        Me.fraInsuredDriverDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInsuredDriverDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInsuredDriverDetails.Location = New System.Drawing.Point(8, 12)
        Me.fraInsuredDriverDetails.Name = "fraInsuredDriverDetails"
        Me.fraInsuredDriverDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInsuredDriverDetails.Size = New System.Drawing.Size(297, 129)
        Me.fraInsuredDriverDetails.TabIndex = 50
        Me.fraInsuredDriverDetails.TabStop = False
        Me.fraInsuredDriverDetails.Text = "Insured Driver Details"
        '
        '_txtOpenClaim_31
        '
        Me._txtOpenClaim_31.AcceptsReturn = True
        Me._txtOpenClaim_31.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_31.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_31.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_31.Location = New System.Drawing.Point(120, 48)
        Me._txtOpenClaim_31.MaxLength = 255
        Me._txtOpenClaim_31.Name = "_txtOpenClaim_31"
        Me._txtOpenClaim_31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_31.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_31.TabIndex = 54
        '
        '_txtOpenClaim_32
        '
        Me._txtOpenClaim_32.AcceptsReturn = True
        Me._txtOpenClaim_32.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_32.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_32.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_32.Location = New System.Drawing.Point(120, 72)
        Me._txtOpenClaim_32.MaxLength = 255
        Me._txtOpenClaim_32.Name = "_txtOpenClaim_32"
        Me._txtOpenClaim_32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_32.Size = New System.Drawing.Size(169, 20)
        Me._txtOpenClaim_32.TabIndex = 102
        '
        '_txtOpenClaim_33
        '
        Me._txtOpenClaim_33.AcceptsReturn = True
        Me._txtOpenClaim_33.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_33.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_33.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_33.Location = New System.Drawing.Point(120, 96)
        Me._txtOpenClaim_33.MaxLength = 50
        Me._txtOpenClaim_33.Name = "_txtOpenClaim_33"
        Me._txtOpenClaim_33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_33.Size = New System.Drawing.Size(137, 20)
        Me._txtOpenClaim_33.TabIndex = 101
        '
        'ddDriverTitle
        '
        Me.ddDriverTitle.AllowAbiCodeEntry = False
        Me.ddDriverTitle.AutoCompleteText = True
        Me.ddDriverTitle.DataModel = "GIIM"
        Me.ddDriverTitle.ListIndex = -1
        Me.ddDriverTitle.ListManager = Nothing
        Me.ddDriverTitle.Location = New System.Drawing.Point(120, 24)
        Me.ddDriverTitle.Login = False
        Me.ddDriverTitle.LongList = False
        Me.ddDriverTitle.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddDriverTitle.Name = "ddDriverTitle"
        Me.ddDriverTitle.PropertyId = "131085"
        Me.ddDriverTitle.ReadOnly_Renamed = False
        Me.ddDriverTitle.SelLength = 0
        Me.ddDriverTitle.SelStart = 0
        Me.ddDriverTitle.SelText = ""
        Me.ddDriverTitle.Size = New System.Drawing.Size(169, 21)
        Me.ddDriverTitle.TabIndex = 52
        Me.ddDriverTitle.ToolTipText = ""
        Me.ddDriverTitle.VehicleListId = ""
        Me.ddDriverTitle.VehicleMake = ""
        '
        'lblDriverTitle
        '
        Me.lblDriverTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblDriverTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDriverTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDriverTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDriverTitle.Location = New System.Drawing.Point(8, 24)
        Me.lblDriverTitle.Name = "lblDriverTitle"
        Me.lblDriverTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDriverTitle.Size = New System.Drawing.Size(73, 17)
        Me.lblDriverTitle.TabIndex = 51
        Me.lblDriverTitle.Text = "Title:"
        '
        'lblDriverForename
        '
        Me.lblDriverForename.BackColor = System.Drawing.SystemColors.Control
        Me.lblDriverForename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDriverForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDriverForename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDriverForename.Location = New System.Drawing.Point(8, 48)
        Me.lblDriverForename.Name = "lblDriverForename"
        Me.lblDriverForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDriverForename.Size = New System.Drawing.Size(65, 17)
        Me.lblDriverForename.TabIndex = 53
        Me.lblDriverForename.Text = "Forename:"
        '
        'lblDriverSurname
        '
        Me.lblDriverSurname.BackColor = System.Drawing.SystemColors.Control
        Me.lblDriverSurname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDriverSurname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDriverSurname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDriverSurname.Location = New System.Drawing.Point(8, 72)
        Me.lblDriverSurname.Name = "lblDriverSurname"
        Me.lblDriverSurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDriverSurname.Size = New System.Drawing.Size(73, 17)
        Me.lblDriverSurname.TabIndex = 104
        Me.lblDriverSurname.Text = "Last name:"
        '
        'lblDatePassedTest
        '
        Me.lblDatePassedTest.BackColor = System.Drawing.SystemColors.Control
        Me.lblDatePassedTest.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDatePassedTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatePassedTest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDatePassedTest.Location = New System.Drawing.Point(8, 96)
        Me.lblDatePassedTest.Name = "lblDatePassedTest"
        Me.lblDatePassedTest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDatePassedTest.Size = New System.Drawing.Size(105, 17)
        Me.lblDatePassedTest.TabIndex = 103
        Me.lblDatePassedTest.Text = "Date Passed Test:"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserDefinedFieldE)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserDefinedFieldD)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserDefinedFieldC)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserDefinedFieldB)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblUserDefinedFieldA)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblSubsidiaryCompany)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblClientName)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblTelePhoneNumberH)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblTelePhoneNumberO)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblFaxNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblMobileNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblEmailAddress)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblVAT)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblVATRegistartionNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblClientClaimNumber)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboUDFA)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboUDFB)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboUDFC)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboUDFD)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboUDFE)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_42)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_18)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_23)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_22)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_21)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_20)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_19)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_17)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_16)
        Me._tabMainTab_TabPage2.Controls.Add(Me._txtOpenClaim_15)
        Me._tabMainTab_TabPage2.Controls.Add(Me.CmdClient)
        Me._tabMainTab_TabPage2.Controls.Add(Me.chkVATRegistered)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(631, 396)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Client Details"
        '
        'lblUserDefinedFieldE
        '
        Me.lblUserDefinedFieldE.AutoSize = True
        Me.lblUserDefinedFieldE.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserDefinedFieldE.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserDefinedFieldE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserDefinedFieldE.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserDefinedFieldE.Location = New System.Drawing.Point(16, 324)
        Me.lblUserDefinedFieldE.Name = "lblUserDefinedFieldE"
        Me.lblUserDefinedFieldE.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserDefinedFieldE.Size = New System.Drawing.Size(102, 13)
        Me.lblUserDefinedFieldE.TabIndex = 131
        Me.lblUserDefinedFieldE.Text = "User defined field E:"
        '
        'lblUserDefinedFieldD
        '
        Me.lblUserDefinedFieldD.AutoSize = True
        Me.lblUserDefinedFieldD.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserDefinedFieldD.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserDefinedFieldD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserDefinedFieldD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserDefinedFieldD.Location = New System.Drawing.Point(16, 300)
        Me.lblUserDefinedFieldD.Name = "lblUserDefinedFieldD"
        Me.lblUserDefinedFieldD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserDefinedFieldD.Size = New System.Drawing.Size(103, 13)
        Me.lblUserDefinedFieldD.TabIndex = 132
        Me.lblUserDefinedFieldD.Text = "User defined field D:"
        '
        'lblUserDefinedFieldC
        '
        Me.lblUserDefinedFieldC.AutoSize = True
        Me.lblUserDefinedFieldC.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserDefinedFieldC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserDefinedFieldC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserDefinedFieldC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserDefinedFieldC.Location = New System.Drawing.Point(16, 276)
        Me.lblUserDefinedFieldC.Name = "lblUserDefinedFieldC"
        Me.lblUserDefinedFieldC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserDefinedFieldC.Size = New System.Drawing.Size(102, 13)
        Me.lblUserDefinedFieldC.TabIndex = 133
        Me.lblUserDefinedFieldC.Text = "User defined field C:"
        '
        'lblUserDefinedFieldB
        '
        Me.lblUserDefinedFieldB.AutoSize = True
        Me.lblUserDefinedFieldB.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserDefinedFieldB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserDefinedFieldB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserDefinedFieldB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserDefinedFieldB.Location = New System.Drawing.Point(16, 252)
        Me.lblUserDefinedFieldB.Name = "lblUserDefinedFieldB"
        Me.lblUserDefinedFieldB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserDefinedFieldB.Size = New System.Drawing.Size(102, 13)
        Me.lblUserDefinedFieldB.TabIndex = 134
        Me.lblUserDefinedFieldB.Text = "User defined field B:"
        '
        'lblUserDefinedFieldA
        '
        Me.lblUserDefinedFieldA.AutoSize = True
        Me.lblUserDefinedFieldA.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserDefinedFieldA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserDefinedFieldA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserDefinedFieldA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserDefinedFieldA.Location = New System.Drawing.Point(16, 230)
        Me.lblUserDefinedFieldA.Name = "lblUserDefinedFieldA"
        Me.lblUserDefinedFieldA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserDefinedFieldA.Size = New System.Drawing.Size(102, 13)
        Me.lblUserDefinedFieldA.TabIndex = 135
        Me.lblUserDefinedFieldA.Text = "User defined field A:"
        '
        'lblSubsidiaryCompany
        '
        Me.lblSubsidiaryCompany.AutoSize = True
        Me.lblSubsidiaryCompany.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubsidiaryCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubsidiaryCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubsidiaryCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubsidiaryCompany.Location = New System.Drawing.Point(16, 199)
        Me.lblSubsidiaryCompany.Name = "lblSubsidiaryCompany"
        Me.lblSubsidiaryCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubsidiaryCompany.Size = New System.Drawing.Size(105, 13)
        Me.lblSubsidiaryCompany.TabIndex = 142
        Me.lblSubsidiaryCompany.Text = "Subsidiary Company:"
        '
        'lblClientName
        '
        Me.lblClientName.AutoSize = True
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(16, 22)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(65, 13)
        Me.lblClientName.TabIndex = 143
        Me.lblClientName.Text = "Client name:"
        '
        'lblTelePhoneNumberH
        '
        Me.lblTelePhoneNumberH.AutoSize = True
        Me.lblTelePhoneNumberH.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelePhoneNumberH.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelePhoneNumberH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelePhoneNumberH.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelePhoneNumberH.Location = New System.Drawing.Point(16, 102)
        Me.lblTelePhoneNumberH.Name = "lblTelePhoneNumberH"
        Me.lblTelePhoneNumberH.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelePhoneNumberH.Size = New System.Drawing.Size(116, 13)
        Me.lblTelePhoneNumberH.TabIndex = 144
        Me.lblTelePhoneNumberH.Text = "Telephone number (H):"
        '
        'lblTelePhoneNumberO
        '
        Me.lblTelePhoneNumberO.AutoSize = True
        Me.lblTelePhoneNumberO.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelePhoneNumberO.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelePhoneNumberO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelePhoneNumberO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelePhoneNumberO.Location = New System.Drawing.Point(16, 127)
        Me.lblTelePhoneNumberO.Name = "lblTelePhoneNumberO"
        Me.lblTelePhoneNumberO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelePhoneNumberO.Size = New System.Drawing.Size(119, 13)
        Me.lblTelePhoneNumberO.TabIndex = 145
        Me.lblTelePhoneNumberO.Text = "Telephone number (W):"
        '
        'lblFaxNumber
        '
        Me.lblFaxNumber.AutoSize = True
        Me.lblFaxNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblFaxNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFaxNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFaxNumber.Location = New System.Drawing.Point(328, 105)
        Me.lblFaxNumber.Name = "lblFaxNumber"
        Me.lblFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFaxNumber.Size = New System.Drawing.Size(65, 13)
        Me.lblFaxNumber.TabIndex = 146
        Me.lblFaxNumber.Text = "Fax number:"
        '
        'lblMobileNumber
        '
        Me.lblMobileNumber.AutoSize = True
        Me.lblMobileNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblMobileNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMobileNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMobileNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMobileNumber.Location = New System.Drawing.Point(328, 130)
        Me.lblMobileNumber.Name = "lblMobileNumber"
        Me.lblMobileNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMobileNumber.Size = New System.Drawing.Size(79, 13)
        Me.lblMobileNumber.TabIndex = 147
        Me.lblMobileNumber.Text = "Mobile number:"
        '
        'lblEmailAddress
        '
        Me.lblEmailAddress.AutoSize = True
        Me.lblEmailAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmailAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmailAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmailAddress.Location = New System.Drawing.Point(328, 155)
        Me.lblEmailAddress.Name = "lblEmailAddress"
        Me.lblEmailAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmailAddress.Size = New System.Drawing.Size(75, 13)
        Me.lblEmailAddress.TabIndex = 148
        Me.lblEmailAddress.Text = "Email address:"
        '
        'lblVAT
        '
        Me.lblVAT.AutoSize = True
        Me.lblVAT.BackColor = System.Drawing.SystemColors.Control
        Me.lblVAT.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVAT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVAT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVAT.Location = New System.Drawing.Point(328, 177)
        Me.lblVAT.Name = "lblVAT"
        Me.lblVAT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVAT.Size = New System.Drawing.Size(80, 13)
        Me.lblVAT.TabIndex = 149
        Me.lblVAT.Text = "VAT registered:"
        '
        'lblVATRegistartionNumber
        '
        Me.lblVATRegistartionNumber.AutoSize = True
        Me.lblVATRegistartionNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblVATRegistartionNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVATRegistartionNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVATRegistartionNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVATRegistartionNumber.Location = New System.Drawing.Point(16, 150)
        Me.lblVATRegistartionNumber.Name = "lblVATRegistartionNumber"
        Me.lblVATRegistartionNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVATRegistartionNumber.Size = New System.Drawing.Size(123, 13)
        Me.lblVATRegistartionNumber.TabIndex = 150
        Me.lblVATRegistartionNumber.Text = "VAT registration number:"
        '
        'lblClientClaimNumber
        '
        Me.lblClientClaimNumber.AutoSize = True
        Me.lblClientClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientClaimNumber.Location = New System.Drawing.Point(16, 175)
        Me.lblClientClaimNumber.Name = "lblClientClaimNumber"
        Me.lblClientClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientClaimNumber.Size = New System.Drawing.Size(101, 13)
        Me.lblClientClaimNumber.TabIndex = 151
        Me.lblClientClaimNumber.Text = "Client claim number:"
        '
        'cboUDFA
        '
        Me.cboUDFA.DefaultItemId = 0
        Me.cboUDFA.FirstItem = "(None)"
        Me.cboUDFA.GISDataModelCode = "None"
        Me.cboUDFA.ItemId = 0
        Me.cboUDFA.ListIndex = -1
        Me.cboUDFA.Location = New System.Drawing.Point(168, 228)
        Me.cboUDFA.Name = "cboUDFA"
        Me.cboUDFA.ParentDetailId = 0
        Me.cboUDFA.ParentHeaderId = 0
        Me.cboUDFA.SingleItemId = 0
        Me.cboUDFA.Size = New System.Drawing.Size(166, 21)
        Me.cboUDFA.TabIndex = 129
        Me.cboUDFA.Table = 0
        Me.cboUDFA.ToolTipText = ""
        Me.cboUDFA.WhatsThisHelpID = 0
        '
        'cboUDFB
        '
        Me.cboUDFB.DefaultItemId = 0
        Me.cboUDFB.FirstItem = "(None)"
        Me.cboUDFB.GISDataModelCode = "None"
        Me.cboUDFB.ItemId = 0
        Me.cboUDFB.ListIndex = -1
        Me.cboUDFB.Location = New System.Drawing.Point(168, 252)
        Me.cboUDFB.Name = "cboUDFB"
        Me.cboUDFB.ParentDetailId = 0
        Me.cboUDFB.ParentHeaderId = 0
        Me.cboUDFB.SingleItemId = 0
        Me.cboUDFB.Size = New System.Drawing.Size(166, 21)
        Me.cboUDFB.TabIndex = 128
        Me.cboUDFB.Table = 0
        Me.cboUDFB.ToolTipText = ""
        Me.cboUDFB.WhatsThisHelpID = 0
        '
        'cboUDFC
        '
        Me.cboUDFC.DefaultItemId = 0
        Me.cboUDFC.FirstItem = "(None)"
        Me.cboUDFC.GISDataModelCode = "None"
        Me.cboUDFC.ItemId = 0
        Me.cboUDFC.ListIndex = -1
        Me.cboUDFC.Location = New System.Drawing.Point(168, 276)
        Me.cboUDFC.Name = "cboUDFC"
        Me.cboUDFC.ParentDetailId = 0
        Me.cboUDFC.ParentHeaderId = 0
        Me.cboUDFC.SingleItemId = 0
        Me.cboUDFC.Size = New System.Drawing.Size(166, 21)
        Me.cboUDFC.TabIndex = 127
        Me.cboUDFC.Table = 0
        Me.cboUDFC.ToolTipText = ""
        Me.cboUDFC.WhatsThisHelpID = 0
        '
        'cboUDFD
        '
        Me.cboUDFD.DefaultItemId = 0
        Me.cboUDFD.FirstItem = "(None)"
        Me.cboUDFD.GISDataModelCode = "None"
        Me.cboUDFD.ItemId = 0
        Me.cboUDFD.ListIndex = -1
        Me.cboUDFD.Location = New System.Drawing.Point(168, 300)
        Me.cboUDFD.Name = "cboUDFD"
        Me.cboUDFD.ParentDetailId = 0
        Me.cboUDFD.ParentHeaderId = 0
        Me.cboUDFD.SingleItemId = 0
        Me.cboUDFD.Size = New System.Drawing.Size(166, 21)
        Me.cboUDFD.TabIndex = 126
        Me.cboUDFD.Table = 0
        Me.cboUDFD.ToolTipText = ""
        Me.cboUDFD.WhatsThisHelpID = 0
        '
        'cboUDFE
        '
        Me.cboUDFE.DefaultItemId = 0
        Me.cboUDFE.FirstItem = "(None)"
        Me.cboUDFE.GISDataModelCode = "None"
        Me.cboUDFE.ItemId = 0
        Me.cboUDFE.ListIndex = -1
        Me.cboUDFE.Location = New System.Drawing.Point(168, 324)
        Me.cboUDFE.Name = "cboUDFE"
        Me.cboUDFE.ParentDetailId = 0
        Me.cboUDFE.ParentHeaderId = 0
        Me.cboUDFE.SingleItemId = 0
        Me.cboUDFE.Size = New System.Drawing.Size(166, 21)
        Me.cboUDFE.TabIndex = 125
        Me.cboUDFE.Table = 0
        Me.cboUDFE.ToolTipText = ""
        Me.cboUDFE.WhatsThisHelpID = 0
        '
        '_txtOpenClaim_42
        '
        Me._txtOpenClaim_42.AcceptsReturn = True
        Me._txtOpenClaim_42.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_42.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_42.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_42.Location = New System.Drawing.Point(168, 200)
        Me._txtOpenClaim_42.MaxLength = 50
        Me._txtOpenClaim_42.Name = "_txtOpenClaim_42"
        Me._txtOpenClaim_42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_42.Size = New System.Drawing.Size(433, 20)
        Me._txtOpenClaim_42.TabIndex = 69
        '
        '_txtOpenClaim_18
        '
        Me._txtOpenClaim_18.AcceptsReturn = True
        Me._txtOpenClaim_18.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_18.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_18.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_18.Location = New System.Drawing.Point(168, 125)
        Me._txtOpenClaim_18.MaxLength = 50
        Me._txtOpenClaim_18.Name = "_txtOpenClaim_18"
        Me._txtOpenClaim_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_18.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_18.TabIndex = 105
        '
        '_txtOpenClaim_23
        '
        Me._txtOpenClaim_23.AcceptsReturn = True
        Me._txtOpenClaim_23.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_23.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_23.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_23.Location = New System.Drawing.Point(168, 175)
        Me._txtOpenClaim_23.MaxLength = 50
        Me._txtOpenClaim_23.Name = "_txtOpenClaim_23"
        Me._txtOpenClaim_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_23.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_23.TabIndex = 106
        '
        '_txtOpenClaim_22
        '
        Me._txtOpenClaim_22.AcceptsReturn = True
        Me._txtOpenClaim_22.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_22.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_22.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_22.Location = New System.Drawing.Point(168, 150)
        Me._txtOpenClaim_22.MaxLength = 50
        Me._txtOpenClaim_22.Name = "_txtOpenClaim_22"
        Me._txtOpenClaim_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_22.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_22.TabIndex = 107
        '
        '_txtOpenClaim_21
        '
        Me._txtOpenClaim_21.AcceptsReturn = True
        Me._txtOpenClaim_21.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_21.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_21.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_21.Location = New System.Drawing.Point(424, 150)
        Me._txtOpenClaim_21.MaxLength = 50
        Me._txtOpenClaim_21.Name = "_txtOpenClaim_21"
        Me._txtOpenClaim_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_21.Size = New System.Drawing.Size(177, 20)
        Me._txtOpenClaim_21.TabIndex = 108
        '
        '_txtOpenClaim_20
        '
        Me._txtOpenClaim_20.AcceptsReturn = True
        Me._txtOpenClaim_20.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_20.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_20.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_20.Location = New System.Drawing.Point(424, 125)
        Me._txtOpenClaim_20.MaxLength = 50
        Me._txtOpenClaim_20.Name = "_txtOpenClaim_20"
        Me._txtOpenClaim_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_20.Size = New System.Drawing.Size(177, 20)
        Me._txtOpenClaim_20.TabIndex = 109
        '
        '_txtOpenClaim_19
        '
        Me._txtOpenClaim_19.AcceptsReturn = True
        Me._txtOpenClaim_19.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_19.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_19.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_19.Location = New System.Drawing.Point(424, 100)
        Me._txtOpenClaim_19.MaxLength = 50
        Me._txtOpenClaim_19.Name = "_txtOpenClaim_19"
        Me._txtOpenClaim_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_19.Size = New System.Drawing.Size(177, 20)
        Me._txtOpenClaim_19.TabIndex = 110
        '
        '_txtOpenClaim_17
        '
        Me._txtOpenClaim_17.AcceptsReturn = True
        Me._txtOpenClaim_17.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_17.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_17.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_17.Location = New System.Drawing.Point(168, 100)
        Me._txtOpenClaim_17.MaxLength = 50
        Me._txtOpenClaim_17.Name = "_txtOpenClaim_17"
        Me._txtOpenClaim_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_17.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_17.TabIndex = 111
        '
        '_txtOpenClaim_16
        '
        Me._txtOpenClaim_16.AcceptsReturn = True
        Me._txtOpenClaim_16.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_16.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_16.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_16.Location = New System.Drawing.Point(168, 45)
        Me._txtOpenClaim_16.MaxLength = 0
        Me._txtOpenClaim_16.Multiline = True
        Me._txtOpenClaim_16.Name = "_txtOpenClaim_16"
        Me._txtOpenClaim_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_16.Size = New System.Drawing.Size(433, 50)
        Me._txtOpenClaim_16.TabIndex = 112
        '
        '_txtOpenClaim_15
        '
        Me._txtOpenClaim_15.AcceptsReturn = True
        Me._txtOpenClaim_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_15.Location = New System.Drawing.Point(168, 20)
        Me._txtOpenClaim_15.MaxLength = 50
        Me._txtOpenClaim_15.Name = "_txtOpenClaim_15"
        Me._txtOpenClaim_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_15.Size = New System.Drawing.Size(433, 20)
        Me._txtOpenClaim_15.TabIndex = 113
        '
        'CmdClient
        '
        Me.CmdClient.BackColor = System.Drawing.SystemColors.Control
        Me.CmdClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdClient.Location = New System.Drawing.Point(16, 45)
        Me.CmdClient.Name = "CmdClient"
        Me.CmdClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdClient.Size = New System.Drawing.Size(80, 22)
        Me.CmdClient.TabIndex = 114
        Me.CmdClient.Text = "&Address..."
        Me.CmdClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdClient.UseVisualStyleBackColor = False
        '
        'chkVATRegistered
        '
        Me.chkVATRegistered.BackColor = System.Drawing.SystemColors.Control
        Me.chkVATRegistered.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkVATRegistered.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVATRegistered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkVATRegistered.Location = New System.Drawing.Point(424, 177)
        Me.chkVATRegistered.Name = "chkVATRegistered"
        Me.chkVATRegistered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVATRegistered.Size = New System.Drawing.Size(41, 14)
        Me.chkVATRegistered.TabIndex = 115
        Me.chkVATRegistered.Text = "  "
        Me.chkVATRegistered.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblInsurerName)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblEmailIns)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblContact)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblIFaxNumber)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblTelephoneNumber)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblInsurerClaimNumber)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraCoinsurers)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_25)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_26)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_27)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_28)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_29)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_30)
        Me._tabMainTab_TabPage3.Controls.Add(Me._txtOpenClaim_24)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cmdInsurer)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cmdInsurerDetails)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(631, 396)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Insurer Details"
        '
        'lblInsurerName
        '
        Me.lblInsurerName.AutoSize = True
        Me.lblInsurerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerName.Location = New System.Drawing.Point(16, 20)
        Me.lblInsurerName.Name = "lblInsurerName"
        Me.lblInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerName.Size = New System.Drawing.Size(71, 13)
        Me.lblInsurerName.TabIndex = 152
        Me.lblInsurerName.Text = "Insurer name:"
        '
        'lblEmailIns
        '
        Me.lblEmailIns.AutoSize = True
        Me.lblEmailIns.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmailIns.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmailIns.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailIns.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmailIns.Location = New System.Drawing.Point(16, 178)
        Me.lblEmailIns.Name = "lblEmailIns"
        Me.lblEmailIns.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmailIns.Size = New System.Drawing.Size(75, 13)
        Me.lblEmailIns.TabIndex = 153
        Me.lblEmailIns.Text = "Email address:"
        '
        'lblContact
        '
        Me.lblContact.AutoSize = True
        Me.lblContact.BackColor = System.Drawing.SystemColors.Control
        Me.lblContact.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContact.Location = New System.Drawing.Point(16, 153)
        Me.lblContact.Name = "lblContact"
        Me.lblContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContact.Size = New System.Drawing.Size(47, 13)
        Me.lblContact.TabIndex = 154
        Me.lblContact.Text = "Contact:"
        '
        'lblIFaxNumber
        '
        Me.lblIFaxNumber.AutoSize = True
        Me.lblIFaxNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblIFaxNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIFaxNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIFaxNumber.Location = New System.Drawing.Point(16, 127)
        Me.lblIFaxNumber.Name = "lblIFaxNumber"
        Me.lblIFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIFaxNumber.Size = New System.Drawing.Size(65, 13)
        Me.lblIFaxNumber.TabIndex = 155
        Me.lblIFaxNumber.Text = "Fax number:"
        '
        'lblTelephoneNumber
        '
        Me.lblTelephoneNumber.AutoSize = True
        Me.lblTelephoneNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephoneNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephoneNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephoneNumber.Location = New System.Drawing.Point(16, 102)
        Me.lblTelephoneNumber.Name = "lblTelephoneNumber"
        Me.lblTelephoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephoneNumber.Size = New System.Drawing.Size(99, 13)
        Me.lblTelephoneNumber.TabIndex = 156
        Me.lblTelephoneNumber.Text = "Telephone number:"
        '
        'lblInsurerClaimNumber
        '
        Me.lblInsurerClaimNumber.AutoSize = True
        Me.lblInsurerClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerClaimNumber.Location = New System.Drawing.Point(16, 203)
        Me.lblInsurerClaimNumber.Name = "lblInsurerClaimNumber"
        Me.lblInsurerClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerClaimNumber.Size = New System.Drawing.Size(107, 13)
        Me.lblInsurerClaimNumber.TabIndex = 157
        Me.lblInsurerClaimNumber.Text = "Insurer claim number:"
        '
        'fraCoinsurers
        '
        Me.fraCoinsurers.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoinsurers.Controls.Add(Me.txtAllocated)
        Me.fraCoinsurers.Controls.Add(Me.txtFormatPercent)
        Me.fraCoinsurers.Controls.Add(Me.lvwCoinsurers)
        Me.fraCoinsurers.Controls.Add(Me.lblAllocated)
        Me.fraCoinsurers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoinsurers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoinsurers.Location = New System.Drawing.Point(8, 228)
        Me.fraCoinsurers.Name = "fraCoinsurers"
        Me.fraCoinsurers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoinsurers.Size = New System.Drawing.Size(593, 137)
        Me.fraCoinsurers.TabIndex = 60
        Me.fraCoinsurers.TabStop = False
        Me.fraCoinsurers.Text = "Co-Insurer Information"
        '
        'txtAllocated
        '
        Me.txtAllocated.AcceptsReturn = True
        Me.txtAllocated.BackColor = System.Drawing.SystemColors.Window
        Me.txtAllocated.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAllocated.Enabled = False
        Me.txtAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllocated.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAllocated.Location = New System.Drawing.Point(88, 16)
        Me.txtAllocated.MaxLength = 0
        Me.txtAllocated.Name = "txtAllocated"
        Me.txtAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllocated.Size = New System.Drawing.Size(73, 20)
        Me.txtAllocated.TabIndex = 62
        Me.txtAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFormatPercent
        '
        Me.txtFormatPercent.AcceptsReturn = True
        Me.txtFormatPercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatPercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatPercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatPercent.Location = New System.Drawing.Point(184, 16)
        Me.txtFormatPercent.MaxLength = 0
        Me.txtFormatPercent.Name = "txtFormatPercent"
        Me.txtFormatPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatPercent.Size = New System.Drawing.Size(73, 20)
        Me.txtFormatPercent.TabIndex = 61
        Me.txtFormatPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFormatPercent.Visible = False
        '
        'lvwCoinsurers
        '
        Me.lvwCoinsurers.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lvwCoinsurers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCoinsurers_ColumnHeader_1, Me._lvwCoinsurers_ColumnHeader_2})
        Me.lvwCoinsurers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCoinsurers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCoinsurers.LabelEdit = True
        Me.lvwCoinsurers.Location = New System.Drawing.Point(8, 40)
        Me.lvwCoinsurers.Name = "lvwCoinsurers"
        Me.lvwCoinsurers.Size = New System.Drawing.Size(577, 89)
        Me.lvwCoinsurers.TabIndex = 63
        Me.lvwCoinsurers.UseCompatibleStateImageBehavior = False
        Me.lvwCoinsurers.View = System.Windows.Forms.View.Details
        '
        '_lvwCoinsurers_ColumnHeader_1
        '
        Me._lvwCoinsurers_ColumnHeader_1.Text = "Insurer"
        Me._lvwCoinsurers_ColumnHeader_1.Width = 167
        '
        '_lvwCoinsurers_ColumnHeader_2
        '
        Me._lvwCoinsurers_ColumnHeader_2.Text = "% Taken"
        Me._lvwCoinsurers_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCoinsurers_ColumnHeader_2.Width = 67
        '
        'lblAllocated
        '
        Me.lblAllocated.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocated.Location = New System.Drawing.Point(8, 16)
        Me.lblAllocated.Name = "lblAllocated"
        Me.lblAllocated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocated.Size = New System.Drawing.Size(81, 17)
        Me.lblAllocated.TabIndex = 64
        Me.lblAllocated.Text = "% Allocated:"
        '
        '_txtOpenClaim_25
        '
        Me._txtOpenClaim_25.AcceptsReturn = True
        Me._txtOpenClaim_25.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_25.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_25.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_25.Location = New System.Drawing.Point(168, 45)
        Me._txtOpenClaim_25.MaxLength = 0
        Me._txtOpenClaim_25.Multiline = True
        Me._txtOpenClaim_25.Name = "_txtOpenClaim_25"
        Me._txtOpenClaim_25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_25.Size = New System.Drawing.Size(361, 50)
        Me._txtOpenClaim_25.TabIndex = 116
        '
        '_txtOpenClaim_26
        '
        Me._txtOpenClaim_26.AcceptsReturn = True
        Me._txtOpenClaim_26.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_26.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_26.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_26.Location = New System.Drawing.Point(168, 100)
        Me._txtOpenClaim_26.MaxLength = 50
        Me._txtOpenClaim_26.Name = "_txtOpenClaim_26"
        Me._txtOpenClaim_26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_26.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_26.TabIndex = 117
        '
        '_txtOpenClaim_27
        '
        Me._txtOpenClaim_27.AcceptsReturn = True
        Me._txtOpenClaim_27.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_27.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_27.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_27.Location = New System.Drawing.Point(168, 125)
        Me._txtOpenClaim_27.MaxLength = 50
        Me._txtOpenClaim_27.Name = "_txtOpenClaim_27"
        Me._txtOpenClaim_27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_27.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_27.TabIndex = 118
        '
        '_txtOpenClaim_28
        '
        Me._txtOpenClaim_28.AcceptsReturn = True
        Me._txtOpenClaim_28.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_28.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_28.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_28.Location = New System.Drawing.Point(168, 151)
        Me._txtOpenClaim_28.MaxLength = 50
        Me._txtOpenClaim_28.Name = "_txtOpenClaim_28"
        Me._txtOpenClaim_28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_28.Size = New System.Drawing.Size(361, 20)
        Me._txtOpenClaim_28.TabIndex = 119
        '
        '_txtOpenClaim_29
        '
        Me._txtOpenClaim_29.AcceptsReturn = True
        Me._txtOpenClaim_29.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_29.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_29.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_29.Location = New System.Drawing.Point(168, 176)
        Me._txtOpenClaim_29.MaxLength = 50
        Me._txtOpenClaim_29.Name = "_txtOpenClaim_29"
        Me._txtOpenClaim_29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_29.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_29.TabIndex = 120
        '
        '_txtOpenClaim_30
        '
        Me._txtOpenClaim_30.AcceptsReturn = True
        Me._txtOpenClaim_30.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_30.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_30.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_30.Location = New System.Drawing.Point(168, 201)
        Me._txtOpenClaim_30.MaxLength = 50
        Me._txtOpenClaim_30.Name = "_txtOpenClaim_30"
        Me._txtOpenClaim_30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_30.Size = New System.Drawing.Size(153, 20)
        Me._txtOpenClaim_30.TabIndex = 121
        '
        '_txtOpenClaim_24
        '
        Me._txtOpenClaim_24.AcceptsReturn = True
        Me._txtOpenClaim_24.BackColor = System.Drawing.SystemColors.Window
        Me._txtOpenClaim_24.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOpenClaim_24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOpenClaim_24.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOpenClaim_24.Location = New System.Drawing.Point(168, 20)
        Me._txtOpenClaim_24.MaxLength = 50
        Me._txtOpenClaim_24.Name = "_txtOpenClaim_24"
        Me._txtOpenClaim_24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOpenClaim_24.Size = New System.Drawing.Size(361, 20)
        Me._txtOpenClaim_24.TabIndex = 122
        '
        'cmdInsurer
        '
        Me.cmdInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurer.Location = New System.Drawing.Point(16, 45)
        Me.cmdInsurer.Name = "cmdInsurer"
        Me.cmdInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurer.Size = New System.Drawing.Size(80, 22)
        Me.cmdInsurer.TabIndex = 123
        Me.cmdInsurer.Text = "&Address..."
        Me.cmdInsurer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurer.UseVisualStyleBackColor = False
        '
        'cmdInsurerDetails
        '
        Me.cmdInsurerDetails.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurerDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurerDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurerDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurerDetails.Location = New System.Drawing.Point(17, 20)
        Me.cmdInsurerDetails.Name = "cmdInsurerDetails"
        Me.cmdInsurerDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurerDetails.Size = New System.Drawing.Size(80, 22)
        Me.cmdInsurerDetails.TabIndex = 124
        Me.cmdInsurerDetails.Text = "&Insurer..."
        Me.cmdInsurerDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurerDetails.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.uctCLMListPaymentsC1)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(631, 396)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - History"
        '
        'uctCLMListPaymentsC1
        '
        Me.uctCLMListPaymentsC1.ClaimId = 0
        Me.uctCLMListPaymentsC1.CountColumn = 0
        Me.uctCLMListPaymentsC1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMListPaymentsC1.Location = New System.Drawing.Point(0, 3)
        Me.uctCLMListPaymentsC1.Name = "uctCLMListPaymentsC1"
        Me.uctCLMListPaymentsC1.selectedItem = 0
        Me.uctCLMListPaymentsC1.ShowPaymentView = False
        Me.uctCLMListPaymentsC1.Size = New System.Drawing.Size(612, 364)
        Me.uctCLMListPaymentsC1.TabIndex = 130
        Me.uctCLMListPaymentsC1.visibleCmdView = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        Me.ImageList1.Images.SetKeyName(12, "")
        Me.ImageList1.Images.SetKeyName(13, "")
        Me.ImageList1.Images.SetKeyName(14, "")
        Me.ImageList1.Images.SetKeyName(15, "")
        Me.ImageList1.Images.SetKeyName(16, "")
        Me.ImageList1.Images.SetKeyName(17, "")
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(659, 490)
        Me.Controls.Add(Me.cmdChangeClientPolicy)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 149)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "."
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraDeductibles.ResumeLayout(False)
        Me.fraDeductibles.PerformLayout()
        Me.fraNCDDetails.ResumeLayout(False)
        Me.fraULRDetails.ResumeLayout(False)
        Me.fraULRDetails.PerformLayout()
        Me.fraEmployeeDetails.ResumeLayout(False)
        Me.fraEmployeeDetails.PerformLayout()
        Me.fraPreviousClaimDetails.ResumeLayout(False)
        Me.fraPreviousClaimDetails.PerformLayout()
        Me.fraInsuredDriverDetails.ResumeLayout(False)
        Me.fraInsuredDriverDetails.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage3.PerformLayout()
        Me.fraCoinsurers.ResumeLayout(False)
        Me.fraCoinsurers.PerformLayout()
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializetxtOpenClaim()
        Me.txtOpenClaim(3) = _txtOpenClaim_3
        Me.txtOpenClaim(11) = _txtOpenClaim_11
        Me.txtOpenClaim(13) = _txtOpenClaim_13
        Me.txtOpenClaim(12) = _txtOpenClaim_12
        Me.txtOpenClaim(8) = _txtOpenClaim_8
        Me.txtOpenClaim(7) = _txtOpenClaim_7
        Me.txtOpenClaim(4) = _txtOpenClaim_4
        Me.txtOpenClaim(9) = _txtOpenClaim_9
        Me.txtOpenClaim(6) = _txtOpenClaim_6
        Me.txtOpenClaim(5) = _txtOpenClaim_5
        Me.txtOpenClaim(2) = _txtOpenClaim_2
        Me.txtOpenClaim(1) = _txtOpenClaim_1
        Me.txtOpenClaim(0) = _txtOpenClaim_0
        Me.txtOpenClaim(10) = _txtOpenClaim_10
        Me.txtOpenClaim(24) = _txtOpenClaim_24
        Me.txtOpenClaim(30) = _txtOpenClaim_30
        Me.txtOpenClaim(29) = _txtOpenClaim_29
        Me.txtOpenClaim(28) = _txtOpenClaim_28
        Me.txtOpenClaim(27) = _txtOpenClaim_27
        Me.txtOpenClaim(26) = _txtOpenClaim_26
        Me.txtOpenClaim(25) = _txtOpenClaim_25
        Me.txtOpenClaim(15) = _txtOpenClaim_15
        Me.txtOpenClaim(16) = _txtOpenClaim_16
        Me.txtOpenClaim(17) = _txtOpenClaim_17
        Me.txtOpenClaim(19) = _txtOpenClaim_19
        Me.txtOpenClaim(20) = _txtOpenClaim_20
        Me.txtOpenClaim(21) = _txtOpenClaim_21
        Me.txtOpenClaim(22) = _txtOpenClaim_22
        Me.txtOpenClaim(23) = _txtOpenClaim_23
        Me.txtOpenClaim(18) = _txtOpenClaim_18
        Me.txtOpenClaim(31) = _txtOpenClaim_31
        Me.txtOpenClaim(32) = _txtOpenClaim_32
        Me.txtOpenClaim(33) = _txtOpenClaim_33
        Me.txtOpenClaim(37) = _txtOpenClaim_37
        Me.txtOpenClaim(34) = _txtOpenClaim_34
        Me.txtOpenClaim(35) = _txtOpenClaim_35
        Me.txtOpenClaim(36) = _txtOpenClaim_36
        Me.txtOpenClaim(38) = _txtOpenClaim_38
        Me.txtOpenClaim(39) = _txtOpenClaim_39
        Me.txtOpenClaim(40) = _txtOpenClaim_40
        Me.txtOpenClaim(41) = _txtOpenClaim_41
        Me.txtOpenClaim(42) = _txtOpenClaim_42
        Me.txtOpenClaim(44) = _txtOpenClaim_44
        Me.txtOpenClaim(45) = _txtOpenClaim_45
        Me.txtOpenClaim(43) = _txtOpenClaim_43
    End Sub
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents chkClaimHandled As GEMControlLib.YesNoCheck
    Friend WithEvents cmdTPA As System.Windows.Forms.Button
    Public WithEvents txtTPA As System.Windows.Forms.TextBox
#End Region
End Class