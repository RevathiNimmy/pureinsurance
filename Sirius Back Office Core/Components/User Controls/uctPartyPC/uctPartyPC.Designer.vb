<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyPCControl
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
		lvwLifestyle_InitializeColumnKeys()
		lvwConvictions_InitializeColumnKeys()
		lvwContacts_InitializeColumnKeys()
		lvwAddresses_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents _cmdPrevious_7 As System.Windows.Forms.Button
	Friend WithEvents txtFileCode As System.Windows.Forms.TextBox
	Friend WithEvents cboArea As System.Windows.Forms.ComboBox
	Friend WithEvents lblFileCode As System.Windows.Forms.Label
	Friend WithEvents lblArea As System.Windows.Forms.Label
	Friend WithEvents fraAreaCode As System.Windows.Forms.GroupBox
	Friend WithEvents cmdConsultantLookup As System.Windows.Forms.Button
	Friend WithEvents txtConsultantRef As System.Windows.Forms.TextBox
	Friend WithEvents PnlConsultantName As System.Windows.Forms.Label
	Friend WithEvents lblConsultantName As System.Windows.Forms.Label
	Friend WithEvents fraConsultant As System.Windows.Forms.GroupBox
	Friend WithEvents chkFeeClient As System.Windows.Forms.CheckBox
	Friend WithEvents chkProspect As System.Windows.Forms.CheckBox
	Friend WithEvents chkAgent As System.Windows.Forms.CheckBox
	Friend WithEvents pnlClientBalance As System.Windows.Forms.Label
	Friend WithEvents pnlLastYearTurnover As System.Windows.Forms.Label
	Friend WithEvents pnlYearToDateTurnover As System.Windows.Forms.Label
	Friend WithEvents lblClientBalance As System.Windows.Forms.Label
	Friend WithEvents lblLastYearTurnover As System.Windows.Forms.Label
	Friend WithEvents lblYearToDateTurnOver As System.Windows.Forms.Label
	Friend WithEvents fraAccounts As System.Windows.Forms.GroupBox
	Friend WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Friend WithEvents txtTradingName As System.Windows.Forms.TextBox
	Friend WithEvents txtSurname As System.Windows.Forms.TextBox
	Friend WithEvents txtForename As System.Windows.Forms.TextBox
	Friend WithEvents txtInitials As System.Windows.Forms.TextBox
	Friend WithEvents txtIDReference As System.Windows.Forms.TextBox
	Friend WithEvents ddTitle As PMListMgrDropdown.uctDropdown
	Friend WithEvents lblTradingName As System.Windows.Forms.Label
	Friend WithEvents lblSurname As System.Windows.Forms.Label
	Friend WithEvents lblForename As System.Windows.Forms.Label
	Friend WithEvents lblTitle As System.Windows.Forms.Label
	Friend WithEvents lblInitials As System.Windows.Forms.Label
	Friend WithEvents lblIDReference As System.Windows.Forms.Label
	Friend WithEvents fraClient As System.Windows.Forms.GroupBox
	Friend WithEvents cboServiceLevel As System.Windows.Forms.ComboBox
	Friend WithEvents txtAlternativeIdentifier As System.Windows.Forms.TextBox
	Friend WithEvents cboBranch As System.Windows.Forms.ComboBox
	Friend WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Friend WithEvents lblServicelevel As System.Windows.Forms.Label
	Friend WithEvents lblAlternativeIdentifier As System.Windows.Forms.Label
	Friend WithEvents lblBranch As System.Windows.Forms.Label
	Friend WithEvents lblSubBranch As System.Windows.Forms.Label
	Friend WithEvents Frame1 As System.Windows.Forms.GroupBox
	Friend WithEvents txtMembershipId As System.Windows.Forms.TextBox
	Friend WithEvents txtAgentRef As System.Windows.Forms.TextBox
	Friend WithEvents cmdAgentLookUp As System.Windows.Forms.Button
	Friend WithEvents pnlAgentName As System.Windows.Forms.Label
	Friend WithEvents lblMembershipId As System.Windows.Forms.Label
	Friend WithEvents lblAgentName As System.Windows.Forms.Label
	Friend WithEvents fraAgent As System.Windows.Forms.GroupBox
	Friend WithEvents cboBlackListReason As System.Windows.Forms.ComboBox
	Friend WithEvents lblBlacklistReason As System.Windows.Forms.Label
	Friend WithEvents fraBlackList As System.Windows.Forms.GroupBox
	Friend WithEvents _TabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents chkeMPS As System.Windows.Forms.CheckBox
	Friend WithEvents chkTPS As System.Windows.Forms.CheckBox
	Friend WithEvents txtSalutation As System.Windows.Forms.TextBox
	Friend WithEvents chkMailshot As System.Windows.Forms.CheckBox
	Friend WithEvents cboCorrespondenceType As System.Windows.Forms.ComboBox
	Friend WithEvents lblTPS As System.Windows.Forms.Label
	Friend WithEvents lblSalutation As System.Windows.Forms.Label
	Friend WithEvents lblPreferredCorrespondence As System.Windows.Forms.Label
	Friend WithEvents fraCorrespondence As System.Windows.Forms.GroupBox
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
	Friend WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
	Friend WithEvents _TabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Friend WithEvents cmdAssociates As System.Windows.Forms.Button
	Friend WithEvents _cmdNext_2 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Friend WithEvents txtSource As System.Windows.Forms.TextBox
	Friend WithEvents cboRenewalStopCode As System.Windows.Forms.ComboBox
	Friend WithEvents cboReminderType As System.Windows.Forms.ComboBox
	Friend WithEvents cboCreditCard As System.Windows.Forms.ComboBox
	Friend WithEvents ddPaymentMethod As PMListMgrDropdown.uctDropdown
    Friend WithEvents cboCurrency As UserControls.CurrencyLookup
	Friend WithEvents lblSource As System.Windows.Forms.Label
	Friend WithEvents lblRenewalStopCode As System.Windows.Forms.Label
	Friend WithEvents lblReminderType As System.Windows.Forms.Label
	Friend WithEvents lblCreditCard As System.Windows.Forms.Label
	Friend WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
	Friend WithEvents fraPaymentDetails As System.Windows.Forms.GroupBox
	Friend WithEvents ddBusiness As PMListMgrDropdown.uctDropdown
	Friend WithEvents ddOccupation As PMListMgrDropdown.uctDropdown
	Friend WithEvents ddSecondaryBusiness As PMListMgrDropdown.uctDropdown
	Friend WithEvents ddSecondaryOccupation As PMListMgrDropdown.uctDropdown
	Friend WithEvents ddEmployment As PMListMgrDropdown.uctDropdown
	Friend WithEvents ddSecEmploymentStatus As PMListMgrDropdown.uctDropdown
	Friend WithEvents lblEmployer As System.Windows.Forms.Label
	Friend WithEvents lblOccupation As System.Windows.Forms.Label
	Friend WithEvents lblEmploymentStatus As System.Windows.Forms.Label
	Friend WithEvents lblSecOccupation As System.Windows.Forms.Label
	Friend WithEvents lblSecEmployer As System.Windows.Forms.Label
	Friend WithEvents lblSecEmploymentStatus As System.Windows.Forms.Label
	Friend WithEvents fraEmploymentDetails As System.Windows.Forms.GroupBox
	Friend WithEvents txtTobLetter As System.Windows.Forms.TextBox
	Friend WithEvents lblTobLetter As System.Windows.Forms.Label
	Friend WithEvents fraFSA As System.Windows.Forms.GroupBox
	Friend WithEvents _TabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Friend WithEvents txtCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtDate As System.Windows.Forms.TextBox
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
	Friend WithEvents _lvwConvictions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwConvictions As System.Windows.Forms.ListView
	Friend WithEvents frmContacts As System.Windows.Forms.GroupBox
	Friend WithEvents lblCCJ As System.Windows.Forms.Label
	Friend WithEvents _TabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdNext_4 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
	Friend WithEvents cboSeasonalGift As System.Windows.Forms.ComboBox
	Friend WithEvents txtTPPassword As System.Windows.Forms.TextBox
	Friend WithEvents txtLoyaltyNumberPrefix As System.Windows.Forms.TextBox
	Friend WithEvents txtLoyaltyNumber As System.Windows.Forms.TextBox
	Friend WithEvents ddGender As PMListMgrDropdown.uctDropdown
	Friend WithEvents cboAccommodation As System.Windows.Forms.ComboBox
	Friend WithEvents txtDOB As System.Windows.Forms.TextBox
	Friend WithEvents cboNationality As System.Windows.Forms.ComboBox
	Friend WithEvents chkPets As System.Windows.Forms.CheckBox
	Friend WithEvents chkSmoker As System.Windows.Forms.CheckBox
	Friend WithEvents ddMaritalStatus As PMListMgrDropdown.uctDropdown
	Friend WithEvents lblTPPassword As System.Windows.Forms.Label
	Friend WithEvents lblLoyaltyNumber As System.Windows.Forms.Label
	Friend WithEvents lblSeasonalGift As System.Windows.Forms.Label
	Friend WithEvents lblMaritalStatus As System.Windows.Forms.Label
	Friend WithEvents lblAccommodation As System.Windows.Forms.Label
	Friend WithEvents lblGender As System.Windows.Forms.Label
	Friend WithEvents lblDOB As System.Windows.Forms.Label
	Friend WithEvents lblNationality As System.Windows.Forms.Label
	Friend WithEvents lblSmoker As System.Windows.Forms.Label
	Friend WithEvents fraLifestyle As System.Windows.Forms.GroupBox
	Friend WithEvents cmdAddLife As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteLife As System.Windows.Forms.Button
	Friend WithEvents cmdEditLife As System.Windows.Forms.Button
	Friend WithEvents _lvwLifestyle_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLifestyle_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwLifestyle As System.Windows.Forms.ListView
	Friend WithEvents fraDependants As System.Windows.Forms.GroupBox
	Friend WithEvents _TabMainTab_TabPage4 As System.Windows.Forms.TabPage
	Friend WithEvents _cmdNext_5 As System.Windows.Forms.Button
	Friend WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
	Friend WithEvents cmdEditLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents cmdDeleteLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents cmdAddLoyaltyScheme As System.Windows.Forms.Button
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwLoyaltySchemes As System.Windows.Forms.ListView
	Friend WithEvents fraLoyaltySchemes As System.Windows.Forms.GroupBox
	Friend WithEvents _TabMainTab_TabPage5 As System.Windows.Forms.TabPage
	Friend WithEvents cmdEditPol As System.Windows.Forms.Button
	Friend WithEvents cmdDeletePol As System.Windows.Forms.Button
	Friend WithEvents cmdAddPol As System.Windows.Forms.Button
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
	Friend WithEvents cmdCurrentAgent As System.Windows.Forms.Button
	Friend WithEvents txtAgentReference As System.Windows.Forms.TextBox
	Friend WithEvents cboProspectingStatus As System.Windows.Forms.ComboBox
	Friend WithEvents cboStrengthCode As System.Windows.Forms.ComboBox
	Friend WithEvents pnlCurrentAgent As System.Windows.Forms.Label
	Friend WithEvents lblAgentReference As System.Windows.Forms.Label
	Friend WithEvents lblProspectStatus As System.Windows.Forms.Label
	Friend WithEvents lblStrengthCode As System.Windows.Forms.Label
	Friend WithEvents fraProspect As System.Windows.Forms.GroupBox
	Friend WithEvents cmdInsurerLookup As System.Windows.Forms.Button
	Friend WithEvents txtInsurerRef As System.Windows.Forms.TextBox
	Friend WithEvents pnlInsurerName As System.Windows.Forms.Label
	Friend WithEvents lblInsurerName As System.Windows.Forms.Label
	Friend WithEvents fraPreviousInsurer As System.Windows.Forms.GroupBox
	Friend WithEvents txtBrokerRef As System.Windows.Forms.TextBox
	Friend WithEvents cmdBrokerLookup As System.Windows.Forms.Button
	Friend WithEvents pnlBrokerName As System.Windows.Forms.Label
	Friend WithEvents lblBrokerName As System.Windows.Forms.Label
	Friend WithEvents fraPreviousBroker As System.Windows.Forms.GroupBox
	Friend WithEvents cboPolicyType As System.Windows.Forms.ComboBox
	Friend WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
	Friend WithEvents _cmdNext_6 As System.Windows.Forms.Button
	Friend WithEvents _TabMainTab_TabPage6 As System.Windows.Forms.TabPage
	Friend WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
	Friend WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
	Friend WithEvents _cmdNext_7 As System.Windows.Forms.Button
	Friend WithEvents _TabMainTab_TabPage7 As System.Windows.Forms.TabPage
	Friend WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
	Friend WithEvents _TabMainTab_TabPage8 As System.Windows.Forms.TabPage
	Friend WithEvents TabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
	Friend cmdNext(7) As System.Windows.Forms.Button
	Friend cmdPrevious(7) As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartyPCControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabMainTab = New System.Windows.Forms.TabControl()
        Me._TabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblAssociates = New System.Windows.Forms.Label()
        Me.fraAreaCode = New System.Windows.Forms.GroupBox()
        Me.txtFileCode = New System.Windows.Forms.TextBox()
        Me.cboArea = New System.Windows.Forms.ComboBox()
        Me.lblFileCode = New System.Windows.Forms.Label()
        Me.lblArea = New System.Windows.Forms.Label()
        Me.fraConsultant = New System.Windows.Forms.GroupBox()
        Me.cmdConsultantLookup = New System.Windows.Forms.Button()
        Me.txtConsultantRef = New System.Windows.Forms.TextBox()
        Me.PnlConsultantName = New System.Windows.Forms.Label()
        Me.lblConsultantName = New System.Windows.Forms.Label()
        Me.fraAccounts = New System.Windows.Forms.GroupBox()
        Me.chkFeeClient = New System.Windows.Forms.CheckBox()
        Me.chkProspect = New System.Windows.Forms.CheckBox()
        Me.chkAgent = New System.Windows.Forms.CheckBox()
        Me.pnlClientBalance = New System.Windows.Forms.Label()
        Me.pnlLastYearTurnover = New System.Windows.Forms.Label()
        Me.pnlYearToDateTurnover = New System.Windows.Forms.Label()
        Me.lblClientBalance = New System.Windows.Forms.Label()
        Me.lblLastYearTurnover = New System.Windows.Forms.Label()
        Me.lblYearToDateTurnOver = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me.fraClient = New System.Windows.Forms.GroupBox()
        Me.txtTradingName = New System.Windows.Forms.TextBox()
        Me.txtSurname = New System.Windows.Forms.TextBox()
        Me.txtForename = New System.Windows.Forms.TextBox()
        Me.txtInitials = New System.Windows.Forms.TextBox()
        Me.txtIDReference = New System.Windows.Forms.TextBox()
        Me.ddTitle = New PMListMgrDropdown.uctDropdown()
        Me.lblTradingName = New System.Windows.Forms.Label()
        Me.lblSurname = New System.Windows.Forms.Label()
        Me.lblForename = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblInitials = New System.Windows.Forms.Label()
        Me.lblIDReference = New System.Windows.Forms.Label()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.cboServiceLevel = New System.Windows.Forms.ComboBox()
        Me.txtAlternativeIdentifier = New System.Windows.Forms.TextBox()
        Me.cboBranch = New System.Windows.Forms.ComboBox()
        Me.cboSubBranch = New System.Windows.Forms.ComboBox()
        Me.lblServicelevel = New System.Windows.Forms.Label()
        Me.lblAlternativeIdentifier = New System.Windows.Forms.Label()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.lblSubBranch = New System.Windows.Forms.Label()
        Me.fraAgent = New System.Windows.Forms.GroupBox()
        Me.txtMembershipId = New System.Windows.Forms.TextBox()
        Me.txtAgentRef = New System.Windows.Forms.TextBox()
        Me.cmdAgentLookUp = New System.Windows.Forms.Button()
        Me.pnlAgentName = New System.Windows.Forms.Label()
        Me.lblMembershipId = New System.Windows.Forms.Label()
        Me.lblAgentName = New System.Windows.Forms.Label()
        Me.fraBlackList = New System.Windows.Forms.GroupBox()
        Me.cboBlackListReason = New System.Windows.Forms.ComboBox()
        Me.lblBlacklistReason = New System.Windows.Forms.Label()
        Me._TabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraCorrespondence = New System.Windows.Forms.GroupBox()
        Me.chkeMPS = New System.Windows.Forms.CheckBox()
        Me.chkTPS = New System.Windows.Forms.CheckBox()
        Me.txtSalutation = New System.Windows.Forms.TextBox()
        Me.chkMailshot = New System.Windows.Forms.CheckBox()
        Me.cboCorrespondenceType = New System.Windows.Forms.ComboBox()
        Me.lblTPS = New System.Windows.Forms.Label()
        Me.lblSalutation = New System.Windows.Forms.Label()
        Me.lblPreferredCorrespondence = New System.Windows.Forms.Label()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me.fraAddress = New System.Windows.Forms.GroupBox()
        Me.cmdEditAd = New System.Windows.Forms.Button()
        Me.cmdDeleteAd = New System.Windows.Forms.Button()
        Me.cmdAddAd = New System.Windows.Forms.Button()
        Me.lvwAddresses = New System.Windows.Forms.ListView()
        Me._lvwAddresses_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddresses_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddresses_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddresses_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddresses_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddresses_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.fraContact = New System.Windows.Forms.GroupBox()
        Me.cmdEditCon = New System.Windows.Forms.Button()
        Me.cmdDeleteCon = New System.Windows.Forms.Button()
        Me.cmdAddCon = New System.Windows.Forms.Button()
        Me.lvwContacts = New System.Windows.Forms.ListView()
        Me._lvwContacts_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContacts_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContacts_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContacts_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContacts_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me._TabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.cmdAssociates = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me.fraPaymentDetails = New System.Windows.Forms.GroupBox()
        Me.lblTermsOfPayment = New System.Windows.Forms.Label()
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox()
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.cboRenewalStopCode = New System.Windows.Forms.ComboBox()
        Me.cboReminderType = New System.Windows.Forms.ComboBox()
        Me.cboCreditCard = New System.Windows.Forms.ComboBox()
        Me.ddPaymentMethod = New PMListMgrDropdown.uctDropdown()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.lblRenewalStopCode = New System.Windows.Forms.Label()
        Me.lblReminderType = New System.Windows.Forms.Label()
        Me.lblCreditCard = New System.Windows.Forms.Label()
        Me.lblPaymentMethod = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.fraEmploymentDetails = New System.Windows.Forms.GroupBox()
        Me.ddBusiness = New PMListMgrDropdown.uctDropdown()
        Me.ddOccupation = New PMListMgrDropdown.uctDropdown()
        Me.ddSecondaryBusiness = New PMListMgrDropdown.uctDropdown()
        Me.ddSecondaryOccupation = New PMListMgrDropdown.uctDropdown()
        Me.ddEmployment = New PMListMgrDropdown.uctDropdown()
        Me.ddSecEmploymentStatus = New PMListMgrDropdown.uctDropdown()
        Me.lblEmployer = New System.Windows.Forms.Label()
        Me.lblOccupation = New System.Windows.Forms.Label()
        Me.lblEmploymentStatus = New System.Windows.Forms.Label()
        Me.lblSecOccupation = New System.Windows.Forms.Label()
        Me.lblSecEmployer = New System.Windows.Forms.Label()
        Me.lblSecEmploymentStatus = New System.Windows.Forms.Label()
        Me.fraFSA = New System.Windows.Forms.GroupBox()
        Me.txtTobLetter = New System.Windows.Forms.TextBox()
        Me.lblTobLetter = New System.Windows.Forms.Label()
        Me._TabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.txtCurrency = New System.Windows.Forms.TextBox()
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me.txtCCJ = New System.Windows.Forms.TextBox()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me.frmContacts = New System.Windows.Forms.GroupBox()
        Me.cmdAddConv = New System.Windows.Forms.Button()
        Me.cmdDeleteConv = New System.Windows.Forms.Button()
        Me.cmdEditConv = New System.Windows.Forms.Button()
        Me.lvwConvictions = New System.Windows.Forms.ListView()
        Me._lvwConvictions_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwConvictions_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwConvictions_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwConvictions_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwConvictions_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwConvictions_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblCCJ = New System.Windows.Forms.Label()
        Me._TabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me._cmdNext_4 = New System.Windows.Forms.Button()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
        Me.fraLifestyle = New System.Windows.Forms.GroupBox()
        Me.cboSeasonalGift = New System.Windows.Forms.ComboBox()
        Me.txtTPPassword = New System.Windows.Forms.TextBox()
        Me.txtLoyaltyNumberPrefix = New System.Windows.Forms.TextBox()
        Me.txtLoyaltyNumber = New System.Windows.Forms.TextBox()
        Me.ddGender = New PMListMgrDropdown.uctDropdown()
        Me.cboAccommodation = New System.Windows.Forms.ComboBox()
        Me.txtDOB = New System.Windows.Forms.TextBox()
        Me.cboNationality = New System.Windows.Forms.ComboBox()
        Me.chkPets = New System.Windows.Forms.CheckBox()
        Me.chkSmoker = New System.Windows.Forms.CheckBox()
        Me.ddMaritalStatus = New PMListMgrDropdown.uctDropdown()
        Me.lblTPPassword = New System.Windows.Forms.Label()
        Me.lblLoyaltyNumber = New System.Windows.Forms.Label()
        Me.lblSeasonalGift = New System.Windows.Forms.Label()
        Me.lblMaritalStatus = New System.Windows.Forms.Label()
        Me.lblAccommodation = New System.Windows.Forms.Label()
        Me.lblGender = New System.Windows.Forms.Label()
        Me.lblDOB = New System.Windows.Forms.Label()
        Me.lblNationality = New System.Windows.Forms.Label()
        Me.lblSmoker = New System.Windows.Forms.Label()
        Me.fraDependants = New System.Windows.Forms.GroupBox()
        Me.cmdAddLife = New System.Windows.Forms.Button()
        Me.cmdDeleteLife = New System.Windows.Forms.Button()
        Me.cmdEditLife = New System.Windows.Forms.Button()
        Me.lvwLifestyle = New System.Windows.Forms.ListView()
        Me._lvwLifestyle_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLifestyle_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._TabMainTab_TabPage5 = New System.Windows.Forms.TabPage()
        Me._cmdNext_5 = New System.Windows.Forms.Button()
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me.fraLoyaltySchemes = New System.Windows.Forms.GroupBox()
        Me.cmdEditLoyaltyScheme = New System.Windows.Forms.Button()
        Me.cmdDeleteLoyaltyScheme = New System.Windows.Forms.Button()
        Me.cmdAddLoyaltyScheme = New System.Windows.Forms.Button()
        Me.lvwLoyaltySchemes = New System.Windows.Forms.ListView()
        Me._lvwLoyaltySchemes_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._TabMainTab_TabPage6 = New System.Windows.Forms.TabPage()
        Me.fraPolicies = New System.Windows.Forms.GroupBox()
        Me.cmdEditPol = New System.Windows.Forms.Button()
        Me.cmdDeletePol = New System.Windows.Forms.Button()
        Me.cmdAddPol = New System.Windows.Forms.Button()
        Me.lvwPolicies = New System.Windows.Forms.ListView()
        Me._lvwPolicies_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraCampaign = New System.Windows.Forms.GroupBox()
        Me.lvwCampaigns = New System.Windows.Forms.ListView()
        Me._lvwCampaigns_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCampaigns_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCampaigns_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraProspect = New System.Windows.Forms.GroupBox()
        Me.cmdCurrentAgent = New System.Windows.Forms.Button()
        Me.txtAgentReference = New System.Windows.Forms.TextBox()
        Me.cboProspectingStatus = New System.Windows.Forms.ComboBox()
        Me.cboStrengthCode = New System.Windows.Forms.ComboBox()
        Me.pnlCurrentAgent = New System.Windows.Forms.Label()
        Me.lblAgentReference = New System.Windows.Forms.Label()
        Me.lblProspectStatus = New System.Windows.Forms.Label()
        Me.lblStrengthCode = New System.Windows.Forms.Label()
        Me.fraPreviousInsurer = New System.Windows.Forms.GroupBox()
        Me.cmdInsurerLookup = New System.Windows.Forms.Button()
        Me.txtInsurerRef = New System.Windows.Forms.TextBox()
        Me.pnlInsurerName = New System.Windows.Forms.Label()
        Me.lblInsurerName = New System.Windows.Forms.Label()
        Me.fraPreviousBroker = New System.Windows.Forms.GroupBox()
        Me.txtBrokerRef = New System.Windows.Forms.TextBox()
        Me.cmdBrokerLookup = New System.Windows.Forms.Button()
        Me.pnlBrokerName = New System.Windows.Forms.Label()
        Me.lblBrokerName = New System.Windows.Forms.Label()
        Me.cboPolicyType = New System.Windows.Forms.ComboBox()
        Me._TabMainTab_TabPage7 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax()
        Me._cmdPrevious_5 = New System.Windows.Forms.Button()
        Me._cmdNext_6 = New System.Windows.Forms.Button()
        Me._TabMainTab_TabPage8 = New System.Windows.Forms.TabPage()
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl()
        Me._cmdPrevious_6 = New System.Windows.Forms.Button()
        Me._cmdNext_7 = New System.Windows.Forms.Button()
        Me._cmdPrevious_7 = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.TabMainTab.SuspendLayout()
        Me._TabMainTab_TabPage0.SuspendLayout()
        Me.fraAreaCode.SuspendLayout()
        Me.fraConsultant.SuspendLayout()
        Me.fraAccounts.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraAgent.SuspendLayout()
        Me.fraBlackList.SuspendLayout()
        Me._TabMainTab_TabPage1.SuspendLayout()
        Me.fraCorrespondence.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._TabMainTab_TabPage2.SuspendLayout()
        Me.fraPaymentDetails.SuspendLayout()
        Me.fraEmploymentDetails.SuspendLayout()
        Me.fraFSA.SuspendLayout()
        Me._TabMainTab_TabPage3.SuspendLayout()
        Me.frmContacts.SuspendLayout()
        Me._TabMainTab_TabPage4.SuspendLayout()
        Me.fraLifestyle.SuspendLayout()
        Me.fraDependants.SuspendLayout()
        Me._TabMainTab_TabPage5.SuspendLayout()
        Me.fraLoyaltySchemes.SuspendLayout()
        Me._TabMainTab_TabPage6.SuspendLayout()
        Me.fraPolicies.SuspendLayout()
        Me.fraCampaign.SuspendLayout()
        Me.fraProspect.SuspendLayout()
        Me.fraPreviousInsurer.SuspendLayout()
        Me.fraPreviousBroker.SuspendLayout()
        Me._TabMainTab_TabPage7.SuspendLayout()
        Me._TabMainTab_TabPage8.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabMainTab
        '
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage0)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage1)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage2)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage3)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage4)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage5)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage6)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage7)
        Me.TabMainTab.Controls.Add(Me._TabMainTab_TabPage8)
        Me.TabMainTab.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabMainTab.ItemSize = New System.Drawing.Size(78, 18)
        Me.TabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.TabMainTab.Multiline = True
        Me.TabMainTab.Name = "TabMainTab"
        Me.TabMainTab.SelectedIndex = 0
        Me.TabMainTab.Size = New System.Drawing.Size(718, 435)
        Me.TabMainTab.TabIndex = 0
        '
        '_TabMainTab_TabPage0
        '
        Me._TabMainTab_TabPage0.Controls.Add(Me.lblAssociates)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraAreaCode)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraConsultant)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraAccounts)
        Me._TabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraClient)
        Me._TabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraAgent)
        Me._TabMainTab_TabPage0.Controls.Add(Me.fraBlackList)
        Me._TabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage0.Name = "_TabMainTab_TabPage0"
        Me._TabMainTab_TabPage0.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage0.TabIndex = 0
        Me._TabMainTab_TabPage0.Text = "1 - Client"
        Me._TabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblAssociates
        '
        Me.lblAssociates.AutoSize = True
        Me.lblAssociates.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAssociates.ForeColor = System.Drawing.Color.Red
        Me.lblAssociates.Location = New System.Drawing.Point(8, 373)
        Me.lblAssociates.Name = "lblAssociates"
        Me.lblAssociates.Size = New System.Drawing.Size(410, 18)
        Me.lblAssociates.TabIndex = 1
        Me.lblAssociates.Text = "An Associated Client is attached to this record"
        Me.lblAssociates.Visible = False
        '
        'fraAreaCode
        '
        Me.fraAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.fraAreaCode.Controls.Add(Me.txtFileCode)
        Me.fraAreaCode.Controls.Add(Me.cboArea)
        Me.fraAreaCode.Controls.Add(Me.lblFileCode)
        Me.fraAreaCode.Controls.Add(Me.lblArea)
        Me.fraAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAreaCode.Location = New System.Drawing.Point(8, 300)
        Me.fraAreaCode.Name = "fraAreaCode"
        Me.fraAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAreaCode.Size = New System.Drawing.Size(289, 70)
        Me.fraAreaCode.TabIndex = 2
        Me.fraAreaCode.TabStop = False
        '
        'txtFileCode
        '
        Me.txtFileCode.AcceptsReturn = True
        Me.txtFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileCode.Location = New System.Drawing.Point(104, 40)
        Me.txtFileCode.MaxLength = 0
        Me.txtFileCode.Name = "txtFileCode"
        Me.txtFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileCode.Size = New System.Drawing.Size(177, 21)
        Me.txtFileCode.TabIndex = 11
        '
        'cboArea
        '
        Me.cboArea.BackColor = System.Drawing.SystemColors.Window
        Me.cboArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboArea.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboArea.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboArea.Location = New System.Drawing.Point(104, 16)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboArea.Size = New System.Drawing.Size(177, 21)
        Me.cboArea.TabIndex = 10
        '
        'lblFileCode
        '
        Me.lblFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileCode.Location = New System.Drawing.Point(8, 43)
        Me.lblFileCode.Name = "lblFileCode"
        Me.lblFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileCode.Size = New System.Drawing.Size(65, 17)
        Me.lblFileCode.TabIndex = 101
        Me.lblFileCode.Text = "Filecode:"
        '
        'lblArea
        '
        Me.lblArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArea.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArea.Location = New System.Drawing.Point(8, 19)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArea.Size = New System.Drawing.Size(57, 17)
        Me.lblArea.TabIndex = 100
        Me.lblArea.Text = "Area:"
        '
        'fraConsultant
        '
        Me.fraConsultant.BackColor = System.Drawing.SystemColors.Control
        Me.fraConsultant.Controls.Add(Me.cmdConsultantLookup)
        Me.fraConsultant.Controls.Add(Me.txtConsultantRef)
        Me.fraConsultant.Controls.Add(Me.PnlConsultantName)
        Me.fraConsultant.Controls.Add(Me.lblConsultantName)
        Me.fraConsultant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraConsultant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraConsultant.Location = New System.Drawing.Point(304, 300)
        Me.fraConsultant.Name = "fraConsultant"
        Me.fraConsultant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraConsultant.Size = New System.Drawing.Size(313, 70)
        Me.fraConsultant.TabIndex = 5
        Me.fraConsultant.TabStop = False
        Me.fraConsultant.Text = "Consultant"
        '
        'cmdConsultantLookup
        '
        Me.cmdConsultantLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConsultantLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConsultantLookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConsultantLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConsultantLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdConsultantLookup.Name = "cmdConsultantLookup"
        Me.cmdConsultantLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConsultantLookup.Size = New System.Drawing.Size(57, 20)
        Me.cmdConsultantLookup.TabIndex = 17
        Me.cmdConsultantLookup.Text = "Code..."
        Me.cmdConsultantLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConsultantLookup.UseVisualStyleBackColor = False
        '
        'txtConsultantRef
        '
        Me.txtConsultantRef.AcceptsReturn = True
        Me.txtConsultantRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtConsultantRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConsultantRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsultantRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConsultantRef.Location = New System.Drawing.Point(72, 16)
        Me.txtConsultantRef.MaxLength = 0
        Me.txtConsultantRef.Name = "txtConsultantRef"
        Me.txtConsultantRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConsultantRef.Size = New System.Drawing.Size(233, 21)
        Me.txtConsultantRef.TabIndex = 18
        '
        'PnlConsultantName
        '
        Me.PnlConsultantName.BackColor = System.Drawing.SystemColors.Control
        Me.PnlConsultantName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlConsultantName.Cursor = System.Windows.Forms.Cursors.Default
        Me.PnlConsultantName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlConsultantName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PnlConsultantName.Location = New System.Drawing.Point(72, 40)
        Me.PnlConsultantName.Name = "PnlConsultantName"
        Me.PnlConsultantName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PnlConsultantName.Size = New System.Drawing.Size(233, 17)
        Me.PnlConsultantName.TabIndex = 133
        '
        'lblConsultantName
        '
        Me.lblConsultantName.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsultantName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsultantName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsultantName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsultantName.Location = New System.Drawing.Point(8, 40)
        Me.lblConsultantName.Name = "lblConsultantName"
        Me.lblConsultantName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsultantName.Size = New System.Drawing.Size(65, 13)
        Me.lblConsultantName.TabIndex = 98
        Me.lblConsultantName.Text = "Name:"
        '
        'fraAccounts
        '
        Me.fraAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccounts.Controls.Add(Me.chkFeeClient)
        Me.fraAccounts.Controls.Add(Me.chkProspect)
        Me.fraAccounts.Controls.Add(Me.chkAgent)
        Me.fraAccounts.Controls.Add(Me.pnlClientBalance)
        Me.fraAccounts.Controls.Add(Me.pnlLastYearTurnover)
        Me.fraAccounts.Controls.Add(Me.pnlYearToDateTurnover)
        Me.fraAccounts.Controls.Add(Me.lblClientBalance)
        Me.fraAccounts.Controls.Add(Me.lblLastYearTurnover)
        Me.fraAccounts.Controls.Add(Me.lblYearToDateTurnOver)
        Me.fraAccounts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccounts.Location = New System.Drawing.Point(304, 4)
        Me.fraAccounts.Name = "fraAccounts"
        Me.fraAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccounts.Size = New System.Drawing.Size(313, 166)
        Me.fraAccounts.TabIndex = 3
        Me.fraAccounts.TabStop = False
        '
        'chkFeeClient
        '
        Me.chkFeeClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkFeeClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFeeClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFeeClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFeeClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFeeClient.Location = New System.Drawing.Point(208, 136)
        Me.chkFeeClient.Name = "chkFeeClient"
        Me.chkFeeClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFeeClient.Size = New System.Drawing.Size(97, 17)
        Me.chkFeeClient.TabIndex = 185
        Me.chkFeeClient.Text = "Fee Client:"
        Me.chkFeeClient.UseVisualStyleBackColor = False
        '
        'chkProspect
        '
        Me.chkProspect.BackColor = System.Drawing.SystemColors.Control
        Me.chkProspect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProspect.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProspect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProspect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProspect.Location = New System.Drawing.Point(8, 136)
        Me.chkProspect.Name = "chkProspect"
        Me.chkProspect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProspect.Size = New System.Drawing.Size(105, 17)
        Me.chkProspect.TabIndex = 12
        Me.chkProspect.Text = "Is Prospect ?"
        Me.chkProspect.UseVisualStyleBackColor = False
        '
        'chkAgent
        '
        Me.chkAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgent.Location = New System.Drawing.Point(120, 136)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgent.Size = New System.Drawing.Size(85, 17)
        Me.chkAgent.TabIndex = 13
        Me.chkAgent.Text = "Is Agent ?"
        Me.chkAgent.UseVisualStyleBackColor = False
        '
        'pnlClientBalance
        '
        Me.pnlClientBalance.BackColor = System.Drawing.SystemColors.Control
        Me.pnlClientBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClientBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlClientBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClientBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlClientBalance.Location = New System.Drawing.Point(8, 32)
        Me.pnlClientBalance.Name = "pnlClientBalance"
        Me.pnlClientBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlClientBalance.Size = New System.Drawing.Size(297, 19)
        Me.pnlClientBalance.TabIndex = 144
        Me.pnlClientBalance.Text = " "
        Me.pnlClientBalance.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlLastYearTurnover
        '
        Me.pnlLastYearTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.pnlLastYearTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLastYearTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlLastYearTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLastYearTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlLastYearTurnover.Location = New System.Drawing.Point(8, 112)
        Me.pnlLastYearTurnover.Name = "pnlLastYearTurnover"
        Me.pnlLastYearTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlLastYearTurnover.Size = New System.Drawing.Size(297, 19)
        Me.pnlLastYearTurnover.TabIndex = 142
        Me.pnlLastYearTurnover.Text = " "
        Me.pnlLastYearTurnover.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlYearToDateTurnover
        '
        Me.pnlYearToDateTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.pnlYearToDateTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlYearToDateTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlYearToDateTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlYearToDateTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlYearToDateTurnover.Location = New System.Drawing.Point(8, 72)
        Me.pnlYearToDateTurnover.Name = "pnlYearToDateTurnover"
        Me.pnlYearToDateTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlYearToDateTurnover.Size = New System.Drawing.Size(297, 19)
        Me.pnlYearToDateTurnover.TabIndex = 141
        Me.pnlYearToDateTurnover.Text = " "
        Me.pnlYearToDateTurnover.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblClientBalance
        '
        Me.lblClientBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientBalance.Location = New System.Drawing.Point(8, 16)
        Me.lblClientBalance.Name = "lblClientBalance"
        Me.lblClientBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientBalance.Size = New System.Drawing.Size(105, 17)
        Me.lblClientBalance.TabIndex = 143
        Me.lblClientBalance.Text = "Account Balance:"
        '
        'lblLastYearTurnover
        '
        Me.lblLastYearTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastYearTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastYearTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastYearTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastYearTurnover.Location = New System.Drawing.Point(8, 96)
        Me.lblLastYearTurnover.Name = "lblLastYearTurnover"
        Me.lblLastYearTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastYearTurnover.Size = New System.Drawing.Size(249, 13)
        Me.lblLastYearTurnover.TabIndex = 140
        Me.lblLastYearTurnover.Text = "Last Year Turnover:"
        '
        'lblYearToDateTurnOver
        '
        Me.lblYearToDateTurnOver.BackColor = System.Drawing.SystemColors.Control
        Me.lblYearToDateTurnOver.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYearToDateTurnOver.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYearToDateTurnOver.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYearToDateTurnOver.Location = New System.Drawing.Point(8, 56)
        Me.lblYearToDateTurnOver.Name = "lblYearToDateTurnOver"
        Me.lblYearToDateTurnOver.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYearToDateTurnOver.Size = New System.Drawing.Size(153, 17)
        Me.lblYearToDateTurnOver.TabIndex = 139
        Me.lblYearToDateTurnOver.Text = "Year to Date Turnover:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 19
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.txtTradingName)
        Me.fraClient.Controls.Add(Me.txtSurname)
        Me.fraClient.Controls.Add(Me.txtForename)
        Me.fraClient.Controls.Add(Me.txtInitials)
        Me.fraClient.Controls.Add(Me.txtIDReference)
        Me.fraClient.Controls.Add(Me.ddTitle)
        Me.fraClient.Controls.Add(Me.lblTradingName)
        Me.fraClient.Controls.Add(Me.lblSurname)
        Me.fraClient.Controls.Add(Me.lblForename)
        Me.fraClient.Controls.Add(Me.lblTitle)
        Me.fraClient.Controls.Add(Me.lblInitials)
        Me.fraClient.Controls.Add(Me.lblIDReference)
        Me.fraClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(8, 4)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(289, 167)
        Me.fraClient.TabIndex = 0
        Me.fraClient.TabStop = False
        '
        'txtTradingName
        '
        Me.txtTradingName.AcceptsReturn = True
        Me.txtTradingName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTradingName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTradingName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTradingName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTradingName.Location = New System.Drawing.Point(104, 135)
        Me.txtTradingName.MaxLength = 255
        Me.txtTradingName.Name = "txtTradingName"
        Me.txtTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTradingName.Size = New System.Drawing.Size(165, 21)
        Me.txtTradingName.TabIndex = 5
        '
        'txtSurname
        '
        Me.txtSurname.AcceptsReturn = True
        Me.txtSurname.BackColor = System.Drawing.SystemColors.Window
        Me.txtSurname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSurname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSurname.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSurname.Location = New System.Drawing.Point(104, 40)
        Me.txtSurname.MaxLength = 0
        Me.txtSurname.Name = "txtSurname"
        Me.txtSurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSurname.Size = New System.Drawing.Size(165, 21)
        Me.txtSurname.TabIndex = 1
        '
        'txtForename
        '
        Me.txtForename.AcceptsReturn = True
        Me.txtForename.BackColor = System.Drawing.SystemColors.Window
        Me.txtForename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtForename.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtForename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtForename.Location = New System.Drawing.Point(104, 64)
        Me.txtForename.MaxLength = 0
        Me.txtForename.Name = "txtForename"
        Me.txtForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtForename.Size = New System.Drawing.Size(165, 21)
        Me.txtForename.TabIndex = 2
        '
        'txtInitials
        '
        Me.txtInitials.AcceptsReturn = True
        Me.txtInitials.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitials.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitials.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitials.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitials.Location = New System.Drawing.Point(104, 112)
        Me.txtInitials.MaxLength = 0
        Me.txtInitials.Name = "txtInitials"
        Me.txtInitials.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitials.Size = New System.Drawing.Size(49, 21)
        Me.txtInitials.TabIndex = 4
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(104, 16)
        Me.txtIDReference.MaxLength = 20
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(165, 21)
        Me.txtIDReference.TabIndex = 0
        '
        'ddTitle
        '
        Me.ddTitle.AllowAbiCodeEntry = False
        Me.ddTitle.AutoCompleteText = True
        Me.ddTitle.DataModel = "GIIM"
        Me.ddTitle.ListIndex = -1
        Me.ddTitle.ListManager = Nothing
        Me.ddTitle.Location = New System.Drawing.Point(104, 88)
        Me.ddTitle.Login = False
        Me.ddTitle.LongList = False
        Me.ddTitle.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddTitle.Name = "ddTitle"
        Me.ddTitle.PropertyId = "131085"
        Me.ddTitle.ReadOnly_Renamed = False
        Me.ddTitle.SelLength = 0
        Me.ddTitle.SelStart = 0
        Me.ddTitle.SelText = ""
        Me.ddTitle.Size = New System.Drawing.Size(165, 21)
        Me.ddTitle.TabIndex = 3
        Me.ddTitle.ToolTipText = ""
        Me.ddTitle.VehicleListId = ""
        Me.ddTitle.VehicleMake = ""
        '
        'lblTradingName
        '
        Me.lblTradingName.AutoSize = True
        Me.lblTradingName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTradingName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTradingName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTradingName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTradingName.Location = New System.Drawing.Point(8, 138)
        Me.lblTradingName.Name = "lblTradingName"
        Me.lblTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTradingName.Size = New System.Drawing.Size(90, 13)
        Me.lblTradingName.TabIndex = 128
        Me.lblTradingName.Text = "Trading name:"
        '
        'lblSurname
        '
        Me.lblSurname.AutoSize = True
        Me.lblSurname.BackColor = System.Drawing.SystemColors.Control
        Me.lblSurname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSurname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSurname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSurname.Location = New System.Drawing.Point(8, 43)
        Me.lblSurname.Name = "lblSurname"
        Me.lblSurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSurname.Size = New System.Drawing.Size(71, 13)
        Me.lblSurname.TabIndex = 93
        Me.lblSurname.Text = "Last name:"
        '
        'lblForename
        '
        Me.lblForename.AutoSize = True
        Me.lblForename.BackColor = System.Drawing.SystemColors.Control
        Me.lblForename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblForename.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblForename.Location = New System.Drawing.Point(8, 67)
        Me.lblForename.Name = "lblForename"
        Me.lblForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblForename.Size = New System.Drawing.Size(69, 13)
        Me.lblForename.TabIndex = 92
        Me.lblForename.Text = "Forename:"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(8, 92)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(36, 13)
        Me.lblTitle.TabIndex = 91
        Me.lblTitle.Text = "Title:"
        '
        'lblInitials
        '
        Me.lblInitials.AutoSize = True
        Me.lblInitials.BackColor = System.Drawing.SystemColors.Control
        Me.lblInitials.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInitials.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInitials.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInitials.Location = New System.Drawing.Point(8, 115)
        Me.lblInitials.Name = "lblInitials"
        Me.lblInitials.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInitials.Size = New System.Drawing.Size(50, 13)
        Me.lblInitials.TabIndex = 90
        Me.lblInitials.Text = "Initials:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(8, 19)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(76, 13)
        Me.lblIDReference.TabIndex = 89
        Me.lblIDReference.Text = "Client code:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboServiceLevel)
        Me.Frame1.Controls.Add(Me.txtAlternativeIdentifier)
        Me.Frame1.Controls.Add(Me.cboBranch)
        Me.Frame1.Controls.Add(Me.cboSubBranch)
        Me.Frame1.Controls.Add(Me.lblServicelevel)
        Me.Frame1.Controls.Add(Me.lblAlternativeIdentifier)
        Me.Frame1.Controls.Add(Me.lblBranch)
        Me.Frame1.Controls.Add(Me.lblSubBranch)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 172)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(289, 129)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        '
        'cboServiceLevel
        '
        Me.cboServiceLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboServiceLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboServiceLevel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboServiceLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboServiceLevel.Location = New System.Drawing.Point(104, 44)
        Me.cboServiceLevel.Name = "cboServiceLevel"
        Me.cboServiceLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboServiceLevel.Size = New System.Drawing.Size(169, 21)
        Me.cboServiceLevel.TabIndex = 7
        '
        'txtAlternativeIdentifier
        '
        Me.txtAlternativeIdentifier.AcceptsReturn = True
        Me.txtAlternativeIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlternativeIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlternativeIdentifier.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlternativeIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlternativeIdentifier.Location = New System.Drawing.Point(104, 16)
        Me.txtAlternativeIdentifier.MaxLength = 0
        Me.txtAlternativeIdentifier.Name = "txtAlternativeIdentifier"
        Me.txtAlternativeIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlternativeIdentifier.Size = New System.Drawing.Size(169, 21)
        Me.txtAlternativeIdentifier.TabIndex = 6
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(104, 72)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(169, 21)
        Me.cboBranch.TabIndex = 8
        Me.cboBranch.Text = " "
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(104, 96)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(169, 21)
        Me.cboSubBranch.TabIndex = 9
        Me.cboSubBranch.Text = " "
        '
        'lblServicelevel
        '
        Me.lblServicelevel.AutoSize = True
        Me.lblServicelevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblServicelevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServicelevel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServicelevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServicelevel.Location = New System.Drawing.Point(8, 48)
        Me.lblServicelevel.Name = "lblServicelevel"
        Me.lblServicelevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServicelevel.Size = New System.Drawing.Size(86, 13)
        Me.lblServicelevel.TabIndex = 149
        Me.lblServicelevel.Text = "Service level:"
        '
        'lblAlternativeIdentifier
        '
        Me.lblAlternativeIdentifier.BackColor = System.Drawing.SystemColors.Control
        Me.lblAlternativeIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAlternativeIdentifier.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblAlternativeIdentifier.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlternativeIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlternativeIdentifier.Location = New System.Drawing.Point(8, 16)
        Me.lblAlternativeIdentifier.Name = "lblAlternativeIdentifier"
        Me.lblAlternativeIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAlternativeIdentifier.Size = New System.Drawing.Size(86, 30)
        Me.lblAlternativeIdentifier.TabIndex = 148
        Me.lblAlternativeIdentifier.Text = "Alternative" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Identifier:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 74)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(61, 17)
        Me.lblBranch.TabIndex = 147
        Me.lblBranch.Text = "Branch:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(8, 98)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(75, 17)
        Me.lblSubBranch.TabIndex = 146
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'fraAgent
        '
        Me.fraAgent.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgent.Controls.Add(Me.txtMembershipId)
        Me.fraAgent.Controls.Add(Me.txtAgentRef)
        Me.fraAgent.Controls.Add(Me.cmdAgentLookUp)
        Me.fraAgent.Controls.Add(Me.pnlAgentName)
        Me.fraAgent.Controls.Add(Me.lblMembershipId)
        Me.fraAgent.Controls.Add(Me.lblAgentName)
        Me.fraAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgent.Location = New System.Drawing.Point(304, 172)
        Me.fraAgent.Name = "fraAgent"
        Me.fraAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgent.Size = New System.Drawing.Size(313, 80)
        Me.fraAgent.TabIndex = 4
        Me.fraAgent.TabStop = False
        Me.fraAgent.Text = "Lead Agent"
        '
        'txtMembershipId
        '
        Me.txtMembershipId.AcceptsReturn = True
        Me.txtMembershipId.BackColor = System.Drawing.SystemColors.Window
        Me.txtMembershipId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMembershipId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMembershipId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMembershipId.Location = New System.Drawing.Point(96, 80)
        Me.txtMembershipId.MaxLength = 0
        Me.txtMembershipId.Name = "txtMembershipId"
        Me.txtMembershipId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMembershipId.Size = New System.Drawing.Size(209, 21)
        Me.txtMembershipId.TabIndex = 16
        '
        'txtAgentRef
        '
        Me.txtAgentRef.AcceptsReturn = True
        Me.txtAgentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentRef.Location = New System.Drawing.Point(96, 24)
        Me.txtAgentRef.MaxLength = 0
        Me.txtAgentRef.Name = "txtAgentRef"
        Me.txtAgentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentRef.Size = New System.Drawing.Size(209, 21)
        Me.txtAgentRef.TabIndex = 15
        '
        'cmdAgentLookUp
        '
        Me.cmdAgentLookUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentLookUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentLookUp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentLookUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentLookUp.Location = New System.Drawing.Point(8, 24)
        Me.cmdAgentLookUp.Name = "cmdAgentLookUp"
        Me.cmdAgentLookUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentLookUp.Size = New System.Drawing.Size(65, 19)
        Me.cmdAgentLookUp.TabIndex = 14
        Me.cmdAgentLookUp.Text = "Code..."
        Me.cmdAgentLookUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentLookUp.UseVisualStyleBackColor = False
        '
        'pnlAgentName
        '
        Me.pnlAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.pnlAgentName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlAgentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlAgentName.Location = New System.Drawing.Point(96, 56)
        Me.pnlAgentName.Name = "pnlAgentName"
        Me.pnlAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlAgentName.Size = New System.Drawing.Size(209, 19)
        Me.pnlAgentName.TabIndex = 132
        Me.pnlAgentName.UseMnemonic = False
        '
        'lblMembershipId
        '
        Me.lblMembershipId.BackColor = System.Drawing.SystemColors.Control
        Me.lblMembershipId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMembershipId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMembershipId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMembershipId.Location = New System.Drawing.Point(8, 80)
        Me.lblMembershipId.Name = "lblMembershipId"
        Me.lblMembershipId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMembershipId.Size = New System.Drawing.Size(85, 25)
        Me.lblMembershipId.TabIndex = 130
        Me.lblMembershipId.Text = "Membership Id:"
        '
        'lblAgentName
        '
        Me.lblAgentName.AutoSize = True
        Me.lblAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentName.Location = New System.Drawing.Point(8, 59)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentName.Size = New System.Drawing.Size(45, 13)
        Me.lblAgentName.TabIndex = 95
        Me.lblAgentName.Text = "Name:"
        '
        'fraBlackList
        '
        Me.fraBlackList.BackColor = System.Drawing.SystemColors.Control
        Me.fraBlackList.Controls.Add(Me.cboBlackListReason)
        Me.fraBlackList.Controls.Add(Me.lblBlacklistReason)
        Me.fraBlackList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBlackList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBlackList.Location = New System.Drawing.Point(304, 252)
        Me.fraBlackList.Name = "fraBlackList"
        Me.fraBlackList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBlackList.Size = New System.Drawing.Size(313, 49)
        Me.fraBlackList.TabIndex = 182
        Me.fraBlackList.TabStop = False
        Me.fraBlackList.Text = "Blacklisting"
        '
        'cboBlackListReason
        '
        Me.cboBlackListReason.BackColor = System.Drawing.SystemColors.Window
        Me.cboBlackListReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBlackListReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBlackListReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBlackListReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBlackListReason.Location = New System.Drawing.Point(72, 20)
        Me.cboBlackListReason.Name = "cboBlackListReason"
        Me.cboBlackListReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBlackListReason.Size = New System.Drawing.Size(233, 21)
        Me.cboBlackListReason.TabIndex = 183
        '
        'lblBlacklistReason
        '
        Me.lblBlacklistReason.AutoSize = True
        Me.lblBlacklistReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblBlacklistReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBlacklistReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBlacklistReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBlacklistReason.Location = New System.Drawing.Point(8, 24)
        Me.lblBlacklistReason.Name = "lblBlacklistReason"
        Me.lblBlacklistReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBlacklistReason.Size = New System.Drawing.Size(54, 13)
        Me.lblBlacklistReason.TabIndex = 184
        Me.lblBlacklistReason.Text = "Reason:"
        '
        '_TabMainTab_TabPage1
        '
        Me._TabMainTab_TabPage1.Controls.Add(Me.fraCorrespondence)
        Me._TabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._TabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._TabMainTab_TabPage1.Controls.Add(Me.fraContact)
        Me._TabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._TabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage1.Name = "_TabMainTab_TabPage1"
        Me._TabMainTab_TabPage1.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage1.TabIndex = 1
        Me._TabMainTab_TabPage1.Text = "2 - Contacts"
        Me._TabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraCorrespondence
        '
        Me.fraCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.fraCorrespondence.Controls.Add(Me.chkeMPS)
        Me.fraCorrespondence.Controls.Add(Me.chkTPS)
        Me.fraCorrespondence.Controls.Add(Me.txtSalutation)
        Me.fraCorrespondence.Controls.Add(Me.chkMailshot)
        Me.fraCorrespondence.Controls.Add(Me.cboCorrespondenceType)
        Me.fraCorrespondence.Controls.Add(Me.lblTPS)
        Me.fraCorrespondence.Controls.Add(Me.lblSalutation)
        Me.fraCorrespondence.Controls.Add(Me.lblPreferredCorrespondence)
        Me.fraCorrespondence.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCorrespondence.Location = New System.Drawing.Point(16, 258)
        Me.fraCorrespondence.Name = "fraCorrespondence"
        Me.fraCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCorrespondence.Size = New System.Drawing.Size(569, 93)
        Me.fraCorrespondence.TabIndex = 134
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
        Me.chkeMPS.Location = New System.Drawing.Point(214, 68)
        Me.chkeMPS.Name = "chkeMPS"
        Me.chkeMPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkeMPS.Size = New System.Drawing.Size(81, 17)
        Me.chkeMPS.TabIndex = 32
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
        Me.chkTPS.Location = New System.Drawing.Point(68, 68)
        Me.chkTPS.Name = "chkTPS"
        Me.chkTPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTPS.Size = New System.Drawing.Size(17, 17)
        Me.chkTPS.TabIndex = 30
        Me.chkTPS.Text = "Mailshot:"
        Me.chkTPS.UseVisualStyleBackColor = False
        '
        'txtSalutation
        '
        Me.txtSalutation.AcceptsReturn = True
        Me.txtSalutation.BackColor = System.Drawing.SystemColors.Window
        Me.txtSalutation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSalutation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalutation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSalutation.Location = New System.Drawing.Point(102, 16)
        Me.txtSalutation.MaxLength = 0
        Me.txtSalutation.Name = "txtSalutation"
        Me.txtSalutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSalutation.Size = New System.Drawing.Size(451, 21)
        Me.txtSalutation.TabIndex = 28
        '
        'chkMailshot
        '
        Me.chkMailshot.BackColor = System.Drawing.SystemColors.Control
        Me.chkMailshot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMailshot.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMailshot.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMailshot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMailshot.Location = New System.Drawing.Point(116, 68)
        Me.chkMailshot.Name = "chkMailshot"
        Me.chkMailshot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMailshot.Size = New System.Drawing.Size(73, 17)
        Me.chkMailshot.TabIndex = 31
        Me.chkMailshot.Text = "MPS:"
        Me.chkMailshot.UseVisualStyleBackColor = False
        '
        'cboCorrespondenceType
        '
        Me.cboCorrespondenceType.BackColor = System.Drawing.SystemColors.Window
        Me.cboCorrespondenceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCorrespondenceType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCorrespondenceType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCorrespondenceType.Location = New System.Drawing.Point(184, 42)
        Me.cboCorrespondenceType.Name = "cboCorrespondenceType"
        Me.cboCorrespondenceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCorrespondenceType.Size = New System.Drawing.Size(217, 21)
        Me.cboCorrespondenceType.TabIndex = 29
        '
        'lblTPS
        '
        Me.lblTPS.BackColor = System.Drawing.SystemColors.Control
        Me.lblTPS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTPS.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTPS.Location = New System.Drawing.Point(16, 68)
        Me.lblTPS.Name = "lblTPS"
        Me.lblTPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTPS.Size = New System.Drawing.Size(85, 17)
        Me.lblTPS.TabIndex = 82
        Me.lblTPS.Text = "TPS:"
        '
        'lblSalutation
        '
        Me.lblSalutation.AutoSize = True
        Me.lblSalutation.BackColor = System.Drawing.SystemColors.Control
        Me.lblSalutation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSalutation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalutation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSalutation.Location = New System.Drawing.Point(16, 18)
        Me.lblSalutation.Name = "lblSalutation"
        Me.lblSalutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSalutation.Size = New System.Drawing.Size(69, 13)
        Me.lblSalutation.TabIndex = 70
        Me.lblSalutation.Text = "Salutation:"
        '
        'lblPreferredCorrespondence
        '
        Me.lblPreferredCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreferredCorrespondence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreferredCorrespondence.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreferredCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreferredCorrespondence.Location = New System.Drawing.Point(16, 46)
        Me.lblPreferredCorrespondence.Name = "lblPreferredCorrespondence"
        Me.lblPreferredCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreferredCorrespondence.Size = New System.Drawing.Size(161, 17)
        Me.lblPreferredCorrespondence.TabIndex = 135
        Me.lblPreferredCorrespondence.Text = "Preferred Correspondence:"
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 34
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
        Me.fraAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(16, 24)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(569, 115)
        Me.fraAddress.TabIndex = 87
        Me.fraAddress.TabStop = False
        Me.fraAddress.Text = "Address"
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(176, 85)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 23
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(96, 85)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 22
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(16, 85)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 21
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.lvwAddresses.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.HideSelection = False
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(16, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(537, 61)
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.lvwAddresses.TabIndex = 20
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
        Me.ImageList2.Images.SetKeyName(0, "")
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
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(16, 144)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(569, 109)
        Me.fraContact.TabIndex = 86
        Me.fraContact.TabStop = False
        Me.fraContact.Text = "Contacts"
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(176, 80)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 27
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(96, 80)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 26
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(16, 80)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 25
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.lvwContacts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.LargeImageList = Me.ImageList2
        Me.lvwContacts.Location = New System.Drawing.Point(16, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(537, 57)
        Me.lvwContacts.SmallImageList = Me.ImageList2
        Me.lvwContacts.TabIndex = 24
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
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 33
        Me._cmdPrevious_0.TabStop = False
        Me._cmdPrevious_0.Text = "<&<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_TabMainTab_TabPage2
        '
        Me._TabMainTab_TabPage2.Controls.Add(Me.cmdAssociates)
        Me._TabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._TabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._TabMainTab_TabPage2.Controls.Add(Me.fraPaymentDetails)
        Me._TabMainTab_TabPage2.Controls.Add(Me.fraEmploymentDetails)
        Me._TabMainTab_TabPage2.Controls.Add(Me.fraFSA)
        Me._TabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage2.Name = "_TabMainTab_TabPage2"
        Me._TabMainTab_TabPage2.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage2.TabIndex = 2
        Me._TabMainTab_TabPage2.Text = "3 - Additions"
        Me._TabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'cmdAssociates
        '
        Me.cmdAssociates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAssociates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAssociates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAssociates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAssociates.Location = New System.Drawing.Point(16, 289)
        Me.cmdAssociates.Name = "cmdAssociates"
        Me.cmdAssociates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAssociates.Size = New System.Drawing.Size(73, 21)
        Me.cmdAssociates.TabIndex = 163
        Me.cmdAssociates.Text = "&Associates"
        Me.cmdAssociates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAssociates.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 50
        Me._cmdNext_2.Text = "&>>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 49
        Me._cmdPrevious_1.TabStop = False
        Me._cmdPrevious_1.Text = "<&<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'fraPaymentDetails
        '
        Me.fraPaymentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentDetails.Controls.Add(Me.lblTermsOfPayment)
        Me.fraPaymentDetails.Controls.Add(Me.cboTermsOfPayment)
        Me.fraPaymentDetails.Controls.Add(Me.txtSource)
        Me.fraPaymentDetails.Controls.Add(Me.cboRenewalStopCode)
        Me.fraPaymentDetails.Controls.Add(Me.cboReminderType)
        Me.fraPaymentDetails.Controls.Add(Me.cboCreditCard)
        Me.fraPaymentDetails.Controls.Add(Me.ddPaymentMethod)
        Me.fraPaymentDetails.Controls.Add(Me.cboCurrency)
        Me.fraPaymentDetails.Controls.Add(Me.lblSource)
        Me.fraPaymentDetails.Controls.Add(Me.lblRenewalStopCode)
        Me.fraPaymentDetails.Controls.Add(Me.lblReminderType)
        Me.fraPaymentDetails.Controls.Add(Me.lblCreditCard)
        Me.fraPaymentDetails.Controls.Add(Me.lblPaymentMethod)
        Me.fraPaymentDetails.Controls.Add(Me.lblCurrency)
        Me.fraPaymentDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentDetails.Location = New System.Drawing.Point(16, 12)
        Me.fraPaymentDetails.Name = "fraPaymentDetails"
        Me.fraPaymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentDetails.Size = New System.Drawing.Size(609, 137)
        Me.fraPaymentDetails.TabIndex = 102
        Me.fraPaymentDetails.TabStop = False
        Me.fraPaymentDetails.Text = "Payment Details"
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.AutoSize = True
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(304, 24)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(101, 13)
        Me.lblTermsOfPayment.TabIndex = 138
        Me.lblTermsOfPayment.Text = "Payment Terms:"
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.FormattingEnabled = True
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(424, 19)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(176, 21)
        Me.cboTermsOfPayment.TabIndex = 137
        '
        'txtSource
        '
        Me.txtSource.AcceptsReturn = True
        Me.txtSource.BackColor = System.Drawing.SystemColors.Window
        Me.txtSource.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSource.Location = New System.Drawing.Point(424, 78)
        Me.txtSource.MaxLength = 0
        Me.txtSource.Name = "txtSource"
        Me.txtSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSource.Size = New System.Drawing.Size(177, 21)
        Me.txtSource.TabIndex = 40
        '
        'cboRenewalStopCode
        '
        Me.cboRenewalStopCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalStopCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalStopCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalStopCode.Location = New System.Drawing.Point(424, 48)
        Me.cboRenewalStopCode.Name = "cboRenewalStopCode"
        Me.cboRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalStopCode.Size = New System.Drawing.Size(177, 21)
        Me.cboRenewalStopCode.TabIndex = 39
        '
        'cboReminderType
        '
        Me.cboReminderType.BackColor = System.Drawing.SystemColors.Window
        Me.cboReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReminderType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReminderType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReminderType.Location = New System.Drawing.Point(112, 80)
        Me.cboReminderType.Name = "cboReminderType"
        Me.cboReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReminderType.Size = New System.Drawing.Size(177, 21)
        Me.cboReminderType.TabIndex = 37
        '
        'cboCreditCard
        '
        Me.cboCreditCard.BackColor = System.Drawing.SystemColors.Window
        Me.cboCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCreditCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCreditCard.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCreditCard.Location = New System.Drawing.Point(424, 108)
        Me.cboCreditCard.Name = "cboCreditCard"
        Me.cboCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCreditCard.Size = New System.Drawing.Size(177, 21)
        Me.cboCreditCard.TabIndex = 41
        Me.cboCreditCard.Tag = "31245190 Financial_Detail,Payment_Card_Code"
        '
        'ddPaymentMethod
        '
        Me.ddPaymentMethod.AllowAbiCodeEntry = False
        Me.ddPaymentMethod.AutoCompleteText = False
        Me.ddPaymentMethod.DataModel = "GIIM"
        Me.ddPaymentMethod.ListIndex = -1
        Me.ddPaymentMethod.ListManager = Nothing
        Me.ddPaymentMethod.Location = New System.Drawing.Point(112, 50)
        Me.ddPaymentMethod.Login = False
        Me.ddPaymentMethod.LongList = False
        Me.ddPaymentMethod.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddPaymentMethod.Name = "ddPaymentMethod"
        Me.ddPaymentMethod.PropertyId = "6946819"
        Me.ddPaymentMethod.ReadOnly_Renamed = False
        Me.ddPaymentMethod.SelLength = 0
        Me.ddPaymentMethod.SelStart = 0
        Me.ddPaymentMethod.SelText = ""
        Me.ddPaymentMethod.Size = New System.Drawing.Size(177, 21)
        Me.ddPaymentMethod.TabIndex = 36
        Me.ddPaymentMethod.ToolTipText = ""
        Me.ddPaymentMethod.VehicleListId = ""
        Me.ddPaymentMethod.VehicleMake = ""
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(112, 20)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(177, 21)
        Me.cboCurrency.TabIndex = 35
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(304, 82)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(52, 13)
        Me.lblSource.TabIndex = 136
        Me.lblSource.Text = "Source:"
        '
        'lblRenewalStopCode
        '
        Me.lblRenewalStopCode.AutoSize = True
        Me.lblRenewalStopCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStopCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStopCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStopCode.Location = New System.Drawing.Point(304, 52)
        Me.lblRenewalStopCode.Name = "lblRenewalStopCode"
        Me.lblRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStopCode.Size = New System.Drawing.Size(119, 13)
        Me.lblRenewalStopCode.TabIndex = 127
        Me.lblRenewalStopCode.Text = "Renewal stop code:"
        '
        'lblReminderType
        '
        Me.lblReminderType.AutoSize = True
        Me.lblReminderType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReminderType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReminderType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReminderType.Location = New System.Drawing.Point(8, 84)
        Me.lblReminderType.Name = "lblReminderType"
        Me.lblReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReminderType.Size = New System.Drawing.Size(96, 13)
        Me.lblReminderType.TabIndex = 107
        Me.lblReminderType.Text = "Reminder type:"
        '
        'lblCreditCard
        '
        Me.lblCreditCard.AutoSize = True
        Me.lblCreditCard.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreditCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreditCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreditCard.Location = New System.Drawing.Point(304, 112)
        Me.lblCreditCard.Name = "lblCreditCard"
        Me.lblCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreditCard.Size = New System.Drawing.Size(69, 13)
        Me.lblCreditCard.TabIndex = 106
        Me.lblCreditCard.Text = "Card type:"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.AutoSize = True
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(8, 54)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(109, 13)
        Me.lblPaymentMethod.TabIndex = 105
        Me.lblPaymentMethod.Text = "Payment method:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 24)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(65, 13)
        Me.lblCurrency.TabIndex = 103
        Me.lblCurrency.Text = "Currency:"
        '
        'fraEmploymentDetails
        '
        Me.fraEmploymentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraEmploymentDetails.Controls.Add(Me.ddBusiness)
        Me.fraEmploymentDetails.Controls.Add(Me.ddOccupation)
        Me.fraEmploymentDetails.Controls.Add(Me.ddSecondaryBusiness)
        Me.fraEmploymentDetails.Controls.Add(Me.ddSecondaryOccupation)
        Me.fraEmploymentDetails.Controls.Add(Me.ddEmployment)
        Me.fraEmploymentDetails.Controls.Add(Me.ddSecEmploymentStatus)
        Me.fraEmploymentDetails.Controls.Add(Me.lblEmployer)
        Me.fraEmploymentDetails.Controls.Add(Me.lblOccupation)
        Me.fraEmploymentDetails.Controls.Add(Me.lblEmploymentStatus)
        Me.fraEmploymentDetails.Controls.Add(Me.lblSecOccupation)
        Me.fraEmploymentDetails.Controls.Add(Me.lblSecEmployer)
        Me.fraEmploymentDetails.Controls.Add(Me.lblSecEmploymentStatus)
        Me.fraEmploymentDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEmploymentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEmploymentDetails.Location = New System.Drawing.Point(16, 151)
        Me.fraEmploymentDetails.Name = "fraEmploymentDetails"
        Me.fraEmploymentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEmploymentDetails.Size = New System.Drawing.Size(609, 117)
        Me.fraEmploymentDetails.TabIndex = 108
        Me.fraEmploymentDetails.TabStop = False
        Me.fraEmploymentDetails.Text = "Employment  Details                                       Secondary Details"
        '
        'ddBusiness
        '
        Me.ddBusiness.AllowAbiCodeEntry = False
        Me.ddBusiness.AutoCompleteText = False
        Me.ddBusiness.DataModel = "GIIM"
        Me.ddBusiness.ListIndex = -1
        Me.ddBusiness.ListManager = Nothing
        Me.ddBusiness.Location = New System.Drawing.Point(88, 54)
        Me.ddBusiness.Login = False
        Me.ddBusiness.LongList = True
        Me.ddBusiness.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddBusiness.Name = "ddBusiness"
        Me.ddBusiness.PropertyId = "2228228"
        Me.ddBusiness.ReadOnly_Renamed = False
        Me.ddBusiness.SelLength = 0
        Me.ddBusiness.SelStart = 0
        Me.ddBusiness.SelText = ""
        Me.ddBusiness.Size = New System.Drawing.Size(177, 21)
        Me.ddBusiness.TabIndex = 43
        Me.ddBusiness.ToolTipText = ""
        Me.ddBusiness.VehicleListId = ""
        Me.ddBusiness.VehicleMake = ""
        '
        'ddOccupation
        '
        Me.ddOccupation.AllowAbiCodeEntry = False
        Me.ddOccupation.AutoCompleteText = False
        Me.ddOccupation.DataModel = "GIIM"
        Me.ddOccupation.ListIndex = -1
        Me.ddOccupation.ListManager = Nothing
        Me.ddOccupation.Location = New System.Drawing.Point(88, 23)
        Me.ddOccupation.Login = False
        Me.ddOccupation.LongList = True
        Me.ddOccupation.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddOccupation.Name = "ddOccupation"
        Me.ddOccupation.PropertyId = "2228226"
        Me.ddOccupation.ReadOnly_Renamed = False
        Me.ddOccupation.SelLength = 0
        Me.ddOccupation.SelStart = 0
        Me.ddOccupation.SelText = ""
        Me.ddOccupation.Size = New System.Drawing.Size(177, 21)
        Me.ddOccupation.TabIndex = 42
        Me.ddOccupation.ToolTipText = ""
        Me.ddOccupation.VehicleListId = ""
        Me.ddOccupation.VehicleMake = ""
        '
        'ddSecondaryBusiness
        '
        Me.ddSecondaryBusiness.AllowAbiCodeEntry = False
        Me.ddSecondaryBusiness.AutoCompleteText = False
        Me.ddSecondaryBusiness.DataModel = "GIIM"
        Me.ddSecondaryBusiness.ListIndex = -1
        Me.ddSecondaryBusiness.ListManager = Nothing
        Me.ddSecondaryBusiness.Location = New System.Drawing.Point(424, 54)
        Me.ddSecondaryBusiness.Login = False
        Me.ddSecondaryBusiness.LongList = True
        Me.ddSecondaryBusiness.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddSecondaryBusiness.Name = "ddSecondaryBusiness"
        Me.ddSecondaryBusiness.PropertyId = "2228228"
        Me.ddSecondaryBusiness.ReadOnly_Renamed = False
        Me.ddSecondaryBusiness.SelLength = 0
        Me.ddSecondaryBusiness.SelStart = 0
        Me.ddSecondaryBusiness.SelText = ""
        Me.ddSecondaryBusiness.Size = New System.Drawing.Size(177, 21)
        Me.ddSecondaryBusiness.TabIndex = 46
        Me.ddSecondaryBusiness.ToolTipText = ""
        Me.ddSecondaryBusiness.VehicleListId = ""
        Me.ddSecondaryBusiness.VehicleMake = ""
        '
        'ddSecondaryOccupation
        '
        Me.ddSecondaryOccupation.AllowAbiCodeEntry = False
        Me.ddSecondaryOccupation.AutoCompleteText = False
        Me.ddSecondaryOccupation.DataModel = "GIIM"
        Me.ddSecondaryOccupation.ListIndex = -1
        Me.ddSecondaryOccupation.ListManager = Nothing
        Me.ddSecondaryOccupation.Location = New System.Drawing.Point(424, 23)
        Me.ddSecondaryOccupation.Login = False
        Me.ddSecondaryOccupation.LongList = True
        Me.ddSecondaryOccupation.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddSecondaryOccupation.Name = "ddSecondaryOccupation"
        Me.ddSecondaryOccupation.PropertyId = "2228226"
        Me.ddSecondaryOccupation.ReadOnly_Renamed = False
        Me.ddSecondaryOccupation.SelLength = 0
        Me.ddSecondaryOccupation.SelStart = 0
        Me.ddSecondaryOccupation.SelText = ""
        Me.ddSecondaryOccupation.Size = New System.Drawing.Size(177, 21)
        Me.ddSecondaryOccupation.TabIndex = 45
        Me.ddSecondaryOccupation.ToolTipText = ""
        Me.ddSecondaryOccupation.VehicleListId = ""
        Me.ddSecondaryOccupation.VehicleMake = ""
        '
        'ddEmployment
        '
        Me.ddEmployment.AllowAbiCodeEntry = False
        Me.ddEmployment.AutoCompleteText = False
        Me.ddEmployment.DataModel = "GIIM"
        Me.ddEmployment.ListIndex = -1
        Me.ddEmployment.ListManager = Nothing
        Me.ddEmployment.Location = New System.Drawing.Point(88, 85)
        Me.ddEmployment.Login = False
        Me.ddEmployment.LongList = True
        Me.ddEmployment.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddEmployment.Name = "ddEmployment"
        Me.ddEmployment.PropertyId = "2228230"
        Me.ddEmployment.ReadOnly_Renamed = False
        Me.ddEmployment.SelLength = 0
        Me.ddEmployment.SelStart = 0
        Me.ddEmployment.SelText = ""
        Me.ddEmployment.Size = New System.Drawing.Size(177, 21)
        Me.ddEmployment.TabIndex = 44
        Me.ddEmployment.ToolTipText = ""
        Me.ddEmployment.VehicleListId = ""
        Me.ddEmployment.VehicleMake = ""
        '
        'ddSecEmploymentStatus
        '
        Me.ddSecEmploymentStatus.AllowAbiCodeEntry = False
        Me.ddSecEmploymentStatus.AutoCompleteText = False
        Me.ddSecEmploymentStatus.DataModel = "GIIM"
        Me.ddSecEmploymentStatus.ListIndex = -1
        Me.ddSecEmploymentStatus.ListManager = Nothing
        Me.ddSecEmploymentStatus.Location = New System.Drawing.Point(424, 85)
        Me.ddSecEmploymentStatus.Login = False
        Me.ddSecEmploymentStatus.LongList = True
        Me.ddSecEmploymentStatus.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddSecEmploymentStatus.Name = "ddSecEmploymentStatus"
        Me.ddSecEmploymentStatus.PropertyId = "2228230"
        Me.ddSecEmploymentStatus.ReadOnly_Renamed = False
        Me.ddSecEmploymentStatus.SelLength = 0
        Me.ddSecEmploymentStatus.SelStart = 0
        Me.ddSecEmploymentStatus.SelText = ""
        Me.ddSecEmploymentStatus.Size = New System.Drawing.Size(177, 21)
        Me.ddSecEmploymentStatus.TabIndex = 47
        Me.ddSecEmploymentStatus.ToolTipText = ""
        Me.ddSecEmploymentStatus.VehicleListId = ""
        Me.ddSecEmploymentStatus.VehicleMake = ""
        '
        'lblEmployer
        '
        Me.lblEmployer.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmployer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmployer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmployer.Location = New System.Drawing.Point(8, 50)
        Me.lblEmployer.Name = "lblEmployer"
        Me.lblEmployer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmployer.Size = New System.Drawing.Size(73, 29)
        Me.lblEmployer.TabIndex = 114
        Me.lblEmployer.Text = "Employer's Business:"
        '
        'lblOccupation
        '
        Me.lblOccupation.AutoSize = True
        Me.lblOccupation.BackColor = System.Drawing.SystemColors.Control
        Me.lblOccupation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOccupation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOccupation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOccupation.Location = New System.Drawing.Point(8, 27)
        Me.lblOccupation.Name = "lblOccupation"
        Me.lblOccupation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOccupation.Size = New System.Drawing.Size(75, 13)
        Me.lblOccupation.TabIndex = 113
        Me.lblOccupation.Text = "Occupation:"
        '
        'lblEmploymentStatus
        '
        Me.lblEmploymentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmploymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmploymentStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmploymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmploymentStatus.Location = New System.Drawing.Point(8, 87)
        Me.lblEmploymentStatus.Name = "lblEmploymentStatus"
        Me.lblEmploymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmploymentStatus.Size = New System.Drawing.Size(121, 17)
        Me.lblEmploymentStatus.TabIndex = 112
        Me.lblEmploymentStatus.Text = "Status:"
        '
        'lblSecOccupation
        '
        Me.lblSecOccupation.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecOccupation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecOccupation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecOccupation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecOccupation.Location = New System.Drawing.Point(304, 25)
        Me.lblSecOccupation.Name = "lblSecOccupation"
        Me.lblSecOccupation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecOccupation.Size = New System.Drawing.Size(81, 17)
        Me.lblSecOccupation.TabIndex = 111
        Me.lblSecOccupation.Text = "Occupation:"
        '
        'lblSecEmployer
        '
        Me.lblSecEmployer.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecEmployer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecEmployer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecEmployer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecEmployer.Location = New System.Drawing.Point(304, 50)
        Me.lblSecEmployer.Name = "lblSecEmployer"
        Me.lblSecEmployer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecEmployer.Size = New System.Drawing.Size(121, 29)
        Me.lblSecEmployer.TabIndex = 110
        Me.lblSecEmployer.Text = "Employer's Business:"
        '
        'lblSecEmploymentStatus
        '
        Me.lblSecEmploymentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecEmploymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecEmploymentStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecEmploymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecEmploymentStatus.Location = New System.Drawing.Point(304, 87)
        Me.lblSecEmploymentStatus.Name = "lblSecEmploymentStatus"
        Me.lblSecEmploymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecEmploymentStatus.Size = New System.Drawing.Size(81, 17)
        Me.lblSecEmploymentStatus.TabIndex = 109
        Me.lblSecEmploymentStatus.Text = "Status:"
        '
        'fraFSA
        '
        Me.fraFSA.BackColor = System.Drawing.SystemColors.Control
        Me.fraFSA.Controls.Add(Me.txtTobLetter)
        Me.fraFSA.Controls.Add(Me.lblTobLetter)
        Me.fraFSA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFSA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFSA.Location = New System.Drawing.Point(16, 317)
        Me.fraFSA.Name = "fraFSA"
        Me.fraFSA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFSA.Size = New System.Drawing.Size(609, 49)
        Me.fraFSA.TabIndex = 137
        Me.fraFSA.TabStop = False
        Me.fraFSA.Text = "Compliance Details"
        '
        'txtTobLetter
        '
        Me.txtTobLetter.AcceptsReturn = True
        Me.txtTobLetter.BackColor = System.Drawing.SystemColors.Window
        Me.txtTobLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTobLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTobLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTobLetter.Location = New System.Drawing.Point(384, 16)
        Me.txtTobLetter.MaxLength = 0
        Me.txtTobLetter.Name = "txtTobLetter"
        Me.txtTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTobLetter.Size = New System.Drawing.Size(177, 21)
        Me.txtTobLetter.TabIndex = 48
        '
        'lblTobLetter
        '
        Me.lblTobLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblTobLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTobLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTobLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTobLetter.Location = New System.Drawing.Point(272, 16)
        Me.lblTobLetter.Name = "lblTobLetter"
        Me.lblTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTobLetter.Size = New System.Drawing.Size(121, 25)
        Me.lblTobLetter.TabIndex = 138
        Me.lblTobLetter.Text = "Terms Of Business Letter Sent:"
        '
        '_TabMainTab_TabPage3
        '
        Me._TabMainTab_TabPage3.Controls.Add(Me.txtCurrency)
        Me._TabMainTab_TabPage3.Controls.Add(Me.txtDate)
        Me._TabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._TabMainTab_TabPage3.Controls.Add(Me.txtCCJ)
        Me._TabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._TabMainTab_TabPage3.Controls.Add(Me.frmContacts)
        Me._TabMainTab_TabPage3.Controls.Add(Me.lblCCJ)
        Me._TabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage3.Name = "_TabMainTab_TabPage3"
        Me._TabMainTab_TabPage3.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage3.TabIndex = 3
        Me._TabMainTab_TabPage3.Text = "4 - Convictions"
        Me._TabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(432, 304)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(65, 20)
        Me.txtCurrency.TabIndex = 125
        Me.txtCurrency.TabStop = False
        Me.txtCurrency.Visible = False
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(336, 304)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(65, 20)
        Me.txtDate.TabIndex = 124
        Me.txtDate.TabStop = False
        Me.txtDate.Visible = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 57
        Me._cmdNext_3.Text = "&>>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'txtCCJ
        '
        Me.txtCCJ.AcceptsReturn = True
        Me.txtCCJ.BackColor = System.Drawing.SystemColors.Window
        Me.txtCCJ.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCCJ.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCCJ.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCCJ.Location = New System.Drawing.Point(192, 280)
        Me.txtCCJ.MaxLength = 0
        Me.txtCCJ.Name = "txtCCJ"
        Me.txtCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCCJ.Size = New System.Drawing.Size(41, 21)
        Me.txtCCJ.TabIndex = 55
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 56
        Me._cmdPrevious_2.TabStop = False
        Me._cmdPrevious_2.Text = "<&<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'frmContacts
        '
        Me.frmContacts.BackColor = System.Drawing.SystemColors.Control
        Me.frmContacts.Controls.Add(Me.cmdAddConv)
        Me.frmContacts.Controls.Add(Me.cmdDeleteConv)
        Me.frmContacts.Controls.Add(Me.cmdEditConv)
        Me.frmContacts.Controls.Add(Me.lvwConvictions)
        Me.frmContacts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmContacts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmContacts.Location = New System.Drawing.Point(12, 32)
        Me.frmContacts.Name = "frmContacts"
        Me.frmContacts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmContacts.Size = New System.Drawing.Size(577, 241)
        Me.frmContacts.TabIndex = 85
        Me.frmContacts.TabStop = False
        '
        'cmdAddConv
        '
        Me.cmdAddConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddConv.Location = New System.Drawing.Point(16, 208)
        Me.cmdAddConv.Name = "cmdAddConv"
        Me.cmdAddConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddConv.TabIndex = 52
        Me.cmdAddConv.Text = "&Add"
        Me.cmdAddConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddConv.UseVisualStyleBackColor = False
        '
        'cmdDeleteConv
        '
        Me.cmdDeleteConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteConv.Location = New System.Drawing.Point(104, 208)
        Me.cmdDeleteConv.Name = "cmdDeleteConv"
        Me.cmdDeleteConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteConv.TabIndex = 53
        Me.cmdDeleteConv.Text = "&Delete"
        Me.cmdDeleteConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteConv.UseVisualStyleBackColor = False
        '
        'cmdEditConv
        '
        Me.cmdEditConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditConv.Location = New System.Drawing.Point(184, 210)
        Me.cmdEditConv.Name = "cmdEditConv"
        Me.cmdEditConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditConv.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditConv.TabIndex = 54
        Me.cmdEditConv.Text = "&Edit"
        Me.cmdEditConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditConv.UseVisualStyleBackColor = False
        '
        'lvwConvictions
        '
        Me.lvwConvictions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwConvictions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwConvictions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwConvictions_ColumnHeader_1, Me._lvwConvictions_ColumnHeader_2, Me._lvwConvictions_ColumnHeader_3, Me._lvwConvictions_ColumnHeader_4, Me._lvwConvictions_ColumnHeader_5, Me._lvwConvictions_ColumnHeader_6})
        Me.lvwConvictions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwConvictions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwConvictions.LargeImageList = Me.ImageList2
        Me.lvwConvictions.Location = New System.Drawing.Point(16, 16)
        Me.lvwConvictions.Name = "lvwConvictions"
        Me.lvwConvictions.Size = New System.Drawing.Size(545, 185)
        Me.lvwConvictions.SmallImageList = Me.ImageList2
        Me.lvwConvictions.TabIndex = 51
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
        Me._lvwConvictions_ColumnHeader_3.Width = 97
        '
        '_lvwConvictions_ColumnHeader_4
        '
        Me._lvwConvictions_ColumnHeader_4.Tag = ""
        Me._lvwConvictions_ColumnHeader_4.Text = "Fine"
        Me._lvwConvictions_ColumnHeader_4.Width = 134
        '
        '_lvwConvictions_ColumnHeader_5
        '
        Me._lvwConvictions_ColumnHeader_5.Tag = ""
        Me._lvwConvictions_ColumnHeader_5.Text = "Conviction Status"
        Me._lvwConvictions_ColumnHeader_5.Width = 97
        '
        '_lvwConvictions_ColumnHeader_6
        '
        Me._lvwConvictions_ColumnHeader_6.Tag = ""
        Me._lvwConvictions_ColumnHeader_6.Text = "Penalty Points"
        Me._lvwConvictions_ColumnHeader_6.Width = 97
        '
        'lblCCJ
        '
        Me.lblCCJ.BackColor = System.Drawing.SystemColors.Control
        Me.lblCCJ.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCCJ.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCCJ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCCJ.Location = New System.Drawing.Point(24, 283)
        Me.lblCCJ.Name = "lblCCJ"
        Me.lblCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCCJ.Size = New System.Drawing.Size(169, 17)
        Me.lblCCJ.TabIndex = 123
        Me.lblCCJ.Text = "County court judgements:"
        '
        '_TabMainTab_TabPage4
        '
        Me._TabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._TabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._TabMainTab_TabPage4.Controls.Add(Me.fraLifestyle)
        Me._TabMainTab_TabPage4.Controls.Add(Me.fraDependants)
        Me._TabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage4.Name = "_TabMainTab_TabPage4"
        Me._TabMainTab_TabPage4.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage4.TabIndex = 4
        Me._TabMainTab_TabPage4.Text = "5 - Lifestyle"
        Me._TabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 75
        Me._cmdNext_4.Text = "&>>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 74
        Me._cmdPrevious_3.TabStop = False
        Me._cmdPrevious_3.Text = "<&<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'fraLifestyle
        '
        Me.fraLifestyle.BackColor = System.Drawing.SystemColors.Control
        Me.fraLifestyle.Controls.Add(Me.cboSeasonalGift)
        Me.fraLifestyle.Controls.Add(Me.txtTPPassword)
        Me.fraLifestyle.Controls.Add(Me.txtLoyaltyNumberPrefix)
        Me.fraLifestyle.Controls.Add(Me.txtLoyaltyNumber)
        Me.fraLifestyle.Controls.Add(Me.ddGender)
        Me.fraLifestyle.Controls.Add(Me.cboAccommodation)
        Me.fraLifestyle.Controls.Add(Me.txtDOB)
        Me.fraLifestyle.Controls.Add(Me.cboNationality)
        Me.fraLifestyle.Controls.Add(Me.chkPets)
        Me.fraLifestyle.Controls.Add(Me.chkSmoker)
        Me.fraLifestyle.Controls.Add(Me.ddMaritalStatus)
        Me.fraLifestyle.Controls.Add(Me.lblTPPassword)
        Me.fraLifestyle.Controls.Add(Me.lblLoyaltyNumber)
        Me.fraLifestyle.Controls.Add(Me.lblSeasonalGift)
        Me.fraLifestyle.Controls.Add(Me.lblMaritalStatus)
        Me.fraLifestyle.Controls.Add(Me.lblAccommodation)
        Me.fraLifestyle.Controls.Add(Me.lblGender)
        Me.fraLifestyle.Controls.Add(Me.lblDOB)
        Me.fraLifestyle.Controls.Add(Me.lblNationality)
        Me.fraLifestyle.Controls.Add(Me.lblSmoker)
        Me.fraLifestyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLifestyle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLifestyle.Location = New System.Drawing.Point(8, 32)
        Me.fraLifestyle.Name = "fraLifestyle"
        Me.fraLifestyle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLifestyle.Size = New System.Drawing.Size(577, 161)
        Me.fraLifestyle.TabIndex = 115
        Me.fraLifestyle.TabStop = False
        Me.fraLifestyle.Text = "Personal Details"
        '
        'cboSeasonalGift
        '
        Me.cboSeasonalGift.BackColor = System.Drawing.SystemColors.Window
        Me.cboSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSeasonalGift.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSeasonalGift.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSeasonalGift.Location = New System.Drawing.Point(96, 72)
        Me.cboSeasonalGift.Name = "cboSeasonalGift"
        Me.cboSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSeasonalGift.Size = New System.Drawing.Size(161, 21)
        Me.cboSeasonalGift.TabIndex = 60
        '
        'txtTPPassword
        '
        Me.txtTPPassword.AcceptsReturn = True
        Me.txtTPPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtTPPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTPPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTPPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTPPassword.Location = New System.Drawing.Point(384, 96)
        Me.txtTPPassword.MaxLength = 0
        Me.txtTPPassword.Name = "txtTPPassword"
        Me.txtTPPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTPPassword.Size = New System.Drawing.Size(183, 21)
        Me.txtTPPassword.TabIndex = 66
        '
        'txtLoyaltyNumberPrefix
        '
        Me.txtLoyaltyNumberPrefix.AcceptsReturn = True
        Me.txtLoyaltyNumberPrefix.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumberPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumberPrefix.Enabled = False
        Me.txtLoyaltyNumberPrefix.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumberPrefix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumberPrefix.Location = New System.Drawing.Point(96, 96)
        Me.txtLoyaltyNumberPrefix.MaxLength = 10
        Me.txtLoyaltyNumberPrefix.Name = "txtLoyaltyNumberPrefix"
        Me.txtLoyaltyNumberPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumberPrefix.Size = New System.Drawing.Size(65, 21)
        Me.txtLoyaltyNumberPrefix.TabIndex = 61
        Me.txtLoyaltyNumberPrefix.Text = "6014 35"
        '
        'txtLoyaltyNumber
        '
        Me.txtLoyaltyNumber.AcceptsReturn = True
        Me.txtLoyaltyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumber.Location = New System.Drawing.Point(168, 96)
        Me.txtLoyaltyNumber.MaxLength = 0
        Me.txtLoyaltyNumber.Name = "txtLoyaltyNumber"
        Me.txtLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumber.Size = New System.Drawing.Size(89, 21)
        Me.txtLoyaltyNumber.TabIndex = 62
        '
        'ddGender
        '
        Me.ddGender.AllowAbiCodeEntry = False
        Me.ddGender.AutoCompleteText = False
        Me.ddGender.DataModel = "GIIM"
        Me.ddGender.ListIndex = -1
        Me.ddGender.ListManager = Nothing
        Me.ddGender.Location = New System.Drawing.Point(384, 24)
        Me.ddGender.Login = False
        Me.ddGender.LongList = False
        Me.ddGender.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddGender.Name = "ddGender"
        Me.ddGender.PropertyId = "131091"
        Me.ddGender.ReadOnly_Renamed = False
        Me.ddGender.SelLength = 0
        Me.ddGender.SelStart = 0
        Me.ddGender.SelText = ""
        Me.ddGender.Size = New System.Drawing.Size(137, 21)
        Me.ddGender.TabIndex = 63
        Me.ddGender.ToolTipText = ""
        Me.ddGender.VehicleListId = ""
        Me.ddGender.VehicleMake = ""
        '
        'cboAccommodation
        '
        Me.cboAccommodation.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccommodation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccommodation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccommodation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAccommodation.Location = New System.Drawing.Point(384, 72)
        Me.cboAccommodation.Name = "cboAccommodation"
        Me.cboAccommodation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccommodation.Size = New System.Drawing.Size(137, 21)
        Me.cboAccommodation.TabIndex = 65
        Me.cboAccommodation.Tag = "32555905 Resident,Resident_Code"
        '
        'txtDOB
        '
        Me.txtDOB.AcceptsReturn = True
        Me.txtDOB.BackColor = System.Drawing.SystemColors.Window
        Me.txtDOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDOB.Location = New System.Drawing.Point(96, 24)
        Me.txtDOB.MaxLength = 0
        Me.txtDOB.Name = "txtDOB"
        Me.txtDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDOB.Size = New System.Drawing.Size(161, 21)
        Me.txtDOB.TabIndex = 58
        '
        'cboNationality
        '
        Me.cboNationality.BackColor = System.Drawing.SystemColors.Window
        Me.cboNationality.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNationality.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNationality.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNationality.Location = New System.Drawing.Point(384, 48)
        Me.cboNationality.Name = "cboNationality"
        Me.cboNationality.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNationality.Size = New System.Drawing.Size(137, 21)
        Me.cboNationality.TabIndex = 64
        '
        'chkPets
        '
        Me.chkPets.BackColor = System.Drawing.SystemColors.Control
        Me.chkPets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPets.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPets.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPets.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPets.Location = New System.Drawing.Point(3, 136)
        Me.chkPets.Name = "chkPets"
        Me.chkPets.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPets.Size = New System.Drawing.Size(49, 17)
        Me.chkPets.TabIndex = 67
        Me.chkPets.Text = "Pets:"
        Me.chkPets.UseVisualStyleBackColor = False
        '
        'chkSmoker
        '
        Me.chkSmoker.BackColor = System.Drawing.SystemColors.Control
        Me.chkSmoker.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSmoker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSmoker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSmoker.Location = New System.Drawing.Point(132, 136)
        Me.chkSmoker.Name = "chkSmoker"
        Me.chkSmoker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSmoker.Size = New System.Drawing.Size(17, 17)
        Me.chkSmoker.TabIndex = 68
        Me.chkSmoker.Text = "Check1"
        Me.chkSmoker.UseVisualStyleBackColor = False
        '
        'ddMaritalStatus
        '
        Me.ddMaritalStatus.AllowAbiCodeEntry = False
        Me.ddMaritalStatus.AutoCompleteText = False
        Me.ddMaritalStatus.DataModel = "GIIM"
        Me.ddMaritalStatus.ListIndex = -1
        Me.ddMaritalStatus.ListManager = Nothing
        Me.ddMaritalStatus.Location = New System.Drawing.Point(96, 48)
        Me.ddMaritalStatus.Login = False
        Me.ddMaritalStatus.LongList = False
        Me.ddMaritalStatus.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddMaritalStatus.Name = "ddMaritalStatus"
        Me.ddMaritalStatus.PropertyId = "131107"
        Me.ddMaritalStatus.ReadOnly_Renamed = False
        Me.ddMaritalStatus.SelLength = 0
        Me.ddMaritalStatus.SelStart = 0
        Me.ddMaritalStatus.SelText = ""
        Me.ddMaritalStatus.Size = New System.Drawing.Size(161, 21)
        Me.ddMaritalStatus.TabIndex = 59
        Me.ddMaritalStatus.ToolTipText = ""
        Me.ddMaritalStatus.VehicleListId = ""
        Me.ddMaritalStatus.VehicleMake = ""
        '
        'lblTPPassword
        '
        Me.lblTPPassword.AutoSize = True
        Me.lblTPPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblTPPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTPPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTPPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTPPassword.Location = New System.Drawing.Point(268, 99)
        Me.lblTPPassword.Name = "lblTPPassword"
        Me.lblTPPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTPPassword.Size = New System.Drawing.Size(66, 13)
        Me.lblTPPassword.TabIndex = 83
        Me.lblTPPassword.Text = "Password:"
        '
        'lblLoyaltyNumber
        '
        Me.lblLoyaltyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoyaltyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoyaltyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoyaltyNumber.Location = New System.Drawing.Point(8, 96)
        Me.lblLoyaltyNumber.Name = "lblLoyaltyNumber"
        Me.lblLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoyaltyNumber.Size = New System.Drawing.Size(82, 37)
        Me.lblLoyaltyNumber.TabIndex = 129
        Me.lblLoyaltyNumber.Text = "Loyalty number:"
        '
        'lblSeasonalGift
        '
        Me.lblSeasonalGift.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeasonalGift.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeasonalGift.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeasonalGift.Location = New System.Drawing.Point(8, 75)
        Me.lblSeasonalGift.Name = "lblSeasonalGift"
        Me.lblSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeasonalGift.Size = New System.Drawing.Size(121, 17)
        Me.lblSeasonalGift.TabIndex = 126
        Me.lblSeasonalGift.Text = "Seasonal gift:"
        '
        'lblMaritalStatus
        '
        Me.lblMaritalStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaritalStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaritalStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaritalStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaritalStatus.Location = New System.Drawing.Point(8, 51)
        Me.lblMaritalStatus.Name = "lblMaritalStatus"
        Me.lblMaritalStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaritalStatus.Size = New System.Drawing.Size(105, 17)
        Me.lblMaritalStatus.TabIndex = 121
        Me.lblMaritalStatus.Text = "Marital status:"
        '
        'lblAccommodation
        '
        Me.lblAccommodation.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccommodation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccommodation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccommodation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccommodation.Location = New System.Drawing.Point(268, 75)
        Me.lblAccommodation.Name = "lblAccommodation"
        Me.lblAccommodation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccommodation.Size = New System.Drawing.Size(113, 17)
        Me.lblAccommodation.TabIndex = 120
        Me.lblAccommodation.Text = "Accommodation:"
        '
        'lblGender
        '
        Me.lblGender.BackColor = System.Drawing.SystemColors.Control
        Me.lblGender.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGender.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGender.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGender.Location = New System.Drawing.Point(268, 27)
        Me.lblGender.Name = "lblGender"
        Me.lblGender.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGender.Size = New System.Drawing.Size(49, 17)
        Me.lblGender.TabIndex = 119
        Me.lblGender.Text = "Gender:"
        '
        'lblDOB
        '
        Me.lblDOB.AutoSize = True
        Me.lblDOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblDOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDOB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDOB.Location = New System.Drawing.Point(8, 27)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDOB.Size = New System.Drawing.Size(46, 13)
        Me.lblDOB.TabIndex = 118
        Me.lblDOB.Text = "D.O.B:"
        '
        'lblNationality
        '
        Me.lblNationality.BackColor = System.Drawing.SystemColors.Control
        Me.lblNationality.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNationality.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNationality.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNationality.Location = New System.Drawing.Point(268, 51)
        Me.lblNationality.Name = "lblNationality"
        Me.lblNationality.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNationality.Size = New System.Drawing.Size(89, 17)
        Me.lblNationality.TabIndex = 117
        Me.lblNationality.Text = "Nationality:"
        '
        'lblSmoker
        '
        Me.lblSmoker.BackColor = System.Drawing.SystemColors.Control
        Me.lblSmoker.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSmoker.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSmoker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSmoker.Location = New System.Drawing.Point(76, 136)
        Me.lblSmoker.Name = "lblSmoker"
        Me.lblSmoker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSmoker.Size = New System.Drawing.Size(65, 17)
        Me.lblSmoker.TabIndex = 116
        Me.lblSmoker.Text = "Smoker:"
        '
        'fraDependants
        '
        Me.fraDependants.BackColor = System.Drawing.SystemColors.Control
        Me.fraDependants.Controls.Add(Me.cmdAddLife)
        Me.fraDependants.Controls.Add(Me.cmdDeleteLife)
        Me.fraDependants.Controls.Add(Me.cmdEditLife)
        Me.fraDependants.Controls.Add(Me.lvwLifestyle)
        Me.fraDependants.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDependants.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDependants.Location = New System.Drawing.Point(8, 196)
        Me.fraDependants.Name = "fraDependants"
        Me.fraDependants.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDependants.Size = New System.Drawing.Size(577, 135)
        Me.fraDependants.TabIndex = 122
        Me.fraDependants.TabStop = False
        Me.fraDependants.Text = "Dependants"
        '
        'cmdAddLife
        '
        Me.cmdAddLife.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddLife.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddLife.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddLife.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddLife.Location = New System.Drawing.Point(8, 104)
        Me.cmdAddLife.Name = "cmdAddLife"
        Me.cmdAddLife.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddLife.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddLife.TabIndex = 71
        Me.cmdAddLife.Text = "&Add"
        Me.cmdAddLife.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddLife.UseVisualStyleBackColor = False
        '
        'cmdDeleteLife
        '
        Me.cmdDeleteLife.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteLife.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteLife.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteLife.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteLife.Location = New System.Drawing.Point(88, 104)
        Me.cmdDeleteLife.Name = "cmdDeleteLife"
        Me.cmdDeleteLife.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLife.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteLife.TabIndex = 72
        Me.cmdDeleteLife.Text = "&Delete"
        Me.cmdDeleteLife.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteLife.UseVisualStyleBackColor = False
        '
        'cmdEditLife
        '
        Me.cmdEditLife.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditLife.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditLife.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditLife.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditLife.Location = New System.Drawing.Point(168, 104)
        Me.cmdEditLife.Name = "cmdEditLife"
        Me.cmdEditLife.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditLife.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditLife.TabIndex = 73
        Me.cmdEditLife.Text = "&Edit"
        Me.cmdEditLife.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditLife.UseVisualStyleBackColor = False
        '
        'lvwLifestyle
        '
        Me.lvwLifestyle.BackColor = System.Drawing.SystemColors.Window
        Me.lvwLifestyle.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwLifestyle.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLifestyle_ColumnHeader_1, Me._lvwLifestyle_ColumnHeader_2, Me._lvwLifestyle_ColumnHeader_3, Me._lvwLifestyle_ColumnHeader_4, Me._lvwLifestyle_ColumnHeader_5, Me._lvwLifestyle_ColumnHeader_6, Me._lvwLifestyle_ColumnHeader_7})
        Me.lvwLifestyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLifestyle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwLifestyle.Location = New System.Drawing.Point(8, 16)
        Me.lvwLifestyle.Name = "lvwLifestyle"
        Me.lvwLifestyle.Size = New System.Drawing.Size(561, 81)
        Me.lvwLifestyle.SmallImageList = Me.ImageList2
        Me.lvwLifestyle.TabIndex = 69
        Me.lvwLifestyle.UseCompatibleStateImageBehavior = False
        Me.lvwLifestyle.View = System.Windows.Forms.View.Details
        '
        '_lvwLifestyle_ColumnHeader_1
        '
        Me._lvwLifestyle_ColumnHeader_1.Tag = ""
        Me._lvwLifestyle_ColumnHeader_1.Text = "Relationship"
        Me._lvwLifestyle_ColumnHeader_1.Width = 97
        '
        '_lvwLifestyle_ColumnHeader_2
        '
        Me._lvwLifestyle_ColumnHeader_2.Tag = ""
        Me._lvwLifestyle_ColumnHeader_2.Text = "Name"
        Me._lvwLifestyle_ColumnHeader_2.Width = 134
        '
        '_lvwLifestyle_ColumnHeader_3
        '
        Me._lvwLifestyle_ColumnHeader_3.Tag = ""
        Me._lvwLifestyle_ColumnHeader_3.Text = "Date Of Birth"
        Me._lvwLifestyle_ColumnHeader_3.Width = 97
        '
        '_lvwLifestyle_ColumnHeader_4
        '
        Me._lvwLifestyle_ColumnHeader_4.Tag = ""
        Me._lvwLifestyle_ColumnHeader_4.Text = "Gender"
        Me._lvwLifestyle_ColumnHeader_4.Width = 67
        '
        '_lvwLifestyle_ColumnHeader_5
        '
        Me._lvwLifestyle_ColumnHeader_5.Tag = ""
        Me._lvwLifestyle_ColumnHeader_5.Text = "Occupation"
        Me._lvwLifestyle_ColumnHeader_5.Width = 97
        '
        '_lvwLifestyle_ColumnHeader_6
        '
        Me._lvwLifestyle_ColumnHeader_6.Tag = ""
        Me._lvwLifestyle_ColumnHeader_6.Text = "Second Occupation"
        Me._lvwLifestyle_ColumnHeader_6.Width = 97
        '
        '_lvwLifestyle_ColumnHeader_7
        '
        Me._lvwLifestyle_ColumnHeader_7.Tag = ""
        Me._lvwLifestyle_ColumnHeader_7.Text = "Smoker"
        Me._lvwLifestyle_ColumnHeader_7.Width = 67
        '
        '_TabMainTab_TabPage5
        '
        Me._TabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._TabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._TabMainTab_TabPage5.Controls.Add(Me.fraLoyaltySchemes)
        Me._TabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage5.Name = "_TabMainTab_TabPage5"
        Me._TabMainTab_TabPage5.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage5.TabIndex = 5
        Me._TabMainTab_TabPage5.Text = "6 - Misc"
        Me._TabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_5.TabIndex = 81
        Me._cmdNext_5.Text = "&>>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 80
        Me._cmdPrevious_4.TabStop = False
        Me._cmdPrevious_4.Text = "<&<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        'fraLoyaltySchemes
        '
        Me.fraLoyaltySchemes.BackColor = System.Drawing.SystemColors.Control
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdEditLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdDeleteLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdAddLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.lvwLoyaltySchemes)
        Me.fraLoyaltySchemes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLoyaltySchemes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLoyaltySchemes.Location = New System.Drawing.Point(12, 40)
        Me.fraLoyaltySchemes.Name = "fraLoyaltySchemes"
        Me.fraLoyaltySchemes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLoyaltySchemes.Size = New System.Drawing.Size(577, 281)
        Me.fraLoyaltySchemes.TabIndex = 131
        Me.fraLoyaltySchemes.TabStop = False
        Me.fraLoyaltySchemes.Text = "{Loyalty Schemes}"
        '
        'cmdEditLoyaltyScheme
        '
        Me.cmdEditLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditLoyaltyScheme.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditLoyaltyScheme.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditLoyaltyScheme.Name = "cmdEditLoyaltyScheme"
        Me.cmdEditLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditLoyaltyScheme.TabIndex = 79
        Me.cmdEditLoyaltyScheme.Text = "{&Edit}"
        Me.cmdEditLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'cmdDeleteLoyaltyScheme
        '
        Me.cmdDeleteLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteLoyaltyScheme.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteLoyaltyScheme.Location = New System.Drawing.Point(88, 248)
        Me.cmdDeleteLoyaltyScheme.Name = "cmdDeleteLoyaltyScheme"
        Me.cmdDeleteLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteLoyaltyScheme.TabIndex = 78
        Me.cmdDeleteLoyaltyScheme.Text = "{&Delete}"
        Me.cmdDeleteLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'cmdAddLoyaltyScheme
        '
        Me.cmdAddLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddLoyaltyScheme.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddLoyaltyScheme.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddLoyaltyScheme.Name = "cmdAddLoyaltyScheme"
        Me.cmdAddLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddLoyaltyScheme.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddLoyaltyScheme.TabIndex = 77
        Me.cmdAddLoyaltyScheme.Text = "{&Add}"
        Me.cmdAddLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddLoyaltyScheme.UseVisualStyleBackColor = False
        '
        'lvwLoyaltySchemes
        '
        Me.lvwLoyaltySchemes.BackColor = System.Drawing.SystemColors.Window
        Me.lvwLoyaltySchemes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwLoyaltySchemes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLoyaltySchemes_ColumnHeader_1, Me._lvwLoyaltySchemes_ColumnHeader_2, Me._lvwLoyaltySchemes_ColumnHeader_3, Me._lvwLoyaltySchemes_ColumnHeader_4})
        Me.lvwLoyaltySchemes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLoyaltySchemes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwLoyaltySchemes.HideSelection = False
        Me.lvwLoyaltySchemes.LargeImageList = Me.ImageList2
        Me.lvwLoyaltySchemes.Location = New System.Drawing.Point(8, 22)
        Me.lvwLoyaltySchemes.Name = "lvwLoyaltySchemes"
        Me.lvwLoyaltySchemes.Size = New System.Drawing.Size(561, 217)
        Me.lvwLoyaltySchemes.SmallImageList = Me.ImageList2
        Me.lvwLoyaltySchemes.TabIndex = 76
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
        '_TabMainTab_TabPage6
        '
        Me._TabMainTab_TabPage6.Controls.Add(Me.fraPolicies)
        Me._TabMainTab_TabPage6.Controls.Add(Me.fraCampaign)
        Me._TabMainTab_TabPage6.Controls.Add(Me.fraProspect)
        Me._TabMainTab_TabPage6.Controls.Add(Me.fraPreviousInsurer)
        Me._TabMainTab_TabPage6.Controls.Add(Me.fraPreviousBroker)
        Me._TabMainTab_TabPage6.Controls.Add(Me.cboPolicyType)
        Me._TabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage6.Name = "_TabMainTab_TabPage6"
        Me._TabMainTab_TabPage6.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage6.TabIndex = 6
        Me._TabMainTab_TabPage6.Text = "7 - Prospecting"
        Me._TabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'fraPolicies
        '
        Me.fraPolicies.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicies.Controls.Add(Me.cmdEditPol)
        Me.fraPolicies.Controls.Add(Me.cmdDeletePol)
        Me.fraPolicies.Controls.Add(Me.cmdAddPol)
        Me.fraPolicies.Controls.Add(Me.lvwPolicies)
        Me.fraPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicies.Location = New System.Drawing.Point(304, 204)
        Me.fraPolicies.Name = "fraPolicies"
        Me.fraPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicies.Size = New System.Drawing.Size(289, 121)
        Me.fraPolicies.TabIndex = 172
        Me.fraPolicies.TabStop = False
        Me.fraPolicies.Text = "Policies"
        '
        'cmdEditPol
        '
        Me.cmdEditPol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditPol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditPol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditPol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditPol.Location = New System.Drawing.Point(208, 88)
        Me.cmdEditPol.Name = "cmdEditPol"
        Me.cmdEditPol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditPol.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditPol.TabIndex = 176
        Me.cmdEditPol.Text = "&Edit"
        Me.cmdEditPol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditPol.UseVisualStyleBackColor = False
        '
        'cmdDeletePol
        '
        Me.cmdDeletePol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeletePol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeletePol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeletePol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeletePol.Location = New System.Drawing.Point(128, 88)
        Me.cmdDeletePol.Name = "cmdDeletePol"
        Me.cmdDeletePol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeletePol.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeletePol.TabIndex = 175
        Me.cmdDeletePol.Text = "&Delete"
        Me.cmdDeletePol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeletePol.UseVisualStyleBackColor = False
        '
        'cmdAddPol
        '
        Me.cmdAddPol.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddPol.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddPol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddPol.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddPol.Location = New System.Drawing.Point(48, 88)
        Me.cmdAddPol.Name = "cmdAddPol"
        Me.cmdAddPol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddPol.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddPol.TabIndex = 174
        Me.cmdAddPol.Text = "&Add"
        Me.cmdAddPol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddPol.UseVisualStyleBackColor = False
        '
        'lvwPolicies
        '
        Me.lvwPolicies.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicies.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwPolicies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicies_ColumnHeader_1, Me._lvwPolicies_ColumnHeader_2, Me._lvwPolicies_ColumnHeader_3, Me._lvwPolicies_ColumnHeader_4, Me._lvwPolicies_ColumnHeader_5})
        Me.lvwPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicies.HideSelection = False
        Me.lvwPolicies.LargeImageList = Me.ImageList2
        Me.lvwPolicies.Location = New System.Drawing.Point(8, 24)
        Me.lvwPolicies.Name = "lvwPolicies"
        Me.lvwPolicies.Size = New System.Drawing.Size(272, 57)
        Me.lvwPolicies.SmallImageList = Me.ImageList2
        Me.lvwPolicies.TabIndex = 173
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
        Me.fraCampaign.BackColor = System.Drawing.SystemColors.Control
        Me.fraCampaign.Controls.Add(Me.lvwCampaigns)
        Me.fraCampaign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCampaign.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCampaign.Location = New System.Drawing.Point(8, 204)
        Me.fraCampaign.Name = "fraCampaign"
        Me.fraCampaign.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCampaign.Size = New System.Drawing.Size(289, 121)
        Me.fraCampaign.TabIndex = 170
        Me.fraCampaign.TabStop = False
        Me.fraCampaign.Text = "Campaigns"
        '
        'lvwCampaigns
        '
        Me.lvwCampaigns.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCampaigns.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwCampaigns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCampaigns_ColumnHeader_1, Me._lvwCampaigns_ColumnHeader_2, Me._lvwCampaigns_ColumnHeader_3})
        Me.lvwCampaigns.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCampaigns.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCampaigns.HideSelection = False
        Me.lvwCampaigns.LargeImageList = Me.ImageList2
        Me.lvwCampaigns.Location = New System.Drawing.Point(8, 24)
        Me.lvwCampaigns.Name = "lvwCampaigns"
        Me.lvwCampaigns.Size = New System.Drawing.Size(263, 81)
        Me.lvwCampaigns.SmallImageList = Me.ImageList2
        Me.lvwCampaigns.TabIndex = 171
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
        Me.fraProspect.BackColor = System.Drawing.SystemColors.Control
        Me.fraProspect.Controls.Add(Me.cmdCurrentAgent)
        Me.fraProspect.Controls.Add(Me.txtAgentReference)
        Me.fraProspect.Controls.Add(Me.cboProspectingStatus)
        Me.fraProspect.Controls.Add(Me.cboStrengthCode)
        Me.fraProspect.Controls.Add(Me.pnlCurrentAgent)
        Me.fraProspect.Controls.Add(Me.lblAgentReference)
        Me.fraProspect.Controls.Add(Me.lblProspectStatus)
        Me.fraProspect.Controls.Add(Me.lblStrengthCode)
        Me.fraProspect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProspect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProspect.Location = New System.Drawing.Point(8, 20)
        Me.fraProspect.Name = "fraProspect"
        Me.fraProspect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraProspect.Size = New System.Drawing.Size(585, 73)
        Me.fraProspect.TabIndex = 150
        Me.fraProspect.TabStop = False
        '
        'cmdCurrentAgent
        '
        Me.cmdCurrentAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCurrentAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCurrentAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCurrentAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCurrentAgent.Location = New System.Drawing.Point(8, 40)
        Me.cmdCurrentAgent.Name = "cmdCurrentAgent"
        Me.cmdCurrentAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCurrentAgent.Size = New System.Drawing.Size(105, 21)
        Me.cmdCurrentAgent.TabIndex = 155
        Me.cmdCurrentAgent.Text = "Current Agent..."
        Me.cmdCurrentAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCurrentAgent.UseVisualStyleBackColor = False
        '
        'txtAgentReference
        '
        Me.txtAgentReference.AcceptsReturn = True
        Me.txtAgentReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentReference.Location = New System.Drawing.Point(128, 16)
        Me.txtAgentReference.MaxLength = 0
        Me.txtAgentReference.Name = "txtAgentReference"
        Me.txtAgentReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentReference.Size = New System.Drawing.Size(161, 21)
        Me.txtAgentReference.TabIndex = 152
        '
        'cboProspectingStatus
        '
        Me.cboProspectingStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboProspectingStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProspectingStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProspectingStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProspectingStatus.Location = New System.Drawing.Point(416, 40)
        Me.cboProspectingStatus.Name = "cboProspectingStatus"
        Me.cboProspectingStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProspectingStatus.Size = New System.Drawing.Size(161, 21)
        Me.cboProspectingStatus.TabIndex = 158
        '
        'cboStrengthCode
        '
        Me.cboStrengthCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboStrengthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStrengthCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStrengthCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStrengthCode.Location = New System.Drawing.Point(416, 16)
        Me.cboStrengthCode.Name = "cboStrengthCode"
        Me.cboStrengthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStrengthCode.Size = New System.Drawing.Size(161, 21)
        Me.cboStrengthCode.TabIndex = 154
        '
        'pnlCurrentAgent
        '
        Me.pnlCurrentAgent.BackColor = System.Drawing.SystemColors.Control
        Me.pnlCurrentAgent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCurrentAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlCurrentAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCurrentAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlCurrentAgent.Location = New System.Drawing.Point(128, 40)
        Me.pnlCurrentAgent.Name = "pnlCurrentAgent"
        Me.pnlCurrentAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlCurrentAgent.Size = New System.Drawing.Size(161, 19)
        Me.pnlCurrentAgent.TabIndex = 156
        '
        'lblAgentReference
        '
        Me.lblAgentReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentReference.Location = New System.Drawing.Point(8, 19)
        Me.lblAgentReference.Name = "lblAgentReference"
        Me.lblAgentReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentReference.Size = New System.Drawing.Size(105, 17)
        Me.lblAgentReference.TabIndex = 151
        Me.lblAgentReference.Text = "Agent Reference:"
        '
        'lblProspectStatus
        '
        Me.lblProspectStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblProspectStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProspectStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProspectStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProspectStatus.Location = New System.Drawing.Point(304, 40)
        Me.lblProspectStatus.Name = "lblProspectStatus"
        Me.lblProspectStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProspectStatus.Size = New System.Drawing.Size(113, 17)
        Me.lblProspectStatus.TabIndex = 157
        Me.lblProspectStatus.Text = "Status:"
        '
        'lblStrengthCode
        '
        Me.lblStrengthCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblStrengthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStrengthCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStrengthCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStrengthCode.Location = New System.Drawing.Point(304, 16)
        Me.lblStrengthCode.Name = "lblStrengthCode"
        Me.lblStrengthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStrengthCode.Size = New System.Drawing.Size(121, 17)
        Me.lblStrengthCode.TabIndex = 153
        Me.lblStrengthCode.Text = "Strength Code:"
        '
        'fraPreviousInsurer
        '
        Me.fraPreviousInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreviousInsurer.Controls.Add(Me.cmdInsurerLookup)
        Me.fraPreviousInsurer.Controls.Add(Me.txtInsurerRef)
        Me.fraPreviousInsurer.Controls.Add(Me.pnlInsurerName)
        Me.fraPreviousInsurer.Controls.Add(Me.lblInsurerName)
        Me.fraPreviousInsurer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPreviousInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreviousInsurer.Location = New System.Drawing.Point(8, 116)
        Me.fraPreviousInsurer.Name = "fraPreviousInsurer"
        Me.fraPreviousInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousInsurer.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousInsurer.TabIndex = 159
        Me.fraPreviousInsurer.TabStop = False
        Me.fraPreviousInsurer.Text = "Previous Insurer"
        '
        'cmdInsurerLookup
        '
        Me.cmdInsurerLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsurerLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsurerLookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsurerLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsurerLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdInsurerLookup.Name = "cmdInsurerLookup"
        Me.cmdInsurerLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsurerLookup.Size = New System.Drawing.Size(60, 21)
        Me.cmdInsurerLookup.TabIndex = 160
        Me.cmdInsurerLookup.Text = "Code..."
        Me.cmdInsurerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurerLookup.UseVisualStyleBackColor = False
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
        Me.txtInsurerRef.TabIndex = 161
        '
        'pnlInsurerName
        '
        Me.pnlInsurerName.BackColor = System.Drawing.SystemColors.Control
        Me.pnlInsurerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlInsurerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlInsurerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlInsurerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlInsurerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlInsurerName.Name = "pnlInsurerName"
        Me.pnlInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlInsurerName.Size = New System.Drawing.Size(201, 19)
        Me.pnlInsurerName.TabIndex = 164
        '
        'lblInsurerName
        '
        Me.lblInsurerName.AutoSize = True
        Me.lblInsurerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerName.Location = New System.Drawing.Point(8, 43)
        Me.lblInsurerName.Name = "lblInsurerName"
        Me.lblInsurerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerName.Size = New System.Drawing.Size(45, 13)
        Me.lblInsurerName.TabIndex = 162
        Me.lblInsurerName.Text = "Name:"
        '
        'fraPreviousBroker
        '
        Me.fraPreviousBroker.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreviousBroker.Controls.Add(Me.txtBrokerRef)
        Me.fraPreviousBroker.Controls.Add(Me.cmdBrokerLookup)
        Me.fraPreviousBroker.Controls.Add(Me.pnlBrokerName)
        Me.fraPreviousBroker.Controls.Add(Me.lblBrokerName)
        Me.fraPreviousBroker.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPreviousBroker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreviousBroker.Location = New System.Drawing.Point(304, 116)
        Me.fraPreviousBroker.Name = "fraPreviousBroker"
        Me.fraPreviousBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousBroker.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousBroker.TabIndex = 165
        Me.fraPreviousBroker.TabStop = False
        Me.fraPreviousBroker.Text = "Previous Broker"
        '
        'txtBrokerRef
        '
        Me.txtBrokerRef.AcceptsReturn = True
        Me.txtBrokerRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerRef.Location = New System.Drawing.Point(80, 16)
        Me.txtBrokerRef.MaxLength = 0
        Me.txtBrokerRef.Name = "txtBrokerRef"
        Me.txtBrokerRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerRef.Size = New System.Drawing.Size(201, 21)
        Me.txtBrokerRef.TabIndex = 167
        '
        'cmdBrokerLookup
        '
        Me.cmdBrokerLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrokerLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrokerLookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrokerLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrokerLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdBrokerLookup.Name = "cmdBrokerLookup"
        Me.cmdBrokerLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrokerLookup.Size = New System.Drawing.Size(60, 20)
        Me.cmdBrokerLookup.TabIndex = 166
        Me.cmdBrokerLookup.Text = "Code..."
        Me.cmdBrokerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrokerLookup.UseVisualStyleBackColor = False
        '
        'pnlBrokerName
        '
        Me.pnlBrokerName.BackColor = System.Drawing.SystemColors.Control
        Me.pnlBrokerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBrokerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlBrokerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlBrokerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlBrokerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlBrokerName.Name = "pnlBrokerName"
        Me.pnlBrokerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlBrokerName.Size = New System.Drawing.Size(201, 19)
        Me.pnlBrokerName.TabIndex = 169
        '
        'lblBrokerName
        '
        Me.lblBrokerName.AutoSize = True
        Me.lblBrokerName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerName.Location = New System.Drawing.Point(8, 43)
        Me.lblBrokerName.Name = "lblBrokerName"
        Me.lblBrokerName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerName.Size = New System.Drawing.Size(45, 13)
        Me.lblBrokerName.TabIndex = 168
        Me.lblBrokerName.Text = "Name:"
        '
        'cboPolicyType
        '
        Me.cboPolicyType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyType.Location = New System.Drawing.Point(424, 348)
        Me.cboPolicyType.Name = "cboPolicyType"
        Me.cboPolicyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyType.Size = New System.Drawing.Size(169, 21)
        Me.cboPolicyType.TabIndex = 177
        Me.cboPolicyType.TabStop = False
        Me.cboPolicyType.Visible = False
        '
        '_TabMainTab_TabPage7
        '
        Me._TabMainTab_TabPage7.Controls.Add(Me.Label1)
        Me._TabMainTab_TabPage7.Controls.Add(Me.uctPartyTax1)
        Me._TabMainTab_TabPage7.Controls.Add(Me._cmdPrevious_5)
        Me._TabMainTab_TabPage7.Controls.Add(Me._cmdNext_6)
        Me._TabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage7.Name = "_TabMainTab_TabPage7"
        Me._TabMainTab_TabPage7.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage7.TabIndex = 7
        Me._TabMainTab_TabPage7.Text = "8 - Tax"
        Me._TabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 188
        Me.Label1.Text = "Party Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(3, 7)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(585, 233)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 179
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0.0R
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_5.TabIndex = 178
        Me._cmdPrevious_5.TabStop = False
        Me._cmdPrevious_5.Text = "<&<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        '_cmdNext_6
        '
        Me._cmdNext_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_6.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_6.Name = "_cmdNext_6"
        Me._cmdNext_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_6.TabIndex = 180
        Me._cmdNext_6.Text = "&>>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        '_TabMainTab_TabPage8
        '
        Me._TabMainTab_TabPage8.Controls.Add(Me.uctPartyBankControl1)
        Me._TabMainTab_TabPage8.Controls.Add(Me._cmdPrevious_6)
        Me._TabMainTab_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._TabMainTab_TabPage8.Name = "_TabMainTab_TabPage8"
        Me._TabMainTab_TabPage8.Size = New System.Drawing.Size(710, 409)
        Me._TabMainTab_TabPage8.TabIndex = 8
        Me._TabMainTab_TabPage8.Text = "8 - Bank"
        Me._TabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = CType(0, Byte)
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(16, 20)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = CType(0, Byte)
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(689, 345)
        Me.uctPartyBankControl1.TabIndex = 188
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_6.TabIndex = 181
        Me._cmdPrevious_6.TabStop = False
        Me._cmdPrevious_6.Text = "<&<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
        '
        '_cmdNext_7
        '
        Me._cmdNext_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_7.Location = New System.Drawing.Point(600, 372)
        Me._cmdNext_7.Name = "_cmdNext_7"
        Me._cmdNext_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_7.TabIndex = 187
        Me._cmdNext_7.Text = "&>>"
        Me._cmdNext_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_7.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_7
        '
        Me._cmdPrevious_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_7.Location = New System.Drawing.Point(16, 372)
        Me._cmdPrevious_7.Name = "_cmdPrevious_7"
        Me._cmdPrevious_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_7.TabIndex = 186
        Me._cmdPrevious_7.TabStop = False
        Me._cmdPrevious_7.Text = "<&<"
        Me._cmdPrevious_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_7.UseVisualStyleBackColor = False
        '
        'uctPartyPCControl
        '
        Me.Controls.Add(Me.TabMainTab)
        Me.Location = New System.Drawing.Point(3, 3)
        Me.Name = "uctPartyPCControl"
        Me.Size = New System.Drawing.Size(718, 439)
        Me.TabMainTab.ResumeLayout(False)
        Me._TabMainTab_TabPage0.ResumeLayout(False)
        Me._TabMainTab_TabPage0.PerformLayout()
        Me.fraAreaCode.ResumeLayout(False)
        Me.fraAreaCode.PerformLayout()
        Me.fraConsultant.ResumeLayout(False)
        Me.fraConsultant.PerformLayout()
        Me.fraAccounts.ResumeLayout(False)
        Me.fraClient.ResumeLayout(False)
        Me.fraClient.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraAgent.ResumeLayout(False)
        Me.fraAgent.PerformLayout()
        Me.fraBlackList.ResumeLayout(False)
        Me.fraBlackList.PerformLayout()
        Me._TabMainTab_TabPage1.ResumeLayout(False)
        Me.fraCorrespondence.ResumeLayout(False)
        Me.fraCorrespondence.PerformLayout()
        Me.fraAddress.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._TabMainTab_TabPage2.ResumeLayout(False)
        Me.fraPaymentDetails.ResumeLayout(False)
        Me.fraPaymentDetails.PerformLayout()
        Me.fraEmploymentDetails.ResumeLayout(False)
        Me.fraEmploymentDetails.PerformLayout()
        Me.fraFSA.ResumeLayout(False)
        Me.fraFSA.PerformLayout()
        Me._TabMainTab_TabPage3.ResumeLayout(False)
        Me._TabMainTab_TabPage3.PerformLayout()
        Me.frmContacts.ResumeLayout(False)
        Me._TabMainTab_TabPage4.ResumeLayout(False)
        Me.fraLifestyle.ResumeLayout(False)
        Me.fraLifestyle.PerformLayout()
        Me.fraDependants.ResumeLayout(False)
        Me._TabMainTab_TabPage5.ResumeLayout(False)
        Me.fraLoyaltySchemes.ResumeLayout(False)
        Me._TabMainTab_TabPage6.ResumeLayout(False)
        Me.fraPolicies.ResumeLayout(False)
        Me.fraCampaign.ResumeLayout(False)
        Me.fraProspect.ResumeLayout(False)
        Me.fraProspect.PerformLayout()
        Me.fraPreviousInsurer.ResumeLayout(False)
        Me.fraPreviousInsurer.PerformLayout()
        Me.fraPreviousBroker.ResumeLayout(False)
        Me.fraPreviousBroker.PerformLayout()
        Me._TabMainTab_TabPage7.ResumeLayout(False)
        Me._TabMainTab_TabPage7.PerformLayout()
        Me._TabMainTab_TabPage8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(7) = _cmdPrevious_7
        Me.cmdPrevious(6) = _cmdPrevious_6
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(0) = _cmdPrevious_0
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(2) = _cmdPrevious_2
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(7) = _cmdNext_7
        Me.cmdNext(6) = _cmdNext_6
        Me.cmdNext(5) = _cmdNext_5
        Me.cmdNext(4) = _cmdNext_4
        Me.cmdNext(0) = _cmdNext_0
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(3) = _cmdNext_3
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
    Sub lvwLifestyle_InitializeColumnKeys()
        Me._lvwLifestyle_ColumnHeader_1.Name = ""
        Me._lvwLifestyle_ColumnHeader_2.Name = ""
        Me._lvwLifestyle_ColumnHeader_3.Name = ""
        Me._lvwLifestyle_ColumnHeader_4.Name = ""
        Me._lvwLifestyle_ColumnHeader_5.Name = ""
        Me._lvwLifestyle_ColumnHeader_6.Name = ""
        Me._lvwLifestyle_ColumnHeader_7.Name = ""
    End Sub
    Sub lvwConvictions_InitializeColumnKeys()
        Me._lvwConvictions_ColumnHeader_1.Name = ""
        Me._lvwConvictions_ColumnHeader_2.Name = ""
        Me._lvwConvictions_ColumnHeader_3.Name = ""
        Me._lvwConvictions_ColumnHeader_4.Name = ""
        Me._lvwConvictions_ColumnHeader_5.Name = ""
        Me._lvwConvictions_ColumnHeader_6.Name = ""
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTermsOfPayment As System.Windows.Forms.Label
    Friend WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
    Friend WithEvents lblAssociates As System.Windows.Forms.Label
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
