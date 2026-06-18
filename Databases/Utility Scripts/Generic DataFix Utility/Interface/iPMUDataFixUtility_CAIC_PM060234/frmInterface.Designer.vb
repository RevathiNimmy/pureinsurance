<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptSinglePolicy()
		InitializeoptReservePayment()
		InitializeoptMiscellaneous()
        InitializechkReserve()
		tabPolicyVersionPreviousTab = tabPolicyVersion.SelectedIndex
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
	Private WithEvents _optSinglePolicy_0 As System.Windows.Forms.RadioButton
    Private WithEvents _optSinglePolicy_2 As System.Windows.Forms.RadioButton
    Public WithEvents lblInsuranceFileCnt As System.Windows.Forms.Label
	Private WithEvents _lvwRisk_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRisk_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRisk_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRisk_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRisk_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRisk As System.Windows.Forms.ListView
	Public WithEvents fraRisk As System.Windows.Forms.GroupBox
	Public WithEvents cmdRiskRefresh As System.Windows.Forms.Button
	Public WithEvents txtInsuranceFileCnt As System.Windows.Forms.TextBox
	Private WithEvents _tabPolicyVersion_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents lblTransactionExportPolicyID As System.Windows.Forms.Label
	Public WithEvents txtTransactionExportPolicyID As System.Windows.Forms.TextBox
	Public WithEvents cmdTransactionExportRefresh As System.Windows.Forms.Button
	Private WithEvents _lvwTransactionExport_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactionExport_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactionExport_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactionExport_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactionExport_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactionExport_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTransactionExport As System.Windows.Forms.ListView
	Public WithEvents fraTransactionExport As System.Windows.Forms.GroupBox
	Private WithEvents _tabPolicyVersion_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabPolicyVersion As System.Windows.Forms.TabControl
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents chkDeleteStats As System.Windows.Forms.CheckBox
	Private WithEvents _lvwClaim_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClaim As System.Windows.Forms.ListView
	Public WithEvents fraFailedClaimTransaction As System.Windows.Forms.GroupBox
	Private WithEvents _tabClaim_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwImbalancedClosedClaim_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwImbalancedClosedClaim As System.Windows.Forms.ListView
	Public WithEvents fraImbalancedClosedClaim As System.Windows.Forms.GroupBox
	Public WithEvents chkAutoProcess As System.Windows.Forms.CheckBox
	Private WithEvents _tabClaim_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents txtCPClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents cmdCPRefresh As System.Windows.Forms.Button
	Private WithEvents _lvwClaimPosting_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaimPosting_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClaimPosting As System.Windows.Forms.ListView
	Public WithEvents fraClaimPosting As System.Windows.Forms.GroupBox
	Private WithEvents _optReservePayment_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optReservePayment_1 As System.Windows.Forms.RadioButton
	Private WithEvents _chkReserve_0 As System.Windows.Forms.CheckBox
	Private WithEvents _chkReserve_1 As System.Windows.Forms.CheckBox
	Private WithEvents _tabClaim_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Private WithEvents _lvwReserve_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReserve_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwReserve As System.Windows.Forms.ListView
	Public WithEvents fraReserve As System.Windows.Forms.GroupBox
	Private WithEvents _lvwPayment_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPayment_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPayment_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPayment_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPayment_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPayment_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPayment As System.Windows.Forms.ListView
	Public WithEvents fraPayment As System.Windows.Forms.GroupBox
	Public WithEvents cmdReservePaymentRefresh As System.Windows.Forms.Button
	Private WithEvents _tabClaim_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents lvwClaimMisc As System.Windows.Forms.ListView
	Public WithEvents fraClaimMisc As System.Windows.Forms.GroupBox
	Private WithEvents _tabClaim_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents tabClaim As System.Windows.Forms.TabControl
	Private WithEvents _tabMain_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _optMiscellaneous_4 As System.Windows.Forms.RadioButton
	Private WithEvents _optMiscellaneous_3 As System.Windows.Forms.RadioButton
	Private WithEvents _optMiscellaneous_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optMiscellaneous_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optMiscellaneous_2 As System.Windows.Forms.RadioButton
	Private WithEvents _tabMain_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
    Public WithEvents MESSAGE As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents COUNT As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbMain As System.Windows.Forms.StatusStrip
	Public chkReserve(1) As System.Windows.Forms.CheckBox
	Public mnuPopUpItem(0) As System.Windows.Forms.ToolStripMenuItem
    Public optMiscellaneous(5) As System.Windows.Forms.RadioButton
	Public optReservePayment(1) As System.Windows.Forms.RadioButton
    Public optSinglePolicy(4) As System.Windows.Forms.RadioButton
	Public WithEvents Ctx_mnuPopUp As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabPolicyVersionPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSinglePolicy_0 = New System.Windows.Forms.RadioButton()
        Me._optSinglePolicy_2 = New System.Windows.Forms.RadioButton()
        Me.cmdRiskRefresh = New System.Windows.Forms.Button()
        Me.cmdTransactionExportRefresh = New System.Windows.Forms.Button()
        Me.cmdCPRefresh = New System.Windows.Forms.Button()
        Me.cmdReservePaymentRefresh = New System.Windows.Forms.Button()
        Me._optMiscellaneous_4 = New System.Windows.Forms.RadioButton()
        Me._optMiscellaneous_3 = New System.Windows.Forms.RadioButton()
        Me._optMiscellaneous_0 = New System.Windows.Forms.RadioButton()
        Me._optMiscellaneous_1 = New System.Windows.Forms.RadioButton()
        Me._optMiscellaneous_2 = New System.Windows.Forms.RadioButton()
        Me._optSinglePolicy_3 = New System.Windows.Forms.RadioButton()
        Me._optSinglePolicy_1 = New System.Windows.Forms.RadioButton()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.CmdSelectAllPolicy = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabPolicyVersion = New System.Windows.Forms.TabControl()
        Me._tabPolicyVersion_TabPage0 = New System.Windows.Forms.TabPage()
        Me.ChkOutStanding = New System.Windows.Forms.CheckBox()
        Me.ChkComm = New System.Windows.Forms.CheckBox()
        Me.chkRIRefresh = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSqlQuery = New System.Windows.Forms.TextBox()
        Me.lvwPolicyVersion = New System.Windows.Forms.ListView()
        Me._lvwPolicyVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._optSinglePolicy_4 = New System.Windows.Forms.RadioButton()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.cmdGetPolicyVersion = New System.Windows.Forms.Button()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me._tabPolicyVersion_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblInsuranceFileCnt = New System.Windows.Forms.Label()
        Me.fraRisk = New System.Windows.Forms.GroupBox()
        Me.lvwRisk = New System.Windows.Forms.ListView()
        Me._lvwRisk_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.txtInsuranceFileCnt = New System.Windows.Forms.TextBox()
        Me._tabPolicyVersion_TabPage2 = New System.Windows.Forms.TabPage()
        Me.lblTransactionExportPolicyID = New System.Windows.Forms.Label()
        Me.txtTransactionExportPolicyID = New System.Windows.Forms.TextBox()
        Me.fraTransactionExport = New System.Windows.Forms.GroupBox()
        Me.lvwTransactionExport = New System.Windows.Forms.ListView()
        Me._lvwTransactionExport_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtClaimNo = New System.Windows.Forms.TextBox()
        Me.OptReverseReg = New System.Windows.Forms.RadioButton()
        Me.OptReverse = New System.Windows.Forms.RadioButton()
        Me.CmdClaiimFix = New System.Windows.Forms.Button()
        Me.CmdGetClaimVersions = New System.Windows.Forms.Button()
        Me.LvwClaimVersion = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtSqlClaim = New System.Windows.Forms.TextBox()
        Me._tabMain_TabPage2 = New System.Windows.Forms.TabPage()
        Me.tabClaim = New System.Windows.Forms.TabControl()
        Me._tabClaim_TabPage0 = New System.Windows.Forms.TabPage()
        Me.chkDeleteStats = New System.Windows.Forms.CheckBox()
        Me.fraFailedClaimTransaction = New System.Windows.Forms.GroupBox()
        Me.lvwClaim = New System.Windows.Forms.ListView()
        Me._lvwClaim_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._tabClaim_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraImbalancedClosedClaim = New System.Windows.Forms.GroupBox()
        Me.lvwImbalancedClosedClaim = New System.Windows.Forms.ListView()
        Me._lvwImbalancedClosedClaim_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chkAutoProcess = New System.Windows.Forms.CheckBox()
        Me._tabClaim_TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCPClaimNumber = New System.Windows.Forms.TextBox()
        Me.fraClaimPosting = New System.Windows.Forms.GroupBox()
        Me.lvwClaimPosting = New System.Windows.Forms.ListView()
        Me._lvwClaimPosting_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._optReservePayment_0 = New System.Windows.Forms.RadioButton()
        Me._optReservePayment_1 = New System.Windows.Forms.RadioButton()
        Me._chkReserve_0 = New System.Windows.Forms.CheckBox()
        Me._chkReserve_1 = New System.Windows.Forms.CheckBox()
        Me._tabClaim_TabPage3 = New System.Windows.Forms.TabPage()
        Me.lblClaimNumber = New System.Windows.Forms.Label()
        Me.txtClaimNumber = New System.Windows.Forms.TextBox()
        Me.fraReserve = New System.Windows.Forms.GroupBox()
        Me.lvwReserve = New System.Windows.Forms.ListView()
        Me._lvwReserve_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.fraPayment = New System.Windows.Forms.GroupBox()
        Me.lvwPayment = New System.Windows.Forms.ListView()
        Me._lvwPayment_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._tabClaim_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fraClaimMisc = New System.Windows.Forms.GroupBox()
        Me.lvwClaimMisc = New System.Windows.Forms.ListView()
        Me._tabMain_TabPage3 = New System.Windows.Forms.TabPage()
        Me._optMiscellaneous_5 = New System.Windows.Forms.RadioButton()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.chkReverseAllocation = New System.Windows.Forms.CheckBox()
        Me.lvwSRPDcouments = New System.Windows.Forms.ListView()
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.btnGetSRPDetails = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSRPSQL = New System.Windows.Forms.TextBox()
        Me.btnReverseTrans = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDocumentRef = New System.Windows.Forms.TextBox()
        Me.stbMain = New System.Windows.Forms.StatusStrip()
        Me.MESSAGE = New System.Windows.Forms.ToolStripStatusLabel()
        Me.COUNT = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Ctx_mnuPopUp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lblPolicyStatus = New System.Windows.Forms.Label()
        Me.cboPolicyStatus = New System.Windows.Forms.ComboBox()
        Me.uctAnchor = New uSIRCommonControls.uctAnchor()
        Me.txtPMNumber = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tabMain.SuspendLayout
        Me._tabMain_TabPage1.SuspendLayout
        Me.tabPolicyVersion.SuspendLayout
        Me._tabPolicyVersion_TabPage0.SuspendLayout
        Me._tabPolicyVersion_TabPage1.SuspendLayout
        Me.fraRisk.SuspendLayout
        Me._tabPolicyVersion_TabPage2.SuspendLayout
        Me.fraTransactionExport.SuspendLayout
        Me.TabPage2.SuspendLayout
        Me._tabMain_TabPage2.SuspendLayout
        Me.tabClaim.SuspendLayout
        Me._tabClaim_TabPage0.SuspendLayout
        Me.fraFailedClaimTransaction.SuspendLayout
        Me._tabClaim_TabPage1.SuspendLayout
        Me.fraImbalancedClosedClaim.SuspendLayout
        Me._tabClaim_TabPage2.SuspendLayout
        Me.fraClaimPosting.SuspendLayout
        Me._tabClaim_TabPage3.SuspendLayout
        Me.fraReserve.SuspendLayout
        Me.fraPayment.SuspendLayout
        Me._tabClaim_TabPage4.SuspendLayout
        Me.fraClaimMisc.SuspendLayout
        Me._tabMain_TabPage3.SuspendLayout
        Me.TabPage1.SuspendLayout
        Me.stbMain.SuspendLayout
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        '_optSinglePolicy_0
        '
        Me._optSinglePolicy_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_0.Checked = true
        Me._optSinglePolicy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optSinglePolicy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_0.Location = New System.Drawing.Point(637, 95)
        Me._optSinglePolicy_0.Name = "_optSinglePolicy_0"
        Me._optSinglePolicy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_0.Size = New System.Drawing.Size(169, 22)
        Me._optSinglePolicy_0.TabIndex = 21
        Me._optSinglePolicy_0.TabStop = true
        Me._optSinglePolicy_0.Text = "Post Failed Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_0, "Recreate stats folder, details and export folders and repost with option to delet"& _ 
        "e existing document and recalculate reinsurance")
        Me._optSinglePolicy_0.UseVisualStyleBackColor = false
        '
        '_optSinglePolicy_2
        '
        Me._optSinglePolicy_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optSinglePolicy_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_2.Location = New System.Drawing.Point(836, 159)
        Me._optSinglePolicy_2.Name = "_optSinglePolicy_2"
        Me._optSinglePolicy_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_2.Size = New System.Drawing.Size(185, 17)
        Me._optSinglePolicy_2.TabIndex = 40
        Me._optSinglePolicy_2.TabStop = true
        Me._optSinglePolicy_2.Text = "Set Policy Status"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_2, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_2.UseVisualStyleBackColor = false
        Me._optSinglePolicy_2.Visible = false
        '
        'cmdRiskRefresh
        '
        Me.cmdRiskRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRiskRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRiskRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdRiskRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRiskRefresh.Location = New System.Drawing.Point(171, 18)
        Me.cmdRiskRefresh.Name = "cmdRiskRefresh"
        Me.cmdRiskRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRiskRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdRiskRefresh.TabIndex = 28
        Me.cmdRiskRefresh.Text = "<>"
        Me.cmdRiskRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdRiskRefresh, "Refresh risk details")
        Me.cmdRiskRefresh.UseVisualStyleBackColor = false
        '
        'cmdTransactionExportRefresh
        '
        Me.cmdTransactionExportRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransactionExportRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransactionExportRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdTransactionExportRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransactionExportRefresh.Location = New System.Drawing.Point(171, 18)
        Me.cmdTransactionExportRefresh.Name = "cmdTransactionExportRefresh"
        Me.cmdTransactionExportRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransactionExportRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdTransactionExportRefresh.TabIndex = 33
        Me.cmdTransactionExportRefresh.Text = "<>"
        Me.cmdTransactionExportRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdTransactionExportRefresh, "Refresh risk details")
        Me.cmdTransactionExportRefresh.UseVisualStyleBackColor = false
        '
        'cmdCPRefresh
        '
        Me.cmdCPRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCPRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCPRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCPRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCPRefresh.Location = New System.Drawing.Point(342, 27)
        Me.cmdCPRefresh.Name = "cmdCPRefresh"
        Me.cmdCPRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCPRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdCPRefresh.TabIndex = 51
        Me.cmdCPRefresh.Text = "<>"
        Me.cmdCPRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCPRefresh, "Refresh reserve and payment details")
        Me.cmdCPRefresh.UseVisualStyleBackColor = false
        '
        'cmdReservePaymentRefresh
        '
        Me.cmdReservePaymentRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReservePaymentRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReservePaymentRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdReservePaymentRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReservePaymentRefresh.Location = New System.Drawing.Point(342, 30)
        Me.cmdReservePaymentRefresh.Name = "cmdReservePaymentRefresh"
        Me.cmdReservePaymentRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReservePaymentRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdReservePaymentRefresh.TabIndex = 48
        Me.cmdReservePaymentRefresh.Text = "<>"
        Me.cmdReservePaymentRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdReservePaymentRefresh, "Refresh reserve and payment details")
        Me.cmdReservePaymentRefresh.UseVisualStyleBackColor = false
        '
        '_optMiscellaneous_4
        '
        Me._optMiscellaneous_4.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optMiscellaneous_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_4.Location = New System.Drawing.Point(24, 97)
        Me._optMiscellaneous_4.Name = "_optMiscellaneous_4"
        Me._optMiscellaneous_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_4.Size = New System.Drawing.Size(367, 19)
        Me._optMiscellaneous_4.TabIndex = 41
        Me._optMiscellaneous_4.TabStop = true
        Me._optMiscellaneous_4.Text = "Delete claim and all associated postings"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_4, "Delete claim and all associated postings including stats")
        Me._optMiscellaneous_4.UseVisualStyleBackColor = false
        '
        '_optMiscellaneous_3
        '
        Me._optMiscellaneous_3.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optMiscellaneous_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_3.Location = New System.Drawing.Point(24, 79)
        Me._optMiscellaneous_3.Name = "_optMiscellaneous_3"
        Me._optMiscellaneous_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_3.Size = New System.Drawing.Size(367, 19)
        Me._optMiscellaneous_3.TabIndex = 37
        Me._optMiscellaneous_3.TabStop = true
        Me._optMiscellaneous_3.Text = "Add 100% Retained RI model to policy with no reinsurance"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_3, "Add reinsurance to supplied policy or policies with no reinsurance. Note only 100"& _ 
        "% retained model will work")
        Me._optMiscellaneous_3.UseVisualStyleBackColor = false
        '
        '_optMiscellaneous_0
        '
        Me._optMiscellaneous_0.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optMiscellaneous_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_0.Location = New System.Drawing.Point(24, 25)
        Me._optMiscellaneous_0.Name = "_optMiscellaneous_0"
        Me._optMiscellaneous_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_0.Size = New System.Drawing.Size(220, 19)
        Me._optMiscellaneous_0.TabIndex = 11
        Me._optMiscellaneous_0.TabStop = true
        Me._optMiscellaneous_0.Text = "Delete Document From Account"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_0, "Delete document and its allocation details")
        Me._optMiscellaneous_0.UseVisualStyleBackColor = false
        '
        '_optMiscellaneous_1
        '
        Me._optMiscellaneous_1.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optMiscellaneous_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_1.Location = New System.Drawing.Point(24, 43)
        Me._optMiscellaneous_1.Name = "_optMiscellaneous_1"
        Me._optMiscellaneous_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_1.Size = New System.Drawing.Size(220, 19)
        Me._optMiscellaneous_1.TabIndex = 10
        Me._optMiscellaneous_1.TabStop = true
        Me._optMiscellaneous_1.Text = "Delete Document's Allocation"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_1, "Delete all allocation details for this document")
        Me._optMiscellaneous_1.UseVisualStyleBackColor = false
        '
        '_optMiscellaneous_2
        '
        Me._optMiscellaneous_2.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optMiscellaneous_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_2.Location = New System.Drawing.Point(24, 61)
        Me._optMiscellaneous_2.Name = "_optMiscellaneous_2"
        Me._optMiscellaneous_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_2.Size = New System.Drawing.Size(307, 19)
        Me._optMiscellaneous_2.TabIndex = 9
        Me._optMiscellaneous_2.TabStop = true
        Me._optMiscellaneous_2.Text = "Repost This Revision/Payment For Closed Claims"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_2, resources.GetString("_optMiscellaneous_2.ToolTip"))
        Me._optMiscellaneous_2.UseVisualStyleBackColor = false
        '
        '_optSinglePolicy_3
        '
        Me._optSinglePolicy_3.AutoSize = true
        Me._optSinglePolicy_3.Location = New System.Drawing.Point(514, 37)
        Me._optSinglePolicy_3.Name = "_optSinglePolicy_3"
        Me._optSinglePolicy_3.Size = New System.Drawing.Size(172, 17)
        Me._optSinglePolicy_3.TabIndex = 41
        Me._optSinglePolicy_3.TabStop = true
        Me._optSinglePolicy_3.Text = "Reverse Stats and Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_3, "Reverse the document ref.")
        Me._optSinglePolicy_3.UseVisualStyleBackColor = true
        Me._optSinglePolicy_3.Visible = false
        '
        '_optSinglePolicy_1
        '
        Me._optSinglePolicy_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optSinglePolicy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_1.Location = New System.Drawing.Point(514, 16)
        Me._optSinglePolicy_1.Name = "_optSinglePolicy_1"
        Me._optSinglePolicy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_1.Size = New System.Drawing.Size(116, 17)
        Me._optSinglePolicy_1.TabIndex = 55
        Me._optSinglePolicy_1.TabStop = true
        Me._optSinglePolicy_1.Text = "Delete Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_1, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_1.UseVisualStyleBackColor = false
        Me._optSinglePolicy_1.Visible = false
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage2)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage3)
        Me.tabMain.Controls.Add(Me.TabPage1)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(195, 18)
        Me.tabMain.Location = New System.Drawing.Point(0, 48)
        Me.tabMain.Multiline = true
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(807, 544)
        Me.tabMain.TabIndex = 4
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.CmdSelectAllPolicy)
        Me._tabMain_TabPage1.Controls.Add(Me.cmdExit)
        Me._tabMain_TabPage1.Controls.Add(Me.cmdOK)
        Me._tabMain_TabPage1.Controls.Add(Me.tabPolicyVersion)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(799, 518)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "Policy"
        '
        'CmdSelectAllPolicy
        '
        Me.CmdSelectAllPolicy.Location = New System.Drawing.Point(13, 474)
        Me.CmdSelectAllPolicy.Name = "CmdSelectAllPolicy"
        Me.CmdSelectAllPolicy.Size = New System.Drawing.Size(86, 24)
        Me.CmdSelectAllPolicy.TabIndex = 48
        Me.CmdSelectAllPolicy.Text = "Select All"
        Me.CmdSelectAllPolicy.UseVisualStyleBackColor = true
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(694, 474)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(70, 22)
        Me.cmdExit.TabIndex = 21
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = false
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(620, 474)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(70, 22)
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = false
        '
        'tabPolicyVersion
        '
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage0)
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage1)
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage2)
        Me.tabPolicyVersion.Controls.Add(Me.TabPage2)
        Me.tabPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tabPolicyVersion.ItemSize = New System.Drawing.Size(259, 18)
        Me.tabPolicyVersion.Location = New System.Drawing.Point(3, 0)
        Me.tabPolicyVersion.Multiline = true
        Me.tabPolicyVersion.Name = "tabPolicyVersion"
        Me.tabPolicyVersion.SelectedIndex = 0
        Me.tabPolicyVersion.Size = New System.Drawing.Size(796, 468)
        Me.tabPolicyVersion.TabIndex = 19
        '
        '_tabPolicyVersion_TabPage0
        '
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_1)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.ChkOutStanding)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.ChkComm)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.chkRIRefresh)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.Label2)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.txtSqlQuery)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lvwPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_4)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_3)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lblPolicyNumber)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_0)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdGetPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.txtPolicyNumber)
        Me._tabPolicyVersion_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage0.Name = "_tabPolicyVersion_TabPage0"
        Me._tabPolicyVersion_TabPage0.Size = New System.Drawing.Size(788, 442)
        Me._tabPolicyVersion_TabPage0.TabIndex = 0
        Me._tabPolicyVersion_TabPage0.Text = "Policy Version"
        '
        'ChkOutStanding
        '
        Me.ChkOutStanding.AutoSize = true
        Me.ChkOutStanding.Location = New System.Drawing.Point(523, 104)
        Me.ChkOutStanding.Name = "ChkOutStanding"
        Me.ChkOutStanding.Size = New System.Drawing.Size(269, 17)
        Me.ChkOutStanding.TabIndex = 48
        Me.ChkOutStanding.Text = "Only show transactiom with an outstanding Balance"
        Me.ChkOutStanding.UseVisualStyleBackColor = true
        Me.ChkOutStanding.Visible = false
        '
        'ChkComm
        '
        Me.ChkComm.AutoSize = true
        Me.ChkComm.Location = New System.Drawing.Point(665, 123)
        Me.ChkComm.Name = "ChkComm"
        Me.ChkComm.Size = New System.Drawing.Size(141, 17)
        Me.ChkComm.TabIndex = 47
        Me.ChkComm.Text = "Recalculate Commission"
        Me.ChkComm.UseVisualStyleBackColor = true
        Me.ChkComm.Visible = false
        '
        'chkRIRefresh
        '
        Me.chkRIRefresh.AutoSize = true
        Me.chkRIRefresh.Checked = true
        Me.chkRIRefresh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRIRefresh.Location = New System.Drawing.Point(514, 81)
        Me.chkRIRefresh.Name = "chkRIRefresh"
        Me.chkRIRefresh.Size = New System.Drawing.Size(77, 17)
        Me.chkRIRefresh.TabIndex = 46
        Me.chkRIRefresh.Text = "RI Refresh"
        Me.chkRIRefresh.UseVisualStyleBackColor = true
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(3, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(372, 13)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "Copy Sql Here ( In SQL type column in same sequence as show in below List)"
        '
        'txtSqlQuery
        '
        Me.txtSqlQuery.Location = New System.Drawing.Point(10, 58)
        Me.txtSqlQuery.Multiline = true
        Me.txtSqlQuery.Name = "txtSqlQuery"
        Me.txtSqlQuery.Size = New System.Drawing.Size(489, 88)
        Me.txtSqlQuery.TabIndex = 44
        '
        'lvwPolicyVersion
        '
        Me.lvwPolicyVersion.AllowColumnReorder = true
        Me.lvwPolicyVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicyVersion.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyVersion_ColumnHeader_1, Me._lvwPolicyVersion_ColumnHeader_2, Me._lvwPolicyVersion_ColumnHeader_7, Me._lvwPolicyVersion_ColumnHeader_3, Me._lvwPolicyVersion_ColumnHeader_4, Me._lvwPolicyVersion_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicyVersion, false)
        Me.lvwPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwPolicyVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicyVersion.FullRowSelect = true
        Me.lvwPolicyVersion.GridLines = true
        Me.lvwPolicyVersion.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Location = New System.Drawing.Point(3, 154)
        Me.lvwPolicyVersion.Name = "lvwPolicyVersion"
        Me.lvwPolicyVersion.Size = New System.Drawing.Size(763, 292)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicyVersion, false)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicyVersion, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicyVersion, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicyVersion.TabIndex = 43
        Me.lvwPolicyVersion.UseCompatibleStateImageBehavior = false
        Me.lvwPolicyVersion.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicyVersion_ColumnHeader_1
        '
        Me._lvwPolicyVersion_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwPolicyVersion_ColumnHeader_1.Text = "Policy ID"
        Me._lvwPolicyVersion_ColumnHeader_1.Width = 90
        '
        '_lvwPolicyVersion_ColumnHeader_2
        '
        Me._lvwPolicyVersion_ColumnHeader_2.Text = "Policy Ref"
        Me._lvwPolicyVersion_ColumnHeader_2.Width = 130
        '
        '_lvwPolicyVersion_ColumnHeader_7
        '
        Me._lvwPolicyVersion_ColumnHeader_7.Text = "Insurance File Type Id"
        Me._lvwPolicyVersion_ColumnHeader_7.Width = 120
        '
        '_lvwPolicyVersion_ColumnHeader_3
        '
        Me._lvwPolicyVersion_ColumnHeader_3.Text = "Insurance File Type"
        Me._lvwPolicyVersion_ColumnHeader_3.Width = 121
        '
        '_lvwPolicyVersion_ColumnHeader_4
        '
        Me._lvwPolicyVersion_ColumnHeader_4.Text = "Document Ref"
        Me._lvwPolicyVersion_ColumnHeader_4.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_6
        '
        Me._lvwPolicyVersion_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_6.Text = "Document Date"
        Me._lvwPolicyVersion_ColumnHeader_6.Width = 97
        '
        '_optSinglePolicy_4
        '
        Me._optSinglePolicy_4.AutoSize = true
        Me._optSinglePolicy_4.Location = New System.Drawing.Point(514, 58)
        Me._optSinglePolicy_4.Name = "_optSinglePolicy_4"
        Me._optSinglePolicy_4.Size = New System.Drawing.Size(204, 17)
        Me._optSinglePolicy_4.TabIndex = 42
        Me._optSinglePolicy_4.TabStop = true
        Me._optSinglePolicy_4.Text = "Reverse and Regenerate Transaction"
        Me._optSinglePolicy_4.UseVisualStyleBackColor = true
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(3, 16)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(84, 19)
        Me.lblPolicyNumber.TabIndex = 26
        Me.lblPolicyNumber.Text = "Policy Number :"
        Me.lblPolicyNumber.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPolicyNumber.Visible = false
        '
        'cmdGetPolicyVersion
        '
        Me.cmdGetPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetPolicyVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdGetPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetPolicyVersion.Location = New System.Drawing.Point(514, 115)
        Me.cmdGetPolicyVersion.Name = "cmdGetPolicyVersion"
        Me.cmdGetPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetPolicyVersion.Size = New System.Drawing.Size(118, 25)
        Me.cmdGetPolicyVersion.TabIndex = 22
        Me.cmdGetPolicyVersion.Text = "Get Policy Versions"
        Me.cmdGetPolicyVersion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetPolicyVersion.UseVisualStyleBackColor = false
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = true
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(93, 13)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(406, 20)
        Me.txtPolicyNumber.TabIndex = 25
        Me.txtPolicyNumber.Visible = false
        '
        '_tabPolicyVersion_TabPage1
        '
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.lblInsuranceFileCnt)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.fraRisk)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdRiskRefresh)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.txtInsuranceFileCnt)
        Me._tabPolicyVersion_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage1.Name = "_tabPolicyVersion_TabPage1"
        Me._tabPolicyVersion_TabPage1.Size = New System.Drawing.Size(788, 442)
        Me._tabPolicyVersion_TabPage1.TabIndex = 1
        Me._tabPolicyVersion_TabPage1.Text = "Risk"
        '
        'lblInsuranceFileCnt
        '
        Me.lblInsuranceFileCnt.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuranceFileCnt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuranceFileCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblInsuranceFileCnt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuranceFileCnt.Location = New System.Drawing.Point(12, 21)
        Me.lblInsuranceFileCnt.Name = "lblInsuranceFileCnt"
        Me.lblInsuranceFileCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuranceFileCnt.Size = New System.Drawing.Size(49, 13)
        Me.lblInsuranceFileCnt.TabIndex = 30
        Me.lblInsuranceFileCnt.Text = "Policy ID"
        '
        'fraRisk
        '
        Me.fraRisk.BackColor = System.Drawing.SystemColors.Control
        Me.fraRisk.Controls.Add(Me.lvwRisk)
        Me.fraRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRisk.Location = New System.Drawing.Point(6, 46)
        Me.fraRisk.Name = "fraRisk"
        Me.fraRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRisk.Size = New System.Drawing.Size(769, 437)
        Me.fraRisk.TabIndex = 27
        Me.fraRisk.TabStop = false
        Me.fraRisk.Text = "Risk Details"
        '
        'lvwRisk
        '
        Me.lvwRisk.AllowColumnReorder = true
        Me.lvwRisk.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRisk, "")
        Me.lvwRisk.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRisk_ColumnHeader_1, Me._lvwRisk_ColumnHeader_2, Me._lvwRisk_ColumnHeader_3, Me._lvwRisk_ColumnHeader_4, Me._lvwRisk_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRisk, false)
        Me.lvwRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwRisk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRisk.FullRowSelect = true
        Me.lvwRisk.GridLines = true
        Me.lvwRisk.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRisk, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRisk, "")
        Me.lvwRisk.Location = New System.Drawing.Point(3, 15)
        Me.lvwRisk.Name = "lvwRisk"
        Me.lvwRisk.Size = New System.Drawing.Size(760, 416)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRisk, "")
        Me.listViewHelper1.SetSorted(Me.lvwRisk, false)
        Me.listViewHelper1.SetSortKey(Me.lvwRisk, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRisk, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRisk.TabIndex = 31
        Me.lvwRisk.UseCompatibleStateImageBehavior = false
        Me.lvwRisk.View = System.Windows.Forms.View.Details
        '
        '_lvwRisk_ColumnHeader_1
        '
        Me._lvwRisk_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwRisk_ColumnHeader_1.Text = "InsuranceFileCnt"
        Me._lvwRisk_ColumnHeader_1.Width = 97
        '
        '_lvwRisk_ColumnHeader_2
        '
        Me._lvwRisk_ColumnHeader_2.Tag = "VALUESORT"
        Me._lvwRisk_ColumnHeader_2.Text = "RiskCnt"
        Me._lvwRisk_ColumnHeader_2.Width = 97
        '
        '_lvwRisk_ColumnHeader_3
        '
        Me._lvwRisk_ColumnHeader_3.Text = "Status Flag"
        Me._lvwRisk_ColumnHeader_3.Width = 97
        '
        '_lvwRisk_ColumnHeader_4
        '
        Me._lvwRisk_ColumnHeader_4.Text = "Desc"
        Me._lvwRisk_ColumnHeader_4.Width = 346
        '
        '_lvwRisk_ColumnHeader_5
        '
        Me._lvwRisk_ColumnHeader_5.Text = "Risk Status"
        Me._lvwRisk_ColumnHeader_5.Width = 121
        '
        'txtInsuranceFileCnt
        '
        Me.txtInsuranceFileCnt.AcceptsReturn = true
        Me.txtInsuranceFileCnt.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuranceFileCnt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuranceFileCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtInsuranceFileCnt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuranceFileCnt.Location = New System.Drawing.Point(69, 18)
        Me.txtInsuranceFileCnt.MaxLength = 0
        Me.txtInsuranceFileCnt.Name = "txtInsuranceFileCnt"
        Me.txtInsuranceFileCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuranceFileCnt.Size = New System.Drawing.Size(100, 20)
        Me.txtInsuranceFileCnt.TabIndex = 29
        '
        '_tabPolicyVersion_TabPage2
        '
        Me._tabPolicyVersion_TabPage2.Controls.Add(Me.lblTransactionExportPolicyID)
        Me._tabPolicyVersion_TabPage2.Controls.Add(Me.txtTransactionExportPolicyID)
        Me._tabPolicyVersion_TabPage2.Controls.Add(Me.cmdTransactionExportRefresh)
        Me._tabPolicyVersion_TabPage2.Controls.Add(Me.fraTransactionExport)
        Me._tabPolicyVersion_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage2.Name = "_tabPolicyVersion_TabPage2"
        Me._tabPolicyVersion_TabPage2.Size = New System.Drawing.Size(788, 442)
        Me._tabPolicyVersion_TabPage2.TabIndex = 2
        Me._tabPolicyVersion_TabPage2.Text = "Transaction"
        '
        'lblTransactionExportPolicyID
        '
        Me.lblTransactionExportPolicyID.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionExportPolicyID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionExportPolicyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTransactionExportPolicyID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionExportPolicyID.Location = New System.Drawing.Point(12, 21)
        Me.lblTransactionExportPolicyID.Name = "lblTransactionExportPolicyID"
        Me.lblTransactionExportPolicyID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionExportPolicyID.Size = New System.Drawing.Size(55, 13)
        Me.lblTransactionExportPolicyID.TabIndex = 36
        Me.lblTransactionExportPolicyID.Text = "Policy ID"
        '
        'txtTransactionExportPolicyID
        '
        Me.txtTransactionExportPolicyID.AcceptsReturn = true
        Me.txtTransactionExportPolicyID.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransactionExportPolicyID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransactionExportPolicyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtTransactionExportPolicyID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTransactionExportPolicyID.Location = New System.Drawing.Point(69, 18)
        Me.txtTransactionExportPolicyID.MaxLength = 0
        Me.txtTransactionExportPolicyID.Name = "txtTransactionExportPolicyID"
        Me.txtTransactionExportPolicyID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTransactionExportPolicyID.Size = New System.Drawing.Size(100, 20)
        Me.txtTransactionExportPolicyID.TabIndex = 32
        '
        'fraTransactionExport
        '
        Me.fraTransactionExport.BackColor = System.Drawing.SystemColors.Control
        Me.fraTransactionExport.Controls.Add(Me.lvwTransactionExport)
        Me.fraTransactionExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraTransactionExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTransactionExport.Location = New System.Drawing.Point(6, 46)
        Me.fraTransactionExport.Name = "fraTransactionExport"
        Me.fraTransactionExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTransactionExport.Size = New System.Drawing.Size(769, 426)
        Me.fraTransactionExport.TabIndex = 34
        Me.fraTransactionExport.TabStop = false
        Me.fraTransactionExport.Text = "Risk Details"
        '
        'lvwTransactionExport
        '
        Me.lvwTransactionExport.AllowColumnReorder = true
        Me.lvwTransactionExport.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTransactionExport, "")
        Me.lvwTransactionExport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTransactionExport_ColumnHeader_1, Me._lvwTransactionExport_ColumnHeader_2, Me._lvwTransactionExport_ColumnHeader_3, Me._lvwTransactionExport_ColumnHeader_4, Me._lvwTransactionExport_ColumnHeader_5, Me._lvwTransactionExport_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTransactionExport, false)
        Me.lvwTransactionExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwTransactionExport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTransactionExport.FullRowSelect = true
        Me.lvwTransactionExport.GridLines = true
        Me.lvwTransactionExport.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTransactionExport, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTransactionExport, "")
        Me.lvwTransactionExport.Location = New System.Drawing.Point(3, 15)
        Me.lvwTransactionExport.Name = "lvwTransactionExport"
        Me.lvwTransactionExport.Size = New System.Drawing.Size(760, 405)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTransactionExport, "")
        Me.listViewHelper1.SetSorted(Me.lvwTransactionExport, false)
        Me.listViewHelper1.SetSortKey(Me.lvwTransactionExport, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTransactionExport, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTransactionExport.TabIndex = 35
        Me.lvwTransactionExport.UseCompatibleStateImageBehavior = false
        Me.lvwTransactionExport.View = System.Windows.Forms.View.Details
        '
        '_lvwTransactionExport_ColumnHeader_1
        '
        Me._lvwTransactionExport_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwTransactionExport_ColumnHeader_1.Text = "Exprt Folder Cnt"
        Me._lvwTransactionExport_ColumnHeader_1.Width = 97
        '
        '_lvwTransactionExport_ColumnHeader_2
        '
        Me._lvwTransactionExport_ColumnHeader_2.Tag = "VALUESORT"
        Me._lvwTransactionExport_ColumnHeader_2.Text = "Insurance File Cnt"
        Me._lvwTransactionExport_ColumnHeader_2.Width = 97
        '
        '_lvwTransactionExport_ColumnHeader_3
        '
        Me._lvwTransactionExport_ColumnHeader_3.Text = "Insurance Ref"
        Me._lvwTransactionExport_ColumnHeader_3.Width = 97
        '
        '_lvwTransactionExport_ColumnHeader_4
        '
        Me._lvwTransactionExport_ColumnHeader_4.Text = "Document Ref"
        Me._lvwTransactionExport_ColumnHeader_4.Width = 97
        '
        '_lvwTransactionExport_ColumnHeader_5
        '
        Me._lvwTransactionExport_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwTransactionExport_ColumnHeader_5.Text = "Document Date"
        Me._lvwTransactionExport_ColumnHeader_5.Width = 97
        '
        '_lvwTransactionExport_ColumnHeader_6
        '
        Me._lvwTransactionExport_ColumnHeader_6.Text = "Export Status"
        Me._lvwTransactionExport_ColumnHeader_6.Width = 97
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.txtClaimNo)
        Me.TabPage2.Controls.Add(Me.OptReverseReg)
        Me.TabPage2.Controls.Add(Me.OptReverse)
        Me.TabPage2.Controls.Add(Me.CmdClaiimFix)
        Me.TabPage2.Controls.Add(Me.CmdGetClaimVersions)
        Me.TabPage2.Controls.Add(Me.LvwClaimVersion)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.TxtSqlClaim)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(788, 442)
        Me.TabPage2.TabIndex = 3
        Me.TabPage2.Text = "Claim"
        Me.TabPage2.UseVisualStyleBackColor = true
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Location = New System.Drawing.Point(9, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 13)
        Me.Label7.TabIndex = 55
        Me.Label7.Text = "Claim Number"
        '
        'txtClaimNo
        '
        Me.txtClaimNo.Location = New System.Drawing.Point(98, 12)
        Me.txtClaimNo.Name = "txtClaimNo"
        Me.txtClaimNo.Size = New System.Drawing.Size(243, 20)
        Me.txtClaimNo.TabIndex = 54
        '
        'OptReverseReg
        '
        Me.OptReverseReg.AutoSize = true
        Me.OptReverseReg.Location = New System.Drawing.Point(629, 64)
        Me.OptReverseReg.Name = "OptReverseReg"
        Me.OptReverseReg.Size = New System.Drawing.Size(139, 17)
        Me.OptReverseReg.TabIndex = 53
        Me.OptReverseReg.Text = "Reverse and Regenrate"
        Me.OptReverseReg.UseVisualStyleBackColor = true
        '
        'OptReverse
        '
        Me.OptReverse.AutoSize = true
        Me.OptReverse.Checked = true
        Me.OptReverse.Location = New System.Drawing.Point(629, 41)
        Me.OptReverse.Name = "OptReverse"
        Me.OptReverse.Size = New System.Drawing.Size(65, 17)
        Me.OptReverse.TabIndex = 52
        Me.OptReverse.TabStop = true
        Me.OptReverse.Text = "Reverse"
        Me.OptReverse.UseVisualStyleBackColor = true
        '
        'CmdClaiimFix
        '
        Me.CmdClaiimFix.BackColor = System.Drawing.SystemColors.Control
        Me.CmdClaiimFix.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdClaiimFix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.CmdClaiimFix.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdClaiimFix.Location = New System.Drawing.Point(657, 401)
        Me.CmdClaiimFix.Name = "CmdClaiimFix"
        Me.CmdClaiimFix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdClaiimFix.Size = New System.Drawing.Size(70, 22)
        Me.CmdClaiimFix.TabIndex = 50
        Me.CmdClaiimFix.Text = "Fix Claim"
        Me.CmdClaiimFix.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdClaiimFix.UseVisualStyleBackColor = false
        '
        'CmdGetClaimVersions
        '
        Me.CmdGetClaimVersions.BackColor = System.Drawing.SystemColors.Control
        Me.CmdGetClaimVersions.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdGetClaimVersions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.CmdGetClaimVersions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdGetClaimVersions.Location = New System.Drawing.Point(524, 114)
        Me.CmdGetClaimVersions.Name = "CmdGetClaimVersions"
        Me.CmdGetClaimVersions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdGetClaimVersions.Size = New System.Drawing.Size(118, 25)
        Me.CmdGetClaimVersions.TabIndex = 49
        Me.CmdGetClaimVersions.Text = "Get Claims Versions"
        Me.CmdGetClaimVersions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdGetClaimVersions.UseVisualStyleBackColor = false
        '
        'LvwClaimVersion
        '
        Me.LvwClaimVersion.AllowColumnReorder = true
        Me.LvwClaimVersion.BackColor = System.Drawing.SystemColors.Window
        Me.LvwClaimVersion.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.LvwClaimVersion, "")
        Me.LvwClaimVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.LvwClaimVersion, false)
        Me.LvwClaimVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LvwClaimVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LvwClaimVersion.FullRowSelect = true
        Me.LvwClaimVersion.GridLines = true
        Me.LvwClaimVersion.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.LvwClaimVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.LvwClaimVersion, "")
        Me.LvwClaimVersion.Location = New System.Drawing.Point(12, 145)
        Me.LvwClaimVersion.Name = "LvwClaimVersion"
        Me.LvwClaimVersion.Size = New System.Drawing.Size(715, 235)
        Me.listViewHelper1.SetSmallIcons(Me.LvwClaimVersion, "")
        Me.listViewHelper1.SetSorted(Me.LvwClaimVersion, false)
        Me.listViewHelper1.SetSortKey(Me.LvwClaimVersion, 0)
        Me.listViewHelper1.SetSortOrder(Me.LvwClaimVersion, System.Windows.Forms.SortOrder.Ascending)
        Me.LvwClaimVersion.TabIndex = 48
        Me.LvwClaimVersion.UseCompatibleStateImageBehavior = false
        Me.LvwClaimVersion.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Tag = "VALUESORT"
        Me.ColumnHeader1.Text = "Claim Id"
        Me.ColumnHeader1.Width = 90
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Claim Number"
        Me.ColumnHeader2.Width = 130
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Document Ref"
        Me.ColumnHeader3.Width = 120
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Transaction Type Code"
        Me.ColumnHeader4.Width = 144
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = " "
        Me.ColumnHeader5.Width = 97
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Tag = "DATESORT"
        Me.ColumnHeader6.Text = " "
        Me.ColumnHeader6.Width = 97
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Tag = "DATESORT"
        Me.ColumnHeader7.Text = " "
        Me.ColumnHeader7.Width = 97
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Location = New System.Drawing.Point(9, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(451, 13)
        Me.Label4.TabIndex = 47
        Me.Label4.Text = "Copy Sql here for Claim Process( In SQL type column in same sequence as show in b"& _ 
    "elow List)"
        '
        'TxtSqlClaim
        '
        Me.TxtSqlClaim.Location = New System.Drawing.Point(12, 51)
        Me.TxtSqlClaim.Multiline = true
        Me.TxtSqlClaim.Name = "TxtSqlClaim"
        Me.TxtSqlClaim.Size = New System.Drawing.Size(489, 88)
        Me.TxtSqlClaim.TabIndex = 46
        '
        '_tabMain_TabPage2
        '
        Me._tabMain_TabPage2.Controls.Add(Me.tabClaim)
        Me._tabMain_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage2.Name = "_tabMain_TabPage2"
        Me._tabMain_TabPage2.Size = New System.Drawing.Size(799, 518)
        Me._tabMain_TabPage2.TabIndex = 2
        Me._tabMain_TabPage2.Text = "Claim"
        '
        'tabClaim
        '
        Me.tabClaim.Controls.Add(Me._tabClaim_TabPage0)
        Me.tabClaim.Controls.Add(Me._tabClaim_TabPage1)
        Me.tabClaim.Controls.Add(Me._tabClaim_TabPage2)
        Me.tabClaim.Controls.Add(Me._tabClaim_TabPage3)
        Me.tabClaim.Controls.Add(Me._tabClaim_TabPage4)
        Me.tabClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tabClaim.ItemSize = New System.Drawing.Size(154, 18)
        Me.tabClaim.Location = New System.Drawing.Point(3, 10)
        Me.tabClaim.Multiline = true
        Me.tabClaim.Name = "tabClaim"
        Me.tabClaim.SelectedIndex = 0
        Me.tabClaim.Size = New System.Drawing.Size(782, 455)
        Me.tabClaim.TabIndex = 12
        '
        '_tabClaim_TabPage0
        '
        Me._tabClaim_TabPage0.Controls.Add(Me.chkDeleteStats)
        Me._tabClaim_TabPage0.Controls.Add(Me.fraFailedClaimTransaction)
        Me._tabClaim_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabClaim_TabPage0.Name = "_tabClaim_TabPage0"
        Me._tabClaim_TabPage0.Size = New System.Drawing.Size(774, 429)
        Me._tabClaim_TabPage0.TabIndex = 0
        Me._tabClaim_TabPage0.Text = "Failed Claim's Transactions"
        '
        'chkDeleteStats
        '
        Me.chkDeleteStats.BackColor = System.Drawing.SystemColors.Control
        Me.chkDeleteStats.Checked = true
        Me.chkDeleteStats.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDeleteStats.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDeleteStats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDeleteStats.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDeleteStats.Location = New System.Drawing.Point(3, 44)
        Me.chkDeleteStats.Name = "chkDeleteStats"
        Me.chkDeleteStats.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDeleteStats.Size = New System.Drawing.Size(304, 16)
        Me.chkDeleteStats.TabIndex = 13
        Me.chkDeleteStats.Text = "Delete Existing Stats For Selected Documents"
        Me.chkDeleteStats.UseVisualStyleBackColor = false
        '
        'fraFailedClaimTransaction
        '
        Me.fraFailedClaimTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.fraFailedClaimTransaction.Controls.Add(Me.lvwClaim)
        Me.fraFailedClaimTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraFailedClaimTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFailedClaimTransaction.Location = New System.Drawing.Point(3, 62)
        Me.fraFailedClaimTransaction.Name = "fraFailedClaimTransaction"
        Me.fraFailedClaimTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFailedClaimTransaction.Size = New System.Drawing.Size(772, 403)
        Me.fraFailedClaimTransaction.TabIndex = 14
        Me.fraFailedClaimTransaction.TabStop = false
        Me.fraFailedClaimTransaction.Text = "Failed Claim Transaction"
        '
        'lvwClaim
        '
        Me.lvwClaim.AllowColumnReorder = true
        Me.lvwClaim.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaim.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaim, "")
        Me.lvwClaim.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaim_ColumnHeader_1, Me._lvwClaim_ColumnHeader_2, Me._lvwClaim_ColumnHeader_3, Me._lvwClaim_ColumnHeader_4, Me._lvwClaim_ColumnHeader_5, Me._lvwClaim_ColumnHeader_6, Me._lvwClaim_ColumnHeader_7, Me._lvwClaim_ColumnHeader_8, Me._lvwClaim_ColumnHeader_9, Me._lvwClaim_ColumnHeader_10, Me._lvwClaim_ColumnHeader_11, Me._lvwClaim_ColumnHeader_12, Me._lvwClaim_ColumnHeader_13, Me._lvwClaim_ColumnHeader_14})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaim, false)
        Me.lvwClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaim.FullRowSelect = true
        Me.lvwClaim.GridLines = true
        Me.lvwClaim.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaim, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaim, "")
        Me.lvwClaim.Location = New System.Drawing.Point(3, 12)
        Me.lvwClaim.Name = "lvwClaim"
        Me.lvwClaim.Size = New System.Drawing.Size(766, 388)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaim, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaim, false)
        Me.listViewHelper1.SetSortKey(Me.lvwClaim, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaim, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaim.TabIndex = 15
        Me.lvwClaim.UseCompatibleStateImageBehavior = false
        Me.lvwClaim.View = System.Windows.Forms.View.Details
        '
        '_lvwClaim_ColumnHeader_1
        '
        Me._lvwClaim_ColumnHeader_1.Text = "ClaimNumber"
        Me._lvwClaim_ColumnHeader_1.Width = 97
        '
        '_lvwClaim_ColumnHeader_2
        '
        Me._lvwClaim_ColumnHeader_2.Text = "DocRef"
        Me._lvwClaim_ColumnHeader_2.Width = 97
        '
        '_lvwClaim_ColumnHeader_3
        '
        Me._lvwClaim_ColumnHeader_3.Tag = "DATESORT"
        Me._lvwClaim_ColumnHeader_3.Text = "DocDate"
        Me._lvwClaim_ColumnHeader_3.Width = 97
        '
        '_lvwClaim_ColumnHeader_4
        '
        Me._lvwClaim_ColumnHeader_4.Tag = "DATESORT"
        Me._lvwClaim_ColumnHeader_4.Text = "PaymentDate"
        Me._lvwClaim_ColumnHeader_4.Width = 97
        '
        '_lvwClaim_ColumnHeader_5
        '
        Me._lvwClaim_ColumnHeader_5.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_5.Text = "PremiumTotal"
        Me._lvwClaim_ColumnHeader_5.Width = 97
        '
        '_lvwClaim_ColumnHeader_6
        '
        Me._lvwClaim_ColumnHeader_6.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_6.Text = "PaymentAmount"
        Me._lvwClaim_ColumnHeader_6.Width = 97
        '
        '_lvwClaim_ColumnHeader_7
        '
        Me._lvwClaim_ColumnHeader_7.Text = "PaymentPartyCode"
        Me._lvwClaim_ColumnHeader_7.Width = 97
        '
        '_lvwClaim_ColumnHeader_8
        '
        Me._lvwClaim_ColumnHeader_8.Text = "Posting Status"
        Me._lvwClaim_ColumnHeader_8.Width = 97
        '
        '_lvwClaim_ColumnHeader_9
        '
        Me._lvwClaim_ColumnHeader_9.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_9.Text = "ExportFolderCnt"
        Me._lvwClaim_ColumnHeader_9.Width = 97
        '
        '_lvwClaim_ColumnHeader_10
        '
        Me._lvwClaim_ColumnHeader_10.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_10.Text = "InsuranceFileCnt"
        Me._lvwClaim_ColumnHeader_10.Width = 97
        '
        '_lvwClaim_ColumnHeader_11
        '
        Me._lvwClaim_ColumnHeader_11.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_11.Text = "ClaimID"
        Me._lvwClaim_ColumnHeader_11.Width = 97
        '
        '_lvwClaim_ColumnHeader_12
        '
        Me._lvwClaim_ColumnHeader_12.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_12.Text = "PaymentPartyCnt"
        Me._lvwClaim_ColumnHeader_12.Width = 97
        '
        '_lvwClaim_ColumnHeader_13
        '
        Me._lvwClaim_ColumnHeader_13.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_13.Text = "OriginalReserveID"
        Me._lvwClaim_ColumnHeader_13.Width = 97
        '
        '_lvwClaim_ColumnHeader_14
        '
        Me._lvwClaim_ColumnHeader_14.Tag = "VALUESORT"
        Me._lvwClaim_ColumnHeader_14.Text = "PaymentID"
        Me._lvwClaim_ColumnHeader_14.Width = 97
        '
        '_tabClaim_TabPage1
        '
        Me._tabClaim_TabPage1.Controls.Add(Me.fraImbalancedClosedClaim)
        Me._tabClaim_TabPage1.Controls.Add(Me.chkAutoProcess)
        Me._tabClaim_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabClaim_TabPage1.Name = "_tabClaim_TabPage1"
        Me._tabClaim_TabPage1.Size = New System.Drawing.Size(774, 429)
        Me._tabClaim_TabPage1.TabIndex = 1
        Me._tabClaim_TabPage1.Text = "Imbalanced Closed Claims"
        '
        'fraImbalancedClosedClaim
        '
        Me.fraImbalancedClosedClaim.BackColor = System.Drawing.SystemColors.Control
        Me.fraImbalancedClosedClaim.Controls.Add(Me.lvwImbalancedClosedClaim)
        Me.fraImbalancedClosedClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraImbalancedClosedClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImbalancedClosedClaim.Location = New System.Drawing.Point(6, 80)
        Me.fraImbalancedClosedClaim.Name = "fraImbalancedClosedClaim"
        Me.fraImbalancedClosedClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImbalancedClosedClaim.Size = New System.Drawing.Size(766, 385)
        Me.fraImbalancedClosedClaim.TabIndex = 16
        Me.fraImbalancedClosedClaim.TabStop = false
        Me.fraImbalancedClosedClaim.Text = "Imbalanced Closed Claims"
        '
        'lvwImbalancedClosedClaim
        '
        Me.lvwImbalancedClosedClaim.AllowColumnReorder = true
        Me.lvwImbalancedClosedClaim.BackColor = System.Drawing.SystemColors.Window
        Me.lvwImbalancedClosedClaim.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwImbalancedClosedClaim, "")
        Me.lvwImbalancedClosedClaim.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwImbalancedClosedClaim_ColumnHeader_1, Me._lvwImbalancedClosedClaim_ColumnHeader_2, Me._lvwImbalancedClosedClaim_ColumnHeader_3, Me._lvwImbalancedClosedClaim_ColumnHeader_4, Me._lvwImbalancedClosedClaim_ColumnHeader_5, Me._lvwImbalancedClosedClaim_ColumnHeader_6, Me._lvwImbalancedClosedClaim_ColumnHeader_7, Me._lvwImbalancedClosedClaim_ColumnHeader_8, Me._lvwImbalancedClosedClaim_ColumnHeader_9, Me._lvwImbalancedClosedClaim_ColumnHeader_10, Me._lvwImbalancedClosedClaim_ColumnHeader_11})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwImbalancedClosedClaim, false)
        Me.lvwImbalancedClosedClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwImbalancedClosedClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwImbalancedClosedClaim.FullRowSelect = true
        Me.lvwImbalancedClosedClaim.GridLines = true
        Me.lvwImbalancedClosedClaim.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwImbalancedClosedClaim, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwImbalancedClosedClaim, "")
        Me.lvwImbalancedClosedClaim.Location = New System.Drawing.Point(3, 15)
        Me.lvwImbalancedClosedClaim.Name = "lvwImbalancedClosedClaim"
        Me.lvwImbalancedClosedClaim.Size = New System.Drawing.Size(760, 367)
        Me.listViewHelper1.SetSmallIcons(Me.lvwImbalancedClosedClaim, "")
        Me.listViewHelper1.SetSorted(Me.lvwImbalancedClosedClaim, false)
        Me.listViewHelper1.SetSortKey(Me.lvwImbalancedClosedClaim, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwImbalancedClosedClaim, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwImbalancedClosedClaim.TabIndex = 17
        Me.lvwImbalancedClosedClaim.UseCompatibleStateImageBehavior = false
        Me.lvwImbalancedClosedClaim.View = System.Windows.Forms.View.Details
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_1
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_1.Text = "ClaimID"
        Me._lvwImbalancedClosedClaim_ColumnHeader_1.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_2
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_2.Text = "ClaimNumber"
        Me._lvwImbalancedClosedClaim_ColumnHeader_2.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_3
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_3.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_3.Text = "CLO"
        Me._lvwImbalancedClosedClaim_ColumnHeader_3.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_4
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_4.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_4.Text = "InitReserve"
        Me._lvwImbalancedClosedClaim_ColumnHeader_4.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_5
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_5.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_5.Text = "CLA"
        Me._lvwImbalancedClosedClaim_ColumnHeader_5.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_6
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_6.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_6.Text = "RevisedRes"
        Me._lvwImbalancedClosedClaim_ColumnHeader_6.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_7
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_7.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_7.Text = "CLP"
        Me._lvwImbalancedClosedClaim_ColumnHeader_7.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_8
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_8.Tag = "DATESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_8.Text = "PaidToDate"
        Me._lvwImbalancedClosedClaim_ColumnHeader_8.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_9
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_9.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_9.Text = "PaymentTable"
        Me._lvwImbalancedClosedClaim_ColumnHeader_9.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_10
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_10.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_10.Text = "TotalStats"
        Me._lvwImbalancedClosedClaim_ColumnHeader_10.Width = 97
        '
        '_lvwImbalancedClosedClaim_ColumnHeader_11
        '
        Me._lvwImbalancedClosedClaim_ColumnHeader_11.Tag = "VALUESORT"
        Me._lvwImbalancedClosedClaim_ColumnHeader_11.Text = "TotalReserve"
        Me._lvwImbalancedClosedClaim_ColumnHeader_11.Width = 97
        '
        'chkAutoProcess
        '
        Me.chkAutoProcess.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoProcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAutoProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoProcess.Location = New System.Drawing.Point(6, 53)
        Me.chkAutoProcess.Name = "chkAutoProcess"
        Me.chkAutoProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoProcess.Size = New System.Drawing.Size(103, 16)
        Me.chkAutoProcess.TabIndex = 18
        Me.chkAutoProcess.Text = "Auto Process"
        Me.chkAutoProcess.UseVisualStyleBackColor = false
        '
        '_tabClaim_TabPage2
        '
        Me._tabClaim_TabPage2.Controls.Add(Me.Label1)
        Me._tabClaim_TabPage2.Controls.Add(Me.txtCPClaimNumber)
        Me._tabClaim_TabPage2.Controls.Add(Me.cmdCPRefresh)
        Me._tabClaim_TabPage2.Controls.Add(Me.fraClaimPosting)
        Me._tabClaim_TabPage2.Controls.Add(Me._optReservePayment_0)
        Me._tabClaim_TabPage2.Controls.Add(Me._optReservePayment_1)
        Me._tabClaim_TabPage2.Controls.Add(Me._chkReserve_0)
        Me._tabClaim_TabPage2.Controls.Add(Me._chkReserve_1)
        Me._tabClaim_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabClaim_TabPage2.Name = "_tabClaim_TabPage2"
        Me._tabClaim_TabPage2.Size = New System.Drawing.Size(774, 429)
        Me._tabClaim_TabPage2.TabIndex = 2
        Me._tabClaim_TabPage2.Text = "Claim Postings"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(6, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "Claim Number"
        '
        'txtCPClaimNumber
        '
        Me.txtCPClaimNumber.AcceptsReturn = true
        Me.txtCPClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCPClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCPClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtCPClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCPClaimNumber.Location = New System.Drawing.Point(96, 27)
        Me.txtCPClaimNumber.MaxLength = 0
        Me.txtCPClaimNumber.Name = "txtCPClaimNumber"
        Me.txtCPClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCPClaimNumber.Size = New System.Drawing.Size(244, 20)
        Me.txtCPClaimNumber.TabIndex = 49
        '
        'fraClaimPosting
        '
        Me.fraClaimPosting.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimPosting.Controls.Add(Me.lvwClaimPosting)
        Me.fraClaimPosting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraClaimPosting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimPosting.Location = New System.Drawing.Point(3, 99)
        Me.fraClaimPosting.Name = "fraClaimPosting"
        Me.fraClaimPosting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimPosting.Size = New System.Drawing.Size(772, 328)
        Me.fraClaimPosting.TabIndex = 52
        Me.fraClaimPosting.TabStop = false
        Me.fraClaimPosting.Text = "Claim Postings"
        '
        'lvwClaimPosting
        '
        Me.lvwClaimPosting.AllowColumnReorder = true
        Me.lvwClaimPosting.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimPosting.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaimPosting, "")
        Me.lvwClaimPosting.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaimPosting_ColumnHeader_1, Me._lvwClaimPosting_ColumnHeader_2, Me._lvwClaimPosting_ColumnHeader_3, Me._lvwClaimPosting_ColumnHeader_4, Me._lvwClaimPosting_ColumnHeader_5, Me._lvwClaimPosting_ColumnHeader_6, Me._lvwClaimPosting_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaimPosting, false)
        Me.lvwClaimPosting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwClaimPosting.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimPosting.FullRowSelect = true
        Me.lvwClaimPosting.GridLines = true
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaimPosting, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaimPosting, "")
        Me.lvwClaimPosting.Location = New System.Drawing.Point(3, 15)
        Me.lvwClaimPosting.Name = "lvwClaimPosting"
        Me.lvwClaimPosting.Size = New System.Drawing.Size(766, 310)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaimPosting, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaimPosting, false)
        Me.listViewHelper1.SetSortKey(Me.lvwClaimPosting, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaimPosting, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaimPosting.TabIndex = 53
        Me.lvwClaimPosting.UseCompatibleStateImageBehavior = false
        Me.lvwClaimPosting.View = System.Windows.Forms.View.Details
        '
        '_lvwClaimPosting_ColumnHeader_1
        '
        Me._lvwClaimPosting_ColumnHeader_1.Text = "Policy Number"
        Me._lvwClaimPosting_ColumnHeader_1.Width = 145
        '
        '_lvwClaimPosting_ColumnHeader_2
        '
        Me._lvwClaimPosting_ColumnHeader_2.Text = "Claim Number"
        Me._lvwClaimPosting_ColumnHeader_2.Width = 97
        '
        '_lvwClaimPosting_ColumnHeader_3
        '
        Me._lvwClaimPosting_ColumnHeader_3.Text = "DocumentRef"
        Me._lvwClaimPosting_ColumnHeader_3.Width = 132
        '
        '_lvwClaimPosting_ColumnHeader_4
        '
        Me._lvwClaimPosting_ColumnHeader_4.Tag = "DATESORT"
        Me._lvwClaimPosting_ColumnHeader_4.Text = "DocumentDate"
        Me._lvwClaimPosting_ColumnHeader_4.Width = 125
        '
        '_lvwClaimPosting_ColumnHeader_5
        '
        Me._lvwClaimPosting_ColumnHeader_5.Text = "StatsDetailType"
        Me._lvwClaimPosting_ColumnHeader_5.Width = 112
        '
        '_lvwClaimPosting_ColumnHeader_6
        '
        Me._lvwClaimPosting_ColumnHeader_6.Tag = "VALUESORT"
        Me._lvwClaimPosting_ColumnHeader_6.Text = "ThisPremiumHome"
        Me._lvwClaimPosting_ColumnHeader_6.Width = 150
        '
        '_lvwClaimPosting_ColumnHeader_7
        '
        Me._lvwClaimPosting_ColumnHeader_7.Text = "PeriodID"
        Me._lvwClaimPosting_ColumnHeader_7.Width = 97
        '
        '_optReservePayment_0
        '
        Me._optReservePayment_0.BackColor = System.Drawing.SystemColors.Control
        Me._optReservePayment_0.Checked = true
        Me._optReservePayment_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReservePayment_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optReservePayment_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optReservePayment_0.Location = New System.Drawing.Point(606, 23)
        Me._optReservePayment_0.Name = "_optReservePayment_0"
        Me._optReservePayment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReservePayment_0.Size = New System.Drawing.Size(163, 17)
        Me._optReservePayment_0.TabIndex = 54
        Me._optReservePayment_0.TabStop = true
        Me._optReservePayment_0.Text = "Add Reserve Posting"
        Me._optReservePayment_0.UseVisualStyleBackColor = false
        '
        '_optReservePayment_1
        '
        Me._optReservePayment_1.BackColor = System.Drawing.SystemColors.Control
        Me._optReservePayment_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReservePayment_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._optReservePayment_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optReservePayment_1.Location = New System.Drawing.Point(606, 77)
        Me._optReservePayment_1.Name = "_optReservePayment_1"
        Me._optReservePayment_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReservePayment_1.Size = New System.Drawing.Size(163, 28)
        Me._optReservePayment_1.TabIndex = 55
        Me._optReservePayment_1.TabStop = true
        Me._optReservePayment_1.Text = "Add Payment Posting"
        Me._optReservePayment_1.UseVisualStyleBackColor = false
        '
        '_chkReserve_0
        '
        Me._chkReserve_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkReserve_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkReserve_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._chkReserve_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkReserve_0.Location = New System.Drawing.Point(624, 34)
        Me._chkReserve_0.Name = "_chkReserve_0"
        Me._chkReserve_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkReserve_0.Size = New System.Drawing.Size(112, 26)
        Me._chkReserve_0.TabIndex = 58
        Me._chkReserve_0.Text = "Initial Reserve"
        Me._chkReserve_0.UseVisualStyleBackColor = false
        '
        '_chkReserve_1
        '
        Me._chkReserve_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkReserve_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkReserve_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me._chkReserve_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkReserve_1.Location = New System.Drawing.Point(624, 58)
        Me._chkReserve_1.Name = "_chkReserve_1"
        Me._chkReserve_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkReserve_1.Size = New System.Drawing.Size(112, 25)
        Me._chkReserve_1.TabIndex = 59
        Me._chkReserve_1.Text = "Revise Reserve"
        Me._chkReserve_1.UseVisualStyleBackColor = false
        '
        '_tabClaim_TabPage3
        '
        Me._tabClaim_TabPage3.Controls.Add(Me.lblClaimNumber)
        Me._tabClaim_TabPage3.Controls.Add(Me.txtClaimNumber)
        Me._tabClaim_TabPage3.Controls.Add(Me.fraReserve)
        Me._tabClaim_TabPage3.Controls.Add(Me.fraPayment)
        Me._tabClaim_TabPage3.Controls.Add(Me.cmdReservePaymentRefresh)
        Me._tabClaim_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabClaim_TabPage3.Name = "_tabClaim_TabPage3"
        Me._tabClaim_TabPage3.Size = New System.Drawing.Size(774, 429)
        Me._tabClaim_TabPage3.TabIndex = 3
        Me._tabClaim_TabPage3.Text = "Reserve/Payment Details"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(6, 36)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(88, 13)
        Me.lblClaimNumber.TabIndex = 47
        Me.lblClaimNumber.Text = "Claim Number"
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = true
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(96, 30)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(244, 20)
        Me.txtClaimNumber.TabIndex = 42
        '
        'fraReserve
        '
        Me.fraReserve.BackColor = System.Drawing.SystemColors.Control
        Me.fraReserve.Controls.Add(Me.lvwReserve)
        Me.fraReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserve.Location = New System.Drawing.Point(0, 66)
        Me.fraReserve.Name = "fraReserve"
        Me.fraReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserve.Size = New System.Drawing.Size(766, 220)
        Me.fraReserve.TabIndex = 43
        Me.fraReserve.TabStop = false
        Me.fraReserve.Text = "Reserve"
        '
        'lvwReserve
        '
        Me.lvwReserve.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwReserve, "")
        Me.lvwReserve.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReserve_ColumnHeader_1, Me._lvwReserve_ColumnHeader_2, Me._lvwReserve_ColumnHeader_3, Me._lvwReserve_ColumnHeader_4, Me._lvwReserve_ColumnHeader_5, Me._lvwReserve_ColumnHeader_6, Me._lvwReserve_ColumnHeader_7, Me._lvwReserve_ColumnHeader_8, Me._lvwReserve_ColumnHeader_9, Me._lvwReserve_ColumnHeader_10})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReserve, false)
        Me.lvwReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReserve.FullRowSelect = true
        Me.lvwReserve.GridLines = true
        Me.lvwReserve.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwReserve, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwReserve, "")
        Me.lvwReserve.Location = New System.Drawing.Point(3, 15)
        Me.lvwReserve.Name = "lvwReserve"
        Me.lvwReserve.Size = New System.Drawing.Size(757, 202)
        Me.listViewHelper1.SetSmallIcons(Me.lvwReserve, "")
        Me.listViewHelper1.SetSorted(Me.lvwReserve, false)
        Me.listViewHelper1.SetSortKey(Me.lvwReserve, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwReserve, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwReserve.TabIndex = 44
        Me.lvwReserve.UseCompatibleStateImageBehavior = false
        Me.lvwReserve.View = System.Windows.Forms.View.Details
        '
        '_lvwReserve_ColumnHeader_1
        '
        Me._lvwReserve_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_1.Text = "ReserveID"
        Me._lvwReserve_ColumnHeader_1.Width = 97
        '
        '_lvwReserve_ColumnHeader_2
        '
        Me._lvwReserve_ColumnHeader_2.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_2.Text = "ClaimPerilID"
        Me._lvwReserve_ColumnHeader_2.Width = 97
        '
        '_lvwReserve_ColumnHeader_3
        '
        Me._lvwReserve_ColumnHeader_3.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_3.Text = "ReserveTypeID"
        Me._lvwReserve_ColumnHeader_3.Width = 97
        '
        '_lvwReserve_ColumnHeader_4
        '
        Me._lvwReserve_ColumnHeader_4.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_4.Text = "InitReserve"
        Me._lvwReserve_ColumnHeader_4.Width = 97
        '
        '_lvwReserve_ColumnHeader_5
        '
        Me._lvwReserve_ColumnHeader_5.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_5.Text = "RevisedRes"
        Me._lvwReserve_ColumnHeader_5.Width = 97
        '
        '_lvwReserve_ColumnHeader_6
        '
        Me._lvwReserve_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwReserve_ColumnHeader_6.Text = "PaidToDate"
        Me._lvwReserve_ColumnHeader_6.Width = 97
        '
        '_lvwReserve_ColumnHeader_7
        '
        Me._lvwReserve_ColumnHeader_7.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_7.Text = "ThisRevision"
        Me._lvwReserve_ColumnHeader_7.Width = 97
        '
        '_lvwReserve_ColumnHeader_8
        '
        Me._lvwReserve_ColumnHeader_8.Tag = "VALUESORT"
        Me._lvwReserve_ColumnHeader_8.Text = "ThisPayment"
        Me._lvwReserve_ColumnHeader_8.Width = 97
        '
        '_lvwReserve_ColumnHeader_9
        '
        Me._lvwReserve_ColumnHeader_9.Text = "ReserveType"
        Me._lvwReserve_ColumnHeader_9.Width = 134
        '
        '_lvwReserve_ColumnHeader_10
        '
        Me._lvwReserve_ColumnHeader_10.Text = "ClaimPeril"
        Me._lvwReserve_ColumnHeader_10.Width = 134
        '
        'fraPayment
        '
        Me.fraPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayment.Controls.Add(Me.lvwPayment)
        Me.fraPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayment.Location = New System.Drawing.Point(0, 288)
        Me.fraPayment.Name = "fraPayment"
        Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayment.Size = New System.Drawing.Size(766, 160)
        Me.fraPayment.TabIndex = 45
        Me.fraPayment.TabStop = false
        Me.fraPayment.Text = "Payment"
        '
        'lvwPayment
        '
        Me.lvwPayment.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPayment, "")
        Me.lvwPayment.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPayment_ColumnHeader_1, Me._lvwPayment_ColumnHeader_2, Me._lvwPayment_ColumnHeader_3, Me._lvwPayment_ColumnHeader_4, Me._lvwPayment_ColumnHeader_5, Me._lvwPayment_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPayment, false)
        Me.lvwPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPayment.FullRowSelect = true
        Me.lvwPayment.GridLines = true
        Me.lvwPayment.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPayment, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPayment, "")
        Me.lvwPayment.Location = New System.Drawing.Point(3, 15)
        Me.lvwPayment.Name = "lvwPayment"
        Me.lvwPayment.Size = New System.Drawing.Size(757, 142)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPayment, "")
        Me.listViewHelper1.SetSorted(Me.lvwPayment, false)
        Me.listViewHelper1.SetSortKey(Me.lvwPayment, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPayment, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPayment.TabIndex = 46
        Me.lvwPayment.UseCompatibleStateImageBehavior = false
        Me.lvwPayment.View = System.Windows.Forms.View.Details
        '
        '_lvwPayment_ColumnHeader_1
        '
        Me._lvwPayment_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwPayment_ColumnHeader_1.Text = "PaymentID"
        Me._lvwPayment_ColumnHeader_1.Width = 97
        '
        '_lvwPayment_ColumnHeader_2
        '
        Me._lvwPayment_ColumnHeader_2.Tag = "VALUESORT"
        Me._lvwPayment_ColumnHeader_2.Text = "ReserveID"
        Me._lvwPayment_ColumnHeader_2.Width = 97
        '
        '_lvwPayment_ColumnHeader_3
        '
        Me._lvwPayment_ColumnHeader_3.Tag = "VALUESORT"
        Me._lvwPayment_ColumnHeader_3.Text = "ClaimPerilID"
        Me._lvwPayment_ColumnHeader_3.Width = 97
        '
        '_lvwPayment_ColumnHeader_4
        '
        Me._lvwPayment_ColumnHeader_4.Tag = "VALUESORT"
        Me._lvwPayment_ColumnHeader_4.Text = "Amount"
        Me._lvwPayment_ColumnHeader_4.Width = 97
        '
        '_lvwPayment_ColumnHeader_5
        '
        Me._lvwPayment_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwPayment_ColumnHeader_5.Text = "PaymentDate"
        Me._lvwPayment_ColumnHeader_5.Width = 97
        '
        '_lvwPayment_ColumnHeader_6
        '
        Me._lvwPayment_ColumnHeader_6.Text = "PartyCode"
        Me._lvwPayment_ColumnHeader_6.Width = 97
        '
        '_tabClaim_TabPage4
        '
        Me._tabClaim_TabPage4.Controls.Add(Me.fraClaimMisc)
        Me._tabClaim_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabClaim_TabPage4.Name = "_tabClaim_TabPage4"
        Me._tabClaim_TabPage4.Size = New System.Drawing.Size(774, 429)
        Me._tabClaim_TabPage4.TabIndex = 4
        Me._tabClaim_TabPage4.Text = "Miscellaneous"
        '
        'fraClaimMisc
        '
        Me.fraClaimMisc.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimMisc.Controls.Add(Me.lvwClaimMisc)
        Me.fraClaimMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraClaimMisc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimMisc.Location = New System.Drawing.Point(6, 1)
        Me.fraClaimMisc.Name = "fraClaimMisc"
        Me.fraClaimMisc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimMisc.Size = New System.Drawing.Size(766, 424)
        Me.fraClaimMisc.TabIndex = 56
        Me.fraClaimMisc.TabStop = false
        Me.fraClaimMisc.Text = "Claim Misc"
        '
        'lvwClaimMisc
        '
        Me.lvwClaimMisc.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimMisc.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaimMisc, false)
        Me.lvwClaimMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwClaimMisc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimMisc.FullRowSelect = true
        Me.lvwClaimMisc.GridLines = true
        Me.lvwClaimMisc.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaimMisc, "")
        Me.lvwClaimMisc.Location = New System.Drawing.Point(3, 15)
        Me.lvwClaimMisc.Name = "lvwClaimMisc"
        Me.lvwClaimMisc.Size = New System.Drawing.Size(760, 406)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaimMisc, false)
        Me.listViewHelper1.SetSortKey(Me.lvwClaimMisc, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaimMisc, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaimMisc.TabIndex = 57
        Me.lvwClaimMisc.UseCompatibleStateImageBehavior = false
        Me.lvwClaimMisc.View = System.Windows.Forms.View.Details
        '
        '_tabMain_TabPage3
        '
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_5)
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_4)
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_3)
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_0)
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_1)
        Me._tabMain_TabPage3.Controls.Add(Me._optMiscellaneous_2)
        Me._tabMain_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage3.Name = "_tabMain_TabPage3"
        Me._tabMain_TabPage3.Size = New System.Drawing.Size(799, 518)
        Me._tabMain_TabPage3.TabIndex = 3
        Me._tabMain_TabPage3.Text = "Miscellaneous"
        '
        '_optMiscellaneous_5
        '
        Me._optMiscellaneous_5.AutoSize = true
        Me._optMiscellaneous_5.Location = New System.Drawing.Point(24, 122)
        Me._optMiscellaneous_5.Name = "_optMiscellaneous_5"
        Me._optMiscellaneous_5.Size = New System.Drawing.Size(170, 17)
        Me._optMiscellaneous_5.TabIndex = 42
        Me._optMiscellaneous_5.TabStop = true
        Me._optMiscellaneous_5.Text = "Reverse Document Reference"
        Me._optMiscellaneous_5.UseVisualStyleBackColor = true
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.chkReverseAllocation)
        Me.TabPage1.Controls.Add(Me.lvwSRPDcouments)
        Me.TabPage1.Controls.Add(Me.btnGetSRPDetails)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.txtSRPSQL)
        Me.TabPage1.Controls.Add(Me.btnReverseTrans)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.txtDocumentRef)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(799, 518)
        Me.TabPage1.TabIndex = 4
        Me.TabPage1.Text = "SRP Reversal"
        Me.TabPage1.UseVisualStyleBackColor = true
        '
        'chkReverseAllocation
        '
        Me.chkReverseAllocation.AutoSize = true
        Me.chkReverseAllocation.Location = New System.Drawing.Point(145, 359)
        Me.chkReverseAllocation.Name = "chkReverseAllocation"
        Me.chkReverseAllocation.Size = New System.Drawing.Size(136, 17)
        Me.chkReverseAllocation.TabIndex = 52
        Me.chkReverseAllocation.Text = "Reverse allocation only"
        Me.chkReverseAllocation.UseVisualStyleBackColor = true
        '
        'lvwSRPDcouments
        '
        Me.lvwSRPDcouments.AllowColumnReorder = true
        Me.lvwSRPDcouments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSRPDcouments.CheckBoxes = true
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSRPDcouments, "")
        Me.lvwSRPDcouments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader8, Me.ColumnHeader9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSRPDcouments, false)
        Me.lvwSRPDcouments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwSRPDcouments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSRPDcouments.FullRowSelect = true
        Me.lvwSRPDcouments.GridLines = true
        Me.lvwSRPDcouments.HideSelection = false
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSRPDcouments, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSRPDcouments, "")
        Me.lvwSRPDcouments.Location = New System.Drawing.Point(39, 160)
        Me.lvwSRPDcouments.Name = "lvwSRPDcouments"
        Me.lvwSRPDcouments.Size = New System.Drawing.Size(376, 175)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSRPDcouments, "")
        Me.listViewHelper1.SetSorted(Me.lvwSRPDcouments, false)
        Me.listViewHelper1.SetSortKey(Me.lvwSRPDcouments, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSRPDcouments, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSRPDcouments.TabIndex = 51
        Me.lvwSRPDcouments.UseCompatibleStateImageBehavior = false
        Me.lvwSRPDcouments.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Tag = "VALUESORT"
        Me.ColumnHeader8.Text = "Document Id"
        Me.ColumnHeader8.Width = 90
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Document Ref"
        Me.ColumnHeader9.Width = 130
        '
        'btnGetSRPDetails
        '
        Me.btnGetSRPDetails.BackColor = System.Drawing.SystemColors.Control
        Me.btnGetSRPDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnGetSRPDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnGetSRPDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGetSRPDetails.Location = New System.Drawing.Point(534, 120)
        Me.btnGetSRPDetails.Name = "btnGetSRPDetails"
        Me.btnGetSRPDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnGetSRPDetails.Size = New System.Drawing.Size(141, 25)
        Me.btnGetSRPDetails.TabIndex = 50
        Me.btnGetSRPDetails.Text = "Get Document Details"
        Me.btnGetSRPDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnGetSRPDetails.UseVisualStyleBackColor = false
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Location = New System.Drawing.Point(36, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(454, 13)
        Me.Label5.TabIndex = 49
        Me.Label5.Text = "Copy Sql here for Document Ref( In SQL type column in same sequence as show in be"& _ 
    "low List)"
        '
        'txtSRPSQL
        '
        Me.txtSRPSQL.Location = New System.Drawing.Point(39, 57)
        Me.txtSRPSQL.Multiline = true
        Me.txtSRPSQL.Name = "txtSRPSQL"
        Me.txtSRPSQL.Size = New System.Drawing.Size(489, 88)
        Me.txtSRPSQL.TabIndex = 48
        '
        'btnReverseTrans
        '
        Me.btnReverseTrans.Location = New System.Drawing.Point(129, 391)
        Me.btnReverseTrans.Name = "btnReverseTrans"
        Me.btnReverseTrans.Size = New System.Drawing.Size(159, 33)
        Me.btnReverseTrans.TabIndex = 2
        Me.btnReverseTrans.Text = "Process Transaction"
        Me.btnReverseTrans.UseVisualStyleBackColor = true
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(36, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Document Ref"
        '
        'txtDocumentRef
        '
        Me.txtDocumentRef.Location = New System.Drawing.Point(129, 14)
        Me.txtDocumentRef.Name = "txtDocumentRef"
        Me.txtDocumentRef.Size = New System.Drawing.Size(150, 20)
        Me.txtDocumentRef.TabIndex = 0
        '
        'stbMain
        '
        Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MESSAGE, Me.COUNT})
        Me.stbMain.Location = New System.Drawing.Point(0, 617)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = true
        Me.stbMain.Size = New System.Drawing.Size(817, 22)
        Me.stbMain.TabIndex = 3
        '
        'MESSAGE
        '
        Me.MESSAGE.AutoSize = false
        Me.MESSAGE.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom),System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.MESSAGE.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.MESSAGE.DoubleClickEnabled = true
        Me.MESSAGE.Margin = New System.Windows.Forms.Padding(0)
        Me.MESSAGE.Name = "MESSAGE"
        Me.MESSAGE.Size = New System.Drawing.Size(702, 22)
        Me.MESSAGE.Text = "Ready"
        Me.MESSAGE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'COUNT
        '
        Me.COUNT.AutoSize = false
        Me.COUNT.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom),System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.COUNT.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.COUNT.DoubleClickEnabled = true
        Me.COUNT.Margin = New System.Windows.Forms.Padding(0)
        Me.COUNT.Name = "COUNT"
        Me.COUNT.Size = New System.Drawing.Size(96, 22)
        Me.COUNT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Ctx_mnuPopUp
        '
        Me.Ctx_mnuPopUp.Name = "Ctx_mnuPopUp"
        Me.Ctx_mnuPopUp.Size = New System.Drawing.Size(61, 4)
        '
        'lblPolicyStatus
        '
        Me.lblPolicyStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStatus.Enabled = false
        Me.lblPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPolicyStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStatus.Location = New System.Drawing.Point(826, 189)
        Me.lblPolicyStatus.Name = "lblPolicyStatus"
        Me.lblPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStatus.Size = New System.Drawing.Size(73, 19)
        Me.lblPolicyStatus.TabIndex = 42
        Me.lblPolicyStatus.Text = "Policy Status :"
        Me.lblPolicyStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPolicyStatus.Visible = false
        '
        'cboPolicyStatus
        '
        Me.cboPolicyStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPolicyStatus.Enabled = false
        Me.cboPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboPolicyStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyStatus.Location = New System.Drawing.Point(926, 187)
        Me.cboPolicyStatus.Name = "cboPolicyStatus"
        Me.cboPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyStatus.Size = New System.Drawing.Size(277, 21)
        Me.cboPolicyStatus.TabIndex = 41
        Me.cboPolicyStatus.Visible = false
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(0, 598)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 43
        '
        'txtPMNumber
        '
        Me.txtPMNumber.Location = New System.Drawing.Point(663, 12)
        Me.txtPMNumber.Name = "txtPMNumber"
        Me.txtPMNumber.Size = New System.Drawing.Size(140, 21)
        Me.txtPMNumber.TabIndex = 52
        '
        'Label6
        '
        Me.Label6.AutoSize = true
        Me.Label6.Location = New System.Drawing.Point(543, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(114, 13)
        Me.Label6.TabIndex = 53
        Me.Label6.Text = "IM/PM Ref Number"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(817, 639)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtPMNumber)
        Me.Controls.Add(Me.lblPolicyStatus)
        Me.Controls.Add(Me.cboPolicyStatus)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me._optSinglePolicy_2)
        Me.Controls.Add(Me.uctAnchor)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Fix Utility"
        Me.tabMain.ResumeLayout(false)
        Me._tabMain_TabPage1.ResumeLayout(false)
        Me.tabPolicyVersion.ResumeLayout(false)
        Me._tabPolicyVersion_TabPage0.ResumeLayout(false)
        Me._tabPolicyVersion_TabPage0.PerformLayout
        Me._tabPolicyVersion_TabPage1.ResumeLayout(false)
        Me._tabPolicyVersion_TabPage1.PerformLayout
        Me.fraRisk.ResumeLayout(false)
        Me._tabPolicyVersion_TabPage2.ResumeLayout(false)
        Me._tabPolicyVersion_TabPage2.PerformLayout
        Me.fraTransactionExport.ResumeLayout(false)
        Me.TabPage2.ResumeLayout(false)
        Me.TabPage2.PerformLayout
        Me._tabMain_TabPage2.ResumeLayout(false)
        Me.tabClaim.ResumeLayout(false)
        Me._tabClaim_TabPage0.ResumeLayout(false)
        Me.fraFailedClaimTransaction.ResumeLayout(false)
        Me._tabClaim_TabPage1.ResumeLayout(false)
        Me.fraImbalancedClosedClaim.ResumeLayout(false)
        Me._tabClaim_TabPage2.ResumeLayout(false)
        Me._tabClaim_TabPage2.PerformLayout
        Me.fraClaimPosting.ResumeLayout(false)
        Me._tabClaim_TabPage3.ResumeLayout(false)
        Me._tabClaim_TabPage3.PerformLayout
        Me.fraReserve.ResumeLayout(false)
        Me.fraPayment.ResumeLayout(false)
        Me._tabClaim_TabPage4.ResumeLayout(false)
        Me.fraClaimMisc.ResumeLayout(false)
        Me._tabMain_TabPage3.ResumeLayout(false)
        Me._tabMain_TabPage3.PerformLayout
        Me.TabPage1.ResumeLayout(false)
        Me.TabPage1.PerformLayout
        Me.stbMain.ResumeLayout(false)
        Me.stbMain.PerformLayout
        CType(Me.listViewHelper1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Sub InitializeoptSinglePolicy()
        'Me.optSinglePolicy(2) = _optSinglePolicy_2
        'Me.optSinglePolicy(2) = _optSinglePolicy_0
        'Me.optSinglePolicy(1) = _optSinglePolicy_1
        Me.optSinglePolicy(0) = _optSinglePolicy_3
        Me.optSinglePolicy(1) = _optSinglePolicy_4
        Me.optSinglePolicy(2) = _optSinglePolicy_0
    End Sub
    Sub InitializeoptReservePayment()
        Me.optReservePayment(1) = _optReservePayment_1
        Me.optReservePayment(0) = _optReservePayment_0
    End Sub
    Sub InitializeoptMiscellaneous()
        Me.optMiscellaneous(4) = _optMiscellaneous_4
        Me.optMiscellaneous(3) = _optMiscellaneous_3
        Me.optMiscellaneous(0) = _optMiscellaneous_0
        Me.optMiscellaneous(1) = _optMiscellaneous_1
        Me.optMiscellaneous(2) = _optMiscellaneous_2
        Me.optMiscellaneous(5) = _optMiscellaneous_5
    End Sub

    Sub InitializechkReserve()
        Me.chkReserve(1) = _chkReserve_1
        Me.chkReserve(0) = _chkReserve_0
    End Sub
    Friend WithEvents _optMiscellaneous_5 As System.Windows.Forms.RadioButton
    Private WithEvents _tabPolicyVersion_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lvwPolicyVersion As System.Windows.Forms.ListView
    Private WithEvents _lvwPolicyVersion_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _optSinglePolicy_4 As System.Windows.Forms.RadioButton
    Friend WithEvents _optSinglePolicy_3 As System.Windows.Forms.RadioButton
    Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Public WithEvents cmdGetPolicyVersion As System.Windows.Forms.Button
    Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Public WithEvents lblPolicyStatus As System.Windows.Forms.Label
    Public WithEvents cboPolicyStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtSqlQuery As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents _lvwPolicyVersion_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Friend WithEvents chkRIRefresh As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents btnReverseTrans As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDocumentRef As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents CmdGetClaimVersions As System.Windows.Forms.Button
    Public WithEvents LvwClaimVersion As System.Windows.Forms.ListView
    Private WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtSqlClaim As System.Windows.Forms.TextBox
    Public WithEvents CmdClaiimFix As System.Windows.Forms.Button
    Friend WithEvents OptReverseReg As System.Windows.Forms.RadioButton
    Friend WithEvents OptReverse As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSRPSQL As System.Windows.Forms.TextBox
    Public WithEvents btnGetSRPDetails As System.Windows.Forms.Button
    Public WithEvents lvwSRPDcouments As System.Windows.Forms.ListView
    Private WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkReverseAllocation As System.Windows.Forms.CheckBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents txtPMNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtClaimNo As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents ChkComm As CheckBox
    Friend WithEvents CmdSelectAllPolicy As Button
    Friend WithEvents ChkOutStanding As CheckBox
    Private WithEvents _optSinglePolicy_1 As RadioButton
#End Region
End Class