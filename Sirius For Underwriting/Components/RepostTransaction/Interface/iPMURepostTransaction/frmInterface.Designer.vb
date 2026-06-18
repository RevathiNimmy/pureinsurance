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
		InitializemnuPopUpItem()
		InitializechkReserve()
		tabPolicyVersionPreviousTab = tabPolicyVersion.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
	Private Sub Ctx_mnuPopUp_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuPopUp.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuPopUp.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuPopUp.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuPopUp.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuPopUp_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuPopUp.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuPopUp.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuPopUp.DropDownItems.Add(item)
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
	Private WithEvents _mnuPopUpItem_0 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPopUp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
	Public WithEvents chkRecreateStats As System.Windows.Forms.CheckBox
	Public WithEvents chkRecreateTransExport As System.Windows.Forms.CheckBox
	Public WithEvents fraMain1 As System.Windows.Forms.GroupBox
	Private WithEvents _lvwSelectPolicy_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectPolicy_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSelectPolicy As System.Windows.Forms.ListView
	Public WithEvents fraMain2 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents lblPolicyStatus As System.Windows.Forms.Label
	Private WithEvents _optSinglePolicy_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optSinglePolicy_0 As System.Windows.Forms.RadioButton
	Public WithEvents cmdGetPolicyVersion As System.Windows.Forms.Button
	Private WithEvents _lvwPolicyVersion_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicyVersion_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPolicyVersion As System.Windows.Forms.ListView
	Public WithEvents fraPolicyVersion As System.Windows.Forms.GroupBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents cboPolicyStatus As System.Windows.Forms.ComboBox
	Private WithEvents _optSinglePolicy_2 As System.Windows.Forms.RadioButton
	Private WithEvents _tabPolicyVersion_TabPage0 As System.Windows.Forms.TabPage
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
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
        Me.chkRecreateStats = New System.Windows.Forms.CheckBox()
        Me._optSinglePolicy_1 = New System.Windows.Forms.RadioButton()
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
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuPopUp = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuPopUpItem_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me.uctAnchor = New uSIRCommonControls.uctAnchor()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraMain1 = New System.Windows.Forms.GroupBox()
        Me.chkRecreateTransExport = New System.Windows.Forms.CheckBox()
        Me.fraMain2 = New System.Windows.Forms.GroupBox()
        Me.lvwSelectPolicy = New System.Windows.Forms.ListView()
        Me._lvwSelectPolicy_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSelectPolicy_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.tabPolicyVersion = New System.Windows.Forms.TabControl()
        Me._tabPolicyVersion_TabPage0 = New System.Windows.Forms.TabPage()
        Me._optSinglePolicy_4 = New System.Windows.Forms.RadioButton()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.lblPolicyStatus = New System.Windows.Forms.Label()
        Me.cmdGetPolicyVersion = New System.Windows.Forms.Button()
        Me.fraPolicyVersion = New System.Windows.Forms.GroupBox()
        Me.lvwPolicyVersion = New System.Windows.Forms.ListView()
        Me._lvwPolicyVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.cboPolicyStatus = New System.Windows.Forms.ComboBox()
        Me._tabPolicyVersion_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblInsuranceFileCnt = New System.Windows.Forms.Label()
        Me.fraRisk = New System.Windows.Forms.GroupBox()
        Me.lvwRisk = New System.Windows.Forms.ListView()
        Me._lvwRisk_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRisk_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtInsuranceFileCnt = New System.Windows.Forms.TextBox()
        Me._tabPolicyVersion_TabPage2 = New System.Windows.Forms.TabPage()
        Me.lblTransactionExportPolicyID = New System.Windows.Forms.Label()
        Me.txtTransactionExportPolicyID = New System.Windows.Forms.TextBox()
        Me.fraTransactionExport = New System.Windows.Forms.GroupBox()
        Me.lvwTransactionExport = New System.Windows.Forms.ListView()
        Me._lvwTransactionExport_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTransactionExport_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabMain_TabPage2 = New System.Windows.Forms.TabPage()
        Me.tabClaim = New System.Windows.Forms.TabControl()
        Me._tabClaim_TabPage0 = New System.Windows.Forms.TabPage()
        Me.chkDeleteStats = New System.Windows.Forms.CheckBox()
        Me.fraFailedClaimTransaction = New System.Windows.Forms.GroupBox()
        Me.lvwClaim = New System.Windows.Forms.ListView()
        Me._lvwClaim_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaim_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabClaim_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraImbalancedClosedClaim = New System.Windows.Forms.GroupBox()
        Me.lvwImbalancedClosedClaim = New System.Windows.Forms.ListView()
        Me._lvwImbalancedClosedClaim_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwImbalancedClosedClaim_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chkAutoProcess = New System.Windows.Forms.CheckBox()
        Me._tabClaim_TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCPClaimNumber = New System.Windows.Forms.TextBox()
        Me.fraClaimPosting = New System.Windows.Forms.GroupBox()
        Me.lvwClaimPosting = New System.Windows.Forms.ListView()
        Me._lvwClaimPosting_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClaimPosting_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._optReservePayment_0 = New System.Windows.Forms.RadioButton()
        Me._optReservePayment_1 = New System.Windows.Forms.RadioButton()
        Me._chkReserve_0 = New System.Windows.Forms.CheckBox()
        Me._chkReserve_1 = New System.Windows.Forms.CheckBox()
        Me._tabClaim_TabPage3 = New System.Windows.Forms.TabPage()
        Me.lblClaimNumber = New System.Windows.Forms.Label()
        Me.txtClaimNumber = New System.Windows.Forms.TextBox()
        Me.fraReserve = New System.Windows.Forms.GroupBox()
        Me.lvwReserve = New System.Windows.Forms.ListView()
        Me._lvwReserve_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwReserve_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraPayment = New System.Windows.Forms.GroupBox()
        Me.lvwPayment = New System.Windows.Forms.ListView()
        Me._lvwPayment_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPayment_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabClaim_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fraClaimMisc = New System.Windows.Forms.GroupBox()
        Me.lvwClaimMisc = New System.Windows.Forms.ListView()
        Me._tabMain_TabPage3 = New System.Windows.Forms.TabPage()
        Me._optMiscellaneous_5 = New System.Windows.Forms.RadioButton()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.stbMain = New System.Windows.Forms.StatusStrip()
        Me.MESSAGE = New System.Windows.Forms.ToolStripStatusLabel()
        Me.COUNT = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Ctx_mnuPopUp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.fraMain1.SuspendLayout()
        Me.fraMain2.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        Me.tabPolicyVersion.SuspendLayout()
        Me._tabPolicyVersion_TabPage0.SuspendLayout()
        Me.fraPolicyVersion.SuspendLayout()
        Me._tabPolicyVersion_TabPage1.SuspendLayout()
        Me.fraRisk.SuspendLayout()
        Me._tabPolicyVersion_TabPage2.SuspendLayout()
        Me.fraTransactionExport.SuspendLayout()
        Me._tabMain_TabPage2.SuspendLayout()
        Me.tabClaim.SuspendLayout()
        Me._tabClaim_TabPage0.SuspendLayout()
        Me.fraFailedClaimTransaction.SuspendLayout()
        Me._tabClaim_TabPage1.SuspendLayout()
        Me.fraImbalancedClosedClaim.SuspendLayout()
        Me._tabClaim_TabPage2.SuspendLayout()
        Me.fraClaimPosting.SuspendLayout()
        Me._tabClaim_TabPage3.SuspendLayout()
        Me.fraReserve.SuspendLayout()
        Me.fraPayment.SuspendLayout()
        Me._tabClaim_TabPage4.SuspendLayout()
        Me.fraClaimMisc.SuspendLayout()
        Me._tabMain_TabPage3.SuspendLayout()
        Me.stbMain.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkRecreateStats
        '
        Me.chkRecreateStats.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecreateStats.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecreateStats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecreateStats.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecreateStats.Location = New System.Drawing.Point(15, 16)
        Me.chkRecreateStats.Name = "chkRecreateStats"
        Me.chkRecreateStats.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecreateStats.Size = New System.Drawing.Size(196, 23)
        Me.chkRecreateStats.TabIndex = 0
        Me.chkRecreateStats.Text = "Recreate Stats Details"
        Me.ToolTip1.SetToolTip(Me.chkRecreateStats, "delete existing stats folder and detail and recreate them and repost")
        Me.chkRecreateStats.UseVisualStyleBackColor = False
        '
        '_optSinglePolicy_1
        '
        Me._optSinglePolicy_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_1.Location = New System.Drawing.Point(545, 32)
        Me._optSinglePolicy_1.Name = "_optSinglePolicy_1"
        Me._optSinglePolicy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_1.Size = New System.Drawing.Size(192, 17)
        Me._optSinglePolicy_1.TabIndex = 20
        Me._optSinglePolicy_1.TabStop = True
        Me._optSinglePolicy_1.Text = "Delete This Policy Version"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_1, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_1.UseVisualStyleBackColor = False
        '
        '_optSinglePolicy_0
        '
        Me._optSinglePolicy_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_0.Checked = True
        Me._optSinglePolicy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_0.Location = New System.Drawing.Point(545, 10)
        Me._optSinglePolicy_0.Name = "_optSinglePolicy_0"
        Me._optSinglePolicy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_0.Size = New System.Drawing.Size(169, 22)
        Me._optSinglePolicy_0.TabIndex = 21
        Me._optSinglePolicy_0.TabStop = True
        Me._optSinglePolicy_0.Text = "Repost Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_0, "Recreate stats folder, details and export folders and repost with option to delet" & _
        "e existing document and recalculate reinsurance")
        Me._optSinglePolicy_0.UseVisualStyleBackColor = False
        '
        '_optSinglePolicy_2
        '
        Me._optSinglePolicy_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_2.Location = New System.Drawing.Point(545, 49)
        Me._optSinglePolicy_2.Name = "_optSinglePolicy_2"
        Me._optSinglePolicy_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_2.Size = New System.Drawing.Size(185, 20)
        Me._optSinglePolicy_2.TabIndex = 40
        Me._optSinglePolicy_2.TabStop = True
        Me._optSinglePolicy_2.Text = "Set Policy Status"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_2, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_2.UseVisualStyleBackColor = False
        '
        'cmdRiskRefresh
        '
        Me.cmdRiskRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRiskRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRiskRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRiskRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRiskRefresh.Location = New System.Drawing.Point(171, 18)
        Me.cmdRiskRefresh.Name = "cmdRiskRefresh"
        Me.cmdRiskRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRiskRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdRiskRefresh.TabIndex = 28
        Me.cmdRiskRefresh.Text = "<>"
        Me.cmdRiskRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdRiskRefresh, "Refresh risk details")
        Me.cmdRiskRefresh.UseVisualStyleBackColor = False
        '
        'cmdTransactionExportRefresh
        '
        Me.cmdTransactionExportRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransactionExportRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransactionExportRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransactionExportRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransactionExportRefresh.Location = New System.Drawing.Point(171, 18)
        Me.cmdTransactionExportRefresh.Name = "cmdTransactionExportRefresh"
        Me.cmdTransactionExportRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransactionExportRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdTransactionExportRefresh.TabIndex = 33
        Me.cmdTransactionExportRefresh.Text = "<>"
        Me.cmdTransactionExportRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdTransactionExportRefresh, "Refresh risk details")
        Me.cmdTransactionExportRefresh.UseVisualStyleBackColor = False
        '
        'cmdCPRefresh
        '
        Me.cmdCPRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCPRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCPRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCPRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCPRefresh.Location = New System.Drawing.Point(342, 27)
        Me.cmdCPRefresh.Name = "cmdCPRefresh"
        Me.cmdCPRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCPRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdCPRefresh.TabIndex = 51
        Me.cmdCPRefresh.Text = "<>"
        Me.cmdCPRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCPRefresh, "Refresh reserve and payment details")
        Me.cmdCPRefresh.UseVisualStyleBackColor = False
        '
        'cmdReservePaymentRefresh
        '
        Me.cmdReservePaymentRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReservePaymentRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReservePaymentRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReservePaymentRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReservePaymentRefresh.Location = New System.Drawing.Point(342, 30)
        Me.cmdReservePaymentRefresh.Name = "cmdReservePaymentRefresh"
        Me.cmdReservePaymentRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReservePaymentRefresh.Size = New System.Drawing.Size(31, 22)
        Me.cmdReservePaymentRefresh.TabIndex = 48
        Me.cmdReservePaymentRefresh.Text = "<>"
        Me.cmdReservePaymentRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdReservePaymentRefresh, "Refresh reserve and payment details")
        Me.cmdReservePaymentRefresh.UseVisualStyleBackColor = False
        '
        '_optMiscellaneous_4
        '
        Me._optMiscellaneous_4.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMiscellaneous_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_4.Location = New System.Drawing.Point(24, 97)
        Me._optMiscellaneous_4.Name = "_optMiscellaneous_4"
        Me._optMiscellaneous_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_4.Size = New System.Drawing.Size(367, 19)
        Me._optMiscellaneous_4.TabIndex = 41
        Me._optMiscellaneous_4.TabStop = True
        Me._optMiscellaneous_4.Text = "Delete claim and all associated postings"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_4, "Delete claim and all associated postings including stats")
        Me._optMiscellaneous_4.UseVisualStyleBackColor = False
        '
        '_optMiscellaneous_3
        '
        Me._optMiscellaneous_3.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMiscellaneous_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_3.Location = New System.Drawing.Point(24, 79)
        Me._optMiscellaneous_3.Name = "_optMiscellaneous_3"
        Me._optMiscellaneous_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_3.Size = New System.Drawing.Size(367, 19)
        Me._optMiscellaneous_3.TabIndex = 37
        Me._optMiscellaneous_3.TabStop = True
        Me._optMiscellaneous_3.Text = "Add 100% Retained RI model to policy with no reinsurance"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_3, "Add reinsurance to supplied policy or policies with no reinsurance. Note only 100" & _
        "% retained model will work")
        Me._optMiscellaneous_3.UseVisualStyleBackColor = False
        '
        '_optMiscellaneous_0
        '
        Me._optMiscellaneous_0.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMiscellaneous_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_0.Location = New System.Drawing.Point(24, 25)
        Me._optMiscellaneous_0.Name = "_optMiscellaneous_0"
        Me._optMiscellaneous_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_0.Size = New System.Drawing.Size(220, 19)
        Me._optMiscellaneous_0.TabIndex = 11
        Me._optMiscellaneous_0.TabStop = True
        Me._optMiscellaneous_0.Text = "Delete Document From Account"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_0, "Delete document and its allocation details")
        Me._optMiscellaneous_0.UseVisualStyleBackColor = False
        '
        '_optMiscellaneous_1
        '
        Me._optMiscellaneous_1.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMiscellaneous_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_1.Location = New System.Drawing.Point(24, 43)
        Me._optMiscellaneous_1.Name = "_optMiscellaneous_1"
        Me._optMiscellaneous_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_1.Size = New System.Drawing.Size(220, 19)
        Me._optMiscellaneous_1.TabIndex = 10
        Me._optMiscellaneous_1.TabStop = True
        Me._optMiscellaneous_1.Text = "Delete Document's Allocation"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_1, "Delete all allocation details for this document")
        Me._optMiscellaneous_1.UseVisualStyleBackColor = False
        '
        '_optMiscellaneous_2
        '
        Me._optMiscellaneous_2.BackColor = System.Drawing.SystemColors.Control
        Me._optMiscellaneous_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMiscellaneous_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMiscellaneous_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMiscellaneous_2.Location = New System.Drawing.Point(24, 61)
        Me._optMiscellaneous_2.Name = "_optMiscellaneous_2"
        Me._optMiscellaneous_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMiscellaneous_2.Size = New System.Drawing.Size(307, 19)
        Me._optMiscellaneous_2.TabIndex = 9
        Me._optMiscellaneous_2.TabStop = True
        Me._optMiscellaneous_2.Text = "Repost This Revision/Payment For Closed Claims"
        Me.ToolTip1.SetToolTip(Me._optMiscellaneous_2, resources.GetString("_optMiscellaneous_2.ToolTip"))
        Me._optMiscellaneous_2.UseVisualStyleBackColor = False
        '
        '_optSinglePolicy_3
        '
        Me._optSinglePolicy_3.AutoSize = True
        Me._optSinglePolicy_3.Location = New System.Drawing.Point(545, 68)
        Me._optSinglePolicy_3.Name = "_optSinglePolicy_3"
        Me._optSinglePolicy_3.Size = New System.Drawing.Size(172, 17)
        Me._optSinglePolicy_3.TabIndex = 41
        Me._optSinglePolicy_3.TabStop = True
        Me._optSinglePolicy_3.Text = "Reverse Stats and Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_3, "Reverse the document ref.")
        Me._optSinglePolicy_3.UseVisualStyleBackColor = True
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopUp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(794, 24)
        Me.MainMenu1.TabIndex = 5
        '
        'mnuPopUp
        '
        Me.mnuPopUp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuPopUpItem_0})
        Me.mnuPopUp.Name = "mnuPopUp"
        Me.mnuPopUp.Size = New System.Drawing.Size(50, 20)
        Me.mnuPopUp.Text = "PopUp"
        '
        '_mnuPopUpItem_0
        '
        Me._mnuPopUpItem_0.Name = "_mnuPopUpItem_0"
        Me._mnuPopUpItem_0.Size = New System.Drawing.Size(82, 22)
        Me._mnuPopUpItem_0.Text = "--"
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(9, 525)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 1
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage2)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage3)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(195, 18)
        Me.tabMain.Location = New System.Drawing.Point(4, 28)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(790, 491)
        Me.tabMain.TabIndex = 4
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.fraMain1)
        Me._tabMain_TabPage0.Controls.Add(Me.fraMain2)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(782, 465)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Failed Transactions"
        '
        'fraMain1
        '
        Me.fraMain1.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain1.Controls.Add(Me.chkRecreateStats)
        Me.fraMain1.Controls.Add(Me.chkRecreateTransExport)
        Me.fraMain1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain1.Location = New System.Drawing.Point(6, 11)
        Me.fraMain1.Name = "fraMain1"
        Me.fraMain1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain1.Size = New System.Drawing.Size(775, 64)
        Me.fraMain1.TabIndex = 5
        Me.fraMain1.TabStop = False
        Me.fraMain1.Text = "Repost Attribute"
        '
        'chkRecreateTransExport
        '
        Me.chkRecreateTransExport.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecreateTransExport.Checked = True
        Me.chkRecreateTransExport.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRecreateTransExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecreateTransExport.Enabled = False
        Me.chkRecreateTransExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecreateTransExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecreateTransExport.Location = New System.Drawing.Point(15, 39)
        Me.chkRecreateTransExport.Name = "chkRecreateTransExport"
        Me.chkRecreateTransExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecreateTransExport.Size = New System.Drawing.Size(196, 19)
        Me.chkRecreateTransExport.TabIndex = 6
        Me.chkRecreateTransExport.Text = "Recreate Transaction Export"
        Me.chkRecreateTransExport.UseVisualStyleBackColor = False
        '
        'fraMain2
        '
        Me.fraMain2.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain2.Controls.Add(Me.lvwSelectPolicy)
        Me.fraMain2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMain2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain2.Location = New System.Drawing.Point(6, 80)
        Me.fraMain2.Name = "fraMain2"
        Me.fraMain2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain2.Size = New System.Drawing.Size(775, 382)
        Me.fraMain2.TabIndex = 7
        Me.fraMain2.TabStop = False
        Me.fraMain2.Text = "Policies To Repost"
        '
        'lvwSelectPolicy
        '
        Me.lvwSelectPolicy.AllowColumnReorder = True
        Me.lvwSelectPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSelectPolicy.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSelectPolicy, "")
        Me.lvwSelectPolicy.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSelectPolicy_ColumnHeader_1, Me._lvwSelectPolicy_ColumnHeader_2, Me._lvwSelectPolicy_ColumnHeader_3, Me._lvwSelectPolicy_ColumnHeader_4, Me._lvwSelectPolicy_ColumnHeader_5, Me._lvwSelectPolicy_ColumnHeader_6, Me._lvwSelectPolicy_ColumnHeader_7, Me._lvwSelectPolicy_ColumnHeader_8})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSelectPolicy, False)
        Me.lvwSelectPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSelectPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSelectPolicy.FullRowSelect = True
        Me.lvwSelectPolicy.GridLines = True
        Me.lvwSelectPolicy.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSelectPolicy, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSelectPolicy, "")
        Me.lvwSelectPolicy.Location = New System.Drawing.Point(4, 12)
        Me.lvwSelectPolicy.Name = "lvwSelectPolicy"
        Me.lvwSelectPolicy.Size = New System.Drawing.Size(769, 364)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSelectPolicy, "")
        Me.listViewHelper1.SetSorted(Me.lvwSelectPolicy, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSelectPolicy, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSelectPolicy, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSelectPolicy.TabIndex = 8
        Me.lvwSelectPolicy.UseCompatibleStateImageBehavior = False
        Me.lvwSelectPolicy.View = System.Windows.Forms.View.Details
        '
        '_lvwSelectPolicy_ColumnHeader_1
        '
        Me._lvwSelectPolicy_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwSelectPolicy_ColumnHeader_1.Text = "Export Folder Cnt"
        Me._lvwSelectPolicy_ColumnHeader_1.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_2
        '
        Me._lvwSelectPolicy_ColumnHeader_2.Tag = "VALUESORT"
        Me._lvwSelectPolicy_ColumnHeader_2.Text = "Insurance File Cnt"
        Me._lvwSelectPolicy_ColumnHeader_2.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_3
        '
        Me._lvwSelectPolicy_ColumnHeader_3.Text = "Doc Ref"
        Me._lvwSelectPolicy_ColumnHeader_3.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_4
        '
        Me._lvwSelectPolicy_ColumnHeader_4.Text = "Policy Ref"
        Me._lvwSelectPolicy_ColumnHeader_4.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_5
        '
        Me._lvwSelectPolicy_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwSelectPolicy_ColumnHeader_5.Text = "Policy Start"
        Me._lvwSelectPolicy_ColumnHeader_5.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_6
        '
        Me._lvwSelectPolicy_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwSelectPolicy_ColumnHeader_6.Text = "Policy End"
        Me._lvwSelectPolicy_ColumnHeader_6.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_7
        '
        Me._lvwSelectPolicy_ColumnHeader_7.Text = "Client"
        Me._lvwSelectPolicy_ColumnHeader_7.Width = 97
        '
        '_lvwSelectPolicy_ColumnHeader_8
        '
        Me._lvwSelectPolicy_ColumnHeader_8.Text = "Posting Status"
        Me._lvwSelectPolicy_ColumnHeader_8.Width = 97
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.tabPolicyVersion)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(782, 465)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "Single Policy"
        '
        'tabPolicyVersion
        '
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage0)
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage1)
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage2)
        Me.tabPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPolicyVersion.ItemSize = New System.Drawing.Size(259, 18)
        Me.tabPolicyVersion.Location = New System.Drawing.Point(3, 7)
        Me.tabPolicyVersion.Multiline = True
        Me.tabPolicyVersion.Name = "tabPolicyVersion"
        Me.tabPolicyVersion.SelectedIndex = 0
        Me.tabPolicyVersion.Size = New System.Drawing.Size(785, 461)
        Me.tabPolicyVersion.TabIndex = 19
        '
        '_tabPolicyVersion_TabPage0
        '
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_4)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_3)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lblPolicyNumber)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lblPolicyStatus)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_1)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_0)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdGetPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.fraPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.txtPolicyNumber)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cboPolicyStatus)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_2)
        Me._tabPolicyVersion_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage0.Name = "_tabPolicyVersion_TabPage0"
        Me._tabPolicyVersion_TabPage0.Size = New System.Drawing.Size(777, 435)
        Me._tabPolicyVersion_TabPage0.TabIndex = 0
        Me._tabPolicyVersion_TabPage0.Text = "Policy Version"
        '
        '_optSinglePolicy_4
        '
        Me._optSinglePolicy_4.AutoSize = True
        Me._optSinglePolicy_4.Location = New System.Drawing.Point(545, 85)
        Me._optSinglePolicy_4.Name = "_optSinglePolicy_4"
        Me._optSinglePolicy_4.Size = New System.Drawing.Size(204, 17)
        Me._optSinglePolicy_4.TabIndex = 42
        Me._optSinglePolicy_4.TabStop = True
        Me._optSinglePolicy_4.Text = "Reverse and Regenerate Transaction"
        Me._optSinglePolicy_4.UseVisualStyleBackColor = True
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(12, 16)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(103, 19)
        Me.lblPolicyNumber.TabIndex = 26
        Me.lblPolicyNumber.Text = "Policy Number :"
        Me.lblPolicyNumber.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPolicyStatus
        '
        Me.lblPolicyStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStatus.Enabled = False
        Me.lblPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStatus.Location = New System.Drawing.Point(12, 43)
        Me.lblPolicyStatus.Name = "lblPolicyStatus"
        Me.lblPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStatus.Size = New System.Drawing.Size(103, 19)
        Me.lblPolicyStatus.TabIndex = 39
        Me.lblPolicyStatus.Text = "Policy Status :"
        Me.lblPolicyStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdGetPolicyVersion
        '
        Me.cmdGetPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetPolicyVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGetPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetPolicyVersion.Location = New System.Drawing.Point(402, 13)
        Me.cmdGetPolicyVersion.Name = "cmdGetPolicyVersion"
        Me.cmdGetPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetPolicyVersion.Size = New System.Drawing.Size(118, 19)
        Me.cmdGetPolicyVersion.TabIndex = 22
        Me.cmdGetPolicyVersion.Text = "Get Policy Versions"
        Me.cmdGetPolicyVersion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetPolicyVersion.UseVisualStyleBackColor = False
        '
        'fraPolicyVersion
        '
        Me.fraPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicyVersion.Controls.Add(Me.lvwPolicyVersion)
        Me.fraPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicyVersion.Location = New System.Drawing.Point(6, 102)
        Me.fraPolicyVersion.Name = "fraPolicyVersion"
        Me.fraPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicyVersion.Size = New System.Drawing.Size(769, 329)
        Me.fraPolicyVersion.TabIndex = 23
        Me.fraPolicyVersion.TabStop = False
        Me.fraPolicyVersion.Text = "Policy Versions"
        '
        'lvwPolicyVersion
        '
        Me.lvwPolicyVersion.AllowColumnReorder = True
        Me.lvwPolicyVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicyVersion.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyVersion_ColumnHeader_1, Me._lvwPolicyVersion_ColumnHeader_2, Me._lvwPolicyVersion_ColumnHeader_3, Me._lvwPolicyVersion_ColumnHeader_4, Me._lvwPolicyVersion_ColumnHeader_5, Me._lvwPolicyVersion_ColumnHeader_6, Me._lvwPolicyVersion_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicyVersion, False)
        Me.lvwPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicyVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicyVersion.FullRowSelect = True
        Me.lvwPolicyVersion.GridLines = True
        Me.lvwPolicyVersion.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Location = New System.Drawing.Point(3, 19)
        Me.lvwPolicyVersion.Name = "lvwPolicyVersion"
        Me.lvwPolicyVersion.Size = New System.Drawing.Size(763, 342)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicyVersion, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicyVersion, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicyVersion, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicyVersion.TabIndex = 24
        Me.lvwPolicyVersion.UseCompatibleStateImageBehavior = False
        Me.lvwPolicyVersion.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicyVersion_ColumnHeader_1
        '
        Me._lvwPolicyVersion_ColumnHeader_1.Tag = "VALUESORT"
        Me._lvwPolicyVersion_ColumnHeader_1.Text = "Policy ID"
        Me._lvwPolicyVersion_ColumnHeader_1.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_2
        '
        Me._lvwPolicyVersion_ColumnHeader_2.Text = "Policy Ref"
        Me._lvwPolicyVersion_ColumnHeader_2.Width = 158
        '
        '_lvwPolicyVersion_ColumnHeader_3
        '
        Me._lvwPolicyVersion_ColumnHeader_3.Text = "Type"
        Me._lvwPolicyVersion_ColumnHeader_3.Width = 121
        '
        '_lvwPolicyVersion_ColumnHeader_4
        '
        Me._lvwPolicyVersion_ColumnHeader_4.Text = "Status"
        Me._lvwPolicyVersion_ColumnHeader_4.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_5
        '
        Me._lvwPolicyVersion_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_5.Text = "Start"
        Me._lvwPolicyVersion_ColumnHeader_5.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_6
        '
        Me._lvwPolicyVersion_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_6.Text = "End"
        Me._lvwPolicyVersion_ColumnHeader_6.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_7
        '
        Me._lvwPolicyVersion_ColumnHeader_7.Text = "Client"
        Me._lvwPolicyVersion_ColumnHeader_7.Width = 97
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(123, 13)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(277, 20)
        Me.txtPolicyNumber.TabIndex = 25
        '
        'cboPolicyStatus
        '
        Me.cboPolicyStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPolicyStatus.Enabled = False
        Me.cboPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyStatus.Location = New System.Drawing.Point(123, 40)
        Me.cboPolicyStatus.Name = "cboPolicyStatus"
        Me.cboPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyStatus.Size = New System.Drawing.Size(277, 21)
        Me.cboPolicyStatus.TabIndex = 38
        '
        '_tabPolicyVersion_TabPage1
        '
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.lblInsuranceFileCnt)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.fraRisk)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdRiskRefresh)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.txtInsuranceFileCnt)
        Me._tabPolicyVersion_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage1.Name = "_tabPolicyVersion_TabPage1"
        Me._tabPolicyVersion_TabPage1.Size = New System.Drawing.Size(777, 435)
        Me._tabPolicyVersion_TabPage1.TabIndex = 1
        Me._tabPolicyVersion_TabPage1.Text = "Risk"
        '
        'lblInsuranceFileCnt
        '
        Me.lblInsuranceFileCnt.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuranceFileCnt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuranceFileCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsuranceFileCnt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuranceFileCnt.Location = New System.Drawing.Point(12, 21)
        Me.lblInsuranceFileCnt.Name = "lblInsuranceFileCnt"
        Me.lblInsuranceFileCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuranceFileCnt.Size = New System.Drawing.Size(58, 13)
        Me.lblInsuranceFileCnt.TabIndex = 30
        Me.lblInsuranceFileCnt.Text = "Policy ID"
        '
        'fraRisk
        '
        Me.fraRisk.BackColor = System.Drawing.SystemColors.Control
        Me.fraRisk.Controls.Add(Me.lvwRisk)
        Me.fraRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRisk.Location = New System.Drawing.Point(6, 46)
        Me.fraRisk.Name = "fraRisk"
        Me.fraRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRisk.Size = New System.Drawing.Size(769, 379)
        Me.fraRisk.TabIndex = 27
        Me.fraRisk.TabStop = False
        Me.fraRisk.Text = "Risk Details"
        '
        'lvwRisk
        '
        Me.lvwRisk.AllowColumnReorder = True
        Me.lvwRisk.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRisk, "")
        Me.lvwRisk.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRisk_ColumnHeader_1, Me._lvwRisk_ColumnHeader_2, Me._lvwRisk_ColumnHeader_3, Me._lvwRisk_ColumnHeader_4, Me._lvwRisk_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRisk, False)
        Me.lvwRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRisk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRisk.FullRowSelect = True
        Me.lvwRisk.GridLines = True
        Me.lvwRisk.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRisk, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRisk, "")
        Me.lvwRisk.Location = New System.Drawing.Point(3, 15)
        Me.lvwRisk.Name = "lvwRisk"
        Me.lvwRisk.Size = New System.Drawing.Size(760, 358)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRisk, "")
        Me.listViewHelper1.SetSorted(Me.lvwRisk, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRisk, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRisk, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRisk.TabIndex = 31
        Me.lvwRisk.UseCompatibleStateImageBehavior = False
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
        Me.txtInsuranceFileCnt.AcceptsReturn = True
        Me.txtInsuranceFileCnt.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuranceFileCnt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuranceFileCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me._tabPolicyVersion_TabPage2.Size = New System.Drawing.Size(777, 435)
        Me._tabPolicyVersion_TabPage2.TabIndex = 2
        Me._tabPolicyVersion_TabPage2.Text = "Transaction"
        '
        'lblTransactionExportPolicyID
        '
        Me.lblTransactionExportPolicyID.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionExportPolicyID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionExportPolicyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtTransactionExportPolicyID.AcceptsReturn = True
        Me.txtTransactionExportPolicyID.BackColor = System.Drawing.SystemColors.Window
        Me.txtTransactionExportPolicyID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTransactionExportPolicyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.fraTransactionExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTransactionExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTransactionExport.Location = New System.Drawing.Point(6, 46)
        Me.fraTransactionExport.Name = "fraTransactionExport"
        Me.fraTransactionExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTransactionExport.Size = New System.Drawing.Size(769, 388)
        Me.fraTransactionExport.TabIndex = 34
        Me.fraTransactionExport.TabStop = False
        Me.fraTransactionExport.Text = "Risk Details"
        '
        'lvwTransactionExport
        '
        Me.lvwTransactionExport.AllowColumnReorder = True
        Me.lvwTransactionExport.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTransactionExport, "")
        Me.lvwTransactionExport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTransactionExport_ColumnHeader_1, Me._lvwTransactionExport_ColumnHeader_2, Me._lvwTransactionExport_ColumnHeader_3, Me._lvwTransactionExport_ColumnHeader_4, Me._lvwTransactionExport_ColumnHeader_5, Me._lvwTransactionExport_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTransactionExport, False)
        Me.lvwTransactionExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTransactionExport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTransactionExport.FullRowSelect = True
        Me.lvwTransactionExport.GridLines = True
        Me.lvwTransactionExport.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTransactionExport, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTransactionExport, "")
        Me.lvwTransactionExport.Location = New System.Drawing.Point(3, 15)
        Me.lvwTransactionExport.Name = "lvwTransactionExport"
        Me.lvwTransactionExport.Size = New System.Drawing.Size(760, 370)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTransactionExport, "")
        Me.listViewHelper1.SetSorted(Me.lvwTransactionExport, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTransactionExport, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTransactionExport, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTransactionExport.TabIndex = 35
        Me.lvwTransactionExport.UseCompatibleStateImageBehavior = False
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
        '_tabMain_TabPage2
        '
        Me._tabMain_TabPage2.Controls.Add(Me.tabClaim)
        Me._tabMain_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage2.Name = "_tabMain_TabPage2"
        Me._tabMain_TabPage2.Size = New System.Drawing.Size(782, 465)
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
        Me.tabClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabClaim.ItemSize = New System.Drawing.Size(154, 18)
        Me.tabClaim.Location = New System.Drawing.Point(3, 10)
        Me.tabClaim.Multiline = True
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
        Me.chkDeleteStats.Checked = True
        Me.chkDeleteStats.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDeleteStats.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDeleteStats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDeleteStats.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDeleteStats.Location = New System.Drawing.Point(3, 44)
        Me.chkDeleteStats.Name = "chkDeleteStats"
        Me.chkDeleteStats.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDeleteStats.Size = New System.Drawing.Size(304, 16)
        Me.chkDeleteStats.TabIndex = 13
        Me.chkDeleteStats.Text = "Delete Existing Stats For Selected Documents"
        Me.chkDeleteStats.UseVisualStyleBackColor = False
        '
        'fraFailedClaimTransaction
        '
        Me.fraFailedClaimTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.fraFailedClaimTransaction.Controls.Add(Me.lvwClaim)
        Me.fraFailedClaimTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFailedClaimTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFailedClaimTransaction.Location = New System.Drawing.Point(3, 62)
        Me.fraFailedClaimTransaction.Name = "fraFailedClaimTransaction"
        Me.fraFailedClaimTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFailedClaimTransaction.Size = New System.Drawing.Size(772, 403)
        Me.fraFailedClaimTransaction.TabIndex = 14
        Me.fraFailedClaimTransaction.TabStop = False
        Me.fraFailedClaimTransaction.Text = "Failed Claim Transaction"
        '
        'lvwClaim
        '
        Me.lvwClaim.AllowColumnReorder = True
        Me.lvwClaim.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaim.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaim, "")
        Me.lvwClaim.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaim_ColumnHeader_1, Me._lvwClaim_ColumnHeader_2, Me._lvwClaim_ColumnHeader_3, Me._lvwClaim_ColumnHeader_4, Me._lvwClaim_ColumnHeader_5, Me._lvwClaim_ColumnHeader_6, Me._lvwClaim_ColumnHeader_7, Me._lvwClaim_ColumnHeader_8, Me._lvwClaim_ColumnHeader_9, Me._lvwClaim_ColumnHeader_10, Me._lvwClaim_ColumnHeader_11, Me._lvwClaim_ColumnHeader_12, Me._lvwClaim_ColumnHeader_13, Me._lvwClaim_ColumnHeader_14})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaim, False)
        Me.lvwClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaim.FullRowSelect = True
        Me.lvwClaim.GridLines = True
        Me.lvwClaim.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaim, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaim, "")
        Me.lvwClaim.Location = New System.Drawing.Point(3, 12)
        Me.lvwClaim.Name = "lvwClaim"
        Me.lvwClaim.Size = New System.Drawing.Size(766, 388)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaim, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaim, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClaim, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaim, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaim.TabIndex = 15
        Me.lvwClaim.UseCompatibleStateImageBehavior = False
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
        Me.fraImbalancedClosedClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraImbalancedClosedClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImbalancedClosedClaim.Location = New System.Drawing.Point(6, 80)
        Me.fraImbalancedClosedClaim.Name = "fraImbalancedClosedClaim"
        Me.fraImbalancedClosedClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImbalancedClosedClaim.Size = New System.Drawing.Size(766, 385)
        Me.fraImbalancedClosedClaim.TabIndex = 16
        Me.fraImbalancedClosedClaim.TabStop = False
        Me.fraImbalancedClosedClaim.Text = "Imbalanced Closed Claims"
        '
        'lvwImbalancedClosedClaim
        '
        Me.lvwImbalancedClosedClaim.AllowColumnReorder = True
        Me.lvwImbalancedClosedClaim.BackColor = System.Drawing.SystemColors.Window
        Me.lvwImbalancedClosedClaim.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwImbalancedClosedClaim, "")
        Me.lvwImbalancedClosedClaim.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwImbalancedClosedClaim_ColumnHeader_1, Me._lvwImbalancedClosedClaim_ColumnHeader_2, Me._lvwImbalancedClosedClaim_ColumnHeader_3, Me._lvwImbalancedClosedClaim_ColumnHeader_4, Me._lvwImbalancedClosedClaim_ColumnHeader_5, Me._lvwImbalancedClosedClaim_ColumnHeader_6, Me._lvwImbalancedClosedClaim_ColumnHeader_7, Me._lvwImbalancedClosedClaim_ColumnHeader_8, Me._lvwImbalancedClosedClaim_ColumnHeader_9, Me._lvwImbalancedClosedClaim_ColumnHeader_10, Me._lvwImbalancedClosedClaim_ColumnHeader_11})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwImbalancedClosedClaim, False)
        Me.lvwImbalancedClosedClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwImbalancedClosedClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwImbalancedClosedClaim.FullRowSelect = True
        Me.lvwImbalancedClosedClaim.GridLines = True
        Me.lvwImbalancedClosedClaim.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwImbalancedClosedClaim, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwImbalancedClosedClaim, "")
        Me.lvwImbalancedClosedClaim.Location = New System.Drawing.Point(3, 15)
        Me.lvwImbalancedClosedClaim.Name = "lvwImbalancedClosedClaim"
        Me.lvwImbalancedClosedClaim.Size = New System.Drawing.Size(760, 367)
        Me.listViewHelper1.SetSmallIcons(Me.lvwImbalancedClosedClaim, "")
        Me.listViewHelper1.SetSorted(Me.lvwImbalancedClosedClaim, False)
        Me.listViewHelper1.SetSortKey(Me.lvwImbalancedClosedClaim, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwImbalancedClosedClaim, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwImbalancedClosedClaim.TabIndex = 17
        Me.lvwImbalancedClosedClaim.UseCompatibleStateImageBehavior = False
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
        Me.chkAutoProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoProcess.Location = New System.Drawing.Point(6, 53)
        Me.chkAutoProcess.Name = "chkAutoProcess"
        Me.chkAutoProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoProcess.Size = New System.Drawing.Size(103, 16)
        Me.chkAutoProcess.TabIndex = 18
        Me.chkAutoProcess.Text = "Auto Process"
        Me.chkAutoProcess.UseVisualStyleBackColor = False
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
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtCPClaimNumber.AcceptsReturn = True
        Me.txtCPClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCPClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCPClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.fraClaimPosting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimPosting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimPosting.Location = New System.Drawing.Point(3, 99)
        Me.fraClaimPosting.Name = "fraClaimPosting"
        Me.fraClaimPosting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimPosting.Size = New System.Drawing.Size(772, 328)
        Me.fraClaimPosting.TabIndex = 52
        Me.fraClaimPosting.TabStop = False
        Me.fraClaimPosting.Text = "Claim Postings"
        '
        'lvwClaimPosting
        '
        Me.lvwClaimPosting.AllowColumnReorder = True
        Me.lvwClaimPosting.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimPosting.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaimPosting, "")
        Me.lvwClaimPosting.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaimPosting_ColumnHeader_1, Me._lvwClaimPosting_ColumnHeader_2, Me._lvwClaimPosting_ColumnHeader_3, Me._lvwClaimPosting_ColumnHeader_4, Me._lvwClaimPosting_ColumnHeader_5, Me._lvwClaimPosting_ColumnHeader_6, Me._lvwClaimPosting_ColumnHeader_7})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaimPosting, False)
        Me.lvwClaimPosting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaimPosting.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimPosting.FullRowSelect = True
        Me.lvwClaimPosting.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaimPosting, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaimPosting, "")
        Me.lvwClaimPosting.Location = New System.Drawing.Point(3, 15)
        Me.lvwClaimPosting.Name = "lvwClaimPosting"
        Me.lvwClaimPosting.Size = New System.Drawing.Size(766, 310)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaimPosting, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaimPosting, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClaimPosting, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaimPosting, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaimPosting.TabIndex = 53
        Me.lvwClaimPosting.UseCompatibleStateImageBehavior = False
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
        Me._optReservePayment_0.Checked = True
        Me._optReservePayment_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReservePayment_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReservePayment_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optReservePayment_0.Location = New System.Drawing.Point(606, 23)
        Me._optReservePayment_0.Name = "_optReservePayment_0"
        Me._optReservePayment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReservePayment_0.Size = New System.Drawing.Size(163, 17)
        Me._optReservePayment_0.TabIndex = 54
        Me._optReservePayment_0.TabStop = True
        Me._optReservePayment_0.Text = "Add Reserve Posting"
        Me._optReservePayment_0.UseVisualStyleBackColor = False
        '
        '_optReservePayment_1
        '
        Me._optReservePayment_1.BackColor = System.Drawing.SystemColors.Control
        Me._optReservePayment_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReservePayment_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReservePayment_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optReservePayment_1.Location = New System.Drawing.Point(606, 78)
        Me._optReservePayment_1.Name = "_optReservePayment_1"
        Me._optReservePayment_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReservePayment_1.Size = New System.Drawing.Size(163, 30)
        Me._optReservePayment_1.TabIndex = 55
        Me._optReservePayment_1.TabStop = True
        Me._optReservePayment_1.Text = "Add Payment Posting"
        Me._optReservePayment_1.UseVisualStyleBackColor = False
        '
        '_chkReserve_0
        '
        Me._chkReserve_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkReserve_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkReserve_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkReserve_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkReserve_0.Location = New System.Drawing.Point(624, 34)
        Me._chkReserve_0.Name = "_chkReserve_0"
        Me._chkReserve_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkReserve_0.Size = New System.Drawing.Size(112, 31)
        Me._chkReserve_0.TabIndex = 58
        Me._chkReserve_0.Text = "Initial Reserve"
        Me._chkReserve_0.UseVisualStyleBackColor = False
        '
        '_chkReserve_1
        '
        Me._chkReserve_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkReserve_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkReserve_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkReserve_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkReserve_1.Location = New System.Drawing.Point(624, 53)
        Me._chkReserve_1.Name = "_chkReserve_1"
        Me._chkReserve_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkReserve_1.Size = New System.Drawing.Size(112, 36)
        Me._chkReserve_1.TabIndex = 59
        Me._chkReserve_1.Text = "Revise Reserve"
        Me._chkReserve_1.UseVisualStyleBackColor = False
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
        Me.lblClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.fraReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReserve.Location = New System.Drawing.Point(0, 66)
        Me.fraReserve.Name = "fraReserve"
        Me.fraReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReserve.Size = New System.Drawing.Size(766, 220)
        Me.fraReserve.TabIndex = 43
        Me.fraReserve.TabStop = False
        Me.fraReserve.Text = "Reserve"
        '
        'lvwReserve
        '
        Me.lvwReserve.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwReserve, "")
        Me.lvwReserve.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwReserve_ColumnHeader_1, Me._lvwReserve_ColumnHeader_2, Me._lvwReserve_ColumnHeader_3, Me._lvwReserve_ColumnHeader_4, Me._lvwReserve_ColumnHeader_5, Me._lvwReserve_ColumnHeader_6, Me._lvwReserve_ColumnHeader_7, Me._lvwReserve_ColumnHeader_8, Me._lvwReserve_ColumnHeader_9, Me._lvwReserve_ColumnHeader_10})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReserve, False)
        Me.lvwReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReserve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReserve.FullRowSelect = True
        Me.lvwReserve.GridLines = True
        Me.lvwReserve.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwReserve, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwReserve, "")
        Me.lvwReserve.Location = New System.Drawing.Point(3, 15)
        Me.lvwReserve.Name = "lvwReserve"
        Me.lvwReserve.Size = New System.Drawing.Size(757, 202)
        Me.listViewHelper1.SetSmallIcons(Me.lvwReserve, "")
        Me.listViewHelper1.SetSorted(Me.lvwReserve, False)
        Me.listViewHelper1.SetSortKey(Me.lvwReserve, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwReserve, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwReserve.TabIndex = 44
        Me.lvwReserve.UseCompatibleStateImageBehavior = False
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
        Me.fraPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayment.Location = New System.Drawing.Point(0, 288)
        Me.fraPayment.Name = "fraPayment"
        Me.fraPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayment.Size = New System.Drawing.Size(766, 160)
        Me.fraPayment.TabIndex = 45
        Me.fraPayment.TabStop = False
        Me.fraPayment.Text = "Payment"
        '
        'lvwPayment
        '
        Me.lvwPayment.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPayment, "")
        Me.lvwPayment.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPayment_ColumnHeader_1, Me._lvwPayment_ColumnHeader_2, Me._lvwPayment_ColumnHeader_3, Me._lvwPayment_ColumnHeader_4, Me._lvwPayment_ColumnHeader_5, Me._lvwPayment_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPayment, False)
        Me.lvwPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPayment.FullRowSelect = True
        Me.lvwPayment.GridLines = True
        Me.lvwPayment.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPayment, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPayment, "")
        Me.lvwPayment.Location = New System.Drawing.Point(3, 15)
        Me.lvwPayment.Name = "lvwPayment"
        Me.lvwPayment.Size = New System.Drawing.Size(757, 142)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPayment, "")
        Me.listViewHelper1.SetSorted(Me.lvwPayment, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPayment, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPayment, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPayment.TabIndex = 46
        Me.lvwPayment.UseCompatibleStateImageBehavior = False
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
        Me.fraClaimMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimMisc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimMisc.Location = New System.Drawing.Point(6, 1)
        Me.fraClaimMisc.Name = "fraClaimMisc"
        Me.fraClaimMisc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimMisc.Size = New System.Drawing.Size(766, 424)
        Me.fraClaimMisc.TabIndex = 56
        Me.fraClaimMisc.TabStop = False
        Me.fraClaimMisc.Text = "Claim Misc"
        '
        'lvwClaimMisc
        '
        Me.lvwClaimMisc.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaimMisc.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaimMisc, False)
        Me.lvwClaimMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaimMisc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaimMisc.FullRowSelect = True
        Me.lvwClaimMisc.GridLines = True
        Me.lvwClaimMisc.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaimMisc, "")
        Me.lvwClaimMisc.Location = New System.Drawing.Point(3, 15)
        Me.lvwClaimMisc.Name = "lvwClaimMisc"
        Me.lvwClaimMisc.Size = New System.Drawing.Size(760, 406)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaimMisc, "")
        Me.listViewHelper1.SetSorted(Me.lvwClaimMisc, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClaimMisc, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaimMisc, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaimMisc.TabIndex = 57
        Me.lvwClaimMisc.UseCompatibleStateImageBehavior = False
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
        Me._tabMain_TabPage3.Size = New System.Drawing.Size(782, 465)
        Me._tabMain_TabPage3.TabIndex = 3
        Me._tabMain_TabPage3.Text = "Miscellaneous"
        '
        '_optMiscellaneous_5
        '
        Me._optMiscellaneous_5.AutoSize = True
        Me._optMiscellaneous_5.Location = New System.Drawing.Point(24, 122)
        Me._optMiscellaneous_5.Name = "_optMiscellaneous_5"
        Me._optMiscellaneous_5.Size = New System.Drawing.Size(170, 17)
        Me._optMiscellaneous_5.TabIndex = 42
        Me._optMiscellaneous_5.TabStop = True
        Me._optMiscellaneous_5.Text = "Reverse Document Reference"
        Me._optMiscellaneous_5.UseVisualStyleBackColor = True
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(722, 520)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(70, 22)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(648, 520)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(70, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbMain
        '
        Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MESSAGE, Me.COUNT})
        Me.stbMain.Location = New System.Drawing.Point(0, 544)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = True
        Me.stbMain.Size = New System.Drawing.Size(794, 22)
        Me.stbMain.TabIndex = 3
        '
        'MESSAGE
        '
        Me.MESSAGE.AutoSize = False
        Me.MESSAGE.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.MESSAGE.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.MESSAGE.DoubleClickEnabled = True
        Me.MESSAGE.Margin = New System.Windows.Forms.Padding(0)
        Me.MESSAGE.Name = "MESSAGE"
        Me.MESSAGE.Size = New System.Drawing.Size(702, 22)
        Me.MESSAGE.Text = "Ready"
        Me.MESSAGE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'COUNT
        '
        Me.COUNT.AutoSize = False
        Me.COUNT.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.COUNT.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.COUNT.DoubleClickEnabled = True
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
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(794, 566)
        Me.Controls.Add(Me.uctAnchor)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Repost Transaction"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me.fraMain1.ResumeLayout(False)
        Me.fraMain2.ResumeLayout(False)
        Me._tabMain_TabPage1.ResumeLayout(False)
        Me.tabPolicyVersion.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.PerformLayout()
        Me.fraPolicyVersion.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage1.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage1.PerformLayout()
        Me.fraRisk.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage2.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage2.PerformLayout()
        Me.fraTransactionExport.ResumeLayout(False)
        Me._tabMain_TabPage2.ResumeLayout(False)
        Me.tabClaim.ResumeLayout(False)
        Me._tabClaim_TabPage0.ResumeLayout(False)
        Me.fraFailedClaimTransaction.ResumeLayout(False)
        Me._tabClaim_TabPage1.ResumeLayout(False)
        Me.fraImbalancedClosedClaim.ResumeLayout(False)
        Me._tabClaim_TabPage2.ResumeLayout(False)
        Me._tabClaim_TabPage2.PerformLayout()
        Me.fraClaimPosting.ResumeLayout(False)
        Me._tabClaim_TabPage3.ResumeLayout(False)
        Me._tabClaim_TabPage3.PerformLayout()
        Me.fraReserve.ResumeLayout(False)
        Me.fraPayment.ResumeLayout(False)
        Me._tabClaim_TabPage4.ResumeLayout(False)
        Me.fraClaimMisc.ResumeLayout(False)
        Me._tabMain_TabPage3.ResumeLayout(False)
        Me._tabMain_TabPage3.PerformLayout()
        Me.stbMain.ResumeLayout(False)
        Me.stbMain.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializeoptSinglePolicy()
		Me.optSinglePolicy(2) = _optSinglePolicy_2
		Me.optSinglePolicy(0) = _optSinglePolicy_0
        Me.optSinglePolicy(1) = _optSinglePolicy_1
        Me.optSinglePolicy(3) = _optSinglePolicy_3
        Me.optSinglePolicy(4) = _optSinglePolicy_4
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
	Sub InitializemnuPopUpItem()
		Me.mnuPopUpItem(0) = _mnuPopUpItem_0
	End Sub
	Sub InitializechkReserve()
		Me.chkReserve(1) = _chkReserve_1
		Me.chkReserve(0) = _chkReserve_0
    End Sub
    Friend WithEvents _optSinglePolicy_3 As System.Windows.Forms.RadioButton
    Friend WithEvents _optSinglePolicy_4 As System.Windows.Forms.RadioButton
    Friend WithEvents _optMiscellaneous_5 As System.Windows.Forms.RadioButton
#End Region 
End Class