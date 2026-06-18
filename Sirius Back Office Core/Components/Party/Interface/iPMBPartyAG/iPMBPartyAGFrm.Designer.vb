<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializecmdPrevious()
        InitializecmdNext()
        lvwCompetent_InitializeColumnKeys()
        lvwAgencyUsers_InitializeColumnKeys()
        lvwUnderTraining_InitializeColumnKeys()
        lvwAssociates_InitializeColumnKeys()
        lvwContact_InitializeColumnKeys()
        lvwAddress_InitializeColumnKeys()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
    Public WithEvents mnuFinancial As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuCommission As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuLetter As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRelatedDocuments As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
    'Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents txtCommonRenewalDate As System.Windows.Forms.TextBox
    Public WithEvents chkOverrideCommission As System.Windows.Forms.CheckBox
    Public WithEvents uctBranch As System.Windows.Forms.ComboBox
    Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
    Public WithEvents cmdRates As System.Windows.Forms.Button
    Public WithEvents uctCategory As PMLookupControl.cboPMLookup
    Public WithEvents txtAgencyReviewDate As System.Windows.Forms.TextBox
    Public WithEvents chkStatement As System.Windows.Forms.CheckBox
    Public WithEvents cboReportIndicator As System.Windows.Forms.ComboBox
    Public WithEvents cboBinderIndicator As System.Windows.Forms.ComboBox
    Public WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
    Public WithEvents cboSource As System.Windows.Forms.ComboBox
    Public WithEvents uctCurrency As UserControls.CurrencyLookup
    Public WithEvents lblCommonRenewalDate As System.Windows.Forms.Label
    Public WithEvents lblOverrideCommission As System.Windows.Forms.Label
    Public WithEvents lblSubBranch As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblStatement As System.Windows.Forms.Label
    Public WithEvents lblReportInd As System.Windows.Forms.Label
    Public WithEvents lblBinderInd As System.Windows.Forms.Label
    Public WithEvents lblTermsOfPayment As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblAgencyNextReview As System.Windows.Forms.Label
    Public WithEvents lblSource As System.Windows.Forms.Label
    Public WithEvents lblCategory As System.Windows.Forms.Label
    Public WithEvents fraAppointment As System.Windows.Forms.GroupBox
    Public WithEvents chkSingleInstalmentPlanOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmbAgentType As System.Windows.Forms.ComboBox
    Public WithEvents uctAgentType As PMLookupControl.cboPMLookup
    Public WithEvents txtAccountNumber As System.Windows.Forms.TextBox
    Public WithEvents txtFileCode As System.Windows.Forms.TextBox
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents txtIDReference As System.Windows.Forms.TextBox
    Public WithEvents actExpenseAcc As UserControls.AccountLookup
    Public WithEvents txtBrokerAbiId As System.Windows.Forms.TextBox
    Public WithEvents lblExpenseAcc As System.Windows.Forms.Label
    Public WithEvents lblBrokerAbiId As System.Windows.Forms.Label
    Public WithEvents lblACcountNumber As System.Windows.Forms.Label
    Public WithEvents lblFileCode As System.Windows.Forms.Label
    Public WithEvents lblAgentType As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblIDReference As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cmdAddAd As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
    Public WithEvents cmdEditAd As System.Windows.Forms.Button
    Private WithEvents _lvwAddress_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddress As System.Windows.Forms.ListView
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents cmdAddCon As System.Windows.Forms.Button
    Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
    Public WithEvents cmdEditCon As System.Windows.Forms.Button
    Private WithEvents _lvwContact_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwContact As System.Windows.Forms.ListView
    Public WithEvents fraContact As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Public WithEvents cboCommissionPostingType As PMLookupControl.cboPMLookup
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents fraCommissionRelease As System.Windows.Forms.GroupBox
    Public WithEvents chkStandardAccount As System.Windows.Forms.CheckBox
    Public WithEvents txtDaysAllowed As System.Windows.Forms.TextBox
    Public WithEvents txtOverdraftExpiry As System.Windows.Forms.TextBox
    Public WithEvents txtOverdraftLimit As System.Windows.Forms.TextBox
    Public WithEvents txtExpectedDailyPremium As System.Windows.Forms.TextBox
    Public WithEvents chkOverdraftAccount As System.Windows.Forms.CheckBox
    Public WithEvents chkPrepaymentAccount As System.Windows.Forms.CheckBox
    Public WithEvents chkFloatBalanceAccount As System.Windows.Forms.CheckBox
    Public WithEvents txtFloatBalanceLimit As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents fraAccounts As System.Windows.Forms.Panel
    Public WithEvents fraAccountLimits As System.Windows.Forms.GroupBox
    Public WithEvents chkConsolidatedCommission As System.Windows.Forms.CheckBox
    Public WithEvents Frame4 As System.Windows.Forms.GroupBox
    Public WithEvents chkMakeLiveCashDeposit As System.Windows.Forms.CheckBox
    Public WithEvents chkMakeLiveBankGuarantee As System.Windows.Forms.CheckBox
    Public WithEvents chkMakeLivePayNow As System.Windows.Forms.CheckBox
    Public WithEvents chkMakeLiveInstallments As System.Windows.Forms.CheckBox
    Public WithEvents chkMakeLiveInvoice As System.Windows.Forms.CheckBox
    Public WithEvents fraMakeLive As System.Windows.Forms.GroupBox
    Public WithEvents cboPaymentFrequency As System.Windows.Forms.ComboBox
    Public WithEvents cboPaymentMethod As PMLookupControl.cboPMLookup
    Public WithEvents lblPaymentMethod As System.Windows.Forms.Label
    Public WithEvents lblPaymentFrequency As System.Windows.Forms.Label
    Public WithEvents lblBankAccount As System.Windows.Forms.Label
    Public WithEvents fraPaymentTaxation As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Public WithEvents chkProductAgentRenewalList As System.Windows.Forms.CheckBox
    Public WithEvents lstDocsChosen As System.Windows.Forms.ListBox
    Public WithEvents cboAddressOnNotice As System.Windows.Forms.ComboBox
    Public WithEvents cmdMaintain As System.Windows.Forms.Button
    Public WithEvents lblAvailableDocs As System.Windows.Forms.Label
    Public WithEvents lblAddressOnNotice As System.Windows.Forms.Label
    Public WithEvents fraDocuments As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents cmdConsultantLookup As System.Windows.Forms.Button
    Public WithEvents txtConsultantRef As System.Windows.Forms.TextBox
    Public WithEvents pnlConsultantName As System.Windows.Forms.Panel
    Public WithEvents lblConsultantName As System.Windows.Forms.Label
    Public WithEvents fraConsultant As System.Windows.Forms.GroupBox
    Public WithEvents txtAgentGroupRef As System.Windows.Forms.TextBox
    Public WithEvents cmdAgentGroupLookup As System.Windows.Forms.Button
    Public WithEvents pnlAgentGroupName As System.Windows.Forms.Panel
    Public WithEvents lblAgentGroupName As System.Windows.Forms.Label
    Public WithEvents fraAgentGroup As System.Windows.Forms.GroupBox
    Public WithEvents cmdMaintainAssociates As System.Windows.Forms.Button
    Private WithEvents _lvwAssociates_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAssociates_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAssociates_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAssociates As System.Windows.Forms.ListView
    Public WithEvents fraAssociate As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
    Public WithEvents chkAltRefMandatory As System.Windows.Forms.CheckBox
    Public WithEvents chkAltRefRequiredForEachTrans As System.Windows.Forms.CheckBox
    Public WithEvents fraAltRef As System.Windows.Forms.GroupBox
    Public WithEvents cboBrokerTransferBusinessType As System.Windows.Forms.ComboBox
    Public WithEvents txtBrokerTransferToCode As System.Windows.Forms.TextBox
    Public WithEvents cmdBrokerTransferTo As System.Windows.Forms.Button
    Public WithEvents chkBrokerInTransferMode As System.Windows.Forms.CheckBox
    Public WithEvents lblBrokerTransferBusinessType As System.Windows.Forms.Label
    Public WithEvents fraBrokerTransfer As System.Windows.Forms.GroupBox
    Public WithEvents txtFirstName As System.Windows.Forms.TextBox
    Public WithEvents txtContactPerson As System.Windows.Forms.TextBox
    Public WithEvents ddTitle As PMListMgrDropdown.uctDropdown
    Public WithEvents lblFirstName As System.Windows.Forms.Label
    Public WithEvents lblContactPerson As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cboRenewalStopCode As System.Windows.Forms.ComboBox
    Public WithEvents txtDateCancelled As System.Windows.Forms.TextBox
    Public WithEvents cboMultipac As System.Windows.Forms.ComboBox
    Public WithEvents lblMultipac As System.Windows.Forms.Label
    Public WithEvents lblRenewalStopCode As System.Windows.Forms.Label
    Public WithEvents lblDateCancelled As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents lblAgentStatus As System.Windows.Forms.Label
    Public WithEvents lblRegistrationNumber As System.Windows.Forms.Label
    Public WithEvents cmdAddAll As System.Windows.Forms.Button
    Public WithEvents cmdRemove As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdRemoveAll As System.Windows.Forms.Button
    Private WithEvents _lvwUnderTraining_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwUnderTraining As System.Windows.Forms.ListView
    Private WithEvents _lvwCompetent_ColumnHeader_1 As System.Windows.Forms.ColumnHeader

    Public WithEvents lvwCompetent As System.Windows.Forms.ListView
    Public WithEvents lblCompetant As System.Windows.Forms.Label
    Public WithEvents lvwAgencyUsers As System.Windows.Forms.ListView
    Public WithEvents lblAgencyUsers As System.Windows.Forms.Label
    Public WithEvents lblUnderTraining As System.Windows.Forms.Label
    Public WithEvents fraRiskGroups As System.Windows.Forms.GroupBox
    Public WithEvents fraUsers As System.Windows.Forms.GroupBox
    Public WithEvents cboAgentStatus As System.Windows.Forms.ComboBox
    Public WithEvents txtRegistrationNumber As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_6 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
    Private WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_7 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents uctPickListBranches As uctPickList.PickList
    Public WithEvents uctPickListProducts As uctPickList.PickList 'AxuctPickList.AxPickList
    Private WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_8 As System.Windows.Forms.Button
    Public WithEvents uctPartyBankControl1 As uctPartyBank.uctPartyBankControl
    Private WithEvents _tabMainTab_TabPage9 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList2 As System.Windows.Forms.ImageList
    'Public WithEvents lvwAgencyUsers As System.Windows.Forms.ListView
    Public cmdNext(8) As System.Windows.Forms.Button
    Public cmdPrevious(8) As System.Windows.Forms.Button
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim _lvwAgencyUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFinancial = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCommission = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRelatedDocuments = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNotes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLetter = New System.Windows.Forms.ToolStripMenuItem()
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip()
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton()
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripButton()
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraAppointment = New System.Windows.Forms.GroupBox()
        Me.txtCommonRenewalDate = New System.Windows.Forms.TextBox()
        Me.chkOverrideCommission = New System.Windows.Forms.CheckBox()
        Me.uctBranch = New System.Windows.Forms.ComboBox()
        Me.cboSubBranch = New System.Windows.Forms.ComboBox()
        Me.cmdRates = New System.Windows.Forms.Button()
        Me.uctCategory = New PMLookupControl.cboPMLookup()
        Me.txtAgencyReviewDate = New System.Windows.Forms.TextBox()
        Me.chkStatement = New System.Windows.Forms.CheckBox()
        Me.cboReportIndicator = New System.Windows.Forms.ComboBox()
        Me.cboBinderIndicator = New System.Windows.Forms.ComboBox()
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox()
        Me.cboSource = New System.Windows.Forms.ComboBox()
        Me.uctCurrency = New UserControls.CurrencyLookup()
        Me.lblCommonRenewalDate = New System.Windows.Forms.Label()
        Me.lblOverrideCommission = New System.Windows.Forms.Label()
        Me.lblSubBranch = New System.Windows.Forms.Label()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.lblStatement = New System.Windows.Forms.Label()
        Me.lblReportInd = New System.Windows.Forms.Label()
        Me.lblBinderInd = New System.Windows.Forms.Label()
        Me.lblTermsOfPayment = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblAgencyNextReview = New System.Windows.Forms.Label()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me.fraPremiumSettlement = New System.Windows.Forms.GroupBox()
        Me.OptNetOfcommission = New System.Windows.Forms.RadioButton()
        Me.OptGrossOfCommission = New System.Windows.Forms.RadioButton()
        Me.chkSingleInstalmentPlanOnly = New System.Windows.Forms.CheckBox()
        Me.cmbAgentType = New System.Windows.Forms.ComboBox()
        Me.uctAgentType = New PMLookupControl.cboPMLookup()
        Me.txtAccountNumber = New System.Windows.Forms.TextBox()
        Me.txtFileCode = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtIDReference = New System.Windows.Forms.TextBox()
        Me.actExpenseAcc = New UserControls.AccountLookup()
        Me.txtBrokerAbiId = New System.Windows.Forms.TextBox()
        Me.lblExpenseAcc = New System.Windows.Forms.Label()
        Me.lblBrokerAbiId = New System.Windows.Forms.Label()
        Me.lblACcountNumber = New System.Windows.Forms.Label()
        Me.lblFileCode = New System.Windows.Forms.Label()
        Me.lblAgentType = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblIDReference = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me.imgIcon = New System.Windows.Forms.PictureBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraAddress = New System.Windows.Forms.GroupBox()
        Me.cmdAddAd = New System.Windows.Forms.Button()
        Me.cmdDeleteAd = New System.Windows.Forms.Button()
        Me.cmdEditAd = New System.Windows.Forms.Button()
        Me.lvwAddress = New System.Windows.Forms.ListView()
        Me._lvwAddress_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddress_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddress_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddress_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddress_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAddress_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraContact = New System.Windows.Forms.GroupBox()
        Me.chkReceivesClientCorr = New System.Windows.Forms.CheckBox()
        Me.lblReceivesClientCorr = New System.Windows.Forms.Label()
        Me.cboCorrespondenceType = New System.Windows.Forms.ComboBox()
        Me.cmdAddCon = New System.Windows.Forms.Button()
        Me.lblPreferredCorrespondence = New System.Windows.Forms.Label()
        Me.cmdDeleteCon = New System.Windows.Forms.Button()
        Me.cmdEditCon = New System.Windows.Forms.Button()
        Me.lvwContact = New System.Windows.Forms.ListView()
        Me._lvwContact_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContact_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContact_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContact_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwContact_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me.fraCommissionRelease = New System.Windows.Forms.GroupBox()
        Me.cboCommissionPostingType = New PMLookupControl.cboPMLookup()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.fraAccountLimits = New System.Windows.Forms.GroupBox()
        Me.chkStandardAccount = New System.Windows.Forms.CheckBox()
        Me.fraAccounts = New System.Windows.Forms.Panel()
        Me.txtDaysAllowed = New System.Windows.Forms.TextBox()
        Me.txtOverdraftExpiry = New System.Windows.Forms.TextBox()
        Me.txtOverdraftLimit = New System.Windows.Forms.TextBox()
        Me.txtExpectedDailyPremium = New System.Windows.Forms.TextBox()
        Me.chkOverdraftAccount = New System.Windows.Forms.CheckBox()
        Me.chkPrepaymentAccount = New System.Windows.Forms.CheckBox()
        Me.chkFloatBalanceAccount = New System.Windows.Forms.CheckBox()
        Me.txtFloatBalanceLimit = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Frame4 = New System.Windows.Forms.GroupBox()
        Me.chkConsolidatedCommission = New System.Windows.Forms.CheckBox()
        Me.fraMakeLive = New System.Windows.Forms.GroupBox()
        Me.chkMakeLiveCashDeposit = New System.Windows.Forms.CheckBox()
        Me.chkMakeLiveBankGuarantee = New System.Windows.Forms.CheckBox()
        Me.chkMakeLivePayNow = New System.Windows.Forms.CheckBox()
        Me.chkMakeLiveInstallments = New System.Windows.Forms.CheckBox()
        Me.chkMakeLiveInvoice = New System.Windows.Forms.CheckBox()
        Me.fraPaymentTaxation = New System.Windows.Forms.GroupBox()
        Me.cboBankAccount = New PMLookupControl.cboPMLookup()
        Me.cboPaymentFrequency = New System.Windows.Forms.ComboBox()
        Me.cboPaymentMethod = New PMLookupControl.cboPMLookup()
        Me.lblPaymentMethod = New System.Windows.Forms.Label()
        Me.lblPaymentFrequency = New System.Windows.Forms.Label()
        Me.lblBankAccount = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me._cmdNext_4 = New System.Windows.Forms.Button()
        Me.fraDocuments = New System.Windows.Forms.GroupBox()
        Me.chkProductAgentRenewalList = New System.Windows.Forms.CheckBox()
        Me.lstDocsChosen = New System.Windows.Forms.ListBox()
        Me.cboAddressOnNotice = New System.Windows.Forms.ComboBox()
        Me.cmdMaintain = New System.Windows.Forms.Button()
        Me.lblAvailableDocs = New System.Windows.Forms.Label()
        Me.lblAddressOnNotice = New System.Windows.Forms.Label()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage()
        Me.fraCommission = New System.Windows.Forms.GroupBox()
        Me.cmdMaintainCommLevel = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me._lvwCommLvl_ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommLvl_ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraConsultant = New System.Windows.Forms.GroupBox()
        Me.cmdConsultantLookup = New System.Windows.Forms.Button()
        Me.txtConsultantRef = New System.Windows.Forms.TextBox()
        Me.pnlConsultantName = New System.Windows.Forms.Panel()
        Me.plblConsultantName = New System.Windows.Forms.Label()
        Me.lblConsultantName = New System.Windows.Forms.Label()
        Me.fraAgentGroup = New System.Windows.Forms.GroupBox()
        Me.txtAgentGroupRef = New System.Windows.Forms.TextBox()
        Me.cmdAgentGroupLookup = New System.Windows.Forms.Button()
        Me.pnlAgentGroupName = New System.Windows.Forms.Panel()
        Me.plblAgentGroupName = New System.Windows.Forms.Label()
        Me.lblAgentGroupName = New System.Windows.Forms.Label()
        Me.fraAssociate = New System.Windows.Forms.GroupBox()
        Me.cmdMaintainAssociates = New System.Windows.Forms.Button()
        Me.lvwAssociates = New System.Windows.Forms.ListView()
        Me._lvwAssociates_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me._cmdNext_5 = New System.Windows.Forms.Button()
        Me.fraAltRef = New System.Windows.Forms.GroupBox()
        Me.chkAltRefMandatory = New System.Windows.Forms.CheckBox()
        Me.chkAltRefRequiredForEachTrans = New System.Windows.Forms.CheckBox()
        Me.fraBrokerTransfer = New System.Windows.Forms.GroupBox()
        Me.cboBrokerTransferBusinessType = New System.Windows.Forms.ComboBox()
        Me.txtBrokerTransferToCode = New System.Windows.Forms.TextBox()
        Me.cmdBrokerTransferTo = New System.Windows.Forms.Button()
        Me.chkBrokerInTransferMode = New System.Windows.Forms.CheckBox()
        Me.lblBrokerTransferBusinessType = New System.Windows.Forms.Label()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.txtFirstName = New System.Windows.Forms.TextBox()
        Me.txtContactPerson = New System.Windows.Forms.TextBox()
        Me.ddTitle = New PMListMgrDropdown.uctDropdown()
        Me.lblFirstName = New System.Windows.Forms.Label()
        Me.lblContactPerson = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.cboRenewalStopCode = New System.Windows.Forms.ComboBox()
        Me.txtDateCancelled = New System.Windows.Forms.TextBox()
        Me.cboMultipac = New System.Windows.Forms.ComboBox()
        Me.lblMultipac = New System.Windows.Forms.Label()
        Me.lblRenewalStopCode = New System.Windows.Forms.Label()
        Me.lblDateCancelled = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_5 = New System.Windows.Forms.Button()
        Me.lblAgentStatus = New System.Windows.Forms.Label()
        Me.lblRegistrationNumber = New System.Windows.Forms.Label()
        Me.fraRiskGroups = New System.Windows.Forms.GroupBox()
        Me.cmdAddAll = New System.Windows.Forms.Button()
        Me.cmdRemove = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdRemoveAll = New System.Windows.Forms.Button()
        Me.lvwUnderTraining = New System.Windows.Forms.ListView()
        Me._lvwUnderTraining_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwCompetent = New System.Windows.Forms.ListView()
        Me._lvwCompetent_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblCompetant = New System.Windows.Forms.Label()
        Me.lblUnderTraining = New System.Windows.Forms.Label()
        Me.cboAgentStatus = New System.Windows.Forms.ComboBox()
        Me.txtRegistrationNumber = New System.Windows.Forms.TextBox()
        Me._cmdNext_6 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax()
        Me._cmdPrevious_6 = New System.Windows.Forms.Button()
        Me._cmdNext_7 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_7 = New System.Windows.Forms.Button()
        Me._cmdNext_8 = New System.Windows.Forms.Button()
        Me.uctPickListBranches = New uctPickList.PickList()
        Me._tabMainTab_TabPage9 = New System.Windows.Forms.TabPage()
        Me._cmdNext_9 = New System.Windows.Forms.Button()
        Me._cmdPrevious_8 = New System.Windows.Forms.Button()
        Me.uctPartyBankControl1 = New uctPartyBank.uctPartyBankControl()
        Me._tabMainTab_TabPage10 = New System.Windows.Forms.TabPage()
        Me._cmdNext_10 = New System.Windows.Forms.Button()
        Me._cmdPrevious_9 = New System.Windows.Forms.Button()
        Me.uctPickListProducts = New uctPickList.PickList()
        Me._tabMainTab_TabPage11 = New System.Windows.Forms.TabPage()
        Me._cmdNext_11 = New System.Windows.Forms.Button()
        Me._cmdPrevious_10 = New System.Windows.Forms.Button()
        Me.fraUsers = New System.Windows.Forms.GroupBox()
        Me.lvwAgencyUsers = New System.Windows.Forms.ListView()
        Me._tabMainTab_TabPage12 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_11 = New System.Windows.Forms.Button()
        Me.fraCertYears = New System.Windows.Forms.GroupBox()
        Me.cmdAddCertYear = New System.Windows.Forms.Button()
        Me.cmdDelCertYear = New System.Windows.Forms.Button()
        Me.cmdEditCertYear = New System.Windows.Forms.Button()
        Me.lvwCertYears = New System.Windows.Forms.ListView()
        Me._lvwCertYears_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCertYears_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCertYears_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCertYears_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCertYears_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblAgencyUsers = New System.Windows.Forms.Label()
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblReceiversClientCorr = New System.Windows.Forms.Label()
        Me.lblOverrideCommissionRen = New System.Windows.Forms.Label()
        Me.chkOverrideCommissionRen = New System.Windows.Forms.CheckBox()
        _lvwAgencyUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MainMenu1.SuspendLayout()
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraAppointment.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.fraPremiumSettlement.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraCommissionRelease.SuspendLayout()
        Me.fraAccountLimits.SuspendLayout()
        Me.fraAccounts.SuspendLayout()
        Me.Frame4.SuspendLayout()
        Me.fraMakeLive.SuspendLayout()
        Me.fraPaymentTaxation.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraDocuments.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraCommission.SuspendLayout()
        Me.fraConsultant.SuspendLayout()
        Me.pnlConsultantName.SuspendLayout()
        Me.fraAgentGroup.SuspendLayout()
        Me.pnlAgentGroupName.SuspendLayout()
        Me.fraAssociate.SuspendLayout()
        Me.fraAltRef.SuspendLayout()
        Me.fraBrokerTransfer.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.fraRiskGroups.SuspendLayout()
        Me._tabMainTab_TabPage7.SuspendLayout()
        Me._tabMainTab_TabPage8.SuspendLayout()
        Me._tabMainTab_TabPage9.SuspendLayout()
        Me._tabMainTab_TabPage10.SuspendLayout()
        Me._tabMainTab_TabPage11.SuspendLayout()
        Me.fraUsers.SuspendLayout()
        Me._tabMainTab_TabPage12.SuspendLayout()
        Me.fraCertYears.SuspendLayout()
        Me.SuspendLayout()
        '
        '_lvwAgencyUsers_ColumnHeader_1
        '
        _lvwAgencyUsers_ColumnHeader_1.Tag = ""
        _lvwAgencyUsers_ColumnHeader_1.Text = "UserName"
        _lvwAgencyUsers_ColumnHeader_1.Width = 232
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFind, Me.mnuRelatedDocuments})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.MainMenu1.Size = New System.Drawing.Size(724, 24)
        Me.MainMenu1.TabIndex = 166
        '
        'mnuFind
        '
        Me.mnuFind.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFinancial, Me.mnuCommission})
        Me.mnuFind.Name = "mnuFind"
        Me.mnuFind.Size = New System.Drawing.Size(42, 20)
        Me.mnuFind.Text = "&Find"
        '
        'mnuFinancial
        '
        Me.mnuFinancial.Name = "mnuFinancial"
        Me.mnuFinancial.Size = New System.Drawing.Size(141, 22)
        Me.mnuFinancial.Text = "Financial"
        '
        'mnuCommission
        '
        Me.mnuCommission.Name = "mnuCommission"
        Me.mnuCommission.Size = New System.Drawing.Size(141, 22)
        Me.mnuCommission.Text = "Commission"
        '
        'mnuRelatedDocuments
        '
        Me.mnuRelatedDocuments.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNotes, Me.mnuLetter})
        Me.mnuRelatedDocuments.Name = "mnuRelatedDocuments"
        Me.mnuRelatedDocuments.Size = New System.Drawing.Size(122, 20)
        Me.mnuRelatedDocuments.Text = "&Related Documents"
        '
        'mnuNotes
        '
        Me.mnuNotes.Name = "mnuNotes"
        Me.mnuNotes.Size = New System.Drawing.Size(105, 22)
        Me.mnuNotes.Text = "Notes"
        '
        'mnuLetter
        '
        Me.mnuLetter.Name = "mnuLetter"
        Me.mnuLetter.Size = New System.Drawing.Size(105, 22)
        Me.mnuLetter.Text = "Letter"
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList2
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Toolbar1_Button1, Me._Toolbar1_Button2, Me._Toolbar1_Button3, Me._Toolbar1_Button4, Me._Toolbar1_Button5, Me._Toolbar1_Button6})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 24)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Toolbar1.Size = New System.Drawing.Size(724, 25)
        Me.Toolbar1.TabIndex = 107
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "COMPETENT")
        Me.ImageList2.Images.SetKeyName(2, "NOTE")
        Me.ImageList2.Images.SetKeyName(3, "LETTER")
        Me.ImageList2.Images.SetKeyName(4, "COMMISSION")
        Me.ImageList2.Images.SetKeyName(5, "ADDRESS")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "CD")
        '
        '_Toolbar1_Button1
        '
        Me._Toolbar1_Button1.AutoSize = False
        Me._Toolbar1_Button1.ImageKey = "FINANCIAL"
        Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button1.Name = "_Toolbar1_Button1"
        Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button1.Tag = ""
        Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button1.ToolTipText = "Financial"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.ImageKey = "COMMISSION"
        Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button2.Tag = ""
        Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button2.ToolTipText = "Commission"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button3.Tag = ""
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.ImageKey = "NOTE"
        Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button4.Tag = ""
        Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button4.ToolTipText = "Notes"
        '
        '_Toolbar1_Button5
        '
        Me._Toolbar1_Button5.AutoSize = False
        Me._Toolbar1_Button5.ImageKey = "LETTER"
        Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button5.Name = "_Toolbar1_Button5"
        Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button5.Tag = ""
        Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button5.ToolTipText = "Letter"
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.ImageKey = "CD"
        Me._Toolbar1_Button6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button6.Tag = ""
        Me._Toolbar1_Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button6.ToolTipText = "Cash Deposit"
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(376, 555)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 165
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(616, 555)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 74
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(536, 555)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 73
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
        Me.cmdOK.Location = New System.Drawing.Point(456, 555)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 72
        Me.cmdOK.Text = "&OK"
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
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage5)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage6)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage7)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage8)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage9)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage10)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage11)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage12)
        Me.tabMainTab.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(75, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 56)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(700, 480)
        Me.tabMainTab.TabIndex = 76
        Me.tabMainTab.Visible = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAppointment)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Agency"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraAppointment
        '
        Me.fraAppointment.BackColor = System.Drawing.SystemColors.Control
        Me.fraAppointment.Controls.Add(Me.chkOverrideCommissionRen)
        Me.fraAppointment.Controls.Add(Me.lblOverrideCommissionRen)
        Me.fraAppointment.Controls.Add(Me.txtCommonRenewalDate)
        Me.fraAppointment.Controls.Add(Me.chkOverrideCommission)
        Me.fraAppointment.Controls.Add(Me.uctBranch)
        Me.fraAppointment.Controls.Add(Me.cboSubBranch)
        Me.fraAppointment.Controls.Add(Me.cmdRates)
        Me.fraAppointment.Controls.Add(Me.uctCategory)
        Me.fraAppointment.Controls.Add(Me.txtAgencyReviewDate)
        Me.fraAppointment.Controls.Add(Me.chkStatement)
        Me.fraAppointment.Controls.Add(Me.cboReportIndicator)
        Me.fraAppointment.Controls.Add(Me.cboBinderIndicator)
        Me.fraAppointment.Controls.Add(Me.cboTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.cboSource)
        Me.fraAppointment.Controls.Add(Me.uctCurrency)
        Me.fraAppointment.Controls.Add(Me.lblCommonRenewalDate)
        Me.fraAppointment.Controls.Add(Me.lblOverrideCommission)
        Me.fraAppointment.Controls.Add(Me.lblSubBranch)
        Me.fraAppointment.Controls.Add(Me.lblBranch)
        Me.fraAppointment.Controls.Add(Me.lblStatement)
        Me.fraAppointment.Controls.Add(Me.lblReportInd)
        Me.fraAppointment.Controls.Add(Me.lblBinderInd)
        Me.fraAppointment.Controls.Add(Me.lblTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.lblCurrency)
        Me.fraAppointment.Controls.Add(Me.lblAgencyNextReview)
        Me.fraAppointment.Controls.Add(Me.lblSource)
        Me.fraAppointment.Controls.Add(Me.lblCategory)
        Me.fraAppointment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAppointment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAppointment.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.fraAppointment.Location = New System.Drawing.Point(8, 175)
        Me.fraAppointment.Name = "fraAppointment"
        Me.fraAppointment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAppointment.Size = New System.Drawing.Size(593, 203)
        Me.fraAppointment.TabIndex = 75
        Me.fraAppointment.TabStop = False
        '
        'txtCommonRenewalDate
        '
        Me.txtCommonRenewalDate.AcceptsReturn = True
        Me.txtCommonRenewalDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommonRenewalDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommonRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommonRenewalDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommonRenewalDate.Location = New System.Drawing.Point(416, 160)
        Me.txtCommonRenewalDate.MaxLength = 0
        Me.txtCommonRenewalDate.Name = "txtCommonRenewalDate"
        Me.txtCommonRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommonRenewalDate.Size = New System.Drawing.Size(97, 20)
        Me.txtCommonRenewalDate.TabIndex = 18
        '
        'chkOverrideCommission
        '
        Me.chkOverrideCommission.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideCommission.Location = New System.Drawing.Point(272, 160)
        Me.chkOverrideCommission.Name = "chkOverrideCommission"
        Me.chkOverrideCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideCommission.Size = New System.Drawing.Size(17, 17)
        Me.chkOverrideCommission.TabIndex = 17
        Me.chkOverrideCommission.UseVisualStyleBackColor = False
        '
        'uctBranch
        '
        Me.uctBranch.BackColor = System.Drawing.SystemColors.Window
        Me.uctBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctBranch.Location = New System.Drawing.Point(416, 96)
        Me.uctBranch.Name = "uctBranch"
        Me.uctBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctBranch.Size = New System.Drawing.Size(161, 21)
        Me.uctBranch.TabIndex = 14
        Me.uctBranch.Text = " "
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(416, 128)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(161, 21)
        Me.cboSubBranch.TabIndex = 16
        Me.cboSubBranch.Text = " "
        '
        'cmdRates
        '
        Me.cmdRates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRates.Location = New System.Drawing.Point(504, 72)
        Me.cmdRates.Name = "cmdRates"
        Me.cmdRates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRates.Size = New System.Drawing.Size(73, 22)
        Me.cmdRates.TabIndex = 12
        Me.cmdRates.Text = "&Rates"
        Me.cmdRates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRates.UseVisualStyleBackColor = False
        '
        'uctCategory
        '
        Me.uctCategory.DefaultItemId = 0
        Me.uctCategory.FirstItem = "()"
        Me.uctCategory.ItemId = 0
        Me.uctCategory.ListIndex = -1
        Me.uctCategory.Location = New System.Drawing.Point(128, 40)
        Me.uctCategory.Name = "uctCategory"
        Me.uctCategory.PMLookupProductFamily = 9
        Me.uctCategory.SingleItemId = 0
        Me.uctCategory.Size = New System.Drawing.Size(161, 21)
        Me.uctCategory.SortColumnName = ""
        Me.uctCategory.Sorted = True
        Me.uctCategory.TabIndex = 8
        Me.uctCategory.TableName = "party_category"
        Me.uctCategory.ToolTipText = ""
        Me.uctCategory.WhereClause = ""
        '
        'txtAgencyReviewDate
        '
        Me.txtAgencyReviewDate.AcceptsReturn = True
        Me.txtAgencyReviewDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgencyReviewDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgencyReviewDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgencyReviewDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgencyReviewDate.Location = New System.Drawing.Point(128, 72)
        Me.txtAgencyReviewDate.MaxLength = 0
        Me.txtAgencyReviewDate.Name = "txtAgencyReviewDate"
        Me.txtAgencyReviewDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgencyReviewDate.Size = New System.Drawing.Size(161, 20)
        Me.txtAgencyReviewDate.TabIndex = 10
        '
        'chkStatement
        '
        Me.chkStatement.BackColor = System.Drawing.SystemColors.Control
        Me.chkStatement.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkStatement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStatement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStatement.Location = New System.Drawing.Point(416, 73)
        Me.chkStatement.Name = "chkStatement"
        Me.chkStatement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkStatement.Size = New System.Drawing.Size(17, 17)
        Me.chkStatement.TabIndex = 11
        Me.chkStatement.UseVisualStyleBackColor = False
        '
        'cboReportIndicator
        '
        Me.cboReportIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboReportIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReportIndicator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReportIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReportIndicator.Location = New System.Drawing.Point(416, 40)
        Me.cboReportIndicator.Name = "cboReportIndicator"
        Me.cboReportIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReportIndicator.Size = New System.Drawing.Size(161, 21)
        Me.cboReportIndicator.TabIndex = 9
        '
        'cboBinderIndicator
        '
        Me.cboBinderIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboBinderIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBinderIndicator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBinderIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBinderIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBinderIndicator.Location = New System.Drawing.Point(416, 16)
        Me.cboBinderIndicator.Name = "cboBinderIndicator"
        Me.cboBinderIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBinderIndicator.Size = New System.Drawing.Size(161, 21)
        Me.cboBinderIndicator.TabIndex = 7
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.BackColor = System.Drawing.SystemColors.Window
        Me.cboTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTermsOfPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(128, 128)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(161, 21)
        Me.cboTermsOfPayment.TabIndex = 15
        Me.cboTermsOfPayment.Tag = "1441846"
        '
        'cboSource
        '
        Me.cboSource.BackColor = System.Drawing.SystemColors.Window
        Me.cboSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSource.Location = New System.Drawing.Point(128, 16)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSource.Size = New System.Drawing.Size(161, 21)
        Me.cboSource.TabIndex = 6
        '
        'uctCurrency
        '
        Me.uctCurrency.CompanyId = 0
        Me.uctCurrency.CurrencyId = 0
        Me.uctCurrency.DefaultCurrencyId = 26
        Me.uctCurrency.FirstItem = ""
        Me.uctCurrency.ListIndex = -1
        Me.uctCurrency.Location = New System.Drawing.Point(128, 96)
        Me.uctCurrency.Name = "uctCurrency"
        Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.uctCurrency.Size = New System.Drawing.Size(161, 21)
        Me.uctCurrency.TabIndex = 13
        Me.uctCurrency.ToolTipText = ""
        Me.uctCurrency.WhatsThisHelpID = 0
        '
        'lblCommonRenewalDate
        '
        Me.lblCommonRenewalDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommonRenewalDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommonRenewalDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommonRenewalDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommonRenewalDate.Location = New System.Drawing.Point(296, 160)
        Me.lblCommonRenewalDate.Name = "lblCommonRenewalDate"
        Me.lblCommonRenewalDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommonRenewalDate.Size = New System.Drawing.Size(120, 27)
        Me.lblCommonRenewalDate.TabIndex = 169
        Me.lblCommonRenewalDate.Text = "Common Renewal Date:"
        '
        'lblOverrideCommission
        '
        Me.lblOverrideCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverrideCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverrideCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverrideCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverrideCommission.Location = New System.Drawing.Point(8, 160)
        Me.lblOverrideCommission.Name = "lblOverrideCommission"
        Me.lblOverrideCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverrideCommission.Size = New System.Drawing.Size(193, 17)
        Me.lblOverrideCommission.TabIndex = 97
        Me.lblOverrideCommission.Text = "Use Override Commission Rate:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(296, 131)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(65, 13)
        Me.lblSubBranch.TabIndex = 96
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(296, 99)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 94
        Me.lblBranch.Text = "Branch:"
        '
        'lblStatement
        '
        Me.lblStatement.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatement.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatement.Location = New System.Drawing.Point(296, 75)
        Me.lblStatement.Name = "lblStatement"
        Me.lblStatement.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatement.Size = New System.Drawing.Size(97, 17)
        Me.lblStatement.TabIndex = 92
        Me.lblStatement.Text = "Statement ?"
        '
        'lblReportInd
        '
        Me.lblReportInd.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportInd.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportInd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportInd.Location = New System.Drawing.Point(296, 43)
        Me.lblReportInd.Name = "lblReportInd"
        Me.lblReportInd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportInd.Size = New System.Drawing.Size(105, 17)
        Me.lblReportInd.TabIndex = 90
        Me.lblReportInd.Text = "Report indicator:"
        '
        'lblBinderInd
        '
        Me.lblBinderInd.BackColor = System.Drawing.SystemColors.Control
        Me.lblBinderInd.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBinderInd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBinderInd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBinderInd.Location = New System.Drawing.Point(296, 19)
        Me.lblBinderInd.Name = "lblBinderInd"
        Me.lblBinderInd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBinderInd.Size = New System.Drawing.Size(105, 17)
        Me.lblBinderInd.TabIndex = 88
        Me.lblBinderInd.Text = "Binder indicator:"
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermsOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(8, 131)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(121, 17)
        Me.lblTermsOfPayment.TabIndex = 95
        Me.lblTermsOfPayment.Text = "Terms of payment:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 99)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(97, 17)
        Me.lblCurrency.TabIndex = 93
        Me.lblCurrency.Text = "Currency:"
        '
        'lblAgencyNextReview
        '
        Me.lblAgencyNextReview.AutoSize = True
        Me.lblAgencyNextReview.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgencyNextReview.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgencyNextReview.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyNextReview.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgencyNextReview.Location = New System.Drawing.Point(8, 75)
        Me.lblAgencyNextReview.Name = "lblAgencyNextReview"
        Me.lblAgencyNextReview.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgencyNextReview.Size = New System.Drawing.Size(90, 13)
        Me.lblAgencyNextReview.TabIndex = 91
        Me.lblAgencyNextReview.Text = "Next review date:"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(8, 19)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(44, 13)
        Me.lblSource.TabIndex = 87
        Me.lblSource.Text = "Source:"
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Location = New System.Drawing.Point(8, 43)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategory.Size = New System.Drawing.Size(52, 13)
        Me.lblCategory.TabIndex = 89
        Me.lblCategory.Text = "Category:"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.fraPremiumSettlement)
        Me.Frame3.Controls.Add(Me.chkSingleInstalmentPlanOnly)
        Me.Frame3.Controls.Add(Me.cmbAgentType)
        Me.Frame3.Controls.Add(Me.uctAgentType)
        Me.Frame3.Controls.Add(Me.txtAccountNumber)
        Me.Frame3.Controls.Add(Me.txtFileCode)
        Me.Frame3.Controls.Add(Me.txtName)
        Me.Frame3.Controls.Add(Me.txtIDReference)
        Me.Frame3.Controls.Add(Me.actExpenseAcc)
        Me.Frame3.Controls.Add(Me.txtBrokerAbiId)
        Me.Frame3.Controls.Add(Me.lblExpenseAcc)
        Me.Frame3.Controls.Add(Me.lblBrokerAbiId)
        Me.Frame3.Controls.Add(Me.lblACcountNumber)
        Me.Frame3.Controls.Add(Me.lblFileCode)
        Me.Frame3.Controls.Add(Me.lblAgentType)
        Me.Frame3.Controls.Add(Me.lblName)
        Me.Frame3.Controls.Add(Me.lblIDReference)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 4)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(593, 165)
        Me.Frame3.TabIndex = 77
        Me.Frame3.TabStop = False
        '
        'fraPremiumSettlement
        '
        Me.fraPremiumSettlement.Controls.Add(Me.OptNetOfcommission)
        Me.fraPremiumSettlement.Controls.Add(Me.OptGrossOfCommission)
        Me.fraPremiumSettlement.Location = New System.Drawing.Point(399, 89)
        Me.fraPremiumSettlement.Name = "fraPremiumSettlement"
        Me.fraPremiumSettlement.Size = New System.Drawing.Size(179, 65)
        Me.fraPremiumSettlement.TabIndex = 87
        Me.fraPremiumSettlement.TabStop = False
        Me.fraPremiumSettlement.Text = "Premium Settlement"
        '
        'OptNetOfcommission
        '
        Me.OptNetOfcommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.OptNetOfcommission.Location = New System.Drawing.Point(11, 39)
        Me.OptNetOfcommission.Name = "OptNetOfcommission"
        Me.OptNetOfcommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptNetOfcommission.Size = New System.Drawing.Size(134, 17)
        Me.OptNetOfcommission.TabIndex = 1
        Me.OptNetOfcommission.Tag = ""
        Me.OptNetOfcommission.Text = "Net Of Commission             "
        Me.OptNetOfcommission.UseVisualStyleBackColor = True
        '
        'OptGrossOfCommission
        '
        Me.OptGrossOfCommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.OptGrossOfCommission.Location = New System.Drawing.Point(9, 17)
        Me.OptGrossOfCommission.Name = "OptGrossOfCommission"
        Me.OptGrossOfCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptGrossOfCommission.Size = New System.Drawing.Size(135, 17)
        Me.OptGrossOfCommission.TabIndex = 0
        Me.OptGrossOfCommission.Text = "Gross Of Commission"
        Me.OptGrossOfCommission.UseVisualStyleBackColor = False
        '
        'chkSingleInstalmentPlanOnly
        '
        Me.chkSingleInstalmentPlanOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkSingleInstalmentPlanOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSingleInstalmentPlanOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSingleInstalmentPlanOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSingleInstalmentPlanOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSingleInstalmentPlanOnly.Location = New System.Drawing.Point(392, 64)
        Me.chkSingleInstalmentPlanOnly.Name = "chkSingleInstalmentPlanOnly"
        Me.chkSingleInstalmentPlanOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSingleInstalmentPlanOnly.Size = New System.Drawing.Size(185, 25)
        Me.chkSingleInstalmentPlanOnly.TabIndex = 5
        Me.chkSingleInstalmentPlanOnly.Text = "Single Instalment Plan Only"
        Me.chkSingleInstalmentPlanOnly.UseVisualStyleBackColor = False
        '
        'cmbAgentType
        '
        Me.cmbAgentType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbAgentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbAgentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAgentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAgentType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbAgentType.Location = New System.Drawing.Point(418, 16)
        Me.cmbAgentType.Name = "cmbAgentType"
        Me.cmbAgentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbAgentType.Size = New System.Drawing.Size(161, 21)
        Me.cmbAgentType.TabIndex = 1
        '
        'uctAgentType
        '
        Me.uctAgentType.DefaultItemId = 0
        Me.uctAgentType.FirstItem = ""
        Me.uctAgentType.ItemId = 0
        Me.uctAgentType.ListIndex = -1
        Me.uctAgentType.Location = New System.Drawing.Point(416, 16)
        Me.uctAgentType.Name = "uctAgentType"
        Me.uctAgentType.PMLookupProductFamily = 9
        Me.uctAgentType.SingleItemId = 0
        Me.uctAgentType.Size = New System.Drawing.Size(161, 21)
        Me.uctAgentType.SortColumnName = ""
        Me.uctAgentType.Sorted = True
        Me.uctAgentType.TabIndex = 80
        Me.uctAgentType.TableName = "party_agent_type"
        Me.uctAgentType.ToolTipText = ""
        Me.uctAgentType.WhereClause = ""
        '
        'txtAccountNumber
        '
        Me.txtAccountNumber.AcceptsReturn = True
        Me.txtAccountNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountNumber.Location = New System.Drawing.Point(128, 64)
        Me.txtAccountNumber.MaxLength = 0
        Me.txtAccountNumber.Name = "txtAccountNumber"
        Me.txtAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountNumber.Size = New System.Drawing.Size(161, 20)
        Me.txtAccountNumber.TabIndex = 4
        '
        'txtFileCode
        '
        Me.txtFileCode.AcceptsReturn = True
        Me.txtFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileCode.Location = New System.Drawing.Point(416, 40)
        Me.txtFileCode.MaxLength = 50
        Me.txtFileCode.Name = "txtFileCode"
        Me.txtFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileCode.Size = New System.Drawing.Size(161, 20)
        Me.txtFileCode.TabIndex = 3
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(128, 40)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(161, 20)
        Me.txtName.TabIndex = 2
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(128, 16)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(161, 20)
        Me.txtIDReference.TabIndex = 0
        '
        'actExpenseAcc
        '
        Me.actExpenseAcc.AccountId = 0
        Me.actExpenseAcc.AllowStoppedAccounts = False
        Me.actExpenseAcc.BackStyle = 0
        Me.actExpenseAcc.CompanyId = 0
        Me.actExpenseAcc.Default_Renamed = False
        Me.actExpenseAcc.Location = New System.Drawing.Point(416, 64)
        Me.actExpenseAcc.LookupCaption = "..."
        Me.actExpenseAcc.LookupHeight = 285
        Me.actExpenseAcc.LookupLeft = 2055
        Me.actExpenseAcc.LookupTextLeft = 0
        Me.actExpenseAcc.LookupTextWidth = 2055
        Me.actExpenseAcc.LookupWidth = 360
        Me.actExpenseAcc.Name = "actExpenseAcc"
        Me.actExpenseAcc.OnlyUpdatableAccounts = False
        Me.actExpenseAcc.SelLength = 0
        Me.actExpenseAcc.SelStart = 0
        Me.actExpenseAcc.SelText = ""
        Me.actExpenseAcc.ShowEditOnFindAccount = False
        Me.actExpenseAcc.Size = New System.Drawing.Size(161, 19)
        Me.actExpenseAcc.TabIndex = 20
        Me.actExpenseAcc.ToolTipText = ""
        Me.actExpenseAcc.Visible = False
        '
        'txtBrokerAbiId
        '
        Me.txtBrokerAbiId.AcceptsReturn = True
        Me.txtBrokerAbiId.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerAbiId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerAbiId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerAbiId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerAbiId.Location = New System.Drawing.Point(416, 64)
        Me.txtBrokerAbiId.MaxLength = 8
        Me.txtBrokerAbiId.Name = "txtBrokerAbiId"
        Me.txtBrokerAbiId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerAbiId.Size = New System.Drawing.Size(161, 20)
        Me.txtBrokerAbiId.TabIndex = 86
        Me.txtBrokerAbiId.Visible = False
        '
        'lblExpenseAcc
        '
        Me.lblExpenseAcc.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpenseAcc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpenseAcc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpenseAcc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpenseAcc.Location = New System.Drawing.Point(296, 64)
        Me.lblExpenseAcc.Name = "lblExpenseAcc"
        Me.lblExpenseAcc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpenseAcc.Size = New System.Drawing.Size(121, 17)
        Me.lblExpenseAcc.TabIndex = 84
        Me.lblExpenseAcc.Text = "Expense account:"
        Me.lblExpenseAcc.Visible = False
        '
        'lblBrokerAbiId
        '
        Me.lblBrokerAbiId.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerAbiId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerAbiId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerAbiId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerAbiId.Location = New System.Drawing.Point(296, 64)
        Me.lblBrokerAbiId.Name = "lblBrokerAbiId"
        Me.lblBrokerAbiId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerAbiId.Size = New System.Drawing.Size(105, 17)
        Me.lblBrokerAbiId.TabIndex = 85
        Me.lblBrokerAbiId.Text = "Broker Abi Id:"
        Me.lblBrokerAbiId.Visible = False
        '
        'lblACcountNumber
        '
        Me.lblACcountNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblACcountNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblACcountNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblACcountNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblACcountNumber.Location = New System.Drawing.Point(8, 67)
        Me.lblACcountNumber.Name = "lblACcountNumber"
        Me.lblACcountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblACcountNumber.Size = New System.Drawing.Size(105, 17)
        Me.lblACcountNumber.TabIndex = 83
        Me.lblACcountNumber.Text = "Account number:"
        '
        'lblFileCode
        '
        Me.lblFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileCode.Location = New System.Drawing.Point(296, 43)
        Me.lblFileCode.Name = "lblFileCode"
        Me.lblFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileCode.Size = New System.Drawing.Size(105, 17)
        Me.lblFileCode.TabIndex = 82
        Me.lblFileCode.Text = "File code:"
        '
        'lblAgentType
        '
        Me.lblAgentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentType.Location = New System.Drawing.Point(296, 19)
        Me.lblAgentType.Name = "lblAgentType"
        Me.lblAgentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentType.Size = New System.Drawing.Size(121, 17)
        Me.lblAgentType.TabIndex = 79
        Me.lblAgentType.Text = "Agent type:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(8, 43)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(75, 13)
        Me.lblName.TabIndex = 81
        Me.lblName.Text = "Trading name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(8, 19)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(65, 13)
        Me.lblIDReference.TabIndex = 78
        Me.lblIDReference.Text = "Agent code:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(563, 390)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 19
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, 24)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 78
        Me.imgIcon.TabStop = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.lvwAddress)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(593, 285)
        Me.fraAddress.TabIndex = 98
        Me.fraAddress.TabStop = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 22
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Enabled = False
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 248)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 23
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Enabled = False
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 24
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'lvwAddress
        '
        Me.lvwAddress.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddress.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddress_ColumnHeader_1, Me._lvwAddress_ColumnHeader_2, Me._lvwAddress_ColumnHeader_3, Me._lvwAddress_ColumnHeader_4, Me._lvwAddress_ColumnHeader_5, Me._lvwAddress_ColumnHeader_6})
        Me.lvwAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddress.LargeImageList = Me.ImageList2
        Me.lvwAddress.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddress.Name = "lvwAddress"
        Me.lvwAddress.Size = New System.Drawing.Size(569, 225)
        Me.lvwAddress.SmallImageList = Me.ImageList2
        Me.lvwAddress.TabIndex = 21
        Me.lvwAddress.UseCompatibleStateImageBehavior = False
        Me.lvwAddress.View = System.Windows.Forms.View.Details
        '
        '_lvwAddress_ColumnHeader_1
        '
        Me._lvwAddress_ColumnHeader_1.Tag = ""
        Me._lvwAddress_ColumnHeader_1.Text = ""
        Me._lvwAddress_ColumnHeader_1.Width = 97
        '
        '_lvwAddress_ColumnHeader_2
        '
        Me._lvwAddress_ColumnHeader_2.Tag = ""
        Me._lvwAddress_ColumnHeader_2.Text = ""
        Me._lvwAddress_ColumnHeader_2.Width = 97
        '
        '_lvwAddress_ColumnHeader_3
        '
        Me._lvwAddress_ColumnHeader_3.Tag = ""
        Me._lvwAddress_ColumnHeader_3.Text = ""
        Me._lvwAddress_ColumnHeader_3.Width = 97
        '
        '_lvwAddress_ColumnHeader_4
        '
        Me._lvwAddress_ColumnHeader_4.Tag = ""
        Me._lvwAddress_ColumnHeader_4.Text = ""
        Me._lvwAddress_ColumnHeader_4.Width = 97
        '
        '_lvwAddress_ColumnHeader_5
        '
        Me._lvwAddress_ColumnHeader_5.Tag = ""
        Me._lvwAddress_ColumnHeader_5.Text = ""
        Me._lvwAddress_ColumnHeader_5.Width = 97
        '
        '_lvwAddress_ColumnHeader_6
        '
        Me._lvwAddress_ColumnHeader_6.Tag = ""
        Me._lvwAddress_ColumnHeader_6.Text = ""
        Me._lvwAddress_ColumnHeader_6.Width = 97
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(560, 300)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 26
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 300)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 25
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Contacts"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.chkReceivesClientCorr)
        Me.fraContact.Controls.Add(Me.lblReceivesClientCorr)
        Me.fraContact.Controls.Add(Me.cboCorrespondenceType)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lblPreferredCorrespondence)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.lvwContact)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 3)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(599, 336)
        Me.fraContact.TabIndex = 99
        Me.fraContact.TabStop = False
        '
        'chkReceivesClientCorr
        '
        Me.chkReceivesClientCorr.AutoSize = True
        Me.chkReceivesClientCorr.Location = New System.Drawing.Point(216, 312)
        Me.chkReceivesClientCorr.Name = "chkReceivesClientCorr"
        Me.chkReceivesClientCorr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReceivesClientCorr.Size = New System.Drawing.Size(15, 14)
        Me.chkReceivesClientCorr.TabIndex = 139
        Me.chkReceivesClientCorr.UseVisualStyleBackColor = True
        '
        'lblReceivesClientCorr
        '
        Me.lblReceivesClientCorr.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceivesClientCorr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceivesClientCorr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceivesClientCorr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceivesClientCorr.Location = New System.Drawing.Point(6, 312)
        Me.lblReceivesClientCorr.Name = "lblReceivesClientCorr"
        Me.lblReceivesClientCorr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceivesClientCorr.Size = New System.Drawing.Size(203, 17)
        Me.lblReceivesClientCorr.TabIndex = 138
        Me.lblReceivesClientCorr.Text = "Receives Client Correspondence:"
        '
        'cboCorrespondenceType
        '
        Me.cboCorrespondenceType.BackColor = System.Drawing.SystemColors.Window
        Me.cboCorrespondenceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCorrespondenceType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCorrespondenceType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCorrespondenceType.Location = New System.Drawing.Point(174, 280)
        Me.cboCorrespondenceType.Name = "cboCorrespondenceType"
        Me.cboCorrespondenceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCorrespondenceType.Size = New System.Drawing.Size(217, 21)
        Me.cboCorrespondenceType.TabIndex = 136
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 28
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lblPreferredCorrespondence
        '
        Me.lblPreferredCorrespondence.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreferredCorrespondence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreferredCorrespondence.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreferredCorrespondence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreferredCorrespondence.Location = New System.Drawing.Point(6, 284)
        Me.lblPreferredCorrespondence.Name = "lblPreferredCorrespondence"
        Me.lblPreferredCorrespondence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreferredCorrespondence.Size = New System.Drawing.Size(161, 17)
        Me.lblPreferredCorrespondence.TabIndex = 137
        Me.lblPreferredCorrespondence.Text = "Preferred Correspondence:"
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Enabled = False
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 248)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 29
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Enabled = False
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 30
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'lvwContact
        '
        Me.lvwContact.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContact.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContact_ColumnHeader_1, Me._lvwContact_ColumnHeader_2, Me._lvwContact_ColumnHeader_3, Me._lvwContact_ColumnHeader_4, Me._lvwContact_ColumnHeader_5})
        Me.lvwContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContact.LargeImageList = Me.ImageList2
        Me.lvwContact.Location = New System.Drawing.Point(8, 16)
        Me.lvwContact.Name = "lvwContact"
        Me.lvwContact.Size = New System.Drawing.Size(569, 225)
        Me.lvwContact.TabIndex = 27
        Me.lvwContact.UseCompatibleStateImageBehavior = False
        Me.lvwContact.View = System.Windows.Forms.View.Details
        '
        '_lvwContact_ColumnHeader_1
        '
        Me._lvwContact_ColumnHeader_1.Tag = ""
        Me._lvwContact_ColumnHeader_1.Text = ""
        Me._lvwContact_ColumnHeader_1.Width = 97
        '
        '_lvwContact_ColumnHeader_2
        '
        Me._lvwContact_ColumnHeader_2.Tag = ""
        Me._lvwContact_ColumnHeader_2.Text = ""
        Me._lvwContact_ColumnHeader_2.Width = 97
        '
        '_lvwContact_ColumnHeader_3
        '
        Me._lvwContact_ColumnHeader_3.Tag = ""
        Me._lvwContact_ColumnHeader_3.Text = ""
        Me._lvwContact_ColumnHeader_3.Width = 97
        '
        '_lvwContact_ColumnHeader_4
        '
        Me._lvwContact_ColumnHeader_4.Tag = ""
        Me._lvwContact_ColumnHeader_4.Text = ""
        Me._lvwContact_ColumnHeader_4.Width = 97
        '
        '_lvwContact_ColumnHeader_5
        '
        Me._lvwContact_ColumnHeader_5.Tag = ""
        Me._lvwContact_ColumnHeader_5.Text = ""
        Me._lvwContact_ColumnHeader_5.Width = 97
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 347)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 31
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(560, 347)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 32
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraCommissionRelease)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraAccountLimits)
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame4)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraMakeLive)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraPaymentTaxation)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Payments"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 314)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 33
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(560, 314)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 34
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'fraCommissionRelease
        '
        Me.fraCommissionRelease.BackColor = System.Drawing.SystemColors.Control
        Me.fraCommissionRelease.Controls.Add(Me.cboCommissionPostingType)
        Me.fraCommissionRelease.Controls.Add(Me.Label6)
        Me.fraCommissionRelease.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCommissionRelease.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCommissionRelease.Location = New System.Drawing.Point(8, 262)
        Me.fraCommissionRelease.Name = "fraCommissionRelease"
        Me.fraCommissionRelease.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCommissionRelease.Size = New System.Drawing.Size(593, 49)
        Me.fraCommissionRelease.TabIndex = 131
        Me.fraCommissionRelease.TabStop = False
        Me.fraCommissionRelease.Text = "Commission Release"
        '
        'cboCommissionPostingType
        '
        Me.cboCommissionPostingType.DefaultItemId = 0
        Me.cboCommissionPostingType.FirstItem = ""
        Me.cboCommissionPostingType.ItemId = 0
        Me.cboCommissionPostingType.ListIndex = -1
        Me.cboCommissionPostingType.Location = New System.Drawing.Point(208, 16)
        Me.cboCommissionPostingType.Name = "cboCommissionPostingType"
        Me.cboCommissionPostingType.PMLookupProductFamily = 2
        Me.cboCommissionPostingType.SingleItemId = 0
        Me.cboCommissionPostingType.Size = New System.Drawing.Size(377, 21)
        Me.cboCommissionPostingType.SortColumnName = ""
        Me.cboCommissionPostingType.Sorted = True
        Me.cboCommissionPostingType.TabIndex = 150
        Me.cboCommissionPostingType.TableName = "Commission_Posting_Type"
        Me.cboCommissionPostingType.ToolTipText = ""
        Me.cboCommissionPostingType.WhereClause = ""
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(16, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(194, 13)
        Me.Label6.TabIndex = 151
        Me.Label6.Text = "Agent Commission is posted:"
        '
        'fraAccountLimits
        '
        Me.fraAccountLimits.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccountLimits.Controls.Add(Me.chkStandardAccount)
        Me.fraAccountLimits.Controls.Add(Me.fraAccounts)
        Me.fraAccountLimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccountLimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccountLimits.Location = New System.Drawing.Point(8, 143)
        Me.fraAccountLimits.Name = "fraAccountLimits"
        Me.fraAccountLimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccountLimits.Size = New System.Drawing.Size(593, 113)
        Me.fraAccountLimits.TabIndex = 152
        Me.fraAccountLimits.TabStop = False
        Me.fraAccountLimits.Text = "Account Limits"
        '
        'chkStandardAccount
        '
        Me.chkStandardAccount.BackColor = System.Drawing.SystemColors.Control
        Me.chkStandardAccount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkStandardAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkStandardAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStandardAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStandardAccount.Location = New System.Drawing.Point(24, 16)
        Me.chkStandardAccount.Name = "chkStandardAccount"
        Me.chkStandardAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkStandardAccount.Size = New System.Drawing.Size(137, 17)
        Me.chkStandardAccount.TabIndex = 141
        Me.chkStandardAccount.Text = "Standard Account"
        Me.chkStandardAccount.UseVisualStyleBackColor = False
        '
        'fraAccounts
        '
        Me.fraAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccounts.Controls.Add(Me.txtDaysAllowed)
        Me.fraAccounts.Controls.Add(Me.txtOverdraftExpiry)
        Me.fraAccounts.Controls.Add(Me.txtOverdraftLimit)
        Me.fraAccounts.Controls.Add(Me.txtExpectedDailyPremium)
        Me.fraAccounts.Controls.Add(Me.chkOverdraftAccount)
        Me.fraAccounts.Controls.Add(Me.chkPrepaymentAccount)
        Me.fraAccounts.Controls.Add(Me.chkFloatBalanceAccount)
        Me.fraAccounts.Controls.Add(Me.txtFloatBalanceLimit)
        Me.fraAccounts.Controls.Add(Me.Label4)
        Me.fraAccounts.Controls.Add(Me.Label3)
        Me.fraAccounts.Controls.Add(Me.Label2)
        Me.fraAccounts.Controls.Add(Me.Label1)
        Me.fraAccounts.Controls.Add(Me.Label5)
        Me.fraAccounts.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraAccounts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccounts.Location = New System.Drawing.Point(4, 34)
        Me.fraAccounts.Name = "fraAccounts"
        Me.fraAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccounts.Size = New System.Drawing.Size(585, 73)
        Me.fraAccounts.TabIndex = 153
        '
        'txtDaysAllowed
        '
        Me.txtDaysAllowed.AcceptsReturn = True
        Me.txtDaysAllowed.BackColor = System.Drawing.SystemColors.Window
        Me.txtDaysAllowed.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDaysAllowed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDaysAllowed.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDaysAllowed.Location = New System.Drawing.Point(492, 22)
        Me.txtDaysAllowed.MaxLength = 0
        Me.txtDaysAllowed.Name = "txtDaysAllowed"
        Me.txtDaysAllowed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDaysAllowed.Size = New System.Drawing.Size(89, 20)
        Me.txtDaysAllowed.TabIndex = 148
        '
        'txtOverdraftExpiry
        '
        Me.txtOverdraftExpiry.AcceptsReturn = True
        Me.txtOverdraftExpiry.BackColor = System.Drawing.SystemColors.Window
        Me.txtOverdraftExpiry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverdraftExpiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverdraftExpiry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverdraftExpiry.Location = New System.Drawing.Point(492, 46)
        Me.txtOverdraftExpiry.MaxLength = 0
        Me.txtOverdraftExpiry.Name = "txtOverdraftExpiry"
        Me.txtOverdraftExpiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverdraftExpiry.Size = New System.Drawing.Size(89, 20)
        Me.txtOverdraftExpiry.TabIndex = 149
        '
        'txtOverdraftLimit
        '
        Me.txtOverdraftLimit.AcceptsReturn = True
        Me.txtOverdraftLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtOverdraftLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverdraftLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverdraftLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverdraftLimit.Location = New System.Drawing.Point(292, 46)
        Me.txtOverdraftLimit.MaxLength = 0
        Me.txtOverdraftLimit.Name = "txtOverdraftLimit"
        Me.txtOverdraftLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverdraftLimit.Size = New System.Drawing.Size(113, 20)
        Me.txtOverdraftLimit.TabIndex = 147
        '
        'txtExpectedDailyPremium
        '
        Me.txtExpectedDailyPremium.AcceptsReturn = True
        Me.txtExpectedDailyPremium.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpectedDailyPremium.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpectedDailyPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpectedDailyPremium.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpectedDailyPremium.Location = New System.Drawing.Point(292, 22)
        Me.txtExpectedDailyPremium.MaxLength = 0
        Me.txtExpectedDailyPremium.Name = "txtExpectedDailyPremium"
        Me.txtExpectedDailyPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpectedDailyPremium.Size = New System.Drawing.Size(113, 20)
        Me.txtExpectedDailyPremium.TabIndex = 146
        '
        'chkOverdraftAccount
        '
        Me.chkOverdraftAccount.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverdraftAccount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOverdraftAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverdraftAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverdraftAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverdraftAccount.Location = New System.Drawing.Point(20, 57)
        Me.chkOverdraftAccount.Name = "chkOverdraftAccount"
        Me.chkOverdraftAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverdraftAccount.Size = New System.Drawing.Size(137, 17)
        Me.chkOverdraftAccount.TabIndex = 144
        Me.chkOverdraftAccount.Text = "Overdraft Facility"
        Me.chkOverdraftAccount.UseVisualStyleBackColor = False
        '
        'chkPrepaymentAccount
        '
        Me.chkPrepaymentAccount.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrepaymentAccount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrepaymentAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrepaymentAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrepaymentAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrepaymentAccount.Location = New System.Drawing.Point(20, 41)
        Me.chkPrepaymentAccount.Name = "chkPrepaymentAccount"
        Me.chkPrepaymentAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrepaymentAccount.Size = New System.Drawing.Size(137, 17)
        Me.chkPrepaymentAccount.TabIndex = 143
        Me.chkPrepaymentAccount.Text = "Cash Deposit"
        Me.chkPrepaymentAccount.UseVisualStyleBackColor = False
        '
        'chkFloatBalanceAccount
        '
        Me.chkFloatBalanceAccount.BackColor = System.Drawing.SystemColors.Control
        Me.chkFloatBalanceAccount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFloatBalanceAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFloatBalanceAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFloatBalanceAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFloatBalanceAccount.Location = New System.Drawing.Point(20, 1)
        Me.chkFloatBalanceAccount.Name = "chkFloatBalanceAccount"
        Me.chkFloatBalanceAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFloatBalanceAccount.Size = New System.Drawing.Size(137, 17)
        Me.chkFloatBalanceAccount.TabIndex = 142
        Me.chkFloatBalanceAccount.Text = "Float Balance"
        Me.chkFloatBalanceAccount.UseVisualStyleBackColor = False
        '
        'txtFloatBalanceLimit
        '
        Me.txtFloatBalanceLimit.AcceptsReturn = True
        Me.txtFloatBalanceLimit.BackColor = System.Drawing.Color.White
        Me.txtFloatBalanceLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFloatBalanceLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFloatBalanceLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFloatBalanceLimit.Location = New System.Drawing.Point(292, 0)
        Me.txtFloatBalanceLimit.MaxLength = 0
        Me.txtFloatBalanceLimit.Name = "txtFloatBalanceLimit"
        Me.txtFloatBalanceLimit.ReadOnly = True
        Me.txtFloatBalanceLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFloatBalanceLimit.Size = New System.Drawing.Size(113, 20)
        Me.txtFloatBalanceLimit.TabIndex = 145
        Me.txtFloatBalanceLimit.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(140, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(121, 13)
        Me.Label4.TabIndex = 158
        Me.Label4.Text = "Expected Daily Premium"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(252, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(28, 13)
        Me.Label3.TabIndex = 157
        Me.Label3.Text = "Limit"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(420, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 156
        Me.Label2.Text = "Expiry Date"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(412, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 155
        Me.Label1.Text = "Days allowed"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(204, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 154
        Me.Label5.Text = "Float Balance"
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.chkConsolidatedCommission)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(8, 85)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(249, 59)
        Me.Frame4.TabIndex = 159
        Me.Frame4.TabStop = False
        Me.Frame4.Text = "True Monthly Policies"
        '
        'chkConsolidatedCommission
        '
        Me.chkConsolidatedCommission.BackColor = System.Drawing.SystemColors.Control
        Me.chkConsolidatedCommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkConsolidatedCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkConsolidatedCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkConsolidatedCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkConsolidatedCommission.Location = New System.Drawing.Point(16, 24)
        Me.chkConsolidatedCommission.Name = "chkConsolidatedCommission"
        Me.chkConsolidatedCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkConsolidatedCommission.Size = New System.Drawing.Size(217, 17)
        Me.chkConsolidatedCommission.TabIndex = 135
        Me.chkConsolidatedCommission.Text = "Allow Consolidated Commission:"
        Me.chkConsolidatedCommission.UseVisualStyleBackColor = False
        '
        'fraMakeLive
        '
        Me.fraMakeLive.BackColor = System.Drawing.SystemColors.Control
        Me.fraMakeLive.Controls.Add(Me.chkMakeLiveCashDeposit)
        Me.fraMakeLive.Controls.Add(Me.chkMakeLiveBankGuarantee)
        Me.fraMakeLive.Controls.Add(Me.chkMakeLivePayNow)
        Me.fraMakeLive.Controls.Add(Me.chkMakeLiveInstallments)
        Me.fraMakeLive.Controls.Add(Me.chkMakeLiveInvoice)
        Me.fraMakeLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMakeLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMakeLive.Location = New System.Drawing.Point(264, 85)
        Me.fraMakeLive.Name = "fraMakeLive"
        Me.fraMakeLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMakeLive.Size = New System.Drawing.Size(337, 59)
        Me.fraMakeLive.TabIndex = 160
        Me.fraMakeLive.TabStop = False
        Me.fraMakeLive.Text = "Options at Make Live"
        '
        'chkMakeLiveCashDeposit
        '
        Me.chkMakeLiveCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.chkMakeLiveCashDeposit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMakeLiveCashDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMakeLiveCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMakeLiveCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMakeLiveCashDeposit.Location = New System.Drawing.Point(25, 34)
        Me.chkMakeLiveCashDeposit.Name = "chkMakeLiveCashDeposit"
        Me.chkMakeLiveCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMakeLiveCashDeposit.Size = New System.Drawing.Size(117, 17)
        Me.chkMakeLiveCashDeposit.TabIndex = 139
        Me.chkMakeLiveCashDeposit.Text = "Cash Deposit"
        Me.chkMakeLiveCashDeposit.UseVisualStyleBackColor = False
        '
        'chkMakeLiveBankGuarantee
        '
        Me.chkMakeLiveBankGuarantee.BackColor = System.Drawing.SystemColors.Control
        Me.chkMakeLiveBankGuarantee.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMakeLiveBankGuarantee.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMakeLiveBankGuarantee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMakeLiveBankGuarantee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMakeLiveBankGuarantee.Location = New System.Drawing.Point(170, 36)
        Me.chkMakeLiveBankGuarantee.Name = "chkMakeLiveBankGuarantee"
        Me.chkMakeLiveBankGuarantee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMakeLiveBankGuarantee.Size = New System.Drawing.Size(117, 17)
        Me.chkMakeLiveBankGuarantee.TabIndex = 140
        Me.chkMakeLiveBankGuarantee.Text = "Bank Guarantee"
        Me.chkMakeLiveBankGuarantee.UseVisualStyleBackColor = False
        '
        'chkMakeLivePayNow
        '
        Me.chkMakeLivePayNow.BackColor = System.Drawing.SystemColors.Control
        Me.chkMakeLivePayNow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMakeLivePayNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMakeLivePayNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMakeLivePayNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMakeLivePayNow.Location = New System.Drawing.Point(232, 16)
        Me.chkMakeLivePayNow.Name = "chkMakeLivePayNow"
        Me.chkMakeLivePayNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMakeLivePayNow.Size = New System.Drawing.Size(81, 17)
        Me.chkMakeLivePayNow.TabIndex = 138
        Me.chkMakeLivePayNow.Text = "Pay Now"
        Me.chkMakeLivePayNow.UseVisualStyleBackColor = False
        '
        'chkMakeLiveInstallments
        '
        Me.chkMakeLiveInstallments.BackColor = System.Drawing.SystemColors.Control
        Me.chkMakeLiveInstallments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMakeLiveInstallments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMakeLiveInstallments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMakeLiveInstallments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMakeLiveInstallments.Location = New System.Drawing.Point(104, 16)
        Me.chkMakeLiveInstallments.Name = "chkMakeLiveInstallments"
        Me.chkMakeLiveInstallments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMakeLiveInstallments.Size = New System.Drawing.Size(105, 17)
        Me.chkMakeLiveInstallments.TabIndex = 137
        Me.chkMakeLiveInstallments.Text = "Instalments"
        Me.chkMakeLiveInstallments.UseVisualStyleBackColor = False
        '
        'chkMakeLiveInvoice
        '
        Me.chkMakeLiveInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.chkMakeLiveInvoice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMakeLiveInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMakeLiveInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMakeLiveInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMakeLiveInvoice.Location = New System.Drawing.Point(8, 16)
        Me.chkMakeLiveInvoice.Name = "chkMakeLiveInvoice"
        Me.chkMakeLiveInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMakeLiveInvoice.Size = New System.Drawing.Size(81, 17)
        Me.chkMakeLiveInvoice.TabIndex = 136
        Me.chkMakeLiveInvoice.Text = "Invoice"
        Me.chkMakeLiveInvoice.UseVisualStyleBackColor = False
        '
        'fraPaymentTaxation
        '
        Me.fraPaymentTaxation.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentTaxation.Controls.Add(Me.cboBankAccount)
        Me.fraPaymentTaxation.Controls.Add(Me.cboPaymentFrequency)
        Me.fraPaymentTaxation.Controls.Add(Me.cboPaymentMethod)
        Me.fraPaymentTaxation.Controls.Add(Me.lblPaymentMethod)
        Me.fraPaymentTaxation.Controls.Add(Me.lblPaymentFrequency)
        Me.fraPaymentTaxation.Controls.Add(Me.lblBankAccount)
        Me.fraPaymentTaxation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentTaxation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentTaxation.Location = New System.Drawing.Point(8, 4)
        Me.fraPaymentTaxation.Name = "fraPaymentTaxation"
        Me.fraPaymentTaxation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentTaxation.Size = New System.Drawing.Size(593, 81)
        Me.fraPaymentTaxation.TabIndex = 161
        Me.fraPaymentTaxation.TabStop = False
        Me.fraPaymentTaxation.Text = "Payments"
        '
        'cboBankAccount
        '
        Me.cboBankAccount.DefaultItemId = 0
        Me.cboBankAccount.FirstItem = """(Any)"""
        Me.cboBankAccount.ItemId = 0
        Me.cboBankAccount.ListIndex = -1
        Me.cboBankAccount.Location = New System.Drawing.Point(414, 54)
        Me.cboBankAccount.Name = "cboBankAccount"
        Me.cboBankAccount.PMLookupProductFamily = 1
        Me.cboBankAccount.SingleItemId = 0
        Me.cboBankAccount.Size = New System.Drawing.Size(153, 21)
        Me.cboBankAccount.SortColumnName = ""
        Me.cboBankAccount.Sorted = True
        Me.cboBankAccount.TabIndex = 165
        Me.cboBankAccount.TableName = "bankaccount"
        Me.cboBankAccount.ToolTipText = ""
        Me.cboBankAccount.WhereClause = ""
        '
        'cboPaymentFrequency
        '
        Me.cboPaymentFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentFrequency.Location = New System.Drawing.Point(414, 18)
        Me.cboPaymentFrequency.Name = "cboPaymentFrequency"
        Me.cboPaymentFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentFrequency.Size = New System.Drawing.Size(153, 21)
        Me.cboPaymentFrequency.TabIndex = 133
        '
        'cboPaymentMethod
        '
        Me.cboPaymentMethod.DefaultItemId = 0
        Me.cboPaymentMethod.FirstItem = "()"
        Me.cboPaymentMethod.ItemId = 0
        Me.cboPaymentMethod.ListIndex = -1
        Me.cboPaymentMethod.Location = New System.Drawing.Point(144, 16)
        Me.cboPaymentMethod.Name = "cboPaymentMethod"
        Me.cboPaymentMethod.PMLookupProductFamily = 1
        Me.cboPaymentMethod.SingleItemId = 0
        Me.cboPaymentMethod.Size = New System.Drawing.Size(141, 21)
        Me.cboPaymentMethod.SortColumnName = ""
        Me.cboPaymentMethod.Sorted = True
        Me.cboPaymentMethod.TabIndex = 132
        Me.cboPaymentMethod.TableName = "MediaType"
        Me.cboPaymentMethod.ToolTipText = ""
        Me.cboPaymentMethod.WhereClause = "is_payment=1"
        '
        'lblPaymentMethod
        '
        Me.lblPaymentMethod.AutoSize = True
        Me.lblPaymentMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentMethod.Location = New System.Drawing.Point(20, 22)
        Me.lblPaymentMethod.Name = "lblPaymentMethod"
        Me.lblPaymentMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentMethod.Size = New System.Drawing.Size(90, 13)
        Me.lblPaymentMethod.TabIndex = 164
        Me.lblPaymentMethod.Text = "Payment Method:"
        '
        'lblPaymentFrequency
        '
        Me.lblPaymentFrequency.AutoSize = True
        Me.lblPaymentFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentFrequency.Location = New System.Drawing.Point(294, 22)
        Me.lblPaymentFrequency.Name = "lblPaymentFrequency"
        Me.lblPaymentFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentFrequency.Size = New System.Drawing.Size(104, 13)
        Me.lblPaymentFrequency.TabIndex = 163
        Me.lblPaymentFrequency.Text = "Payment Frequency:"
        '
        'lblBankAccount
        '
        Me.lblBankAccount.AutoSize = True
        Me.lblBankAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccount.Location = New System.Drawing.Point(294, 52)
        Me.lblBankAccount.Name = "lblBankAccount"
        Me.lblBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccount.Size = New System.Drawing.Size(78, 13)
        Me.lblBankAccount.TabIndex = 162
        Me.lblBankAccount.Text = "Bank Account:"
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraDocuments)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Documents"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(560, 300)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 40
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        'fraDocuments
        '
        Me.fraDocuments.BackColor = System.Drawing.SystemColors.Control
        Me.fraDocuments.Controls.Add(Me.chkProductAgentRenewalList)
        Me.fraDocuments.Controls.Add(Me.lstDocsChosen)
        Me.fraDocuments.Controls.Add(Me.cboAddressOnNotice)
        Me.fraDocuments.Controls.Add(Me.cmdMaintain)
        Me.fraDocuments.Controls.Add(Me.lblAvailableDocs)
        Me.fraDocuments.Controls.Add(Me.lblAddressOnNotice)
        Me.fraDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDocuments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDocuments.Location = New System.Drawing.Point(8, 4)
        Me.fraDocuments.Name = "fraDocuments"
        Me.fraDocuments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDocuments.Size = New System.Drawing.Size(593, 265)
        Me.fraDocuments.TabIndex = 100
        Me.fraDocuments.TabStop = False
        Me.fraDocuments.Text = "Documents && Output"
        '
        'chkProductAgentRenewalList
        '
        Me.chkProductAgentRenewalList.BackColor = System.Drawing.SystemColors.Control
        Me.chkProductAgentRenewalList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProductAgentRenewalList.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProductAgentRenewalList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProductAgentRenewalList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProductAgentRenewalList.Location = New System.Drawing.Point(6, 66)
        Me.chkProductAgentRenewalList.Name = "chkProductAgentRenewalList"
        Me.chkProductAgentRenewalList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProductAgentRenewalList.Size = New System.Drawing.Size(189, 17)
        Me.chkProductAgentRenewalList.TabIndex = 36
        Me.chkProductAgentRenewalList.Text = "Produce Agent Renewal List"
        Me.chkProductAgentRenewalList.UseVisualStyleBackColor = False
        '
        'lstDocsChosen
        '
        Me.lstDocsChosen.BackColor = System.Drawing.SystemColors.Window
        Me.lstDocsChosen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstDocsChosen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstDocsChosen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstDocsChosen.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstDocsChosen.Location = New System.Drawing.Point(8, 120)
        Me.lstDocsChosen.Name = "lstDocsChosen"
        Me.lstDocsChosen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstDocsChosen, System.Windows.Forms.SelectionMode.MultiExtended)
        Me.lstDocsChosen.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstDocsChosen.Size = New System.Drawing.Size(257, 108)
        Me.lstDocsChosen.TabIndex = 37
        '
        'cboAddressOnNotice
        '
        Me.cboAddressOnNotice.BackColor = System.Drawing.SystemColors.Window
        Me.cboAddressOnNotice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAddressOnNotice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAddressOnNotice.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAddressOnNotice.Location = New System.Drawing.Point(152, 32)
        Me.cboAddressOnNotice.Name = "cboAddressOnNotice"
        Me.cboAddressOnNotice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAddressOnNotice.Size = New System.Drawing.Size(217, 21)
        Me.cboAddressOnNotice.TabIndex = 35
        '
        'cmdMaintain
        '
        Me.cmdMaintain.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMaintain.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMaintain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMaintain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.Location = New System.Drawing.Point(272, 120)
        Me.cmdMaintain.Name = "cmdMaintain"
        Me.cmdMaintain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMaintain.Size = New System.Drawing.Size(73, 20)
        Me.cmdMaintain.TabIndex = 38
        Me.cmdMaintain.Text = "Maintain"
        Me.cmdMaintain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMaintain.UseVisualStyleBackColor = False
        '
        'lblAvailableDocs
        '
        Me.lblAvailableDocs.AutoSize = True
        Me.lblAvailableDocs.BackColor = System.Drawing.SystemColors.Control
        Me.lblAvailableDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAvailableDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAvailableDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAvailableDocs.Location = New System.Drawing.Point(8, 104)
        Me.lblAvailableDocs.Name = "lblAvailableDocs"
        Me.lblAvailableDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAvailableDocs.Size = New System.Drawing.Size(110, 13)
        Me.lblAvailableDocs.TabIndex = 102
        Me.lblAvailableDocs.Text = "Available Documents:"
        '
        'lblAddressOnNotice
        '
        Me.lblAddressOnNotice.AutoSize = True
        Me.lblAddressOnNotice.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddressOnNotice.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddressOnNotice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddressOnNotice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddressOnNotice.Location = New System.Drawing.Point(8, 32)
        Me.lblAddressOnNotice.Name = "lblAddressOnNotice"
        Me.lblAddressOnNotice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddressOnNotice.Size = New System.Drawing.Size(97, 13)
        Me.lblAddressOnNotice.TabIndex = 101
        Me.lblAddressOnNotice.Text = "Address on Notice:"
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 300)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 39
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraCommission)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraConsultant)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraAgentGroup)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraAssociate)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraAltRef)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraBrokerTransfer)
        Me._tabMainTab_TabPage5.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage5.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Additional Details"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraCommission
        '
        Me.fraCommission.BackColor = System.Drawing.SystemColors.Control
        Me.fraCommission.Controls.Add(Me.cmdMaintainCommLevel)
        Me.fraCommission.Controls.Add(Me.ListView1)
        Me.fraCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCommission.Location = New System.Drawing.Point(312, 263)
        Me.fraCommission.Name = "fraCommission"
        Me.fraCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCommission.Size = New System.Drawing.Size(290, 138)
        Me.fraCommission.TabIndex = 128
        Me.fraCommission.TabStop = False
        Me.fraCommission.Text = "Commission"
        '
        'cmdMaintainCommLevel
        '
        Me.cmdMaintainCommLevel.Location = New System.Drawing.Point(6, 110)
        Me.cmdMaintainCommLevel.Name = "cmdMaintainCommLevel"
        Me.cmdMaintainCommLevel.Size = New System.Drawing.Size(75, 23)
        Me.cmdMaintainCommLevel.TabIndex = 123
        Me.cmdMaintainCommLevel.Text = "Maintain"
        Me.cmdMaintainCommLevel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMaintainCommLevel.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCommLvl_ColumnHeader1, Me._lvwCommLvl_ColumnHeader2})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(6, 19)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(273, 85)
        Me.ListView1.TabIndex = 122
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        '_lvwCommLvl_ColumnHeader1
        '
        Me._lvwCommLvl_ColumnHeader1.Text = "Commission Level"
        Me._lvwCommLvl_ColumnHeader1.Width = 150
        '
        '_lvwCommLvl_ColumnHeader2
        '
        Me._lvwCommLvl_ColumnHeader2.Text = "Effective Date"
        Me._lvwCommLvl_ColumnHeader2.Width = 150
        '
        'fraConsultant
        '
        Me.fraConsultant.Controls.Add(Me.cmdConsultantLookup)
        Me.fraConsultant.Controls.Add(Me.txtConsultantRef)
        Me.fraConsultant.Controls.Add(Me.pnlConsultantName)
        Me.fraConsultant.Controls.Add(Me.lblConsultantName)
        Me.fraConsultant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraConsultant.Location = New System.Drawing.Point(8, 5)
        Me.fraConsultant.Name = "fraConsultant"
        Me.fraConsultant.Size = New System.Drawing.Size(289, 70)
        Me.fraConsultant.TabIndex = 127
        Me.fraConsultant.TabStop = False
        Me.fraConsultant.Text = "Account Executive"
        '
        'cmdConsultantLookup
        '
        Me.cmdConsultantLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConsultantLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConsultantLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConsultantLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConsultantLookup.Location = New System.Drawing.Point(8, 20)
        Me.cmdConsultantLookup.Name = "cmdConsultantLookup"
        Me.cmdConsultantLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConsultantLookup.Size = New System.Drawing.Size(60, 20)
        Me.cmdConsultantLookup.TabIndex = 41
        Me.cmdConsultantLookup.Text = "Code..."
        Me.cmdConsultantLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConsultantLookup.UseVisualStyleBackColor = False
        '
        'txtConsultantRef
        '
        Me.txtConsultantRef.AcceptsReturn = True
        Me.txtConsultantRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtConsultantRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConsultantRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsultantRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConsultantRef.Location = New System.Drawing.Point(80, 20)
        Me.txtConsultantRef.MaxLength = 0
        Me.txtConsultantRef.Name = "txtConsultantRef"
        Me.txtConsultantRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConsultantRef.Size = New System.Drawing.Size(201, 20)
        Me.txtConsultantRef.TabIndex = 42
        '
        'pnlConsultantName
        '
        Me.pnlConsultantName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConsultantName.Controls.Add(Me.plblConsultantName)
        Me.pnlConsultantName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConsultantName.Location = New System.Drawing.Point(80, 44)
        Me.pnlConsultantName.Name = "pnlConsultantName"
        Me.pnlConsultantName.Size = New System.Drawing.Size(201, 17)
        Me.pnlConsultantName.TabIndex = 128
        '
        'plblConsultantName
        '
        Me.plblConsultantName.AutoSize = True
        Me.plblConsultantName.Location = New System.Drawing.Point(2, 0)
        Me.plblConsultantName.Name = "plblConsultantName"
        Me.plblConsultantName.Size = New System.Drawing.Size(0, 13)
        Me.plblConsultantName.TabIndex = 0
        '
        'lblConsultantName
        '
        Me.lblConsultantName.AutoSize = True
        Me.lblConsultantName.BackColor = System.Drawing.SystemColors.Control
        Me.lblConsultantName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConsultantName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsultantName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConsultantName.Location = New System.Drawing.Point(8, 44)
        Me.lblConsultantName.Name = "lblConsultantName"
        Me.lblConsultantName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConsultantName.Size = New System.Drawing.Size(38, 13)
        Me.lblConsultantName.TabIndex = 129
        Me.lblConsultantName.Text = "Name:"
        '
        'fraAgentGroup
        '
        Me.fraAgentGroup.Controls.Add(Me.txtAgentGroupRef)
        Me.fraAgentGroup.Controls.Add(Me.cmdAgentGroupLookup)
        Me.fraAgentGroup.Controls.Add(Me.pnlAgentGroupName)
        Me.fraAgentGroup.Controls.Add(Me.lblAgentGroupName)
        Me.fraAgentGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgentGroup.Location = New System.Drawing.Point(8, 82)
        Me.fraAgentGroup.Name = "fraAgentGroup"
        Me.fraAgentGroup.Size = New System.Drawing.Size(289, 70)
        Me.fraAgentGroup.TabIndex = 124
        Me.fraAgentGroup.TabStop = False
        Me.fraAgentGroup.Text = "Agent Group"
        '
        'txtAgentGroupRef
        '
        Me.txtAgentGroupRef.AcceptsReturn = True
        Me.txtAgentGroupRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentGroupRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentGroupRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentGroupRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentGroupRef.Location = New System.Drawing.Point(80, 20)
        Me.txtAgentGroupRef.MaxLength = 0
        Me.txtAgentGroupRef.Name = "txtAgentGroupRef"
        Me.txtAgentGroupRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentGroupRef.Size = New System.Drawing.Size(201, 20)
        Me.txtAgentGroupRef.TabIndex = 47
        '
        'cmdAgentGroupLookup
        '
        Me.cmdAgentGroupLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentGroupLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentGroupLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentGroupLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentGroupLookup.Location = New System.Drawing.Point(8, 20)
        Me.cmdAgentGroupLookup.Name = "cmdAgentGroupLookup"
        Me.cmdAgentGroupLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentGroupLookup.Size = New System.Drawing.Size(60, 20)
        Me.cmdAgentGroupLookup.TabIndex = 46
        Me.cmdAgentGroupLookup.Text = "Code..."
        Me.cmdAgentGroupLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentGroupLookup.UseVisualStyleBackColor = False
        '
        'pnlAgentGroupName
        '
        Me.pnlAgentGroupName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAgentGroupName.Controls.Add(Me.plblAgentGroupName)
        Me.pnlAgentGroupName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAgentGroupName.Location = New System.Drawing.Point(80, 44)
        Me.pnlAgentGroupName.Name = "pnlAgentGroupName"
        Me.pnlAgentGroupName.Size = New System.Drawing.Size(201, 17)
        Me.pnlAgentGroupName.TabIndex = 125
        '
        'plblAgentGroupName
        '
        Me.plblAgentGroupName.AutoSize = True
        Me.plblAgentGroupName.Location = New System.Drawing.Point(2, 0)
        Me.plblAgentGroupName.Name = "plblAgentGroupName"
        Me.plblAgentGroupName.Size = New System.Drawing.Size(0, 13)
        Me.plblAgentGroupName.TabIndex = 0
        '
        'lblAgentGroupName
        '
        Me.lblAgentGroupName.AutoSize = True
        Me.lblAgentGroupName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentGroupName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentGroupName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentGroupName.Location = New System.Drawing.Point(8, 44)
        Me.lblAgentGroupName.Name = "lblAgentGroupName"
        Me.lblAgentGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentGroupName.Size = New System.Drawing.Size(38, 13)
        Me.lblAgentGroupName.TabIndex = 126
        Me.lblAgentGroupName.Text = "Name:"
        '
        'fraAssociate
        '
        Me.fraAssociate.Controls.Add(Me.cmdMaintainAssociates)
        Me.fraAssociate.Controls.Add(Me.lvwAssociates)
        Me.fraAssociate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAssociate.Location = New System.Drawing.Point(8, 159)
        Me.fraAssociate.Name = "fraAssociate"
        Me.fraAssociate.Size = New System.Drawing.Size(289, 104)
        Me.fraAssociate.TabIndex = 123
        Me.fraAssociate.TabStop = False
        Me.fraAssociate.Text = "Associated Agents"
        '
        'cmdMaintainAssociates
        '
        Me.cmdMaintainAssociates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMaintainAssociates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMaintainAssociates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMaintainAssociates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintainAssociates.Location = New System.Drawing.Point(8, 80)
        Me.cmdMaintainAssociates.Name = "cmdMaintainAssociates"
        Me.cmdMaintainAssociates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMaintainAssociates.Size = New System.Drawing.Size(73, 20)
        Me.cmdMaintainAssociates.TabIndex = 52
        Me.cmdMaintainAssociates.Text = "Maintain"
        Me.cmdMaintainAssociates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMaintainAssociates.UseVisualStyleBackColor = False
        '
        'lvwAssociates
        '
        Me.lvwAssociates.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAssociates.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAssociates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAssociates_ColumnHeader_1, Me._lvwAssociates_ColumnHeader_2, Me._lvwAssociates_ColumnHeader_3})
        Me.lvwAssociates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAssociates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAssociates.Location = New System.Drawing.Point(8, 20)
        Me.lvwAssociates.Name = "lvwAssociates"
        Me.lvwAssociates.Size = New System.Drawing.Size(273, 57)
        Me.lvwAssociates.TabIndex = 51
        Me.lvwAssociates.UseCompatibleStateImageBehavior = False
        Me.lvwAssociates.View = System.Windows.Forms.View.Details
        '
        '_lvwAssociates_ColumnHeader_1
        '
        Me._lvwAssociates_ColumnHeader_1.Tag = ""
        Me._lvwAssociates_ColumnHeader_1.Text = "Associate"
        Me._lvwAssociates_ColumnHeader_1.Width = 67
        '
        '_lvwAssociates_ColumnHeader_2
        '
        Me._lvwAssociates_ColumnHeader_2.Tag = ""
        Me._lvwAssociates_ColumnHeader_2.Text = "Name"
        Me._lvwAssociates_ColumnHeader_2.Width = 81
        '
        '_lvwAssociates_ColumnHeader_3
        '
        Me._lvwAssociates_ColumnHeader_3.Tag = ""
        Me._lvwAssociates_ColumnHeader_3.Text = "Relationship"
        Me._lvwAssociates_ColumnHeader_3.Width = 97
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(16, 407)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 59
        Me._cmdPrevious_4.Text = "<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(564, 409)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_5.TabIndex = 60
        Me._cmdNext_5.Text = ">>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        'fraAltRef
        '
        Me.fraAltRef.BackColor = System.Drawing.SystemColors.Control
        Me.fraAltRef.Controls.Add(Me.chkAltRefMandatory)
        Me.fraAltRef.Controls.Add(Me.chkAltRefRequiredForEachTrans)
        Me.fraAltRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAltRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAltRef.Location = New System.Drawing.Point(8, 263)
        Me.fraAltRef.Name = "fraAltRef"
        Me.fraAltRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAltRef.Size = New System.Drawing.Size(289, 62)
        Me.fraAltRef.TabIndex = 112
        Me.fraAltRef.TabStop = False
        Me.fraAltRef.Text = "Alternative Reference"
        '
        'chkAltRefMandatory
        '
        Me.chkAltRefMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkAltRefMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAltRefMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAltRefMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAltRefMandatory.Location = New System.Drawing.Point(16, 14)
        Me.chkAltRefMandatory.Name = "chkAltRefMandatory"
        Me.chkAltRefMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAltRefMandatory.Size = New System.Drawing.Size(219, 16)
        Me.chkAltRefMandatory.TabIndex = 57
        Me.chkAltRefMandatory.Text = "Alternative Reference Mandatory"
        Me.chkAltRefMandatory.UseVisualStyleBackColor = False
        '
        'chkAltRefRequiredForEachTrans
        '
        Me.chkAltRefRequiredForEachTrans.BackColor = System.Drawing.SystemColors.Control
        Me.chkAltRefRequiredForEachTrans.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAltRefRequiredForEachTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAltRefRequiredForEachTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAltRefRequiredForEachTrans.Location = New System.Drawing.Point(16, 36)
        Me.chkAltRefRequiredForEachTrans.Name = "chkAltRefRequiredForEachTrans"
        Me.chkAltRefRequiredForEachTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAltRefRequiredForEachTrans.Size = New System.Drawing.Size(265, 16)
        Me.chkAltRefRequiredForEachTrans.TabIndex = 58
        Me.chkAltRefRequiredForEachTrans.Text = "Alternative Reference cleared for each transaction"
        Me.chkAltRefRequiredForEachTrans.UseVisualStyleBackColor = False
        '
        'fraBrokerTransfer
        '
        Me.fraBrokerTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.fraBrokerTransfer.Controls.Add(Me.cboBrokerTransferBusinessType)
        Me.fraBrokerTransfer.Controls.Add(Me.txtBrokerTransferToCode)
        Me.fraBrokerTransfer.Controls.Add(Me.cmdBrokerTransferTo)
        Me.fraBrokerTransfer.Controls.Add(Me.chkBrokerInTransferMode)
        Me.fraBrokerTransfer.Controls.Add(Me.lblBrokerTransferBusinessType)
        Me.fraBrokerTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBrokerTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBrokerTransfer.Location = New System.Drawing.Point(312, 181)
        Me.fraBrokerTransfer.Name = "fraBrokerTransfer"
        Me.fraBrokerTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBrokerTransfer.Size = New System.Drawing.Size(290, 82)
        Me.fraBrokerTransfer.TabIndex = 113
        Me.fraBrokerTransfer.TabStop = False
        Me.fraBrokerTransfer.Text = "Broker/Agent Transfer"
        '
        'cboBrokerTransferBusinessType
        '
        Me.cboBrokerTransferBusinessType.BackColor = System.Drawing.SystemColors.Window
        Me.cboBrokerTransferBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBrokerTransferBusinessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBrokerTransferBusinessType.Enabled = False
        Me.cboBrokerTransferBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBrokerTransferBusinessType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBrokerTransferBusinessType.Location = New System.Drawing.Point(112, 34)
        Me.cboBrokerTransferBusinessType.Name = "cboBrokerTransferBusinessType"
        Me.cboBrokerTransferBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBrokerTransferBusinessType.Size = New System.Drawing.Size(159, 21)
        Me.cboBrokerTransferBusinessType.TabIndex = 54
        '
        'txtBrokerTransferToCode
        '
        Me.txtBrokerTransferToCode.AcceptsReturn = True
        Me.txtBrokerTransferToCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerTransferToCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerTransferToCode.Enabled = False
        Me.txtBrokerTransferToCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerTransferToCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerTransferToCode.Location = New System.Drawing.Point(112, 57)
        Me.txtBrokerTransferToCode.MaxLength = 0
        Me.txtBrokerTransferToCode.Name = "txtBrokerTransferToCode"
        Me.txtBrokerTransferToCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerTransferToCode.Size = New System.Drawing.Size(158, 20)
        Me.txtBrokerTransferToCode.TabIndex = 56
        '
        'cmdBrokerTransferTo
        '
        Me.cmdBrokerTransferTo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrokerTransferTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrokerTransferTo.Enabled = False
        Me.cmdBrokerTransferTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrokerTransferTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrokerTransferTo.Location = New System.Drawing.Point(24, 58)
        Me.cmdBrokerTransferTo.Name = "cmdBrokerTransferTo"
        Me.cmdBrokerTransferTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrokerTransferTo.Size = New System.Drawing.Size(60, 20)
        Me.cmdBrokerTransferTo.TabIndex = 55
        Me.cmdBrokerTransferTo.Text = "Code..."
        Me.cmdBrokerTransferTo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrokerTransferTo.UseVisualStyleBackColor = False
        '
        'chkBrokerInTransferMode
        '
        Me.chkBrokerInTransferMode.BackColor = System.Drawing.SystemColors.Control
        Me.chkBrokerInTransferMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkBrokerInTransferMode.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBrokerInTransferMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBrokerInTransferMode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBrokerInTransferMode.Location = New System.Drawing.Point(8, 15)
        Me.chkBrokerInTransferMode.Name = "chkBrokerInTransferMode"
        Me.chkBrokerInTransferMode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBrokerInTransferMode.Size = New System.Drawing.Size(163, 16)
        Me.chkBrokerInTransferMode.TabIndex = 53
        Me.chkBrokerInTransferMode.Text = "Broker in transfer mode"
        Me.chkBrokerInTransferMode.UseVisualStyleBackColor = False
        '
        'lblBrokerTransferBusinessType
        '
        Me.lblBrokerTransferBusinessType.AutoSize = True
        Me.lblBrokerTransferBusinessType.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerTransferBusinessType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerTransferBusinessType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerTransferBusinessType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerTransferBusinessType.Location = New System.Drawing.Point(11, 38)
        Me.lblBrokerTransferBusinessType.Name = "lblBrokerTransferBusinessType"
        Me.lblBrokerTransferBusinessType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerTransferBusinessType.Size = New System.Drawing.Size(79, 13)
        Me.lblBrokerTransferBusinessType.TabIndex = 114
        Me.lblBrokerTransferBusinessType.Text = "Business Type:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtFirstName)
        Me.Frame1.Controls.Add(Me.txtContactPerson)
        Me.Frame1.Controls.Add(Me.ddTitle)
        Me.Frame1.Controls.Add(Me.lblFirstName)
        Me.Frame1.Controls.Add(Me.lblContactPerson)
        Me.Frame1.Controls.Add(Me.lblTitle)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(312, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(289, 85)
        Me.Frame1.TabIndex = 115
        Me.Frame1.TabStop = False
        '
        'txtFirstName
        '
        Me.txtFirstName.AcceptsReturn = True
        Me.txtFirstName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstName.Location = New System.Drawing.Point(128, 37)
        Me.txtFirstName.MaxLength = 0
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstName.Size = New System.Drawing.Size(145, 20)
        Me.txtFirstName.TabIndex = 44
        '
        'txtContactPerson
        '
        Me.txtContactPerson.AcceptsReturn = True
        Me.txtContactPerson.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactPerson.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactPerson.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactPerson.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactPerson.Location = New System.Drawing.Point(128, 60)
        Me.txtContactPerson.MaxLength = 0
        Me.txtContactPerson.Name = "txtContactPerson"
        Me.txtContactPerson.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContactPerson.Size = New System.Drawing.Size(145, 20)
        Me.txtContactPerson.TabIndex = 45
        '
        'ddTitle
        '
        Me.ddTitle.AllowAbiCodeEntry = False
        Me.ddTitle.AutoCompleteText = False
        Me.ddTitle.DataModel = "GIIM"
        Me.ddTitle.ListIndex = -1
        Me.ddTitle.ListManager = Nothing
        Me.ddTitle.Location = New System.Drawing.Point(128, 14)
        Me.ddTitle.Login = False
        Me.ddTitle.LongList = False
        Me.ddTitle.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddTitle.Name = "ddTitle"
        Me.ddTitle.PropertyId = "131085"
        Me.ddTitle.ReadOnly_Renamed = False
        Me.ddTitle.SelLength = 0
        Me.ddTitle.SelStart = 0
        Me.ddTitle.SelText = ""
        Me.ddTitle.Size = New System.Drawing.Size(145, 21)
        Me.ddTitle.TabIndex = 43
        Me.ddTitle.ToolTipText = ""
        Me.ddTitle.VehicleListId = ""
        Me.ddTitle.VehicleMake = ""
        '
        'lblFirstName
        '
        Me.lblFirstName.AutoSize = True
        Me.lblFirstName.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstName.Location = New System.Drawing.Point(8, 40)
        Me.lblFirstName.Name = "lblFirstName"
        Me.lblFirstName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstName.Size = New System.Drawing.Size(60, 13)
        Me.lblFirstName.TabIndex = 118
        Me.lblFirstName.Text = "First Name:"
        '
        'lblContactPerson
        '
        Me.lblContactPerson.AutoSize = True
        Me.lblContactPerson.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactPerson.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactPerson.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactPerson.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactPerson.Location = New System.Drawing.Point(8, 63)
        Me.lblContactPerson.Name = "lblContactPerson"
        Me.lblContactPerson.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactPerson.Size = New System.Drawing.Size(83, 13)
        Me.lblContactPerson.TabIndex = 117
        Me.lblContactPerson.Text = "Contact Person:"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(8, 18)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(30, 13)
        Me.lblTitle.TabIndex = 116
        Me.lblTitle.Text = "Title:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cboRenewalStopCode)
        Me.Frame2.Controls.Add(Me.txtDateCancelled)
        Me.Frame2.Controls.Add(Me.cboMultipac)
        Me.Frame2.Controls.Add(Me.lblMultipac)
        Me.Frame2.Controls.Add(Me.lblRenewalStopCode)
        Me.Frame2.Controls.Add(Me.lblDateCancelled)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(312, 91)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(289, 87)
        Me.Frame2.TabIndex = 119
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Cancellation Details"
        '
        'cboRenewalStopCode
        '
        Me.cboRenewalStopCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalStopCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalStopCode.Location = New System.Drawing.Point(128, 60)
        Me.cboRenewalStopCode.Name = "cboRenewalStopCode"
        Me.cboRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalStopCode.Size = New System.Drawing.Size(145, 21)
        Me.cboRenewalStopCode.TabIndex = 50
        '
        'txtDateCancelled
        '
        Me.txtDateCancelled.AcceptsReturn = True
        Me.txtDateCancelled.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateCancelled.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateCancelled.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateCancelled.Location = New System.Drawing.Point(128, 14)
        Me.txtDateCancelled.MaxLength = 0
        Me.txtDateCancelled.Name = "txtDateCancelled"
        Me.txtDateCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateCancelled.Size = New System.Drawing.Size(145, 20)
        Me.txtDateCancelled.TabIndex = 48
        '
        'cboMultipac
        '
        Me.cboMultipac.BackColor = System.Drawing.SystemColors.Window
        Me.cboMultipac.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMultipac.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMultipac.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMultipac.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMultipac.Location = New System.Drawing.Point(128, 35)
        Me.cboMultipac.Name = "cboMultipac"
        Me.cboMultipac.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMultipac.Size = New System.Drawing.Size(57, 21)
        Me.cboMultipac.TabIndex = 49
        '
        'lblMultipac
        '
        Me.lblMultipac.AutoSize = True
        Me.lblMultipac.BackColor = System.Drawing.SystemColors.Control
        Me.lblMultipac.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMultipac.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMultipac.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMultipac.Location = New System.Drawing.Point(8, 39)
        Me.lblMultipac.Name = "lblMultipac"
        Me.lblMultipac.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMultipac.Size = New System.Drawing.Size(53, 13)
        Me.lblMultipac.TabIndex = 122
        Me.lblMultipac.Text = "Multipac?"
        '
        'lblRenewalStopCode
        '
        Me.lblRenewalStopCode.AutoSize = True
        Me.lblRenewalStopCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalStopCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStopCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStopCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStopCode.Location = New System.Drawing.Point(8, 64)
        Me.lblRenewalStopCode.Name = "lblRenewalStopCode"
        Me.lblRenewalStopCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStopCode.Size = New System.Drawing.Size(102, 13)
        Me.lblRenewalStopCode.TabIndex = 121
        Me.lblRenewalStopCode.Text = "Renewal stop code:"
        '
        'lblDateCancelled
        '
        Me.lblDateCancelled.AutoSize = True
        Me.lblDateCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateCancelled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateCancelled.Location = New System.Drawing.Point(8, 17)
        Me.lblDateCancelled.Name = "lblDateCancelled"
        Me.lblDateCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateCancelled.Size = New System.Drawing.Size(83, 13)
        Me.lblDateCancelled.TabIndex = 120
        Me.lblDateCancelled.Text = "Date Cancelled:"
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage6.Controls.Add(Me.lblAgentStatus)
        Me._tabMainTab_TabPage6.Controls.Add(Me.lblRegistrationNumber)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraRiskGroups)
        Me._tabMainTab_TabPage6.Controls.Add(Me.cboAgentStatus)
        Me._tabMainTab_TabPage6.Controls.Add(Me.txtRegistrationNumber)
        Me._tabMainTab_TabPage6.Controls.Add(Me._cmdNext_6)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "5 - FSA Compliance"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(19, 300)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_5.TabIndex = 131
        Me._cmdPrevious_5.Text = "<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        'lblAgentStatus
        '
        Me.lblAgentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentStatus.Location = New System.Drawing.Point(296, 254)
        Me.lblAgentStatus.Name = "lblAgentStatus"
        Me.lblAgentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentStatus.Size = New System.Drawing.Size(41, 17)
        Me.lblAgentStatus.TabIndex = 105
        Me.lblAgentStatus.Text = "Status"
        Me.lblAgentStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblRegistrationNumber
        '
        Me.lblRegistrationNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegistrationNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegistrationNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegistrationNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegistrationNumber.Location = New System.Drawing.Point(216, 277)
        Me.lblRegistrationNumber.Name = "lblRegistrationNumber"
        Me.lblRegistrationNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegistrationNumber.Size = New System.Drawing.Size(121, 17)
        Me.lblRegistrationNumber.TabIndex = 106
        Me.lblRegistrationNumber.Text = "Registration Number"
        Me.lblRegistrationNumber.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraRiskGroups
        '
        Me.fraRiskGroups.BackColor = System.Drawing.SystemColors.Control
        Me.fraRiskGroups.Controls.Add(Me.cmdAddAll)
        Me.fraRiskGroups.Controls.Add(Me.cmdRemove)
        Me.fraRiskGroups.Controls.Add(Me.cmdAdd)
        Me.fraRiskGroups.Controls.Add(Me.cmdRemoveAll)
        Me.fraRiskGroups.Controls.Add(Me.lvwUnderTraining)
        Me.fraRiskGroups.Controls.Add(Me.lvwCompetent)
        Me.fraRiskGroups.Controls.Add(Me.lblCompetant)
        Me.fraRiskGroups.Controls.Add(Me.lblUnderTraining)
        Me.fraRiskGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRiskGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRiskGroups.Location = New System.Drawing.Point(8, 4)
        Me.fraRiskGroups.Name = "fraRiskGroups"
        Me.fraRiskGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRiskGroups.Size = New System.Drawing.Size(593, 233)
        Me.fraRiskGroups.TabIndex = 103
        Me.fraRiskGroups.TabStop = False
        Me.fraRiskGroups.Text = "Risk Groups"
        '
        'cmdAddAll
        '
        Me.cmdAddAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAll.Location = New System.Drawing.Point(256, 72)
        Me.cmdAddAll.Name = "cmdAddAll"
        Me.cmdAddAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAll.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAll.TabIndex = 63
        Me.cmdAddAll.Text = "->>"
        Me.cmdAddAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAll.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(256, 168)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemove.TabIndex = 64
        Me.cmdRemove.Text = "&<--"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(256, 40)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 62
        Me.cmdAdd.Text = "--&>"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdRemoveAll
        '
        Me.cmdRemoveAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveAll.Location = New System.Drawing.Point(256, 200)
        Me.cmdRemoveAll.Name = "cmdRemoveAll"
        Me.cmdRemoveAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveAll.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemoveAll.TabIndex = 65
        Me.cmdRemoveAll.Text = "<<-"
        Me.cmdRemoveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveAll.UseVisualStyleBackColor = False
        '
        'lvwUnderTraining
        '
        Me.lvwUnderTraining.BackColor = System.Drawing.SystemColors.Window
        Me.lvwUnderTraining.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwUnderTraining_ColumnHeader_1})
        Me.lvwUnderTraining.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwUnderTraining.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwUnderTraining.HideSelection = False
        Me.lvwUnderTraining.Location = New System.Drawing.Point(8, 40)
        Me.lvwUnderTraining.Name = "lvwUnderTraining"
        Me.lvwUnderTraining.Size = New System.Drawing.Size(241, 185)
        Me.lvwUnderTraining.TabIndex = 61
        Me.lvwUnderTraining.UseCompatibleStateImageBehavior = False
        Me.lvwUnderTraining.View = System.Windows.Forms.View.Details
        '
        '_lvwUnderTraining_ColumnHeader_1
        '
        Me._lvwUnderTraining_ColumnHeader_1.Tag = ""
        Me._lvwUnderTraining_ColumnHeader_1.Text = ""
        Me._lvwUnderTraining_ColumnHeader_1.Width = 97
        '
        'lvwCompetent
        '
        Me.lvwCompetent.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCompetent.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCompetent_ColumnHeader_1})
        Me.lvwCompetent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCompetent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCompetent.HideSelection = False
        Me.lvwCompetent.Location = New System.Drawing.Point(336, 40)
        Me.lvwCompetent.Name = "lvwCompetent"
        Me.lvwCompetent.Size = New System.Drawing.Size(241, 185)
        Me.lvwCompetent.TabIndex = 66
        Me.lvwCompetent.UseCompatibleStateImageBehavior = False
        Me.lvwCompetent.View = System.Windows.Forms.View.Details
        '
        '_lvwCompetent_ColumnHeader_1
        '
        Me._lvwCompetent_ColumnHeader_1.Tag = ""
        Me._lvwCompetent_ColumnHeader_1.Text = ""
        Me._lvwCompetent_ColumnHeader_1.Width = 97
        '
        'lblCompetant
        '
        Me.lblCompetant.BackColor = System.Drawing.SystemColors.Control
        Me.lblCompetant.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompetant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompetant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompetant.Location = New System.Drawing.Point(336, 24)
        Me.lblCompetant.Name = "lblCompetant"
        Me.lblCompetant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompetant.Size = New System.Drawing.Size(153, 17)
        Me.lblCompetant.TabIndex = 108
        Me.lblCompetant.Text = "Competent"
        '
        'lblUnderTraining
        '
        Me.lblUnderTraining.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnderTraining.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnderTraining.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnderTraining.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnderTraining.Location = New System.Drawing.Point(8, 24)
        Me.lblUnderTraining.Name = "lblUnderTraining"
        Me.lblUnderTraining.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnderTraining.Size = New System.Drawing.Size(153, 17)
        Me.lblUnderTraining.TabIndex = 104
        Me.lblUnderTraining.Text = "Under Training"
        '
        'cboAgentStatus
        '
        Me.cboAgentStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgentStatus.Location = New System.Drawing.Point(352, 252)
        Me.cboAgentStatus.Name = "cboAgentStatus"
        Me.cboAgentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentStatus.Size = New System.Drawing.Size(249, 21)
        Me.cboAgentStatus.TabIndex = 67
        Me.cboAgentStatus.Tag = "1441846"
        '
        'txtRegistrationNumber
        '
        Me.txtRegistrationNumber.AcceptsReturn = True
        Me.txtRegistrationNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtRegistrationNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRegistrationNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegistrationNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRegistrationNumber.Location = New System.Drawing.Point(352, 276)
        Me.txtRegistrationNumber.MaxLength = 0
        Me.txtRegistrationNumber.Name = "txtRegistrationNumber"
        Me.txtRegistrationNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRegistrationNumber.Size = New System.Drawing.Size(249, 20)
        Me.txtRegistrationNumber.TabIndex = 68
        '
        '_cmdNext_6
        '
        Me._cmdNext_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_6.Location = New System.Drawing.Point(560, 300)
        Me._cmdNext_6.Name = "_cmdNext_6"
        Me._cmdNext_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_6.TabIndex = 130
        Me._cmdNext_6.Text = ">>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me.Label7)
        Me._tabMainTab_TabPage7.Controls.Add(Me.uctPartyTax1)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdPrevious_6)
        Me._tabMainTab_TabPage7.Controls.Add(Me._cmdNext_7)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(692, 454)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "7 - Tax"
        Me._tabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(30, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 14)
        Me.Label7.TabIndex = 112
        Me.Label7.Text = "Party Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(13, 10)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(593, 137)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 70
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0R
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(8, 300)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_6.TabIndex = 71
        Me._cmdPrevious_6.Text = "<<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
        '
        '_cmdNext_7
        '
        Me._cmdNext_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_7.Location = New System.Drawing.Point(560, 300)
        Me._cmdNext_7.Name = "_cmdNext_7"
        Me._cmdNext_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_7.TabIndex = 111
        Me._cmdNext_7.Text = ">>"
        Me._cmdNext_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_7.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me._cmdPrevious_7)
        Me._tabMainTab_TabPage8.Controls.Add(Me._cmdNext_8)
        Me._tabMainTab_TabPage8.Controls.Add(Me.uctPickListBranches)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "8 - Branches"
        Me._tabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_7
        '
        Me._cmdPrevious_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_7.Location = New System.Drawing.Point(14, 311)
        Me._cmdPrevious_7.Name = "_cmdPrevious_7"
        Me._cmdPrevious_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_7.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_7.TabIndex = 163
        Me._cmdPrevious_7.Text = "<<"
        Me._cmdPrevious_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_7.UseVisualStyleBackColor = False
        '
        '_cmdNext_8
        '
        Me._cmdNext_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_8.Location = New System.Drawing.Point(586, 311)
        Me._cmdNext_8.Name = "_cmdNext_8"
        Me._cmdNext_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_8.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_8.TabIndex = 164
        Me._cmdNext_8.Text = ">>"
        Me._cmdNext_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_8.UseVisualStyleBackColor = False
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Restrict to Branches"
        Me.uctPickListBranches.BusinessObject = "bSIRPartyAG.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(32, 20)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "Source"
        Me.uctPickListBranches.Size = New System.Drawing.Size(553, 273)
        Me.uctPickListBranches.TabIndex = 109
        '
        '_tabMainTab_TabPage9
        '
        Me._tabMainTab_TabPage9.Controls.Add(Me._cmdNext_9)
        Me._tabMainTab_TabPage9.Controls.Add(Me._cmdPrevious_8)
        Me._tabMainTab_TabPage9.Controls.Add(Me.uctPartyBankControl1)
        Me._tabMainTab_TabPage9.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage9.Name = "_tabMainTab_TabPage9"
        Me._tabMainTab_TabPage9.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage9.TabIndex = 9
        Me._tabMainTab_TabPage9.Text = "9 - Bank"
        Me._tabMainTab_TabPage9.UseVisualStyleBackColor = True
        '
        '_cmdNext_9
        '
        Me._cmdNext_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_9.Location = New System.Drawing.Point(610, 348)
        Me._cmdNext_9.Name = "_cmdNext_9"
        Me._cmdNext_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_9.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_9.TabIndex = 169
        Me._cmdNext_9.Text = ">>"
        Me._cmdNext_9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_9.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_8
        '
        Me._cmdPrevious_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_8.Location = New System.Drawing.Point(14, 348)
        Me._cmdPrevious_8.Name = "_cmdPrevious_8"
        Me._cmdPrevious_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_8.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_8.TabIndex = 167
        Me._cmdPrevious_8.Text = "<<"
        Me._cmdPrevious_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_8.UseVisualStyleBackColor = False
        '
        'uctPartyBankControl1
        '
        Me.uctPartyBankControl1.AccountId = Nothing
        Me.uctPartyBankControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl1.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl1.Location = New System.Drawing.Point(5, 4)
        Me.uctPartyBankControl1.Name = "uctPartyBankControl1"
        Me.uctPartyBankControl1.PartyBankDetails = Nothing
        Me.uctPartyBankControl1.PartyBankHistory = Nothing
        Me.uctPartyBankControl1.PartyCnt = Nothing
        Me.uctPartyBankControl1.ResetPreviousOne = False
        Me.uctPartyBankControl1.Size = New System.Drawing.Size(654, 343)
        Me.uctPartyBankControl1.TabIndex = 168
        '
        '_tabMainTab_TabPage10
        '
        Me._tabMainTab_TabPage10.Controls.Add(Me._cmdNext_10)
        Me._tabMainTab_TabPage10.Controls.Add(Me._cmdPrevious_9)
        Me._tabMainTab_TabPage10.Controls.Add(Me.uctPickListProducts)
        Me._tabMainTab_TabPage10.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage10.Name = "_tabMainTab_TabPage10"
        Me._tabMainTab_TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage10.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage10.TabIndex = 10
        Me._tabMainTab_TabPage10.Text = "10-Products"
        Me._tabMainTab_TabPage10.UseVisualStyleBackColor = True
        '
        '_cmdNext_10
        '
        Me._cmdNext_10.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_10.Location = New System.Drawing.Point(616, 323)
        Me._cmdNext_10.Name = "_cmdNext_10"
        Me._cmdNext_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_10.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_10.TabIndex = 171
        Me._cmdNext_10.Text = ">>"
        Me._cmdNext_10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_10.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_9
        '
        Me._cmdPrevious_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_9.Location = New System.Drawing.Point(20, 323)
        Me._cmdPrevious_9.Name = "_cmdPrevious_9"
        Me._cmdPrevious_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_9.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_9.TabIndex = 170
        Me._cmdPrevious_9.Text = "<<"
        Me._cmdPrevious_9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_9.UseVisualStyleBackColor = False
        '
        'uctPickListProducts
        '
        Me.uctPickListProducts.AvailableCaption = "Products Available"
        Me.uctPickListProducts.BusinessObject = "bSIRPartyAG.Business"
        Me.uctPickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListProducts.ForeignKeys = CType(resources.GetObject("uctPickListProducts.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListProducts.IsSearchable = False
        Me.uctPickListProducts.Location = New System.Drawing.Point(32, 20)
        Me.uctPickListProducts.Name = "uctPickListProducts"
        Me.uctPickListProducts.PickListType = "PRODUCT"
        Me.uctPickListProducts.Size = New System.Drawing.Size(553, 273)
        Me.uctPickListProducts.TabIndex = 109
        '
        '_tabMainTab_TabPage11
        '
        Me._tabMainTab_TabPage11.Controls.Add(Me._cmdNext_11)
        Me._tabMainTab_TabPage11.Controls.Add(Me._cmdPrevious_10)
        Me._tabMainTab_TabPage11.Controls.Add(Me.fraUsers)
        Me._tabMainTab_TabPage11.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage11.Name = "_tabMainTab_TabPage11"
        Me._tabMainTab_TabPage11.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage11.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage11.TabIndex = 11
        Me._tabMainTab_TabPage11.Text = "11- Users"
        Me._tabMainTab_TabPage11.UseVisualStyleBackColor = True
        '
        '_cmdNext_11
        '
        Me._cmdNext_11.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_11.Location = New System.Drawing.Point(603, 318)
        Me._cmdNext_11.Name = "_cmdNext_11"
        Me._cmdNext_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_11.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_11.TabIndex = 172
        Me._cmdNext_11.Text = ">>"
        Me._cmdNext_11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_11.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_10
        '
        Me._cmdPrevious_10.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_10.Location = New System.Drawing.Point(16, 318)
        Me._cmdPrevious_10.Name = "_cmdPrevious_10"
        Me._cmdPrevious_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_10.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_10.TabIndex = 171
        Me._cmdPrevious_10.Text = "<<"
        Me._cmdPrevious_10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_10.UseVisualStyleBackColor = False
        '
        'fraUsers
        '
        Me.fraUsers.BackColor = System.Drawing.SystemColors.Control
        Me.fraUsers.Controls.Add(Me.lvwAgencyUsers)
        Me.fraUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUsers.Location = New System.Drawing.Point(8, 4)
        Me.fraUsers.Name = "fraUsers"
        Me.fraUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUsers.Size = New System.Drawing.Size(228, 269)
        Me.fraUsers.TabIndex = 103
        Me.fraUsers.TabStop = False
        Me.fraUsers.Text = "Allocated Users"
        '
        'lvwAgencyUsers
        '
        Me.lvwAgencyUsers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAgencyUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {_lvwAgencyUsers_ColumnHeader_1})
        Me.lvwAgencyUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAgencyUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAgencyUsers.HideSelection = False
        Me.lvwAgencyUsers.Location = New System.Drawing.Point(8, 40)
        Me.lvwAgencyUsers.Name = "lvwAgencyUsers"
        Me.lvwAgencyUsers.Size = New System.Drawing.Size(197, 206)
        Me.lvwAgencyUsers.TabIndex = 61
        Me.lvwAgencyUsers.UseCompatibleStateImageBehavior = False
        Me.lvwAgencyUsers.View = System.Windows.Forms.View.Details
        '
        '_tabMainTab_TabPage12
        '
        Me._tabMainTab_TabPage12.Controls.Add(Me._cmdPrevious_11)
        Me._tabMainTab_TabPage12.Controls.Add(Me.fraCertYears)
        Me._tabMainTab_TabPage12.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage12.Name = "_tabMainTab_TabPage12"
        Me._tabMainTab_TabPage12.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage12.Size = New System.Drawing.Size(692, 436)
        Me._tabMainTab_TabPage12.TabIndex = 12
        Me._tabMainTab_TabPage12.Text = "12-Certificate Years"
        Me._tabMainTab_TabPage12.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_11
        '
        Me._cmdPrevious_11.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_11.Location = New System.Drawing.Point(14, 331)
        Me._cmdPrevious_11.Name = "_cmdPrevious_11"
        Me._cmdPrevious_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_11.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_11.TabIndex = 172
        Me._cmdPrevious_11.Text = "<<"
        Me._cmdPrevious_11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_11.UseVisualStyleBackColor = False
        '
        'fraCertYears
        '
        Me.fraCertYears.BackColor = System.Drawing.SystemColors.Control
        Me.fraCertYears.Controls.Add(Me.cmdAddCertYear)
        Me.fraCertYears.Controls.Add(Me.cmdDelCertYear)
        Me.fraCertYears.Controls.Add(Me.cmdEditCertYear)
        Me.fraCertYears.Controls.Add(Me.lvwCertYears)
        Me.fraCertYears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCertYears.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCertYears.Location = New System.Drawing.Point(6, 6)
        Me.fraCertYears.Name = "fraCertYears"
        Me.fraCertYears.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCertYears.Size = New System.Drawing.Size(652, 300)
        Me.fraCertYears.TabIndex = 100
        Me.fraCertYears.TabStop = False
        '
        'cmdAddCertYear
        '
        Me.cmdAddCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCertYear.Location = New System.Drawing.Point(8, 248)
        Me.cmdAddCertYear.Name = "cmdAddCertYear"
        Me.cmdAddCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCertYear.TabIndex = 28
        Me.cmdAddCertYear.Text = "&Add"
        Me.cmdAddCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCertYear.UseVisualStyleBackColor = False
        '
        'cmdDelCertYear
        '
        Me.cmdDelCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelCertYear.Enabled = False
        Me.cmdDelCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelCertYear.Location = New System.Drawing.Point(88, 248)
        Me.cmdDelCertYear.Name = "cmdDelCertYear"
        Me.cmdDelCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelCertYear.TabIndex = 29
        Me.cmdDelCertYear.Text = "&Delete"
        Me.cmdDelCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelCertYear.UseVisualStyleBackColor = False
        '
        'cmdEditCertYear
        '
        Me.cmdEditCertYear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCertYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCertYear.Enabled = False
        Me.cmdEditCertYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCertYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCertYear.Location = New System.Drawing.Point(168, 248)
        Me.cmdEditCertYear.Name = "cmdEditCertYear"
        Me.cmdEditCertYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCertYear.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCertYear.TabIndex = 30
        Me.cmdEditCertYear.Text = "&Edit"
        Me.cmdEditCertYear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCertYear.UseVisualStyleBackColor = False
        '
        'lvwCertYears
        '
        Me.lvwCertYears.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCertYears.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCertYears_ColumnHeader_1, Me._lvwCertYears_ColumnHeader_2, Me._lvwCertYears_ColumnHeader_3, Me._lvwCertYears_ColumnHeader_4, Me._lvwCertYears_ColumnHeader_5})
        Me.lvwCertYears.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCertYears.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCertYears.LargeImageList = Me.ImageList2
        Me.lvwCertYears.Location = New System.Drawing.Point(6, 19)
        Me.lvwCertYears.Name = "lvwCertYears"
        Me.lvwCertYears.Size = New System.Drawing.Size(621, 225)
        Me.lvwCertYears.TabIndex = 27
        Me.lvwCertYears.UseCompatibleStateImageBehavior = False
        Me.lvwCertYears.View = System.Windows.Forms.View.Details
        '
        '_lvwCertYears_ColumnHeader_1
        '
        Me._lvwCertYears_ColumnHeader_1.Tag = ""
        Me._lvwCertYears_ColumnHeader_1.Text = "Code"
        Me._lvwCertYears_ColumnHeader_1.Width = 97
        '
        '_lvwCertYears_ColumnHeader_2
        '
        Me._lvwCertYears_ColumnHeader_2.Tag = ""
        Me._lvwCertYears_ColumnHeader_2.Text = "Description"
        Me._lvwCertYears_ColumnHeader_2.Width = 97
        '
        '_lvwCertYears_ColumnHeader_3
        '
        Me._lvwCertYears_ColumnHeader_3.Tag = ""
        Me._lvwCertYears_ColumnHeader_3.Text = "Start Date"
        Me._lvwCertYears_ColumnHeader_3.Width = 97
        '
        '_lvwCertYears_ColumnHeader_4
        '
        Me._lvwCertYears_ColumnHeader_4.Tag = ""
        Me._lvwCertYears_ColumnHeader_4.Text = "End Date"
        Me._lvwCertYears_ColumnHeader_4.Width = 97
        '
        '_lvwCertYears_ColumnHeader_5
        '
        Me._lvwCertYears_ColumnHeader_5.Tag = ""
        Me._lvwCertYears_ColumnHeader_5.Text = ""
        Me._lvwCertYears_ColumnHeader_5.Width = 0
        '
        'lblAgencyUsers
        '
        Me.lblAgencyUsers.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgencyUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgencyUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgencyUsers.Location = New System.Drawing.Point(336, 24)
        Me.lblAgencyUsers.Name = "lblAgencyUsers"
        Me.lblAgencyUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgencyUsers.Size = New System.Drawing.Size(153, 17)
        Me.lblAgencyUsers.TabIndex = 108
        Me.lblAgencyUsers.Text = "UserName"
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 40)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(692, 373)
        Me.TabPage1.TabIndex = 10
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lblReceiversClientCorr
        '
        Me.lblReceiversClientCorr.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiversClientCorr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiversClientCorr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiversClientCorr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiversClientCorr.Location = New System.Drawing.Point(6, 310)
        Me.lblReceiversClientCorr.Name = "lblReceiversClientCorr"
        Me.lblReceiversClientCorr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiversClientCorr.Size = New System.Drawing.Size(161, 17)
        Me.lblReceiversClientCorr.TabIndex = 138
        Me.lblReceiversClientCorr.Text = "Receives Client Correspondence:"
        'lblOverrideCommissionRen
        '
        Me.lblOverrideCommissionRen.BackColor = System.Drawing.SystemColors.Control
        Me.lblOverrideCommissionRen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOverrideCommissionRen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverrideCommissionRen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOverrideCommissionRen.Location = New System.Drawing.Point(8, 182)
        Me.lblOverrideCommissionRen.Name = "lblOverrideCommissionRen"
        Me.lblOverrideCommissionRen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOverrideCommissionRen.Size = New System.Drawing.Size(230, 17)
        Me.lblOverrideCommissionRen.TabIndex = 170
        Me.lblOverrideCommissionRen.Text = "Use Override Commission Rate at Renewal:"
        'chkOverrideCommissionRen
        '
        Me.chkOverrideCommissionRen.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideCommissionRen.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideCommissionRen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideCommissionRen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideCommissionRen.Location = New System.Drawing.Point(272, 181)
        Me.chkOverrideCommissionRen.Name = "chkOverrideCommissionRen"
        Me.chkOverrideCommissionRen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideCommissionRen.Size = New System.Drawing.Size(17, 17)
        Me.chkOverrideCommissionRen.TabIndex = 171
        Me.chkOverrideCommissionRen.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(724, 585)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 182)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Agent"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraAppointment.ResumeLayout(False)
        Me.fraAppointment.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me.fraPremiumSettlement.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me.fraContact.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraCommissionRelease.ResumeLayout(False)
        Me.fraCommissionRelease.PerformLayout()
        Me.fraAccountLimits.ResumeLayout(False)
        Me.fraAccounts.ResumeLayout(False)
        Me.fraAccounts.PerformLayout()
        Me.Frame4.ResumeLayout(False)
        Me.fraMakeLive.ResumeLayout(False)
        Me.fraPaymentTaxation.ResumeLayout(False)
        Me.fraPaymentTaxation.PerformLayout()
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraDocuments.ResumeLayout(False)
        Me.fraDocuments.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraCommission.ResumeLayout(False)
        Me.fraConsultant.ResumeLayout(False)
        Me.fraConsultant.PerformLayout()
        Me.pnlConsultantName.ResumeLayout(False)
        Me.pnlConsultantName.PerformLayout()
        Me.fraAgentGroup.ResumeLayout(False)
        Me.fraAgentGroup.PerformLayout()
        Me.pnlAgentGroupName.ResumeLayout(False)
        Me.pnlAgentGroupName.PerformLayout()
        Me.fraAssociate.ResumeLayout(False)
        Me.fraAltRef.ResumeLayout(False)
        Me.fraBrokerTransfer.ResumeLayout(False)
        Me.fraBrokerTransfer.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me._tabMainTab_TabPage6.PerformLayout()
        Me.fraRiskGroups.ResumeLayout(False)
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._tabMainTab_TabPage7.PerformLayout()
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        Me._tabMainTab_TabPage9.ResumeLayout(False)
        Me._tabMainTab_TabPage10.ResumeLayout(False)
        Me._tabMainTab_TabPage11.ResumeLayout(False)
        Me.fraUsers.ResumeLayout(False)
        Me._tabMainTab_TabPage12.ResumeLayout(False)
        Me.fraCertYears.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrevious()
        ' Me.cmdPrevious(10) = _cmdPrevious_10
        'Me.cmdPrevious(9) = _cmdPrevious_9
        Me.cmdPrevious(8) = _cmdPrevious_8
        Me.cmdPrevious(7) = _cmdPrevious_7
        Me.cmdPrevious(6) = _cmdPrevious_6
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        ' Me.cmdNext(10) = _cmdNext_10
        ' Me.cmdNext(9) = _cmdNext_9
        Me.cmdNext(8) = _cmdNext_8
        Me.cmdNext(6) = _cmdNext_6
        Me.cmdNext(7) = _cmdNext_7
        Me.cmdNext(5) = _cmdNext_5
        Me.cmdNext(4) = _cmdNext_4
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Sub lvwCompetent_InitializeColumnKeys()
        Me._lvwCompetent_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwAgencyUsers_InitializeColumnKeys()
        'Me._lvwAgencyUsers_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwUnderTraining_InitializeColumnKeys()
        Me._lvwUnderTraining_ColumnHeader_1.Name = ""
    End Sub
    Sub lvwAssociates_InitializeColumnKeys()
        Me._lvwAssociates_ColumnHeader_1.Name = ""
        Me._lvwAssociates_ColumnHeader_2.Name = ""
        Me._lvwAssociates_ColumnHeader_3.Name = ""
    End Sub
    Sub lvwContact_InitializeColumnKeys()
        Me._lvwContact_ColumnHeader_1.Name = ""
        Me._lvwContact_ColumnHeader_2.Name = ""
        Me._lvwContact_ColumnHeader_3.Name = ""
        Me._lvwContact_ColumnHeader_4.Name = ""
        Me._lvwContact_ColumnHeader_5.Name = ""
    End Sub
    Sub lvwAddress_InitializeColumnKeys()
        Me._lvwAddress_ColumnHeader_1.Name = ""
        Me._lvwAddress_ColumnHeader_2.Name = ""
        Me._lvwAddress_ColumnHeader_3.Name = ""
        Me._lvwAddress_ColumnHeader_4.Name = ""
        Me._lvwAddress_ColumnHeader_5.Name = ""
        Me._lvwAddress_ColumnHeader_6.Name = ""
    End Sub
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents plblConsultantName As System.Windows.Forms.Label
    Friend WithEvents plblAgentGroupName As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents _tabMainTab_TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents _tabMainTab_TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents fraCommission As System.Windows.Forms.GroupBox
    Public WithEvents Label9 As System.Windows.Forms.Label
    Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_7 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_8 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_9 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_10 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_9 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_10 As System.Windows.Forms.Button
    Friend WithEvents _tabMainTab_TabPage12 As System.Windows.Forms.TabPage
    Public WithEvents fraCertYears As System.Windows.Forms.GroupBox
    Public WithEvents cmdAddCertYear As System.Windows.Forms.Button
    Public WithEvents cmdDelCertYear As System.Windows.Forms.Button
    Public WithEvents cmdEditCertYear As System.Windows.Forms.Button
    Public WithEvents lvwCertYears As System.Windows.Forms.ListView
    Private WithEvents _lvwCertYears_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCertYears_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _cmdNext_11 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_11 As System.Windows.Forms.Button
    Friend WithEvents _lvwCertYears_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents fraPremiumSettlement As System.Windows.Forms.GroupBox
    Friend WithEvents OptNetOfcommission As System.Windows.Forms.RadioButton
    Friend WithEvents OptGrossOfCommission As System.Windows.Forms.RadioButton
    Public WithEvents cboBankAccount As PMLookupControl.cboPMLookup
    Friend WithEvents cboCorrespondenceType As System.Windows.Forms.ComboBox
    Friend WithEvents lblPreferredCorrespondence As System.Windows.Forms.Label
    Friend WithEvents lblReceivesClientCorr As System.Windows.Forms.Label
    Friend WithEvents chkReceivesClientCorr As System.Windows.Forms.CheckBox
    Friend WithEvents lblReceiversClientCorr As System.Windows.Forms.Label
    Public WithEvents cmdMaintainCommLevel As Button
    Public WithEvents ListView1 As ListView
    Private WithEvents _lvwCommLvl_ColumnHeader1 As ColumnHeader
    Private WithEvents _lvwCommLvl_ColumnHeader2 As ColumnHeader
    Public WithEvents chkOverrideCommissionRen As CheckBox
    Public WithEvents lblOverrideCommissionRen As Label
#End Region
End Class
