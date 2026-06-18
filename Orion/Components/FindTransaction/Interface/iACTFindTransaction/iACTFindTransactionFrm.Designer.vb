<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		tabMainPreviousTab = tabMain.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
    Private Sub Ctx_mnuListView_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuListView.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuListView.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuListView.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuListView.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuListView_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuListView.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuListView.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuListView.DropDownItems.Add(item)
        Next item
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
    Public WithEvents cmdReverseAndReplace As System.Windows.Forms.Button
    Public WithEvents RowPicture As System.Windows.Forms.PictureBox
    Private WithEvents _Event As System.Windows.Forms.ToolStripButton
    Private WithEvents _tbrMain_Button2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _Risk_Details As System.Windows.Forms.ToolStripButton
    Private WithEvents _tbrMain_Button4 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _Information_Checklist As System.Windows.Forms.ToolStripButton
    Public WithEvents tbrMain As System.Windows.Forms.ToolStrip
    Public WithEvents cmdExpand As System.Windows.Forms.Button
    Public WithEvents cmdViewAllocation As System.Windows.Forms.Button
    Public WithEvents cmdReverse As System.Windows.Forms.Button
    Public WithEvents cmdBalance As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents panDefaultedInstalments As System.Windows.Forms.Label
    Public WithEvents panInvoiceBalance As System.Windows.Forms.Label
    Public WithEvents panPhoneExtension As System.Windows.Forms.Label
    Public WithEvents panPhoneNumber As System.Windows.Forms.Label
    Public WithEvents panPhoneAreaCode As System.Windows.Forms.Label
    Public WithEvents panContactName As System.Windows.Forms.Label
    Public WithEvents panAccountName As System.Windows.Forms.Label
    Public WithEvents panStatus As System.Windows.Forms.Label
    Public WithEvents lblSource As System.Windows.Forms.Label
    Public WithEvents lblDefaultedInstalments As System.Windows.Forms.Label
    Public WithEvents lblReason As System.Windows.Forms.Label
    Public WithEvents lblAccountName As System.Windows.Forms.Label
    Public WithEvents lblTelephone As System.Windows.Forms.Label
    Public WithEvents lblContactName As System.Windows.Forms.Label
    Public WithEvents lblAccountCode As System.Windows.Forms.Label
    Public WithEvents lblInvoiceBalance As System.Windows.Forms.Label
    Public WithEvents lblYTDTurnover As System.Windows.Forms.Label
    Public WithEvents lblYTDIncome As System.Windows.Forms.Label
    Public WithEvents lblYTDNetIncome As System.Windows.Forms.Label
    Public WithEvents panYTDTurnover As System.Windows.Forms.Label
    Public WithEvents panYTDIncome As System.Windows.Forms.Label
    Public WithEvents panYTDNetIncome As System.Windows.Forms.Label
    Public WithEvents BranchOsBalInBaseCurrency As System.Windows.Forms.Label
    Public WithEvents lblBranchOsBalInBaseCurrency As System.Windows.Forms.Label
    Public WithEvents panAccountBalance As System.Windows.Forms.Label
    Public WithEvents lblAccountBalance As System.Windows.Forms.Label
    Public WithEvents lblPolicyBalance As System.Windows.Forms.Label
    Public WithEvents lblPremiumFinanceBalance As System.Windows.Forms.Label
    Public WithEvents lblSelectedItemBalance As System.Windows.Forms.Label
    Public WithEvents panPolicyBalance As System.Windows.Forms.Label
    Public WithEvents panPremiumFinanceBalance As System.Windows.Forms.Label
    Public WithEvents panSelectedItemBalance As System.Windows.Forms.Label
    Public WithEvents fraBalanceColumn As System.Windows.Forms.GroupBox
    Public WithEvents cboSource As System.Windows.Forms.ComboBox
    Public WithEvents chkBalance As System.Windows.Forms.CheckBox
    Public WithEvents chkDisplayOutstanding As System.Windows.Forms.CheckBox
    Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
    Public WithEvents cmdAccountCode As System.Windows.Forms.Button
    Public WithEvents chkDisplay500 As System.Windows.Forms.CheckBox
    Public WithEvents pnlReason As System.Windows.Forms.TextBox
    Public WithEvents chkIncReversedTxs As System.Windows.Forms.CheckBox
    Public WithEvents optAmountTransaction As System.Windows.Forms.RadioButton
    Public WithEvents optAmountAccount As System.Windows.Forms.RadioButton
    Public WithEvents optAmountBase As System.Windows.Forms.RadioButton
    Public WithEvents fraAmountColumn As System.Windows.Forms.GroupBox
    Public WithEvents optOutstandingAccount As System.Windows.Forms.RadioButton
    Public WithEvents optOutstandingBase As System.Windows.Forms.RadioButton
    Public WithEvents optOutstandingTransaction As System.Windows.Forms.RadioButton
    Public WithEvents fraOutstandingColumn As System.Windows.Forms.GroupBox
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cmbAccountType As System.Windows.Forms.ComboBox
    Public WithEvents txtDocumentRef As System.Windows.Forms.TextBox
    Public WithEvents cmbDocTypeGroup As System.Windows.Forms.ComboBox
    Public WithEvents cmbDocumentType As System.Windows.Forms.ComboBox
    Public WithEvents cmbPeriod As System.Windows.Forms.ComboBox
    Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
    Public WithEvents txtDateTo As System.Windows.Forms.TextBox
    Public WithEvents lblAccountType As System.Windows.Forms.Label
    Public WithEvents lblDateTo As System.Windows.Forms.Label
    Public WithEvents lblDateFrom As System.Windows.Forms.Label
    Public WithEvents lblDocumentRef As System.Windows.Forms.Label
    Public WithEvents lblDocumentType As System.Windows.Forms.Label
    Public WithEvents lblDocTypeGroup As System.Windows.Forms.Label
    Public WithEvents lblPeriod As System.Windows.Forms.Label
    Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents panTranCurrOS As System.Windows.Forms.Label
    Public WithEvents lblTranCurrOS As System.Windows.Forms.Label
    Public WithEvents lblCurrencyAmount As System.Windows.Forms.Label
    Public WithEvents lblTolerance As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents cmbCurrency As PMLookupControl.cboPMLookup
    Public WithEvents txtTolerance As System.Windows.Forms.TextBox
    Public WithEvents txtCurrencyAmount As System.Windows.Forms.TextBox
    Private WithEvents _tabMain_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents txtBGRef As System.Windows.Forms.TextBox
    Public WithEvents txtAltRef As System.Windows.Forms.TextBox
    Public WithEvents cmdInsuredAccountCode As System.Windows.Forms.Button
    Public WithEvents txtInsuredAccountCode As System.Windows.Forms.TextBox
    Public WithEvents txtPurchaseInvoiceNo As System.Windows.Forms.TextBox
    Public WithEvents txtInsuranceRef As System.Windows.Forms.TextBox
    Public WithEvents txtPurchaseOrderNo As System.Windows.Forms.TextBox
    Public WithEvents txtSpare As System.Windows.Forms.TextBox
    Public WithEvents cboDepartment As PMLookupControl.cboPMLookup
    Public WithEvents cboUnderwritingYearID As PMLookupControl.cboPMLookup
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents panInsuredAccountName As System.Windows.Forms.Label
    Public WithEvents lblInsuranceRef As System.Windows.Forms.Label
    Public WithEvents lblPurchaseOrderNo As System.Windows.Forms.Label
    Public WithEvents lblOperatorID As System.Windows.Forms.Label
    Public WithEvents lblDepartment As System.Windows.Forms.Label
    Public WithEvents lblPurchaseInvoiceNo As System.Windows.Forms.Label
    Public WithEvents lblSpare As System.Windows.Forms.Label
    Public WithEvents lblUnderwritingYearID As System.Windows.Forms.Label
    Private WithEvents _tabMain_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents tabMain As System.Windows.Forms.TabControl
    Public WithEvents cmdAllocate As System.Windows.Forms.Button
    Public WithEvents cmdFindDocTrans As System.Windows.Forms.Button
    Public WithEvents cmdFindAccTrans As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _stbStatus_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _stbStatus_Panel3 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _stbStatus_Panel4 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Private WithEvents _lvwSearchResults_ColumnHeader_0 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_21 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_22 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_23 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_24 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_25 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_26 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_27 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_28 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_29 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_30 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_31 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_32 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_33 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_34 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchResults_ColumnHeader_35 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSearchResults As System.Windows.Forms.ListView
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    Public WithEvents Ctx_mnuListView As System.Windows.Forms.ContextMenuStrip
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReverseAndReplace = New System.Windows.Forms.Button()
        Me.RowPicture = New System.Windows.Forms.PictureBox()
        Me.tbrMain = New System.Windows.Forms.ToolStrip()
        Me._Event = New System.Windows.Forms.ToolStripButton()
        Me._tbrMain_Button2 = New System.Windows.Forms.ToolStripSeparator()
        Me._Risk_Details = New System.Windows.Forms.ToolStripButton()
        Me._tbrMain_Button4 = New System.Windows.Forms.ToolStripSeparator()
        Me._Information_Checklist = New System.Windows.Forms.ToolStripButton()
        Me.cmdExpand = New System.Windows.Forms.Button()
        Me.cmdViewAllocation = New System.Windows.Forms.Button()
        Me.cmdReverse = New System.Windows.Forms.Button()
        Me.cmdBalance = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage()
        Me.panAgentCode = New System.Windows.Forms.Label()
        Me.panAgentName = New System.Windows.Forms.Label()
        Me.lblAgentName = New System.Windows.Forms.Label()
        Me.lblAgentCode = New System.Windows.Forms.Label()
        Me.panDefaultedInstalments = New System.Windows.Forms.Label()
        Me.panInvoiceBalance = New System.Windows.Forms.Label()
        Me.panPhoneExtension = New System.Windows.Forms.Label()
        Me.panPhoneNumber = New System.Windows.Forms.Label()
        Me.panPhoneAreaCode = New System.Windows.Forms.Label()
        Me.panContactName = New System.Windows.Forms.Label()
        Me.panAccountName = New System.Windows.Forms.Label()
        Me.panStatus = New System.Windows.Forms.Label()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.lblDefaultedInstalments = New System.Windows.Forms.Label()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.lblAccountName = New System.Windows.Forms.Label()
        Me.lblTelephone = New System.Windows.Forms.Label()
        Me.lblContactName = New System.Windows.Forms.Label()
        Me.lblAccountCode = New System.Windows.Forms.Label()
        Me.lblInvoiceBalance = New System.Windows.Forms.Label()
        Me.lblYTDTurnover = New System.Windows.Forms.Label()
        Me.lblYTDIncome = New System.Windows.Forms.Label()
        Me.lblYTDNetIncome = New System.Windows.Forms.Label()
        Me.panYTDTurnover = New System.Windows.Forms.Label()
        Me.panYTDIncome = New System.Windows.Forms.Label()
        Me.panYTDNetIncome = New System.Windows.Forms.Label()
        Me.BranchOsBalInBaseCurrency = New System.Windows.Forms.Label()
        Me.lblBranchOsBalInBaseCurrency = New System.Windows.Forms.Label()
        Me.fraBalanceColumn = New System.Windows.Forms.GroupBox()
        Me.panAccountBalance = New System.Windows.Forms.Label()
        Me.lblAccountBalance = New System.Windows.Forms.Label()
        Me.lblPolicyBalance = New System.Windows.Forms.Label()
        Me.lblPremiumFinanceBalance = New System.Windows.Forms.Label()
        Me.lblSelectedItemBalance = New System.Windows.Forms.Label()
        Me.panPolicyBalance = New System.Windows.Forms.Label()
        Me.panPremiumFinanceBalance = New System.Windows.Forms.Label()
        Me.panSelectedItemBalance = New System.Windows.Forms.Label()
        Me.cboSource = New System.Windows.Forms.ComboBox()
        Me.chkBalance = New System.Windows.Forms.CheckBox()
        Me.chkDisplayOutstanding = New System.Windows.Forms.CheckBox()
        Me.txtAccountCode = New System.Windows.Forms.TextBox()
        Me.cmdAccountCode = New System.Windows.Forms.Button()
        Me.chkDisplay500 = New System.Windows.Forms.CheckBox()
        Me.pnlReason = New System.Windows.Forms.TextBox()
        Me.chkIncReversedTxs = New System.Windows.Forms.CheckBox()
        Me.fraAmountColumn = New System.Windows.Forms.GroupBox()
        Me.optAmountTransaction = New System.Windows.Forms.RadioButton()
        Me.optAmountAccount = New System.Windows.Forms.RadioButton()
        Me.optAmountBase = New System.Windows.Forms.RadioButton()
        Me.fraOutstandingColumn = New System.Windows.Forms.GroupBox()
        Me.optOutstandingAccount = New System.Windows.Forms.RadioButton()
        Me.optOutstandingBase = New System.Windows.Forms.RadioButton()
        Me.optOutstandingTransaction = New System.Windows.Forms.RadioButton()
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtDueDateTo = New System.Windows.Forms.TextBox()
        Me.lblDueDateTo = New System.Windows.Forms.Label()
        Me.txtDueDateFrom = New System.Windows.Forms.TextBox()
        Me.lblDueDateFrom = New System.Windows.Forms.Label()
        Me.cmbAccountType = New System.Windows.Forms.ComboBox()
        Me.txtDocumentRef = New System.Windows.Forms.TextBox()
        Me.cmbDocTypeGroup = New System.Windows.Forms.ComboBox()
        Me.cmbDocumentType = New System.Windows.Forms.ComboBox()
        Me.cmbPeriod = New System.Windows.Forms.ComboBox()
        Me.txtDateFrom = New System.Windows.Forms.TextBox()
        Me.txtDateTo = New System.Windows.Forms.TextBox()
        Me.lblAccountType = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblDocumentRef = New System.Windows.Forms.Label()
        Me.lblDocumentType = New System.Windows.Forms.Label()
        Me.lblDocTypeGroup = New System.Windows.Forms.Label()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me._tabMain_TabPage2 = New System.Windows.Forms.TabPage()
        Me.udTolerance = New System.Windows.Forms.NumericUpDown()
        Me.panTranCurrOS = New System.Windows.Forms.Label()
        Me.lblTranCurrOS = New System.Windows.Forms.Label()
        Me.lblCurrencyAmount = New System.Windows.Forms.Label()
        Me.lblTolerance = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.txtTolerance = New System.Windows.Forms.TextBox()
        Me.txtCurrencyAmount = New System.Windows.Forms.TextBox()
        Me.cmbCurrency = New PMLookupControl.cboPMLookup()
        Me._tabMain_TabPage3 = New System.Windows.Forms.TabPage()
        Me.cmdCaseNumber = New System.Windows.Forms.Button()
        Me.txtCaseNumber = New System.Windows.Forms.TextBox()
        Me.txtBGRef = New System.Windows.Forms.TextBox()
        Me.txtAltRef = New System.Windows.Forms.TextBox()
        Me.cmdInsuredAccountCode = New System.Windows.Forms.Button()
        Me.txtInsuredAccountCode = New System.Windows.Forms.TextBox()
        Me.txtPurchaseInvoiceNo = New System.Windows.Forms.TextBox()
        Me.txtInsuranceRef = New System.Windows.Forms.TextBox()
        Me.txtPurchaseOrderNo = New System.Windows.Forms.TextBox()
        Me.txtSpare = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.panInsuredAccountName = New System.Windows.Forms.Label()
        Me.lblInsuranceRef = New System.Windows.Forms.Label()
        Me.lblPurchaseOrderNo = New System.Windows.Forms.Label()
        Me.lblOperatorID = New System.Windows.Forms.Label()
        Me.lblDepartment = New System.Windows.Forms.Label()
        Me.lblPurchaseInvoiceNo = New System.Windows.Forms.Label()
        Me.lblSpare = New System.Windows.Forms.Label()
        Me.lblUnderwritingYearID = New System.Windows.Forms.Label()
        Me.cboDepartment = New PMLookupControl.cboPMLookup()
        Me.cboUnderwritingYearID = New PMLookupControl.cboPMLookup()
        Me.cboPMUser = New PMUserLookupControl.cboPMUserLookup()
        Me.cmdAllocate = New System.Windows.Forms.Button()
        Me.cmdFindDocTrans = New System.Windows.Forms.Button()
        Me.cmdFindAccTrans = New System.Windows.Forms.Button()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._stbStatus_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._stbStatus_Panel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._stbStatus_Panel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._stbStatus_Panel5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._stbStatus_Panel6 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lvwSearchResults = New System.Windows.Forms.ListView()
        Me._lvwSearchResults_ColumnHeader_0 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_36 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_20 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_21 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_22 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_23 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_24 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_25 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_26 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_27 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_28 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_29 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_30 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_31 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_32 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_33 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_34 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_35 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchResults_ColumnHeader_37 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ImgImage = New System.Windows.Forms.PictureBox()
        Me.Ctx_mnuListView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFindAccount = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFindDocument = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInstalmentPlan = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTransactionWriteOff = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTransfer = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRefund = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuApprove = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReject = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep7 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAddComment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditComment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep8 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReportOnThis = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDoNotReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep9 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuBreakdown = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.cmdSplitReceipt = New System.Windows.Forms.Button()
        Me.cmdEditSplit = New System.Windows.Forms.Button()
        CType(Me.RowPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbrMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.fraBalanceColumn.SuspendLayout()
        Me.fraAmountColumn.SuspendLayout()
        Me.fraOutstandingColumn.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        Me._tabMain_TabPage2.SuspendLayout()
        CType(Me.udTolerance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMain_TabPage3.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdReverseAndReplace
        '
        Me.cmdReverseAndReplace.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReverseAndReplace.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReverseAndReplace.Enabled = False
        Me.cmdReverseAndReplace.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReverseAndReplace.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReverseAndReplace.Location = New System.Drawing.Point(248, 424)
        Me.cmdReverseAndReplace.Name = "cmdReverseAndReplace"
        Me.cmdReverseAndReplace.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReverseAndReplace.Size = New System.Drawing.Size(127, 23)
        Me.cmdReverseAndReplace.TabIndex = 78
        Me.cmdReverseAndReplace.Text = "&Reverse And Replace"
        Me.cmdReverseAndReplace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReverseAndReplace.UseVisualStyleBackColor = False
        '
        'RowPicture
        '
        Me.RowPicture.BackColor = System.Drawing.SystemColors.Control
        Me.RowPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.RowPicture.Cursor = System.Windows.Forms.Cursors.Default
        Me.RowPicture.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RowPicture.Location = New System.Drawing.Point(855, 297)
        Me.RowPicture.Name = "RowPicture"
        Me.RowPicture.Size = New System.Drawing.Size(43, 35)
        Me.RowPicture.TabIndex = 66
        Me.RowPicture.TabStop = False
        Me.RowPicture.Visible = False
        '
        'tbrMain
        '
        Me.tbrMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Event, Me._tbrMain_Button2, Me._Risk_Details, Me._tbrMain_Button4, Me._Information_Checklist})
        Me.tbrMain.Location = New System.Drawing.Point(0, 0)
        Me.tbrMain.Name = "tbrMain"
        Me.tbrMain.Size = New System.Drawing.Size(1241, 25)
        Me.tbrMain.TabIndex = 0
        '
        '_Event
        '
        Me._Event.AutoSize = False
        Me._Event.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Event.Name = "_Event"
        Me._Event.Size = New System.Drawing.Size(24, 22)
        Me._Event.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Event.ToolTipText = "Event"
        '
        '_tbrMain_Button2
        '
        Me._tbrMain_Button2.AutoSize = False
        Me._tbrMain_Button2.Name = "_tbrMain_Button2"
        Me._tbrMain_Button2.Size = New System.Drawing.Size(6, 22)
        Me._tbrMain_Button2.Visible = False
        '
        '_Risk_Details
        '
        Me._Risk_Details.AutoSize = False
        Me._Risk_Details.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Risk_Details.Name = "_Risk_Details"
        Me._Risk_Details.Size = New System.Drawing.Size(24, 22)
        Me._Risk_Details.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Risk_Details.ToolTipText = "Risk Details"
        '
        '_tbrMain_Button4
        '
        Me._tbrMain_Button4.AutoSize = False
        Me._tbrMain_Button4.Name = "_tbrMain_Button4"
        Me._tbrMain_Button4.Size = New System.Drawing.Size(6, 22)
        Me._tbrMain_Button4.Visible = False
        '
        '_Information_Checklist
        '
        Me._Information_Checklist.AutoSize = False
        Me._Information_Checklist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Information_Checklist.Name = "_Information_Checklist"
        Me._Information_Checklist.Size = New System.Drawing.Size(24, 22)
        Me._Information_Checklist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Information_Checklist.ToolTipText = "Information Checklist"
        '
        'cmdExpand
        '
        Me.cmdExpand.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExpand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExpand.Enabled = False
        Me.cmdExpand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExpand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExpand.Location = New System.Drawing.Point(576, 422)
        Me.cmdExpand.Name = "cmdExpand"
        Me.cmdExpand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExpand.Size = New System.Drawing.Size(54, 23)
        Me.cmdExpand.TabIndex = 72
        Me.cmdExpand.TabStop = False
        Me.cmdExpand.Text = "&Expand"
        Me.cmdExpand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExpand.UseVisualStyleBackColor = False
        '
        'cmdViewAllocation
        '
        Me.cmdViewAllocation.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewAllocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewAllocation.Enabled = False
        Me.cmdViewAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewAllocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewAllocation.Location = New System.Drawing.Point(449, 422)
        Me.cmdViewAllocation.Name = "cmdViewAllocation"
        Me.cmdViewAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewAllocation.Size = New System.Drawing.Size(97, 23)
        Me.cmdViewAllocation.TabIndex = 71
        Me.cmdViewAllocation.TabStop = False
        Me.cmdViewAllocation.Text = "&View Allocation"
        Me.cmdViewAllocation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewAllocation.UseVisualStyleBackColor = False
        '
        'cmdReverse
        '
        Me.cmdReverse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReverse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReverse.Enabled = False
        Me.cmdReverse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReverse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReverse.Location = New System.Drawing.Point(168, 424)
        Me.cmdReverse.Name = "cmdReverse"
        Me.cmdReverse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReverse.Size = New System.Drawing.Size(73, 23)
        Me.cmdReverse.TabIndex = 69
        Me.cmdReverse.Text = "&Reverse"
        Me.cmdReverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReverse.UseVisualStyleBackColor = False
        '
        'cmdBalance
        '
        Me.cmdBalance.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBalance.Location = New System.Drawing.Point(880, 112)
        Me.cmdBalance.Name = "cmdBalance"
        Me.cmdBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBalance.Size = New System.Drawing.Size(79, 23)
        Me.cmdBalance.TabIndex = 63
        Me.cmdBalance.TabStop = False
        Me.cmdBalance.Text = "&Balance"
        Me.cmdBalance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBalance.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(880, 144)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(79, 23)
        Me.cmdNewSearch.TabIndex = 64
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage2)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage3)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(222, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 28)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1233, 255)
        Me.tabMain.TabIndex = 1
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.panAgentCode)
        Me._tabMain_TabPage0.Controls.Add(Me.panAgentName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAgentName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAgentCode)
        Me._tabMain_TabPage0.Controls.Add(Me.panDefaultedInstalments)
        Me._tabMain_TabPage0.Controls.Add(Me.panInvoiceBalance)
        Me._tabMain_TabPage0.Controls.Add(Me.panPhoneExtension)
        Me._tabMain_TabPage0.Controls.Add(Me.panPhoneNumber)
        Me._tabMain_TabPage0.Controls.Add(Me.panPhoneAreaCode)
        Me._tabMain_TabPage0.Controls.Add(Me.panContactName)
        Me._tabMain_TabPage0.Controls.Add(Me.panAccountName)
        Me._tabMain_TabPage0.Controls.Add(Me.panStatus)
        Me._tabMain_TabPage0.Controls.Add(Me.lblSource)
        Me._tabMain_TabPage0.Controls.Add(Me.lblDefaultedInstalments)
        Me._tabMain_TabPage0.Controls.Add(Me.lblReason)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAccountName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblTelephone)
        Me._tabMain_TabPage0.Controls.Add(Me.lblContactName)
        Me._tabMain_TabPage0.Controls.Add(Me.lblAccountCode)
        Me._tabMain_TabPage0.Controls.Add(Me.lblInvoiceBalance)
        Me._tabMain_TabPage0.Controls.Add(Me.lblYTDTurnover)
        Me._tabMain_TabPage0.Controls.Add(Me.lblYTDIncome)
        Me._tabMain_TabPage0.Controls.Add(Me.lblYTDNetIncome)
        Me._tabMain_TabPage0.Controls.Add(Me.panYTDTurnover)
        Me._tabMain_TabPage0.Controls.Add(Me.panYTDIncome)
        Me._tabMain_TabPage0.Controls.Add(Me.panYTDNetIncome)
        Me._tabMain_TabPage0.Controls.Add(Me.BranchOsBalInBaseCurrency)
        Me._tabMain_TabPage0.Controls.Add(Me.lblBranchOsBalInBaseCurrency)
        Me._tabMain_TabPage0.Controls.Add(Me.fraBalanceColumn)
        Me._tabMain_TabPage0.Controls.Add(Me.cboSource)
        Me._tabMain_TabPage0.Controls.Add(Me.chkBalance)
        Me._tabMain_TabPage0.Controls.Add(Me.chkDisplayOutstanding)
        Me._tabMain_TabPage0.Controls.Add(Me.txtAccountCode)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdAccountCode)
        Me._tabMain_TabPage0.Controls.Add(Me.chkDisplay500)
        Me._tabMain_TabPage0.Controls.Add(Me.pnlReason)
        Me._tabMain_TabPage0.Controls.Add(Me.chkIncReversedTxs)
        Me._tabMain_TabPage0.Controls.Add(Me.fraAmountColumn)
        Me._tabMain_TabPage0.Controls.Add(Me.fraOutstandingColumn)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(1225, 229)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "1 - Account"
        '
        'panAgentCode
        '
        Me.panAgentCode.BackColor = System.Drawing.Color.Silver
        Me.panAgentCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.panAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAgentCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panAgentCode.Location = New System.Drawing.Point(988, 6)
        Me.panAgentCode.Name = "panAgentCode"
        Me.panAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panAgentCode.Size = New System.Drawing.Size(229, 21)
        Me.panAgentCode.TabIndex = 109
        '
        'panAgentName
        '
        Me.panAgentName.BackColor = System.Drawing.Color.Silver
        Me.panAgentName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.panAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panAgentName.Location = New System.Drawing.Point(988, 34)
        Me.panAgentName.Name = "panAgentName"
        Me.panAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panAgentName.Size = New System.Drawing.Size(229, 21)
        Me.panAgentName.TabIndex = 107
        '
        'lblAgentName
        '
        Me.lblAgentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentName.Location = New System.Drawing.Point(903, 37)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentName.Size = New System.Drawing.Size(95, 19)
        Me.lblAgentName.TabIndex = 108
        Me.lblAgentName.Text = "Account Name:"
        '
        'lblAgentCode
        '
        Me.lblAgentCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentCode.Location = New System.Drawing.Point(903, 10)
        Me.lblAgentCode.Name = "lblAgentCode"
        Me.lblAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentCode.Size = New System.Drawing.Size(57, 19)
        Me.lblAgentCode.TabIndex = 106
        Me.lblAgentCode.Text = "&Account:"
        '
        'panDefaultedInstalments
        '
        Me.panDefaultedInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.panDefaultedInstalments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panDefaultedInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.panDefaultedInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panDefaultedInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panDefaultedInstalments.Location = New System.Drawing.Point(612, 72)
        Me.panDefaultedInstalments.Name = "panDefaultedInstalments"
        Me.panDefaultedInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panDefaultedInstalments.Size = New System.Drawing.Size(181, 21)
        Me.panDefaultedInstalments.TabIndex = 46
        Me.panDefaultedInstalments.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'panInvoiceBalance
        '
        Me.panInvoiceBalance.BackColor = System.Drawing.SystemColors.Control
        Me.panInvoiceBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panInvoiceBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panInvoiceBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panInvoiceBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panInvoiceBalance.Location = New System.Drawing.Point(612, 30)
        Me.panInvoiceBalance.Name = "panInvoiceBalance"
        Me.panInvoiceBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panInvoiceBalance.Size = New System.Drawing.Size(181, 21)
        Me.panInvoiceBalance.TabIndex = 21
        Me.panInvoiceBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'panPhoneExtension
        '
        Me.panPhoneExtension.BackColor = System.Drawing.Color.Silver
        Me.panPhoneExtension.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPhoneExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.panPhoneExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPhoneExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panPhoneExtension.Location = New System.Drawing.Point(322, 99)
        Me.panPhoneExtension.Name = "panPhoneExtension"
        Me.panPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPhoneExtension.Size = New System.Drawing.Size(84, 21)
        Me.panPhoneExtension.TabIndex = 51
        '
        'panPhoneNumber
        '
        Me.panPhoneNumber.BackColor = System.Drawing.Color.Silver
        Me.panPhoneNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPhoneNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.panPhoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPhoneNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panPhoneNumber.Location = New System.Drawing.Point(202, 99)
        Me.panPhoneNumber.Name = "panPhoneNumber"
        Me.panPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPhoneNumber.Size = New System.Drawing.Size(115, 21)
        Me.panPhoneNumber.TabIndex = 50
        '
        'panPhoneAreaCode
        '
        Me.panPhoneAreaCode.BackColor = System.Drawing.Color.Silver
        Me.panPhoneAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.panPhoneAreaCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPhoneAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panPhoneAreaCode.Location = New System.Drawing.Point(106, 99)
        Me.panPhoneAreaCode.Name = "panPhoneAreaCode"
        Me.panPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPhoneAreaCode.Size = New System.Drawing.Size(89, 21)
        Me.panPhoneAreaCode.TabIndex = 49
        '
        'panContactName
        '
        Me.panContactName.BackColor = System.Drawing.Color.Silver
        Me.panContactName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.panContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panContactName.Location = New System.Drawing.Point(106, 70)
        Me.panContactName.Name = "panContactName"
        Me.panContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panContactName.Size = New System.Drawing.Size(299, 21)
        Me.panContactName.TabIndex = 38
        '
        'panAccountName
        '
        Me.panAccountName.BackColor = System.Drawing.Color.Silver
        Me.panAccountName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.panAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panAccountName.Location = New System.Drawing.Point(106, 41)
        Me.panAccountName.Name = "panAccountName"
        Me.panAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panAccountName.Size = New System.Drawing.Size(299, 21)
        Me.panAccountName.TabIndex = 24
        '
        'panStatus
        '
        Me.panStatus.BackColor = System.Drawing.Color.Silver
        Me.panStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.panStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panStatus.Location = New System.Drawing.Point(314, 12)
        Me.panStatus.Name = "panStatus"
        Me.panStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panStatus.Size = New System.Drawing.Size(91, 21)
        Me.panStatus.TabIndex = 11
        Me.panStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSource
        '
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(8, 157)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(45, 13)
        Me.lblSource.TabIndex = 44
        Me.lblSource.Text = "Branch:"
        '
        'lblDefaultedInstalments
        '
        Me.lblDefaultedInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefaultedInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefaultedInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultedInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefaultedInstalments.Location = New System.Drawing.Point(614, 56)
        Me.lblDefaultedInstalments.Name = "lblDefaultedInstalments"
        Me.lblDefaultedInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefaultedInstalments.Size = New System.Drawing.Size(169, 19)
        Me.lblDefaultedInstalments.TabIndex = 32
        Me.lblDefaultedInstalments.Text = "Defaulted Instalments :"
        Me.lblDefaultedInstalments.Visible = False
        '
        'lblReason
        '
        Me.lblReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReason.Location = New System.Drawing.Point(8, 128)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReason.Size = New System.Drawing.Size(73, 19)
        Me.lblReason.TabIndex = 61
        Me.lblReason.Text = "Reason:"
        '
        'lblAccountName
        '
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(8, 41)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(95, 19)
        Me.lblAccountName.TabIndex = 27
        Me.lblAccountName.Text = "Account Name:"
        '
        'lblTelephone
        '
        Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephone.Location = New System.Drawing.Point(8, 99)
        Me.lblTelephone.Name = "lblTelephone"
        Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephone.Size = New System.Drawing.Size(89, 19)
        Me.lblTelephone.TabIndex = 52
        Me.lblTelephone.Text = "Telephone No:"
        '
        'lblContactName
        '
        Me.lblContactName.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactName.Location = New System.Drawing.Point(8, 70)
        Me.lblContactName.Name = "lblContactName"
        Me.lblContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactName.Size = New System.Drawing.Size(97, 19)
        Me.lblContactName.TabIndex = 39
        Me.lblContactName.Text = "Contact Name:"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(8, 14)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(57, 19)
        Me.lblAccountCode.TabIndex = 14
        Me.lblAccountCode.Text = "&Account:"
        '
        'lblInvoiceBalance
        '
        Me.lblInvoiceBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvoiceBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvoiceBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvoiceBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvoiceBalance.Location = New System.Drawing.Point(618, 14)
        Me.lblInvoiceBalance.Name = "lblInvoiceBalance"
        Me.lblInvoiceBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvoiceBalance.Size = New System.Drawing.Size(121, 19)
        Me.lblInvoiceBalance.TabIndex = 16
        Me.lblInvoiceBalance.Text = "Invoice Balance:"
        Me.lblInvoiceBalance.Visible = False
        '
        'lblYTDTurnover
        '
        Me.lblYTDTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.lblYTDTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYTDTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYTDTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYTDTurnover.Location = New System.Drawing.Point(800, 12)
        Me.lblYTDTurnover.Name = "lblYTDTurnover"
        Me.lblYTDTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYTDTurnover.Size = New System.Drawing.Size(89, 15)
        Me.lblYTDTurnover.TabIndex = 79
        Me.lblYTDTurnover.Text = "YTD Turnover:"
        Me.lblYTDTurnover.Visible = False
        '
        'lblYTDIncome
        '
        Me.lblYTDIncome.BackColor = System.Drawing.SystemColors.Control
        Me.lblYTDIncome.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYTDIncome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYTDIncome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYTDIncome.Location = New System.Drawing.Point(800, 76)
        Me.lblYTDIncome.Name = "lblYTDIncome"
        Me.lblYTDIncome.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYTDIncome.Size = New System.Drawing.Size(89, 23)
        Me.lblYTDIncome.TabIndex = 80
        Me.lblYTDIncome.Text = "YTD Income:"
        Me.lblYTDIncome.Visible = False
        '
        'lblYTDNetIncome
        '
        Me.lblYTDNetIncome.BackColor = System.Drawing.SystemColors.Control
        Me.lblYTDNetIncome.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYTDNetIncome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYTDNetIncome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYTDNetIncome.Location = New System.Drawing.Point(800, 140)
        Me.lblYTDNetIncome.Name = "lblYTDNetIncome"
        Me.lblYTDNetIncome.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYTDNetIncome.Size = New System.Drawing.Size(98, 23)
        Me.lblYTDNetIncome.TabIndex = 81
        Me.lblYTDNetIncome.Text = "YTD Net Income:"
        Me.lblYTDNetIncome.Visible = False
        '
        'panYTDTurnover
        '
        Me.panYTDTurnover.BackColor = System.Drawing.Color.Silver
        Me.panYTDTurnover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panYTDTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.panYTDTurnover.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panYTDTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panYTDTurnover.Location = New System.Drawing.Point(800, 36)
        Me.panYTDTurnover.Name = "panYTDTurnover"
        Me.panYTDTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panYTDTurnover.Size = New System.Drawing.Size(97, 21)
        Me.panYTDTurnover.TabIndex = 82
        Me.panYTDTurnover.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.panYTDTurnover.Visible = False
        '
        'panYTDIncome
        '
        Me.panYTDIncome.BackColor = System.Drawing.Color.Silver
        Me.panYTDIncome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panYTDIncome.Cursor = System.Windows.Forms.Cursors.Default
        Me.panYTDIncome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panYTDIncome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panYTDIncome.Location = New System.Drawing.Point(800, 100)
        Me.panYTDIncome.Name = "panYTDIncome"
        Me.panYTDIncome.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panYTDIncome.Size = New System.Drawing.Size(97, 21)
        Me.panYTDIncome.TabIndex = 83
        Me.panYTDIncome.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.panYTDIncome.Visible = False
        '
        'panYTDNetIncome
        '
        Me.panYTDNetIncome.BackColor = System.Drawing.Color.Silver
        Me.panYTDNetIncome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panYTDNetIncome.Cursor = System.Windows.Forms.Cursors.Default
        Me.panYTDNetIncome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panYTDNetIncome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panYTDNetIncome.Location = New System.Drawing.Point(800, 164)
        Me.panYTDNetIncome.Name = "panYTDNetIncome"
        Me.panYTDNetIncome.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panYTDNetIncome.Size = New System.Drawing.Size(97, 21)
        Me.panYTDNetIncome.TabIndex = 84
        Me.panYTDNetIncome.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.panYTDNetIncome.Visible = False
        '
        'BranchOsBalInBaseCurrency
        '
        Me.BranchOsBalInBaseCurrency.BackColor = System.Drawing.Color.Silver
        Me.BranchOsBalInBaseCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.BranchOsBalInBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.BranchOsBalInBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BranchOsBalInBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BranchOsBalInBaseCurrency.Location = New System.Drawing.Point(413, 157)
        Me.BranchOsBalInBaseCurrency.Name = "BranchOsBalInBaseCurrency"
        Me.BranchOsBalInBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BranchOsBalInBaseCurrency.Size = New System.Drawing.Size(181, 21)
        Me.BranchOsBalInBaseCurrency.TabIndex = 98
        Me.BranchOsBalInBaseCurrency.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBranchOsBalInBaseCurrency
        '
        Me.lblBranchOsBalInBaseCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranchOsBalInBaseCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranchOsBalInBaseCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranchOsBalInBaseCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranchOsBalInBaseCurrency.Location = New System.Drawing.Point(248, 154)
        Me.lblBranchOsBalInBaseCurrency.Name = "lblBranchOsBalInBaseCurrency"
        Me.lblBranchOsBalInBaseCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranchOsBalInBaseCurrency.Size = New System.Drawing.Size(161, 29)
        Me.lblBranchOsBalInBaseCurrency.TabIndex = 99
        Me.lblBranchOsBalInBaseCurrency.Text = "Branch Outstanding Balance in Base Currency"
        '
        'fraBalanceColumn
        '
        Me.fraBalanceColumn.BackColor = System.Drawing.SystemColors.Control
        Me.fraBalanceColumn.Controls.Add(Me.panAccountBalance)
        Me.fraBalanceColumn.Controls.Add(Me.lblAccountBalance)
        Me.fraBalanceColumn.Controls.Add(Me.lblPolicyBalance)
        Me.fraBalanceColumn.Controls.Add(Me.lblPremiumFinanceBalance)
        Me.fraBalanceColumn.Controls.Add(Me.lblSelectedItemBalance)
        Me.fraBalanceColumn.Controls.Add(Me.panPolicyBalance)
        Me.fraBalanceColumn.Controls.Add(Me.panPremiumFinanceBalance)
        Me.fraBalanceColumn.Controls.Add(Me.panSelectedItemBalance)
        Me.fraBalanceColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBalanceColumn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBalanceColumn.Location = New System.Drawing.Point(604, 3)
        Me.fraBalanceColumn.Name = "fraBalanceColumn"
        Me.fraBalanceColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBalanceColumn.Size = New System.Drawing.Size(185, 182)
        Me.fraBalanceColumn.TabIndex = 90
        Me.fraBalanceColumn.TabStop = False
        Me.fraBalanceColumn.Text = "Balance Column"
        '
        'panAccountBalance
        '
        Me.panAccountBalance.BackColor = System.Drawing.Color.Silver
        Me.panAccountBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panAccountBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panAccountBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panAccountBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panAccountBalance.Location = New System.Drawing.Point(8, 41)
        Me.panAccountBalance.Name = "panAccountBalance"
        Me.panAccountBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panAccountBalance.Size = New System.Drawing.Size(173, 21)
        Me.panAccountBalance.TabIndex = 100
        Me.panAccountBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAccountBalance
        '
        Me.lblAccountBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountBalance.Location = New System.Drawing.Point(8, 11)
        Me.lblAccountBalance.Name = "lblAccountBalance"
        Me.lblAccountBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountBalance.Size = New System.Drawing.Size(167, 28)
        Me.lblAccountBalance.TabIndex = 97
        Me.lblAccountBalance.Text = "Account Balance in Account Currency:"
        '
        'lblPolicyBalance
        '
        Me.lblPolicyBalance.AutoSize = True
        Me.lblPolicyBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyBalance.Location = New System.Drawing.Point(11, 61)
        Me.lblPolicyBalance.Name = "lblPolicyBalance"
        Me.lblPolicyBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyBalance.Size = New System.Drawing.Size(80, 13)
        Me.lblPolicyBalance.TabIndex = 96
        Me.lblPolicyBalance.Text = "Policy Balance:"
        '
        'lblPremiumFinanceBalance
        '
        Me.lblPremiumFinanceBalance.AutoSize = True
        Me.lblPremiumFinanceBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumFinanceBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumFinanceBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumFinanceBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumFinanceBalance.Location = New System.Drawing.Point(8, 99)
        Me.lblPremiumFinanceBalance.Name = "lblPremiumFinanceBalance"
        Me.lblPremiumFinanceBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumFinanceBalance.Size = New System.Drawing.Size(133, 13)
        Me.lblPremiumFinanceBalance.TabIndex = 95
        Me.lblPremiumFinanceBalance.Text = "Premium Finance Balance:"
        'Me.lblPremiumFinanceBalance.Visible = False
        '
        'lblSelectedItemBalance
        '
        Me.lblSelectedItemBalance.AutoSize = True
        Me.lblSelectedItemBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblSelectedItemBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedItemBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedItemBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSelectedItemBalance.Location = New System.Drawing.Point(8, 138)
        Me.lblSelectedItemBalance.Name = "lblSelectedItemBalance"
        Me.lblSelectedItemBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedItemBalance.Size = New System.Drawing.Size(117, 13)
        Me.lblSelectedItemBalance.TabIndex = 94
        Me.lblSelectedItemBalance.Text = "Selected Item Balance:"
        '
        'panPolicyBalance
        '
        Me.panPolicyBalance.BackColor = System.Drawing.Color.Silver
        Me.panPolicyBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPolicyBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panPolicyBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPolicyBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panPolicyBalance.Location = New System.Drawing.Point(8, 78)
        Me.panPolicyBalance.Name = "panPolicyBalance"
        Me.panPolicyBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPolicyBalance.Size = New System.Drawing.Size(173, 21)
        Me.panPolicyBalance.TabIndex = 93
        Me.panPolicyBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'panPremiumFinanceBalance
        '
        Me.panPremiumFinanceBalance.BackColor = System.Drawing.Color.Silver
        Me.panPremiumFinanceBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panPremiumFinanceBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panPremiumFinanceBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPremiumFinanceBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panPremiumFinanceBalance.Location = New System.Drawing.Point(8, 117)
        Me.panPremiumFinanceBalance.Name = "panPremiumFinanceBalance"
        Me.panPremiumFinanceBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPremiumFinanceBalance.Size = New System.Drawing.Size(173, 21)
        Me.panPremiumFinanceBalance.TabIndex = 92
        Me.panPremiumFinanceBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        'Me.panPremiumFinanceBalance.Visible = False
        '
        'panSelectedItemBalance
        '
        Me.panSelectedItemBalance.BackColor = System.Drawing.Color.Silver
        Me.panSelectedItemBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panSelectedItemBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.panSelectedItemBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panSelectedItemBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panSelectedItemBalance.Location = New System.Drawing.Point(8, 157)
        Me.panSelectedItemBalance.Name = "panSelectedItemBalance"
        Me.panSelectedItemBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panSelectedItemBalance.Size = New System.Drawing.Size(173, 21)
        Me.panSelectedItemBalance.TabIndex = 91
        Me.panSelectedItemBalance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cboSource
        '
        Me.cboSource.BackColor = System.Drawing.SystemColors.Window
        Me.cboSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSource.Location = New System.Drawing.Point(106, 157)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSource.Size = New System.Drawing.Size(127, 21)
        Me.cboSource.TabIndex = 45
        '
        'chkBalance
        '
        Me.chkBalance.BackColor = System.Drawing.SystemColors.Control
        Me.chkBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBalance.Location = New System.Drawing.Point(412, 38)
        Me.chkBalance.Name = "chkBalance"
        Me.chkBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBalance.Size = New System.Drawing.Size(180, 34)
        Me.chkBalance.TabIndex = 31
        Me.chkBalance.Tag = "SEARCH;"
        Me.chkBalance.Text = "Include future transactions in the balance"
        Me.chkBalance.UseVisualStyleBackColor = False
        '
        'chkDisplayOutstanding
        '
        Me.chkDisplayOutstanding.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayOutstanding.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayOutstanding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayOutstanding.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayOutstanding.Location = New System.Drawing.Point(412, 4)
        Me.chkDisplayOutstanding.Name = "chkDisplayOutstanding"
        Me.chkDisplayOutstanding.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayOutstanding.Size = New System.Drawing.Size(180, 34)
        Me.chkDisplayOutstanding.TabIndex = 15
        Me.chkDisplayOutstanding.Tag = "SEARCH;"
        Me.chkDisplayOutstanding.Text = "Only show outstanding transactions"
        Me.chkDisplayOutstanding.UseVisualStyleBackColor = False
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(106, 12)
        Me.txtAccountCode.MaxLength = 0
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(171, 20)
        Me.txtAccountCode.TabIndex = 10
        '
        'cmdAccountCode
        '
        Me.cmdAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccountCode.Location = New System.Drawing.Point(282, 12)
        Me.cmdAccountCode.Name = "cmdAccountCode"
        Me.cmdAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccountCode.Size = New System.Drawing.Size(22, 21)
        Me.cmdAccountCode.TabIndex = 77
        Me.cmdAccountCode.Text = "..."
        Me.cmdAccountCode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccountCode.UseVisualStyleBackColor = False
        '
        'chkDisplay500
        '
        Me.chkDisplay500.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplay500.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplay500.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplay500.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplay500.Location = New System.Drawing.Point(412, 75)
        Me.chkDisplay500.Name = "chkDisplay500"
        Me.chkDisplay500.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplay500.Size = New System.Drawing.Size(180, 34)
        Me.chkDisplay500.TabIndex = 87
        Me.chkDisplay500.Tag = "SEARCH;"
        Me.chkDisplay500.Text = "Only show latest 500 transactions"
        Me.chkDisplay500.UseVisualStyleBackColor = False
        '
        'pnlReason
        '
        Me.pnlReason.AcceptsReturn = True
        Me.pnlReason.BackColor = System.Drawing.Color.Silver
        Me.pnlReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.pnlReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.pnlReason.Location = New System.Drawing.Point(106, 128)
        Me.pnlReason.MaxLength = 0
        Me.pnlReason.Multiline = True
        Me.pnlReason.Name = "pnlReason"
        Me.pnlReason.ReadOnly = True
        Me.pnlReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.pnlReason.Size = New System.Drawing.Size(300, 21)
        Me.pnlReason.TabIndex = 60
        '
        'chkIncReversedTxs
        '
        Me.chkIncReversedTxs.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncReversedTxs.Checked = True
        Me.chkIncReversedTxs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncReversedTxs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncReversedTxs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncReversedTxs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncReversedTxs.Location = New System.Drawing.Point(412, 112)
        Me.chkIncReversedTxs.Name = "chkIncReversedTxs"
        Me.chkIncReversedTxs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncReversedTxs.Size = New System.Drawing.Size(172, 34)
        Me.chkIncReversedTxs.TabIndex = 86
        Me.chkIncReversedTxs.Text = "Include Reversed/ Reversal transactions"
        Me.chkIncReversedTxs.UseVisualStyleBackColor = False
        '
        'fraAmountColumn
        '
        Me.fraAmountColumn.BackColor = System.Drawing.SystemColors.Control
        Me.fraAmountColumn.Controls.Add(Me.optAmountTransaction)
        Me.fraAmountColumn.Controls.Add(Me.optAmountAccount)
        Me.fraAmountColumn.Controls.Add(Me.optAmountBase)
        Me.fraAmountColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAmountColumn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAmountColumn.Location = New System.Drawing.Point(105, 184)
        Me.fraAmountColumn.Name = "fraAmountColumn"
        Me.fraAmountColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAmountColumn.Size = New System.Drawing.Size(340, 39)
        Me.fraAmountColumn.TabIndex = 101
        Me.fraAmountColumn.TabStop = False
        Me.fraAmountColumn.Text = "Amount Column Currency"
        '
        'optAmountTransaction
        '
        Me.optAmountTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.optAmountTransaction.Checked = True
        Me.optAmountTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAmountTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAmountTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAmountTransaction.Location = New System.Drawing.Point(6, 18)
        Me.optAmountTransaction.Name = "optAmountTransaction"
        Me.optAmountTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAmountTransaction.Size = New System.Drawing.Size(95, 16)
        Me.optAmountTransaction.TabIndex = 104
        Me.optAmountTransaction.TabStop = True
        Me.optAmountTransaction.Tag = "SEARCH;"
        Me.optAmountTransaction.Text = "Transaction"
        Me.optAmountTransaction.UseVisualStyleBackColor = False
        '
        'optAmountAccount
        '
        Me.optAmountAccount.BackColor = System.Drawing.SystemColors.Control
        Me.optAmountAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAmountAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAmountAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAmountAccount.Location = New System.Drawing.Point(165, 18)
        Me.optAmountAccount.Name = "optAmountAccount"
        Me.optAmountAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAmountAccount.Size = New System.Drawing.Size(172, 16)
        Me.optAmountAccount.TabIndex = 103
        Me.optAmountAccount.TabStop = True
        Me.optAmountAccount.Tag = "SEARCH;"
        Me.optAmountAccount.Text = "Account (x)"
        Me.optAmountAccount.UseVisualStyleBackColor = False
        '
        'optAmountBase
        '
        Me.optAmountBase.BackColor = System.Drawing.SystemColors.Control
        Me.optAmountBase.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAmountBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAmountBase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAmountBase.Location = New System.Drawing.Point(102, 18)
        Me.optAmountBase.Name = "optAmountBase"
        Me.optAmountBase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAmountBase.Size = New System.Drawing.Size(52, 16)
        Me.optAmountBase.TabIndex = 102
        Me.optAmountBase.TabStop = True
        Me.optAmountBase.Tag = "SEARCH;"
        Me.optAmountBase.Text = "Base"
        Me.optAmountBase.UseVisualStyleBackColor = False
        '
        'fraOutstandingColumn
        '
        Me.fraOutstandingColumn.BackColor = System.Drawing.SystemColors.Control
        Me.fraOutstandingColumn.Controls.Add(Me.optOutstandingAccount)
        Me.fraOutstandingColumn.Controls.Add(Me.optOutstandingBase)
        Me.fraOutstandingColumn.Controls.Add(Me.optOutstandingTransaction)
        Me.fraOutstandingColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOutstandingColumn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOutstandingColumn.Location = New System.Drawing.Point(450, 185)
        Me.fraOutstandingColumn.Name = "fraOutstandingColumn"
        Me.fraOutstandingColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOutstandingColumn.Size = New System.Drawing.Size(340, 39)
        Me.fraOutstandingColumn.TabIndex = 105
        Me.fraOutstandingColumn.TabStop = False
        Me.fraOutstandingColumn.Text = "Outstanding Column Currency"
        '
        'optOutstandingAccount
        '
        Me.optOutstandingAccount.BackColor = System.Drawing.SystemColors.Control
        Me.optOutstandingAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOutstandingAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOutstandingAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOutstandingAccount.Location = New System.Drawing.Point(160, 18)
        Me.optOutstandingAccount.Name = "optOutstandingAccount"
        Me.optOutstandingAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOutstandingAccount.Size = New System.Drawing.Size(172, 16)
        Me.optOutstandingAccount.TabIndex = 108
        Me.optOutstandingAccount.TabStop = True
        Me.optOutstandingAccount.Tag = "SEARCH;"
        Me.optOutstandingAccount.Text = "Account (x)"
        Me.optOutstandingAccount.UseVisualStyleBackColor = False
        '
        'optOutstandingBase
        '
        Me.optOutstandingBase.BackColor = System.Drawing.SystemColors.Control
        Me.optOutstandingBase.Checked = True
        Me.optOutstandingBase.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOutstandingBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOutstandingBase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOutstandingBase.Location = New System.Drawing.Point(5, 18)
        Me.optOutstandingBase.Name = "optOutstandingBase"
        Me.optOutstandingBase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOutstandingBase.Size = New System.Drawing.Size(56, 16)
        Me.optOutstandingBase.TabIndex = 107
        Me.optOutstandingBase.TabStop = True
        Me.optOutstandingBase.Tag = "SEARCH;"
        Me.optOutstandingBase.Text = "Base"
        Me.optOutstandingBase.UseVisualStyleBackColor = False
        '
        'optOutstandingTransaction
        '
        Me.optOutstandingTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.optOutstandingTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOutstandingTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOutstandingTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOutstandingTransaction.Location = New System.Drawing.Point(72, 18)
        Me.optOutstandingTransaction.Name = "optOutstandingTransaction"
        Me.optOutstandingTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOutstandingTransaction.Size = New System.Drawing.Size(87, 16)
        Me.optOutstandingTransaction.TabIndex = 106
        Me.optOutstandingTransaction.TabStop = True
        Me.optOutstandingTransaction.Tag = "SEARCH;"
        Me.optOutstandingTransaction.Text = "Transaction"
        Me.optOutstandingTransaction.UseVisualStyleBackColor = False
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.txtDueDateTo)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDueDateTo)
        Me._tabMain_TabPage1.Controls.Add(Me.txtDueDateFrom)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDueDateFrom)
        Me._tabMain_TabPage1.Controls.Add(Me.cmbAccountType)
        Me._tabMain_TabPage1.Controls.Add(Me.txtDocumentRef)
        Me._tabMain_TabPage1.Controls.Add(Me.cmbDocTypeGroup)
        Me._tabMain_TabPage1.Controls.Add(Me.cmbDocumentType)
        Me._tabMain_TabPage1.Controls.Add(Me.cmbPeriod)
        Me._tabMain_TabPage1.Controls.Add(Me.txtDateFrom)
        Me._tabMain_TabPage1.Controls.Add(Me.txtDateTo)
        Me._tabMain_TabPage1.Controls.Add(Me.lblAccountType)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDateTo)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDateFrom)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDocumentRef)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDocumentType)
        Me._tabMain_TabPage1.Controls.Add(Me.lblDocTypeGroup)
        Me._tabMain_TabPage1.Controls.Add(Me.lblPeriod)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(1225, 229)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "2 - Document"
        '
        'txtDueDateTo
        '
        Me.txtDueDateTo.AcceptsReturn = True
        Me.txtDueDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDueDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDueDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDueDateTo.Location = New System.Drawing.Point(320, 159)
        Me.txtDueDateTo.MaxLength = 0
        Me.txtDueDateTo.Name = "txtDueDateTo"
        Me.txtDueDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDueDateTo.Size = New System.Drawing.Size(113, 20)
        Me.txtDueDateTo.TabIndex = 92
        Me.txtDueDateTo.Tag = "SEARCH;"
        '
        'lblDueDateTo
        '
        Me.lblDueDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblDueDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDueDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDueDateTo.Location = New System.Drawing.Point(264, 160)
        Me.lblDueDateTo.Name = "lblDueDateTo"
        Me.lblDueDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDueDateTo.Size = New System.Drawing.Size(49, 19)
        Me.lblDueDateTo.TabIndex = 93
        Me.lblDueDateTo.Text = "To:"
        '
        'txtDueDateFrom
        '
        Me.txtDueDateFrom.AcceptsReturn = True
        Me.txtDueDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDueDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDueDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDueDateFrom.Location = New System.Drawing.Point(128, 159)
        Me.txtDueDateFrom.MaxLength = 0
        Me.txtDueDateFrom.Name = "txtDueDateFrom"
        Me.txtDueDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDueDateFrom.Size = New System.Drawing.Size(113, 20)
        Me.txtDueDateFrom.TabIndex = 91
        Me.txtDueDateFrom.Tag = "SEARCH;"
        '
        'lblDueDateFrom
        '
        Me.lblDueDateFrom.AutoSize = True
        Me.lblDueDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueDateFrom.Location = New System.Drawing.Point(8, 163)
        Me.lblDueDateFrom.Name = "lblDueDateFrom"
        Me.lblDueDateFrom.Size = New System.Drawing.Size(82, 13)
        Me.lblDueDateFrom.TabIndex = 90
        Me.lblDueDateFrom.Text = "Due Date From:"
        '
        'cmbAccountType
        '
        Me.cmbAccountType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAccountType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbAccountType.Location = New System.Drawing.Point(128, 132)
        Me.cmbAccountType.Name = "cmbAccountType"
        Me.cmbAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbAccountType.Size = New System.Drawing.Size(153, 21)
        Me.cmbAccountType.TabIndex = 88
        Me.cmbAccountType.Tag = "SEARCH;"
        '
        'txtDocumentRef
        '
        Me.txtDocumentRef.AcceptsReturn = True
        Me.txtDocumentRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocumentRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentRef.Location = New System.Drawing.Point(128, 12)
        Me.txtDocumentRef.MaxLength = 0
        Me.txtDocumentRef.Name = "txtDocumentRef"
        Me.txtDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentRef.Size = New System.Drawing.Size(153, 20)
        Me.txtDocumentRef.TabIndex = 8
        Me.txtDocumentRef.Tag = "SEARCH;"
        '
        'cmbDocTypeGroup
        '
        Me.cmbDocTypeGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDocTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDocTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDocTypeGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDocTypeGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDocTypeGroup.Location = New System.Drawing.Point(128, 36)
        Me.cmbDocTypeGroup.Name = "cmbDocTypeGroup"
        Me.cmbDocTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbDocTypeGroup.Size = New System.Drawing.Size(153, 21)
        Me.cmbDocTypeGroup.TabIndex = 22
        Me.cmbDocTypeGroup.Tag = "SEARCH;"
        '
        'cmbDocumentType
        '
        Me.cmbDocumentType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDocumentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDocumentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDocumentType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDocumentType.Location = New System.Drawing.Point(128, 60)
        Me.cmbDocumentType.Name = "cmbDocumentType"
        Me.cmbDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbDocumentType.Size = New System.Drawing.Size(153, 21)
        Me.cmbDocumentType.TabIndex = 34
        Me.cmbDocumentType.Tag = "SEARCH;"
        '
        'cmbPeriod
        '
        Me.cmbPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPeriod.Location = New System.Drawing.Point(128, 84)
        Me.cmbPeriod.Name = "cmbPeriod"
        Me.cmbPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPeriod.Size = New System.Drawing.Size(153, 21)
        Me.cmbPeriod.TabIndex = 47
        Me.cmbPeriod.Tag = "SEARCH;"
        '
        'txtDateFrom
        '
        Me.txtDateFrom.AcceptsReturn = True
        Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateFrom.Location = New System.Drawing.Point(128, 108)
        Me.txtDateFrom.MaxLength = 0
        Me.txtDateFrom.Name = "txtDateFrom"
        Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateFrom.Size = New System.Drawing.Size(113, 20)
        Me.txtDateFrom.TabIndex = 56
        Me.txtDateFrom.Tag = "SEARCH;"
        '
        'txtDateTo
        '
        Me.txtDateTo.AcceptsReturn = True
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.Location = New System.Drawing.Point(320, 108)
        Me.txtDateTo.MaxLength = 0
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.Size = New System.Drawing.Size(113, 20)
        Me.txtDateTo.TabIndex = 57
        Me.txtDateTo.Tag = "SEARCH;"
        '
        'lblAccountType
        '
        Me.lblAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountType.Location = New System.Drawing.Point(8, 134)
        Me.lblAccountType.Name = "lblAccountType"
        Me.lblAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountType.Size = New System.Drawing.Size(105, 19)
        Me.lblAccountType.TabIndex = 89
        Me.lblAccountType.Text = "Account Type:"
        '
        'lblDateTo
        '
        Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateTo.Location = New System.Drawing.Point(264, 110)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateTo.Size = New System.Drawing.Size(49, 19)
        Me.lblDateTo.TabIndex = 59
        Me.lblDateTo.Text = "To:"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateFrom.Location = New System.Drawing.Point(8, 110)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateFrom.Size = New System.Drawing.Size(81, 19)
        Me.lblDateFrom.TabIndex = 58
        Me.lblDateFrom.Text = "From:"
        '
        'lblDocumentRef
        '
        Me.lblDocumentRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentRef.Location = New System.Drawing.Point(8, 14)
        Me.lblDocumentRef.Name = "lblDocumentRef"
        Me.lblDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentRef.Size = New System.Drawing.Size(105, 19)
        Me.lblDocumentRef.TabIndex = 12
        Me.lblDocumentRef.Text = "Document Ref:"
        '
        'lblDocumentType
        '
        Me.lblDocumentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentType.Location = New System.Drawing.Point(8, 62)
        Me.lblDocumentType.Name = "lblDocumentType"
        Me.lblDocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentType.Size = New System.Drawing.Size(105, 19)
        Me.lblDocumentType.TabIndex = 37
        Me.lblDocumentType.Text = "Document Type:"
        '
        'lblDocTypeGroup
        '
        Me.lblDocTypeGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocTypeGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocTypeGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocTypeGroup.Location = New System.Drawing.Point(8, 38)
        Me.lblDocTypeGroup.Name = "lblDocTypeGroup"
        Me.lblDocTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocTypeGroup.Size = New System.Drawing.Size(105, 19)
        Me.lblDocTypeGroup.TabIndex = 25
        Me.lblDocTypeGroup.Text = "Doc. Type Group:"
        '
        'lblPeriod
        '
        Me.lblPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(8, 86)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPeriod.Size = New System.Drawing.Size(81, 19)
        Me.lblPeriod.TabIndex = 48
        Me.lblPeriod.Text = "Period:"
        '
        '_tabMain_TabPage2
        '
        Me._tabMain_TabPage2.Controls.Add(Me.udTolerance)
        Me._tabMain_TabPage2.Controls.Add(Me.panTranCurrOS)
        Me._tabMain_TabPage2.Controls.Add(Me.lblTranCurrOS)
        Me._tabMain_TabPage2.Controls.Add(Me.lblCurrencyAmount)
        Me._tabMain_TabPage2.Controls.Add(Me.lblTolerance)
        Me._tabMain_TabPage2.Controls.Add(Me.lblCurrency)
        Me._tabMain_TabPage2.Controls.Add(Me.txtTolerance)
        Me._tabMain_TabPage2.Controls.Add(Me.txtCurrencyAmount)
        Me._tabMain_TabPage2.Controls.Add(Me.cmbCurrency)
        Me._tabMain_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage2.Name = "_tabMain_TabPage2"
        Me._tabMain_TabPage2.Size = New System.Drawing.Size(1225, 229)
        Me._tabMain_TabPage2.TabIndex = 2
        Me._tabMain_TabPage2.Text = "3 - Amount"
        '
        'udTolerance
        '
        Me.udTolerance.Location = New System.Drawing.Point(160, 61)
        Me.udTolerance.Name = "udTolerance"
        Me.udTolerance.Size = New System.Drawing.Size(20, 21)
        Me.udTolerance.TabIndex = 37
        '
        'panTranCurrOS
        '
        Me.panTranCurrOS.BackColor = System.Drawing.Color.Silver
        Me.panTranCurrOS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panTranCurrOS.Cursor = System.Windows.Forms.Cursors.Default
        Me.panTranCurrOS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panTranCurrOS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panTranCurrOS.Location = New System.Drawing.Point(496, 12)
        Me.panTranCurrOS.Name = "panTranCurrOS"
        Me.panTranCurrOS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panTranCurrOS.Size = New System.Drawing.Size(145, 21)
        Me.panTranCurrOS.TabIndex = 9
        Me.panTranCurrOS.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTranCurrOS
        '
        Me.lblTranCurrOS.BackColor = System.Drawing.SystemColors.Control
        Me.lblTranCurrOS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTranCurrOS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTranCurrOS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTranCurrOS.Location = New System.Drawing.Point(360, 8)
        Me.lblTranCurrOS.Name = "lblTranCurrOS"
        Me.lblTranCurrOS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTranCurrOS.Size = New System.Drawing.Size(137, 27)
        Me.lblTranCurrOS.TabIndex = 4
        Me.lblTranCurrOS.Text = "Transaction Currency Outstanding Balance:"
        '
        'lblCurrencyAmount
        '
        Me.lblCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyAmount.Location = New System.Drawing.Point(8, 38)
        Me.lblCurrencyAmount.Name = "lblCurrencyAmount"
        Me.lblCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyAmount.Size = New System.Drawing.Size(100, 19)
        Me.lblCurrencyAmount.TabIndex = 26
        Me.lblCurrencyAmount.Text = "Amount:"
        '
        'lblTolerance
        '
        Me.lblTolerance.BackColor = System.Drawing.SystemColors.Control
        Me.lblTolerance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTolerance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTolerance.Location = New System.Drawing.Point(8, 62)
        Me.lblTolerance.Name = "lblTolerance"
        Me.lblTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTolerance.Size = New System.Drawing.Size(100, 19)
        Me.lblTolerance.TabIndex = 36
        Me.lblTolerance.Text = "Tolerance (%):"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 16)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(100, 19)
        Me.lblCurrency.TabIndex = 13
        Me.lblCurrency.Text = "Currency:"
        '
        'txtTolerance
        '
        Me.txtTolerance.AcceptsReturn = True
        Me.txtTolerance.BackColor = System.Drawing.SystemColors.Window
        Me.txtTolerance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTolerance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTolerance.Location = New System.Drawing.Point(128, 60)
        Me.txtTolerance.MaxLength = 0
        Me.txtTolerance.Name = "txtTolerance"
        Me.txtTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTolerance.Size = New System.Drawing.Size(28, 20)
        Me.txtTolerance.TabIndex = 35
        Me.txtTolerance.Tag = "SEARCH;"
        Me.txtTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCurrencyAmount
        '
        Me.txtCurrencyAmount.AcceptsReturn = True
        Me.txtCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyAmount.Location = New System.Drawing.Point(128, 36)
        Me.txtCurrencyAmount.MaxLength = 0
        Me.txtCurrencyAmount.Name = "txtCurrencyAmount"
        Me.txtCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyAmount.Size = New System.Drawing.Size(113, 20)
        Me.txtCurrencyAmount.TabIndex = 23
        Me.txtCurrencyAmount.Tag = "SEARCH;"
        '
        'cmbCurrency
        '
        Me.cmbCurrency.DefaultItemId = 0
        Me.cmbCurrency.FirstItem = "(all)"
        Me.cmbCurrency.ItemId = 0
        Me.cmbCurrency.ListIndex = -1
        Me.cmbCurrency.Location = New System.Drawing.Point(128, 12)
        Me.cmbCurrency.Name = "cmbCurrency"
        Me.cmbCurrency.PMLookupProductFamily = 1
        Me.cmbCurrency.SingleItemId = 0
        Me.cmbCurrency.Size = New System.Drawing.Size(201, 21)
        Me.cmbCurrency.SortColumnName = ""
        Me.cmbCurrency.Sorted = True
        Me.cmbCurrency.TabIndex = 7
        Me.cmbCurrency.TableName = "currency"
        Me.cmbCurrency.Tag = "SEARCH;"
        Me.cmbCurrency.ToolTipText = ""
        Me.cmbCurrency.WhereClause = ""
        '
        '_tabMain_TabPage3
        '
        Me._tabMain_TabPage3.Controls.Add(Me.cmdCaseNumber)
        Me._tabMain_TabPage3.Controls.Add(Me.txtCaseNumber)
        Me._tabMain_TabPage3.Controls.Add(Me.txtBGRef)
        Me._tabMain_TabPage3.Controls.Add(Me.txtAltRef)
        Me._tabMain_TabPage3.Controls.Add(Me.cmdInsuredAccountCode)
        Me._tabMain_TabPage3.Controls.Add(Me.txtInsuredAccountCode)
        Me._tabMain_TabPage3.Controls.Add(Me.txtPurchaseInvoiceNo)
        Me._tabMain_TabPage3.Controls.Add(Me.txtInsuranceRef)
        Me._tabMain_TabPage3.Controls.Add(Me.txtPurchaseOrderNo)
        Me._tabMain_TabPage3.Controls.Add(Me.txtSpare)
        Me._tabMain_TabPage3.Controls.Add(Me.Label2)
        Me._tabMain_TabPage3.Controls.Add(Me.Label1)
        Me._tabMain_TabPage3.Controls.Add(Me.panInsuredAccountName)
        Me._tabMain_TabPage3.Controls.Add(Me.lblInsuranceRef)
        Me._tabMain_TabPage3.Controls.Add(Me.lblPurchaseOrderNo)
        Me._tabMain_TabPage3.Controls.Add(Me.lblOperatorID)
        Me._tabMain_TabPage3.Controls.Add(Me.lblDepartment)
        Me._tabMain_TabPage3.Controls.Add(Me.lblPurchaseInvoiceNo)
        Me._tabMain_TabPage3.Controls.Add(Me.lblSpare)
        Me._tabMain_TabPage3.Controls.Add(Me.lblUnderwritingYearID)
        Me._tabMain_TabPage3.Controls.Add(Me.cboDepartment)
        Me._tabMain_TabPage3.Controls.Add(Me.cboUnderwritingYearID)
        Me._tabMain_TabPage3.Controls.Add(Me.cboPMUser)
        Me._tabMain_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage3.Name = "_tabMain_TabPage3"
        Me._tabMain_TabPage3.Size = New System.Drawing.Size(1225, 229)
        Me._tabMain_TabPage3.TabIndex = 3
        Me._tabMain_TabPage3.Text = "4 - Reference"
        '
        'cmdCaseNumber
        '
        Me.cmdCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCaseNumber.Location = New System.Drawing.Point(8, 124)
        Me.cmdCaseNumber.Name = "cmdCaseNumber"
        Me.cmdCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCaseNumber.Size = New System.Drawing.Size(126, 21)
        Me.cmdCaseNumber.TabIndex = 56
        Me.cmdCaseNumber.Text = "Case Number..."
        Me.cmdCaseNumber.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCaseNumber.UseVisualStyleBackColor = False
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(139, 124)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(172, 20)
        Me.txtCaseNumber.TabIndex = 57
        '
        'txtBGRef
        '
        Me.txtBGRef.AcceptsReturn = True
        Me.txtBGRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBGRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBGRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBGRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBGRef.Location = New System.Drawing.Point(490, 26)
        Me.txtBGRef.MaxLength = 0
        Me.txtBGRef.Name = "txtBGRef"
        Me.txtBGRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBGRef.Size = New System.Drawing.Size(153, 20)
        Me.txtBGRef.TabIndex = 109
        Me.txtBGRef.Tag = "SEARCH;"
        '
        'txtAltRef
        '
        Me.txtAltRef.AcceptsReturn = True
        Me.txtAltRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtAltRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAltRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAltRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAltRef.Location = New System.Drawing.Point(490, 68)
        Me.txtAltRef.MaxLength = 0
        Me.txtAltRef.Name = "txtAltRef"
        Me.txtAltRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAltRef.Size = New System.Drawing.Size(153, 20)
        Me.txtAltRef.TabIndex = 43
        Me.txtAltRef.Tag = "SEARCH;"
        '
        'cmdInsuredAccountCode
        '
        Me.cmdInsuredAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsuredAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsuredAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsuredAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsuredAccountCode.Location = New System.Drawing.Point(8, 98)
        Me.cmdInsuredAccountCode.Name = "cmdInsuredAccountCode"
        Me.cmdInsuredAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsuredAccountCode.Size = New System.Drawing.Size(126, 21)
        Me.cmdInsuredAccountCode.TabIndex = 53
        Me.cmdInsuredAccountCode.Text = "Insured Account..."
        Me.cmdInsuredAccountCode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsuredAccountCode.UseVisualStyleBackColor = False
        '
        'txtInsuredAccountCode
        '
        Me.txtInsuredAccountCode.AcceptsReturn = True
        Me.txtInsuredAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuredAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuredAccountCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsuredAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuredAccountCode.Location = New System.Drawing.Point(140, 98)
        Me.txtInsuredAccountCode.MaxLength = 0
        Me.txtInsuredAccountCode.Name = "txtInsuredAccountCode"
        Me.txtInsuredAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuredAccountCode.Size = New System.Drawing.Size(171, 20)
        Me.txtInsuredAccountCode.TabIndex = 54
        '
        'txtPurchaseInvoiceNo
        '
        Me.txtPurchaseInvoiceNo.AcceptsReturn = True
        Me.txtPurchaseInvoiceNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPurchaseInvoiceNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPurchaseInvoiceNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurchaseInvoiceNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPurchaseInvoiceNo.Location = New System.Drawing.Point(8, 68)
        Me.txtPurchaseInvoiceNo.MaxLength = 0
        Me.txtPurchaseInvoiceNo.Name = "txtPurchaseInvoiceNo"
        Me.txtPurchaseInvoiceNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPurchaseInvoiceNo.Size = New System.Drawing.Size(153, 20)
        Me.txtPurchaseInvoiceNo.TabIndex = 40
        Me.txtPurchaseInvoiceNo.Tag = "SEARCH;"
        '
        'txtInsuranceRef
        '
        Me.txtInsuranceRef.AcceptsReturn = True
        Me.txtInsuranceRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuranceRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuranceRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsuranceRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuranceRef.Location = New System.Drawing.Point(8, 26)
        Me.txtInsuranceRef.MaxLength = 0
        Me.txtInsuranceRef.Name = "txtInsuranceRef"
        Me.txtInsuranceRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuranceRef.Size = New System.Drawing.Size(155, 20)
        Me.txtInsuranceRef.TabIndex = 17
        Me.txtInsuranceRef.Tag = "SEARCH;"
        '
        'txtPurchaseOrderNo
        '
        Me.txtPurchaseOrderNo.AcceptsReturn = True
        Me.txtPurchaseOrderNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPurchaseOrderNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPurchaseOrderNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurchaseOrderNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPurchaseOrderNo.Location = New System.Drawing.Point(170, 68)
        Me.txtPurchaseOrderNo.MaxLength = 0
        Me.txtPurchaseOrderNo.Name = "txtPurchaseOrderNo"
        Me.txtPurchaseOrderNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPurchaseOrderNo.Size = New System.Drawing.Size(153, 20)
        Me.txtPurchaseOrderNo.TabIndex = 41
        Me.txtPurchaseOrderNo.Tag = "SEARCH;"
        '
        'txtSpare
        '
        Me.txtSpare.AcceptsReturn = True
        Me.txtSpare.BackColor = System.Drawing.SystemColors.Window
        Me.txtSpare.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSpare.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSpare.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSpare.Location = New System.Drawing.Point(330, 68)
        Me.txtSpare.MaxLength = 0
        Me.txtSpare.Name = "txtSpare"
        Me.txtSpare.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSpare.Size = New System.Drawing.Size(153, 20)
        Me.txtSpare.TabIndex = 42
        Me.txtSpare.Tag = "SEARCH;"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(490, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(141, 17)
        Me.Label2.TabIndex = 110
        Me.Label2.Text = "BG Ref:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(488, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(129, 17)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "Alternate Reference:"
        '
        'panInsuredAccountName
        '
        Me.panInsuredAccountName.BackColor = System.Drawing.Color.Silver
        Me.panInsuredAccountName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panInsuredAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.panInsuredAccountName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panInsuredAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panInsuredAccountName.Location = New System.Drawing.Point(316, 98)
        Me.panInsuredAccountName.Name = "panInsuredAccountName"
        Me.panInsuredAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panInsuredAccountName.Size = New System.Drawing.Size(327, 21)
        Me.panInsuredAccountName.TabIndex = 55
        '
        'lblInsuranceRef
        '
        Me.lblInsuranceRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuranceRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuranceRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsuranceRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuranceRef.Location = New System.Drawing.Point(8, 10)
        Me.lblInsuranceRef.Name = "lblInsuranceRef"
        Me.lblInsuranceRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuranceRef.Size = New System.Drawing.Size(113, 17)
        Me.lblInsuranceRef.TabIndex = 2
        Me.lblInsuranceRef.Text = "Insurance Ref:"
        '
        'lblPurchaseOrderNo
        '
        Me.lblPurchaseOrderNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurchaseOrderNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurchaseOrderNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseOrderNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseOrderNo.Location = New System.Drawing.Point(170, 52)
        Me.lblPurchaseOrderNo.Name = "lblPurchaseOrderNo"
        Me.lblPurchaseOrderNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchaseOrderNo.Size = New System.Drawing.Size(113, 17)
        Me.lblPurchaseOrderNo.TabIndex = 29
        Me.lblPurchaseOrderNo.Text = "Purchase Order No:"
        '
        'lblOperatorID
        '
        Me.lblOperatorID.BackColor = System.Drawing.SystemColors.Control
        Me.lblOperatorID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOperatorID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperatorID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOperatorID.Location = New System.Drawing.Point(170, 10)
        Me.lblOperatorID.Name = "lblOperatorID"
        Me.lblOperatorID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOperatorID.Size = New System.Drawing.Size(113, 13)
        Me.lblOperatorID.TabIndex = 3
        Me.lblOperatorID.Text = "Operator Name:"
        '
        'lblDepartment
        '
        Me.lblDepartment.BackColor = System.Drawing.SystemColors.Control
        Me.lblDepartment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepartment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepartment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepartment.Location = New System.Drawing.Point(330, 10)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepartment.Size = New System.Drawing.Size(113, 13)
        Me.lblDepartment.TabIndex = 5
        Me.lblDepartment.Text = "Department:"
        '
        'lblPurchaseInvoiceNo
        '
        Me.lblPurchaseInvoiceNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurchaseInvoiceNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurchaseInvoiceNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseInvoiceNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseInvoiceNo.Location = New System.Drawing.Point(8, 52)
        Me.lblPurchaseInvoiceNo.Name = "lblPurchaseInvoiceNo"
        Me.lblPurchaseInvoiceNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchaseInvoiceNo.Size = New System.Drawing.Size(113, 17)
        Me.lblPurchaseInvoiceNo.TabIndex = 28
        Me.lblPurchaseInvoiceNo.Text = "Purchase Invoice No:"
        '
        'lblSpare
        '
        Me.lblSpare.BackColor = System.Drawing.SystemColors.Control
        Me.lblSpare.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSpare.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpare.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSpare.Location = New System.Drawing.Point(330, 52)
        Me.lblSpare.Name = "lblSpare"
        Me.lblSpare.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSpare.Size = New System.Drawing.Size(113, 17)
        Me.lblSpare.TabIndex = 30
        Me.lblSpare.Text = "Media Ref:"
        '
        'lblUnderwritingYearID
        '
        Me.lblUnderwritingYearID.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnderwritingYearID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnderwritingYearID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnderwritingYearID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnderwritingYearID.Location = New System.Drawing.Point(650, 10)
        Me.lblUnderwritingYearID.Name = "lblUnderwritingYearID"
        Me.lblUnderwritingYearID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnderwritingYearID.Size = New System.Drawing.Size(141, 13)
        Me.lblUnderwritingYearID.TabIndex = 6
        Me.lblUnderwritingYearID.Text = "Underwriting Year:"
        Me.lblUnderwritingYearID.Visible = False
        '
        'cboDepartment
        '
        Me.cboDepartment.DefaultItemId = 0
        Me.cboDepartment.FirstItem = ""
        Me.cboDepartment.ItemId = 0
        Me.cboDepartment.ListIndex = -1
        Me.cboDepartment.Location = New System.Drawing.Point(330, 26)
        Me.cboDepartment.Name = "cboDepartment"
        Me.cboDepartment.PMLookupProductFamily = 1
        Me.cboDepartment.SingleItemId = 0
        Me.cboDepartment.Size = New System.Drawing.Size(153, 21)
        Me.cboDepartment.SortColumnName = ""
        Me.cboDepartment.Sorted = True
        Me.cboDepartment.TabIndex = 19
        Me.cboDepartment.TableName = "Department"
        Me.cboDepartment.Tag = "SEARCH;"
        Me.cboDepartment.ToolTipText = ""
        Me.cboDepartment.WhereClause = ""
        '
        'cboUnderwritingYearID
        '
        Me.cboUnderwritingYearID.DefaultItemId = 0
        Me.cboUnderwritingYearID.FirstItem = "(all)"
        Me.cboUnderwritingYearID.ItemId = 0
        Me.cboUnderwritingYearID.ListIndex = -1
        Me.cboUnderwritingYearID.Location = New System.Drawing.Point(650, 26)
        Me.cboUnderwritingYearID.Name = "cboUnderwritingYearID"
        Me.cboUnderwritingYearID.PMLookupProductFamily = 1
        Me.cboUnderwritingYearID.SingleItemId = 0
        Me.cboUnderwritingYearID.Size = New System.Drawing.Size(153, 21)
        Me.cboUnderwritingYearID.SortColumnName = ""
        Me.cboUnderwritingYearID.Sorted = True
        Me.cboUnderwritingYearID.TabIndex = 20
        Me.cboUnderwritingYearID.TableName = "Underwriting_Year"
        Me.cboUnderwritingYearID.Tag = "SEARCH;"
        Me.cboUnderwritingYearID.ToolTipText = ""
        Me.cboUnderwritingYearID.Visible = False
        Me.cboUnderwritingYearID.WhereClause = ""
        '
        'cboPMUser
        '
        Me.cboPMUser.DefaultUserID = 0
        Me.cboPMUser.FirstItem = "(all)"
        Me.cboPMUser.ListIndex = -1
        Me.cboPMUser.Location = New System.Drawing.Point(171, 26)
        Me.cboPMUser.Name = "cboPMUser"
        Me.cboPMUser.PartyCnt = 0
        Me.cboPMUser.PMUserGroupID = 0
        Me.cboPMUser.SingleUserID = 0
        Me.cboPMUser.Size = New System.Drawing.Size(153, 21)
        Me.cboPMUser.Sorted = True
        Me.cboPMUser.TabIndex = 18
        Me.cboPMUser.Tag = "SEARCH;"
        Me.cboPMUser.ToolTipText = ""
        Me.cboPMUser.UserID = 0
        '
        'cmdAllocate
        '
        Me.cmdAllocate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAllocate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAllocate.Enabled = False
        Me.cmdAllocate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllocate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAllocate.Location = New System.Drawing.Point(383, 422)
        Me.cmdAllocate.Name = "cmdAllocate"
        Me.cmdAllocate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAllocate.Size = New System.Drawing.Size(62, 23)
        Me.cmdAllocate.TabIndex = 70
        Me.cmdAllocate.TabStop = False
        Me.cmdAllocate.Text = "&Allocate"
        Me.cmdAllocate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAllocate.UseVisualStyleBackColor = False
        '
        'cmdFindDocTrans
        '
        Me.cmdFindDocTrans.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindDocTrans.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindDocTrans.Enabled = False
        Me.cmdFindDocTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindDocTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindDocTrans.Location = New System.Drawing.Point(88, 422)
        Me.cmdFindDocTrans.Name = "cmdFindDocTrans"
        Me.cmdFindDocTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindDocTrans.Size = New System.Drawing.Size(73, 23)
        Me.cmdFindDocTrans.TabIndex = 68
        Me.cmdFindDocTrans.TabStop = False
        Me.cmdFindDocTrans.Text = "&Document"
        Me.cmdFindDocTrans.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindDocTrans.UseVisualStyleBackColor = False
        '
        'cmdFindAccTrans
        '
        Me.cmdFindAccTrans.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindAccTrans.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindAccTrans.Enabled = False
        Me.cmdFindAccTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindAccTrans.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindAccTrans.Location = New System.Drawing.Point(8, 422)
        Me.cmdFindAccTrans.Name = "cmdFindAccTrans"
        Me.cmdFindAccTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindAccTrans.Size = New System.Drawing.Size(73, 23)
        Me.cmdFindAccTrans.TabIndex = 67
        Me.cmdFindAccTrans.TabStop = False
        Me.cmdFindAccTrans.Text = "&Account"
        Me.cmdFindAccTrans.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindAccTrans.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(880, 80)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(79, 23)
        Me.cmdFindNow.TabIndex = 62
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(692, 401)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(65, 23)
        Me.cmdHelp.TabIndex = 75
        Me.cmdHelp.TabStop = False
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
        Me.cmdCancel.Location = New System.Drawing.Point(584, 422)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(65, 23)
        Me.cmdCancel.TabIndex = 74
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(660, 422)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(60, 23)
        Me.cmdOK.TabIndex = 73
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1, Me._stbStatus_Panel2, Me._stbStatus_Panel3, Me._stbStatus_Panel4, Me._stbStatus_Panel5, Me._stbStatus_Panel6})
        Me.stbStatus.Location = New System.Drawing.Point(0, 465)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(1241, 22)
        Me.stbStatus.TabIndex = 76
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(214, 22)
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_stbStatus_Panel2
        '
        Me._stbStatus_Panel2.AutoSize = False
        Me._stbStatus_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel2.DoubleClickEnabled = True
        Me._stbStatus_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel2.Name = "_stbStatus_Panel2"
        Me._stbStatus_Panel2.Size = New System.Drawing.Size(234, 22)
        Me._stbStatus_Panel2.Text = "Account Balance"
        Me._stbStatus_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_stbStatus_Panel3
        '
        Me._stbStatus_Panel3.AutoSize = False
        Me._stbStatus_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom),System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel3.DoubleClickEnabled = True
        Me._stbStatus_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel3.Name = "_stbStatus_Panel3"
        Me._stbStatus_Panel3.Size = New System.Drawing.Size(214, 22)
        Me._stbStatus_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_stbStatus_Panel4
        '
        Me._stbStatus_Panel4.AutoSize = False
        Me._stbStatus_Panel4.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel4.DoubleClickEnabled = True
        Me._stbStatus_Panel4.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel4.Name = "_stbStatus_Panel4"
        Me._stbStatus_Panel4.Size = New System.Drawing.Size(234, 22)
        Me._stbStatus_Panel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_stbStatus_Panel5
        '
        Me._stbStatus_Panel5.AutoSize = False
        Me._stbStatus_Panel5.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel5.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel5.DoubleClickEnabled = True
        Me._stbStatus_Panel5.Name = "_stbStatus_Panel5"
        Me._stbStatus_Panel5.Size = New System.Drawing.Size(214, 17)
        Me._stbStatus_Panel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_stbStatus_Panel6
        '
        Me._stbStatus_Panel6.AutoSize = False
        Me._stbStatus_Panel6.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel6.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel6.DoubleClickEnabled = True
        Me._stbStatus_Panel6.Name = "_stbStatus_Panel6"
        Me._stbStatus_Panel6.Size = New System.Drawing.Size(214, 17)
        Me._stbStatus_Panel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchResults
        '
        Me.lvwSearchResults.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchResults_ColumnHeader_0, Me._lvwSearchResults_ColumnHeader_1, Me._lvwSearchResults_ColumnHeader_2, Me._lvwSearchResults_ColumnHeader_3, Me._lvwSearchResults_ColumnHeader_4, Me._lvwSearchResults_ColumnHeader_5, Me._lvwSearchResults_ColumnHeader_6, Me._lvwSearchResults_ColumnHeader_7, Me._lvwSearchResults_ColumnHeader_8, Me._lvwSearchResults_ColumnHeader_36, Me._lvwSearchResults_ColumnHeader_9, Me._lvwSearchResults_ColumnHeader_10, Me._lvwSearchResults_ColumnHeader_11, Me._lvwSearchResults_ColumnHeader_12, Me._lvwSearchResults_ColumnHeader_13, Me._lvwSearchResults_ColumnHeader_14, Me._lvwSearchResults_ColumnHeader_15, Me._lvwSearchResults_ColumnHeader_16, Me._lvwSearchResults_ColumnHeader_17, Me._lvwSearchResults_ColumnHeader_18, Me._lvwSearchResults_ColumnHeader_19, Me._lvwSearchResults_ColumnHeader_20, Me._lvwSearchResults_ColumnHeader_21, Me._lvwSearchResults_ColumnHeader_22, Me._lvwSearchResults_ColumnHeader_23, Me._lvwSearchResults_ColumnHeader_24, Me._lvwSearchResults_ColumnHeader_25, Me._lvwSearchResults_ColumnHeader_26, Me._lvwSearchResults_ColumnHeader_27, Me._lvwSearchResults_ColumnHeader_28, Me._lvwSearchResults_ColumnHeader_29, Me._lvwSearchResults_ColumnHeader_30, Me._lvwSearchResults_ColumnHeader_31, Me._lvwSearchResults_ColumnHeader_32, Me._lvwSearchResults_ColumnHeader_33, Me._lvwSearchResults_ColumnHeader_34, Me._lvwSearchResults_ColumnHeader_35, Me._lvwSearchResults_ColumnHeader_37})
        Me.lvwSearchResults.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchResults.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchResults.FullRowSelect = True
        Me.lvwSearchResults.HideSelection = False
        Me.lvwSearchResults.LargeImageList = Me.imglImages
        Me.lvwSearchResults.Location = New System.Drawing.Point(9, 289)
        Me.lvwSearchResults.Name = "lvwSearchResults"
        Me.lvwSearchResults.Size = New System.Drawing.Size(710, 106)
        Me.lvwSearchResults.SmallImageList = Me.imglImages
        Me.lvwSearchResults.TabIndex = 65
        Me.lvwSearchResults.TabStop = False
        Me.lvwSearchResults.UseCompatibleStateImageBehavior = False
        Me.lvwSearchResults.View = System.Windows.Forms.View.Details

        '
        '_lvwSearchResults_ColumnHeader_0
        '
        Me._lvwSearchResults_ColumnHeader_0.Text = "0"
        Me._lvwSearchResults_ColumnHeader_0.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_1
        '
        Me._lvwSearchResults_ColumnHeader_1.Text = "1"
        Me._lvwSearchResults_ColumnHeader_1.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_2
        '
        Me._lvwSearchResults_ColumnHeader_2.Text = "2"
        Me._lvwSearchResults_ColumnHeader_2.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_3
        '
        Me._lvwSearchResults_ColumnHeader_3.Text = "3"
        Me._lvwSearchResults_ColumnHeader_3.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_4
        '
        Me._lvwSearchResults_ColumnHeader_4.Text = "4"
        Me._lvwSearchResults_ColumnHeader_4.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_5
        '
        Me._lvwSearchResults_ColumnHeader_5.Text = "5"
        Me._lvwSearchResults_ColumnHeader_5.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_6
        '
        Me._lvwSearchResults_ColumnHeader_6.Text = "6"
        Me._lvwSearchResults_ColumnHeader_6.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_7
        '
        Me._lvwSearchResults_ColumnHeader_7.Text = "7"
        Me._lvwSearchResults_ColumnHeader_7.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_8
        '
        Me._lvwSearchResults_ColumnHeader_8.Text = "8"
        Me._lvwSearchResults_ColumnHeader_8.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_9
        '
        Me._lvwSearchResults_ColumnHeader_9.Text = "9"
        Me._lvwSearchResults_ColumnHeader_9.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_10
        '
        Me._lvwSearchResults_ColumnHeader_10.Text = "10"
        Me._lvwSearchResults_ColumnHeader_10.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_11
        '
        Me._lvwSearchResults_ColumnHeader_11.Text = "11"
        Me._lvwSearchResults_ColumnHeader_11.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_12
        '
        Me._lvwSearchResults_ColumnHeader_12.Text = "12"
        Me._lvwSearchResults_ColumnHeader_12.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_13
        '
        Me._lvwSearchResults_ColumnHeader_13.Text = "13"
        Me._lvwSearchResults_ColumnHeader_13.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_14
        '
        Me._lvwSearchResults_ColumnHeader_14.Text = "14"
        Me._lvwSearchResults_ColumnHeader_14.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_15
        '
        Me._lvwSearchResults_ColumnHeader_15.Text = "15"
        Me._lvwSearchResults_ColumnHeader_15.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_16
        '
        Me._lvwSearchResults_ColumnHeader_16.Text = "16"
        Me._lvwSearchResults_ColumnHeader_16.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_17
        '
        Me._lvwSearchResults_ColumnHeader_17.Text = "17"
        Me._lvwSearchResults_ColumnHeader_17.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_18
        '
        Me._lvwSearchResults_ColumnHeader_18.Text = "18"
        Me._lvwSearchResults_ColumnHeader_18.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_19
        '
        Me._lvwSearchResults_ColumnHeader_19.Text = "19"
        Me._lvwSearchResults_ColumnHeader_19.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_20
        '
        Me._lvwSearchResults_ColumnHeader_20.Text = "20"
        Me._lvwSearchResults_ColumnHeader_20.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_21
        '
        Me._lvwSearchResults_ColumnHeader_21.Text = "21"
        Me._lvwSearchResults_ColumnHeader_21.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_22
        '
        Me._lvwSearchResults_ColumnHeader_22.Text = "22"
        Me._lvwSearchResults_ColumnHeader_22.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_23
        '
        Me._lvwSearchResults_ColumnHeader_23.Text = "23"
        Me._lvwSearchResults_ColumnHeader_23.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_24
        '
        Me._lvwSearchResults_ColumnHeader_24.Text = "24"
        Me._lvwSearchResults_ColumnHeader_24.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_25
        '
        Me._lvwSearchResults_ColumnHeader_25.Text = "25"
        Me._lvwSearchResults_ColumnHeader_25.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_26
        '
        Me._lvwSearchResults_ColumnHeader_26.Text = "26"
        Me._lvwSearchResults_ColumnHeader_26.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_27
        '
        Me._lvwSearchResults_ColumnHeader_27.Text = "27"
        Me._lvwSearchResults_ColumnHeader_27.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_28
        '
        Me._lvwSearchResults_ColumnHeader_28.Text = "28"
        Me._lvwSearchResults_ColumnHeader_28.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_29
        '
        Me._lvwSearchResults_ColumnHeader_29.Text = "29"
        Me._lvwSearchResults_ColumnHeader_29.Width = 0
        '
        '_lvwSearchResults_ColumnHeader_30
        '
        Me._lvwSearchResults_ColumnHeader_30.Text = "29"
        Me._lvwSearchResults_ColumnHeader_30.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_31
        '
        Me._lvwSearchResults_ColumnHeader_31.Text = "31"
        Me._lvwSearchResults_ColumnHeader_31.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_32
        '
        Me._lvwSearchResults_ColumnHeader_32.Text = "32"
        Me._lvwSearchResults_ColumnHeader_32.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_33
        '
        Me._lvwSearchResults_ColumnHeader_33.Text = "33"
        Me._lvwSearchResults_ColumnHeader_33.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_34
        '
        Me._lvwSearchResults_ColumnHeader_34.Text = "34"
        Me._lvwSearchResults_ColumnHeader_34.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_35
        '
        Me._lvwSearchResults_ColumnHeader_35.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_37
        '
        Me._lvwSearchResults_ColumnHeader_37.Text = "37"
        Me._lvwSearchResults_ColumnHeader_37.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "SecondaryImage")
        Me.imglImages.Images.SetKeyName(1, "PrimaryImage")
        Me.imglImages.Images.SetKeyName(2, "NoteImage")
        Me.imglImages.Images.SetKeyName(3, "Paper1")
        Me.imglImages.Images.SetKeyName(4, "Paper2")
        Me.imglImages.Images.SetKeyName(5, "PaperClip")
        Me.imglImages.Images.SetKeyName(6, "NotReported")
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
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(858, 168)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImgImage.TabIndex = 79
        Me.ImgImage.TabStop = False
        '
        'Ctx_mnuListView
        '
        Me.Ctx_mnuListView.Name = "Ctx_mnuListView"
        Me.Ctx_mnuListView.Size = New System.Drawing.Size(61, 4)
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFindAccount, Me.mnuFindDocument})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFindAccount
        '
        Me.mnuFindAccount.Name = "mnuFindAccount"
        Me.mnuFindAccount.Size = New System.Drawing.Size(224, 22)
        Me.mnuFindAccount.Text = "Find &Account Transactions"
        '
        'mnuFindDocument
        '
        Me.mnuFindDocument.Name = "mnuFindDocument"
        Me.mnuFindDocument.Size = New System.Drawing.Size(224, 22)
        Me.mnuFindDocument.Text = "Find &Document Transactions"
        '
        'mnuListView
        '
        Me.mnuListView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuInstalmentPlan, Me.mnuSep2, Me.mnuTransactionWriteOff, Me.mnuSep3, Me.mnuTransfer, Me.mnuSep4, Me.mnuRefund, Me.mnuSep5, Me.mnuApprove, Me.mnuSep6, Me.mnuReject, Me.mnuSep7, Me.mnuAddComment, Me.mnuEditComment, Me.mnuSep8, Me.mnuReportOnThis, Me.mnuDoNotReport, Me.mnuSep9, Me.mnuBreakdown})
        Me.mnuListView.Name = "mnuListView"
        Me.mnuListView.Size = New System.Drawing.Size(62, 20)
        Me.mnuListView.Text = "&ListView"
        '
        'mnuInstalmentPlan
        '
        Me.mnuInstalmentPlan.Name = "mnuInstalmentPlan"
        Me.mnuInstalmentPlan.Size = New System.Drawing.Size(156, 22)
        Me.mnuInstalmentPlan.Text = "&Instalment Plan"
        '
        'mnuSep2
        '
        Me.mnuSep2.Name = "mnuSep2"
        Me.mnuSep2.Size = New System.Drawing.Size(153, 6)
        '
        'mnuTransactionWriteOff
        '
        Me.mnuTransactionWriteOff.Name = "mnuTransactionWriteOff"
        Me.mnuTransactionWriteOff.Size = New System.Drawing.Size(156, 22)
        Me.mnuTransactionWriteOff.Text = "&Write-off"
        '
        'mnuSep3
        '
        Me.mnuSep3.Name = "mnuSep3"
        Me.mnuSep3.Size = New System.Drawing.Size(153, 6)
        '
        'mnuTransfer
        '
        Me.mnuTransfer.Name = "mnuTransfer"
        Me.mnuTransfer.Size = New System.Drawing.Size(156, 22)
        Me.mnuTransfer.Text = "&Transfer"
        '
        'mnuSep4
        '
        Me.mnuSep4.Name = "mnuSep4"
        Me.mnuSep4.Size = New System.Drawing.Size(153, 6)
        '
        'mnuRefund
        '
        Me.mnuRefund.Name = "mnuRefund"
        Me.mnuRefund.Size = New System.Drawing.Size(156, 22)
        Me.mnuRefund.Text = "&Refund"
        '
        'mnuSep5
        '
        Me.mnuSep5.Name = "mnuSep5"
        Me.mnuSep5.Size = New System.Drawing.Size(153, 6)
        '
        'mnuApprove
        '
        Me.mnuApprove.Name = "mnuApprove"
        Me.mnuApprove.Size = New System.Drawing.Size(156, 22)
        Me.mnuApprove.Text = "&Approve"
        '
        'mnuSep6
        '
        Me.mnuSep6.Name = "mnuSep6"
        Me.mnuSep6.Size = New System.Drawing.Size(153, 6)
        '
        'mnuReject
        '
        Me.mnuReject.Name = "mnuReject"
        Me.mnuReject.Size = New System.Drawing.Size(156, 22)
        Me.mnuReject.Text = "R&eject"
        '
        'mnuSep7
        '
        Me.mnuSep7.Name = "mnuSep7"
        Me.mnuSep7.Size = New System.Drawing.Size(153, 6)
        '
        'mnuAddComment
        '
        Me.mnuAddComment.Name = "mnuAddComment"
        Me.mnuAddComment.Size = New System.Drawing.Size(156, 22)
        Me.mnuAddComment.Text = "& Add Comment"
        '
        'mnuEditComment
        '
        Me.mnuEditComment.Name = "mnuEditComment"
        Me.mnuEditComment.Size = New System.Drawing.Size(156, 22)
        Me.mnuEditComment.Text = "& Edit Comment"
        '
        'mnuSep8
        '
        Me.mnuSep8.Name = "mnuSep8"
        Me.mnuSep8.Size = New System.Drawing.Size(153, 6)
        '
        'mnuReportOnThis
        '
        Me.mnuReportOnThis.Name = "mnuReportOnThis"
        Me.mnuReportOnThis.Size = New System.Drawing.Size(156, 22)
        Me.mnuReportOnThis.Text = "&Report"
        '
        'mnuDoNotReport
        '
        Me.mnuDoNotReport.Name = "mnuDoNotReport"
        Me.mnuDoNotReport.Size = New System.Drawing.Size(156, 22)
        Me.mnuDoNotReport.Text = "Do &Not Report"
        '
        'mnuSep9
        '
        Me.mnuSep9.Name = "mnuSep9"
        Me.mnuSep9.Size = New System.Drawing.Size(153, 6)
        '
        'mnuBreakdown
        '
        Me.mnuBreakdown.Name = "mnuBreakdown"
        Me.mnuBreakdown.Size = New System.Drawing.Size(156, 22)
        Me.mnuBreakdown.Text = "&Breakdown"
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuListView})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(971, 24)
        Me.MainMenu1.TabIndex = 80
        Me.MainMenu1.Visible = False
        '
        'cmdSplitReceipt
        '
        Me.cmdSplitReceipt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSplitReceipt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSplitReceipt.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSplitReceipt.Enabled = False
        Me.cmdSplitReceipt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSplitReceipt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSplitReceipt.Location = New System.Drawing.Point(688, 439)
        Me.cmdSplitReceipt.Name = "cmdSplitReceipt"
        Me.cmdSplitReceipt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSplitReceipt.Size = New System.Drawing.Size(78, 23)
        Me.cmdSplitReceipt.TabIndex = 83
        Me.cmdSplitReceipt.TabStop = False
        Me.cmdSplitReceipt.Text = "&Split Receipt"
        Me.cmdSplitReceipt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSplitReceipt.UseVisualStyleBackColor = False
        Me.cmdSplitReceipt.Visible = False
        '
        'cmdEditSplit
        '
        Me.cmdEditSplit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEditSplit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditSplit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditSplit.Enabled = False
        Me.cmdEditSplit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditSplit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditSplit.Location = New System.Drawing.Point(600, 439)
        Me.cmdEditSplit.Name = "cmdEditSplit"
        Me.cmdEditSplit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditSplit.Size = New System.Drawing.Size(62, 23)
        Me.cmdEditSplit.TabIndex = 84
        Me.cmdEditSplit.TabStop = False
        Me.cmdEditSplit.Text = "&Edit Split"
        Me.cmdEditSplit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditSplit.UseVisualStyleBackColor = False
        Me.cmdEditSplit.Visible = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1241, 487)
        Me.Controls.Add(Me.cmdEditSplit)
        Me.Controls.Add(Me.cmdSplitReceipt)
        Me.Controls.Add(Me.cmdReverseAndReplace)
        Me.Controls.Add(Me.RowPicture)
        Me.Controls.Add(Me.tbrMain)
        Me.Controls.Add(Me.cmdExpand)
        Me.Controls.Add(Me.cmdViewAllocation)
        Me.Controls.Add(Me.cmdReverse)
        Me.Controls.Add(Me.cmdBalance)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdAllocate)
        Me.Controls.Add(Me.cmdFindDocTrans)
        Me.Controls.Add(Me.cmdFindAccTrans)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.ImgImage)
        Me.Controls.Add(Me.lvwSearchResults)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 9)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find: Transaction"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.RowPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbrMain.ResumeLayout(False)
        Me.tbrMain.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        Me.fraBalanceColumn.ResumeLayout(False)
        Me.fraBalanceColumn.PerformLayout()
        Me.fraAmountColumn.ResumeLayout(False)
        Me.fraOutstandingColumn.ResumeLayout(False)
        Me._tabMain_TabPage1.ResumeLayout(False)
        Me._tabMain_TabPage1.PerformLayout()
        Me._tabMain_TabPage2.ResumeLayout(False)
        Me._tabMain_TabPage2.PerformLayout()
        CType(Me.udTolerance, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMain_TabPage3.ResumeLayout(False)
        Me._tabMain_TabPage3.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

End Sub
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFindAccount As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFindDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInstalmentPlan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuTransactionWriteOff As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuTransfer As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep4 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuRefund As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep5 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuApprove As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep6 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuReject As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep7 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuAddComment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEditComment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep8 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuReportOnThis As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDoNotReport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep9 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuBreakdown As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Friend WithEvents udTolerance As System.Windows.Forms.NumericUpDown
    Public WithEvents cboPMUser As PMUserLookupControl.cboPMUserLookup
    Public WithEvents cmdSplitReceipt As System.Windows.Forms.Button
    Public WithEvents cmdEditSplit As System.Windows.Forms.Button
    Public WithEvents txtDueDateFrom As System.Windows.Forms.TextBox
    Friend WithEvents lblDueDateFrom As System.Windows.Forms.Label
    Public WithEvents txtDueDateTo As System.Windows.Forms.TextBox
    Public WithEvents lblDueDateTo As System.Windows.Forms.Label
    Private WithEvents _lvwSearchResults_ColumnHeader_36 As System.Windows.Forms.ColumnHeader
    Public WithEvents cmdCaseNumber As System.Windows.Forms.Button
    Public WithEvents txtCaseNumber As System.Windows.Forms.TextBox
    Private WithEvents _lvwSearchResults_ColumnHeader_37 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _stbStatus_Panel5 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents _stbStatus_Panel6 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents panAgentCode As Label
    Public WithEvents panAgentName As Label
    Public WithEvents lblAgentName As Label
    Public WithEvents lblAgentCode As Label
#End Region
End Class
