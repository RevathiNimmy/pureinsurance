<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyCCControl
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
    Friend WithEvents cboArea As System.Windows.Forms.ComboBox
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents fraAreaCode As System.Windows.Forms.GroupBox
    Friend WithEvents txtConsultantRef As System.Windows.Forms.TextBox
    Friend WithEvents cmdConsultantLookup As System.Windows.Forms.Button
    Friend WithEvents pnlConsultantName As System.Windows.Forms.Panel
    Friend WithEvents lblConsultantName As System.Windows.Forms.Label
    Friend WithEvents fraConsultant As System.Windows.Forms.GroupBox
    Friend WithEvents txtEmployees As System.Windows.Forms.TextBox
    Friend WithEvents cmdMembershipGroups As System.Windows.Forms.Button
    Friend WithEvents ddTrade As PMListMgrDropdown.uctDropdown
    Friend WithEvents cboTrade As PMLookupControl.cboPMLookup
    Friend WithEvents cboEmployees As PMLookupControl.cboPMLookup
    Friend WithEvents cboSICCode As System.Windows.Forms.ComboBox
    Friend WithEvents ddBusiness As PMListMgrDropdown.uctDropdown
    Friend WithEvents txtTradingSince As System.Windows.Forms.TextBox
    Friend WithEvents txtOffices As System.Windows.Forms.TextBox
    Friend WithEvents lblSICCode As System.Windows.Forms.Label
    Friend WithEvents lblTradingSince As System.Windows.Forms.Label
    Friend WithEvents lblTrade As System.Windows.Forms.Label
    Friend WithEvents lblEmployees As System.Windows.Forms.Label
    Friend WithEvents lblOffices As System.Windows.Forms.Label
    Friend WithEvents lblBusiness As System.Windows.Forms.Label
    Friend WithEvents fraBusinessDetails As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Friend WithEvents txtTradingName As System.Windows.Forms.TextBox
    Friend WithEvents txtCompanyReg As System.Windows.Forms.TextBox
    Friend WithEvents txtMainContact As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtIDReference As System.Windows.Forms.TextBox
    Friend WithEvents lblCompanyReg As System.Windows.Forms.Label
    Friend WithEvents lblMainContact As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblIDReference As System.Windows.Forms.Label
    Friend WithEvents fraClient As System.Windows.Forms.GroupBox
    Friend WithEvents txtAgentRef As System.Windows.Forms.TextBox
    Friend WithEvents cmdAgentLookUp As System.Windows.Forms.Button
    Friend WithEvents txtFileCode As System.Windows.Forms.TextBox
    Friend WithEvents txtRecordStatus As System.Windows.Forms.TextBox
    Friend WithEvents pnlAgentName As System.Windows.Forms.Panel
    Friend WithEvents lblAgentName As System.Windows.Forms.Label
    Friend WithEvents lblRecordStatus As System.Windows.Forms.Label
    Friend WithEvents lblFileCode As System.Windows.Forms.Label
    Friend WithEvents fraAgent As System.Windows.Forms.GroupBox
    Friend WithEvents chkFeeClient As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgent As System.Windows.Forms.CheckBox
    Friend WithEvents chkProspect As System.Windows.Forms.CheckBox
    Friend WithEvents pnlClientBalance As System.Windows.Forms.Panel
    Friend WithEvents pnlYearToDateTurnover As System.Windows.Forms.Panel
    Friend WithEvents pnlLastYearTurnover As System.Windows.Forms.Panel
    Friend WithEvents lblLastYearTurnover As System.Windows.Forms.Label
    Friend WithEvents lblYearToDateTurnover As System.Windows.Forms.Label
    Friend WithEvents lblClientBalance As System.Windows.Forms.Label
    Friend WithEvents Frame1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboBranch As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Friend WithEvents cboServiceLevel As System.Windows.Forms.ComboBox
    Friend WithEvents txtAlternativeIdentifier As System.Windows.Forms.TextBox
    Friend WithEvents lblBranch As System.Windows.Forms.Label
    Friend WithEvents lblServicelevel As System.Windows.Forms.Label
    Friend WithEvents lblSubBranch As System.Windows.Forms.Label
    Friend WithEvents lblAlternativeIdentifier As System.Windows.Forms.Label
    Friend WithEvents Frame2 As System.Windows.Forms.GroupBox
    Friend WithEvents cboBlackListReason As System.Windows.Forms.ComboBox
    Friend WithEvents lblBlacklistReason As System.Windows.Forms.Label
    Friend WithEvents fraBlackList As System.Windows.Forms.GroupBox
    Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Friend WithEvents txtSalutation As System.Windows.Forms.TextBox
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
    Friend WithEvents lblSalutation As System.Windows.Forms.Label
    Friend WithEvents fraAddress As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cboCorrespondenceType As System.Windows.Forms.ComboBox
    Friend WithEvents chkMailshot As System.Windows.Forms.CheckBox
    Friend WithEvents chkTPS As System.Windows.Forms.CheckBox
    Friend WithEvents chkeMPS As System.Windows.Forms.CheckBox
    Friend WithEvents lblPreferredCorrespondence As System.Windows.Forms.Label
    Friend WithEvents lblTPS As System.Windows.Forms.Label
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
    Friend WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtHiddenDate As System.Windows.Forms.TextBox
    Friend WithEvents txtHiddenCurrency As System.Windows.Forms.TextBox
    Friend WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Friend WithEvents txtCCJ As System.Windows.Forms.TextBox
    Friend WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
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
    Friend WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents txtTobLetter As System.Windows.Forms.TextBox
    Friend WithEvents txtRealFileCode As System.Windows.Forms.TextBox
    Friend WithEvents cboRealArea As System.Windows.Forms.ComboBox
    Friend WithEvents lblTobLetter As System.Windows.Forms.Label
    Friend WithEvents lblRealFileCode As System.Windows.Forms.Label
    Friend WithEvents lblRealArea As System.Windows.Forms.Label
    Friend WithEvents fraRealArea As System.Windows.Forms.GroupBox
    'Friend WithEvents cboCurrency As AxUserControls.AxCurrencyLookup
    Friend WithEvents cboCurrency As UserControls.CurrencyLookup
    Friend WithEvents txtSource As System.Windows.Forms.TextBox
    Friend WithEvents txtTPPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtLoyaltyNumberPrefix As System.Windows.Forms.TextBox
    Friend WithEvents txtLoyaltyNumber As System.Windows.Forms.TextBox
    Friend WithEvents cboRenewalStopCode As System.Windows.Forms.ComboBox
    Friend WithEvents cboSeasonalGift As System.Windows.Forms.ComboBox
    Friend WithEvents ddPaymentMethod As PMListMgrDropdown.uctDropdown
    Friend WithEvents cboCreditCard As System.Windows.Forms.ComboBox
    Friend WithEvents cboReminderType As System.Windows.Forms.ComboBox
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents lblTPPassword As System.Windows.Forms.Label
    Friend WithEvents lblLoyaltyNumber As System.Windows.Forms.Label
    Friend WithEvents lblRenewalStopCode As System.Windows.Forms.Label
    Friend WithEvents lblSeasonalGift As System.Windows.Forms.Label
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents lblTermsOfPayment As System.Windows.Forms.Label
    Friend WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Friend WithEvents lblCreditCard As System.Windows.Forms.Label
    Friend WithEvents lblReminderType As System.Windows.Forms.Label
    Friend WithEvents fraTab5_1 As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Friend WithEvents txtTurnover As System.Windows.Forms.TextBox
    Friend WithEvents cboTurnover As PMLookupControl.cboPMLookup
    Friend WithEvents txtFinancialYear As System.Windows.Forms.TextBox
    Friend WithEvents txtWageRoll As System.Windows.Forms.TextBox
    Friend WithEvents lblFinancialYear As System.Windows.Forms.Label
    Friend WithEvents lblTurnover As System.Windows.Forms.Label
    Friend WithEvents lblWageRoll As System.Windows.Forms.Label
    Friend WithEvents fraNumbers As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Friend WithEvents cmdAssociates As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents cmdAddLoyaltyScheme As System.Windows.Forms.Button
    Friend WithEvents cmdDeleteLoyaltyScheme As System.Windows.Forms.Button
    Friend WithEvents cmdEditLoyaltyScheme As System.Windows.Forms.Button
    Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwLoyaltySchemes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwLoyaltySchemes As System.Windows.Forms.ListView
    Friend WithEvents fraLoyaltySchemes As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_5 As System.Windows.Forms.Button
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
    Friend WithEvents cboProspectingStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtAgentReference As System.Windows.Forms.TextBox
    Friend WithEvents cmdCurrentAgent As System.Windows.Forms.Button
    Friend WithEvents cboStrengthCode As System.Windows.Forms.ComboBox
    Friend WithEvents pnlCurrentAgent As System.Windows.Forms.Panel
    Friend WithEvents lblProspectStatus As System.Windows.Forms.Label
    Friend WithEvents lblAgentReference As System.Windows.Forms.Label
    Friend WithEvents lblStrengthCode As System.Windows.Forms.Label
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
    Friend WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_6 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
    Friend WithEvents _cmdPrevious_7 As System.Windows.Forms.Button
    Friend WithEvents _cmdNext_7 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents _cmdPrevious_8 As System.Windows.Forms.Button
    Friend WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
    Friend WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend cmdNext(7) As System.Windows.Forms.Button
    Friend cmdPrevious(8) As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartyCCControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblAssociates = New System.Windows.Forms.Label()
        Me.fraAreaCode = New System.Windows.Forms.GroupBox()
        Me.cboArea = New System.Windows.Forms.ComboBox()
        Me.lblArea = New System.Windows.Forms.Label()
        Me.fraConsultant = New System.Windows.Forms.GroupBox()
        Me.txtConsultantRef = New System.Windows.Forms.TextBox()
        Me.cmdConsultantLookup = New System.Windows.Forms.Button()
        Me.pnlConsultantName = New System.Windows.Forms.Panel()
        Me.lblPnlConsultantName = New System.Windows.Forms.Label()
        Me.lblConsultantName = New System.Windows.Forms.Label()
        Me.fraBusinessDetails = New System.Windows.Forms.GroupBox()
        Me.lblEmployees = New System.Windows.Forms.Label()
        Me.txtEmployees = New System.Windows.Forms.TextBox()
        Me.ddTrade = New PMListMgrDropdown.uctDropdown()
        Me.cboTrade = New PMLookupControl.cboPMLookup()
        Me.cboEmployees = New PMLookupControl.cboPMLookup()
        Me.cboSICCode = New System.Windows.Forms.ComboBox()
        Me.ddBusiness = New PMListMgrDropdown.uctDropdown()
        Me.txtTradingSince = New System.Windows.Forms.TextBox()
        Me.txtOffices = New System.Windows.Forms.TextBox()
        Me.cmdMembershipGroups = New System.Windows.Forms.Button()
        Me.lblSICCode = New System.Windows.Forms.Label()
        Me.lblTradingSince = New System.Windows.Forms.Label()
        Me.lblTrade = New System.Windows.Forms.Label()
        Me.lblOffices = New System.Windows.Forms.Label()
        Me.lblBusiness = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me.fraClient = New System.Windows.Forms.GroupBox()
        Me.txtTradingName = New System.Windows.Forms.TextBox()
        Me.txtCompanyReg = New System.Windows.Forms.TextBox()
        Me.txtMainContact = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtIDReference = New System.Windows.Forms.TextBox()
        Me.lblCompanyReg = New System.Windows.Forms.Label()
        Me.lblMainContact = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblIDReference = New System.Windows.Forms.Label()
        Me.fraAgent = New System.Windows.Forms.GroupBox()
        Me.txtAgentRef = New System.Windows.Forms.TextBox()
        Me.cmdAgentLookUp = New System.Windows.Forms.Button()
        Me.txtFileCode = New System.Windows.Forms.TextBox()
        Me.txtRecordStatus = New System.Windows.Forms.TextBox()
        Me.pnlAgentName = New System.Windows.Forms.Panel()
        Me.lblPnlAgentName = New System.Windows.Forms.Label()
        Me.lblAgentName = New System.Windows.Forms.Label()
        Me.lblRecordStatus = New System.Windows.Forms.Label()
        Me.lblFileCode = New System.Windows.Forms.Label()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.pnlLastYear = New System.Windows.Forms.Label()
        Me.lblYearToDate = New System.Windows.Forms.Label()
        Me.pnlClient = New System.Windows.Forms.Label()
        Me.imgIcon = New System.Windows.Forms.PictureBox()
        Me.chkFeeClient = New System.Windows.Forms.CheckBox()
        Me.chkAgent = New System.Windows.Forms.CheckBox()
        Me.chkProspect = New System.Windows.Forms.CheckBox()
        Me.pnlClientBalance = New System.Windows.Forms.Panel()
        Me.pnlYearToDateTurnover = New System.Windows.Forms.Panel()
        Me.pnlLastYearTurnover = New System.Windows.Forms.Panel()
        Me.lblLastYearTurnover = New System.Windows.Forms.Label()
        Me.lblYearToDateTurnover = New System.Windows.Forms.Label()
        Me.lblClientBalance = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.cboBranch = New System.Windows.Forms.ComboBox()
        Me.cboSubBranch = New System.Windows.Forms.ComboBox()
        Me.cboServiceLevel = New System.Windows.Forms.ComboBox()
        Me.txtAlternativeIdentifier = New System.Windows.Forms.TextBox()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.lblServicelevel = New System.Windows.Forms.Label()
        Me.lblSubBranch = New System.Windows.Forms.Label()
        Me.lblAlternativeIdentifier = New System.Windows.Forms.Label()
        Me.fraBlackList = New System.Windows.Forms.GroupBox()
        Me.cboBlackListReason = New System.Windows.Forms.ComboBox()
        Me.lblBlacklistReason = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me.fraAddress = New System.Windows.Forms.GroupBox()
        Me.txtSalutation = New System.Windows.Forms.TextBox()
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
        Me.lblSalutation = New System.Windows.Forms.Label()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraCorrespondence = New System.Windows.Forms.GroupBox()
        Me.cboCorrespondenceType = New System.Windows.Forms.ComboBox()
        Me.chkMailshot = New System.Windows.Forms.CheckBox()
        Me.chkTPS = New System.Windows.Forms.CheckBox()
        Me.chkeMPS = New System.Windows.Forms.CheckBox()
        Me.lblPreferredCorrespondence = New System.Windows.Forms.Label()
        Me.lblTPS = New System.Windows.Forms.Label()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
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
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.txtHiddenDate = New System.Windows.Forms.TextBox()
        Me.txtHiddenCurrency = New System.Windows.Forms.TextBox()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me.txtCCJ = New System.Windows.Forms.TextBox()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
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
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fraRealArea = New System.Windows.Forms.GroupBox()
        Me.txtTobLetter = New System.Windows.Forms.TextBox()
        Me.txtRealFileCode = New System.Windows.Forms.TextBox()
        Me.cboRealArea = New System.Windows.Forms.ComboBox()
        Me.lblTobLetter = New System.Windows.Forms.Label()
        Me.lblRealFileCode = New System.Windows.Forms.Label()
        Me.lblRealArea = New System.Windows.Forms.Label()
        Me.fraTab5_1 = New System.Windows.Forms.GroupBox()
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.txtTPPassword = New System.Windows.Forms.TextBox()
        Me.txtLoyaltyNumberPrefix = New System.Windows.Forms.TextBox()
        Me.txtLoyaltyNumber = New System.Windows.Forms.TextBox()
        Me.cboRenewalStopCode = New System.Windows.Forms.ComboBox()
        Me.cboSeasonalGift = New System.Windows.Forms.ComboBox()
        Me.ddPaymentMethod = New PMListMgrDropdown.uctDropdown()
        Me.cboCreditCard = New System.Windows.Forms.ComboBox()
        Me.cboReminderType = New System.Windows.Forms.ComboBox()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.lblTPPassword = New System.Windows.Forms.Label()
        Me.lblLoyaltyNumber = New System.Windows.Forms.Label()
        Me.lblRenewalStopCode = New System.Windows.Forms.Label()
        Me.lblSeasonalGift = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblTermsOfPayment = New System.Windows.Forms.Label()
        Me.lblPaymentMethod = New System.Windows.Forms.Label()
        Me.lblCreditCard = New System.Windows.Forms.Label()
        Me.lblReminderType = New System.Windows.Forms.Label()
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me.fraNumbers = New System.Windows.Forms.GroupBox()
        Me.lblTurnover = New System.Windows.Forms.Label()
        Me.txtTurnover = New System.Windows.Forms.TextBox()
        Me.cboTurnover = New PMLookupControl.cboPMLookup()
        Me.txtFinancialYear = New System.Windows.Forms.TextBox()
        Me.txtWageRoll = New System.Windows.Forms.TextBox()
        Me.lblFinancialYear = New System.Windows.Forms.Label()
        Me.lblWageRoll = New System.Windows.Forms.Label()
        Me._cmdNext_4 = New System.Windows.Forms.Button()
        Me.cmdAssociates = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage()
        Me.fraLoyaltySchemes = New System.Windows.Forms.GroupBox()
        Me.cmdAddLoyaltyScheme = New System.Windows.Forms.Button()
        Me.cmdDeleteLoyaltyScheme = New System.Windows.Forms.Button()
        Me.cmdEditLoyaltyScheme = New System.Windows.Forms.Button()
        Me.lvwLoyaltySchemes = New System.Windows.Forms.ListView()
        Me._lvwLoyaltySchemes_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwLoyaltySchemes_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdPrevious_5 = New System.Windows.Forms.Button()
        Me._cmdNext_5 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage()
        Me.fraPolicies = New System.Windows.Forms.GroupBox()
        Me.cmdAddPol = New System.Windows.Forms.Button()
        Me.cmdDeletePol = New System.Windows.Forms.Button()
        Me.cmdEditPol = New System.Windows.Forms.Button()
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
        Me.cboProspectingStatus = New System.Windows.Forms.ComboBox()
        Me.txtAgentReference = New System.Windows.Forms.TextBox()
        Me.cmdCurrentAgent = New System.Windows.Forms.Button()
        Me.cboStrengthCode = New System.Windows.Forms.ComboBox()
        Me.pnlCurrentAgent = New System.Windows.Forms.Panel()
        Me.lblPnlCurrentAgent = New System.Windows.Forms.Label()
        Me.lblProspectStatus = New System.Windows.Forms.Label()
        Me.lblAgentReference = New System.Windows.Forms.Label()
        Me.lblStrengthCode = New System.Windows.Forms.Label()
        Me.fraPreviousInsurer = New System.Windows.Forms.GroupBox()
        Me.txtInsurerRef = New System.Windows.Forms.TextBox()
        Me.cmdInsurerLookup = New System.Windows.Forms.Button()
        Me.pnlInsurerName = New System.Windows.Forms.Panel()
        Me.lblPnlInsurerName = New System.Windows.Forms.Label()
        Me.lblInsurerName = New System.Windows.Forms.Label()
        Me.fraPreviousBroker = New System.Windows.Forms.GroupBox()
        Me.cmdBrokerLookup = New System.Windows.Forms.Button()
        Me.txtBrokerRef = New System.Windows.Forms.TextBox()
        Me.pnlBrokerName = New System.Windows.Forms.Panel()
        Me.lblPnlBrokerName = New System.Windows.Forms.Label()
        Me.lblBrokerName = New System.Windows.Forms.Label()
        Me.cboPolicyType = New System.Windows.Forms.ComboBox()
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax()
        Me._cmdPrevious_6 = New System.Windows.Forms.Button()
        Me._cmdNext_6 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_7 = New System.Windows.Forms.Button()
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl()
        Me._cmdNext_7 = New System.Windows.Forms.Button()
        Me._cmdPrevious_8 = New System.Windows.Forms.Button()
        Me.pnlYearToDate = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraAreaCode.SuspendLayout()
        Me.fraConsultant.SuspendLayout()
        Me.pnlConsultantName.SuspendLayout()
        Me.fraBusinessDetails.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.fraAgent.SuspendLayout()
        Me.pnlAgentName.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        Me.fraBlackList.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraCorrespondence.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.frmContacts.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraRealArea.SuspendLayout()
        Me.fraTab5_1.SuspendLayout()
        Me.fraNumbers.SuspendLayout()
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
        Me.tabMainTab.Size = New System.Drawing.Size(713, 428)
        Me.tabMainTab.TabIndex = 154
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAssociates)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAreaCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraConsultant)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraBusinessDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraBlackList)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(705, 402)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Identity"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblAssociates
        '
        Me.lblAssociates.AutoEllipsis = True
        Me.lblAssociates.AutoSize = True
        Me.lblAssociates.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAssociates.ForeColor = System.Drawing.Color.Red
        Me.lblAssociates.Location = New System.Drawing.Point(8, 373)
        Me.lblAssociates.Name = "lblAssociates"
        Me.lblAssociates.Size = New System.Drawing.Size(392, 18)
        Me.lblAssociates.TabIndex = 155
        Me.lblAssociates.Text = "An Associated Client is attached to this record"
        Me.lblAssociates.Visible = False
        '
        'fraAreaCode
        '
        Me.fraAreaCode.Controls.Add(Me.cboArea)
        Me.fraAreaCode.Controls.Add(Me.lblArea)
        Me.fraAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAreaCode.Location = New System.Drawing.Point(328, 352)
        Me.fraAreaCode.Name = "fraAreaCode"
        Me.fraAreaCode.Size = New System.Drawing.Size(268, 38)
        Me.fraAreaCode.TabIndex = 124
        Me.fraAreaCode.TabStop = False
        '
        'cboArea
        '
        Me.cboArea.BackColor = System.Drawing.SystemColors.Window
        Me.cboArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboArea.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboArea.Location = New System.Drawing.Point(48, 11)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboArea.Size = New System.Drawing.Size(217, 21)
        Me.cboArea.TabIndex = 22
        '
        'lblArea
        '
        Me.lblArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArea.Location = New System.Drawing.Point(8, 16)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArea.Size = New System.Drawing.Size(89, 19)
        Me.lblArea.TabIndex = 125
        Me.lblArea.Text = "Area:"
        '
        'fraConsultant
        '
        Me.fraConsultant.Controls.Add(Me.txtConsultantRef)
        Me.fraConsultant.Controls.Add(Me.cmdConsultantLookup)
        Me.fraConsultant.Controls.Add(Me.pnlConsultantName)
        Me.fraConsultant.Controls.Add(Me.lblConsultantName)
        Me.fraConsultant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraConsultant.Location = New System.Drawing.Point(328, 284)
        Me.fraConsultant.Name = "fraConsultant"
        Me.fraConsultant.Size = New System.Drawing.Size(308, 64)
        Me.fraConsultant.TabIndex = 118
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
        Me.txtConsultantRef.Location = New System.Drawing.Point(128, 16)
        Me.txtConsultantRef.MaxLength = 0
        Me.txtConsultantRef.Name = "txtConsultantRef"
        Me.txtConsultantRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConsultantRef.Size = New System.Drawing.Size(169, 20)
        Me.txtConsultantRef.TabIndex = 21
        '
        'cmdConsultantLookup
        '
        Me.cmdConsultantLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConsultantLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConsultantLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConsultantLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConsultantLookup.Location = New System.Drawing.Point(8, 16)
        Me.cmdConsultantLookup.Name = "cmdConsultantLookup"
        Me.cmdConsultantLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConsultantLookup.Size = New System.Drawing.Size(60, 23)
        Me.cmdConsultantLookup.TabIndex = 20
        Me.cmdConsultantLookup.Text = "Code..."
        Me.cmdConsultantLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConsultantLookup.UseVisualStyleBackColor = False
        '
        'pnlConsultantName
        '
        Me.pnlConsultantName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConsultantName.Controls.Add(Me.lblPnlConsultantName)
        Me.pnlConsultantName.Font = New System.Drawing.Font("Verdana", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConsultantName.Location = New System.Drawing.Point(128, 40)
        Me.pnlConsultantName.Name = "pnlConsultantName"
        Me.pnlConsultantName.Size = New System.Drawing.Size(169, 21)
        Me.pnlConsultantName.TabIndex = 119
        '
        'lblPnlConsultantName
        '
        Me.lblPnlConsultantName.AutoSize = True
        Me.lblPnlConsultantName.Location = New System.Drawing.Point(4, 3)
        Me.lblPnlConsultantName.Name = "lblPnlConsultantName"
        Me.lblPnlConsultantName.Size = New System.Drawing.Size(0, 14)
        Me.lblPnlConsultantName.TabIndex = 0
        '
        'lblConsultantName
        '
        Me.lblConsultantName.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsultantName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsultantName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsultantName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsultantName.Location = New System.Drawing.Point(8, 40)
        Me.lblConsultantName.Name = "lblConsultantName"
        Me.lblConsultantName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsultantName.Size = New System.Drawing.Size(49, 19)
        Me.lblConsultantName.TabIndex = 120
        Me.lblConsultantName.Text = "Name:"
        '
        'fraBusinessDetails
        '
        Me.fraBusinessDetails.Controls.Add(Me.lblEmployees)
        Me.fraBusinessDetails.Controls.Add(Me.txtEmployees)
        Me.fraBusinessDetails.Controls.Add(Me.ddTrade)
        Me.fraBusinessDetails.Controls.Add(Me.cboTrade)
        Me.fraBusinessDetails.Controls.Add(Me.cboEmployees)
        Me.fraBusinessDetails.Controls.Add(Me.cboSICCode)
        Me.fraBusinessDetails.Controls.Add(Me.ddBusiness)
        Me.fraBusinessDetails.Controls.Add(Me.txtTradingSince)
        Me.fraBusinessDetails.Controls.Add(Me.txtOffices)
        Me.fraBusinessDetails.Controls.Add(Me.cmdMembershipGroups)
        Me.fraBusinessDetails.Controls.Add(Me.lblSICCode)
        Me.fraBusinessDetails.Controls.Add(Me.lblTradingSince)
        Me.fraBusinessDetails.Controls.Add(Me.lblTrade)
        Me.fraBusinessDetails.Controls.Add(Me.lblOffices)
        Me.fraBusinessDetails.Controls.Add(Me.lblBusiness)
        Me.fraBusinessDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBusinessDetails.Location = New System.Drawing.Point(328, 120)
        Me.fraBusinessDetails.Name = "fraBusinessDetails"
        Me.fraBusinessDetails.Size = New System.Drawing.Size(323, 163)
        Me.fraBusinessDetails.TabIndex = 97
        Me.fraBusinessDetails.TabStop = False
        Me.fraBusinessDetails.Text = "Business Details"
        '
        'lblEmployees
        '
        Me.lblEmployees.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmployees.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmployees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmployees.Location = New System.Drawing.Point(8, 136)
        Me.lblEmployees.Name = "lblEmployees"
        Me.lblEmployees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmployees.Size = New System.Drawing.Size(122, 14)
        Me.lblEmployees.TabIndex = 103
        Me.lblEmployees.Text = "No. of employees:"
        '
        'txtEmployees
        '
        Me.txtEmployees.AcceptsReturn = True
        Me.txtEmployees.BackColor = System.Drawing.SystemColors.Window
        Me.txtEmployees.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEmployees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmployees.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEmployees.Location = New System.Drawing.Point(136, 136)
        Me.txtEmployees.MaxLength = 0
        Me.txtEmployees.Multiline = True
        Me.txtEmployees.Name = "txtEmployees"
        Me.txtEmployees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEmployees.Size = New System.Drawing.Size(99, 21)
        Me.txtEmployees.TabIndex = 156
        Me.txtEmployees.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ddTrade
        '
        Me.ddTrade.AllowAbiCodeEntry = False
        Me.ddTrade.AutoCompleteText = False
        Me.ddTrade.DataModel = "GIIM"
        Me.ddTrade.ListIndex = -1
        Me.ddTrade.ListManager = Nothing
        Me.ddTrade.Location = New System.Drawing.Point(136, 40)
        Me.ddTrade.Login = False
        Me.ddTrade.LongList = True
        Me.ddTrade.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddTrade.Name = "ddTrade"
        Me.ddTrade.PropertyId = "2228228"
        Me.ddTrade.ReadOnly_Renamed = False
        Me.ddTrade.SelLength = 0
        Me.ddTrade.SelStart = 0
        Me.ddTrade.SelText = ""
        Me.ddTrade.Size = New System.Drawing.Size(163, 21)
        Me.ddTrade.TabIndex = 15
        Me.ddTrade.ToolTipText = ""
        Me.ddTrade.VehicleListId = ""
        Me.ddTrade.VehicleMake = ""
        '
        'cboTrade
        '
        Me.cboTrade.DefaultItemId = 0
        Me.cboTrade.FirstItem = ""
        Me.cboTrade.ItemId = 0
        Me.cboTrade.ListIndex = -1
        Me.cboTrade.Location = New System.Drawing.Point(288, 40)
        Me.cboTrade.Name = "cboTrade"
        Me.cboTrade.PMLookupProductFamily = 1
        Me.cboTrade.SingleItemId = 0
        Me.cboTrade.Size = New System.Drawing.Size(79, 21)
        Me.cboTrade.Sorted = True
        Me.cboTrade.TabIndex = 101
        Me.cboTrade.TableName = "Trade"
        Me.cboTrade.ToolTipText = ""
        Me.cboTrade.WhereClause = ""
        '
        'cboEmployees
        '
        Me.cboEmployees.DefaultItemId = 0
        Me.cboEmployees.FirstItem = ""
        Me.cboEmployees.ItemId = 0
        Me.cboEmployees.ListIndex = -1
        Me.cboEmployees.Location = New System.Drawing.Point(136, 136)
        Me.cboEmployees.Name = "cboEmployees"
        Me.cboEmployees.PMLookupProductFamily = 1
        Me.cboEmployees.SingleItemId = 0
        Me.cboEmployees.Size = New System.Drawing.Size(97, 21)
        Me.cboEmployees.Sorted = False
        Me.cboEmployees.TabIndex = 19
        Me.cboEmployees.TableName = "EmployeeBand"
        Me.cboEmployees.ToolTipText = ""
        Me.cboEmployees.WhereClause = ""
        Me.cboEmployees.SortColumnName = "EMPLOYEEBAND_ID"
        '
        'cboSICCode
        '
        Me.cboSICCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboSICCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSICCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSICCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSICCode.Location = New System.Drawing.Point(136, 64)
        Me.cboSICCode.Name = "cboSICCode"
        Me.cboSICCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSICCode.Size = New System.Drawing.Size(162, 21)
        Me.cboSICCode.TabIndex = 16
        '
        'ddBusiness
        '
        Me.ddBusiness.AllowAbiCodeEntry = False
        Me.ddBusiness.AutoCompleteText = False
        Me.ddBusiness.DataModel = "GIIM"
        Me.ddBusiness.ListIndex = -1
        Me.ddBusiness.ListManager = Nothing
        Me.ddBusiness.Location = New System.Drawing.Point(136, 16)
        Me.ddBusiness.Login = False
        Me.ddBusiness.LongList = True
        Me.ddBusiness.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddBusiness.Name = "ddBusiness"
        Me.ddBusiness.PropertyId = "2228228"
        Me.ddBusiness.ReadOnly_Renamed = False
        Me.ddBusiness.SelLength = 0
        Me.ddBusiness.SelStart = 0
        Me.ddBusiness.SelText = ""
        Me.ddBusiness.Size = New System.Drawing.Size(162, 21)
        Me.ddBusiness.TabIndex = 14
        Me.ddBusiness.ToolTipText = ""
        Me.ddBusiness.VehicleListId = ""
        Me.ddBusiness.VehicleMake = ""
        '
        'txtTradingSince
        '
        Me.txtTradingSince.AcceptsReturn = True
        Me.txtTradingSince.BackColor = System.Drawing.SystemColors.Window
        Me.txtTradingSince.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTradingSince.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTradingSince.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTradingSince.Location = New System.Drawing.Point(136, 88)
        Me.txtTradingSince.MaxLength = 0
        Me.txtTradingSince.Name = "txtTradingSince"
        Me.txtTradingSince.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTradingSince.Size = New System.Drawing.Size(161, 20)
        Me.txtTradingSince.TabIndex = 17
        '
        'txtOffices
        '
        Me.txtOffices.AcceptsReturn = True
        Me.txtOffices.BackColor = System.Drawing.SystemColors.Window
        Me.txtOffices.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOffices.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOffices.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOffices.Location = New System.Drawing.Point(136, 112)
        Me.txtOffices.MaxLength = 0
        Me.txtOffices.Multiline = True
        Me.txtOffices.Name = "txtOffices"
        Me.txtOffices.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOffices.Size = New System.Drawing.Size(99, 21)
        Me.txtOffices.TabIndex = 18
        '
        'cmdMembershipGroups
        '
        Me.cmdMembershipGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMembershipGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMembershipGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMembershipGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMembershipGroups.Location = New System.Drawing.Point(184, 68)
        Me.cmdMembershipGroups.Name = "cmdMembershipGroups"
        Me.cmdMembershipGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMembershipGroups.Size = New System.Drawing.Size(81, 39)
        Me.cmdMembershipGroups.TabIndex = 76
        Me.cmdMembershipGroups.Text = "Group Membership"
        Me.cmdMembershipGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMembershipGroups.UseVisualStyleBackColor = False
        Me.cmdMembershipGroups.Visible = False
        '
        'lblSICCode
        '
        Me.lblSICCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSICCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSICCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSICCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSICCode.Location = New System.Drawing.Point(8, 64)
        Me.lblSICCode.Name = "lblSICCode"
        Me.lblSICCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSICCode.Size = New System.Drawing.Size(73, 17)
        Me.lblSICCode.TabIndex = 102
        Me.lblSICCode.Text = "SIC code:"
        '
        'lblTradingSince
        '
        Me.lblTradingSince.BackColor = System.Drawing.SystemColors.Control
        Me.lblTradingSince.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTradingSince.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTradingSince.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTradingSince.Location = New System.Drawing.Point(8, 88)
        Me.lblTradingSince.Name = "lblTradingSince"
        Me.lblTradingSince.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTradingSince.Size = New System.Drawing.Size(89, 17)
        Me.lblTradingSince.TabIndex = 104
        Me.lblTradingSince.Text = "Trading since:"
        '
        'lblTrade
        '
        Me.lblTrade.BackColor = System.Drawing.SystemColors.Control
        Me.lblTrade.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTrade.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrade.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTrade.Location = New System.Drawing.Point(8, 40)
        Me.lblTrade.Name = "lblTrade"
        Me.lblTrade.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTrade.Size = New System.Drawing.Size(41, 17)
        Me.lblTrade.TabIndex = 100
        Me.lblTrade.Text = "Trade:"
        '
        'lblOffices
        '
        Me.lblOffices.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffices.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffices.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOffices.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOffices.Location = New System.Drawing.Point(8, 112)
        Me.lblOffices.Name = "lblOffices"
        Me.lblOffices.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOffices.Size = New System.Drawing.Size(81, 17)
        Me.lblOffices.TabIndex = 99
        Me.lblOffices.Text = "No. of offices:"
        '
        'lblBusiness
        '
        Me.lblBusiness.AutoSize = True
        Me.lblBusiness.BackColor = System.Drawing.SystemColors.Control
        Me.lblBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusiness.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBusiness.Location = New System.Drawing.Point(8, 19)
        Me.lblBusiness.Name = "lblBusiness"
        Me.lblBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBusiness.Size = New System.Drawing.Size(52, 13)
        Me.lblBusiness.TabIndex = 98
        Me.lblBusiness.Text = "Business:"
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
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_0.TabIndex = 23
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.txtTradingName)
        Me.fraClient.Controls.Add(Me.txtCompanyReg)
        Me.fraClient.Controls.Add(Me.txtMainContact)
        Me.fraClient.Controls.Add(Me.txtName)
        Me.fraClient.Controls.Add(Me.txtIDReference)
        Me.fraClient.Controls.Add(Me.lblCompanyReg)
        Me.fraClient.Controls.Add(Me.lblMainContact)
        Me.fraClient.Controls.Add(Me.lblName)
        Me.fraClient.Controls.Add(Me.lblIDReference)
        Me.fraClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(6, 8)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(307, 113)
        Me.fraClient.TabIndex = 80
        Me.fraClient.TabStop = False
        '
        'txtTradingName
        '
        Me.txtTradingName.AcceptsReturn = True
        Me.txtTradingName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTradingName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTradingName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTradingName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTradingName.Location = New System.Drawing.Point(120, 88)
        Me.txtTradingName.MaxLength = 255
        Me.txtTradingName.Name = "txtTradingName"
        Me.txtTradingName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTradingName.Size = New System.Drawing.Size(170, 20)
        Me.txtTradingName.TabIndex = 3
        Me.txtTradingName.Visible = False
        '
        'txtCompanyReg
        '
        Me.txtCompanyReg.AcceptsReturn = True
        Me.txtCompanyReg.BackColor = System.Drawing.SystemColors.Window
        Me.txtCompanyReg.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCompanyReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCompanyReg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCompanyReg.Location = New System.Drawing.Point(120, 88)
        Me.txtCompanyReg.MaxLength = 0
        Me.txtCompanyReg.Name = "txtCompanyReg"
        Me.txtCompanyReg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCompanyReg.Size = New System.Drawing.Size(170, 20)
        Me.txtCompanyReg.TabIndex = 139
        '
        'txtMainContact
        '
        Me.txtMainContact.AcceptsReturn = True
        Me.txtMainContact.BackColor = System.Drawing.SystemColors.Window
        Me.txtMainContact.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMainContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMainContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMainContact.Location = New System.Drawing.Point(120, 62)
        Me.txtMainContact.MaxLength = 0
        Me.txtMainContact.Name = "txtMainContact"
        Me.txtMainContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMainContact.Size = New System.Drawing.Size(170, 20)
        Me.txtMainContact.TabIndex = 2
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(120, 37)
        Me.txtName.MaxLength = 255
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(170, 20)
        Me.txtName.TabIndex = 1
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(120, 14)
        Me.txtIDReference.MaxLength = 20
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(170, 20)
        Me.txtIDReference.TabIndex = 0
        '
        'lblCompanyReg
        '
        Me.lblCompanyReg.AutoSize = True
        Me.lblCompanyReg.BackColor = System.Drawing.SystemColors.Control
        Me.lblCompanyReg.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompanyReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyReg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompanyReg.Location = New System.Drawing.Point(8, 88)
        Me.lblCompanyReg.Name = "lblCompanyReg"
        Me.lblCompanyReg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompanyReg.Size = New System.Drawing.Size(72, 13)
        Me.lblCompanyReg.TabIndex = 140
        Me.lblCompanyReg.Text = "Company reg:"
        '
        'lblMainContact
        '
        Me.lblMainContact.AutoSize = True
        Me.lblMainContact.BackColor = System.Drawing.SystemColors.Control
        Me.lblMainContact.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMainContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMainContact.Location = New System.Drawing.Point(8, 64)
        Me.lblMainContact.Name = "lblMainContact"
        Me.lblMainContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMainContact.Size = New System.Drawing.Size(72, 13)
        Me.lblMainContact.TabIndex = 83
        Me.lblMainContact.Text = "Main contact:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(8, 40)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(75, 13)
        Me.lblName.TabIndex = 82
        Me.lblName.Text = "Trading name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(8, 17)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(63, 13)
        Me.lblIDReference.TabIndex = 81
        Me.lblIDReference.Text = "Client code:"
        '
        'fraAgent
        '
        Me.fraAgent.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgent.Controls.Add(Me.txtAgentRef)
        Me.fraAgent.Controls.Add(Me.cmdAgentLookUp)
        Me.fraAgent.Controls.Add(Me.txtFileCode)
        Me.fraAgent.Controls.Add(Me.txtRecordStatus)
        Me.fraAgent.Controls.Add(Me.pnlAgentName)
        Me.fraAgent.Controls.Add(Me.lblAgentName)
        Me.fraAgent.Controls.Add(Me.lblRecordStatus)
        Me.fraAgent.Controls.Add(Me.lblFileCode)
        Me.fraAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgent.Location = New System.Drawing.Point(8, 236)
        Me.fraAgent.Name = "fraAgent"
        Me.fraAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgent.Size = New System.Drawing.Size(305, 113)
        Me.fraAgent.TabIndex = 113
        Me.fraAgent.TabStop = False
        Me.fraAgent.Text = "Lead Agent"
        '
        'txtAgentRef
        '
        Me.txtAgentRef.AcceptsReturn = True
        Me.txtAgentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentRef.Location = New System.Drawing.Point(120, 16)
        Me.txtAgentRef.MaxLength = 0
        Me.txtAgentRef.Name = "txtAgentRef"
        Me.txtAgentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentRef.Size = New System.Drawing.Size(169, 20)
        Me.txtAgentRef.TabIndex = 9
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
        Me.cmdAgentLookUp.Size = New System.Drawing.Size(60, 21)
        Me.cmdAgentLookUp.TabIndex = 8
        Me.cmdAgentLookUp.Text = "Code..."
        Me.cmdAgentLookUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentLookUp.UseVisualStyleBackColor = False
        '
        'txtFileCode
        '
        Me.txtFileCode.AcceptsReturn = True
        Me.txtFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileCode.Location = New System.Drawing.Point(120, 64)
        Me.txtFileCode.MaxLength = 0
        Me.txtFileCode.Name = "txtFileCode"
        Me.txtFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileCode.Size = New System.Drawing.Size(169, 20)
        Me.txtFileCode.TabIndex = 10
        '
        'txtRecordStatus
        '
        Me.txtRecordStatus.AcceptsReturn = True
        Me.txtRecordStatus.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecordStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRecordStatus.Enabled = False
        Me.txtRecordStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecordStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRecordStatus.Location = New System.Drawing.Point(120, 88)
        Me.txtRecordStatus.MaxLength = 0
        Me.txtRecordStatus.Name = "txtRecordStatus"
        Me.txtRecordStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRecordStatus.Size = New System.Drawing.Size(169, 20)
        Me.txtRecordStatus.TabIndex = 11
        Me.txtRecordStatus.Visible = False
        '
        'pnlAgentName
        '
        Me.pnlAgentName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAgentName.Controls.Add(Me.lblPnlAgentName)
        Me.pnlAgentName.Font = New System.Drawing.Font("Verdana", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAgentName.Location = New System.Drawing.Point(120, 40)
        Me.pnlAgentName.Name = "pnlAgentName"
        Me.pnlAgentName.Size = New System.Drawing.Size(169, 21)
        Me.pnlAgentName.TabIndex = 114
        '
        'lblPnlAgentName
        '
        Me.lblPnlAgentName.AutoSize = True
        Me.lblPnlAgentName.Location = New System.Drawing.Point(4, 1)
        Me.lblPnlAgentName.Name = "lblPnlAgentName"
        Me.lblPnlAgentName.Size = New System.Drawing.Size(0, 14)
        Me.lblPnlAgentName.TabIndex = 0
        '
        'lblAgentName
        '
        Me.lblAgentName.AutoSize = True
        Me.lblAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentName.Location = New System.Drawing.Point(8, 40)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentName.Size = New System.Drawing.Size(38, 13)
        Me.lblAgentName.TabIndex = 115
        Me.lblAgentName.Text = "Name:"
        '
        'lblRecordStatus
        '
        Me.lblRecordStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecordStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecordStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecordStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecordStatus.Location = New System.Drawing.Point(8, 88)
        Me.lblRecordStatus.Name = "lblRecordStatus"
        Me.lblRecordStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecordStatus.Size = New System.Drawing.Size(41, 19)
        Me.lblRecordStatus.TabIndex = 117
        Me.lblRecordStatus.Text = "Status:"
        Me.lblRecordStatus.Visible = False
        '
        'lblFileCode
        '
        Me.lblFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileCode.Location = New System.Drawing.Point(8, 64)
        Me.lblFileCode.Name = "lblFileCode"
        Me.lblFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileCode.Size = New System.Drawing.Size(137, 19)
        Me.lblFileCode.TabIndex = 116
        Me.lblFileCode.Text = "Filecode:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.pnlLastYear)
        Me.Frame1.Controls.Add(Me.lblYearToDate)
        Me.Frame1.Controls.Add(Me.pnlClient)
        Me.Frame1.Controls.Add(Me.imgIcon)
        Me.Frame1.Controls.Add(Me.chkFeeClient)
        Me.Frame1.Controls.Add(Me.chkAgent)
        Me.Frame1.Controls.Add(Me.chkProspect)
        Me.Frame1.Controls.Add(Me.pnlClientBalance)
        Me.Frame1.Controls.Add(Me.pnlYearToDateTurnover)
        Me.Frame1.Controls.Add(Me.pnlLastYearTurnover)
        Me.Frame1.Controls.Add(Me.lblLastYearTurnover)
        Me.Frame1.Controls.Add(Me.lblYearToDateTurnover)
        Me.Frame1.Controls.Add(Me.lblClientBalance)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(328, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(373, 113)
        Me.Frame1.TabIndex = 127
        Me.Frame1.TabStop = False
        '
        'pnlLastYear
        '
        Me.pnlLastYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLastYear.Location = New System.Drawing.Point(211, 64)
        Me.pnlLastYear.Name = "pnlLastYear"
        Me.pnlLastYear.Size = New System.Drawing.Size(153, 19)
        Me.pnlLastYear.TabIndex = 181
        Me.pnlLastYear.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblYearToDate
        '
        Me.lblYearToDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblYearToDate.Location = New System.Drawing.Point(211, 40)
        Me.lblYearToDate.Name = "lblYearToDate"
        Me.lblYearToDate.Size = New System.Drawing.Size(153, 19)
        Me.lblYearToDate.TabIndex = 180
        Me.lblYearToDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlClient
        '
        Me.pnlClient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClient.Location = New System.Drawing.Point(211, 16)
        Me.pnlClient.Name = "pnlClient"
        Me.pnlClient.Size = New System.Drawing.Size(153, 19)
        Me.pnlClient.TabIndex = 179
        Me.pnlClient.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(354, 98)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(10, 10)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'chkFeeClient
        '
        Me.chkFeeClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkFeeClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFeeClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFeeClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFeeClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFeeClient.Location = New System.Drawing.Point(208, 88)
        Me.chkFeeClient.Name = "chkFeeClient"
        Me.chkFeeClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFeeClient.Size = New System.Drawing.Size(89, 21)
        Me.chkFeeClient.TabIndex = 178
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
        Me.chkAgent.Location = New System.Drawing.Point(8, 88)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgent.Size = New System.Drawing.Size(95, 21)
        Me.chkAgent.TabIndex = 12
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
        Me.chkProspect.Location = New System.Drawing.Point(104, 88)
        Me.chkProspect.Name = "chkProspect"
        Me.chkProspect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProspect.Size = New System.Drawing.Size(97, 21)
        Me.chkProspect.TabIndex = 13
        Me.chkProspect.Text = "Is Prospect ?"
        Me.chkProspect.UseVisualStyleBackColor = False
        '
        'pnlClientBalance
        '
        Me.pnlClientBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlClientBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClientBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClientBalance.Location = New System.Drawing.Point(211, 16)
        Me.pnlClientBalance.Name = "pnlClientBalance"
        Me.pnlClientBalance.Size = New System.Drawing.Size(153, 19)
        Me.pnlClientBalance.TabIndex = 128
        '
        'pnlYearToDateTurnover
        '
        Me.pnlYearToDateTurnover.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlYearToDateTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlYearToDateTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlYearToDateTurnover.Location = New System.Drawing.Point(211, 40)
        Me.pnlYearToDateTurnover.Name = "pnlYearToDateTurnover"
        Me.pnlYearToDateTurnover.Size = New System.Drawing.Size(153, 19)
        Me.pnlYearToDateTurnover.TabIndex = 130
        '
        'pnlLastYearTurnover
        '
        Me.pnlLastYearTurnover.BackColor = System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.pnlLastYearTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLastYearTurnover.Font = New System.Drawing.Font("Verdana", 8.26!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLastYearTurnover.Location = New System.Drawing.Point(211, 64)
        Me.pnlLastYearTurnover.Name = "pnlLastYearTurnover"
        Me.pnlLastYearTurnover.Size = New System.Drawing.Size(153, 21)
        Me.pnlLastYearTurnover.TabIndex = 132
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
        Me.lblLastYearTurnover.Size = New System.Drawing.Size(193, 17)
        Me.lblLastYearTurnover.TabIndex = 133
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
        Me.lblYearToDateTurnover.Size = New System.Drawing.Size(193, 17)
        Me.lblYearToDateTurnover.TabIndex = 131
        Me.lblYearToDateTurnover.Text = "Year to Date Turnover:"
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
        Me.lblClientBalance.Size = New System.Drawing.Size(193, 17)
        Me.lblClientBalance.TabIndex = 129
        Me.lblClientBalance.Text = "Account Balance:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cboBranch)
        Me.Frame2.Controls.Add(Me.cboSubBranch)
        Me.Frame2.Controls.Add(Me.cboServiceLevel)
        Me.Frame2.Controls.Add(Me.txtAlternativeIdentifier)
        Me.Frame2.Controls.Add(Me.lblBranch)
        Me.Frame2.Controls.Add(Me.lblServicelevel)
        Me.Frame2.Controls.Add(Me.lblSubBranch)
        Me.Frame2.Controls.Add(Me.lblAlternativeIdentifier)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 120)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(305, 113)
        Me.Frame2.TabIndex = 134
        Me.Frame2.TabStop = False
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(120, 64)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(170, 21)
        Me.cboBranch.TabIndex = 6
        Me.cboBranch.Text = " "
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(120, 88)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(170, 21)
        Me.cboSubBranch.TabIndex = 7
        Me.cboSubBranch.Text = " "
        '
        'cboServiceLevel
        '
        Me.cboServiceLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboServiceLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboServiceLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboServiceLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboServiceLevel.Location = New System.Drawing.Point(120, 40)
        Me.cboServiceLevel.Name = "cboServiceLevel"
        Me.cboServiceLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboServiceLevel.Size = New System.Drawing.Size(170, 21)
        Me.cboServiceLevel.TabIndex = 5
        '
        'txtAlternativeIdentifier
        '
        Me.txtAlternativeIdentifier.AcceptsReturn = True
        Me.txtAlternativeIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlternativeIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAlternativeIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlternativeIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAlternativeIdentifier.Location = New System.Drawing.Point(120, 16)
        Me.txtAlternativeIdentifier.MaxLength = 0
        Me.txtAlternativeIdentifier.Name = "txtAlternativeIdentifier"
        Me.txtAlternativeIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAlternativeIdentifier.Size = New System.Drawing.Size(170, 20)
        Me.txtAlternativeIdentifier.TabIndex = 4
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 68)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 138
        Me.lblBranch.Text = "Branch:"
        '
        'lblServicelevel
        '
        Me.lblServicelevel.AutoSize = True
        Me.lblServicelevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblServicelevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblServicelevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServicelevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblServicelevel.Location = New System.Drawing.Point(8, 44)
        Me.lblServicelevel.Name = "lblServicelevel"
        Me.lblServicelevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblServicelevel.Size = New System.Drawing.Size(71, 13)
        Me.lblServicelevel.TabIndex = 137
        Me.lblServicelevel.Text = "Service level:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(8, 92)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(65, 13)
        Me.lblSubBranch.TabIndex = 136
        Me.lblSubBranch.Text = "Sub-branch:"
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
        Me.lblAlternativeIdentifier.TabIndex = 135
        Me.lblAlternativeIdentifier.Text = "Alternative Identifier:"
        '
        'fraBlackList
        '
        Me.fraBlackList.BackColor = System.Drawing.SystemColors.Control
        Me.fraBlackList.Controls.Add(Me.cboBlackListReason)
        Me.fraBlackList.Controls.Add(Me.lblBlacklistReason)
        Me.fraBlackList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBlackList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBlackList.Location = New System.Drawing.Point(8, 348)
        Me.fraBlackList.Name = "fraBlackList"
        Me.fraBlackList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBlackList.Size = New System.Drawing.Size(305, 49)
        Me.fraBlackList.TabIndex = 175
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
        Me.cboBlackListReason.Location = New System.Drawing.Point(120, 20)
        Me.cboBlackListReason.Name = "cboBlackListReason"
        Me.cboBlackListReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBlackListReason.Size = New System.Drawing.Size(169, 21)
        Me.cboBlackListReason.TabIndex = 176
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
        Me.lblBlacklistReason.TabIndex = 177
        Me.lblBlacklistReason.Text = "Reason:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_1.TabIndex = 29
        Me._cmdPrevious_1.TabStop = False
        Me._cmdPrevious_1.Text = "<&<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.txtSalutation)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Controls.Add(Me.lblSalutation)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(6, 9)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(588, 261)
        Me.fraAddress.TabIndex = 78
        Me.fraAddress.TabStop = False
        '
        'txtSalutation
        '
        Me.txtSalutation.AcceptsReturn = True
        Me.txtSalutation.BackColor = System.Drawing.SystemColors.Window
        Me.txtSalutation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSalutation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalutation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSalutation.Location = New System.Drawing.Point(80, 229)
        Me.txtSalutation.MaxLength = 0
        Me.txtSalutation.Name = "txtSalutation"
        Me.txtSalutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSalutation.Size = New System.Drawing.Size(503, 20)
        Me.txtSalutation.TabIndex = 28
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 192)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditAd.TabIndex = 27
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
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 192)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteAd.TabIndex = 26
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
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 192)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddAd.TabIndex = 25
        Me.cmdAddAd.Text = "Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(571, 169)
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.lvwAddresses.TabIndex = 24
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Tag = ""
        Me._lvwAddresses_ColumnHeader_1.Text = ""
        Me._lvwAddresses_ColumnHeader_1.Width = 97
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Tag = ""
        Me._lvwAddresses_ColumnHeader_2.Text = ""
        Me._lvwAddresses_ColumnHeader_2.Width = 97
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Tag = ""
        Me._lvwAddresses_ColumnHeader_3.Text = ""
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Tag = ""
        Me._lvwAddresses_ColumnHeader_4.Text = ""
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Tag = ""
        Me._lvwAddresses_ColumnHeader_5.Text = ""
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Tag = ""
        Me._lvwAddresses_ColumnHeader_6.Text = ""
        Me._lvwAddresses_ColumnHeader_6.Width = 67
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "AddressImage")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "PolicyImage")
        Me.ImageList2.Images.SetKeyName(5, "")
        Me.ImageList2.Images.SetKeyName(6, "ContactImage")
        Me.ImageList2.Images.SetKeyName(7, "ConvictionImage")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "CampaignImage")
        Me.ImageList2.Images.SetKeyName(10, "")
        '
        'lblSalutation
        '
        Me.lblSalutation.AutoSize = True
        Me.lblSalutation.BackColor = System.Drawing.SystemColors.Control
        Me.lblSalutation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSalutation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalutation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSalutation.Location = New System.Drawing.Point(8, 232)
        Me.lblSalutation.Name = "lblSalutation"
        Me.lblSalutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSalutation.Size = New System.Drawing.Size(57, 13)
        Me.lblSalutation.TabIndex = 79
        Me.lblSalutation.Text = "Salutation:"
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
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_1.TabIndex = 30
        Me._cmdNext_1.TabStop = False
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraCorrespondence)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Contact"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraCorrespondence
        '
        Me.fraCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.fraCorrespondence.Controls.Add(Me.cboCorrespondenceType)
        Me.fraCorrespondence.Controls.Add(Me.chkMailshot)
        Me.fraCorrespondence.Controls.Add(Me.chkTPS)
        Me.fraCorrespondence.Controls.Add(Me.chkeMPS)
        Me.fraCorrespondence.Controls.Add(Me.lblPreferredCorrespondence)
        Me.fraCorrespondence.Controls.Add(Me.lblTPS)
        Me.fraCorrespondence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCorrespondence.Location = New System.Drawing.Point(8, 242)
        Me.fraCorrespondence.Name = "fraCorrespondence"
        Me.fraCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCorrespondence.Size = New System.Drawing.Size(587, 77)
        Me.fraCorrespondence.TabIndex = 109
        Me.fraCorrespondence.TabStop = False
        Me.fraCorrespondence.Text = "Correspondence"
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
        Me.cboCorrespondenceType.TabIndex = 35
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
        Me.chkMailshot.TabIndex = 37
        Me.chkMailshot.Text = "MPS:"
        Me.chkMailshot.UseVisualStyleBackColor = False
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
        Me.chkTPS.TabIndex = 36
        Me.chkTPS.Text = "Mailshot:"
        Me.chkTPS.UseVisualStyleBackColor = False
        '
        'chkeMPS
        '
        Me.chkeMPS.BackColor = System.Drawing.SystemColors.Control
        Me.chkeMPS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkeMPS.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkeMPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkeMPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkeMPS.Location = New System.Drawing.Point(216, 48)
        Me.chkeMPS.Name = "chkeMPS"
        Me.chkeMPS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkeMPS.Size = New System.Drawing.Size(81, 17)
        Me.chkeMPS.TabIndex = 38
        Me.chkeMPS.Text = "eMPS:"
        Me.chkeMPS.UseVisualStyleBackColor = False
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
        Me.lblPreferredCorrespondence.TabIndex = 110
        Me.lblPreferredCorrespondence.Text = "Preferred Correspondence:"
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
        Me.lblTPS.TabIndex = 111
        Me.lblTPS.Text = "TPS:"
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
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_2.TabIndex = 40
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
        Me.fraContact.Location = New System.Drawing.Point(6, 9)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(588, 226)
        Me.fraContact.TabIndex = 77
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
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditCon.TabIndex = 34
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
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteCon.TabIndex = 33
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
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddCon.TabIndex = 32
        Me.cmdAddCon.Text = "Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.LargeImageList = Me.ImageList2
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(571, 169)
        Me.lvwContacts.SmallImageList = Me.ImageList2
        Me.lvwContacts.TabIndex = 31
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
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_2.TabIndex = 39
        Me._cmdPrevious_2.TabStop = False
        Me._cmdPrevious_2.Text = "<&<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtHiddenDate)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtHiddenCurrency)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtCCJ)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmContacts)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblCCJ)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Convictions"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'txtHiddenDate
        '
        Me.txtHiddenDate.AcceptsReturn = True
        Me.txtHiddenDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtHiddenDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHiddenDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHiddenDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHiddenDate.Location = New System.Drawing.Point(400, 268)
        Me.txtHiddenDate.MaxLength = 0
        Me.txtHiddenDate.Name = "txtHiddenDate"
        Me.txtHiddenDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHiddenDate.Size = New System.Drawing.Size(73, 20)
        Me.txtHiddenDate.TabIndex = 46
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
        Me.txtHiddenCurrency.Location = New System.Drawing.Point(480, 268)
        Me.txtHiddenCurrency.MaxLength = 0
        Me.txtHiddenCurrency.Name = "txtHiddenCurrency"
        Me.txtHiddenCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHiddenCurrency.Size = New System.Drawing.Size(97, 20)
        Me.txtHiddenCurrency.TabIndex = 47
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
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_3.TabIndex = 49
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
        Me.txtCCJ.Location = New System.Drawing.Point(177, 247)
        Me.txtCCJ.MaxLength = 0
        Me.txtCCJ.Name = "txtCCJ"
        Me.txtCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCCJ.Size = New System.Drawing.Size(41, 20)
        Me.txtCCJ.TabIndex = 45
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_3.TabIndex = 48
        Me._cmdPrevious_3.TabStop = False
        Me._cmdPrevious_3.Text = "<&<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'frmContacts
        '
        Me.frmContacts.Controls.Add(Me.cmdAddConv)
        Me.frmContacts.Controls.Add(Me.cmdDeleteConv)
        Me.frmContacts.Controls.Add(Me.cmdEditConv)
        Me.frmContacts.Controls.Add(Me.lvwConvictions)
        Me.frmContacts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmContacts.Location = New System.Drawing.Point(6, 10)
        Me.frmContacts.Name = "frmContacts"
        Me.frmContacts.Size = New System.Drawing.Size(588, 225)
        Me.frmContacts.TabIndex = 85
        Me.frmContacts.TabStop = False
        '
        'cmdAddConv
        '
        Me.cmdAddConv.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddConv.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddConv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddConv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddConv.Location = New System.Drawing.Point(8, 192)
        Me.cmdAddConv.Name = "cmdAddConv"
        Me.cmdAddConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddConv.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddConv.TabIndex = 42
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
        Me.cmdDeleteConv.Location = New System.Drawing.Point(88, 192)
        Me.cmdDeleteConv.Name = "cmdDeleteConv"
        Me.cmdDeleteConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteConv.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteConv.TabIndex = 43
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
        Me.cmdEditConv.Location = New System.Drawing.Point(168, 192)
        Me.cmdEditConv.Name = "cmdEditConv"
        Me.cmdEditConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditConv.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditConv.TabIndex = 44
        Me.cmdEditConv.Text = "Edit"
        Me.cmdEditConv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditConv.UseVisualStyleBackColor = False
        '
        'lvwConvictions
        '
        Me.lvwConvictions.BackColor = System.Drawing.SystemColors.Window
        Me.lvwConvictions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwConvictions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwConvictions_ColumnHeader_1, Me._lvwConvictions_ColumnHeader_2, Me._lvwConvictions_ColumnHeader_3, Me._lvwConvictions_ColumnHeader_4, Me._lvwConvictions_ColumnHeader_5, Me._lvwConvictions_ColumnHeader_6})
        Me.lvwConvictions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwConvictions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwConvictions.LargeImageList = Me.ImageList2
        Me.lvwConvictions.Location = New System.Drawing.Point(8, 15)
        Me.lvwConvictions.Name = "lvwConvictions"
        Me.lvwConvictions.Size = New System.Drawing.Size(571, 169)
        Me.lvwConvictions.SmallImageList = Me.ImageList2
        Me.lvwConvictions.TabIndex = 41
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
        Me._lvwConvictions_ColumnHeader_6.Tag = ""
        Me._lvwConvictions_ColumnHeader_6.Text = "Penalty Points"
        Me._lvwConvictions_ColumnHeader_6.Width = 97
        '
        'lblCCJ
        '
        Me.lblCCJ.BackColor = System.Drawing.SystemColors.Control
        Me.lblCCJ.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCCJ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCCJ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCCJ.Location = New System.Drawing.Point(8, 249)
        Me.lblCCJ.Name = "lblCCJ"
        Me.lblCCJ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCCJ.Size = New System.Drawing.Size(161, 19)
        Me.lblCCJ.TabIndex = 112
        Me.lblCCJ.Text = "County court judgements:"
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraRealArea)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraTab5_1)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraNumbers)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me.cmdAssociates)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Additions"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'fraRealArea
        '
        Me.fraRealArea.BackColor = System.Drawing.SystemColors.Control
        Me.fraRealArea.Controls.Add(Me.txtTobLetter)
        Me.fraRealArea.Controls.Add(Me.txtRealFileCode)
        Me.fraRealArea.Controls.Add(Me.cboRealArea)
        Me.fraRealArea.Controls.Add(Me.lblTobLetter)
        Me.fraRealArea.Controls.Add(Me.lblRealFileCode)
        Me.fraRealArea.Controls.Add(Me.lblRealArea)
        Me.fraRealArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRealArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRealArea.Location = New System.Drawing.Point(6, 268)
        Me.fraRealArea.Name = "fraRealArea"
        Me.fraRealArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRealArea.Size = New System.Drawing.Size(588, 73)
        Me.fraRealArea.TabIndex = 121
        Me.fraRealArea.TabStop = False
        '
        'txtTobLetter
        '
        Me.txtTobLetter.AcceptsReturn = True
        Me.txtTobLetter.BackColor = System.Drawing.SystemColors.Window
        Me.txtTobLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTobLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTobLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTobLetter.Location = New System.Drawing.Point(408, 48)
        Me.txtTobLetter.MaxLength = 0
        Me.txtTobLetter.Name = "txtTobLetter"
        Me.txtTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTobLetter.Size = New System.Drawing.Size(169, 20)
        Me.txtTobLetter.TabIndex = 66
        '
        'txtRealFileCode
        '
        Me.txtRealFileCode.AcceptsReturn = True
        Me.txtRealFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtRealFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRealFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRealFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRealFileCode.Location = New System.Drawing.Point(409, 17)
        Me.txtRealFileCode.MaxLength = 0
        Me.txtRealFileCode.Name = "txtRealFileCode"
        Me.txtRealFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRealFileCode.Size = New System.Drawing.Size(170, 20)
        Me.txtRealFileCode.TabIndex = 65
        '
        'cboRealArea
        '
        Me.cboRealArea.BackColor = System.Drawing.SystemColors.Window
        Me.cboRealArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRealArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRealArea.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRealArea.Location = New System.Drawing.Point(112, 17)
        Me.cboRealArea.Name = "cboRealArea"
        Me.cboRealArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRealArea.Size = New System.Drawing.Size(170, 21)
        Me.cboRealArea.TabIndex = 64
        '
        'lblTobLetter
        '
        Me.lblTobLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblTobLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTobLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTobLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTobLetter.Location = New System.Drawing.Point(288, 40)
        Me.lblTobLetter.Name = "lblTobLetter"
        Me.lblTobLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTobLetter.Size = New System.Drawing.Size(113, 25)
        Me.lblTobLetter.TabIndex = 126
        Me.lblTobLetter.Text = "Terms Of Business Letter Sent:"
        '
        'lblRealFileCode
        '
        Me.lblRealFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblRealFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRealFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRealFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRealFileCode.Location = New System.Drawing.Point(293, 19)
        Me.lblRealFileCode.Name = "lblRealFileCode"
        Me.lblRealFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRealFileCode.Size = New System.Drawing.Size(73, 19)
        Me.lblRealFileCode.TabIndex = 123
        Me.lblRealFileCode.Text = "File code:"
        '
        'lblRealArea
        '
        Me.lblRealArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblRealArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRealArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRealArea.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRealArea.Location = New System.Drawing.Point(8, 19)
        Me.lblRealArea.Name = "lblRealArea"
        Me.lblRealArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRealArea.Size = New System.Drawing.Size(49, 19)
        Me.lblRealArea.TabIndex = 122
        Me.lblRealArea.Text = "Area:"
        '
        'fraTab5_1
        '
        Me.fraTab5_1.Controls.Add(Me.cboTermsOfPayment)
        Me.fraTab5_1.Controls.Add(Me.cboCurrency)
        Me.fraTab5_1.Controls.Add(Me.txtSource)
        Me.fraTab5_1.Controls.Add(Me.txtTPPassword)
        Me.fraTab5_1.Controls.Add(Me.txtLoyaltyNumberPrefix)
        Me.fraTab5_1.Controls.Add(Me.txtLoyaltyNumber)
        Me.fraTab5_1.Controls.Add(Me.cboRenewalStopCode)
        Me.fraTab5_1.Controls.Add(Me.cboSeasonalGift)
        Me.fraTab5_1.Controls.Add(Me.ddPaymentMethod)
        Me.fraTab5_1.Controls.Add(Me.cboCreditCard)
        Me.fraTab5_1.Controls.Add(Me.cboReminderType)
        Me.fraTab5_1.Controls.Add(Me.lblSource)
        Me.fraTab5_1.Controls.Add(Me.lblTPPassword)
        Me.fraTab5_1.Controls.Add(Me.lblLoyaltyNumber)
        Me.fraTab5_1.Controls.Add(Me.lblRenewalStopCode)
        Me.fraTab5_1.Controls.Add(Me.lblSeasonalGift)
        Me.fraTab5_1.Controls.Add(Me.lblCurrency)
        Me.fraTab5_1.Controls.Add(Me.lblTermsOfPayment)
        Me.fraTab5_1.Controls.Add(Me.lblPaymentMethod)
        Me.fraTab5_1.Controls.Add(Me.lblCreditCard)
        Me.fraTab5_1.Controls.Add(Me.lblReminderType)
        Me.fraTab5_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTab5_1.Location = New System.Drawing.Point(6, 10)
        Me.fraTab5_1.Name = "fraTab5_1"
        Me.fraTab5_1.Size = New System.Drawing.Size(588, 179)
        Me.fraTab5_1.TabIndex = 86
        Me.fraTab5_1.TabStop = False
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.FormattingEnabled = True
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(408, 17)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(171, 21)
        Me.cboTermsOfPayment.TabIndex = 97
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
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(169, 21)
        Me.cboCurrency.TabIndex = 50
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'txtSource
        '
        Me.txtSource.AcceptsReturn = True
        Me.txtSource.BackColor = System.Drawing.SystemColors.Window
        Me.txtSource.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSource.Location = New System.Drawing.Point(409, 70)
        Me.txtSource.MaxLength = 0
        Me.txtSource.Name = "txtSource"
        Me.txtSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSource.Size = New System.Drawing.Size(170, 20)
        Me.txtSource.TabIndex = 58
        '
        'txtTPPassword
        '
        Me.txtTPPassword.AcceptsReturn = True
        Me.txtTPPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtTPPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTPPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTPPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTPPassword.Location = New System.Drawing.Point(112, 152)
        Me.txtTPPassword.MaxLength = 255
        Me.txtTPPassword.Name = "txtTPPassword"
        Me.txtTPPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTPPassword.Size = New System.Drawing.Size(169, 20)
        Me.txtTPPassword.TabIndex = 55
        '
        'txtLoyaltyNumberPrefix
        '
        Me.txtLoyaltyNumberPrefix.AcceptsReturn = True
        Me.txtLoyaltyNumberPrefix.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumberPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumberPrefix.Enabled = False
        Me.txtLoyaltyNumberPrefix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumberPrefix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumberPrefix.Location = New System.Drawing.Point(112, 124)
        Me.txtLoyaltyNumberPrefix.MaxLength = 10
        Me.txtLoyaltyNumberPrefix.Name = "txtLoyaltyNumberPrefix"
        Me.txtLoyaltyNumberPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumberPrefix.Size = New System.Drawing.Size(65, 20)
        Me.txtLoyaltyNumberPrefix.TabIndex = 53
        Me.txtLoyaltyNumberPrefix.Text = "6014 35"
        '
        'txtLoyaltyNumber
        '
        Me.txtLoyaltyNumber.AcceptsReturn = True
        Me.txtLoyaltyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLoyaltyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoyaltyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLoyaltyNumber.Location = New System.Drawing.Point(181, 124)
        Me.txtLoyaltyNumber.MaxLength = 10
        Me.txtLoyaltyNumber.Name = "txtLoyaltyNumber"
        Me.txtLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLoyaltyNumber.Size = New System.Drawing.Size(101, 20)
        Me.txtLoyaltyNumber.TabIndex = 54
        '
        'cboRenewalStopCode
        '
        Me.cboRenewalStopCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalStopCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalStopCode.Location = New System.Drawing.Point(409, 43)
        Me.cboRenewalStopCode.Name = "cboRenewalStopCode"
        Me.cboRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalStopCode.Size = New System.Drawing.Size(170, 21)
        Me.cboRenewalStopCode.TabIndex = 57
        '
        'cboSeasonalGift
        '
        Me.cboSeasonalGift.BackColor = System.Drawing.SystemColors.Window
        Me.cboSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSeasonalGift.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSeasonalGift.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSeasonalGift.Location = New System.Drawing.Point(409, 98)
        Me.cboSeasonalGift.Name = "cboSeasonalGift"
        Me.cboSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSeasonalGift.Size = New System.Drawing.Size(170, 21)
        Me.cboSeasonalGift.TabIndex = 60
        '
        'ddPaymentMethod
        '
        Me.ddPaymentMethod.AllowAbiCodeEntry = False
        Me.ddPaymentMethod.AutoCompleteText = False
        Me.ddPaymentMethod.DataModel = "GIIM"
        Me.ddPaymentMethod.ListIndex = -1
        Me.ddPaymentMethod.ListManager = Nothing
        Me.ddPaymentMethod.Location = New System.Drawing.Point(112, 43)
        Me.ddPaymentMethod.Login = False
        Me.ddPaymentMethod.LongList = False
        Me.ddPaymentMethod.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddPaymentMethod.Name = "ddPaymentMethod"
        Me.ddPaymentMethod.PropertyId = "6946819"
        Me.ddPaymentMethod.ReadOnly_Renamed = False
        Me.ddPaymentMethod.SelLength = 0
        Me.ddPaymentMethod.SelStart = 0
        Me.ddPaymentMethod.SelText = ""
        Me.ddPaymentMethod.Size = New System.Drawing.Size(170, 21)
        Me.ddPaymentMethod.TabIndex = 51
        Me.ddPaymentMethod.ToolTipText = ""
        Me.ddPaymentMethod.VehicleListId = ""
        Me.ddPaymentMethod.VehicleMake = ""
        '
        'cboCreditCard
        '
        Me.cboCreditCard.BackColor = System.Drawing.SystemColors.Window
        Me.cboCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCreditCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCreditCard.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCreditCard.Location = New System.Drawing.Point(409, 124)
        Me.cboCreditCard.Name = "cboCreditCard"
        Me.cboCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCreditCard.Size = New System.Drawing.Size(170, 21)
        Me.cboCreditCard.TabIndex = 59
        Me.cboCreditCard.Tag = "31245190 Financial_Detail,Payment_Card_Code"
        '
        'cboReminderType
        '
        Me.cboReminderType.BackColor = System.Drawing.SystemColors.Window
        Me.cboReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReminderType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReminderType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReminderType.Location = New System.Drawing.Point(112, 70)
        Me.cboReminderType.Name = "cboReminderType"
        Me.cboReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReminderType.Size = New System.Drawing.Size(170, 21)
        Me.cboReminderType.TabIndex = 52
        '
        'lblSource
        '
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(288, 72)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(129, 17)
        Me.lblSource.TabIndex = 96
        Me.lblSource.Text = "Source:"
        '
        'lblTPPassword
        '
        Me.lblTPPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblTPPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTPPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTPPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTPPassword.Location = New System.Drawing.Point(8, 152)
        Me.lblTPPassword.Name = "lblTPPassword"
        Me.lblTPPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTPPassword.Size = New System.Drawing.Size(129, 19)
        Me.lblTPPassword.TabIndex = 95
        Me.lblTPPassword.Text = "Password:"
        '
        'lblLoyaltyNumber
        '
        Me.lblLoyaltyNumber.AutoSize = True
        Me.lblLoyaltyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoyaltyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoyaltyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoyaltyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoyaltyNumber.Location = New System.Drawing.Point(8, 126)
        Me.lblLoyaltyNumber.Name = "lblLoyaltyNumber"
        Me.lblLoyaltyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoyaltyNumber.Size = New System.Drawing.Size(81, 13)
        Me.lblLoyaltyNumber.TabIndex = 93
        Me.lblLoyaltyNumber.Text = "Loyalty number:"
        '
        'lblRenewalStopCode
        '
        Me.lblRenewalStopCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStopCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStopCode.Location = New System.Drawing.Point(288, 45)
        Me.lblRenewalStopCode.Name = "lblRenewalStopCode"
        Me.lblRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStopCode.Size = New System.Drawing.Size(129, 19)
        Me.lblRenewalStopCode.TabIndex = 94
        Me.lblRenewalStopCode.Text = "Renewal stop code:"
        '
        'lblSeasonalGift
        '
        Me.lblSeasonalGift.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeasonalGift.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeasonalGift.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeasonalGift.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeasonalGift.Location = New System.Drawing.Point(288, 101)
        Me.lblSeasonalGift.Name = "lblSeasonalGift"
        Me.lblSeasonalGift.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeasonalGift.Size = New System.Drawing.Size(140, 19)
        Me.lblSeasonalGift.TabIndex = 92
        Me.lblSeasonalGift.Text = "Seasonal gift:"
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
        Me.lblCurrency.Size = New System.Drawing.Size(97, 19)
        Me.lblCurrency.TabIndex = 87
        Me.lblCurrency.Text = "Currency:"
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermsOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(288, 17)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(121, 19)
        Me.lblTermsOfPayment.TabIndex = 91
        Me.lblTermsOfPayment.Text = "Terms of payment:"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(8, 45)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(105, 19)
        Me.lblPaymentMethod.TabIndex = 88
        Me.lblPaymentMethod.Text = "Payment method:"
        '
        'lblCreditCard
        '
        Me.lblCreditCard.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreditCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreditCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreditCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreditCard.Location = New System.Drawing.Point(288, 130)
        Me.lblCreditCard.Name = "lblCreditCard"
        Me.lblCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreditCard.Size = New System.Drawing.Size(113, 19)
        Me.lblCreditCard.TabIndex = 89
        Me.lblCreditCard.Text = "Credit card type:"
        '
        'lblReminderType
        '
        Me.lblReminderType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReminderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReminderType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReminderType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReminderType.Location = New System.Drawing.Point(8, 72)
        Me.lblReminderType.Name = "lblReminderType"
        Me.lblReminderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReminderType.Size = New System.Drawing.Size(97, 19)
        Me.lblReminderType.TabIndex = 90
        Me.lblReminderType.Text = "Reminder type:"
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_4.TabIndex = 68
        Me._cmdPrevious_4.TabStop = False
        Me._cmdPrevious_4.Text = "<&<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        'fraNumbers
        '
        Me.fraNumbers.BackColor = System.Drawing.SystemColors.Control
        Me.fraNumbers.Controls.Add(Me.lblTurnover)
        Me.fraNumbers.Controls.Add(Me.txtTurnover)
        Me.fraNumbers.Controls.Add(Me.cboTurnover)
        Me.fraNumbers.Controls.Add(Me.txtFinancialYear)
        Me.fraNumbers.Controls.Add(Me.txtWageRoll)
        Me.fraNumbers.Controls.Add(Me.lblFinancialYear)
        Me.fraNumbers.Controls.Add(Me.lblWageRoll)
        Me.fraNumbers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNumbers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNumbers.Location = New System.Drawing.Point(6, 192)
        Me.fraNumbers.Name = "fraNumbers"
        Me.fraNumbers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNumbers.Size = New System.Drawing.Size(588, 77)
        Me.fraNumbers.TabIndex = 105
        Me.fraNumbers.TabStop = False
        '
        'lblTurnover
        '
        Me.lblTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTurnover.Location = New System.Drawing.Point(293, 18)
        Me.lblTurnover.Name = "lblTurnover"
        Me.lblTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTurnover.Size = New System.Drawing.Size(73, 19)
        Me.lblTurnover.TabIndex = 107
        Me.lblTurnover.Text = "Turnover:"
        '
        'txtTurnover
        '
        Me.txtTurnover.AcceptsReturn = True
        Me.txtTurnover.BackColor = System.Drawing.SystemColors.Window
        Me.txtTurnover.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTurnover.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTurnover.Location = New System.Drawing.Point(409, 19)
        Me.txtTurnover.MaxLength = 0
        Me.txtTurnover.Name = "txtTurnover"
        Me.txtTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTurnover.Size = New System.Drawing.Size(146, 20)
        Me.txtTurnover.TabIndex = 158
        '
        'cboTurnover
        '
        Me.cboTurnover.DefaultItemId = 0
        Me.cboTurnover.FirstItem = ""
        Me.cboTurnover.ItemId = 0
        Me.cboTurnover.ListIndex = -1
        Me.cboTurnover.Location = New System.Drawing.Point(409, 16)
        Me.cboTurnover.Name = "cboTurnover"
        Me.cboTurnover.PMLookupProductFamily = 1
        Me.cboTurnover.SingleItemId = 0
        Me.cboTurnover.Size = New System.Drawing.Size(162, 21)
        Me.cboTurnover.Sorted = True
        Me.cboTurnover.TabIndex = 62
        Me.cboTurnover.TableName = "TurnoverBand"
        Me.cboTurnover.ToolTipText = ""
        Me.cboTurnover.WhereClause = ""
        '
        'txtFinancialYear
        '
        Me.txtFinancialYear.AcceptsReturn = True
        Me.txtFinancialYear.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinancialYear.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinancialYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinancialYear.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinancialYear.Location = New System.Drawing.Point(409, 42)
        Me.txtFinancialYear.MaxLength = 0
        Me.txtFinancialYear.Name = "txtFinancialYear"
        Me.txtFinancialYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinancialYear.Size = New System.Drawing.Size(170, 20)
        Me.txtFinancialYear.TabIndex = 63
        '
        'txtWageRoll
        '
        Me.txtWageRoll.AcceptsReturn = True
        Me.txtWageRoll.BackColor = System.Drawing.SystemColors.Window
        Me.txtWageRoll.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWageRoll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWageRoll.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWageRoll.Location = New System.Drawing.Point(112, 16)
        Me.txtWageRoll.MaxLength = 0
        Me.txtWageRoll.Name = "txtWageRoll"
        Me.txtWageRoll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWageRoll.Size = New System.Drawing.Size(170, 20)
        Me.txtWageRoll.TabIndex = 61
        '
        'lblFinancialYear
        '
        Me.lblFinancialYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinancialYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinancialYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinancialYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinancialYear.Location = New System.Drawing.Point(293, 44)
        Me.lblFinancialYear.Name = "lblFinancialYear"
        Me.lblFinancialYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinancialYear.Size = New System.Drawing.Size(89, 19)
        Me.lblFinancialYear.TabIndex = 108
        Me.lblFinancialYear.Text = "Financial year:"
        '
        'lblWageRoll
        '
        Me.lblWageRoll.BackColor = System.Drawing.SystemColors.Control
        Me.lblWageRoll.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWageRoll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWageRoll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWageRoll.Location = New System.Drawing.Point(8, 18)
        Me.lblWageRoll.Name = "lblWageRoll"
        Me.lblWageRoll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWageRoll.Size = New System.Drawing.Size(97, 19)
        Me.lblWageRoll.TabIndex = 106
        Me.lblWageRoll.Text = "Wage roll:"
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
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_4.TabIndex = 69
        Me._cmdNext_4.TabStop = False
        Me._cmdNext_4.Text = "&>>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        'cmdAssociates
        '
        Me.cmdAssociates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAssociates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAssociates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAssociates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAssociates.Location = New System.Drawing.Point(8, 347)
        Me.cmdAssociates.Name = "cmdAssociates"
        Me.cmdAssociates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAssociates.Size = New System.Drawing.Size(73, 23)
        Me.cmdAssociates.TabIndex = 67
        Me.cmdAssociates.Text = "Associates"
        Me.cmdAssociates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAssociates.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraLoyaltySchemes)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Misc"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraLoyaltySchemes
        '
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdAddLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdDeleteLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.cmdEditLoyaltyScheme)
        Me.fraLoyaltySchemes.Controls.Add(Me.lvwLoyaltySchemes)
        Me.fraLoyaltySchemes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLoyaltySchemes.Location = New System.Drawing.Point(6, 10)
        Me.fraLoyaltySchemes.Name = "fraLoyaltySchemes"
        Me.fraLoyaltySchemes.Size = New System.Drawing.Size(588, 225)
        Me.fraLoyaltySchemes.TabIndex = 84
        Me.fraLoyaltySchemes.TabStop = False
        Me.fraLoyaltySchemes.Text = "{Loyalty Schemes}"
        '
        'cmdAddLoyaltyScheme
        '
        Me.cmdAddLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddLoyaltyScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddLoyaltyScheme.Location = New System.Drawing.Point(8, 192)
        Me.cmdAddLoyaltyScheme.Name = "cmdAddLoyaltyScheme"
        Me.cmdAddLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddLoyaltyScheme.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddLoyaltyScheme.TabIndex = 71
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
        Me.cmdDeleteLoyaltyScheme.Location = New System.Drawing.Point(88, 192)
        Me.cmdDeleteLoyaltyScheme.Name = "cmdDeleteLoyaltyScheme"
        Me.cmdDeleteLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteLoyaltyScheme.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeleteLoyaltyScheme.TabIndex = 72
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
        Me.cmdEditLoyaltyScheme.Location = New System.Drawing.Point(168, 192)
        Me.cmdEditLoyaltyScheme.Name = "cmdEditLoyaltyScheme"
        Me.cmdEditLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditLoyaltyScheme.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditLoyaltyScheme.TabIndex = 73
        Me.cmdEditLoyaltyScheme.Text = "{Edit}"
        Me.cmdEditLoyaltyScheme.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditLoyaltyScheme.UseVisualStyleBackColor = False
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
        Me.lvwLoyaltySchemes.Location = New System.Drawing.Point(8, 15)
        Me.lvwLoyaltySchemes.Name = "lvwLoyaltySchemes"
        Me.lvwLoyaltySchemes.Size = New System.Drawing.Size(571, 169)
        Me.lvwLoyaltySchemes.SmallImageList = Me.ImageList2
        Me.lvwLoyaltySchemes.TabIndex = 70
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
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_5.TabIndex = 74
        Me._cmdPrevious_5.TabStop = False
        Me._cmdPrevious_5.Text = "<&<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
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
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_5.TabIndex = 75
        Me._cmdNext_5.TabStop = False
        Me._cmdNext_5.Text = "&>>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
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
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "7 - Tax"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'fraPolicies
        '
        Me.fraPolicies.Controls.Add(Me.cmdAddPol)
        Me.fraPolicies.Controls.Add(Me.cmdDeletePol)
        Me.fraPolicies.Controls.Add(Me.cmdEditPol)
        Me.fraPolicies.Controls.Add(Me.lvwPolicies)
        Me.fraPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicies.Location = New System.Drawing.Point(312, 192)
        Me.fraPolicies.Name = "fraPolicies"
        Me.fraPolicies.Size = New System.Drawing.Size(289, 121)
        Me.fraPolicies.TabIndex = 165
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
        Me.cmdAddPol.Size = New System.Drawing.Size(73, 23)
        Me.cmdAddPol.TabIndex = 167
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
        Me.cmdDeletePol.Size = New System.Drawing.Size(73, 23)
        Me.cmdDeletePol.TabIndex = 168
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
        Me.cmdEditPol.Size = New System.Drawing.Size(73, 23)
        Me.cmdEditPol.TabIndex = 169
        Me.cmdEditPol.Text = "Edit"
        Me.cmdEditPol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditPol.UseVisualStyleBackColor = False
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
        Me.lvwPolicies.TabIndex = 166
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
        Me.fraCampaign.Location = New System.Drawing.Point(8, 192)
        Me.fraCampaign.Name = "fraCampaign"
        Me.fraCampaign.Size = New System.Drawing.Size(289, 121)
        Me.fraCampaign.TabIndex = 163
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
        Me.lvwCampaigns.Size = New System.Drawing.Size(271, 81)
        Me.lvwCampaigns.SmallImageList = Me.ImageList2
        Me.lvwCampaigns.TabIndex = 164
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
        Me.fraProspect.Controls.Add(Me.cboProspectingStatus)
        Me.fraProspect.Controls.Add(Me.txtAgentReference)
        Me.fraProspect.Controls.Add(Me.cmdCurrentAgent)
        Me.fraProspect.Controls.Add(Me.cboStrengthCode)
        Me.fraProspect.Controls.Add(Me.pnlCurrentAgent)
        Me.fraProspect.Controls.Add(Me.lblProspectStatus)
        Me.fraProspect.Controls.Add(Me.lblAgentReference)
        Me.fraProspect.Controls.Add(Me.lblStrengthCode)
        Me.fraProspect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProspect.Location = New System.Drawing.Point(8, 8)
        Me.fraProspect.Name = "fraProspect"
        Me.fraProspect.Size = New System.Drawing.Size(585, 73)
        Me.fraProspect.TabIndex = 141
        Me.fraProspect.TabStop = False
        '
        'cboProspectingStatus
        '
        Me.cboProspectingStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboProspectingStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProspectingStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProspectingStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProspectingStatus.Location = New System.Drawing.Point(408, 40)
        Me.cboProspectingStatus.Name = "cboProspectingStatus"
        Me.cboProspectingStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProspectingStatus.Size = New System.Drawing.Size(169, 21)
        Me.cboProspectingStatus.TabIndex = 149
        '
        'txtAgentReference
        '
        Me.txtAgentReference.AcceptsReturn = True
        Me.txtAgentReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentReference.Location = New System.Drawing.Point(117, 16)
        Me.txtAgentReference.MaxLength = 0
        Me.txtAgentReference.Name = "txtAgentReference"
        Me.txtAgentReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentReference.Size = New System.Drawing.Size(169, 20)
        Me.txtAgentReference.TabIndex = 143
        '
        'cmdCurrentAgent
        '
        Me.cmdCurrentAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCurrentAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCurrentAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCurrentAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCurrentAgent.Location = New System.Drawing.Point(6, 40)
        Me.cmdCurrentAgent.Name = "cmdCurrentAgent"
        Me.cmdCurrentAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCurrentAgent.Size = New System.Drawing.Size(104, 23)
        Me.cmdCurrentAgent.TabIndex = 146
        Me.cmdCurrentAgent.Text = "Current Agent..."
        Me.cmdCurrentAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCurrentAgent.UseVisualStyleBackColor = False
        '
        'cboStrengthCode
        '
        Me.cboStrengthCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboStrengthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStrengthCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStrengthCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStrengthCode.Location = New System.Drawing.Point(408, 16)
        Me.cboStrengthCode.Name = "cboStrengthCode"
        Me.cboStrengthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStrengthCode.Size = New System.Drawing.Size(169, 21)
        Me.cboStrengthCode.TabIndex = 145
        '
        'pnlCurrentAgent
        '
        Me.pnlCurrentAgent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCurrentAgent.Controls.Add(Me.lblPnlCurrentAgent)
        Me.pnlCurrentAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCurrentAgent.Location = New System.Drawing.Point(117, 40)
        Me.pnlCurrentAgent.Name = "pnlCurrentAgent"
        Me.pnlCurrentAgent.Size = New System.Drawing.Size(169, 21)
        Me.pnlCurrentAgent.TabIndex = 147
        '
        'lblPnlCurrentAgent
        '
        Me.lblPnlCurrentAgent.AutoSize = True
        Me.lblPnlCurrentAgent.Location = New System.Drawing.Point(4, 1)
        Me.lblPnlCurrentAgent.Name = "lblPnlCurrentAgent"
        Me.lblPnlCurrentAgent.Size = New System.Drawing.Size(0, 13)
        Me.lblPnlCurrentAgent.TabIndex = 0
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
        Me.lblProspectStatus.Size = New System.Drawing.Size(105, 17)
        Me.lblProspectStatus.TabIndex = 148
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
        Me.lblAgentReference.TabIndex = 142
        Me.lblAgentReference.Text = "Agent Reference:"
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
        Me.lblStrengthCode.Size = New System.Drawing.Size(105, 17)
        Me.lblStrengthCode.TabIndex = 144
        Me.lblStrengthCode.Text = "Strength Code:"
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
        Me.fraPreviousInsurer.Location = New System.Drawing.Point(8, 104)
        Me.fraPreviousInsurer.Name = "fraPreviousInsurer"
        Me.fraPreviousInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousInsurer.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousInsurer.TabIndex = 150
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
        Me.txtInsurerRef.TabIndex = 152
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
        Me.cmdInsurerLookup.Size = New System.Drawing.Size(60, 23)
        Me.cmdInsurerLookup.TabIndex = 151
        Me.cmdInsurerLookup.Text = "Code..."
        Me.cmdInsurerLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsurerLookup.UseVisualStyleBackColor = False
        '
        'pnlInsurerName
        '
        Me.pnlInsurerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlInsurerName.Controls.Add(Me.lblPnlInsurerName)
        Me.pnlInsurerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlInsurerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlInsurerName.Name = "pnlInsurerName"
        Me.pnlInsurerName.Size = New System.Drawing.Size(201, 21)
        Me.pnlInsurerName.TabIndex = 155
        '
        'lblPnlInsurerName
        '
        Me.lblPnlInsurerName.AutoSize = True
        Me.lblPnlInsurerName.Location = New System.Drawing.Point(4, 1)
        Me.lblPnlInsurerName.Name = "lblPnlInsurerName"
        Me.lblPnlInsurerName.Size = New System.Drawing.Size(0, 13)
        Me.lblPnlInsurerName.TabIndex = 0
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
        Me.lblInsurerName.TabIndex = 153
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
        Me.fraPreviousBroker.Location = New System.Drawing.Point(320, 104)
        Me.fraPreviousBroker.Name = "fraPreviousBroker"
        Me.fraPreviousBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreviousBroker.Size = New System.Drawing.Size(289, 70)
        Me.fraPreviousBroker.TabIndex = 157
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
        Me.cmdBrokerLookup.Size = New System.Drawing.Size(60, 23)
        Me.cmdBrokerLookup.TabIndex = 159
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
        Me.txtBrokerRef.TabIndex = 160
        '
        'pnlBrokerName
        '
        Me.pnlBrokerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBrokerName.Controls.Add(Me.lblPnlBrokerName)
        Me.pnlBrokerName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlBrokerName.Location = New System.Drawing.Point(80, 40)
        Me.pnlBrokerName.Name = "pnlBrokerName"
        Me.pnlBrokerName.Size = New System.Drawing.Size(201, 21)
        Me.pnlBrokerName.TabIndex = 162
        '
        'lblPnlBrokerName
        '
        Me.lblPnlBrokerName.AutoSize = True
        Me.lblPnlBrokerName.Location = New System.Drawing.Point(3, 1)
        Me.lblPnlBrokerName.Name = "lblPnlBrokerName"
        Me.lblPnlBrokerName.Size = New System.Drawing.Size(0, 13)
        Me.lblPnlBrokerName.TabIndex = 0
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
        Me.lblBrokerName.TabIndex = 161
        Me.lblBrokerName.Text = "Name:"
        '
        'cboPolicyType
        '
        Me.cboPolicyType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyType.Location = New System.Drawing.Point(416, 328)
        Me.cboPolicyType.Name = "cboPolicyType"
        Me.cboPolicyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyType.Size = New System.Drawing.Size(169, 21)
        Me.cboPolicyType.TabIndex = 170
        Me.cboPolicyType.TabStop = False
        Me.cboPolicyType.Visible = False
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage7.Controls.Add(Me.uctPartyTax1)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdPrevious_6)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdNext_6)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "8 - Bank"
        Me._tabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(38, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 181
        Me.Label1.Text = "Party Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(8, 12)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(585, 249)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 172
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0.0R
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_6.TabIndex = 171
        Me._cmdPrevious_6.TabStop = False
        Me._cmdPrevious_6.Text = "<&<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
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
        Me._cmdNext_6.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_6.TabIndex = 173
        Me._cmdNext_6.TabStop = False
        Me._cmdNext_6.Text = "&>>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me._cmdPrevious_7)
        Me._tabMainTab_TabPage8.Controls.Add(Me.uctPartyBankControl1)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(747, 459)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "8 - Bank"
        Me._tabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_7
        '
        Me._cmdPrevious_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_7.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_7.Name = "_cmdPrevious_7"
        Me._cmdPrevious_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_7.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_7.TabIndex = 174
        Me._cmdPrevious_7.TabStop = False
        Me._cmdPrevious_7.Text = "<&<"
        Me._cmdPrevious_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_7.UseVisualStyleBackColor = False
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = CType(0, Byte)
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(9, 14)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = CType(0, Byte)
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(693, 339)
        Me.uctPartyBankControl1.TabIndex = 181
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
        Me._cmdNext_7.Size = New System.Drawing.Size(38, 23)
        Me._cmdNext_7.TabIndex = 180
        Me._cmdNext_7.TabStop = False
        Me._cmdNext_7.Text = "&>>"
        Me._cmdNext_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_7.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_8
        '
        Me._cmdPrevious_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_8.Location = New System.Drawing.Point(8, 372)
        Me._cmdPrevious_8.Name = "_cmdPrevious_8"
        Me._cmdPrevious_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_8.Size = New System.Drawing.Size(38, 23)
        Me._cmdPrevious_8.TabIndex = 179
        Me._cmdPrevious_8.TabStop = False
        Me._cmdPrevious_8.Text = "<&<"
        Me._cmdPrevious_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_8.UseVisualStyleBackColor = False
        '
        'pnlYearToDate
        '
        Me.pnlYearToDate.AutoSize = True
        Me.pnlYearToDate.Location = New System.Drawing.Point(211, 40)
        Me.pnlYearToDate.Name = "pnlYearToDate"
        Me.pnlYearToDate.Size = New System.Drawing.Size(39, 13)
        Me.pnlYearToDate.TabIndex = 180
        Me.pnlYearToDate.Text = "Label3"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(211, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 181
        '
        'uctPartyCCControl
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPartyCCControl"
        Me.Size = New System.Drawing.Size(771, 840)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.fraAreaCode.ResumeLayout(False)
        Me.fraConsultant.ResumeLayout(False)
        Me.fraConsultant.PerformLayout()
        Me.pnlConsultantName.ResumeLayout(False)
        Me.pnlConsultantName.PerformLayout()
        Me.fraBusinessDetails.ResumeLayout(False)
        Me.fraBusinessDetails.PerformLayout()
        Me.fraClient.ResumeLayout(False)
        Me.fraClient.PerformLayout()
        Me.fraAgent.ResumeLayout(False)
        Me.fraAgent.PerformLayout()
        Me.pnlAgentName.ResumeLayout(False)
        Me.pnlAgentName.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.fraBlackList.ResumeLayout(False)
        Me.fraBlackList.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me.fraAddress.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraCorrespondence.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage3.PerformLayout()
        Me.frmContacts.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraRealArea.ResumeLayout(False)
        Me.fraRealArea.PerformLayout()
        Me.fraTab5_1.ResumeLayout(False)
        Me.fraTab5_1.PerformLayout()
        Me.fraNumbers.ResumeLayout(False)
        Me.fraNumbers.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraLoyaltySchemes.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraPolicies.ResumeLayout(False)
        Me.fraCampaign.ResumeLayout(False)
        Me.fraProspect.ResumeLayout(False)
        Me.fraProspect.PerformLayout()
        Me.pnlCurrentAgent.ResumeLayout(False)
        Me.pnlCurrentAgent.PerformLayout()
        Me.fraPreviousInsurer.ResumeLayout(False)
        Me.fraPreviousInsurer.PerformLayout()
        Me.pnlInsurerName.ResumeLayout(False)
        Me.pnlInsurerName.PerformLayout()
        Me.fraPreviousBroker.ResumeLayout(False)
        Me.fraPreviousBroker.PerformLayout()
        Me.pnlBrokerName.ResumeLayout(False)
        Me.pnlBrokerName.PerformLayout()
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._tabMainTab_TabPage7.PerformLayout()
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(8) = _cmdPrevious_8
        Me.cmdPrevious(7) = _cmdPrevious_7
        Me.cmdPrevious(6) = _cmdPrevious_6
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
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
    'developer guide no. 26(Latest Guide)
    Friend WithEvents lblPnlAgentName As System.Windows.Forms.Label
    Friend WithEvents lblPnlConsultantName As System.Windows.Forms.Label
    Friend WithEvents lblPnlCurrentAgent As System.Windows.Forms.Label
    Friend WithEvents lblPnlInsurerName As System.Windows.Forms.Label
    Friend WithEvents lblPnlBrokerName As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
    Friend WithEvents pnlLastYear As System.Windows.Forms.Label
    Friend WithEvents lblYearToDate As System.Windows.Forms.Label
    Friend WithEvents pnlClient As System.Windows.Forms.Label
    Friend WithEvents pnlYearToDate As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
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
