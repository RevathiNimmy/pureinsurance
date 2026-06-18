<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyGCControl
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializecmdPrevious()
		InitializecmdNext()
		lvwCampaigns_InitializeColumnKeys()
		lvwPolicies_InitializeColumnKeys()
		lvwLoyaltySchemes_InitializeColumnKeys()
		lvwConvictions_InitializeColumnKeys()
		lvwContacts_InitializeColumnKeys()
		lvwAddresses_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents imgIcon As System.Windows.Forms.PictureBox
	Friend WithEvents txtConsultantRef As System.Windows.Forms.TextBox
	Friend WithEvents cmdConsultantLookup As System.Windows.Forms.Button
	Friend WithEvents pnlConsultantName As System.Windows.Forms.Panel
	Friend WithEvents lblConsultantName As System.Windows.Forms.Label
	Friend WithEvents fraConsultant As System.Windows.Forms.GroupBox
	Friend WithEvents chkCharity As System.Windows.Forms.CheckBox
	Friend WithEvents txtCharityNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtMembers As System.Windows.Forms.TextBox
	Friend WithEvents lblMembers As System.Windows.Forms.Label
	Friend WithEvents lblCharityNumber As System.Windows.Forms.Label
	Friend WithEvents lblCharity As System.Windows.Forms.Label
	Friend WithEvents fraCharityDetails As System.Windows.Forms.GroupBox
	Friend WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Friend WithEvents txtTradingName As System.Windows.Forms.TextBox
	Friend WithEvents txtMainContact As System.Windows.Forms.TextBox
	Friend WithEvents cboGroupType As System.Windows.Forms.ComboBox
	Friend WithEvents txtName As System.Windows.Forms.TextBox
	Friend WithEvents txtIDReference As System.Windows.Forms.TextBox
	Friend WithEvents lblTradingName As System.Windows.Forms.Label
	Friend WithEvents lblMainContact As System.Windows.Forms.Label
	Friend WithEvents lblGroupType As System.Windows.Forms.Label
	Friend WithEvents lblName As System.Windows.Forms.Label
	Friend WithEvents lblIDReference As System.Windows.Forms.Label
	Friend WithEvents fraClient As System.Windows.Forms.GroupBox
	Friend WithEvents cmdAgentLookUp As System.Windows.Forms.Button
	Friend WithEvents txtAgentRef As System.Windows.Forms.TextBox
	Friend WithEvents pnlAgentName As System.Windows.Forms.Panel
	Friend WithEvents lblAgentName As System.Windows.Forms.Label
	Friend WithEvents fraAgent As System.Windows.Forms.GroupBox
	Friend WithEvents chkFeeClient As System.Windows.Forms.CheckBox
	Friend WithEvents chkAgent As System.Windows.Forms.CheckBox
	Friend WithEvents chkProspect As System.Windows.Forms.CheckBox
	Friend WithEvents pnlClientBalance As System.Windows.Forms.Panel
	Friend WithEvents pnlLastYearTurnover As System.Windows.Forms.Panel
	Friend WithEvents pnlYearToDateTurnover As System.Windows.Forms.Panel
	Friend WithEvents lblClientBalance As System.Windows.Forms.Label
	Friend WithEvents lblLastYearTurnover As System.Windows.Forms.Label
	Friend WithEvents lblYearToDateTurnover As System.Windows.Forms.Label
	Friend WithEvents fraClientAccounts As System.Windows.Forms.GroupBox
	Friend WithEvents cboServiceLevel As System.Windows.Forms.ComboBox
	Friend WithEvents txtAlternativeIdentifier As System.Windows.Forms.TextBox
	Friend WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Friend WithEvents cboBranch As System.Windows.Forms.ComboBox
	Friend WithEvents lblServicelevel As System.Windows.Forms.Label
	Friend WithEvents lblAlternativeIdentifier As System.Windows.Forms.Label
	Friend WithEvents lblSubBranch As System.Windows.Forms.Label
	Friend WithEvents lblBranch As System.Windows.Forms.Label
	Friend WithEvents Frame1 As System.Windows.Forms.GroupBox
	Friend WithEvents cboBlackListReason As System.Windows.Forms.ComboBox
	Friend WithEvents lblBlacklistReason As System.Windows.Forms.Label
	Friend WithEvents fraBlackList As System.Windows.Forms.GroupBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdNext_1 As System.Windows.Forms.Button
	Friend WithEvents cmdEditAd As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteAd As System.Windows.Forms.Button
	Friend WithEvents cmdAddAd As System.Windows.Forms.Button
	Friend WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwAddresses As System.Windows.Forms.ListView
	Friend WithEvents fraAddress As System.Windows.Forms.GroupBox
	Friend WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
	Friend WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents chkeMPS As System.Windows.Forms.CheckBox
	Friend WithEvents chkTPS As System.Windows.Forms.CheckBox
	Friend WithEvents chkMailshot As System.Windows.Forms.CheckBox
	Friend WithEvents cboCorrespondenceType As System.Windows.Forms.ComboBox
	Friend WithEvents lblTPS As System.Windows.Forms.Label
	Friend WithEvents lblPreferredCorrespondence As System.Windows.Forms.Label
	Friend WithEvents fraCorrespondence As System.Windows.Forms.GroupBox
	Friend WithEvents _cmdNext_2 As System.Windows.Forms.Button
	Friend WithEvents cmdEditCon As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteCon As System.Windows.Forms.Button
	Friend WithEvents cmdAddCon As System.Windows.Forms.Button
	Friend WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwContacts As System.Windows.Forms.ListView
	Friend WithEvents fraContact As System.Windows.Forms.GroupBox
	Friend WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Friend WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Friend WithEvents txtHiddenDate As System.Windows.Forms.TextBox
	Friend WithEvents txtHiddenCurrency As System.Windows.Forms.TextBox
	Friend WithEvents _cmdNext_3 As System.Windows.Forms.Button
	Friend WithEvents txtCCJ As System.Windows.Forms.TextBox
	Friend WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
	Friend WithEvents cmdAddConv As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteConv As System.Windows.Forms.Button
	Friend WithEvents cmdEditConv As System.Windows.Forms.Button
	Friend WithEvents _lvwConvictions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwConvictions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwConvictions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwConvictions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwConvictions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwConvictions As System.Windows.Forms.ListView
	Friend WithEvents frmContacts As System.Windows.Forms.GroupBox
	Friend WithEvents lblCCJ As System.Windows.Forms.Label
	Friend WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Friend WithEvents cmdAssociates As System.Windows.Forms.Button
	Friend WithEvents _cmdNext_4 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Friend WithEvents txtTobLetter As System.Windows.Forms.TextBox
	Friend WithEvents cboArea As System.Windows.Forms.ComboBox
	Friend WithEvents txtFileCode As System.Windows.Forms.TextBox
	Friend WithEvents lblTobLetter As System.Windows.Forms.Label
	Friend WithEvents lblArea As System.Windows.Forms.Label
	Friend WithEvents lblFileCode As System.Windows.Forms.Label
	Friend WithEvents fraAreaCode As System.Windows.Forms.GroupBox
	Friend WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdNext_5 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
	Friend WithEvents cmdAddLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents cmdEditLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwLoyaltySchemes As System.Windows.Forms.ListView
	Friend WithEvents fraLoyaltySchemes As System.Windows.Forms.GroupBox
	Friend WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
	Friend WithEvents cmdAddPol As System.Windows.Forms.Button
	Friend WithEvents cmdDeletePol As System.Windows.Forms.Button
	Friend WithEvents cmdEditPol As System.Windows.Forms.Button
	Friend WithEvents _lvwPolicies_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwPolicies_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwPolicies_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwPolicies_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwPolicies_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwPolicies As System.Windows.Forms.ListView
	Friend WithEvents fraPolicies As System.Windows.Forms.GroupBox
	Friend WithEvents _lvwCampaigns_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCampaigns_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwCampaigns_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwCampaigns As System.Windows.Forms.ListView
	Friend WithEvents fraCampaign As System.Windows.Forms.GroupBox
	Friend WithEvents cboStrengthCode As System.Windows.Forms.ComboBox
	Friend WithEvents cboProspectingStatus As System.Windows.Forms.ComboBox
	Friend WithEvents txtAgentReference As System.Windows.Forms.TextBox
	Friend WithEvents cmdCurrentAgent As System.Windows.Forms.Button
	Friend WithEvents pnlCurrentAgent As System.Windows.Forms.Panel
	Friend WithEvents lblStrengthCode As System.Windows.Forms.Label
	Friend WithEvents lblProspectStatus As System.Windows.Forms.Label
	Friend WithEvents lblAgentReference As System.Windows.Forms.Label
	Friend WithEvents fraProspect As System.Windows.Forms.GroupBox
	Friend WithEvents txtInsurerRef As System.Windows.Forms.TextBox
	Friend WithEvents cmdInsurerLookup As System.Windows.Forms.Button
	Friend WithEvents pnlInsurerName As System.Windows.Forms.Panel
	Friend WithEvents lblInsurerName As System.Windows.Forms.Label
	Friend WithEvents fraPreviousInsurer As System.Windows.Forms.GroupBox
	Friend WithEvents cmdBrokerLookup As System.Windows.Forms.Button
	Friend WithEvents txtBrokerRef As System.Windows.Forms.TextBox
	Friend WithEvents pnlBrokerName As System.Windows.Forms.Panel
	Friend WithEvents lblBrokerName As System.Windows.Forms.Label
	Friend WithEvents fraPreviousBroker As System.Windows.Forms.GroupBox
	Friend WithEvents cboPolicyType As System.Windows.Forms.ComboBox
	Friend WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
	Friend WithEvents _cmdNext_6 As System.Windows.Forms.Button
	Friend WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdNext_7 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
	Friend WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
	Friend WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdPrevious_7 As System.Windows.Forms.Button
	Friend WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
	Friend WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
	Friend cmdNext(7) As System.Windows.Forms.Button
	Friend cmdPrevious(7) As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'developer guide no. 26 latest guide
    Friend WithEvents lblConsultantNamelabel As System.Windows.Forms.Label
    Friend WithEvents lblAgentNameLabel As System.Windows.Forms.Label
    Friend WithEvents lblClientBalanceLabel As System.Windows.Forms.Label
    Friend WithEvents lblLastYearTurnoverLabel As System.Windows.Forms.Label
    Friend WithEvents lblYearToDateTurnoverLabel As System.Windows.Forms.Label
    'developer guide no. 26(latest guide)
    'start
    Friend WithEvents lblCurrentAgentLabel As System.Windows.Forms.Label
    Friend WithEvents lblInsurerNameLabel As System.Windows.Forms.Label
    Friend WithEvents lblBrokerNameLabel As System.Windows.Forms.Label
    'ends
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartyGCControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.fraConsultant = New System.Windows.Forms.GroupBox
        Me.txtConsultantRef = New System.Windows.Forms.TextBox
        Me.cmdConsultantLookup = New System.Windows.Forms.Button
        Me.pnlConsultantName = New System.Windows.Forms.Panel
        Me.lblConsultantNamelabel = New System.Windows.Forms.Label
        Me.lblConsultantName = New System.Windows.Forms.Label
        Me.fraCharityDetails = New System.Windows.Forms.GroupBox
        Me.chkCharity = New System.Windows.Forms.CheckBox
        Me.txtCharityNumber = New System.Windows.Forms.TextBox
        Me.txtMembers = New System.Windows.Forms.TextBox
        Me.lblMembers = New System.Windows.Forms.Label
        Me.lblCharityNumber = New System.Windows.Forms.Label
        Me.lblCharity = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.fraClient = New System.Windows.Forms.GroupBox
        Me.txtTradingName = New System.Windows.Forms.TextBox
        Me.txtMainContact = New System.Windows.Forms.TextBox
        Me.cboGroupType = New System.Windows.Forms.ComboBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.lblTradingName = New System.Windows.Forms.Label
        Me.lblMainContact = New System.Windows.Forms.Label
        Me.lblGroupType = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblIDReference = New System.Windows.Forms.Label
        Me.fraAgent = New System.Windows.Forms.GroupBox
        Me.cmdAgentLookUp = New System.Windows.Forms.Button
        Me.txtAgentRef = New System.Windows.Forms.TextBox
        Me.pnlAgentName = New System.Windows.Forms.Panel
        Me.lblAgentNameLabel = New System.Windows.Forms.Label
        Me.lblAgentName = New System.Windows.Forms.Label
        Me.fraClientAccounts = New System.Windows.Forms.GroupBox
        Me.chkFeeClient = New System.Windows.Forms.CheckBox
        Me.chkAgent = New System.Windows.Forms.CheckBox
        Me.chkProspect = New System.Windows.Forms.CheckBox
        Me.pnlClientBalance = New System.Windows.Forms.Panel
        Me.lblClientBalanceLabel = New System.Windows.Forms.Label
        Me.pnlLastYearTurnover = New System.Windows.Forms.Panel
        Me.lblLastYearTurnoverLabel = New System.Windows.Forms.Label
        Me.pnlYearToDateTurnover = New System.Windows.Forms.Panel
        Me.lblYearToDateTurnoverLabel = New System.Windows.Forms.Label
        Me.lblClientBalance = New System.Windows.Forms.Label
        Me.lblLastYearTurnover = New System.Windows.Forms.Label
        Me.lblYearToDateTurnover = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboServiceLevel = New System.Windows.Forms.ComboBox
        Me.txtAlternativeIdentifier = New System.Windows.Forms.TextBox
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.lblServicelevel = New System.Windows.Forms.Label
        Me.lblAlternativeIdentifier = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.fraBlackList = New System.Windows.Forms.GroupBox
        Me.cboBlackListReason = New System.Windows.Forms.ComboBox
        Me.lblBlacklistReason = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.lvwAddresses = New System.Windows.Forms.ListView
        Me._lvwAddresses_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraCorrespondence = New System.Windows.Forms.GroupBox
        Me.chkeMPS = New System.Windows.Forms.CheckBox
        Me.chkTPS = New System.Windows.Forms.CheckBox
        Me.chkMailshot = New System.Windows.Forms.CheckBox
        Me.cboCorrespondenceType = New System.Windows.Forms.ComboBox
        Me.lblTPS = New System.Windows.Forms.Label
        Me.lblPreferredCorrespondence = New System.Windows.Forms.Label
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me.txtHiddenDate = New System.Windows.Forms.TextBox
        Me.txtHiddenCurrency = New System.Windows.Forms.TextBox
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me.txtCCJ = New System.Windows.Forms.TextBox
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me.frmContacts = New System.Windows.Forms.GroupBox
        Me.cmdAddConv = New System.Windows.Forms.Button
        Me.cmdDeleteConv = New System.Windows.Forms.Button
        Me.cmdEditConv = New System.Windows.Forms.Button
        Me.lvwConvictions = New System.Windows.Forms.ListView
        Me._lvwConvictions_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwConvictions_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.lblCCJ = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.cmdAssociates = New System.Windows.Forms.Button
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me.fraTab5_1 = New System.Windows.Forms.GroupBox
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox
        Me.cboTurnover = New PMLookupControl.cboPMLookup
        Me.cboRenewalStopCode = New System.Windows.Forms.ComboBox
        Me.txtLoyaltyNumber = New System.Windows.Forms.TextBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.txtLoyaltyNumberPrefix = New System.Windows.Forms.TextBox
        Me.cboSeasonalGift = New System.Windows.Forms.ComboBox
        Me.cboCreditCard = New System.Windows.Forms.ComboBox
        Me.cboReminderType = New System.Windows.Forms.ComboBox
        Me.ddPaymentMethod = New PMListMgrDropdown.uctDropdown
        Me.lblTurnover = New System.Windows.Forms.Label
        Me.lblCreditCard = New System.Windows.Forms.Label
        Me.lblTermsOfPayment = New System.Windows.Forms.Label
        Me.lblSeasonalGift = New System.Windows.Forms.Label
        Me.lblRenewalStopCode = New System.Windows.Forms.Label
        Me.lblLoyaltyNumber = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblPaymentMethod = New System.Windows.Forms.Label
        Me.lblReminderType = New System.Windows.Forms.Label
        Me.fraAreaCode = New System.Windows.Forms.GroupBox
        Me.txtTobLetter = New System.Windows.Forms.TextBox
        Me.cboArea = New System.Windows.Forms.ComboBox
        Me.txtFileCode = New System.Windows.Forms.TextBox
        Me.lblTobLetter = New System.Windows.Forms.Label
        Me.lblArea = New System.Windows.Forms.Label
        Me.lblFileCode = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me._cmdNext_5 = New System.Windows.Forms.Button
        Me._cmdPrevious_4 = New System.Windows.Forms.Button
        Me.fraLoyaltySchemes = New System.Windows.Forms.GroupBox
        Me.cmdAddLoyaltyScheme = New System.Windows.Forms.Button
        Me.cmdDeleteLoyaltyScheme = New System.Windows.Forms.Button
        Me.cmdEditLoyaltyScheme = New System.Windows.Forms.Button
        Me.lvwLoyaltySchemes = New System.Windows.Forms.ListView
        Me._lvwLoyaltySchemes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwLoyaltySchemes_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwLoyaltySchemes_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwLoyaltySchemes_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me.fraPolicies = New System.Windows.Forms.GroupBox
        Me.cmdAddPol = New System.Windows.Forms.Button
        Me.cmdDeletePol = New System.Windows.Forms.Button
        Me.cmdEditPol = New System.Windows.Forms.Button
        Me.lvwPolicies = New System.Windows.Forms.ListView
        Me._lvwPolicies_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicies_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicies_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicies_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicies_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.fraCampaign = New System.Windows.Forms.GroupBox
        Me.lvwCampaigns = New System.Windows.Forms.ListView
        Me._lvwCampaigns_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCampaigns_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCampaigns_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.fraProspect = New System.Windows.Forms.GroupBox
        Me.cboStrengthCode = New System.Windows.Forms.ComboBox
        Me.cboProspectingStatus = New System.Windows.Forms.ComboBox
        Me.txtAgentReference = New System.Windows.Forms.TextBox
        Me.cmdCurrentAgent = New System.Windows.Forms.Button
        Me.pnlCurrentAgent = New System.Windows.Forms.Panel
        Me.lblCurrentAgentLabel = New System.Windows.Forms.Label
        Me.lblStrengthCode = New System.Windows.Forms.Label
        Me.lblProspectStatus = New System.Windows.Forms.Label
        Me.lblAgentReference = New System.Windows.Forms.Label
        Me.fraPreviousInsurer = New System.Windows.Forms.GroupBox
        Me.txtInsurerRef = New System.Windows.Forms.TextBox
        Me.cmdInsurerLookup = New System.Windows.Forms.Button
        Me.pnlInsurerName = New System.Windows.Forms.Panel
        Me.lblInsurerNameLabel = New System.Windows.Forms.Label
        Me.lblInsurerName = New System.Windows.Forms.Label
        Me.fraPreviousBroker = New System.Windows.Forms.GroupBox
        Me.cmdBrokerLookup = New System.Windows.Forms.Button
        Me.txtBrokerRef = New System.Windows.Forms.TextBox
        Me.pnlBrokerName = New System.Windows.Forms.Panel
        Me.lblBrokerNameLabel = New System.Windows.Forms.Label
        Me.lblBrokerName = New System.Windows.Forms.Label
        Me.cboPolicyType = New System.Windows.Forms.ComboBox
        Me._cmdPrevious_5 = New System.Windows.Forms.Button
        Me._cmdNext_6 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_6 = New System.Windows.Forms.Button
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl
        Me._cmdNext_7 = New System.Windows.Forms.Button
        Me._cmdPrevious_7 = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraConsultant.SuspendLayout()
        Me.pnlConsultantName.SuspendLayout()
        Me.fraCharityDetails.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.fraAgent.SuspendLayout()
        Me.pnlAgentName.SuspendLayout()
        Me.fraClientAccounts.SuspendLayout()
        Me.pnlClientBalance.SuspendLayout()
        Me.pnlLastYearTurnover.SuspendLayout()
        Me.pnlYearToDateTurnover.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraBlackList.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraCorrespondence.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.frmContacts.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraTab5_1.SuspendLayout()
        Me.fraAreaCode.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraLoyaltySchemes.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.fraPolicies.SuspendLayout()
        Me.fraCampaign.SuspendLayout()
        Me.fraProspect.SuspendLayout()
        Me.pnlCurrentAgent.SuspendLayout()
        Me.fraPreviousInsurer.SuspendLayout()
        Me.pnlInsurerName.SuspendLayout()
        Me.fraPreviousBroker.SuspendLayout()
        Me.pnlBrokerName.SuspendLayout()
        Me._tabMainTab_TabPage7.SuspendLayout()
        Me._tabMainTab_TabPage8.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage5)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage6)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage7)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage8)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(77, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(746, 467)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraConsultant)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraCharityDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClientAccounts)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraBlackList)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Identity"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(665, 106)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'fraConsultant
        '
        Me.fraConsultant.Controls.Add(Me.txtConsultantRef)
        Me.fraConsultant.Controls.Add(Me.cmdConsultantLookup)
        Me.fraConsultant.Controls.Add(Me.pnlConsultantName)
        Me.fraConsultant.Controls.Add(Me.lblConsultantName)
        Me.fraConsultant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraConsultant.Location = New System.Drawing.Point(330, 244)
        Me.fraConsultant.Name = "fraConsultant"
        Me.fraConsultant.Size = New System.Drawing.Size(329, 86)
        Me.fraConsultant.TabIndex = 6
        Me.fraConsultant.TabStop = False
        Me.fraConsultant.Text = "Account Executive"
        '
        'txtConsultantRef
        '
        Me.txtConsultantRef.AcceptsReturn = True
        Me.txtConsultantRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtConsultantRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConsultantRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsultantRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConsultantRef.Location = New System.Drawing.Point(80, 24)
        Me.txtConsultantRef.MaxLength = 0
        Me.txtConsultantRef.Name = "txtConsultantRef"
        Me.txtConsultantRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConsultantRef.Size = New System.Drawing.Size(241, 20)
        Me.txtConsultantRef.TabIndex = 1
        '
        'cmdConsultantLookup
        '
        Me.cmdConsultantLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConsultantLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConsultantLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConsultantLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConsultantLookup.Location = New System.Drawing.Point(8, 24)
        Me.cmdConsultantLookup.Name = "cmdConsultantLookup"
        Me.cmdConsultantLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConsultantLookup.Size = New System.Drawing.Size(60, 19)
        Me.cmdConsultantLookup.TabIndex = 0
        Me.cmdConsultantLookup.Text = "Code..."
        Me.cmdConsultantLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConsultantLookup.UseVisualStyleBackColor = False
        '
        'pnlConsultantName
        '
        Me.pnlConsultantName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConsultantName.Controls.Add(Me.lblConsultantNamelabel)
        Me.pnlConsultantName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConsultantName.Location = New System.Drawing.Point(80, 48)
        Me.pnlConsultantName.Name = "pnlConsultantName"
        Me.pnlConsultantName.Size = New System.Drawing.Size(241, 19)
        Me.pnlConsultantName.TabIndex = 78
        '
        'lblConsultantNamelabel
        '
        Me.lblConsultantNamelabel.AutoSize = True
        Me.lblConsultantNamelabel.Location = New System.Drawing.Point(0, 2)
        Me.lblConsultantNamelabel.Name = "lblConsultantNamelabel"
        Me.lblConsultantNamelabel.Size = New System.Drawing.Size(0, 13)
        Me.lblConsultantNamelabel.TabIndex = 0
        '
        'lblConsultantName
        '
        Me.lblConsultantName.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsultantName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsultantName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsultantName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsultantName.Location = New System.Drawing.Point(8, 48)
        Me.lblConsultantName.Name = "lblConsultantName"
        Me.lblConsultantName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsultantName.Size = New System.Drawing.Size(49, 13)
        Me.lblConsultantName.TabIndex = 2
        Me.lblConsultantName.Text = "Name:"
        '
        'fraCharityDetails
        '
        Me.fraCharityDetails.Controls.Add(Me.chkCharity)
        Me.fraCharityDetails.Controls.Add(Me.txtCharityNumber)
        Me.fraCharityDetails.Controls.Add(Me.txtMembers)
        Me.fraCharityDetails.Controls.Add(Me.lblMembers)
        Me.fraCharityDetails.Controls.Add(Me.lblCharityNumber)
        Me.fraCharityDetails.Controls.Add(Me.lblCharity)
        Me.fraCharityDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCharityDetails.Location = New System.Drawing.Point(330, 142)
        Me.fraCharityDetails.Name = "fraCharityDetails"
        Me.fraCharityDetails.Size = New System.Drawing.Size(329, 101)
        Me.fraCharityDetails.TabIndex = 5
        Me.fraCharityDetails.TabStop = False
        Me.fraCharityDetails.Text = "Charity Details"
        '
        'chkCharity
        '
        Me.chkCharity.BackColor = System.Drawing.SystemColors.Control
        Me.chkCharity.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCharity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCharity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCharity.Location = New System.Drawing.Point(144, 24)
        Me.chkCharity.Name = "chkCharity"
        Me.chkCharity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCharity.Size = New System.Drawing.Size(17, 13)
        Me.chkCharity.TabIndex = 1
        Me.chkCharity.UseVisualStyleBackColor = False
        '
        'txtCharityNumber
        '
        Me.txtCharityNumber.AcceptsReturn = True
        Me.txtCharityNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCharityNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCharityNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCharityNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCharityNumber.Location = New System.Drawing.Point(144, 72)
        Me.txtCharityNumber.MaxLength = 0
        Me.txtCharityNumber.Name = "txtCharityNumber"
        Me.txtCharityNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCharityNumber.Size = New System.Drawing.Size(177, 20)
        Me.txtCharityNumber.TabIndex = 5
        '
        'txtMembers
        '
        Me.txtMembers.AcceptsReturn = True
        Me.txtMembers.BackColor = System.Drawing.SystemColors.Window
        Me.txtMembers.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMembers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMembers.Location = New System.Drawing.Point(144, 44)
        Me.txtMembers.MaxLength = 0
        Me.txtMembers.Multiline = True
        Me.txtMembers.Name = "txtMembers"
        Me.txtMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMembers.Size = New System.Drawing.Size(177, 19)
        Me.txtMembers.TabIndex = 3
        '
        'lblMembers
        '
        Me.lblMembers.BackColor = System.Drawing.SystemColors.Control
        Me.lblMembers.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMembers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMembers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMembers.Location = New System.Drawing.Point(8, 48)
        Me.lblMembers.Name = "lblMembers"
        Me.lblMembers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMembers.Size = New System.Drawing.Size(105, 17)
        Me.lblMembers.TabIndex = 2
        Me.lblMembers.Text = "No. of members:"
        '
        'lblCharityNumber
        '
        Me.lblCharityNumber.AutoSize = True
        Me.lblCharityNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCharityNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCharityNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCharityNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCharityNumber.Location = New System.Drawing.Point(8, 72)
        Me.lblCharityNumber.Name = "lblCharityNumber"
        Me.lblCharityNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCharityNumber.Size = New System.Drawing.Size(59, 13)
        Me.lblCharityNumber.TabIndex = 4
        Me.lblCharityNumber.Text = "Charity No:"
        '
        'lblCharity
        '
        Me.lblCharity.AutoSize = True
        Me.lblCharity.BackColor = System.Drawing.SystemColors.Control
        Me.lblCharity.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCharity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCharity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCharity.Location = New System.Drawing.Point(8, 24)
        Me.lblCharity.Name = "lblCharity"
        Me.lblCharity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCharity.Size = New System.Drawing.Size(42, 13)
        Me.lblCharity.TabIndex = 0
        Me.lblCharity.Text = "Charity:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 7
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.txtTradingName)
        Me.fraClient.Controls.Add(Me.txtMainContact)
        Me.fraClient.Controls.Add(Me.cboGroupType)
        Me.fraClient.Controls.Add(Me.txtName)
        Me.fraClient.Controls.Add(Me.txtIDReference)
        Me.fraClient.Controls.Add(Me.lblTradingName)
        Me.fraClient.Controls.Add(Me.lblMainContact)
        Me.fraClient.Controls.Add(Me.lblGroupType)
        Me.fraClient.Controls.Add(Me.lblName)
        Me.fraClient.Controls.Add(Me.lblIDReference)
        Me.fraClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(8, 8)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(303, 131)
        Me.fraClient.TabIndex = 0
        Me.fraClient.TabStop = False
        '
        'txtTradingName
        '
        Me.txtTradingName.AcceptsReturn = True
        Me.txtTradingName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTradingName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTradingName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTradingName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTradingName.Location = New System.Drawing.Point(112, 104)
        Me.txtTradingName.MaxLength = 0
        Me.txtTradingName.Name = "txtTradingName"
        Me.txtTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTradingName.Size = New System.Drawing.Size(185, 20)
        Me.txtTradingName.TabIndex = 9
        '
        'txtMainContact
        '
        Me.txtMainContact.AcceptsReturn = True
        Me.txtMainContact.BackColor = System.Drawing.SystemColors.Window
        Me.txtMainContact.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMainContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMainContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMainContact.Location = New System.Drawing.Point(112, 85)
        Me.txtMainContact.MaxLength = 0
        Me.txtMainContact.Name = "txtMainContact"
        Me.txtMainContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMainContact.Size = New System.Drawing.Size(185, 20)
        Me.txtMainContact.TabIndex = 7
        '
        'cboGroupType
        '
        Me.cboGroupType.BackColor = System.Drawing.SystemColors.Window
        Me.cboGroupType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGroupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGroupType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGroupType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGroupType.Location = New System.Drawing.Point(112, 58)
        Me.cboGroupType.Name = "cboGroupType"
        Me.cboGroupType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGroupType.Size = New System.Drawing.Size(185, 21)
        Me.cboGroupType.TabIndex = 5
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(112, 34)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(185, 20)
        Me.txtName.TabIndex = 3
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(112, 13)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(185, 20)
        Me.txtIDReference.TabIndex = 1
        '
        'lblTradingName
        '
        Me.lblTradingName.AutoSize = True
        Me.lblTradingName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTradingName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTradingName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTradingName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTradingName.Location = New System.Drawing.Point(8, 107)
        Me.lblTradingName.Name = "lblTradingName"
        Me.lblTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTradingName.Size = New System.Drawing.Size(75, 13)
        Me.lblTradingName.TabIndex = 8
        Me.lblTradingName.Text = "Trading name:"
        '
        'lblMainContact
        '
        Me.lblMainContact.BackColor = System.Drawing.SystemColors.Control
        Me.lblMainContact.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMainContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMainContact.Location = New System.Drawing.Point(8, 88)
        Me.lblMainContact.Name = "lblMainContact"
        Me.lblMainContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMainContact.Size = New System.Drawing.Size(85, 17)
        Me.lblMainContact.TabIndex = 6
        Me.lblMainContact.Text = "Main contact:"
        '
        'lblGroupType
        '
        Me.lblGroupType.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroupType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroupType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGroupType.Location = New System.Drawing.Point(8, 60)
        Me.lblGroupType.Name = "lblGroupType"
        Me.lblGroupType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGroupType.Size = New System.Drawing.Size(85, 17)
        Me.lblGroupType.TabIndex = 4
        Me.lblGroupType.Text = "Group type:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(8, 37)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(68, 13)
        Me.lblName.TabIndex = 2
        Me.lblName.Text = "Group name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(8, 16)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(66, 13)
        Me.lblIDReference.TabIndex = 0
        Me.lblIDReference.Text = "Group code:"
        '
        'fraAgent
        '
        Me.fraAgent.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgent.Controls.Add(Me.cmdAgentLookUp)
        Me.fraAgent.Controls.Add(Me.txtAgentRef)
        Me.fraAgent.Controls.Add(Me.pnlAgentName)
        Me.fraAgent.Controls.Add(Me.lblAgentName)
        Me.fraAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgent.Location = New System.Drawing.Point(8, 268)
        Me.fraAgent.Name = "fraAgent"
        Me.fraAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgent.Size = New System.Drawing.Size(303, 65)
        Me.fraAgent.TabIndex = 2
        Me.fraAgent.TabStop = False
        Me.fraAgent.Text = "Lead Agent"
        '
        'cmdAgentLookUp
        '
        Me.cmdAgentLookUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentLookUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentLookUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentLookUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentLookUp.Location = New System.Drawing.Point(8, 16)
        Me.cmdAgentLookUp.Name = "cmdAgentLookUp"
        Me.cmdAgentLookUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentLookUp.Size = New System.Drawing.Size(60, 19)
        Me.cmdAgentLookUp.TabIndex = 0
        Me.cmdAgentLookUp.Text = "Code..."
        Me.cmdAgentLookUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentLookUp.UseVisualStyleBackColor = False
        '
        'txtAgentRef
        '
        Me.txtAgentRef.AcceptsReturn = True
        Me.txtAgentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentRef.Location = New System.Drawing.Point(112, 16)
        Me.txtAgentRef.MaxLength = 0
        Me.txtAgentRef.Name = "txtAgentRef"
        Me.txtAgentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentRef.Size = New System.Drawing.Size(185, 20)
        Me.txtAgentRef.TabIndex = 1
        '
        'pnlAgentName
        '
        Me.pnlAgentName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAgentName.Controls.Add(Me.lblAgentNameLabel)
        Me.pnlAgentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAgentName.Location = New System.Drawing.Point(112, 41)
        Me.pnlAgentName.Name = "pnlAgentName"
        Me.pnlAgentName.Size = New System.Drawing.Size(185, 19)
        Me.pnlAgentName.TabIndex = 67
        '
        'lblAgentNameLabel
        '
        Me.lblAgentNameLabel.AutoSize = True
        Me.lblAgentNameLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblAgentNameLabel.Name = "lblAgentNameLabel"
        Me.lblAgentNameLabel.Size = New System.Drawing.Size(0, 13)
        Me.lblAgentNameLabel.TabIndex = 0
        '
        'lblAgentName
        '
        Me.lblAgentName.AutoSize = True
        Me.lblAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentName.Location = New System.Drawing.Point(8, 43)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentName.Size = New System.Drawing.Size(38, 13)
        Me.lblAgentName.TabIndex = 2
        Me.lblAgentName.Text = "Name:"
        '
        'fraClientAccounts
        '
        Me.fraClientAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.fraClientAccounts.Controls.Add(Me.chkFeeClient)
        Me.fraClientAccounts.Controls.Add(Me.chkAgent)
        Me.fraClientAccounts.Controls.Add(Me.chkProspect)
        Me.fraClientAccounts.Controls.Add(Me.pnlClientBalance)
        Me.fraClientAccounts.Controls.Add(Me.pnlLastYearTurnover)
        Me.fraClientAccounts.Controls.Add(Me.pnlYearToDateTurnover)
        Me.fraClientAccounts.Controls.Add(Me.lblClientBalance)
        Me.fraClientAccounts.Controls.Add(Me.lblLastYearTurnover)
        Me.fraClientAccounts.Controls.Add(Me.lblYearToDateTurnover)
        Me.fraClientAccounts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClientAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClientAccounts.Location = New System.Drawing.Point(330, 7)
        Me.fraClientAccounts.Name = "fraClientAccounts"
        Me.fraClientAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClientAccounts.Size = New System.Drawing.Size(329, 131)
        Me.fraClientAccounts.TabIndex = 4
        Me.fraClientAccounts.TabStop = False
        '
        'chkFeeClient
        '
        Me.chkFeeClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkFeeClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFeeClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFeeClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFeeClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFeeClient.Location = New System.Drawing.Point(232, 88)
        Me.chkFeeClient.Name = "chkFeeClient"
        Me.chkFeeClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFeeClient.Size = New System.Drawing.Size(89, 17)
        Me.chkFeeClient.TabIndex = 4
        Me.chkFeeClient.Text = "Fee Client:"
        Me.chkFeeClient.UseVisualStyleBackColor = False
        '
        'chkAgent
        '
        Me.chkAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgent.Location = New System.Drawing.Point(8, 112)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgent.Size = New System.Drawing.Size(149, 17)
        Me.chkAgent.TabIndex = 5
        Me.chkAgent.Text = "Is Agent ?"
        Me.chkAgent.UseVisualStyleBackColor = False
        '
        'chkProspect
        '
        Me.chkProspect.BackColor = System.Drawing.SystemColors.Control
        Me.chkProspect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProspect.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProspect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProspect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProspect.Location = New System.Drawing.Point(8, 88)
        Me.chkProspect.Name = "chkProspect"
        Me.chkProspect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProspect.Size = New System.Drawing.Size(149, 17)
        Me.chkProspect.TabIndex = 3
        Me.chkProspect.Text = "Is Prospect"
        Me.chkProspect.UseVisualStyleBackColor = False
        '
        'pnlClientBalance
        '
        Me.pnlClientBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlClientBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClientBalance.Controls.Add(Me.lblClientBalanceLabel)
        Me.pnlClientBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClientBalance.Location = New System.Drawing.Point(144, 16)
        Me.pnlClientBalance.Name = "pnlClientBalance"
        Me.pnlClientBalance.Size = New System.Drawing.Size(177, 19)
        Me.pnlClientBalance.TabIndex = 103
        '
        'lblClientBalanceLabel
        '
        Me.lblClientBalanceLabel.AutoSize = True
        Me.lblClientBalanceLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblClientBalanceLabel.Name = "lblClientBalanceLabel"
        Me.lblClientBalanceLabel.Size = New System.Drawing.Size(0, 13)
        Me.lblClientBalanceLabel.TabIndex = 0
        '
        'pnlLastYearTurnover
        '
        Me.pnlLastYearTurnover.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlLastYearTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLastYearTurnover.Controls.Add(Me.lblLastYearTurnoverLabel)
        Me.pnlLastYearTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLastYearTurnover.Location = New System.Drawing.Point(144, 64)
        Me.pnlLastYearTurnover.Name = "pnlLastYearTurnover"
        Me.pnlLastYearTurnover.Size = New System.Drawing.Size(177, 19)
        Me.pnlLastYearTurnover.TabIndex = 102
        '
        'lblLastYearTurnoverLabel
        '
        Me.lblLastYearTurnoverLabel.AutoSize = True
        Me.lblLastYearTurnoverLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblLastYearTurnoverLabel.Name = "lblLastYearTurnoverLabel"
        Me.lblLastYearTurnoverLabel.Size = New System.Drawing.Size(0, 13)
        Me.lblLastYearTurnoverLabel.TabIndex = 0
        '
        'pnlYearToDateTurnover
        '
        Me.pnlYearToDateTurnover.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlYearToDateTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlYearToDateTurnover.Controls.Add(Me.lblYearToDateTurnoverLabel)
        Me.pnlYearToDateTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlYearToDateTurnover.Location = New System.Drawing.Point(144, 40)
        Me.pnlYearToDateTurnover.Name = "pnlYearToDateTurnover"
        Me.pnlYearToDateTurnover.Size = New System.Drawing.Size(177, 19)
        Me.pnlYearToDateTurnover.TabIndex = 101
        '
        'lblYearToDateTurnoverLabel
        '
        Me.lblYearToDateTurnoverLabel.AutoSize = True
        Me.lblYearToDateTurnoverLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblYearToDateTurnoverLabel.Name = "lblYearToDateTurnoverLabel"
        Me.lblYearToDateTurnoverLabel.Size = New System.Drawing.Size(0, 13)
        Me.lblYearToDateTurnoverLabel.TabIndex = 0
        '
        'lblClientBalance
        '
        Me.lblClientBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientBalance.Location = New System.Drawing.Point(8, 16)
        Me.lblClientBalance.Name = "lblClientBalance"
        Me.lblClientBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientBalance.Size = New System.Drawing.Size(105, 17)
        Me.lblClientBalance.TabIndex = 0
        Me.lblClientBalance.Text = "Account Balance:"
        '
        'lblLastYearTurnover
        '
        Me.lblLastYearTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastYearTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastYearTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastYearTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastYearTurnover.Location = New System.Drawing.Point(8, 64)
        Me.lblLastYearTurnover.Name = "lblLastYearTurnover"
        Me.lblLastYearTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastYearTurnover.Size = New System.Drawing.Size(137, 17)
        Me.lblLastYearTurnover.TabIndex = 2
        Me.lblLastYearTurnover.Text = "Last Year Turnover:"
        '
        'lblYearToDateTurnover
        '
        Me.lblYearToDateTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblYearToDateTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYearToDateTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYearToDateTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYearToDateTurnover.Location = New System.Drawing.Point(8, 40)
        Me.lblYearToDateTurnover.Name = "lblYearToDateTurnover"
        Me.lblYearToDateTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYearToDateTurnover.Size = New System.Drawing.Size(185, 17)
        Me.lblYearToDateTurnover.TabIndex = 1
        Me.lblYearToDateTurnover.Text = "Year to Date Turnover:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboServiceLevel)
        Me.Frame1.Controls.Add(Me.txtAlternativeIdentifier)
        Me.Frame1.Controls.Add(Me.cboSubBranch)
        Me.Frame1.Controls.Add(Me.cboBranch)
        Me.Frame1.Controls.Add(Me.lblServicelevel)
        Me.Frame1.Controls.Add(Me.lblAlternativeIdentifier)
        Me.Frame1.Controls.Add(Me.lblSubBranch)
        Me.Frame1.Controls.Add(Me.lblBranch)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 142)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(303, 125)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        '
        'cboServiceLevel
        '
        Me.cboServiceLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboServiceLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboServiceLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboServiceLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboServiceLevel.Location = New System.Drawing.Point(112, 44)
        Me.cboServiceLevel.Name = "cboServiceLevel"
        Me.cboServiceLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboServiceLevel.Size = New System.Drawing.Size(185, 21)
        Me.cboServiceLevel.TabIndex = 3
        '
        'txtAlternativeIdentifier
        '
        Me.txtAlternativeIdentifier.AcceptsReturn = True
        Me.txtAlternativeIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlternativeIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlternativeIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlternativeIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlternativeIdentifier.Location = New System.Drawing.Point(112, 16)
        Me.txtAlternativeIdentifier.MaxLength = 0
        Me.txtAlternativeIdentifier.Name = "txtAlternativeIdentifier"
        Me.txtAlternativeIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlternativeIdentifier.Size = New System.Drawing.Size(185, 20)
        Me.txtAlternativeIdentifier.TabIndex = 1
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(112, 96)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(185, 21)
        Me.cboSubBranch.TabIndex = 7
        Me.cboSubBranch.Text = " "
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(112, 72)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(185, 21)
        Me.cboBranch.TabIndex = 5
        Me.cboBranch.Text = " "
        '
        'lblServicelevel
        '
        Me.lblServicelevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblServicelevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServicelevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServicelevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServicelevel.Location = New System.Drawing.Point(8, 48)
        Me.lblServicelevel.Name = "lblServicelevel"
        Me.lblServicelevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServicelevel.Size = New System.Drawing.Size(121, 17)
        Me.lblServicelevel.TabIndex = 2
        Me.lblServicelevel.Text = "Service level:"
        '
        'lblAlternativeIdentifier
        '
        Me.lblAlternativeIdentifier.AutoSize = True
        Me.lblAlternativeIdentifier.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlternativeIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlternativeIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlternativeIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlternativeIdentifier.Location = New System.Drawing.Point(8, 16)
        Me.lblAlternativeIdentifier.Name = "lblAlternativeIdentifier"
        Me.lblAlternativeIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlternativeIdentifier.Size = New System.Drawing.Size(103, 13)
        Me.lblAlternativeIdentifier.TabIndex = 0
        Me.lblAlternativeIdentifier.Text = "Alternative Identifier:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(8, 96)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(73, 17)
        Me.lblSubBranch.TabIndex = 6
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 72)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(57, 17)
        Me.lblBranch.TabIndex = 4
        Me.lblBranch.Text = "Branch:"
        '
        'fraBlackList
        '
        Me.fraBlackList.BackColor = System.Drawing.SystemColors.Control
        Me.fraBlackList.Controls.Add(Me.cboBlackListReason)
        Me.fraBlackList.Controls.Add(Me.lblBlacklistReason)
        Me.fraBlackList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBlackList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBlackList.Location = New System.Drawing.Point(8, 339)
        Me.fraBlackList.Name = "fraBlackList"
        Me.fraBlackList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBlackList.Size = New System.Drawing.Size(303, 49)
        Me.fraBlackList.TabIndex = 3
        Me.fraBlackList.TabStop = False
        Me.fraBlackList.Text = "Blacklisting"
        '
        'cboBlackListReason
        '
        Me.cboBlackListReason.BackColor = System.Drawing.SystemColors.Window
        Me.cboBlackListReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBlackListReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBlackListReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBlackListReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBlackListReason.Location = New System.Drawing.Point(104, 20)
        Me.cboBlackListReason.Name = "cboBlackListReason"
        Me.cboBlackListReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBlackListReason.Size = New System.Drawing.Size(177, 21)
        Me.cboBlackListReason.TabIndex = 1
        '
        'lblBlacklistReason
        '
        Me.lblBlacklistReason.AutoSize = True
        Me.lblBlacklistReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblBlacklistReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBlacklistReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBlacklistReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBlacklistReason.Location = New System.Drawing.Point(8, 24)
        Me.lblBlacklistReason.Name = "lblBlacklistReason"
        Me.lblBlacklistReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBlacklistReason.Size = New System.Drawing.Size(47, 13)
        Me.lblBlacklistReason.TabIndex = 0
        Me.lblBlacklistReason.Text = "Reason:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 2
        Me._cmdNext_1.TabStop = False
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(16, 8)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(569, 265)
        Me.fraAddress.TabIndex = 0
        Me.fraAddress.TabStop = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 232)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 3
        Me.cmdEditAd.Text = "Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 232)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 2
        Me.cmdDeleteAd.Text = "Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 232)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 1
        Me.cmdAddAd.Text = "Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAddresses, True)
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwAddresses, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(553, 209)
        Me.listViewHelper1.SetSmallIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwAddresses, False)
        Me.listViewHelper1.SetSortKey(Me.lvwAddresses, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwAddresses, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAddresses.TabIndex = 0
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Tag = ""
        Me._lvwAddresses_ColumnHeader_1.Text = "Address Usage"
        Me._lvwAddresses_ColumnHeader_1.Width = 167
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Tag = ""
        Me._lvwAddresses_ColumnHeader_2.Text = "Address Line 1"
        Me._lvwAddresses_ColumnHeader_2.Width = 97
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Tag = ""
        Me._lvwAddresses_ColumnHeader_3.Text = "Address Line 2"
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Tag = ""
        Me._lvwAddresses_ColumnHeader_4.Text = "Address Line 3"
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Tag = ""
        Me._lvwAddresses_ColumnHeader_5.Text = "Address Line 4"
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Tag = ""
        Me._lvwAddresses_ColumnHeader_6.Text = "PostCode"
        Me._lvwAddresses_ColumnHeader_6.Width = 67
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "AddressImage")
        Me.ImageList2.Images.SetKeyName(2, "ConvictionImage")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "PolicyImage")
        Me.ImageList2.Images.SetKeyName(5, "")
        Me.ImageList2.Images.SetKeyName(6, "ContactImage")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "CampaignImage")
        Me.ImageList2.Images.SetKeyName(10, "")
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 1
        Me._cmdPrevious_0.TabStop = False
        Me._cmdPrevious_0.Text = "<&<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraCorrespondence)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "&3 - Contact"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraCorrespondence
        '
        Me.fraCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.fraCorrespondence.Controls.Add(Me.chkeMPS)
        Me.fraCorrespondence.Controls.Add(Me.chkTPS)
        Me.fraCorrespondence.Controls.Add(Me.chkMailshot)
        Me.fraCorrespondence.Controls.Add(Me.cboCorrespondenceType)
        Me.fraCorrespondence.Controls.Add(Me.lblTPS)
        Me.fraCorrespondence.Controls.Add(Me.lblPreferredCorrespondence)
        Me.fraCorrespondence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCorrespondence.Location = New System.Drawing.Point(16, 244)
        Me.fraCorrespondence.Name = "fraCorrespondence"
        Me.fraCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCorrespondence.Size = New System.Drawing.Size(571, 69)
        Me.fraCorrespondence.TabIndex = 1
        Me.fraCorrespondence.TabStop = False
        Me.fraCorrespondence.Text = "Correspondence"
        '
        'chkeMPS
        '
        Me.chkeMPS.BackColor = System.Drawing.SystemColors.Control
        Me.chkeMPS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkeMPS.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkeMPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkeMPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkeMPS.Location = New System.Drawing.Point(206, 48)
        Me.chkeMPS.Name = "chkeMPS"
        Me.chkeMPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkeMPS.Size = New System.Drawing.Size(89, 17)
        Me.chkeMPS.TabIndex = 5
        Me.chkeMPS.Text = "eMPS:"
        Me.chkeMPS.UseVisualStyleBackColor = False
        '
        'chkTPS
        '
        Me.chkTPS.BackColor = System.Drawing.SystemColors.Control
        Me.chkTPS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTPS.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTPS.Location = New System.Drawing.Point(68, 48)
        Me.chkTPS.Name = "chkTPS"
        Me.chkTPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTPS.Size = New System.Drawing.Size(17, 17)
        Me.chkTPS.TabIndex = 3
        Me.chkTPS.Text = "Mailshot:"
        Me.chkTPS.UseVisualStyleBackColor = False
        '
        'chkMailshot
        '
        Me.chkMailshot.BackColor = System.Drawing.SystemColors.Control
        Me.chkMailshot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMailshot.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMailshot.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMailshot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMailshot.Location = New System.Drawing.Point(108, 48)
        Me.chkMailshot.Name = "chkMailshot"
        Me.chkMailshot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMailshot.Size = New System.Drawing.Size(81, 17)
        Me.chkMailshot.TabIndex = 4
        Me.chkMailshot.Text = "MPS:"
        Me.chkMailshot.UseVisualStyleBackColor = False
        '
        'cboCorrespondenceType
        '
        Me.cboCorrespondenceType.BackColor = System.Drawing.SystemColors.Window
        Me.cboCorrespondenceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCorrespondenceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCorrespondenceType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCorrespondenceType.Location = New System.Drawing.Point(190, 20)
        Me.cboCorrespondenceType.Name = "cboCorrespondenceType"
        Me.cboCorrespondenceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCorrespondenceType.Size = New System.Drawing.Size(217, 21)
        Me.cboCorrespondenceType.TabIndex = 1
        '
        'lblTPS
        '
        Me.lblTPS.BackColor = System.Drawing.SystemColors.Control
        Me.lblTPS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTPS.Location = New System.Drawing.Point(16, 48)
        Me.lblTPS.Name = "lblTPS"
        Me.lblTPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTPS.Size = New System.Drawing.Size(85, 17)
        Me.lblTPS.TabIndex = 2
        Me.lblTPS.Text = "TPS:"
        '
        'lblPreferredCorrespondence
        '
        Me.lblPreferredCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreferredCorrespondence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreferredCorrespondence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreferredCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreferredCorrespondence.Location = New System.Drawing.Point(14, 22)
        Me.lblPreferredCorrespondence.Name = "lblPreferredCorrespondence"
        Me.lblPreferredCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreferredCorrespondence.Size = New System.Drawing.Size(161, 19)
        Me.lblPreferredCorrespondence.TabIndex = 0
        Me.lblPreferredCorrespondence.Text = "Preferred Correspondence:"
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 3
        Me._cmdNext_2.TabStop = False
        Me._cmdNext_2.Text = "&>>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(16, 8)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(569, 225)
        Me.fraContact.TabIndex = 0
        Me.fraContact.TabStop = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 192)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 3
        Me.cmdEditCon.Text = "Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 192)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 2
        Me.cmdDeleteCon.Text = "Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 192)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 1
        Me.cmdAddCon.Text = "Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContacts, "")
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContacts, True)
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwContacts, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwContacts, "")
        Me.lvwContacts.LargeImageList = Me.ImageList2
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(553, 169)
        Me.listViewHelper1.SetSmallIcons(Me.lvwContacts, "")
        Me.lvwContacts.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwContacts, False)
        Me.listViewHelper1.SetSortKey(Me.lvwContacts, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwContacts, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContacts.TabIndex = 0
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Tag = ""
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 67
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Tag = ""
        Me._lvwContacts_ColumnHeader_2.Text = "Number"
        Me._lvwContacts_ColumnHeader_2.Width = 67
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Tag = ""
        Me._lvwContacts_ColumnHeader_3.Text = "Extension"
        Me._lvwContacts_ColumnHeader_3.Width = 67
        '
        '_lvwContacts_ColumnHeader_4
        '
        Me._lvwContacts_ColumnHeader_4.Tag = ""
        Me._lvwContacts_ColumnHeader_4.Text = "Type"
        Me._lvwContacts_ColumnHeader_4.Width = 97
        '
        '_lvwContacts_ColumnHeader_5
        '
        Me._lvwContacts_ColumnHeader_5.Tag = ""
        Me._lvwContacts_ColumnHeader_5.Text = "Description"
        Me._lvwContacts_ColumnHeader_5.Width = 134
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 2
        Me._cmdPrevious_1.TabStop = False
        Me._cmdPrevious_1.Text = "<&<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtHiddenDate)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtHiddenCurrency)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtCCJ)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmContacts)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblCCJ)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "&4 - Convictions"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'txtHiddenDate
        '
        Me.txtHiddenDate.AcceptsReturn = True
        Me.txtHiddenDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtHiddenDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHiddenDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHiddenDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHiddenDate.Location = New System.Drawing.Point(400, 260)
        Me.txtHiddenDate.MaxLength = 0
        Me.txtHiddenDate.Name = "txtHiddenDate"
        Me.txtHiddenDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHiddenDate.Size = New System.Drawing.Size(73, 20)
        Me.txtHiddenDate.TabIndex = 3
        Me.txtHiddenDate.TabStop = False
        Me.txtHiddenDate.Text = "hidden date"
        Me.txtHiddenDate.Visible = False
        '
        'txtHiddenCurrency
        '
        Me.txtHiddenCurrency.AcceptsReturn = True
        Me.txtHiddenCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtHiddenCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHiddenCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHiddenCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHiddenCurrency.Location = New System.Drawing.Point(480, 260)
        Me.txtHiddenCurrency.MaxLength = 0
        Me.txtHiddenCurrency.Name = "txtHiddenCurrency"
        Me.txtHiddenCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHiddenCurrency.Size = New System.Drawing.Size(97, 20)
        Me.txtHiddenCurrency.TabIndex = 4
        Me.txtHiddenCurrency.TabStop = False
        Me.txtHiddenCurrency.Text = "hidden currency"
        Me.txtHiddenCurrency.Visible = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 6
        Me._cmdNext_3.TabStop = False
        Me._cmdNext_3.Text = "&>>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'txtCCJ
        '
        Me.txtCCJ.AcceptsReturn = True
        Me.txtCCJ.BackColor = System.Drawing.SystemColors.Window
        Me.txtCCJ.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCCJ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCCJ.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCCJ.Location = New System.Drawing.Point(192, 260)
        Me.txtCCJ.MaxLength = 0
        Me.txtCCJ.Name = "txtCCJ"
        Me.txtCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCCJ.Size = New System.Drawing.Size(41, 20)
        Me.txtCCJ.TabIndex = 2
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 5
        Me._cmdPrevious_2.TabStop = False
        Me._cmdPrevious_2.Text = "<&<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'frmContacts
        '
        Me.frmContacts.Controls.Add(Me.cmdAddConv)
        Me.frmContacts.Controls.Add(Me.cmdDeleteConv)
        Me.frmContacts.Controls.Add(Me.cmdEditConv)
        Me.frmContacts.Controls.Add(Me.lvwConvictions)
        Me.frmContacts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmContacts.Location = New System.Drawing.Point(16, 8)
        Me.frmContacts.Name = "frmContacts"
        Me.frmContacts.Size = New System.Drawing.Size(569, 233)
        Me.frmContacts.TabIndex = 0
        Me.frmContacts.TabStop = False
        '
        'cmdAddConv
        '
        Me.cmdAddConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddConv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddConv.Location = New System.Drawing.Point(8, 200)
        Me.cmdAddConv.Name = "cmdAddConv"
        Me.cmdAddConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddConv.TabIndex = 1
        Me.cmdAddConv.Text = "Add"
        Me.cmdAddConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddConv.UseVisualStyleBackColor = False
        '
        'cmdDeleteConv
        '
        Me.cmdDeleteConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteConv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteConv.Location = New System.Drawing.Point(88, 200)
        Me.cmdDeleteConv.Name = "cmdDeleteConv"
        Me.cmdDeleteConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteConv.TabIndex = 2
        Me.cmdDeleteConv.Text = "Delete"
        Me.cmdDeleteConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteConv.UseVisualStyleBackColor = False
        '
        'cmdEditConv
        '
        Me.cmdEditConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditConv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditConv.Location = New System.Drawing.Point(168, 200)
        Me.cmdEditConv.Name = "cmdEditConv"
        Me.cmdEditConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditConv.TabIndex = 3
        Me.cmdEditConv.Text = "Edit"
        Me.cmdEditConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditConv.UseVisualStyleBackColor = False
        '
        'lvwConvictions
        '
        Me.lvwConvictions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwConvictions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwConvictions, "")
        Me.lvwConvictions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwConvictions_ColumnHeader_1, Me._lvwConvictions_ColumnHeader_2, Me._lvwConvictions_ColumnHeader_3, Me._lvwConvictions_ColumnHeader_4, Me._lvwConvictions_ColumnHeader_5, Me._lvwConvictions_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwConvictions, False)
        Me.lvwConvictions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwConvictions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwConvictions, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwConvictions, "")
        Me.lvwConvictions.Location = New System.Drawing.Point(8, 16)
        Me.lvwConvictions.Name = "lvwConvictions"
        Me.lvwConvictions.Size = New System.Drawing.Size(553, 177)
        Me.listViewHelper1.SetSmallIcons(Me.lvwConvictions, "")
        Me.lvwConvictions.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwConvictions, False)
        Me.listViewHelper1.SetSortKey(Me.lvwConvictions, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwConvictions, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwConvictions.TabIndex = 0
        Me.lvwConvictions.UseCompatibleStateImageBehavior = False
        Me.lvwConvictions.View = System.Windows.Forms.View.Details
        '
        '_lvwConvictions_ColumnHeader_1
        '
        Me._lvwConvictions_ColumnHeader_1.Tag = ""
        Me._lvwConvictions_ColumnHeader_1.Text = "Conviction Type"
        Me._lvwConvictions_ColumnHeader_1.Width = 97
        '
        '_lvwConvictions_ColumnHeader_2
        '
        Me._lvwConvictions_ColumnHeader_2.Tag = ""
        Me._lvwConvictions_ColumnHeader_2.Text = "Date"
        Me._lvwConvictions_ColumnHeader_2.Width = 67
        '
        '_lvwConvictions_ColumnHeader_3
        '
        Me._lvwConvictions_ColumnHeader_3.Tag = ""
        Me._lvwConvictions_ColumnHeader_3.Text = "Description"
        Me._lvwConvictions_ColumnHeader_3.Width = 134
        '
        '_lvwConvictions_ColumnHeader_4
        '
        Me._lvwConvictions_ColumnHeader_4.Tag = ""
        Me._lvwConvictions_ColumnHeader_4.Text = "Fine"
        Me._lvwConvictions_ColumnHeader_4.Width = 97
        '
        '_lvwConvictions_ColumnHeader_5
        '
        Me._lvwConvictions_ColumnHeader_5.Tag = ""
        Me._lvwConvictions_ColumnHeader_5.Text = "Status"
        Me._lvwConvictions_ColumnHeader_5.Width = 97
        '
        '_lvwConvictions_ColumnHeader_6
        '
        Me._lvwConvictions_ColumnHeader_6.Text = "Penalty Points"
        Me._lvwConvictions_ColumnHeader_6.Width = 97
        '
        'lblCCJ
        '
        Me.lblCCJ.BackColor = System.Drawing.SystemColors.Control
        Me.lblCCJ.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCCJ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCCJ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCCJ.Location = New System.Drawing.Point(24, 260)
        Me.lblCCJ.Name = "lblCCJ"
        Me.lblCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCCJ.Size = New System.Drawing.Size(161, 17)
        Me.lblCCJ.TabIndex = 1
        Me.lblCCJ.Text = "County court judgements:"
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.cmdAssociates)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraTab5_1)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraAreaCode)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "&5 - Additions"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'cmdAssociates
        '
        Me.cmdAssociates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAssociates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAssociates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAssociates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAssociates.Location = New System.Drawing.Point(8, 256)
        Me.cmdAssociates.Name = "cmdAssociates"
        Me.cmdAssociates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAssociates.Size = New System.Drawing.Size(73, 23)
        Me.cmdAssociates.TabIndex = 2
        Me.cmdAssociates.Text = "Associates"
        Me.cmdAssociates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAssociates.UseVisualStyleBackColor = False
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 4
        Me._cmdNext_4.TabStop = False
        Me._cmdNext_4.Text = "&>>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 3
        Me._cmdPrevious_3.TabStop = False
        Me._cmdPrevious_3.Text = "<&<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'fraTab5_1
        '
        Me.fraTab5_1.Controls.Add(Me.cboTermsOfPayment)
        Me.fraTab5_1.Controls.Add(Me.cboTurnover)
        Me.fraTab5_1.Controls.Add(Me.cboRenewalStopCode)
        Me.fraTab5_1.Controls.Add(Me.txtLoyaltyNumber)
        Me.fraTab5_1.Controls.Add(Me.cboCurrency)
        Me.fraTab5_1.Controls.Add(Me.txtLoyaltyNumberPrefix)
        Me.fraTab5_1.Controls.Add(Me.cboSeasonalGift)
        Me.fraTab5_1.Controls.Add(Me.cboCreditCard)
        Me.fraTab5_1.Controls.Add(Me.cboReminderType)
        Me.fraTab5_1.Controls.Add(Me.ddPaymentMethod)
        Me.fraTab5_1.Controls.Add(Me.lblTurnover)
        Me.fraTab5_1.Controls.Add(Me.lblCreditCard)
        Me.fraTab5_1.Controls.Add(Me.lblTermsOfPayment)
        Me.fraTab5_1.Controls.Add(Me.lblSeasonalGift)
        Me.fraTab5_1.Controls.Add(Me.lblRenewalStopCode)
        Me.fraTab5_1.Controls.Add(Me.lblLoyaltyNumber)
        Me.fraTab5_1.Controls.Add(Me.lblCurrency)
        Me.fraTab5_1.Controls.Add(Me.lblPaymentMethod)
        Me.fraTab5_1.Controls.Add(Me.lblReminderType)
        Me.fraTab5_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTab5_1.Location = New System.Drawing.Point(8, 4)
        Me.fraTab5_1.Name = "fraTab5_1"
        Me.fraTab5_1.Size = New System.Drawing.Size(601, 149)
        Me.fraTab5_1.TabIndex = 0
        Me.fraTab5_1.TabStop = False
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(424, 41)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(169, 21)
        Me.cboTermsOfPayment.TabIndex = 0
        '
        'cboTurnover
        '
        Me.cboTurnover.DefaultItemId = 0
        Me.cboTurnover.FirstItem = ""
        Me.cboTurnover.ItemId = 0
        Me.cboTurnover.ListIndex = -1
        Me.cboTurnover.Location = New System.Drawing.Point(424, 16)
        Me.cboTurnover.Name = "cboTurnover"
        Me.cboTurnover.PMLookupProductFamily = 1
        Me.cboTurnover.SingleItemId = 0
        Me.cboTurnover.Size = New System.Drawing.Size(169, 21)
        Me.cboTurnover.Sorted = True
        Me.cboTurnover.TabIndex = 10
        Me.cboTurnover.TableName = "TurnoverBand"
        Me.cboTurnover.ToolTipText = ""
        Me.cboTurnover.WhereClause = ""
        '
        'cboRenewalStopCode
        '
        Me.cboRenewalStopCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalStopCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalStopCode.Location = New System.Drawing.Point(424, 64)
        Me.cboRenewalStopCode.Name = "cboRenewalStopCode"
        Me.cboRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalStopCode.Size = New System.Drawing.Size(169, 21)
        Me.cboRenewalStopCode.TabIndex = 14
        '
        'txtLoyaltyNumber
        '
        Me.txtLoyaltyNumber.AcceptsReturn = True
        Me.txtLoyaltyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumber.Location = New System.Drawing.Point(184, 88)
        Me.txtLoyaltyNumber.MaxLength = 0
        Me.txtLoyaltyNumber.Name = "txtLoyaltyNumber"
        Me.txtLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumber.Size = New System.Drawing.Size(91, 20)
        Me.txtLoyaltyNumber.TabIndex = 8
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(112, 16)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(169, 21)
        Me.cboCurrency.TabIndex = 1
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'txtLoyaltyNumberPrefix
        '
        Me.txtLoyaltyNumberPrefix.AcceptsReturn = True
        Me.txtLoyaltyNumberPrefix.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumberPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumberPrefix.Enabled = False
        Me.txtLoyaltyNumberPrefix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumberPrefix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumberPrefix.Location = New System.Drawing.Point(112, 88)
        Me.txtLoyaltyNumberPrefix.MaxLength = 10
        Me.txtLoyaltyNumberPrefix.Name = "txtLoyaltyNumberPrefix"
        Me.txtLoyaltyNumberPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumberPrefix.Size = New System.Drawing.Size(65, 20)
        Me.txtLoyaltyNumberPrefix.TabIndex = 7
        Me.txtLoyaltyNumberPrefix.Text = "6014 35"
        '
        'cboSeasonalGift
        '
        Me.cboSeasonalGift.BackColor = System.Drawing.SystemColors.Window
        Me.cboSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSeasonalGift.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSeasonalGift.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSeasonalGift.Location = New System.Drawing.Point(424, 88)
        Me.cboSeasonalGift.Name = "cboSeasonalGift"
        Me.cboSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSeasonalGift.Size = New System.Drawing.Size(169, 21)
        Me.cboSeasonalGift.TabIndex = 16
        '
        'cboCreditCard
        '
        Me.cboCreditCard.BackColor = System.Drawing.SystemColors.Window
        Me.cboCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCreditCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCreditCard.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCreditCard.Location = New System.Drawing.Point(424, 112)
        Me.cboCreditCard.Name = "cboCreditCard"
        Me.cboCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCreditCard.Size = New System.Drawing.Size(169, 21)
        Me.cboCreditCard.TabIndex = 18
        Me.cboCreditCard.Tag = "31245190 Financial_Detail,Payment_Card_Code"
        Me.cboCreditCard.Visible = False
        '
        'cboReminderType
        '
        Me.cboReminderType.BackColor = System.Drawing.SystemColors.Window
        Me.cboReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReminderType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReminderType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReminderType.Location = New System.Drawing.Point(112, 64)
        Me.cboReminderType.Name = "cboReminderType"
        Me.cboReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReminderType.Size = New System.Drawing.Size(169, 21)
        Me.cboReminderType.TabIndex = 5
        Me.cboReminderType.Text = " "
        '
        'ddPaymentMethod
        '
        Me.ddPaymentMethod.AllowAbiCodeEntry = False
        Me.ddPaymentMethod.AutoCompleteText = False
        Me.ddPaymentMethod.DataModel = "GIIM"
        Me.ddPaymentMethod.ListIndex = -1
        Me.ddPaymentMethod.ListManager = Nothing
        Me.ddPaymentMethod.Location = New System.Drawing.Point(112, 41)
        Me.ddPaymentMethod.Login = False
        Me.ddPaymentMethod.LongList = False
        Me.ddPaymentMethod.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddPaymentMethod.Name = "ddPaymentMethod"
        Me.ddPaymentMethod.PropertyId = "6946819"
        Me.ddPaymentMethod.ReadOnly_Renamed = False
        Me.ddPaymentMethod.SelLength = 0
        Me.ddPaymentMethod.SelStart = 0
        Me.ddPaymentMethod.SelText = ""
        Me.ddPaymentMethod.Size = New System.Drawing.Size(169, 21)
        Me.ddPaymentMethod.TabIndex = 3
        Me.ddPaymentMethod.ToolTipText = ""
        Me.ddPaymentMethod.VehicleListId = ""
        Me.ddPaymentMethod.VehicleMake = ""
        '
        'lblTurnover
        '
        Me.lblTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTurnover.Location = New System.Drawing.Point(304, 16)
        Me.lblTurnover.Name = "lblTurnover"
        Me.lblTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTurnover.Size = New System.Drawing.Size(105, 17)
        Me.lblTurnover.TabIndex = 9
        Me.lblTurnover.Text = "Turnover:"
        '
        'lblCreditCard
        '
        Me.lblCreditCard.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreditCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreditCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreditCard.Location = New System.Drawing.Point(304, 114)
        Me.lblCreditCard.Name = "lblCreditCard"
        Me.lblCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreditCard.Size = New System.Drawing.Size(113, 17)
        Me.lblCreditCard.TabIndex = 17
        Me.lblCreditCard.Text = "Credit card type:"
        Me.lblCreditCard.Visible = False
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermsOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(304, 41)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(121, 17)
        Me.lblTermsOfPayment.TabIndex = 11
        Me.lblTermsOfPayment.Text = "Terms of payment:"
        '
        'lblSeasonalGift
        '
        Me.lblSeasonalGift.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeasonalGift.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeasonalGift.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeasonalGift.Location = New System.Drawing.Point(304, 90)
        Me.lblSeasonalGift.Name = "lblSeasonalGift"
        Me.lblSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeasonalGift.Size = New System.Drawing.Size(129, 17)
        Me.lblSeasonalGift.TabIndex = 15
        Me.lblSeasonalGift.Text = "Seasonal gift:"
        '
        'lblRenewalStopCode
        '
        Me.lblRenewalStopCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStopCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStopCode.Location = New System.Drawing.Point(304, 65)
        Me.lblRenewalStopCode.Name = "lblRenewalStopCode"
        Me.lblRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStopCode.Size = New System.Drawing.Size(129, 17)
        Me.lblRenewalStopCode.TabIndex = 13
        Me.lblRenewalStopCode.Text = "Renewal stop code:"
        '
        'lblLoyaltyNumber
        '
        Me.lblLoyaltyNumber.AutoSize = True
        Me.lblLoyaltyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoyaltyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoyaltyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoyaltyNumber.Location = New System.Drawing.Point(8, 92)
        Me.lblLoyaltyNumber.Name = "lblLoyaltyNumber"
        Me.lblLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoyaltyNumber.Size = New System.Drawing.Size(81, 13)
        Me.lblLoyaltyNumber.TabIndex = 6
        Me.lblLoyaltyNumber.Text = "Loyalty number:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 18)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(97, 17)
        Me.lblCurrency.TabIndex = 0
        Me.lblCurrency.Text = "Currency:"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(8, 42)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(105, 17)
        Me.lblPaymentMethod.TabIndex = 2
        Me.lblPaymentMethod.Text = "Payment method:"
        '
        'lblReminderType
        '
        Me.lblReminderType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReminderType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReminderType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReminderType.Location = New System.Drawing.Point(8, 66)
        Me.lblReminderType.Name = "lblReminderType"
        Me.lblReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReminderType.Size = New System.Drawing.Size(97, 17)
        Me.lblReminderType.TabIndex = 4
        Me.lblReminderType.Text = "Reminder type:"
        '
        'fraAreaCode
        '
        Me.fraAreaCode.Controls.Add(Me.txtTobLetter)
        Me.fraAreaCode.Controls.Add(Me.cboArea)
        Me.fraAreaCode.Controls.Add(Me.txtFileCode)
        Me.fraAreaCode.Controls.Add(Me.lblTobLetter)
        Me.fraAreaCode.Controls.Add(Me.lblArea)
        Me.fraAreaCode.Controls.Add(Me.lblFileCode)
        Me.fraAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAreaCode.Location = New System.Drawing.Point(8, 164)
        Me.fraAreaCode.Name = "fraAreaCode"
        Me.fraAreaCode.Size = New System.Drawing.Size(601, 86)
        Me.fraAreaCode.TabIndex = 1
        Me.fraAreaCode.TabStop = False
        '
        'txtTobLetter
        '
        Me.txtTobLetter.AcceptsReturn = True
        Me.txtTobLetter.BackColor = System.Drawing.SystemColors.Window
        Me.txtTobLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTobLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTobLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTobLetter.Location = New System.Drawing.Point(400, 48)
        Me.txtTobLetter.MaxLength = 0
        Me.txtTobLetter.Name = "txtTobLetter"
        Me.txtTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTobLetter.Size = New System.Drawing.Size(153, 20)
        Me.txtTobLetter.TabIndex = 5
        '
        'cboArea
        '
        Me.cboArea.BackColor = System.Drawing.SystemColors.Window
        Me.cboArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboArea.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboArea.Location = New System.Drawing.Point(112, 16)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboArea.Size = New System.Drawing.Size(129, 21)
        Me.cboArea.TabIndex = 1
        '
        'txtFileCode
        '
        Me.txtFileCode.AcceptsReturn = True
        Me.txtFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileCode.Location = New System.Drawing.Point(400, 16)
        Me.txtFileCode.MaxLength = 0
        Me.txtFileCode.Name = "txtFileCode"
        Me.txtFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileCode.Size = New System.Drawing.Size(153, 20)
        Me.txtFileCode.TabIndex = 3
        '
        'lblTobLetter
        '
        Me.lblTobLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblTobLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTobLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTobLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTobLetter.Location = New System.Drawing.Point(272, 48)
        Me.lblTobLetter.Name = "lblTobLetter"
        Me.lblTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTobLetter.Size = New System.Drawing.Size(113, 33)
        Me.lblTobLetter.TabIndex = 4
        Me.lblTobLetter.Text = "Terms Of Business Letter Sent:"
        '
        'lblArea
        '
        Me.lblArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArea.Location = New System.Drawing.Point(8, 19)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArea.Size = New System.Drawing.Size(89, 17)
        Me.lblArea.TabIndex = 0
        Me.lblArea.Text = "Area:"
        '
        'lblFileCode
        '
        Me.lblFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileCode.Location = New System.Drawing.Point(272, 19)
        Me.lblFileCode.Name = "lblFileCode"
        Me.lblFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileCode.Size = New System.Drawing.Size(65, 17)
        Me.lblFileCode.TabIndex = 2
        Me.lblFileCode.Text = "Filecode:"
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraLoyaltySchemes)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "&6 - Misc"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_5.TabIndex = 5
        Me._cmdNext_5.TabStop = False
        Me._cmdNext_5.Text = "&>>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 1
        Me._cmdPrevious_4.TabStop = False
        Me._cmdPrevious_4.Text = "<&<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        'fraLoyaltySchemes
        '
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdAddLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdDeleteLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdEditLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.lvwLoyaltySchemes)
        Me.fraLoyaltySchemes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLoyaltySchemes.Location = New System.Drawing.Point(8, 8)
        Me.fraLoyaltySchemes.Name = "fraLoyaltySchemes"
        Me.fraLoyaltySchemes.Size = New System.Drawing.Size(577, 281)
        Me.fraLoyaltySchemes.TabIndex = 0
        Me.fraLoyaltySchemes.TabStop = False
        Me.fraLoyaltySchemes.Text = "{Loyalty Schemes}"
        '
        'cmdAddLoyaltyScheme
        '
        Me.cmdAddLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddLoyaltyScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddLoyaltyScheme.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddLoyaltyScheme.Name = "cmdAddLoyaltyScheme"
        Me.cmdAddLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddLoyaltyScheme.TabIndex = 1
        Me.cmdAddLoyaltyScheme.Text = "{Add}"
        Me.cmdAddLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'cmdDeleteLoyaltyScheme
        '
        Me.cmdDeleteLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteLoyaltyScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteLoyaltyScheme.Location = New System.Drawing.Point(88, 248)
        Me.cmdDeleteLoyaltyScheme.Name = "cmdDeleteLoyaltyScheme"
        Me.cmdDeleteLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteLoyaltyScheme.TabIndex = 2
        Me.cmdDeleteLoyaltyScheme.Text = "{Delete}"
        Me.cmdDeleteLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'cmdEditLoyaltyScheme
        '
        Me.cmdEditLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditLoyaltyScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditLoyaltyScheme.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditLoyaltyScheme.Name = "cmdEditLoyaltyScheme"
        Me.cmdEditLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditLoyaltyScheme.TabIndex = 3
        Me.cmdEditLoyaltyScheme.Text = "{Edit}"
        Me.cmdEditLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'lvwLoyaltySchemes
        '
        Me.lvwLoyaltySchemes.BackColor = System.Drawing.SystemColors.Window
        Me.lvwLoyaltySchemes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwLoyaltySchemes, "")
        Me.lvwLoyaltySchemes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLoyaltySchemes_ColumnHeader_1, Me._lvwLoyaltySchemes_ColumnHeader_2, Me._lvwLoyaltySchemes_ColumnHeader_3, Me._lvwLoyaltySchemes_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwLoyaltySchemes, False)
        Me.lvwLoyaltySchemes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLoyaltySchemes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwLoyaltySchemes, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwLoyaltySchemes, "")
        Me.lvwLoyaltySchemes.LargeImageList = Me.ImageList2
        Me.lvwLoyaltySchemes.Location = New System.Drawing.Point(8, 16)
        Me.lvwLoyaltySchemes.Name = "lvwLoyaltySchemes"
        Me.lvwLoyaltySchemes.Size = New System.Drawing.Size(561, 217)
        Me.listViewHelper1.SetSmallIcons(Me.lvwLoyaltySchemes, "")
        Me.lvwLoyaltySchemes.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwLoyaltySchemes, False)
        Me.listViewHelper1.SetSortKey(Me.lvwLoyaltySchemes, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwLoyaltySchemes, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwLoyaltySchemes.TabIndex = 0
        Me.lvwLoyaltySchemes.UseCompatibleStateImageBehavior = False
        Me.lvwLoyaltySchemes.View = System.Windows.Forms.View.Details
        '
        '_lvwLoyaltySchemes_ColumnHeader_1
        '
        Me._lvwLoyaltySchemes_ColumnHeader_1.Tag = ""
        Me._lvwLoyaltySchemes_ColumnHeader_1.Text = "Scheme Name"
        Me._lvwLoyaltySchemes_ColumnHeader_1.Width = 134
        '
        '_lvwLoyaltySchemes_ColumnHeader_2
        '
        Me._lvwLoyaltySchemes_ColumnHeader_2.Tag = ""
        Me._lvwLoyaltySchemes_ColumnHeader_2.Text = "Membership Number"
        Me._lvwLoyaltySchemes_ColumnHeader_2.Width = 134
        '
        '_lvwLoyaltySchemes_ColumnHeader_3
        '
        Me._lvwLoyaltySchemes_ColumnHeader_3.Tag = ""
        Me._lvwLoyaltySchemes_ColumnHeader_3.Text = "StartDate"
        Me._lvwLoyaltySchemes_ColumnHeader_3.Width = 114
        '
        '_lvwLoyaltySchemes_ColumnHeader_4
        '
        Me._lvwLoyaltySchemes_ColumnHeader_4.Tag = ""
        Me._lvwLoyaltySchemes_ColumnHeader_4.Text = "End Date"
        Me._lvwLoyaltySchemes_ColumnHeader_4.Width = 114
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraPolicies)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraCampaign)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraProspect)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraPreviousInsurer)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraPreviousBroker)
        Me._tabMainTab_TabPage6.Controls.Add(Me.cboPolicyType)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "&7 - Tax"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'fraPolicies
        '
        Me.fraPolicies.Controls.Add(Me.cmdAddPol)
        Me.fraPolicies.Controls.Add(Me.cmdDeletePol)
        Me.fraPolicies.Controls.Add(Me.cmdEditPol)
        Me.fraPolicies.Controls.Add(Me.lvwPolicies)
        Me.fraPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicies.Location = New System.Drawing.Point(304, 188)
        Me.fraPolicies.Name = "fraPolicies"
        Me.fraPolicies.Size = New System.Drawing.Size(289, 121)
        Me.fraPolicies.TabIndex = 4
        Me.fraPolicies.TabStop = False
        Me.fraPolicies.Text = "Policies"
        '
        'cmdAddPol
        '
        Me.cmdAddPol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddPol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddPol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddPol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddPol.Location = New System.Drawing.Point(48, 88)
        Me.cmdAddPol.Name = "cmdAddPol"
        Me.cmdAddPol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddPol.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddPol.TabIndex = 1
        Me.cmdAddPol.Text = "Add"
        Me.cmdAddPol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddPol.UseVisualStyleBackColor = False
        '
        'cmdDeletePol
        '
        Me.cmdDeletePol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeletePol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeletePol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeletePol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeletePol.Location = New System.Drawing.Point(128, 88)
        Me.cmdDeletePol.Name = "cmdDeletePol"
        Me.cmdDeletePol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeletePol.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeletePol.TabIndex = 2
        Me.cmdDeletePol.Text = "Delete"
        Me.cmdDeletePol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeletePol.UseVisualStyleBackColor = False
        '
        'cmdEditPol
        '
        Me.cmdEditPol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditPol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditPol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditPol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditPol.Location = New System.Drawing.Point(208, 88)
        Me.cmdEditPol.Name = "cmdEditPol"
        Me.cmdEditPol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditPol.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditPol.TabIndex = 3
        Me.cmdEditPol.Text = "Edit"
        Me.cmdEditPol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditPol.UseVisualStyleBackColor = False
        '
        'lvwPolicies
        '
        Me.lvwPolicies.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicies.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicies_ColumnHeader_1, Me._lvwPolicies_ColumnHeader_2, Me._lvwPolicies_ColumnHeader_3, Me._lvwPolicies_ColumnHeader_4, Me._lvwPolicies_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicies, False)
        Me.lvwPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicies.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicies, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.Location = New System.Drawing.Point(8, 24)
        Me.lvwPolicies.Name = "lvwPolicies"
        Me.lvwPolicies.Size = New System.Drawing.Size(272, 57)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwPolicies, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicies, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicies, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicies.TabIndex = 0
        Me.lvwPolicies.UseCompatibleStateImageBehavior = False
        Me.lvwPolicies.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicies_ColumnHeader_1
        '
        Me._lvwPolicies_ColumnHeader_1.Tag = ""
        Me._lvwPolicies_ColumnHeader_1.Text = "Type"
        Me._lvwPolicies_ColumnHeader_1.Width = 97
        '
        '_lvwPolicies_ColumnHeader_2
        '
        Me._lvwPolicies_ColumnHeader_2.Tag = ""
        Me._lvwPolicies_ColumnHeader_2.Text = "Renewal Date"
        Me._lvwPolicies_ColumnHeader_2.Width = 97
        '
        '_lvwPolicies_ColumnHeader_3
        '
        Me._lvwPolicies_ColumnHeader_3.Tag = ""
        Me._lvwPolicies_ColumnHeader_3.Text = "Quoted"
        Me._lvwPolicies_ColumnHeader_3.Width = 97
        '
        '_lvwPolicies_ColumnHeader_4
        '
        Me._lvwPolicies_ColumnHeader_4.Tag = ""
        Me._lvwPolicies_ColumnHeader_4.Text = "Target Premium"
        Me._lvwPolicies_ColumnHeader_4.Width = 97
        '
        '_lvwPolicies_ColumnHeader_5
        '
        Me._lvwPolicies_ColumnHeader_5.Tag = ""
        Me._lvwPolicies_ColumnHeader_5.Text = "Sorted Date"
        Me._lvwPolicies_ColumnHeader_5.Width = 0
        '
        'fraCampaign
        '
        Me.fraCampaign.Controls.Add(Me.lvwCampaigns)
        Me.fraCampaign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCampaign.Location = New System.Drawing.Point(8, 188)
        Me.fraCampaign.Name = "fraCampaign"
        Me.fraCampaign.Size = New System.Drawing.Size(289, 121)
        Me.fraCampaign.TabIndex = 3
        Me.fraCampaign.TabStop = False
        Me.fraCampaign.Text = "Campaigns"
        '
        'lvwCampaigns
        '
        Me.lvwCampaigns.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCampaigns.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCampaigns, "")
        Me.lvwCampaigns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCampaigns_ColumnHeader_1, Me._lvwCampaigns_ColumnHeader_2, Me._lvwCampaigns_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCampaigns, False)
        Me.lvwCampaigns.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCampaigns.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCampaigns.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCampaigns, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCampaigns, "")
        Me.lvwCampaigns.LargeImageList = Me.ImageList2
        Me.lvwCampaigns.Location = New System.Drawing.Point(8, 24)
        Me.lvwCampaigns.Name = "lvwCampaigns"
        Me.lvwCampaigns.Size = New System.Drawing.Size(263, 81)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCampaigns, "")
        Me.lvwCampaigns.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwCampaigns, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCampaigns, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCampaigns, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCampaigns.TabIndex = 0
        Me.lvwCampaigns.UseCompatibleStateImageBehavior = False
        Me.lvwCampaigns.View = System.Windows.Forms.View.Details
        '
        '_lvwCampaigns_ColumnHeader_1
        '
        Me._lvwCampaigns_ColumnHeader_1.Tag = ""
        Me._lvwCampaigns_ColumnHeader_1.Text = "Campaign"
        Me._lvwCampaigns_ColumnHeader_1.Width = 97
        '
        '_lvwCampaigns_ColumnHeader_2
        '
        Me._lvwCampaigns_ColumnHeader_2.Tag = ""
        Me._lvwCampaigns_ColumnHeader_2.Text = "Campaign Date"
        Me._lvwCampaigns_ColumnHeader_2.Width = 97
        '
        '_lvwCampaigns_ColumnHeader_3
        '
        Me._lvwCampaigns_ColumnHeader_3.Tag = ""
        Me._lvwCampaigns_ColumnHeader_3.Text = "Sorted Date"
        Me._lvwCampaigns_ColumnHeader_3.Width = 0
        '
        'fraProspect
        '
        Me.fraProspect.Controls.Add(Me.cboStrengthCode)
        Me.fraProspect.Controls.Add(Me.cboProspectingStatus)
        Me.fraProspect.Controls.Add(Me.txtAgentReference)
        Me.fraProspect.Controls.Add(Me.cmdCurrentAgent)
        Me.fraProspect.Controls.Add(Me.pnlCurrentAgent)
        Me.fraProspect.Controls.Add(Me.lblStrengthCode)
        Me.fraProspect.Controls.Add(Me.lblProspectStatus)
        Me.fraProspect.Controls.Add(Me.lblAgentReference)
        Me.fraProspect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProspect.Location = New System.Drawing.Point(8, 8)
        Me.fraProspect.Name = "fraProspect"
        Me.fraProspect.Size = New System.Drawing.Size(585, 73)
        Me.fraProspect.TabIndex = 0
        Me.fraProspect.TabStop = False
        '
        'cboStrengthCode
        '
        Me.cboStrengthCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboStrengthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStrengthCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStrengthCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStrengthCode.Location = New System.Drawing.Point(416, 16)
        Me.cboStrengthCode.Name = "cboStrengthCode"
        Me.cboStrengthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStrengthCode.Size = New System.Drawing.Size(161, 21)
        Me.cboStrengthCode.TabIndex = 4
        '
        'cboProspectingStatus
        '
        Me.cboProspectingStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboProspectingStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProspectingStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProspectingStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProspectingStatus.Location = New System.Drawing.Point(416, 40)
        Me.cboProspectingStatus.Name = "cboProspectingStatus"
        Me.cboProspectingStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProspectingStatus.Size = New System.Drawing.Size(161, 21)
        Me.cboProspectingStatus.TabIndex = 6
        '
        'txtAgentReference
        '
        Me.txtAgentReference.AcceptsReturn = True
        Me.txtAgentReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentReference.Location = New System.Drawing.Point(120, 16)
        Me.txtAgentReference.MaxLength = 0
        Me.txtAgentReference.Name = "txtAgentReference"
        Me.txtAgentReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentReference.Size = New System.Drawing.Size(169, 20)
        Me.txtAgentReference.TabIndex = 1
        '
        'cmdCurrentAgent
        '
        Me.cmdCurrentAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCurrentAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCurrentAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCurrentAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCurrentAgent.Location = New System.Drawing.Point(8, 40)
        Me.cmdCurrentAgent.Name = "cmdCurrentAgent"
        Me.cmdCurrentAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCurrentAgent.Size = New System.Drawing.Size(105, 21)
        Me.cmdCurrentAgent.TabIndex = 2
        Me.cmdCurrentAgent.Text = "Current Agent..."
        Me.cmdCurrentAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCurrentAgent.UseVisualStyleBackColor = False
        '
        'pnlCurrentAgent
        '
        Me.pnlCurrentAgent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCurrentAgent.Controls.Add(Me.lblCurrentAgentLabel)
        Me.pnlCurrentAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCurrentAgent.Location = New System.Drawing.Point(120, 40)
        Me.pnlCurrentAgent.Name = "pnlCurrentAgent"
        Me.pnlCurrentAgent.Size = New System.Drawing.Size(169, 19)
        Me.pnlCurrentAgent.TabIndex = 116
        '
        'lblCurrentAgentLabel
        '
        Me.lblCurrentAgentLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblCurrentAgentLabel.Name = "lblCurrentAgentLabel"
        Me.lblCurrentAgentLabel.Size = New System.Drawing.Size(100, 17)
        Me.lblCurrentAgentLabel.TabIndex = 0
        '
        'lblStrengthCode
        '
        Me.lblStrengthCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblStrengthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStrengthCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStrengthCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStrengthCode.Location = New System.Drawing.Point(304, 16)
        Me.lblStrengthCode.Name = "lblStrengthCode"
        Me.lblStrengthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStrengthCode.Size = New System.Drawing.Size(121, 17)
        Me.lblStrengthCode.TabIndex = 3
        Me.lblStrengthCode.Text = "Strength Code:"
        '
        'lblProspectStatus
        '
        Me.lblProspectStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblProspectStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProspectStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProspectStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProspectStatus.Location = New System.Drawing.Point(304, 40)
        Me.lblProspectStatus.Name = "lblProspectStatus"
        Me.lblProspectStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProspectStatus.Size = New System.Drawing.Size(113, 17)
        Me.lblProspectStatus.TabIndex = 5
        Me.lblProspectStatus.Text = "Status:"
        '
        'lblAgentReference
        '
        Me.lblAgentReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentReference.Location = New System.Drawing.Point(8, 19)
        Me.lblAgentReference.Name = "lblAgentReference"
        Me.lblAgentReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentReference.Size = New System.Drawing.Size(105, 17)
        Me.lblAgentReference.TabIndex = 0
        Me.lblAgentReference.Text = "Agent Reference:"
        '
        'fraPreviousInsurer
        '
        Me.fraPreviousInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreviousInsurer.Controls.Add(Me.txtInsurerRef)
        Me.fraPreviousInsurer.Controls.Add(Me.cmdInsurerLookup)
        Me.fraPreviousInsurer.Controls.Add(Me.pnlInsurerName)
        Me.fraPreviousInsurer.Controls.Add(Me.lblInsurerName)
        Me.fraPreviousInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPreviousInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreviousInsurer.Location = New System.Drawing.Point(8, 100)
        Me.fraPreviousInsurer.Name = "fraPreviousInsurer"
        Me.fraPreviousInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousInsurer.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousInsurer.TabIndex = 1
        Me.fraPreviousInsurer.TabStop = False
        Me.fraPreviousInsurer.Text = "Previous Insurer"
        '
        'txtInsurerRef
        '
        Me.txtInsurerRef.AcceptsReturn = True
        Me.txtInsurerRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerRef.Location = New System.Drawing.Point(80, 16)
        Me.txtInsurerRef.MaxLength = 0
        Me.txtInsurerRef.Name = "txtInsurerRef"
        Me.txtInsurerRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerRef.Size = New System.Drawing.Size(201, 20)
        Me.txtInsurerRef.TabIndex = 1
        '
        'cmdInsurerLookup
        '
        Me.cmdInsurerLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurerLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurerLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurerLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurerLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdInsurerLookup.Name = "cmdInsurerLookup"
        Me.cmdInsurerLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurerLookup.Size = New System.Drawing.Size(60, 19)
        Me.cmdInsurerLookup.TabIndex = 0
        Me.cmdInsurerLookup.Text = "Code..."
        Me.cmdInsurerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurerLookup.UseVisualStyleBackColor = False
        '
        'pnlInsurerName
        '
        Me.pnlInsurerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlInsurerName.Controls.Add(Me.lblInsurerNameLabel)
        Me.pnlInsurerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlInsurerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlInsurerName.Name = "pnlInsurerName"
        Me.pnlInsurerName.Size = New System.Drawing.Size(201, 19)
        Me.pnlInsurerName.TabIndex = 123
        '
        'lblInsurerNameLabel
        '
        Me.lblInsurerNameLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblInsurerNameLabel.Name = "lblInsurerNameLabel"
        Me.lblInsurerNameLabel.Size = New System.Drawing.Size(100, 17)
        Me.lblInsurerNameLabel.TabIndex = 0
        '
        'lblInsurerName
        '
        Me.lblInsurerName.AutoSize = True
        Me.lblInsurerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerName.Location = New System.Drawing.Point(8, 43)
        Me.lblInsurerName.Name = "lblInsurerName"
        Me.lblInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerName.Size = New System.Drawing.Size(38, 13)
        Me.lblInsurerName.TabIndex = 2
        Me.lblInsurerName.Text = "Name:"
        '
        'fraPreviousBroker
        '
        Me.fraPreviousBroker.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreviousBroker.Controls.Add(Me.cmdBrokerLookup)
        Me.fraPreviousBroker.Controls.Add(Me.txtBrokerRef)
        Me.fraPreviousBroker.Controls.Add(Me.pnlBrokerName)
        Me.fraPreviousBroker.Controls.Add(Me.lblBrokerName)
        Me.fraPreviousBroker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPreviousBroker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreviousBroker.Location = New System.Drawing.Point(304, 100)
        Me.fraPreviousBroker.Name = "fraPreviousBroker"
        Me.fraPreviousBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousBroker.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousBroker.TabIndex = 2
        Me.fraPreviousBroker.TabStop = False
        Me.fraPreviousBroker.Text = "Previous Broker"
        '
        'cmdBrokerLookup
        '
        Me.cmdBrokerLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrokerLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrokerLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrokerLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrokerLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdBrokerLookup.Name = "cmdBrokerLookup"
        Me.cmdBrokerLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrokerLookup.Size = New System.Drawing.Size(60, 19)
        Me.cmdBrokerLookup.TabIndex = 0
        Me.cmdBrokerLookup.Text = "Code..."
        Me.cmdBrokerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrokerLookup.UseVisualStyleBackColor = False
        '
        'txtBrokerRef
        '
        Me.txtBrokerRef.AcceptsReturn = True
        Me.txtBrokerRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerRef.Location = New System.Drawing.Point(80, 16)
        Me.txtBrokerRef.MaxLength = 0
        Me.txtBrokerRef.Name = "txtBrokerRef"
        Me.txtBrokerRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerRef.Size = New System.Drawing.Size(201, 20)
        Me.txtBrokerRef.TabIndex = 1
        '
        'pnlBrokerName
        '
        Me.pnlBrokerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBrokerName.Controls.Add(Me.lblBrokerNameLabel)
        Me.pnlBrokerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlBrokerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlBrokerName.Name = "pnlBrokerName"
        Me.pnlBrokerName.Size = New System.Drawing.Size(201, 19)
        Me.pnlBrokerName.TabIndex = 129
        '
        'lblBrokerNameLabel
        '
        Me.lblBrokerNameLabel.Location = New System.Drawing.Point(0, 0)
        Me.lblBrokerNameLabel.Name = "lblBrokerNameLabel"
        Me.lblBrokerNameLabel.Size = New System.Drawing.Size(100, 17)
        Me.lblBrokerNameLabel.TabIndex = 0
        '
        'lblBrokerName
        '
        Me.lblBrokerName.AutoSize = True
        Me.lblBrokerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerName.Location = New System.Drawing.Point(8, 43)
        Me.lblBrokerName.Name = "lblBrokerName"
        Me.lblBrokerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerName.Size = New System.Drawing.Size(38, 13)
        Me.lblBrokerName.TabIndex = 2
        Me.lblBrokerName.Text = "Name:"
        '
        'cboPolicyType
        '
        Me.cboPolicyType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyType.Location = New System.Drawing.Point(424, 324)
        Me.cboPolicyType.Name = "cboPolicyType"
        Me.cboPolicyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyType.Size = New System.Drawing.Size(169, 21)
        Me.cboPolicyType.TabIndex = 5
        Me.cboPolicyType.TabStop = False
        Me.cboPolicyType.Visible = False
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_5.TabIndex = 6
        Me._cmdPrevious_5.TabStop = False
        Me._cmdPrevious_5.Text = "<&<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        '_cmdNext_6
        '
        Me._cmdNext_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_6.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_6.Name = "_cmdNext_6"
        Me._cmdNext_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_6.TabIndex = 7
        Me._cmdNext_6.TabStop = False
        Me._cmdNext_6.Text = "&>>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdNext_6)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage7.Controls.Add(Me.uctPartyTax1)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "8 - Tax"
        Me._tabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(8, 8)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(577, 129)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 0
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me._cmdPrevious_6)
        Me._tabMainTab_TabPage8.Controls.Add(Me.uctPartyBankControl1)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(738, 441)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "8 - Bank"
        Me._tabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_6.TabIndex = 1
        Me._cmdPrevious_6.TabStop = False
        Me._cmdPrevious_6.Text = "<&<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = Nothing
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(8, 12)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = Nothing
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(689, 372)
        Me.uctPartyBankControl1.TabIndex = 0
        '
        '_cmdNext_7
        '
        Me._cmdNext_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_7.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_7.Name = "_cmdNext_7"
        Me._cmdNext_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_7.TabIndex = 2
        Me._cmdNext_7.TabStop = False
        Me._cmdNext_7.Text = "&>>"
        Me._cmdNext_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_7.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_7
        '
        Me._cmdPrevious_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_7.Location = New System.Drawing.Point(16, 383)
        Me._cmdPrevious_7.Name = "_cmdPrevious_7"
        Me._cmdPrevious_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_7.TabIndex = 1
        Me._cmdPrevious_7.TabStop = False
        Me._cmdPrevious_7.Text = "<&<"
        Me._cmdPrevious_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_7.UseVisualStyleBackColor = False
        '
        'uctPartyGCControl
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPartyGCControl"
        Me.Size = New System.Drawing.Size(756, 479)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraConsultant.ResumeLayout(False)
        Me.fraConsultant.PerformLayout()
        Me.pnlConsultantName.ResumeLayout(False)
        Me.pnlConsultantName.PerformLayout()
        Me.fraCharityDetails.ResumeLayout(False)
        Me.fraCharityDetails.PerformLayout()
        Me.fraClient.ResumeLayout(False)
        Me.fraClient.PerformLayout()
        Me.fraAgent.ResumeLayout(False)
        Me.fraAgent.PerformLayout()
        Me.pnlAgentName.ResumeLayout(False)
        Me.pnlAgentName.PerformLayout()
        Me.fraClientAccounts.ResumeLayout(False)
        Me.pnlClientBalance.ResumeLayout(False)
        Me.pnlClientBalance.PerformLayout()
        Me.pnlLastYearTurnover.ResumeLayout(False)
        Me.pnlLastYearTurnover.PerformLayout()
        Me.pnlYearToDateTurnover.ResumeLayout(False)
        Me.pnlYearToDateTurnover.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraBlackList.ResumeLayout(False)
        Me.fraBlackList.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraCorrespondence.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage3.PerformLayout()
        Me.frmContacts.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraTab5_1.ResumeLayout(False)
        Me.fraTab5_1.PerformLayout()
        Me.fraAreaCode.ResumeLayout(False)
        Me.fraAreaCode.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraLoyaltySchemes.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraPolicies.ResumeLayout(False)
        Me.fraCampaign.ResumeLayout(False)
        Me.fraProspect.ResumeLayout(False)
        Me.fraProspect.PerformLayout()
        Me.pnlCurrentAgent.ResumeLayout(False)
        Me.fraPreviousInsurer.ResumeLayout(False)
        Me.fraPreviousInsurer.PerformLayout()
        Me.pnlInsurerName.ResumeLayout(False)
        Me.fraPreviousBroker.ResumeLayout(False)
        Me.fraPreviousBroker.PerformLayout()
        Me.pnlBrokerName.ResumeLayout(False)
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdPrevious()
		Me.cmdPrevious(7) = _cmdPrevious_7
		Me.cmdPrevious(6) = _cmdPrevious_6
		Me.cmdPrevious(5) = _cmdPrevious_5
		Me.cmdPrevious(4) = _cmdPrevious_4
		Me.cmdPrevious(2) = _cmdPrevious_2
		Me.cmdPrevious(3) = _cmdPrevious_3
		Me.cmdPrevious(1) = _cmdPrevious_1
		Me.cmdPrevious(0) = _cmdPrevious_0
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(7) = _cmdNext_7
		Me.cmdNext(6) = _cmdNext_6
		Me.cmdNext(5) = _cmdNext_5
		Me.cmdNext(4) = _cmdNext_4
		Me.cmdNext(2) = _cmdNext_2
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(3) = _cmdNext_3
		Me.cmdNext(0) = _cmdNext_0
	End Sub
	Sub lvwCampaigns_InitializeColumnKeys()
		Me._lvwCampaigns_ColumnHeader_1.Name = ""
		Me._lvwCampaigns_ColumnHeader_2.Name = ""
		Me._lvwCampaigns_ColumnHeader_3.Name = ""
	End Sub
	Sub lvwPolicies_InitializeColumnKeys()
		Me._lvwPolicies_ColumnHeader_1.Name = ""
		Me._lvwPolicies_ColumnHeader_2.Name = ""
		Me._lvwPolicies_ColumnHeader_3.Name = ""
		Me._lvwPolicies_ColumnHeader_4.Name = ""
		Me._lvwPolicies_ColumnHeader_5.Name = ""
	End Sub
	Sub lvwLoyaltySchemes_InitializeColumnKeys()
		Me._lvwLoyaltySchemes_ColumnHeader_1.Name = ""
		Me._lvwLoyaltySchemes_ColumnHeader_2.Name = ""
		Me._lvwLoyaltySchemes_ColumnHeader_3.Name = ""
		Me._lvwLoyaltySchemes_ColumnHeader_4.Name = ""
	End Sub
	Sub lvwConvictions_InitializeColumnKeys()
		Me._lvwConvictions_ColumnHeader_1.Name = ""
		Me._lvwConvictions_ColumnHeader_2.Name = ""
		Me._lvwConvictions_ColumnHeader_3.Name = ""
		Me._lvwConvictions_ColumnHeader_4.Name = ""
		Me._lvwConvictions_ColumnHeader_5.Name = ""
	End Sub
	Sub lvwContacts_InitializeColumnKeys()
		Me._lvwContacts_ColumnHeader_1.Name = ""
		Me._lvwContacts_ColumnHeader_2.Name = ""
		Me._lvwContacts_ColumnHeader_3.Name = ""
		Me._lvwContacts_ColumnHeader_4.Name = ""
		Me._lvwContacts_ColumnHeader_5.Name = ""
	End Sub
	Sub lvwAddresses_InitializeColumnKeys()
		Me._lvwAddresses_ColumnHeader_1.Name = ""
		Me._lvwAddresses_ColumnHeader_2.Name = ""
		Me._lvwAddresses_ColumnHeader_3.Name = ""
		Me._lvwAddresses_ColumnHeader_4.Name = ""
		Me._lvwAddresses_ColumnHeader_5.Name = ""
		Me._lvwAddresses_ColumnHeader_6.Name = ""
    End Sub
    Friend WithEvents _lvwConvictions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents fraTab5_1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
    Friend WithEvents cboTurnover As PMLookupControl.cboPMLookup
    Friend WithEvents cboRenewalStopCode As System.Windows.Forms.ComboBox
    Friend WithEvents txtLoyaltyNumber As System.Windows.Forms.TextBox
    Friend WithEvents cboCurrency As UserControls.CurrencyLookup
    Friend WithEvents txtLoyaltyNumberPrefix As System.Windows.Forms.TextBox
    Friend WithEvents cboSeasonalGift As System.Windows.Forms.ComboBox
    Friend WithEvents cboCreditCard As System.Windows.Forms.ComboBox
    Friend WithEvents cboReminderType As System.Windows.Forms.ComboBox
    Friend WithEvents ddPaymentMethod As PMListMgrDropdown.uctDropdown
    Friend WithEvents lblTurnover As System.Windows.Forms.Label
    Friend WithEvents lblCreditCard As System.Windows.Forms.Label
    Friend WithEvents lblTermsOfPayment As System.Windows.Forms.Label
    Friend WithEvents lblSeasonalGift As System.Windows.Forms.Label
    Friend WithEvents lblRenewalStopCode As System.Windows.Forms.Label
    Friend WithEvents lblLoyaltyNumber As System.Windows.Forms.Label
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Friend WithEvents lblReminderType As System.Windows.Forms.Label
#End Region 
#Region "Upgrade Support"
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
