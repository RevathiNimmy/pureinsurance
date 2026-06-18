'TODO
Imports PMLookupControl
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMUPolicyControl
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializecmdPrevious()
        InitializecmdNext()
        lvwClients_InitializeColumnKeys()
        lvwAgents_InitializeColumnKeys()
        lvwPolicyWording_InitializeColumnKeys()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
        UserControl_InitProperties()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Friend WithEvents chkConsolidatedLeadCommission As System.Windows.Forms.CheckBox
    Friend WithEvents cmdHandler As System.Windows.Forms.Button
    Friend WithEvents cboProduct As System.Windows.Forms.ComboBox
    Friend WithEvents txtInsuredName As System.Windows.Forms.TextBox
    Friend WithEvents txtAlternateReference As System.Windows.Forms.TextBox
    Friend WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Friend WithEvents cboBusinessType As System.Windows.Forms.ComboBox
    Friend WithEvents cboAnalysisCode As System.Windows.Forms.ComboBox
    Friend WithEvents chkQuote As System.Windows.Forms.CheckBox
    Friend WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtRegarding As System.Windows.Forms.TextBox
    Friend WithEvents cmdAgentCode As System.Windows.Forms.Button
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents cboBranchCode As System.Windows.Forms.ComboBox
    Friend WithEvents cboPolicyStatus As cboPMLookup
    Friend WithEvents pnlHandler As System.Windows.Forms.Panel
    Friend WithEvents lblHandler As System.Windows.Forms.Label
    Friend WithEvents cboCurrency As System.Windows.Forms.ComboBox
    Friend WithEvents lblConsolidatedLeadCommission As System.Windows.Forms.Label
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents lblPolicyStatus As System.Windows.Forms.Label
    Friend WithEvents lblInsuredName As System.Windows.Forms.Label
    Friend WithEvents lblAlternateReference As System.Windows.Forms.Label
    Friend WithEvents lblSubBranch As System.Windows.Forms.Label
    Friend WithEvents lblBranchCode As System.Windows.Forms.Label
    Friend WithEvents lblBusinessType As System.Windows.Forms.Label
    Friend WithEvents lblAnalysisCode As System.Windows.Forms.Label
    Friend WithEvents lblProduct As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents llblPolicyOrProspect As System.Windows.Forms.Label
    Friend WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Friend WithEvents lblRegarding As System.Windows.Forms.Label
    Friend WithEvents fraRisks As System.Windows.Forms.GroupBox

    Friend WithEvents pnlPolicyType As System.Windows.Forms.Label
    Friend WithEvents lblPolicyType As System.Windows.Forms.Label
    Friend WithEvents pnlScheme As System.Windows.Forms.Label
    Friend WithEvents lblScheme As System.Windows.Forms.Label
    Friend WithEvents fraStatus As System.Windows.Forms.GroupBox
    Friend WithEvents txtAnniversaryDate As System.Windows.Forms.TextBox
    Friend WithEvents txtInceptionTPI As System.Windows.Forms.TextBox
    Friend WithEvents txtProposalDate As System.Windows.Forms.TextBox
    Friend WithEvents txtInceptionDate As System.Windows.Forms.TextBox
    Friend WithEvents txtCoverFromDate As System.Windows.Forms.TextBox
    Friend WithEvents cboPolicyLimits As cboPMLookup
    Friend WithEvents cboRenewalDayNumber As System.Windows.Forms.ComboBox
    Friend WithEvents chkPutOnNextInstalmentRenewal As System.Windows.Forms.CheckBox
    Friend WithEvents txtQuoteExpiryDate As System.Windows.Forms.TextBox
    Friend WithEvents txtIssuedDate As System.Windows.Forms.TextBox
    Friend WithEvents txtCoverToDate As System.Windows.Forms.TextBox
    Friend WithEvents txtRenewalDate As System.Windows.Forms.TextBox
    Friend WithEvents cboPolicyDeductible As cboPMLookup
    Friend WithEvents cboUnderwritingYearID As cboPMLookup
    Friend WithEvents lblPolicyLimits As System.Windows.Forms.Label
    Friend WithEvents lblPolicyDeductible As System.Windows.Forms.Label
    Friend WithEvents lblPutOnNextInstalmentRenewal As System.Windows.Forms.Label
    Friend WithEvents lblAnniversaryDate As System.Windows.Forms.Label
    Friend WithEvents lblInceptionTPI As System.Windows.Forms.Label
    Friend WithEvents lblQuoteExpiryDate As System.Windows.Forms.Label
    Friend WithEvents lblProposalDate As System.Windows.Forms.Label
    Friend WithEvents lblIssuedDate As System.Windows.Forms.Label
    Friend WithEvents lblRenewalDate As System.Windows.Forms.Label
    Friend WithEvents lblCoverToDate As System.Windows.Forms.Label
    Friend WithEvents lblCoverFromDate As System.Windows.Forms.Label
    Friend WithEvents lblInceptionDate As System.Windows.Forms.Label
    Friend WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Friend WithEvents lblUnderwritingYearID As System.Windows.Forms.Label
    Friend WithEvents fraDates As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents lblManualDiscountPercentage As System.Windows.Forms.Label
    Friend WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Friend WithEvents chkReferredOnMTA As System.Windows.Forms.CheckBox
    Friend WithEvents chkReferredAtRenewal As System.Windows.Forms.CheckBox
    Friend WithEvents cboRenewalMethod As System.Windows.Forms.ComboBox
    Friend WithEvents txtLapsedDate As System.Windows.Forms.TextBox
    Friend WithEvents cboLapsedReason As System.Windows.Forms.ComboBox
    Friend WithEvents cboFrequency As System.Windows.Forms.ComboBox
    Friend WithEvents txtPolicyLTUExpiryDate As System.Windows.Forms.TextBox
    Friend WithEvents cboRenewalStop As System.Windows.Forms.ComboBox
    Friend WithEvents pnlRenewalCount As System.Windows.Forms.Panel
    Friend WithEvents lblRenCount As System.Windows.Forms.Label
    Friend WithEvents lblReferredOnMTA As System.Windows.Forms.Label
    Friend WithEvents lblReferredAtRenewal As System.Windows.Forms.Label
    Friend WithEvents lblRenewalMethod As System.Windows.Forms.Label
    Friend WithEvents lblLapsedDate As System.Windows.Forms.Label
    Friend WithEvents lblLapsedReason As System.Windows.Forms.Label
    Friend WithEvents lblFrequency As System.Windows.Forms.Label
    Friend WithEvents lblRenewalCount As System.Windows.Forms.Label
    Friend WithEvents lblPolicyLTUExpiryDate As System.Windows.Forms.Label
    Friend WithEvents lblRenewalStop As System.Windows.Forms.Label
    Friend WithEvents fraSource As System.Windows.Forms.GroupBox
    Friend WithEvents txtDiscountedPremium As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscountPercentage As System.Windows.Forms.TextBox
    Friend WithEvents cboDiscountReason As cboPMLookup
    Friend WithEvents cboDiscountRecurringType As cboPMLookup
    Friend WithEvents lblDiscountReason As System.Windows.Forms.Label
    Friend WithEvents lblDiscountedPremium As System.Windows.Forms.Label
    Friend WithEvents lblDiscountPercentage As System.Windows.Forms.Label
    Friend WithEvents lblRecurring As System.Windows.Forms.Label
    Friend WithEvents fraDiscount As System.Windows.Forms.GroupBox
    Friend WithEvents _lvwPolicyWording_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwPolicyWording_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwPolicyWording As System.Windows.Forms.ListView
    Friend WithEvents cmdDownNarrative As System.Windows.Forms.Button
    Friend WithEvents cmdUpNarrative As System.Windows.Forms.Button
    Friend WithEvents cmdAddPolicyWording As System.Windows.Forms.Button
    Friend WithEvents cmdDeletePolicyWording As System.Windows.Forms.Button
    Friend WithEvents cboPolicyStyle As cboPMLookup
    Friend WithEvents lblPolicyStyle As System.Windows.Forms.Label
    Friend WithEvents lblMove As System.Windows.Forms.Label
    Friend WithEvents fraNarrations As System.Windows.Forms.GroupBox
    Friend WithEvents txtFutureAnnualPremium As System.Windows.Forms.TextBox
    Friend WithEvents txtPremiumExcTax As System.Windows.Forms.TextBox
    Friend WithEvents txtOldPolicyNo As System.Windows.Forms.TextBox
    Friend WithEvents cmdCoInsurer As System.Windows.Forms.Button
    Friend WithEvents lblFutureAnnualPremium As System.Windows.Forms.Label
    Friend WithEvents lblPremiumExcTax As System.Windows.Forms.Label
    Friend WithEvents lblOldPolicyNo As System.Windows.Forms.Label
    Friend WithEvents fraExtra As System.Windows.Forms.GroupBox
    Friend WithEvents txtManualDiscountPercentage As System.Windows.Forms.TextBox
    Friend WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents chkConsolidatedSubCommission As System.Windows.Forms.CheckBox
    Friend WithEvents cmdAddAgent As System.Windows.Forms.Button
    Friend WithEvents cmdDeleteAgent As System.Windows.Forms.Button
    Friend WithEvents cmdEditAgent As System.Windows.Forms.Button
    Friend WithEvents _lvwAgents_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwAgents_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwAgents_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwAgents_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwAgents As System.Windows.Forms.ListView
    Friend WithEvents lblConsolidatedSubCommission As System.Windows.Forms.Label
    Friend WithEvents fraAgents As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Friend WithEvents txtCoverNoteSheet As System.Windows.Forms.TextBox
    Friend WithEvents txtCoverNoteBook As System.Windows.Forms.TextBox
    Friend WithEvents lblCoverNoteSheet As System.Windows.Forms.Label
    Friend WithEvents lblCoverNoteBook As System.Windows.Forms.Label
    Friend WithEvents fraCoverNote As System.Windows.Forms.GroupBox
    Friend WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Friend WithEvents cmdDeleteClient As System.Windows.Forms.Button
    Friend WithEvents cmdAddClient As System.Windows.Forms.Button
    Friend WithEvents cmdSetCorrespondence As System.Windows.Forms.Button
    Friend WithEvents _lvwClients_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwClients_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwClients_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwClients_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwClients_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwClients As System.Windows.Forms.ListView
    Friend WithEvents fraClients As System.Windows.Forms.GroupBox
    Friend WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    Friend WithEvents txtAgentAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtAgentPercentage As System.Windows.Forms.TextBox
    Friend WithEvents txtAgentName As System.Windows.Forms.TextBox
    Friend WithEvents txtAgentCode As System.Windows.Forms.TextBox
    Friend WithEvents lblAgentAmount As System.Windows.Forms.Label
    Friend WithEvents lblAgentPercentage As System.Windows.Forms.Label
    Friend WithEvents lblAgentName As System.Windows.Forms.Label
    Friend WithEvents lblAgentCode As System.Windows.Forms.Label
    Friend WithEvents SSFrame1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdAgentMain As System.Windows.Forms.Button
    Friend WithEvents _tabAgent_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents tabAgent As System.Windows.Forms.TabControl
    Friend WithEvents txtFeeIPTable As System.Windows.Forms.TextBox
    Friend WithEvents txtFeeCommissionAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Friend WithEvents chkOverrideRateTable As System.Windows.Forms.CheckBox
    Friend WithEvents txtCommissionPercentage As System.Windows.Forms.TextBox
    Friend WithEvents txtCommissionPayable As System.Windows.Forms.TextBox
    Friend WithEvents txtCommissionCharge As System.Windows.Forms.TextBox
    Friend WithEvents pnlPremiumAmount As System.Windows.Forms.Panel
    Friend WithEvents pnlCommissionAccount As System.Windows.Forms.Panel
    Friend WithEvents lblCommAccount As System.Windows.Forms.Label
    Friend WithEvents lblCommissionPercentage As System.Windows.Forms.Label
    Friend WithEvents lblCommissionCharge As System.Windows.Forms.Label
    Friend WithEvents lblCommissionPremium As System.Windows.Forms.Label
    Friend WithEvents lblCommissionAccount As System.Windows.Forms.Label
    Friend WithEvents lblCommissionPayable As System.Windows.Forms.Label
    Friend WithEvents lblOverride As System.Windows.Forms.Label
    Friend WithEvents fraCommission As System.Windows.Forms.GroupBox
    Friend WithEvents cmdMain As System.Windows.Forms.Button
    Friend WithEvents _tabCommissionTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents tabCommissionTab As System.Windows.Forms.TabControl
    Friend WithEvents cboRelationship As System.Windows.Forms.ComboBox
    Friend WithEvents cmdRelatedPolicy As System.Windows.Forms.Button
    Friend WithEvents pnlRelatedPolicy As System.Windows.Forms.Panel
    Friend WithEvents lblRelatedPolicy As System.Windows.Forms.Label
    Friend WithEvents lblRelationship As System.Windows.Forms.Label
    Friend WithEvents fraStorage As System.Windows.Forms.GroupBox
    Friend WithEvents txtPercentage As System.Windows.Forms.TextBox
    Friend WithEvents txtFeeCommissionPercentage As System.Windows.Forms.TextBox
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend cmdNext(3) As System.Windows.Forms.Button
    Friend cmdPrevious(4) As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    ' <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPMUPolicyControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.gbPreferredCorrespondence = New System.Windows.Forms.GroupBox()
        Me.txtCorrespondenceType = New System.Windows.Forms.TextBox()
        Me.cboCorrespondenceMethod = New PMLookupControl.cboPMLookup()
        Me.lblCorrespondenceType = New System.Windows.Forms.Label()
        Me.fraRisks = New System.Windows.Forms.GroupBox()
        Me.chkHandler = New System.Windows.Forms.CheckBox()
        Me.txtRegarding = New System.Windows.Forms.TextBox()
        Me.cboAgencyContact = New System.Windows.Forms.ComboBox()
        Me.grpCoInsuranceLead = New System.Windows.Forms.GroupBox()
        Me.optNet = New System.Windows.Forms.RadioButton()
        Me.optGross = New System.Windows.Forms.RadioButton()
        Me.lblCoinsurancePlacement = New System.Windows.Forms.Label()
        Me.pnlAgentCode = New System.Windows.Forms.TextBox()
        Me.lblAgencyContact = New System.Windows.Forms.Label()
        Me.chkConsolidatedLeadCommission = New System.Windows.Forms.CheckBox()
        Me.cmdHandler = New System.Windows.Forms.Button()
        Me.cboProduct = New System.Windows.Forms.ComboBox()
        Me.txtInsuredName = New System.Windows.Forms.TextBox()
        Me.txtAlternateReference = New System.Windows.Forms.TextBox()
        Me.cboSubBranch = New System.Windows.Forms.ComboBox()
        Me.cboBusinessType = New System.Windows.Forms.ComboBox()
        Me.cboAnalysisCode = New System.Windows.Forms.ComboBox()
        Me.chkQuote = New System.Windows.Forms.CheckBox()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.cmdAgentCode = New System.Windows.Forms.Button()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.cboBranchCode = New System.Windows.Forms.ComboBox()
        Me.cboPolicyStatus = New PMLookupControl.cboPMLookup()
        Me.pnlHandler = New System.Windows.Forms.Panel()
        Me.lblHandler = New System.Windows.Forms.Label()
        Me.cboCurrency = New System.Windows.Forms.ComboBox()
        Me.lblConsolidatedLeadCommission = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblPolicyStatus = New System.Windows.Forms.Label()
        Me.lblInsuredName = New System.Windows.Forms.Label()
        Me.lblAlternateReference = New System.Windows.Forms.Label()
        Me.lblSubBranch = New System.Windows.Forms.Label()
        Me.lblBranchCode = New System.Windows.Forms.Label()
        Me.lblBusinessType = New System.Windows.Forms.Label()
        Me.lblAnalysisCode = New System.Windows.Forms.Label()
        Me.lblProduct = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.llblPolicyOrProspect = New System.Windows.Forms.Label()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.lblRegarding = New System.Windows.Forms.Label()
        Me.fraStatus = New System.Windows.Forms.GroupBox()
        Me.pnlPolicyType = New System.Windows.Forms.Label()
        Me.lblPolicyType = New System.Windows.Forms.Label()
        Me.pnlScheme = New System.Windows.Forms.Label()
        Me.lblScheme = New System.Windows.Forms.Label()
        Me.fraDates = New System.Windows.Forms.GroupBox()
        Me.txtAnniversaryDate = New System.Windows.Forms.TextBox()
        Me.txtInceptionTPI = New System.Windows.Forms.TextBox()
        Me.txtProposalDate = New System.Windows.Forms.TextBox()
        Me.txtInceptionDate = New System.Windows.Forms.TextBox()
        Me.txtCoverFromDate = New System.Windows.Forms.TextBox()
        Me.cboPolicyLimits = New PMLookupControl.cboPMLookup()
        Me.cboRenewalDayNumber = New System.Windows.Forms.ComboBox()
        Me.chkPutOnNextInstalmentRenewal = New System.Windows.Forms.CheckBox()
        Me.txtQuoteExpiryDate = New System.Windows.Forms.TextBox()
        Me.txtIssuedDate = New System.Windows.Forms.TextBox()
        Me.txtCoverToDate = New System.Windows.Forms.TextBox()
        Me.txtRenewalDate = New System.Windows.Forms.TextBox()
        Me.cboPolicyDeductible = New PMLookupControl.cboPMLookup()
        Me.cboUnderwritingYearID = New PMLookupControl.cboPMLookup()
        Me.lblPolicyLimits = New System.Windows.Forms.Label()
        Me.lblPolicyDeductible = New System.Windows.Forms.Label()
        Me.lblPutOnNextInstalmentRenewal = New System.Windows.Forms.Label()
        Me.lblAnniversaryDate = New System.Windows.Forms.Label()
        Me.lblInceptionTPI = New System.Windows.Forms.Label()
        Me.lblQuoteExpiryDate = New System.Windows.Forms.Label()
        Me.lblProposalDate = New System.Windows.Forms.Label()
        Me.lblIssuedDate = New System.Windows.Forms.Label()
        Me.lblRenewalDate = New System.Windows.Forms.Label()
        Me.lblCoverToDate = New System.Windows.Forms.Label()
        Me.lblCoverFromDate = New System.Windows.Forms.Label()
        Me.lblInceptionDate = New System.Windows.Forms.Label()
        Me.lblPaymentMethod = New System.Windows.Forms.Label()
        Me.lblUnderwritingYearID = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblManualDiscountPercentage = New System.Windows.Forms.Label()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me.fraSource = New System.Windows.Forms.GroupBox()
        Me.chkReferredOnMTA = New System.Windows.Forms.CheckBox()
        Me.chkReferredAtRenewal = New System.Windows.Forms.CheckBox()
        Me.cboRenewalMethod = New System.Windows.Forms.ComboBox()
        Me.txtLapsedDate = New System.Windows.Forms.TextBox()
        Me.cboLapsedReason = New System.Windows.Forms.ComboBox()
        Me.cboFrequency = New System.Windows.Forms.ComboBox()
        Me.txtPolicyLTUExpiryDate = New System.Windows.Forms.TextBox()
        Me.cboRenewalStop = New System.Windows.Forms.ComboBox()
        Me.pnlRenewalCount = New System.Windows.Forms.Panel()
        Me.lblRenCount = New System.Windows.Forms.Label()
        Me.lblReferredOnMTA = New System.Windows.Forms.Label()
        Me.lblReferredAtRenewal = New System.Windows.Forms.Label()
        Me.lblRenewalMethod = New System.Windows.Forms.Label()
        Me.lblLapsedDate = New System.Windows.Forms.Label()
        Me.lblLapsedReason = New System.Windows.Forms.Label()
        Me.lblFrequency = New System.Windows.Forms.Label()
        Me.lblRenewalCount = New System.Windows.Forms.Label()
        Me.lblPolicyLTUExpiryDate = New System.Windows.Forms.Label()
        Me.lblRenewalStop = New System.Windows.Forms.Label()
        Me.fraDiscount = New System.Windows.Forms.GroupBox()
        Me.txtDiscountedPremium = New System.Windows.Forms.TextBox()
        Me.txtDiscountPercentage = New System.Windows.Forms.TextBox()
        Me.cboDiscountReason = New PMLookupControl.cboPMLookup()
        Me.cboDiscountRecurringType = New PMLookupControl.cboPMLookup()
        Me.lblDiscountReason = New System.Windows.Forms.Label()
        Me.lblDiscountedPremium = New System.Windows.Forms.Label()
        Me.lblDiscountPercentage = New System.Windows.Forms.Label()
        Me.lblRecurring = New System.Windows.Forms.Label()
        Me.fraNarrations = New System.Windows.Forms.GroupBox()
        Me.lvwPolicyWording = New System.Windows.Forms.ListView()
        Me._lvwPolicyWording_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyWording_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDownNarrative = New System.Windows.Forms.Button()
        Me.cmdUpNarrative = New System.Windows.Forms.Button()
        Me.cmdAddPolicyWording = New System.Windows.Forms.Button()
        Me.cmdDeletePolicyWording = New System.Windows.Forms.Button()
        Me.cboPolicyStyle = New PMLookupControl.cboPMLookup()
        Me.lblPolicyStyle = New System.Windows.Forms.Label()
        Me.lblMove = New System.Windows.Forms.Label()
        Me.fraExtra = New System.Windows.Forms.GroupBox()
        Me.txtFutureAnnualPremium = New System.Windows.Forms.TextBox()
        Me.txtPremiumExcTax = New System.Windows.Forms.TextBox()
        Me.txtOldPolicyNo = New System.Windows.Forms.TextBox()
        Me.cmdCoInsurer = New System.Windows.Forms.Button()
        Me.lblFutureAnnualPremium = New System.Windows.Forms.Label()
        Me.lblPremiumExcTax = New System.Windows.Forms.Label()
        Me.lblOldPolicyNo = New System.Windows.Forms.Label()
        Me.txtManualDiscountPercentage = New System.Windows.Forms.TextBox()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraAgents = New System.Windows.Forms.GroupBox()
        Me.chkConsolidatedSubCommission = New System.Windows.Forms.CheckBox()
        Me.cmdAddAgent = New System.Windows.Forms.Button()
        Me.cmdDeleteAgent = New System.Windows.Forms.Button()
        Me.cmdEditAgent = New System.Windows.Forms.Button()
        Me.lvwAgents = New System.Windows.Forms.ListView()
        Me._lvwAgents_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAgents_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAgents_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAgents_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblConsolidatedSubCommission = New System.Windows.Forms.Label()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me.fraCoverNote = New System.Windows.Forms.GroupBox()
        Me.txtCoverNoteSheet = New System.Windows.Forms.TextBox()
        Me.txtCoverNoteBook = New System.Windows.Forms.TextBox()
        Me.lblCoverNoteSheet = New System.Windows.Forms.Label()
        Me.lblCoverNoteBook = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me.fraClients = New System.Windows.Forms.GroupBox()
        Me.cmdDeleteClient = New System.Windows.Forms.Button()
        Me.cmdAddClient = New System.Windows.Forms.Button()
        Me.cmdSetCorrespondence = New System.Windows.Forms.Button()
        Me.lvwClients = New System.Windows.Forms.ListView()
        Me._lvwClients_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClients_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClients_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClients_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClients_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me.tabAgent = New System.Windows.Forms.TabControl()
        Me._tabAgent_TabPage0 = New System.Windows.Forms.TabPage()
        Me.SSFrame1 = New System.Windows.Forms.GroupBox()
        Me.txtAgentAmount = New System.Windows.Forms.TextBox()
        Me.txtAgentPercentage = New System.Windows.Forms.TextBox()
        Me.txtAgentName = New System.Windows.Forms.TextBox()
        Me.txtAgentCode = New System.Windows.Forms.TextBox()
        Me.lblAgentAmount = New System.Windows.Forms.Label()
        Me.lblAgentPercentage = New System.Windows.Forms.Label()
        Me.lblAgentName = New System.Windows.Forms.Label()
        Me.lblAgentCode = New System.Windows.Forms.Label()
        Me.cmdAgentMain = New System.Windows.Forms.Button()
        Me.txtFeeIPTable = New System.Windows.Forms.TextBox()
        Me.txtFeeCommissionAmount = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabCommissionTab = New System.Windows.Forms.TabControl()
        Me._tabCommissionTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraCommission = New System.Windows.Forms.GroupBox()
        Me.chkOverrideRateTable = New System.Windows.Forms.CheckBox()
        Me.txtCommissionPercentage = New System.Windows.Forms.TextBox()
        Me.txtCommissionPayable = New System.Windows.Forms.TextBox()
        Me.txtCommissionCharge = New System.Windows.Forms.TextBox()
        Me.pnlPremiumAmount = New System.Windows.Forms.Panel()
        Me.pnlCommissionAccount = New System.Windows.Forms.Panel()
        Me.lblCommAccount = New System.Windows.Forms.Label()
        Me.lblCommissionPercentage = New System.Windows.Forms.Label()
        Me.lblCommissionCharge = New System.Windows.Forms.Label()
        Me.lblCommissionPremium = New System.Windows.Forms.Label()
        Me.lblCommissionAccount = New System.Windows.Forms.Label()
        Me.lblCommissionPayable = New System.Windows.Forms.Label()
        Me.lblOverride = New System.Windows.Forms.Label()
        Me.cmdMain = New System.Windows.Forms.Button()
        Me.fraStorage = New System.Windows.Forms.GroupBox()
        Me.cboRelationship = New System.Windows.Forms.ComboBox()
        Me.cmdRelatedPolicy = New System.Windows.Forms.Button()
        Me.pnlRelatedPolicy = New System.Windows.Forms.Panel()
        Me.lblRelatedPolicy = New System.Windows.Forms.Label()
        Me.lblRelationship = New System.Windows.Forms.Label()
        Me.txtPercentage = New System.Windows.Forms.TextBox()
        Me.txtFeeCommissionPercentage = New System.Windows.Forms.TextBox()
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.gbPreferredCorrespondence.SuspendLayout()
        Me.fraRisks.SuspendLayout()
        Me.grpCoInsuranceLead.SuspendLayout()
        Me.pnlHandler.SuspendLayout()
        Me.fraStatus.SuspendLayout()
        Me.fraDates.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraSource.SuspendLayout()
        Me.pnlRenewalCount.SuspendLayout()
        Me.fraDiscount.SuspendLayout()
        Me.fraNarrations.SuspendLayout()
        Me.fraExtra.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraAgents.SuspendLayout()
        Me.fraCoverNote.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraClients.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.tabAgent.SuspendLayout()
        Me._tabAgent_TabPage0.SuspendLayout()
        Me.SSFrame1.SuspendLayout()
        Me.tabCommissionTab.SuspendLayout()
        Me._tabCommissionTab_TabPage0.SuspendLayout()
        Me.fraCommission.SuspendLayout()
        Me.pnlCommissionAccount.SuspendLayout()
        Me.fraStorage.SuspendLayout()
        Me.pnlRelatedPolicy.SuspendLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(120, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(2, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(613, 570)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.gbPreferredCorrespondence)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraRisks)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDates)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(605, 544)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Main Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'gbPreferredCorrespondence
        '
        Me.gbPreferredCorrespondence.Controls.Add(Me.txtCorrespondenceType)
        Me.gbPreferredCorrespondence.Controls.Add(Me.cboCorrespondenceMethod)
        Me.gbPreferredCorrespondence.Controls.Add(Me.lblCorrespondenceType)
        Me.gbPreferredCorrespondence.Location = New System.Drawing.Point(8, 233)
        Me.gbPreferredCorrespondence.Name = "gbPreferredCorrespondence"
        Me.gbPreferredCorrespondence.Size = New System.Drawing.Size(587, 53)
        Me.gbPreferredCorrespondence.TabIndex = 169
        Me.gbPreferredCorrespondence.TabStop = False
        Me.gbPreferredCorrespondence.Text = "Preferred Correspondence"
        '
        'txtCorrespondenceType
        '
        Me.txtCorrespondenceType.Enabled = False
        Me.txtCorrespondenceType.Location = New System.Drawing.Point(317, 19)
        Me.txtCorrespondenceType.Name = "txtCorrespondenceType"
        Me.txtCorrespondenceType.Size = New System.Drawing.Size(153, 21)
        Me.txtCorrespondenceType.TabIndex = 173
        Me.txtCorrespondenceType.Visible = False
        '
        'cboCorrespondenceMethod
        '
        Me.cboCorrespondenceMethod.DefaultItemId = 0
        Me.cboCorrespondenceMethod.FirstItem = ""
        Me.cboCorrespondenceMethod.ItemId = 0
        Me.cboCorrespondenceMethod.ListIndex = -1
        Me.cboCorrespondenceMethod.Location = New System.Drawing.Point(156, 19)
        Me.cboCorrespondenceMethod.Name = "cboCorrespondenceMethod"
        Me.cboCorrespondenceMethod.PMLookupProductFamily = 2
        Me.cboCorrespondenceMethod.SingleItemId = 0
        Me.cboCorrespondenceMethod.Size = New System.Drawing.Size(153, 21)
        Me.cboCorrespondenceMethod.Sorted = True
        Me.cboCorrespondenceMethod.TabIndex = 172
        Me.cboCorrespondenceMethod.TableName = "Correspondence_Type"
        Me.cboCorrespondenceMethod.ToolTipText = ""
        Me.cboCorrespondenceMethod.WhereClause = ""
        '
        'lblCorrespondenceType
        '
        Me.lblCorrespondenceType.AutoSize = True
        Me.lblCorrespondenceType.Location = New System.Drawing.Point(9, 19)
        Me.lblCorrespondenceType.Name = "lblCorrespondenceType"
        Me.lblCorrespondenceType.Size = New System.Drawing.Size(138, 13)
        Me.lblCorrespondenceType.TabIndex = 170
        Me.lblCorrespondenceType.Text = "Client Correspondence"
        '
        'fraRisks
        '
        Me.fraRisks.Controls.Add(Me.chkHandler)
        Me.fraRisks.Controls.Add(Me.txtRegarding)
        Me.fraRisks.Controls.Add(Me.cboAgencyContact)
        Me.fraRisks.Controls.Add(Me.grpCoInsuranceLead)
        Me.fraRisks.Controls.Add(Me.lblCoinsurancePlacement)
        Me.fraRisks.Controls.Add(Me.pnlAgentCode)
        Me.fraRisks.Controls.Add(Me.lblAgencyContact)
        Me.fraRisks.Controls.Add(Me.chkConsolidatedLeadCommission)
        Me.fraRisks.Controls.Add(Me.cmdHandler)
        Me.fraRisks.Controls.Add(Me.cboProduct)
        Me.fraRisks.Controls.Add(Me.txtInsuredName)
        Me.fraRisks.Controls.Add(Me.txtAlternateReference)
        Me.fraRisks.Controls.Add(Me.cboSubBranch)
        Me.fraRisks.Controls.Add(Me.cboBusinessType)
        Me.fraRisks.Controls.Add(Me.cboAnalysisCode)
        Me.fraRisks.Controls.Add(Me.chkQuote)
        Me.fraRisks.Controls.Add(Me.txtPolicyNumber)
        Me.fraRisks.Controls.Add(Me.cmdAgentCode)
        Me.fraRisks.Controls.Add(Me.cboStatus)
        Me.fraRisks.Controls.Add(Me.cboBranchCode)
        Me.fraRisks.Controls.Add(Me.cboPolicyStatus)
        Me.fraRisks.Controls.Add(Me.pnlHandler)
        Me.fraRisks.Controls.Add(Me.cboCurrency)
        Me.fraRisks.Controls.Add(Me.lblConsolidatedLeadCommission)
        Me.fraRisks.Controls.Add(Me.lblCurrency)
        Me.fraRisks.Controls.Add(Me.lblPolicyStatus)
        Me.fraRisks.Controls.Add(Me.lblInsuredName)
        Me.fraRisks.Controls.Add(Me.lblAlternateReference)
        Me.fraRisks.Controls.Add(Me.lblSubBranch)
        Me.fraRisks.Controls.Add(Me.lblBranchCode)
        Me.fraRisks.Controls.Add(Me.lblBusinessType)
        Me.fraRisks.Controls.Add(Me.lblAnalysisCode)
        Me.fraRisks.Controls.Add(Me.lblProduct)
        Me.fraRisks.Controls.Add(Me.lblStatus)
        Me.fraRisks.Controls.Add(Me.llblPolicyOrProspect)
        Me.fraRisks.Controls.Add(Me.lblPolicyNumber)
        Me.fraRisks.Controls.Add(Me.lblRegarding)
        Me.fraRisks.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRisks.Location = New System.Drawing.Point(8, 4)
        Me.fraRisks.Name = "fraRisks"
        Me.fraRisks.Size = New System.Drawing.Size(589, 230)
        Me.fraRisks.TabIndex = 1
        Me.fraRisks.TabStop = False
        Me.fraRisks.Text = "Risk"
        '
        'chkHandler
        '
        Me.chkHandler.AutoSize = True
        Me.chkHandler.Checked = True
        Me.chkHandler.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHandler.Location = New System.Drawing.Point(267, 206)
        Me.chkHandler.Name = "chkHandler"
        Me.chkHandler.Size = New System.Drawing.Size(15, 14)
        Me.chkHandler.TabIndex = 163
        Me.chkHandler.UseVisualStyleBackColor = True
        '
        'txtRegarding
        '
        Me.txtRegarding.AcceptsReturn = True
        Me.txtRegarding.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegarding.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegarding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegarding.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegarding.Location = New System.Drawing.Point(105, 172)
        Me.txtRegarding.MaxLength = 0
        Me.txtRegarding.Name = "txtRegarding"
        Me.txtRegarding.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegarding.Size = New System.Drawing.Size(177, 20)
        Me.txtRegarding.TabIndex = 28
        '
        'cboAgencyContact
        '
        Me.cboAgencyContact.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgencyContact.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgencyContact.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAgencyContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgencyContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgencyContact.Location = New System.Drawing.Point(105, 149)
        Me.cboAgencyContact.Name = "cboAgencyContact"
        Me.cboAgencyContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgencyContact.Size = New System.Drawing.Size(177, 21)
        Me.cboAgencyContact.TabIndex = 161
        '
        'grpCoInsuranceLead
        '
        Me.grpCoInsuranceLead.Controls.Add(Me.optNet)
        Me.grpCoInsuranceLead.Controls.Add(Me.optGross)
        Me.grpCoInsuranceLead.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCoInsuranceLead.Location = New System.Drawing.Point(401, 149)
        Me.grpCoInsuranceLead.Name = "grpCoInsuranceLead"
        Me.grpCoInsuranceLead.Size = New System.Drawing.Size(177, 34)
        Me.grpCoInsuranceLead.TabIndex = 73
        Me.grpCoInsuranceLead.TabStop = False
        Me.grpCoInsuranceLead.Visible = False
        '
        'optNet
        '
        Me.optNet.AutoSize = True
        Me.optNet.Location = New System.Drawing.Point(122, 11)
        Me.optNet.Name = "optNet"
        Me.optNet.Size = New System.Drawing.Size(47, 17)
        Me.optNet.TabIndex = 1
        Me.optNet.TabStop = True
        Me.optNet.Text = "NET"
        Me.optNet.UseVisualStyleBackColor = True
        '
        'optGross
        '
        Me.optGross.AutoSize = True
        Me.optGross.Location = New System.Drawing.Point(11, 11)
        Me.optGross.Name = "optGross"
        Me.optGross.Size = New System.Drawing.Size(67, 17)
        Me.optGross.TabIndex = 0
        Me.optGross.TabStop = True
        Me.optGross.Text = "GROSS"
        Me.optGross.UseVisualStyleBackColor = True
        '
        'lblCoinsurancePlacement
        '
        Me.lblCoinsurancePlacement.AutoSize = True
        Me.lblCoinsurancePlacement.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoinsurancePlacement.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoinsurancePlacement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoinsurancePlacement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoinsurancePlacement.Location = New System.Drawing.Point(280, 162)
        Me.lblCoinsurancePlacement.Name = "lblCoinsurancePlacement"
        Me.lblCoinsurancePlacement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoinsurancePlacement.Size = New System.Drawing.Size(125, 13)
        Me.lblCoinsurancePlacement.TabIndex = 163
        Me.lblCoinsurancePlacement.Text = "Coinsurance Placement :"
        '
        'pnlAgentCode
        '
        Me.pnlAgentCode.Location = New System.Drawing.Point(106, 126)
        Me.pnlAgentCode.Multiline = True
        Me.pnlAgentCode.Name = "pnlAgentCode"
        Me.pnlAgentCode.ReadOnly = True
        Me.pnlAgentCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.pnlAgentCode.Size = New System.Drawing.Size(175, 20)
        Me.pnlAgentCode.TabIndex = 162
        '
        'lblAgencyContact
        '
        Me.lblAgencyContact.AutoSize = True
        Me.lblAgencyContact.Location = New System.Drawing.Point(6, 153)
        Me.lblAgencyContact.Name = "lblAgencyContact"
        Me.lblAgencyContact.Size = New System.Drawing.Size(102, 13)
        Me.lblAgencyContact.TabIndex = 160
        Me.lblAgencyContact.Text = "Agency Contact:"
        '
        'chkConsolidatedLeadCommission
        '
        Me.chkConsolidatedLeadCommission.BackColor = System.Drawing.SystemColors.Control
        Me.chkConsolidatedLeadCommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkConsolidatedLeadCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkConsolidatedLeadCommission.Enabled = False
        Me.chkConsolidatedLeadCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkConsolidatedLeadCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkConsolidatedLeadCommission.Location = New System.Drawing.Point(466, 189)
        Me.chkConsolidatedLeadCommission.Name = "chkConsolidatedLeadCommission"
        Me.chkConsolidatedLeadCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkConsolidatedLeadCommission.Size = New System.Drawing.Size(13, 17)
        Me.chkConsolidatedLeadCommission.TabIndex = 32
        Me.chkConsolidatedLeadCommission.Text = " "
        Me.chkConsolidatedLeadCommission.UseVisualStyleBackColor = False
        '
        'cmdHandler
        '
        Me.cmdHandler.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdHandler, True)
        Me.cmdHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdHandler, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdHandler, Nothing)
        Me.cmdHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHandler.Location = New System.Drawing.Point(283, 204)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdHandler, System.Drawing.Color.Silver)
        Me.cmdHandler.Name = "cmdHandler"
        Me.cmdHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHandler.Size = New System.Drawing.Size(103, 21)
        Me.commandButtonHelper1.SetStyle(Me.cmdHandler, 0)
        Me.cmdHandler.TabIndex = 25
        Me.cmdHandler.Text = "Handler..."
        Me.cmdHandler.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHandler.UseVisualStyleBackColor = False
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.Enabled = False
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(104, 58)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(177, 21)
        Me.cboProduct.Sorted = True
        Me.cboProduct.TabIndex = 11
        '
        'txtInsuredName
        '
        Me.txtInsuredName.AcceptsReturn = True
        Me.txtInsuredName.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuredName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuredName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsuredName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuredName.Location = New System.Drawing.Point(104, 14)
        Me.txtInsuredName.MaxLength = 0
        Me.txtInsuredName.Name = "txtInsuredName"
        Me.txtInsuredName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuredName.Size = New System.Drawing.Size(177, 20)
        Me.txtInsuredName.TabIndex = 3
        '
        'txtAlternateReference
        '
        Me.txtAlternateReference.AcceptsReturn = True
        Me.txtAlternateReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlternateReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlternateReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlternateReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlternateReference.Location = New System.Drawing.Point(401, 14)
        Me.txtAlternateReference.MaxLength = 0
        Me.txtAlternateReference.Name = "txtAlternateReference"
        Me.txtAlternateReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlternateReference.Size = New System.Drawing.Size(177, 20)
        Me.txtAlternateReference.TabIndex = 5
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(104, 104)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(177, 21)
        Me.cboSubBranch.TabIndex = 19
        '
        'cboBusinessType
        '
        Me.cboBusinessType.BackColor = System.Drawing.SystemColors.Window
        Me.cboBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBusinessType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBusinessType.Location = New System.Drawing.Point(401, 104)
        Me.cboBusinessType.Name = "cboBusinessType"
        Me.cboBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBusinessType.Size = New System.Drawing.Size(177, 21)
        Me.cboBusinessType.TabIndex = 21
        '
        'cboAnalysisCode
        '
        Me.cboAnalysisCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboAnalysisCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAnalysisCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAnalysisCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAnalysisCode.Location = New System.Drawing.Point(401, 81)
        Me.cboAnalysisCode.Name = "cboAnalysisCode"
        Me.cboAnalysisCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAnalysisCode.Size = New System.Drawing.Size(177, 21)
        Me.cboAnalysisCode.TabIndex = 17
        '
        'chkQuote
        '
        Me.chkQuote.BackColor = System.Drawing.SystemColors.Control
        Me.chkQuote.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkQuote.Enabled = False
        Me.chkQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkQuote.Location = New System.Drawing.Point(557, 190)
        Me.chkQuote.Name = "chkQuote"
        Me.chkQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkQuote.Size = New System.Drawing.Size(19, 18)
        Me.chkQuote.TabIndex = 30
        Me.chkQuote.Text = " "
        Me.chkQuote.UseVisualStyleBackColor = False
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(104, 36)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(177, 20)
        Me.txtPolicyNumber.TabIndex = 7
        '
        'cmdAgentCode
        '
        Me.cmdAgentCode.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAgentCode, True)
        Me.cmdAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAgentCode, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAgentCode, Nothing)
        Me.cmdAgentCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentCode.Location = New System.Drawing.Point(2, 126)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAgentCode, System.Drawing.Color.Silver)
        Me.cmdAgentCode.Name = "cmdAgentCode"
        Me.cmdAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentCode.Size = New System.Drawing.Size(103, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdAgentCode, 0)
        Me.cmdAgentCode.TabIndex = 22
        Me.cmdAgentCode.Text = "Agent Code..."
        Me.cmdAgentCode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentCode.UseVisualStyleBackColor = False
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.Enabled = False
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatus.Location = New System.Drawing.Point(401, 35)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(177, 21)
        Me.cboStatus.TabIndex = 9
        '
        'cboBranchCode
        '
        Me.cboBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranchCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranchCode.Location = New System.Drawing.Point(104, 81)
        Me.cboBranchCode.Name = "cboBranchCode"
        Me.cboBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranchCode.Size = New System.Drawing.Size(177, 21)
        Me.cboBranchCode.Sorted = True
        Me.cboBranchCode.TabIndex = 15
        '
        'cboPolicyStatus
        '
        Me.cboPolicyStatus.DefaultItemId = 0
        Me.cboPolicyStatus.FirstItem = ""
        Me.cboPolicyStatus.ItemId = 0
        Me.cboPolicyStatus.ListIndex = -1
        Me.cboPolicyStatus.Location = New System.Drawing.Point(401, 58)
        Me.cboPolicyStatus.Name = "cboPolicyStatus"
        Me.cboPolicyStatus.PMLookupProductFamily = 1
        Me.cboPolicyStatus.SingleItemId = 0
        Me.cboPolicyStatus.Size = New System.Drawing.Size(177, 21)
        Me.cboPolicyStatus.Sorted = True
        Me.cboPolicyStatus.TabIndex = 13
        Me.cboPolicyStatus.TableName = "policy_status"
        Me.cboPolicyStatus.ToolTipText = ""
        Me.cboPolicyStatus.WhereClause = ""
        '
        'pnlHandler
        '
        Me.pnlHandler.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlHandler.Controls.Add(Me.lblHandler)
        Me.pnlHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlHandler.Location = New System.Drawing.Point(401, 206)
        Me.pnlHandler.Name = "pnlHandler"
        Me.pnlHandler.Size = New System.Drawing.Size(177, 19)
        Me.pnlHandler.TabIndex = 26
        '
        'lblHandler
        '
        Me.lblHandler.Location = New System.Drawing.Point(0, 3)
        Me.lblHandler.Name = "lblHandler"
        Me.lblHandler.Size = New System.Drawing.Size(100, 23)
        Me.lblHandler.TabIndex = 0
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(401, 127)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(177, 21)
        Me.cboCurrency.TabIndex = 24
        Me.cboCurrency.Text = " "
        '
        'lblConsolidatedLeadCommission
        '
        Me.lblConsolidatedLeadCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsolidatedLeadCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsolidatedLeadCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsolidatedLeadCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsolidatedLeadCommission.Location = New System.Drawing.Point(240, 186)
        Me.lblConsolidatedLeadCommission.Name = "lblConsolidatedLeadCommission"
        Me.lblConsolidatedLeadCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsolidatedLeadCommission.Size = New System.Drawing.Size(227, 17)
        Me.lblConsolidatedLeadCommission.TabIndex = 31
        Me.lblConsolidatedLeadCommission.Text = "Consolidated Lead Agent Commission:"
        Me.lblConsolidatedLeadCommission.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(281, 131)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(70, 13)
        Me.lblCurrency.TabIndex = 159
        Me.lblCurrency.Text = "Currency:"
        '
        'lblPolicyStatus
        '
        Me.lblPolicyStatus.AutoSize = True
        Me.lblPolicyStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStatus.Location = New System.Drawing.Point(281, 62)
        Me.lblPolicyStatus.Name = "lblPolicyStatus"
        Me.lblPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStatus.Size = New System.Drawing.Size(71, 13)
        Me.lblPolicyStatus.TabIndex = 12
        Me.lblPolicyStatus.Text = "Policy Status:"
        '
        'lblInsuredName
        '
        Me.lblInsuredName.AutoSize = True
        Me.lblInsuredName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuredName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuredName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsuredName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuredName.Location = New System.Drawing.Point(6, 17)
        Me.lblInsuredName.Name = "lblInsuredName"
        Me.lblInsuredName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuredName.Size = New System.Drawing.Size(103, 13)
        Me.lblInsuredName.TabIndex = 2
        Me.lblInsuredName.Text = "Insured Name:"
        '
        'lblAlternateReference
        '
        Me.lblAlternateReference.AutoSize = True
        Me.lblAlternateReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlternateReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlternateReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlternateReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlternateReference.Location = New System.Drawing.Point(281, 17)
        Me.lblAlternateReference.Name = "lblAlternateReference"
        Me.lblAlternateReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlternateReference.Size = New System.Drawing.Size(105, 13)
        Me.lblAlternateReference.TabIndex = 4
        Me.lblAlternateReference.Text = "Alternate Reference:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(6, 108)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(65, 13)
        Me.lblSubBranch.TabIndex = 18
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblBranchCode
        '
        Me.lblBranchCode.AutoSize = True
        Me.lblBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranchCode.Location = New System.Drawing.Point(6, 85)
        Me.lblBranchCode.Name = "lblBranchCode"
        Me.lblBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranchCode.Size = New System.Drawing.Size(92, 13)
        Me.lblBranchCode.TabIndex = 14
        Me.lblBranchCode.Text = "Branch Code:"
        '
        'lblBusinessType
        '
        Me.lblBusinessType.AutoSize = True
        Me.lblBusinessType.BackColor = System.Drawing.SystemColors.Control
        Me.lblBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusinessType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBusinessType.Location = New System.Drawing.Point(281, 108)
        Me.lblBusinessType.Name = "lblBusinessType"
        Me.lblBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBusinessType.Size = New System.Drawing.Size(75, 13)
        Me.lblBusinessType.TabIndex = 20
        Me.lblBusinessType.Text = "Business type:"
        '
        'lblAnalysisCode
        '
        Me.lblAnalysisCode.AutoSize = True
        Me.lblAnalysisCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAnalysisCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAnalysisCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAnalysisCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAnalysisCode.Location = New System.Drawing.Point(281, 85)
        Me.lblAnalysisCode.Name = "lblAnalysisCode"
        Me.lblAnalysisCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAnalysisCode.Size = New System.Drawing.Size(76, 13)
        Me.lblAnalysisCode.TabIndex = 16
        Me.lblAnalysisCode.Text = "Analysis Code:"
        '
        'lblProduct
        '
        Me.lblProduct.AutoSize = True
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(6, 62)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(47, 13)
        Me.lblProduct.TabIndex = 10
        Me.lblProduct.Text = "Product:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(281, 39)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 8
        Me.lblStatus.Text = "Status:"
        '
        'llblPolicyOrProspect
        '
        Me.llblPolicyOrProspect.BackColor = System.Drawing.SystemColors.Control
        Me.llblPolicyOrProspect.Cursor = System.Windows.Forms.Cursors.Default
        Me.llblPolicyOrProspect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llblPolicyOrProspect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.llblPolicyOrProspect.Location = New System.Drawing.Point(494, 190)
        Me.llblPolicyOrProspect.Name = "llblPolicyOrProspect"
        Me.llblPolicyOrProspect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.llblPolicyOrProspect.Size = New System.Drawing.Size(67, 17)
        Me.llblPolicyOrProspect.TabIndex = 29
        Me.llblPolicyOrProspect.Text = "Quote:"
        Me.llblPolicyOrProspect.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.AutoSize = True
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(6, 39)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(58, 13)
        Me.lblPolicyNumber.TabIndex = 6
        Me.lblPolicyNumber.Text = "Policy No.:"
        '
        'lblRegarding
        '
        Me.lblRegarding.AutoSize = True
        Me.lblRegarding.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegarding.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegarding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegarding.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegarding.Location = New System.Drawing.Point(6, 175)
        Me.lblRegarding.Name = "lblRegarding"
        Me.lblRegarding.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegarding.Size = New System.Drawing.Size(59, 13)
        Me.lblRegarding.TabIndex = 27
        Me.lblRegarding.Text = "Regarding:"
        '
        'fraStatus
        '
        Me.fraStatus.Controls.Add(Me.pnlPolicyType)
        Me.fraStatus.Controls.Add(Me.lblPolicyType)
        Me.fraStatus.Controls.Add(Me.pnlScheme)
        Me.fraStatus.Controls.Add(Me.lblScheme)
        Me.fraStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStatus.Location = New System.Drawing.Point(6, 457)
        Me.fraStatus.Name = "fraStatus"
        Me.fraStatus.Size = New System.Drawing.Size(589, 41)
        Me.fraStatus.TabIndex = 62
        Me.fraStatus.TabStop = False
        '
        'pnlPolicyType
        '
        Me.pnlPolicyType.BackColor = System.Drawing.SystemColors.Control
        Me.pnlPolicyType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPolicyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlPolicyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPolicyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlPolicyType.Location = New System.Drawing.Point(96, 16)
        Me.pnlPolicyType.Name = "pnlPolicyType"
        Me.pnlPolicyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlPolicyType.Size = New System.Drawing.Size(177, 19)
        Me.pnlPolicyType.TabIndex = 64
        Me.pnlPolicyType.UseMnemonic = False
        '
        'lblPolicyType
        '
        Me.lblPolicyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyType.Location = New System.Drawing.Point(8, 16)
        Me.lblPolicyType.Name = "lblPolicyType"
        Me.lblPolicyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyType.Size = New System.Drawing.Size(81, 17)
        Me.lblPolicyType.TabIndex = 63
        Me.lblPolicyType.Text = "Policy type:"
        '
        'pnlScheme
        '
        Me.pnlScheme.BackColor = System.Drawing.SystemColors.Control
        Me.pnlScheme.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlScheme.Location = New System.Drawing.Point(344, 16)
        Me.pnlScheme.Name = "pnlScheme"
        Me.pnlScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlScheme.Size = New System.Drawing.Size(225, 19)
        Me.pnlScheme.TabIndex = 66
        Me.pnlScheme.UseMnemonic = False
        '
        'lblScheme
        '
        Me.lblScheme.BackColor = System.Drawing.SystemColors.Control
        Me.lblScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScheme.Location = New System.Drawing.Point(277, 16)
        Me.lblScheme.Name = "lblScheme"
        Me.lblScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScheme.Size = New System.Drawing.Size(81, 17)
        Me.lblScheme.TabIndex = 65
        Me.lblScheme.Text = "Scheme:"
        '
        'fraDates
        '
        Me.fraDates.Controls.Add(Me.txtAnniversaryDate)
        Me.fraDates.Controls.Add(Me.txtInceptionTPI)
        Me.fraDates.Controls.Add(Me.txtProposalDate)
        Me.fraDates.Controls.Add(Me.txtInceptionDate)
        Me.fraDates.Controls.Add(Me.txtCoverFromDate)
        Me.fraDates.Controls.Add(Me.cboPolicyLimits)
        Me.fraDates.Controls.Add(Me.cboRenewalDayNumber)
        Me.fraDates.Controls.Add(Me.chkPutOnNextInstalmentRenewal)
        Me.fraDates.Controls.Add(Me.txtQuoteExpiryDate)
        Me.fraDates.Controls.Add(Me.txtIssuedDate)
        Me.fraDates.Controls.Add(Me.txtCoverToDate)
        Me.fraDates.Controls.Add(Me.txtRenewalDate)
        Me.fraDates.Controls.Add(Me.cboPolicyDeductible)
        Me.fraDates.Controls.Add(Me.cboUnderwritingYearID)
        Me.fraDates.Controls.Add(Me.lblPolicyLimits)
        Me.fraDates.Controls.Add(Me.lblPolicyDeductible)
        Me.fraDates.Controls.Add(Me.lblPutOnNextInstalmentRenewal)
        Me.fraDates.Controls.Add(Me.lblAnniversaryDate)
        Me.fraDates.Controls.Add(Me.lblInceptionTPI)
        Me.fraDates.Controls.Add(Me.lblQuoteExpiryDate)
        Me.fraDates.Controls.Add(Me.lblProposalDate)
        Me.fraDates.Controls.Add(Me.lblIssuedDate)
        Me.fraDates.Controls.Add(Me.lblRenewalDate)
        Me.fraDates.Controls.Add(Me.lblCoverToDate)
        Me.fraDates.Controls.Add(Me.lblCoverFromDate)
        Me.fraDates.Controls.Add(Me.lblInceptionDate)
        Me.fraDates.Controls.Add(Me.lblPaymentMethod)
        Me.fraDates.Controls.Add(Me.lblUnderwritingYearID)
        Me.fraDates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDates.Location = New System.Drawing.Point(8, 288)
        Me.fraDates.Name = "fraDates"
        Me.fraDates.Size = New System.Drawing.Size(589, 172)
        Me.fraDates.TabIndex = 33
        Me.fraDates.TabStop = False
        Me.fraDates.Text = "Dates"
        '
        'txtAnniversaryDate
        '
        Me.txtAnniversaryDate.AcceptsReturn = True
        Me.txtAnniversaryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtAnniversaryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnniversaryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnniversaryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAnniversaryDate.Location = New System.Drawing.Point(119, 102)
        Me.txtAnniversaryDate.MaxLength = 0
        Me.txtAnniversaryDate.Name = "txtAnniversaryDate"
        Me.txtAnniversaryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnniversaryDate.Size = New System.Drawing.Size(161, 20)
        Me.txtAnniversaryDate.TabIndex = 52
        '
        'txtInceptionTPI
        '
        Me.txtInceptionTPI.AcceptsReturn = True
        Me.txtInceptionTPI.BackColor = System.Drawing.SystemColors.Window
        Me.txtInceptionTPI.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInceptionTPI.Enabled = False
        Me.txtInceptionTPI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInceptionTPI.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInceptionTPI.Location = New System.Drawing.Point(119, 58)
        Me.txtInceptionTPI.MaxLength = 0
        Me.txtInceptionTPI.Name = "txtInceptionTPI"
        Me.txtInceptionTPI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInceptionTPI.Size = New System.Drawing.Size(161, 20)
        Me.txtInceptionTPI.TabIndex = 44
        '
        'txtProposalDate
        '
        Me.txtProposalDate.AcceptsReturn = True
        Me.txtProposalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtProposalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProposalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProposalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProposalDate.Location = New System.Drawing.Point(119, 80)
        Me.txtProposalDate.MaxLength = 0
        Me.txtProposalDate.Name = "txtProposalDate"
        Me.txtProposalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProposalDate.Size = New System.Drawing.Size(161, 20)
        Me.txtProposalDate.TabIndex = 48
        '
        'txtInceptionDate
        '
        Me.txtInceptionDate.AcceptsReturn = True
        Me.txtInceptionDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtInceptionDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInceptionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInceptionDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInceptionDate.Location = New System.Drawing.Point(119, 36)
        Me.txtInceptionDate.MaxLength = 0
        Me.txtInceptionDate.Name = "txtInceptionDate"
        Me.txtInceptionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInceptionDate.Size = New System.Drawing.Size(161, 20)
        Me.txtInceptionDate.TabIndex = 39
        '
        'txtCoverFromDate
        '
        Me.txtCoverFromDate.AcceptsReturn = True
        Me.txtCoverFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverFromDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverFromDate.Location = New System.Drawing.Point(119, 14)
        Me.txtCoverFromDate.MaxLength = 0
        Me.txtCoverFromDate.Name = "txtCoverFromDate"
        Me.txtCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverFromDate.Size = New System.Drawing.Size(161, 20)
        Me.txtCoverFromDate.TabIndex = 35
        '
        'cboPolicyLimits
        '
        Me.cboPolicyLimits.DefaultItemId = 0
        Me.cboPolicyLimits.FirstItem = ""
        Me.cboPolicyLimits.ItemId = 0
        Me.cboPolicyLimits.ListIndex = -1
        Me.cboPolicyLimits.Location = New System.Drawing.Point(398, 124)
        Me.cboPolicyLimits.Name = "cboPolicyLimits"
        Me.cboPolicyLimits.PMLookupProductFamily = 1
        Me.cboPolicyLimits.SingleItemId = 0
        Me.cboPolicyLimits.Size = New System.Drawing.Size(177, 21)
        Me.cboPolicyLimits.Sorted = True
        Me.cboPolicyLimits.TabIndex = 59
        Me.cboPolicyLimits.TableName = "policy_limits"
        Me.cboPolicyLimits.ToolTipText = ""
        Me.cboPolicyLimits.WhereClause = ""
        '
        'cboRenewalDayNumber
        '
        Me.cboRenewalDayNumber.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalDayNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalDayNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalDayNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalDayNumber.Location = New System.Drawing.Point(528, 37)
        Me.cboRenewalDayNumber.Name = "cboRenewalDayNumber"
        Me.cboRenewalDayNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalDayNumber.Size = New System.Drawing.Size(49, 21)
        Me.cboRenewalDayNumber.TabIndex = 42
        '
        'chkPutOnNextInstalmentRenewal
        '
        Me.chkPutOnNextInstalmentRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkPutOnNextInstalmentRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPutOnNextInstalmentRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPutOnNextInstalmentRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPutOnNextInstalmentRenewal.Location = New System.Drawing.Point(254, 123)
        Me.chkPutOnNextInstalmentRenewal.Name = "chkPutOnNextInstalmentRenewal"
        Me.chkPutOnNextInstalmentRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPutOnNextInstalmentRenewal.Size = New System.Drawing.Size(17, 19)
        Me.chkPutOnNextInstalmentRenewal.TabIndex = 57
        Me.chkPutOnNextInstalmentRenewal.Text = "Check1"
        Me.chkPutOnNextInstalmentRenewal.UseVisualStyleBackColor = False
        '
        'txtQuoteExpiryDate
        '
        Me.txtQuoteExpiryDate.AcceptsReturn = True
        Me.txtQuoteExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtQuoteExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtQuoteExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuoteExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtQuoteExpiryDate.Location = New System.Drawing.Point(398, 79)
        Me.txtQuoteExpiryDate.MaxLength = 0
        Me.txtQuoteExpiryDate.Name = "txtQuoteExpiryDate"
        Me.txtQuoteExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtQuoteExpiryDate.Size = New System.Drawing.Size(177, 20)
        Me.txtQuoteExpiryDate.TabIndex = 50
        '
        'txtIssuedDate
        '
        Me.txtIssuedDate.AcceptsReturn = True
        Me.txtIssuedDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssuedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIssuedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIssuedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIssuedDate.Location = New System.Drawing.Point(398, 58)
        Me.txtIssuedDate.MaxLength = 0
        Me.txtIssuedDate.Name = "txtIssuedDate"
        Me.txtIssuedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIssuedDate.Size = New System.Drawing.Size(177, 20)
        Me.txtIssuedDate.TabIndex = 46
        '
        'txtCoverToDate
        '
        Me.txtCoverToDate.AcceptsReturn = True
        Me.txtCoverToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverToDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverToDate.Location = New System.Drawing.Point(398, 14)
        Me.txtCoverToDate.MaxLength = 0
        Me.txtCoverToDate.Name = "txtCoverToDate"
        Me.txtCoverToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverToDate.Size = New System.Drawing.Size(177, 20)
        Me.txtCoverToDate.TabIndex = 37
        '
        'txtRenewalDate
        '
        Me.txtRenewalDate.AcceptsReturn = True
        Me.txtRenewalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRenewalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRenewalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRenewalDate.Location = New System.Drawing.Point(398, 36)
        Me.txtRenewalDate.MaxLength = 0
        Me.txtRenewalDate.Name = "txtRenewalDate"
        Me.txtRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRenewalDate.Size = New System.Drawing.Size(127, 20)
        Me.txtRenewalDate.TabIndex = 41
        '
        'cboPolicyDeductible
        '
        Me.cboPolicyDeductible.DefaultItemId = 0
        Me.cboPolicyDeductible.FirstItem = ""
        Me.cboPolicyDeductible.ItemId = 0
        Me.cboPolicyDeductible.ListIndex = -1
        Me.cboPolicyDeductible.Location = New System.Drawing.Point(119, 142)
        Me.cboPolicyDeductible.Name = "cboPolicyDeductible"
        Me.cboPolicyDeductible.PMLookupProductFamily = 1
        Me.cboPolicyDeductible.SingleItemId = 0
        Me.cboPolicyDeductible.Size = New System.Drawing.Size(161, 21)
        Me.cboPolicyDeductible.Sorted = True
        Me.cboPolicyDeductible.TabIndex = 61
        Me.cboPolicyDeductible.TableName = "policy_deductibles"
        Me.cboPolicyDeductible.ToolTipText = ""
        Me.cboPolicyDeductible.WhereClause = ""
        '
        'cboUnderwritingYearID
        '
        Me.cboUnderwritingYearID.DefaultItemId = 0
        Me.cboUnderwritingYearID.FirstItem = ""
        Me.cboUnderwritingYearID.ItemId = 0
        Me.cboUnderwritingYearID.ListIndex = -1
        Me.cboUnderwritingYearID.Location = New System.Drawing.Point(398, 101)
        Me.cboUnderwritingYearID.Name = "cboUnderwritingYearID"
        Me.cboUnderwritingYearID.PMLookupProductFamily = 1
        Me.cboUnderwritingYearID.SingleItemId = 0
        Me.cboUnderwritingYearID.Size = New System.Drawing.Size(177, 21)
        Me.cboUnderwritingYearID.Sorted = True
        Me.cboUnderwritingYearID.TabIndex = 55
        Me.cboUnderwritingYearID.TableName = "Underwriting_Year"
        Me.cboUnderwritingYearID.ToolTipText = ""
        Me.cboUnderwritingYearID.Visible = False
        Me.cboUnderwritingYearID.WhereClause = ""
        '
        'lblPolicyLimits
        '
        Me.lblPolicyLimits.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyLimits.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyLimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyLimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyLimits.Location = New System.Drawing.Point(283, 126)
        Me.lblPolicyLimits.Name = "lblPolicyLimits"
        Me.lblPolicyLimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyLimits.Size = New System.Drawing.Size(113, 17)
        Me.lblPolicyLimits.TabIndex = 58
        Me.lblPolicyLimits.Text = "Policy Limits:"
        '
        'lblPolicyDeductible
        '
        Me.lblPolicyDeductible.AutoSize = True
        Me.lblPolicyDeductible.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyDeductible.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyDeductible.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyDeductible.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyDeductible.Location = New System.Drawing.Point(6, 146)
        Me.lblPolicyDeductible.Name = "lblPolicyDeductible"
        Me.lblPolicyDeductible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyDeductible.Size = New System.Drawing.Size(92, 13)
        Me.lblPolicyDeductible.TabIndex = 60
        Me.lblPolicyDeductible.Text = "Policy Deductible:"
        '
        'lblPutOnNextInstalmentRenewal
        '
        Me.lblPutOnNextInstalmentRenewal.AutoSize = True
        Me.lblPutOnNextInstalmentRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.lblPutOnNextInstalmentRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPutOnNextInstalmentRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPutOnNextInstalmentRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPutOnNextInstalmentRenewal.Location = New System.Drawing.Point(6, 126)
        Me.lblPutOnNextInstalmentRenewal.Name = "lblPutOnNextInstalmentRenewal"
        Me.lblPutOnNextInstalmentRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPutOnNextInstalmentRenewal.Size = New System.Drawing.Size(154, 13)
        Me.lblPutOnNextInstalmentRenewal.TabIndex = 56
        Me.lblPutOnNextInstalmentRenewal.Text = "Put on next instalment renewal:"
        '
        'lblAnniversaryDate
        '
        Me.lblAnniversaryDate.AutoSize = True
        Me.lblAnniversaryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAnniversaryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAnniversaryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAnniversaryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAnniversaryDate.Location = New System.Drawing.Point(6, 105)
        Me.lblAnniversaryDate.Name = "lblAnniversaryDate"
        Me.lblAnniversaryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAnniversaryDate.Size = New System.Drawing.Size(62, 13)
        Me.lblAnniversaryDate.TabIndex = 51
        Me.lblAnniversaryDate.Text = "Anniversary"
        '
        'lblInceptionTPI
        '
        Me.lblInceptionTPI.AutoSize = True
        Me.lblInceptionTPI.BackColor = System.Drawing.SystemColors.Control
        Me.lblInceptionTPI.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInceptionTPI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInceptionTPI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInceptionTPI.Location = New System.Drawing.Point(6, 61)
        Me.lblInceptionTPI.Name = "lblInceptionTPI"
        Me.lblInceptionTPI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInceptionTPI.Size = New System.Drawing.Size(74, 13)
        Me.lblInceptionTPI.TabIndex = 43
        Me.lblInceptionTPI.Text = "Inception TPI:"
        '
        'lblQuoteExpiryDate
        '
        Me.lblQuoteExpiryDate.AutoSize = True
        Me.lblQuoteExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblQuoteExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblQuoteExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuoteExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblQuoteExpiryDate.Location = New System.Drawing.Point(283, 82)
        Me.lblQuoteExpiryDate.Name = "lblQuoteExpiryDate"
        Me.lblQuoteExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblQuoteExpiryDate.Size = New System.Drawing.Size(96, 13)
        Me.lblQuoteExpiryDate.TabIndex = 49
        Me.lblQuoteExpiryDate.Text = "Quote Expiry Date:"
        '
        'lblProposalDate
        '
        Me.lblProposalDate.AutoSize = True
        Me.lblProposalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblProposalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProposalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProposalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProposalDate.Location = New System.Drawing.Point(6, 82)
        Me.lblProposalDate.Name = "lblProposalDate"
        Me.lblProposalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProposalDate.Size = New System.Drawing.Size(77, 13)
        Me.lblProposalDate.TabIndex = 47
        Me.lblProposalDate.Text = "Proposal Date:"
        '
        'lblIssuedDate
        '
        Me.lblIssuedDate.AutoSize = True
        Me.lblIssuedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblIssuedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIssuedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIssuedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIssuedDate.Location = New System.Drawing.Point(283, 60)
        Me.lblIssuedDate.Name = "lblIssuedDate"
        Me.lblIssuedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIssuedDate.Size = New System.Drawing.Size(41, 13)
        Me.lblIssuedDate.TabIndex = 45
        Me.lblIssuedDate.Text = "Issued:"
        '
        'lblRenewalDate
        '
        Me.lblRenewalDate.AutoSize = True
        Me.lblRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalDate.Location = New System.Drawing.Point(283, 39)
        Me.lblRenewalDate.Name = "lblRenewalDate"
        Me.lblRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalDate.Size = New System.Drawing.Size(52, 13)
        Me.lblRenewalDate.TabIndex = 40
        Me.lblRenewalDate.Text = "Renewal:"
        '
        'lblCoverToDate
        '
        Me.lblCoverToDate.AutoSize = True
        Me.lblCoverToDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverToDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverToDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverToDate.Location = New System.Drawing.Point(283, 17)
        Me.lblCoverToDate.Name = "lblCoverToDate"
        Me.lblCoverToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverToDate.Size = New System.Drawing.Size(50, 13)
        Me.lblCoverToDate.TabIndex = 36
        Me.lblCoverToDate.Text = "Cover to:"
        '
        'lblCoverFromDate
        '
        Me.lblCoverFromDate.AutoSize = True
        Me.lblCoverFromDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverFromDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverFromDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverFromDate.Location = New System.Drawing.Point(6, 17)
        Me.lblCoverFromDate.Name = "lblCoverFromDate"
        Me.lblCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverFromDate.Size = New System.Drawing.Size(61, 13)
        Me.lblCoverFromDate.TabIndex = 34
        Me.lblCoverFromDate.Text = "Cover from:"
        '
        'lblInceptionDate
        '
        Me.lblInceptionDate.AutoSize = True
        Me.lblInceptionDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInceptionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInceptionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInceptionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInceptionDate.Location = New System.Drawing.Point(6, 39)
        Me.lblInceptionDate.Name = "lblInceptionDate"
        Me.lblInceptionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInceptionDate.Size = New System.Drawing.Size(54, 13)
        Me.lblInceptionDate.TabIndex = 38
        Me.lblInceptionDate.Text = "Inception:"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.AutoSize = True
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(283, 105)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(89, 13)
        Me.lblPaymentMethod.TabIndex = 54
        Me.lblPaymentMethod.Text = "Payment method:"
        Me.lblPaymentMethod.Visible = False
        '
        'lblUnderwritingYearID
        '
        Me.lblUnderwritingYearID.AutoSize = True
        Me.lblUnderwritingYearID.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnderwritingYearID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnderwritingYearID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnderwritingYearID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnderwritingYearID.Location = New System.Drawing.Point(283, 105)
        Me.lblUnderwritingYearID.Name = "lblUnderwritingYearID"
        Me.lblUnderwritingYearID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnderwritingYearID.Size = New System.Drawing.Size(94, 13)
        Me.lblUnderwritingYearID.TabIndex = 53
        Me.lblUnderwritingYearID.Text = "Underwriting Year:"
        Me.lblUnderwritingYearID.Visible = False
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdNext_0, True)
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdNext_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdNext_0, Nothing)
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(559, 520)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdNext_0, System.Drawing.Color.Silver)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdNext_0, 0)
        Me._cmdNext_0.TabIndex = 72
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblManualDiscountPercentage)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraSource)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraDiscount)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraNarrations)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraExtra)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtManualDiscountPercentage)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(605, 544)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Extension"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'lblManualDiscountPercentage
        '
        Me.lblManualDiscountPercentage.AutoSize = True
        Me.lblManualDiscountPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblManualDiscountPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblManualDiscountPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManualDiscountPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblManualDiscountPercentage.Location = New System.Drawing.Point(259, 398)
        Me.lblManualDiscountPercentage.Name = "lblManualDiscountPercentage"
        Me.lblManualDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblManualDiscountPercentage.Size = New System.Drawing.Size(191, 13)
        Me.lblManualDiscountPercentage.TabIndex = 176
        Me.lblManualDiscountPercentage.Text = "Manual Discount/Loading Percentage:"
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdPrevious_1, True)
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdPrevious_1, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdPrevious_1, Nothing)
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdPrevious_1, System.Drawing.Color.Silver)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(41, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdPrevious_1, 0)
        Me._cmdPrevious_1.TabIndex = 176
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdNext_1, True)
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdNext_1, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdNext_1, Nothing)
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(560, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdNext_1, System.Drawing.Color.Silver)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdNext_1, 0)
        Me._cmdNext_1.TabIndex = 177
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'fraSource
        '
        Me.fraSource.BackColor = System.Drawing.SystemColors.Control
        Me.fraSource.Controls.Add(Me.chkReferredOnMTA)
        Me.fraSource.Controls.Add(Me.chkReferredAtRenewal)
        Me.fraSource.Controls.Add(Me.cboRenewalMethod)
        Me.fraSource.Controls.Add(Me.txtLapsedDate)
        Me.fraSource.Controls.Add(Me.cboLapsedReason)
        Me.fraSource.Controls.Add(Me.cboFrequency)
        Me.fraSource.Controls.Add(Me.txtPolicyLTUExpiryDate)
        Me.fraSource.Controls.Add(Me.cboRenewalStop)
        Me.fraSource.Controls.Add(Me.pnlRenewalCount)
        Me.fraSource.Controls.Add(Me.lblReferredOnMTA)
        Me.fraSource.Controls.Add(Me.lblReferredAtRenewal)
        Me.fraSource.Controls.Add(Me.lblRenewalMethod)
        Me.fraSource.Controls.Add(Me.lblLapsedDate)
        Me.fraSource.Controls.Add(Me.lblLapsedReason)
        Me.fraSource.Controls.Add(Me.lblFrequency)
        Me.fraSource.Controls.Add(Me.lblRenewalCount)
        Me.fraSource.Controls.Add(Me.lblPolicyLTUExpiryDate)
        Me.fraSource.Controls.Add(Me.lblRenewalStop)
        Me.fraSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSource.Location = New System.Drawing.Point(8, 4)
        Me.fraSource.Name = "fraSource"
        Me.fraSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSource.Size = New System.Drawing.Size(589, 117)
        Me.fraSource.TabIndex = 112
        Me.fraSource.TabStop = False
        Me.fraSource.Text = "Source / Renewal Information"
        '
        'chkReferredOnMTA
        '
        Me.chkReferredOnMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkReferredOnMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReferredOnMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReferredOnMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReferredOnMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReferredOnMTA.Location = New System.Drawing.Point(560, 91)
        Me.chkReferredOnMTA.Name = "chkReferredOnMTA"
        Me.chkReferredOnMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReferredOnMTA.Size = New System.Drawing.Size(13, 17)
        Me.chkReferredOnMTA.TabIndex = 120
        Me.chkReferredOnMTA.Text = " "
        Me.chkReferredOnMTA.UseVisualStyleBackColor = False
        '
        'chkReferredAtRenewal
        '
        Me.chkReferredAtRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkReferredAtRenewal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReferredAtRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReferredAtRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReferredAtRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReferredAtRenewal.Location = New System.Drawing.Point(360, 91)
        Me.chkReferredAtRenewal.Name = "chkReferredAtRenewal"
        Me.chkReferredAtRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReferredAtRenewal.Size = New System.Drawing.Size(13, 17)
        Me.chkReferredAtRenewal.TabIndex = 119
        Me.chkReferredAtRenewal.Text = " "
        Me.chkReferredAtRenewal.UseVisualStyleBackColor = False
        '
        'cboRenewalMethod
        '
        Me.cboRenewalMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalMethod.Location = New System.Drawing.Point(424, 16)
        Me.cboRenewalMethod.Name = "cboRenewalMethod"
        Me.cboRenewalMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalMethod.Size = New System.Drawing.Size(153, 21)
        Me.cboRenewalMethod.TabIndex = 114
        '
        'txtLapsedDate
        '
        Me.txtLapsedDate.AcceptsReturn = True
        Me.txtLapsedDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtLapsedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLapsedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLapsedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLapsedDate.Location = New System.Drawing.Point(424, 64)
        Me.txtLapsedDate.MaxLength = 0
        Me.txtLapsedDate.Name = "txtLapsedDate"
        Me.txtLapsedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLapsedDate.Size = New System.Drawing.Size(153, 20)
        Me.txtLapsedDate.TabIndex = 118
        '
        'cboLapsedReason
        '
        Me.cboLapsedReason.BackColor = System.Drawing.SystemColors.Window
        Me.cboLapsedReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLapsedReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLapsedReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLapsedReason.Location = New System.Drawing.Point(424, 40)
        Me.cboLapsedReason.Name = "cboLapsedReason"
        Me.cboLapsedReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLapsedReason.Size = New System.Drawing.Size(153, 21)
        Me.cboLapsedReason.TabIndex = 116
        '
        'cboFrequency
        '
        Me.cboFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.cboFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboFrequency.Location = New System.Drawing.Point(104, 16)
        Me.cboFrequency.Name = "cboFrequency"
        Me.cboFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFrequency.Size = New System.Drawing.Size(153, 21)
        Me.cboFrequency.TabIndex = 113
        '
        'txtPolicyLTUExpiryDate
        '
        Me.txtPolicyLTUExpiryDate.AcceptsReturn = True
        Me.txtPolicyLTUExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyLTUExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyLTUExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyLTUExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyLTUExpiryDate.Location = New System.Drawing.Point(104, 40)
        Me.txtPolicyLTUExpiryDate.MaxLength = 0
        Me.txtPolicyLTUExpiryDate.Name = "txtPolicyLTUExpiryDate"
        Me.txtPolicyLTUExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyLTUExpiryDate.Size = New System.Drawing.Size(153, 20)
        Me.txtPolicyLTUExpiryDate.TabIndex = 115
        '
        'cboRenewalStop
        '
        Me.cboRenewalStop.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalStop.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalStop.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalStop.Location = New System.Drawing.Point(104, 64)
        Me.cboRenewalStop.Name = "cboRenewalStop"
        Me.cboRenewalStop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalStop.Size = New System.Drawing.Size(153, 21)
        Me.cboRenewalStop.TabIndex = 117
        '
        'pnlRenewalCount
        '
        Me.pnlRenewalCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRenewalCount.Controls.Add(Me.lblRenCount)
        Me.pnlRenewalCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRenewalCount.Location = New System.Drawing.Point(104, 90)
        Me.pnlRenewalCount.Name = "pnlRenewalCount"
        Me.pnlRenewalCount.Size = New System.Drawing.Size(57, 19)
        Me.pnlRenewalCount.TabIndex = 121
        '
        'lblRenCount
        '
        Me.lblRenCount.Location = New System.Drawing.Point(0, 0)
        Me.lblRenCount.Name = "lblRenCount"
        Me.lblRenCount.Size = New System.Drawing.Size(100, 23)
        Me.lblRenCount.TabIndex = 0
        '
        'lblReferredOnMTA
        '
        Me.lblReferredOnMTA.BackColor = System.Drawing.SystemColors.Control
        Me.lblReferredOnMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReferredOnMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReferredOnMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReferredOnMTA.Location = New System.Drawing.Point(408, 93)
        Me.lblReferredOnMTA.Name = "lblReferredOnMTA"
        Me.lblReferredOnMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReferredOnMTA.Size = New System.Drawing.Size(129, 17)
        Me.lblReferredOnMTA.TabIndex = 130
        Me.lblReferredOnMTA.Text = "Referred on MTA?:"
        '
        'lblReferredAtRenewal
        '
        Me.lblReferredAtRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.lblReferredAtRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReferredAtRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReferredAtRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReferredAtRenewal.Location = New System.Drawing.Point(208, 93)
        Me.lblReferredAtRenewal.Name = "lblReferredAtRenewal"
        Me.lblReferredAtRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReferredAtRenewal.Size = New System.Drawing.Size(137, 17)
        Me.lblReferredAtRenewal.TabIndex = 129
        Me.lblReferredAtRenewal.Text = "Referred at renewal?:"
        '
        'lblRenewalMethod
        '
        Me.lblRenewalMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalMethod.Location = New System.Drawing.Point(264, 19)
        Me.lblRenewalMethod.Name = "lblRenewalMethod"
        Me.lblRenewalMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalMethod.Size = New System.Drawing.Size(105, 17)
        Me.lblRenewalMethod.TabIndex = 128
        Me.lblRenewalMethod.Text = "Renewal method:"
        '
        'lblLapsedDate
        '
        Me.lblLapsedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLapsedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLapsedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapsedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLapsedDate.Location = New System.Drawing.Point(264, 67)
        Me.lblLapsedDate.Name = "lblLapsedDate"
        Me.lblLapsedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLapsedDate.Size = New System.Drawing.Size(145, 17)
        Me.lblLapsedDate.TabIndex = 127
        Me.lblLapsedDate.Text = "Lapse/Cancellation date:"
        '
        'lblLapsedReason
        '
        Me.lblLapsedReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblLapsedReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLapsedReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapsedReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLapsedReason.Location = New System.Drawing.Point(264, 43)
        Me.lblLapsedReason.Name = "lblLapsedReason"
        Me.lblLapsedReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLapsedReason.Size = New System.Drawing.Size(161, 17)
        Me.lblLapsedReason.TabIndex = 126
        Me.lblLapsedReason.Text = "Lapse/Cancellation reason:"
        '
        'lblFrequency
        '
        Me.lblFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFrequency.Location = New System.Drawing.Point(8, 19)
        Me.lblFrequency.Name = "lblFrequency"
        Me.lblFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFrequency.Size = New System.Drawing.Size(89, 17)
        Me.lblFrequency.TabIndex = 125
        Me.lblFrequency.Text = "Frequency:"
        '
        'lblRenewalCount
        '
        Me.lblRenewalCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalCount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalCount.Location = New System.Drawing.Point(8, 93)
        Me.lblRenewalCount.Name = "lblRenewalCount"
        Me.lblRenewalCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalCount.Size = New System.Drawing.Size(97, 17)
        Me.lblRenewalCount.TabIndex = 124
        Me.lblRenewalCount.Text = "Times renewed:"
        '
        'lblPolicyLTUExpiryDate
        '
        Me.lblPolicyLTUExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyLTUExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyLTUExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyLTUExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyLTUExpiryDate.Location = New System.Drawing.Point(8, 43)
        Me.lblPolicyLTUExpiryDate.Name = "lblPolicyLTUExpiryDate"
        Me.lblPolicyLTUExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyLTUExpiryDate.Size = New System.Drawing.Size(89, 17)
        Me.lblPolicyLTUExpiryDate.TabIndex = 123
        Me.lblPolicyLTUExpiryDate.Text = "LTU expiry:"
        '
        'lblRenewalStop
        '
        Me.lblRenewalStop.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalStop.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStop.Location = New System.Drawing.Point(8, 67)
        Me.lblRenewalStop.Name = "lblRenewalStop"
        Me.lblRenewalStop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStop.Size = New System.Drawing.Size(89, 19)
        Me.lblRenewalStop.TabIndex = 122
        Me.lblRenewalStop.Text = "Stop reason:"
        '
        'fraDiscount
        '
        Me.fraDiscount.BackColor = System.Drawing.SystemColors.Control
        Me.fraDiscount.Controls.Add(Me.txtDiscountedPremium)
        Me.fraDiscount.Controls.Add(Me.txtDiscountPercentage)
        Me.fraDiscount.Controls.Add(Me.cboDiscountReason)
        Me.fraDiscount.Controls.Add(Me.cboDiscountRecurringType)
        Me.fraDiscount.Controls.Add(Me.lblDiscountReason)
        Me.fraDiscount.Controls.Add(Me.lblDiscountedPremium)
        Me.fraDiscount.Controls.Add(Me.lblDiscountPercentage)
        Me.fraDiscount.Controls.Add(Me.lblRecurring)
        Me.fraDiscount.Enabled = False
        Me.fraDiscount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDiscount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDiscount.Location = New System.Drawing.Point(8, 328)
        Me.fraDiscount.Name = "fraDiscount"
        Me.fraDiscount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDiscount.Size = New System.Drawing.Size(589, 66)
        Me.fraDiscount.TabIndex = 141
        Me.fraDiscount.TabStop = False
        Me.fraDiscount.Text = "Discount/Loading"
        '
        'txtDiscountedPremium
        '
        Me.txtDiscountedPremium.AcceptsReturn = True
        Me.txtDiscountedPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtDiscountedPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDiscountedPremium.Enabled = False
        Me.txtDiscountedPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscountedPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDiscountedPremium.Location = New System.Drawing.Point(480, 16)
        Me.txtDiscountedPremium.MaxLength = 0
        Me.txtDiscountedPremium.Name = "txtDiscountedPremium"
        Me.txtDiscountedPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDiscountedPremium.Size = New System.Drawing.Size(105, 20)
        Me.txtDiscountedPremium.TabIndex = 143
        Me.txtDiscountedPremium.Text = "0.00"
        Me.txtDiscountedPremium.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDiscountPercentage
        '
        Me.txtDiscountPercentage.AcceptsReturn = True
        Me.txtDiscountPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtDiscountPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDiscountPercentage.Enabled = False
        Me.txtDiscountPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscountPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDiscountPercentage.Location = New System.Drawing.Point(480, 40)
        Me.txtDiscountPercentage.MaxLength = 0
        Me.txtDiscountPercentage.Name = "txtDiscountPercentage"
        Me.txtDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDiscountPercentage.Size = New System.Drawing.Size(105, 20)
        Me.txtDiscountPercentage.TabIndex = 144
        Me.txtDiscountPercentage.Text = "0.00000000"
        Me.txtDiscountPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboDiscountReason
        '
        Me.cboDiscountReason.DefaultItemId = 0
        Me.cboDiscountReason.Enabled = False
        Me.cboDiscountReason.FirstItem = ""
        Me.cboDiscountReason.ItemId = 0
        Me.cboDiscountReason.ListIndex = -1
        Me.cboDiscountReason.Location = New System.Drawing.Point(167, 16)
        Me.cboDiscountReason.Name = "cboDiscountReason"
        Me.cboDiscountReason.PMLookupProductFamily = 1
        Me.cboDiscountReason.SingleItemId = 0
        Me.cboDiscountReason.Size = New System.Drawing.Size(123, 21)
        Me.cboDiscountReason.Sorted = True
        Me.cboDiscountReason.TabIndex = 142
        Me.cboDiscountReason.TableName = "Discount_Reason"
        Me.cboDiscountReason.ToolTipText = ""
        Me.cboDiscountReason.WhereClause = ""
        '
        'cboDiscountRecurringType
        '
        Me.cboDiscountRecurringType.DefaultItemId = 0
        Me.cboDiscountRecurringType.Enabled = False
        Me.cboDiscountRecurringType.FirstItem = ""
        Me.cboDiscountRecurringType.ItemId = 0
        Me.cboDiscountRecurringType.ListIndex = -1
        Me.cboDiscountRecurringType.Location = New System.Drawing.Point(167, 40)
        Me.cboDiscountRecurringType.Name = "cboDiscountRecurringType"
        Me.cboDiscountRecurringType.PMLookupProductFamily = 1
        Me.cboDiscountRecurringType.SingleItemId = 0
        Me.cboDiscountRecurringType.Size = New System.Drawing.Size(123, 21)
        Me.cboDiscountRecurringType.Sorted = True
        Me.cboDiscountRecurringType.TabIndex = 143
        Me.cboDiscountRecurringType.TableName = "Discount_Recurring_Type"
        Me.cboDiscountRecurringType.ToolTipText = ""
        Me.cboDiscountRecurringType.WhereClause = ""
        '
        'lblDiscountReason
        '
        Me.lblDiscountReason.AutoSize = True
        Me.lblDiscountReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblDiscountReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDiscountReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDiscountReason.Location = New System.Drawing.Point(3, 18)
        Me.lblDiscountReason.Name = "lblDiscountReason"
        Me.lblDiscountReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDiscountReason.Size = New System.Drawing.Size(135, 13)
        Me.lblDiscountReason.TabIndex = 148
        Me.lblDiscountReason.Text = "Discount/Loading Reason:"
        '
        'lblDiscountedPremium
        '
        Me.lblDiscountedPremium.AutoSize = True
        Me.lblDiscountedPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblDiscountedPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDiscountedPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountedPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDiscountedPremium.Location = New System.Drawing.Point(294, 20)
        Me.lblDiscountedPremium.Name = "lblDiscountedPremium"
        Me.lblDiscountedPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDiscountedPremium.Size = New System.Drawing.Size(148, 13)
        Me.lblDiscountedPremium.TabIndex = 147
        Me.lblDiscountedPremium.Text = "Discounted/Loaded Premium:"
        '
        'lblDiscountPercentage
        '
        Me.lblDiscountPercentage.AutoSize = True
        Me.lblDiscountPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblDiscountPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDiscountPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDiscountPercentage.Location = New System.Drawing.Point(294, 42)
        Me.lblDiscountPercentage.Name = "lblDiscountPercentage"
        Me.lblDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDiscountPercentage.Size = New System.Drawing.Size(153, 13)
        Me.lblDiscountPercentage.TabIndex = 146
        Me.lblDiscountPercentage.Text = "Discount/Loading Percentage:"
        '
        'lblRecurring
        '
        Me.lblRecurring.AutoSize = True
        Me.lblRecurring.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecurring.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecurring.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecurring.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecurring.Location = New System.Drawing.Point(3, 42)
        Me.lblRecurring.Name = "lblRecurring"
        Me.lblRecurring.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecurring.Size = New System.Drawing.Size(56, 13)
        Me.lblRecurring.TabIndex = 145
        Me.lblRecurring.Text = "Recurring:"
        '
        'fraNarrations
        '
        Me.fraNarrations.BackColor = System.Drawing.SystemColors.Control
        Me.fraNarrations.Controls.Add(Me.lvwPolicyWording)
        Me.fraNarrations.Controls.Add(Me.cmdDownNarrative)
        Me.fraNarrations.Controls.Add(Me.cmdUpNarrative)
        Me.fraNarrations.Controls.Add(Me.cmdAddPolicyWording)
        Me.fraNarrations.Controls.Add(Me.cmdDeletePolicyWording)
        Me.fraNarrations.Controls.Add(Me.cboPolicyStyle)
        Me.fraNarrations.Controls.Add(Me.lblPolicyStyle)
        Me.fraNarrations.Controls.Add(Me.lblMove)
        Me.fraNarrations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNarrations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNarrations.Location = New System.Drawing.Point(8, 124)
        Me.fraNarrations.Name = "fraNarrations"
        Me.fraNarrations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNarrations.Size = New System.Drawing.Size(589, 122)
        Me.fraNarrations.TabIndex = 149
        Me.fraNarrations.TabStop = False
        Me.fraNarrations.Text = "Standard Policy Wording"
        '
        'lvwPolicyWording
        '
        Me.lvwPolicyWording.AllowColumnReorder = True
        Me.lvwPolicyWording.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicyWording.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyWording_ColumnHeader_1, Me._lvwPolicyWording_ColumnHeader_2})
        Me.lvwPolicyWording.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicyWording.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicyWording.FullRowSelect = True
        Me.lvwPolicyWording.HideSelection = False
        Me.lvwPolicyWording.Location = New System.Drawing.Point(7, 16)
        Me.lvwPolicyWording.MultiSelect = False
        Me.lvwPolicyWording.Name = "lvwPolicyWording"
        Me.lvwPolicyWording.Size = New System.Drawing.Size(541, 71)
        Me.lvwPolicyWording.SmallImageList = Me.ImageList2
        Me.lvwPolicyWording.TabIndex = 150
        Me.lvwPolicyWording.UseCompatibleStateImageBehavior = False
        Me.lvwPolicyWording.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicyWording_ColumnHeader_1
        '
        Me._lvwPolicyWording_ColumnHeader_1.Tag = ""
        Me._lvwPolicyWording_ColumnHeader_1.Text = "Code"
        Me._lvwPolicyWording_ColumnHeader_1.Width = 97
        '
        '_lvwPolicyWording_ColumnHeader_2
        '
        Me._lvwPolicyWording_ColumnHeader_2.Tag = ""
        Me._lvwPolicyWording_ColumnHeader_2.Text = "Description"
        Me._lvwPolicyWording_ColumnHeader_2.Width = 385
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "history")
        Me.ImageList2.Images.SetKeyName(1, "AddressImage")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "PolicyImage")
        Me.ImageList2.Images.SetKeyName(5, " ")
        Me.ImageList2.Images.SetKeyName(6, "ContactImage")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "CampaignImage")
        Me.ImageList2.Images.SetKeyName(10, "ConvictionImage")
        Me.ImageList2.Images.SetKeyName(11, "LifestyleImage")
        Me.ImageList2.Images.SetKeyName(12, "AgentImage")
        '
        'cmdDownNarrative
        '
        Me.cmdDownNarrative.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDownNarrative, True)
        Me.cmdDownNarrative.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDownNarrative, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDownNarrative, Nothing)
        Me.cmdDownNarrative.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDownNarrative.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDownNarrative.Image = CType(resources.GetObject("cmdDownNarrative.Image"), System.Drawing.Image)
        Me.cmdDownNarrative.Location = New System.Drawing.Point(556, 64)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDownNarrative, System.Drawing.Color.Silver)
        Me.cmdDownNarrative.Name = "cmdDownNarrative"
        Me.cmdDownNarrative.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDownNarrative.Size = New System.Drawing.Size(17, 17)
        Me.commandButtonHelper1.SetStyle(Me.cmdDownNarrative, 1)
        Me.cmdDownNarrative.TabIndex = 155
        Me.cmdDownNarrative.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdDownNarrative.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDownNarrative.UseVisualStyleBackColor = False
        '
        'cmdUpNarrative
        '
        Me.cmdUpNarrative.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdUpNarrative, True)
        Me.cmdUpNarrative.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdUpNarrative, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdUpNarrative, Nothing)
        Me.cmdUpNarrative.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpNarrative.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpNarrative.Image = CType(resources.GetObject("cmdUpNarrative.Image"), System.Drawing.Image)
        Me.cmdUpNarrative.Location = New System.Drawing.Point(556, 16)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdUpNarrative, System.Drawing.Color.Silver)
        Me.cmdUpNarrative.Name = "cmdUpNarrative"
        Me.cmdUpNarrative.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpNarrative.Size = New System.Drawing.Size(17, 17)
        Me.commandButtonHelper1.SetStyle(Me.cmdUpNarrative, 1)
        Me.cmdUpNarrative.TabIndex = 154
        Me.cmdUpNarrative.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdUpNarrative.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUpNarrative.UseVisualStyleBackColor = False
        '
        'cmdAddPolicyWording
        '
        Me.cmdAddPolicyWording.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAddPolicyWording, True)
        Me.cmdAddPolicyWording.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAddPolicyWording, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAddPolicyWording, Nothing)
        Me.cmdAddPolicyWording.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddPolicyWording.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddPolicyWording.Location = New System.Drawing.Point(8, 94)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAddPolicyWording, System.Drawing.Color.Silver)
        Me.cmdAddPolicyWording.Name = "cmdAddPolicyWording"
        Me.cmdAddPolicyWording.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddPolicyWording.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdAddPolicyWording, 0)
        Me.cmdAddPolicyWording.TabIndex = 151
        Me.cmdAddPolicyWording.Text = "Add"
        Me.cmdAddPolicyWording.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddPolicyWording.UseVisualStyleBackColor = False
        '
        'cmdDeletePolicyWording
        '
        Me.cmdDeletePolicyWording.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDeletePolicyWording, True)
        Me.cmdDeletePolicyWording.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDeletePolicyWording, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDeletePolicyWording, Nothing)
        Me.cmdDeletePolicyWording.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeletePolicyWording.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeletePolicyWording.Location = New System.Drawing.Point(88, 94)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDeletePolicyWording, System.Drawing.Color.Silver)
        Me.cmdDeletePolicyWording.Name = "cmdDeletePolicyWording"
        Me.cmdDeletePolicyWording.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeletePolicyWording.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdDeletePolicyWording, 0)
        Me.cmdDeletePolicyWording.TabIndex = 152
        Me.cmdDeletePolicyWording.Text = "Delete"
        Me.cmdDeletePolicyWording.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeletePolicyWording.UseVisualStyleBackColor = False
        '
        'cboPolicyStyle
        '
        Me.cboPolicyStyle.DefaultItemId = 0
        Me.cboPolicyStyle.FirstItem = ""
        Me.cboPolicyStyle.ItemId = 0
        Me.cboPolicyStyle.ListIndex = -1
        Me.cboPolicyStyle.Location = New System.Drawing.Point(328, 92)
        Me.cboPolicyStyle.Name = "cboPolicyStyle"
        Me.cboPolicyStyle.PMLookupProductFamily = 1
        Me.cboPolicyStyle.SingleItemId = 0
        Me.cboPolicyStyle.Size = New System.Drawing.Size(213, 21)
        Me.cboPolicyStyle.Sorted = True
        Me.cboPolicyStyle.TabIndex = 153
        Me.cboPolicyStyle.TableName = "Policy_Style"
        Me.cboPolicyStyle.ToolTipText = ""
        Me.cboPolicyStyle.WhereClause = ""
        '
        'lblPolicyStyle
        '
        Me.lblPolicyStyle.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStyle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStyle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStyle.Location = New System.Drawing.Point(248, 96)
        Me.lblPolicyStyle.Name = "lblPolicyStyle"
        Me.lblPolicyStyle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStyle.Size = New System.Drawing.Size(97, 19)
        Me.lblPolicyStyle.TabIndex = 157
        Me.lblPolicyStyle.Text = "Policy Style:"
        '
        'lblMove
        '
        Me.lblMove.BackColor = System.Drawing.SystemColors.Control
        Me.lblMove.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMove.Location = New System.Drawing.Point(548, 40)
        Me.lblMove.Name = "lblMove"
        Me.lblMove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMove.Size = New System.Drawing.Size(33, 17)
        Me.lblMove.TabIndex = 156
        Me.lblMove.Text = "Move"
        '
        'fraExtra
        '
        Me.fraExtra.BackColor = System.Drawing.SystemColors.Control
        Me.fraExtra.Controls.Add(Me.txtFutureAnnualPremium)
        Me.fraExtra.Controls.Add(Me.txtPremiumExcTax)
        Me.fraExtra.Controls.Add(Me.txtOldPolicyNo)
        Me.fraExtra.Controls.Add(Me.cmdCoInsurer)
        Me.fraExtra.Controls.Add(Me.lblFutureAnnualPremium)
        Me.fraExtra.Controls.Add(Me.lblPremiumExcTax)
        Me.fraExtra.Controls.Add(Me.lblOldPolicyNo)
        Me.fraExtra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraExtra.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExtra.Location = New System.Drawing.Point(8, 250)
        Me.fraExtra.Name = "fraExtra"
        Me.fraExtra.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraExtra.Size = New System.Drawing.Size(589, 73)
        Me.fraExtra.TabIndex = 160
        Me.fraExtra.TabStop = False
        Me.fraExtra.Text = "Additional Premium Information (exc. TAX)"
        '
        'txtFutureAnnualPremium
        '
        Me.txtFutureAnnualPremium.AcceptsReturn = True
        Me.txtFutureAnnualPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtFutureAnnualPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFutureAnnualPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFutureAnnualPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFutureAnnualPremium.Location = New System.Drawing.Point(125, 18)
        Me.txtFutureAnnualPremium.MaxLength = 0
        Me.txtFutureAnnualPremium.Name = "txtFutureAnnualPremium"
        Me.txtFutureAnnualPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFutureAnnualPremium.Size = New System.Drawing.Size(161, 20)
        Me.txtFutureAnnualPremium.TabIndex = 161
        Me.txtFutureAnnualPremium.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPremiumExcTax
        '
        Me.txtPremiumExcTax.AcceptsReturn = True
        Me.txtPremiumExcTax.BackColor = System.Drawing.SystemColors.Window
        Me.txtPremiumExcTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremiumExcTax.Enabled = False
        Me.txtPremiumExcTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremiumExcTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremiumExcTax.Location = New System.Drawing.Point(125, 42)
        Me.txtPremiumExcTax.MaxLength = 0
        Me.txtPremiumExcTax.Name = "txtPremiumExcTax"
        Me.txtPremiumExcTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremiumExcTax.Size = New System.Drawing.Size(161, 20)
        Me.txtPremiumExcTax.TabIndex = 163
        Me.txtPremiumExcTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOldPolicyNo
        '
        Me.txtOldPolicyNo.AcceptsReturn = True
        Me.txtOldPolicyNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtOldPolicyNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOldPolicyNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPolicyNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOldPolicyNo.Location = New System.Drawing.Point(392, 18)
        Me.txtOldPolicyNo.MaxLength = 30
        Me.txtOldPolicyNo.Name = "txtOldPolicyNo"
        Me.txtOldPolicyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOldPolicyNo.Size = New System.Drawing.Size(185, 20)
        Me.txtOldPolicyNo.TabIndex = 162
        '
        'cmdCoInsurer
        '
        Me.cmdCoInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdCoInsurer, True)
        Me.cmdCoInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdCoInsurer, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdCoInsurer, Nothing)
        Me.cmdCoInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCoInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCoInsurer.Location = New System.Drawing.Point(392, 42)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdCoInsurer, System.Drawing.Color.Silver)
        Me.cmdCoInsurer.Name = "cmdCoInsurer"
        Me.cmdCoInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCoInsurer.Size = New System.Drawing.Size(184, 21)
        Me.commandButtonHelper1.SetStyle(Me.cmdCoInsurer, 0)
        Me.cmdCoInsurer.TabIndex = 164
        Me.cmdCoInsurer.Text = "Co-insurers..."
        Me.cmdCoInsurer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCoInsurer.UseVisualStyleBackColor = False
        '
        'lblFutureAnnualPremium
        '
        Me.lblFutureAnnualPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblFutureAnnualPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFutureAnnualPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFutureAnnualPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFutureAnnualPremium.Location = New System.Drawing.Point(8, 21)
        Me.lblFutureAnnualPremium.Name = "lblFutureAnnualPremium"
        Me.lblFutureAnnualPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFutureAnnualPremium.Size = New System.Drawing.Size(101, 17)
        Me.lblFutureAnnualPremium.TabIndex = 167
        Me.lblFutureAnnualPremium.Text = "Future premium:"
        '
        'lblPremiumExcTax
        '
        Me.lblPremiumExcTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumExcTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumExcTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumExcTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumExcTax.Location = New System.Drawing.Point(8, 45)
        Me.lblPremiumExcTax.Name = "lblPremiumExcTax"
        Me.lblPremiumExcTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumExcTax.Size = New System.Drawing.Size(125, 17)
        Me.lblPremiumExcTax.TabIndex = 166
        Me.lblPremiumExcTax.Text = "Premium:"
        '
        'lblOldPolicyNo
        '
        Me.lblOldPolicyNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblOldPolicyNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOldPolicyNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOldPolicyNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOldPolicyNo.Location = New System.Drawing.Point(302, 21)
        Me.lblOldPolicyNo.Name = "lblOldPolicyNo"
        Me.lblOldPolicyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOldPolicyNo.Size = New System.Drawing.Size(85, 17)
        Me.lblOldPolicyNo.TabIndex = 165
        Me.lblOldPolicyNo.Text = "Old Policy No.:"
        '
        'txtManualDiscountPercentage
        '
        Me.txtManualDiscountPercentage.AcceptsReturn = True
        Me.txtManualDiscountPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtManualDiscountPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtManualDiscountPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtManualDiscountPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtManualDiscountPercentage.Location = New System.Drawing.Point(488, 396)
        Me.txtManualDiscountPercentage.MaxLength = 0
        Me.txtManualDiscountPercentage.Name = "txtManualDiscountPercentage"
        Me.txtManualDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtManualDiscountPercentage.Size = New System.Drawing.Size(105, 20)
        Me.txtManualDiscountPercentage.TabIndex = 175
        Me.txtManualDiscountPercentage.Text = "0.00000000"
        Me.txtManualDiscountPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraAgents)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraCoverNote)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(605, 544)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "Agent"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraAgents
        '
        Me.fraAgents.Controls.Add(Me.chkConsolidatedSubCommission)
        Me.fraAgents.Controls.Add(Me.cmdAddAgent)
        Me.fraAgents.Controls.Add(Me.cmdDeleteAgent)
        Me.fraAgents.Controls.Add(Me.cmdEditAgent)
        Me.fraAgents.Controls.Add(Me.lvwAgents)
        Me.fraAgents.Controls.Add(Me.lblConsolidatedSubCommission)
        Me.fraAgents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgents.Location = New System.Drawing.Point(8, 4)
        Me.fraAgents.Name = "fraAgents"
        Me.fraAgents.Size = New System.Drawing.Size(589, 312)
        Me.fraAgents.TabIndex = 131
        Me.fraAgents.TabStop = False
        Me.fraAgents.Text = "Agents"
        '
        'chkConsolidatedSubCommission
        '
        Me.chkConsolidatedSubCommission.BackColor = System.Drawing.SystemColors.Control
        Me.chkConsolidatedSubCommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkConsolidatedSubCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkConsolidatedSubCommission.Enabled = False
        Me.chkConsolidatedSubCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkConsolidatedSubCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkConsolidatedSubCommission.Location = New System.Drawing.Point(556, 288)
        Me.chkConsolidatedSubCommission.Name = "chkConsolidatedSubCommission"
        Me.chkConsolidatedSubCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkConsolidatedSubCommission.Size = New System.Drawing.Size(13, 17)
        Me.chkConsolidatedSubCommission.TabIndex = 168
        Me.chkConsolidatedSubCommission.Text = " "
        Me.chkConsolidatedSubCommission.UseVisualStyleBackColor = False
        '
        'cmdAddAgent
        '
        Me.cmdAddAgent.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAddAgent, True)
        Me.cmdAddAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAddAgent, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAddAgent, Nothing)
        Me.cmdAddAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAgent.Location = New System.Drawing.Point(8, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAddAgent, System.Drawing.Color.Silver)
        Me.cmdAddAgent.Name = "cmdAddAgent"
        Me.cmdAddAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAgent.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdAddAgent, 0)
        Me.cmdAddAgent.TabIndex = 133
        Me.cmdAddAgent.Text = "Add"
        Me.cmdAddAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAgent.UseVisualStyleBackColor = False
        '
        'cmdDeleteAgent
        '
        Me.cmdDeleteAgent.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDeleteAgent, True)
        Me.cmdDeleteAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDeleteAgent, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDeleteAgent, Nothing)
        Me.cmdDeleteAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAgent.Location = New System.Drawing.Point(88, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDeleteAgent, System.Drawing.Color.Silver)
        Me.cmdDeleteAgent.Name = "cmdDeleteAgent"
        Me.cmdDeleteAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAgent.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdDeleteAgent, 0)
        Me.cmdDeleteAgent.TabIndex = 134
        Me.cmdDeleteAgent.Text = "Delete"
        Me.cmdDeleteAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAgent.UseVisualStyleBackColor = False
        '
        'cmdEditAgent
        '
        Me.cmdEditAgent.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdEditAgent, True)
        Me.cmdEditAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdEditAgent, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdEditAgent, Nothing)
        Me.cmdEditAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAgent.Location = New System.Drawing.Point(168, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdEditAgent, System.Drawing.Color.Silver)
        Me.cmdEditAgent.Name = "cmdEditAgent"
        Me.cmdEditAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAgent.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdEditAgent, 0)
        Me.cmdEditAgent.TabIndex = 135
        Me.cmdEditAgent.Text = "Edit"
        Me.cmdEditAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAgent.UseVisualStyleBackColor = False
        Me.cmdEditAgent.Visible = False
        '
        'lvwAgents
        '
        Me.lvwAgents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAgents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAgents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAgents_ColumnHeader_1, Me._lvwAgents_ColumnHeader_2, Me._lvwAgents_ColumnHeader_3, Me._lvwAgents_ColumnHeader_4})
        Me.lvwAgents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAgents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAgents.FullRowSelect = True
        Me.lvwAgents.HideSelection = False
        Me.lvwAgents.LargeImageList = Me.ImageList2
        Me.lvwAgents.Location = New System.Drawing.Point(8, 24)
        Me.lvwAgents.MultiSelect = False
        Me.lvwAgents.Name = "lvwAgents"
        Me.lvwAgents.Size = New System.Drawing.Size(561, 253)
        Me.lvwAgents.SmallImageList = Me.ImageList2
        Me.lvwAgents.TabIndex = 132
        Me.lvwAgents.UseCompatibleStateImageBehavior = False
        Me.lvwAgents.View = System.Windows.Forms.View.Details
        '
        '_lvwAgents_ColumnHeader_1
        '
        Me._lvwAgents_ColumnHeader_1.Tag = ""
        Me._lvwAgents_ColumnHeader_1.Text = "Code"
        Me._lvwAgents_ColumnHeader_1.Width = 97
        '
        '_lvwAgents_ColumnHeader_2
        '
        Me._lvwAgents_ColumnHeader_2.Tag = ""
        Me._lvwAgents_ColumnHeader_2.Text = "Name"
        Me._lvwAgents_ColumnHeader_2.Width = 188
        '
        '_lvwAgents_ColumnHeader_3
        '
        Me._lvwAgents_ColumnHeader_3.Tag = ""
        Me._lvwAgents_ColumnHeader_3.Text = "Percentage"
        Me._lvwAgents_ColumnHeader_3.Width = 97
        '
        '_lvwAgents_ColumnHeader_4
        '
        Me._lvwAgents_ColumnHeader_4.Tag = ""
        Me._lvwAgents_ColumnHeader_4.Text = "Amount"
        Me._lvwAgents_ColumnHeader_4.Width = 97
        '
        'lblConsolidatedSubCommission
        '
        Me.lblConsolidatedSubCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsolidatedSubCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsolidatedSubCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsolidatedSubCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsolidatedSubCommission.Location = New System.Drawing.Point(320, 288)
        Me.lblConsolidatedSubCommission.Name = "lblConsolidatedSubCommission"
        Me.lblConsolidatedSubCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsolidatedSubCommission.Size = New System.Drawing.Size(227, 17)
        Me.lblConsolidatedSubCommission.TabIndex = 169
        Me.lblConsolidatedSubCommission.Text = "Consolidated Sub-Agent Commission:"
        Me.lblConsolidatedSubCommission.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdPrevious_2, True)
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdPrevious_2, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdPrevious_2, Nothing)
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdPrevious_2, System.Drawing.Color.Silver)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(41, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdPrevious_2, 0)
        Me._cmdPrevious_2.TabIndex = 173
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdNext_2, True)
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdNext_2, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdNext_2, Nothing)
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(560, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdNext_2, System.Drawing.Color.Silver)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdNext_2, 0)
        Me._cmdNext_2.TabIndex = 174
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        Me._cmdNext_2.Visible = False
        '
        'fraCoverNote
        '
        Me.fraCoverNote.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoverNote.Controls.Add(Me.txtCoverNoteSheet)
        Me.fraCoverNote.Controls.Add(Me.txtCoverNoteBook)
        Me.fraCoverNote.Controls.Add(Me.lblCoverNoteSheet)
        Me.fraCoverNote.Controls.Add(Me.lblCoverNoteBook)
        Me.fraCoverNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoverNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoverNote.Location = New System.Drawing.Point(7, 323)
        Me.fraCoverNote.Name = "fraCoverNote"
        Me.fraCoverNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoverNote.Size = New System.Drawing.Size(591, 56)
        Me.fraCoverNote.TabIndex = 170
        Me.fraCoverNote.TabStop = False
        Me.fraCoverNote.Text = "Cover Note"
        '
        'txtCoverNoteSheet
        '
        Me.txtCoverNoteSheet.AcceptsReturn = True
        Me.txtCoverNoteSheet.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverNoteSheet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverNoteSheet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverNoteSheet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverNoteSheet.Location = New System.Drawing.Point(445, 22)
        Me.txtCoverNoteSheet.MaxLength = 0
        Me.txtCoverNoteSheet.Name = "txtCoverNoteSheet"
        Me.txtCoverNoteSheet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverNoteSheet.Size = New System.Drawing.Size(127, 20)
        Me.txtCoverNoteSheet.TabIndex = 172
        '
        'txtCoverNoteBook
        '
        Me.txtCoverNoteBook.AcceptsReturn = True
        Me.txtCoverNoteBook.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverNoteBook.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverNoteBook.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverNoteBook.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverNoteBook.Location = New System.Drawing.Point(119, 22)
        Me.txtCoverNoteBook.MaxLength = 50
        Me.txtCoverNoteBook.Name = "txtCoverNoteBook"
        Me.txtCoverNoteBook.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverNoteBook.Size = New System.Drawing.Size(204, 20)
        Me.txtCoverNoteBook.TabIndex = 171
        '
        'lblCoverNoteSheet
        '
        Me.lblCoverNoteSheet.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteSheet.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteSheet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteSheet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteSheet.Location = New System.Drawing.Point(333, 25)
        Me.lblCoverNoteSheet.Name = "lblCoverNoteSheet"
        Me.lblCoverNoteSheet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteSheet.Size = New System.Drawing.Size(115, 17)
        Me.lblCoverNoteSheet.TabIndex = 174
        Me.lblCoverNoteSheet.Text = "Cover Note Sheet:"
        '
        'lblCoverNoteBook
        '
        Me.lblCoverNoteBook.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteBook.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteBook.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteBook.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteBook.Location = New System.Drawing.Point(9, 25)
        Me.lblCoverNoteBook.Name = "lblCoverNoteBook"
        Me.lblCoverNoteBook.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteBook.Size = New System.Drawing.Size(120, 17)
        Me.lblCoverNoteBook.TabIndex = 173
        Me.lblCoverNoteBook.Text = "Cover Note Book:"
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraClients)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(605, 544)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "Insured Client"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdPrevious_3, True)
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdPrevious_3, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdPrevious_3, Nothing)
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdPrevious_3, System.Drawing.Color.Silver)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(41, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdPrevious_3, 0)
        Me._cmdPrevious_3.TabIndex = 139
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdNext_3, True)
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdNext_3, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdNext_3, Nothing)
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(560, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdNext_3, System.Drawing.Color.Silver)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdNext_3, 0)
        Me._cmdNext_3.TabIndex = 140
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        Me._cmdNext_3.Visible = False
        '
        'fraClients
        '
        Me.fraClients.BackColor = System.Drawing.SystemColors.Control
        Me.fraClients.Controls.Add(Me.cmdDeleteClient)
        Me.fraClients.Controls.Add(Me.cmdAddClient)
        Me.fraClients.Controls.Add(Me.cmdSetCorrespondence)
        Me.fraClients.Controls.Add(Me.lvwClients)
        Me.fraClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClients.Location = New System.Drawing.Point(8, 4)
        Me.fraClients.Name = "fraClients"
        Me.fraClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClients.Size = New System.Drawing.Size(589, 312)
        Me.fraClients.TabIndex = 136
        Me.fraClients.TabStop = False
        Me.fraClients.Text = "Insured Clients"
        '
        'cmdDeleteClient
        '
        Me.cmdDeleteClient.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDeleteClient, True)
        Me.cmdDeleteClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDeleteClient, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDeleteClient, Nothing)
        Me.cmdDeleteClient.Enabled = False
        Me.cmdDeleteClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteClient.Location = New System.Drawing.Point(88, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDeleteClient, System.Drawing.Color.Silver)
        Me.cmdDeleteClient.Name = "cmdDeleteClient"
        Me.cmdDeleteClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteClient.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdDeleteClient, 0)
        Me.cmdDeleteClient.TabIndex = 139
        Me.cmdDeleteClient.Text = "Delete"
        Me.cmdDeleteClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteClient.UseVisualStyleBackColor = False
        '
        'cmdAddClient
        '
        Me.cmdAddClient.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAddClient, True)
        Me.cmdAddClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAddClient, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAddClient, Nothing)
        Me.cmdAddClient.Enabled = False
        Me.cmdAddClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddClient.Location = New System.Drawing.Point(8, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAddClient, System.Drawing.Color.Silver)
        Me.cmdAddClient.Name = "cmdAddClient"
        Me.cmdAddClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddClient.Size = New System.Drawing.Size(73, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdAddClient, 0)
        Me.cmdAddClient.TabIndex = 138
        Me.cmdAddClient.Text = "Add"
        Me.cmdAddClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddClient.UseVisualStyleBackColor = False
        '
        'cmdSetCorrespondence
        '
        Me.cmdSetCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdSetCorrespondence, True)
        Me.cmdSetCorrespondence.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdSetCorrespondence, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdSetCorrespondence, Nothing)
        Me.cmdSetCorrespondence.Enabled = False
        Me.cmdSetCorrespondence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSetCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSetCorrespondence.Location = New System.Drawing.Point(424, 285)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdSetCorrespondence, System.Drawing.Color.Silver)
        Me.cmdSetCorrespondence.Name = "cmdSetCorrespondence"
        Me.cmdSetCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSetCorrespondence.Size = New System.Drawing.Size(141, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdSetCorrespondence, 0)
        Me.cmdSetCorrespondence.TabIndex = 137
        Me.cmdSetCorrespondence.Text = "Set Correspondence"
        Me.cmdSetCorrespondence.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSetCorrespondence.UseVisualStyleBackColor = False
        '
        'lvwClients
        '
        Me.lvwClients.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClients.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClients_ColumnHeader_1, Me._lvwClients_ColumnHeader_2, Me._lvwClients_ColumnHeader_3, Me._lvwClients_ColumnHeader_4, Me._lvwClients_ColumnHeader_5})
        Me.lvwClients.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClients.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClients.HideSelection = False
        Me.lvwClients.Location = New System.Drawing.Point(8, 24)
        Me.lvwClients.Name = "lvwClients"
        Me.lvwClients.Size = New System.Drawing.Size(561, 253)
        Me.lvwClients.TabIndex = 140
        Me.lvwClients.UseCompatibleStateImageBehavior = False
        Me.lvwClients.View = System.Windows.Forms.View.Details
        '
        '_lvwClients_ColumnHeader_1
        '
        Me._lvwClients_ColumnHeader_1.Tag = ""
        Me._lvwClients_ColumnHeader_1.Text = "Client Number"
        Me._lvwClients_ColumnHeader_1.Width = 81
        '
        '_lvwClients_ColumnHeader_2
        '
        Me._lvwClients_ColumnHeader_2.Tag = ""
        Me._lvwClients_ColumnHeader_2.Text = "Client Name"
        Me._lvwClients_ColumnHeader_2.Width = 81
        '
        '_lvwClients_ColumnHeader_3
        '
        Me._lvwClients_ColumnHeader_3.Tag = ""
        Me._lvwClients_ColumnHeader_3.Text = "Address"
        Me._lvwClients_ColumnHeader_3.Width = 121
        '
        '_lvwClients_ColumnHeader_4
        '
        Me._lvwClients_ColumnHeader_4.Tag = ""
        Me._lvwClients_ColumnHeader_4.Text = "Lead Client"
        Me._lvwClients_ColumnHeader_4.Width = 87
        '
        '_lvwClients_ColumnHeader_5
        '
        Me._lvwClients_ColumnHeader_5.Tag = ""
        Me._lvwClients_ColumnHeader_5.Text = "Correspondence"
        Me._lvwClients_ColumnHeader_5.Width = 87
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(605, 544)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "Tab 4"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdPrevious_4, True)
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdPrevious_4, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdPrevious_4, Nothing)
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(8, 422)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdPrevious_4, System.Drawing.Color.Silver)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(41, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdPrevious_4, 0)
        Me._cmdPrevious_4.TabIndex = 71
        Me._cmdPrevious_4.Text = "<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdPrevious_0, True)
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdPrevious_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdPrevious_0, Nothing)
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(-4984, 304)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdPrevious_0, System.Drawing.Color.Silver)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(41, 19)
        Me.commandButtonHelper1.SetStyle(Me._cmdPrevious_0, 0)
        Me._cmdPrevious_0.TabIndex = 67
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        'tabAgent
        '
        Me.tabAgent.Controls.Add(Me._tabAgent_TabPage0)
        Me.tabAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabAgent.ItemSize = New System.Drawing.Size(576, 18)
        Me.tabAgent.Location = New System.Drawing.Point(0, 472)
        Me.tabAgent.Multiline = True
        Me.tabAgent.Name = "tabAgent"
        Me.tabAgent.SelectedIndex = 0
        Me.tabAgent.Size = New System.Drawing.Size(581, 257)
        Me.tabAgent.TabIndex = 101
        '
        '_tabAgent_TabPage0
        '
        Me._tabAgent_TabPage0.Controls.Add(Me.SSFrame1)
        Me._tabAgent_TabPage0.Controls.Add(Me.cmdAgentMain)
        Me._tabAgent_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabAgent_TabPage0.Name = "_tabAgent_TabPage0"
        Me._tabAgent_TabPage0.Size = New System.Drawing.Size(573, 231)
        Me._tabAgent_TabPage0.TabIndex = 0
        Me._tabAgent_TabPage0.Text = "Agent"
        '
        'SSFrame1
        '
        Me.SSFrame1.Controls.Add(Me.txtAgentAmount)
        Me.SSFrame1.Controls.Add(Me.txtAgentPercentage)
        Me.SSFrame1.Controls.Add(Me.txtAgentName)
        Me.SSFrame1.Controls.Add(Me.txtAgentCode)
        Me.SSFrame1.Controls.Add(Me.lblAgentAmount)
        Me.SSFrame1.Controls.Add(Me.lblAgentPercentage)
        Me.SSFrame1.Controls.Add(Me.lblAgentName)
        Me.SSFrame1.Controls.Add(Me.lblAgentCode)
        Me.SSFrame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSFrame1.Location = New System.Drawing.Point(8, 8)
        Me.SSFrame1.Name = "SSFrame1"
        Me.SSFrame1.Size = New System.Drawing.Size(537, 185)
        Me.SSFrame1.TabIndex = 102
        Me.SSFrame1.TabStop = False
        '
        'txtAgentAmount
        '
        Me.txtAgentAmount.AcceptsReturn = True
        Me.txtAgentAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentAmount.Location = New System.Drawing.Point(90, 112)
        Me.txtAgentAmount.MaxLength = 0
        Me.txtAgentAmount.Name = "txtAgentAmount"
        Me.txtAgentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentAmount.Size = New System.Drawing.Size(177, 20)
        Me.txtAgentAmount.TabIndex = 110
        '
        'txtAgentPercentage
        '
        Me.txtAgentPercentage.AcceptsReturn = True
        Me.txtAgentPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentPercentage.Location = New System.Drawing.Point(90, 80)
        Me.txtAgentPercentage.MaxLength = 0
        Me.txtAgentPercentage.Name = "txtAgentPercentage"
        Me.txtAgentPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentPercentage.Size = New System.Drawing.Size(177, 20)
        Me.txtAgentPercentage.TabIndex = 108
        '
        'txtAgentName
        '
        Me.txtAgentName.AcceptsReturn = True
        Me.txtAgentName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentName.Enabled = False
        Me.txtAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentName.Location = New System.Drawing.Point(90, 48)
        Me.txtAgentName.MaxLength = 0
        Me.txtAgentName.Name = "txtAgentName"
        Me.txtAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentName.Size = New System.Drawing.Size(177, 20)
        Me.txtAgentName.TabIndex = 106
        '
        'txtAgentCode
        '
        Me.txtAgentCode.AcceptsReturn = True
        Me.txtAgentCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentCode.Enabled = False
        Me.txtAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentCode.Location = New System.Drawing.Point(90, 16)
        Me.txtAgentCode.MaxLength = 0
        Me.txtAgentCode.Name = "txtAgentCode"
        Me.txtAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentCode.Size = New System.Drawing.Size(177, 20)
        Me.txtAgentCode.TabIndex = 104
        '
        'lblAgentAmount
        '
        Me.lblAgentAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentAmount.Location = New System.Drawing.Point(8, 115)
        Me.lblAgentAmount.Name = "lblAgentAmount"
        Me.lblAgentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentAmount.Size = New System.Drawing.Size(80, 17)
        Me.lblAgentAmount.TabIndex = 109
        Me.lblAgentAmount.Text = "Amount:"
        '
        'lblAgentPercentage
        '
        Me.lblAgentPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentPercentage.Location = New System.Drawing.Point(8, 83)
        Me.lblAgentPercentage.Name = "lblAgentPercentage"
        Me.lblAgentPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentPercentage.Size = New System.Drawing.Size(80, 17)
        Me.lblAgentPercentage.TabIndex = 107
        Me.lblAgentPercentage.Text = "Percentage:"
        '
        'lblAgentName
        '
        Me.lblAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentName.Location = New System.Drawing.Point(8, 51)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentName.Size = New System.Drawing.Size(80, 17)
        Me.lblAgentName.TabIndex = 105
        Me.lblAgentName.Text = "Name:"
        '
        'lblAgentCode
        '
        Me.lblAgentCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentCode.Location = New System.Drawing.Point(8, 19)
        Me.lblAgentCode.Name = "lblAgentCode"
        Me.lblAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentCode.Size = New System.Drawing.Size(80, 17)
        Me.lblAgentCode.TabIndex = 103
        Me.lblAgentCode.Text = "Code:"
        '
        'cmdAgentMain
        '
        Me.cmdAgentMain.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdAgentMain, True)
        Me.cmdAgentMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdAgentMain, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdAgentMain, Nothing)
        Me.cmdAgentMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentMain.Location = New System.Drawing.Point(507, 199)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdAgentMain, System.Drawing.Color.Silver)
        Me.cmdAgentMain.Name = "cmdAgentMain"
        Me.cmdAgentMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentMain.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdAgentMain, 0)
        Me.cmdAgentMain.TabIndex = 111
        Me.cmdAgentMain.Text = "&>>"
        Me.cmdAgentMain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentMain.UseVisualStyleBackColor = False
        '
        'txtFeeIPTable
        '
        Me.txtFeeIPTable.AcceptsReturn = True
        Me.txtFeeIPTable.BackColor = System.Drawing.SystemColors.Window
        Me.txtFeeIPTable.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeIPTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeIPTable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeIPTable.Location = New System.Drawing.Point(496, 416)
        Me.txtFeeIPTable.MaxLength = 0
        Me.txtFeeIPTable.Name = "txtFeeIPTable"
        Me.txtFeeIPTable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeIPTable.Size = New System.Drawing.Size(41, 20)
        Me.txtFeeIPTable.TabIndex = 78
        Me.txtFeeIPTable.Visible = False
        '
        'txtFeeCommissionAmount
        '
        Me.txtFeeCommissionAmount.AcceptsReturn = True
        Me.txtFeeCommissionAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtFeeCommissionAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeCommissionAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeCommissionAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeCommissionAmount.Location = New System.Drawing.Point(288, 440)
        Me.txtFeeCommissionAmount.MaxLength = 0
        Me.txtFeeCommissionAmount.Name = "txtFeeCommissionAmount"
        Me.txtFeeCommissionAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeCommissionAmount.Size = New System.Drawing.Size(193, 20)
        Me.txtFeeCommissionAmount.TabIndex = 80
        Me.txtFeeCommissionAmount.Visible = False
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(32, 440)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(241, 20)
        Me.txtAmount.TabIndex = 79
        Me.txtAmount.Visible = False
        '
        'tabCommissionTab
        '
        Me.tabCommissionTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabCommissionTab.Controls.Add(Me._tabCommissionTab_TabPage0)
        Me.tabCommissionTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabCommissionTab.ItemSize = New System.Drawing.Size(576, 18)
        Me.tabCommissionTab.Location = New System.Drawing.Point(0, 472)
        Me.tabCommissionTab.Multiline = True
        Me.tabCommissionTab.Name = "tabCommissionTab"
        Me.tabCommissionTab.SelectedIndex = 0
        Me.tabCommissionTab.Size = New System.Drawing.Size(581, 257)
        Me.tabCommissionTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabCommissionTab.TabIndex = 86
        '
        '_tabCommissionTab_TabPage0
        '
        Me._tabCommissionTab_TabPage0.Controls.Add(Me.fraCommission)
        Me._tabCommissionTab_TabPage0.Controls.Add(Me.cmdMain)
        Me._tabCommissionTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabCommissionTab_TabPage0.Name = "_tabCommissionTab_TabPage0"
        Me._tabCommissionTab_TabPage0.Size = New System.Drawing.Size(573, 231)
        Me._tabCommissionTab_TabPage0.TabIndex = 0
        Me._tabCommissionTab_TabPage0.Text = "&1 - Commission Rate"
        '
        'fraCommission
        '
        Me.fraCommission.Controls.Add(Me.chkOverrideRateTable)
        Me.fraCommission.Controls.Add(Me.txtCommissionPercentage)
        Me.fraCommission.Controls.Add(Me.txtCommissionPayable)
        Me.fraCommission.Controls.Add(Me.txtCommissionCharge)
        Me.fraCommission.Controls.Add(Me.pnlPremiumAmount)
        Me.fraCommission.Controls.Add(Me.pnlCommissionAccount)
        Me.fraCommission.Controls.Add(Me.lblCommissionPercentage)
        Me.fraCommission.Controls.Add(Me.lblCommissionCharge)
        Me.fraCommission.Controls.Add(Me.lblCommissionPremium)
        Me.fraCommission.Controls.Add(Me.lblCommissionAccount)
        Me.fraCommission.Controls.Add(Me.lblCommissionPayable)
        Me.fraCommission.Controls.Add(Me.lblOverride)
        Me.fraCommission.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCommission.Location = New System.Drawing.Point(8, 8)
        Me.fraCommission.Name = "fraCommission"
        Me.fraCommission.Size = New System.Drawing.Size(537, 185)
        Me.fraCommission.TabIndex = 87
        Me.fraCommission.TabStop = False
        '
        'chkOverrideRateTable
        '
        Me.chkOverrideRateTable.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideRateTable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideRateTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideRateTable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideRateTable.Location = New System.Drawing.Point(504, 144)
        Me.chkOverrideRateTable.Name = "chkOverrideRateTable"
        Me.chkOverrideRateTable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideRateTable.Size = New System.Drawing.Size(17, 17)
        Me.chkOverrideRateTable.TabIndex = 97
        Me.chkOverrideRateTable.Text = "Check1"
        Me.chkOverrideRateTable.UseVisualStyleBackColor = False
        '
        'txtCommissionPercentage
        '
        Me.txtCommissionPercentage.AcceptsReturn = True
        Me.txtCommissionPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionPercentage.Location = New System.Drawing.Point(164, 80)
        Me.txtCommissionPercentage.MaxLength = 0
        Me.txtCommissionPercentage.Name = "txtCommissionPercentage"
        Me.txtCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionPercentage.Size = New System.Drawing.Size(153, 20)
        Me.txtCommissionPercentage.TabIndex = 92
        Me.txtCommissionPercentage.Text = " "
        Me.txtCommissionPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCommissionPayable
        '
        Me.txtCommissionPayable.AcceptsReturn = True
        Me.txtCommissionPayable.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionPayable.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionPayable.Enabled = False
        Me.txtCommissionPayable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionPayable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionPayable.Location = New System.Drawing.Point(164, 144)
        Me.txtCommissionPayable.MaxLength = 0
        Me.txtCommissionPayable.Name = "txtCommissionPayable"
        Me.txtCommissionPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionPayable.Size = New System.Drawing.Size(153, 20)
        Me.txtCommissionPayable.TabIndex = 96
        Me.txtCommissionPayable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCommissionCharge
        '
        Me.txtCommissionCharge.AcceptsReturn = True
        Me.txtCommissionCharge.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionCharge.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionCharge.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionCharge.Location = New System.Drawing.Point(164, 112)
        Me.txtCommissionCharge.MaxLength = 0
        Me.txtCommissionCharge.Name = "txtCommissionCharge"
        Me.txtCommissionCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionCharge.Size = New System.Drawing.Size(153, 20)
        Me.txtCommissionCharge.TabIndex = 94
        Me.txtCommissionCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'pnlPremiumAmount
        '
        Me.pnlPremiumAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPremiumAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPremiumAmount.Location = New System.Drawing.Point(164, 48)
        Me.pnlPremiumAmount.Name = "pnlPremiumAmount"
        Me.pnlPremiumAmount.Size = New System.Drawing.Size(153, 17)
        Me.pnlPremiumAmount.TabIndex = 90
        '
        'pnlCommissionAccount
        '
        Me.pnlCommissionAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCommissionAccount.Controls.Add(Me.lblCommAccount)
        Me.pnlCommissionAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCommissionAccount.Location = New System.Drawing.Point(164, 16)
        Me.pnlCommissionAccount.Name = "pnlCommissionAccount"
        Me.pnlCommissionAccount.Size = New System.Drawing.Size(225, 17)
        Me.pnlCommissionAccount.TabIndex = 88
        '
        'lblCommAccount
        '
        Me.lblCommAccount.Location = New System.Drawing.Point(0, 0)
        Me.lblCommAccount.Name = "lblCommAccount"
        Me.lblCommAccount.Size = New System.Drawing.Size(100, 23)
        Me.lblCommAccount.TabIndex = 0
        '
        'lblCommissionPercentage
        '
        Me.lblCommissionPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionPercentage.Location = New System.Drawing.Point(8, 83)
        Me.lblCommissionPercentage.Name = "lblCommissionPercentage"
        Me.lblCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionPercentage.Size = New System.Drawing.Size(153, 17)
        Me.lblCommissionPercentage.TabIndex = 93
        Me.lblCommissionPercentage.Text = "Commission percentage:"
        '
        'lblCommissionCharge
        '
        Me.lblCommissionCharge.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionCharge.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionCharge.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionCharge.Location = New System.Drawing.Point(8, 115)
        Me.lblCommissionCharge.Name = "lblCommissionCharge"
        Me.lblCommissionCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionCharge.Size = New System.Drawing.Size(137, 17)
        Me.lblCommissionCharge.TabIndex = 95
        Me.lblCommissionCharge.Text = "Commission charge:"
        '
        'lblCommissionPremium
        '
        Me.lblCommissionPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionPremium.Location = New System.Drawing.Point(8, 51)
        Me.lblCommissionPremium.Name = "lblCommissionPremium"
        Me.lblCommissionPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionPremium.Size = New System.Drawing.Size(105, 25)
        Me.lblCommissionPremium.TabIndex = 91
        Me.lblCommissionPremium.Text = "Premium amount:"
        '
        'lblCommissionAccount
        '
        Me.lblCommissionAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionAccount.Location = New System.Drawing.Point(8, 19)
        Me.lblCommissionAccount.Name = "lblCommissionAccount"
        Me.lblCommissionAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionAccount.Size = New System.Drawing.Size(137, 17)
        Me.lblCommissionAccount.TabIndex = 89
        Me.lblCommissionAccount.Text = "Commission account:"
        '
        'lblCommissionPayable
        '
        Me.lblCommissionPayable.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionPayable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionPayable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionPayable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionPayable.Location = New System.Drawing.Point(8, 147)
        Me.lblCommissionPayable.Name = "lblCommissionPayable"
        Me.lblCommissionPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionPayable.Size = New System.Drawing.Size(137, 17)
        Me.lblCommissionPayable.TabIndex = 98
        Me.lblCommissionPayable.Text = "Commission payable:"
        '
        'lblOverride
        '
        Me.lblOverride.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverride.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverride.Location = New System.Drawing.Point(344, 147)
        Me.lblOverride.Name = "lblOverride"
        Me.lblOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverride.Size = New System.Drawing.Size(129, 17)
        Me.lblOverride.TabIndex = 99
        Me.lblOverride.Text = "Override rate table ?"
        '
        'cmdMain
        '
        Me.cmdMain.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdMain, True)
        Me.cmdMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdMain, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdMain, Nothing)
        Me.cmdMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMain.Location = New System.Drawing.Point(512, 196)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdMain, System.Drawing.Color.Silver)
        Me.cmdMain.Name = "cmdMain"
        Me.cmdMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMain.Size = New System.Drawing.Size(38, 19)
        Me.commandButtonHelper1.SetStyle(Me.cmdMain, 0)
        Me.cmdMain.TabIndex = 100
        Me.cmdMain.Text = "&>>"
        Me.cmdMain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMain.UseVisualStyleBackColor = False
        '
        'fraStorage
        '
        Me.fraStorage.BackColor = System.Drawing.SystemColors.Control
        Me.fraStorage.Controls.Add(Me.cboRelationship)
        Me.fraStorage.Controls.Add(Me.cmdRelatedPolicy)
        Me.fraStorage.Controls.Add(Me.pnlRelatedPolicy)
        Me.fraStorage.Controls.Add(Me.lblRelationship)
        Me.fraStorage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStorage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStorage.Location = New System.Drawing.Point(543, 408)
        Me.fraStorage.Name = "fraStorage"
        Me.fraStorage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStorage.Size = New System.Drawing.Size(145, 177)
        Me.fraStorage.TabIndex = 81
        Me.fraStorage.TabStop = False
        Me.fraStorage.Text = "Storage"
        Me.fraStorage.Visible = False
        '
        'cboRelationship
        '
        Me.cboRelationship.BackColor = System.Drawing.SystemColors.Window
        Me.cboRelationship.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRelationship.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRelationship.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRelationship.Location = New System.Drawing.Point(8, 128)
        Me.cboRelationship.Name = "cboRelationship"
        Me.cboRelationship.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRelationship.Size = New System.Drawing.Size(169, 21)
        Me.cboRelationship.TabIndex = 85
        '
        'cmdRelatedPolicy
        '
        Me.cmdRelatedPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdRelatedPolicy, True)
        Me.cmdRelatedPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdRelatedPolicy, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdRelatedPolicy, Nothing)
        Me.cmdRelatedPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRelatedPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelatedPolicy.Location = New System.Drawing.Point(8, 40)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdRelatedPolicy, System.Drawing.Color.Silver)
        Me.cmdRelatedPolicy.Name = "cmdRelatedPolicy"
        Me.cmdRelatedPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelatedPolicy.Size = New System.Drawing.Size(105, 24)
        Me.commandButtonHelper1.SetStyle(Me.cmdRelatedPolicy, 0)
        Me.cmdRelatedPolicy.TabIndex = 82
        Me.cmdRelatedPolicy.Text = "Related policy..."
        Me.cmdRelatedPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelatedPolicy.UseVisualStyleBackColor = False
        '
        'pnlRelatedPolicy
        '
        Me.pnlRelatedPolicy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRelatedPolicy.Controls.Add(Me.lblRelatedPolicy)
        Me.pnlRelatedPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRelatedPolicy.Location = New System.Drawing.Point(8, 64)
        Me.pnlRelatedPolicy.Name = "pnlRelatedPolicy"
        Me.pnlRelatedPolicy.Size = New System.Drawing.Size(169, 19)
        Me.pnlRelatedPolicy.TabIndex = 83
        '
        'lblRelatedPolicy
        '
        Me.lblRelatedPolicy.Location = New System.Drawing.Point(0, 0)
        Me.lblRelatedPolicy.Name = "lblRelatedPolicy"
        Me.lblRelatedPolicy.Size = New System.Drawing.Size(100, 23)
        Me.lblRelatedPolicy.TabIndex = 0
        '
        'lblRelationship
        '
        Me.lblRelationship.BackColor = System.Drawing.SystemColors.Control
        Me.lblRelationship.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRelationship.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRelationship.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRelationship.Location = New System.Drawing.Point(16, 104)
        Me.lblRelationship.Name = "lblRelationship"
        Me.lblRelationship.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRelationship.Size = New System.Drawing.Size(105, 17)
        Me.lblRelationship.TabIndex = 84
        Me.lblRelationship.Text = "Relationship:"
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(40, 408)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(241, 20)
        Me.txtPercentage.TabIndex = 76
        Me.txtPercentage.Visible = False
        '
        'txtFeeCommissionPercentage
        '
        Me.txtFeeCommissionPercentage.AcceptsReturn = True
        Me.txtFeeCommissionPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtFeeCommissionPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeCommissionPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeCommissionPercentage.Location = New System.Drawing.Point(288, 408)
        Me.txtFeeCommissionPercentage.MaxLength = 0
        Me.txtFeeCommissionPercentage.Name = "txtFeeCommissionPercentage"
        Me.txtFeeCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeCommissionPercentage.Size = New System.Drawing.Size(193, 20)
        Me.txtFeeCommissionPercentage.TabIndex = 77
        Me.txtFeeCommissionPercentage.Visible = False
        '
        'uctPMUPolicyControl
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.tabAgent)
        Me.Controls.Add(Me.txtFeeIPTable)
        Me.Controls.Add(Me.txtFeeCommissionAmount)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.tabCommissionTab)
        Me.Controls.Add(Me.fraStorage)
        Me.Controls.Add(Me.txtPercentage)
        Me.Controls.Add(Me.txtFeeCommissionPercentage)
        Me.Name = "uctPMUPolicyControl"
        Me.Size = New System.Drawing.Size(642, 742)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.gbPreferredCorrespondence.ResumeLayout(False)
        Me.gbPreferredCorrespondence.PerformLayout()
        Me.fraRisks.ResumeLayout(False)
        Me.fraRisks.PerformLayout()
        Me.grpCoInsuranceLead.ResumeLayout(False)
        Me.grpCoInsuranceLead.PerformLayout()
        Me.pnlHandler.ResumeLayout(False)
        Me.fraStatus.ResumeLayout(False)
        Me.fraDates.ResumeLayout(False)
        Me.fraDates.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me.fraSource.ResumeLayout(False)
        Me.fraSource.PerformLayout()
        Me.pnlRenewalCount.ResumeLayout(False)
        Me.fraDiscount.ResumeLayout(False)
        Me.fraDiscount.PerformLayout()
        Me.fraNarrations.ResumeLayout(False)
        Me.fraExtra.ResumeLayout(False)
        Me.fraExtra.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraAgents.ResumeLayout(False)
        Me.fraCoverNote.ResumeLayout(False)
        Me.fraCoverNote.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraClients.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.tabAgent.ResumeLayout(False)
        Me._tabAgent_TabPage0.ResumeLayout(False)
        Me.SSFrame1.ResumeLayout(False)
        Me.SSFrame1.PerformLayout()
        Me.tabCommissionTab.ResumeLayout(False)
        Me._tabCommissionTab_TabPage0.ResumeLayout(False)
        Me.fraCommission.ResumeLayout(False)
        Me.fraCommission.PerformLayout()
        Me.pnlCommissionAccount.ResumeLayout(False)
        Me.fraStorage.ResumeLayout(False)
        Me.pnlRelatedPolicy.ResumeLayout(False)
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
        Me.cmdPrevious(2) = _cmdPrevious_2
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Sub lvwClients_InitializeColumnKeys()
        Me._lvwClients_ColumnHeader_1.Name = ""
        Me._lvwClients_ColumnHeader_2.Name = ""
        Me._lvwClients_ColumnHeader_3.Name = ""
        Me._lvwClients_ColumnHeader_4.Name = ""
        Me._lvwClients_ColumnHeader_5.Name = ""
    End Sub
    Sub lvwAgents_InitializeColumnKeys()
        Me._lvwAgents_ColumnHeader_1.Name = ""
        Me._lvwAgents_ColumnHeader_2.Name = ""
        Me._lvwAgents_ColumnHeader_3.Name = ""
        Me._lvwAgents_ColumnHeader_4.Name = ""
    End Sub
    Sub lvwPolicyWording_InitializeColumnKeys()
        Me._lvwPolicyWording_ColumnHeader_1.Name = ""
        Me._lvwPolicyWording_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents cboAgencyContact As System.Windows.Forms.ComboBox
    Friend WithEvents lblAgencyContact As System.Windows.Forms.Label
    Friend WithEvents pnlAgentCode As System.Windows.Forms.TextBox
    Friend WithEvents chkHandler As System.Windows.Forms.CheckBox
    Friend WithEvents grpCoInsuranceLead As System.Windows.Forms.GroupBox
    Friend WithEvents optNet As System.Windows.Forms.RadioButton
    Friend WithEvents optGross As System.Windows.Forms.RadioButton
    Friend WithEvents lblCoinsurancePlacement As System.Windows.Forms.Label
    Friend WithEvents gbPreferredCorrespondence As System.Windows.Forms.GroupBox
    Friend WithEvents lblCorrespondenceType As System.Windows.Forms.Label
    Friend WithEvents cboCorrespondenceMethod As PMLookupControl.cboPMLookup
    Friend WithEvents txtCorrespondenceType As System.Windows.Forms.TextBox
#End Region
#Region "Upgrade Support"
    <System.Runtime.InteropServices.ProgId("BusinessTypeChangeEventArgs_NET.BusinessTypeChangeEventArgs")> _
    Public NotInheritable Class BusinessTypeChangeEventArgs
        Inherits System.EventArgs
        Public BusinessType As Object
        Public Sub New(ByRef BusinessType As Object)
            MyBase.New()
            Me.BusinessType = BusinessType
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
    Public NotInheritable Class MouseUpEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
    Public NotInheritable Class MouseMoveEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
    Public NotInheritable Class MouseDownEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
    Public NotInheritable Class KeyUpEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
    Public NotInheritable Class KeyPressEventArgs
        Inherits System.EventArgs
        Public KeyAscii As Integer
        Public Sub New(ByRef KeyAscii As Integer)
            MyBase.New()
            Me.KeyAscii = KeyAscii
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
    Public NotInheritable Class KeyDownEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
#End Region
End Class