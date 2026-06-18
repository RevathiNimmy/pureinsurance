<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
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
    Public WithEvents cmdCancelPolicy As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdSave As System.Windows.Forms.Button
    Public WithEvents cmdMTA As System.Windows.Forms.Button
    Public WithEvents cmdRePrint As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents txtFirstInstalmentDate As System.Windows.Forms.TextBox
    Public WithEvents txtNextInstalmentDate As System.Windows.Forms.TextBox
    Public WithEvents txtLastInstalmentDate As System.Windows.Forms.TextBox
    Public WithEvents txtFinanceCharge As System.Windows.Forms.TextBox
    Public WithEvents txtInterestRate As System.Windows.Forms.TextBox
    Public WithEvents txtTotalAmount As System.Windows.Forms.TextBox
    Public WithEvents txtTaxes As System.Windows.Forms.TextBox
    Public WithEvents txtAPR As System.Windows.Forms.TextBox
    Public WithEvents txtOtherInstalments As System.Windows.Forms.TextBox
    Public WithEvents txtFirstInstalmentValue As System.Windows.Forms.TextBox
    Public WithEvents txtDeposit As System.Windows.Forms.TextBox
    Public WithEvents txtCostOfProtection As System.Windows.Forms.TextBox
    Public WithEvents lblFirstInstalmentDate As System.Windows.Forms.Label
    Public WithEvents lblNextInstalmentDate As System.Windows.Forms.Label
    Public WithEvents lblLastInstalmentDate As System.Windows.Forms.Label
    Public WithEvents Line1 As System.Windows.Forms.Label
    Public WithEvents lblFinanceCharge As System.Windows.Forms.Label
    Public WithEvents lblInterestRate As System.Windows.Forms.Label
    Public WithEvents lblTotalAmount As System.Windows.Forms.Label
    Public WithEvents lbTaxes As System.Windows.Forms.Label
    Public WithEvents lblAPR As System.Windows.Forms.Label
    Public WithEvents lblOtherInstalments As System.Windows.Forms.Label
    Public WithEvents lblFirstInstalmentValue As System.Windows.Forms.Label
    Public WithEvents lblDeposit As System.Windows.Forms.Label
    Public WithEvents lblCostOfProtection As System.Windows.Forms.Label
    Public WithEvents fraPayment As System.Windows.Forms.GroupBox
    Public WithEvents cboStatementFrequency As System.Windows.Forms.ComboBox
    Public WithEvents dtpModifiedDate As System.Windows.Forms.DateTimePicker
    Public WithEvents dtpConfirmedDate As System.Windows.Forms.DateTimePicker
    Public WithEvents dtpReviewDate As System.Windows.Forms.DateTimePicker
    Public WithEvents dtpCreatedDate As System.Windows.Forms.DateTimePicker
    Public WithEvents chkNoStatements As System.Windows.Forms.CheckBox
    Public WithEvents lblReviewedDate As System.Windows.Forms.Label
    Public WithEvents lblConfirmedDate As System.Windows.Forms.Label
    Public WithEvents lblModifiedDate As System.Windows.Forms.Label
    Public WithEvents lblCreateDate As System.Windows.Forms.Label
    Public WithEvents lblStatementFrequency As System.Windows.Forms.Label
    Public WithEvents fraDates As System.Windows.Forms.GroupBox
    Public WithEvents cboCancelReason As PMLookupControl.cboPMLookup
    Public WithEvents txtTerms As System.Windows.Forms.TextBox
    Public WithEvents txtReference As System.Windows.Forms.TextBox
    Public WithEvents txtFinancedAmount As System.Windows.Forms.TextBox
    Public WithEvents txtOriginalDebt As System.Windows.Forms.TextBox
    Public WithEvents cboStatus As System.Windows.Forms.ComboBox
    Public WithEvents txtDaysDelay As System.Windows.Forms.TextBox
    Public WithEvents txtStartDate As System.Windows.Forms.TextBox
    Public WithEvents txtNumberOfInstalments As System.Windows.Forms.TextBox
    Public WithEvents cmdHistory As System.Windows.Forms.Button
    Public WithEvents cmdTransactions As System.Windows.Forms.Button
    Public WithEvents lblCancelReason As System.Windows.Forms.Label
    Public WithEvents lblTerms As System.Windows.Forms.Label
    Public WithEvents lblReference As System.Windows.Forms.Label
    Public WithEvents lblFinanceAmount As System.Windows.Forms.Label
    Public WithEvents lblOriginalDebt As System.Windows.Forms.Label
    Public WithEvents lblHeader As System.Windows.Forms.Label
    Public WithEvents lblDaysDelay As System.Windows.Forms.Label
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents lblNumberOfInstalments As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents fraSummary As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents addClient As PMAddressControl.uctPMAddressControl
    Public WithEvents chkPartnership As System.Windows.Forms.CheckBox
    Public WithEvents cmdDeletePartner As System.Windows.Forms.Button
    Public WithEvents cmdAddPartner As System.Windows.Forms.Button
    Private WithEvents _lvwPartners_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPartners_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPartners_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwPartners As System.Windows.Forms.ListView
    Public WithEvents frmPartners As System.Windows.Forms.GroupBox
    Public WithEvents txtClientFaxCode As System.Windows.Forms.TextBox
    Public WithEvents txtClientFaxNumber As System.Windows.Forms.TextBox
    Public WithEvents txtClientAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtClientNumber As System.Windows.Forms.TextBox
    Public WithEvents txtClientExtension As System.Windows.Forms.TextBox
    Public WithEvents lblClientFax As System.Windows.Forms.Label
    Public WithEvents lblClientTelephone As System.Windows.Forms.Label
    Public WithEvents lblClientAreaCode As System.Windows.Forms.Label
    Public WithEvents lblClientNumber As System.Windows.Forms.Label
    Public WithEvents lblClientExtension As System.Windows.Forms.Label
    Public WithEvents framPhone As System.Windows.Forms.GroupBox
    Public WithEvents txtClientName As System.Windows.Forms.TextBox
    Public WithEvents cboRefundType As System.Windows.Forms.ComboBox
    Public WithEvents cboBusinessCode As System.Windows.Forms.ComboBox
    Public WithEvents txtAuthCode As System.Windows.Forms.TextBox
    Public WithEvents txtPFReference As System.Windows.Forms.TextBox
    Public WithEvents txtDateOfBirth As System.Windows.Forms.TextBox
    Public WithEvents txtCompanyReg As System.Windows.Forms.TextBox
    Public WithEvents lblRefundType As System.Windows.Forms.Label
    Public WithEvents lblBusinessCode As System.Windows.Forms.Label
    Public WithEvents lblAuthCode As System.Windows.Forms.Label
    Public WithEvents lblPFReference As System.Windows.Forms.Label
    Public WithEvents lblDateOfBirth As System.Windows.Forms.Label
    Public WithEvents lblCompanyReg As System.Windows.Forms.Label
    Public WithEvents framAdditional As System.Windows.Forms.GroupBox
    Public WithEvents lblClientName As System.Windows.Forms.Label
    Public WithEvents fraClient As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents uctPartyBankCombo1 As uctPartyBank.uctPartyBankCombo
    Public WithEvents txtBankName As System.Windows.Forms.TextBox
    Public WithEvents txtAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtNumber As System.Windows.Forms.TextBox
    Public WithEvents txtExtension As System.Windows.Forms.TextBox
    Public WithEvents txtFaxAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtFaxNumber As System.Windows.Forms.TextBox
    Public WithEvents addBank As PMAddressControl.uctPMAddressControl
    Public WithEvents lblPaymentAccountType As System.Windows.Forms.Label
    Public WithEvents lblBankPhone As System.Windows.Forms.Label
    Public WithEvents lblBankFax As System.Windows.Forms.Label
    Public WithEvents lblBankName As System.Windows.Forms.Label
    Public WithEvents lblAreaCode As System.Windows.Forms.Label
    Public WithEvents lblNumber As System.Windows.Forms.Label
    Public WithEvents lblExtension As System.Windows.Forms.Label
    Public WithEvents fraBank As System.Windows.Forms.GroupBox
    Public WithEvents chkPaperDD As System.Windows.Forms.CheckBox
    Public WithEvents chkDDCancelled As System.Windows.Forms.CheckBox
    Public WithEvents txtDateBankDetailsChanged As System.Windows.Forms.TextBox
    Public WithEvents cboCopyFromBank As System.Windows.Forms.ComboBox
    Public WithEvents txtAccountName As System.Windows.Forms.TextBox
    Public WithEvents txtAccountNumber As System.Windows.Forms.TextBox
    Public WithEvents txtSortCode As System.Windows.Forms.TextBox
    Public WithEvents txtBranch As System.Windows.Forms.TextBox
    Public WithEvents lblCancelled As System.Windows.Forms.Label
    Public WithEvents lblDateBankDetailsChanged As System.Windows.Forms.Label
    Public WithEvents lblCopyFromBank As System.Windows.Forms.Label
    Public WithEvents lblAccountName As System.Windows.Forms.Label
    Public WithEvents lblAccountNumber As System.Windows.Forms.Label
    Public WithEvents lblSortCode As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents fraAccount As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lvwHistory As System.Windows.Forms.ListView
    Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents txtCardholderName As System.Windows.Forms.TextBox
    Public WithEvents addCardholder As PMAddressControl.uctPMAddressControl
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents txtTrackingNumber As System.Windows.Forms.TextBox
    Public WithEvents uctPartyBankCombo2 As uctPartyBank.uctPartyBankCombo
    Public WithEvents txtCardNo As System.Windows.Forms.TextBox
    Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
    Public WithEvents txtIssueNo As System.Windows.Forms.TextBox
    Public WithEvents cboCopyFromCard As System.Windows.Forms.ComboBox
    Public WithEvents chkCardholder As System.Windows.Forms.CheckBox
    Public WithEvents chkCCCancelled As System.Windows.Forms.CheckBox
    Public WithEvents cboCardType As System.Windows.Forms.ComboBox
    Public WithEvents txtPin As System.Windows.Forms.TextBox
    Public WithEvents txtCardStartDate As System.Windows.Forms.TextBox
    Public WithEvents txtCardName As System.Windows.Forms.TextBox
    Public WithEvents lblTrackingNumber As System.Windows.Forms.Label
    Public WithEvents lblReceiptAccountType As System.Windows.Forms.Label
    Public WithEvents lblCCCancelled As System.Windows.Forms.Label
    Public WithEvents lblCardType As System.Windows.Forms.Label
    Public WithEvents lblPin As System.Windows.Forms.Label
    Public WithEvents lblCardStartDate As System.Windows.Forms.Label
    Public WithEvents lblCardName As System.Windows.Forms.Label
    Public WithEvents lblCopyFromCard As System.Windows.Forms.Label
    Public WithEvents lblIssueNo As System.Windows.Forms.Label
    Public WithEvents lblExpiryDate As System.Windows.Forms.Label
    Public WithEvents lblCardNo As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTabCreditCard_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lvwCCHistory As System.Windows.Forms.ListView
    Private WithEvents _SSTabCreditCard_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents SSTabCreditCard As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _lvwInstalment_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwInstalment_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwInstalment As System.Windows.Forms.ListView
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents uctPMAgentAddressControl As PMAddressControl.uctPMAddressControl
    Public WithEvents txtAgentFaxExtension As System.Windows.Forms.TextBox
    Public WithEvents txtAgent As System.Windows.Forms.TextBox
    Public WithEvents txtAgentRef As System.Windows.Forms.TextBox
    Public WithEvents txtAgentFaxAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtAgentFaxNumber As System.Windows.Forms.TextBox
    Public WithEvents txtAgentTelAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtAgentTelNumber As System.Windows.Forms.TextBox
    Public WithEvents txtAgentTelExtension As System.Windows.Forms.TextBox
    Public WithEvents cmdAgentSelect As System.Windows.Forms.Button
    Public WithEvents lblAgentRef As System.Windows.Forms.Label
    Public WithEvents lblAgentFaxNo As System.Windows.Forms.Label
    Public WithEvents lblAgentTelNo As System.Windows.Forms.Label
    Public WithEvents lblAgentAreaCode As System.Windows.Forms.Label
    Public WithEvents lblAgentNumber As System.Windows.Forms.Label
    Public WithEvents lblAgentExtension As System.Windows.Forms.Label
    Public WithEvents fraAgentDetails As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Private WithEvents _lvwPolicyList_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyList_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyList_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyList_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyList_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwPolicyList As System.Windows.Forms.ListView
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdTransact As System.Windows.Forms.Button
    Public WithEvents cmdSettlePlan As System.Windows.Forms.Button
    Public WithEvents cmdRelease As System.Windows.Forms.Button
    Public WithEvents cmdReSend As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Private WithEvents _SSTab1_ImageList As System.Windows.Forms.ImageList
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    Public WithEvents grpInstalmentActions As System.Windows.Forms.GroupBox
    Public WithEvents txtSelectedTotal As System.Windows.Forms.TextBox
    Public WithEvents lblSelectedTotal As System.Windows.Forms.Label
    Public WithEvents btnReverse As System.Windows.Forms.Button
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancelPolicy = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdMTA = New System.Windows.Forms.Button
        Me.cmdRePrint = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdExit = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraPayment = New System.Windows.Forms.GroupBox
        Me.txtFirstInstalmentDate = New System.Windows.Forms.TextBox
        Me.txtNextInstalmentDate = New System.Windows.Forms.TextBox
        Me.txtLastInstalmentDate = New System.Windows.Forms.TextBox
        Me.txtFinanceCharge = New System.Windows.Forms.TextBox
        Me.txtInterestRate = New System.Windows.Forms.TextBox
        Me.txtTotalAmount = New System.Windows.Forms.TextBox
        Me.txtTaxes = New System.Windows.Forms.TextBox
        Me.txtAPR = New System.Windows.Forms.TextBox
        Me.txtOtherInstalments = New System.Windows.Forms.TextBox
        Me.txtFirstInstalmentValue = New System.Windows.Forms.TextBox
        Me.txtDeposit = New System.Windows.Forms.TextBox
        Me.txtCostOfProtection = New System.Windows.Forms.TextBox
        Me.lblFirstInstalmentDate = New System.Windows.Forms.Label
        Me.lblNextInstalmentDate = New System.Windows.Forms.Label
        Me.lblLastInstalmentDate = New System.Windows.Forms.Label
        Me.Line1 = New System.Windows.Forms.Label
        Me.lblFinanceCharge = New System.Windows.Forms.Label
        Me.lblInterestRate = New System.Windows.Forms.Label
        Me.lblTotalAmount = New System.Windows.Forms.Label
        Me.lbTaxes = New System.Windows.Forms.Label
        Me.lblAPR = New System.Windows.Forms.Label
        Me.lblOtherInstalments = New System.Windows.Forms.Label
        Me.lblFirstInstalmentValue = New System.Windows.Forms.Label
        Me.lblDeposit = New System.Windows.Forms.Label
        Me.lblCostOfProtection = New System.Windows.Forms.Label
        Me.fraDates = New System.Windows.Forms.GroupBox
        Me.cboStatementFrequency = New System.Windows.Forms.ComboBox
        Me.dtpModifiedDate = New System.Windows.Forms.DateTimePicker
        Me.dtpConfirmedDate = New System.Windows.Forms.DateTimePicker
        Me.dtpReviewDate = New System.Windows.Forms.DateTimePicker
        Me.dtpCreatedDate = New System.Windows.Forms.DateTimePicker
        Me.chkNoStatements = New System.Windows.Forms.CheckBox
        Me.lblReviewedDate = New System.Windows.Forms.Label
        Me.lblConfirmedDate = New System.Windows.Forms.Label
        Me.lblModifiedDate = New System.Windows.Forms.Label
        Me.lblCreateDate = New System.Windows.Forms.Label
        Me.lblStatementFrequency = New System.Windows.Forms.Label
        Me.fraSummary = New System.Windows.Forms.GroupBox
        Me.cboCancelReason = New PMLookupControl.cboPMLookup
        Me.txtTerms = New System.Windows.Forms.TextBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtFinancedAmount = New System.Windows.Forms.TextBox
        Me.txtOriginalDebt = New System.Windows.Forms.TextBox
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.txtDaysDelay = New System.Windows.Forms.TextBox
        Me.txtStartDate = New System.Windows.Forms.TextBox
        Me.txtNumberOfInstalments = New System.Windows.Forms.TextBox
        Me.cmdHistory = New System.Windows.Forms.Button
        Me.cmdTransactions = New System.Windows.Forms.Button
        Me.lblCancelReason = New System.Windows.Forms.Label
        Me.lblTerms = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.Label
        Me.lblFinanceAmount = New System.Windows.Forms.Label
        Me.lblOriginalDebt = New System.Windows.Forms.Label
        Me.lblHeader = New System.Windows.Forms.Label
        Me.lblDaysDelay = New System.Windows.Forms.Label
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.lblNumberOfInstalments = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraClient = New System.Windows.Forms.GroupBox
        Me.addClient = New PMAddressControl.uctPMAddressControl
        Me.chkPartnership = New System.Windows.Forms.CheckBox
        Me.frmPartners = New System.Windows.Forms.GroupBox
        Me.cmdDeletePartner = New System.Windows.Forms.Button
        Me.cmdAddPartner = New System.Windows.Forms.Button
        Me.lvwPartners = New System.Windows.Forms.ListView
        Me._lvwPartners_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPartners_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwPartners_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.framPhone = New System.Windows.Forms.GroupBox
        Me.txtClientFaxCode = New System.Windows.Forms.TextBox
        Me.txtClientFaxNumber = New System.Windows.Forms.TextBox
        Me.txtClientAreaCode = New System.Windows.Forms.TextBox
        Me.txtClientNumber = New System.Windows.Forms.TextBox
        Me.txtClientExtension = New System.Windows.Forms.TextBox
        Me.lblClientFax = New System.Windows.Forms.Label
        Me.lblClientTelephone = New System.Windows.Forms.Label
        Me.lblClientAreaCode = New System.Windows.Forms.Label
        Me.lblClientNumber = New System.Windows.Forms.Label
        Me.lblClientExtension = New System.Windows.Forms.Label
        Me.txtClientName = New System.Windows.Forms.TextBox
        Me.framAdditional = New System.Windows.Forms.GroupBox
        Me.cboRefundType = New System.Windows.Forms.ComboBox
        Me.cboBusinessCode = New System.Windows.Forms.ComboBox
        Me.txtAuthCode = New System.Windows.Forms.TextBox
        Me.txtPFReference = New System.Windows.Forms.TextBox
        Me.txtDateOfBirth = New System.Windows.Forms.TextBox
        Me.txtCompanyReg = New System.Windows.Forms.TextBox
        Me.lblRefundType = New System.Windows.Forms.Label
        Me.lblBusinessCode = New System.Windows.Forms.Label
        Me.lblAuthCode = New System.Windows.Forms.Label
        Me.lblPFReference = New System.Windows.Forms.Label
        Me.lblDateOfBirth = New System.Windows.Forms.Label
        Me.lblCompanyReg = New System.Windows.Forms.Label
        Me.lblClientName = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraBank = New System.Windows.Forms.GroupBox
        Me.uctPartyBankCombo1 = New uctPartyBank.uctPartyBankCombo
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtAreaCode = New System.Windows.Forms.TextBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.txtExtension = New System.Windows.Forms.TextBox
        Me.txtFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtFaxNumber = New System.Windows.Forms.TextBox
        Me.addBank = New PMAddressControl.uctPMAddressControl
        Me.lblPaymentAccountType = New System.Windows.Forms.Label
        Me.lblBankPhone = New System.Windows.Forms.Label
        Me.lblBankFax = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblAreaCode = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me.lblExtension = New System.Windows.Forms.Label
        Me.fraAccount = New System.Windows.Forms.GroupBox
        Me.txtIBAN = New System.Windows.Forms.TextBox
        Me.lblIBAN = New System.Windows.Forms.Label
        Me.txtBIC = New System.Windows.Forms.TextBox
        Me.lblBIC = New System.Windows.Forms.Label
        Me.chkPaperDD = New System.Windows.Forms.CheckBox
        Me.chkDDCancelled = New System.Windows.Forms.CheckBox
        Me.txtDateBankDetailsChanged = New System.Windows.Forms.TextBox
        Me.cboCopyFromBank = New System.Windows.Forms.ComboBox
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.txtAccountNumber = New System.Windows.Forms.TextBox
        Me.txtSortCode = New System.Windows.Forms.TextBox
        Me.txtBranch = New System.Windows.Forms.TextBox
        Me.lblCancelled = New System.Windows.Forms.Label
        Me.lblDateBankDetailsChanged = New System.Windows.Forms.Label
        Me.lblCopyFromBank = New System.Windows.Forms.Label
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblAccountNumber = New System.Windows.Forms.Label
        Me.lblSortCode = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.lvwHistory = New System.Windows.Forms.ListView
        Me._SSTab1_ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me.SSTabCreditCard = New System.Windows.Forms.TabControl
        Me._SSTabCreditCard_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.txtCardholderName = New System.Windows.Forms.TextBox
        Me.addCardholder = New PMAddressControl.uctPMAddressControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtTrackingNumber = New System.Windows.Forms.TextBox
        Me.uctPartyBankCombo2 = New uctPartyBank.uctPartyBankCombo
        Me.txtCardNo = New System.Windows.Forms.TextBox
        Me.txtExpiryDate = New System.Windows.Forms.TextBox
        Me.txtIssueNo = New System.Windows.Forms.TextBox
        Me.cboCopyFromCard = New System.Windows.Forms.ComboBox
        Me.chkCardholder = New System.Windows.Forms.CheckBox
        Me.chkCCCancelled = New System.Windows.Forms.CheckBox
        Me.cboCardType = New System.Windows.Forms.ComboBox
        Me.txtPin = New System.Windows.Forms.TextBox
        Me.txtCardStartDate = New System.Windows.Forms.TextBox
        Me.txtCardName = New System.Windows.Forms.TextBox
        Me.lblTrackingNumber = New System.Windows.Forms.Label
        Me.lblReceiptAccountType = New System.Windows.Forms.Label
        Me.lblCCCancelled = New System.Windows.Forms.Label
        Me.lblCardType = New System.Windows.Forms.Label
        Me.lblPin = New System.Windows.Forms.Label
        Me.lblCardStartDate = New System.Windows.Forms.Label
        Me.lblCardName = New System.Windows.Forms.Label
        Me.lblCopyFromCard = New System.Windows.Forms.Label
        Me.lblIssueNo = New System.Windows.Forms.Label
        Me.lblExpiryDate = New System.Windows.Forms.Label
        Me.lblCardNo = New System.Windows.Forms.Label
        Me._SSTabCreditCard_TabPage1 = New System.Windows.Forms.TabPage
        Me.lvwCCHistory = New System.Windows.Forms.ListView
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.grpInstalmentActions = New System.Windows.Forms.GroupBox
        Me.txtSelectedTotal = New System.Windows.Forms.TextBox()
        Me.btnReverseInstalment = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lvwInstalment = New System.Windows.Forms.ListView
        Me._lvwInstalment_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwInstalment_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.fraAgentDetails = New System.Windows.Forms.GroupBox
        Me.uctPMAgentAddressControl = New PMAddressControl.uctPMAddressControl
        Me.txtAgentFaxExtension = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtAgentRef = New System.Windows.Forms.TextBox
        Me.txtAgentFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtAgentFaxNumber = New System.Windows.Forms.TextBox
        Me.txtAgentTelAreaCode = New System.Windows.Forms.TextBox
        Me.txtAgentTelNumber = New System.Windows.Forms.TextBox
        Me.txtAgentTelExtension = New System.Windows.Forms.TextBox
        Me.cmdAgentSelect = New System.Windows.Forms.Button
        Me.lblAgentRef = New System.Windows.Forms.Label
        Me.lblAgentFaxNo = New System.Windows.Forms.Label
        Me.lblAgentTelNo = New System.Windows.Forms.Label
        Me.lblAgentAreaCode = New System.Windows.Forms.Label
        Me.lblAgentNumber = New System.Windows.Forms.Label
        Me.lblAgentExtension = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me.lvwPolicyList = New System.Windows.Forms.ListView
        Me._lvwPolicyList_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicyList_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicyList_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicyList_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicyList_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdTransact = New System.Windows.Forms.Button
        Me.cmdSettlePlan = New System.Windows.Forms.Button
        Me.cmdRelease = New System.Windows.Forms.Button
        Me.cmdReSend = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraPayment.SuspendLayout()
        Me.fraDates.SuspendLayout()
        Me.fraSummary.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.frmPartners.SuspendLayout()
        Me.framPhone.SuspendLayout()
        Me.framAdditional.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraBank.SuspendLayout()
        Me.fraAccount.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.SSTabCreditCard.SuspendLayout()
        Me._SSTabCreditCard_TabPage0.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._SSTabCreditCard_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.grpInstalmentActions.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraAgentDetails.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancelPolicy
        '
        Me.cmdCancelPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancelPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancelPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancelPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancelPolicy.Location = New System.Drawing.Point(336, 509)
        Me.cmdCancelPolicy.Name = "cmdCancelPolicy"
        Me.cmdCancelPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancelPolicy.Size = New System.Drawing.Size(89, 22)
        Me.cmdCancelPolicy.TabIndex = 182
        Me.cmdCancelPolicy.Text = "Cancel Policy"
        Me.cmdCancelPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancelPolicy.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(248, 509)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(81, 22)
        Me.cmdCancel.TabIndex = 101
        Me.cmdCancel.Text = "Cancel Plan"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(480, 509)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(73, 22)
        Me.cmdSave.TabIndex = 104
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdMTA
        '
        Me.cmdMTA.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMTA.Location = New System.Drawing.Point(168, 509)
        Me.cmdMTA.Name = "cmdMTA"
        Me.cmdMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMTA.Size = New System.Drawing.Size(73, 22)
        Me.cmdMTA.TabIndex = 100
        Me.cmdMTA.TabStop = False
        Me.cmdMTA.Text = "&MTA"
        Me.cmdMTA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMTA.UseVisualStyleBackColor = False
        '
        'cmdRePrint
        '
        Me.cmdRePrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRePrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRePrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRePrint.Location = New System.Drawing.Point(8, 509)
        Me.cmdRePrint.Name = "cmdRePrint"
        Me.cmdRePrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRePrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdRePrint.TabIndex = 96
        Me.cmdRePrint.Text = "Re&Print"
        Me.cmdRePrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRePrint.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.CausesValidation = False
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(560, 509)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 106
        Me.cmdExit.TabStop = False
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(89, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(686, 495)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPayment)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDates)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraSummary)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Plan Details"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraPayment
        '
        Me.fraPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayment.Controls.Add(Me.txtFirstInstalmentDate)
        Me.fraPayment.Controls.Add(Me.txtNextInstalmentDate)
        Me.fraPayment.Controls.Add(Me.txtLastInstalmentDate)
        Me.fraPayment.Controls.Add(Me.txtFinanceCharge)
        Me.fraPayment.Controls.Add(Me.txtInterestRate)
        Me.fraPayment.Controls.Add(Me.txtTotalAmount)
        Me.fraPayment.Controls.Add(Me.txtTaxes)
        Me.fraPayment.Controls.Add(Me.txtAPR)
        Me.fraPayment.Controls.Add(Me.txtOtherInstalments)
        Me.fraPayment.Controls.Add(Me.txtFirstInstalmentValue)
        Me.fraPayment.Controls.Add(Me.txtDeposit)
        Me.fraPayment.Controls.Add(Me.txtCostOfProtection)
        Me.fraPayment.Controls.Add(Me.lblFirstInstalmentDate)
        Me.fraPayment.Controls.Add(Me.lblNextInstalmentDate)
        Me.fraPayment.Controls.Add(Me.lblLastInstalmentDate)
        Me.fraPayment.Controls.Add(Me.Line1)
        Me.fraPayment.Controls.Add(Me.lblFinanceCharge)
        Me.fraPayment.Controls.Add(Me.lblInterestRate)
        Me.fraPayment.Controls.Add(Me.lblTotalAmount)
        Me.fraPayment.Controls.Add(Me.lbTaxes)
        Me.fraPayment.Controls.Add(Me.lblAPR)
        Me.fraPayment.Controls.Add(Me.lblOtherInstalments)
        Me.fraPayment.Controls.Add(Me.lblFirstInstalmentValue)
        Me.fraPayment.Controls.Add(Me.lblDeposit)
        Me.fraPayment.Controls.Add(Me.lblCostOfProtection)
        Me.fraPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayment.Location = New System.Drawing.Point(8, 224)
        Me.fraPayment.Name = "fraPayment"
        Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayment.Size = New System.Drawing.Size(667, 197)
        Me.fraPayment.TabIndex = 71
        Me.fraPayment.TabStop = False
        Me.fraPayment.Tag = "CHILD;"
        Me.fraPayment.Text = "Breakdown"
        '
        'txtFirstInstalmentDate
        '
        Me.txtFirstInstalmentDate.AcceptsReturn = True
        Me.txtFirstInstalmentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalmentDate.Enabled = False
        Me.txtFirstInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalmentDate.Location = New System.Drawing.Point(160, 112)
        Me.txtFirstInstalmentDate.MaxLength = 0
        Me.txtFirstInstalmentDate.Name = "txtFirstInstalmentDate"
        Me.txtFirstInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalmentDate.Size = New System.Drawing.Size(145, 20)
        Me.txtFirstInstalmentDate.TabIndex = 85
        Me.txtFirstInstalmentDate.TabStop = False
        Me.txtFirstInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNextInstalmentDate
        '
        Me.txtNextInstalmentDate.AcceptsReturn = True
        Me.txtNextInstalmentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtNextInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextInstalmentDate.Enabled = False
        Me.txtNextInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNextInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNextInstalmentDate.Location = New System.Drawing.Point(160, 136)
        Me.txtNextInstalmentDate.MaxLength = 0
        Me.txtNextInstalmentDate.Name = "txtNextInstalmentDate"
        Me.txtNextInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNextInstalmentDate.Size = New System.Drawing.Size(145, 20)
        Me.txtNextInstalmentDate.TabIndex = 89
        Me.txtNextInstalmentDate.TabStop = False
        Me.txtNextInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLastInstalmentDate
        '
        Me.txtLastInstalmentDate.AcceptsReturn = True
        Me.txtLastInstalmentDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtLastInstalmentDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLastInstalmentDate.Enabled = False
        Me.txtLastInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLastInstalmentDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLastInstalmentDate.Location = New System.Drawing.Point(160, 160)
        Me.txtLastInstalmentDate.MaxLength = 0
        Me.txtLastInstalmentDate.Name = "txtLastInstalmentDate"
        Me.txtLastInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLastInstalmentDate.Size = New System.Drawing.Size(145, 20)
        Me.txtLastInstalmentDate.TabIndex = 93
        Me.txtLastInstalmentDate.TabStop = False
        Me.txtLastInstalmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFinanceCharge
        '
        Me.txtFinanceCharge.AcceptsReturn = True
        Me.txtFinanceCharge.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinanceCharge.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinanceCharge.Enabled = False
        Me.txtFinanceCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinanceCharge.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinanceCharge.Location = New System.Drawing.Point(464, 40)
        Me.txtFinanceCharge.MaxLength = 0
        Me.txtFinanceCharge.Name = "txtFinanceCharge"
        Me.txtFinanceCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinanceCharge.Size = New System.Drawing.Size(145, 20)
        Me.txtFinanceCharge.TabIndex = 79
        Me.txtFinanceCharge.TabStop = False
        Me.txtFinanceCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInterestRate
        '
        Me.txtInterestRate.AcceptsReturn = True
        Me.txtInterestRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtInterestRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInterestRate.Enabled = False
        Me.txtInterestRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterestRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInterestRate.Location = New System.Drawing.Point(160, 16)
        Me.txtInterestRate.MaxLength = 0
        Me.txtInterestRate.Name = "txtInterestRate"
        Me.txtInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInterestRate.Size = New System.Drawing.Size(145, 20)
        Me.txtInterestRate.TabIndex = 73
        Me.txtInterestRate.TabStop = False
        Me.txtInterestRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalAmount
        '
        Me.txtTotalAmount.AcceptsReturn = True
        Me.txtTotalAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalAmount.Enabled = False
        Me.txtTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalAmount.Location = New System.Drawing.Point(464, 160)
        Me.txtTotalAmount.MaxLength = 0
        Me.txtTotalAmount.Name = "txtTotalAmount"
        Me.txtTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalAmount.Size = New System.Drawing.Size(145, 20)
        Me.txtTotalAmount.TabIndex = 95
        Me.txtTotalAmount.TabStop = False
        Me.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTaxes
        '
        Me.txtTaxes.AcceptsReturn = True
        Me.txtTaxes.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxes.Enabled = False
        Me.txtTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxes.Location = New System.Drawing.Point(464, 64)
        Me.txtTaxes.MaxLength = 0
        Me.txtTaxes.Name = "txtTaxes"
        Me.txtTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxes.Size = New System.Drawing.Size(145, 20)
        Me.txtTaxes.TabIndex = 83
        Me.txtTaxes.TabStop = False
        Me.txtTaxes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAPR
        '
        Me.txtAPR.AcceptsReturn = True
        Me.txtAPR.BackColor = System.Drawing.SystemColors.Window
        Me.txtAPR.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAPR.Enabled = False
        Me.txtAPR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAPR.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAPR.Location = New System.Drawing.Point(464, 16)
        Me.txtAPR.MaxLength = 0
        Me.txtAPR.Name = "txtAPR"
        Me.txtAPR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAPR.Size = New System.Drawing.Size(145, 20)
        Me.txtAPR.TabIndex = 75
        Me.txtAPR.TabStop = False
        Me.txtAPR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOtherInstalments
        '
        Me.txtOtherInstalments.AcceptsReturn = True
        Me.txtOtherInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.txtOtherInstalments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOtherInstalments.Enabled = False
        Me.txtOtherInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOtherInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOtherInstalments.Location = New System.Drawing.Point(464, 136)
        Me.txtOtherInstalments.MaxLength = 0
        Me.txtOtherInstalments.Name = "txtOtherInstalments"
        Me.txtOtherInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOtherInstalments.Size = New System.Drawing.Size(145, 20)
        Me.txtOtherInstalments.TabIndex = 91
        Me.txtOtherInstalments.TabStop = False
        Me.txtOtherInstalments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFirstInstalmentValue
        '
        Me.txtFirstInstalmentValue.AcceptsReturn = True
        Me.txtFirstInstalmentValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalmentValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalmentValue.Enabled = False
        Me.txtFirstInstalmentValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalmentValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalmentValue.Location = New System.Drawing.Point(464, 112)
        Me.txtFirstInstalmentValue.MaxLength = 0
        Me.txtFirstInstalmentValue.Name = "txtFirstInstalmentValue"
        Me.txtFirstInstalmentValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalmentValue.Size = New System.Drawing.Size(145, 20)
        Me.txtFirstInstalmentValue.TabIndex = 87
        Me.txtFirstInstalmentValue.TabStop = False
        Me.txtFirstInstalmentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDeposit
        '
        Me.txtDeposit.AcceptsReturn = True
        Me.txtDeposit.BackColor = System.Drawing.SystemColors.Window
        Me.txtDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeposit.Enabled = False
        Me.txtDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeposit.Location = New System.Drawing.Point(160, 40)
        Me.txtDeposit.MaxLength = 0
        Me.txtDeposit.Name = "txtDeposit"
        Me.txtDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeposit.Size = New System.Drawing.Size(145, 20)
        Me.txtDeposit.TabIndex = 77
        Me.txtDeposit.TabStop = False
        Me.txtDeposit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCostOfProtection
        '
        Me.txtCostOfProtection.AcceptsReturn = True
        Me.txtCostOfProtection.BackColor = System.Drawing.SystemColors.Window
        Me.txtCostOfProtection.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCostOfProtection.Enabled = False
        Me.txtCostOfProtection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCostOfProtection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCostOfProtection.Location = New System.Drawing.Point(160, 64)
        Me.txtCostOfProtection.MaxLength = 0
        Me.txtCostOfProtection.Name = "txtCostOfProtection"
        Me.txtCostOfProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCostOfProtection.Size = New System.Drawing.Size(145, 20)
        Me.txtCostOfProtection.TabIndex = 81
        Me.txtCostOfProtection.TabStop = False
        Me.txtCostOfProtection.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFirstInstalmentDate
        '
        Me.lblFirstInstalmentDate.AutoSize = True
        Me.lblFirstInstalmentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstalmentDate.Location = New System.Drawing.Point(8, 112)
        Me.lblFirstInstalmentDate.Name = "lblFirstInstalmentDate"
        Me.lblFirstInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstalmentDate.Size = New System.Drawing.Size(106, 13)
        Me.lblFirstInstalmentDate.TabIndex = 84
        Me.lblFirstInstalmentDate.Text = "First Instalment Date:"
        '
        'lblNextInstalmentDate
        '
        Me.lblNextInstalmentDate.AutoSize = True
        Me.lblNextInstalmentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblNextInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNextInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNextInstalmentDate.Location = New System.Drawing.Point(8, 136)
        Me.lblNextInstalmentDate.Name = "lblNextInstalmentDate"
        Me.lblNextInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNextInstalmentDate.Size = New System.Drawing.Size(109, 13)
        Me.lblNextInstalmentDate.TabIndex = 88
        Me.lblNextInstalmentDate.Text = "Next Instalment Date:"
        '
        'lblLastInstalmentDate
        '
        Me.lblLastInstalmentDate.AutoSize = True
        Me.lblLastInstalmentDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastInstalmentDate.Location = New System.Drawing.Point(8, 160)
        Me.lblLastInstalmentDate.Name = "lblLastInstalmentDate"
        Me.lblLastInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastInstalmentDate.Size = New System.Drawing.Size(107, 13)
        Me.lblLastInstalmentDate.TabIndex = 92
        Me.lblLastInstalmentDate.Text = "Last Instalment Date:"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Line1.Location = New System.Drawing.Point(0, 96)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(688, 1)
        Me.Line1.TabIndex = 96
        '
        'lblFinanceCharge
        '
        Me.lblFinanceCharge.AutoSize = True
        Me.lblFinanceCharge.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinanceCharge.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinanceCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinanceCharge.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinanceCharge.Location = New System.Drawing.Point(320, 40)
        Me.lblFinanceCharge.Name = "lblFinanceCharge"
        Me.lblFinanceCharge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinanceCharge.Size = New System.Drawing.Size(85, 13)
        Me.lblFinanceCharge.TabIndex = 78
        Me.lblFinanceCharge.Text = "Finance Charge:"
        '
        'lblInterestRate
        '
        Me.lblInterestRate.AutoSize = True
        Me.lblInterestRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInterestRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInterestRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterestRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInterestRate.Location = New System.Drawing.Point(8, 16)
        Me.lblInterestRate.Name = "lblInterestRate"
        Me.lblInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInterestRate.Size = New System.Drawing.Size(33, 13)
        Me.lblInterestRate.TabIndex = 72
        Me.lblInterestRate.Text = "Rate:"
        '
        'lblTotalAmount
        '
        Me.lblTotalAmount.AutoSize = True
        Me.lblTotalAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalAmount.Location = New System.Drawing.Point(320, 162)
        Me.lblTotalAmount.Name = "lblTotalAmount"
        Me.lblTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalAmount.Size = New System.Drawing.Size(75, 13)
        Me.lblTotalAmount.TabIndex = 94
        Me.lblTotalAmount.Text = "Total Payable:"
        '
        'lbTaxes
        '
        Me.lbTaxes.AutoSize = True
        Me.lbTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.lbTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbTaxes.Location = New System.Drawing.Point(320, 64)
        Me.lbTaxes.Name = "lbTaxes"
        Me.lbTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbTaxes.Size = New System.Drawing.Size(39, 13)
        Me.lbTaxes.TabIndex = 82
        Me.lbTaxes.Text = "Taxes:"
        '
        'lblAPR
        '
        Me.lblAPR.AutoSize = True
        Me.lblAPR.BackColor = System.Drawing.SystemColors.Control
        Me.lblAPR.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAPR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAPR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAPR.Location = New System.Drawing.Point(320, 16)
        Me.lblAPR.Name = "lblAPR"
        Me.lblAPR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAPR.Size = New System.Drawing.Size(32, 13)
        Me.lblAPR.TabIndex = 74
        Me.lblAPR.Text = "APR:"
        '
        'lblOtherInstalments
        '
        Me.lblOtherInstalments.AutoSize = True
        Me.lblOtherInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblOtherInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOtherInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOtherInstalments.Location = New System.Drawing.Point(320, 138)
        Me.lblOtherInstalments.Name = "lblOtherInstalments"
        Me.lblOtherInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOtherInstalments.Size = New System.Drawing.Size(92, 13)
        Me.lblOtherInstalments.TabIndex = 90
        Me.lblOtherInstalments.Text = "Other Instalments:"
        '
        'lblFirstInstalmentValue
        '
        Me.lblFirstInstalmentValue.AutoSize = True
        Me.lblFirstInstalmentValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstalmentValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstalmentValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstalmentValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstalmentValue.Location = New System.Drawing.Point(320, 114)
        Me.lblFirstInstalmentValue.Name = "lblFirstInstalmentValue"
        Me.lblFirstInstalmentValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstalmentValue.Size = New System.Drawing.Size(80, 13)
        Me.lblFirstInstalmentValue.TabIndex = 86
        Me.lblFirstInstalmentValue.Text = "First Instalment:"
        '
        'lblDeposit
        '
        Me.lblDeposit.AutoSize = True
        Me.lblDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeposit.Location = New System.Drawing.Point(8, 40)
        Me.lblDeposit.Name = "lblDeposit"
        Me.lblDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeposit.Size = New System.Drawing.Size(46, 13)
        Me.lblDeposit.TabIndex = 76
        Me.lblDeposit.Text = "Deposit:"
        '
        'lblCostOfProtection
        '
        Me.lblCostOfProtection.AutoSize = True
        Me.lblCostOfProtection.BackColor = System.Drawing.SystemColors.Control
        Me.lblCostOfProtection.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCostOfProtection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostOfProtection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCostOfProtection.Location = New System.Drawing.Point(8, 64)
        Me.lblCostOfProtection.Name = "lblCostOfProtection"
        Me.lblCostOfProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCostOfProtection.Size = New System.Drawing.Size(95, 13)
        Me.lblCostOfProtection.TabIndex = 80
        Me.lblCostOfProtection.Text = "Protection Charge:"
        '
        'fraDates
        '
        Me.fraDates.BackColor = System.Drawing.SystemColors.Control
        Me.fraDates.Controls.Add(Me.cboStatementFrequency)
        Me.fraDates.Controls.Add(Me.dtpModifiedDate)
        Me.fraDates.Controls.Add(Me.dtpConfirmedDate)
        Me.fraDates.Controls.Add(Me.dtpReviewDate)
        Me.fraDates.Controls.Add(Me.dtpCreatedDate)
        Me.fraDates.Controls.Add(Me.chkNoStatements)
        Me.fraDates.Controls.Add(Me.lblReviewedDate)
        Me.fraDates.Controls.Add(Me.lblConfirmedDate)
        Me.fraDates.Controls.Add(Me.lblModifiedDate)
        Me.fraDates.Controls.Add(Me.lblCreateDate)
        Me.fraDates.Controls.Add(Me.lblStatementFrequency)
        Me.fraDates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDates.Location = New System.Drawing.Point(400, 4)
        Me.fraDates.Name = "fraDates"
        Me.fraDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDates.Size = New System.Drawing.Size(275, 221)
        Me.fraDates.TabIndex = 18
        Me.fraDates.TabStop = False
        Me.fraDates.Text = "Dates"
        '
        'cboStatementFrequency
        '
        Me.cboStatementFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatementFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatementFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatementFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboStatementFrequency, New Integer(-1) {})
        Me.cboStatementFrequency.Location = New System.Drawing.Point(84, 143)
        Me.cboStatementFrequency.Name = "cboStatementFrequency"
        Me.cboStatementFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatementFrequency.Size = New System.Drawing.Size(107, 21)
        Me.cboStatementFrequency.TabIndex = 29
        '
        'dtpModifiedDate
        '
        Me.dtpModifiedDate.Enabled = False
        Me.dtpModifiedDate.Location = New System.Drawing.Point(84, 43)
        Me.dtpModifiedDate.Name = "dtpModifiedDate"
        Me.dtpModifiedDate.ShowCheckBox = True
        Me.dtpModifiedDate.Size = New System.Drawing.Size(107, 20)
        Me.dtpModifiedDate.TabIndex = 22
        '
        'dtpConfirmedDate
        '
        Me.dtpConfirmedDate.Location = New System.Drawing.Point(84, 67)
        Me.dtpConfirmedDate.Name = "dtpConfirmedDate"
        Me.dtpConfirmedDate.ShowCheckBox = True
        Me.dtpConfirmedDate.Size = New System.Drawing.Size(107, 20)
        Me.dtpConfirmedDate.TabIndex = 24
        '
        'dtpReviewDate
        '
        Me.dtpReviewDate.Location = New System.Drawing.Point(84, 91)
        Me.dtpReviewDate.Name = "dtpReviewDate"
        Me.dtpReviewDate.ShowCheckBox = True
        Me.dtpReviewDate.Size = New System.Drawing.Size(107, 20)
        Me.dtpReviewDate.TabIndex = 26
        '
        'dtpCreatedDate
        '
        Me.dtpCreatedDate.Enabled = False
        Me.dtpCreatedDate.Location = New System.Drawing.Point(84, 19)
        Me.dtpCreatedDate.Name = "dtpCreatedDate"
        Me.dtpCreatedDate.ShowCheckBox = True
        Me.dtpCreatedDate.Size = New System.Drawing.Size(107, 20)
        Me.dtpCreatedDate.TabIndex = 20
        '
        'chkNoStatements
        '
        Me.chkNoStatements.BackColor = System.Drawing.SystemColors.Control
        Me.chkNoStatements.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNoStatements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoStatements.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNoStatements.Location = New System.Drawing.Point(8, 119)
        Me.chkNoStatements.Name = "chkNoStatements"
        Me.chkNoStatements.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNoStatements.Size = New System.Drawing.Size(177, 18)
        Me.chkNoStatements.TabIndex = 27
        Me.chkNoStatements.Text = "No Statements"
        Me.chkNoStatements.UseVisualStyleBackColor = False
        '
        'lblReviewedDate
        '
        Me.lblReviewedDate.AutoSize = True
        Me.lblReviewedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblReviewedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReviewedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReviewedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReviewedDate.Location = New System.Drawing.Point(8, 95)
        Me.lblReviewedDate.Name = "lblReviewedDate"
        Me.lblReviewedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReviewedDate.Size = New System.Drawing.Size(46, 13)
        Me.lblReviewedDate.TabIndex = 25
        Me.lblReviewedDate.Tag = "CHILD;"
        Me.lblReviewedDate.Text = "Review:"
        '
        'lblConfirmedDate
        '
        Me.lblConfirmedDate.AutoSize = True
        Me.lblConfirmedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblConfirmedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConfirmedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConfirmedDate.Location = New System.Drawing.Point(8, 71)
        Me.lblConfirmedDate.Name = "lblConfirmedDate"
        Me.lblConfirmedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConfirmedDate.Size = New System.Drawing.Size(57, 13)
        Me.lblConfirmedDate.TabIndex = 23
        Me.lblConfirmedDate.Tag = "CHILD;"
        Me.lblConfirmedDate.Text = "Confirmed:"
        '
        'lblModifiedDate
        '
        Me.lblModifiedDate.AutoSize = True
        Me.lblModifiedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblModifiedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblModifiedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModifiedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblModifiedDate.Location = New System.Drawing.Point(8, 47)
        Me.lblModifiedDate.Name = "lblModifiedDate"
        Me.lblModifiedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblModifiedDate.Size = New System.Drawing.Size(50, 13)
        Me.lblModifiedDate.TabIndex = 21
        Me.lblModifiedDate.Tag = "CHILD;"
        Me.lblModifiedDate.Text = "Modified:"
        '
        'lblCreateDate
        '
        Me.lblCreateDate.AutoSize = True
        Me.lblCreateDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreateDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreateDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreateDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreateDate.Location = New System.Drawing.Point(8, 23)
        Me.lblCreateDate.Name = "lblCreateDate"
        Me.lblCreateDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreateDate.Size = New System.Drawing.Size(47, 13)
        Me.lblCreateDate.TabIndex = 19
        Me.lblCreateDate.Tag = "CHILD;"
        Me.lblCreateDate.Text = "Created:"
        '
        'lblStatementFrequency
        '
        Me.lblStatementFrequency.AutoSize = True
        Me.lblStatementFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatementFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatementFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatementFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatementFrequency.Location = New System.Drawing.Point(8, 147)
        Me.lblStatementFrequency.Name = "lblStatementFrequency"
        Me.lblStatementFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatementFrequency.Size = New System.Drawing.Size(55, 13)
        Me.lblStatementFrequency.TabIndex = 28
        Me.lblStatementFrequency.Tag = "CHILD;"
        Me.lblStatementFrequency.Text = "Stmt Freq:"
        '
        'fraSummary
        '
        Me.fraSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraSummary.Controls.Add(Me.cboCancelReason)
        Me.fraSummary.Controls.Add(Me.txtTerms)
        Me.fraSummary.Controls.Add(Me.txtReference)
        Me.fraSummary.Controls.Add(Me.txtFinancedAmount)
        Me.fraSummary.Controls.Add(Me.txtOriginalDebt)
        Me.fraSummary.Controls.Add(Me.cboStatus)
        Me.fraSummary.Controls.Add(Me.txtDaysDelay)
        Me.fraSummary.Controls.Add(Me.txtStartDate)
        Me.fraSummary.Controls.Add(Me.txtNumberOfInstalments)
        Me.fraSummary.Controls.Add(Me.cmdHistory)
        Me.fraSummary.Controls.Add(Me.cmdTransactions)
        Me.fraSummary.Controls.Add(Me.lblCancelReason)
        Me.fraSummary.Controls.Add(Me.lblTerms)
        Me.fraSummary.Controls.Add(Me.lblReference)
        Me.fraSummary.Controls.Add(Me.lblFinanceAmount)
        Me.fraSummary.Controls.Add(Me.lblOriginalDebt)
        Me.fraSummary.Controls.Add(Me.lblHeader)
        Me.fraSummary.Controls.Add(Me.lblDaysDelay)
        Me.fraSummary.Controls.Add(Me.lblStartDate)
        Me.fraSummary.Controls.Add(Me.lblNumberOfInstalments)
        Me.fraSummary.Controls.Add(Me.lblStatus)
        Me.fraSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSummary.Location = New System.Drawing.Point(8, 2)
        Me.fraSummary.Name = "fraSummary"
        Me.fraSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSummary.Size = New System.Drawing.Size(389, 221)
        Me.fraSummary.TabIndex = 1
        Me.fraSummary.TabStop = False
        Me.fraSummary.Text = "Summary"
        '
        'cboCancelReason
        '
        Me.cboCancelReason.DefaultItemId = 0
        Me.cboCancelReason.FirstItem = ""
        Me.cboCancelReason.ItemId = 0
        Me.cboCancelReason.ListIndex = -1
        Me.cboCancelReason.Location = New System.Drawing.Point(80, 88)
        Me.cboCancelReason.Name = "cboCancelReason"
        Me.cboCancelReason.PMLookupProductFamily = 1
        Me.cboCancelReason.SingleItemId = 0
        Me.cboCancelReason.Size = New System.Drawing.Size(137, 21)
        Me.cboCancelReason.Sorted = True
        Me.cboCancelReason.TabIndex = 183
        Me.cboCancelReason.TableName = "PFPremiumFinance_Cancel_Reason"
        Me.cboCancelReason.ToolTipText = ""
        Me.cboCancelReason.WhereClause = ""
        '
        'txtTerms
        '
        Me.txtTerms.AcceptsReturn = True
        Me.txtTerms.BackColor = System.Drawing.SystemColors.Window
        Me.txtTerms.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTerms.Enabled = False
        Me.txtTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTerms.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTerms.Location = New System.Drawing.Point(328, 196)
        Me.txtTerms.MaxLength = 0
        Me.txtTerms.Name = "txtTerms"
        Me.txtTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTerms.Size = New System.Drawing.Size(33, 20)
        Me.txtTerms.TabIndex = 112
        Me.txtTerms.TabStop = False
        Me.txtTerms.Tag = "CHILD;"
        Me.txtTerms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReference
        '
        Me.txtReference.AcceptsReturn = True
        Me.txtReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReference.Location = New System.Drawing.Point(112, 192)
        Me.txtReference.MaxLength = 0
        Me.txtReference.Name = "txtReference"
        Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReference.Size = New System.Drawing.Size(105, 20)
        Me.txtReference.TabIndex = 17
        Me.txtReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFinancedAmount
        '
        Me.txtFinancedAmount.AcceptsReturn = True
        Me.txtFinancedAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinancedAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinancedAmount.Enabled = False
        Me.txtFinancedAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinancedAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinancedAmount.Location = New System.Drawing.Point(112, 172)
        Me.txtFinancedAmount.MaxLength = 0
        Me.txtFinancedAmount.Name = "txtFinancedAmount"
        Me.txtFinancedAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinancedAmount.Size = New System.Drawing.Size(105, 20)
        Me.txtFinancedAmount.TabIndex = 14
        Me.txtFinancedAmount.TabStop = False
        Me.txtFinancedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOriginalDebt
        '
        Me.txtOriginalDebt.AcceptsReturn = True
        Me.txtOriginalDebt.BackColor = System.Drawing.SystemColors.Window
        Me.txtOriginalDebt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOriginalDebt.Enabled = False
        Me.txtOriginalDebt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOriginalDebt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOriginalDebt.Location = New System.Drawing.Point(112, 148)
        Me.txtOriginalDebt.MaxLength = 0
        Me.txtOriginalDebt.Name = "txtOriginalDebt"
        Me.txtOriginalDebt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOriginalDebt.Size = New System.Drawing.Size(105, 20)
        Me.txtOriginalDebt.TabIndex = 12
        Me.txtOriginalDebt.TabStop = False
        Me.txtOriginalDebt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboStatus
        '
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Enabled = False
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboStatus, New Integer(-1) {})
        Me.cboStatus.Location = New System.Drawing.Point(80, 59)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(137, 21)
        Me.cboStatus.TabIndex = 4
        '
        'txtDaysDelay
        '
        Me.txtDaysDelay.AcceptsReturn = True
        Me.txtDaysDelay.BackColor = System.Drawing.SystemColors.Window
        Me.txtDaysDelay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDaysDelay.Enabled = False
        Me.txtDaysDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDaysDelay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDaysDelay.Location = New System.Drawing.Point(328, 124)
        Me.txtDaysDelay.MaxLength = 0
        Me.txtDaysDelay.Name = "txtDaysDelay"
        Me.txtDaysDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDaysDelay.Size = New System.Drawing.Size(33, 20)
        Me.txtDaysDelay.TabIndex = 10
        Me.txtDaysDelay.TabStop = False
        Me.txtDaysDelay.Tag = "CHILD;"
        Me.txtDaysDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Enabled = False
        Me.txtStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(80, 124)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(137, 20)
        Me.txtStartDate.TabIndex = 8
        Me.txtStartDate.TabStop = False
        Me.txtStartDate.Tag = "CHILD;"
        '
        'txtNumberOfInstalments
        '
        Me.txtNumberOfInstalments.AcceptsReturn = True
        Me.txtNumberOfInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumberOfInstalments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumberOfInstalments.Enabled = False
        Me.txtNumberOfInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumberOfInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumberOfInstalments.Location = New System.Drawing.Point(328, 172)
        Me.txtNumberOfInstalments.MaxLength = 0
        Me.txtNumberOfInstalments.Name = "txtNumberOfInstalments"
        Me.txtNumberOfInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumberOfInstalments.Size = New System.Drawing.Size(33, 20)
        Me.txtNumberOfInstalments.TabIndex = 16
        Me.txtNumberOfInstalments.TabStop = False
        Me.txtNumberOfInstalments.Tag = "CHILD;"
        Me.txtNumberOfInstalments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdHistory
        '
        Me.cmdHistory.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHistory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHistory.Location = New System.Drawing.Point(304, 59)
        Me.cmdHistory.Name = "cmdHistory"
        Me.cmdHistory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHistory.Size = New System.Drawing.Size(59, 22)
        Me.cmdHistory.TabIndex = 6
        Me.cmdHistory.Tag = "CHILD;"
        Me.cmdHistory.Text = "&History"
        Me.cmdHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHistory.UseVisualStyleBackColor = False
        '
        'cmdTransactions
        '
        Me.cmdTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransactions.Location = New System.Drawing.Point(232, 59)
        Me.cmdTransactions.Name = "cmdTransactions"
        Me.cmdTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransactions.Size = New System.Drawing.Size(59, 22)
        Me.cmdTransactions.TabIndex = 5
        Me.cmdTransactions.Tag = "CHILD;"
        Me.cmdTransactions.Text = "Tr&ans "
        Me.cmdTransactions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTransactions.UseVisualStyleBackColor = False
        '
        'lblCancelReason
        '
        Me.lblCancelReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblCancelReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCancelReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCancelReason.Location = New System.Drawing.Point(8, 88)
        Me.lblCancelReason.Name = "lblCancelReason"
        Me.lblCancelReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCancelReason.Size = New System.Drawing.Size(60, 39)
        Me.lblCancelReason.TabIndex = 181
        Me.lblCancelReason.Text = "Cancel Reason:"
        '
        'lblTerms
        '
        Me.lblTerms.AutoSize = True
        Me.lblTerms.BackColor = System.Drawing.SystemColors.Control
        Me.lblTerms.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTerms.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTerms.Location = New System.Drawing.Point(240, 199)
        Me.lblTerms.Name = "lblTerms"
        Me.lblTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTerms.Size = New System.Drawing.Size(69, 13)
        Me.lblTerms.TabIndex = 113
        Me.lblTerms.Tag = "CHILD;"
        Me.lblTerms.Text = "Credit Terms:"
        '
        'lblReference
        '
        Me.lblReference.AutoSize = True
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(8, 199)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(60, 13)
        Me.lblReference.TabIndex = 107
        Me.lblReference.Text = "Reference:"
        '
        'lblFinanceAmount
        '
        Me.lblFinanceAmount.AutoSize = True
        Me.lblFinanceAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinanceAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinanceAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinanceAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinanceAmount.Location = New System.Drawing.Point(8, 175)
        Me.lblFinanceAmount.Name = "lblFinanceAmount"
        Me.lblFinanceAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinanceAmount.Size = New System.Drawing.Size(93, 13)
        Me.lblFinanceAmount.TabIndex = 13
        Me.lblFinanceAmount.Text = "Financed Amount:"
        '
        'lblOriginalDebt
        '
        Me.lblOriginalDebt.AutoSize = True
        Me.lblOriginalDebt.BackColor = System.Drawing.SystemColors.Control
        Me.lblOriginalDebt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOriginalDebt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOriginalDebt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOriginalDebt.Location = New System.Drawing.Point(8, 151)
        Me.lblOriginalDebt.Name = "lblOriginalDebt"
        Me.lblOriginalDebt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOriginalDebt.Size = New System.Drawing.Size(71, 13)
        Me.lblOriginalDebt.TabIndex = 11
        Me.lblOriginalDebt.Text = "Original Debt:"
        '
        'lblHeader
        '
        Me.lblHeader.BackColor = System.Drawing.SystemColors.Highlight
        Me.lblHeader.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHeader.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblHeader.Location = New System.Drawing.Point(8, 16)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHeader.Size = New System.Drawing.Size(355, 33)
        Me.lblHeader.TabIndex = 2
        Me.lblHeader.Text = "<Scheme Description>,Frequency, Payment Method"
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDaysDelay
        '
        Me.lblDaysDelay.AutoSize = True
        Me.lblDaysDelay.BackColor = System.Drawing.SystemColors.Control
        Me.lblDaysDelay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDaysDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDaysDelay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDaysDelay.Location = New System.Drawing.Point(248, 127)
        Me.lblDaysDelay.Name = "lblDaysDelay"
        Me.lblDaysDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDaysDelay.Size = New System.Drawing.Size(64, 13)
        Me.lblDaysDelay.TabIndex = 9
        Me.lblDaysDelay.Tag = "CHILD;"
        Me.lblDaysDelay.Text = "Days Delay:"
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(8, 127)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(58, 13)
        Me.lblStartDate.TabIndex = 7
        Me.lblStartDate.Tag = "CHILD;"
        Me.lblStartDate.Text = "Start Date:"
        '
        'lblNumberOfInstalments
        '
        Me.lblNumberOfInstalments.AutoSize = True
        Me.lblNumberOfInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumberOfInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumberOfInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumberOfInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumberOfInstalments.Location = New System.Drawing.Point(248, 175)
        Me.lblNumberOfInstalments.Name = "lblNumberOfInstalments"
        Me.lblNumberOfInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumberOfInstalments.Size = New System.Drawing.Size(63, 13)
        Me.lblNumberOfInstalments.TabIndex = 15
        Me.lblNumberOfInstalments.Tag = "CHILD;"
        Me.lblNumberOfInstalments.Text = "Instalments:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(8, 63)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 3
        Me.lblStatus.Text = "Status:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraClient)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2-Client Information"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.addClient)
        Me.fraClient.Controls.Add(Me.chkPartnership)
        Me.fraClient.Controls.Add(Me.frmPartners)
        Me.fraClient.Controls.Add(Me.framPhone)
        Me.fraClient.Controls.Add(Me.txtClientName)
        Me.fraClient.Controls.Add(Me.framAdditional)
        Me.fraClient.Controls.Add(Me.lblClientName)
        Me.fraClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(8, 3)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(667, 422)
        Me.fraClient.TabIndex = 47
        Me.fraClient.TabStop = False
        '
        'addClient
        '
        Me.addClient.AddressLine1 = ""
        Me.addClient.AddressLine2 = ""
        Me.addClient.AddressLine3 = ""
        Me.addClient.AddressLine4 = ""
        Me.addClient.Caption = ""
        Me.addClient.CaptionAddress1 = "No. && street name:"
        Me.addClient.CaptionAddress2 = "Locality:"
        Me.addClient.CaptionAddress3 = "Town:"
        Me.addClient.CaptionAddress4 = "County:"
        Me.addClient.CaptionCountry = "Country:"
        Me.addClient.CaptionFontBoldAddress1 = False
        Me.addClient.CaptionFontBoldPostCode = False
        Me.addClient.CaptionPostCode = "Postcode:"
        Me.addClient.ClearButtonCaption = "X"
        Me.addClient.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addClient.ClearButtonLeft = 4695
        Me.addClient.ClearButtonWidth = 360
        Me.addClient.CountryId = 2
        Me.addClient.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addClient.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addClient.IsCountryRequired = 1
        Me.addClient.IsPostCodeRequired = 1
        Me.addClient.Location = New System.Drawing.Point(6, 48)
        Me.addClient.Name = "addClient"
        Me.addClient.Organisation = ""
        Me.addClient.PMAddressCnt = 0
        Me.addClient.PMDatabaseID = 0
        Me.addClient.PostCode = ""
        Me.addClient.QAS2PMAddress1 = "3,4,2,5,6"
        Me.addClient.QAS2PMAddress2 = "8,7"
        Me.addClient.QAS2PMAddress3 = "9"
        Me.addClient.QAS2PMAddress4 = ""
        Me.addClient.QASDatabaseID = 1
        Me.addClient.SearchButtonCaption = ".."
        Me.addClient.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addClient.SearchButtonHeight = 285
        Me.addClient.SearchButtonLeft = 4305
        Me.addClient.SearchButtonTop = 1530
        Me.addClient.SearchButtonWidth = 360
        Me.addClient.Size = New System.Drawing.Size(357, 154)
        Me.addClient.TabIndex = 118
        Me.addClient.WarningMessage = ""
        '
        'chkPartnership
        '
        Me.chkPartnership.BackColor = System.Drawing.SystemColors.Control
        Me.chkPartnership.Checked = True
        Me.chkPartnership.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPartnership.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPartnership.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPartnership.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPartnership.Location = New System.Drawing.Point(400, 20)
        Me.chkPartnership.Name = "chkPartnership"
        Me.chkPartnership.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPartnership.Size = New System.Drawing.Size(174, 17)
        Me.chkPartnership.TabIndex = 109
        Me.chkPartnership.Text = "Is this a limited company?"
        Me.chkPartnership.UseVisualStyleBackColor = False
        '
        'frmPartners
        '
        Me.frmPartners.BackColor = System.Drawing.SystemColors.Control
        Me.frmPartners.Controls.Add(Me.cmdDeletePartner)
        Me.frmPartners.Controls.Add(Me.cmdAddPartner)
        Me.frmPartners.Controls.Add(Me.lvwPartners)
        Me.frmPartners.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmPartners.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmPartners.Location = New System.Drawing.Point(369, 57)
        Me.frmPartners.Name = "frmPartners"
        Me.frmPartners.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmPartners.Size = New System.Drawing.Size(255, 145)
        Me.frmPartners.TabIndex = 114
        Me.frmPartners.TabStop = False
        '
        'cmdDeletePartner
        '
        Me.cmdDeletePartner.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeletePartner.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeletePartner.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeletePartner.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeletePartner.Location = New System.Drawing.Point(80, 102)
        Me.cmdDeletePartner.Name = "cmdDeletePartner"
        Me.cmdDeletePartner.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeletePartner.Size = New System.Drawing.Size(65, 22)
        Me.cmdDeletePartner.TabIndex = 117
        Me.cmdDeletePartner.Text = "&Delete"
        Me.cmdDeletePartner.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeletePartner.UseVisualStyleBackColor = False
        '
        'cmdAddPartner
        '
        Me.cmdAddPartner.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddPartner.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddPartner.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddPartner.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddPartner.Location = New System.Drawing.Point(8, 102)
        Me.cmdAddPartner.Name = "cmdAddPartner"
        Me.cmdAddPartner.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddPartner.Size = New System.Drawing.Size(65, 22)
        Me.cmdAddPartner.TabIndex = 116
        Me.cmdAddPartner.Text = "&Add"
        Me.cmdAddPartner.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddPartner.UseVisualStyleBackColor = False
        '
        'lvwPartners
        '
        Me.lvwPartners.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPartners, "")
        Me.lvwPartners.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPartners_ColumnHeader_1, Me._lvwPartners_ColumnHeader_2, Me._lvwPartners_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPartners, False)
        Me.lvwPartners.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPartners.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPartners, "")
        Me.lvwPartners.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwPartners, "")
        Me.lvwPartners.Location = New System.Drawing.Point(8, 16)
        Me.lvwPartners.Name = "lvwPartners"
        Me.lvwPartners.Size = New System.Drawing.Size(233, 81)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPartners, "")
        Me.listViewHelper1.SetSorted(Me.lvwPartners, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPartners, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPartners, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPartners.TabIndex = 115
        Me.lvwPartners.UseCompatibleStateImageBehavior = False
        Me.lvwPartners.View = System.Windows.Forms.View.Details
        '
        '_lvwPartners_ColumnHeader_1
        '
        Me._lvwPartners_ColumnHeader_1.Text = "Name"
        Me._lvwPartners_ColumnHeader_1.Width = 97
        '
        '_lvwPartners_ColumnHeader_2
        '
        Me._lvwPartners_ColumnHeader_2.Text = "Address Line 1"
        Me._lvwPartners_ColumnHeader_2.Width = 97
        '
        '_lvwPartners_ColumnHeader_3
        '
        Me._lvwPartners_ColumnHeader_3.Text = "Postcode"
        Me._lvwPartners_ColumnHeader_3.Width = 97
        '
        'framPhone
        '
        Me.framPhone.BackColor = System.Drawing.SystemColors.Control
        Me.framPhone.Controls.Add(Me.txtClientFaxCode)
        Me.framPhone.Controls.Add(Me.txtClientFaxNumber)
        Me.framPhone.Controls.Add(Me.txtClientAreaCode)
        Me.framPhone.Controls.Add(Me.txtClientNumber)
        Me.framPhone.Controls.Add(Me.txtClientExtension)
        Me.framPhone.Controls.Add(Me.lblClientFax)
        Me.framPhone.Controls.Add(Me.lblClientTelephone)
        Me.framPhone.Controls.Add(Me.lblClientAreaCode)
        Me.framPhone.Controls.Add(Me.lblClientNumber)
        Me.framPhone.Controls.Add(Me.lblClientExtension)
        Me.framPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.framPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framPhone.Location = New System.Drawing.Point(8, 208)
        Me.framPhone.Name = "framPhone"
        Me.framPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framPhone.Size = New System.Drawing.Size(567, 82)
        Me.framPhone.TabIndex = 50
        Me.framPhone.TabStop = False
        Me.framPhone.Text = "Contact Information"
        '
        'txtClientFaxCode
        '
        Me.txtClientFaxCode.AcceptsReturn = True
        Me.txtClientFaxCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientFaxCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientFaxCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientFaxCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientFaxCode.Location = New System.Drawing.Point(126, 48)
        Me.txtClientFaxCode.MaxLength = 10
        Me.txtClientFaxCode.Name = "txtClientFaxCode"
        Me.txtClientFaxCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientFaxCode.Size = New System.Drawing.Size(73, 20)
        Me.txtClientFaxCode.TabIndex = 59
        '
        'txtClientFaxNumber
        '
        Me.txtClientFaxNumber.AcceptsReturn = True
        Me.txtClientFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientFaxNumber.Location = New System.Drawing.Point(222, 48)
        Me.txtClientFaxNumber.MaxLength = 15
        Me.txtClientFaxNumber.Name = "txtClientFaxNumber"
        Me.txtClientFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientFaxNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtClientFaxNumber.TabIndex = 60
        '
        'txtClientAreaCode
        '
        Me.txtClientAreaCode.AcceptsReturn = True
        Me.txtClientAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientAreaCode.Location = New System.Drawing.Point(126, 24)
        Me.txtClientAreaCode.MaxLength = 10
        Me.txtClientAreaCode.Name = "txtClientAreaCode"
        Me.txtClientAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientAreaCode.Size = New System.Drawing.Size(73, 20)
        Me.txtClientAreaCode.TabIndex = 55
        '
        'txtClientNumber
        '
        Me.txtClientNumber.AcceptsReturn = True
        Me.txtClientNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientNumber.Location = New System.Drawing.Point(222, 24)
        Me.txtClientNumber.MaxLength = 50
        Me.txtClientNumber.Name = "txtClientNumber"
        Me.txtClientNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtClientNumber.TabIndex = 56
        '
        'txtClientExtension
        '
        Me.txtClientExtension.AcceptsReturn = True
        Me.txtClientExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientExtension.Location = New System.Drawing.Point(402, 24)
        Me.txtClientExtension.MaxLength = 10
        Me.txtClientExtension.Name = "txtClientExtension"
        Me.txtClientExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientExtension.Size = New System.Drawing.Size(89, 20)
        Me.txtClientExtension.TabIndex = 57
        '
        'lblClientFax
        '
        Me.lblClientFax.AutoSize = True
        Me.lblClientFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientFax.Location = New System.Drawing.Point(10, 48)
        Me.lblClientFax.Name = "lblClientFax"
        Me.lblClientFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientFax.Size = New System.Drawing.Size(44, 13)
        Me.lblClientFax.TabIndex = 58
        Me.lblClientFax.Text = "Fax No:"
        '
        'lblClientTelephone
        '
        Me.lblClientTelephone.AutoSize = True
        Me.lblClientTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientTelephone.Location = New System.Drawing.Point(10, 24)
        Me.lblClientTelephone.Name = "lblClientTelephone"
        Me.lblClientTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientTelephone.Size = New System.Drawing.Size(78, 13)
        Me.lblClientTelephone.TabIndex = 54
        Me.lblClientTelephone.Text = "Telephone No:"
        '
        'lblClientAreaCode
        '
        Me.lblClientAreaCode.AutoSize = True
        Me.lblClientAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientAreaCode.Location = New System.Drawing.Point(126, 8)
        Me.lblClientAreaCode.Name = "lblClientAreaCode"
        Me.lblClientAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientAreaCode.Size = New System.Drawing.Size(57, 13)
        Me.lblClientAreaCode.TabIndex = 51
        Me.lblClientAreaCode.Text = "Area Code"
        Me.lblClientAreaCode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblClientNumber
        '
        Me.lblClientNumber.AutoSize = True
        Me.lblClientNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientNumber.Location = New System.Drawing.Point(222, 8)
        Me.lblClientNumber.Name = "lblClientNumber"
        Me.lblClientNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientNumber.Size = New System.Drawing.Size(44, 13)
        Me.lblClientNumber.TabIndex = 52
        Me.lblClientNumber.Text = "Number"
        Me.lblClientNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblClientExtension
        '
        Me.lblClientExtension.AutoSize = True
        Me.lblClientExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientExtension.Location = New System.Drawing.Point(402, 8)
        Me.lblClientExtension.Name = "lblClientExtension"
        Me.lblClientExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientExtension.Size = New System.Drawing.Size(53, 13)
        Me.lblClientExtension.TabIndex = 53
        Me.lblClientExtension.Text = "Extension"
        Me.lblClientExtension.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = True
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.txtClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(104, 20)
        Me.txtClientName.MaxLength = 255
        Me.txtClientName.Multiline = True
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(287, 19)
        Me.txtClientName.TabIndex = 49
        '
        'framAdditional
        '
        Me.framAdditional.BackColor = System.Drawing.SystemColors.Control
        Me.framAdditional.Controls.Add(Me.cboRefundType)
        Me.framAdditional.Controls.Add(Me.cboBusinessCode)
        Me.framAdditional.Controls.Add(Me.txtAuthCode)
        Me.framAdditional.Controls.Add(Me.txtPFReference)
        Me.framAdditional.Controls.Add(Me.txtDateOfBirth)
        Me.framAdditional.Controls.Add(Me.txtCompanyReg)
        Me.framAdditional.Controls.Add(Me.lblRefundType)
        Me.framAdditional.Controls.Add(Me.lblBusinessCode)
        Me.framAdditional.Controls.Add(Me.lblAuthCode)
        Me.framAdditional.Controls.Add(Me.lblPFReference)
        Me.framAdditional.Controls.Add(Me.lblDateOfBirth)
        Me.framAdditional.Controls.Add(Me.lblCompanyReg)
        Me.framAdditional.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.framAdditional.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framAdditional.Location = New System.Drawing.Point(8, 296)
        Me.framAdditional.Name = "framAdditional"
        Me.framAdditional.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framAdditional.Size = New System.Drawing.Size(567, 99)
        Me.framAdditional.TabIndex = 61
        Me.framAdditional.TabStop = False
        Me.framAdditional.Text = "Additional Information"
        '
        'cboRefundType
        '
        Me.cboRefundType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRefundType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRefundType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRefundType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboRefundType, New Integer() {0, 0, 0})
        Me.cboRefundType.Items.AddRange(New Object() {"MD", "Pro-Rata", "Short Period"})
        Me.cboRefundType.Location = New System.Drawing.Point(416, 64)
        Me.cboRefundType.Name = "cboRefundType"
        Me.cboRefundType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRefundType.Size = New System.Drawing.Size(129, 21)
        Me.cboRefundType.TabIndex = 111
        '
        'cboBusinessCode
        '
        Me.cboBusinessCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboBusinessCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBusinessCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBusinessCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboBusinessCode, New Integer(-1) {})
        Me.cboBusinessCode.Location = New System.Drawing.Point(128, 64)
        Me.cboBusinessCode.Name = "cboBusinessCode"
        Me.cboBusinessCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBusinessCode.Size = New System.Drawing.Size(161, 21)
        Me.cboBusinessCode.TabIndex = 70
        Me.cboBusinessCode.Text = " "
        '
        'txtAuthCode
        '
        Me.txtAuthCode.AcceptsReturn = True
        Me.txtAuthCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAuthCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAuthCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAuthCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAuthCode.Location = New System.Drawing.Point(128, 16)
        Me.txtAuthCode.MaxLength = 10
        Me.txtAuthCode.Name = "txtAuthCode"
        Me.txtAuthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAuthCode.Size = New System.Drawing.Size(105, 20)
        Me.txtAuthCode.TabIndex = 63
        '
        'txtPFReference
        '
        Me.txtPFReference.AcceptsReturn = True
        Me.txtPFReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtPFReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFReference.Location = New System.Drawing.Point(128, 40)
        Me.txtPFReference.MaxLength = 10
        Me.txtPFReference.Name = "txtPFReference"
        Me.txtPFReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFReference.Size = New System.Drawing.Size(105, 20)
        Me.txtPFReference.TabIndex = 66
        Me.txtPFReference.Visible = False
        '
        'txtDateOfBirth
        '
        Me.txtDateOfBirth.AcceptsReturn = True
        Me.txtDateOfBirth.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateOfBirth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateOfBirth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateOfBirth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateOfBirth.Location = New System.Drawing.Point(416, 16)
        Me.txtDateOfBirth.MaxLength = 11
        Me.txtDateOfBirth.Name = "txtDateOfBirth"
        Me.txtDateOfBirth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateOfBirth.Size = New System.Drawing.Size(129, 20)
        Me.txtDateOfBirth.TabIndex = 65
        Me.txtDateOfBirth.Visible = False
        '
        'txtCompanyReg
        '
        Me.txtCompanyReg.AcceptsReturn = True
        Me.txtCompanyReg.BackColor = System.Drawing.SystemColors.Window
        Me.txtCompanyReg.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCompanyReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCompanyReg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCompanyReg.Location = New System.Drawing.Point(416, 40)
        Me.txtCompanyReg.MaxLength = 10
        Me.txtCompanyReg.Name = "txtCompanyReg"
        Me.txtCompanyReg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCompanyReg.Size = New System.Drawing.Size(129, 20)
        Me.txtCompanyReg.TabIndex = 69
        Me.txtCompanyReg.Visible = False
        '
        'lblRefundType
        '
        Me.lblRefundType.AutoSize = True
        Me.lblRefundType.BackColor = System.Drawing.Color.Transparent
        Me.lblRefundType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRefundType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRefundType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRefundType.Location = New System.Drawing.Point(326, 67)
        Me.lblRefundType.Name = "lblRefundType"
        Me.lblRefundType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRefundType.Size = New System.Drawing.Size(92, 13)
        Me.lblRefundType.TabIndex = 110
        Me.lblRefundType.Text = "Refund Type:"
        '
        'lblBusinessCode
        '
        Me.lblBusinessCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBusinessCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBusinessCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusinessCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBusinessCode.Location = New System.Drawing.Point(8, 64)
        Me.lblBusinessCode.Name = "lblBusinessCode"
        Me.lblBusinessCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBusinessCode.Size = New System.Drawing.Size(121, 17)
        Me.lblBusinessCode.TabIndex = 108
        Me.lblBusinessCode.Text = "Business Code:"
        '
        'lblAuthCode
        '
        Me.lblAuthCode.AutoSize = True
        Me.lblAuthCode.BackColor = System.Drawing.Color.Transparent
        Me.lblAuthCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAuthCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAuthCode.Location = New System.Drawing.Point(8, 16)
        Me.lblAuthCode.Name = "lblAuthCode"
        Me.lblAuthCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAuthCode.Size = New System.Drawing.Size(99, 13)
        Me.lblAuthCode.TabIndex = 62
        Me.lblAuthCode.Text = "Authorisation Code:"
        '
        'lblPFReference
        '
        Me.lblPFReference.AutoSize = True
        Me.lblPFReference.BackColor = System.Drawing.Color.Transparent
        Me.lblPFReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPFReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPFReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPFReference.Location = New System.Drawing.Point(8, 40)
        Me.lblPFReference.Name = "lblPFReference"
        Me.lblPFReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPFReference.Size = New System.Drawing.Size(102, 13)
        Me.lblPFReference.TabIndex = 67
        Me.lblPFReference.Text = "Provider Reference:"
        Me.lblPFReference.Visible = False
        '
        'lblDateOfBirth
        '
        Me.lblDateOfBirth.AutoSize = True
        Me.lblDateOfBirth.BackColor = System.Drawing.Color.Transparent
        Me.lblDateOfBirth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOfBirth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOfBirth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOfBirth.Location = New System.Drawing.Point(325, 16)
        Me.lblDateOfBirth.Name = "lblDateOfBirth"
        Me.lblDateOfBirth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOfBirth.Size = New System.Drawing.Size(93, 13)
        Me.lblDateOfBirth.TabIndex = 64
        Me.lblDateOfBirth.Text = "Date of Birth:"
        Me.lblDateOfBirth.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblDateOfBirth.Visible = False
        '
        'lblCompanyReg
        '
        Me.lblCompanyReg.AutoSize = True
        Me.lblCompanyReg.BackColor = System.Drawing.Color.Transparent
        Me.lblCompanyReg.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompanyReg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyReg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompanyReg.Location = New System.Drawing.Point(260, 40)
        Me.lblCompanyReg.Name = "lblCompanyReg"
        Me.lblCompanyReg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompanyReg.Size = New System.Drawing.Size(158, 13)
        Me.lblCompanyReg.TabIndex = 68
        Me.lblCompanyReg.Text = "Company Reg. Number:"
        Me.lblCompanyReg.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCompanyReg.Visible = False
        '
        'lblClientName
        '
        Me.lblClientName.AutoSize = True
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(11, 23)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(89, 13)
        Me.lblClientName.TabIndex = 48
        Me.lblClientName.Text = "Client Name:"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.SSTab1)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3-Bank Details"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ImageList = Me._SSTab1_ImageList
        Me.SSTab1.ItemSize = New System.Drawing.Size(203, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(651, 458)
        Me.SSTab1.TabIndex = 120
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraBank)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraAccount)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(643, 432)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Current"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'fraBank
        '
        Me.fraBank.BackColor = System.Drawing.SystemColors.Control
        Me.fraBank.Controls.Add(Me.uctPartyBankCombo1)
        Me.fraBank.Controls.Add(Me.txtBankName)
        Me.fraBank.Controls.Add(Me.txtAreaCode)
        Me.fraBank.Controls.Add(Me.txtNumber)
        Me.fraBank.Controls.Add(Me.txtExtension)
        Me.fraBank.Controls.Add(Me.txtFaxAreaCode)
        Me.fraBank.Controls.Add(Me.txtFaxNumber)
        Me.fraBank.Controls.Add(Me.addBank)
        Me.fraBank.Controls.Add(Me.lblPaymentAccountType)
        Me.fraBank.Controls.Add(Me.lblBankPhone)
        Me.fraBank.Controls.Add(Me.lblBankFax)
        Me.fraBank.Controls.Add(Me.lblBankName)
        Me.fraBank.Controls.Add(Me.lblAreaCode)
        Me.fraBank.Controls.Add(Me.lblNumber)
        Me.fraBank.Controls.Add(Me.lblExtension)
        Me.fraBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBank.Location = New System.Drawing.Point(4, 4)
        Me.fraBank.Name = "fraBank"
        Me.fraBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBank.Size = New System.Drawing.Size(636, 274)
        Me.fraBank.TabIndex = 121
        Me.fraBank.TabStop = False
        Me.fraBank.Text = "Bank Information"
        '
        'uctPartyBankCombo1
        '
        Me.uctPartyBankCombo1.BankPaymentTypeCode = ""
        Me.uctPartyBankCombo1.EnableAdd = False
        Me.uctPartyBankCombo1.EnableEdit = False
        Me.uctPartyBankCombo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankCombo1.Location = New System.Drawing.Point(140, 12)
        Me.uctPartyBankCombo1.Name = "uctPartyBankCombo1"
        Me.uctPartyBankCombo1.PartyBankDetails = Nothing
        Me.uctPartyBankCombo1.PartyCnt = Nothing
        Me.uctPartyBankCombo1.SelectedPaymentID = 0
        Me.uctPartyBankCombo1.Size = New System.Drawing.Size(383, 27)
        Me.uctPartyBankCombo1.TabIndex = 179
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(144, 40)
        Me.txtBankName.MaxLength = 60
        Me.txtBankName.Multiline = True
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(413, 19)
        Me.txtBankName.TabIndex = 127
        '
        'txtAreaCode
        '
        Me.txtAreaCode.AcceptsReturn = True
        Me.txtAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAreaCode.Location = New System.Drawing.Point(112, 226)
        Me.txtAreaCode.MaxLength = 10
        Me.txtAreaCode.Name = "txtAreaCode"
        Me.txtAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAreaCode.Size = New System.Drawing.Size(73, 20)
        Me.txtAreaCode.TabIndex = 126
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(208, 226)
        Me.txtNumber.MaxLength = 15
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtNumber.TabIndex = 125
        '
        'txtExtension
        '
        Me.txtExtension.AcceptsReturn = True
        Me.txtExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExtension.Location = New System.Drawing.Point(400, 226)
        Me.txtExtension.MaxLength = 6
        Me.txtExtension.Name = "txtExtension"
        Me.txtExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExtension.Size = New System.Drawing.Size(89, 20)
        Me.txtExtension.TabIndex = 124
        '
        'txtFaxAreaCode
        '
        Me.txtFaxAreaCode.AcceptsReturn = True
        Me.txtFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxAreaCode.Location = New System.Drawing.Point(112, 250)
        Me.txtFaxAreaCode.MaxLength = 10
        Me.txtFaxAreaCode.Name = "txtFaxAreaCode"
        Me.txtFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxAreaCode.Size = New System.Drawing.Size(73, 20)
        Me.txtFaxAreaCode.TabIndex = 123
        '
        'txtFaxNumber
        '
        Me.txtFaxNumber.AcceptsReturn = True
        Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxNumber.Location = New System.Drawing.Point(208, 250)
        Me.txtFaxNumber.MaxLength = 15
        Me.txtFaxNumber.Name = "txtFaxNumber"
        Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtFaxNumber.TabIndex = 122
        '
        'addBank
        '
        Me.addBank.AddressLine1 = ""
        Me.addBank.AddressLine2 = ""
        Me.addBank.AddressLine3 = ""
        Me.addBank.AddressLine4 = ""
        Me.addBank.Caption = ""
        Me.addBank.CaptionAddress1 = "No. && street name:"
        Me.addBank.CaptionAddress2 = "Locality:"
        Me.addBank.CaptionAddress3 = "Town:"
        Me.addBank.CaptionAddress4 = "County:"
        Me.addBank.CaptionCountry = "Country:"
        Me.addBank.CaptionFontBoldAddress1 = False
        Me.addBank.CaptionFontBoldPostCode = False
        Me.addBank.CaptionPostCode = "Postcode:"
        Me.addBank.ClearButtonCaption = "X"
        Me.addBank.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addBank.ClearButtonLeft = 4695
        Me.addBank.ClearButtonWidth = 360
        Me.addBank.CountryId = 2
        Me.addBank.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addBank.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addBank.IsCountryRequired = 1
        Me.addBank.IsPostCodeRequired = 1
        Me.addBank.Location = New System.Drawing.Point(8, 56)
        Me.addBank.Name = "addBank"
        Me.addBank.Organisation = ""
        Me.addBank.PMAddressCnt = 0
        Me.addBank.PMDatabaseID = 0
        Me.addBank.PostCode = ""
        Me.addBank.QAS2PMAddress1 = "3,4,2,5,6"
        Me.addBank.QAS2PMAddress2 = "8,7"
        Me.addBank.QAS2PMAddress3 = "9"
        Me.addBank.QAS2PMAddress4 = ""
        Me.addBank.QASDatabaseID = 0
        Me.addBank.SearchButtonCaption = ".."
        Me.addBank.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addBank.SearchButtonHeight = 285
        Me.addBank.SearchButtonLeft = 4305
        Me.addBank.SearchButtonTop = 1530
        Me.addBank.SearchButtonWidth = 360
        Me.addBank.Size = New System.Drawing.Size(561, 152)
        Me.addBank.TabIndex = 128
        Me.addBank.WarningMessage = ""
        '
        'lblPaymentAccountType
        '
        Me.lblPaymentAccountType.AutoSize = True
        Me.lblPaymentAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentAccountType.Location = New System.Drawing.Point(8, 20)
        Me.lblPaymentAccountType.Name = "lblPaymentAccountType"
        Me.lblPaymentAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAccountType.Size = New System.Drawing.Size(77, 13)
        Me.lblPaymentAccountType.TabIndex = 185
        Me.lblPaymentAccountType.Text = "Account Type:"
        '
        'lblBankPhone
        '
        Me.lblBankPhone.AutoSize = True
        Me.lblBankPhone.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankPhone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankPhone.Location = New System.Drawing.Point(8, 229)
        Me.lblBankPhone.Name = "lblBankPhone"
        Me.lblBankPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankPhone.Size = New System.Drawing.Size(78, 13)
        Me.lblBankPhone.TabIndex = 134
        Me.lblBankPhone.Text = "Telephone No:"
        '
        'lblBankFax
        '
        Me.lblBankFax.AutoSize = True
        Me.lblBankFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankFax.Location = New System.Drawing.Point(8, 253)
        Me.lblBankFax.Name = "lblBankFax"
        Me.lblBankFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankFax.Size = New System.Drawing.Size(44, 13)
        Me.lblBankFax.TabIndex = 133
        Me.lblBankFax.Text = "Fax No:"
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(8, 43)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(66, 13)
        Me.lblBankName.TabIndex = 132
        Me.lblBankName.Text = "Bank Name:"
        '
        'lblAreaCode
        '
        Me.lblAreaCode.AutoSize = True
        Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAreaCode.Location = New System.Drawing.Point(117, 209)
        Me.lblAreaCode.Name = "lblAreaCode"
        Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAreaCode.Size = New System.Drawing.Size(57, 13)
        Me.lblAreaCode.TabIndex = 131
        Me.lblAreaCode.Text = "Area Code"
        Me.lblAreaCode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNumber
        '
        Me.lblNumber.AutoSize = True
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(263, 209)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(44, 13)
        Me.lblNumber.TabIndex = 130
        Me.lblNumber.Text = "Number"
        Me.lblNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblExtension
        '
        Me.lblExtension.AutoSize = True
        Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExtension.Location = New System.Drawing.Point(416, 209)
        Me.lblExtension.Name = "lblExtension"
        Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExtension.Size = New System.Drawing.Size(53, 13)
        Me.lblExtension.TabIndex = 129
        Me.lblExtension.Text = "Extension"
        Me.lblExtension.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'fraAccount
        '
        Me.fraAccount.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccount.Controls.Add(Me.txtIBAN)
        Me.fraAccount.Controls.Add(Me.lblIBAN)
        Me.fraAccount.Controls.Add(Me.txtBIC)
        Me.fraAccount.Controls.Add(Me.lblBIC)
        Me.fraAccount.Controls.Add(Me.chkPaperDD)
        Me.fraAccount.Controls.Add(Me.chkDDCancelled)
        Me.fraAccount.Controls.Add(Me.txtDateBankDetailsChanged)
        Me.fraAccount.Controls.Add(Me.cboCopyFromBank)
        Me.fraAccount.Controls.Add(Me.txtAccountName)
        Me.fraAccount.Controls.Add(Me.txtAccountNumber)
        Me.fraAccount.Controls.Add(Me.txtSortCode)
        Me.fraAccount.Controls.Add(Me.txtBranch)
        Me.fraAccount.Controls.Add(Me.lblCancelled)
        Me.fraAccount.Controls.Add(Me.lblDateBankDetailsChanged)
        Me.fraAccount.Controls.Add(Me.lblCopyFromBank)
        Me.fraAccount.Controls.Add(Me.lblAccountName)
        Me.fraAccount.Controls.Add(Me.lblAccountNumber)
        Me.fraAccount.Controls.Add(Me.lblSortCode)
        Me.fraAccount.Controls.Add(Me.lblBranch)
        Me.fraAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccount.Location = New System.Drawing.Point(8, 278)
        Me.fraAccount.Name = "fraAccount"
        Me.fraAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccount.Size = New System.Drawing.Size(632, 151)
        Me.fraAccount.TabIndex = 135
        Me.fraAccount.TabStop = False
        Me.fraAccount.Text = "Account Information"
        '
        'txtIBAN
        '
        Me.txtIBAN.AcceptsReturn = True
        Me.txtIBAN.BackColor = System.Drawing.SystemColors.Window
        Me.txtIBAN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIBAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIBAN.Location = New System.Drawing.Point(392, 86)
        Me.txtIBAN.MaxLength = 50
        Me.txtIBAN.Name = "txtIBAN"
        Me.txtIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIBAN.Size = New System.Drawing.Size(234, 20)
        Me.txtIBAN.TabIndex = 181
        '
        'lblIBAN
        '
        Me.lblIBAN.AutoSize = True
        Me.lblIBAN.BackColor = System.Drawing.SystemColors.Control
        Me.lblIBAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIBAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIBAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIBAN.Location = New System.Drawing.Point(280, 89)
        Me.lblIBAN.Name = "lblIBAN"
        Me.lblIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIBAN.Size = New System.Drawing.Size(35, 13)
        Me.lblIBAN.TabIndex = 182
        Me.lblIBAN.Text = "IBAN:"
        '
        'txtBIC
        '
        Me.txtBIC.AcceptsReturn = True
        Me.txtBIC.BackColor = System.Drawing.SystemColors.Window
        Me.txtBIC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBIC.Location = New System.Drawing.Point(105, 87)
        Me.txtBIC.MaxLength = 50
        Me.txtBIC.Name = "txtBIC"
        Me.txtBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBIC.Size = New System.Drawing.Size(169, 20)
        Me.txtBIC.TabIndex = 179
        '
        'lblBIC
        '
        Me.lblBIC.AutoSize = True
        Me.lblBIC.BackColor = System.Drawing.SystemColors.Control
        Me.lblBIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBIC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBIC.Location = New System.Drawing.Point(8, 90)
        Me.lblBIC.Name = "lblBIC"
        Me.lblBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBIC.Size = New System.Drawing.Size(27, 13)
        Me.lblBIC.TabIndex = 180
        Me.lblBIC.Text = "BIC:"
        '
        'chkPaperDD
        '
        Me.chkPaperDD.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaperDD.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPaperDD.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaperDD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaperDD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaperDD.Location = New System.Drawing.Point(280, 123)
        Me.chkPaperDD.Name = "chkPaperDD"
        Me.chkPaperDD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaperDD.Size = New System.Drawing.Size(125, 16)
        Me.chkPaperDD.TabIndex = 178
        Me.chkPaperDD.Text = "Paper DD:"
        Me.chkPaperDD.UseVisualStyleBackColor = False
        '
        'chkDDCancelled
        '
        Me.chkDDCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.chkDDCancelled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDDCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDDCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDDCancelled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDDCancelled.Location = New System.Drawing.Point(97, 120)
        Me.chkDDCancelled.Name = "chkDDCancelled"
        Me.chkDDCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDDCancelled.Size = New System.Drawing.Size(19, 19)
        Me.chkDDCancelled.TabIndex = 148
        Me.chkDDCancelled.UseVisualStyleBackColor = False
        '
        'txtDateBankDetailsChanged
        '
        Me.txtDateBankDetailsChanged.AcceptsReturn = True
        Me.txtDateBankDetailsChanged.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateBankDetailsChanged.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateBankDetailsChanged.Enabled = False
        Me.txtDateBankDetailsChanged.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateBankDetailsChanged.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateBankDetailsChanged.Location = New System.Drawing.Point(104, 63)
        Me.txtDateBankDetailsChanged.MaxLength = 30
        Me.txtDateBankDetailsChanged.Multiline = True
        Me.txtDateBankDetailsChanged.Name = "txtDateBankDetailsChanged"
        Me.txtDateBankDetailsChanged.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateBankDetailsChanged.Size = New System.Drawing.Size(169, 19)
        Me.txtDateBankDetailsChanged.TabIndex = 141
        '
        'cboCopyFromBank
        '
        Me.cboCopyFromBank.BackColor = System.Drawing.SystemColors.Window
        Me.cboCopyFromBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCopyFromBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCopyFromBank.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCopyFromBank, New Integer(-1) {})
        Me.cboCopyFromBank.Location = New System.Drawing.Point(392, 62)
        Me.cboCopyFromBank.Name = "cboCopyFromBank"
        Me.cboCopyFromBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCopyFromBank.Size = New System.Drawing.Size(169, 21)
        Me.cboCopyFromBank.TabIndex = 140
        Me.cboCopyFromBank.Text = "Combo1"
        Me.cboCopyFromBank.Visible = False
        '
        'txtAccountName
        '
        Me.txtAccountName.AcceptsReturn = True
        Me.txtAccountName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountName.Location = New System.Drawing.Point(104, 39)
        Me.txtAccountName.MaxLength = 30
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountName.Size = New System.Drawing.Size(169, 20)
        Me.txtAccountName.TabIndex = 139
        '
        'txtAccountNumber
        '
        Me.txtAccountNumber.AcceptsReturn = True
        Me.txtAccountNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountNumber.Location = New System.Drawing.Point(392, 39)
        Me.txtAccountNumber.MaxLength = 30
        Me.txtAccountNumber.Name = "txtAccountNumber"
        Me.txtAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountNumber.Size = New System.Drawing.Size(169, 20)
        Me.txtAccountNumber.TabIndex = 138
        '
        'txtSortCode
        '
        Me.txtSortCode.AcceptsReturn = True
        Me.txtSortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtSortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSortCode.Location = New System.Drawing.Point(392, 16)
        Me.txtSortCode.MaxLength = 20
        Me.txtSortCode.Name = "txtSortCode"
        Me.txtSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSortCode.Size = New System.Drawing.Size(121, 20)
        Me.txtSortCode.TabIndex = 137
        '
        'txtBranch
        '
        Me.txtBranch.AcceptsReturn = True
        Me.txtBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranch.Location = New System.Drawing.Point(104, 16)
        Me.txtBranch.MaxLength = 20
        Me.txtBranch.Name = "txtBranch"
        Me.txtBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranch.Size = New System.Drawing.Size(169, 20)
        Me.txtBranch.TabIndex = 136
        '
        'lblCancelled
        '
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.lblCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCancelled.Location = New System.Drawing.Point(7, 123)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCancelled.Size = New System.Drawing.Size(57, 13)
        Me.lblCancelled.TabIndex = 149
        Me.lblCancelled.Text = "Cancelled:"
        '
        'lblDateBankDetailsChanged
        '
        Me.lblDateBankDetailsChanged.AutoSize = True
        Me.lblDateBankDetailsChanged.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateBankDetailsChanged.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateBankDetailsChanged.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateBankDetailsChanged.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateBankDetailsChanged.Location = New System.Drawing.Point(8, 66)
        Me.lblDateBankDetailsChanged.Name = "lblDateBankDetailsChanged"
        Me.lblDateBankDetailsChanged.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateBankDetailsChanged.Size = New System.Drawing.Size(79, 13)
        Me.lblDateBankDetailsChanged.TabIndex = 147
        Me.lblDateBankDetailsChanged.Text = "Date Changed:"
        '
        'lblCopyFromBank
        '
        Me.lblCopyFromBank.AutoSize = True
        Me.lblCopyFromBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblCopyFromBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCopyFromBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCopyFromBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCopyFromBank.Location = New System.Drawing.Point(281, 66)
        Me.lblCopyFromBank.Name = "lblCopyFromBank"
        Me.lblCopyFromBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCopyFromBank.Size = New System.Drawing.Size(60, 13)
        Me.lblCopyFromBank.TabIndex = 146
        Me.lblCopyFromBank.Text = "Copy From:"
        Me.lblCopyFromBank.Visible = False
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(7, 42)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(81, 13)
        Me.lblAccountName.TabIndex = 145
        Me.lblAccountName.Text = "Account Name:"
        '
        'lblAccountNumber
        '
        Me.lblAccountNumber.AutoSize = True
        Me.lblAccountNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountNumber.Location = New System.Drawing.Point(280, 42)
        Me.lblAccountNumber.Name = "lblAccountNumber"
        Me.lblAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountNumber.Size = New System.Drawing.Size(90, 13)
        Me.lblAccountNumber.TabIndex = 144
        Me.lblAccountNumber.Text = "Account Number:"
        '
        'lblSortCode
        '
        Me.lblSortCode.AutoSize = True
        Me.lblSortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSortCode.Location = New System.Drawing.Point(280, 19)
        Me.lblSortCode.Name = "lblSortCode"
        Me.lblSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSortCode.Size = New System.Drawing.Size(72, 13)
        Me.lblSortCode.TabIndex = 143
        Me.lblSortCode.Text = "Branch Code:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(7, 19)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(75, 13)
        Me.lblBranch.TabIndex = 142
        Me.lblBranch.Text = "Branch Name:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.lvwHistory)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(643, 432)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "History"
        Me._SSTab1_TabPage1.UseVisualStyleBackColor = True
        '
        'lvwHistory
        '
        Me.lvwHistory.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwHistory, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwHistory, False)
        Me.lvwHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwHistory, "")
        Me.lvwHistory.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwHistory, "")
        Me.lvwHistory.Location = New System.Drawing.Point(8, 4)
        Me.lvwHistory.Name = "lvwHistory"
        Me.lvwHistory.Size = New System.Drawing.Size(593, 377)
        Me.listViewHelper1.SetSmallIcons(Me.lvwHistory, "")
        Me.listViewHelper1.SetSorted(Me.lvwHistory, False)
        Me.listViewHelper1.SetSortKey(Me.lvwHistory, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwHistory, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwHistory.TabIndex = 150
        Me.lvwHistory.UseCompatibleStateImageBehavior = False
        Me.lvwHistory.View = System.Windows.Forms.View.Details
        '
        '_SSTab1_ImageList
        '
        Me._SSTab1_ImageList.ImageStream = CType(resources.GetObject("_SSTab1_ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me._SSTab1_ImageList.TransparentColor = System.Drawing.Color.Transparent
        Me._SSTab1_ImageList.Images.SetKeyName(0, "")
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.SSTabCreditCard)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4-Credit Card Details"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'SSTabCreditCard
        '
        Me.SSTabCreditCard.Controls.Add(Me._SSTabCreditCard_TabPage0)
        Me.SSTabCreditCard.Controls.Add(Me._SSTabCreditCard_TabPage1)
        Me.SSTabCreditCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTabCreditCard.ItemSize = New System.Drawing.Size(307, 18)
        Me.SSTabCreditCard.Location = New System.Drawing.Point(8, 8)
        Me.SSTabCreditCard.Multiline = True
        Me.SSTabCreditCard.Name = "SSTabCreditCard"
        Me.SSTabCreditCard.SelectedIndex = 0
        Me.SSTabCreditCard.Size = New System.Drawing.Size(667, 413)
        Me.SSTabCreditCard.TabIndex = 151
        '
        '_SSTabCreditCard_TabPage0
        '
        Me._SSTabCreditCard_TabPage0.Controls.Add(Me.Frame2)
        Me._SSTabCreditCard_TabPage0.Controls.Add(Me.Frame1)
        Me._SSTabCreditCard_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTabCreditCard_TabPage0.Name = "_SSTabCreditCard_TabPage0"
        Me._SSTabCreditCard_TabPage0.Size = New System.Drawing.Size(659, 387)
        Me._SSTabCreditCard_TabPage0.TabIndex = 0
        Me._SSTabCreditCard_TabPage0.Text = "Current"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.txtCardholderName)
        Me.Frame2.Controls.Add(Me.addCardholder)
        Me.Frame2.Controls.Add(Me.Label2)
        Me.Frame2.Controls.Add(Me.Label3)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 178)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(648, 207)
        Me.Frame2.TabIndex = 152
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Cardholder"
        '
        'txtCardholderName
        '
        Me.txtCardholderName.AcceptsReturn = True
        Me.txtCardholderName.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardholderName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardholderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardholderName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardholderName.Location = New System.Drawing.Point(64, 32)
        Me.txtCardholderName.MaxLength = 0
        Me.txtCardholderName.Name = "txtCardholderName"
        Me.txtCardholderName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardholderName.Size = New System.Drawing.Size(497, 20)
        Me.txtCardholderName.TabIndex = 154
        '
        'addCardholder
        '
        Me.addCardholder.AddressLine1 = ""
        Me.addCardholder.AddressLine2 = ""
        Me.addCardholder.AddressLine3 = ""
        Me.addCardholder.AddressLine4 = ""
        Me.addCardholder.Caption = ""
        Me.addCardholder.CaptionAddress1 = "No. && street name:"
        Me.addCardholder.CaptionAddress2 = "Locality:"
        Me.addCardholder.CaptionAddress3 = "Town:"
        Me.addCardholder.CaptionAddress4 = "County:"
        Me.addCardholder.CaptionCountry = "Country:"
        Me.addCardholder.CaptionFontBoldAddress1 = False
        Me.addCardholder.CaptionFontBoldPostCode = False
        Me.addCardholder.CaptionPostCode = "Postcode:"
        Me.addCardholder.ClearButtonCaption = "X"
        Me.addCardholder.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addCardholder.ClearButtonLeft = 4695
        Me.addCardholder.ClearButtonWidth = 360
        Me.addCardholder.CountryId = 2
        Me.addCardholder.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addCardholder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addCardholder.IsCountryRequired = 1
        Me.addCardholder.IsPostCodeRequired = 1
        Me.addCardholder.Location = New System.Drawing.Point(16, 50)
        Me.addCardholder.Name = "addCardholder"
        Me.addCardholder.Organisation = ""
        Me.addCardholder.PMAddressCnt = 0
        Me.addCardholder.PMDatabaseID = 0
        Me.addCardholder.PostCode = ""
        Me.addCardholder.QAS2PMAddress1 = "3,4,2,5,6"
        Me.addCardholder.QAS2PMAddress2 = "8,7"
        Me.addCardholder.QAS2PMAddress3 = "9"
        Me.addCardholder.QAS2PMAddress4 = ""
        Me.addCardholder.QASDatabaseID = 0
        Me.addCardholder.SearchButtonCaption = ".."
        Me.addCardholder.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.addCardholder.SearchButtonHeight = 285
        Me.addCardholder.SearchButtonLeft = 4305
        Me.addCardholder.SearchButtonTop = 1530
        Me.addCardholder.SearchButtonWidth = 360
        Me.addCardholder.Size = New System.Drawing.Size(545, 152)
        Me.addCardholder.TabIndex = 153
        Me.addCardholder.WarningMessage = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(279, 13)
        Me.Label2.TabIndex = 156
        Me.Label2.Text = "Please provide details of the registered card holder below:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(16, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 155
        Me.Label3.Text = "Name:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtTrackingNumber)
        Me.Frame1.Controls.Add(Me.uctPartyBankCombo2)
        Me.Frame1.Controls.Add(Me.txtCardNo)
        Me.Frame1.Controls.Add(Me.txtExpiryDate)
        Me.Frame1.Controls.Add(Me.txtIssueNo)
        Me.Frame1.Controls.Add(Me.cboCopyFromCard)
        Me.Frame1.Controls.Add(Me.chkCardholder)
        Me.Frame1.Controls.Add(Me.chkCCCancelled)
        Me.Frame1.Controls.Add(Me.cboCardType)
        Me.Frame1.Controls.Add(Me.txtPin)
        Me.Frame1.Controls.Add(Me.txtCardStartDate)
        Me.Frame1.Controls.Add(Me.txtCardName)
        Me.Frame1.Controls.Add(Me.lblTrackingNumber)
        Me.Frame1.Controls.Add(Me.lblReceiptAccountType)
        Me.Frame1.Controls.Add(Me.lblCCCancelled)
        Me.Frame1.Controls.Add(Me.lblCardType)
        Me.Frame1.Controls.Add(Me.lblPin)
        Me.Frame1.Controls.Add(Me.lblCardStartDate)
        Me.Frame1.Controls.Add(Me.lblCardName)
        Me.Frame1.Controls.Add(Me.lblCopyFromCard)
        Me.Frame1.Controls.Add(Me.lblIssueNo)
        Me.Frame1.Controls.Add(Me.lblExpiryDate)
        Me.Frame1.Controls.Add(Me.lblCardNo)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(648, 173)
        Me.Frame1.TabIndex = 157
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Credit Card Information"
        '
        'txtTrackingNumber
        '
        Me.txtTrackingNumber.AcceptsReturn = True
        Me.txtTrackingNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtTrackingNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTrackingNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrackingNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTrackingNumber.Location = New System.Drawing.Point(418, 148)
        Me.txtTrackingNumber.MaxLength = 30
        Me.txtTrackingNumber.Name = "txtTrackingNumber"
        Me.txtTrackingNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTrackingNumber.Size = New System.Drawing.Size(161, 20)
        Me.txtTrackingNumber.TabIndex = 187
        Me.txtTrackingNumber.TabStop = False
        '
        'uctPartyBankCombo2
        '
        Me.uctPartyBankCombo2.BankPaymentTypeCode = ""
        Me.uctPartyBankCombo2.EnableAdd = False
        Me.uctPartyBankCombo2.EnableEdit = False
        Me.uctPartyBankCombo2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankCombo2.Location = New System.Drawing.Point(102, 14)
        Me.uctPartyBankCombo2.Name = "uctPartyBankCombo2"
        Me.uctPartyBankCombo2.PartyBankDetails = Nothing
        Me.uctPartyBankCombo2.PartyCnt = Nothing
        Me.uctPartyBankCombo2.SelectedPaymentID = 0
        Me.uctPartyBankCombo2.Size = New System.Drawing.Size(377, 25)
        Me.uctPartyBankCombo2.TabIndex = 180
        '
        'txtCardNo
        '
        Me.txtCardNo.AcceptsReturn = True
        Me.txtCardNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardNo.Location = New System.Drawing.Point(106, 46)
        Me.txtCardNo.MaxLength = 30
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardNo.Size = New System.Drawing.Size(161, 20)
        Me.txtCardNo.TabIndex = 167
        Me.txtCardNo.TabStop = False
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(106, 71)
        Me.txtExpiryDate.MaxLength = 10
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(81, 20)
        Me.txtExpiryDate.TabIndex = 166
        Me.txtExpiryDate.TabStop = False
        '
        'txtIssueNo
        '
        Me.txtIssueNo.AcceptsReturn = True
        Me.txtIssueNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssueNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIssueNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIssueNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIssueNo.Location = New System.Drawing.Point(106, 97)
        Me.txtIssueNo.MaxLength = 2
        Me.txtIssueNo.Name = "txtIssueNo"
        Me.txtIssueNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIssueNo.Size = New System.Drawing.Size(49, 20)
        Me.txtIssueNo.TabIndex = 165
        Me.txtIssueNo.TabStop = False
        '
        'cboCopyFromCard
        '
        Me.cboCopyFromCard.BackColor = System.Drawing.SystemColors.Window
        Me.cboCopyFromCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCopyFromCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCopyFromCard.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCopyFromCard, New Integer(-1) {})
        Me.cboCopyFromCard.Location = New System.Drawing.Point(384, 124)
        Me.cboCopyFromCard.Name = "cboCopyFromCard"
        Me.cboCopyFromCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCopyFromCard.Size = New System.Drawing.Size(161, 21)
        Me.cboCopyFromCard.TabIndex = 164
        Me.cboCopyFromCard.Text = "Combo1"
        Me.cboCopyFromCard.Visible = False
        '
        'chkCardholder
        '
        Me.chkCardholder.BackColor = System.Drawing.SystemColors.Control
        Me.chkCardholder.Checked = True
        Me.chkCardholder.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCardholder.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCardholder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCardholder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCardholder.Location = New System.Drawing.Point(6, 150)
        Me.chkCardholder.Name = "chkCardholder"
        Me.chkCardholder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCardholder.Size = New System.Drawing.Size(227, 17)
        Me.chkCardholder.TabIndex = 163
        Me.chkCardholder.Text = "Are you the registered card holder?"
        Me.chkCardholder.UseVisualStyleBackColor = False
        '
        'chkCCCancelled
        '
        Me.chkCCCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.chkCCCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCCCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCCCancelled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCCCancelled.Location = New System.Drawing.Point(558, 19)
        Me.chkCCCancelled.Name = "chkCCCancelled"
        Me.chkCCCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCCCancelled.Size = New System.Drawing.Size(19, 19)
        Me.chkCCCancelled.TabIndex = 162
        Me.chkCCCancelled.Text = "Check1"
        Me.chkCCCancelled.UseVisualStyleBackColor = False
        '
        'cboCardType
        '
        Me.cboCardType.BackColor = System.Drawing.SystemColors.Window
        Me.cboCardType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCardType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCardType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCardType, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.cboCardType.Items.AddRange(New Object() {"VISA", "DELTA", "Mastercard", "JCB", "EDC Maestro", "Switch - Standard", "Switch - Bank of Scotland", "Switch - Clydesdale", "Switch - HSBC", "Switch - Halifax", "Switch - NatWest", "Switch - Royal Bank of Scotland", "Switch - Yorkshire Bank"})
        Me.cboCardType.Location = New System.Drawing.Point(106, 122)
        Me.cboCardType.Name = "cboCardType"
        Me.cboCardType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCardType.Size = New System.Drawing.Size(161, 21)
        Me.cboCardType.TabIndex = 161
        '
        'txtPin
        '
        Me.txtPin.AcceptsReturn = True
        Me.txtPin.BackColor = System.Drawing.SystemColors.Window
        Me.txtPin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPin.Location = New System.Drawing.Point(384, 97)
        Me.txtPin.MaxLength = 20
        Me.txtPin.Name = "txtPin"
        Me.txtPin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPin.Size = New System.Drawing.Size(81, 20)
        Me.txtPin.TabIndex = 160
        Me.txtPin.TabStop = False
        '
        'txtCardStartDate
        '
        Me.txtCardStartDate.AcceptsReturn = True
        Me.txtCardStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardStartDate.Location = New System.Drawing.Point(384, 71)
        Me.txtCardStartDate.MaxLength = 10
        Me.txtCardStartDate.Name = "txtCardStartDate"
        Me.txtCardStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardStartDate.Size = New System.Drawing.Size(81, 20)
        Me.txtCardStartDate.TabIndex = 159
        Me.txtCardStartDate.TabStop = False
        '
        'txtCardName
        '
        Me.txtCardName.AcceptsReturn = True
        Me.txtCardName.BackColor = System.Drawing.SystemColors.Window
        Me.txtCardName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCardName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCardName.Location = New System.Drawing.Point(384, 46)
        Me.txtCardName.MaxLength = 30
        Me.txtCardName.Name = "txtCardName"
        Me.txtCardName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCardName.Size = New System.Drawing.Size(193, 20)
        Me.txtCardName.TabIndex = 158
        Me.txtCardName.TabStop = False
        '
        'lblTrackingNumber
        '
        Me.lblTrackingNumber.AutoSize = True
        Me.lblTrackingNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblTrackingNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTrackingNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrackingNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTrackingNumber.Location = New System.Drawing.Point(310, 152)
        Me.lblTrackingNumber.Name = "lblTrackingNumber"
        Me.lblTrackingNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTrackingNumber.Size = New System.Drawing.Size(92, 13)
        Me.lblTrackingNumber.TabIndex = 188
        Me.lblTrackingNumber.Text = "Tracking Number:"
        '
        'lblReceiptAccountType
        '
        Me.lblReceiptAccountType.AutoSize = True
        Me.lblReceiptAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptAccountType.Location = New System.Drawing.Point(16, 22)
        Me.lblReceiptAccountType.Name = "lblReceiptAccountType"
        Me.lblReceiptAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptAccountType.Size = New System.Drawing.Size(77, 13)
        Me.lblReceiptAccountType.TabIndex = 186
        Me.lblReceiptAccountType.Text = "Account Type:"
        '
        'lblCCCancelled
        '
        Me.lblCCCancelled.AutoSize = True
        Me.lblCCCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.lblCCCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCCCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCCCancelled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCCCancelled.Location = New System.Drawing.Point(486, 22)
        Me.lblCCCancelled.Name = "lblCCCancelled"
        Me.lblCCCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCCCancelled.Size = New System.Drawing.Size(57, 13)
        Me.lblCCCancelled.TabIndex = 176
        Me.lblCCCancelled.Text = "Cancelled:"
        '
        'lblCardType
        '
        Me.lblCardType.AutoSize = True
        Me.lblCardType.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardType.Location = New System.Drawing.Point(18, 126)
        Me.lblCardType.Name = "lblCardType"
        Me.lblCardType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardType.Size = New System.Drawing.Size(34, 13)
        Me.lblCardType.TabIndex = 175
        Me.lblCardType.Text = "Type:"
        '
        'lblPin
        '
        Me.lblPin.AutoSize = True
        Me.lblPin.BackColor = System.Drawing.SystemColors.Control
        Me.lblPin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPin.Location = New System.Drawing.Point(312, 100)
        Me.lblPin.Name = "lblPin"
        Me.lblPin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPin.Size = New System.Drawing.Size(28, 13)
        Me.lblPin.TabIndex = 174
        Me.lblPin.Text = "PIN:"
        '
        'lblCardStartDate
        '
        Me.lblCardStartDate.AutoSize = True
        Me.lblCardStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardStartDate.Location = New System.Drawing.Point(312, 74)
        Me.lblCardStartDate.Name = "lblCardStartDate"
        Me.lblCardStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardStartDate.Size = New System.Drawing.Size(58, 13)
        Me.lblCardStartDate.TabIndex = 173
        Me.lblCardStartDate.Text = "Start Date:"
        '
        'lblCardName
        '
        Me.lblCardName.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardName.Location = New System.Drawing.Point(312, 47)
        Me.lblCardName.Name = "lblCardName"
        Me.lblCardName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardName.Size = New System.Drawing.Size(65, 17)
        Me.lblCardName.TabIndex = 172
        Me.lblCardName.Text = "Name"
        '
        'lblCopyFromCard
        '
        Me.lblCopyFromCard.AutoSize = True
        Me.lblCopyFromCard.BackColor = System.Drawing.SystemColors.Control
        Me.lblCopyFromCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCopyFromCard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCopyFromCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCopyFromCard.Location = New System.Drawing.Point(312, 126)
        Me.lblCopyFromCard.Name = "lblCopyFromCard"
        Me.lblCopyFromCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCopyFromCard.Size = New System.Drawing.Size(60, 13)
        Me.lblCopyFromCard.TabIndex = 171
        Me.lblCopyFromCard.Text = "Copy From:"
        Me.lblCopyFromCard.Visible = False
        '
        'lblIssueNo
        '
        Me.lblIssueNo.AutoSize = True
        Me.lblIssueNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblIssueNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIssueNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIssueNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIssueNo.Location = New System.Drawing.Point(16, 97)
        Me.lblIssueNo.Name = "lblIssueNo"
        Me.lblIssueNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIssueNo.Size = New System.Drawing.Size(75, 13)
        Me.lblIssueNo.TabIndex = 170
        Me.lblIssueNo.Text = "Issue Number:"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(16, 71)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(64, 13)
        Me.lblExpiryDate.TabIndex = 169
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblCardNo
        '
        Me.lblCardNo.AutoSize = True
        Me.lblCardNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCardNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCardNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCardNo.Location = New System.Drawing.Point(16, 46)
        Me.lblCardNo.Name = "lblCardNo"
        Me.lblCardNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCardNo.Size = New System.Drawing.Size(72, 13)
        Me.lblCardNo.TabIndex = 168
        Me.lblCardNo.Text = "Card Number:"
        '
        '_SSTabCreditCard_TabPage1
        '
        Me._SSTabCreditCard_TabPage1.Controls.Add(Me.lvwCCHistory)
        Me._SSTabCreditCard_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTabCreditCard_TabPage1.Name = "_SSTabCreditCard_TabPage1"
        Me._SSTabCreditCard_TabPage1.Size = New System.Drawing.Size(613, 387)
        Me._SSTabCreditCard_TabPage1.TabIndex = 1
        Me._SSTabCreditCard_TabPage1.Text = "History"
        '
        'lvwCCHistory
        '
        Me.lvwCCHistory.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCCHistory, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCCHistory, False)
        Me.lvwCCHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCCHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCCHistory, "")
        Me.lvwCCHistory.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwCCHistory, "")
        Me.lvwCCHistory.Location = New System.Drawing.Point(8, 4)
        Me.lvwCCHistory.Name = "lvwCCHistory"
        Me.lvwCCHistory.Size = New System.Drawing.Size(569, 377)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCCHistory, "")
        Me.listViewHelper1.SetSorted(Me.lvwCCHistory, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCCHistory, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCCHistory, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCCHistory.TabIndex = 177
        Me.lvwCCHistory.UseCompatibleStateImageBehavior = False
        Me.lvwCCHistory.View = System.Windows.Forms.View.Details
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.txtSelectedTotal)
        Me._tabMainTab_TabPage4.Controls.Add(Me.btnReverseInstalment)
        Me._tabMainTab_TabPage4.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage4.Controls.Add(Me.lvwInstalment)
        Me._tabMainTab_TabPage4.Controls.Add(Me.grpInstalmentActions)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5-Instalments"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'grpInstalmentActions
        '
        Me.grpInstalmentActions.BackColor = System.Drawing.SystemColors.Control
        Me.grpInstalmentActions.Controls.Add(Me.btnReverse)
        Me.grpInstalmentActions.Controls.Add(Me.txtSelectedTotal)
        Me.grpInstalmentActions.Controls.Add(Me.lblSelectedTotal)
        Me.grpInstalmentActions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpInstalmentActions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpInstalmentActions.Location = New System.Drawing.Point(8, 369)
        Me.grpInstalmentActions.Name = "grpInstalmentActions"
        Me.grpInstalmentActions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.grpInstalmentActions.Size = New System.Drawing.Size(567, 58)
        Me.grpInstalmentActions.TabIndex = 51
        Me.grpInstalmentActions.TabStop = False
        Me.grpInstalmentActions.Text = "Instalment Actions"
        'txtSelectedTotal
        '
        Me.txtSelectedTotal.Enabled = False
        Me.txtSelectedTotal.Location = New System.Drawing.Point(345, 414)
        Me.txtSelectedTotal.Name = "txtSelectedTotal"
        Me.txtSelectedTotal.Size = New System.Drawing.Size(89, 21)
        Me.txtSelectedTotal.TabIndex = 49
        Me.txtSelectedTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnReverseInstalment
        '
        Me.btnReverseInstalment.Location = New System.Drawing.Point(440, 413)
        Me.btnReverseInstalment.Name = "btnReverseInstalment"
        Me.btnReverseInstalment.Size = New System.Drawing.Size(135, 22)
        Me.btnReverseInstalment.TabIndex = 48
        Me.btnReverseInstalment.Text = "Reverse Instalment"
        Me.btnReverseInstalment.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(241, 416)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 13)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "Selected Total"
        '
        '
        'lvwInstalment
        '
        Me.lvwInstalment.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwInstalment, "")
        Me.lvwInstalment.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwInstalment_ColumnHeader_1, Me._lvwInstalment_ColumnHeader_2, Me._lvwInstalment_ColumnHeader_3, Me._lvwInstalment_ColumnHeader_4, Me._lvwInstalment_ColumnHeader_5, Me._lvwInstalment_ColumnHeader_6, Me._lvwInstalment_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwInstalment, True)
        Me.lvwInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwInstalment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwInstalment.FullRowSelect = True
        Me.lvwInstalment.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwInstalment, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwInstalment, "")
        Me.lvwInstalment.Location = New System.Drawing.Point(8, 11)
        Me.lvwInstalment.MultiSelect = True
        Me.lvwInstalment.HideSelection = False
        'lvwInstalment.
        Me.lvwInstalment.Name = "lvwInstalment"
        Me.lvwInstalment.Size = New System.Drawing.Size(567, 352)
        Me.listViewHelper1.SetSmallIcons(Me.lvwInstalment, "")
        Me.listViewHelper1.SetSorted(Me.lvwInstalment, False)
        Me.listViewHelper1.SetSortKey(Me.lvwInstalment, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwInstalment, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwInstalment.TabIndex = 46
        Me.lvwInstalment.UseCompatibleStateImageBehavior = False
        Me.lvwInstalment.View = System.Windows.Forms.View.Details
        '
        '_lvwInstalment_ColumnHeader_1
        '
        Me._lvwInstalment_ColumnHeader_1.Text = "Number"
        Me._lvwInstalment_ColumnHeader_1.Width = 61
        '
        '_lvwInstalment_ColumnHeader_2
        '
        Me._lvwInstalment_ColumnHeader_2.Text = "Due Date"
        Me._lvwInstalment_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalment_ColumnHeader_2.Width = 101
        '
        '_lvwInstalment_ColumnHeader_3
        '
        Me._lvwInstalment_ColumnHeader_3.Text = "Date Paid"
        Me._lvwInstalment_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalment_ColumnHeader_3.Width = 101
        '
        '_lvwInstalment_ColumnHeader_4
        '
        Me._lvwInstalment_ColumnHeader_4.Text = "Amount"
        Me._lvwInstalment_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwInstalment_ColumnHeader_4.Width = 61
        '
        '_lvwInstalment_ColumnHeader_5
        '
        Me._lvwInstalment_ColumnHeader_5.Text = "Status"
        Me._lvwInstalment_ColumnHeader_5.Width = 121
        '
        '_lvwInstalment_ColumnHeader_6
        '
        Me._lvwInstalment_ColumnHeader_6.Text = "Reason"
        Me._lvwInstalment_ColumnHeader_6.Width = 118
        '
        '_lvwInstalment_ColumnHeader_7
        '
        Me._lvwInstalment_ColumnHeader_7.Text = "Writeoff Reason Description"
        Me._lvwInstalment_ColumnHeader_7.Width = 156
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraAgentDetails)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6-Agent Details"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraAgentDetails
        '
        Me.fraAgentDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgentDetails.Controls.Add(Me.uctPMAgentAddressControl)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentFaxExtension)
        Me.fraAgentDetails.Controls.Add(Me.txtAgent)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentRef)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentFaxAreaCode)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentFaxNumber)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentTelAreaCode)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentTelNumber)
        Me.fraAgentDetails.Controls.Add(Me.txtAgentTelExtension)
        Me.fraAgentDetails.Controls.Add(Me.cmdAgentSelect)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentRef)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentFaxNo)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentTelNo)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentAreaCode)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentNumber)
        Me.fraAgentDetails.Controls.Add(Me.lblAgentExtension)
        Me.fraAgentDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgentDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgentDetails.Location = New System.Drawing.Point(8, 11)
        Me.fraAgentDetails.Name = "fraAgentDetails"
        Me.fraAgentDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgentDetails.Size = New System.Drawing.Size(667, 345)
        Me.fraAgentDetails.TabIndex = 30
        Me.fraAgentDetails.TabStop = False
        Me.fraAgentDetails.Text = "Agent Information"
        '
        'uctPMAgentAddressControl
        '
        Me.uctPMAgentAddressControl.AddressLine1 = ""
        Me.uctPMAgentAddressControl.AddressLine2 = ""
        Me.uctPMAgentAddressControl.AddressLine3 = ""
        Me.uctPMAgentAddressControl.AddressLine4 = ""
        Me.uctPMAgentAddressControl.Caption = ""
        Me.uctPMAgentAddressControl.CaptionAddress1 = "No. && street name:"
        Me.uctPMAgentAddressControl.CaptionAddress2 = "Locality:"
        Me.uctPMAgentAddressControl.CaptionAddress3 = "Town:"
        Me.uctPMAgentAddressControl.CaptionAddress4 = "County:"
        Me.uctPMAgentAddressControl.CaptionCountry = "Country:"
        Me.uctPMAgentAddressControl.CaptionFontBoldAddress1 = False
        Me.uctPMAgentAddressControl.CaptionFontBoldPostCode = False
        Me.uctPMAgentAddressControl.CaptionPostCode = "Postcode:"
        Me.uctPMAgentAddressControl.ClearButtonCaption = "X"
        Me.uctPMAgentAddressControl.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAgentAddressControl.ClearButtonLeft = 4695
        Me.uctPMAgentAddressControl.ClearButtonWidth = 360
        Me.uctPMAgentAddressControl.CountryId = 2
        Me.uctPMAgentAddressControl.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAgentAddressControl.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAgentAddressControl.IsCountryRequired = 1
        Me.uctPMAgentAddressControl.IsPostCodeRequired = 1
        Me.uctPMAgentAddressControl.Location = New System.Drawing.Point(8, 72)
        Me.uctPMAgentAddressControl.Name = "uctPMAgentAddressControl"
        Me.uctPMAgentAddressControl.Organisation = ""
        Me.uctPMAgentAddressControl.PMAddressCnt = 0
        Me.uctPMAgentAddressControl.PMDatabaseID = 0
        Me.uctPMAgentAddressControl.PostCode = ""
        Me.uctPMAgentAddressControl.QAS2PMAddress1 = "3,4,2,5,6"
        Me.uctPMAgentAddressControl.QAS2PMAddress2 = "8, 7"
        Me.uctPMAgentAddressControl.QAS2PMAddress3 = "9"
        Me.uctPMAgentAddressControl.QAS2PMAddress4 = ""
        Me.uctPMAgentAddressControl.QASDatabaseID = 0
        Me.uctPMAgentAddressControl.SearchButtonCaption = ".."
        Me.uctPMAgentAddressControl.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAgentAddressControl.SearchButtonHeight = 285
        Me.uctPMAgentAddressControl.SearchButtonLeft = 4305
        Me.uctPMAgentAddressControl.SearchButtonTop = 1530
        Me.uctPMAgentAddressControl.SearchButtonWidth = 360
        Me.uctPMAgentAddressControl.Size = New System.Drawing.Size(561, 152)
        Me.uctPMAgentAddressControl.TabIndex = 119
        Me.uctPMAgentAddressControl.WarningMessage = ""
        '
        'txtAgentFaxExtension
        '
        Me.txtAgentFaxExtension.AcceptsReturn = True
        Me.txtAgentFaxExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentFaxExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentFaxExtension.Enabled = False
        Me.txtAgentFaxExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentFaxExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentFaxExtension.Location = New System.Drawing.Point(416, 276)
        Me.txtAgentFaxExtension.MaxLength = 10
        Me.txtAgentFaxExtension.Name = "txtAgentFaxExtension"
        Me.txtAgentFaxExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentFaxExtension.Size = New System.Drawing.Size(89, 20)
        Me.txtAgentFaxExtension.TabIndex = 45
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.txtAgent.Enabled = False
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(120, 40)
        Me.txtAgent.MaxLength = 30
        Me.txtAgent.Multiline = True
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(433, 19)
        Me.txtAgent.TabIndex = 34
        '
        'txtAgentRef
        '
        Me.txtAgentRef.AcceptsReturn = True
        Me.txtAgentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentRef.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.txtAgentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentRef.Location = New System.Drawing.Point(120, 16)
        Me.txtAgentRef.MaxLength = 30
        Me.txtAgentRef.Multiline = True
        Me.txtAgentRef.Name = "txtAgentRef"
        Me.txtAgentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentRef.Size = New System.Drawing.Size(433, 19)
        Me.txtAgentRef.TabIndex = 32
        '
        'txtAgentFaxAreaCode
        '
        Me.txtAgentFaxAreaCode.AcceptsReturn = True
        Me.txtAgentFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentFaxAreaCode.Enabled = False
        Me.txtAgentFaxAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentFaxAreaCode.Location = New System.Drawing.Point(140, 276)
        Me.txtAgentFaxAreaCode.MaxLength = 10
        Me.txtAgentFaxAreaCode.Name = "txtAgentFaxAreaCode"
        Me.txtAgentFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentFaxAreaCode.Size = New System.Drawing.Size(73, 20)
        Me.txtAgentFaxAreaCode.TabIndex = 43
        '
        'txtAgentFaxNumber
        '
        Me.txtAgentFaxNumber.AcceptsReturn = True
        Me.txtAgentFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentFaxNumber.Enabled = False
        Me.txtAgentFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentFaxNumber.Location = New System.Drawing.Point(236, 276)
        Me.txtAgentFaxNumber.MaxLength = 10
        Me.txtAgentFaxNumber.Name = "txtAgentFaxNumber"
        Me.txtAgentFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentFaxNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtAgentFaxNumber.TabIndex = 44
        '
        'txtAgentTelAreaCode
        '
        Me.txtAgentTelAreaCode.AcceptsReturn = True
        Me.txtAgentTelAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentTelAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentTelAreaCode.Enabled = False
        Me.txtAgentTelAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentTelAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentTelAreaCode.Location = New System.Drawing.Point(140, 252)
        Me.txtAgentTelAreaCode.MaxLength = 10
        Me.txtAgentTelAreaCode.Name = "txtAgentTelAreaCode"
        Me.txtAgentTelAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentTelAreaCode.Size = New System.Drawing.Size(73, 20)
        Me.txtAgentTelAreaCode.TabIndex = 39
        '
        'txtAgentTelNumber
        '
        Me.txtAgentTelNumber.AcceptsReturn = True
        Me.txtAgentTelNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentTelNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentTelNumber.Enabled = False
        Me.txtAgentTelNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentTelNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentTelNumber.Location = New System.Drawing.Point(236, 252)
        Me.txtAgentTelNumber.MaxLength = 10
        Me.txtAgentTelNumber.Name = "txtAgentTelNumber"
        Me.txtAgentTelNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentTelNumber.Size = New System.Drawing.Size(157, 20)
        Me.txtAgentTelNumber.TabIndex = 40
        '
        'txtAgentTelExtension
        '
        Me.txtAgentTelExtension.AcceptsReturn = True
        Me.txtAgentTelExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentTelExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentTelExtension.Enabled = False
        Me.txtAgentTelExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentTelExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentTelExtension.Location = New System.Drawing.Point(416, 252)
        Me.txtAgentTelExtension.MaxLength = 10
        Me.txtAgentTelExtension.Name = "txtAgentTelExtension"
        Me.txtAgentTelExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentTelExtension.Size = New System.Drawing.Size(89, 20)
        Me.txtAgentTelExtension.TabIndex = 41
        '
        'cmdAgentSelect
        '
        Me.cmdAgentSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentSelect.Location = New System.Drawing.Point(24, 39)
        Me.cmdAgentSelect.Name = "cmdAgentSelect"
        Me.cmdAgentSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentSelect.Size = New System.Drawing.Size(73, 22)
        Me.cmdAgentSelect.TabIndex = 33
        Me.cmdAgentSelect.Text = "Agent..."
        Me.cmdAgentSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentSelect.UseVisualStyleBackColor = False
        '
        'lblAgentRef
        '
        Me.lblAgentRef.AutoSize = True
        Me.lblAgentRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentRef.Location = New System.Drawing.Point(24, 19)
        Me.lblAgentRef.Name = "lblAgentRef"
        Me.lblAgentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentRef.Size = New System.Drawing.Size(58, 13)
        Me.lblAgentRef.TabIndex = 31
        Me.lblAgentRef.Text = "Agent Ref:"
        '
        'lblAgentFaxNo
        '
        Me.lblAgentFaxNo.AutoSize = True
        Me.lblAgentFaxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentFaxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentFaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentFaxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentFaxNo.Location = New System.Drawing.Point(24, 279)
        Me.lblAgentFaxNo.Name = "lblAgentFaxNo"
        Me.lblAgentFaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentFaxNo.Size = New System.Drawing.Size(44, 13)
        Me.lblAgentFaxNo.TabIndex = 42
        Me.lblAgentFaxNo.Text = "Fax No:"
        '
        'lblAgentTelNo
        '
        Me.lblAgentTelNo.AutoSize = True
        Me.lblAgentTelNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentTelNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentTelNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentTelNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentTelNo.Location = New System.Drawing.Point(24, 255)
        Me.lblAgentTelNo.Name = "lblAgentTelNo"
        Me.lblAgentTelNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentTelNo.Size = New System.Drawing.Size(78, 13)
        Me.lblAgentTelNo.TabIndex = 38
        Me.lblAgentTelNo.Text = "Telephone No:"
        '
        'lblAgentAreaCode
        '
        Me.lblAgentAreaCode.AutoSize = True
        Me.lblAgentAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentAreaCode.Location = New System.Drawing.Point(140, 236)
        Me.lblAgentAreaCode.Name = "lblAgentAreaCode"
        Me.lblAgentAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentAreaCode.Size = New System.Drawing.Size(57, 13)
        Me.lblAgentAreaCode.TabIndex = 35
        Me.lblAgentAreaCode.Text = "Area Code"
        Me.lblAgentAreaCode.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAgentNumber
        '
        Me.lblAgentNumber.AutoSize = True
        Me.lblAgentNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentNumber.Location = New System.Drawing.Point(236, 236)
        Me.lblAgentNumber.Name = "lblAgentNumber"
        Me.lblAgentNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentNumber.Size = New System.Drawing.Size(44, 13)
        Me.lblAgentNumber.TabIndex = 36
        Me.lblAgentNumber.Text = "Number"
        Me.lblAgentNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAgentExtension
        '
        Me.lblAgentExtension.AutoSize = True
        Me.lblAgentExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentExtension.Location = New System.Drawing.Point(416, 236)
        Me.lblAgentExtension.Name = "lblAgentExtension"
        Me.lblAgentExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentExtension.Size = New System.Drawing.Size(53, 13)
        Me.lblAgentExtension.TabIndex = 37
        Me.lblAgentExtension.Text = "Extension"
        Me.lblAgentExtension.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.lvwPolicyList)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(678, 469)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "7-Policy List"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'lvwPolicyList
        '
        Me.lvwPolicyList.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicyList, "")
        Me.lvwPolicyList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyList_ColumnHeader_1, Me._lvwPolicyList_ColumnHeader_2, Me._lvwPolicyList_ColumnHeader_3, Me._lvwPolicyList_ColumnHeader_4, Me._lvwPolicyList_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicyList, False)
        Me.lvwPolicyList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicyList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicyList, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicyList, "")
        Me.lvwPolicyList.Location = New System.Drawing.Point(3, 14)
        Me.lvwPolicyList.Name = "lvwPolicyList"
        Me.lvwPolicyList.Size = New System.Drawing.Size(568, 392)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicyList, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicyList, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicyList, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicyList, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicyList.TabIndex = 184
        Me.lvwPolicyList.UseCompatibleStateImageBehavior = False
        Me.lvwPolicyList.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicyList_ColumnHeader_1
        '
        Me._lvwPolicyList_ColumnHeader_1.Text = "Policy Number"
        Me._lvwPolicyList_ColumnHeader_1.Width = 134
        '
        '_lvwPolicyList_ColumnHeader_2
        '
        Me._lvwPolicyList_ColumnHeader_2.Text = "Insured Name"
        Me._lvwPolicyList_ColumnHeader_2.Width = 147
        '
        '_lvwPolicyList_ColumnHeader_3
        '
        Me._lvwPolicyList_ColumnHeader_3.Text = "Total Premium"
        Me._lvwPolicyList_ColumnHeader_3.Width = 147
        '
        '_lvwPolicyList_ColumnHeader_4
        '
        Me._lvwPolicyList_ColumnHeader_4.Text = "Inception Date"
        Me._lvwPolicyList_ColumnHeader_4.Width = 178
        '
        '_lvwPolicyList_ColumnHeader_5
        '
        Me._lvwPolicyList_ColumnHeader_5.Text = "Cancel Amount"
        Me._lvwPolicyList_ColumnHeader_5.Width = 178

        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(480, 509)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 102
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdTransact
        '
        Me.cmdTransact.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransact.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransact.Location = New System.Drawing.Point(480, 509)
        Me.cmdTransact.Name = "cmdTransact"
        Me.cmdTransact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransact.Size = New System.Drawing.Size(73, 22)
        Me.cmdTransact.TabIndex = 103
        Me.cmdTransact.TabStop = False
        Me.cmdTransact.Text = "&Transact"
        Me.cmdTransact.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTransact.UseVisualStyleBackColor = False
        '
        'cmdSettlePlan
        '
        Me.cmdSettlePlan.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSettlePlan.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSettlePlan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSettlePlan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSettlePlan.Location = New System.Drawing.Point(88, 509)
        Me.cmdSettlePlan.Name = "cmdSettlePlan"
        Me.cmdSettlePlan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSettlePlan.Size = New System.Drawing.Size(73, 22)
        Me.cmdSettlePlan.TabIndex = 98
        Me.cmdSettlePlan.Text = "&Settle"
        Me.cmdSettlePlan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSettlePlan.UseVisualStyleBackColor = False
        '
        'cmdRelease
        '
        Me.cmdRelease.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRelease.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRelease.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRelease.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelease.Location = New System.Drawing.Point(480, 509)
        Me.cmdRelease.Name = "cmdRelease"
        Me.cmdRelease.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelease.Size = New System.Drawing.Size(73, 22)
        Me.cmdRelease.TabIndex = 105
        Me.cmdRelease.Text = "&Release"
        Me.cmdRelease.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelease.UseVisualStyleBackColor = False
        '
        'cmdReSend
        '
        Me.cmdReSend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReSend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReSend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReSend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReSend.Location = New System.Drawing.Point(88, 509)
        Me.cmdReSend.Name = "cmdReSend"
        Me.cmdReSend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReSend.Size = New System.Drawing.Size(73, 22)
        Me.cmdReSend.TabIndex = 99
        Me.cmdReSend.Text = "&Resend"
        Me.cmdReSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReSend.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(88, 509)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 97
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(706, 536)
        Me.Controls.Add(Me.cmdCancelPolicy)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdMTA)
        Me.Controls.Add(Me.cmdRePrint)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdTransact)
        Me.Controls.Add(Me.cmdSettlePlan)
        Me.Controls.Add(Me.cmdRelease)
        Me.Controls.Add(Me.cmdReSend)
        Me.Controls.Add(Me.cmdDelete)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(157, 146)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Premium Finance Maintenance"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraPayment.ResumeLayout(False)
        Me.fraPayment.PerformLayout()
        Me.fraDates.ResumeLayout(False)
        Me.fraDates.PerformLayout()
        Me.fraSummary.ResumeLayout(False)
        Me.fraSummary.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraClient.ResumeLayout(False)
        Me.fraClient.PerformLayout()
        Me.frmPartners.ResumeLayout(False)
        Me.framPhone.ResumeLayout(False)
        Me.framPhone.PerformLayout()
        Me.framAdditional.ResumeLayout(False)
        Me.framAdditional.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraBank.ResumeLayout(False)
        Me.fraBank.PerformLayout()
        Me.fraAccount.ResumeLayout(False)
        Me.fraAccount.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.SSTabCreditCard.ResumeLayout(False)
        Me._SSTabCreditCard_TabPage0.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._SSTabCreditCard_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.grpInstalmentActions.ResumeLayout(False)
        Me.grpInstalmentActions.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraAgentDetails.ResumeLayout(False)
        Me.fraAgentDetails.PerformLayout()
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents txtIBAN As System.Windows.Forms.TextBox
    Public WithEvents lblIBAN As System.Windows.Forms.Label
    Public WithEvents txtBIC As System.Windows.Forms.TextBox
    Public WithEvents lblBIC As System.Windows.Forms.Label
    Friend WithEvents btnReverseInstalment As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
#End Region
End Class
