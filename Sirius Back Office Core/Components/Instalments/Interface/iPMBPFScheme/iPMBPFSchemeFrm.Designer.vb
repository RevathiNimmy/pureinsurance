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
        lvRates_InitializeColumnKeys()
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
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents chkDepositAsInstalment As System.Windows.Forms.CheckBox
    Public WithEvents chkPlanRefEditable As System.Windows.Forms.CheckBox
    Public WithEvents cboBranch As PMLookupControl.cboPMLookup
    Public WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Public WithEvents txtSchemeDescription As System.Windows.Forms.TextBox
    Public WithEvents txtSchemeName As System.Windows.Forms.TextBox
    Public WithEvents txtSchemeNo As System.Windows.Forms.TextBox
    Public WithEvents txtSchemeVersion As System.Windows.Forms.TextBox
    Public WithEvents txtStartDate As System.Windows.Forms.TextBox
    Public WithEvents txtEndDate As System.Windows.Forms.TextBox
    Public WithEvents cboCurrency As PMLookupControl.cboPMLookup
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblSchemeDescription As System.Windows.Forms.Label
    Public WithEvents lblSchemeName As System.Windows.Forms.Label
    Public WithEvents lblSchemeNo As System.Windows.Forms.Label
    Public WithEvents lblSchemeVersion As System.Windows.Forms.Label
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents lblEndDate As System.Windows.Forms.Label
    Public WithEvents fraScheme As System.Windows.Forms.GroupBox
    Public WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Public WithEvents cmdRelatedPartyFind As System.Windows.Forms.Button
    Public WithEvents lblPartyCode As System.Windows.Forms.Label
    Public WithEvents framFP As System.Windows.Forms.GroupBox
    Public WithEvents cboSchemeType As PMLookupControl.cboPMLookup
    Public WithEvents lblSchemeType As System.Windows.Forms.Label
    Public WithEvents Frame4 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents chkRatesForInformationOnly As System.Windows.Forms.CheckBox
    Public WithEvents chkAllowClientFees As System.Windows.Forms.CheckBox
    Public WithEvents cboDifference As System.Windows.Forms.ComboBox
    Public WithEvents txtPFMessage As System.Windows.Forms.TextBox
    Public WithEvents chkDepositOnOtherMediaType As System.Windows.Forms.CheckBox
    Public WithEvents chkSpreadTaxes As System.Windows.Forms.CheckBox
    Public WithEvents chkSpreadRI As System.Windows.Forms.CheckBox
    Public WithEvents chkSpread As System.Windows.Forms.CheckBox
    Public WithEvents cboExportMethod As System.Windows.Forms.ComboBox
    Public WithEvents cboMediaType As PMLookupControl.cboPMLookup
    Public WithEvents lblPFMessage As System.Windows.Forms.Label
    Public WithEvents lblExportMethod As System.Windows.Forms.Label
    Public WithEvents lblMediaType As System.Windows.Forms.Label
    Public WithEvents lblDifference As System.Windows.Forms.Label
    Public WithEvents SSFrame4 As System.Windows.Forms.GroupBox
    Public WithEvents cmdCollectionNotificationClear As System.Windows.Forms.Button
    Public WithEvents txtCollectionNotification As System.Windows.Forms.TextBox
    Public WithEvents cmdCollectionNotification As System.Windows.Forms.Button
    Public WithEvents cmdConfirmationDocClear As System.Windows.Forms.Button
    Public WithEvents cmdCreditClear As System.Windows.Forms.Button
    Public WithEvents cmdBankClear As System.Windows.Forms.Button
    Public WithEvents cmdQuoteClear As System.Windows.Forms.Button
    Public WithEvents cmdConfirmationDoc As System.Windows.Forms.Button
    Public WithEvents cmdCredit As System.Windows.Forms.Button
    Public WithEvents cmbBank As System.Windows.Forms.Button
    Public WithEvents cmdQuote As System.Windows.Forms.Button
    Public WithEvents txtConfirmationDoc As System.Windows.Forms.TextBox
    Public WithEvents txtcredit As System.Windows.Forms.TextBox
    Public WithEvents txtBank As System.Windows.Forms.TextBox
    Public WithEvents txtQuote As System.Windows.Forms.TextBox
    Public WithEvents cboPrintType As PMLookupControl.cboPMLookup
    Public WithEvents lblPrintType As System.Windows.Forms.Label
    Public WithEvents SSFrame1 As System.Windows.Forms.GroupBox
    Public WithEvents chkBusinessCodeMandatory As System.Windows.Forms.CheckBox
    Public WithEvents chkAgentRefMandatory As System.Windows.Forms.CheckBox
    Public WithEvents chkBankAddress As System.Windows.Forms.CheckBox
    Public WithEvents chkBranchName As System.Windows.Forms.CheckBox
    Public WithEvents chkBranchCode As System.Windows.Forms.CheckBox
    Public WithEvents chkBankName As System.Windows.Forms.CheckBox
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Public WithEvents txtNotificationDays As System.Windows.Forms.TextBox
    Public WithEvents lblNotificationDays As System.Windows.Forms.Label
    Public WithEvents fraCollectionNotification As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cboTransactionType As System.Windows.Forms.ComboBox
    Public WithEvents lblTransactionType As System.Windows.Forms.Label
    Private WithEvents _lvRates_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvRates_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvRates_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvRates_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvRates_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvRates_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvRates As System.Windows.Forms.ListView
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents actAdmin As UserControls.AccountLookup
    Public WithEvents lblAdmin As System.Windows.Forms.Label
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Public WithEvents actInterest As UserControls.AccountLookup
    Public WithEvents actProtection As UserControls.AccountLookup
    Public WithEvents lblProtection As System.Windows.Forms.Label
    Public WithEvents lblInterest As System.Windows.Forms.Label
    Public WithEvents Frame6 As System.Windows.Forms.GroupBox
    Public WithEvents actCommissionSuspense As UserControls.AccountLookup
    Public WithEvents actSuspense As UserControls.AccountLookup
    Public WithEvents actReInsuranceSuspense As UserControls.AccountLookup
    Public WithEvents lblReInsuranceSuspense As System.Windows.Forms.Label
    Public WithEvents lblSuspense As System.Windows.Forms.Label
    Public WithEvents lblCommissionSuspense As System.Windows.Forms.Label
    Public WithEvents fmeSuspense As System.Windows.Forms.GroupBox
    Public WithEvents cboTaxGroupID As PMLookupControl.cboPMLookup
    Public WithEvents actTaxSuspense As UserControls.AccountLookup
    Public WithEvents lblTaxSuspense As System.Windows.Forms.Label
    Public WithEvents lblTaxGroup As System.Windows.Forms.Label
    Public WithEvents frameTax As System.Windows.Forms.GroupBox
    Public WithEvents cboBankAccount As UserControls.BankAccount
    Public WithEvents lblBankAccount As System.Windows.Forms.Label
    Public WithEvents Frame9 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents txtEDIMessageCount As System.Windows.Forms.TextBox
    Public WithEvents txtInsrMailboxNo As System.Windows.Forms.TextBox
    Public WithEvents lblEDIMessageCount As System.Windows.Forms.Label
    Public WithEvents lblInsrMailboxNo As System.Windows.Forms.Label
    Public WithEvents framEDI As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Public WithEvents txtProviderWebsite As System.Windows.Forms.TextBox
    Public WithEvents txtProviderUsername As System.Windows.Forms.TextBox
    Public WithEvents txtProviderPassword As System.Windows.Forms.TextBox
    Public WithEvents txtProviderTimeout As System.Windows.Forms.TextBox
    Public WithEvents txtProviderBrokerID As System.Windows.Forms.TextBox
    Public WithEvents lblProviderWebsite As System.Windows.Forms.Label
    Public WithEvents lblProviderUsername As System.Windows.Forms.Label
    Public WithEvents lblProviderPassword As System.Windows.Forms.Label
    Public WithEvents lblProviderTimeout As System.Windows.Forms.Label
    Public WithEvents lblProviderBrokerID As System.Windows.Forms.Label
    Public WithEvents framXML As System.Windows.Forms.GroupBox
    Public WithEvents txtProcessingDays As System.Windows.Forms.TextBox
    Public WithEvents txtRemitter As System.Windows.Forms.TextBox
    Public WithEvents txtDirectDebitSupplierID As System.Windows.Forms.TextBox
    Public WithEvents txtDirectDebitSupplierName As System.Windows.Forms.TextBox
    Public WithEvents txtFinancialInstitutionCode As System.Windows.Forms.TextBox
    Public WithEvents lblProcessingDays As System.Windows.Forms.Label
    Public WithEvents lblRemitter As System.Windows.Forms.Label
    Public WithEvents lblDirectDebitSupplierID As System.Windows.Forms.Label
    Public WithEvents lblDirectDebitSupplierName As System.Windows.Forms.Label
    Public WithEvents lblFinancialInstitutionCode As System.Windows.Forms.Label
    Public WithEvents fraXMLExport As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents uctPickListBranches As uctPickList.PickList
    Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents uctPickListProducts As uctPickList.PickList
    Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public cmdNext(5) As System.Windows.Forms.Button
    Public cmdPrevious(5) As System.Windows.Forms.Button
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraScheme = New System.Windows.Forms.GroupBox()
        Me.chkDepositAsInstalment = New System.Windows.Forms.CheckBox()
        Me.chkPlanRefEditable = New System.Windows.Forms.CheckBox()
        Me.cboBranch = New PMLookupControl.cboPMLookup()
        Me.chkEnabled = New System.Windows.Forms.CheckBox()
        Me.txtSchemeDescription = New System.Windows.Forms.TextBox()
        Me.txtSchemeName = New System.Windows.Forms.TextBox()
        Me.txtSchemeNo = New System.Windows.Forms.TextBox()
        Me.txtSchemeVersion = New System.Windows.Forms.TextBox()
        Me.txtStartDate = New System.Windows.Forms.TextBox()
        Me.txtEndDate = New System.Windows.Forms.TextBox()
        Me.cboCurrency = New PMLookupControl.cboPMLookup()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.lblSchemeDescription = New System.Windows.Forms.Label()
        Me.lblSchemeName = New System.Windows.Forms.Label()
        Me.lblSchemeNo = New System.Windows.Forms.Label()
        Me.lblSchemeVersion = New System.Windows.Forms.Label()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.framFP = New System.Windows.Forms.GroupBox()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.cmdRelatedPartyFind = New System.Windows.Forms.Button()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.Frame4 = New System.Windows.Forms.GroupBox()
        Me.cboSchemeType = New PMLookupControl.cboPMLookup()
        Me.lblSchemeType = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.SSFrame4 = New System.Windows.Forms.GroupBox()
        Me.chkRatesForInformationOnly = New System.Windows.Forms.CheckBox()
        Me.chkAllowClientFees = New System.Windows.Forms.CheckBox()
        Me.cboDifference = New System.Windows.Forms.ComboBox()
        Me.chkDepositOnOtherMediaType = New System.Windows.Forms.CheckBox()
        Me.cboExportMethod = New System.Windows.Forms.ComboBox()
        Me.cboMediaType = New PMLookupControl.cboPMLookup()
        Me.lblExportMethod = New System.Windows.Forms.Label()
        Me.lblMediaType = New System.Windows.Forms.Label()
        Me.lblDifference = New System.Windows.Forms.Label()
        Me.txtPFMessage = New System.Windows.Forms.TextBox()
        Me.lblPFMessage = New System.Windows.Forms.Label()
        Me.chkSpread = New System.Windows.Forms.CheckBox()
        Me.chkSpreadRI = New System.Windows.Forms.CheckBox()
        Me.chkSpreadTaxes = New System.Windows.Forms.CheckBox()
        Me.chkSubAgentSpread = New System.Windows.Forms.CheckBox()
        Me.SSFrame1 = New System.Windows.Forms.GroupBox()
        Me.cmdCollectionNotificationClear = New System.Windows.Forms.Button()
        Me.txtCollectionNotification = New System.Windows.Forms.TextBox()
        Me.cmdCollectionNotification = New System.Windows.Forms.Button()
        Me.cmdConfirmationDocClear = New System.Windows.Forms.Button()
        Me.cmdCreditClear = New System.Windows.Forms.Button()
        Me.cmdBankClear = New System.Windows.Forms.Button()
        Me.cmdQuoteClear = New System.Windows.Forms.Button()
        Me.cmdConfirmationDoc = New System.Windows.Forms.Button()
        Me.cmdCredit = New System.Windows.Forms.Button()
        Me.cmbBank = New System.Windows.Forms.Button()
        Me.cmdQuote = New System.Windows.Forms.Button()
        Me.txtConfirmationDoc = New System.Windows.Forms.TextBox()
        Me.txtcredit = New System.Windows.Forms.TextBox()
        Me.txtBank = New System.Windows.Forms.TextBox()
        Me.txtQuote = New System.Windows.Forms.TextBox()
        Me.cboPrintType = New PMLookupControl.cboPMLookup()
        Me.lblPrintType = New System.Windows.Forms.Label()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.chkBusinessCodeMandatory = New System.Windows.Forms.CheckBox()
        Me.chkAgentRefMandatory = New System.Windows.Forms.CheckBox()
        Me.chkBankAddress = New System.Windows.Forms.CheckBox()
        Me.chkBranchName = New System.Windows.Forms.CheckBox()
        Me.chkBranchCode = New System.Windows.Forms.CheckBox()
        Me.chkBankName = New System.Windows.Forms.CheckBox()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me.fraCollectionNotification = New System.Windows.Forms.GroupBox()
        Me.txtNotificationDays = New System.Windows.Forms.TextBox()
        Me.lblNotificationDays = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cboTransactionType = New System.Windows.Forms.ComboBox()
        Me.lblTransactionType = New System.Windows.Forms.Label()
        Me.lvRates = New System.Windows.Forms.ListView()
        Me._lvRates_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvRates_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvRates_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvRates_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvRates_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvRates_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.Frame5 = New System.Windows.Forms.GroupBox()
        Me.actAdmin = New UserControls.AccountLookup()
        Me.lblAdmin = New System.Windows.Forms.Label()
        Me.Frame6 = New System.Windows.Forms.GroupBox()
        Me.actInterest = New UserControls.AccountLookup()
        Me.actProtection = New UserControls.AccountLookup()
        Me.lblProtection = New System.Windows.Forms.Label()
        Me.lblInterest = New System.Windows.Forms.Label()
        Me.fmeSuspense = New System.Windows.Forms.GroupBox()
        Me.actSubAgentCommissionSuspense = New UserControls.AccountLookup()
        Me.lblSubAgentCommissionSuspense = New System.Windows.Forms.Label()
        Me.actCommissionSuspense = New UserControls.AccountLookup()
        Me.actSuspense = New UserControls.AccountLookup()
        Me.actReInsuranceSuspense = New UserControls.AccountLookup()
        Me.lblReInsuranceSuspense = New System.Windows.Forms.Label()
        Me.lblSuspense = New System.Windows.Forms.Label()
        Me.lblCommissionSuspense = New System.Windows.Forms.Label()
        Me.frameTax = New System.Windows.Forms.GroupBox()
        Me.cboTaxGroupID = New PMLookupControl.cboPMLookup()
        Me.actTaxSuspense = New UserControls.AccountLookup()
        Me.lblTaxSuspense = New System.Windows.Forms.Label()
        Me.lblTaxGroup = New System.Windows.Forms.Label()
        Me.Frame9 = New System.Windows.Forms.GroupBox()
        Me.cboBankAccount = New UserControls.BankAccount()
        Me.lblBankAccount = New System.Windows.Forms.Label()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me.framEDI = New System.Windows.Forms.GroupBox()
        Me.txtEDIMessageCount = New System.Windows.Forms.TextBox()
        Me.txtInsrMailboxNo = New System.Windows.Forms.TextBox()
        Me.lblEDIMessageCount = New System.Windows.Forms.Label()
        Me.lblInsrMailboxNo = New System.Windows.Forms.Label()
        Me._cmdNext_4 = New System.Windows.Forms.Button()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
        Me.framXML = New System.Windows.Forms.GroupBox()
        Me.txtProviderWebsite = New System.Windows.Forms.TextBox()
        Me.txtProviderUsername = New System.Windows.Forms.TextBox()
        Me.txtProviderPassword = New System.Windows.Forms.TextBox()
        Me.txtProviderTimeout = New System.Windows.Forms.TextBox()
        Me.txtProviderBrokerID = New System.Windows.Forms.TextBox()
        Me.lblProviderWebsite = New System.Windows.Forms.Label()
        Me.lblProviderUsername = New System.Windows.Forms.Label()
        Me.lblProviderPassword = New System.Windows.Forms.Label()
        Me.lblProviderTimeout = New System.Windows.Forms.Label()
        Me.lblProviderBrokerID = New System.Windows.Forms.Label()
        Me.fraXMLExport = New System.Windows.Forms.GroupBox()
        Me.txtProcessingDays = New System.Windows.Forms.TextBox()
        Me.txtRemitter = New System.Windows.Forms.TextBox()
        Me.txtDirectDebitSupplierID = New System.Windows.Forms.TextBox()
        Me.txtDirectDebitSupplierName = New System.Windows.Forms.TextBox()
        Me.txtFinancialInstitutionCode = New System.Windows.Forms.TextBox()
        Me.lblProcessingDays = New System.Windows.Forms.Label()
        Me.lblRemitter = New System.Windows.Forms.Label()
        Me.lblDirectDebitSupplierID = New System.Windows.Forms.Label()
        Me.lblDirectDebitSupplierName = New System.Windows.Forms.Label()
        Me.lblFinancialInstitutionCode = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage()
        Me.uctPickListBranches = New uctPickList.PickList()
        Me._cmdNext_5 = New System.Windows.Forms.Button()
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage()
        Me.uctPickListProducts = New uctPickList.PickList()
        Me._cmdPrevious_5 = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout
        Me._tabMainTab_TabPage0.SuspendLayout
        Me.fraScheme.SuspendLayout
        Me.framFP.SuspendLayout
        Me.Frame4.SuspendLayout
        Me._tabMainTab_TabPage1.SuspendLayout
        Me.SSFrame4.SuspendLayout
        Me.SSFrame1.SuspendLayout
        Me.Frame1.SuspendLayout
        Me.fraCollectionNotification.SuspendLayout
        Me._tabMainTab_TabPage2.SuspendLayout
        Me._tabMainTab_TabPage3.SuspendLayout
        Me.Frame5.SuspendLayout
        Me.Frame6.SuspendLayout
        Me.fmeSuspense.SuspendLayout
        Me.frameTax.SuspendLayout
        Me.Frame9.SuspendLayout
        Me._tabMainTab_TabPage4.SuspendLayout
        Me.framEDI.SuspendLayout
        Me.framXML.SuspendLayout
        Me.fraXMLExport.SuspendLayout
        Me._tabMainTab_TabPage5.SuspendLayout
        Me._tabMainTab_TabPage6.SuspendLayout
        CType(Me.listBoxComboBoxHelper1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 512)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 130
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = false
        Me.cmdNavigate.Visible = false
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(638, 512)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 133
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = false
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(558, 512)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 132
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = false
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(478, 512)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 131
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = false
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(99, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = true
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(707, 497)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraScheme)
        Me._tabMainTab_TabPage0.Controls.Add(Me.framFP)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame4)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Scheme"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = true
        '
        'fraScheme
        '
        Me.fraScheme.BackColor = System.Drawing.SystemColors.Control
        Me.fraScheme.Controls.Add(Me.chkDepositAsInstalment)
        Me.fraScheme.Controls.Add(Me.chkPlanRefEditable)
        Me.fraScheme.Controls.Add(Me.cboBranch)
        Me.fraScheme.Controls.Add(Me.chkEnabled)
        Me.fraScheme.Controls.Add(Me.txtSchemeDescription)
        Me.fraScheme.Controls.Add(Me.txtSchemeName)
        Me.fraScheme.Controls.Add(Me.txtSchemeNo)
        Me.fraScheme.Controls.Add(Me.txtSchemeVersion)
        Me.fraScheme.Controls.Add(Me.txtStartDate)
        Me.fraScheme.Controls.Add(Me.txtEndDate)
        Me.fraScheme.Controls.Add(Me.cboCurrency)
        Me.fraScheme.Controls.Add(Me.lblCurrency)
        Me.fraScheme.Controls.Add(Me.lblBranch)
        Me.fraScheme.Controls.Add(Me.lblSchemeDescription)
        Me.fraScheme.Controls.Add(Me.lblSchemeName)
        Me.fraScheme.Controls.Add(Me.lblSchemeNo)
        Me.fraScheme.Controls.Add(Me.lblSchemeVersion)
        Me.fraScheme.Controls.Add(Me.lblStartDate)
        Me.fraScheme.Controls.Add(Me.lblEndDate)
        Me.fraScheme.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraScheme.Location = New System.Drawing.Point(56, 134)
        Me.fraScheme.Name = "fraScheme"
        Me.fraScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraScheme.Size = New System.Drawing.Size(585, 238)
        Me.fraScheme.TabIndex = 70
        Me.fraScheme.TabStop = false
        Me.fraScheme.Text = "Scheme Information"
        '
        'chkDepositAsInstalment
        '
        Me.chkDepositAsInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.chkDepositAsInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDepositAsInstalment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDepositAsInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDepositAsInstalment.Location = New System.Drawing.Point(104, 192)
        Me.chkDepositAsInstalment.Name = "chkDepositAsInstalment"
        Me.chkDepositAsInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDepositAsInstalment.Size = New System.Drawing.Size(361, 17)
        Me.chkDepositAsInstalment.TabIndex = 87
        Me.chkDepositAsInstalment.Text = "Deposit will be transacted as an Instalment"
        Me.chkDepositAsInstalment.UseVisualStyleBackColor = false
        '
        'chkPlanRefEditable
        '
        Me.chkPlanRefEditable.BackColor = System.Drawing.SystemColors.Control
        Me.chkPlanRefEditable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPlanRefEditable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkPlanRefEditable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPlanRefEditable.Location = New System.Drawing.Point(104, 170)
        Me.chkPlanRefEditable.Name = "chkPlanRefEditable"
        Me.chkPlanRefEditable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPlanRefEditable.Size = New System.Drawing.Size(361, 17)
        Me.chkPlanRefEditable.TabIndex = 87
        Me.chkPlanRefEditable.Text = "Plan Reference Editable"
        Me.chkPlanRefEditable.UseVisualStyleBackColor = false
        '
        'cboBranch
        '
        Me.cboBranch.DefaultItemId = 0
        Me.cboBranch.FirstItem = ""
        Me.cboBranch.ItemId = 0
        Me.cboBranch.ListIndex = -1
        Me.cboBranch.Location = New System.Drawing.Point(104, 22)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.PMLookupProductFamily = 1
        Me.cboBranch.SingleItemId = 0
        Me.cboBranch.Size = New System.Drawing.Size(147, 21)
        Me.cboBranch.SortColumnName = ""
        Me.cboBranch.Sorted = true
        Me.cboBranch.TabIndex = 71
        Me.cboBranch.TableName = "Source"
        Me.cboBranch.ToolTipText = ""
        Me.cboBranch.WhereClause = ""
        '
        'chkEnabled
        '
        Me.chkEnabled.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnabled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnabled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkEnabled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnabled.Location = New System.Drawing.Point(104, 215)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnabled.Size = New System.Drawing.Size(129, 17)
        Me.chkEnabled.TabIndex = 88
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = false
        '
        'txtSchemeDescription
        '
        Me.txtSchemeDescription.AcceptsReturn = true
        Me.txtSchemeDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSchemeDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeDescription.Location = New System.Drawing.Point(104, 120)
        Me.txtSchemeDescription.MaxLength = 0
        Me.txtSchemeDescription.Name = "txtSchemeDescription"
        Me.txtSchemeDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeDescription.Size = New System.Drawing.Size(409, 21)
        Me.txtSchemeDescription.TabIndex = 81
        Me.txtSchemeDescription.Tag = "TP;"
        '
        'txtSchemeName
        '
        Me.txtSchemeName.AcceptsReturn = true
        Me.txtSchemeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSchemeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeName.Location = New System.Drawing.Point(104, 96)
        Me.txtSchemeName.MaxLength = 20
        Me.txtSchemeName.Name = "txtSchemeName"
        Me.txtSchemeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeName.Size = New System.Drawing.Size(241, 21)
        Me.txtSchemeName.TabIndex = 79
        Me.txtSchemeName.Tag = "TP;"
        '
        'txtSchemeNo
        '
        Me.txtSchemeNo.AcceptsReturn = true
        Me.txtSchemeNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSchemeNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeNo.Location = New System.Drawing.Point(104, 48)
        Me.txtSchemeNo.MaxLength = 0
        Me.txtSchemeNo.Name = "txtSchemeNo"
        Me.txtSchemeNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeNo.Size = New System.Drawing.Size(57, 21)
        Me.txtSchemeNo.TabIndex = 75
        Me.txtSchemeNo.Tag = "E;"
        '
        'txtSchemeVersion
        '
        Me.txtSchemeVersion.AcceptsReturn = true
        Me.txtSchemeVersion.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeVersion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSchemeVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeVersion.Location = New System.Drawing.Point(104, 72)
        Me.txtSchemeVersion.MaxLength = 0
        Me.txtSchemeVersion.Name = "txtSchemeVersion"
        Me.txtSchemeVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeVersion.Size = New System.Drawing.Size(57, 21)
        Me.txtSchemeVersion.TabIndex = 77
        Me.txtSchemeVersion.Tag = "E;"
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = true
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(104, 144)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(129, 21)
        Me.txtStartDate.TabIndex = 83
        Me.txtStartDate.Tag = "TP;"
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = true
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(384, 144)
        Me.txtEndDate.MaxLength = 0
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(129, 21)
        Me.txtEndDate.TabIndex = 84
        Me.txtEndDate.Tag = "TP;"
        '
        'cboCurrency
        '
        Me.cboCurrency.DefaultItemId = 0
        Me.cboCurrency.Enabled = false
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ItemId = 0
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(354, 22)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.PMLookupProductFamily = 1
        Me.cboCurrency.SingleItemId = 0
        Me.cboCurrency.Size = New System.Drawing.Size(159, 21)
        Me.cboCurrency.SortColumnName = ""
        Me.cboCurrency.Sorted = true
        Me.cboCurrency.TabIndex = 72
        Me.cboCurrency.TableName = "Currency"
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhereClause = ""
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Enabled = false
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(284, 26)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(81, 19)
        Me.lblCurrency.TabIndex = 74
        Me.lblCurrency.Text = "Currency:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(10, 26)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(97, 19)
        Me.lblBranch.TabIndex = 73
        Me.lblBranch.Text = "Branch:"
        '
        'lblSchemeDescription
        '
        Me.lblSchemeDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSchemeDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeDescription.Location = New System.Drawing.Point(10, 122)
        Me.lblSchemeDescription.Name = "lblSchemeDescription"
        Me.lblSchemeDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeDescription.Size = New System.Drawing.Size(97, 19)
        Me.lblSchemeDescription.TabIndex = 82
        Me.lblSchemeDescription.Text = "Description:"
        '
        'lblSchemeName
        '
        Me.lblSchemeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSchemeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeName.Location = New System.Drawing.Point(10, 98)
        Me.lblSchemeName.Name = "lblSchemeName"
        Me.lblSchemeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeName.Size = New System.Drawing.Size(97, 19)
        Me.lblSchemeName.TabIndex = 80
        Me.lblSchemeName.Text = "Name:"
        '
        'lblSchemeNo
        '
        Me.lblSchemeNo.AutoSize = true
        Me.lblSchemeNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSchemeNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeNo.Location = New System.Drawing.Point(10, 50)
        Me.lblSchemeNo.Name = "lblSchemeNo"
        Me.lblSchemeNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeNo.Size = New System.Drawing.Size(77, 13)
        Me.lblSchemeNo.TabIndex = 76
        Me.lblSchemeNo.Text = "Scheme No:"
        '
        'lblSchemeVersion
        '
        Me.lblSchemeVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSchemeVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeVersion.Location = New System.Drawing.Point(10, 74)
        Me.lblSchemeVersion.Name = "lblSchemeVersion"
        Me.lblSchemeVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeVersion.Size = New System.Drawing.Size(97, 19)
        Me.lblSchemeVersion.TabIndex = 78
        Me.lblSchemeVersion.Text = "&Version:"
        '
        'lblStartDate
        '
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(10, 146)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(105, 19)
        Me.lblStartDate.TabIndex = 85
        Me.lblStartDate.Text = "&Start Date:"
        '
        'lblEndDate
        '
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(304, 146)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(81, 19)
        Me.lblEndDate.TabIndex = 86
        Me.lblEndDate.Text = "&End Date:"
        '
        'framFP
        '
        Me.framFP.BackColor = System.Drawing.SystemColors.Control
        Me.framFP.Controls.Add(Me.txtPartyCode)
        Me.framFP.Controls.Add(Me.cmdRelatedPartyFind)
        Me.framFP.Controls.Add(Me.lblPartyCode)
        Me.framFP.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.framFP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framFP.Location = New System.Drawing.Point(56, 72)
        Me.framFP.Name = "framFP"
        Me.framFP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framFP.Size = New System.Drawing.Size(585, 51)
        Me.framFP.TabIndex = 27
        Me.framFP.TabStop = false
        Me.framFP.Text = "Finance Provider"
        '
        'txtPartyCode
        '
        Me.txtPartyCode.AcceptsReturn = true
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Enabled = false
        Me.txtPartyCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(104, 18)
        Me.txtPartyCode.MaxLength = 0
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(201, 21)
        Me.txtPartyCode.TabIndex = 28
        '
        'cmdRelatedPartyFind
        '
        Me.cmdRelatedPartyFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRelatedPartyFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRelatedPartyFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdRelatedPartyFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelatedPartyFind.Location = New System.Drawing.Point(304, 18)
        Me.cmdRelatedPartyFind.Name = "cmdRelatedPartyFind"
        Me.cmdRelatedPartyFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelatedPartyFind.Size = New System.Drawing.Size(24, 21)
        Me.cmdRelatedPartyFind.TabIndex = 30
        Me.cmdRelatedPartyFind.Text = "..."
        Me.cmdRelatedPartyFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelatedPartyFind.UseVisualStyleBackColor = false
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = true
        Me.lblPartyCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCode.Location = New System.Drawing.Point(10, 22)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCode.Size = New System.Drawing.Size(60, 13)
        Me.lblPartyCode.TabIndex = 29
        Me.lblPartyCode.Text = "Provider:"
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.cboSchemeType)
        Me.Frame4.Controls.Add(Me.lblSchemeType)
        Me.Frame4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(56, 12)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(585, 51)
        Me.Frame4.TabIndex = 1
        Me.Frame4.TabStop = false
        Me.Frame4.Text = "Scheme Type"
        '
        'cboSchemeType
        '
        Me.cboSchemeType.DefaultItemId = 0
        Me.cboSchemeType.FirstItem = ""
        Me.cboSchemeType.ItemId = 0
        Me.cboSchemeType.ListIndex = -1
        Me.cboSchemeType.Location = New System.Drawing.Point(104, 18)
        Me.cboSchemeType.Name = "cboSchemeType"
        Me.cboSchemeType.PMLookupProductFamily = 1
        Me.cboSchemeType.SingleItemId = 0
        Me.cboSchemeType.Size = New System.Drawing.Size(201, 21)
        Me.cboSchemeType.SortColumnName = ""
        Me.cboSchemeType.Sorted = true
        Me.cboSchemeType.TabIndex = 2
        Me.cboSchemeType.TableName = "PFScheme_Type"
        Me.cboSchemeType.ToolTipText = ""
        Me.cboSchemeType.WhereClause = ""
        '
        'lblSchemeType
        '
        Me.lblSchemeType.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSchemeType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeType.Location = New System.Drawing.Point(10, 22)
        Me.lblSchemeType.Name = "lblSchemeType"
        Me.lblSchemeType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeType.Size = New System.Drawing.Size(97, 19)
        Me.lblSchemeType.TabIndex = 3
        Me.lblSchemeType.Text = "Scheme Type:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(654, 364)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 123
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = false
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.SSFrame4)
        Me._tabMainTab_TabPage1.Controls.Add(Me.SSFrame1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraCollectionNotification)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Details && Documents"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = true
        '
        'SSFrame4
        '
        Me.SSFrame4.BackColor = System.Drawing.SystemColors.Control
        Me.SSFrame4.Controls.Add(Me.chkRatesForInformationOnly)
        Me.SSFrame4.Controls.Add(Me.chkAllowClientFees)
        Me.SSFrame4.Controls.Add(Me.cboDifference)
        Me.SSFrame4.Controls.Add(Me.chkDepositOnOtherMediaType)
        Me.SSFrame4.Controls.Add(Me.cboExportMethod)
        Me.SSFrame4.Controls.Add(Me.cboMediaType)
        Me.SSFrame4.Controls.Add(Me.lblExportMethod)
        Me.SSFrame4.Controls.Add(Me.lblMediaType)
        Me.SSFrame4.Controls.Add(Me.lblDifference)
        Me.SSFrame4.Controls.Add(Me.txtPFMessage)
        Me.SSFrame4.Controls.Add(Me.lblPFMessage)
        Me.SSFrame4.Controls.Add(Me.chkSpread)
        Me.SSFrame4.Controls.Add(Me.chkSpreadRI)
        Me.SSFrame4.Controls.Add(Me.chkSpreadTaxes)
        Me.SSFrame4.Controls.Add(Me.chkSubAgentSpread)
        Me.SSFrame4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.SSFrame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SSFrame4.Location = New System.Drawing.Point(64, 12)
        Me.SSFrame4.Name = "SSFrame4"
        Me.SSFrame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SSFrame4.Size = New System.Drawing.Size(595, 164)
        Me.SSFrame4.TabIndex = 9
        Me.SSFrame4.TabStop = false
        Me.SSFrame4.Text = "Schemes Specifics"
        '
        'chkRatesForInformationOnly
        '
        Me.chkRatesForInformationOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkRatesForInformationOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRatesForInformationOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkRatesForInformationOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRatesForInformationOnly.Location = New System.Drawing.Point(360, 64)
        Me.chkRatesForInformationOnly.Name = "chkRatesForInformationOnly"
        Me.chkRatesForInformationOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRatesForInformationOnly.Size = New System.Drawing.Size(201, 17)
        Me.chkRatesForInformationOnly.TabIndex = 135
        Me.chkRatesForInformationOnly.Text = "Rates are for Information Only"
        Me.chkRatesForInformationOnly.UseVisualStyleBackColor = false
        '
        'chkAllowClientFees
        '
        Me.chkAllowClientFees.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowClientFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowClientFees.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowClientFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowClientFees.Location = New System.Drawing.Point(360, 42)
        Me.chkAllowClientFees.Name = "chkAllowClientFees"
        Me.chkAllowClientFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowClientFees.Size = New System.Drawing.Size(201, 17)
        Me.chkAllowClientFees.TabIndex = 134
        Me.chkAllowClientFees.Text = "Allow Client Fees"
        Me.chkAllowClientFees.UseVisualStyleBackColor = false
        '
        'cboDifference
        '
        Me.cboDifference.BackColor = System.Drawing.SystemColors.Window
        Me.cboDifference.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDifference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboDifference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboDifference, New Integer() {0, 1, 2})
        Me.cboDifference.Items.AddRange(New Object() {"Write-off difference", "Take exact amount", "User's choice"})
        Me.cboDifference.Location = New System.Drawing.Point(139, 90)
        Me.cboDifference.Name = "cboDifference"
        Me.cboDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDifference.Size = New System.Drawing.Size(153, 21)
        Me.cboDifference.TabIndex = 13
        '
        'chkDepositOnOtherMediaType
        '
        Me.chkDepositOnOtherMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.chkDepositOnOtherMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDepositOnOtherMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDepositOnOtherMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDepositOnOtherMediaType.Location = New System.Drawing.Point(360, 16)
        Me.chkDepositOnOtherMediaType.Name = "chkDepositOnOtherMediaType"
        Me.chkDepositOnOtherMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDepositOnOtherMediaType.Size = New System.Drawing.Size(219, 25)
        Me.chkDepositOnOtherMediaType.TabIndex = 12
        Me.chkDepositOnOtherMediaType.Text = "Deposit on a different media type"
        Me.chkDepositOnOtherMediaType.UseVisualStyleBackColor = false
        '
        'cboExportMethod
        '
        Me.cboExportMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboExportMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboExportMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboExportMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboExportMethod, New Integer(-1) {})
        Me.cboExportMethod.Location = New System.Drawing.Point(418, 90)
        Me.cboExportMethod.Name = "cboExportMethod"
        Me.cboExportMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboExportMethod.Size = New System.Drawing.Size(153, 21)
        Me.cboExportMethod.TabIndex = 14
        '
        'cboMediaType
        '
        Me.cboMediaType.DefaultItemId = 0
        Me.cboMediaType.FirstItem = ""
        Me.cboMediaType.ItemId = 0
        Me.cboMediaType.ListIndex = -1
        Me.cboMediaType.Location = New System.Drawing.Point(139, 18)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.PMLookupProductFamily = 1
        Me.cboMediaType.SingleItemId = 0
        Me.cboMediaType.Size = New System.Drawing.Size(153, 21)
        Me.cboMediaType.SortColumnName = ""
        Me.cboMediaType.Sorted = true
        Me.cboMediaType.TabIndex = 10
        Me.cboMediaType.TableName = "MediaType"
        Me.cboMediaType.ToolTipText = ""
        Me.cboMediaType.WhereClause = ""
        '
        'lblExportMethod
        '
        Me.lblExportMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblExportMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExportMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblExportMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExportMethod.Location = New System.Drawing.Point(306, 93)
        Me.lblExportMethod.Name = "lblExportMethod"
        Me.lblExportMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExportMethod.Size = New System.Drawing.Size(113, 17)
        Me.lblExportMethod.TabIndex = 16
        Me.lblExportMethod.Text = "Export Format:"
        '
        'lblMediaType
        '
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(8, 22)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(137, 17)
        Me.lblMediaType.TabIndex = 11
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblDifference
        '
        Me.lblDifference.BackColor = System.Drawing.SystemColors.Control
        Me.lblDifference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDifference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDifference.Location = New System.Drawing.Point(8, 93)
        Me.lblDifference.Name = "lblDifference"
        Me.lblDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDifference.Size = New System.Drawing.Size(147, 17)
        Me.lblDifference.TabIndex = 15
        Me.lblDifference.Text = "Receipt Difference:"
        '
        'txtPFMessage
        '
        Me.txtPFMessage.AcceptsReturn = true
        Me.txtPFMessage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPFMessage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFMessage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPFMessage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFMessage.Location = New System.Drawing.Point(139, 117)
        Me.txtPFMessage.MaxLength = 0
        Me.txtPFMessage.Name = "txtPFMessage"
        Me.txtPFMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFMessage.Size = New System.Drawing.Size(432, 21)
        Me.txtPFMessage.TabIndex = 20
        Me.txtPFMessage.Tag = "TP;"
        '
        'lblPFMessage
        '
        Me.lblPFMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPFMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPFMessage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPFMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPFMessage.Location = New System.Drawing.Point(8, 120)
        Me.lblPFMessage.Name = "lblPFMessage"
        Me.lblPFMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPFMessage.Size = New System.Drawing.Size(139, 19)
        Me.lblPFMessage.TabIndex = 21
        Me.lblPFMessage.Text = "Third Party Message:"
        '
        'chkSpread
        '
        Me.chkSpread.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpread.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpread.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSpread.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpread.Location = New System.Drawing.Point(139, 113)
        Me.chkSpread.Name = "chkSpread"
        Me.chkSpread.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpread.Size = New System.Drawing.Size(145, 25)
        Me.chkSpread.TabIndex = 17
        Me.chkSpread.Text = "Spread Commission"
        Me.chkSpread.UseVisualStyleBackColor = false
        '
        'chkSpreadRI
        '
        Me.chkSpreadRI.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpreadRI.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpreadRI.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSpreadRI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpreadRI.Location = New System.Drawing.Point(304, 113)
        Me.chkSpreadRI.Name = "chkSpreadRI"
        Me.chkSpreadRI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpreadRI.Size = New System.Drawing.Size(145, 25)
        Me.chkSpreadRI.TabIndex = 18
        Me.chkSpreadRI.Text = "Spread Re-Insurance"
        Me.chkSpreadRI.UseVisualStyleBackColor = false
        '
        'chkSpreadTaxes
        '
        Me.chkSpreadTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpreadTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpreadTaxes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSpreadTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpreadTaxes.Location = New System.Drawing.Point(472, 113)
        Me.chkSpreadTaxes.Name = "chkSpreadTaxes"
        Me.chkSpreadTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpreadTaxes.Size = New System.Drawing.Size(105, 25)
        Me.chkSpreadTaxes.TabIndex = 19
        Me.chkSpreadTaxes.Text = "Spread Taxes"
        Me.chkSpreadTaxes.UseVisualStyleBackColor = false
        '
        'chkSubAgentSpread
        '
        Me.chkSubAgentSpread.BackColor = System.Drawing.SystemColors.Control
        Me.chkSubAgentSpread.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSubAgentSpread.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSubAgentSpread.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSubAgentSpread.Location = New System.Drawing.Point(139, 133)
        Me.chkSubAgentSpread.Name = "chkSubAgentSpread"
        Me.chkSubAgentSpread.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSubAgentSpread.Size = New System.Drawing.Size(204, 25)
        Me.chkSubAgentSpread.TabIndex = 136
        Me.chkSubAgentSpread.Text = "Spread Sub Agnt Commission"
        Me.chkSubAgentSpread.UseVisualStyleBackColor = false
        '
        'SSFrame1
        '
        Me.SSFrame1.BackColor = System.Drawing.SystemColors.Control
        Me.SSFrame1.Controls.Add(Me.cmdCollectionNotificationClear)
        Me.SSFrame1.Controls.Add(Me.txtCollectionNotification)
        Me.SSFrame1.Controls.Add(Me.cmdCollectionNotification)
        Me.SSFrame1.Controls.Add(Me.cmdConfirmationDocClear)
        Me.SSFrame1.Controls.Add(Me.cmdCreditClear)
        Me.SSFrame1.Controls.Add(Me.cmdBankClear)
        Me.SSFrame1.Controls.Add(Me.cmdQuoteClear)
        Me.SSFrame1.Controls.Add(Me.cmdConfirmationDoc)
        Me.SSFrame1.Controls.Add(Me.cmdCredit)
        Me.SSFrame1.Controls.Add(Me.cmbBank)
        Me.SSFrame1.Controls.Add(Me.cmdQuote)
        Me.SSFrame1.Controls.Add(Me.txtConfirmationDoc)
        Me.SSFrame1.Controls.Add(Me.txtcredit)
        Me.SSFrame1.Controls.Add(Me.txtBank)
        Me.SSFrame1.Controls.Add(Me.txtQuote)
        Me.SSFrame1.Controls.Add(Me.cboPrintType)
        Me.SSFrame1.Controls.Add(Me.lblPrintType)
        Me.SSFrame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.SSFrame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SSFrame1.Location = New System.Drawing.Point(64, 182)
        Me.SSFrame1.Name = "SSFrame1"
        Me.SSFrame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SSFrame1.Size = New System.Drawing.Size(385, 201)
        Me.SSFrame1.TabIndex = 48
        Me.SSFrame1.TabStop = false
        Me.SSFrame1.Text = "Documents"
        '
        'cmdCollectionNotificationClear
        '
        Me.cmdCollectionNotificationClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCollectionNotificationClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCollectionNotificationClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCollectionNotificationClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCollectionNotificationClear.Location = New System.Drawing.Point(328, 80)
        Me.cmdCollectionNotificationClear.Name = "cmdCollectionNotificationClear"
        Me.cmdCollectionNotificationClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCollectionNotificationClear.Size = New System.Drawing.Size(49, 21)
        Me.cmdCollectionNotificationClear.TabIndex = 138
        Me.cmdCollectionNotificationClear.Text = "Clear"
        Me.cmdCollectionNotificationClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCollectionNotificationClear.UseVisualStyleBackColor = false
        '
        'txtCollectionNotification
        '
        Me.txtCollectionNotification.AcceptsReturn = true
        Me.txtCollectionNotification.BackColor = System.Drawing.SystemColors.Window
        Me.txtCollectionNotification.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCollectionNotification.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtCollectionNotification.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCollectionNotification.Location = New System.Drawing.Point(160, 80)
        Me.txtCollectionNotification.MaxLength = 0
        Me.txtCollectionNotification.Name = "txtCollectionNotification"
        Me.txtCollectionNotification.ReadOnly = true
        Me.txtCollectionNotification.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCollectionNotification.Size = New System.Drawing.Size(161, 21)
        Me.txtCollectionNotification.TabIndex = 137
        '
        'cmdCollectionNotification
        '
        Me.cmdCollectionNotification.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCollectionNotification.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCollectionNotification.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCollectionNotification.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCollectionNotification.Location = New System.Drawing.Point(8, 78)
        Me.cmdCollectionNotification.Name = "cmdCollectionNotification"
        Me.cmdCollectionNotification.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCollectionNotification.Size = New System.Drawing.Size(139, 27)
        Me.cmdCollectionNotification.TabIndex = 136
        Me.cmdCollectionNotification.Text = "Collection Notification"
        Me.cmdCollectionNotification.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCollectionNotification.UseVisualStyleBackColor = false
        '
        'cmdConfirmationDocClear
        '
        Me.cmdConfirmationDocClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConfirmationDocClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConfirmationDocClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdConfirmationDocClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConfirmationDocClear.Location = New System.Drawing.Point(328, 133)
        Me.cmdConfirmationDocClear.Name = "cmdConfirmationDocClear"
        Me.cmdConfirmationDocClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConfirmationDocClear.Size = New System.Drawing.Size(49, 21)
        Me.cmdConfirmationDocClear.TabIndex = 60
        Me.cmdConfirmationDocClear.Text = "Clear"
        Me.cmdConfirmationDocClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConfirmationDocClear.UseVisualStyleBackColor = false
        '
        'cmdCreditClear
        '
        Me.cmdCreditClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCreditClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreditClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCreditClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreditClear.Location = New System.Drawing.Point(328, 107)
        Me.cmdCreditClear.Name = "cmdCreditClear"
        Me.cmdCreditClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreditClear.Size = New System.Drawing.Size(49, 21)
        Me.cmdCreditClear.TabIndex = 57
        Me.cmdCreditClear.Text = "Clear"
        Me.cmdCreditClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCreditClear.UseVisualStyleBackColor = false
        '
        'cmdBankClear
        '
        Me.cmdBankClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdBankClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankClear.Location = New System.Drawing.Point(328, 52)
        Me.cmdBankClear.Name = "cmdBankClear"
        Me.cmdBankClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankClear.Size = New System.Drawing.Size(49, 21)
        Me.cmdBankClear.TabIndex = 54
        Me.cmdBankClear.Text = "Clear"
        Me.cmdBankClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankClear.UseVisualStyleBackColor = false
        '
        'cmdQuoteClear
        '
        Me.cmdQuoteClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuoteClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuoteClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdQuoteClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuoteClear.Location = New System.Drawing.Point(328, 26)
        Me.cmdQuoteClear.Name = "cmdQuoteClear"
        Me.cmdQuoteClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuoteClear.Size = New System.Drawing.Size(49, 21)
        Me.cmdQuoteClear.TabIndex = 51
        Me.cmdQuoteClear.Text = "Clear"
        Me.cmdQuoteClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdQuoteClear.UseVisualStyleBackColor = false
        '
        'cmdConfirmationDoc
        '
        Me.cmdConfirmationDoc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConfirmationDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConfirmationDoc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdConfirmationDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConfirmationDoc.Location = New System.Drawing.Point(8, 133)
        Me.cmdConfirmationDoc.Name = "cmdConfirmationDoc"
        Me.cmdConfirmationDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConfirmationDoc.Size = New System.Drawing.Size(139, 21)
        Me.cmdConfirmationDoc.TabIndex = 58
        Me.cmdConfirmationDoc.Text = "Confirmation:"
        Me.cmdConfirmationDoc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConfirmationDoc.UseVisualStyleBackColor = false
        '
        'cmdCredit
        '
        Me.cmdCredit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCredit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCredit.Location = New System.Drawing.Point(8, 107)
        Me.cmdCredit.Name = "cmdCredit"
        Me.cmdCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCredit.Size = New System.Drawing.Size(139, 21)
        Me.cmdCredit.TabIndex = 55
        Me.cmdCredit.Text = "Credit Detail:"
        Me.cmdCredit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCredit.UseVisualStyleBackColor = false
        '
        'cmbBank
        '
        Me.cmbBank.BackColor = System.Drawing.SystemColors.Control
        Me.cmbBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmbBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmbBank.Location = New System.Drawing.Point(8, 52)
        Me.cmbBank.Name = "cmbBank"
        Me.cmbBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBank.Size = New System.Drawing.Size(139, 21)
        Me.cmbBank.TabIndex = 52
        Me.cmbBank.Text = "Collection Form:"
        Me.cmbBank.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmbBank.UseVisualStyleBackColor = false
        '
        'cmdQuote
        '
        Me.cmdQuote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuote.Location = New System.Drawing.Point(8, 26)
        Me.cmdQuote.Name = "cmdQuote"
        Me.cmdQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuote.Size = New System.Drawing.Size(139, 21)
        Me.cmdQuote.TabIndex = 49
        Me.cmdQuote.Text = "Quote"
        Me.cmdQuote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdQuote.UseVisualStyleBackColor = false
        '
        'txtConfirmationDoc
        '
        Me.txtConfirmationDoc.AcceptsReturn = true
        Me.txtConfirmationDoc.BackColor = System.Drawing.SystemColors.Window
        Me.txtConfirmationDoc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConfirmationDoc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtConfirmationDoc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConfirmationDoc.Location = New System.Drawing.Point(160, 135)
        Me.txtConfirmationDoc.MaxLength = 0
        Me.txtConfirmationDoc.Name = "txtConfirmationDoc"
        Me.txtConfirmationDoc.ReadOnly = true
        Me.txtConfirmationDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConfirmationDoc.Size = New System.Drawing.Size(161, 21)
        Me.txtConfirmationDoc.TabIndex = 59
        '
        'txtcredit
        '
        Me.txtcredit.AcceptsReturn = true
        Me.txtcredit.BackColor = System.Drawing.SystemColors.Window
        Me.txtcredit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtcredit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtcredit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtcredit.Location = New System.Drawing.Point(160, 109)
        Me.txtcredit.MaxLength = 0
        Me.txtcredit.Name = "txtcredit"
        Me.txtcredit.ReadOnly = true
        Me.txtcredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtcredit.Size = New System.Drawing.Size(161, 21)
        Me.txtcredit.TabIndex = 56
        '
        'txtBank
        '
        Me.txtBank.AcceptsReturn = true
        Me.txtBank.BackColor = System.Drawing.SystemColors.Window
        Me.txtBank.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtBank.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBank.Location = New System.Drawing.Point(160, 52)
        Me.txtBank.MaxLength = 0
        Me.txtBank.Name = "txtBank"
        Me.txtBank.ReadOnly = true
        Me.txtBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBank.Size = New System.Drawing.Size(161, 21)
        Me.txtBank.TabIndex = 53
        '
        'txtQuote
        '
        Me.txtQuote.AcceptsReturn = true
        Me.txtQuote.BackColor = System.Drawing.SystemColors.Window
        Me.txtQuote.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtQuote.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtQuote.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtQuote.Location = New System.Drawing.Point(160, 28)
        Me.txtQuote.MaxLength = 0
        Me.txtQuote.Name = "txtQuote"
        Me.txtQuote.ReadOnly = true
        Me.txtQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtQuote.Size = New System.Drawing.Size(161, 21)
        Me.txtQuote.TabIndex = 50
        '
        'cboPrintType
        '
        Me.cboPrintType.DefaultItemId = 0
        Me.cboPrintType.FirstItem = ""
        Me.cboPrintType.ItemId = 0
        Me.cboPrintType.ListIndex = -1
        Me.cboPrintType.Location = New System.Drawing.Point(160, 164)
        Me.cboPrintType.Name = "cboPrintType"
        Me.cboPrintType.PMLookupProductFamily = 1
        Me.cboPrintType.SingleItemId = 0
        Me.cboPrintType.Size = New System.Drawing.Size(215, 21)
        Me.cboPrintType.SortColumnName = ""
        Me.cboPrintType.Sorted = true
        Me.cboPrintType.TabIndex = 61
        Me.cboPrintType.TableName = "pfScheme_PrintType"
        Me.cboPrintType.ToolTipText = ""
        Me.cboPrintType.WhereClause = ""
        '
        'lblPrintType
        '
        Me.lblPrintType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrintType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrintType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPrintType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrintType.Location = New System.Drawing.Point(8, 168)
        Me.lblPrintType.Name = "lblPrintType"
        Me.lblPrintType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrintType.Size = New System.Drawing.Size(139, 17)
        Me.lblPrintType.TabIndex = 62
        Me.lblPrintType.Text = "When To Print:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chkBusinessCodeMandatory)
        Me.Frame1.Controls.Add(Me.chkAgentRefMandatory)
        Me.Frame1.Controls.Add(Me.chkBankAddress)
        Me.Frame1.Controls.Add(Me.chkBranchName)
        Me.Frame1.Controls.Add(Me.chkBranchCode)
        Me.Frame1.Controls.Add(Me.chkBankName)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(452, 182)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(209, 169)
        Me.Frame1.TabIndex = 63
        Me.Frame1.TabStop = false
        Me.Frame1.Text = "Mandatory Fields"
        '
        'chkBusinessCodeMandatory
        '
        Me.chkBusinessCodeMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkBusinessCodeMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBusinessCodeMandatory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkBusinessCodeMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBusinessCodeMandatory.Location = New System.Drawing.Point(8, 144)
        Me.chkBusinessCodeMandatory.Name = "chkBusinessCodeMandatory"
        Me.chkBusinessCodeMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBusinessCodeMandatory.Size = New System.Drawing.Size(199, 19)
        Me.chkBusinessCodeMandatory.TabIndex = 69
        Me.chkBusinessCodeMandatory.Text = "Business Code Mandatory"
        Me.chkBusinessCodeMandatory.UseVisualStyleBackColor = false
        '
        'chkAgentRefMandatory
        '
        Me.chkAgentRefMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgentRefMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgentRefMandatory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAgentRefMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgentRefMandatory.Location = New System.Drawing.Point(8, 120)
        Me.chkAgentRefMandatory.Name = "chkAgentRefMandatory"
        Me.chkAgentRefMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgentRefMandatory.Size = New System.Drawing.Size(177, 17)
        Me.chkAgentRefMandatory.TabIndex = 68
        Me.chkAgentRefMandatory.Text = "Agent Ref Mandatory"
        Me.chkAgentRefMandatory.UseVisualStyleBackColor = false
        '
        'chkBankAddress
        '
        Me.chkBankAddress.BackColor = System.Drawing.SystemColors.Control
        Me.chkBankAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBankAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkBankAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBankAddress.Location = New System.Drawing.Point(8, 48)
        Me.chkBankAddress.Name = "chkBankAddress"
        Me.chkBankAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBankAddress.Size = New System.Drawing.Size(153, 17)
        Me.chkBankAddress.TabIndex = 65
        Me.chkBankAddress.Text = "Bank Address"
        Me.chkBankAddress.UseVisualStyleBackColor = false
        '
        'chkBranchName
        '
        Me.chkBranchName.BackColor = System.Drawing.SystemColors.Control
        Me.chkBranchName.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBranchName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkBranchName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBranchName.Location = New System.Drawing.Point(8, 72)
        Me.chkBranchName.Name = "chkBranchName"
        Me.chkBranchName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBranchName.Size = New System.Drawing.Size(145, 17)
        Me.chkBranchName.TabIndex = 66
        Me.chkBranchName.Text = "Branch Name"
        Me.chkBranchName.UseVisualStyleBackColor = false
        '
        'chkBranchCode
        '
        Me.chkBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.chkBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkBranchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBranchCode.Location = New System.Drawing.Point(8, 96)
        Me.chkBranchCode.Name = "chkBranchCode"
        Me.chkBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBranchCode.Size = New System.Drawing.Size(137, 17)
        Me.chkBranchCode.TabIndex = 67
        Me.chkBranchCode.Text = "Branch Code"
        Me.chkBranchCode.UseVisualStyleBackColor = false
        '
        'chkBankName
        '
        Me.chkBankName.BackColor = System.Drawing.SystemColors.Control
        Me.chkBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBankName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBankName.Location = New System.Drawing.Point(8, 24)
        Me.chkBankName.Name = "chkBankName"
        Me.chkBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBankName.Size = New System.Drawing.Size(153, 17)
        Me.chkBankName.TabIndex = 64
        Me.chkBankName.Text = "Bank Name"
        Me.chkBankName.UseVisualStyleBackColor = false
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(654, 430)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 27)
        Me._cmdNext_1.TabIndex = 124
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = false
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 430)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 27)
        Me._cmdPrevious_0.TabIndex = 118
        Me._cmdPrevious_0.Text = "&<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = false
        '
        'fraCollectionNotification
        '
        Me.fraCollectionNotification.BackColor = System.Drawing.SystemColors.Control
        Me.fraCollectionNotification.Controls.Add(Me.txtNotificationDays)
        Me.fraCollectionNotification.Controls.Add(Me.lblNotificationDays)
        Me.fraCollectionNotification.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraCollectionNotification.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCollectionNotification.Location = New System.Drawing.Point(454, 354)
        Me.fraCollectionNotification.Name = "fraCollectionNotification"
        Me.fraCollectionNotification.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCollectionNotification.Size = New System.Drawing.Size(207, 59)
        Me.fraCollectionNotification.TabIndex = 139
        Me.fraCollectionNotification.TabStop = false
        Me.fraCollectionNotification.Text = "Collection Notification"
        '
        'txtNotificationDays
        '
        Me.txtNotificationDays.AcceptsReturn = true
        Me.txtNotificationDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtNotificationDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNotificationDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtNotificationDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNotificationDays.Location = New System.Drawing.Point(120, 22)
        Me.txtNotificationDays.MaxLength = 0
        Me.txtNotificationDays.Name = "txtNotificationDays"
        Me.txtNotificationDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNotificationDays.Size = New System.Drawing.Size(79, 21)
        Me.txtNotificationDays.TabIndex = 141
        '
        'lblNotificationDays
        '
        Me.lblNotificationDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblNotificationDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNotificationDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblNotificationDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNotificationDays.Location = New System.Drawing.Point(12, 24)
        Me.lblNotificationDays.Name = "lblNotificationDays"
        Me.lblNotificationDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNotificationDays.Size = New System.Drawing.Size(97, 13)
        Me.lblNotificationDays.TabIndex = 140
        Me.lblNotificationDays.Text = "Notification Days"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cboTransactionType)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lblTransactionType)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lvRates)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Rates"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = true
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 119
        Me._cmdPrevious_1.Text = "&<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = false
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(654, 364)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 125
        Me._cmdNext_2.Text = "&>>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = false
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = false
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(580, 332)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(57, 22)
        Me.cmdDelete.TabIndex = 117
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = false
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = false
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(516, 332)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(57, 22)
        Me.cmdEdit.TabIndex = 116
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = false
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(452, 332)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(57, 22)
        Me.cmdAdd.TabIndex = 115
        Me.cmdAdd.Tag = "TP;"
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = false
        '
        'lblTransactionType
        '
        Me.lblTransactionType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTransactionType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionType.Location = New System.Drawing.Point(60, 8)
        Me.lblTransactionType.Name = "lblTransactionType"
        Me.lblTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionType.Size = New System.Drawing.Size(113, 17)
        Me.lblTransactionType.TabIndex = 142
        Me.lblTransactionType.Text = "Transaction Type:"
        Me.lblTransactionType.Visible = false
        '
        'cboTransactionType
        '
        Me.cboTransactionType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransactionType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboTransactionType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTransactionType.Items.AddRange(New Object() {"All", "Salvage Recovery", "Third-Party Recovery"})
        Me.cboTransactionType.Location = New System.Drawing.Point(176, 5)
        Me.cboTransactionType.Name = "cboTransactionType"
        Me.cboTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransactionType.Size = New System.Drawing.Size(185, 21)
        Me.cboTransactionType.TabIndex = 143
        Me.cboTransactionType.Visible = false
        '
        'lvRates
        '
        Me.lvRates.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvRates, "")
        Me.lvRates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvRates_ColumnHeader_1, Me._lvRates_ColumnHeader_2, Me._lvRates_ColumnHeader_3, Me._lvRates_ColumnHeader_4, Me._lvRates_ColumnHeader_5, Me._lvRates_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvRates, true)
        Me.lvRates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvRates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvRates.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvRates, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvRates, "")
        Me.lvRates.Location = New System.Drawing.Point(60, 28)
        Me.lvRates.Name = "lvRates"
        Me.lvRates.Size = New System.Drawing.Size(581, 289)
        Me.listViewHelper1.SetSmallIcons(Me.lvRates, "")
        Me.listViewHelper1.SetSorted(Me.lvRates, false)
        Me.listViewHelper1.SetSortKey(Me.lvRates, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvRates, System.Windows.Forms.SortOrder.Ascending)
        Me.lvRates.TabIndex = 47
        Me.lvRates.UseCompatibleStateImageBehavior = false
        Me.lvRates.View = System.Windows.Forms.View.Details
        '
        '_lvRates_ColumnHeader_1
        '
        Me._lvRates_ColumnHeader_1.Tag = ""
        Me._lvRates_ColumnHeader_1.Text = "Start Date"
        Me._lvRates_ColumnHeader_1.Width = 97
        '
        '_lvRates_ColumnHeader_2
        '
        Me._lvRates_ColumnHeader_2.Tag = ""
        Me._lvRates_ColumnHeader_2.Text = "End Date"
        Me._lvRates_ColumnHeader_2.Width = 97
        '
        '_lvRates_ColumnHeader_3
        '
        Me._lvRates_ColumnHeader_3.Tag = ""
        Me._lvRates_ColumnHeader_3.Text = "Code"
        Me._lvRates_ColumnHeader_3.Width = 97
        '
        '_lvRates_ColumnHeader_4
        '
        Me._lvRates_ColumnHeader_4.Tag = ""
        Me._lvRates_ColumnHeader_4.Text = "Type"
        Me._lvRates_ColumnHeader_4.Width = 97
        '
        '_lvRates_ColumnHeader_5
        '
        Me._lvRates_ColumnHeader_5.Tag = ""
        Me._lvRates_ColumnHeader_5.Text = "Frequency"
        Me._lvRates_ColumnHeader_5.Width = 97
        '
        '_lvRates_ColumnHeader_6
        '
        Me._lvRates_ColumnHeader_6.Tag = ""
        Me._lvRates_ColumnHeader_6.Text = "PFRF"
        Me._lvRates_ColumnHeader_6.Width = 0
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame5)
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame6)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fmeSuspense)
        Me._tabMainTab_TabPage3.Controls.Add(Me.frameTax)
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame9)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Accounts"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = true
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.actAdmin)
        Me.Frame5.Controls.Add(Me.lblAdmin)
        Me.Frame5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(8, 290)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(585, 57)
        Me.Frame5.TabIndex = 112
        Me.Frame5.TabStop = false
        Me.Frame5.Text = "Admin Fees"
        Me.Frame5.Visible = false
        '
        'actAdmin
        '
        Me.actAdmin.AccountId = 0
        Me.actAdmin.AllowStoppedAccounts = false
        Me.actAdmin.BackStyle = 0
        Me.actAdmin.CompanyId = 0
        Me.actAdmin.Default_Renamed = false
        Me.actAdmin.Location = New System.Drawing.Point(128, 24)
        Me.actAdmin.LookupCaption = "..."
        Me.actAdmin.LookupHeight = 285
        Me.actAdmin.LookupLeft = 2895
        Me.actAdmin.LookupTextLeft = 0
        Me.actAdmin.LookupTextWidth = 2895
        Me.actAdmin.LookupWidth = 360
        Me.actAdmin.Name = "actAdmin"
        Me.actAdmin.OnlyUpdatableAccounts = false
        Me.actAdmin.SelLength = 0
        Me.actAdmin.SelStart = 0
        Me.actAdmin.SelText = ""
        Me.actAdmin.ShowEditOnFindAccount = false
        Me.actAdmin.Size = New System.Drawing.Size(217, 19)
        Me.actAdmin.TabIndex = 114
        Me.actAdmin.ToolTipText = ""
        '
        'lblAdmin
        '
        Me.lblAdmin.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdmin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdmin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAdmin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdmin.Location = New System.Drawing.Point(8, 24)
        Me.lblAdmin.Name = "lblAdmin"
        Me.lblAdmin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdmin.Size = New System.Drawing.Size(57, 17)
        Me.lblAdmin.TabIndex = 113
        Me.lblAdmin.Text = "Account:"
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me.actInterest)
        Me.Frame6.Controls.Add(Me.actProtection)
        Me.Frame6.Controls.Add(Me.lblProtection)
        Me.Frame6.Controls.Add(Me.lblInterest)
        Me.Frame6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(52, 76)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(585, 81)
        Me.Frame6.TabIndex = 31
        Me.Frame6.TabStop = false
        Me.Frame6.Text = "Charges"
        '
        'actInterest
        '
        Me.actInterest.AccountId = 0
        Me.actInterest.AllowStoppedAccounts = false
        Me.actInterest.BackStyle = 0
        Me.actInterest.CompanyId = 0
        Me.actInterest.Default_Renamed = false
        Me.actInterest.Location = New System.Drawing.Point(160, 24)
        Me.actInterest.LookupCaption = "..."
        Me.actInterest.LookupHeight = 285
        Me.actInterest.LookupLeft = 2895
        Me.actInterest.LookupTextLeft = 0
        Me.actInterest.LookupTextWidth = 2895
        Me.actInterest.LookupWidth = 360
        Me.actInterest.Name = "actInterest"
        Me.actInterest.OnlyUpdatableAccounts = false
        Me.actInterest.SelLength = 0
        Me.actInterest.SelStart = 0
        Me.actInterest.SelText = ""
        Me.actInterest.ShowEditOnFindAccount = false
        Me.actInterest.Size = New System.Drawing.Size(217, 19)
        Me.actInterest.TabIndex = 32
        Me.actInterest.ToolTipText = ""
        '
        'actProtection
        '
        Me.actProtection.AccountId = 0
        Me.actProtection.AllowStoppedAccounts = false
        Me.actProtection.BackStyle = 0
        Me.actProtection.CompanyId = 0
        Me.actProtection.Default_Renamed = false
        Me.actProtection.Location = New System.Drawing.Point(160, 48)
        Me.actProtection.LookupCaption = "..."
        Me.actProtection.LookupHeight = 285
        Me.actProtection.LookupLeft = 2895
        Me.actProtection.LookupTextLeft = 0
        Me.actProtection.LookupTextWidth = 2895
        Me.actProtection.LookupWidth = 360
        Me.actProtection.Name = "actProtection"
        Me.actProtection.OnlyUpdatableAccounts = false
        Me.actProtection.SelLength = 0
        Me.actProtection.SelStart = 0
        Me.actProtection.SelText = ""
        Me.actProtection.ShowEditOnFindAccount = false
        Me.actProtection.Size = New System.Drawing.Size(217, 19)
        Me.actProtection.TabIndex = 35
        Me.actProtection.ToolTipText = ""
        '
        'lblProtection
        '
        Me.lblProtection.BackColor = System.Drawing.SystemColors.Control
        Me.lblProtection.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProtection.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProtection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProtection.Location = New System.Drawing.Point(8, 48)
        Me.lblProtection.Name = "lblProtection"
        Me.lblProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProtection.Size = New System.Drawing.Size(161, 17)
        Me.lblProtection.TabIndex = 34
        Me.lblProtection.Text = "Protection Account:"
        '
        'lblInterest
        '
        Me.lblInterest.BackColor = System.Drawing.SystemColors.Control
        Me.lblInterest.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInterest.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblInterest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInterest.Location = New System.Drawing.Point(8, 24)
        Me.lblInterest.Name = "lblInterest"
        Me.lblInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInterest.Size = New System.Drawing.Size(161, 17)
        Me.lblInterest.TabIndex = 33
        Me.lblInterest.Text = "Interest Account:"
        '
        'fmeSuspense
        '
        Me.fmeSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.fmeSuspense.Controls.Add(Me.actSubAgentCommissionSuspense)
        Me.fmeSuspense.Controls.Add(Me.lblSubAgentCommissionSuspense)
        Me.fmeSuspense.Controls.Add(Me.actCommissionSuspense)
        Me.fmeSuspense.Controls.Add(Me.actSuspense)
        Me.fmeSuspense.Controls.Add(Me.actReInsuranceSuspense)
        Me.fmeSuspense.Controls.Add(Me.lblReInsuranceSuspense)
        Me.fmeSuspense.Controls.Add(Me.lblSuspense)
        Me.fmeSuspense.Controls.Add(Me.lblCommissionSuspense)
        Me.fmeSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fmeSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeSuspense.Location = New System.Drawing.Point(52, 164)
        Me.fmeSuspense.Name = "fmeSuspense"
        Me.fmeSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeSuspense.Size = New System.Drawing.Size(585, 120)
        Me.fmeSuspense.TabIndex = 89
        Me.fmeSuspense.TabStop = false
        Me.fmeSuspense.Text = "Suspense"
        '
        'actSubAgentCommissionSuspense
        '
        Me.actSubAgentCommissionSuspense.AccountId = 0
        Me.actSubAgentCommissionSuspense.AllowStoppedAccounts = false
        Me.actSubAgentCommissionSuspense.BackStyle = 0
        Me.actSubAgentCommissionSuspense.CompanyId = 0
        Me.actSubAgentCommissionSuspense.Default_Renamed = false
        Me.actSubAgentCommissionSuspense.Location = New System.Drawing.Point(270, 65)
        Me.actSubAgentCommissionSuspense.LookupCaption = "..."
        Me.actSubAgentCommissionSuspense.LookupHeight = 285
        Me.actSubAgentCommissionSuspense.LookupLeft = 2895
        Me.actSubAgentCommissionSuspense.LookupTextLeft = 0
        Me.actSubAgentCommissionSuspense.LookupTextWidth = 2895
        Me.actSubAgentCommissionSuspense.LookupWidth = 360
        Me.actSubAgentCommissionSuspense.Name = "actSubAgentCommissionSuspense"
        Me.actSubAgentCommissionSuspense.OnlyUpdatableAccounts = false
        Me.actSubAgentCommissionSuspense.SelLength = 0
        Me.actSubAgentCommissionSuspense.SelStart = 0
        Me.actSubAgentCommissionSuspense.SelText = ""
        Me.actSubAgentCommissionSuspense.ShowEditOnFindAccount = false
        Me.actSubAgentCommissionSuspense.Size = New System.Drawing.Size(217, 19)
        Me.actSubAgentCommissionSuspense.TabIndex = 96
        Me.actSubAgentCommissionSuspense.ToolTipText = ""
        '
        'lblSubAgentCommissionSuspense
        '
        Me.lblSubAgentCommissionSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubAgentCommissionSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubAgentCommissionSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSubAgentCommissionSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubAgentCommissionSuspense.Location = New System.Drawing.Point(8, 67)
        Me.lblSubAgentCommissionSuspense.Name = "lblSubAgentCommissionSuspense"
        Me.lblSubAgentCommissionSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubAgentCommissionSuspense.Size = New System.Drawing.Size(235, 17)
        Me.lblSubAgentCommissionSuspense.TabIndex = 97
        Me.lblSubAgentCommissionSuspense.Text = "Sub Agent Commission Account:"
        '
        'actCommissionSuspense
        '
        Me.actCommissionSuspense.AccountId = 0
        Me.actCommissionSuspense.AllowStoppedAccounts = false
        Me.actCommissionSuspense.BackStyle = 0
        Me.actCommissionSuspense.CompanyId = 0
        Me.actCommissionSuspense.Default_Renamed = false
        Me.actCommissionSuspense.Location = New System.Drawing.Point(270, 40)
        Me.actCommissionSuspense.LookupCaption = "..."
        Me.actCommissionSuspense.LookupHeight = 285
        Me.actCommissionSuspense.LookupLeft = 2895
        Me.actCommissionSuspense.LookupTextLeft = 0
        Me.actCommissionSuspense.LookupTextWidth = 2895
        Me.actCommissionSuspense.LookupWidth = 360
        Me.actCommissionSuspense.Name = "actCommissionSuspense"
        Me.actCommissionSuspense.OnlyUpdatableAccounts = false
        Me.actCommissionSuspense.SelLength = 0
        Me.actCommissionSuspense.SelStart = 0
        Me.actCommissionSuspense.SelText = ""
        Me.actCommissionSuspense.ShowEditOnFindAccount = false
        Me.actCommissionSuspense.Size = New System.Drawing.Size(217, 19)
        Me.actCommissionSuspense.TabIndex = 92
        Me.actCommissionSuspense.ToolTipText = ""
        '
        'actSuspense
        '
        Me.actSuspense.AccountId = 0
        Me.actSuspense.AllowStoppedAccounts = false
        Me.actSuspense.BackStyle = 0
        Me.actSuspense.CompanyId = 0
        Me.actSuspense.Default_Renamed = false
        Me.actSuspense.Location = New System.Drawing.Point(270, 15)
        Me.actSuspense.LookupCaption = "..."
        Me.actSuspense.LookupHeight = 285
        Me.actSuspense.LookupLeft = 2895
        Me.actSuspense.LookupTextLeft = 0
        Me.actSuspense.LookupTextWidth = 2895
        Me.actSuspense.LookupWidth = 360
        Me.actSuspense.Name = "actSuspense"
        Me.actSuspense.OnlyUpdatableAccounts = false
        Me.actSuspense.SelLength = 0
        Me.actSuspense.SelStart = 0
        Me.actSuspense.SelText = ""
        Me.actSuspense.ShowEditOnFindAccount = false
        Me.actSuspense.Size = New System.Drawing.Size(217, 19)
        Me.actSuspense.TabIndex = 90
        Me.actSuspense.ToolTipText = ""
        '
        'actReInsuranceSuspense
        '
        Me.actReInsuranceSuspense.AccountId = 0
        Me.actReInsuranceSuspense.AllowStoppedAccounts = false
        Me.actReInsuranceSuspense.BackStyle = 0
        Me.actReInsuranceSuspense.CompanyId = 0
        Me.actReInsuranceSuspense.Default_Renamed = false
        Me.actReInsuranceSuspense.Location = New System.Drawing.Point(270, 90)
        Me.actReInsuranceSuspense.LookupCaption = "..."
        Me.actReInsuranceSuspense.LookupHeight = 285
        Me.actReInsuranceSuspense.LookupLeft = 2895
        Me.actReInsuranceSuspense.LookupTextLeft = 0
        Me.actReInsuranceSuspense.LookupTextWidth = 2895
        Me.actReInsuranceSuspense.LookupWidth = 360
        Me.actReInsuranceSuspense.Name = "actReInsuranceSuspense"
        Me.actReInsuranceSuspense.OnlyUpdatableAccounts = false
        Me.actReInsuranceSuspense.SelLength = 0
        Me.actReInsuranceSuspense.SelStart = 0
        Me.actReInsuranceSuspense.SelText = ""
        Me.actReInsuranceSuspense.ShowEditOnFindAccount = false
        Me.actReInsuranceSuspense.Size = New System.Drawing.Size(217, 19)
        Me.actReInsuranceSuspense.TabIndex = 94
        Me.actReInsuranceSuspense.ToolTipText = ""
        '
        'lblReInsuranceSuspense
        '
        Me.lblReInsuranceSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblReInsuranceSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReInsuranceSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblReInsuranceSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReInsuranceSuspense.Location = New System.Drawing.Point(8, 92)
        Me.lblReInsuranceSuspense.Name = "lblReInsuranceSuspense"
        Me.lblReInsuranceSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReInsuranceSuspense.Size = New System.Drawing.Size(161, 17)
        Me.lblReInsuranceSuspense.TabIndex = 95
        Me.lblReInsuranceSuspense.Text = "Re-Insurance Account:"
        '
        'lblSuspense
        '
        Me.lblSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSuspense.Location = New System.Drawing.Point(8, 17)
        Me.lblSuspense.Name = "lblSuspense"
        Me.lblSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSuspense.Size = New System.Drawing.Size(161, 17)
        Me.lblSuspense.TabIndex = 91
        Me.lblSuspense.Text = "Revenue Account:"
        '
        'lblCommissionSuspense
        '
        Me.lblCommissionSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCommissionSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionSuspense.Location = New System.Drawing.Point(8, 42)
        Me.lblCommissionSuspense.Name = "lblCommissionSuspense"
        Me.lblCommissionSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionSuspense.Size = New System.Drawing.Size(161, 17)
        Me.lblCommissionSuspense.TabIndex = 93
        Me.lblCommissionSuspense.Text = "Commission Account:"
        '
        'frameTax
        '
        Me.frameTax.BackColor = System.Drawing.SystemColors.Control
        Me.frameTax.Controls.Add(Me.cboTaxGroupID)
        Me.frameTax.Controls.Add(Me.actTaxSuspense)
        Me.frameTax.Controls.Add(Me.lblTaxSuspense)
        Me.frameTax.Controls.Add(Me.lblTaxGroup)
        Me.frameTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.frameTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameTax.Location = New System.Drawing.Point(52, 290)
        Me.frameTax.Name = "frameTax"
        Me.frameTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameTax.Size = New System.Drawing.Size(585, 57)
        Me.frameTax.TabIndex = 107
        Me.frameTax.TabStop = false
        Me.frameTax.Text = "Taxes"
        '
        'cboTaxGroupID
        '
        Me.cboTaxGroupID.DefaultItemId = 0
        Me.cboTaxGroupID.FirstItem = ""
        Me.cboTaxGroupID.ItemId = 0
        Me.cboTaxGroupID.ListIndex = -1
        Me.cboTaxGroupID.Location = New System.Drawing.Point(160, 22)
        Me.cboTaxGroupID.Name = "cboTaxGroupID"
        Me.cboTaxGroupID.PMLookupProductFamily = 1
        Me.cboTaxGroupID.SingleItemId = 0
        Me.cboTaxGroupID.Size = New System.Drawing.Size(209, 21)
        Me.cboTaxGroupID.SortColumnName = ""
        Me.cboTaxGroupID.Sorted = true
        Me.cboTaxGroupID.TabIndex = 108
        Me.cboTaxGroupID.TableName = "Tax_Group"
        Me.cboTaxGroupID.ToolTipText = ""
        Me.cboTaxGroupID.WhereClause = ""
        '
        'actTaxSuspense
        '
        Me.actTaxSuspense.AccountId = 0
        Me.actTaxSuspense.AllowStoppedAccounts = false
        Me.actTaxSuspense.BackStyle = 0
        Me.actTaxSuspense.CompanyId = 0
        Me.actTaxSuspense.Default_Renamed = false
        Me.actTaxSuspense.Location = New System.Drawing.Point(160, 48)
        Me.actTaxSuspense.LookupCaption = "..."
        Me.actTaxSuspense.LookupHeight = 285
        Me.actTaxSuspense.LookupLeft = 2895
        Me.actTaxSuspense.LookupTextLeft = 0
        Me.actTaxSuspense.LookupTextWidth = 2895
        Me.actTaxSuspense.LookupWidth = 360
        Me.actTaxSuspense.Name = "actTaxSuspense"
        Me.actTaxSuspense.OnlyUpdatableAccounts = false
        Me.actTaxSuspense.SelLength = 0
        Me.actTaxSuspense.SelStart = 0
        Me.actTaxSuspense.SelText = ""
        Me.actTaxSuspense.ShowEditOnFindAccount = false
        Me.actTaxSuspense.Size = New System.Drawing.Size(217, 19)
        Me.actTaxSuspense.TabIndex = 110
        Me.actTaxSuspense.ToolTipText = ""
        Me.actTaxSuspense.Visible = false
        '
        'lblTaxSuspense
        '
        Me.lblTaxSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxSuspense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTaxSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxSuspense.Location = New System.Drawing.Point(8, 50)
        Me.lblTaxSuspense.Name = "lblTaxSuspense"
        Me.lblTaxSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxSuspense.Size = New System.Drawing.Size(161, 17)
        Me.lblTaxSuspense.TabIndex = 111
        Me.lblTaxSuspense.Text = "Suspense Account:"
        Me.lblTaxSuspense.Visible = false
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(8, 26)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(161, 17)
        Me.lblTaxGroup.TabIndex = 109
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'Frame9
        '
        Me.Frame9.BackColor = System.Drawing.SystemColors.Control
        Me.Frame9.Controls.Add(Me.cboBankAccount)
        Me.Frame9.Controls.Add(Me.lblBankAccount)
        Me.Frame9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Frame9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame9.Location = New System.Drawing.Point(52, 12)
        Me.Frame9.Name = "Frame9"
        Me.Frame9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame9.Size = New System.Drawing.Size(585, 57)
        Me.Frame9.TabIndex = 4
        Me.Frame9.TabStop = false
        Me.Frame9.Text = "Bank"
        '
        'cboBankAccount
        '
        Me.cboBankAccount.DefaultId = "0"
        Me.cboBankAccount.FirstItem = ""
        Me.cboBankAccount.Id = 0
        Me.cboBankAccount.ListIndex = -1
        Me.cboBankAccount.Location = New System.Drawing.Point(160, 24)
        Me.cboBankAccount.Name = "cboBankAccount"
        Me.cboBankAccount.Size = New System.Drawing.Size(209, 21)
        Me.cboBankAccount.TabIndex = 6
        Me.cboBankAccount.ToolTipText = ""
        Me.cboBankAccount.WhatsThisHelpID = 0
        '
        'lblBankAccount
        '
        Me.lblBankAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccount.Location = New System.Drawing.Point(8, 24)
        Me.lblBankAccount.Name = "lblBankAccount"
        Me.lblBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccount.Size = New System.Drawing.Size(145, 17)
        Me.lblBankAccount.TabIndex = 5
        Me.lblBankAccount.Text = "Account:"
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(654, 364)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 126
        Me._cmdNext_3.Text = "&>>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = false
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 120
        Me._cmdPrevious_2.Text = "&<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = false
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.framEDI)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Controls.Add(Me.framXML)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraXMLExport)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Connectivity"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = true
        '
        'framEDI
        '
        Me.framEDI.BackColor = System.Drawing.SystemColors.Control
        Me.framEDI.Controls.Add(Me.txtEDIMessageCount)
        Me.framEDI.Controls.Add(Me.txtInsrMailboxNo)
        Me.framEDI.Controls.Add(Me.lblEDIMessageCount)
        Me.framEDI.Controls.Add(Me.lblInsrMailboxNo)
        Me.framEDI.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.framEDI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framEDI.Location = New System.Drawing.Point(58, 12)
        Me.framEDI.Name = "framEDI"
        Me.framEDI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framEDI.Size = New System.Drawing.Size(243, 89)
        Me.framEDI.TabIndex = 22
        Me.framEDI.TabStop = false
        Me.framEDI.Text = "EDI"
        '
        'txtEDIMessageCount
        '
        Me.txtEDIMessageCount.AcceptsReturn = true
        Me.txtEDIMessageCount.BackColor = System.Drawing.SystemColors.Window
        Me.txtEDIMessageCount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEDIMessageCount.Enabled = false
        Me.txtEDIMessageCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtEDIMessageCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEDIMessageCount.Location = New System.Drawing.Point(144, 52)
        Me.txtEDIMessageCount.MaxLength = 0
        Me.txtEDIMessageCount.Name = "txtEDIMessageCount"
        Me.txtEDIMessageCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEDIMessageCount.Size = New System.Drawing.Size(57, 21)
        Me.txtEDIMessageCount.TabIndex = 25
        Me.txtEDIMessageCount.Tag = "E;"
        '
        'txtInsrMailboxNo
        '
        Me.txtInsrMailboxNo.AcceptsReturn = true
        Me.txtInsrMailboxNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsrMailboxNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsrMailboxNo.Enabled = false
        Me.txtInsrMailboxNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtInsrMailboxNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsrMailboxNo.Location = New System.Drawing.Point(144, 24)
        Me.txtInsrMailboxNo.MaxLength = 0
        Me.txtInsrMailboxNo.Name = "txtInsrMailboxNo"
        Me.txtInsrMailboxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsrMailboxNo.Size = New System.Drawing.Size(89, 21)
        Me.txtInsrMailboxNo.TabIndex = 23
        Me.txtInsrMailboxNo.Tag = "E;"
        '
        'lblEDIMessageCount
        '
        Me.lblEDIMessageCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblEDIMessageCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEDIMessageCount.Enabled = false
        Me.lblEDIMessageCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblEDIMessageCount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEDIMessageCount.Location = New System.Drawing.Point(8, 54)
        Me.lblEDIMessageCount.Name = "lblEDIMessageCount"
        Me.lblEDIMessageCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEDIMessageCount.Size = New System.Drawing.Size(121, 17)
        Me.lblEDIMessageCount.TabIndex = 26
        Me.lblEDIMessageCount.Text = "&EDI  Message Count:"
        '
        'lblInsrMailboxNo
        '
        Me.lblInsrMailboxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsrMailboxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsrMailboxNo.Enabled = false
        Me.lblInsrMailboxNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblInsrMailboxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsrMailboxNo.Location = New System.Drawing.Point(8, 26)
        Me.lblInsrMailboxNo.Name = "lblInsrMailboxNo"
        Me.lblInsrMailboxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsrMailboxNo.Size = New System.Drawing.Size(137, 17)
        Me.lblInsrMailboxNo.TabIndex = 24
        Me.lblInsrMailboxNo.Text = "&Insurer Mailbox No:"
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(654, 364)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 128
        Me._cmdNext_4.Text = "&>>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = false
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 127
        Me._cmdPrevious_3.Text = "&<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = false
        '
        'framXML
        '
        Me.framXML.BackColor = System.Drawing.SystemColors.Control
        Me.framXML.Controls.Add(Me.txtProviderWebsite)
        Me.framXML.Controls.Add(Me.txtProviderUsername)
        Me.framXML.Controls.Add(Me.txtProviderPassword)
        Me.framXML.Controls.Add(Me.txtProviderTimeout)
        Me.framXML.Controls.Add(Me.txtProviderBrokerID)
        Me.framXML.Controls.Add(Me.lblProviderWebsite)
        Me.framXML.Controls.Add(Me.lblProviderUsername)
        Me.framXML.Controls.Add(Me.lblProviderPassword)
        Me.framXML.Controls.Add(Me.lblProviderTimeout)
        Me.framXML.Controls.Add(Me.lblProviderBrokerID)
        Me.framXML.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.framXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framXML.Location = New System.Drawing.Point(310, 12)
        Me.framXML.Name = "framXML"
        Me.framXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framXML.Size = New System.Drawing.Size(327, 171)
        Me.framXML.TabIndex = 36
        Me.framXML.TabStop = false
        Me.framXML.Text = "Stargate XML"
        '
        'txtProviderWebsite
        '
        Me.txtProviderWebsite.AcceptsReturn = true
        Me.txtProviderWebsite.BackColor = System.Drawing.SystemColors.Window
        Me.txtProviderWebsite.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProviderWebsite.Enabled = false
        Me.txtProviderWebsite.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProviderWebsite.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProviderWebsite.Location = New System.Drawing.Point(146, 24)
        Me.txtProviderWebsite.MaxLength = 0
        Me.txtProviderWebsite.Name = "txtProviderWebsite"
        Me.txtProviderWebsite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProviderWebsite.Size = New System.Drawing.Size(169, 21)
        Me.txtProviderWebsite.TabIndex = 37
        Me.txtProviderWebsite.Tag = "E;"
        '
        'txtProviderUsername
        '
        Me.txtProviderUsername.AcceptsReturn = true
        Me.txtProviderUsername.BackColor = System.Drawing.SystemColors.Window
        Me.txtProviderUsername.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProviderUsername.Enabled = false
        Me.txtProviderUsername.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProviderUsername.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProviderUsername.Location = New System.Drawing.Point(146, 79)
        Me.txtProviderUsername.MaxLength = 0
        Me.txtProviderUsername.Name = "txtProviderUsername"
        Me.txtProviderUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProviderUsername.Size = New System.Drawing.Size(113, 21)
        Me.txtProviderUsername.TabIndex = 41
        Me.txtProviderUsername.Tag = "E;"
        '
        'txtProviderPassword
        '
        Me.txtProviderPassword.AcceptsReturn = true
        Me.txtProviderPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtProviderPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProviderPassword.Enabled = false
        Me.txtProviderPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProviderPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProviderPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtProviderPassword.Location = New System.Drawing.Point(146, 107)
        Me.txtProviderPassword.MaxLength = 0
        Me.txtProviderPassword.Name = "txtProviderPassword"
        Me.txtProviderPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtProviderPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProviderPassword.Size = New System.Drawing.Size(113, 21)
        Me.txtProviderPassword.TabIndex = 43
        Me.txtProviderPassword.Tag = "E;"
        '
        'txtProviderTimeout
        '
        Me.txtProviderTimeout.AcceptsReturn = true
        Me.txtProviderTimeout.BackColor = System.Drawing.SystemColors.Window
        Me.txtProviderTimeout.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProviderTimeout.Enabled = false
        Me.txtProviderTimeout.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProviderTimeout.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProviderTimeout.Location = New System.Drawing.Point(146, 136)
        Me.txtProviderTimeout.MaxLength = 0
        Me.txtProviderTimeout.Name = "txtProviderTimeout"
        Me.txtProviderTimeout.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProviderTimeout.Size = New System.Drawing.Size(113, 21)
        Me.txtProviderTimeout.TabIndex = 45
        Me.txtProviderTimeout.Tag = "E;"
        '
        'txtProviderBrokerID
        '
        Me.txtProviderBrokerID.AcceptsReturn = true
        Me.txtProviderBrokerID.BackColor = System.Drawing.SystemColors.Window
        Me.txtProviderBrokerID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProviderBrokerID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProviderBrokerID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProviderBrokerID.Location = New System.Drawing.Point(146, 51)
        Me.txtProviderBrokerID.MaxLength = 0
        Me.txtProviderBrokerID.Name = "txtProviderBrokerID"
        Me.txtProviderBrokerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProviderBrokerID.Size = New System.Drawing.Size(113, 21)
        Me.txtProviderBrokerID.TabIndex = 39
        Me.txtProviderBrokerID.Tag = "E;"
        '
        'lblProviderWebsite
        '
        Me.lblProviderWebsite.BackColor = System.Drawing.SystemColors.Control
        Me.lblProviderWebsite.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProviderWebsite.Enabled = false
        Me.lblProviderWebsite.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProviderWebsite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProviderWebsite.Location = New System.Drawing.Point(10, 26)
        Me.lblProviderWebsite.Name = "lblProviderWebsite"
        Me.lblProviderWebsite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProviderWebsite.Size = New System.Drawing.Size(137, 17)
        Me.lblProviderWebsite.TabIndex = 38
        Me.lblProviderWebsite.Text = "Provider Website:"
        '
        'lblProviderUsername
        '
        Me.lblProviderUsername.BackColor = System.Drawing.SystemColors.Control
        Me.lblProviderUsername.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProviderUsername.Enabled = false
        Me.lblProviderUsername.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProviderUsername.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProviderUsername.Location = New System.Drawing.Point(10, 81)
        Me.lblProviderUsername.Name = "lblProviderUsername"
        Me.lblProviderUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProviderUsername.Size = New System.Drawing.Size(137, 17)
        Me.lblProviderUsername.TabIndex = 42
        Me.lblProviderUsername.Text = "Username:"
        '
        'lblProviderPassword
        '
        Me.lblProviderPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblProviderPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProviderPassword.Enabled = false
        Me.lblProviderPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProviderPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProviderPassword.Location = New System.Drawing.Point(10, 109)
        Me.lblProviderPassword.Name = "lblProviderPassword"
        Me.lblProviderPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProviderPassword.Size = New System.Drawing.Size(137, 17)
        Me.lblProviderPassword.TabIndex = 44
        Me.lblProviderPassword.Text = "Password:"
        '
        'lblProviderTimeout
        '
        Me.lblProviderTimeout.BackColor = System.Drawing.SystemColors.Control
        Me.lblProviderTimeout.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProviderTimeout.Enabled = false
        Me.lblProviderTimeout.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProviderTimeout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProviderTimeout.Location = New System.Drawing.Point(10, 138)
        Me.lblProviderTimeout.Name = "lblProviderTimeout"
        Me.lblProviderTimeout.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProviderTimeout.Size = New System.Drawing.Size(137, 17)
        Me.lblProviderTimeout.TabIndex = 46
        Me.lblProviderTimeout.Text = "Timeout:"
        '
        'lblProviderBrokerID
        '
        Me.lblProviderBrokerID.BackColor = System.Drawing.SystemColors.Control
        Me.lblProviderBrokerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProviderBrokerID.Enabled = false
        Me.lblProviderBrokerID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProviderBrokerID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProviderBrokerID.Location = New System.Drawing.Point(10, 53)
        Me.lblProviderBrokerID.Name = "lblProviderBrokerID"
        Me.lblProviderBrokerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProviderBrokerID.Size = New System.Drawing.Size(137, 17)
        Me.lblProviderBrokerID.TabIndex = 40
        Me.lblProviderBrokerID.Text = "Broker ID:"
        '
        'fraXMLExport
        '
        Me.fraXMLExport.BackColor = System.Drawing.SystemColors.Control
        Me.fraXMLExport.Controls.Add(Me.txtProcessingDays)
        Me.fraXMLExport.Controls.Add(Me.txtRemitter)
        Me.fraXMLExport.Controls.Add(Me.txtDirectDebitSupplierID)
        Me.fraXMLExport.Controls.Add(Me.txtDirectDebitSupplierName)
        Me.fraXMLExport.Controls.Add(Me.txtFinancialInstitutionCode)
        Me.fraXMLExport.Controls.Add(Me.lblProcessingDays)
        Me.fraXMLExport.Controls.Add(Me.lblRemitter)
        Me.fraXMLExport.Controls.Add(Me.lblDirectDebitSupplierID)
        Me.fraXMLExport.Controls.Add(Me.lblDirectDebitSupplierName)
        Me.fraXMLExport.Controls.Add(Me.lblFinancialInstitutionCode)
        Me.fraXMLExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraXMLExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraXMLExport.Location = New System.Drawing.Point(58, 186)
        Me.fraXMLExport.Name = "fraXMLExport"
        Me.fraXMLExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraXMLExport.Size = New System.Drawing.Size(579, 161)
        Me.fraXMLExport.TabIndex = 96
        Me.fraXMLExport.TabStop = false
        Me.fraXMLExport.Text = "Additional fields for XML Export"
        '
        'txtProcessingDays
        '
        Me.txtProcessingDays.AcceptsReturn = true
        Me.txtProcessingDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtProcessingDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProcessingDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtProcessingDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProcessingDays.Location = New System.Drawing.Point(182, 126)
        Me.txtProcessingDays.MaxLength = 0
        Me.txtProcessingDays.Name = "txtProcessingDays"
        Me.txtProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProcessingDays.Size = New System.Drawing.Size(171, 21)
        Me.txtProcessingDays.TabIndex = 105
        Me.txtProcessingDays.Tag = "E;"
        '
        'txtRemitter
        '
        Me.txtRemitter.AcceptsReturn = true
        Me.txtRemitter.BackColor = System.Drawing.SystemColors.Window
        Me.txtRemitter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRemitter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtRemitter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRemitter.Location = New System.Drawing.Point(182, 100)
        Me.txtRemitter.MaxLength = 0
        Me.txtRemitter.Name = "txtRemitter"
        Me.txtRemitter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRemitter.Size = New System.Drawing.Size(171, 21)
        Me.txtRemitter.TabIndex = 103
        Me.txtRemitter.Tag = "E;"
        '
        'txtDirectDebitSupplierID
        '
        Me.txtDirectDebitSupplierID.AcceptsReturn = true
        Me.txtDirectDebitSupplierID.BackColor = System.Drawing.SystemColors.Window
        Me.txtDirectDebitSupplierID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDirectDebitSupplierID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtDirectDebitSupplierID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDirectDebitSupplierID.Location = New System.Drawing.Point(182, 74)
        Me.txtDirectDebitSupplierID.MaxLength = 0
        Me.txtDirectDebitSupplierID.Name = "txtDirectDebitSupplierID"
        Me.txtDirectDebitSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDirectDebitSupplierID.Size = New System.Drawing.Size(171, 21)
        Me.txtDirectDebitSupplierID.TabIndex = 101
        Me.txtDirectDebitSupplierID.Tag = "E;"
        '
        'txtDirectDebitSupplierName
        '
        Me.txtDirectDebitSupplierName.AcceptsReturn = true
        Me.txtDirectDebitSupplierName.BackColor = System.Drawing.SystemColors.Window
        Me.txtDirectDebitSupplierName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDirectDebitSupplierName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtDirectDebitSupplierName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDirectDebitSupplierName.Location = New System.Drawing.Point(182, 48)
        Me.txtDirectDebitSupplierName.MaxLength = 0
        Me.txtDirectDebitSupplierName.Name = "txtDirectDebitSupplierName"
        Me.txtDirectDebitSupplierName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDirectDebitSupplierName.Size = New System.Drawing.Size(171, 21)
        Me.txtDirectDebitSupplierName.TabIndex = 99
        Me.txtDirectDebitSupplierName.Tag = "E;"
        '
        'txtFinancialInstitutionCode
        '
        Me.txtFinancialInstitutionCode.AcceptsReturn = true
        Me.txtFinancialInstitutionCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinancialInstitutionCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinancialInstitutionCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtFinancialInstitutionCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinancialInstitutionCode.Location = New System.Drawing.Point(182, 22)
        Me.txtFinancialInstitutionCode.MaxLength = 0
        Me.txtFinancialInstitutionCode.Name = "txtFinancialInstitutionCode"
        Me.txtFinancialInstitutionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinancialInstitutionCode.Size = New System.Drawing.Size(171, 21)
        Me.txtFinancialInstitutionCode.TabIndex = 97
        Me.txtFinancialInstitutionCode.Tag = "E;"
        '
        'lblProcessingDays
        '
        Me.lblProcessingDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblProcessingDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProcessingDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProcessingDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProcessingDays.Location = New System.Drawing.Point(8, 128)
        Me.lblProcessingDays.Name = "lblProcessingDays"
        Me.lblProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProcessingDays.Size = New System.Drawing.Size(163, 17)
        Me.lblProcessingDays.TabIndex = 106
        Me.lblProcessingDays.Text = "Processing Days:"
        '
        'lblRemitter
        '
        Me.lblRemitter.BackColor = System.Drawing.SystemColors.Control
        Me.lblRemitter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRemitter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblRemitter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRemitter.Location = New System.Drawing.Point(8, 102)
        Me.lblRemitter.Name = "lblRemitter"
        Me.lblRemitter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRemitter.Size = New System.Drawing.Size(163, 17)
        Me.lblRemitter.TabIndex = 104
        Me.lblRemitter.Text = "Remitter:"
        '
        'lblDirectDebitSupplierID
        '
        Me.lblDirectDebitSupplierID.BackColor = System.Drawing.SystemColors.Control
        Me.lblDirectDebitSupplierID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDirectDebitSupplierID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblDirectDebitSupplierID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDirectDebitSupplierID.Location = New System.Drawing.Point(8, 76)
        Me.lblDirectDebitSupplierID.Name = "lblDirectDebitSupplierID"
        Me.lblDirectDebitSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDirectDebitSupplierID.Size = New System.Drawing.Size(163, 17)
        Me.lblDirectDebitSupplierID.TabIndex = 102
        Me.lblDirectDebitSupplierID.Text = "Direct Debit Supplier ID:"
        '
        'lblDirectDebitSupplierName
        '
        Me.lblDirectDebitSupplierName.BackColor = System.Drawing.SystemColors.Control
        Me.lblDirectDebitSupplierName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDirectDebitSupplierName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblDirectDebitSupplierName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDirectDebitSupplierName.Location = New System.Drawing.Point(8, 50)
        Me.lblDirectDebitSupplierName.Name = "lblDirectDebitSupplierName"
        Me.lblDirectDebitSupplierName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDirectDebitSupplierName.Size = New System.Drawing.Size(163, 17)
        Me.lblDirectDebitSupplierName.TabIndex = 100
        Me.lblDirectDebitSupplierName.Text = "Direct Debit Supplier Name:"
        '
        'lblFinancialInstitutionCode
        '
        Me.lblFinancialInstitutionCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinancialInstitutionCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinancialInstitutionCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblFinancialInstitutionCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinancialInstitutionCode.Location = New System.Drawing.Point(8, 24)
        Me.lblFinancialInstitutionCode.Name = "lblFinancialInstitutionCode"
        Me.lblFinancialInstitutionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinancialInstitutionCode.Size = New System.Drawing.Size(163, 17)
        Me.lblFinancialInstitutionCode.TabIndex = 98
        Me.lblFinancialInstitutionCode.Text = "Financial Institution Code:"
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.uctPickListBranches)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Branches"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = true
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Branches/Companies"
        Me.uctPickListBranches.BusinessObject = "bSIRPFScheme.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"),Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = false
        Me.uctPickListBranches.Location = New System.Drawing.Point(54, 12)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "Source"
        Me.uctPickListBranches.Size = New System.Drawing.Size(589, 345)
        Me.uctPickListBranches.TabIndex = 7
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(654, 364)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_5.TabIndex = 129
        Me._cmdNext_5.Text = "&>>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = false
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 121
        Me._cmdPrevious_4.Text = "&<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = false
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.uctPickListProducts)
        Me._tabMainTab_TabPage6.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(699, 471)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "7 - Products"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = true
        '
        'uctPickListProducts
        '
        Me.uctPickListProducts.AvailableCaption = "Products"
        Me.uctPickListProducts.BusinessObject = "bSIRPFScheme.Business"
        Me.uctPickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.uctPickListProducts.ForeignKeys = CType(resources.GetObject("uctPickListProducts.ForeignKeys"),Microsoft.VisualBasic.Collection)
        Me.uctPickListProducts.IsSearchable = false
        Me.uctPickListProducts.Location = New System.Drawing.Point(54, 12)
        Me.uctPickListProducts.Name = "uctPickListProducts"
        Me.uctPickListProducts.PickListType = "Product"
        Me.uctPickListProducts.Size = New System.Drawing.Size(589, 345)
        Me.uctPickListProducts.TabIndex = 8
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(8, 364)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_5.TabIndex = 122
        Me._cmdPrevious_5.Text = "&<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = false
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(718, 558)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = true
        Me.KeyPreview = true
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Instalments Scheme"
        Me.tabMainTab.ResumeLayout(false)
        Me._tabMainTab_TabPage0.ResumeLayout(false)
        Me.fraScheme.ResumeLayout(false)
        Me.fraScheme.PerformLayout
        Me.framFP.ResumeLayout(false)
        Me.framFP.PerformLayout
        Me.Frame4.ResumeLayout(false)
        Me._tabMainTab_TabPage1.ResumeLayout(false)
        Me.SSFrame4.ResumeLayout(false)
        Me.SSFrame4.PerformLayout
        Me.SSFrame1.ResumeLayout(false)
        Me.SSFrame1.PerformLayout
        Me.Frame1.ResumeLayout(false)
        Me.fraCollectionNotification.ResumeLayout(false)
        Me.fraCollectionNotification.PerformLayout
        Me._tabMainTab_TabPage2.ResumeLayout(false)
        Me._tabMainTab_TabPage3.ResumeLayout(false)
        Me.Frame5.ResumeLayout(false)
        Me.Frame6.ResumeLayout(false)
        Me.fmeSuspense.ResumeLayout(false)
        Me.frameTax.ResumeLayout(false)
        Me.Frame9.ResumeLayout(false)
        Me._tabMainTab_TabPage4.ResumeLayout(false)
        Me.framEDI.ResumeLayout(false)
        Me.framEDI.PerformLayout
        Me.framXML.ResumeLayout(false)
        Me.framXML.PerformLayout
        Me.fraXMLExport.ResumeLayout(false)
        Me.fraXMLExport.PerformLayout
        Me._tabMainTab_TabPage5.ResumeLayout(false)
        Me._tabMainTab_TabPage6.ResumeLayout(false)
        CType(Me.listBoxComboBoxHelper1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(5) = _cmdNext_5
        Me.cmdNext(4) = _cmdNext_4
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
        Me.cmdNext(2) = _cmdNext_2
    End Sub
    Sub lvRates_InitializeColumnKeys()
        Me._lvRates_ColumnHeader_1.Name = ""
        Me._lvRates_ColumnHeader_2.Name = ""
        Me._lvRates_ColumnHeader_3.Name = ""
        Me._lvRates_ColumnHeader_4.Name = ""
        Me._lvRates_ColumnHeader_5.Name = ""
        Me._lvRates_ColumnHeader_6.Name = ""
    End Sub

    Public WithEvents chkSubAgentSpread As CheckBox
    Public WithEvents actSubAgentCommissionSuspense As UserControls.AccountLookup
    Public WithEvents lblSubAgentCommissionSuspense As Label
#End Region
End Class
