<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBanking
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtTotalFloat()
		InitializetxtTotalCash()
		InitializetxtTotalCC()
		InitializetxtFloatRem()
		InitializetdgFloat()
		InitializetdgCash()
		InitializelblTotalCC()
		InitializelblFloatTot()
		InitializelblFloatRem()
		InitializelblConfirm()
		InitializelblCashTot()
		InitializefraFloat()
		InitializefraCash()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Private WithEvents _txtTotalCash_0 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_0 As System.Windows.Forms.Label
	Private WithEvents _fraCash_0 As System.Windows.Forms.GroupBox
	Private WithEvents _txtFloatRem_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_0 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_0 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_0 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_0 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_0 As System.Windows.Forms.TextBox
	Private WithEvents _lblTotalCC_0 As System.Windows.Forms.Label
	Private WithEvents _tabMediaTypes_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalFloat_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_1 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_1 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_1 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_1 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_1 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_1 As System.Windows.Forms.Label
	Private WithEvents _fraCash_1 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_1 As System.Windows.Forms.TextBox
	Private WithEvents _lblTotalCC_1 As System.Windows.Forms.Label
	Private WithEvents _tabMediaTypes_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _lblTotalCC_2 As System.Windows.Forms.Label
	Private WithEvents _txtTotalCC_2 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_2 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_2 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_2 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_2 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_2 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_2 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_2 As System.Windows.Forms.Label
	Private WithEvents _fraCash_2 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_3 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_3 As System.Windows.Forms.Label
	Private WithEvents _fraCash_3 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalFloat_3 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_3 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_3 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_3 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_3 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_3 As System.Windows.Forms.TextBox
	Private WithEvents _lblTotalCC_3 As System.Windows.Forms.Label
	Private WithEvents _tabMediaTypes_TabPage3 As System.Windows.Forms.TabPage
	Private WithEvents _lblTotalCC_4 As System.Windows.Forms.Label
	Private WithEvents _txtTotalCC_4 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_4 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_4 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_4 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_4 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_4 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_4 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_4 As System.Windows.Forms.Label
	Private WithEvents _fraCash_4 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage4 As System.Windows.Forms.TabPage
	Private WithEvents _lblTotalCC_5 As System.Windows.Forms.Label
	Private WithEvents _txtTotalCC_5 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_5 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_5 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_5 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_5 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_5 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_5 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_5 As System.Windows.Forms.Label
	Private WithEvents _fraCash_5 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage5 As System.Windows.Forms.TabPage
	Private WithEvents _lblTotalCC_6 As System.Windows.Forms.Label
	Private WithEvents _txtTotalCC_6 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_6 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_6 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_6 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_6 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_6 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_6 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_6 As System.Windows.Forms.Label
	Private WithEvents _fraCash_6 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage6 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_7 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_7 As System.Windows.Forms.Label
	Private WithEvents _fraCash_7 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalFloat_7 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_7 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_7 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_7 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_7 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_7 As System.Windows.Forms.TextBox
	Private WithEvents _lblTotalCC_7 As System.Windows.Forms.Label
	Private WithEvents _tabMediaTypes_TabPage7 As System.Windows.Forms.TabPage
	Private WithEvents _lblTotalCC_8 As System.Windows.Forms.Label
	Private WithEvents _txtTotalCC_8 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_8 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_8 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_8 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_8 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_8 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_8 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_8 As System.Windows.Forms.Label
	Private WithEvents _fraCash_8 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage8 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_9 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_9 As System.Windows.Forms.Label
	Private WithEvents _fraCash_9 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalFloat_9 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_9 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatTot_9 As System.Windows.Forms.Label
	Private WithEvents _lblFloatRem_9 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_9 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_9 As System.Windows.Forms.TextBox
	Private WithEvents _lblTotalCC_9 As System.Windows.Forms.Label
	Private WithEvents _tabMediaTypes_TabPage9 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_10 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_10 As System.Windows.Forms.Label
	Private WithEvents _fraCash_10 As System.Windows.Forms.GroupBox
	Private WithEvents _txtFloatRem_10 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_10 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_10 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_10 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_10 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_10 As System.Windows.Forms.TextBox
	Private WithEvents _tabMediaTypes_TabPage10 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_11 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_11 As System.Windows.Forms.Label
	Private WithEvents _fraCash_11 As System.Windows.Forms.GroupBox
	Private WithEvents _txtFloatRem_11 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_11 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_11 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_11 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_11 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_11 As System.Windows.Forms.TextBox
	Private WithEvents _tabMediaTypes_TabPage11 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCC_12 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_12 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_12 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_12 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_12 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_12 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_12 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_12 As System.Windows.Forms.Label
	Private WithEvents _fraCash_12 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage12 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_13 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_13 As System.Windows.Forms.Label
	Private WithEvents _fraCash_13 As System.Windows.Forms.GroupBox
	Private WithEvents _txtFloatRem_13 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_13 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_13 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_13 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_13 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_13 As System.Windows.Forms.TextBox
	Private WithEvents _tabMediaTypes_TabPage13 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCC_14 As System.Windows.Forms.TextBox
	Private WithEvents _txtFloatRem_14 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_14 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_14 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_14 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_14 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCash_14 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_14 As System.Windows.Forms.Label
	Private WithEvents _fraCash_14 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMediaTypes_TabPage14 As System.Windows.Forms.TabPage
	Private WithEvents _txtTotalCash_15 As System.Windows.Forms.TextBox
	Private WithEvents _lblCashTot_15 As System.Windows.Forms.Label
	Private WithEvents _fraCash_15 As System.Windows.Forms.GroupBox
	Private WithEvents _txtFloatRem_15 As System.Windows.Forms.TextBox
	Private WithEvents _txtTotalFloat_15 As System.Windows.Forms.TextBox
	Private WithEvents _lblFloatRem_15 As System.Windows.Forms.Label
	Private WithEvents _lblFloatTot_15 As System.Windows.Forms.Label
	Private WithEvents _fraFloat_15 As System.Windows.Forms.GroupBox
	Private WithEvents _txtTotalCC_15 As System.Windows.Forms.TextBox
	Private WithEvents _tabMediaTypes_TabPage15 As System.Windows.Forms.TabPage
	Public WithEvents tabMediaTypes As System.Windows.Forms.TabControl
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _lblConfirm_0 As System.Windows.Forms.Label
	Public WithEvents txtFloat As System.Windows.Forms.TextBox
	Public WithEvents txtBalance As System.Windows.Forms.TextBox
	Public WithEvents txtAdjustments As System.Windows.Forms.TextBox
	Public WithEvents txtSubTotal As System.Windows.Forms.TextBox
	Public WithEvents txtCCTotal As System.Windows.Forms.TextBox
	Public WithEvents txtBankingTotal As System.Windows.Forms.TextBox
	Public WithEvents cmdAdjustment As System.Windows.Forms.Button
	Public WithEvents cmdReverse As System.Windows.Forms.Button
	Public WithEvents txtTotalReceipts As System.Windows.Forms.TextBox
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_8 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_7 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_6 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_5 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_4 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_3 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_2 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_1 As System.Windows.Forms.Label
	Public WithEvents fraTotals As System.Windows.Forms.GroupBox
	Public WithEvents txtDepositDate As System.Windows.Forms.TextBox
	Public WithEvents cboPMUserAuthorise1 As PMUserLookupControl.cboPMUserLookup
	Public WithEvents txtDateApproved As System.Windows.Forms.TextBox
	Public WithEvents cboPMUserAuthorise2 As PMUserLookupControl.cboPMUserLookup
	Private WithEvents _lblConfirm_11 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_10 As System.Windows.Forms.Label
	Private WithEvents _lblConfirm_9 As System.Windows.Forms.Label
	Public WithEvents fraAuth As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdApprove As System.Windows.Forms.Button
	Public WithEvents cmdViewAdj As System.Windows.Forms.Button
	Public fraCash(15) As System.Windows.Forms.GroupBox
	Public fraFloat(15) As System.Windows.Forms.GroupBox
	Public lblCashTot(15) As System.Windows.Forms.Label
	Public lblConfirm(11) As System.Windows.Forms.Label
	Public lblFloatRem(15) As System.Windows.Forms.Label
	Public lblFloatTot(15) As System.Windows.Forms.Label
	Public lblTotalCC(9) As System.Windows.Forms.Label
	Public tdgCash(15) As Artinsoft.Windows.Forms.ExtendedDataGridView
	Public tdgFloat(15) As Artinsoft.Windows.Forms.ExtendedDataGridView
	Public txtFloatRem(15) As System.Windows.Forms.TextBox
	Public txtTotalCC(15) As System.Windows.Forms.TextBox
	Public txtTotalCash(15) As System.Windows.Forms.TextBox
	Public txtTotalFloat(15) As System.Windows.Forms.TextBox
	Private WithEvents _tdgFloat_15 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_15 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_14 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_14 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_13 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_13 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_12 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_12 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_11 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_11 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_10 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_10 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_9 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_9 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_8 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_8 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_7 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_7 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_6 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_6 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_5 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_5 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_4 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_4 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_3 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_3 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_2 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_2 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_1 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_1 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgFloat_0 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents _tdgCash_0 As Artinsoft.Windows.Forms.ExtendedDataGridView
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle33 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle34 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle35 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle36 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle37 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle38 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle39 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle40 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle41 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle42 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle43 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle44 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle45 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle46 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle47 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle48 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle49 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle50 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle51 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle52 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle53 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle54 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle55 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle56 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle57 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle58 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle59 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle60 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle61 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle62 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle63 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle64 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBanking))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.tabMediaTypes = New System.Windows.Forms.TabControl
        Me._tabMediaTypes_TabPage0 = New System.Windows.Forms.TabPage
        Me._fraCash_0 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_0 = New System.Windows.Forms.TextBox
        Me._tdgCash_0 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_0 = New System.Windows.Forms.Label
        Me._fraFloat_0 = New System.Windows.Forms.GroupBox
        Me._tdgFloat_0 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._txtFloatRem_0 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_0 = New System.Windows.Forms.TextBox
        Me._lblFloatRem_0 = New System.Windows.Forms.Label
        Me._lblFloatTot_0 = New System.Windows.Forms.Label
        Me._txtTotalCC_0 = New System.Windows.Forms.TextBox
        Me._lblTotalCC_0 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage1 = New System.Windows.Forms.TabPage
        Me._fraFloat_1 = New System.Windows.Forms.GroupBox
        Me._tdgFloat_1 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._txtTotalFloat_1 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_1 = New System.Windows.Forms.TextBox
        Me._lblFloatTot_1 = New System.Windows.Forms.Label
        Me._lblFloatRem_1 = New System.Windows.Forms.Label
        Me._fraCash_1 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_1 = New System.Windows.Forms.TextBox
        Me._tdgCash_1 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_1 = New System.Windows.Forms.Label
        Me._txtTotalCC_1 = New System.Windows.Forms.TextBox
        Me._lblTotalCC_1 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage2 = New System.Windows.Forms.TabPage
        Me._lblTotalCC_2 = New System.Windows.Forms.Label
        Me._txtTotalCC_2 = New System.Windows.Forms.TextBox
        Me._fraFloat_2 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_2 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_2 = New System.Windows.Forms.TextBox
        Me._tdgFloat_2 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_2 = New System.Windows.Forms.Label
        Me._lblFloatRem_2 = New System.Windows.Forms.Label
        Me._fraCash_2 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_2 = New System.Windows.Forms.TextBox
        Me._tdgCash_2 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_2 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage3 = New System.Windows.Forms.TabPage
        Me._fraCash_3 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_3 = New System.Windows.Forms.TextBox
        Me._tdgCash_3 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_3 = New System.Windows.Forms.Label
        Me._fraFloat_3 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_3 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_3 = New System.Windows.Forms.TextBox
        Me._tdgFloat_3 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_3 = New System.Windows.Forms.Label
        Me._lblFloatRem_3 = New System.Windows.Forms.Label
        Me._txtTotalCC_3 = New System.Windows.Forms.TextBox
        Me._lblTotalCC_3 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage4 = New System.Windows.Forms.TabPage
        Me._lblTotalCC_4 = New System.Windows.Forms.Label
        Me._txtTotalCC_4 = New System.Windows.Forms.TextBox
        Me._fraFloat_4 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_4 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_4 = New System.Windows.Forms.TextBox
        Me._tdgFloat_4 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_4 = New System.Windows.Forms.Label
        Me._lblFloatRem_4 = New System.Windows.Forms.Label
        Me._fraCash_4 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_4 = New System.Windows.Forms.TextBox
        Me._tdgCash_4 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_4 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage5 = New System.Windows.Forms.TabPage
        Me._lblTotalCC_5 = New System.Windows.Forms.Label
        Me._txtTotalCC_5 = New System.Windows.Forms.TextBox
        Me._fraFloat_5 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_5 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_5 = New System.Windows.Forms.TextBox
        Me._tdgFloat_5 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_5 = New System.Windows.Forms.Label
        Me._lblFloatRem_5 = New System.Windows.Forms.Label
        Me._fraCash_5 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_5 = New System.Windows.Forms.TextBox
        Me._tdgCash_5 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_5 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage6 = New System.Windows.Forms.TabPage
        Me._lblTotalCC_6 = New System.Windows.Forms.Label
        Me._txtTotalCC_6 = New System.Windows.Forms.TextBox
        Me._fraFloat_6 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_6 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_6 = New System.Windows.Forms.TextBox
        Me._tdgFloat_6 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_6 = New System.Windows.Forms.Label
        Me._lblFloatRem_6 = New System.Windows.Forms.Label
        Me._fraCash_6 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_6 = New System.Windows.Forms.TextBox
        Me._tdgCash_6 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_6 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage7 = New System.Windows.Forms.TabPage
        Me._fraCash_7 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_7 = New System.Windows.Forms.TextBox
        Me._tdgCash_7 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_7 = New System.Windows.Forms.Label
        Me._fraFloat_7 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_7 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_7 = New System.Windows.Forms.TextBox
        Me._tdgFloat_7 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_7 = New System.Windows.Forms.Label
        Me._lblFloatRem_7 = New System.Windows.Forms.Label
        Me._txtTotalCC_7 = New System.Windows.Forms.TextBox
        Me._lblTotalCC_7 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage8 = New System.Windows.Forms.TabPage
        Me._lblTotalCC_8 = New System.Windows.Forms.Label
        Me._txtTotalCC_8 = New System.Windows.Forms.TextBox
        Me._fraFloat_8 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_8 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_8 = New System.Windows.Forms.TextBox
        Me._tdgFloat_8 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_8 = New System.Windows.Forms.Label
        Me._lblFloatRem_8 = New System.Windows.Forms.Label
        Me._fraCash_8 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_8 = New System.Windows.Forms.TextBox
        Me._tdgCash_8 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_8 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage9 = New System.Windows.Forms.TabPage
        Me._fraCash_9 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_9 = New System.Windows.Forms.TextBox
        Me._tdgCash_9 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_9 = New System.Windows.Forms.Label
        Me._fraFloat_9 = New System.Windows.Forms.GroupBox
        Me._txtTotalFloat_9 = New System.Windows.Forms.TextBox
        Me._txtFloatRem_9 = New System.Windows.Forms.TextBox
        Me._tdgFloat_9 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatTot_9 = New System.Windows.Forms.Label
        Me._lblFloatRem_9 = New System.Windows.Forms.Label
        Me._txtTotalCC_9 = New System.Windows.Forms.TextBox
        Me._lblTotalCC_9 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage10 = New System.Windows.Forms.TabPage
        Me._fraCash_10 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_10 = New System.Windows.Forms.TextBox
        Me._tdgCash_10 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_10 = New System.Windows.Forms.Label
        Me._fraFloat_10 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_10 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_10 = New System.Windows.Forms.TextBox
        Me._tdgFloat_10 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_10 = New System.Windows.Forms.Label
        Me._lblFloatTot_10 = New System.Windows.Forms.Label
        Me._txtTotalCC_10 = New System.Windows.Forms.TextBox
        Me._tabMediaTypes_TabPage11 = New System.Windows.Forms.TabPage
        Me._fraCash_11 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_11 = New System.Windows.Forms.TextBox
        Me._tdgCash_11 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_11 = New System.Windows.Forms.Label
        Me._fraFloat_11 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_11 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_11 = New System.Windows.Forms.TextBox
        Me._tdgFloat_11 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_11 = New System.Windows.Forms.Label
        Me._lblFloatTot_11 = New System.Windows.Forms.Label
        Me._txtTotalCC_11 = New System.Windows.Forms.TextBox
        Me._tabMediaTypes_TabPage12 = New System.Windows.Forms.TabPage
        Me._txtTotalCC_12 = New System.Windows.Forms.TextBox
        Me._fraFloat_12 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_12 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_12 = New System.Windows.Forms.TextBox
        Me._tdgFloat_12 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_12 = New System.Windows.Forms.Label
        Me._lblFloatTot_12 = New System.Windows.Forms.Label
        Me._fraCash_12 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_12 = New System.Windows.Forms.TextBox
        Me._tdgCash_12 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_12 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage13 = New System.Windows.Forms.TabPage
        Me._fraCash_13 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_13 = New System.Windows.Forms.TextBox
        Me._tdgCash_13 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_13 = New System.Windows.Forms.Label
        Me._fraFloat_13 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_13 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_13 = New System.Windows.Forms.TextBox
        Me._tdgFloat_13 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_13 = New System.Windows.Forms.Label
        Me._lblFloatTot_13 = New System.Windows.Forms.Label
        Me._txtTotalCC_13 = New System.Windows.Forms.TextBox
        Me._tabMediaTypes_TabPage14 = New System.Windows.Forms.TabPage
        Me._txtTotalCC_14 = New System.Windows.Forms.TextBox
        Me._fraFloat_14 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_14 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_14 = New System.Windows.Forms.TextBox
        Me._tdgFloat_14 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_14 = New System.Windows.Forms.Label
        Me._lblFloatTot_14 = New System.Windows.Forms.Label
        Me._fraCash_14 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_14 = New System.Windows.Forms.TextBox
        Me._tdgCash_14 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_14 = New System.Windows.Forms.Label
        Me._tabMediaTypes_TabPage15 = New System.Windows.Forms.TabPage
        Me._fraCash_15 = New System.Windows.Forms.GroupBox
        Me._txtTotalCash_15 = New System.Windows.Forms.TextBox
        Me._tdgCash_15 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblCashTot_15 = New System.Windows.Forms.Label
        Me._fraFloat_15 = New System.Windows.Forms.GroupBox
        Me._txtFloatRem_15 = New System.Windows.Forms.TextBox
        Me._txtTotalFloat_15 = New System.Windows.Forms.TextBox
        Me._tdgFloat_15 = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
        Me._lblFloatRem_15 = New System.Windows.Forms.Label
        Me._lblFloatTot_15 = New System.Windows.Forms.Label
        Me._txtTotalCC_15 = New System.Windows.Forms.TextBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._lblConfirm_0 = New System.Windows.Forms.Label
        Me.fraTotals = New System.Windows.Forms.GroupBox
        Me.txtFloat = New System.Windows.Forms.TextBox
        Me.txtBalance = New System.Windows.Forms.TextBox
        Me.txtAdjustments = New System.Windows.Forms.TextBox
        Me.txtSubTotal = New System.Windows.Forms.TextBox
        Me.txtCCTotal = New System.Windows.Forms.TextBox
        Me.txtBankingTotal = New System.Windows.Forms.TextBox
        Me.cmdAdjustment = New System.Windows.Forms.Button
        Me.cmdReverse = New System.Windows.Forms.Button
        Me.txtTotalReceipts = New System.Windows.Forms.TextBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me._lblConfirm_8 = New System.Windows.Forms.Label
        Me._lblConfirm_7 = New System.Windows.Forms.Label
        Me._lblConfirm_6 = New System.Windows.Forms.Label
        Me._lblConfirm_5 = New System.Windows.Forms.Label
        Me._lblConfirm_4 = New System.Windows.Forms.Label
        Me._lblConfirm_3 = New System.Windows.Forms.Label
        Me._lblConfirm_2 = New System.Windows.Forms.Label
        Me._lblConfirm_1 = New System.Windows.Forms.Label
        Me.txtDepositDate = New System.Windows.Forms.TextBox
        Me.fraAuth = New System.Windows.Forms.GroupBox
        Me.cboPMUserAuthorise1 = New PMUserLookupControl.cboPMUserLookup
        Me.txtDateApproved = New System.Windows.Forms.TextBox
        Me.cboPMUserAuthorise2 = New PMUserLookupControl.cboPMUserLookup
        Me._lblConfirm_11 = New System.Windows.Forms.Label
        Me._lblConfirm_10 = New System.Windows.Forms.Label
        Me._lblConfirm_9 = New System.Windows.Forms.Label
        Me.cmdApprove = New System.Windows.Forms.Button
        Me.cmdViewAdj = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.tabMediaTypes.SuspendLayout()
        Me._tabMediaTypes_TabPage0.SuspendLayout()
        Me._fraCash_0.SuspendLayout()
        CType(Me._tdgCash_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_0.SuspendLayout()
        CType(Me._tdgFloat_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage1.SuspendLayout()
        Me._fraFloat_1.SuspendLayout()
        CType(Me._tdgFloat_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_1.SuspendLayout()
        CType(Me._tdgCash_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage2.SuspendLayout()
        Me._fraFloat_2.SuspendLayout()
        CType(Me._tdgFloat_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_2.SuspendLayout()
        CType(Me._tdgCash_2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage3.SuspendLayout()
        Me._fraCash_3.SuspendLayout()
        CType(Me._tdgCash_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_3.SuspendLayout()
        CType(Me._tdgFloat_3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage4.SuspendLayout()
        Me._fraFloat_4.SuspendLayout()
        CType(Me._tdgFloat_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_4.SuspendLayout()
        CType(Me._tdgCash_4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage5.SuspendLayout()
        Me._fraFloat_5.SuspendLayout()
        CType(Me._tdgFloat_5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_5.SuspendLayout()
        CType(Me._tdgCash_5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage6.SuspendLayout()
        Me._fraFloat_6.SuspendLayout()
        CType(Me._tdgFloat_6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_6.SuspendLayout()
        CType(Me._tdgCash_6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage7.SuspendLayout()
        Me._fraCash_7.SuspendLayout()
        CType(Me._tdgCash_7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_7.SuspendLayout()
        CType(Me._tdgFloat_7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage8.SuspendLayout()
        Me._fraFloat_8.SuspendLayout()
        CType(Me._tdgFloat_8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_8.SuspendLayout()
        CType(Me._tdgCash_8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage9.SuspendLayout()
        Me._fraCash_9.SuspendLayout()
        CType(Me._tdgCash_9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_9.SuspendLayout()
        CType(Me._tdgFloat_9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage10.SuspendLayout()
        Me._fraCash_10.SuspendLayout()
        CType(Me._tdgCash_10, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_10.SuspendLayout()
        CType(Me._tdgFloat_10, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage11.SuspendLayout()
        Me._fraCash_11.SuspendLayout()
        CType(Me._tdgCash_11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_11.SuspendLayout()
        CType(Me._tdgFloat_11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage12.SuspendLayout()
        Me._fraFloat_12.SuspendLayout()
        CType(Me._tdgFloat_12, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_12.SuspendLayout()
        CType(Me._tdgCash_12, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage13.SuspendLayout()
        Me._fraCash_13.SuspendLayout()
        CType(Me._tdgCash_13, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_13.SuspendLayout()
        CType(Me._tdgFloat_13, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage14.SuspendLayout()
        Me._fraFloat_14.SuspendLayout()
        CType(Me._tdgFloat_14, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraCash_14.SuspendLayout()
        CType(Me._tdgCash_14, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMediaTypes_TabPage15.SuspendLayout()
        Me._fraCash_15.SuspendLayout()
        CType(Me._tdgCash_15, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._fraFloat_15.SuspendLayout()
        CType(Me._tdgFloat_15, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraTotals.SuspendLayout()
        Me.fraAuth.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(448, 416)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Tag = "CAP;615"
        Me.cmdCancel.Text = "*{&Cancel}"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(368, 416)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Tag = "CAP;614"
        Me.cmdOK.Text = "*{&OK}"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        Me.cmdOK.Visible = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 416)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 3
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(528, 416)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.Tag = "CAP;616"
        Me.cmdHelp.Text = "*{&Help}"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(117, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(597, 405)
        Me.tabMainTab.TabIndex = 4
        Me.tabMainTab.Tag = "CAP;601"
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.tabMediaTypes)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(589, 379)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "*{Summary}"
        '
        'tabMediaTypes
        '
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage0)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage1)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage2)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage3)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage4)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage5)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage6)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage7)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage8)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage9)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage10)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage11)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage12)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage13)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage14)
        Me.tabMediaTypes.Controls.Add(Me._tabMediaTypes_TabPage15)
        Me.tabMediaTypes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMediaTypes.ItemSize = New System.Drawing.Size(92, 18)
        Me.tabMediaTypes.Location = New System.Drawing.Point(16, 12)
        Me.tabMediaTypes.Multiline = True
        Me.tabMediaTypes.Name = "tabMediaTypes"
        Me.tabMediaTypes.SelectedIndex = 0
        Me.tabMediaTypes.Size = New System.Drawing.Size(565, 359)
        Me.tabMediaTypes.TabIndex = 5
        '
        '_tabMediaTypes_TabPage0
        '
        Me._tabMediaTypes_TabPage0.Controls.Add(Me._fraCash_0)
        Me._tabMediaTypes_TabPage0.Controls.Add(Me._fraFloat_0)
        Me._tabMediaTypes_TabPage0.Controls.Add(Me._txtTotalCC_0)
        Me._tabMediaTypes_TabPage0.Controls.Add(Me._lblTotalCC_0)
        Me._tabMediaTypes_TabPage0.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage0.Name = "_tabMediaTypes_TabPage0"
        Me._tabMediaTypes_TabPage0.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage0.TabIndex = 0
        Me._tabMediaTypes_TabPage0.Text = "Tab 0"
        '
        '_fraCash_0
        '
        Me._fraCash_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_0.Controls.Add(Me._txtTotalCash_0)
        Me._fraCash_0.Controls.Add(Me._tdgCash_0)
        Me._fraCash_0.Controls.Add(Me._lblCashTot_0)
        Me._fraCash_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_0.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_0.Name = "_fraCash_0"
        Me._fraCash_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_0.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_0.TabIndex = 218
        Me._fraCash_0.TabStop = False
        Me._fraCash_0.Tag = "CAP;665"
        Me._fraCash_0.Text = "*{Cash Breakdown}"
        Me._fraCash_0.Visible = False
        '
        '_txtTotalCash_0
        '
        Me._txtTotalCash_0.AcceptsReturn = True
        Me._txtTotalCash_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_0.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_0.MaxLength = 0
        Me._txtTotalCash_0.Name = "_txtTotalCash_0"
        Me._txtTotalCash_0.ReadOnly = True
        Me._txtTotalCash_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_0.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_0.TabIndex = 219
        Me._txtTotalCash_0.TabStop = False
        Me._txtTotalCash_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_0
        '
        Me._tdgCash_0.AllowBigSelection = False
        Me._tdgCash_0.AllowRowSelection = False
        Me._tdgCash_0.AlternatingRows = False
        Me._tdgCash_0.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_0.ColumnsCount = 0
        Me._tdgCash_0.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_0.EvenStyle = DataGridViewCellStyle1
        Me._tdgCash_0.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_0.FixedColumns = -1
        Me._tdgCash_0.FixedRows = -1
        Me._tdgCash_0.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_0.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_0.GridLineWidth = 0
        Me._tdgCash_0.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_0.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_0.Name = "_tdgCash_0"
        Me._tdgCash_0.OddStyle = DataGridViewCellStyle2
        Me._tdgCash_0.RowHeightMin = 0
        Me._tdgCash_0.RowsCount = 0
        Me._tdgCash_0.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_0.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_0.SelectedStyle = Nothing
        Me._tdgCash_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_0.SelLength = -1
        Me._tdgCash_0.SelStart = -1
        Me._tdgCash_0.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_0.TabIndex = 220
        Me._tdgCash_0.ToolTipText = ""
        '
        '_lblCashTot_0
        '
        Me._lblCashTot_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_0.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_0.Name = "_lblCashTot_0"
        Me._lblCashTot_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_0.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_0.TabIndex = 221
        Me._lblCashTot_0.Tag = "CAP;666"
        Me._lblCashTot_0.Text = "*{Total}"
        '
        '_fraFloat_0
        '
        Me._fraFloat_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_0.Controls.Add(Me._tdgFloat_0)
        Me._fraFloat_0.Controls.Add(Me._txtFloatRem_0)
        Me._fraFloat_0.Controls.Add(Me._txtTotalFloat_0)
        Me._fraFloat_0.Controls.Add(Me._lblFloatRem_0)
        Me._fraFloat_0.Controls.Add(Me._lblFloatTot_0)
        Me._fraFloat_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_0.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_0.Name = "_fraFloat_0"
        Me._fraFloat_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_0.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_0.TabIndex = 6
        Me._fraFloat_0.TabStop = False
        Me._fraFloat_0.Tag = "CAP;664"
        Me._fraFloat_0.Text = "*{Float Breakdown}"
        Me._fraFloat_0.Visible = False
        '
        '_tdgFloat_0
        '
        Me._tdgFloat_0.AllowBigSelection = False
        Me._tdgFloat_0.AllowRowSelection = False
        Me._tdgFloat_0.AlternatingRows = False
        Me._tdgFloat_0.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_0.ColumnsCount = 0
        Me._tdgFloat_0.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_0.EvenStyle = DataGridViewCellStyle3
        Me._tdgFloat_0.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_0.FixedColumns = -1
        Me._tdgFloat_0.FixedRows = -1
        Me._tdgFloat_0.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_0.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_0.GridLineWidth = 0
        Me._tdgFloat_0.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_0.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_0.Name = "_tdgFloat_0"
        Me._tdgFloat_0.OddStyle = DataGridViewCellStyle4
        Me._tdgFloat_0.RowHeightMin = 0
        Me._tdgFloat_0.RowsCount = 0
        Me._tdgFloat_0.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_0.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_0.SelectedStyle = Nothing
        Me._tdgFloat_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_0.SelLength = -1
        Me._tdgFloat_0.SelStart = -1
        Me._tdgFloat_0.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_0.TabIndex = 7
        Me._tdgFloat_0.ToolTipText = ""
        '
        '_txtFloatRem_0
        '
        Me._txtFloatRem_0.AcceptsReturn = True
        Me._txtFloatRem_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_0.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_0.MaxLength = 0
        Me._txtFloatRem_0.Name = "_txtFloatRem_0"
        Me._txtFloatRem_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_0.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_0.TabIndex = 9
        Me._txtFloatRem_0.TabStop = False
        Me._txtFloatRem_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_0
        '
        Me._txtTotalFloat_0.AcceptsReturn = True
        Me._txtTotalFloat_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_0.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_0.MaxLength = 0
        Me._txtTotalFloat_0.Name = "_txtTotalFloat_0"
        Me._txtTotalFloat_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_0.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_0.TabIndex = 8
        Me._txtTotalFloat_0.TabStop = False
        Me._txtTotalFloat_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_lblFloatRem_0
        '
        Me._lblFloatRem_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_0.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_0.Name = "_lblFloatRem_0"
        Me._lblFloatRem_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_0.Size = New System.Drawing.Size(105, 17)
        Me._lblFloatRem_0.TabIndex = 111
        Me._lblFloatRem_0.Tag = "CAP;667"
        Me._lblFloatRem_0.Text = "*{Float Remaining}"
        '
        '_lblFloatTot_0
        '
        Me._lblFloatTot_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_0.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_0.Name = "_lblFloatTot_0"
        Me._lblFloatTot_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_0.Size = New System.Drawing.Size(105, 17)
        Me._lblFloatTot_0.TabIndex = 110
        Me._lblFloatTot_0.Tag = "CAP;666"
        Me._lblFloatTot_0.Text = "*{Total}"
        '
        '_txtTotalCC_0
        '
        Me._txtTotalCC_0.AcceptsReturn = True
        Me._txtTotalCC_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_0.Enabled = False
        Me._txtTotalCC_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_0.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_0.MaxLength = 0
        Me._txtTotalCC_0.Name = "_txtTotalCC_0"
        Me._txtTotalCC_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_0.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_0.TabIndex = 10
        Me._txtTotalCC_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_0.Visible = False
        '
        '_lblTotalCC_0
        '
        Me._lblTotalCC_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_0.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_0.Name = "_lblTotalCC_0"
        Me._lblTotalCC_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_0.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_0.TabIndex = 120
        Me._lblTotalCC_0.Text = "Total"
        Me._lblTotalCC_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_0.Visible = False
        '
        '_tabMediaTypes_TabPage1
        '
        Me._tabMediaTypes_TabPage1.Controls.Add(Me._fraFloat_1)
        Me._tabMediaTypes_TabPage1.Controls.Add(Me._fraCash_1)
        Me._tabMediaTypes_TabPage1.Controls.Add(Me._txtTotalCC_1)
        Me._tabMediaTypes_TabPage1.Controls.Add(Me._lblTotalCC_1)
        Me._tabMediaTypes_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage1.Name = "_tabMediaTypes_TabPage1"
        Me._tabMediaTypes_TabPage1.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage1.TabIndex = 1
        Me._tabMediaTypes_TabPage1.Text = "Tab 1"
        '
        '_fraFloat_1
        '
        Me._fraFloat_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_1.Controls.Add(Me._tdgFloat_1)
        Me._fraFloat_1.Controls.Add(Me._txtTotalFloat_1)
        Me._fraFloat_1.Controls.Add(Me._txtFloatRem_1)
        Me._fraFloat_1.Controls.Add(Me._lblFloatTot_1)
        Me._fraFloat_1.Controls.Add(Me._lblFloatRem_1)
        Me._fraFloat_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_1.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_1.Name = "_fraFloat_1"
        Me._fraFloat_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_1.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_1.TabIndex = 11
        Me._fraFloat_1.TabStop = False
        Me._fraFloat_1.Tag = "CAP;664"
        Me._fraFloat_1.Text = "Float Breakdown"
        Me._fraFloat_1.Visible = False
        '
        '_tdgFloat_1
        '
        Me._tdgFloat_1.AllowBigSelection = False
        Me._tdgFloat_1.AllowRowSelection = False
        Me._tdgFloat_1.AlternatingRows = False
        Me._tdgFloat_1.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_1.ColumnsCount = 0
        Me._tdgFloat_1.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_1.EvenStyle = DataGridViewCellStyle5
        Me._tdgFloat_1.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_1.FixedColumns = -1
        Me._tdgFloat_1.FixedRows = -1
        Me._tdgFloat_1.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_1.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_1.GridLineWidth = 0
        Me._tdgFloat_1.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_1.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_1.Name = "_tdgFloat_1"
        Me._tdgFloat_1.OddStyle = DataGridViewCellStyle6
        Me._tdgFloat_1.RowHeightMin = 0
        Me._tdgFloat_1.RowsCount = 0
        Me._tdgFloat_1.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_1.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_1.SelectedStyle = Nothing
        Me._tdgFloat_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_1.SelLength = -1
        Me._tdgFloat_1.SelStart = -1
        Me._tdgFloat_1.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_1.TabIndex = 12
        Me._tdgFloat_1.ToolTipText = ""
        '
        '_txtTotalFloat_1
        '
        Me._txtTotalFloat_1.AcceptsReturn = True
        Me._txtTotalFloat_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_1.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_1.MaxLength = 0
        Me._txtTotalFloat_1.Name = "_txtTotalFloat_1"
        Me._txtTotalFloat_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_1.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_1.TabIndex = 13
        Me._txtTotalFloat_1.TabStop = False
        Me._txtTotalFloat_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_1
        '
        Me._txtFloatRem_1.AcceptsReturn = True
        Me._txtFloatRem_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_1.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_1.MaxLength = 0
        Me._txtFloatRem_1.Name = "_txtFloatRem_1"
        Me._txtFloatRem_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_1.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_1.TabIndex = 14
        Me._txtFloatRem_1.TabStop = False
        Me._txtFloatRem_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_lblFloatTot_1
        '
        Me._lblFloatTot_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_1.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_1.Name = "_lblFloatTot_1"
        Me._lblFloatTot_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_1.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_1.TabIndex = 113
        Me._lblFloatTot_1.Tag = "CAP;666"
        Me._lblFloatTot_1.Text = "Total"
        '
        '_lblFloatRem_1
        '
        Me._lblFloatRem_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_1.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_1.Name = "_lblFloatRem_1"
        Me._lblFloatRem_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_1.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_1.TabIndex = 112
        Me._lblFloatRem_1.Tag = "CAP;667"
        Me._lblFloatRem_1.Text = "Float Remaining"
        '
        '_fraCash_1
        '
        Me._fraCash_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_1.Controls.Add(Me._txtTotalCash_1)
        Me._fraCash_1.Controls.Add(Me._tdgCash_1)
        Me._fraCash_1.Controls.Add(Me._lblCashTot_1)
        Me._fraCash_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_1.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_1.Name = "_fraCash_1"
        Me._fraCash_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_1.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_1.TabIndex = 15
        Me._fraCash_1.TabStop = False
        Me._fraCash_1.Tag = "CAP;665"
        Me._fraCash_1.Text = "Cash Breakdown"
        Me._fraCash_1.Visible = False
        '
        '_txtTotalCash_1
        '
        Me._txtTotalCash_1.AcceptsReturn = True
        Me._txtTotalCash_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_1.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_1.MaxLength = 0
        Me._txtTotalCash_1.Name = "_txtTotalCash_1"
        Me._txtTotalCash_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_1.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_1.TabIndex = 17
        Me._txtTotalCash_1.TabStop = False
        Me._txtTotalCash_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_1
        '
        Me._tdgCash_1.AllowBigSelection = False
        Me._tdgCash_1.AllowRowSelection = False
        Me._tdgCash_1.AlternatingRows = False
        Me._tdgCash_1.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_1.ColumnsCount = 0
        Me._tdgCash_1.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_1.EvenStyle = DataGridViewCellStyle7
        Me._tdgCash_1.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_1.FixedColumns = -1
        Me._tdgCash_1.FixedRows = -1
        Me._tdgCash_1.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_1.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_1.GridLineWidth = 0
        Me._tdgCash_1.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_1.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_1.Name = "_tdgCash_1"
        Me._tdgCash_1.OddStyle = DataGridViewCellStyle8
        Me._tdgCash_1.RowHeightMin = 0
        Me._tdgCash_1.RowsCount = 0
        Me._tdgCash_1.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_1.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_1.SelectedStyle = Nothing
        Me._tdgCash_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_1.SelLength = -1
        Me._tdgCash_1.SelStart = -1
        Me._tdgCash_1.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_1.TabIndex = 16
        Me._tdgCash_1.ToolTipText = ""
        '
        '_lblCashTot_1
        '
        Me._lblCashTot_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_1.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_1.Name = "_lblCashTot_1"
        Me._lblCashTot_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_1.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_1.TabIndex = 145
        Me._lblCashTot_1.Tag = "CAP;666"
        Me._lblCashTot_1.Text = "Total"
        '
        '_txtTotalCC_1
        '
        Me._txtTotalCC_1.AcceptsReturn = True
        Me._txtTotalCC_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_1.Enabled = False
        Me._txtTotalCC_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_1.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_1.MaxLength = 0
        Me._txtTotalCC_1.Name = "_txtTotalCC_1"
        Me._txtTotalCC_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_1.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_1.TabIndex = 121
        Me._txtTotalCC_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_1.Visible = False
        '
        '_lblTotalCC_1
        '
        Me._lblTotalCC_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_1.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_1.Name = "_lblTotalCC_1"
        Me._lblTotalCC_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_1.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_1.TabIndex = 122
        Me._lblTotalCC_1.Text = "Total"
        Me._lblTotalCC_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_1.Visible = False
        '
        '_tabMediaTypes_TabPage2
        '
        Me._tabMediaTypes_TabPage2.Controls.Add(Me._lblTotalCC_2)
        Me._tabMediaTypes_TabPage2.Controls.Add(Me._txtTotalCC_2)
        Me._tabMediaTypes_TabPage2.Controls.Add(Me._fraFloat_2)
        Me._tabMediaTypes_TabPage2.Controls.Add(Me._fraCash_2)
        Me._tabMediaTypes_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage2.Name = "_tabMediaTypes_TabPage2"
        Me._tabMediaTypes_TabPage2.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage2.TabIndex = 2
        Me._tabMediaTypes_TabPage2.Text = "Tab 2"
        '
        '_lblTotalCC_2
        '
        Me._lblTotalCC_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_2.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_2.Name = "_lblTotalCC_2"
        Me._lblTotalCC_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_2.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_2.TabIndex = 124
        Me._lblTotalCC_2.Text = "Total"
        Me._lblTotalCC_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_2.Visible = False
        '
        '_txtTotalCC_2
        '
        Me._txtTotalCC_2.AcceptsReturn = True
        Me._txtTotalCC_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_2.Enabled = False
        Me._txtTotalCC_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_2.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_2.MaxLength = 0
        Me._txtTotalCC_2.Name = "_txtTotalCC_2"
        Me._txtTotalCC_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_2.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_2.TabIndex = 123
        Me._txtTotalCC_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_2.Visible = False
        '
        '_fraFloat_2
        '
        Me._fraFloat_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_2.Controls.Add(Me._txtTotalFloat_2)
        Me._fraFloat_2.Controls.Add(Me._txtFloatRem_2)
        Me._fraFloat_2.Controls.Add(Me._tdgFloat_2)
        Me._fraFloat_2.Controls.Add(Me._lblFloatTot_2)
        Me._fraFloat_2.Controls.Add(Me._lblFloatRem_2)
        Me._fraFloat_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_2.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_2.Name = "_fraFloat_2"
        Me._fraFloat_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_2.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_2.TabIndex = 18
        Me._fraFloat_2.TabStop = False
        Me._fraFloat_2.Tag = "CAP;664"
        Me._fraFloat_2.Text = "Float Breakdown"
        Me._fraFloat_2.Visible = False
        '
        '_txtTotalFloat_2
        '
        Me._txtTotalFloat_2.AcceptsReturn = True
        Me._txtTotalFloat_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_2.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_2.MaxLength = 0
        Me._txtTotalFloat_2.Name = "_txtTotalFloat_2"
        Me._txtTotalFloat_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_2.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_2.TabIndex = 20
        Me._txtTotalFloat_2.TabStop = False
        Me._txtTotalFloat_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_2
        '
        Me._txtFloatRem_2.AcceptsReturn = True
        Me._txtFloatRem_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_2.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_2.MaxLength = 0
        Me._txtFloatRem_2.Name = "_txtFloatRem_2"
        Me._txtFloatRem_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_2.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_2.TabIndex = 21
        Me._txtFloatRem_2.TabStop = False
        Me._txtFloatRem_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_2
        '
        Me._tdgFloat_2.AllowBigSelection = False
        Me._tdgFloat_2.AllowRowSelection = False
        Me._tdgFloat_2.AlternatingRows = False
        Me._tdgFloat_2.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_2.ColumnsCount = 0
        Me._tdgFloat_2.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_2.EvenStyle = DataGridViewCellStyle9
        Me._tdgFloat_2.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_2.FixedColumns = -1
        Me._tdgFloat_2.FixedRows = -1
        Me._tdgFloat_2.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_2.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_2.GridLineWidth = 0
        Me._tdgFloat_2.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_2.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_2.Name = "_tdgFloat_2"
        Me._tdgFloat_2.OddStyle = DataGridViewCellStyle10
        Me._tdgFloat_2.RowHeightMin = 0
        Me._tdgFloat_2.RowsCount = 0
        Me._tdgFloat_2.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_2.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_2.SelectedStyle = Nothing
        Me._tdgFloat_2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_2.SelLength = -1
        Me._tdgFloat_2.SelStart = -1
        Me._tdgFloat_2.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_2.TabIndex = 19
        Me._tdgFloat_2.ToolTipText = ""
        '
        '_lblFloatTot_2
        '
        Me._lblFloatTot_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_2.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_2.Name = "_lblFloatTot_2"
        Me._lblFloatTot_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_2.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_2.TabIndex = 115
        Me._lblFloatTot_2.Tag = "CAP;666"
        Me._lblFloatTot_2.Text = "Total"
        '
        '_lblFloatRem_2
        '
        Me._lblFloatRem_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_2.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_2.Name = "_lblFloatRem_2"
        Me._lblFloatRem_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_2.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_2.TabIndex = 114
        Me._lblFloatRem_2.Tag = "CAP;667"
        Me._lblFloatRem_2.Text = "Float Remaining"
        '
        '_fraCash_2
        '
        Me._fraCash_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_2.Controls.Add(Me._txtTotalCash_2)
        Me._fraCash_2.Controls.Add(Me._tdgCash_2)
        Me._fraCash_2.Controls.Add(Me._lblCashTot_2)
        Me._fraCash_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_2.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_2.Name = "_fraCash_2"
        Me._fraCash_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_2.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_2.TabIndex = 22
        Me._fraCash_2.TabStop = False
        Me._fraCash_2.Tag = "CAP;665"
        Me._fraCash_2.Text = "Cash Breakdown"
        Me._fraCash_2.Visible = False
        '
        '_txtTotalCash_2
        '
        Me._txtTotalCash_2.AcceptsReturn = True
        Me._txtTotalCash_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_2.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_2.MaxLength = 0
        Me._txtTotalCash_2.Name = "_txtTotalCash_2"
        Me._txtTotalCash_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_2.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_2.TabIndex = 24
        Me._txtTotalCash_2.TabStop = False
        Me._txtTotalCash_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_2
        '
        Me._tdgCash_2.AllowBigSelection = False
        Me._tdgCash_2.AllowRowSelection = False
        Me._tdgCash_2.AlternatingRows = False
        Me._tdgCash_2.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_2.ColumnsCount = 0
        Me._tdgCash_2.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_2.EvenStyle = DataGridViewCellStyle11
        Me._tdgCash_2.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_2.FixedColumns = -1
        Me._tdgCash_2.FixedRows = -1
        Me._tdgCash_2.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_2.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_2.GridLineWidth = 0
        Me._tdgCash_2.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_2.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_2.Name = "_tdgCash_2"
        Me._tdgCash_2.OddStyle = DataGridViewCellStyle12
        Me._tdgCash_2.RowHeightMin = 0
        Me._tdgCash_2.RowsCount = 0
        Me._tdgCash_2.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_2.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_2.SelectedStyle = Nothing
        Me._tdgCash_2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_2.SelLength = -1
        Me._tdgCash_2.SelStart = -1
        Me._tdgCash_2.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_2.TabIndex = 23
        Me._tdgCash_2.ToolTipText = ""
        '
        '_lblCashTot_2
        '
        Me._lblCashTot_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_2.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_2.Name = "_lblCashTot_2"
        Me._lblCashTot_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_2.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_2.TabIndex = 137
        Me._lblCashTot_2.Tag = "CAP;666"
        Me._lblCashTot_2.Text = "Total"
        '
        '_tabMediaTypes_TabPage3
        '
        Me._tabMediaTypes_TabPage3.Controls.Add(Me._fraCash_3)
        Me._tabMediaTypes_TabPage3.Controls.Add(Me._fraFloat_3)
        Me._tabMediaTypes_TabPage3.Controls.Add(Me._txtTotalCC_3)
        Me._tabMediaTypes_TabPage3.Controls.Add(Me._lblTotalCC_3)
        Me._tabMediaTypes_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage3.Name = "_tabMediaTypes_TabPage3"
        Me._tabMediaTypes_TabPage3.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage3.TabIndex = 3
        Me._tabMediaTypes_TabPage3.Text = "Tab 3"
        '
        '_fraCash_3
        '
        Me._fraCash_3.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_3.Controls.Add(Me._txtTotalCash_3)
        Me._fraCash_3.Controls.Add(Me._tdgCash_3)
        Me._fraCash_3.Controls.Add(Me._lblCashTot_3)
        Me._fraCash_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_3.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_3.Name = "_fraCash_3"
        Me._fraCash_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_3.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_3.TabIndex = 29
        Me._fraCash_3.TabStop = False
        Me._fraCash_3.Tag = "CAP;665"
        Me._fraCash_3.Text = "Cash Breakdown"
        Me._fraCash_3.Visible = False
        '
        '_txtTotalCash_3
        '
        Me._txtTotalCash_3.AcceptsReturn = True
        Me._txtTotalCash_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_3.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_3.MaxLength = 0
        Me._txtTotalCash_3.Name = "_txtTotalCash_3"
        Me._txtTotalCash_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_3.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_3.TabIndex = 31
        Me._txtTotalCash_3.TabStop = False
        Me._txtTotalCash_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_3
        '
        Me._tdgCash_3.AllowBigSelection = False
        Me._tdgCash_3.AllowRowSelection = False
        Me._tdgCash_3.AlternatingRows = False
        Me._tdgCash_3.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_3.ColumnsCount = 0
        Me._tdgCash_3.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_3.EvenStyle = DataGridViewCellStyle13
        Me._tdgCash_3.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_3.FixedColumns = -1
        Me._tdgCash_3.FixedRows = -1
        Me._tdgCash_3.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_3.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_3.GridLineWidth = 0
        Me._tdgCash_3.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_3.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_3.Name = "_tdgCash_3"
        Me._tdgCash_3.OddStyle = DataGridViewCellStyle14
        Me._tdgCash_3.RowHeightMin = 0
        Me._tdgCash_3.RowsCount = 0
        Me._tdgCash_3.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_3.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_3.SelectedStyle = Nothing
        Me._tdgCash_3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_3.SelLength = -1
        Me._tdgCash_3.SelStart = -1
        Me._tdgCash_3.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_3.TabIndex = 30
        Me._tdgCash_3.ToolTipText = ""
        '
        '_lblCashTot_3
        '
        Me._lblCashTot_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_3.Location = New System.Drawing.Point(88, 212)
        Me._lblCashTot_3.Name = "_lblCashTot_3"
        Me._lblCashTot_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_3.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_3.TabIndex = 138
        Me._lblCashTot_3.Tag = "CAP;666"
        Me._lblCashTot_3.Text = "Total"
        '
        '_fraFloat_3
        '
        Me._fraFloat_3.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_3.Controls.Add(Me._txtTotalFloat_3)
        Me._fraFloat_3.Controls.Add(Me._txtFloatRem_3)
        Me._fraFloat_3.Controls.Add(Me._tdgFloat_3)
        Me._fraFloat_3.Controls.Add(Me._lblFloatTot_3)
        Me._fraFloat_3.Controls.Add(Me._lblFloatRem_3)
        Me._fraFloat_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_3.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_3.Name = "_fraFloat_3"
        Me._fraFloat_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_3.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_3.TabIndex = 25
        Me._fraFloat_3.TabStop = False
        Me._fraFloat_3.Tag = "CAP;664"
        Me._fraFloat_3.Text = "Float Breakdown"
        Me._fraFloat_3.Visible = False
        '
        '_txtTotalFloat_3
        '
        Me._txtTotalFloat_3.AcceptsReturn = True
        Me._txtTotalFloat_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_3.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_3.MaxLength = 0
        Me._txtTotalFloat_3.Name = "_txtTotalFloat_3"
        Me._txtTotalFloat_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_3.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_3.TabIndex = 27
        Me._txtTotalFloat_3.TabStop = False
        Me._txtTotalFloat_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_3
        '
        Me._txtFloatRem_3.AcceptsReturn = True
        Me._txtFloatRem_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_3.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_3.MaxLength = 0
        Me._txtFloatRem_3.Name = "_txtFloatRem_3"
        Me._txtFloatRem_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_3.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_3.TabIndex = 28
        Me._txtFloatRem_3.TabStop = False
        Me._txtFloatRem_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_3
        '
        Me._tdgFloat_3.AllowBigSelection = False
        Me._tdgFloat_3.AllowRowSelection = False
        Me._tdgFloat_3.AlternatingRows = False
        Me._tdgFloat_3.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_3.ColumnsCount = 0
        Me._tdgFloat_3.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_3.EvenStyle = DataGridViewCellStyle15
        Me._tdgFloat_3.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_3.FixedColumns = -1
        Me._tdgFloat_3.FixedRows = -1
        Me._tdgFloat_3.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_3.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_3.GridLineWidth = 0
        Me._tdgFloat_3.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_3.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_3.Name = "_tdgFloat_3"
        Me._tdgFloat_3.OddStyle = DataGridViewCellStyle16
        Me._tdgFloat_3.RowHeightMin = 0
        Me._tdgFloat_3.RowsCount = 0
        Me._tdgFloat_3.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_3.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_3.SelectedStyle = Nothing
        Me._tdgFloat_3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_3.SelLength = -1
        Me._tdgFloat_3.SelStart = -1
        Me._tdgFloat_3.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_3.TabIndex = 26
        Me._tdgFloat_3.ToolTipText = ""
        '
        '_lblFloatTot_3
        '
        Me._lblFloatTot_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_3.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_3.Name = "_lblFloatTot_3"
        Me._lblFloatTot_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_3.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_3.TabIndex = 117
        Me._lblFloatTot_3.Tag = "CAP;666"
        Me._lblFloatTot_3.Text = "Total"
        '
        '_lblFloatRem_3
        '
        Me._lblFloatRem_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_3.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_3.Name = "_lblFloatRem_3"
        Me._lblFloatRem_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_3.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_3.TabIndex = 116
        Me._lblFloatRem_3.Tag = "CAP;667"
        Me._lblFloatRem_3.Text = "Float Remaining"
        '
        '_txtTotalCC_3
        '
        Me._txtTotalCC_3.AcceptsReturn = True
        Me._txtTotalCC_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_3.Enabled = False
        Me._txtTotalCC_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_3.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_3.MaxLength = 0
        Me._txtTotalCC_3.Name = "_txtTotalCC_3"
        Me._txtTotalCC_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_3.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_3.TabIndex = 125
        Me._txtTotalCC_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_3.Visible = False
        '
        '_lblTotalCC_3
        '
        Me._lblTotalCC_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_3.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_3.Name = "_lblTotalCC_3"
        Me._lblTotalCC_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_3.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_3.TabIndex = 126
        Me._lblTotalCC_3.Text = "Total"
        Me._lblTotalCC_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_3.Visible = False
        '
        '_tabMediaTypes_TabPage4
        '
        Me._tabMediaTypes_TabPage4.Controls.Add(Me._lblTotalCC_4)
        Me._tabMediaTypes_TabPage4.Controls.Add(Me._txtTotalCC_4)
        Me._tabMediaTypes_TabPage4.Controls.Add(Me._fraFloat_4)
        Me._tabMediaTypes_TabPage4.Controls.Add(Me._fraCash_4)
        Me._tabMediaTypes_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage4.Name = "_tabMediaTypes_TabPage4"
        Me._tabMediaTypes_TabPage4.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage4.TabIndex = 4
        Me._tabMediaTypes_TabPage4.Text = "Tab 4"
        '
        '_lblTotalCC_4
        '
        Me._lblTotalCC_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_4.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_4.Name = "_lblTotalCC_4"
        Me._lblTotalCC_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_4.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_4.TabIndex = 119
        Me._lblTotalCC_4.Text = "Total"
        Me._lblTotalCC_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_4.Visible = False
        '
        '_txtTotalCC_4
        '
        Me._txtTotalCC_4.AcceptsReturn = True
        Me._txtTotalCC_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_4.Enabled = False
        Me._txtTotalCC_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_4.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_4.MaxLength = 0
        Me._txtTotalCC_4.Name = "_txtTotalCC_4"
        Me._txtTotalCC_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_4.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_4.TabIndex = 118
        Me._txtTotalCC_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_4.Visible = False
        '
        '_fraFloat_4
        '
        Me._fraFloat_4.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_4.Controls.Add(Me._txtTotalFloat_4)
        Me._fraFloat_4.Controls.Add(Me._txtFloatRem_4)
        Me._fraFloat_4.Controls.Add(Me._tdgFloat_4)
        Me._fraFloat_4.Controls.Add(Me._lblFloatTot_4)
        Me._fraFloat_4.Controls.Add(Me._lblFloatRem_4)
        Me._fraFloat_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_4.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_4.Name = "_fraFloat_4"
        Me._fraFloat_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_4.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_4.TabIndex = 32
        Me._fraFloat_4.TabStop = False
        Me._fraFloat_4.Tag = "CAP;664"
        Me._fraFloat_4.Text = "Float Breakdown"
        Me._fraFloat_4.Visible = False
        '
        '_txtTotalFloat_4
        '
        Me._txtTotalFloat_4.AcceptsReturn = True
        Me._txtTotalFloat_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_4.Location = New System.Drawing.Point(152, 208)
        Me._txtTotalFloat_4.MaxLength = 0
        Me._txtTotalFloat_4.Name = "_txtTotalFloat_4"
        Me._txtTotalFloat_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_4.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_4.TabIndex = 34
        Me._txtTotalFloat_4.TabStop = False
        Me._txtTotalFloat_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_4
        '
        Me._txtFloatRem_4.AcceptsReturn = True
        Me._txtFloatRem_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_4.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_4.MaxLength = 0
        Me._txtFloatRem_4.Name = "_txtFloatRem_4"
        Me._txtFloatRem_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_4.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_4.TabIndex = 35
        Me._txtFloatRem_4.TabStop = False
        Me._txtFloatRem_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_4
        '
        Me._tdgFloat_4.AllowBigSelection = False
        Me._tdgFloat_4.AllowRowSelection = False
        Me._tdgFloat_4.AlternatingRows = False
        Me._tdgFloat_4.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_4.ColumnsCount = 0
        Me._tdgFloat_4.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_4.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_4.EvenStyle = DataGridViewCellStyle17
        Me._tdgFloat_4.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_4.FixedColumns = -1
        Me._tdgFloat_4.FixedRows = -1
        Me._tdgFloat_4.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_4.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_4.GridLineWidth = 0
        Me._tdgFloat_4.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_4.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_4.Name = "_tdgFloat_4"
        Me._tdgFloat_4.OddStyle = DataGridViewCellStyle18
        Me._tdgFloat_4.RowHeightMin = 0
        Me._tdgFloat_4.RowsCount = 0
        Me._tdgFloat_4.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_4.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_4.SelectedStyle = Nothing
        Me._tdgFloat_4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_4.SelLength = -1
        Me._tdgFloat_4.SelStart = -1
        Me._tdgFloat_4.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_4.TabIndex = 33
        Me._tdgFloat_4.ToolTipText = ""
        '
        '_lblFloatTot_4
        '
        Me._lblFloatTot_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_4.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_4.Name = "_lblFloatTot_4"
        Me._lblFloatTot_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_4.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_4.TabIndex = 147
        Me._lblFloatTot_4.Tag = "CAP;666"
        Me._lblFloatTot_4.Text = "Total"
        '
        '_lblFloatRem_4
        '
        Me._lblFloatRem_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_4.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_4.Name = "_lblFloatRem_4"
        Me._lblFloatRem_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_4.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_4.TabIndex = 146
        Me._lblFloatRem_4.Tag = "CAP;667"
        Me._lblFloatRem_4.Text = "Float Remaining"
        '
        '_fraCash_4
        '
        Me._fraCash_4.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_4.Controls.Add(Me._txtTotalCash_4)
        Me._fraCash_4.Controls.Add(Me._tdgCash_4)
        Me._fraCash_4.Controls.Add(Me._lblCashTot_4)
        Me._fraCash_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_4.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_4.Name = "_fraCash_4"
        Me._fraCash_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_4.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_4.TabIndex = 36
        Me._fraCash_4.TabStop = False
        Me._fraCash_4.Tag = "CAP;665"
        Me._fraCash_4.Text = "Cash Breakdown"
        Me._fraCash_4.Visible = False
        '
        '_txtTotalCash_4
        '
        Me._txtTotalCash_4.AcceptsReturn = True
        Me._txtTotalCash_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_4.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_4.MaxLength = 0
        Me._txtTotalCash_4.Name = "_txtTotalCash_4"
        Me._txtTotalCash_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_4.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_4.TabIndex = 38
        Me._txtTotalCash_4.TabStop = False
        Me._txtTotalCash_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_4
        '
        Me._tdgCash_4.AllowBigSelection = False
        Me._tdgCash_4.AllowRowSelection = False
        Me._tdgCash_4.AlternatingRows = False
        Me._tdgCash_4.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_4.ColumnsCount = 0
        Me._tdgCash_4.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_4.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_4.EvenStyle = DataGridViewCellStyle19
        Me._tdgCash_4.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_4.FixedColumns = -1
        Me._tdgCash_4.FixedRows = -1
        Me._tdgCash_4.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_4.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_4.GridLineWidth = 0
        Me._tdgCash_4.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_4.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_4.Name = "_tdgCash_4"
        Me._tdgCash_4.OddStyle = DataGridViewCellStyle20
        Me._tdgCash_4.RowHeightMin = 0
        Me._tdgCash_4.RowsCount = 0
        Me._tdgCash_4.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_4.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_4.SelectedStyle = Nothing
        Me._tdgCash_4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_4.SelLength = -1
        Me._tdgCash_4.SelStart = -1
        Me._tdgCash_4.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_4.TabIndex = 37
        Me._tdgCash_4.ToolTipText = ""
        '
        '_lblCashTot_4
        '
        Me._lblCashTot_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_4.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_4.Name = "_lblCashTot_4"
        Me._lblCashTot_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_4.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_4.TabIndex = 139
        Me._lblCashTot_4.Tag = "CAP;666"
        Me._lblCashTot_4.Text = "Total"
        '
        '_tabMediaTypes_TabPage5
        '
        Me._tabMediaTypes_TabPage5.Controls.Add(Me._lblTotalCC_5)
        Me._tabMediaTypes_TabPage5.Controls.Add(Me._txtTotalCC_5)
        Me._tabMediaTypes_TabPage5.Controls.Add(Me._fraFloat_5)
        Me._tabMediaTypes_TabPage5.Controls.Add(Me._fraCash_5)
        Me._tabMediaTypes_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage5.Name = "_tabMediaTypes_TabPage5"
        Me._tabMediaTypes_TabPage5.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage5.TabIndex = 5
        Me._tabMediaTypes_TabPage5.Text = "Tab 5"
        '
        '_lblTotalCC_5
        '
        Me._lblTotalCC_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_5.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_5.Name = "_lblTotalCC_5"
        Me._lblTotalCC_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_5.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_5.TabIndex = 128
        Me._lblTotalCC_5.Text = "Total"
        Me._lblTotalCC_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_5.Visible = False
        '
        '_txtTotalCC_5
        '
        Me._txtTotalCC_5.AcceptsReturn = True
        Me._txtTotalCC_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_5.Enabled = False
        Me._txtTotalCC_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_5.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_5.MaxLength = 0
        Me._txtTotalCC_5.Name = "_txtTotalCC_5"
        Me._txtTotalCC_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_5.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_5.TabIndex = 127
        Me._txtTotalCC_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_5.Visible = False
        '
        '_fraFloat_5
        '
        Me._fraFloat_5.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_5.Controls.Add(Me._txtTotalFloat_5)
        Me._fraFloat_5.Controls.Add(Me._txtFloatRem_5)
        Me._fraFloat_5.Controls.Add(Me._tdgFloat_5)
        Me._fraFloat_5.Controls.Add(Me._lblFloatTot_5)
        Me._fraFloat_5.Controls.Add(Me._lblFloatRem_5)
        Me._fraFloat_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_5.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_5.Name = "_fraFloat_5"
        Me._fraFloat_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_5.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_5.TabIndex = 39
        Me._fraFloat_5.TabStop = False
        Me._fraFloat_5.Tag = "CAP;664"
        Me._fraFloat_5.Text = "Float Breakdown"
        Me._fraFloat_5.Visible = False
        '
        '_txtTotalFloat_5
        '
        Me._txtTotalFloat_5.AcceptsReturn = True
        Me._txtTotalFloat_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_5.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_5.MaxLength = 0
        Me._txtTotalFloat_5.Name = "_txtTotalFloat_5"
        Me._txtTotalFloat_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_5.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_5.TabIndex = 41
        Me._txtTotalFloat_5.TabStop = False
        Me._txtTotalFloat_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_5
        '
        Me._txtFloatRem_5.AcceptsReturn = True
        Me._txtFloatRem_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_5.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_5.MaxLength = 0
        Me._txtFloatRem_5.Name = "_txtFloatRem_5"
        Me._txtFloatRem_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_5.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_5.TabIndex = 42
        Me._txtFloatRem_5.TabStop = False
        Me._txtFloatRem_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_5
        '
        Me._tdgFloat_5.AllowBigSelection = False
        Me._tdgFloat_5.AllowRowSelection = False
        Me._tdgFloat_5.AlternatingRows = False
        Me._tdgFloat_5.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_5.ColumnsCount = 0
        Me._tdgFloat_5.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_5.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_5.EvenStyle = DataGridViewCellStyle21
        Me._tdgFloat_5.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_5.FixedColumns = -1
        Me._tdgFloat_5.FixedRows = -1
        Me._tdgFloat_5.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_5.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_5.GridLineWidth = 0
        Me._tdgFloat_5.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_5.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_5.Name = "_tdgFloat_5"
        Me._tdgFloat_5.OddStyle = DataGridViewCellStyle22
        Me._tdgFloat_5.RowHeightMin = 0
        Me._tdgFloat_5.RowsCount = 0
        Me._tdgFloat_5.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_5.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_5.SelectedStyle = Nothing
        Me._tdgFloat_5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_5.SelLength = -1
        Me._tdgFloat_5.SelStart = -1
        Me._tdgFloat_5.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_5.TabIndex = 40
        Me._tdgFloat_5.ToolTipText = ""
        '
        '_lblFloatTot_5
        '
        Me._lblFloatTot_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_5.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_5.Name = "_lblFloatTot_5"
        Me._lblFloatTot_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_5.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_5.TabIndex = 43
        Me._lblFloatTot_5.Tag = "CAP;666"
        Me._lblFloatTot_5.Text = "Total"
        '
        '_lblFloatRem_5
        '
        Me._lblFloatRem_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_5.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_5.Name = "_lblFloatRem_5"
        Me._lblFloatRem_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_5.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_5.TabIndex = 44
        Me._lblFloatRem_5.Tag = "CAP;667"
        Me._lblFloatRem_5.Text = "Float Remaining"
        '
        '_fraCash_5
        '
        Me._fraCash_5.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_5.Controls.Add(Me._txtTotalCash_5)
        Me._fraCash_5.Controls.Add(Me._tdgCash_5)
        Me._fraCash_5.Controls.Add(Me._lblCashTot_5)
        Me._fraCash_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_5.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_5.Name = "_fraCash_5"
        Me._fraCash_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_5.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_5.TabIndex = 45
        Me._fraCash_5.TabStop = False
        Me._fraCash_5.Tag = "CAP;665"
        Me._fraCash_5.Text = "Cash Breakdown"
        Me._fraCash_5.Visible = False
        '
        '_txtTotalCash_5
        '
        Me._txtTotalCash_5.AcceptsReturn = True
        Me._txtTotalCash_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_5.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_5.MaxLength = 0
        Me._txtTotalCash_5.Name = "_txtTotalCash_5"
        Me._txtTotalCash_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_5.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_5.TabIndex = 47
        Me._txtTotalCash_5.TabStop = False
        Me._txtTotalCash_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_5
        '
        Me._tdgCash_5.AllowBigSelection = False
        Me._tdgCash_5.AllowRowSelection = False
        Me._tdgCash_5.AlternatingRows = False
        Me._tdgCash_5.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_5.ColumnsCount = 0
        Me._tdgCash_5.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_5.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_5.EvenStyle = DataGridViewCellStyle23
        Me._tdgCash_5.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_5.FixedColumns = -1
        Me._tdgCash_5.FixedRows = -1
        Me._tdgCash_5.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_5.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_5.GridLineWidth = 0
        Me._tdgCash_5.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_5.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_5.Name = "_tdgCash_5"
        Me._tdgCash_5.OddStyle = DataGridViewCellStyle24
        Me._tdgCash_5.RowHeightMin = 0
        Me._tdgCash_5.RowsCount = 0
        Me._tdgCash_5.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_5.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_5.SelectedStyle = Nothing
        Me._tdgCash_5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_5.SelLength = -1
        Me._tdgCash_5.SelStart = -1
        Me._tdgCash_5.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_5.TabIndex = 46
        Me._tdgCash_5.ToolTipText = ""
        '
        '_lblCashTot_5
        '
        Me._lblCashTot_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_5.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_5.Name = "_lblCashTot_5"
        Me._lblCashTot_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_5.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_5.TabIndex = 144
        Me._lblCashTot_5.Tag = "CAP;666"
        Me._lblCashTot_5.Text = "Total"
        '
        '_tabMediaTypes_TabPage6
        '
        Me._tabMediaTypes_TabPage6.Controls.Add(Me._lblTotalCC_6)
        Me._tabMediaTypes_TabPage6.Controls.Add(Me._txtTotalCC_6)
        Me._tabMediaTypes_TabPage6.Controls.Add(Me._fraFloat_6)
        Me._tabMediaTypes_TabPage6.Controls.Add(Me._fraCash_6)
        Me._tabMediaTypes_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage6.Name = "_tabMediaTypes_TabPage6"
        Me._tabMediaTypes_TabPage6.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage6.TabIndex = 6
        Me._tabMediaTypes_TabPage6.Text = "Tab 6"
        '
        '_lblTotalCC_6
        '
        Me._lblTotalCC_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_6.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_6.Name = "_lblTotalCC_6"
        Me._lblTotalCC_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_6.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_6.TabIndex = 130
        Me._lblTotalCC_6.Text = "Total"
        Me._lblTotalCC_6.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_6.Visible = False
        '
        '_txtTotalCC_6
        '
        Me._txtTotalCC_6.AcceptsReturn = True
        Me._txtTotalCC_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_6.Enabled = False
        Me._txtTotalCC_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_6.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_6.MaxLength = 0
        Me._txtTotalCC_6.Name = "_txtTotalCC_6"
        Me._txtTotalCC_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_6.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_6.TabIndex = 129
        Me._txtTotalCC_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_6.Visible = False
        '
        '_fraFloat_6
        '
        Me._fraFloat_6.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_6.Controls.Add(Me._txtTotalFloat_6)
        Me._fraFloat_6.Controls.Add(Me._txtFloatRem_6)
        Me._fraFloat_6.Controls.Add(Me._tdgFloat_6)
        Me._fraFloat_6.Controls.Add(Me._lblFloatTot_6)
        Me._fraFloat_6.Controls.Add(Me._lblFloatRem_6)
        Me._fraFloat_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_6.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_6.Name = "_fraFloat_6"
        Me._fraFloat_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_6.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_6.TabIndex = 48
        Me._fraFloat_6.TabStop = False
        Me._fraFloat_6.Tag = "CAP;664"
        Me._fraFloat_6.Text = "Float Breakdown"
        Me._fraFloat_6.Visible = False
        '
        '_txtTotalFloat_6
        '
        Me._txtTotalFloat_6.AcceptsReturn = True
        Me._txtTotalFloat_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_6.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_6.MaxLength = 0
        Me._txtTotalFloat_6.Name = "_txtTotalFloat_6"
        Me._txtTotalFloat_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_6.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_6.TabIndex = 50
        Me._txtTotalFloat_6.TabStop = False
        Me._txtTotalFloat_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_6
        '
        Me._txtFloatRem_6.AcceptsReturn = True
        Me._txtFloatRem_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_6.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_6.MaxLength = 0
        Me._txtFloatRem_6.Name = "_txtFloatRem_6"
        Me._txtFloatRem_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_6.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_6.TabIndex = 51
        Me._txtFloatRem_6.TabStop = False
        Me._txtFloatRem_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_6
        '
        Me._tdgFloat_6.AllowBigSelection = False
        Me._tdgFloat_6.AllowRowSelection = False
        Me._tdgFloat_6.AlternatingRows = False
        Me._tdgFloat_6.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_6.ColumnsCount = 0
        Me._tdgFloat_6.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_6.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_6.EvenStyle = DataGridViewCellStyle25
        Me._tdgFloat_6.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_6.FixedColumns = -1
        Me._tdgFloat_6.FixedRows = -1
        Me._tdgFloat_6.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_6.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_6.GridLineWidth = 0
        Me._tdgFloat_6.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_6.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_6.Name = "_tdgFloat_6"
        Me._tdgFloat_6.OddStyle = DataGridViewCellStyle26
        Me._tdgFloat_6.RowHeightMin = 0
        Me._tdgFloat_6.RowsCount = 0
        Me._tdgFloat_6.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_6.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_6.SelectedStyle = Nothing
        Me._tdgFloat_6.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_6.SelLength = -1
        Me._tdgFloat_6.SelStart = -1
        Me._tdgFloat_6.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_6.TabIndex = 49
        Me._tdgFloat_6.ToolTipText = ""
        '
        '_lblFloatTot_6
        '
        Me._lblFloatTot_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_6.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_6.Name = "_lblFloatTot_6"
        Me._lblFloatTot_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_6.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_6.TabIndex = 52
        Me._lblFloatTot_6.Tag = "CAP;666"
        Me._lblFloatTot_6.Text = "Total"
        '
        '_lblFloatRem_6
        '
        Me._lblFloatRem_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_6.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_6.Name = "_lblFloatRem_6"
        Me._lblFloatRem_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_6.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_6.TabIndex = 53
        Me._lblFloatRem_6.Tag = "CAP;667"
        Me._lblFloatRem_6.Text = "Float Remaining"
        '
        '_fraCash_6
        '
        Me._fraCash_6.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_6.Controls.Add(Me._txtTotalCash_6)
        Me._fraCash_6.Controls.Add(Me._tdgCash_6)
        Me._fraCash_6.Controls.Add(Me._lblCashTot_6)
        Me._fraCash_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_6.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_6.Name = "_fraCash_6"
        Me._fraCash_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_6.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_6.TabIndex = 54
        Me._fraCash_6.TabStop = False
        Me._fraCash_6.Tag = "CAP;665"
        Me._fraCash_6.Text = "Cash Breakdown"
        Me._fraCash_6.Visible = False
        '
        '_txtTotalCash_6
        '
        Me._txtTotalCash_6.AcceptsReturn = True
        Me._txtTotalCash_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_6.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_6.MaxLength = 0
        Me._txtTotalCash_6.Name = "_txtTotalCash_6"
        Me._txtTotalCash_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_6.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_6.TabIndex = 56
        Me._txtTotalCash_6.TabStop = False
        Me._txtTotalCash_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_6
        '
        Me._tdgCash_6.AllowBigSelection = False
        Me._tdgCash_6.AllowRowSelection = False
        Me._tdgCash_6.AlternatingRows = False
        Me._tdgCash_6.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_6.ColumnsCount = 0
        Me._tdgCash_6.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_6.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_6.EvenStyle = DataGridViewCellStyle27
        Me._tdgCash_6.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_6.FixedColumns = -1
        Me._tdgCash_6.FixedRows = -1
        Me._tdgCash_6.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_6.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_6.GridLineWidth = 0
        Me._tdgCash_6.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_6.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_6.Name = "_tdgCash_6"
        Me._tdgCash_6.OddStyle = DataGridViewCellStyle28
        Me._tdgCash_6.RowHeightMin = 0
        Me._tdgCash_6.RowsCount = 0
        Me._tdgCash_6.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_6.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_6.SelectedStyle = Nothing
        Me._tdgCash_6.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_6.SelLength = -1
        Me._tdgCash_6.SelStart = -1
        Me._tdgCash_6.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_6.TabIndex = 55
        Me._tdgCash_6.ToolTipText = ""
        '
        '_lblCashTot_6
        '
        Me._lblCashTot_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_6.Location = New System.Drawing.Point(96, 212)
        Me._lblCashTot_6.Name = "_lblCashTot_6"
        Me._lblCashTot_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_6.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_6.TabIndex = 140
        Me._lblCashTot_6.Tag = "CAP;666"
        Me._lblCashTot_6.Text = "Total"
        '
        '_tabMediaTypes_TabPage7
        '
        Me._tabMediaTypes_TabPage7.Controls.Add(Me._fraCash_7)
        Me._tabMediaTypes_TabPage7.Controls.Add(Me._fraFloat_7)
        Me._tabMediaTypes_TabPage7.Controls.Add(Me._txtTotalCC_7)
        Me._tabMediaTypes_TabPage7.Controls.Add(Me._lblTotalCC_7)
        Me._tabMediaTypes_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage7.Name = "_tabMediaTypes_TabPage7"
        Me._tabMediaTypes_TabPage7.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage7.TabIndex = 7
        Me._tabMediaTypes_TabPage7.Text = "Tab 7"
        '
        '_fraCash_7
        '
        Me._fraCash_7.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_7.Controls.Add(Me._txtTotalCash_7)
        Me._fraCash_7.Controls.Add(Me._tdgCash_7)
        Me._fraCash_7.Controls.Add(Me._lblCashTot_7)
        Me._fraCash_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_7.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_7.Name = "_fraCash_7"
        Me._fraCash_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_7.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_7.TabIndex = 63
        Me._fraCash_7.TabStop = False
        Me._fraCash_7.Tag = "CAP;665"
        Me._fraCash_7.Text = "Cash Breakdown"
        Me._fraCash_7.Visible = False
        '
        '_txtTotalCash_7
        '
        Me._txtTotalCash_7.AcceptsReturn = True
        Me._txtTotalCash_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_7.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_7.MaxLength = 0
        Me._txtTotalCash_7.Name = "_txtTotalCash_7"
        Me._txtTotalCash_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_7.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_7.TabIndex = 65
        Me._txtTotalCash_7.TabStop = False
        Me._txtTotalCash_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_7
        '
        Me._tdgCash_7.AllowBigSelection = False
        Me._tdgCash_7.AllowRowSelection = False
        Me._tdgCash_7.AlternatingRows = False
        Me._tdgCash_7.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_7.ColumnsCount = 0
        Me._tdgCash_7.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_7.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_7.EvenStyle = DataGridViewCellStyle29
        Me._tdgCash_7.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_7.FixedColumns = -1
        Me._tdgCash_7.FixedRows = -1
        Me._tdgCash_7.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_7.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_7.GridLineWidth = 0
        Me._tdgCash_7.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_7.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_7.Name = "_tdgCash_7"
        Me._tdgCash_7.OddStyle = DataGridViewCellStyle30
        Me._tdgCash_7.RowHeightMin = 0
        Me._tdgCash_7.RowsCount = 0
        Me._tdgCash_7.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_7.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_7.SelectedStyle = Nothing
        Me._tdgCash_7.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_7.SelLength = -1
        Me._tdgCash_7.SelStart = -1
        Me._tdgCash_7.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_7.TabIndex = 64
        Me._tdgCash_7.ToolTipText = ""
        '
        '_lblCashTot_7
        '
        Me._lblCashTot_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_7.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_7.Name = "_lblCashTot_7"
        Me._lblCashTot_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_7.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_7.TabIndex = 141
        Me._lblCashTot_7.Tag = "CAP;666"
        Me._lblCashTot_7.Text = "Total"
        '
        '_fraFloat_7
        '
        Me._fraFloat_7.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_7.Controls.Add(Me._txtTotalFloat_7)
        Me._fraFloat_7.Controls.Add(Me._txtFloatRem_7)
        Me._fraFloat_7.Controls.Add(Me._tdgFloat_7)
        Me._fraFloat_7.Controls.Add(Me._lblFloatTot_7)
        Me._fraFloat_7.Controls.Add(Me._lblFloatRem_7)
        Me._fraFloat_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_7.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_7.Name = "_fraFloat_7"
        Me._fraFloat_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_7.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_7.TabIndex = 57
        Me._fraFloat_7.TabStop = False
        Me._fraFloat_7.Tag = "CAP;664"
        Me._fraFloat_7.Text = "Float Breakdown"
        Me._fraFloat_7.Visible = False
        '
        '_txtTotalFloat_7
        '
        Me._txtTotalFloat_7.AcceptsReturn = True
        Me._txtTotalFloat_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_7.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_7.MaxLength = 0
        Me._txtTotalFloat_7.Name = "_txtTotalFloat_7"
        Me._txtTotalFloat_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_7.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_7.TabIndex = 59
        Me._txtTotalFloat_7.TabStop = False
        Me._txtTotalFloat_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_7
        '
        Me._txtFloatRem_7.AcceptsReturn = True
        Me._txtFloatRem_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_7.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_7.MaxLength = 0
        Me._txtFloatRem_7.Name = "_txtFloatRem_7"
        Me._txtFloatRem_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_7.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_7.TabIndex = 60
        Me._txtFloatRem_7.TabStop = False
        Me._txtFloatRem_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_7
        '
        Me._tdgFloat_7.AllowBigSelection = False
        Me._tdgFloat_7.AllowRowSelection = False
        Me._tdgFloat_7.AlternatingRows = False
        Me._tdgFloat_7.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_7.ColumnsCount = 0
        Me._tdgFloat_7.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_7.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_7.EvenStyle = DataGridViewCellStyle31
        Me._tdgFloat_7.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_7.FixedColumns = -1
        Me._tdgFloat_7.FixedRows = -1
        Me._tdgFloat_7.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_7.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_7.GridLineWidth = 0
        Me._tdgFloat_7.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_7.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_7.Name = "_tdgFloat_7"
        Me._tdgFloat_7.OddStyle = DataGridViewCellStyle32
        Me._tdgFloat_7.RowHeightMin = 0
        Me._tdgFloat_7.RowsCount = 0
        Me._tdgFloat_7.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_7.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_7.SelectedStyle = Nothing
        Me._tdgFloat_7.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_7.SelLength = -1
        Me._tdgFloat_7.SelStart = -1
        Me._tdgFloat_7.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_7.TabIndex = 58
        Me._tdgFloat_7.ToolTipText = ""
        '
        '_lblFloatTot_7
        '
        Me._lblFloatTot_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_7.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_7.Name = "_lblFloatTot_7"
        Me._lblFloatTot_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_7.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_7.TabIndex = 61
        Me._lblFloatTot_7.Tag = "CAP;666"
        Me._lblFloatTot_7.Text = "Total"
        '
        '_lblFloatRem_7
        '
        Me._lblFloatRem_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_7.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_7.Name = "_lblFloatRem_7"
        Me._lblFloatRem_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_7.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_7.TabIndex = 62
        Me._lblFloatRem_7.Tag = "CAP;667"
        Me._lblFloatRem_7.Text = "Float Remaining"
        '
        '_txtTotalCC_7
        '
        Me._txtTotalCC_7.AcceptsReturn = True
        Me._txtTotalCC_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_7.Enabled = False
        Me._txtTotalCC_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_7.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_7.MaxLength = 0
        Me._txtTotalCC_7.Name = "_txtTotalCC_7"
        Me._txtTotalCC_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_7.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_7.TabIndex = 131
        Me._txtTotalCC_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_7.Visible = False
        '
        '_lblTotalCC_7
        '
        Me._lblTotalCC_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_7.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_7.Name = "_lblTotalCC_7"
        Me._lblTotalCC_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_7.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_7.TabIndex = 132
        Me._lblTotalCC_7.Text = "Total"
        Me._lblTotalCC_7.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_7.Visible = False
        '
        '_tabMediaTypes_TabPage8
        '
        Me._tabMediaTypes_TabPage8.Controls.Add(Me._lblTotalCC_8)
        Me._tabMediaTypes_TabPage8.Controls.Add(Me._txtTotalCC_8)
        Me._tabMediaTypes_TabPage8.Controls.Add(Me._fraFloat_8)
        Me._tabMediaTypes_TabPage8.Controls.Add(Me._fraCash_8)
        Me._tabMediaTypes_TabPage8.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage8.Name = "_tabMediaTypes_TabPage8"
        Me._tabMediaTypes_TabPage8.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage8.TabIndex = 8
        Me._tabMediaTypes_TabPage8.Text = "Tab 8"
        '
        '_lblTotalCC_8
        '
        Me._lblTotalCC_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_8.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_8.Name = "_lblTotalCC_8"
        Me._lblTotalCC_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_8.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_8.TabIndex = 134
        Me._lblTotalCC_8.Text = "Total"
        Me._lblTotalCC_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_8.Visible = False
        '
        '_txtTotalCC_8
        '
        Me._txtTotalCC_8.AcceptsReturn = True
        Me._txtTotalCC_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_8.Enabled = False
        Me._txtTotalCC_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_8.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_8.MaxLength = 0
        Me._txtTotalCC_8.Name = "_txtTotalCC_8"
        Me._txtTotalCC_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_8.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_8.TabIndex = 133
        Me._txtTotalCC_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_8.Visible = False
        '
        '_fraFloat_8
        '
        Me._fraFloat_8.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_8.Controls.Add(Me._txtTotalFloat_8)
        Me._fraFloat_8.Controls.Add(Me._txtFloatRem_8)
        Me._fraFloat_8.Controls.Add(Me._tdgFloat_8)
        Me._fraFloat_8.Controls.Add(Me._lblFloatTot_8)
        Me._fraFloat_8.Controls.Add(Me._lblFloatRem_8)
        Me._fraFloat_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_8.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_8.Name = "_fraFloat_8"
        Me._fraFloat_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_8.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_8.TabIndex = 66
        Me._fraFloat_8.TabStop = False
        Me._fraFloat_8.Tag = "CAP;664"
        Me._fraFloat_8.Text = "Float Breakdown"
        Me._fraFloat_8.Visible = False
        '
        '_txtTotalFloat_8
        '
        Me._txtTotalFloat_8.AcceptsReturn = True
        Me._txtTotalFloat_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_8.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_8.MaxLength = 0
        Me._txtTotalFloat_8.Name = "_txtTotalFloat_8"
        Me._txtTotalFloat_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_8.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_8.TabIndex = 68
        Me._txtTotalFloat_8.TabStop = False
        Me._txtTotalFloat_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_8
        '
        Me._txtFloatRem_8.AcceptsReturn = True
        Me._txtFloatRem_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_8.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_8.MaxLength = 0
        Me._txtFloatRem_8.Name = "_txtFloatRem_8"
        Me._txtFloatRem_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_8.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_8.TabIndex = 69
        Me._txtFloatRem_8.TabStop = False
        Me._txtFloatRem_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_8
        '
        Me._tdgFloat_8.AllowBigSelection = False
        Me._tdgFloat_8.AllowRowSelection = False
        Me._tdgFloat_8.AlternatingRows = False
        Me._tdgFloat_8.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_8.ColumnsCount = 0
        Me._tdgFloat_8.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_8.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_8.EvenStyle = DataGridViewCellStyle33
        Me._tdgFloat_8.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_8.FixedColumns = -1
        Me._tdgFloat_8.FixedRows = -1
        Me._tdgFloat_8.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_8.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_8.GridLineWidth = 0
        Me._tdgFloat_8.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_8.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_8.Name = "_tdgFloat_8"
        Me._tdgFloat_8.OddStyle = DataGridViewCellStyle34
        Me._tdgFloat_8.RowHeightMin = 0
        Me._tdgFloat_8.RowsCount = 0
        Me._tdgFloat_8.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_8.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_8.SelectedStyle = Nothing
        Me._tdgFloat_8.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_8.SelLength = -1
        Me._tdgFloat_8.SelStart = -1
        Me._tdgFloat_8.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_8.TabIndex = 67
        Me._tdgFloat_8.ToolTipText = ""
        '
        '_lblFloatTot_8
        '
        Me._lblFloatTot_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_8.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_8.Name = "_lblFloatTot_8"
        Me._lblFloatTot_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_8.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_8.TabIndex = 70
        Me._lblFloatTot_8.Tag = "CAP;666"
        Me._lblFloatTot_8.Text = "Total"
        '
        '_lblFloatRem_8
        '
        Me._lblFloatRem_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_8.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_8.Name = "_lblFloatRem_8"
        Me._lblFloatRem_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_8.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_8.TabIndex = 71
        Me._lblFloatRem_8.Tag = "CAP;667"
        Me._lblFloatRem_8.Text = "Float Remaining"
        '
        '_fraCash_8
        '
        Me._fraCash_8.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_8.Controls.Add(Me._txtTotalCash_8)
        Me._fraCash_8.Controls.Add(Me._tdgCash_8)
        Me._fraCash_8.Controls.Add(Me._lblCashTot_8)
        Me._fraCash_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_8.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_8.Name = "_fraCash_8"
        Me._fraCash_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_8.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_8.TabIndex = 72
        Me._fraCash_8.TabStop = False
        Me._fraCash_8.Tag = "CAP;665"
        Me._fraCash_8.Text = "Cash Breakdown"
        Me._fraCash_8.Visible = False
        '
        '_txtTotalCash_8
        '
        Me._txtTotalCash_8.AcceptsReturn = True
        Me._txtTotalCash_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_8.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_8.MaxLength = 0
        Me._txtTotalCash_8.Name = "_txtTotalCash_8"
        Me._txtTotalCash_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_8.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_8.TabIndex = 74
        Me._txtTotalCash_8.TabStop = False
        Me._txtTotalCash_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_8
        '
        Me._tdgCash_8.AllowBigSelection = False
        Me._tdgCash_8.AllowRowSelection = False
        Me._tdgCash_8.AlternatingRows = False
        Me._tdgCash_8.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_8.ColumnsCount = 0
        Me._tdgCash_8.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_8.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_8.EvenStyle = DataGridViewCellStyle35
        Me._tdgCash_8.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_8.FixedColumns = -1
        Me._tdgCash_8.FixedRows = -1
        Me._tdgCash_8.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_8.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_8.GridLineWidth = 0
        Me._tdgCash_8.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_8.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_8.Name = "_tdgCash_8"
        Me._tdgCash_8.OddStyle = DataGridViewCellStyle36
        Me._tdgCash_8.RowHeightMin = 0
        Me._tdgCash_8.RowsCount = 0
        Me._tdgCash_8.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_8.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_8.SelectedStyle = Nothing
        Me._tdgCash_8.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_8.SelLength = -1
        Me._tdgCash_8.SelStart = -1
        Me._tdgCash_8.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_8.TabIndex = 73
        Me._tdgCash_8.ToolTipText = ""
        '
        '_lblCashTot_8
        '
        Me._lblCashTot_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_8.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_8.Name = "_lblCashTot_8"
        Me._lblCashTot_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_8.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_8.TabIndex = 142
        Me._lblCashTot_8.Tag = "CAP;666"
        Me._lblCashTot_8.Text = "Total"
        '
        '_tabMediaTypes_TabPage9
        '
        Me._tabMediaTypes_TabPage9.Controls.Add(Me._fraCash_9)
        Me._tabMediaTypes_TabPage9.Controls.Add(Me._fraFloat_9)
        Me._tabMediaTypes_TabPage9.Controls.Add(Me._txtTotalCC_9)
        Me._tabMediaTypes_TabPage9.Controls.Add(Me._lblTotalCC_9)
        Me._tabMediaTypes_TabPage9.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage9.Name = "_tabMediaTypes_TabPage9"
        Me._tabMediaTypes_TabPage9.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage9.TabIndex = 9
        Me._tabMediaTypes_TabPage9.Text = "Tab 9"
        '
        '_fraCash_9
        '
        Me._fraCash_9.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_9.Controls.Add(Me._txtTotalCash_9)
        Me._fraCash_9.Controls.Add(Me._tdgCash_9)
        Me._fraCash_9.Controls.Add(Me._lblCashTot_9)
        Me._fraCash_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_9.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_9.Name = "_fraCash_9"
        Me._fraCash_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_9.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_9.TabIndex = 81
        Me._fraCash_9.TabStop = False
        Me._fraCash_9.Tag = "CAP;665"
        Me._fraCash_9.Text = "Cash Breakdown"
        Me._fraCash_9.Visible = False
        '
        '_txtTotalCash_9
        '
        Me._txtTotalCash_9.AcceptsReturn = True
        Me._txtTotalCash_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_9.Location = New System.Drawing.Point(152, 208)
        Me._txtTotalCash_9.MaxLength = 0
        Me._txtTotalCash_9.Name = "_txtTotalCash_9"
        Me._txtTotalCash_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_9.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_9.TabIndex = 83
        Me._txtTotalCash_9.TabStop = False
        Me._txtTotalCash_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_9
        '
        Me._tdgCash_9.AllowBigSelection = False
        Me._tdgCash_9.AllowRowSelection = False
        Me._tdgCash_9.AlternatingRows = False
        Me._tdgCash_9.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_9.ColumnsCount = 0
        Me._tdgCash_9.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_9.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_9.EvenStyle = DataGridViewCellStyle37
        Me._tdgCash_9.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_9.FixedColumns = -1
        Me._tdgCash_9.FixedRows = -1
        Me._tdgCash_9.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_9.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_9.GridLineWidth = 0
        Me._tdgCash_9.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_9.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_9.Name = "_tdgCash_9"
        Me._tdgCash_9.OddStyle = DataGridViewCellStyle38
        Me._tdgCash_9.RowHeightMin = 0
        Me._tdgCash_9.RowsCount = 0
        Me._tdgCash_9.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_9.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_9.SelectedStyle = Nothing
        Me._tdgCash_9.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_9.SelLength = -1
        Me._tdgCash_9.SelStart = -1
        Me._tdgCash_9.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_9.TabIndex = 82
        Me._tdgCash_9.ToolTipText = ""
        '
        '_lblCashTot_9
        '
        Me._lblCashTot_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_9.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_9.Name = "_lblCashTot_9"
        Me._lblCashTot_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_9.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_9.TabIndex = 143
        Me._lblCashTot_9.Tag = "CAP;666"
        Me._lblCashTot_9.Text = "Total"
        '
        '_fraFloat_9
        '
        Me._fraFloat_9.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_9.Controls.Add(Me._txtTotalFloat_9)
        Me._fraFloat_9.Controls.Add(Me._txtFloatRem_9)
        Me._fraFloat_9.Controls.Add(Me._tdgFloat_9)
        Me._fraFloat_9.Controls.Add(Me._lblFloatTot_9)
        Me._fraFloat_9.Controls.Add(Me._lblFloatRem_9)
        Me._fraFloat_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_9.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_9.Name = "_fraFloat_9"
        Me._fraFloat_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_9.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_9.TabIndex = 75
        Me._fraFloat_9.TabStop = False
        Me._fraFloat_9.Tag = "CAP;664"
        Me._fraFloat_9.Text = "Float Breakdown"
        Me._fraFloat_9.Visible = False
        '
        '_txtTotalFloat_9
        '
        Me._txtTotalFloat_9.AcceptsReturn = True
        Me._txtTotalFloat_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_9.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_9.MaxLength = 0
        Me._txtTotalFloat_9.Name = "_txtTotalFloat_9"
        Me._txtTotalFloat_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_9.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_9.TabIndex = 77
        Me._txtTotalFloat_9.TabStop = False
        Me._txtTotalFloat_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtFloatRem_9
        '
        Me._txtFloatRem_9.AcceptsReturn = True
        Me._txtFloatRem_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_9.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_9.MaxLength = 0
        Me._txtFloatRem_9.Name = "_txtFloatRem_9"
        Me._txtFloatRem_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_9.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_9.TabIndex = 78
        Me._txtFloatRem_9.TabStop = False
        Me._txtFloatRem_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_9
        '
        Me._tdgFloat_9.AllowBigSelection = False
        Me._tdgFloat_9.AllowRowSelection = False
        Me._tdgFloat_9.AlternatingRows = False
        Me._tdgFloat_9.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_9.ColumnsCount = 0
        Me._tdgFloat_9.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_9.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_9.EvenStyle = DataGridViewCellStyle39
        Me._tdgFloat_9.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_9.FixedColumns = -1
        Me._tdgFloat_9.FixedRows = -1
        Me._tdgFloat_9.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_9.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_9.GridLineWidth = 0
        Me._tdgFloat_9.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_9.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_9.Name = "_tdgFloat_9"
        Me._tdgFloat_9.OddStyle = DataGridViewCellStyle40
        Me._tdgFloat_9.RowHeightMin = 0
        Me._tdgFloat_9.RowsCount = 0
        Me._tdgFloat_9.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_9.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_9.SelectedStyle = Nothing
        Me._tdgFloat_9.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_9.SelLength = -1
        Me._tdgFloat_9.SelStart = -1
        Me._tdgFloat_9.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_9.TabIndex = 76
        Me._tdgFloat_9.ToolTipText = ""
        '
        '_lblFloatTot_9
        '
        Me._lblFloatTot_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_9.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_9.Name = "_lblFloatTot_9"
        Me._lblFloatTot_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_9.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_9.TabIndex = 79
        Me._lblFloatTot_9.Tag = "CAP;666"
        Me._lblFloatTot_9.Text = "Total"
        '
        '_lblFloatRem_9
        '
        Me._lblFloatRem_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_9.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_9.Name = "_lblFloatRem_9"
        Me._lblFloatRem_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_9.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_9.TabIndex = 80
        Me._lblFloatRem_9.Tag = "CAP;667"
        Me._lblFloatRem_9.Text = "Float Remaining"
        '
        '_txtTotalCC_9
        '
        Me._txtTotalCC_9.AcceptsReturn = True
        Me._txtTotalCC_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_9.Enabled = False
        Me._txtTotalCC_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_9.Location = New System.Drawing.Point(264, 96)
        Me._txtTotalCC_9.MaxLength = 0
        Me._txtTotalCC_9.Name = "_txtTotalCC_9"
        Me._txtTotalCC_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_9.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_9.TabIndex = 135
        Me._txtTotalCC_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_9.Visible = False
        '
        '_lblTotalCC_9
        '
        Me._lblTotalCC_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalCC_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalCC_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalCC_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalCC_9.Location = New System.Drawing.Point(152, 100)
        Me._lblTotalCC_9.Name = "_lblTotalCC_9"
        Me._lblTotalCC_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalCC_9.Size = New System.Drawing.Size(97, 17)
        Me._lblTotalCC_9.TabIndex = 136
        Me._lblTotalCC_9.Text = "Total"
        Me._lblTotalCC_9.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblTotalCC_9.Visible = False
        '
        '_tabMediaTypes_TabPage10
        '
        Me._tabMediaTypes_TabPage10.Controls.Add(Me._fraCash_10)
        Me._tabMediaTypes_TabPage10.Controls.Add(Me._fraFloat_10)
        Me._tabMediaTypes_TabPage10.Controls.Add(Me._txtTotalCC_10)
        Me._tabMediaTypes_TabPage10.Location = New System.Drawing.Point(4, 22)
        Me._tabMediaTypes_TabPage10.Name = "_tabMediaTypes_TabPage10"
        Me._tabMediaTypes_TabPage10.Size = New System.Drawing.Size(557, 333)
        Me._tabMediaTypes_TabPage10.TabIndex = 10
        Me._tabMediaTypes_TabPage10.Text = "Tab 10"
        '
        '_fraCash_10
        '
        Me._fraCash_10.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_10.Controls.Add(Me._txtTotalCash_10)
        Me._fraCash_10.Controls.Add(Me._tdgCash_10)
        Me._fraCash_10.Controls.Add(Me._lblCashTot_10)
        Me._fraCash_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_10.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_10.Name = "_fraCash_10"
        Me._fraCash_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_10.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_10.TabIndex = 153
        Me._fraCash_10.TabStop = False
        Me._fraCash_10.Tag = "CAP;665"
        Me._fraCash_10.Text = "Cash Breakdown"
        Me._fraCash_10.Visible = False
        '
        '_txtTotalCash_10
        '
        Me._txtTotalCash_10.AcceptsReturn = True
        Me._txtTotalCash_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_10.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_10.MaxLength = 0
        Me._txtTotalCash_10.Name = "_txtTotalCash_10"
        Me._txtTotalCash_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_10.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_10.TabIndex = 154
        Me._txtTotalCash_10.TabStop = False
        Me._txtTotalCash_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_10
        '
        Me._tdgCash_10.AllowBigSelection = False
        Me._tdgCash_10.AllowRowSelection = False
        Me._tdgCash_10.AlternatingRows = False
        Me._tdgCash_10.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_10.ColumnsCount = 0
        Me._tdgCash_10.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_10.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_10.EvenStyle = DataGridViewCellStyle41
        Me._tdgCash_10.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_10.FixedColumns = -1
        Me._tdgCash_10.FixedRows = -1
        Me._tdgCash_10.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_10.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_10.GridLineWidth = 0
        Me._tdgCash_10.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_10.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_10.Name = "_tdgCash_10"
        Me._tdgCash_10.OddStyle = DataGridViewCellStyle42
        Me._tdgCash_10.RowHeightMin = 0
        Me._tdgCash_10.RowsCount = 0
        Me._tdgCash_10.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_10.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_10.SelectedStyle = Nothing
        Me._tdgCash_10.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_10.SelLength = -1
        Me._tdgCash_10.SelStart = -1
        Me._tdgCash_10.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_10.TabIndex = 155
        Me._tdgCash_10.ToolTipText = ""
        '
        '_lblCashTot_10
        '
        Me._lblCashTot_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_10.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_10.Name = "_lblCashTot_10"
        Me._lblCashTot_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_10.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_10.TabIndex = 156
        Me._lblCashTot_10.Tag = "CAP;666"
        Me._lblCashTot_10.Text = "Total"
        '
        '_fraFloat_10
        '
        Me._fraFloat_10.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_10.Controls.Add(Me._txtFloatRem_10)
        Me._fraFloat_10.Controls.Add(Me._txtTotalFloat_10)
        Me._fraFloat_10.Controls.Add(Me._tdgFloat_10)
        Me._fraFloat_10.Controls.Add(Me._lblFloatRem_10)
        Me._fraFloat_10.Controls.Add(Me._lblFloatTot_10)
        Me._fraFloat_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_10.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_10.Name = "_fraFloat_10"
        Me._fraFloat_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_10.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_10.TabIndex = 157
        Me._fraFloat_10.TabStop = False
        Me._fraFloat_10.Tag = "CAP;664"
        Me._fraFloat_10.Text = "Float Breakdown"
        Me._fraFloat_10.Visible = False
        '
        '_txtFloatRem_10
        '
        Me._txtFloatRem_10.AcceptsReturn = True
        Me._txtFloatRem_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_10.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_10.MaxLength = 0
        Me._txtFloatRem_10.Name = "_txtFloatRem_10"
        Me._txtFloatRem_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_10.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_10.TabIndex = 159
        Me._txtFloatRem_10.TabStop = False
        Me._txtFloatRem_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_10
        '
        Me._txtTotalFloat_10.AcceptsReturn = True
        Me._txtTotalFloat_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_10.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_10.MaxLength = 0
        Me._txtTotalFloat_10.Name = "_txtTotalFloat_10"
        Me._txtTotalFloat_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_10.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_10.TabIndex = 158
        Me._txtTotalFloat_10.TabStop = False
        Me._txtTotalFloat_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_10
        '
        Me._tdgFloat_10.AllowBigSelection = False
        Me._tdgFloat_10.AllowRowSelection = False
        Me._tdgFloat_10.AlternatingRows = False
        Me._tdgFloat_10.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_10.ColumnsCount = 0
        Me._tdgFloat_10.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_10.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_10.EvenStyle = DataGridViewCellStyle43
        Me._tdgFloat_10.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_10.FixedColumns = -1
        Me._tdgFloat_10.FixedRows = -1
        Me._tdgFloat_10.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_10.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_10.GridLineWidth = 0
        Me._tdgFloat_10.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_10.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_10.Name = "_tdgFloat_10"
        Me._tdgFloat_10.OddStyle = DataGridViewCellStyle44
        Me._tdgFloat_10.RowHeightMin = 0
        Me._tdgFloat_10.RowsCount = 0
        Me._tdgFloat_10.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_10.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_10.SelectedStyle = Nothing
        Me._tdgFloat_10.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_10.SelLength = -1
        Me._tdgFloat_10.SelStart = -1
        Me._tdgFloat_10.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_10.TabIndex = 160
        Me._tdgFloat_10.ToolTipText = ""
        '
        '_lblFloatRem_10
        '
        Me._lblFloatRem_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_10.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_10.Name = "_lblFloatRem_10"
        Me._lblFloatRem_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_10.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_10.TabIndex = 162
        Me._lblFloatRem_10.Tag = "CAP;667"
        Me._lblFloatRem_10.Text = "Float Remaining"
        '
        '_lblFloatTot_10
        '
        Me._lblFloatTot_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_10.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_10.Name = "_lblFloatTot_10"
        Me._lblFloatTot_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_10.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_10.TabIndex = 161
        Me._lblFloatTot_10.Tag = "CAP;666"
        Me._lblFloatTot_10.Text = "Total"
        '
        '_txtTotalCC_10
        '
        Me._txtTotalCC_10.AcceptsReturn = True
        Me._txtTotalCC_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_10.Enabled = False
        Me._txtTotalCC_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_10.Location = New System.Drawing.Point(256, 100)
        Me._txtTotalCC_10.MaxLength = 0
        Me._txtTotalCC_10.Name = "_txtTotalCC_10"
        Me._txtTotalCC_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_10.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_10.TabIndex = 152
        Me._txtTotalCC_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_10.Visible = False
        '
        '_tabMediaTypes_TabPage11
        '
        Me._tabMediaTypes_TabPage11.Controls.Add(Me._fraCash_11)
        Me._tabMediaTypes_TabPage11.Controls.Add(Me._fraFloat_11)
        Me._tabMediaTypes_TabPage11.Controls.Add(Me._txtTotalCC_11)
        Me._tabMediaTypes_TabPage11.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage11.Name = "_tabMediaTypes_TabPage11"
        Me._tabMediaTypes_TabPage11.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage11.TabIndex = 11
        Me._tabMediaTypes_TabPage11.Text = "Tab 11"
        '
        '_fraCash_11
        '
        Me._fraCash_11.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_11.Controls.Add(Me._txtTotalCash_11)
        Me._fraCash_11.Controls.Add(Me._tdgCash_11)
        Me._fraCash_11.Controls.Add(Me._lblCashTot_11)
        Me._fraCash_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_11.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_11.Name = "_fraCash_11"
        Me._fraCash_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_11.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_11.TabIndex = 164
        Me._fraCash_11.TabStop = False
        Me._fraCash_11.Tag = "CAP;665"
        Me._fraCash_11.Text = "Cash Breakdown"
        Me._fraCash_11.Visible = False
        '
        '_txtTotalCash_11
        '
        Me._txtTotalCash_11.AcceptsReturn = True
        Me._txtTotalCash_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_11.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_11.MaxLength = 0
        Me._txtTotalCash_11.Name = "_txtTotalCash_11"
        Me._txtTotalCash_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_11.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_11.TabIndex = 165
        Me._txtTotalCash_11.TabStop = False
        Me._txtTotalCash_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_11
        '
        Me._tdgCash_11.AllowBigSelection = False
        Me._tdgCash_11.AllowRowSelection = False
        Me._tdgCash_11.AlternatingRows = False
        Me._tdgCash_11.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_11.ColumnsCount = 0
        Me._tdgCash_11.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_11.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_11.EvenStyle = DataGridViewCellStyle45
        Me._tdgCash_11.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_11.FixedColumns = -1
        Me._tdgCash_11.FixedRows = -1
        Me._tdgCash_11.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_11.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_11.GridLineWidth = 0
        Me._tdgCash_11.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_11.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_11.Name = "_tdgCash_11"
        Me._tdgCash_11.OddStyle = DataGridViewCellStyle46
        Me._tdgCash_11.RowHeightMin = 0
        Me._tdgCash_11.RowsCount = 0
        Me._tdgCash_11.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_11.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_11.SelectedStyle = Nothing
        Me._tdgCash_11.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_11.SelLength = -1
        Me._tdgCash_11.SelStart = -1
        Me._tdgCash_11.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_11.TabIndex = 166
        Me._tdgCash_11.ToolTipText = ""
        '
        '_lblCashTot_11
        '
        Me._lblCashTot_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_11.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_11.Name = "_lblCashTot_11"
        Me._lblCashTot_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_11.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_11.TabIndex = 167
        Me._lblCashTot_11.Tag = "CAP;666"
        Me._lblCashTot_11.Text = "Total"
        '
        '_fraFloat_11
        '
        Me._fraFloat_11.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_11.Controls.Add(Me._txtFloatRem_11)
        Me._fraFloat_11.Controls.Add(Me._txtTotalFloat_11)
        Me._fraFloat_11.Controls.Add(Me._tdgFloat_11)
        Me._fraFloat_11.Controls.Add(Me._lblFloatRem_11)
        Me._fraFloat_11.Controls.Add(Me._lblFloatTot_11)
        Me._fraFloat_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_11.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_11.Name = "_fraFloat_11"
        Me._fraFloat_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_11.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_11.TabIndex = 168
        Me._fraFloat_11.TabStop = False
        Me._fraFloat_11.Tag = "CAP;664"
        Me._fraFloat_11.Text = "Float Breakdown"
        Me._fraFloat_11.Visible = False
        '
        '_txtFloatRem_11
        '
        Me._txtFloatRem_11.AcceptsReturn = True
        Me._txtFloatRem_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_11.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_11.MaxLength = 0
        Me._txtFloatRem_11.Name = "_txtFloatRem_11"
        Me._txtFloatRem_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_11.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_11.TabIndex = 170
        Me._txtFloatRem_11.TabStop = False
        Me._txtFloatRem_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_11
        '
        Me._txtTotalFloat_11.AcceptsReturn = True
        Me._txtTotalFloat_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_11.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_11.MaxLength = 0
        Me._txtTotalFloat_11.Name = "_txtTotalFloat_11"
        Me._txtTotalFloat_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_11.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_11.TabIndex = 169
        Me._txtTotalFloat_11.TabStop = False
        Me._txtTotalFloat_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_11
        '
        Me._tdgFloat_11.AllowBigSelection = False
        Me._tdgFloat_11.AllowRowSelection = False
        Me._tdgFloat_11.AlternatingRows = False
        Me._tdgFloat_11.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_11.ColumnsCount = 0
        Me._tdgFloat_11.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_11.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_11.EvenStyle = DataGridViewCellStyle47
        Me._tdgFloat_11.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_11.FixedColumns = -1
        Me._tdgFloat_11.FixedRows = -1
        Me._tdgFloat_11.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_11.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_11.GridLineWidth = 0
        Me._tdgFloat_11.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_11.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_11.Name = "_tdgFloat_11"
        Me._tdgFloat_11.OddStyle = DataGridViewCellStyle48
        Me._tdgFloat_11.RowHeightMin = 0
        Me._tdgFloat_11.RowsCount = 0
        Me._tdgFloat_11.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_11.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_11.SelectedStyle = Nothing
        Me._tdgFloat_11.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_11.SelLength = -1
        Me._tdgFloat_11.SelStart = -1
        Me._tdgFloat_11.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_11.TabIndex = 171
        Me._tdgFloat_11.ToolTipText = ""
        '
        '_lblFloatRem_11
        '
        Me._lblFloatRem_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_11.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_11.Name = "_lblFloatRem_11"
        Me._lblFloatRem_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_11.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_11.TabIndex = 173
        Me._lblFloatRem_11.Tag = "CAP;667"
        Me._lblFloatRem_11.Text = "Float Remaining"
        '
        '_lblFloatTot_11
        '
        Me._lblFloatTot_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_11.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_11.Name = "_lblFloatTot_11"
        Me._lblFloatTot_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_11.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_11.TabIndex = 172
        Me._lblFloatTot_11.Tag = "CAP;666"
        Me._lblFloatTot_11.Text = "Total"
        '
        '_txtTotalCC_11
        '
        Me._txtTotalCC_11.AcceptsReturn = True
        Me._txtTotalCC_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_11.Enabled = False
        Me._txtTotalCC_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_11.Location = New System.Drawing.Point(264, 100)
        Me._txtTotalCC_11.MaxLength = 0
        Me._txtTotalCC_11.Name = "_txtTotalCC_11"
        Me._txtTotalCC_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_11.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_11.TabIndex = 163
        Me._txtTotalCC_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_11.Visible = False
        '
        '_tabMediaTypes_TabPage12
        '
        Me._tabMediaTypes_TabPage12.Controls.Add(Me._txtTotalCC_12)
        Me._tabMediaTypes_TabPage12.Controls.Add(Me._fraFloat_12)
        Me._tabMediaTypes_TabPage12.Controls.Add(Me._fraCash_12)
        Me._tabMediaTypes_TabPage12.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage12.Name = "_tabMediaTypes_TabPage12"
        Me._tabMediaTypes_TabPage12.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage12.TabIndex = 12
        Me._tabMediaTypes_TabPage12.Text = "Tab 12"
        '
        '_txtTotalCC_12
        '
        Me._txtTotalCC_12.AcceptsReturn = True
        Me._txtTotalCC_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_12.Enabled = False
        Me._txtTotalCC_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_12.Location = New System.Drawing.Point(256, 100)
        Me._txtTotalCC_12.MaxLength = 0
        Me._txtTotalCC_12.Name = "_txtTotalCC_12"
        Me._txtTotalCC_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_12.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_12.TabIndex = 174
        Me._txtTotalCC_12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_12.Visible = False
        '
        '_fraFloat_12
        '
        Me._fraFloat_12.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_12.Controls.Add(Me._txtFloatRem_12)
        Me._fraFloat_12.Controls.Add(Me._txtTotalFloat_12)
        Me._fraFloat_12.Controls.Add(Me._tdgFloat_12)
        Me._fraFloat_12.Controls.Add(Me._lblFloatRem_12)
        Me._fraFloat_12.Controls.Add(Me._lblFloatTot_12)
        Me._fraFloat_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_12.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_12.Name = "_fraFloat_12"
        Me._fraFloat_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_12.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_12.TabIndex = 179
        Me._fraFloat_12.TabStop = False
        Me._fraFloat_12.Tag = "CAP;664"
        Me._fraFloat_12.Text = "Float Breakdown"
        Me._fraFloat_12.Visible = False
        '
        '_txtFloatRem_12
        '
        Me._txtFloatRem_12.AcceptsReturn = True
        Me._txtFloatRem_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_12.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_12.MaxLength = 0
        Me._txtFloatRem_12.Name = "_txtFloatRem_12"
        Me._txtFloatRem_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_12.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_12.TabIndex = 181
        Me._txtFloatRem_12.TabStop = False
        Me._txtFloatRem_12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_12
        '
        Me._txtTotalFloat_12.AcceptsReturn = True
        Me._txtTotalFloat_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_12.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_12.MaxLength = 0
        Me._txtTotalFloat_12.Name = "_txtTotalFloat_12"
        Me._txtTotalFloat_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_12.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_12.TabIndex = 180
        Me._txtTotalFloat_12.TabStop = False
        Me._txtTotalFloat_12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_12
        '
        Me._tdgFloat_12.AllowBigSelection = False
        Me._tdgFloat_12.AllowRowSelection = False
        Me._tdgFloat_12.AlternatingRows = False
        Me._tdgFloat_12.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_12.ColumnsCount = 0
        Me._tdgFloat_12.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_12.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_12.EvenStyle = DataGridViewCellStyle49
        Me._tdgFloat_12.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_12.FixedColumns = -1
        Me._tdgFloat_12.FixedRows = -1
        Me._tdgFloat_12.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_12.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_12.GridLineWidth = 0
        Me._tdgFloat_12.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_12.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_12.Name = "_tdgFloat_12"
        Me._tdgFloat_12.OddStyle = DataGridViewCellStyle50
        Me._tdgFloat_12.RowHeightMin = 0
        Me._tdgFloat_12.RowsCount = 0
        Me._tdgFloat_12.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_12.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_12.SelectedStyle = Nothing
        Me._tdgFloat_12.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_12.SelLength = -1
        Me._tdgFloat_12.SelStart = -1
        Me._tdgFloat_12.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_12.TabIndex = 182
        Me._tdgFloat_12.ToolTipText = ""
        '
        '_lblFloatRem_12
        '
        Me._lblFloatRem_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_12.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_12.Name = "_lblFloatRem_12"
        Me._lblFloatRem_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_12.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_12.TabIndex = 184
        Me._lblFloatRem_12.Tag = "CAP;667"
        Me._lblFloatRem_12.Text = "Float Remaining"
        '
        '_lblFloatTot_12
        '
        Me._lblFloatTot_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_12.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_12.Name = "_lblFloatTot_12"
        Me._lblFloatTot_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_12.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_12.TabIndex = 183
        Me._lblFloatTot_12.Tag = "CAP;666"
        Me._lblFloatTot_12.Text = "Total"
        '
        '_fraCash_12
        '
        Me._fraCash_12.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_12.Controls.Add(Me._txtTotalCash_12)
        Me._fraCash_12.Controls.Add(Me._tdgCash_12)
        Me._fraCash_12.Controls.Add(Me._lblCashTot_12)
        Me._fraCash_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_12.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_12.Name = "_fraCash_12"
        Me._fraCash_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_12.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_12.TabIndex = 175
        Me._fraCash_12.TabStop = False
        Me._fraCash_12.Tag = "CAP;665"
        Me._fraCash_12.Text = "Cash Breakdown"
        Me._fraCash_12.Visible = False
        '
        '_txtTotalCash_12
        '
        Me._txtTotalCash_12.AcceptsReturn = True
        Me._txtTotalCash_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_12.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_12.MaxLength = 0
        Me._txtTotalCash_12.Name = "_txtTotalCash_12"
        Me._txtTotalCash_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_12.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_12.TabIndex = 176
        Me._txtTotalCash_12.TabStop = False
        Me._txtTotalCash_12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_12
        '
        Me._tdgCash_12.AllowBigSelection = False
        Me._tdgCash_12.AllowRowSelection = False
        Me._tdgCash_12.AlternatingRows = False
        Me._tdgCash_12.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_12.ColumnsCount = 0
        Me._tdgCash_12.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_12.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_12.EvenStyle = DataGridViewCellStyle51
        Me._tdgCash_12.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_12.FixedColumns = -1
        Me._tdgCash_12.FixedRows = -1
        Me._tdgCash_12.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_12.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_12.GridLineWidth = 0
        Me._tdgCash_12.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_12.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_12.Name = "_tdgCash_12"
        Me._tdgCash_12.OddStyle = DataGridViewCellStyle52
        Me._tdgCash_12.RowHeightMin = 0
        Me._tdgCash_12.RowsCount = 0
        Me._tdgCash_12.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_12.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_12.SelectedStyle = Nothing
        Me._tdgCash_12.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_12.SelLength = -1
        Me._tdgCash_12.SelStart = -1
        Me._tdgCash_12.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_12.TabIndex = 177
        Me._tdgCash_12.ToolTipText = ""
        '
        '_lblCashTot_12
        '
        Me._lblCashTot_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_12.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_12.Name = "_lblCashTot_12"
        Me._lblCashTot_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_12.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_12.TabIndex = 178
        Me._lblCashTot_12.Tag = "CAP;666"
        Me._lblCashTot_12.Text = "Total"
        '
        '_tabMediaTypes_TabPage13
        '
        Me._tabMediaTypes_TabPage13.Controls.Add(Me._fraCash_13)
        Me._tabMediaTypes_TabPage13.Controls.Add(Me._fraFloat_13)
        Me._tabMediaTypes_TabPage13.Controls.Add(Me._txtTotalCC_13)
        Me._tabMediaTypes_TabPage13.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage13.Name = "_tabMediaTypes_TabPage13"
        Me._tabMediaTypes_TabPage13.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage13.TabIndex = 13
        Me._tabMediaTypes_TabPage13.Text = "Tab 13"
        '
        '_fraCash_13
        '
        Me._fraCash_13.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_13.Controls.Add(Me._txtTotalCash_13)
        Me._fraCash_13.Controls.Add(Me._tdgCash_13)
        Me._fraCash_13.Controls.Add(Me._lblCashTot_13)
        Me._fraCash_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_13.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_13.Name = "_fraCash_13"
        Me._fraCash_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_13.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_13.TabIndex = 186
        Me._fraCash_13.TabStop = False
        Me._fraCash_13.Tag = "CAP;665"
        Me._fraCash_13.Text = "Cash Breakdown"
        Me._fraCash_13.Visible = False
        '
        '_txtTotalCash_13
        '
        Me._txtTotalCash_13.AcceptsReturn = True
        Me._txtTotalCash_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_13.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_13.MaxLength = 0
        Me._txtTotalCash_13.Name = "_txtTotalCash_13"
        Me._txtTotalCash_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_13.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_13.TabIndex = 187
        Me._txtTotalCash_13.TabStop = False
        Me._txtTotalCash_13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_13
        '
        Me._tdgCash_13.AllowBigSelection = False
        Me._tdgCash_13.AllowRowSelection = False
        Me._tdgCash_13.AlternatingRows = False
        Me._tdgCash_13.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_13.ColumnsCount = 0
        Me._tdgCash_13.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_13.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_13.EvenStyle = DataGridViewCellStyle53
        Me._tdgCash_13.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_13.FixedColumns = -1
        Me._tdgCash_13.FixedRows = -1
        Me._tdgCash_13.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_13.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_13.GridLineWidth = 0
        Me._tdgCash_13.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_13.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_13.Name = "_tdgCash_13"
        Me._tdgCash_13.OddStyle = DataGridViewCellStyle54
        Me._tdgCash_13.RowHeightMin = 0
        Me._tdgCash_13.RowsCount = 0
        Me._tdgCash_13.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_13.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_13.SelectedStyle = Nothing
        Me._tdgCash_13.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_13.SelLength = -1
        Me._tdgCash_13.SelStart = -1
        Me._tdgCash_13.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_13.TabIndex = 188
        Me._tdgCash_13.ToolTipText = ""
        '
        '_lblCashTot_13
        '
        Me._lblCashTot_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_13.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_13.Name = "_lblCashTot_13"
        Me._lblCashTot_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_13.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_13.TabIndex = 189
        Me._lblCashTot_13.Tag = "CAP;666"
        Me._lblCashTot_13.Text = "Total"
        '
        '_fraFloat_13
        '
        Me._fraFloat_13.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_13.Controls.Add(Me._txtFloatRem_13)
        Me._fraFloat_13.Controls.Add(Me._txtTotalFloat_13)
        Me._fraFloat_13.Controls.Add(Me._tdgFloat_13)
        Me._fraFloat_13.Controls.Add(Me._lblFloatRem_13)
        Me._fraFloat_13.Controls.Add(Me._lblFloatTot_13)
        Me._fraFloat_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_13.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_13.Name = "_fraFloat_13"
        Me._fraFloat_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_13.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_13.TabIndex = 190
        Me._fraFloat_13.TabStop = False
        Me._fraFloat_13.Tag = "CAP;664"
        Me._fraFloat_13.Text = "Float Breakdown"
        Me._fraFloat_13.Visible = False
        '
        '_txtFloatRem_13
        '
        Me._txtFloatRem_13.AcceptsReturn = True
        Me._txtFloatRem_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_13.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_13.MaxLength = 0
        Me._txtFloatRem_13.Name = "_txtFloatRem_13"
        Me._txtFloatRem_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_13.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_13.TabIndex = 192
        Me._txtFloatRem_13.TabStop = False
        Me._txtFloatRem_13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_13
        '
        Me._txtTotalFloat_13.AcceptsReturn = True
        Me._txtTotalFloat_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_13.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_13.MaxLength = 0
        Me._txtTotalFloat_13.Name = "_txtTotalFloat_13"
        Me._txtTotalFloat_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_13.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_13.TabIndex = 191
        Me._txtTotalFloat_13.TabStop = False
        Me._txtTotalFloat_13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_13
        '
        Me._tdgFloat_13.AllowBigSelection = False
        Me._tdgFloat_13.AllowRowSelection = False
        Me._tdgFloat_13.AlternatingRows = False
        Me._tdgFloat_13.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_13.ColumnsCount = 0
        Me._tdgFloat_13.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_13.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_13.EvenStyle = DataGridViewCellStyle55
        Me._tdgFloat_13.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_13.FixedColumns = -1
        Me._tdgFloat_13.FixedRows = -1
        Me._tdgFloat_13.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_13.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_13.GridLineWidth = 0
        Me._tdgFloat_13.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_13.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_13.Name = "_tdgFloat_13"
        Me._tdgFloat_13.OddStyle = DataGridViewCellStyle56
        Me._tdgFloat_13.RowHeightMin = 0
        Me._tdgFloat_13.RowsCount = 0
        Me._tdgFloat_13.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_13.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_13.SelectedStyle = Nothing
        Me._tdgFloat_13.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_13.SelLength = -1
        Me._tdgFloat_13.SelStart = -1
        Me._tdgFloat_13.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_13.TabIndex = 193
        Me._tdgFloat_13.ToolTipText = ""
        '
        '_lblFloatRem_13
        '
        Me._lblFloatRem_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_13.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_13.Name = "_lblFloatRem_13"
        Me._lblFloatRem_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_13.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_13.TabIndex = 195
        Me._lblFloatRem_13.Tag = "CAP;667"
        Me._lblFloatRem_13.Text = "Float Remaining"
        '
        '_lblFloatTot_13
        '
        Me._lblFloatTot_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_13.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_13.Name = "_lblFloatTot_13"
        Me._lblFloatTot_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_13.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_13.TabIndex = 194
        Me._lblFloatTot_13.Tag = "CAP;666"
        Me._lblFloatTot_13.Text = "Total"
        '
        '_txtTotalCC_13
        '
        Me._txtTotalCC_13.AcceptsReturn = True
        Me._txtTotalCC_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_13.Enabled = False
        Me._txtTotalCC_13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_13.Location = New System.Drawing.Point(264, 100)
        Me._txtTotalCC_13.MaxLength = 0
        Me._txtTotalCC_13.Name = "_txtTotalCC_13"
        Me._txtTotalCC_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_13.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_13.TabIndex = 185
        Me._txtTotalCC_13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_13.Visible = False
        '
        '_tabMediaTypes_TabPage14
        '
        Me._tabMediaTypes_TabPage14.Controls.Add(Me._txtTotalCC_14)
        Me._tabMediaTypes_TabPage14.Controls.Add(Me._fraFloat_14)
        Me._tabMediaTypes_TabPage14.Controls.Add(Me._fraCash_14)
        Me._tabMediaTypes_TabPage14.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage14.Name = "_tabMediaTypes_TabPage14"
        Me._tabMediaTypes_TabPage14.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage14.TabIndex = 14
        Me._tabMediaTypes_TabPage14.Text = "Tab 14"
        '
        '_txtTotalCC_14
        '
        Me._txtTotalCC_14.AcceptsReturn = True
        Me._txtTotalCC_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_14.Enabled = False
        Me._txtTotalCC_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_14.Location = New System.Drawing.Point(248, 100)
        Me._txtTotalCC_14.MaxLength = 0
        Me._txtTotalCC_14.Name = "_txtTotalCC_14"
        Me._txtTotalCC_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_14.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_14.TabIndex = 196
        Me._txtTotalCC_14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_14.Visible = False
        '
        '_fraFloat_14
        '
        Me._fraFloat_14.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_14.Controls.Add(Me._txtFloatRem_14)
        Me._fraFloat_14.Controls.Add(Me._txtTotalFloat_14)
        Me._fraFloat_14.Controls.Add(Me._tdgFloat_14)
        Me._fraFloat_14.Controls.Add(Me._lblFloatRem_14)
        Me._fraFloat_14.Controls.Add(Me._lblFloatTot_14)
        Me._fraFloat_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_14.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_14.Name = "_fraFloat_14"
        Me._fraFloat_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_14.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_14.TabIndex = 201
        Me._fraFloat_14.TabStop = False
        Me._fraFloat_14.Tag = "CAP;664"
        Me._fraFloat_14.Text = "Float Breakdown"
        Me._fraFloat_14.Visible = False
        '
        '_txtFloatRem_14
        '
        Me._txtFloatRem_14.AcceptsReturn = True
        Me._txtFloatRem_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_14.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_14.MaxLength = 0
        Me._txtFloatRem_14.Name = "_txtFloatRem_14"
        Me._txtFloatRem_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_14.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_14.TabIndex = 203
        Me._txtFloatRem_14.TabStop = False
        Me._txtFloatRem_14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_14
        '
        Me._txtTotalFloat_14.AcceptsReturn = True
        Me._txtTotalFloat_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_14.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_14.MaxLength = 0
        Me._txtTotalFloat_14.Name = "_txtTotalFloat_14"
        Me._txtTotalFloat_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_14.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_14.TabIndex = 202
        Me._txtTotalFloat_14.TabStop = False
        Me._txtTotalFloat_14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_14
        '
        Me._tdgFloat_14.AllowBigSelection = False
        Me._tdgFloat_14.AllowRowSelection = False
        Me._tdgFloat_14.AlternatingRows = False
        Me._tdgFloat_14.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_14.ColumnsCount = 0
        Me._tdgFloat_14.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_14.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_14.EvenStyle = DataGridViewCellStyle57
        Me._tdgFloat_14.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_14.FixedColumns = -1
        Me._tdgFloat_14.FixedRows = -1
        Me._tdgFloat_14.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_14.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_14.GridLineWidth = 0
        Me._tdgFloat_14.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_14.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_14.Name = "_tdgFloat_14"
        Me._tdgFloat_14.OddStyle = DataGridViewCellStyle58
        Me._tdgFloat_14.RowHeightMin = 0
        Me._tdgFloat_14.RowsCount = 0
        Me._tdgFloat_14.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_14.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_14.SelectedStyle = Nothing
        Me._tdgFloat_14.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_14.SelLength = -1
        Me._tdgFloat_14.SelStart = -1
        Me._tdgFloat_14.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_14.TabIndex = 204
        Me._tdgFloat_14.ToolTipText = ""
        '
        '_lblFloatRem_14
        '
        Me._lblFloatRem_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_14.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_14.Name = "_lblFloatRem_14"
        Me._lblFloatRem_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_14.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_14.TabIndex = 206
        Me._lblFloatRem_14.Tag = "CAP;667"
        Me._lblFloatRem_14.Text = "Float Remaining"
        '
        '_lblFloatTot_14
        '
        Me._lblFloatTot_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_14.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_14.Name = "_lblFloatTot_14"
        Me._lblFloatTot_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_14.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_14.TabIndex = 205
        Me._lblFloatTot_14.Tag = "CAP;666"
        Me._lblFloatTot_14.Text = "Total"
        '
        '_fraCash_14
        '
        Me._fraCash_14.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_14.Controls.Add(Me._txtTotalCash_14)
        Me._fraCash_14.Controls.Add(Me._tdgCash_14)
        Me._fraCash_14.Controls.Add(Me._lblCashTot_14)
        Me._fraCash_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_14.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_14.Name = "_fraCash_14"
        Me._fraCash_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_14.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_14.TabIndex = 197
        Me._fraCash_14.TabStop = False
        Me._fraCash_14.Tag = "CAP;665"
        Me._fraCash_14.Text = "Cash Breakdown"
        Me._fraCash_14.Visible = False
        '
        '_txtTotalCash_14
        '
        Me._txtTotalCash_14.AcceptsReturn = True
        Me._txtTotalCash_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_14.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_14.MaxLength = 0
        Me._txtTotalCash_14.Name = "_txtTotalCash_14"
        Me._txtTotalCash_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_14.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_14.TabIndex = 198
        Me._txtTotalCash_14.TabStop = False
        Me._txtTotalCash_14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_14
        '
        Me._tdgCash_14.AllowBigSelection = False
        Me._tdgCash_14.AllowRowSelection = False
        Me._tdgCash_14.AlternatingRows = False
        Me._tdgCash_14.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_14.ColumnsCount = 0
        Me._tdgCash_14.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_14.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_14.EvenStyle = DataGridViewCellStyle59
        Me._tdgCash_14.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_14.FixedColumns = -1
        Me._tdgCash_14.FixedRows = -1
        Me._tdgCash_14.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_14.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_14.GridLineWidth = 0
        Me._tdgCash_14.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_14.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_14.Name = "_tdgCash_14"
        Me._tdgCash_14.OddStyle = DataGridViewCellStyle60
        Me._tdgCash_14.RowHeightMin = 0
        Me._tdgCash_14.RowsCount = 0
        Me._tdgCash_14.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_14.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_14.SelectedStyle = Nothing
        Me._tdgCash_14.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_14.SelLength = -1
        Me._tdgCash_14.SelStart = -1
        Me._tdgCash_14.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_14.TabIndex = 199
        Me._tdgCash_14.ToolTipText = ""
        '
        '_lblCashTot_14
        '
        Me._lblCashTot_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_14.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_14.Name = "_lblCashTot_14"
        Me._lblCashTot_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_14.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_14.TabIndex = 200
        Me._lblCashTot_14.Tag = "CAP;666"
        Me._lblCashTot_14.Text = "Total"
        '
        '_tabMediaTypes_TabPage15
        '
        Me._tabMediaTypes_TabPage15.Controls.Add(Me._fraCash_15)
        Me._tabMediaTypes_TabPage15.Controls.Add(Me._fraFloat_15)
        Me._tabMediaTypes_TabPage15.Controls.Add(Me._txtTotalCC_15)
        Me._tabMediaTypes_TabPage15.Location = New System.Drawing.Point(4, 40)
        Me._tabMediaTypes_TabPage15.Name = "_tabMediaTypes_TabPage15"
        Me._tabMediaTypes_TabPage15.Size = New System.Drawing.Size(557, 315)
        Me._tabMediaTypes_TabPage15.TabIndex = 15
        Me._tabMediaTypes_TabPage15.Text = "Tab 15"
        '
        '_fraCash_15
        '
        Me._fraCash_15.BackColor = System.Drawing.SystemColors.Control
        Me._fraCash_15.Controls.Add(Me._txtTotalCash_15)
        Me._fraCash_15.Controls.Add(Me._tdgCash_15)
        Me._fraCash_15.Controls.Add(Me._lblCashTot_15)
        Me._fraCash_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraCash_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraCash_15.Location = New System.Drawing.Point(104, 8)
        Me._fraCash_15.Name = "_fraCash_15"
        Me._fraCash_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraCash_15.Size = New System.Drawing.Size(249, 265)
        Me._fraCash_15.TabIndex = 208
        Me._fraCash_15.TabStop = False
        Me._fraCash_15.Tag = "CAP;665"
        Me._fraCash_15.Text = "Cash Breakdown"
        Me._fraCash_15.Visible = False
        '
        '_txtTotalCash_15
        '
        Me._txtTotalCash_15.AcceptsReturn = True
        Me._txtTotalCash_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCash_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCash_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCash_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCash_15.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalCash_15.MaxLength = 0
        Me._txtTotalCash_15.Name = "_txtTotalCash_15"
        Me._txtTotalCash_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCash_15.Size = New System.Drawing.Size(81, 19)
        Me._txtTotalCash_15.TabIndex = 209
        Me._txtTotalCash_15.TabStop = False
        Me._txtTotalCash_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgCash_15
        '
        Me._tdgCash_15.AllowBigSelection = False
        Me._tdgCash_15.AllowRowSelection = False
        Me._tdgCash_15.AlternatingRows = False
        Me._tdgCash_15.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgCash_15.ColumnsCount = 0
        Me._tdgCash_15.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgCash_15.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgCash_15.EvenStyle = DataGridViewCellStyle61
        Me._tdgCash_15.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgCash_15.FixedColumns = -1
        Me._tdgCash_15.FixedRows = -1
        Me._tdgCash_15.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgCash_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgCash_15.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgCash_15.GridLineWidth = 0
        Me._tdgCash_15.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgCash_15.Location = New System.Drawing.Point(16, 22)
        Me._tdgCash_15.Name = "_tdgCash_15"
        Me._tdgCash_15.OddStyle = DataGridViewCellStyle62
        Me._tdgCash_15.RowHeightMin = 0
        Me._tdgCash_15.RowsCount = 0
        Me._tdgCash_15.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_15.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgCash_15.SelectedStyle = Nothing
        Me._tdgCash_15.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgCash_15.SelLength = -1
        Me._tdgCash_15.SelStart = -1
        Me._tdgCash_15.Size = New System.Drawing.Size(217, 184)
        Me._tdgCash_15.TabIndex = 210
        Me._tdgCash_15.ToolTipText = ""
        '
        '_lblCashTot_15
        '
        Me._lblCashTot_15.BackColor = System.Drawing.SystemColors.Control
        Me._lblCashTot_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCashTot_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCashTot_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCashTot_15.Location = New System.Drawing.Point(104, 212)
        Me._lblCashTot_15.Name = "_lblCashTot_15"
        Me._lblCashTot_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCashTot_15.Size = New System.Drawing.Size(97, 17)
        Me._lblCashTot_15.TabIndex = 211
        Me._lblCashTot_15.Tag = "CAP;666"
        Me._lblCashTot_15.Text = "Total"
        '
        '_fraFloat_15
        '
        Me._fraFloat_15.BackColor = System.Drawing.SystemColors.Control
        Me._fraFloat_15.Controls.Add(Me._txtFloatRem_15)
        Me._fraFloat_15.Controls.Add(Me._txtTotalFloat_15)
        Me._fraFloat_15.Controls.Add(Me._tdgFloat_15)
        Me._fraFloat_15.Controls.Add(Me._lblFloatRem_15)
        Me._fraFloat_15.Controls.Add(Me._lblFloatTot_15)
        Me._fraFloat_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFloat_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFloat_15.Location = New System.Drawing.Point(24, 8)
        Me._fraFloat_15.Name = "_fraFloat_15"
        Me._fraFloat_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFloat_15.Size = New System.Drawing.Size(249, 265)
        Me._fraFloat_15.TabIndex = 212
        Me._fraFloat_15.TabStop = False
        Me._fraFloat_15.Tag = "CAP;664"
        Me._fraFloat_15.Text = "Float Breakdown"
        Me._fraFloat_15.Visible = False
        '
        '_txtFloatRem_15
        '
        Me._txtFloatRem_15.AcceptsReturn = True
        Me._txtFloatRem_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtFloatRem_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtFloatRem_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtFloatRem_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtFloatRem_15.Location = New System.Drawing.Point(152, 232)
        Me._txtFloatRem_15.MaxLength = 0
        Me._txtFloatRem_15.Name = "_txtFloatRem_15"
        Me._txtFloatRem_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtFloatRem_15.Size = New System.Drawing.Size(80, 19)
        Me._txtFloatRem_15.TabIndex = 214
        Me._txtFloatRem_15.TabStop = False
        Me._txtFloatRem_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_txtTotalFloat_15
        '
        Me._txtTotalFloat_15.AcceptsReturn = True
        Me._txtTotalFloat_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalFloat_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalFloat_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalFloat_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalFloat_15.Location = New System.Drawing.Point(152, 210)
        Me._txtTotalFloat_15.MaxLength = 0
        Me._txtTotalFloat_15.Name = "_txtTotalFloat_15"
        Me._txtTotalFloat_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalFloat_15.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalFloat_15.TabIndex = 213
        Me._txtTotalFloat_15.TabStop = False
        Me._txtTotalFloat_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_tdgFloat_15
        '
        Me._tdgFloat_15.AllowBigSelection = False
        Me._tdgFloat_15.AllowRowSelection = False
        Me._tdgFloat_15.AlternatingRows = False
        Me._tdgFloat_15.BackColorFixed = System.Drawing.Color.Empty
        Me._tdgFloat_15.ColumnsCount = 0
        Me._tdgFloat_15.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
        Me._tdgFloat_15.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me._tdgFloat_15.EvenStyle = DataGridViewCellStyle63
        Me._tdgFloat_15.FillStyle = Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle
        Me._tdgFloat_15.FixedColumns = -1
        Me._tdgFloat_15.FixedRows = -1
        Me._tdgFloat_15.FocusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusNone
        Me._tdgFloat_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tdgFloat_15.ForeColorFixed = System.Drawing.SystemColors.WindowText
        Me._tdgFloat_15.GridLineWidth = 0
        Me._tdgFloat_15.HighLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightNever
        Me._tdgFloat_15.Location = New System.Drawing.Point(16, 22)
        Me._tdgFloat_15.Name = "_tdgFloat_15"
        Me._tdgFloat_15.OddStyle = DataGridViewCellStyle64
        Me._tdgFloat_15.RowHeightMin = 0
        Me._tdgFloat_15.RowsCount = 0
        Me._tdgFloat_15.SelectedBackColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_15.SelectedForeColor = System.Drawing.SystemColors.AppWorkspace
        Me._tdgFloat_15.SelectedStyle = Nothing
        Me._tdgFloat_15.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me._tdgFloat_15.SelLength = -1
        Me._tdgFloat_15.SelStart = -1
        Me._tdgFloat_15.Size = New System.Drawing.Size(217, 184)
        Me._tdgFloat_15.TabIndex = 215
        Me._tdgFloat_15.ToolTipText = ""
        '
        '_lblFloatRem_15
        '
        Me._lblFloatRem_15.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatRem_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatRem_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatRem_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatRem_15.Location = New System.Drawing.Point(48, 232)
        Me._lblFloatRem_15.Name = "_lblFloatRem_15"
        Me._lblFloatRem_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatRem_15.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatRem_15.TabIndex = 217
        Me._lblFloatRem_15.Tag = "CAP;667"
        Me._lblFloatRem_15.Text = "Float Remaining"
        '
        '_lblFloatTot_15
        '
        Me._lblFloatTot_15.BackColor = System.Drawing.SystemColors.Control
        Me._lblFloatTot_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFloatTot_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFloatTot_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFloatTot_15.Location = New System.Drawing.Point(48, 212)
        Me._lblFloatTot_15.Name = "_lblFloatTot_15"
        Me._lblFloatTot_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFloatTot_15.Size = New System.Drawing.Size(97, 17)
        Me._lblFloatTot_15.TabIndex = 216
        Me._lblFloatTot_15.Tag = "CAP;666"
        Me._lblFloatTot_15.Text = "Total"
        '
        '_txtTotalCC_15
        '
        Me._txtTotalCC_15.AcceptsReturn = True
        Me._txtTotalCC_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtTotalCC_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtTotalCC_15.Enabled = False
        Me._txtTotalCC_15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtTotalCC_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtTotalCC_15.Location = New System.Drawing.Point(256, 100)
        Me._txtTotalCC_15.MaxLength = 0
        Me._txtTotalCC_15.Name = "_txtTotalCC_15"
        Me._txtTotalCC_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtTotalCC_15.Size = New System.Drawing.Size(80, 19)
        Me._txtTotalCC_15.TabIndex = 207
        Me._txtTotalCC_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtTotalCC_15.Visible = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._lblConfirm_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraTotals)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtDepositDate)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAuth)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(589, 379)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "*{Confirm}"
        '
        '_lblConfirm_0
        '
        Me._lblConfirm_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_0.Location = New System.Drawing.Point(50, 30)
        Me._lblConfirm_0.Name = "_lblConfirm_0"
        Me._lblConfirm_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_0.Size = New System.Drawing.Size(137, 17)
        Me._lblConfirm_0.TabIndex = 84
        Me._lblConfirm_0.Tag = "CAP;650"
        Me._lblConfirm_0.Text = "*{Bank Deposit Date:}"
        '
        'fraTotals
        '
        Me.fraTotals.BackColor = System.Drawing.SystemColors.Control
        Me.fraTotals.Controls.Add(Me.txtFloat)
        Me.fraTotals.Controls.Add(Me.txtBalance)
        Me.fraTotals.Controls.Add(Me.txtAdjustments)
        Me.fraTotals.Controls.Add(Me.txtSubTotal)
        Me.fraTotals.Controls.Add(Me.txtCCTotal)
        Me.fraTotals.Controls.Add(Me.txtBankingTotal)
        Me.fraTotals.Controls.Add(Me.cmdAdjustment)
        Me.fraTotals.Controls.Add(Me.cmdReverse)
        Me.fraTotals.Controls.Add(Me.txtTotalReceipts)
        Me.fraTotals.Controls.Add(Me.lblStatus)
        Me.fraTotals.Controls.Add(Me._lblConfirm_8)
        Me.fraTotals.Controls.Add(Me._lblConfirm_7)
        Me.fraTotals.Controls.Add(Me._lblConfirm_6)
        Me.fraTotals.Controls.Add(Me._lblConfirm_5)
        Me.fraTotals.Controls.Add(Me._lblConfirm_4)
        Me.fraTotals.Controls.Add(Me._lblConfirm_3)
        Me.fraTotals.Controls.Add(Me._lblConfirm_2)
        Me.fraTotals.Controls.Add(Me._lblConfirm_1)
        Me.fraTotals.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTotals.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTotals.Location = New System.Drawing.Point(40, 60)
        Me.fraTotals.Name = "fraTotals"
        Me.fraTotals.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTotals.Size = New System.Drawing.Size(273, 249)
        Me.fraTotals.TabIndex = 97
        Me.fraTotals.TabStop = False
        Me.fraTotals.Tag = "CAP;651"
        Me.fraTotals.Text = "*{Totals}"
        '
        'txtFloat
        '
        Me.txtFloat.AcceptsReturn = True
        Me.txtFloat.BackColor = System.Drawing.SystemColors.Window
        Me.txtFloat.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFloat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFloat.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFloat.Location = New System.Drawing.Point(144, 220)
        Me.txtFloat.MaxLength = 0
        Me.txtFloat.Name = "txtFloat"
        Me.txtFloat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFloat.Size = New System.Drawing.Size(97, 19)
        Me.txtFloat.TabIndex = 95
        Me.txtFloat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBalance
        '
        Me.txtBalance.AcceptsReturn = True
        Me.txtBalance.BackColor = System.Drawing.SystemColors.Window
        Me.txtBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBalance.Location = New System.Drawing.Point(144, 196)
        Me.txtBalance.MaxLength = 0
        Me.txtBalance.Name = "txtBalance"
        Me.txtBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBalance.Size = New System.Drawing.Size(97, 19)
        Me.txtBalance.TabIndex = 94
        Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjustments
        '
        Me.txtAdjustments.AcceptsReturn = True
        Me.txtAdjustments.BackColor = System.Drawing.SystemColors.Window
        Me.txtAdjustments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdjustments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjustments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdjustments.Location = New System.Drawing.Point(144, 172)
        Me.txtAdjustments.MaxLength = 0
        Me.txtAdjustments.Name = "txtAdjustments"
        Me.txtAdjustments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdjustments.Size = New System.Drawing.Size(97, 19)
        Me.txtAdjustments.TabIndex = 93
        Me.txtAdjustments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSubTotal
        '
        Me.txtSubTotal.AcceptsReturn = True
        Me.txtSubTotal.BackColor = System.Drawing.SystemColors.Window
        Me.txtSubTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubTotal.Location = New System.Drawing.Point(144, 92)
        Me.txtSubTotal.MaxLength = 0
        Me.txtSubTotal.Name = "txtSubTotal"
        Me.txtSubTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubTotal.Size = New System.Drawing.Size(97, 19)
        Me.txtSubTotal.TabIndex = 89
        Me.txtSubTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCCTotal
        '
        Me.txtCCTotal.AcceptsReturn = True
        Me.txtCCTotal.BackColor = System.Drawing.SystemColors.Window
        Me.txtCCTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCCTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCCTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCCTotal.Location = New System.Drawing.Point(144, 68)
        Me.txtCCTotal.MaxLength = 0
        Me.txtCCTotal.Name = "txtCCTotal"
        Me.txtCCTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCCTotal.Size = New System.Drawing.Size(97, 19)
        Me.txtCCTotal.TabIndex = 88
        Me.txtCCTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBankingTotal
        '
        Me.txtBankingTotal.AcceptsReturn = True
        Me.txtBankingTotal.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankingTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankingTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankingTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankingTotal.Location = New System.Drawing.Point(144, 44)
        Me.txtBankingTotal.MaxLength = 0
        Me.txtBankingTotal.Name = "txtBankingTotal"
        Me.txtBankingTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankingTotal.Size = New System.Drawing.Size(97, 19)
        Me.txtBankingTotal.TabIndex = 87
        Me.txtBankingTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdAdjustment
        '
        Me.cmdAdjustment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdjustment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdjustment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdjustment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdjustment.Location = New System.Drawing.Point(144, 144)
        Me.cmdAdjustment.Name = "cmdAdjustment"
        Me.cmdAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdjustment.Size = New System.Drawing.Size(113, 19)
        Me.cmdAdjustment.TabIndex = 92
        Me.cmdAdjustment.Tag = "CAP;611"
        Me.cmdAdjustment.Text = "*{Add Adjustment...}"
        Me.cmdAdjustment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdjustment.UseVisualStyleBackColor = False
        '
        'cmdReverse
        '
        Me.cmdReverse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReverse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReverse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReverse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReverse.Location = New System.Drawing.Point(16, 144)
        Me.cmdReverse.Name = "cmdReverse"
        Me.cmdReverse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReverse.Size = New System.Drawing.Size(113, 19)
        Me.cmdReverse.TabIndex = 91
        Me.cmdReverse.Tag = "CAP;610"
        Me.cmdReverse.Text = "*{Reverse Receipt...}"
        Me.cmdReverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReverse.UseVisualStyleBackColor = False
        '
        'txtTotalReceipts
        '
        Me.txtTotalReceipts.AcceptsReturn = True
        Me.txtTotalReceipts.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalReceipts.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalReceipts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalReceipts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalReceipts.Location = New System.Drawing.Point(144, 20)
        Me.txtTotalReceipts.MaxLength = 0
        Me.txtTotalReceipts.Name = "txtTotalReceipts"
        Me.txtTotalReceipts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalReceipts.Size = New System.Drawing.Size(97, 19)
        Me.txtTotalReceipts.TabIndex = 86
        Me.txtTotalReceipts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.GrayText
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(80, 120)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(185, 17)
        Me.lblStatus.TabIndex = 90
        Me.lblStatus.Text = "DRAWER DOES NOT BALANCE"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblConfirm_8
        '
        Me._lblConfirm_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_8.Location = New System.Drawing.Point(8, 224)
        Me._lblConfirm_8.Name = "_lblConfirm_8"
        Me._lblConfirm_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_8.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_8.TabIndex = 105
        Me._lblConfirm_8.Tag = "CAP;660"
        Me._lblConfirm_8.Text = "*{Float:}"
        '
        '_lblConfirm_7
        '
        Me._lblConfirm_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_7.Location = New System.Drawing.Point(8, 200)
        Me._lblConfirm_7.Name = "_lblConfirm_7"
        Me._lblConfirm_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_7.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_7.TabIndex = 104
        Me._lblConfirm_7.Tag = "CAP;659"
        Me._lblConfirm_7.Text = "*{Balance:}"
        '
        '_lblConfirm_6
        '
        Me._lblConfirm_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_6.Location = New System.Drawing.Point(8, 176)
        Me._lblConfirm_6.Name = "_lblConfirm_6"
        Me._lblConfirm_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_6.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_6.TabIndex = 103
        Me._lblConfirm_6.Tag = "CAP;658"
        Me._lblConfirm_6.Text = "*{Adjustments:}"
        '
        '_lblConfirm_5
        '
        Me._lblConfirm_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_5.Location = New System.Drawing.Point(8, 120)
        Me._lblConfirm_5.Name = "_lblConfirm_5"
        Me._lblConfirm_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_5.Size = New System.Drawing.Size(65, 17)
        Me._lblConfirm_5.TabIndex = 102
        Me._lblConfirm_5.Tag = "CAP;657"
        Me._lblConfirm_5.Text = "*{Status:}"
        '
        '_lblConfirm_4
        '
        Me._lblConfirm_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_4.Location = New System.Drawing.Point(8, 96)
        Me._lblConfirm_4.Name = "_lblConfirm_4"
        Me._lblConfirm_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_4.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_4.TabIndex = 101
        Me._lblConfirm_4.Tag = "CAP;656"
        Me._lblConfirm_4.Text = "*{Sub-Total:}"
        '
        '_lblConfirm_3
        '
        Me._lblConfirm_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_3.Location = New System.Drawing.Point(8, 72)
        Me._lblConfirm_3.Name = "_lblConfirm_3"
        Me._lblConfirm_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_3.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_3.TabIndex = 100
        Me._lblConfirm_3.Tag = "CAP;655"
        Me._lblConfirm_3.Text = "*{EFTPos/CC Total:}"
        '
        '_lblConfirm_2
        '
        Me._lblConfirm_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_2.Location = New System.Drawing.Point(8, 48)
        Me._lblConfirm_2.Name = "_lblConfirm_2"
        Me._lblConfirm_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_2.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_2.TabIndex = 99
        Me._lblConfirm_2.Tag = "CAP;654"
        Me._lblConfirm_2.Text = "*{Banking Total:}"
        '
        '_lblConfirm_1
        '
        Me._lblConfirm_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_1.Location = New System.Drawing.Point(8, 24)
        Me._lblConfirm_1.Name = "_lblConfirm_1"
        Me._lblConfirm_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_1.Size = New System.Drawing.Size(121, 17)
        Me._lblConfirm_1.TabIndex = 98
        Me._lblConfirm_1.Tag = "CAP;653"
        Me._lblConfirm_1.Text = "*{Total Receipts:}"
        '
        'txtDepositDate
        '
        Me.txtDepositDate.AcceptsReturn = True
        Me.txtDepositDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDepositDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDepositDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDepositDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDepositDate.Location = New System.Drawing.Point(184, 26)
        Me.txtDepositDate.MaxLength = 0
        Me.txtDepositDate.Name = "txtDepositDate"
        Me.txtDepositDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDepositDate.Size = New System.Drawing.Size(97, 19)
        Me.txtDepositDate.TabIndex = 85
        Me.txtDepositDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'fraAuth
        '
        Me.fraAuth.BackColor = System.Drawing.SystemColors.Control
        Me.fraAuth.Controls.Add(Me.cboPMUserAuthorise1)
        Me.fraAuth.Controls.Add(Me.txtDateApproved)
        Me.fraAuth.Controls.Add(Me.cboPMUserAuthorise2)
        Me.fraAuth.Controls.Add(Me._lblConfirm_11)
        Me.fraAuth.Controls.Add(Me._lblConfirm_10)
        Me.fraAuth.Controls.Add(Me._lblConfirm_9)
        Me.fraAuth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAuth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAuth.Location = New System.Drawing.Point(328, 60)
        Me.fraAuth.Name = "fraAuth"
        Me.fraAuth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAuth.Size = New System.Drawing.Size(217, 249)
        Me.fraAuth.TabIndex = 106
        Me.fraAuth.TabStop = False
        Me.fraAuth.Tag = "CAP;652"
        Me.fraAuth.Text = "*{Authorisation}"
        '
        'cboPMUserAuthorise1
        '
        Me.cboPMUserAuthorise1.DefaultUserID = 0
        Me.cboPMUserAuthorise1.FirstItem = ""
        Me.cboPMUserAuthorise1.ListIndex = -1
        Me.cboPMUserAuthorise1.Location = New System.Drawing.Point(40, 64)
        Me.cboPMUserAuthorise1.Name = "cboPMUserAuthorise1"
        Me.cboPMUserAuthorise1.PMUserGroupID = 0
        Me.cboPMUserAuthorise1.SingleUserID = 0
        Me.cboPMUserAuthorise1.Size = New System.Drawing.Size(129, 21)
        Me.cboPMUserAuthorise1.Sorted = True
        Me.cboPMUserAuthorise1.TabIndex = 150
        Me.cboPMUserAuthorise1.ToolTipText = ""
        Me.cboPMUserAuthorise1.UserID = 0
        '
        'txtDateApproved
        '
        Me.txtDateApproved.AcceptsReturn = True
        Me.txtDateApproved.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateApproved.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateApproved.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateApproved.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateApproved.Location = New System.Drawing.Point(40, 176)
        Me.txtDateApproved.MaxLength = 0
        Me.txtDateApproved.Name = "txtDateApproved"
        Me.txtDateApproved.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateApproved.Size = New System.Drawing.Size(129, 19)
        Me.txtDateApproved.TabIndex = 96
        '
        'cboPMUserAuthorise2
        '
        Me.cboPMUserAuthorise2.DefaultUserID = 0
        Me.cboPMUserAuthorise2.FirstItem = ""
        Me.cboPMUserAuthorise2.ListIndex = -1
        Me.cboPMUserAuthorise2.Location = New System.Drawing.Point(40, 120)
        Me.cboPMUserAuthorise2.Name = "cboPMUserAuthorise2"
        Me.cboPMUserAuthorise2.PMUserGroupID = 0
        Me.cboPMUserAuthorise2.SingleUserID = 0
        Me.cboPMUserAuthorise2.Size = New System.Drawing.Size(129, 21)
        Me.cboPMUserAuthorise2.Sorted = True
        Me.cboPMUserAuthorise2.TabIndex = 151
        Me.cboPMUserAuthorise2.ToolTipText = ""
        Me.cboPMUserAuthorise2.UserID = 0
        '
        '_lblConfirm_11
        '
        Me._lblConfirm_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_11.Location = New System.Drawing.Point(40, 160)
        Me._lblConfirm_11.Name = "_lblConfirm_11"
        Me._lblConfirm_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_11.Size = New System.Drawing.Size(129, 17)
        Me._lblConfirm_11.TabIndex = 109
        Me._lblConfirm_11.Tag = "CAP;663"
        Me._lblConfirm_11.Text = "*{Date Approved:}"
        '
        '_lblConfirm_10
        '
        Me._lblConfirm_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_10.Location = New System.Drawing.Point(40, 104)
        Me._lblConfirm_10.Name = "_lblConfirm_10"
        Me._lblConfirm_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_10.Size = New System.Drawing.Size(169, 17)
        Me._lblConfirm_10.TabIndex = 108
        Me._lblConfirm_10.Tag = "CAP;662"
        Me._lblConfirm_10.Text = "*{Second authorising user:}"
        '
        '_lblConfirm_9
        '
        Me._lblConfirm_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblConfirm_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblConfirm_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblConfirm_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblConfirm_9.Location = New System.Drawing.Point(40, 48)
        Me._lblConfirm_9.Name = "_lblConfirm_9"
        Me._lblConfirm_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblConfirm_9.Size = New System.Drawing.Size(169, 17)
        Me._lblConfirm_9.TabIndex = 107
        Me._lblConfirm_9.Tag = "CAP;661"
        Me._lblConfirm_9.Text = "*{First Authorising user:}"
        '
        'cmdApprove
        '
        Me.cmdApprove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApprove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApprove.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApprove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApprove.Location = New System.Drawing.Point(224, 416)
        Me.cmdApprove.Name = "cmdApprove"
        Me.cmdApprove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApprove.Size = New System.Drawing.Size(81, 22)
        Me.cmdApprove.TabIndex = 149
        Me.cmdApprove.Tag = "CAP;612"
        Me.cmdApprove.Text = "*{Approve}"
        Me.cmdApprove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApprove.UseVisualStyleBackColor = False
        Me.cmdApprove.Visible = False
        '
        'cmdViewAdj
        '
        Me.cmdViewAdj.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewAdj.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewAdj.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewAdj.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewAdj.Location = New System.Drawing.Point(312, 416)
        Me.cmdViewAdj.Name = "cmdViewAdj"
        Me.cmdViewAdj.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewAdj.Size = New System.Drawing.Size(129, 22)
        Me.cmdViewAdj.TabIndex = 148
        Me.cmdViewAdj.Tag = "CAP;613"
        Me.cmdViewAdj.Text = "*{View Adjustments}"
        Me.cmdViewAdj.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewAdj.UseVisualStyleBackColor = False
        Me.cmdViewAdj.Visible = False
        '
        'frmBanking
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(615, 445)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdApprove)
        Me.Controls.Add(Me.cmdViewAdj)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBanking"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Tag = "CAP;600"
        Me.Text = "*{Banking}"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.tabMediaTypes.ResumeLayout(False)
        Me._tabMediaTypes_TabPage0.ResumeLayout(False)
        Me._fraCash_0.ResumeLayout(False)
        CType(Me._tdgCash_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_0.ResumeLayout(False)
        CType(Me._tdgFloat_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage1.ResumeLayout(False)
        Me._fraFloat_1.ResumeLayout(False)
        CType(Me._tdgFloat_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_1.ResumeLayout(False)
        CType(Me._tdgCash_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage2.ResumeLayout(False)
        Me._fraFloat_2.ResumeLayout(False)
        CType(Me._tdgFloat_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_2.ResumeLayout(False)
        CType(Me._tdgCash_2, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage3.ResumeLayout(False)
        Me._fraCash_3.ResumeLayout(False)
        CType(Me._tdgCash_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_3.ResumeLayout(False)
        CType(Me._tdgFloat_3, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage4.ResumeLayout(False)
        Me._fraFloat_4.ResumeLayout(False)
        CType(Me._tdgFloat_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_4.ResumeLayout(False)
        CType(Me._tdgCash_4, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage5.ResumeLayout(False)
        Me._fraFloat_5.ResumeLayout(False)
        CType(Me._tdgFloat_5, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_5.ResumeLayout(False)
        CType(Me._tdgCash_5, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage6.ResumeLayout(False)
        Me._fraFloat_6.ResumeLayout(False)
        CType(Me._tdgFloat_6, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_6.ResumeLayout(False)
        CType(Me._tdgCash_6, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage7.ResumeLayout(False)
        Me._fraCash_7.ResumeLayout(False)
        CType(Me._tdgCash_7, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_7.ResumeLayout(False)
        CType(Me._tdgFloat_7, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage8.ResumeLayout(False)
        Me._fraFloat_8.ResumeLayout(False)
        CType(Me._tdgFloat_8, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_8.ResumeLayout(False)
        CType(Me._tdgCash_8, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage9.ResumeLayout(False)
        Me._fraCash_9.ResumeLayout(False)
        CType(Me._tdgCash_9, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_9.ResumeLayout(False)
        CType(Me._tdgFloat_9, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage10.ResumeLayout(False)
        Me._fraCash_10.ResumeLayout(False)
        CType(Me._tdgCash_10, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_10.ResumeLayout(False)
        CType(Me._tdgFloat_10, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage11.ResumeLayout(False)
        Me._fraCash_11.ResumeLayout(False)
        CType(Me._tdgCash_11, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_11.ResumeLayout(False)
        CType(Me._tdgFloat_11, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage12.ResumeLayout(False)
        Me._fraFloat_12.ResumeLayout(False)
        CType(Me._tdgFloat_12, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_12.ResumeLayout(False)
        CType(Me._tdgCash_12, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage13.ResumeLayout(False)
        Me._fraCash_13.ResumeLayout(False)
        CType(Me._tdgCash_13, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_13.ResumeLayout(False)
        CType(Me._tdgFloat_13, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage14.ResumeLayout(False)
        Me._fraFloat_14.ResumeLayout(False)
        CType(Me._tdgFloat_14, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraCash_14.ResumeLayout(False)
        CType(Me._tdgCash_14, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMediaTypes_TabPage15.ResumeLayout(False)
        Me._fraCash_15.ResumeLayout(False)
        CType(Me._tdgCash_15, System.ComponentModel.ISupportInitialize).EndInit()
        Me._fraFloat_15.ResumeLayout(False)
        CType(Me._tdgFloat_15, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraTotals.ResumeLayout(False)
        Me.fraAuth.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializetxtTotalFloat()
		Me.txtTotalFloat(11) = _txtTotalFloat_11
		Me.txtTotalFloat(1) = _txtTotalFloat_1
		Me.txtTotalFloat(0) = _txtTotalFloat_0
		Me.txtTotalFloat(15) = _txtTotalFloat_15
		Me.txtTotalFloat(14) = _txtTotalFloat_14
		Me.txtTotalFloat(13) = _txtTotalFloat_13
		Me.txtTotalFloat(12) = _txtTotalFloat_12
		Me.txtTotalFloat(10) = _txtTotalFloat_10
		Me.txtTotalFloat(9) = _txtTotalFloat_9
		Me.txtTotalFloat(8) = _txtTotalFloat_8
		Me.txtTotalFloat(7) = _txtTotalFloat_7
		Me.txtTotalFloat(6) = _txtTotalFloat_6
		Me.txtTotalFloat(5) = _txtTotalFloat_5
		Me.txtTotalFloat(4) = _txtTotalFloat_4
		Me.txtTotalFloat(3) = _txtTotalFloat_3
		Me.txtTotalFloat(2) = _txtTotalFloat_2
	End Sub
	Sub InitializetxtTotalCash()
		Me.txtTotalCash(11) = _txtTotalCash_11
		Me.txtTotalCash(15) = _txtTotalCash_15
		Me.txtTotalCash(14) = _txtTotalCash_14
		Me.txtTotalCash(12) = _txtTotalCash_12
		Me.txtTotalCash(9) = _txtTotalCash_9
		Me.txtTotalCash(8) = _txtTotalCash_8
		Me.txtTotalCash(3) = _txtTotalCash_3
		Me.txtTotalCash(2) = _txtTotalCash_2
		Me.txtTotalCash(1) = _txtTotalCash_1
		Me.txtTotalCash(0) = _txtTotalCash_0
		Me.txtTotalCash(6) = _txtTotalCash_6
		Me.txtTotalCash(13) = _txtTotalCash_13
		Me.txtTotalCash(10) = _txtTotalCash_10
		Me.txtTotalCash(7) = _txtTotalCash_7
		Me.txtTotalCash(5) = _txtTotalCash_5
		Me.txtTotalCash(4) = _txtTotalCash_4
	End Sub
	Sub InitializetxtTotalCC()
		Me.txtTotalCC(15) = _txtTotalCC_15
		Me.txtTotalCC(14) = _txtTotalCC_14
		Me.txtTotalCC(13) = _txtTotalCC_13
		Me.txtTotalCC(12) = _txtTotalCC_12
		Me.txtTotalCC(11) = _txtTotalCC_11
		Me.txtTotalCC(10) = _txtTotalCC_10
		Me.txtTotalCC(9) = _txtTotalCC_9
		Me.txtTotalCC(8) = _txtTotalCC_8
		Me.txtTotalCC(7) = _txtTotalCC_7
		Me.txtTotalCC(6) = _txtTotalCC_6
		Me.txtTotalCC(5) = _txtTotalCC_5
		Me.txtTotalCC(3) = _txtTotalCC_3
		Me.txtTotalCC(2) = _txtTotalCC_2
		Me.txtTotalCC(1) = _txtTotalCC_1
		Me.txtTotalCC(0) = _txtTotalCC_0
		Me.txtTotalCC(4) = _txtTotalCC_4
	End Sub
	Sub InitializetxtFloatRem()
		Me.txtFloatRem(11) = _txtFloatRem_11
		Me.txtFloatRem(1) = _txtFloatRem_1
		Me.txtFloatRem(0) = _txtFloatRem_0
		Me.txtFloatRem(15) = _txtFloatRem_15
		Me.txtFloatRem(14) = _txtFloatRem_14
		Me.txtFloatRem(13) = _txtFloatRem_13
		Me.txtFloatRem(12) = _txtFloatRem_12
		Me.txtFloatRem(10) = _txtFloatRem_10
		Me.txtFloatRem(9) = _txtFloatRem_9
		Me.txtFloatRem(8) = _txtFloatRem_8
		Me.txtFloatRem(7) = _txtFloatRem_7
		Me.txtFloatRem(6) = _txtFloatRem_6
		Me.txtFloatRem(5) = _txtFloatRem_5
		Me.txtFloatRem(4) = _txtFloatRem_4
		Me.txtFloatRem(3) = _txtFloatRem_3
		Me.txtFloatRem(2) = _txtFloatRem_2
	End Sub
	Sub InitializetdgFloat()
		Me.tdgFloat(11) = _tdgFloat_11
		Me.tdgFloat(1) = _tdgFloat_1
		Me.tdgFloat(0) = _tdgFloat_0
		Me.tdgFloat(15) = _tdgFloat_15
		Me.tdgFloat(14) = _tdgFloat_14
		Me.tdgFloat(13) = _tdgFloat_13
		Me.tdgFloat(12) = _tdgFloat_12
		Me.tdgFloat(10) = _tdgFloat_10
		Me.tdgFloat(9) = _tdgFloat_9
		Me.tdgFloat(8) = _tdgFloat_8
		Me.tdgFloat(7) = _tdgFloat_7
		Me.tdgFloat(6) = _tdgFloat_6
		Me.tdgFloat(5) = _tdgFloat_5
		Me.tdgFloat(4) = _tdgFloat_4
		Me.tdgFloat(3) = _tdgFloat_3
		Me.tdgFloat(2) = _tdgFloat_2
	End Sub
	Sub InitializetdgCash()
		Me.tdgCash(11) = _tdgCash_11
		Me.tdgCash(15) = _tdgCash_15
		Me.tdgCash(14) = _tdgCash_14
		Me.tdgCash(12) = _tdgCash_12
		Me.tdgCash(9) = _tdgCash_9
		Me.tdgCash(8) = _tdgCash_8
		Me.tdgCash(3) = _tdgCash_3
		Me.tdgCash(2) = _tdgCash_2
		Me.tdgCash(1) = _tdgCash_1
		Me.tdgCash(0) = _tdgCash_0
		Me.tdgCash(6) = _tdgCash_6
		Me.tdgCash(13) = _tdgCash_13
		Me.tdgCash(10) = _tdgCash_10
		Me.tdgCash(7) = _tdgCash_7
		Me.tdgCash(5) = _tdgCash_5
		Me.tdgCash(4) = _tdgCash_4
	End Sub
	Sub InitializelblTotalCC()
		Me.lblTotalCC(9) = _lblTotalCC_9
		Me.lblTotalCC(8) = _lblTotalCC_8
		Me.lblTotalCC(7) = _lblTotalCC_7
		Me.lblTotalCC(6) = _lblTotalCC_6
		Me.lblTotalCC(5) = _lblTotalCC_5
		Me.lblTotalCC(3) = _lblTotalCC_3
		Me.lblTotalCC(2) = _lblTotalCC_2
		Me.lblTotalCC(1) = _lblTotalCC_1
		Me.lblTotalCC(0) = _lblTotalCC_0
		Me.lblTotalCC(4) = _lblTotalCC_4
	End Sub
	Sub InitializelblFloatTot()
		Me.lblFloatTot(11) = _lblFloatTot_11
		Me.lblFloatTot(1) = _lblFloatTot_1
		Me.lblFloatTot(0) = _lblFloatTot_0
		Me.lblFloatTot(15) = _lblFloatTot_15
		Me.lblFloatTot(14) = _lblFloatTot_14
		Me.lblFloatTot(13) = _lblFloatTot_13
		Me.lblFloatTot(12) = _lblFloatTot_12
		Me.lblFloatTot(10) = _lblFloatTot_10
		Me.lblFloatTot(9) = _lblFloatTot_9
		Me.lblFloatTot(8) = _lblFloatTot_8
		Me.lblFloatTot(7) = _lblFloatTot_7
		Me.lblFloatTot(6) = _lblFloatTot_6
		Me.lblFloatTot(5) = _lblFloatTot_5
		Me.lblFloatTot(4) = _lblFloatTot_4
		Me.lblFloatTot(3) = _lblFloatTot_3
		Me.lblFloatTot(2) = _lblFloatTot_2
	End Sub
	Sub InitializelblFloatRem()
		Me.lblFloatRem(11) = _lblFloatRem_11
		Me.lblFloatRem(1) = _lblFloatRem_1
		Me.lblFloatRem(0) = _lblFloatRem_0
		Me.lblFloatRem(15) = _lblFloatRem_15
		Me.lblFloatRem(14) = _lblFloatRem_14
		Me.lblFloatRem(13) = _lblFloatRem_13
		Me.lblFloatRem(12) = _lblFloatRem_12
		Me.lblFloatRem(10) = _lblFloatRem_10
		Me.lblFloatRem(9) = _lblFloatRem_9
		Me.lblFloatRem(8) = _lblFloatRem_8
		Me.lblFloatRem(7) = _lblFloatRem_7
		Me.lblFloatRem(6) = _lblFloatRem_6
		Me.lblFloatRem(5) = _lblFloatRem_5
		Me.lblFloatRem(4) = _lblFloatRem_4
		Me.lblFloatRem(3) = _lblFloatRem_3
		Me.lblFloatRem(2) = _lblFloatRem_2
	End Sub
	Sub InitializelblConfirm()
		Me.lblConfirm(11) = _lblConfirm_11
		Me.lblConfirm(10) = _lblConfirm_10
		Me.lblConfirm(9) = _lblConfirm_9
		Me.lblConfirm(8) = _lblConfirm_8
		Me.lblConfirm(7) = _lblConfirm_7
		Me.lblConfirm(6) = _lblConfirm_6
		Me.lblConfirm(5) = _lblConfirm_5
		Me.lblConfirm(4) = _lblConfirm_4
		Me.lblConfirm(3) = _lblConfirm_3
		Me.lblConfirm(2) = _lblConfirm_2
		Me.lblConfirm(1) = _lblConfirm_1
		Me.lblConfirm(0) = _lblConfirm_0
	End Sub
	Sub InitializelblCashTot()
		Me.lblCashTot(11) = _lblCashTot_11
		Me.lblCashTot(15) = _lblCashTot_15
		Me.lblCashTot(14) = _lblCashTot_14
		Me.lblCashTot(12) = _lblCashTot_12
		Me.lblCashTot(9) = _lblCashTot_9
		Me.lblCashTot(8) = _lblCashTot_8
		Me.lblCashTot(3) = _lblCashTot_3
		Me.lblCashTot(2) = _lblCashTot_2
		Me.lblCashTot(1) = _lblCashTot_1
		Me.lblCashTot(0) = _lblCashTot_0
		Me.lblCashTot(6) = _lblCashTot_6
		Me.lblCashTot(13) = _lblCashTot_13
		Me.lblCashTot(10) = _lblCashTot_10
		Me.lblCashTot(7) = _lblCashTot_7
		Me.lblCashTot(5) = _lblCashTot_5
		Me.lblCashTot(4) = _lblCashTot_4
	End Sub
	Sub InitializefraFloat()
		Me.fraFloat(11) = _fraFloat_11
		Me.fraFloat(1) = _fraFloat_1
		Me.fraFloat(0) = _fraFloat_0
		Me.fraFloat(15) = _fraFloat_15
		Me.fraFloat(14) = _fraFloat_14
		Me.fraFloat(13) = _fraFloat_13
		Me.fraFloat(12) = _fraFloat_12
		Me.fraFloat(10) = _fraFloat_10
		Me.fraFloat(9) = _fraFloat_9
		Me.fraFloat(8) = _fraFloat_8
		Me.fraFloat(7) = _fraFloat_7
		Me.fraFloat(6) = _fraFloat_6
		Me.fraFloat(5) = _fraFloat_5
		Me.fraFloat(4) = _fraFloat_4
		Me.fraFloat(3) = _fraFloat_3
		Me.fraFloat(2) = _fraFloat_2
	End Sub
	Sub InitializefraCash()
		Me.fraCash(11) = _fraCash_11
		Me.fraCash(15) = _fraCash_15
		Me.fraCash(14) = _fraCash_14
		Me.fraCash(12) = _fraCash_12
		Me.fraCash(9) = _fraCash_9
		Me.fraCash(8) = _fraCash_8
		Me.fraCash(3) = _fraCash_3
		Me.fraCash(2) = _fraCash_2
		Me.fraCash(1) = _fraCash_1
		Me.fraCash(0) = _fraCash_0
		Me.fraCash(6) = _fraCash_6
		Me.fraCash(13) = _fraCash_13
		Me.fraCash(10) = _fraCash_10
		Me.fraCash(7) = _fraCash_7
		Me.fraCash(5) = _fraCash_5
		Me.fraCash(4) = _fraCash_4
	End Sub
#End Region 
End Class