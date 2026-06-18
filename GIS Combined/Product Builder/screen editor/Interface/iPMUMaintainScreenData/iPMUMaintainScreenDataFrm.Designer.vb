<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializeuctSumInsured1()
        InitializeuctStandardWording1()
        InitializeuctAddress1()
        InitializetxtText()
        InitializepnlPanel()
        InitializemnuFormat()
        InitializelvwListView()
        InitializelblTextLabel()
        InitializelblDateLabel()
        InitializelblComboLabel()
        InitializelblCheckLabel()
        InitializefraFrame()
        InitializecmdListViewSequenceUp()
        InitializecmdListViewSequenceDown()
        InitializecmdListViewEdit()
        InitializecmdListViewDelete()
        InitializecmdListViewAdd()
        InitializecmdCommand()
        InitializechkYesNo()
        InitializecboCombo()
        InitializePBFindControl()
        lvwListView_0_InitializeColumnKeys()
        Form_Initialize_Renamed()
    End Sub
    Private Sub Ctx_mnuControl_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuControl.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuControl.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuControl.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuControl.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuControl_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuControl.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuControl.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuControl.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub Ctx_mnuFrame_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuFrame.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuFrame.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuFrame.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuFrame.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuFrame_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuFrame.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuFrame.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuFrame.DropDownItems.Add(item)
        Next item
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
    Private Sub Ctx_mnuTab_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuTab.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuTab.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuTab.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuTab.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuTab_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuTab.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuTab.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuTab.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub Ctx_mnuTreeView_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuTreeView.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuTreeView.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuTreeView.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuTreeView.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuTreeView_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuTreeView.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuTreeView.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuTreeView.DropDownItems.Add(item)
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
    Public WithEvents mnuTabAdd As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTabDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTabMove As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTabRename As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTabSetControlTabOrder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSnapToGrid As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDefaultScreenSize As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTab As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlCaption As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlEntry As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_0 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_2 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_3 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_4 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_5 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_6 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_7 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_8 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_9 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_10 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_11 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_12 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_13 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_14 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_15 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_16 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_17 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_18 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_19 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_20 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_21 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_22 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_23 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_24 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_25 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_26 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_27 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_28 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuFormat_29 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlFormat As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlIncludeInList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlSetListColumnOrder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControlNameLabel As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuControl As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFrameCaption As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFrameDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFrameShowRateAndPremium As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFrameShowValuation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFrame As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListViewDrillDown As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTreeViewDataType As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTreeView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdDynamicLogic As System.Windows.Forms.Button
    Public WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents cmdCopy As System.Windows.Forms.Button
    Public WithEvents cmdDefaults As System.Windows.Forms.Button
    Public WithEvents cmdValidation As System.Windows.Forms.Button
    Public WithEvents cmdSave As System.Windows.Forms.Button
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lblScreenCode As System.Windows.Forms.Label
    Public WithEvents lblDataDictionary As System.Windows.Forms.Label
    Public WithEvents lblScreenLayout As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblPosition As System.Windows.Forms.Label
    Public WithEvents lblSizer As System.Windows.Forms.Label
    Public WithEvents pnlPosition As System.Windows.Forms.Label
    Public WithEvents pnlSize As System.Windows.Forms.Label
    Public WithEvents VScroll1 As System.Windows.Forms.VScrollBar
    Public WithEvents HScroll1 As System.Windows.Forms.HScrollBar
    Public WithEvents uctCLMCaseHeader1 As uctCLMCaseHeaders.uctCLMCaseHeader
    Public WithEvents uctCLMCaseClaim1 As uctCLMCaseClaimList.uctCLMCaseClaim
    Private WithEvents _cmdListViewSequenceUp_0 As System.Windows.Forms.Button
    Private WithEvents _cmdListViewSequenceDown_0 As System.Windows.Forms.Button
    Public WithEvents uctClaimPayment1 As uctClaimPayments.uctClaimPayment
    Public WithEvents uctCLMPerilDT1 As uctCLMPerilDTControl.uctCLMPerilDT
    Private WithEvents _cboCombo_0 As uctSimulateCombo
    Public WithEvents frmSizer As System.Windows.Forms.Panel
    Public WithEvents uctAssociatedClients As System.Windows.Forms.TextBox
    Public WithEvents uctClaimPeril As System.Windows.Forms.TextBox
    Private WithEvents _cmdCommand_0 As System.Windows.Forms.Button
    Private WithEvents _cmdListViewDelete_0 As System.Windows.Forms.Button
    Private WithEvents _cmdListViewEdit_0 As System.Windows.Forms.Button
    Private WithEvents _cmdListViewAdd_0 As System.Windows.Forms.Button
    Private WithEvents _fraFrame_0 As System.Windows.Forms.GroupBox
    Private WithEvents _chkYesNo_0 As System.Windows.Forms.CheckBox
    Private WithEvents _txtText_0 As System.Windows.Forms.TextBox
    Public WithEvents uctClaimReserve1 As uctClaimReserves.uctClaimReserve
    Private WithEvents _uctSumInsured1_0 As uctSumsInsured.uctSumInsured
    Private WithEvents _uctStandardWording1_0 As uctStandardWordings.uctStandardWording
    Private WithEvents _PBFindControl_0 As uctPBFindDesign.PBFindDesign
    Private WithEvents _uctAddress1_0 As uctAddressControl.uctAddress
    Private WithEvents _lvwListView_0_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwListView_0 As System.Windows.Forms.ListView
    Private WithEvents _0 As System.Windows.Forms.TabPage
    Public WithEvents TabStrip1_Tabs As System.Windows.Forms.TabControl.TabPageCollection
    Public WithEvents TabStrip1 As System.Windows.Forms.TabControl
    Private WithEvents _lblDateLabel_0 As System.Windows.Forms.Label
    Private WithEvents _pnlPanel_0 As System.Windows.Forms.Label
    Private WithEvents _lblTextLabel_0 As System.Windows.Forms.Label
    Private WithEvents _lblCheckLabel_0 As System.Windows.Forms.Label
    Private WithEvents _lblComboLabel_0 As System.Windows.Forms.Label
    Public WithEvents picScreen As System.Windows.Forms.PictureBox
    Public WithEvents picScreenMain As System.Windows.Forms.PictureBox
    Public WithEvents tvwDataDictionary As System.Windows.Forms.TreeView
    Public WithEvents txtScreenCode As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public PBFindControl(0) As uctPBFindDesign.PBFindDesign
    Public cboCombo(0) As uctSimulateCombo
    Public chkYesNo(0) As System.Windows.Forms.CheckBox
    Public cmdCommand(0) As System.Windows.Forms.Button
    Public cmdListViewAdd(0) As System.Windows.Forms.Button
    Public cmdListViewDelete(0) As System.Windows.Forms.Button
    Public cmdListViewEdit(0) As System.Windows.Forms.Button
    Public cmdListViewSequenceDown(0) As System.Windows.Forms.Button
    Public cmdListViewSequenceUp(0) As System.Windows.Forms.Button
    Public fraFrame(0) As System.Windows.Forms.GroupBox
    Public lblCheckLabel(0) As System.Windows.Forms.Label
    Public lblComboLabel(0) As System.Windows.Forms.Label
    Public lblDateLabel(0) As System.Windows.Forms.Label
    Public lblTextLabel(0) As System.Windows.Forms.Label
    Public lvwListView(0) As System.Windows.Forms.ListView
    Public mnuFormat(29) As System.Windows.Forms.ToolStripMenuItem
    Public pnlPanel(0) As System.Windows.Forms.Label
    Public txtText(0) As System.Windows.Forms.TextBox
    Public uctAddress1(0) As uctAddressControl.uctAddress
    Public uctStandardWording1(0) As uctStandardWordings.uctStandardWording
    Public uctSumInsured1(0) As uctSumsInsured.uctSumInsured
    Public WithEvents Ctx_mnuControl As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuFrame As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuListView As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuTab As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuTreeView As System.Windows.Forms.ContextMenuStrip
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._lvwListView_0 = New System.Windows.Forms.ListView()
        Me._lvwListView_0_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTabAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTabDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTabMove = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTabRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTabSetControlTabOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSnapToGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDefaultScreenSize = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlCaption = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlEntry = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlFormat = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_3 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_4 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_5 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_6 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_7 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_8 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_9 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_10 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_11 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_12 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_13 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_14 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_15 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_16 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_17 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_18 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_19 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_20 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_21 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_22 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_23 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_24 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_25 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_26 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_27 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_28 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuFormat_29 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlIncludeInList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlSetListColumnOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuControlNameLabel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrame = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrameCaption = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrameDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrameShowRateAndPremium = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFrameShowValuation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListViewDrillDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTreeView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTreeViewDataType = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdDynamicLogic = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.cmdCopy = New System.Windows.Forms.Button()
        Me.cmdDefaults = New System.Windows.Forms.Button()
        Me.cmdValidation = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.grpCompiledRuleAssembly = New System.Windows.Forms.GroupBox()
        Me.lblValidation = New System.Windows.Forms.Label()
        Me.lblDefaults = New System.Windows.Forms.Label()
        Me.UctCompiledRuleValidation = New uctCompiledRule.uctCompiledRule()
        Me.UctCompiledRuleDefaults = New uctCompiledRule.uctCompiledRule()
        Me.chkEnableCompileRule = New System.Windows.Forms.CheckBox()
        Me.lblScreenCode = New System.Windows.Forms.Label()
        Me.lblDataDictionary = New System.Windows.Forms.Label()
        Me.lblScreenLayout = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblPosition = New System.Windows.Forms.Label()
        Me.lblSizer = New System.Windows.Forms.Label()
        Me.pnlPosition = New System.Windows.Forms.Label()
        Me.pnlSize = New System.Windows.Forms.Label()
        Me.picScreenMain = New System.Windows.Forms.PictureBox()
        Me.VScroll1 = New System.Windows.Forms.VScrollBar()
        Me.HScroll1 = New System.Windows.Forms.HScrollBar()
        Me.picScreen = New System.Windows.Forms.PictureBox()
        Me.uctCLMCaseHeader1 = New uctCLMCaseHeaders.uctCLMCaseHeader()
        Me.uctCLMCaseClaim1 = New uctCLMCaseClaimList.uctCLMCaseClaim()
        Me._cmdListViewSequenceUp_0 = New System.Windows.Forms.Button()
        Me._cmdListViewSequenceDown_0 = New System.Windows.Forms.Button()
        Me.uctClaimPayment1 = New uctClaimPayments.uctClaimPayment()
        Me.uctCLMPerilDT1 = New uctCLMPerilDTControl.uctCLMPerilDT()
        Me._cboCombo_0 = New iPMUMaintainScreenData.uctSimulateCombo()
        Me.frmSizer = New System.Windows.Forms.Panel()
        Me.uctAssociatedClients = New System.Windows.Forms.TextBox()
        Me.uctClaimPeril = New System.Windows.Forms.TextBox()
        Me._cmdCommand_0 = New System.Windows.Forms.Button()
        Me._cmdListViewDelete_0 = New System.Windows.Forms.Button()
        Me._cmdListViewEdit_0 = New System.Windows.Forms.Button()
        Me._cmdListViewAdd_0 = New System.Windows.Forms.Button()
        Me._fraFrame_0 = New System.Windows.Forms.GroupBox()
        Me._chkYesNo_0 = New System.Windows.Forms.CheckBox()
        Me._txtText_0 = New System.Windows.Forms.TextBox()
        Me.uctClaimReserve1 = New uctClaimReserves.uctClaimReserve()
        Me._uctSumInsured1_0 = New uctSumsInsured.uctSumInsured()
        Me._uctStandardWording1_0 = New uctStandardWordings.uctStandardWording()
        Me._PBFindControl_0 = New uctPBFindDesign.PBFindDesign()
        Me._uctAddress1_0 = New uctAddressControl.uctAddress()
        Me.TabStrip1 = New System.Windows.Forms.TabControl()
        Me._0 = New System.Windows.Forms.TabPage()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me._lblDateLabel_0 = New System.Windows.Forms.Label()
        Me._pnlPanel_0 = New System.Windows.Forms.Label()
        Me._lblTextLabel_0 = New System.Windows.Forms.Label()
        Me._lblCheckLabel_0 = New System.Windows.Forms.Label()
        Me._lblComboLabel_0 = New System.Windows.Forms.Label()
        Me.tvwDataDictionary = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.txtScreenCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Ctx_mnuControl = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuFrame = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuListView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuTreeView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.grpCompiledRuleAssembly.SuspendLayout()
        CType(Me.picScreenMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picScreenMain.SuspendLayout()
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picScreen.SuspendLayout()
        Me.TabStrip1.SuspendLayout()
        Me._0.SuspendLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_lvwListView_0
        '
        Me._lvwListView_0.BackColor = System.Drawing.SystemColors.Window
        Me._lvwListView_0.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwListView_0_ColumnHeader_1})
        Me._lvwListView_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwListView_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lvwListView_0.Location = New System.Drawing.Point(17, 20)
        Me._lvwListView_0.Name = "_lvwListView_0"
        Me._lvwListView_0.Size = New System.Drawing.Size(249, 49)
        Me._lvwListView_0.TabIndex = 36
        Me.ToolTip1.SetToolTip(Me._lvwListView_0, "Test_Object")
        Me._lvwListView_0.UseCompatibleStateImageBehavior = False
        Me._lvwListView_0.View = System.Windows.Forms.View.Details
        Me._lvwListView_0.Visible = False
        '
        '_lvwListView_0_ColumnHeader_1
        '
        Me._lvwListView_0_ColumnHeader_1.Tag = ""
        Me._lvwListView_0_ColumnHeader_1.Text = "Thing"
        Me._lvwListView_0_ColumnHeader_1.Width = 97
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTab, Me.mnuControl, Me.mnuFrame, Me.mnuListView, Me.mnuTreeView})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(926, 24)
        Me.MainMenu1.TabIndex = 21
        Me.MainMenu1.Visible = False
        '
        'mnuTab
        '
        Me.mnuTab.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTabAdd, Me.mnuTabDelete, Me.mnuTabMove, Me.mnuTabRename, Me.mnuTabSetControlTabOrder, Me.mnuSnapToGrid, Me.mnuDefaultScreenSize})
        Me.mnuTab.Name = "mnuTab"
        Me.mnuTab.Size = New System.Drawing.Size(66, 20)
        Me.mnuTab.Text = "Tab Menu"
        '
        'mnuTabAdd
        '
        Me.mnuTabAdd.Name = "mnuTabAdd"
        Me.mnuTabAdd.Size = New System.Drawing.Size(185, 22)
        Me.mnuTabAdd.Text = "&Add Tab"
        '
        'mnuTabDelete
        '
        Me.mnuTabDelete.Name = "mnuTabDelete"
        Me.mnuTabDelete.Size = New System.Drawing.Size(185, 22)
        Me.mnuTabDelete.Text = "&Delete Tab"
        '
        'mnuTabMove
        '
        Me.mnuTabMove.Name = "mnuTabMove"
        Me.mnuTabMove.Size = New System.Drawing.Size(185, 22)
        Me.mnuTabMove.Text = "&Move Tab"
        '
        'mnuTabRename
        '
        Me.mnuTabRename.Name = "mnuTabRename"
        Me.mnuTabRename.Size = New System.Drawing.Size(185, 22)
        Me.mnuTabRename.Text = "&Rename Tab"
        '
        'mnuTabSetControlTabOrder
        '
        Me.mnuTabSetControlTabOrder.Name = "mnuTabSetControlTabOrder"
        Me.mnuTabSetControlTabOrder.Size = New System.Drawing.Size(185, 22)
        Me.mnuTabSetControlTabOrder.Text = "&Set Controls Tab Order"
        '
        'mnuSnapToGrid
        '
        Me.mnuSnapToGrid.Name = "mnuSnapToGrid"
        Me.mnuSnapToGrid.Size = New System.Drawing.Size(185, 22)
        Me.mnuSnapToGrid.Text = "S&nap Controls"
        '
        'mnuDefaultScreenSize
        '
        Me.mnuDefaultScreenSize.Name = "mnuDefaultScreenSize"
        Me.mnuDefaultScreenSize.Size = New System.Drawing.Size(185, 22)
        Me.mnuDefaultScreenSize.Text = "Default Screen Size"
        '
        'mnuControl
        '
        Me.mnuControl.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuControlDelete, Me.mnuControlCaption, Me.mnuControlHelp, Me.mnuControlEntry, Me.mnuControlFormat, Me.mnuControlIncludeInList, Me.mnuControlSetListColumnOrder, Me.mnuControlNameLabel})
        Me.mnuControl.Name = "mnuControl"
        Me.mnuControl.Size = New System.Drawing.Size(83, 20)
        Me.mnuControl.Text = "Control Menu"
        '
        'mnuControlDelete
        '
        Me.mnuControlDelete.Name = "mnuControlDelete"
        Me.mnuControlDelete.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlDelete.Text = "&Delete Control"
        '
        'mnuControlCaption
        '
        Me.mnuControlCaption.Name = "mnuControlCaption"
        Me.mnuControlCaption.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlCaption.Text = "Change &Caption"
        '
        'mnuControlHelp
        '
        Me.mnuControlHelp.Name = "mnuControlHelp"
        Me.mnuControlHelp.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlHelp.Text = "Change &Help Text"
        '
        'mnuControlEntry
        '
        Me.mnuControlEntry.Name = "mnuControlEntry"
        Me.mnuControlEntry.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlEntry.Text = "Change &Entry Requirements"
        '
        'mnuControlFormat
        '
        Me.mnuControlFormat.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuFormat_0, Me._mnuFormat_1, Me._mnuFormat_2, Me._mnuFormat_3, Me._mnuFormat_4, Me._mnuFormat_5, Me._mnuFormat_6, Me._mnuFormat_7, Me._mnuFormat_8, Me._mnuFormat_9, Me._mnuFormat_10, Me._mnuFormat_11, Me._mnuFormat_12, Me._mnuFormat_13, Me._mnuFormat_14, Me._mnuFormat_15, Me._mnuFormat_16, Me._mnuFormat_17, Me._mnuFormat_18, Me._mnuFormat_19, Me._mnuFormat_20, Me._mnuFormat_21, Me._mnuFormat_22, Me._mnuFormat_23, Me._mnuFormat_24, Me._mnuFormat_25, Me._mnuFormat_26, Me._mnuFormat_27, Me._mnuFormat_28, Me._mnuFormat_29})
        Me.mnuControlFormat.Name = "mnuControlFormat"
        Me.mnuControlFormat.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlFormat.Text = "Change &Format"
        '
        '_mnuFormat_0
        '
        Me._mnuFormat_0.Name = "_mnuFormat_0"
        Me._mnuFormat_0.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_0.Text = "Normal String"
        '
        '_mnuFormat_1
        '
        Me._mnuFormat_1.Name = "_mnuFormat_1"
        Me._mnuFormat_1.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_1.Text = "Capitalise First Letter"
        '
        '_mnuFormat_2
        '
        Me._mnuFormat_2.Name = "_mnuFormat_2"
        Me._mnuFormat_2.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_2.Text = "Date - Short"
        '
        '_mnuFormat_3
        '
        Me._mnuFormat_3.Name = "_mnuFormat_3"
        Me._mnuFormat_3.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_3.Text = "Date - Medium"
        '
        '_mnuFormat_4
        '
        Me._mnuFormat_4.Name = "_mnuFormat_4"
        Me._mnuFormat_4.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_4.Text = "Date - Long"
        '
        '_mnuFormat_5
        '
        Me._mnuFormat_5.Name = "_mnuFormat_5"
        Me._mnuFormat_5.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_5.Text = "Time - Short"
        '
        '_mnuFormat_6
        '
        Me._mnuFormat_6.Name = "_mnuFormat_6"
        Me._mnuFormat_6.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_6.Text = "Time - Medium"
        '
        '_mnuFormat_7
        '
        Me._mnuFormat_7.Name = "_mnuFormat_7"
        Me._mnuFormat_7.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_7.Text = "Time - Long"
        '
        '_mnuFormat_8
        '
        Me._mnuFormat_8.Name = "_mnuFormat_8"
        Me._mnuFormat_8.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_8.Text = "Date + Time - Short"
        '
        '_mnuFormat_9
        '
        Me._mnuFormat_9.Name = "_mnuFormat_9"
        Me._mnuFormat_9.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_9.Text = "Date + Time - Medium"
        '
        '_mnuFormat_10
        '
        Me._mnuFormat_10.Name = "_mnuFormat_10"
        Me._mnuFormat_10.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_10.Text = "Date + Time - Long"
        '
        '_mnuFormat_11
        '
        Me._mnuFormat_11.Name = "_mnuFormat_11"
        Me._mnuFormat_11.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_11.Text = "Currency"
        '
        '_mnuFormat_12
        '
        Me._mnuFormat_12.Name = "_mnuFormat_12"
        Me._mnuFormat_12.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_12.Text = "Integer"
        '
        '_mnuFormat_13
        '
        Me._mnuFormat_13.Name = "_mnuFormat_13"
        Me._mnuFormat_13.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_13.Text = "Tri-state"
        '
        '_mnuFormat_14
        '
        Me._mnuFormat_14.Name = "_mnuFormat_14"
        Me._mnuFormat_14.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_14.Text = "Upper Case"
        '
        '_mnuFormat_15
        '
        Me._mnuFormat_15.Name = "_mnuFormat_15"
        Me._mnuFormat_15.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_15.Text = "Date - Year Only"
        '
        '_mnuFormat_16
        '
        Me._mnuFormat_16.Name = "_mnuFormat_16"
        Me._mnuFormat_16.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_16.Text = "Percent"
        '
        '_mnuFormat_17
        '
        Me._mnuFormat_17.Name = "_mnuFormat_17"
        Me._mnuFormat_17.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_17.Text = "Double"
        '
        '_mnuFormat_18
        '
        Me._mnuFormat_18.Name = "_mnuFormat_18"
        Me._mnuFormat_18.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_18.Text = "Long"
        '
        '_mnuFormat_19
        '
        Me._mnuFormat_19.Name = "_mnuFormat_19"
        Me._mnuFormat_19.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_19.Text = "Whole Money"
        '
        '_mnuFormat_20
        '
        Me._mnuFormat_20.Name = "_mnuFormat_20"
        Me._mnuFormat_20.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_20.Text = "Money"
        '
        '_mnuFormat_21
        '
        Me._mnuFormat_21.Name = "_mnuFormat_21"
        Me._mnuFormat_21.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_21.Text = "Decimal"
        '
        '_mnuFormat_22
        '
        Me._mnuFormat_22.Name = "_mnuFormat_22"
        Me._mnuFormat_22.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_22.Text = "Date - Month Only Long"
        '
        '_mnuFormat_23
        '
        Me._mnuFormat_23.Name = "_mnuFormat_23"
        Me._mnuFormat_23.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_23.Text = "Date - Month Only Medium"
        '
        '_mnuFormat_24
        '
        Me._mnuFormat_24.Name = "_mnuFormat_24"
        Me._mnuFormat_24.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_24.Text = "Date - Month Only Short"
        '
        '_mnuFormat_25
        '
        Me._mnuFormat_25.Name = "_mnuFormat_25"
        Me._mnuFormat_25.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_25.Text = "Date - Day Only Long"
        '
        '_mnuFormat_26
        '
        Me._mnuFormat_26.Name = "_mnuFormat_26"
        Me._mnuFormat_26.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_26.Text = "Date - Day Only Medium"
        '
        '_mnuFormat_27
        '
        Me._mnuFormat_27.Name = "_mnuFormat_27"
        Me._mnuFormat_27.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_27.Text = "Date - Day Only Short"
        '
        '_mnuFormat_28
        '
        Me._mnuFormat_28.Name = "_mnuFormat_28"
        Me._mnuFormat_28.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_28.Text = "Multiline"
        '
        '_mnuFormat_29
        '
        Me._mnuFormat_29.Name = "_mnuFormat_29"
        Me._mnuFormat_29.Size = New System.Drawing.Size(201, 22)
        Me._mnuFormat_29.Text = "<format>"
        '
        'mnuControlIncludeInList
        '
        Me.mnuControlIncludeInList.Name = "mnuControlIncludeInList"
        Me.mnuControlIncludeInList.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlIncludeInList.Text = "Include in List"
        '
        'mnuControlSetListColumnOrder
        '
        Me.mnuControlSetListColumnOrder.Name = "mnuControlSetListColumnOrder"
        Me.mnuControlSetListColumnOrder.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlSetListColumnOrder.Text = "Set List Column Order"
        '
        'mnuControlNameLabel
        '
        Me.mnuControlNameLabel.Name = "mnuControlNameLabel"
        Me.mnuControlNameLabel.Size = New System.Drawing.Size(209, 22)
        Me.mnuControlNameLabel.Text = "Name Label"
        '
        'mnuFrame
        '
        Me.mnuFrame.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFrameCaption, Me.mnuFrameDelete, Me.mnuFrameShowRateAndPremium, Me.mnuFrameShowValuation})
        Me.mnuFrame.Name = "mnuFrame"
        Me.mnuFrame.Size = New System.Drawing.Size(78, 20)
        Me.mnuFrame.Text = "Frame Menu"
        '
        'mnuFrameCaption
        '
        Me.mnuFrameCaption.Name = "mnuFrameCaption"
        Me.mnuFrameCaption.Size = New System.Drawing.Size(191, 22)
        Me.mnuFrameCaption.Text = "Change &Caption"
        '
        'mnuFrameDelete
        '
        Me.mnuFrameDelete.Name = "mnuFrameDelete"
        Me.mnuFrameDelete.Size = New System.Drawing.Size(191, 22)
        Me.mnuFrameDelete.Text = "&Delete Frame"
        '
        'mnuFrameShowRateAndPremium
        '
        Me.mnuFrameShowRateAndPremium.Name = "mnuFrameShowRateAndPremium"
        Me.mnuFrameShowRateAndPremium.Size = New System.Drawing.Size(191, 22)
        Me.mnuFrameShowRateAndPremium.Text = "Show &Rate And Premium"
        '
        'mnuFrameShowValuation
        '
        Me.mnuFrameShowValuation.Name = "mnuFrameShowValuation"
        Me.mnuFrameShowValuation.Size = New System.Drawing.Size(191, 22)
        Me.mnuFrameShowValuation.Text = "Show &Valuation"
        '
        'mnuListView
        '
        Me.mnuListView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuListViewDrillDown})
        Me.mnuListView.Name = "mnuListView"
        Me.mnuListView.Size = New System.Drawing.Size(86, 20)
        Me.mnuListView.Text = "Listview Menu"
        '
        'mnuListViewDrillDown
        '
        Me.mnuListViewDrillDown.Name = "mnuListViewDrillDown"
        Me.mnuListViewDrillDown.Size = New System.Drawing.Size(121, 22)
        Me.mnuListViewDrillDown.Text = "&Drill Down"
        '
        'mnuTreeView
        '
        Me.mnuTreeView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTreeViewDataType})
        Me.mnuTreeView.Name = "mnuTreeView"
        Me.mnuTreeView.Size = New System.Drawing.Size(92, 20)
        Me.mnuTreeView.Text = "Treeview Menu"
        '
        'mnuTreeViewDataType
        '
        Me.mnuTreeViewDataType.Name = "mnuTreeViewDataType"
        Me.mnuTreeViewDataType.Size = New System.Drawing.Size(124, 22)
        Me.mnuTreeViewDataType.Text = "&Data Type"
        '
        'cmdDynamicLogic
        '
        Me.cmdDynamicLogic.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDynamicLogic, True)
        Me.cmdDynamicLogic.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDynamicLogic, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDynamicLogic, Nothing)
        Me.cmdDynamicLogic.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDynamicLogic.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDynamicLogic.Location = New System.Drawing.Point(448, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDynamicLogic, System.Drawing.Color.Silver)
        Me.cmdDynamicLogic.Name = "cmdDynamicLogic"
        Me.cmdDynamicLogic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDynamicLogic.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdDynamicLogic, 0)
        Me.cmdDynamicLogic.TabIndex = 20
        Me.cmdDynamicLogic.Text = "Log&ic"
        Me.cmdDynamicLogic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDynamicLogic.UseVisualStyleBackColor = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 10
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdCopy, True)
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdCopy, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdCopy, Nothing)
        Me.cmdCopy.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(289, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdCopy, System.Drawing.Color.Silver)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdCopy, 0)
        Me.cmdCopy.TabIndex = 3
        Me.cmdCopy.Text = "C&opy"
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'cmdDefaults
        '
        Me.cmdDefaults.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDefaults, True)
        Me.cmdDefaults.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDefaults, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDefaults, Nothing)
        Me.cmdDefaults.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaults.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaults.Location = New System.Drawing.Point(369, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDefaults, System.Drawing.Color.Silver)
        Me.cmdDefaults.Name = "cmdDefaults"
        Me.cmdDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaults.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdDefaults, 0)
        Me.cmdDefaults.TabIndex = 4
        Me.cmdDefaults.Text = "&Defaults"
        Me.cmdDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDefaults.UseVisualStyleBackColor = False
        '
        'cmdValidation
        '
        Me.cmdValidation.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdValidation, True)
        Me.cmdValidation.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdValidation, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdValidation, Nothing)
        Me.cmdValidation.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdValidation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdValidation.Location = New System.Drawing.Point(527, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdValidation, System.Drawing.Color.Silver)
        Me.cmdValidation.Name = "cmdValidation"
        Me.cmdValidation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdValidation.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdValidation, 0)
        Me.cmdValidation.TabIndex = 5
        Me.cmdValidation.Text = "&Validation"
        Me.cmdValidation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdValidation.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdSave, True)
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdSave, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdSave, Nothing)
        Me.cmdSave.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(607, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdSave, System.Drawing.Color.Silver)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdSave, 0)
        Me.cmdSave.TabIndex = 6
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNavigate, True)
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNavigate, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNavigate, Nothing)
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(12, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNavigate, System.Drawing.Color.Silver)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNavigate, 0)
        Me.cmdNavigate.TabIndex = 10
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdHelp, True)
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdHelp, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdHelp, Nothing)
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(844, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdHelp, System.Drawing.Color.Silver)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdHelp, 0)
        Me.cmdHelp.TabIndex = 9
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdCancel, True)
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdCancel, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdCancel, Nothing)
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(765, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdCancel, System.Drawing.Color.Silver)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdCancel, 0)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdOK, True)
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdOK, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdOK, Nothing)
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(686, 627)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdOK, System.Drawing.Color.Silver)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdOK, 0)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.AllowDrop = True
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(180, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 28)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(909, 579)
        Me.tabMainTab.TabIndex = 11
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.AllowDrop = True
        Me._tabMainTab_TabPage0.Controls.Add(Me.grpCompiledRuleAssembly)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkEnableCompileRule)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblScreenCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDataDictionary)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblScreenLayout)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPosition)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSizer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlPosition)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlSize)
        Me._tabMainTab_TabPage0.Controls.Add(Me.picScreenMain)
        Me._tabMainTab_TabPage0.Controls.Add(Me.tvwDataDictionary)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtScreenCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(901, 553)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'grpCompiledRuleAssembly
        '
        Me.grpCompiledRuleAssembly.Controls.Add(Me.lblValidation)
        Me.grpCompiledRuleAssembly.Controls.Add(Me.lblDefaults)
        Me.grpCompiledRuleAssembly.Controls.Add(Me.UctCompiledRuleValidation)
        Me.grpCompiledRuleAssembly.Controls.Add(Me.UctCompiledRuleDefaults)
        Me.grpCompiledRuleAssembly.Location = New System.Drawing.Point(278, 27)
        Me.grpCompiledRuleAssembly.Name = "grpCompiledRuleAssembly"
        Me.grpCompiledRuleAssembly.Size = New System.Drawing.Size(357, 77)
        Me.grpCompiledRuleAssembly.TabIndex = 50
        Me.grpCompiledRuleAssembly.TabStop = False
        Me.grpCompiledRuleAssembly.Text = "Compiled Rule Assembly"
        '
        'lblValidation
        '
        Me.lblValidation.AutoSize = True
        Me.lblValidation.Location = New System.Drawing.Point(12, 53)
        Me.lblValidation.Name = "lblValidation"
        Me.lblValidation.Size = New System.Drawing.Size(68, 13)
        Me.lblValidation.TabIndex = 53
        Me.lblValidation.Text = "Validation:"
        '
        'lblDefaults
        '
        Me.lblDefaults.AutoSize = True
        Me.lblDefaults.Location = New System.Drawing.Point(12, 22)
        Me.lblDefaults.Name = "lblDefaults"
        Me.lblDefaults.Size = New System.Drawing.Size(59, 13)
        Me.lblDefaults.TabIndex = 52
        Me.lblDefaults.Text = "Defaults:"
        '
        'UctCompiledRuleValidation
        '
        Me.UctCompiledRuleValidation.bEnterOnlyAssemblyName = False
        Me.UctCompiledRuleValidation.Location = New System.Drawing.Point(107, 46)
        Me.UctCompiledRuleValidation.Name = "UctCompiledRuleValidation"
        Me.UctCompiledRuleValidation.Size = New System.Drawing.Size(244, 26)
        Me.UctCompiledRuleValidation.TabIndex = 51
        '
        'UctCompiledRuleDefaults
        '
        Me.UctCompiledRuleDefaults.bEnterOnlyAssemblyName = False
        Me.UctCompiledRuleDefaults.Location = New System.Drawing.Point(106, 15)
        Me.UctCompiledRuleDefaults.Name = "UctCompiledRuleDefaults"
        Me.UctCompiledRuleDefaults.Size = New System.Drawing.Size(244, 26)
        Me.UctCompiledRuleDefaults.TabIndex = 50
        '
        'chkEnableCompileRule
        '
        Me.chkEnableCompileRule.AutoSize = True
        Me.chkEnableCompileRule.Location = New System.Drawing.Point(27, 30)
        Me.chkEnableCompileRule.Name = "chkEnableCompileRule"
        Me.chkEnableCompileRule.Size = New System.Drawing.Size(157, 17)
        Me.chkEnableCompileRule.TabIndex = 46
        Me.chkEnableCompileRule.Text = "Enable Compiled Rules"
        Me.chkEnableCompileRule.UseVisualStyleBackColor = True
        '
        'lblScreenCode
        '
        Me.lblScreenCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblScreenCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScreenCode.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScreenCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScreenCode.Location = New System.Drawing.Point(24, 7)
        Me.lblScreenCode.Name = "lblScreenCode"
        Me.lblScreenCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScreenCode.Size = New System.Drawing.Size(101, 17)
        Me.lblScreenCode.TabIndex = 12
        Me.lblScreenCode.Text = "Screen code:"
        '
        'lblDataDictionary
        '
        Me.lblDataDictionary.BackColor = System.Drawing.SystemColors.Control
        Me.lblDataDictionary.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataDictionary.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataDictionary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDataDictionary.Location = New System.Drawing.Point(24, 85)
        Me.lblDataDictionary.Name = "lblDataDictionary"
        Me.lblDataDictionary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataDictionary.Size = New System.Drawing.Size(101, 17)
        Me.lblDataDictionary.TabIndex = 15
        Me.lblDataDictionary.Text = "Data Dictionary"
        '
        'lblScreenLayout
        '
        Me.lblScreenLayout.BackColor = System.Drawing.SystemColors.Control
        Me.lblScreenLayout.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScreenLayout.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScreenLayout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScreenLayout.Location = New System.Drawing.Point(280, 85)
        Me.lblScreenLayout.Name = "lblScreenLayout"
        Me.lblScreenLayout.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScreenLayout.Size = New System.Drawing.Size(101, 17)
        Me.lblScreenLayout.TabIndex = 16
        Me.lblScreenLayout.Text = "Screen layout"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(280, 7)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(101, 17)
        Me.lblDescription.TabIndex = 17
        Me.lblDescription.Text = "Description:"
        '
        'lblPosition
        '
        Me.lblPosition.BackColor = System.Drawing.SystemColors.Control
        Me.lblPosition.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPosition.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPosition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPosition.Location = New System.Drawing.Point(640, 7)
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPosition.Size = New System.Drawing.Size(58, 17)
        Me.lblPosition.TabIndex = 18
        Me.lblPosition.Text = "Position:"
        '
        'lblSizer
        '
        Me.lblSizer.BackColor = System.Drawing.SystemColors.Control
        Me.lblSizer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSizer.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSizer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSizer.Location = New System.Drawing.Point(641, 34)
        Me.lblSizer.Name = "lblSizer"
        Me.lblSizer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSizer.Size = New System.Drawing.Size(45, 17)
        Me.lblSizer.TabIndex = 19
        Me.lblSizer.Text = "Size:"
        '
        'pnlPosition
        '
        Me.pnlPosition.BackColor = System.Drawing.SystemColors.Control
        Me.pnlPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPosition.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlPosition.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPosition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlPosition.Location = New System.Drawing.Point(704, 4)
        Me.pnlPosition.Name = "pnlPosition"
        Me.pnlPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlPosition.Size = New System.Drawing.Size(188, 22)
        Me.pnlPosition.TabIndex = 2
        '
        'pnlSize
        '
        Me.pnlSize.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlSize.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlSize.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlSize.Location = New System.Drawing.Point(704, 30)
        Me.pnlSize.Name = "pnlSize"
        Me.pnlSize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlSize.Size = New System.Drawing.Size(188, 22)
        Me.pnlSize.TabIndex = 45
        '
        'picScreenMain
        '
        Me.picScreenMain.BackColor = System.Drawing.SystemColors.Window
        Me.picScreenMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picScreenMain.Controls.Add(Me.VScroll1)
        Me.picScreenMain.Controls.Add(Me.HScroll1)
        Me.picScreenMain.Controls.Add(Me.picScreen)
        Me.picScreenMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.picScreenMain.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picScreenMain.Location = New System.Drawing.Point(272, 110)
        Me.picScreenMain.Name = "picScreenMain"
        Me.picScreenMain.Size = New System.Drawing.Size(617, 433)
        Me.picScreenMain.TabIndex = 14
        Me.picScreenMain.TabStop = False
        '
        'VScroll1
        '
        Me.VScroll1.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScroll1.LargeChange = 1
        Me.VScroll1.Location = New System.Drawing.Point(592, 3)
        Me.VScroll1.Maximum = 32767
        Me.VScroll1.Name = "VScroll1"
        Me.VScroll1.Size = New System.Drawing.Size(17, 409)
        Me.VScroll1.TabIndex = 51
        Me.VScroll1.TabStop = True
        Me.VScroll1.Visible = False
        '
        'HScroll1
        '
        Me.HScroll1.Cursor = System.Windows.Forms.Cursors.Default
        Me.HScroll1.LargeChange = 1
        Me.HScroll1.Location = New System.Drawing.Point(0, 411)
        Me.HScroll1.Maximum = 32767
        Me.HScroll1.Name = "HScroll1"
        Me.HScroll1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HScroll1.Size = New System.Drawing.Size(593, 17)
        Me.HScroll1.TabIndex = 50
        Me.HScroll1.TabStop = True
        '
        'picScreen
        '
        Me.picScreen.BackColor = System.Drawing.Color.White
        Me.picScreen.Controls.Add(Me.uctCLMCaseHeader1)
        Me.picScreen.Controls.Add(Me.uctCLMCaseClaim1)
        Me.picScreen.Controls.Add(Me._cmdListViewSequenceUp_0)
        Me.picScreen.Controls.Add(Me._cmdListViewSequenceDown_0)
        Me.picScreen.Controls.Add(Me.uctClaimPayment1)
        Me.picScreen.Controls.Add(Me.uctCLMPerilDT1)
        Me.picScreen.Controls.Add(Me._cboCombo_0)
        Me.picScreen.Controls.Add(Me.frmSizer)
        Me.picScreen.Controls.Add(Me.uctAssociatedClients)
        Me.picScreen.Controls.Add(Me.uctClaimPeril)
        Me.picScreen.Controls.Add(Me._cmdCommand_0)
        Me.picScreen.Controls.Add(Me._cmdListViewDelete_0)
        Me.picScreen.Controls.Add(Me._cmdListViewEdit_0)
        Me.picScreen.Controls.Add(Me._cmdListViewAdd_0)
        Me.picScreen.Controls.Add(Me._fraFrame_0)
        Me.picScreen.Controls.Add(Me._chkYesNo_0)
        Me.picScreen.Controls.Add(Me._txtText_0)
        Me.picScreen.Controls.Add(Me.uctClaimReserve1)
        Me.picScreen.Controls.Add(Me._uctSumInsured1_0)
        Me.picScreen.Controls.Add(Me._uctStandardWording1_0)
        Me.picScreen.Controls.Add(Me._PBFindControl_0)
        Me.picScreen.Controls.Add(Me._uctAddress1_0)
        Me.picScreen.Controls.Add(Me._lvwListView_0)
        Me.picScreen.Controls.Add(Me.TabStrip1)
        Me.picScreen.Controls.Add(Me._lblDateLabel_0)
        Me.picScreen.Controls.Add(Me._pnlPanel_0)
        Me.picScreen.Controls.Add(Me._lblTextLabel_0)
        Me.picScreen.Controls.Add(Me._lblCheckLabel_0)
        Me.picScreen.Controls.Add(Me._lblComboLabel_0)
        Me.picScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.picScreen.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picScreen.Location = New System.Drawing.Point(0, 3)
        Me.picScreen.Name = "picScreen"
        Me.picScreen.Size = New System.Drawing.Size(608, 419)
        Me.picScreen.TabIndex = 22
        Me.picScreen.TabStop = False
        '
        'uctCLMCaseHeader1
        '
        Me.uctCLMCaseHeader1.BaseCaseID = 0
        Me.uctCLMCaseHeader1.CaseAssistantID = 0
        Me.uctCLMCaseHeader1.CaseID = 0
        Me.uctCLMCaseHeader1.CaseNumber = ""
        Me.uctCLMCaseHeader1.CaseOpenedDate = New Date(CType(0, Long))
        Me.uctCLMCaseHeader1.CaseProgressStatusCode = Nothing
        Me.uctCLMCaseHeader1.CaseProgressStatusID = 0
        Me.uctCLMCaseHeader1.CaseVersion = 0
        Me.uctCLMCaseHeader1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMCaseHeader1.Location = New System.Drawing.Point(480, 139)
        Me.uctCLMCaseHeader1.MinimumHeight = 1320
        Me.uctCLMCaseHeader1.MinimumWidth = 8520
        Me.uctCLMCaseHeader1.Name = "uctCLMCaseHeader1"
        Me.uctCLMCaseHeader1.Size = New System.Drawing.Size(657, 121)
        Me.uctCLMCaseHeader1.TabIndex = 53
        Me.uctCLMCaseHeader1.Task = 0
        Me.uctCLMCaseHeader1.Visible = False
        '
        'uctCLMCaseClaim1
        '
        Me.uctCLMCaseClaim1.BaseCaseId = 0
        Me.uctCLMCaseClaim1.CaseID = 0
        Me.uctCLMCaseClaim1.CaseProgressStatusCode = ""
        Me.uctCLMCaseClaim1.ClaimId = 0
        Me.uctCLMCaseClaim1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMCaseClaim1.Location = New System.Drawing.Point(472, 243)
        Me.uctCLMCaseClaim1.MinimumHeight = 1995
        Me.uctCLMCaseClaim1.MinimumWidth = 8250
        Me.uctCLMCaseClaim1.Name = "uctCLMCaseClaim1"
        Me.uctCLMCaseClaim1.Size = New System.Drawing.Size(550, 161)
        Me.uctCLMCaseClaim1.TabIndex = 52
        '
        '_cmdListViewSequenceUp_0
        '
        Me._cmdListViewSequenceUp_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdListViewSequenceUp_0, True)
        Me._cmdListViewSequenceUp_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdListViewSequenceUp_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdListViewSequenceUp_0, Nothing)
        Me._cmdListViewSequenceUp_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewSequenceUp_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewSequenceUp_0.Image = CType(resources.GetObject("_cmdListViewSequenceUp_0.Image"), System.Drawing.Image)
        Me._cmdListViewSequenceUp_0.Location = New System.Drawing.Point(0, 3)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdListViewSequenceUp_0, System.Drawing.Color.Silver)
        Me._cmdListViewSequenceUp_0.Name = "_cmdListViewSequenceUp_0"
        Me._cmdListViewSequenceUp_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewSequenceUp_0.Size = New System.Drawing.Size(33, 33)
        Me.commandButtonHelper1.SetStyle(Me._cmdListViewSequenceUp_0, 1)
        Me._cmdListViewSequenceUp_0.TabIndex = 49
        Me._cmdListViewSequenceUp_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdListViewSequenceUp_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewSequenceUp_0.UseVisualStyleBackColor = False
        Me._cmdListViewSequenceUp_0.Visible = False
        '
        '_cmdListViewSequenceDown_0
        '
        Me._cmdListViewSequenceDown_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdListViewSequenceDown_0, True)
        Me._cmdListViewSequenceDown_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdListViewSequenceDown_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdListViewSequenceDown_0, Nothing)
        Me._cmdListViewSequenceDown_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewSequenceDown_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewSequenceDown_0.Image = CType(resources.GetObject("_cmdListViewSequenceDown_0.Image"), System.Drawing.Image)
        Me._cmdListViewSequenceDown_0.Location = New System.Drawing.Point(3, 44)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdListViewSequenceDown_0, System.Drawing.Color.Silver)
        Me._cmdListViewSequenceDown_0.Name = "_cmdListViewSequenceDown_0"
        Me._cmdListViewSequenceDown_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewSequenceDown_0.Size = New System.Drawing.Size(33, 33)
        Me.commandButtonHelper1.SetStyle(Me._cmdListViewSequenceDown_0, 1)
        Me._cmdListViewSequenceDown_0.TabIndex = 48
        Me._cmdListViewSequenceDown_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdListViewSequenceDown_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewSequenceDown_0.UseVisualStyleBackColor = False
        Me._cmdListViewSequenceDown_0.Visible = False
        '
        'uctClaimPayment1
        '
        Me.uctClaimPayment1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uctClaimPayment1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctClaimPayment1.Location = New System.Drawing.Point(464, 235)
        Me.uctClaimPayment1.MinimumHeight = 500
        Me.uctClaimPayment1.MinimumWidth = 870
        Me.uctClaimPayment1.Name = "uctClaimPayment1"
        Me.uctClaimPayment1.Size = New System.Drawing.Size(800, 442)
        Me.uctClaimPayment1.TabIndex = 47
        '
        'uctCLMPerilDT1
        '
        Me.uctCLMPerilDT1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMPerilDT1.Location = New System.Drawing.Point(243, 298)
        Me.uctCLMPerilDT1.MinimumHeight = 2000
        Me.uctCLMPerilDT1.MinimumWidth = 2000
        Me.uctCLMPerilDT1.Name = "uctCLMPerilDT1"
        Me.uctCLMPerilDT1.Size = New System.Drawing.Size(158, 46)
        Me.uctCLMPerilDT1.TabIndex = 46
        Me.uctCLMPerilDT1.Visible = False
        '
        '_cboCombo_0
        '
        Me._cboCombo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboCombo_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboCombo_0.Location = New System.Drawing.Point(241, 354)
        Me._cboCombo_0.MousePointer = System.Windows.Forms.Cursors.Default
        Me._cboCombo_0.Name = "_cboCombo_0"
        Me._cboCombo_0.Size = New System.Drawing.Size(150, 21)
        Me._cboCombo_0.TabIndex = 43
        Me._cboCombo_0.Visible = False
        '
        'frmSizer
        '
        Me.frmSizer.AutoScroll = True
        Me.frmSizer.BackColor = System.Drawing.SystemColors.Control
        Me.frmSizer.Cursor = System.Windows.Forms.Cursors.Default
        Me.frmSizer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmSizer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmSizer.Location = New System.Drawing.Point(372, 356)
        Me.frmSizer.Name = "frmSizer"
        Me.frmSizer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmSizer.Size = New System.Drawing.Size(76, 23)
        Me.frmSizer.TabIndex = 42
        Me.frmSizer.Visible = False
        '
        'uctAssociatedClients
        '
        Me.uctAssociatedClients.AcceptsReturn = True
        Me.uctAssociatedClients.BackColor = System.Drawing.SystemColors.Window
        Me.uctAssociatedClients.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.uctAssociatedClients.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAssociatedClients.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctAssociatedClients.Location = New System.Drawing.Point(52, 222)
        Me.uctAssociatedClients.MaxLength = 0
        Me.uctAssociatedClients.Name = "uctAssociatedClients"
        Me.uctAssociatedClients.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctAssociatedClients.Size = New System.Drawing.Size(20, 21)
        Me.uctAssociatedClients.TabIndex = 41
        Me.uctAssociatedClients.Text = "Text1"
        Me.uctAssociatedClients.Visible = False
        '
        'uctClaimPeril
        '
        Me.uctClaimPeril.AcceptsReturn = True
        Me.uctClaimPeril.BackColor = System.Drawing.SystemColors.Window
        Me.uctClaimPeril.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.uctClaimPeril.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctClaimPeril.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctClaimPeril.Location = New System.Drawing.Point(13, 222)
        Me.uctClaimPeril.MaxLength = 0
        Me.uctClaimPeril.Name = "uctClaimPeril"
        Me.uctClaimPeril.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctClaimPeril.Size = New System.Drawing.Size(24, 21)
        Me.uctClaimPeril.TabIndex = 40
        Me.uctClaimPeril.Text = "Text1"
        Me.uctClaimPeril.Visible = False
        '
        '_cmdCommand_0
        '
        Me._cmdCommand_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdCommand_0, True)
        Me._cmdCommand_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdCommand_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdCommand_0, Nothing)
        Me._cmdCommand_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdCommand_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdCommand_0.Location = New System.Drawing.Point(270, 323)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdCommand_0, System.Drawing.Color.Silver)
        Me._cmdCommand_0.Name = "_cmdCommand_0"
        Me._cmdCommand_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdCommand_0.Size = New System.Drawing.Size(97, 22)
        Me.commandButtonHelper1.SetStyle(Me._cmdCommand_0, 0)
        Me._cmdCommand_0.TabIndex = 33
        Me._cmdCommand_0.Text = "Command"
        Me._cmdCommand_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdCommand_0.UseVisualStyleBackColor = False
        Me._cmdCommand_0.Visible = False
        '
        '_cmdListViewDelete_0
        '
        Me._cmdListViewDelete_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdListViewDelete_0, True)
        Me._cmdListViewDelete_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdListViewDelete_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdListViewDelete_0, Nothing)
        Me._cmdListViewDelete_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewDelete_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewDelete_0.Location = New System.Drawing.Point(162, 77)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdListViewDelete_0, System.Drawing.Color.Silver)
        Me._cmdListViewDelete_0.Name = "_cmdListViewDelete_0"
        Me._cmdListViewDelete_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewDelete_0.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me._cmdListViewDelete_0, 0)
        Me._cmdListViewDelete_0.TabIndex = 32
        Me._cmdListViewDelete_0.Text = "Delete"
        Me._cmdListViewDelete_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewDelete_0.UseVisualStyleBackColor = False
        Me._cmdListViewDelete_0.Visible = False
        '
        '_cmdListViewEdit_0
        '
        Me._cmdListViewEdit_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdListViewEdit_0, True)
        Me._cmdListViewEdit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdListViewEdit_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdListViewEdit_0, Nothing)
        Me._cmdListViewEdit_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewEdit_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewEdit_0.Location = New System.Drawing.Point(85, 76)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdListViewEdit_0, System.Drawing.Color.Silver)
        Me._cmdListViewEdit_0.Name = "_cmdListViewEdit_0"
        Me._cmdListViewEdit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewEdit_0.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me._cmdListViewEdit_0, 0)
        Me._cmdListViewEdit_0.TabIndex = 31
        Me._cmdListViewEdit_0.Text = "Edit"
        Me._cmdListViewEdit_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewEdit_0.UseVisualStyleBackColor = False
        Me._cmdListViewEdit_0.Visible = False
        '
        '_cmdListViewAdd_0
        '
        Me._cmdListViewAdd_0.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me._cmdListViewAdd_0, True)
        Me._cmdListViewAdd_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me._cmdListViewAdd_0, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me._cmdListViewAdd_0, Nothing)
        Me._cmdListViewAdd_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewAdd_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewAdd_0.Location = New System.Drawing.Point(7, 76)
        Me.commandButtonHelper1.SetMaskColor(Me._cmdListViewAdd_0, System.Drawing.Color.Silver)
        Me._cmdListViewAdd_0.Name = "_cmdListViewAdd_0"
        Me._cmdListViewAdd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewAdd_0.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me._cmdListViewAdd_0, 0)
        Me._cmdListViewAdd_0.TabIndex = 30
        Me._cmdListViewAdd_0.Text = "Add"
        Me._cmdListViewAdd_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewAdd_0.UseVisualStyleBackColor = False
        Me._cmdListViewAdd_0.Visible = False
        '
        '_fraFrame_0
        '
        Me._fraFrame_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraFrame_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFrame_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFrame_0.Location = New System.Drawing.Point(15, 187)
        Me._fraFrame_0.Name = "_fraFrame_0"
        Me._fraFrame_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFrame_0.Size = New System.Drawing.Size(225, 25)
        Me._fraFrame_0.TabIndex = 29
        Me._fraFrame_0.TabStop = False
        Me._fraFrame_0.Text = "Frame"
        Me._fraFrame_0.Visible = False
        '
        '_chkYesNo_0
        '
        Me._chkYesNo_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkYesNo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkYesNo_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkYesNo_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkYesNo_0.Location = New System.Drawing.Point(138, 137)
        Me._chkYesNo_0.Name = "_chkYesNo_0"
        Me._chkYesNo_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkYesNo_0.Size = New System.Drawing.Size(72, 13)
        Me._chkYesNo_0.TabIndex = 28
        Me._chkYesNo_0.UseVisualStyleBackColor = False
        Me._chkYesNo_0.Visible = False
        '
        '_txtText_0
        '
        Me._txtText_0.AcceptsReturn = True
        Me._txtText_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtText_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtText_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtText_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtText_0.Location = New System.Drawing.Point(7, 126)
        Me._txtText_0.MaxLength = 0
        Me._txtText_0.Multiline = True
        Me._txtText_0.Name = "_txtText_0"
        Me._txtText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtText_0.Size = New System.Drawing.Size(113, 19)
        Me._txtText_0.TabIndex = 27
        Me._txtText_0.Visible = False
        '
        'uctClaimReserve1
        '
        Me.uctClaimReserve1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctClaimReserve1.Location = New System.Drawing.Point(400, 185)
        Me.uctClaimReserve1.MinimumHeight = 2000
        Me.uctClaimReserve1.MinimumWidth = 2000
        Me.uctClaimReserve1.Name = "uctClaimReserve1"
        Me.uctClaimReserve1.Size = New System.Drawing.Size(133, 133)
        Me.uctClaimReserve1.TabIndex = 23
        Me.uctClaimReserve1.Visible = False
        '
        '_uctSumInsured1_0
        '
        Me._uctSumInsured1_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._uctSumInsured1_0.Location = New System.Drawing.Point(188, 99)
        Me._uctSumInsured1_0.MinimumHeight = 2000
        Me._uctSumInsured1_0.MinimumWidth = 5000
        Me._uctSumInsured1_0.Name = "_uctSumInsured1_0"
        Me._uctSumInsured1_0.ShowRateAndPremium = False
        Me._uctSumInsured1_0.ShowValuation = False
        Me._uctSumInsured1_0.Size = New System.Drawing.Size(333, 133)
        Me._uctSumInsured1_0.TabIndex = 24
        Me._uctSumInsured1_0.Visible = False
        '
        '_uctStandardWording1_0
        '
        Me._uctStandardWording1_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._uctStandardWording1_0.Location = New System.Drawing.Point(297, 15)
        Me._uctStandardWording1_0.MinimumHeight = 2000
        Me._uctStandardWording1_0.MinimumWidth = 2000
        Me._uctStandardWording1_0.Name = "_uctStandardWording1_0"
        Me._uctStandardWording1_0.Size = New System.Drawing.Size(253, 149)
        Me._uctStandardWording1_0.TabIndex = 25
        Me._uctStandardWording1_0.Visible = False
        '
        '_PBFindControl_0
        '
        Me._PBFindControl_0.DataArray = Nothing
        Me._PBFindControl_0.FindControlID = 0
        Me._PBFindControl_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PBFindControl_0.Location = New System.Drawing.Point(11, 150)
        Me._PBFindControl_0.Name = "_PBFindControl_0"
        Me._PBFindControl_0.ScreenControlArray = Nothing
        Me._PBFindControl_0.Size = New System.Drawing.Size(193, 37)
        Me._PBFindControl_0.TabIndex = 26
        Me._PBFindControl_0.Visible = False
        '
        '_uctAddress1_0
        '
        Me._uctAddress1_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._uctAddress1_0.Location = New System.Drawing.Point(10, 253)
        Me._uctAddress1_0.MinimumHeight = 2130
        Me._uctAddress1_0.MinimumWidth = 3000
        Me._uctAddress1_0.Name = "_uctAddress1_0"
        Me._uctAddress1_0.Size = New System.Drawing.Size(200, 142)
        Me._uctAddress1_0.TabIndex = 34
        Me._uctAddress1_0.Visible = False
        '
        'TabStrip1
        '
        Me.TabStrip1.Controls.Add(Me._0)
        Me.TabStrip1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabStrip1.Location = New System.Drawing.Point(0, 3)
        Me.TabStrip1.Name = "TabStrip1"
        Me.TabStrip1.SelectedIndex = 0
        Me.TabStrip1.Size = New System.Drawing.Size(601, 400)
        Me.TabStrip1.TabIndex = 39
        '
        '_0
        '
        Me._0.AllowDrop = True
        Me._0.Controls.Add(Me.RadioButton1)
        Me._0.Location = New System.Drawing.Point(4, 22)
        Me._0.Name = "_0"
        Me._0.Size = New System.Drawing.Size(593, 374)
        Me._0.TabIndex = 0
        Me._0.Tag = ""
        Me._0.Text = "1 - General"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(53, 82)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(101, 17)
        Me.RadioButton1.TabIndex = 22
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "RadioButton1"
        Me.RadioButton1.UseVisualStyleBackColor = True
        Me.RadioButton1.Visible = False
        '
        '_lblDateLabel_0
        '
        Me._lblDateLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblDateLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblDateLabel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblDateLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblDateLabel_0.Location = New System.Drawing.Point(459, 247)
        Me._lblDateLabel_0.Name = "_lblDateLabel_0"
        Me._lblDateLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblDateLabel_0.Size = New System.Drawing.Size(122, 19)
        Me._lblDateLabel_0.TabIndex = 44
        Me._lblDateLabel_0.Text = "Label1"
        '
        '_pnlPanel_0
        '
        Me._pnlPanel_0.BackColor = System.Drawing.SystemColors.Control
        Me._pnlPanel_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPanel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._pnlPanel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPanel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._pnlPanel_0.Location = New System.Drawing.Point(374, 323)
        Me._pnlPanel_0.Name = "_pnlPanel_0"
        Me._pnlPanel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._pnlPanel_0.Size = New System.Drawing.Size(153, 22)
        Me._pnlPanel_0.TabIndex = 35
        Me._pnlPanel_0.Text = "Panel"
        Me._pnlPanel_0.Visible = False
        '
        '_lblTextLabel_0
        '
        Me._lblTextLabel_0.AutoSize = True
        Me._lblTextLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTextLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTextLabel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTextLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTextLabel_0.Location = New System.Drawing.Point(252, 300)
        Me._lblTextLabel_0.Name = "_lblTextLabel_0"
        Me._lblTextLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTextLabel_0.Size = New System.Drawing.Size(66, 13)
        Me._lblTextLabel_0.TabIndex = 21
        Me._lblTextLabel_0.Text = "Text Label"
        Me._lblTextLabel_0.Visible = False
        '
        '_lblCheckLabel_0
        '
        Me._lblCheckLabel_0.AutoSize = True
        Me._lblCheckLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCheckLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCheckLabel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCheckLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCheckLabel_0.Location = New System.Drawing.Point(301, 294)
        Me._lblCheckLabel_0.Name = "_lblCheckLabel_0"
        Me._lblCheckLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCheckLabel_0.Size = New System.Drawing.Size(77, 13)
        Me._lblCheckLabel_0.TabIndex = 38
        Me._lblCheckLabel_0.Text = "Check Label"
        Me._lblCheckLabel_0.Visible = False
        '
        '_lblComboLabel_0
        '
        Me._lblComboLabel_0.AutoSize = True
        Me._lblComboLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblComboLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblComboLabel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblComboLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblComboLabel_0.Location = New System.Drawing.Point(214, 284)
        Me._lblComboLabel_0.Name = "_lblComboLabel_0"
        Me._lblComboLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblComboLabel_0.Size = New System.Drawing.Size(82, 13)
        Me._lblComboLabel_0.TabIndex = 37
        Me._lblComboLabel_0.Text = "Combo Label"
        Me._lblComboLabel_0.Visible = False
        '
        'tvwDataDictionary
        '
        Me.tvwDataDictionary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwDataDictionary.HideSelection = False
        Me.tvwDataDictionary.ImageIndex = 2
        Me.tvwDataDictionary.ImageList = Me.ImageList1
        Me.tvwDataDictionary.Indent = 20
        Me.tvwDataDictionary.Location = New System.Drawing.Point(24, 110)
        Me.tvwDataDictionary.Name = "tvwDataDictionary"
        Me.tvwDataDictionary.SelectedImageIndex = 2
        Me.tvwDataDictionary.Size = New System.Drawing.Size(233, 433)
        Me.tvwDataDictionary.TabIndex = 13
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Closed")
        Me.ImageList1.Images.SetKeyName(1, "Open")
        Me.ImageList1.Images.SetKeyName(2, "none.gif")
        '
        'txtScreenCode
        '
        Me.txtScreenCode.AcceptsReturn = True
        Me.txtScreenCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtScreenCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScreenCode.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScreenCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScreenCode.Location = New System.Drawing.Point(128, 4)
        Me.txtScreenCode.MaxLength = 10
        Me.txtScreenCode.Name = "txtScreenCode"
        Me.txtScreenCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScreenCode.Size = New System.Drawing.Size(129, 20)
        Me.txtScreenCode.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(384, 4)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(209, 20)
        Me.txtDescription.TabIndex = 1
        '
        'Ctx_mnuControl
        '
        Me.Ctx_mnuControl.Name = "Ctx_mnuControl"
        Me.Ctx_mnuControl.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuFrame
        '
        Me.Ctx_mnuFrame.Name = "Ctx_mnuFrame"
        Me.Ctx_mnuFrame.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuListView
        '
        Me.Ctx_mnuListView.Name = "Ctx_mnuListView"
        Me.Ctx_mnuListView.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuTab
        '
        Me.Ctx_mnuTab.Name = "Ctx_mnuTab"
        Me.Ctx_mnuTab.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuTreeView
        '
        Me.Ctx_mnuTreeView.Name = "Ctx_mnuTreeView"
        Me.Ctx_mnuTreeView.Size = New System.Drawing.Size(61, 4)
        '
        'frmInterface
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(926, 661)
        Me.Controls.Add(Me.cmdDynamicLogic)
        Me.Controls.Add(Me.cmdCopy)
        Me.Controls.Add(Me.cmdDefaults)
        Me.Controls.Add(Me.cmdValidation)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maintain Screen Data"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.grpCompiledRuleAssembly.ResumeLayout(False)
        Me.grpCompiledRuleAssembly.PerformLayout()
        CType(Me.picScreenMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picScreenMain.ResumeLayout(False)
        CType(Me.picScreen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picScreen.ResumeLayout(False)
        Me.picScreen.PerformLayout()
        Me.TabStrip1.ResumeLayout(False)
        Me._0.ResumeLayout(False)
        Me._0.PerformLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeuctSumInsured1()
        Me.uctSumInsured1(0) = _uctSumInsured1_0
    End Sub
    Sub InitializeuctStandardWording1()
        Me.uctStandardWording1(0) = _uctStandardWording1_0
    End Sub
    Sub InitializeuctAddress1()
        Me.uctAddress1(0) = _uctAddress1_0
    End Sub
    Sub InitializetxtText()
        Me.txtText(0) = _txtText_0
    End Sub
    Sub InitializepnlPanel()
        Me.pnlPanel(0) = _pnlPanel_0
    End Sub
    Sub InitializemnuFormat()
        Me.mnuFormat(0) = _mnuFormat_0
        Me.mnuFormat(1) = _mnuFormat_1
        Me.mnuFormat(2) = _mnuFormat_2
        Me.mnuFormat(3) = _mnuFormat_3
        Me.mnuFormat(4) = _mnuFormat_4
        Me.mnuFormat(5) = _mnuFormat_5
        Me.mnuFormat(6) = _mnuFormat_6
        Me.mnuFormat(7) = _mnuFormat_7
        Me.mnuFormat(8) = _mnuFormat_8
        Me.mnuFormat(9) = _mnuFormat_9
        Me.mnuFormat(10) = _mnuFormat_10
        Me.mnuFormat(11) = _mnuFormat_11
        Me.mnuFormat(12) = _mnuFormat_12
        Me.mnuFormat(13) = _mnuFormat_13
        Me.mnuFormat(14) = _mnuFormat_14
        Me.mnuFormat(15) = _mnuFormat_15
        Me.mnuFormat(16) = _mnuFormat_16
        Me.mnuFormat(17) = _mnuFormat_17
        Me.mnuFormat(18) = _mnuFormat_18
        Me.mnuFormat(19) = _mnuFormat_19
        Me.mnuFormat(20) = _mnuFormat_20
        Me.mnuFormat(21) = _mnuFormat_21
        Me.mnuFormat(22) = _mnuFormat_22
        Me.mnuFormat(23) = _mnuFormat_23
        Me.mnuFormat(24) = _mnuFormat_24
        Me.mnuFormat(25) = _mnuFormat_25
        Me.mnuFormat(26) = _mnuFormat_26
        Me.mnuFormat(27) = _mnuFormat_27
        Me.mnuFormat(28) = _mnuFormat_28
        Me.mnuFormat(29) = _mnuFormat_29
    End Sub
    Sub InitializelvwListView()
        Me.lvwListView(0) = _lvwListView_0
    End Sub
    Sub InitializelblTextLabel()
        Me.lblTextLabel(0) = _lblTextLabel_0
    End Sub
    Sub InitializelblDateLabel()
        Me.lblDateLabel(0) = _lblDateLabel_0
    End Sub
    Sub InitializelblComboLabel()
        Me.lblComboLabel(0) = _lblComboLabel_0
    End Sub
    Sub InitializelblCheckLabel()
        Me.lblCheckLabel(0) = _lblCheckLabel_0
    End Sub
    Sub InitializefraFrame()
        Me.fraFrame(0) = _fraFrame_0
    End Sub
    Sub InitializecmdListViewSequenceUp()
        Me.cmdListViewSequenceUp(0) = _cmdListViewSequenceUp_0
    End Sub
    Sub InitializecmdListViewSequenceDown()
        Me.cmdListViewSequenceDown(0) = _cmdListViewSequenceDown_0
    End Sub
    Sub InitializecmdListViewEdit()
        Me.cmdListViewEdit(0) = _cmdListViewEdit_0
    End Sub
    Sub InitializecmdListViewDelete()
        Me.cmdListViewDelete(0) = _cmdListViewDelete_0
    End Sub
    Sub InitializecmdListViewAdd()
        Me.cmdListViewAdd(0) = _cmdListViewAdd_0
    End Sub
    Sub InitializecmdCommand()
        Me.cmdCommand(0) = _cmdCommand_0
    End Sub
    Sub InitializechkYesNo()
        Me.chkYesNo(0) = _chkYesNo_0
    End Sub
    Sub InitializecboCombo()
        Me.cboCombo(0) = _cboCombo_0
    End Sub
    Sub InitializePBFindControl()
        Me.PBFindControl(0) = _PBFindControl_0
    End Sub
    Sub lvwListView_0_InitializeColumnKeys()
        Me._lvwListView_0_ColumnHeader_1.Name = ""
    End Sub
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents chkEnableCompileRule As System.Windows.Forms.CheckBox
    Friend WithEvents grpCompiledRuleAssembly As System.Windows.Forms.GroupBox
    Friend WithEvents UctCompiledRuleValidation As uctCompiledRule.uctCompiledRule
    Friend WithEvents UctCompiledRuleDefaults As uctCompiledRule.uctCompiledRule
    Friend WithEvents lblValidation As System.Windows.Forms.Label
    Friend WithEvents lblDefaults As System.Windows.Forms.Label
#End Region
End Class
