<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializefraReceipt()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
    Public WithEvents lblCashDrawer As System.Windows.Forms.Label
    Public WithEvents lblReceiptType As System.Windows.Forms.Label
    Public WithEvents lblBatch As System.Windows.Forms.Label
    Public WithEvents lblPaymentType As System.Windows.Forms.Label
    Public WithEvents panBatchNo As System.Windows.Forms.Label
    Public WithEvents panCashDrawer As System.Windows.Forms.Label
    Public WithEvents TxtWoffAmt As System.Windows.Forms.TextBox
    Public WithEvents txtChange As System.Windows.Forms.TextBox
    Public WithEvents txtTendered As System.Windows.Forms.TextBox
    Public WithEvents txtAmount As System.Windows.Forms.TextBox
    Public WithEvents lblWoff As System.Windows.Forms.Label
    Public WithEvents lblChange As System.Windows.Forms.Label
    Public WithEvents lblTendered As System.Windows.Forms.Label
    Public WithEvents lblAmount As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents uctAccountLookup As UserControls.AccountLookup
    Public WithEvents panAllocationStatus As System.Windows.Forms.Label
    Public WithEvents txtAllocationStatus As System.Windows.Forms.Label
    Public WithEvents lblAccount As System.Windows.Forms.Label
    Public WithEvents fraPostInfo As System.Windows.Forms.GroupBox
    Public WithEvents txtComments As System.Windows.Forms.TextBox
    Public WithEvents txtCollectionDate As System.Windows.Forms.TextBox
    Public WithEvents txtBankRef As System.Windows.Forms.TextBox
    Public WithEvents txtTheirRef As System.Windows.Forms.TextBox
    Public WithEvents txtOurRef As System.Windows.Forms.TextBox
    Public WithEvents txtMediaRef As System.Windows.Forms.TextBox
    Public WithEvents txtTransDate As System.Windows.Forms.TextBox
    Public WithEvents cmbMediaType As System.Windows.Forms.ComboBox
    Public WithEvents cboIssuer As System.Windows.Forms.ComboBox
    Public WithEvents lblComments As System.Windows.Forms.Label
    Public WithEvents lblCollectionDate As System.Windows.Forms.Label
    Public WithEvents lblIssuer As System.Windows.Forms.Label
    Public WithEvents lblBankRef As System.Windows.Forms.Label
    Public WithEvents lblTheirRef As System.Windows.Forms.Label
    Public WithEvents lblOurRef As System.Windows.Forms.Label
    Public WithEvents lblMediaType As System.Windows.Forms.Label
    Public WithEvents lblMediaRef As System.Windows.Forms.Label
    Public WithEvents lblTransDate As System.Windows.Forms.Label
    Public WithEvents fraTransInfo As System.Windows.Forms.GroupBox
    Public WithEvents chkLetter As System.Windows.Forms.CheckBox
    Public WithEvents cboPaymentType As System.Windows.Forms.ComboBox
    Public WithEvents cboReceiptType As System.Windows.Forms.ComboBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents uctReceiptCC As uctACTCreditCard.uctCreditCard
    Private WithEvents _fraReceipt_1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdClaimDebt As System.Windows.Forms.Button
    Public WithEvents cmdSalvage As System.Windows.Forms.Button
    Private WithEvents _fraReceipt_2 As System.Windows.Forms.GroupBox
    Public WithEvents uctPartyBankCombo2 As uctPartyBank.uctPartyBankCombo
    Public WithEvents lblReceiptAccountType As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents txtReverseDate As System.Windows.Forms.TextBox
    Public WithEvents txtReason As System.Windows.Forms.TextBox
    Public WithEvents cboReversalReason As System.Windows.Forms.ComboBox
    Public WithEvents lblReverseDate As System.Windows.Forms.Label
    Public WithEvents lblReason As System.Windows.Forms.Label
    Public WithEvents lblReversalReason As System.Windows.Forms.Label
    Private WithEvents _fraReceipt_3 As System.Windows.Forms.GroupBox
    Public WithEvents cboBank As System.Windows.Forms.ComboBox
    Public WithEvents txtChequeDate As System.Windows.Forms.TextBox
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents txtBankBranch As System.Windows.Forms.TextBox
    Public WithEvents txtBankLocation As System.Windows.Forms.TextBox
    Public WithEvents cboChequeType As System.Windows.Forms.ComboBox
    Public WithEvents cboChequeClearingType As System.Windows.Forms.ComboBox
    Public WithEvents lblBank As System.Windows.Forms.Label
    Public WithEvents lblChequeDate As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblBankBranch As System.Windows.Forms.Label
    Public WithEvents lblBankLocation As System.Windows.Forms.Label
    Public WithEvents lblChequeType As System.Windows.Forms.Label
    Public WithEvents lblChequeClearingType As System.Windows.Forms.Label
    Private WithEvents _fraReceipt_0 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents txtDatePresented As System.Windows.Forms.TextBox
    Public WithEvents txtConfirmation As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentReason As System.Windows.Forms.TextBox
    Public WithEvents chkInPossession As System.Windows.Forms.CheckBox
    Public WithEvents txtStopRequested As System.Windows.Forms.TextBox
    Public WithEvents lblDatePresented As System.Windows.Forms.Label
    Public WithEvents lblCancellationReason As System.Windows.Forms.Label
    Public WithEvents lblConfirmation As System.Windows.Forms.Label
    Public WithEvents lblStopRequested As System.Windows.Forms.Label
    Public WithEvents fraBank As System.Windows.Forms.GroupBox
    Public WithEvents uctPaymentCC As uctACTCreditCard.uctCreditCard
    Public WithEvents fraPaymentCreditCard As System.Windows.Forms.GroupBox
    Public WithEvents txtPayeeName As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentAccountCode As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentBranchCode As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentReference1 As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentReference2 As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentExpiryDate As System.Windows.Forms.TextBox
    Public WithEvents lblPayeeName As System.Windows.Forms.Label
    Public WithEvents lblPaymentReference1 As System.Windows.Forms.Label
    Public WithEvents lblPaymentReference2 As System.Windows.Forms.Label
    Public WithEvents lblPaymentExpiryDate As System.Windows.Forms.Label
    Public WithEvents lblPaymentBranchCode As System.Windows.Forms.Label
    Public WithEvents lblPaymentAccountCode As System.Windows.Forms.Label
    Public WithEvents fraPayee As System.Windows.Forms.GroupBox
    Public WithEvents cboPaymentStatus As System.Windows.Forms.ComboBox
    Public WithEvents lblPaymentStatus As System.Windows.Forms.Label
    Public WithEvents fraPayment As System.Windows.Forms.GroupBox
    Public WithEvents uctPartyBankCombo1 As uctPartyBank.uctPartyBankCombo
    Public WithEvents lblPaymentAccountType As System.Windows.Forms.Label
    Public WithEvents fraPaymentAccountType As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents txtContactName As System.Windows.Forms.TextBox
    Public WithEvents txtFurtherDetails As System.Windows.Forms.TextBox
    Public WithEvents uctAddress As PMAddressControl.uctPMAddressControl
    Public WithEvents lblContactName As System.Windows.Forms.Label
    Public WithEvents lblFurtherDetails As System.Windows.Forms.Label
    Public WithEvents fraFurtherDetails As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents cboUnderwritingYear As PMLookupControl.cboPMLookup
    Public WithEvents lblUnderwritingYear As System.Windows.Forms.Label
    Public WithEvents fraAddFields As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Private WithEvents _lvInstalments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvInstalments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvInstalments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvInstalments_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvInstalments_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvInstalments As System.Windows.Forms.ListView
    Public WithEvents fralvInstalments As System.Windows.Forms.Panel
    Public WithEvents cboInstalment As System.Windows.Forms.ComboBox
    Public WithEvents SSOveralPlanTotal As System.Windows.Forms.Label
    Public WithEvents SSThisPlanTotal As System.Windows.Forms.Label
    Public WithEvents lblOveralTotal As System.Windows.Forms.Label
    Public WithEvents lblPlan As System.Windows.Forms.Label
    Public WithEvents lblTotal As System.Windows.Forms.Label
    Public WithEvents fraInstalments As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Private WithEvents _lvwBGDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBGDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBGDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBGDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBGDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwBGDetails As System.Windows.Forms.ListView
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblBGPolOutstandingAmt As System.Windows.Forms.Label
    Public WithEvents lblBGPolAmtToBePosted As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdCommit As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public fraReceipt(3) As System.Windows.Forms.GroupBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblCashDrawer = New System.Windows.Forms.Label
        Me.lblReceiptType = New System.Windows.Forms.Label
        Me.lblBatch = New System.Windows.Forms.Label
        Me.lblPaymentType = New System.Windows.Forms.Label
        Me.panBatchNo = New System.Windows.Forms.Label
        Me.panCashDrawer = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtTaxAmount = New System.Windows.Forms.TextBox()
        Me.lblTaxAmount = New System.Windows.Forms.Label()
        Me.cboTaxBand = New System.Windows.Forms.ComboBox()
        Me.lblTaxBand = New System.Windows.Forms.Label()
        Me.txtSplitTotal = New System.Windows.Forms.TextBox
        Me.lblSplitTotal = New System.Windows.Forms.Label
        Me.TxtWoffAmt = New System.Windows.Forms.TextBox
        Me.txtChange = New System.Windows.Forms.TextBox
        Me.txtTendered = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.lblWoff = New System.Windows.Forms.Label
        Me.lblChange = New System.Windows.Forms.Label
        Me.lblTendered = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.fraPostInfo = New System.Windows.Forms.GroupBox
        Me.chkIsLeadAccount = New System.Windows.Forms.CheckBox
        Me.uctAccountLookup = New UserControls.AccountLookup
        Me.panAllocationStatus = New System.Windows.Forms.Label
        Me.txtAllocationStatus = New System.Windows.Forms.Label
        Me.lblAccount = New System.Windows.Forms.Label
        Me.fraTransInfo = New System.Windows.Forms.GroupBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.txtCollectionDate = New System.Windows.Forms.TextBox
        Me.txtBankRef = New System.Windows.Forms.TextBox
        Me.txtTheirRef = New System.Windows.Forms.TextBox
        Me.txtOurRef = New System.Windows.Forms.TextBox
        Me.txtMediaRef = New System.Windows.Forms.TextBox
        Me.txtTransDate = New System.Windows.Forms.TextBox
        Me.cmbMediaType = New System.Windows.Forms.ComboBox
        Me.cboIssuer = New System.Windows.Forms.ComboBox
        Me.lblComments = New System.Windows.Forms.Label
        Me.lblCollectionDate = New System.Windows.Forms.Label
        Me.lblIssuer = New System.Windows.Forms.Label
        Me.lblBankRef = New System.Windows.Forms.Label
        Me.lblTheirRef = New System.Windows.Forms.Label
        Me.lblOurRef = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.lblMediaRef = New System.Windows.Forms.Label
        Me.lblTransDate = New System.Windows.Forms.Label
        Me.chkLetter = New System.Windows.Forms.CheckBox
        Me.cboPaymentType = New System.Windows.Forms.ComboBox
        Me.cboReceiptType = New System.Windows.Forms.ComboBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._fraReceipt_1 = New System.Windows.Forms.GroupBox
        Me.uctReceiptCC = New uctACTCreditCard.uctCreditCard
        Me._fraReceipt_2 = New System.Windows.Forms.GroupBox
        Me.cmdClaimDebt = New System.Windows.Forms.Button
        Me.cmdSalvage = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.uctPartyBankCombo2 = New uctPartyBank.uctPartyBankCombo
        Me.lblReceiptAccountType = New System.Windows.Forms.Label
        Me._fraReceipt_3 = New System.Windows.Forms.GroupBox
        Me.txtReverseDate = New System.Windows.Forms.TextBox
        Me.txtReason = New System.Windows.Forms.TextBox
        Me.cboReversalReason = New System.Windows.Forms.ComboBox
        Me.lblReverseDate = New System.Windows.Forms.Label
        Me.lblReason = New System.Windows.Forms.Label
        Me.lblReversalReason = New System.Windows.Forms.Label
        Me._fraReceipt_0 = New System.Windows.Forms.GroupBox
        Me.cboBank = New System.Windows.Forms.ComboBox
        Me.txtChequeDate = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtBankBranch = New System.Windows.Forms.TextBox
        Me.txtBankLocation = New System.Windows.Forms.TextBox
        Me.cboChequeType = New System.Windows.Forms.ComboBox
        Me.cboChequeClearingType = New System.Windows.Forms.ComboBox
        Me.lblBank = New System.Windows.Forms.Label
        Me.lblChequeDate = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblBankBranch = New System.Windows.Forms.Label
        Me.lblBankLocation = New System.Windows.Forms.Label
        Me.lblChequeType = New System.Windows.Forms.Label
        Me.lblChequeClearingType = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraBank = New System.Windows.Forms.GroupBox
        Me.txtDatePresented = New System.Windows.Forms.TextBox
        Me.txtConfirmation = New System.Windows.Forms.TextBox
        Me.txtPaymentReason = New System.Windows.Forms.TextBox
        Me.chkInPossession = New System.Windows.Forms.CheckBox
        Me.txtStopRequested = New System.Windows.Forms.TextBox
        Me.lblDatePresented = New System.Windows.Forms.Label
        Me.lblCancellationReason = New System.Windows.Forms.Label
        Me.lblConfirmation = New System.Windows.Forms.Label
        Me.lblStopRequested = New System.Windows.Forms.Label
        Me.fraPaymentCreditCard = New System.Windows.Forms.GroupBox
        Me.uctPaymentCC = New uctACTCreditCard.uctCreditCard
        Me.fraPayee = New System.Windows.Forms.GroupBox
        Me.txtPayeeName = New System.Windows.Forms.TextBox
        Me.txtPaymentAccountCode = New System.Windows.Forms.TextBox
        Me.txtPaymentBranchCode = New System.Windows.Forms.TextBox
        Me.txtPaymentReference1 = New System.Windows.Forms.TextBox
        Me.txtPaymentReference2 = New System.Windows.Forms.TextBox
        Me.txtPaymentExpiryDate = New System.Windows.Forms.TextBox
        Me.lblPayeeName = New System.Windows.Forms.Label
        Me.lblPaymentReference1 = New System.Windows.Forms.Label
        Me.lblPaymentReference2 = New System.Windows.Forms.Label
        Me.lblPaymentExpiryDate = New System.Windows.Forms.Label
        Me.lblPaymentBranchCode = New System.Windows.Forms.Label
        Me.lblPaymentAccountCode = New System.Windows.Forms.Label
        Me.fraPayment = New System.Windows.Forms.GroupBox
        Me.cboPaymentStatus = New System.Windows.Forms.ComboBox
        Me.lblPaymentStatus = New System.Windows.Forms.Label
        Me.fraPaymentAccountType = New System.Windows.Forms.GroupBox
        Me.uctPartyBankCombo1 = New uctPartyBank.uctPartyBankCombo
        Me.lblPaymentAccountType = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me.fraFurtherDetails = New System.Windows.Forms.GroupBox
        Me.txtContactName = New System.Windows.Forms.TextBox
        Me.txtFurtherDetails = New System.Windows.Forms.TextBox
        Me.uctAddress = New PMAddressControl.uctPMAddressControl
        Me.lblContactName = New System.Windows.Forms.Label
        Me.lblFurtherDetails = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.fraAddFields = New System.Windows.Forms.GroupBox
        Me.cboUnderwritingYear = New PMLookupControl.cboPMLookup
        Me.lblUnderwritingYear = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.fraInstalments = New System.Windows.Forms.GroupBox
        Me.fralvInstalments = New System.Windows.Forms.Panel
        Me.lvInstalments = New System.Windows.Forms.ListView
        Me._lvInstalments_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvInstalments_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvInstalments_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvInstalments_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvInstalments_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.cboInstalment = New System.Windows.Forms.ComboBox
        Me.SSOveralPlanTotal = New System.Windows.Forms.Label
        Me.SSThisPlanTotal = New System.Windows.Forms.Label
        Me.lblOveralTotal = New System.Windows.Forms.Label
        Me.lblPlan = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.lvwBGDetails = New System.Windows.Forms.ListView
        Me._lvwBGDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblBGPolOutstandingAmt = New System.Windows.Forms.Label
        Me.lblBGPolAmtToBePosted = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdCommit = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdOK = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.txtBIC = New System.Windows.Forms.TextBox()
        Me.lblBIC = New System.Windows.Forms.Label()
        Me.txtIBAN = New System.Windows.Forms.TextBox()
        Me.lblIBAN = New System.Windows.Forms.Label()
		  Me.cboInsuranceRef = New System.Windows.Forms.ComboBox()
        Me.lblInsuranceRef = New System.Windows.Forms.Label()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraPostInfo.SuspendLayout()
        Me.fraTransInfo.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._fraReceipt_1.SuspendLayout()
        Me._fraReceipt_2.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._fraReceipt_3.SuspendLayout()
        Me._fraReceipt_0.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraBank.SuspendLayout()
        Me.fraPaymentCreditCard.SuspendLayout()
        Me.fraPayee.SuspendLayout()
        Me.fraPayment.SuspendLayout()
        Me.fraPaymentAccountType.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraFurtherDetails.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraAddFields.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraInstalments.SuspendLayout()
        Me.fralvInstalments.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.Frame3.SuspendLayout()
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
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(86, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(7, 8)
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(630, 500)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCashDrawer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReceiptType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBatch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPaymentType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panBatchNo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panCashDrawer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPostInfo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraTransInfo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkLetter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPaymentType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboReceiptType)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Summary"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblCashDrawer
        '
        Me.lblCashDrawer.BackColor = System.Drawing.SystemColors.Control
        Me.lblCashDrawer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCashDrawer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashDrawer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCashDrawer.Location = New System.Drawing.Point(297, 20)
        Me.lblCashDrawer.Name = "lblCashDrawer"
        Me.lblCashDrawer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCashDrawer.Size = New System.Drawing.Size(89, 17)
        Me.lblCashDrawer.TabIndex = 3
        Me.lblCashDrawer.Text = "Cash Drawer:"
        '
        'lblReceiptType
        '
        Me.lblReceiptType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptType.Location = New System.Drawing.Point(24, 52)
        Me.lblReceiptType.Name = "lblReceiptType"
        Me.lblReceiptType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptType.Size = New System.Drawing.Size(105, 17)
        Me.lblReceiptType.TabIndex = 6
        Me.lblReceiptType.Text = "Receipt Type:"
        '
        'lblBatch
        '
        Me.lblBatch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBatch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBatch.Location = New System.Drawing.Point(24, 21)
        Me.lblBatch.Name = "lblBatch"
        Me.lblBatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBatch.Size = New System.Drawing.Size(97, 16)
        Me.lblBatch.TabIndex = 1
        Me.lblBatch.Text = "Batch:"
        '
        'lblPaymentType
        '
        Me.lblPaymentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentType.Location = New System.Drawing.Point(24, 52)
        Me.lblPaymentType.Name = "lblPaymentType"
        Me.lblPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentType.Size = New System.Drawing.Size(105, 17)
        Me.lblPaymentType.TabIndex = 5
        Me.lblPaymentType.Text = "Payment Type:"
        '
        'panBatchNo
        '
        Me.panBatchNo.BackColor = System.Drawing.SystemColors.Control
        Me.panBatchNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panBatchNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.panBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panBatchNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panBatchNo.Location = New System.Drawing.Point(144, 20)
        Me.panBatchNo.Name = "panBatchNo"
        Me.panBatchNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panBatchNo.Size = New System.Drawing.Size(137, 17)
        Me.panBatchNo.TabIndex = 2
        '
        'panCashDrawer
        '
        Me.panCashDrawer.BackColor = System.Drawing.SystemColors.Control
        Me.panCashDrawer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panCashDrawer.Cursor = System.Windows.Forms.Cursors.Default
        Me.panCashDrawer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panCashDrawer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panCashDrawer.Location = New System.Drawing.Point(392, 20)
        Me.panCashDrawer.Name = "panCashDrawer"
        Me.panCashDrawer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panCashDrawer.Size = New System.Drawing.Size(201, 17)
        Me.panCashDrawer.TabIndex = 4
        '
        'Frame1
        '
        Me.Frame1.AutoSize = True
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtTaxAmount)
        Me.Frame1.Controls.Add(Me.lblTaxAmount)
        Me.Frame1.Controls.Add(Me.cboTaxBand)
        Me.Frame1.Controls.Add(Me.lblTaxBand)
        Me.Frame1.Controls.Add(Me.txtSplitTotal)
        Me.Frame1.Controls.Add(Me.lblSplitTotal)
        Me.Frame1.Controls.Add(Me.TxtWoffAmt)
        Me.Frame1.Controls.Add(Me.txtChange)
        Me.Frame1.Controls.Add(Me.txtTendered)
        Me.Frame1.Controls.Add(Me.txtAmount)
        Me.Frame1.Controls.Add(Me.lblWoff)
        Me.Frame1.Controls.Add(Me.lblChange)
        Me.Frame1.Controls.Add(Me.lblTendered)
        Me.Frame1.Controls.Add(Me.lblAmount)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 319)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(577, 136)
        Me.Frame1.TabIndex = 34
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Amounts"
        '
        'txtTaxAmount
        '
        Me.txtTaxAmount.AcceptsReturn = True
        Me.txtTaxAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxAmount.Location = New System.Drawing.Point(400, 56)
        Me.txtTaxAmount.MaxLength = 0
        Me.txtTaxAmount.Name = "txtTaxAmount"
        Me.txtTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxAmount.Size = New System.Drawing.Size(145, 21)
        Me.txtTaxAmount.TabIndex = 48
        Me.txtTaxAmount.Tag = "F;FMT;$;"
        Me.txtTaxAmount.Visible = False
        '
        'lblTaxAmount
        '
        Me.lblTaxAmount.AutoSize = True
        Me.lblTaxAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxAmount.Location = New System.Drawing.Point(288, 61)
        Me.lblTaxAmount.Name = "lblTaxAmount"
        Me.lblTaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxAmount.Size = New System.Drawing.Size(75, 13)
        Me.lblTaxAmount.TabIndex = 47
        Me.lblTaxAmount.Text = "Tax Amount"
        Me.lblTaxAmount.Visible = False
        '
        'cboTaxBand
        '
        Me.cboTaxBand.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxBand.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxBand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxBand.Location = New System.Drawing.Point(143, 56)
        Me.cboTaxBand.Name = "cboTaxBand"
        Me.cboTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxBand.Size = New System.Drawing.Size(137, 21)
        Me.cboTaxBand.TabIndex = 46
        Me.cboTaxBand.Tag = "F;"
        Me.cboTaxBand.Visible = False
        '
        'lblTaxBand
        '
        Me.lblTaxBand.AutoSize = True
        Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxBand.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxBand.Location = New System.Drawing.Point(8, 59)
        Me.lblTaxBand.Name = "lblTaxBand"
        Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxBand.Size = New System.Drawing.Size(60, 13)
        Me.lblTaxBand.TabIndex = 45
        Me.lblTaxBand.Text = "Tax Band"
        Me.lblTaxBand.Visible = False
        '
        'txtSplitTotal
        '
        Me.txtSplitTotal.Location = New System.Drawing.Point(143, 59)
        Me.txtSplitTotal.Name = "txtSplitTotal"
        Me.txtSplitTotal.Size = New System.Drawing.Size(137, 21)
        Me.txtSplitTotal.TabIndex = 44
        Me.txtSplitTotal.Tag = "F;FMT;$;"
        Me.txtSplitTotal.Visible = False
        '
        'lblSplitTotal
        '
        Me.lblSplitTotal.AutoSize = True
        Me.lblSplitTotal.Location = New System.Drawing.Point(8, 59)
        Me.lblSplitTotal.Name = "lblSplitTotal"
        Me.lblSplitTotal.Size = New System.Drawing.Size(39, 13)
        Me.lblSplitTotal.TabIndex = 43
        Me.lblSplitTotal.Text = "Total:"
        Me.lblSplitTotal.Visible = False
        '
        'TxtWoffAmt
        '
        Me.TxtWoffAmt.AcceptsReturn = True
        Me.TxtWoffAmt.BackColor = System.Drawing.SystemColors.Window
        Me.TxtWoffAmt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtWoffAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtWoffAmt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtWoffAmt.Location = New System.Drawing.Point(400, 24)
        Me.TxtWoffAmt.MaxLength = 0
        Me.TxtWoffAmt.Name = "TxtWoffAmt"
        Me.TxtWoffAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtWoffAmt.Size = New System.Drawing.Size(145, 21)
        Me.TxtWoffAmt.TabIndex = 40
        Me.TxtWoffAmt.Tag = "F;FMT;$;"
        '
        'txtChange
        '
        Me.txtChange.AcceptsReturn = True
        Me.txtChange.BackColor = System.Drawing.SystemColors.Window
        Me.txtChange.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChange.Enabled = False
        Me.txtChange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChange.Location = New System.Drawing.Point(400, 56)
        Me.txtChange.MaxLength = 0
        Me.txtChange.Name = "txtChange"
        Me.txtChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChange.Size = New System.Drawing.Size(145, 21)
        Me.txtChange.TabIndex = 42
        Me.txtChange.Tag = "F;FMT;$;"
        '
        'txtTendered
        '
        Me.txtTendered.AcceptsReturn = True
        Me.txtTendered.BackColor = System.Drawing.SystemColors.Window
        Me.txtTendered.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTendered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTendered.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTendered.Location = New System.Drawing.Point(144, 58)
        Me.txtTendered.MaxLength = 0
        Me.txtTendered.Name = "txtTendered"
        Me.txtTendered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTendered.Size = New System.Drawing.Size(137, 21)
        Me.txtTendered.TabIndex = 38
        Me.txtTendered.Tag = "F;FMT;$;"
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(144, 24)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(137, 21)
        Me.txtAmount.TabIndex = 36
        Me.txtAmount.Tag = "F;FMT;$;"
        '
        'lblWoff
        '
        Me.lblWoff.BackColor = System.Drawing.SystemColors.Control
        Me.lblWoff.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWoff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWoff.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWoff.Location = New System.Drawing.Point(286, 27)
        Me.lblWoff.Name = "lblWoff"
        Me.lblWoff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWoff.Size = New System.Drawing.Size(135, 17)
        Me.lblWoff.TabIndex = 39
        Me.lblWoff.Text = "Write Off Amount:"
        '
        'lblChange
        '
        Me.lblChange.BackColor = System.Drawing.SystemColors.Control
        Me.lblChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChange.Location = New System.Drawing.Point(288, 59)
        Me.lblChange.Name = "lblChange"
        Me.lblChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChange.Size = New System.Drawing.Size(65, 17)
        Me.lblChange.TabIndex = 41
        Me.lblChange.Text = "Change:"
        '
        'lblTendered
        '
        Me.lblTendered.BackColor = System.Drawing.SystemColors.Control
        Me.lblTendered.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTendered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTendered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTendered.Location = New System.Drawing.Point(8, 59)
        Me.lblTendered.Name = "lblTendered"
        Me.lblTendered.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTendered.Size = New System.Drawing.Size(129, 17)
        Me.lblTendered.TabIndex = 37
        Me.lblTendered.Text = "Tendered:"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(8, 27)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(56, 13)
        Me.lblAmount.TabIndex = 35
        Me.lblAmount.Text = "Amount:"
        '
        'fraPostInfo
        '
        Me.fraPostInfo.AutoSize = True
        Me.fraPostInfo.BackColor = System.Drawing.SystemColors.Control
		Me.fraPostInfo.Controls.Add(Me.lblInsuranceRef)
        Me.fraPostInfo.Controls.Add(Me.cboInsuranceRef)
        Me.fraPostInfo.Controls.Add(Me.chkIsLeadAccount)
        Me.fraPostInfo.Controls.Add(Me.uctAccountLookup)
        Me.fraPostInfo.Controls.Add(Me.panAllocationStatus)
        Me.fraPostInfo.Controls.Add(Me.txtAllocationStatus)
        Me.fraPostInfo.Controls.Add(Me.lblAccount)
        Me.fraPostInfo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPostInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPostInfo.Location = New System.Drawing.Point(16, 224)
        Me.fraPostInfo.Name = "fraPostInfo"
        Me.fraPostInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPostInfo.Size = New System.Drawing.Size(577, 96)
        Me.fraPostInfo.TabIndex = 29
        Me.fraPostInfo.TabStop = False
        Me.fraPostInfo.Text = "Posting Information"
        '
        'chkIsLeadAccount
        '
        Me.chkIsLeadAccount.AutoSize = True
        Me.chkIsLeadAccount.Location = New System.Drawing.Point(401, 56)
        Me.chkIsLeadAccount.Name = "chkIsLeadAccount"
        Me.chkIsLeadAccount.Size = New System.Drawing.Size(102, 17)
        Me.chkIsLeadAccount.TabIndex = 34
        Me.chkIsLeadAccount.Text = "Lead Account"
        Me.chkIsLeadAccount.UseVisualStyleBackColor = True
        Me.chkIsLeadAccount.Visible = False
        '
        'uctAccountLookup
        '
        Me.uctAccountLookup.AccountId = 0
        Me.uctAccountLookup.AllowStoppedAccounts = False
        Me.uctAccountLookup.BackStyle = 0
        Me.uctAccountLookup.CompanyId = 0
        Me.uctAccountLookup.Default_Renamed = False
        Me.uctAccountLookup.Location = New System.Drawing.Point(144, 24)
        Me.uctAccountLookup.LookupCaption = "..."
        Me.uctAccountLookup.LookupHeight = 285
        Me.uctAccountLookup.LookupLeft = 1695
        Me.uctAccountLookup.LookupTextLeft = 0
        Me.uctAccountLookup.LookupTextWidth = 1695
        Me.uctAccountLookup.LookupWidth = 360
        Me.uctAccountLookup.Name = "uctAccountLookup"
        Me.uctAccountLookup.OnlyUpdatableAccounts = True
        Me.uctAccountLookup.SelLength = 0
        Me.uctAccountLookup.SelStart = 0
        Me.uctAccountLookup.SelText = ""
        Me.uctAccountLookup.ShowEditOnFindAccount = False
        Me.uctAccountLookup.Size = New System.Drawing.Size(137, 19)
        Me.uctAccountLookup.TabIndex = 31
        Me.uctAccountLookup.Tag = "F;M;"
        Me.uctAccountLookup.ToolTipText = ""
        '
        'panAllocationStatus
        '
        Me.panAllocationStatus.BackColor = System.Drawing.SystemColors.Control
        Me.panAllocationStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAllocationStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.panAllocationStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAllocationStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panAllocationStatus.Location = New System.Drawing.Point(400, 24)
        Me.panAllocationStatus.Name = "panAllocationStatus"
        Me.panAllocationStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panAllocationStatus.Size = New System.Drawing.Size(145, 19)
        Me.panAllocationStatus.TabIndex = 33
        '
        'txtAllocationStatus
        '
        Me.txtAllocationStatus.BackColor = System.Drawing.SystemColors.Control
        Me.txtAllocationStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtAllocationStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllocationStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtAllocationStatus.Location = New System.Drawing.Point(288, 27)
        Me.txtAllocationStatus.Name = "txtAllocationStatus"
        Me.txtAllocationStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllocationStatus.Size = New System.Drawing.Size(105, 17)
        Me.txtAllocationStatus.TabIndex = 32
        Me.txtAllocationStatus.Text = "Allocation Status:"
        '
        'lblAccount
        '
        Me.lblAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccount.Location = New System.Drawing.Point(8, 27)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccount.Size = New System.Drawing.Size(121, 17)
        Me.lblAccount.TabIndex = 30
        Me.lblAccount.Text = "Account:"
        '
        'fraTransInfo
        '
        Me.fraTransInfo.BackColor = System.Drawing.SystemColors.Control
        Me.fraTransInfo.Controls.Add(Me.txtComments)
        Me.fraTransInfo.Controls.Add(Me.txtCollectionDate)
        Me.fraTransInfo.Controls.Add(Me.txtBankRef)
        Me.fraTransInfo.Controls.Add(Me.txtTheirRef)
        Me.fraTransInfo.Controls.Add(Me.txtOurRef)
        Me.fraTransInfo.Controls.Add(Me.txtMediaRef)
        Me.fraTransInfo.Controls.Add(Me.txtTransDate)
        Me.fraTransInfo.Controls.Add(Me.cmbMediaType)
        Me.fraTransInfo.Controls.Add(Me.cboIssuer)
        Me.fraTransInfo.Controls.Add(Me.lblComments)
        Me.fraTransInfo.Controls.Add(Me.lblCollectionDate)
        Me.fraTransInfo.Controls.Add(Me.lblIssuer)
        Me.fraTransInfo.Controls.Add(Me.lblBankRef)
        Me.fraTransInfo.Controls.Add(Me.lblTheirRef)
        Me.fraTransInfo.Controls.Add(Me.lblOurRef)
        Me.fraTransInfo.Controls.Add(Me.lblMediaType)
        Me.fraTransInfo.Controls.Add(Me.lblMediaRef)
        Me.fraTransInfo.Controls.Add(Me.lblTransDate)
        Me.fraTransInfo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTransInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTransInfo.Location = New System.Drawing.Point(16, 80)
        Me.fraTransInfo.Name = "fraTransInfo"
        Me.fraTransInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTransInfo.Size = New System.Drawing.Size(577, 143)
        Me.fraTransInfo.TabIndex = 10
        Me.fraTransInfo.TabStop = False
        Me.fraTransInfo.Text = "Transaction Information"
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Enabled = False
        Me.txtComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(400, 40)
        Me.txtComments.MaxLength = 0
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ReadOnly = True
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.Size = New System.Drawing.Size(145, 21)
        Me.txtComments.TabIndex = 22
        Me.txtComments.Tag = "F;"
        Me.txtComments.Visible = False
        '
        'txtCollectionDate
        '
        Me.txtCollectionDate.AcceptsReturn = True
        Me.txtCollectionDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCollectionDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCollectionDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCollectionDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCollectionDate.Location = New System.Drawing.Point(400, 14)
        Me.txtCollectionDate.MaxLength = 0
        Me.txtCollectionDate.Name = "txtCollectionDate"
        Me.txtCollectionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCollectionDate.Size = New System.Drawing.Size(145, 21)
        Me.txtCollectionDate.TabIndex = 20
        Me.txtCollectionDate.Tag = "F;M;DT;"
        Me.txtCollectionDate.Visible = False
        '
        'txtBankRef
        '
        Me.txtBankRef.AcceptsReturn = True
        Me.txtBankRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankRef.Location = New System.Drawing.Point(400, 92)
        Me.txtBankRef.MaxLength = 0
        Me.txtBankRef.Name = "txtBankRef"
        Me.txtBankRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankRef.Size = New System.Drawing.Size(145, 21)
        Me.txtBankRef.TabIndex = 26
        Me.txtBankRef.Tag = "F;"
        '
        'txtTheirRef
        '
        Me.txtTheirRef.AcceptsReturn = True
        Me.txtTheirRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtTheirRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheirRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheirRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTheirRef.Location = New System.Drawing.Point(400, 118)
        Me.txtTheirRef.MaxLength = 0
        Me.txtTheirRef.Name = "txtTheirRef"
        Me.txtTheirRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTheirRef.Size = New System.Drawing.Size(145, 21)
        Me.txtTheirRef.TabIndex = 28
        Me.txtTheirRef.Tag = "F;"
        '
        'txtOurRef
        '
        Me.txtOurRef.AcceptsReturn = True
        Me.txtOurRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtOurRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOurRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOurRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOurRef.Location = New System.Drawing.Point(144, 94)
        Me.txtOurRef.MaxLength = 30
        Me.txtOurRef.Name = "txtOurRef"
        Me.txtOurRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOurRef.Size = New System.Drawing.Size(137, 21)
        Me.txtOurRef.TabIndex = 18
        Me.txtOurRef.Tag = "F;"
        '
        'txtMediaRef
        '
        Me.txtMediaRef.AcceptsReturn = True
        Me.txtMediaRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtMediaRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMediaRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMediaRef.Location = New System.Drawing.Point(144, 70)
        Me.txtMediaRef.MaxLength = 100
        Me.txtMediaRef.Name = "txtMediaRef"
        Me.txtMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMediaRef.Size = New System.Drawing.Size(137, 21)
        Me.txtMediaRef.TabIndex = 16
        Me.txtMediaRef.Tag = "F;"
        '
        'txtTransDate
        '
        Me.txtTransDate.AcceptsReturn = True
        Me.txtTransDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransDate.Location = New System.Drawing.Point(144, 22)
        Me.txtTransDate.MaxLength = 0
        Me.txtTransDate.Name = "txtTransDate"
        Me.txtTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransDate.Size = New System.Drawing.Size(137, 21)
        Me.txtTransDate.TabIndex = 12
        Me.txtTransDate.Tag = "F;M;DT;"
        '
        'cmbMediaType
        '
        Me.cmbMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbMediaType.Location = New System.Drawing.Point(144, 46)
        Me.cmbMediaType.Name = "cmbMediaType"
        Me.cmbMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbMediaType.Size = New System.Drawing.Size(137, 21)
        Me.cmbMediaType.TabIndex = 14
        Me.cmbMediaType.Tag = "F;M;"
        '
        'cboIssuer
        '
        Me.cboIssuer.BackColor = System.Drawing.SystemColors.Window
        Me.cboIssuer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboIssuer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIssuer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboIssuer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboIssuer.Location = New System.Drawing.Point(400, 64)
        Me.cboIssuer.Name = "cboIssuer"
        Me.cboIssuer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboIssuer.Size = New System.Drawing.Size(145, 21)
        Me.cboIssuer.TabIndex = 24
        Me.cboIssuer.Tag = "F;"
        Me.cboIssuer.Visible = False
        '
        'lblComments
        '
        Me.lblComments.BackColor = System.Drawing.SystemColors.Control
        Me.lblComments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComments.Location = New System.Drawing.Point(288, 43)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComments.Size = New System.Drawing.Size(121, 17)
        Me.lblComments.TabIndex = 21
        Me.lblComments.Text = "Comments:"
        Me.lblComments.Visible = False
        '
        'lblCollectionDate
        '
        Me.lblCollectionDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCollectionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCollectionDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollectionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCollectionDate.Location = New System.Drawing.Point(288, 17)
        Me.lblCollectionDate.Name = "lblCollectionDate"
        Me.lblCollectionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCollectionDate.Size = New System.Drawing.Size(121, 17)
        Me.lblCollectionDate.TabIndex = 19
        Me.lblCollectionDate.Text = "Collection Date:"
        Me.lblCollectionDate.Visible = False
        '
        'lblIssuer
        '
        Me.lblIssuer.BackColor = System.Drawing.SystemColors.Control
        Me.lblIssuer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIssuer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIssuer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIssuer.Location = New System.Drawing.Point(288, 69)
        Me.lblIssuer.Name = "lblIssuer"
        Me.lblIssuer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIssuer.Size = New System.Drawing.Size(89, 17)
        Me.lblIssuer.TabIndex = 23
        Me.lblIssuer.Text = "Issuer:"
        Me.lblIssuer.Visible = False
        '
        'lblBankRef
        '
        Me.lblBankRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankRef.Location = New System.Drawing.Point(288, 95)
        Me.lblBankRef.Name = "lblBankRef"
        Me.lblBankRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankRef.Size = New System.Drawing.Size(129, 17)
        Me.lblBankRef.TabIndex = 25
        Me.lblBankRef.Text = "Bank Reference:"
        '
        'lblTheirRef
        '
        Me.lblTheirRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblTheirRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTheirRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTheirRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTheirRef.Location = New System.Drawing.Point(288, 121)
        Me.lblTheirRef.Name = "lblTheirRef"
        Me.lblTheirRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTheirRef.Size = New System.Drawing.Size(105, 17)
        Me.lblTheirRef.TabIndex = 27
        Me.lblTheirRef.Text = "Their Reference:"
        '
        'lblOurRef
        '
        Me.lblOurRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblOurRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOurRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOurRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOurRef.Location = New System.Drawing.Point(8, 97)
        Me.lblOurRef.Name = "lblOurRef"
        Me.lblOurRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOurRef.Size = New System.Drawing.Size(121, 16)
        Me.lblOurRef.TabIndex = 17
        Me.lblOurRef.Text = "Our Reference:"
        '
        'lblMediaType
        '
        Me.lblMediaType.AutoSize = True
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(8, 49)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(76, 13)
        Me.lblMediaType.TabIndex = 13
        Me.lblMediaType.Text = "Media Type:"
        '
        'lblMediaRef
        '
        Me.lblMediaRef.AutoSize = True
        Me.lblMediaRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaRef.Location = New System.Drawing.Point(8, 72)
        Me.lblMediaRef.Name = "lblMediaRef"
        Me.lblMediaRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaRef.Size = New System.Drawing.Size(107, 13)
        Me.lblMediaRef.TabIndex = 15
        Me.lblMediaRef.Text = "Media Reference:"
        '
        'lblTransDate
        '
        Me.lblTransDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransDate.Location = New System.Drawing.Point(8, 25)
        Me.lblTransDate.Name = "lblTransDate"
        Me.lblTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransDate.Size = New System.Drawing.Size(137, 17)
        Me.lblTransDate.TabIndex = 11
        Me.lblTransDate.Text = "Transaction Date:"
        '
        'chkLetter
        '
        Me.chkLetter.BackColor = System.Drawing.SystemColors.Control
        Me.chkLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLetter.Location = New System.Drawing.Point(416, 52)
        Me.chkLetter.Name = "chkLetter"
        Me.chkLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLetter.Size = New System.Drawing.Size(165, 17)
        Me.chkLetter.TabIndex = 9
        Me.chkLetter.Tag = "F;"
        Me.chkLetter.Text = "Produce Documents"
        Me.chkLetter.UseVisualStyleBackColor = False
        '
        'cboPaymentType
        '
        Me.cboPaymentType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentType.Location = New System.Drawing.Point(144, 49)
        Me.cboPaymentType.Name = "cboPaymentType"
        Me.cboPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentType.Size = New System.Drawing.Size(137, 21)
        Me.cboPaymentType.TabIndex = 7
        Me.cboPaymentType.Tag = "F;"
        '
        'cboReceiptType
        '
        Me.cboReceiptType.BackColor = System.Drawing.SystemColors.Window
        Me.cboReceiptType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReceiptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReceiptType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReceiptType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReceiptType.Location = New System.Drawing.Point(144, 49)
        Me.cboReceiptType.Name = "cboReceiptType"
        Me.cboReceiptType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReceiptType.Size = New System.Drawing.Size(137, 21)
        Me.cboReceiptType.TabIndex = 8
        Me.cboReceiptType.Tag = "F;"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._fraReceipt_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._fraReceipt_2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage1.Controls.Add(Me._fraReceipt_3)
        Me._tabMainTab_TabPage1.Controls.Add(Me._fraReceipt_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Receipt"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_fraReceipt_1
        '
        Me._fraReceipt_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraReceipt_1.Controls.Add(Me.uctReceiptCC)
        Me._fraReceipt_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraReceipt_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraReceipt_1.Location = New System.Drawing.Point(16, 182)
        Me._fraReceipt_1.Name = "_fraReceipt_1"
        Me._fraReceipt_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraReceipt_1.Size = New System.Drawing.Size(585, 227)
        Me._fraReceipt_1.TabIndex = 61
        Me._fraReceipt_1.TabStop = False
        Me._fraReceipt_1.Text = "Credit Card Information"
        '
        'uctReceiptCC
        '
        Me.uctReceiptCC.CardTransSlipNo = ""
        Me.uctReceiptCC.CardTypeId = -1
        Me.uctReceiptCC.CCAutoAuthCode = ""
        Me.uctReceiptCC.CCBankId = -1
        Me.uctReceiptCC.CCCustomerFlag = ""
        Me.uctReceiptCC.CCExpiry = ""
        Me.uctReceiptCC.CCIssue = ""
        Me.uctReceiptCC.CCManualAuthCode = ""
        Me.uctReceiptCC.CCName = ""
        Me.uctReceiptCC.CCNumber = ""
        Me.uctReceiptCC.CCPIN = ""
        Me.uctReceiptCC.CCStart = ""
        Me.uctReceiptCC.CCTransactionCode = ""
        Me.uctReceiptCC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctReceiptCC.IsAdditionalDetailOption = False
        Me.uctReceiptCC.IsExternalCreditCardProcessing = False
        Me.uctReceiptCC.Location = New System.Drawing.Point(8, 16)
        Me.uctReceiptCC.Name = "uctReceiptCC"
        Me.uctReceiptCC.Size = New System.Drawing.Size(569, 200)
        Me.uctReceiptCC.TabIndex = 62
        Me.uctReceiptCC.ViewOnlyMode = True
        '
        '_fraReceipt_2
        '
        Me._fraReceipt_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraReceipt_2.Controls.Add(Me.cmdClaimDebt)
        Me._fraReceipt_2.Controls.Add(Me.cmdSalvage)
        Me._fraReceipt_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraReceipt_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraReceipt_2.Location = New System.Drawing.Point(16, 182)
        Me._fraReceipt_2.Name = "_fraReceipt_2"
        Me._fraReceipt_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraReceipt_2.Size = New System.Drawing.Size(585, 53)
        Me._fraReceipt_2.TabIndex = 63
        Me._fraReceipt_2.TabStop = False
        Me._fraReceipt_2.Text = "Claims Information"
        '
        'cmdClaimDebt
        '
        Me.cmdClaimDebt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClaimDebt.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClaimDebt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClaimDebt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClaimDebt.Location = New System.Drawing.Point(232, 20)
        Me.cmdClaimDebt.Name = "cmdClaimDebt"
        Me.cmdClaimDebt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClaimDebt.Size = New System.Drawing.Size(85, 22)
        Me.cmdClaimDebt.TabIndex = 65
        Me.cmdClaimDebt.Text = "#Claim &Debt"
        Me.cmdClaimDebt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClaimDebt.UseVisualStyleBackColor = False
        Me.cmdClaimDebt.Visible = False
        '
        'cmdSalvage
        '
        Me.cmdSalvage.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSalvage.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSalvage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSalvage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSalvage.Location = New System.Drawing.Point(112, 20)
        Me.cmdSalvage.Name = "cmdSalvage"
        Me.cmdSalvage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSalvage.Size = New System.Drawing.Size(85, 22)
        Me.cmdSalvage.TabIndex = 64
        Me.cmdSalvage.Text = "&Salvage"
        Me.cmdSalvage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSalvage.UseVisualStyleBackColor = False
        Me.cmdSalvage.Visible = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.uctPartyBankCombo2)
        Me.Frame2.Controls.Add(Me.lblReceiptAccountType)
        Me.Frame2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(16, 139)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(585, 43)
        Me.Frame2.TabIndex = 58
        Me.Frame2.TabStop = False
        '
        'uctPartyBankCombo2
        '
        Me.uctPartyBankCombo2.BankPaymentTypeCode = ""
        Me.uctPartyBankCombo2.EnableAdd = False
        Me.uctPartyBankCombo2.EnableEdit = False
        Me.uctPartyBankCombo2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankCombo2.Location = New System.Drawing.Point(104, 10)
        Me.uctPartyBankCombo2.Name = "uctPartyBankCombo2"
        Me.uctPartyBankCombo2.PartyBankDetails = Nothing
        Me.uctPartyBankCombo2.PartyCnt = Nothing
        Me.uctPartyBankCombo2.SelectedPaymentID = 0
        Me.uctPartyBankCombo2.Size = New System.Drawing.Size(393, 25)
        Me.uctPartyBankCombo2.TabIndex = 60
        '
        'lblReceiptAccountType
        '
        Me.lblReceiptAccountType.AutoSize = True
        Me.lblReceiptAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiptAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiptAccountType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiptAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiptAccountType.Location = New System.Drawing.Point(10, 16)
        Me.lblReceiptAccountType.Name = "lblReceiptAccountType"
        Me.lblReceiptAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiptAccountType.Size = New System.Drawing.Size(88, 13)
        Me.lblReceiptAccountType.TabIndex = 59
        Me.lblReceiptAccountType.Text = "Account Type:"
        '
        '_fraReceipt_3
        '
        Me._fraReceipt_3.BackColor = System.Drawing.SystemColors.Control
        Me._fraReceipt_3.Controls.Add(Me.txtReverseDate)
        Me._fraReceipt_3.Controls.Add(Me.txtReason)
        Me._fraReceipt_3.Controls.Add(Me.cboReversalReason)
        Me._fraReceipt_3.Controls.Add(Me.lblReverseDate)
        Me._fraReceipt_3.Controls.Add(Me.lblReason)
        Me._fraReceipt_3.Controls.Add(Me.lblReversalReason)
        Me._fraReceipt_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraReceipt_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraReceipt_3.Location = New System.Drawing.Point(16, 237)
        Me._fraReceipt_3.Name = "_fraReceipt_3"
        Me._fraReceipt_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraReceipt_3.Size = New System.Drawing.Size(585, 93)
        Me._fraReceipt_3.TabIndex = 66
        Me._fraReceipt_3.TabStop = False
        Me._fraReceipt_3.Text = "Reversal"
        '
        'txtReverseDate
        '
        Me.txtReverseDate.AcceptsReturn = True
        Me.txtReverseDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtReverseDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReverseDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReverseDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReverseDate.Location = New System.Drawing.Point(112, 16)
        Me.txtReverseDate.MaxLength = 0
        Me.txtReverseDate.Name = "txtReverseDate"
        Me.txtReverseDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReverseDate.Size = New System.Drawing.Size(145, 21)
        Me.txtReverseDate.TabIndex = 68
        Me.txtReverseDate.Tag = "F;DT;"
        '
        'txtReason
        '
        Me.txtReason.AcceptsReturn = True
        Me.txtReason.BackColor = System.Drawing.SystemColors.Window
        Me.txtReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReason.Location = New System.Drawing.Point(112, 42)
        Me.txtReason.MaxLength = 0
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReason.Size = New System.Drawing.Size(441, 43)
        Me.txtReason.TabIndex = 72
        Me.txtReason.Tag = "F;"
        '
        'cboReversalReason
        '
        Me.cboReversalReason.BackColor = System.Drawing.SystemColors.Window
        Me.cboReversalReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReversalReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReversalReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReversalReason.Location = New System.Drawing.Point(402, 16)
        Me.cboReversalReason.Name = "cboReversalReason"
        Me.cboReversalReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReversalReason.Size = New System.Drawing.Size(153, 21)
        Me.cboReversalReason.TabIndex = 70
        Me.cboReversalReason.Tag = "F;"
        '
        'lblReverseDate
        '
        Me.lblReverseDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblReverseDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReverseDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReverseDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReverseDate.Location = New System.Drawing.Point(16, 19)
        Me.lblReverseDate.Name = "lblReverseDate"
        Me.lblReverseDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReverseDate.Size = New System.Drawing.Size(169, 17)
        Me.lblReverseDate.TabIndex = 67
        Me.lblReverseDate.Text = "Reverse Date:"
        '
        'lblReason
        '
        Me.lblReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReason.Location = New System.Drawing.Point(16, 45)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReason.Size = New System.Drawing.Size(81, 17)
        Me.lblReason.TabIndex = 71
        Me.lblReason.Text = "Reason:"
        '
        'lblReversalReason
        '
        Me.lblReversalReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblReversalReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReversalReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReversalReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReversalReason.Location = New System.Drawing.Point(280, 19)
        Me.lblReversalReason.Name = "lblReversalReason"
        Me.lblReversalReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReversalReason.Size = New System.Drawing.Size(129, 17)
        Me.lblReversalReason.TabIndex = 69
        Me.lblReversalReason.Text = "Reversal Reason:"
        '
        '_fraReceipt_0
        '
        Me._fraReceipt_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraReceipt_0.Controls.Add(Me.cboBank)
        Me._fraReceipt_0.Controls.Add(Me.txtChequeDate)
        Me._fraReceipt_0.Controls.Add(Me.txtName)
        Me._fraReceipt_0.Controls.Add(Me.txtBankBranch)
        Me._fraReceipt_0.Controls.Add(Me.txtBankLocation)
        Me._fraReceipt_0.Controls.Add(Me.cboChequeType)
        Me._fraReceipt_0.Controls.Add(Me.cboChequeClearingType)
        Me._fraReceipt_0.Controls.Add(Me.lblBank)
        Me._fraReceipt_0.Controls.Add(Me.lblChequeDate)
        Me._fraReceipt_0.Controls.Add(Me.lblName)
        Me._fraReceipt_0.Controls.Add(Me.lblBankBranch)
        Me._fraReceipt_0.Controls.Add(Me.lblBankLocation)
        Me._fraReceipt_0.Controls.Add(Me.lblChequeType)
        Me._fraReceipt_0.Controls.Add(Me.lblChequeClearingType)
        Me._fraReceipt_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraReceipt_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraReceipt_0.Location = New System.Drawing.Point(16, 12)
        Me._fraReceipt_0.Name = "_fraReceipt_0"
        Me._fraReceipt_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraReceipt_0.Size = New System.Drawing.Size(585, 127)
        Me._fraReceipt_0.TabIndex = 43
        Me._fraReceipt_0.TabStop = False
        Me._fraReceipt_0.Text = "Cheque Information"
        '
        'cboBank
        '
        Me.cboBank.BackColor = System.Drawing.SystemColors.Window
        Me.cboBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBank.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBank.Location = New System.Drawing.Point(424, 16)
        Me.cboBank.Name = "cboBank"
        Me.cboBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBank.Size = New System.Drawing.Size(153, 21)
        Me.cboBank.TabIndex = 53
        Me.cboBank.Tag = "F;"
        '
        'txtChequeDate
        '
        Me.txtChequeDate.AcceptsReturn = True
        Me.txtChequeDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtChequeDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChequeDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChequeDate.Location = New System.Drawing.Point(112, 38)
        Me.txtChequeDate.MaxLength = 0
        Me.txtChequeDate.Name = "txtChequeDate"
        Me.txtChequeDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChequeDate.Size = New System.Drawing.Size(145, 21)
        Me.txtChequeDate.TabIndex = 47
        Me.txtChequeDate.Tag = "F;DT;"
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(112, 16)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(145, 21)
        Me.txtName.TabIndex = 45
        Me.txtName.Tag = "F;"
        '
        'txtBankBranch
        '
        Me.txtBankBranch.AcceptsReturn = True
        Me.txtBankBranch.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankBranch.Location = New System.Drawing.Point(424, 64)
        Me.txtBankBranch.MaxLength = 0
        Me.txtBankBranch.Name = "txtBankBranch"
        Me.txtBankBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankBranch.Size = New System.Drawing.Size(153, 21)
        Me.txtBankBranch.TabIndex = 57
        Me.txtBankBranch.Tag = "F;"
        '
        'txtBankLocation
        '
        Me.txtBankLocation.AcceptsReturn = True
        Me.txtBankLocation.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankLocation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankLocation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankLocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankLocation.Location = New System.Drawing.Point(424, 40)
        Me.txtBankLocation.MaxLength = 0
        Me.txtBankLocation.Name = "txtBankLocation"
        Me.txtBankLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankLocation.Size = New System.Drawing.Size(153, 21)
        Me.txtBankLocation.TabIndex = 55
        Me.txtBankLocation.Tag = "F;"
        '
        'cboChequeType
        '
        Me.cboChequeType.BackColor = System.Drawing.SystemColors.Window
        Me.cboChequeType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboChequeType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboChequeType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboChequeType.Location = New System.Drawing.Point(112, 64)
        Me.cboChequeType.Name = "cboChequeType"
        Me.cboChequeType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboChequeType.Size = New System.Drawing.Size(145, 21)
        Me.cboChequeType.TabIndex = 49
        Me.cboChequeType.Tag = "F;"
        '
        'cboChequeClearingType
        '
        Me.cboChequeClearingType.BackColor = System.Drawing.SystemColors.Window
        Me.cboChequeClearingType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboChequeClearingType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboChequeClearingType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboChequeClearingType.Location = New System.Drawing.Point(112, 90)
        Me.cboChequeClearingType.Name = "cboChequeClearingType"
        Me.cboChequeClearingType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboChequeClearingType.Size = New System.Drawing.Size(145, 21)
        Me.cboChequeClearingType.TabIndex = 51
        Me.cboChequeClearingType.Tag = "F;"
        '
        'lblBank
        '
        Me.lblBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBank.Location = New System.Drawing.Point(272, 19)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBank.Size = New System.Drawing.Size(153, 17)
        Me.lblBank.TabIndex = 52
        Me.lblBank.Text = "Bank:"
        '
        'lblChequeDate
        '
        Me.lblChequeDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblChequeDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChequeDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChequeDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChequeDate.Location = New System.Drawing.Point(8, 41)
        Me.lblChequeDate.Name = "lblChequeDate"
        Me.lblChequeDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChequeDate.Size = New System.Drawing.Size(105, 17)
        Me.lblChequeDate.TabIndex = 46
        Me.lblChequeDate.Text = "Cheque Date:"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(8, 16)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(137, 17)
        Me.lblName.TabIndex = 44
        Me.lblName.Text = "Name:"
        '
        'lblBankBranch
        '
        Me.lblBankBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankBranch.Location = New System.Drawing.Point(272, 65)
        Me.lblBankBranch.Name = "lblBankBranch"
        Me.lblBankBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankBranch.Size = New System.Drawing.Size(153, 17)
        Me.lblBankBranch.TabIndex = 56
        Me.lblBankBranch.Text = "Drawee Bank Branch:"
        '
        'lblBankLocation
        '
        Me.lblBankLocation.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankLocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankLocation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankLocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankLocation.Location = New System.Drawing.Point(272, 41)
        Me.lblBankLocation.Name = "lblBankLocation"
        Me.lblBankLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankLocation.Size = New System.Drawing.Size(153, 17)
        Me.lblBankLocation.TabIndex = 54
        Me.lblBankLocation.Text = "Drawee Bank Location:"
        '
        'lblChequeType
        '
        Me.lblChequeType.BackColor = System.Drawing.SystemColors.Control
        Me.lblChequeType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChequeType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChequeType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChequeType.Location = New System.Drawing.Point(8, 66)
        Me.lblChequeType.Name = "lblChequeType"
        Me.lblChequeType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChequeType.Size = New System.Drawing.Size(105, 17)
        Me.lblChequeType.TabIndex = 48
        Me.lblChequeType.Text = "Cheque Type:"
        '
        'lblChequeClearingType
        '
        Me.lblChequeClearingType.BackColor = System.Drawing.SystemColors.Control
        Me.lblChequeClearingType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChequeClearingType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChequeClearingType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChequeClearingType.Location = New System.Drawing.Point(8, 88)
        Me.lblChequeClearingType.Name = "lblChequeClearingType"
        Me.lblChequeClearingType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChequeClearingType.Size = New System.Drawing.Size(97, 33)
        Me.lblChequeClearingType.TabIndex = 50
        Me.lblChequeClearingType.Text = "Cheque Clearing Type:"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraBank)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraPaymentCreditCard)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraPayee)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraPayment)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraPaymentAccountType)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Payment"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraBank
        '
        Me.fraBank.BackColor = System.Drawing.SystemColors.Control
        Me.fraBank.Controls.Add(Me.txtDatePresented)
        Me.fraBank.Controls.Add(Me.txtConfirmation)
        Me.fraBank.Controls.Add(Me.txtPaymentReason)
        Me.fraBank.Controls.Add(Me.chkInPossession)
        Me.fraBank.Controls.Add(Me.txtStopRequested)
        Me.fraBank.Controls.Add(Me.lblDatePresented)
        Me.fraBank.Controls.Add(Me.lblCancellationReason)
        Me.fraBank.Controls.Add(Me.lblConfirmation)
        Me.fraBank.Controls.Add(Me.lblStopRequested)
        Me.fraBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBank.Location = New System.Drawing.Point(16, 270)
        Me.fraBank.Name = "fraBank"
        Me.fraBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBank.Size = New System.Drawing.Size(585, 151)
        Me.fraBank.TabIndex = 94
        Me.fraBank.TabStop = False
        Me.fraBank.Text = "Bank"
        '
        'txtDatePresented
        '
        Me.txtDatePresented.AcceptsReturn = True
        Me.txtDatePresented.BackColor = System.Drawing.SystemColors.Window
        Me.txtDatePresented.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDatePresented.Enabled = False
        Me.txtDatePresented.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDatePresented.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDatePresented.Location = New System.Drawing.Point(144, 17)
        Me.txtDatePresented.MaxLength = 0
        Me.txtDatePresented.Name = "txtDatePresented"
        Me.txtDatePresented.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDatePresented.Size = New System.Drawing.Size(145, 21)
        Me.txtDatePresented.TabIndex = 96
        Me.txtDatePresented.Tag = "F;DT;"
        '
        'txtConfirmation
        '
        Me.txtConfirmation.AcceptsReturn = True
        Me.txtConfirmation.BackColor = System.Drawing.SystemColors.Window
        Me.txtConfirmation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConfirmation.Enabled = False
        Me.txtConfirmation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirmation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtConfirmation.Location = New System.Drawing.Point(416, 49)
        Me.txtConfirmation.MaxLength = 0
        Me.txtConfirmation.Name = "txtConfirmation"
        Me.txtConfirmation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtConfirmation.Size = New System.Drawing.Size(145, 21)
        Me.txtConfirmation.TabIndex = 101
        Me.txtConfirmation.Tag = "F;DT;"
        '
        'txtPaymentReason
        '
        Me.txtPaymentReason.AcceptsReturn = True
        Me.txtPaymentReason.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentReason.Location = New System.Drawing.Point(144, 81)
        Me.txtPaymentReason.MaxLength = 0
        Me.txtPaymentReason.Name = "txtPaymentReason"
        Me.txtPaymentReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentReason.Size = New System.Drawing.Size(417, 21)
        Me.txtPaymentReason.TabIndex = 103
        Me.txtPaymentReason.Tag = "F;"
        '
        'chkInPossession
        '
        Me.chkInPossession.BackColor = System.Drawing.SystemColors.Control
        Me.chkInPossession.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInPossession.Enabled = False
        Me.chkInPossession.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInPossession.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInPossession.Location = New System.Drawing.Point(416, 15)
        Me.chkInPossession.Name = "chkInPossession"
        Me.chkInPossession.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInPossession.Size = New System.Drawing.Size(113, 25)
        Me.chkInPossession.TabIndex = 97
        Me.chkInPossession.Tag = "F;"
        Me.chkInPossession.Text = "In Possession"
        Me.chkInPossession.UseVisualStyleBackColor = False
        '
        'txtStopRequested
        '
        Me.txtStopRequested.AcceptsReturn = True
        Me.txtStopRequested.BackColor = System.Drawing.SystemColors.Window
        Me.txtStopRequested.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStopRequested.Enabled = False
        Me.txtStopRequested.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStopRequested.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStopRequested.Location = New System.Drawing.Point(144, 49)
        Me.txtStopRequested.MaxLength = 0
        Me.txtStopRequested.Name = "txtStopRequested"
        Me.txtStopRequested.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStopRequested.Size = New System.Drawing.Size(145, 20)
        Me.txtStopRequested.TabIndex = 99
        Me.txtStopRequested.Tag = "F;DT;"
        '
        'lblDatePresented
        '
        Me.lblDatePresented.BackColor = System.Drawing.SystemColors.Control
        Me.lblDatePresented.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDatePresented.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatePresented.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDatePresented.Location = New System.Drawing.Point(8, 20)
        Me.lblDatePresented.Name = "lblDatePresented"
        Me.lblDatePresented.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDatePresented.Size = New System.Drawing.Size(121, 17)
        Me.lblDatePresented.TabIndex = 95
        Me.lblDatePresented.Text = "Date Presented:"
        '
        'lblCancellationReason
        '
        Me.lblCancellationReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblCancellationReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCancellationReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancellationReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCancellationReason.Location = New System.Drawing.Point(8, 84)
        Me.lblCancellationReason.Name = "lblCancellationReason"
        Me.lblCancellationReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCancellationReason.Size = New System.Drawing.Size(137, 17)
        Me.lblCancellationReason.TabIndex = 102
        Me.lblCancellationReason.Text = "Reason:"
        '
        'lblConfirmation
        '
        Me.lblConfirmation.BackColor = System.Drawing.SystemColors.Control
        Me.lblConfirmation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConfirmation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConfirmation.Location = New System.Drawing.Point(296, 52)
        Me.lblConfirmation.Name = "lblConfirmation"
        Me.lblConfirmation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConfirmation.Size = New System.Drawing.Size(97, 17)
        Me.lblConfirmation.TabIndex = 100
        Me.lblConfirmation.Text = "Confirmation:"
        '
        'lblStopRequested
        '
        Me.lblStopRequested.BackColor = System.Drawing.SystemColors.Control
        Me.lblStopRequested.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStopRequested.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStopRequested.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStopRequested.Location = New System.Drawing.Point(8, 52)
        Me.lblStopRequested.Name = "lblStopRequested"
        Me.lblStopRequested.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStopRequested.Size = New System.Drawing.Size(121, 25)
        Me.lblStopRequested.TabIndex = 98
        Me.lblStopRequested.Text = "Stop Requested:"
        '
        'fraPaymentCreditCard
        '
        Me.fraPaymentCreditCard.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentCreditCard.Controls.Add(Me.uctPaymentCC)
        Me.fraPaymentCreditCard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentCreditCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentCreditCard.Location = New System.Drawing.Point(16, 122)
        Me.fraPaymentCreditCard.Name = "fraPaymentCreditCard"
        Me.fraPaymentCreditCard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentCreditCard.Size = New System.Drawing.Size(585, 217)
        Me.fraPaymentCreditCard.TabIndex = 92
        Me.fraPaymentCreditCard.TabStop = False
        Me.fraPaymentCreditCard.Text = "Credit Card"
        '
        'uctPaymentCC
        '
        Me.uctPaymentCC.CardTransSlipNo = ""
        Me.uctPaymentCC.CardTypeId = -1
        Me.uctPaymentCC.CCAutoAuthCode = ""
        Me.uctPaymentCC.CCBankId = -1
        Me.uctPaymentCC.CCCustomerFlag = ""
        Me.uctPaymentCC.CCExpiry = ""
        Me.uctPaymentCC.CCIssue = ""
        Me.uctPaymentCC.CCManualAuthCode = ""
        Me.uctPaymentCC.CCName = ""
        Me.uctPaymentCC.CCNumber = ""
        Me.uctPaymentCC.CCPIN = ""
        Me.uctPaymentCC.CCStart = ""
        Me.uctPaymentCC.CCTransactionCode = ""
        Me.uctPaymentCC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPaymentCC.IsAdditionalDetailOption = False
        Me.uctPaymentCC.IsExternalCreditCardProcessing = False
        Me.uctPaymentCC.Location = New System.Drawing.Point(8, 16)
        Me.uctPaymentCC.Name = "uctPaymentCC"
        Me.uctPaymentCC.Size = New System.Drawing.Size(561, 193)
        Me.uctPaymentCC.TabIndex = 93
        Me.uctPaymentCC.ViewOnlyMode = True
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.txtIBAN)
        Me.fraPayee.Controls.Add(Me.lblIBAN)
        Me.fraPayee.Controls.Add(Me.txtBIC)
        Me.fraPayee.Controls.Add(Me.lblBIC)
        Me.fraPayee.Controls.Add(Me.txtPayeeName)
        Me.fraPayee.Controls.Add(Me.txtPaymentAccountCode)
        Me.fraPayee.Controls.Add(Me.txtPaymentBranchCode)
        Me.fraPayee.Controls.Add(Me.txtPaymentReference1)
        Me.fraPayee.Controls.Add(Me.txtPaymentReference2)
        Me.fraPayee.Controls.Add(Me.txtPaymentExpiryDate)
        Me.fraPayee.Controls.Add(Me.lblPayeeName)
        Me.fraPayee.Controls.Add(Me.lblPaymentReference1)
        Me.fraPayee.Controls.Add(Me.lblPaymentReference2)
        Me.fraPayee.Controls.Add(Me.lblPaymentExpiryDate)
        Me.fraPayee.Controls.Add(Me.lblPaymentBranchCode)
        Me.fraPayee.Controls.Add(Me.lblPaymentAccountCode)
        Me.fraPayee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(16, 121)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(585, 142)
        Me.fraPayee.TabIndex = 79
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payee"
        '
        'txtPayeeName
        '
        Me.txtPayeeName.AcceptsReturn = True
        Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeName.Location = New System.Drawing.Point(144, 17)
        Me.txtPayeeName.MaxLength = 0
        Me.txtPayeeName.Name = "txtPayeeName"
        Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeName.Size = New System.Drawing.Size(145, 21)
        Me.txtPayeeName.TabIndex = 81
        Me.txtPayeeName.Tag = "F;"
        '
        'txtPaymentAccountCode
        '
        Me.txtPaymentAccountCode.AcceptsReturn = True
        Me.txtPaymentAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentAccountCode.Location = New System.Drawing.Point(416, 17)
        Me.txtPaymentAccountCode.MaxLength = 0
        Me.txtPaymentAccountCode.Name = "txtPaymentAccountCode"
        Me.txtPaymentAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentAccountCode.Size = New System.Drawing.Size(145, 21)
        Me.txtPaymentAccountCode.TabIndex = 83
        Me.txtPaymentAccountCode.Tag = "F;"
        '
        'txtPaymentBranchCode
        '
        Me.txtPaymentBranchCode.AcceptsReturn = True
        Me.txtPaymentBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentBranchCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentBranchCode.Location = New System.Drawing.Point(416, 49)
        Me.txtPaymentBranchCode.MaxLength = 0
        Me.txtPaymentBranchCode.Name = "txtPaymentBranchCode"
        Me.txtPaymentBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentBranchCode.Size = New System.Drawing.Size(145, 21)
        Me.txtPaymentBranchCode.TabIndex = 87
        Me.txtPaymentBranchCode.Tag = "F;"
        '
        'txtPaymentReference1
        '
        Me.txtPaymentReference1.AcceptsReturn = True
        Me.txtPaymentReference1.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentReference1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentReference1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentReference1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentReference1.Location = New System.Drawing.Point(144, 82)
        Me.txtPaymentReference1.MaxLength = 0
        Me.txtPaymentReference1.Name = "txtPaymentReference1"
        Me.txtPaymentReference1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentReference1.Size = New System.Drawing.Size(145, 21)
        Me.txtPaymentReference1.TabIndex = 89
        Me.txtPaymentReference1.Tag = "F;"
        '
        'txtPaymentReference2
        '
        Me.txtPaymentReference2.AcceptsReturn = True
        Me.txtPaymentReference2.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentReference2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentReference2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentReference2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentReference2.Location = New System.Drawing.Point(416, 81)
        Me.txtPaymentReference2.MaxLength = 0
        Me.txtPaymentReference2.Name = "txtPaymentReference2"
        Me.txtPaymentReference2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentReference2.Size = New System.Drawing.Size(145, 21)
        Me.txtPaymentReference2.TabIndex = 91
        Me.txtPaymentReference2.Tag = "F;"
        '
        'txtPaymentExpiryDate
        '
        Me.txtPaymentExpiryDate.AcceptsReturn = True
        Me.txtPaymentExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentExpiryDate.Location = New System.Drawing.Point(144, 49)
        Me.txtPaymentExpiryDate.MaxLength = 0
        Me.txtPaymentExpiryDate.Name = "txtPaymentExpiryDate"
        Me.txtPaymentExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentExpiryDate.Size = New System.Drawing.Size(145, 21)
        Me.txtPaymentExpiryDate.TabIndex = 85
        Me.txtPaymentExpiryDate.Tag = "F;D;"
        '
        'lblPayeeName
        '
        Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeName.Location = New System.Drawing.Point(8, 20)
        Me.lblPayeeName.Name = "lblPayeeName"
        Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeName.Size = New System.Drawing.Size(97, 17)
        Me.lblPayeeName.TabIndex = 80
        Me.lblPayeeName.Text = "Payee Name:"
        '
        'lblPaymentReference1
        '
        Me.lblPaymentReference1.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentReference1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentReference1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentReference1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentReference1.Location = New System.Drawing.Point(8, 84)
        Me.lblPaymentReference1.Name = "lblPaymentReference1"
        Me.lblPaymentReference1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentReference1.Size = New System.Drawing.Size(97, 17)
        Me.lblPaymentReference1.TabIndex = 88
        Me.lblPaymentReference1.Text = "Reference 1:"
        '
        'lblPaymentReference2
        '
        Me.lblPaymentReference2.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentReference2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentReference2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentReference2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentReference2.Location = New System.Drawing.Point(296, 84)
        Me.lblPaymentReference2.Name = "lblPaymentReference2"
        Me.lblPaymentReference2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentReference2.Size = New System.Drawing.Size(121, 17)
        Me.lblPaymentReference2.TabIndex = 90
        Me.lblPaymentReference2.Text = "Reference 2:"
        '
        'lblPaymentExpiryDate
        '
        Me.lblPaymentExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentExpiryDate.Location = New System.Drawing.Point(8, 52)
        Me.lblPaymentExpiryDate.Name = "lblPaymentExpiryDate"
        Me.lblPaymentExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentExpiryDate.Size = New System.Drawing.Size(120, 17)
        Me.lblPaymentExpiryDate.TabIndex = 84
        Me.lblPaymentExpiryDate.Text = "Expiry Date:"
        '
        'lblPaymentBranchCode
        '
        Me.lblPaymentBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentBranchCode.Location = New System.Drawing.Point(296, 52)
        Me.lblPaymentBranchCode.Name = "lblPaymentBranchCode"
        Me.lblPaymentBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentBranchCode.Size = New System.Drawing.Size(121, 17)
        Me.lblPaymentBranchCode.TabIndex = 86
        Me.lblPaymentBranchCode.Text = "Bank Branch Code:"
        '
        'lblPaymentAccountCode
        '
        Me.lblPaymentAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentAccountCode.Location = New System.Drawing.Point(296, 20)
        Me.lblPaymentAccountCode.Name = "lblPaymentAccountCode"
        Me.lblPaymentAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAccountCode.Size = New System.Drawing.Size(121, 17)
        Me.lblPaymentAccountCode.TabIndex = 82
        Me.lblPaymentAccountCode.Text = "Account Number:"
        '
        'fraPayment
        '
        Me.fraPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayment.Controls.Add(Me.cboPaymentStatus)
        Me.fraPayment.Controls.Add(Me.lblPaymentStatus)
        Me.fraPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayment.Location = New System.Drawing.Point(16, 12)
        Me.fraPayment.Name = "fraPayment"
        Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayment.Size = New System.Drawing.Size(585, 60)
        Me.fraPayment.TabIndex = 73
        Me.fraPayment.TabStop = False
        Me.fraPayment.Text = "Payment Information"
        '
        'cboPaymentStatus
        '
        Me.cboPaymentStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentStatus.Location = New System.Drawing.Point(144, 24)
        Me.cboPaymentStatus.Name = "cboPaymentStatus"
        Me.cboPaymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentStatus.Size = New System.Drawing.Size(145, 21)
        Me.cboPaymentStatus.TabIndex = 75
        Me.cboPaymentStatus.Tag = "F;"
        '
        'lblPaymentStatus
        '
        Me.lblPaymentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentStatus.Location = New System.Drawing.Point(8, 27)
        Me.lblPaymentStatus.Name = "lblPaymentStatus"
        Me.lblPaymentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentStatus.Size = New System.Drawing.Size(89, 17)
        Me.lblPaymentStatus.TabIndex = 74
        Me.lblPaymentStatus.Text = "Status:"
        '
        'fraPaymentAccountType
        '
        Me.fraPaymentAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentAccountType.Controls.Add(Me.uctPartyBankCombo1)
        Me.fraPaymentAccountType.Controls.Add(Me.lblPaymentAccountType)
        Me.fraPaymentAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentAccountType.Location = New System.Drawing.Point(16, 72)
        Me.fraPaymentAccountType.Name = "fraPaymentAccountType"
        Me.fraPaymentAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentAccountType.Size = New System.Drawing.Size(585, 49)
        Me.fraPaymentAccountType.TabIndex = 76
        Me.fraPaymentAccountType.TabStop = False
        '
        'uctPartyBankCombo1
        '
        Me.uctPartyBankCombo1.BankPaymentTypeCode = ""
        Me.uctPartyBankCombo1.EnableAdd = False
        Me.uctPartyBankCombo1.EnableEdit = False
        Me.uctPartyBankCombo1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankCombo1.Location = New System.Drawing.Point(112, 12)
        Me.uctPartyBankCombo1.Name = "uctPartyBankCombo1"
        Me.uctPartyBankCombo1.PartyBankDetails = Nothing
        Me.uctPartyBankCombo1.PartyCnt = Nothing
        Me.uctPartyBankCombo1.SelectedPaymentID = 0
        Me.uctPartyBankCombo1.Size = New System.Drawing.Size(409, 25)
        Me.uctPartyBankCombo1.TabIndex = 78
        '
        'lblPaymentAccountType
        '
        Me.lblPaymentAccountType.AutoSize = True
        Me.lblPaymentAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAccountType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentAccountType.Location = New System.Drawing.Point(14, 18)
        Me.lblPaymentAccountType.Name = "lblPaymentAccountType"
        Me.lblPaymentAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAccountType.Size = New System.Drawing.Size(88, 13)
        Me.lblPaymentAccountType.TabIndex = 77
        Me.lblPaymentAccountType.Text = "Account Type:"
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraFurtherDetails)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Address"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'fraFurtherDetails
        '
        Me.fraFurtherDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraFurtherDetails.Controls.Add(Me.txtContactName)
        Me.fraFurtherDetails.Controls.Add(Me.txtFurtherDetails)
        Me.fraFurtherDetails.Controls.Add(Me.uctAddress)
        Me.fraFurtherDetails.Controls.Add(Me.lblContactName)
        Me.fraFurtherDetails.Controls.Add(Me.lblFurtherDetails)
        Me.fraFurtherDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFurtherDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFurtherDetails.Location = New System.Drawing.Point(16, 12)
        Me.fraFurtherDetails.Name = "fraFurtherDetails"
        Me.fraFurtherDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFurtherDetails.Size = New System.Drawing.Size(585, 377)
        Me.fraFurtherDetails.TabIndex = 104
        Me.fraFurtherDetails.TabStop = False
        Me.fraFurtherDetails.Text = "Further Information"
        '
        'txtContactName
        '
        Me.txtContactName.AcceptsReturn = True
        Me.txtContactName.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactName.Location = New System.Drawing.Point(152, 24)
        Me.txtContactName.MaxLength = 0
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContactName.Size = New System.Drawing.Size(385, 21)
        Me.txtContactName.TabIndex = 106
        Me.txtContactName.Tag = "F;"
        '
        'txtFurtherDetails
        '
        Me.txtFurtherDetails.AcceptsReturn = True
        Me.txtFurtherDetails.BackColor = System.Drawing.SystemColors.Window
        Me.txtFurtherDetails.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFurtherDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFurtherDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFurtherDetails.Location = New System.Drawing.Point(152, 224)
        Me.txtFurtherDetails.MaxLength = 0
        Me.txtFurtherDetails.Multiline = True
        Me.txtFurtherDetails.Name = "txtFurtherDetails"
        Me.txtFurtherDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFurtherDetails.Size = New System.Drawing.Size(385, 97)
        Me.txtFurtherDetails.TabIndex = 109
        Me.txtFurtherDetails.Tag = "F;"
        '
        'uctAddress
        '
        Me.uctAddress.AddressLine1 = ""
        Me.uctAddress.AddressLine2 = ""
        Me.uctAddress.AddressLine3 = ""
        Me.uctAddress.AddressLine4 = ""
        Me.uctAddress.Caption = ""
        Me.uctAddress.CaptionAddress1 = "No. && street name:"
        Me.uctAddress.CaptionAddress2 = "Locality:"
        Me.uctAddress.CaptionAddress3 = "Town:"
        Me.uctAddress.CaptionAddress4 = "County:"
        Me.uctAddress.CaptionCountry = "Country:"
        Me.uctAddress.CaptionFontBoldAddress1 = False
        Me.uctAddress.CaptionFontBoldPostCode = False
        Me.uctAddress.CaptionPostCode = "Postcode:"
        Me.uctAddress.ClearButtonCaption = "X"
        Me.uctAddress.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAddress.ClearButtonLeft = 7065
        Me.uctAddress.ClearButtonWidth = 360
        Me.uctAddress.CountryId = 0
        Me.uctAddress.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAddress.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAddress.IsCountryRequired = 1
        Me.uctAddress.IsPostCodeRequired = 1
        Me.uctAddress.Location = New System.Drawing.Point(32, 48)
        Me.uctAddress.Name = "uctAddress"
        Me.uctAddress.Organisation = ""
        Me.uctAddress.PMAddressCnt = 0
        Me.uctAddress.PMDatabaseID = 0
        Me.uctAddress.PostCode = ""
        Me.uctAddress.QAS2PMAddress1 = "3,4,2,5,6"
        Me.uctAddress.QAS2PMAddress2 = "8,7"
        Me.uctAddress.QAS2PMAddress3 = "9"
        Me.uctAddress.QAS2PMAddress4 = ""
        Me.uctAddress.QASDatabaseID = 3
        Me.uctAddress.SearchButtonCaption = ".."
        Me.uctAddress.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAddress.SearchButtonHeight = 285
        Me.uctAddress.SearchButtonLeft = 6630
        Me.uctAddress.SearchButtonTop = 1530
        Me.uctAddress.SearchButtonWidth = 360
        Me.uctAddress.Size = New System.Drawing.Size(507, 152)
        Me.uctAddress.TabIndex = 107
        Me.uctAddress.WarningMessage = ""
        '
        'lblContactName
        '
        Me.lblContactName.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactName.Location = New System.Drawing.Point(32, 27)
        Me.lblContactName.Name = "lblContactName"
        Me.lblContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactName.Size = New System.Drawing.Size(105, 17)
        Me.lblContactName.TabIndex = 105
        Me.lblContactName.Text = "Name:"
        '
        'lblFurtherDetails
        '
        Me.lblFurtherDetails.BackColor = System.Drawing.SystemColors.Control
        Me.lblFurtherDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFurtherDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFurtherDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFurtherDetails.Location = New System.Drawing.Point(32, 227)
        Me.lblFurtherDetails.Name = "lblFurtherDetails"
        Me.lblFurtherDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFurtherDetails.Size = New System.Drawing.Size(129, 25)
        Me.lblFurtherDetails.TabIndex = 108
        Me.lblFurtherDetails.Text = "Further Details:"
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraAddFields)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Additional Fields"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'fraAddFields
        '
        Me.fraAddFields.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddFields.Controls.Add(Me.cboUnderwritingYear)
        Me.fraAddFields.Controls.Add(Me.lblUnderwritingYear)
        Me.fraAddFields.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddFields.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddFields.Location = New System.Drawing.Point(16, 12)
        Me.fraAddFields.Name = "fraAddFields"
        Me.fraAddFields.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddFields.Size = New System.Drawing.Size(585, 377)
        Me.fraAddFields.TabIndex = 110
        Me.fraAddFields.TabStop = False
        Me.fraAddFields.Text = "Additional Fields"
        '
        'cboUnderwritingYear
        '
        Me.cboUnderwritingYear.DefaultItemId = 0
        Me.cboUnderwritingYear.FirstItem = ""
        Me.cboUnderwritingYear.ItemId = 0
        Me.cboUnderwritingYear.ListIndex = -1
        Me.cboUnderwritingYear.Location = New System.Drawing.Point(148, 24)
        Me.cboUnderwritingYear.Name = "cboUnderwritingYear"
        Me.cboUnderwritingYear.PMLookupProductFamily = 1
        Me.cboUnderwritingYear.SingleItemId = 0
        Me.cboUnderwritingYear.Size = New System.Drawing.Size(201, 21)
        Me.cboUnderwritingYear.Sorted = True
        Me.cboUnderwritingYear.TabIndex = 112
        Me.cboUnderwritingYear.TableName = "Underwriting_Year"
        Me.cboUnderwritingYear.Tag = "F;"
        Me.cboUnderwritingYear.ToolTipText = ""
        Me.cboUnderwritingYear.WhereClause = ""
        '
        'lblUnderwritingYear
        '
        Me.lblUnderwritingYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnderwritingYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnderwritingYear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnderwritingYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnderwritingYear.Location = New System.Drawing.Point(12, 28)
        Me.lblUnderwritingYear.Name = "lblUnderwritingYear"
        Me.lblUnderwritingYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnderwritingYear.Size = New System.Drawing.Size(136, 21)
        Me.lblUnderwritingYear.TabIndex = 111
        Me.lblUnderwritingYear.Text = "Underwriting Year:"
        Me.lblUnderwritingYear.Visible = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraInstalments)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6 - Instalments"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraInstalments
        '
        Me.fraInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.fraInstalments.Controls.Add(Me.fralvInstalments)
        Me.fraInstalments.Controls.Add(Me.cboInstalment)
        Me.fraInstalments.Controls.Add(Me.SSOveralPlanTotal)
        Me.fraInstalments.Controls.Add(Me.SSThisPlanTotal)
        Me.fraInstalments.Controls.Add(Me.lblOveralTotal)
        Me.fraInstalments.Controls.Add(Me.lblPlan)
        Me.fraInstalments.Controls.Add(Me.lblTotal)
        Me.fraInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraInstalments.Location = New System.Drawing.Point(16, 12)
        Me.fraInstalments.Name = "fraInstalments"
        Me.fraInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraInstalments.Size = New System.Drawing.Size(585, 377)
        Me.fraInstalments.TabIndex = 113
        Me.fraInstalments.TabStop = False
        Me.fraInstalments.Text = "Instalment Details"
        '
        'fralvInstalments
        '
        Me.fralvInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.fralvInstalments.Controls.Add(Me.lvInstalments)
        Me.fralvInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.fralvInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fralvInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fralvInstalments.Location = New System.Drawing.Point(8, 48)
        Me.fralvInstalments.Name = "fralvInstalments"
        Me.fralvInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fralvInstalments.Size = New System.Drawing.Size(561, 225)
        Me.fralvInstalments.TabIndex = 116
        Me.fralvInstalments.Text = "Frame2"
        '
        'lvInstalments
        '
        Me.lvInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.lvInstalments.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvInstalments, "")
        Me.lvInstalments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvInstalments_ColumnHeader_1, Me._lvInstalments_ColumnHeader_2, Me._lvInstalments_ColumnHeader_3, Me._lvInstalments_ColumnHeader_4, Me._lvInstalments_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvInstalments, False)
        Me.lvInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvInstalments, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvInstalments, "")
        Me.lvInstalments.Location = New System.Drawing.Point(16, 8)
        Me.lvInstalments.Name = "lvInstalments"
        Me.lvInstalments.Size = New System.Drawing.Size(529, 209)
        Me.listViewHelper1.SetSmallIcons(Me.lvInstalments, "")
        Me.listViewHelper1.SetSorted(Me.lvInstalments, False)
        Me.listViewHelper1.SetSortKey(Me.lvInstalments, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvInstalments, System.Windows.Forms.SortOrder.Ascending)
        Me.lvInstalments.TabIndex = 117
        Me.lvInstalments.UseCompatibleStateImageBehavior = False
        Me.lvInstalments.View = System.Windows.Forms.View.Details
        '
        '_lvInstalments_ColumnHeader_1
        '
        Me._lvInstalments_ColumnHeader_1.Text = "Selected"
        Me._lvInstalments_ColumnHeader_1.Width = 67
        '
        '_lvInstalments_ColumnHeader_2
        '
        Me._lvInstalments_ColumnHeader_2.Text = "Instalment"
        Me._lvInstalments_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvInstalments_ColumnHeader_2.Width = 81
        '
        '_lvInstalments_ColumnHeader_3
        '
        Me._lvInstalments_ColumnHeader_3.Text = "Due Date"
        Me._lvInstalments_ColumnHeader_3.Width = 101
        '
        '_lvInstalments_ColumnHeader_4
        '
        Me._lvInstalments_ColumnHeader_4.Text = "Instalment Amount"
        Me._lvInstalments_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvInstalments_ColumnHeader_4.Width = 131
        '
        '_lvInstalments_ColumnHeader_5
        '
        Me._lvInstalments_ColumnHeader_5.Text = "Receipt Amount"
        Me._lvInstalments_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvInstalments_ColumnHeader_5.Width = 131
        '
        'cboInstalment
        '
        Me.cboInstalment.BackColor = System.Drawing.SystemColors.Window
        Me.cboInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboInstalment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInstalment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboInstalment.Location = New System.Drawing.Point(136, 22)
        Me.cboInstalment.Name = "cboInstalment"
        Me.cboInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboInstalment.Size = New System.Drawing.Size(225, 21)
        Me.cboInstalment.TabIndex = 115
        Me.cboInstalment.Tag = "F;"
        '
        'SSOveralPlanTotal
        '
        Me.SSOveralPlanTotal.BackColor = System.Drawing.SystemColors.Control
        Me.SSOveralPlanTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SSOveralPlanTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.SSOveralPlanTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSOveralPlanTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SSOveralPlanTotal.Location = New System.Drawing.Point(448, 306)
        Me.SSOveralPlanTotal.Name = "SSOveralPlanTotal"
        Me.SSOveralPlanTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SSOveralPlanTotal.Size = New System.Drawing.Size(105, 22)
        Me.SSOveralPlanTotal.TabIndex = 121
        '
        'SSThisPlanTotal
        '
        Me.SSThisPlanTotal.BackColor = System.Drawing.SystemColors.Control
        Me.SSThisPlanTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SSThisPlanTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.SSThisPlanTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSThisPlanTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SSThisPlanTotal.Location = New System.Drawing.Point(448, 277)
        Me.SSThisPlanTotal.Name = "SSThisPlanTotal"
        Me.SSThisPlanTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SSThisPlanTotal.Size = New System.Drawing.Size(105, 22)
        Me.SSThisPlanTotal.TabIndex = 119
        '
        'lblOveralTotal
        '
        Me.lblOveralTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblOveralTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOveralTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOveralTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOveralTotal.Location = New System.Drawing.Point(344, 310)
        Me.lblOveralTotal.Name = "lblOveralTotal"
        Me.lblOveralTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOveralTotal.Size = New System.Drawing.Size(97, 17)
        Me.lblOveralTotal.TabIndex = 120
        Me.lblOveralTotal.Text = "Overall Total:"
        '
        'lblPlan
        '
        Me.lblPlan.BackColor = System.Drawing.SystemColors.Control
        Me.lblPlan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPlan.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPlan.Location = New System.Drawing.Point(32, 24)
        Me.lblPlan.Name = "lblPlan"
        Me.lblPlan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlan.Size = New System.Drawing.Size(161, 17)
        Me.lblPlan.TabIndex = 114
        Me.lblPlan.Text = "Instalment Plan:"
        '
        'lblTotal
        '
        Me.lblTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotal.Location = New System.Drawing.Point(344, 280)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotal.Size = New System.Drawing.Size(105, 17)
        Me.lblTotal.TabIndex = 118
        Me.lblTotal.Text = "This Plan Total:"
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(622, 474)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "7 - Bank Guarantee"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.lvwBGDetails)
        Me.Frame3.Controls.Add(Me.Label1)
        Me.Frame3.Controls.Add(Me.lblBGPolOutstandingAmt)
        Me.Frame3.Controls.Add(Me.lblBGPolAmtToBePosted)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(16, 12)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(585, 373)
        Me.Frame3.TabIndex = 122
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Bank Guarantee Debt"
        '
        'lvwBGDetails
        '
        Me.lvwBGDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBGDetails.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwBGDetails, "")
        Me.lvwBGDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBGDetails_ColumnHeader_1, Me._lvwBGDetails_ColumnHeader_2, Me._lvwBGDetails_ColumnHeader_3, Me._lvwBGDetails_ColumnHeader_4, Me._lvwBGDetails_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwBGDetails, False)
        Me.lvwBGDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBGDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBGDetails.FullRowSelect = True
        Me.lvwBGDetails.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwBGDetails, "")
        Me.lvwBGDetails.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwBGDetails, "")
        Me.lvwBGDetails.Location = New System.Drawing.Point(4, 20)
        Me.lvwBGDetails.Name = "lvwBGDetails"
        Me.lvwBGDetails.Size = New System.Drawing.Size(573, 315)
        Me.listViewHelper1.SetSmallIcons(Me.lvwBGDetails, "")
        Me.lvwBGDetails.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwBGDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwBGDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwBGDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwBGDetails.TabIndex = 123
        Me.lvwBGDetails.UseCompatibleStateImageBehavior = False
        Me.lvwBGDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwBGDetails_ColumnHeader_1
        '
        Me._lvwBGDetails_ColumnHeader_1.Text = "BG Number"
        Me._lvwBGDetails_ColumnHeader_1.Width = 147
        '
        '_lvwBGDetails_ColumnHeader_2
        '
        Me._lvwBGDetails_ColumnHeader_2.Text = "Due Date"
        Me._lvwBGDetails_ColumnHeader_2.Width = 97
        '
        '_lvwBGDetails_ColumnHeader_3
        '
        Me._lvwBGDetails_ColumnHeader_3.Text = "Policy Number"
        Me._lvwBGDetails_ColumnHeader_3.Width = 126
        '
        '_lvwBGDetails_ColumnHeader_4
        '
        Me._lvwBGDetails_ColumnHeader_4.Text = "Premium Amount"
        Me._lvwBGDetails_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwBGDetails_ColumnHeader_4.Width = 99
        '
        '_lvwBGDetails_ColumnHeader_5
        '
        Me._lvwBGDetails_ColumnHeader_5.Text = "Amt to be Posted"
        Me._lvwBGDetails_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwBGDetails_ColumnHeader_5.Width = 99
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "add")
        Me.ImageList1.Images.SetKeyName(1, "history")
        Me.ImageList1.Images.SetKeyName(2, "edited")
        Me.ImageList1.Images.SetKeyName(3, "delete")
        Me.ImageList1.Images.SetKeyName(4, "saved")
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(290, 340)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(78, 17)
        Me.Label1.TabIndex = 124
        Me.Label1.Text = "Total Amount:"
        '
        'lblBGPolOutstandingAmt
        '
        Me.lblBGPolOutstandingAmt.BackColor = System.Drawing.SystemColors.Control
        Me.lblBGPolOutstandingAmt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBGPolOutstandingAmt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBGPolOutstandingAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBGPolOutstandingAmt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBGPolOutstandingAmt.Location = New System.Drawing.Point(374, 338)
        Me.lblBGPolOutstandingAmt.Name = "lblBGPolOutstandingAmt"
        Me.lblBGPolOutstandingAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBGPolOutstandingAmt.Size = New System.Drawing.Size(98, 19)
        Me.lblBGPolOutstandingAmt.TabIndex = 125
        Me.lblBGPolOutstandingAmt.Text = "0.00"
        Me.lblBGPolOutstandingAmt.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBGPolAmtToBePosted
        '
        Me.lblBGPolAmtToBePosted.BackColor = System.Drawing.SystemColors.Control
        Me.lblBGPolAmtToBePosted.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBGPolAmtToBePosted.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBGPolAmtToBePosted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBGPolAmtToBePosted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBGPolAmtToBePosted.Location = New System.Drawing.Point(472, 338)
        Me.lblBGPolAmtToBePosted.Name = "lblBGPolAmtToBePosted"
        Me.lblBGPolAmtToBePosted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBGPolAmtToBePosted.Size = New System.Drawing.Size(98, 19)
        Me.lblBGPolAmtToBePosted.TabIndex = 126
        Me.lblBGPolAmtToBePosted.Text = "0.00"
        Me.lblBGPolAmtToBePosted.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(462, 528)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
        Me.cmdCancel.TabIndex = 130
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(542, 528)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 23)
        Me.cmdHelp.TabIndex = 131
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(302, 528)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 23)
        Me.cmdNavigate.TabIndex = 128
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdCommit
        '
        Me.cmdCommit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCommit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCommit.Enabled = False
        Me.cmdCommit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCommit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCommit.Location = New System.Drawing.Point(222, 528)
        Me.cmdCommit.Name = "cmdCommit"
        Me.cmdCommit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCommit.Size = New System.Drawing.Size(73, 23)
        Me.cmdCommit.TabIndex = 127
        Me.cmdCommit.Text = "&Commit"
        Me.cmdCommit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCommit.UseVisualStyleBackColor = False
        Me.cmdCommit.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(383, 528)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 23)
        Me.cmdOK.TabIndex = 129
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'txtBIC
        '
        Me.txtBIC.AcceptsReturn = True
        Me.txtBIC.BackColor = System.Drawing.SystemColors.Window
        Me.txtBIC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBIC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBIC.Location = New System.Drawing.Point(144, 113)
        Me.txtBIC.MaxLength = 50
        Me.txtBIC.Name = "txtBIC"
        Me.txtBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBIC.Size = New System.Drawing.Size(145, 21)
        Me.txtBIC.TabIndex = 93
        Me.txtBIC.Tag = "F;"
        '
        'lblBIC
        '
        Me.lblBIC.BackColor = System.Drawing.SystemColors.Control
        Me.lblBIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBIC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblBIC.Location = New System.Drawing.Point(8, 113)
        Me.lblBIC.Name = "lblBIC"
        Me.lblBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBIC.Size = New System.Drawing.Size(121, 17)
        Me.lblBIC.TabIndex = 92
        Me.lblBIC.Text = "BIC:"
        '
        'txtIBAN
        '
        Me.txtIBAN.AcceptsReturn = True
        Me.txtIBAN.BackColor = System.Drawing.SystemColors.Window
        Me.txtIBAN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIBAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIBAN.Location = New System.Drawing.Point(416, 114)
        Me.txtIBAN.MaxLength = 50
        Me.txtIBAN.Name = "txtIBAN"
        Me.txtIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIBAN.Size = New System.Drawing.Size(145, 21)
        Me.txtIBAN.TabIndex = 95
        Me.txtIBAN.Tag = "F;"
        '
        'lblIBAN
        '
        Me.lblIBAN.BackColor = System.Drawing.SystemColors.Control
        Me.lblIBAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIBAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblIBAN.Location = New System.Drawing.Point(296, 113)
        Me.lblIBAN.Name = "lblIBAN"
        Me.lblIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIBAN.Size = New System.Drawing.Size(121, 17)
        Me.lblIBAN.TabIndex = 94
        Me.lblIBAN.Text = "IBAN:"
		'
        'cboInsuranceRef
        '
        Me.cboInsuranceRef.FormattingEnabled = True
        Me.cboInsuranceRef.Location = New System.Drawing.Point(143, 55)
        Me.cboInsuranceRef.Name = "cboInsuranceRef"
        Me.cboInsuranceRef.Size = New System.Drawing.Size(138, 21)
        Me.cboInsuranceRef.TabIndex = 35
        '
        'lblInsuranceRef
        '
        Me.lblInsuranceRef.AutoSize = True
        Me.lblInsuranceRef.Location = New System.Drawing.Point(9, 58)
        Me.lblInsuranceRef.Name = "lblInsuranceRef"
        Me.lblInsuranceRef.Size = New System.Drawing.Size(89, 13)
        Me.lblInsuranceRef.TabIndex = 36
        Me.lblInsuranceRef.Text = "Policy Number"
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(637, 560)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdCommit)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(99, 193)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Cash List Item"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraPostInfo.ResumeLayout(False)
        Me.fraPostInfo.PerformLayout()
        Me.fraTransInfo.ResumeLayout(False)
        Me.fraTransInfo.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._fraReceipt_1.ResumeLayout(False)
        Me._fraReceipt_2.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me._fraReceipt_3.ResumeLayout(False)
        Me._fraReceipt_3.PerformLayout()
        Me._fraReceipt_0.ResumeLayout(False)
        Me._fraReceipt_0.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraBank.ResumeLayout(False)
        Me.fraBank.PerformLayout()
        Me.fraPaymentCreditCard.ResumeLayout(False)
        Me.fraPayee.ResumeLayout(False)
        Me.fraPayee.PerformLayout()
        Me.fraPayment.ResumeLayout(False)
        Me.fraPaymentAccountType.ResumeLayout(False)
        Me.fraPaymentAccountType.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraFurtherDetails.ResumeLayout(False)
        Me.fraFurtherDetails.PerformLayout()
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraAddFields.ResumeLayout(False)
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraInstalments.ResumeLayout(False)
        Me.fralvInstalments.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializefraReceipt()
        Me.fraReceipt(0) = _fraReceipt_0
        Me.fraReceipt(3) = _fraReceipt_3
        Me.fraReceipt(2) = _fraReceipt_2
        Me.fraReceipt(1) = _fraReceipt_1
    End Sub
    Friend WithEvents chkIsLeadAccount As System.Windows.Forms.CheckBox
    Friend WithEvents lblSplitTotal As System.Windows.Forms.Label
    Friend WithEvents txtSplitTotal As System.Windows.Forms.TextBox
    Public WithEvents lblTaxBand As System.Windows.Forms.Label
    Public WithEvents cboTaxBand As System.Windows.Forms.ComboBox
    Public WithEvents lblTaxAmount As System.Windows.Forms.Label
    Public WithEvents txtTaxAmount As System.Windows.Forms.TextBox
    Public WithEvents txtIBAN As System.Windows.Forms.TextBox
    Public WithEvents lblIBAN As System.Windows.Forms.Label
    Public WithEvents txtBIC As System.Windows.Forms.TextBox
    Public WithEvents lblBIC As System.Windows.Forms.Label
	Friend WithEvents lblInsuranceRef As Label
    Friend WithEvents cboInsuranceRef As ComboBox
#End Region
End Class
