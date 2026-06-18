<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializeoptRiskType()
        InitializemnuInsert()
        InitializemnuClause()
        lvwSubDocuments_InitializeColumnKeys()
        lvwClauses_InitializeColumnKeys()
    End Sub
    Private Sub Ctx_mnuClauses_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuClauses.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuClauses.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuClauses.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuClauses.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuClauses_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuClauses.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuClauses.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuClauses.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub Ctx_mnuField_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuField.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuField.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuField.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuField.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuField_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuField.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuField.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuField.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub Ctx_mnuSubDocuments_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSubDocuments.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuSubDocuments.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuSubDocuments.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuSubDocuments.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuSubDocuments_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSubDocuments.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSubDocuments.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuSubDocuments.DropDownItems.Add(item)
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
    Private WithEvents _mnuClause_0 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuClause_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuClause_2 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuClause_3 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuClauses As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuInsert_0 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuInsert_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuInsert_2 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuField As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSubDocumentInsert As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSubDocuments As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents imgFields As System.Windows.Forms.ImageList
    Public WithEvents tvwFields As System.Windows.Forms.TreeView
    Private WithEvents _tabFields_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents chkStandardWordingPageBreak As System.Windows.Forms.CheckBox
    Public WithEvents cmdInsertStandardWordings As System.Windows.Forms.Button
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents cmdInsertHeaders As System.Windows.Forms.Button
    Public WithEvents chkIncludeHeaders As System.Windows.Forms.CheckBox
    Public WithEvents txtRiskLoopDoc As System.Windows.Forms.TextBox
    Public WithEvents cmdInsertRiskLoop As System.Windows.Forms.Button
    Private WithEvents _optRiskType_3 As System.Windows.Forms.RadioButton
    Private WithEvents _optRiskType_2 As System.Windows.Forms.RadioButton
    Private WithEvents _optRiskType_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optRiskType_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents fraRiskLoop As System.Windows.Forms.GroupBox
    Public WithEvents chkMandatoryQuestion As System.Windows.Forms.CheckBox
    Public WithEvents cmdInsertQuestion As System.Windows.Forms.Button
    Public WithEvents txtQuestion As System.Windows.Forms.TextBox
    Public WithEvents fraQuestion As System.Windows.Forms.GroupBox
    Public WithEvents Command1 As System.Windows.Forms.Button
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Private WithEvents _tabFields_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents txtFileName As System.Windows.Forms.TextBox
    Public WithEvents cmdBrowse As System.Windows.Forms.Button
    Public WithEvents cmdInsertFile As System.Windows.Forms.Button
    Public dlgFileOpen As System.Windows.Forms.OpenFileDialog
    Public WithEvents lblFileName As System.Windows.Forms.Label
    Private WithEvents _tabFields_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _lvwClauses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwClauses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwClauses As System.Windows.Forms.ListView
    Private WithEvents _tabFields_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _lvwSubDocuments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSubDocuments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSubDocuments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSubDocuments As System.Windows.Forms.ListView
    Private WithEvents _tabFields_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents tabFields As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public mnuClause(3) As System.Windows.Forms.ToolStripMenuItem
    Public mnuInsert(2) As System.Windows.Forms.ToolStripMenuItem
    Public optRiskType(3) As System.Windows.Forms.RadioButton
    Public WithEvents Ctx_mnuClauses As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuField As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuSubDocuments As System.Windows.Forms.ContextMenuStrip
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuClauses = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuClause_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuClause_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuClause_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuClause_3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuField = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuInsert_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuInsert_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuInsert_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSubDocuments = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSubDocumentInsert = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.tabFields = New System.Windows.Forms.TabControl()
        Me._tabFields_TabPage0 = New System.Windows.Forms.TabPage()
        Me.tvwFields = New System.Windows.Forms.TreeView()
        Me.imgFields = New System.Windows.Forms.ImageList(Me.components)
        Me._tabFields_TabPage1 = New System.Windows.Forms.TabPage()
        Me.frmDocumentBr = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDocumentSplit = New System.Windows.Forms.TextBox()
        Me.cmdInsertDocBreak = New System.Windows.Forms.Button()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me.optStandardWordingDescription = New System.Windows.Forms.RadioButton()
        Me.optStandardWordingCodes = New System.Windows.Forms.RadioButton()
        Me.optStandardWordingText = New System.Windows.Forms.RadioButton()
        Me.chkStandardWordingPageBreak = New System.Windows.Forms.CheckBox()
        Me.cmdInsertStandardWordings = New System.Windows.Forms.Button()
        Me.fraRiskLoop = New System.Windows.Forms.GroupBox()
        Me.cmdInsertHeaders = New System.Windows.Forms.Button()
        Me.chkIncludeHeaders = New System.Windows.Forms.CheckBox()
        Me.txtRiskLoopDoc = New System.Windows.Forms.TextBox()
        Me.cmdInsertRiskLoop = New System.Windows.Forms.Button()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me._optRiskType_3 = New System.Windows.Forms.RadioButton()
        Me._optRiskType_2 = New System.Windows.Forms.RadioButton()
        Me._optRiskType_1 = New System.Windows.Forms.RadioButton()
        Me._optRiskType_0 = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraQuestion = New System.Windows.Forms.GroupBox()
        Me.chkMandatoryQuestion = New System.Windows.Forms.CheckBox()
        Me.cmdInsertQuestion = New System.Windows.Forms.Button()
        Me.txtQuestion = New System.Windows.Forms.TextBox()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.Command1 = New System.Windows.Forms.Button()
        Me._tabFields_TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.cmdInsertFile = New System.Windows.Forms.Button()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me._tabFields_TabPage3 = New System.Windows.Forms.TabPage()
        Me.lvwClauses = New System.Windows.Forms.ListView()
        Me._lvwClauses_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwClauses_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabFields_TabPage4 = New System.Windows.Forms.TabPage()
        Me.lvwSubDocuments = New System.Windows.Forms.ListView()
        Me._lvwSubDocuments_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSubDocuments_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSubDocuments_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.dlgFileOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.Ctx_mnuClauses = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuField = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuSubDocuments = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.tabFields.SuspendLayout()
        Me._tabFields_TabPage0.SuspendLayout()
        Me._tabFields_TabPage1.SuspendLayout()
        Me.frmDocumentBr.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.fraRiskLoop.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraQuestion.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._tabFields_TabPage2.SuspendLayout()
        Me._tabFields_TabPage3.SuspendLayout()
        Me._tabFields_TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClauses, Me.mnuField, Me.mnuSubDocuments})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(268, 24)
        Me.MainMenu1.TabIndex = 3
        Me.MainMenu1.Visible = False
        '
        'mnuClauses
        '
        Me.mnuClauses.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuClause_0, Me._mnuClause_1, Me._mnuClause_2, Me._mnuClause_3})
        Me.mnuClauses.Name = "mnuClauses"
        Me.mnuClauses.Size = New System.Drawing.Size(54, 20)
        Me.mnuClauses.Text = "Clause"
        '
        '_mnuClause_0
        '
        Me._mnuClause_0.Name = "_mnuClause_0"
        Me._mnuClause_0.Size = New System.Drawing.Size(251, 22)
        Me._mnuClause_0.Text = "Insert Clause Text"
        '
        '_mnuClause_1
        '
        Me._mnuClause_1.Name = "_mnuClause_1"
        Me._mnuClause_1.Size = New System.Drawing.Size(251, 22)
        Me._mnuClause_1.Text = "Insert Clause Code"
        '
        '_mnuClause_2
        '
        Me._mnuClause_2.Name = "_mnuClause_2"
        Me._mnuClause_2.Size = New System.Drawing.Size(251, 22)
        Me._mnuClause_2.Text = "Insert Clause Description"
        '
        '_mnuClause_3
        '
        Me._mnuClause_3.Name = "_mnuClause_3"
        Me._mnuClause_3.Size = New System.Drawing.Size(251, 22)
        Me._mnuClause_3.Text = "Insert Selective Standard Wording"
        '
        'mnuField
        '
        Me.mnuField.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuInsert_0, Me._mnuInsert_1, Me._mnuInsert_2})
        Me.mnuField.Name = "mnuField"
        Me.mnuField.Size = New System.Drawing.Size(44, 20)
        Me.mnuField.Text = "Field"
        '
        '_mnuInsert_0
        '
        Me._mnuInsert_0.Name = "_mnuInsert_0"
        Me._mnuInsert_0.Size = New System.Drawing.Size(184, 22)
        Me._mnuInsert_0.Text = "Insert"
        '
        '_mnuInsert_1
        '
        Me._mnuInsert_1.Name = "_mnuInsert_1"
        Me._mnuInsert_1.Size = New System.Drawing.Size(184, 22)
        Me._mnuInsert_1.Text = "Insert with CR after"
        '
        '_mnuInsert_2
        '
        Me._mnuInsert_2.Name = "_mnuInsert_2"
        Me._mnuInsert_2.Size = New System.Drawing.Size(184, 22)
        Me._mnuInsert_2.Text = "Insert with CR before"
        '
        'mnuSubDocuments
        '
        Me.mnuSubDocuments.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSubDocumentInsert})
        Me.mnuSubDocuments.Name = "mnuSubDocuments"
        Me.mnuSubDocuments.Size = New System.Drawing.Size(103, 20)
        Me.mnuSubDocuments.Text = "Sub Documents"
        '
        'mnuSubDocumentInsert
        '
        Me.mnuSubDocumentInsert.Name = "mnuSubDocumentInsert"
        Me.mnuSubDocumentInsert.Size = New System.Drawing.Size(103, 22)
        Me.mnuSubDocumentInsert.Text = "Insert"
        '
        'Timer1
        '
        Me.Timer1.Interval = 200
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(185, 520)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(81, 25)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Hi&de"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(98, 520)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(81, 25)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabFields
        '
        Me.tabFields.Controls.Add(Me._tabFields_TabPage0)
        Me.tabFields.Controls.Add(Me._tabFields_TabPage1)
        Me.tabFields.Controls.Add(Me._tabFields_TabPage2)
        Me.tabFields.Controls.Add(Me._tabFields_TabPage3)
        Me.tabFields.Controls.Add(Me._tabFields_TabPage4)
        Me.tabFields.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.tabFields.ItemSize = New System.Drawing.Size(51, 18)
        Me.tabFields.Location = New System.Drawing.Point(-3, 3)
        Me.tabFields.Name = "tabFields"
        Me.tabFields.SelectedIndex = 0
        Me.tabFields.Size = New System.Drawing.Size(267, 513)
        Me.tabFields.TabIndex = 0
        '
        '_tabFields_TabPage0
        '
        Me._tabFields_TabPage0.Controls.Add(Me.tvwFields)
        Me._tabFields_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabFields_TabPage0.Name = "_tabFields_TabPage0"
        Me._tabFields_TabPage0.Size = New System.Drawing.Size(259, 487)
        Me._tabFields_TabPage0.TabIndex = 0
        Me._tabFields_TabPage0.Text = "1 Field"
        '
        'tvwFields
        '
        Me.tvwFields.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwFields.HideSelection = False
        Me.tvwFields.ImageIndex = 0
        Me.tvwFields.ImageList = Me.imgFields
        Me.tvwFields.Indent = 24
        Me.tvwFields.Location = New System.Drawing.Point(6, 13)
        Me.tvwFields.Name = "tvwFields"
        Me.tvwFields.SelectedImageIndex = 0
        Me.tvwFields.Size = New System.Drawing.Size(247, 282)
        Me.tvwFields.TabIndex = 3
        '
        'imgFields
        '
        Me.imgFields.ImageStream = CType(resources.GetObject("imgFields.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgFields.TransparentColor = System.Drawing.Color.Transparent
        Me.imgFields.Images.SetKeyName(0, "closed")
        Me.imgFields.Images.SetKeyName(1, "open")
        Me.imgFields.Images.SetKeyName(2, "leaf")
        Me.imgFields.Images.SetKeyName(3, "scheme")
        '
        '_tabFields_TabPage1
        '
        Me._tabFields_TabPage1.Controls.Add(Me.frmDocumentBr)
        Me._tabFields_TabPage1.Controls.Add(Me.Frame3)
        Me._tabFields_TabPage1.Controls.Add(Me.fraRiskLoop)
        Me._tabFields_TabPage1.Controls.Add(Me.fraQuestion)
        Me._tabFields_TabPage1.Controls.Add(Me.Frame2)
        Me._tabFields_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabFields_TabPage1.Name = "_tabFields_TabPage1"
        Me._tabFields_TabPage1.Size = New System.Drawing.Size(259, 487)
        Me._tabFields_TabPage1.TabIndex = 1
        Me._tabFields_TabPage1.Text = "2 Special"
        '
        'frmDocumentBr
        '
        Me.frmDocumentBr.BackColor = System.Drawing.SystemColors.Control
        Me.frmDocumentBr.Controls.Add(Me.Label2)
        Me.frmDocumentBr.Controls.Add(Me.txtDocumentSplit)
        Me.frmDocumentBr.Controls.Add(Me.cmdInsertDocBreak)
        Me.frmDocumentBr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmDocumentBr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmDocumentBr.Location = New System.Drawing.Point(8, 344)
        Me.frmDocumentBr.Name = "frmDocumentBr"
        Me.frmDocumentBr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmDocumentBr.Size = New System.Drawing.Size(250, 98)
        Me.frmDocumentBr.TabIndex = 25
        Me.frmDocumentBr.TabStop = False
        Me.frmDocumentBr.Text = "Document Break:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(7, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(92, 19)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Document:"
        '
        'txtDocumentSplit
        '
        Me.txtDocumentSplit.AcceptsReturn = True
        Me.txtDocumentSplit.BackColor = System.Drawing.SystemColors.Window
        Me.txtDocumentSplit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDocumentSplit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentSplit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDocumentSplit.Location = New System.Drawing.Point(6, 47)
        Me.txtDocumentSplit.MaxLength = 0
        Me.txtDocumentSplit.Name = "txtDocumentSplit"
        Me.txtDocumentSplit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDocumentSplit.Size = New System.Drawing.Size(238, 20)
        Me.txtDocumentSplit.TabIndex = 25
        '
        'cmdInsertDocBreak
        '
        Me.cmdInsertDocBreak.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertDocBreak.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertDocBreak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertDocBreak.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertDocBreak.Location = New System.Drawing.Point(161, 69)
        Me.cmdInsertDocBreak.Name = "cmdInsertDocBreak"
        Me.cmdInsertDocBreak.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertDocBreak.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertDocBreak.TabIndex = 24
        Me.cmdInsertDocBreak.Text = "Insert"
        Me.cmdInsertDocBreak.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertDocBreak.UseVisualStyleBackColor = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.optStandardWordingDescription)
        Me.Frame3.Controls.Add(Me.optStandardWordingCodes)
        Me.Frame3.Controls.Add(Me.optStandardWordingText)
        Me.Frame3.Controls.Add(Me.chkStandardWordingPageBreak)
        Me.Frame3.Controls.Add(Me.cmdInsertStandardWordings)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(6, 259)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(250, 81)
        Me.Frame3.TabIndex = 25
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Standard Wordings:"
        '
        'optStandardWordingDescription
        '
        Me.optStandardWordingDescription.BackColor = System.Drawing.SystemColors.Control
        Me.optStandardWordingDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.optStandardWordingDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optStandardWordingDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStandardWordingDescription.Location = New System.Drawing.Point(152, 19)
        Me.optStandardWordingDescription.Name = "optStandardWordingDescription"
        Me.optStandardWordingDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optStandardWordingDescription.Size = New System.Drawing.Size(89, 22)
        Me.optStandardWordingDescription.TabIndex = 31
        Me.optStandardWordingDescription.TabStop = True
        Me.optStandardWordingDescription.Text = "Description"
        Me.optStandardWordingDescription.UseVisualStyleBackColor = False
        '
        'optStandardWordingCodes
        '
        Me.optStandardWordingCodes.BackColor = System.Drawing.SystemColors.Control
        Me.optStandardWordingCodes.Cursor = System.Windows.Forms.Cursors.Default
        Me.optStandardWordingCodes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optStandardWordingCodes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStandardWordingCodes.Location = New System.Drawing.Point(83, 19)
        Me.optStandardWordingCodes.Name = "optStandardWordingCodes"
        Me.optStandardWordingCodes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optStandardWordingCodes.Size = New System.Drawing.Size(89, 22)
        Me.optStandardWordingCodes.TabIndex = 30
        Me.optStandardWordingCodes.TabStop = True
        Me.optStandardWordingCodes.Text = "Codes"
        Me.optStandardWordingCodes.UseVisualStyleBackColor = False
        '
        'optStandardWordingText
        '
        Me.optStandardWordingText.BackColor = System.Drawing.SystemColors.Control
        Me.optStandardWordingText.Cursor = System.Windows.Forms.Cursors.Default
        Me.optStandardWordingText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optStandardWordingText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStandardWordingText.Location = New System.Drawing.Point(8, 20)
        Me.optStandardWordingText.Name = "optStandardWordingText"
        Me.optStandardWordingText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optStandardWordingText.Size = New System.Drawing.Size(89, 22)
        Me.optStandardWordingText.TabIndex = 29
        Me.optStandardWordingText.TabStop = True
        Me.optStandardWordingText.Text = "Text"
        Me.optStandardWordingText.UseVisualStyleBackColor = False
        '
        'chkStandardWordingPageBreak
        '
        Me.chkStandardWordingPageBreak.BackColor = System.Drawing.SystemColors.Control
        Me.chkStandardWordingPageBreak.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkStandardWordingPageBreak.Enabled = False
        Me.chkStandardWordingPageBreak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStandardWordingPageBreak.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStandardWordingPageBreak.Location = New System.Drawing.Point(8, 48)
        Me.chkStandardWordingPageBreak.Name = "chkStandardWordingPageBreak"
        Me.chkStandardWordingPageBreak.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkStandardWordingPageBreak.Size = New System.Drawing.Size(89, 19)
        Me.chkStandardWordingPageBreak.TabIndex = 28
        Me.chkStandardWordingPageBreak.Text = "Page Break"
        Me.chkStandardWordingPageBreak.UseVisualStyleBackColor = False
        '
        'cmdInsertStandardWordings
        '
        Me.cmdInsertStandardWordings.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertStandardWordings.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertStandardWordings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertStandardWordings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertStandardWordings.Location = New System.Drawing.Point(162, 44)
        Me.cmdInsertStandardWordings.Name = "cmdInsertStandardWordings"
        Me.cmdInsertStandardWordings.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertStandardWordings.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertStandardWordings.TabIndex = 26
        Me.cmdInsertStandardWordings.Text = "Insert"
        Me.cmdInsertStandardWordings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertStandardWordings.UseVisualStyleBackColor = False
        '
        'fraRiskLoop
        '
        Me.fraRiskLoop.BackColor = System.Drawing.SystemColors.Control
        Me.fraRiskLoop.Controls.Add(Me.cmdInsertHeaders)
        Me.fraRiskLoop.Controls.Add(Me.chkIncludeHeaders)
        Me.fraRiskLoop.Controls.Add(Me.txtRiskLoopDoc)
        Me.fraRiskLoop.Controls.Add(Me.cmdInsertRiskLoop)
        Me.fraRiskLoop.Controls.Add(Me.Frame1)
        Me.fraRiskLoop.Controls.Add(Me.Label1)
        Me.fraRiskLoop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRiskLoop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRiskLoop.Location = New System.Drawing.Point(6, 82)
        Me.fraRiskLoop.Name = "fraRiskLoop"
        Me.fraRiskLoop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRiskLoop.Size = New System.Drawing.Size(250, 175)
        Me.fraRiskLoop.TabIndex = 12
        Me.fraRiskLoop.TabStop = False
        Me.fraRiskLoop.Text = "Risk Loop:"
        '
        'cmdInsertHeaders
        '
        Me.cmdInsertHeaders.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertHeaders.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertHeaders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertHeaders.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertHeaders.Location = New System.Drawing.Point(162, 39)
        Me.cmdInsertHeaders.Name = "cmdInsertHeaders"
        Me.cmdInsertHeaders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertHeaders.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertHeaders.TabIndex = 17
        Me.cmdInsertHeaders.Text = "Insert"
        Me.cmdInsertHeaders.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertHeaders.UseVisualStyleBackColor = False
        '
        'chkIncludeHeaders
        '
        Me.chkIncludeHeaders.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeHeaders.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIncludeHeaders.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeHeaders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeHeaders.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeHeaders.Location = New System.Drawing.Point(6, 18)
        Me.chkIncludeHeaders.Name = "chkIncludeHeaders"
        Me.chkIncludeHeaders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeHeaders.Size = New System.Drawing.Size(235, 16)
        Me.chkIncludeHeaders.TabIndex = 16
        Me.chkIncludeHeaders.Text = "Include Headers/Trailers"
        Me.chkIncludeHeaders.UseVisualStyleBackColor = False
        '
        'txtRiskLoopDoc
        '
        Me.txtRiskLoopDoc.AcceptsReturn = True
        Me.txtRiskLoopDoc.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskLoopDoc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskLoopDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskLoopDoc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskLoopDoc.Location = New System.Drawing.Point(6, 72)
        Me.txtRiskLoopDoc.MaxLength = 0
        Me.txtRiskLoopDoc.Name = "txtRiskLoopDoc"
        Me.txtRiskLoopDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskLoopDoc.Size = New System.Drawing.Size(238, 20)
        Me.txtRiskLoopDoc.TabIndex = 14
        '
        'cmdInsertRiskLoop
        '
        Me.cmdInsertRiskLoop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertRiskLoop.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertRiskLoop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertRiskLoop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertRiskLoop.Location = New System.Drawing.Point(162, 99)
        Me.cmdInsertRiskLoop.Name = "cmdInsertRiskLoop"
        Me.cmdInsertRiskLoop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertRiskLoop.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertRiskLoop.TabIndex = 13
        Me.cmdInsertRiskLoop.Text = "Insert"
        Me.cmdInsertRiskLoop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertRiskLoop.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optRiskType_3)
        Me.Frame1.Controls.Add(Me._optRiskType_2)
        Me.Frame1.Controls.Add(Me._optRiskType_1)
        Me.Frame1.Controls.Add(Me._optRiskType_0)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(6, 129)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(238, 40)
        Me.Frame1.TabIndex = 18
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Risk Type:"
        '
        '_optRiskType_3
        '
        Me._optRiskType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optRiskType_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRiskType_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRiskType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRiskType_3.Location = New System.Drawing.Point(171, 16)
        Me._optRiskType_3.Name = "_optRiskType_3"
        Me._optRiskType_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRiskType_3.Size = New System.Drawing.Size(64, 19)
        Me._optRiskType_3.TabIndex = 22
        Me._optRiskType_3.TabStop = True
        Me._optRiskType_3.Text = "Deleted"
        Me._optRiskType_3.UseVisualStyleBackColor = False
        '
        '_optRiskType_2
        '
        Me._optRiskType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optRiskType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRiskType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRiskType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRiskType_2.Location = New System.Drawing.Point(120, 16)
        Me._optRiskType_2.Name = "_optRiskType_2"
        Me._optRiskType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRiskType_2.Size = New System.Drawing.Size(46, 19)
        Me._optRiskType_2.TabIndex = 21
        Me._optRiskType_2.TabStop = True
        Me._optRiskType_2.Text = "New"
        Me._optRiskType_2.UseVisualStyleBackColor = False
        '
        '_optRiskType_1
        '
        Me._optRiskType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optRiskType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRiskType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRiskType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRiskType_1.Location = New System.Drawing.Point(51, 15)
        Me._optRiskType_1.Name = "_optRiskType_1"
        Me._optRiskType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRiskType_1.Size = New System.Drawing.Size(73, 21)
        Me._optRiskType_1.TabIndex = 20
        Me._optRiskType_1.TabStop = True
        Me._optRiskType_1.Text = "Changed"
        Me._optRiskType_1.UseVisualStyleBackColor = False
        '
        '_optRiskType_0
        '
        Me._optRiskType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optRiskType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRiskType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRiskType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRiskType_0.Location = New System.Drawing.Point(6, 15)
        Me._optRiskType_0.Name = "_optRiskType_0"
        Me._optRiskType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRiskType_0.Size = New System.Drawing.Size(37, 22)
        Me._optRiskType_0.TabIndex = 19
        Me._optRiskType_0.TabStop = True
        Me._optRiskType_0.Text = "All"
        Me._optRiskType_0.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(7, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(91, 19)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Document:"
        '
        'fraQuestion
        '
        Me.fraQuestion.BackColor = System.Drawing.SystemColors.Control
        Me.fraQuestion.Controls.Add(Me.chkMandatoryQuestion)
        Me.fraQuestion.Controls.Add(Me.cmdInsertQuestion)
        Me.fraQuestion.Controls.Add(Me.txtQuestion)
        Me.fraQuestion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraQuestion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraQuestion.Location = New System.Drawing.Point(6, 4)
        Me.fraQuestion.Name = "fraQuestion"
        Me.fraQuestion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraQuestion.Size = New System.Drawing.Size(250, 76)
        Me.fraQuestion.TabIndex = 9
        Me.fraQuestion.TabStop = False
        Me.fraQuestion.Text = "Question:"
        '
        'chkMandatoryQuestion
        '
        Me.chkMandatoryQuestion.BackColor = System.Drawing.SystemColors.Control
        Me.chkMandatoryQuestion.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMandatoryQuestion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMandatoryQuestion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMandatoryQuestion.Location = New System.Drawing.Point(8, 48)
        Me.chkMandatoryQuestion.Name = "chkMandatoryQuestion"
        Me.chkMandatoryQuestion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMandatoryQuestion.Size = New System.Drawing.Size(82, 22)
        Me.chkMandatoryQuestion.TabIndex = 27
        Me.chkMandatoryQuestion.Text = "Mandatory"
        Me.chkMandatoryQuestion.UseVisualStyleBackColor = False
        '
        'cmdInsertQuestion
        '
        Me.cmdInsertQuestion.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertQuestion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertQuestion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertQuestion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertQuestion.Location = New System.Drawing.Point(162, 45)
        Me.cmdInsertQuestion.Name = "cmdInsertQuestion"
        Me.cmdInsertQuestion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertQuestion.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertQuestion.TabIndex = 11
        Me.cmdInsertQuestion.Text = "Insert"
        Me.cmdInsertQuestion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertQuestion.UseVisualStyleBackColor = False
        '
        'txtQuestion
        '
        Me.txtQuestion.AcceptsReturn = True
        Me.txtQuestion.BackColor = System.Drawing.SystemColors.Window
        Me.txtQuestion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtQuestion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuestion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtQuestion.Location = New System.Drawing.Point(6, 18)
        Me.txtQuestion.MaxLength = 0
        Me.txtQuestion.Name = "txtQuestion"
        Me.txtQuestion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtQuestion.Size = New System.Drawing.Size(238, 20)
        Me.txtQuestion.TabIndex = 10
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.Command1)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 446)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(250, 43)
        Me.Frame2.TabIndex = 23
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Condition:"
        Me.Frame2.Visible = False
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(162, 12)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(81, 25)
        Me.Command1.TabIndex = 24
        Me.Command1.Text = "Insert"
        Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command1.UseVisualStyleBackColor = False
        '
        '_tabFields_TabPage2
        '
        Me._tabFields_TabPage2.Controls.Add(Me.txtFileName)
        Me._tabFields_TabPage2.Controls.Add(Me.cmdBrowse)
        Me._tabFields_TabPage2.Controls.Add(Me.cmdInsertFile)
        Me._tabFields_TabPage2.Controls.Add(Me.lblFileName)
        Me._tabFields_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabFields_TabPage2.Name = "_tabFields_TabPage2"
        Me._tabFields_TabPage2.Size = New System.Drawing.Size(259, 487)
        Me._tabFields_TabPage2.TabIndex = 2
        Me._tabFields_TabPage2.Text = "3 File"
        '
        'txtFileName
        '
        Me.txtFileName.AcceptsReturn = True
        Me.txtFileName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.Location = New System.Drawing.Point(8, 130)
        Me.txtFileName.MaxLength = 0
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileName.Size = New System.Drawing.Size(229, 20)
        Me.txtFileName.TabIndex = 6
        '
        'cmdBrowse
        '
        Me.cmdBrowse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowse.Location = New System.Drawing.Point(155, 163)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowse.Size = New System.Drawing.Size(81, 25)
        Me.cmdBrowse.TabIndex = 5
        Me.cmdBrowse.Text = "&Browse..."
        Me.cmdBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBrowse.UseVisualStyleBackColor = False
        '
        'cmdInsertFile
        '
        Me.cmdInsertFile.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInsertFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInsertFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInsertFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInsertFile.Location = New System.Drawing.Point(155, 193)
        Me.cmdInsertFile.Name = "cmdInsertFile"
        Me.cmdInsertFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInsertFile.Size = New System.Drawing.Size(81, 25)
        Me.cmdInsertFile.TabIndex = 4
        Me.cmdInsertFile.Text = "&Insert "
        Me.cmdInsertFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInsertFile.UseVisualStyleBackColor = False
        '
        'lblFileName
        '
        Me.lblFileName.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileName.Location = New System.Drawing.Point(8, 112)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileName.Size = New System.Drawing.Size(100, 19)
        Me.lblFileName.TabIndex = 7
        Me.lblFileName.Text = "File Name:"
        '
        '_tabFields_TabPage3
        '
        Me._tabFields_TabPage3.Controls.Add(Me.lvwClauses)
        Me._tabFields_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabFields_TabPage3.Name = "_tabFields_TabPage3"
        Me._tabFields_TabPage3.Size = New System.Drawing.Size(259, 487)
        Me._tabFields_TabPage3.TabIndex = 3
        Me._tabFields_TabPage3.Text = "3 Clause"
        '
        'lvwClauses
        '
        Me.lvwClauses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClauses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClauses_ColumnHeader_1, Me._lvwClauses_ColumnHeader_2})
        Me.lvwClauses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClauses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClauses.HideSelection = False
        Me.lvwClauses.LabelEdit = True
        Me.lvwClauses.Location = New System.Drawing.Point(6, 13)
        Me.lvwClauses.Name = "lvwClauses"
        Me.lvwClauses.Size = New System.Drawing.Size(247, 286)
        Me.lvwClauses.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwClauses.TabIndex = 8
        Me.lvwClauses.UseCompatibleStateImageBehavior = False
        Me.lvwClauses.View = System.Windows.Forms.View.Details
        '
        '_lvwClauses_ColumnHeader_1
        '
        Me._lvwClauses_ColumnHeader_1.Tag = ""
        Me._lvwClauses_ColumnHeader_1.Text = "code"
        Me._lvwClauses_ColumnHeader_1.Width = 54
        '
        '_lvwClauses_ColumnHeader_2
        '
        Me._lvwClauses_ColumnHeader_2.Tag = ""
        Me._lvwClauses_ColumnHeader_2.Text = "description"
        Me._lvwClauses_ColumnHeader_2.Width = 134
        '
        '_tabFields_TabPage4
        '
        Me._tabFields_TabPage4.Controls.Add(Me.lvwSubDocuments)
        Me._tabFields_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabFields_TabPage4.Name = "_tabFields_TabPage4"
        Me._tabFields_TabPage4.Size = New System.Drawing.Size(259, 487)
        Me._tabFields_TabPage4.TabIndex = 4
        Me._tabFields_TabPage4.Text = "Sub Documents"
        '
        'lvwSubDocuments
        '
        Me.lvwSubDocuments.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSubDocuments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSubDocuments_ColumnHeader_1, Me._lvwSubDocuments_ColumnHeader_2, Me._lvwSubDocuments_ColumnHeader_3})
        Me.lvwSubDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSubDocuments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSubDocuments.HideSelection = False
        Me.lvwSubDocuments.LabelEdit = True
        Me.lvwSubDocuments.Location = New System.Drawing.Point(6, 13)
        Me.lvwSubDocuments.Name = "lvwSubDocuments"
        Me.lvwSubDocuments.Size = New System.Drawing.Size(247, 286)
        Me.lvwSubDocuments.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSubDocuments.TabIndex = 29
        Me.lvwSubDocuments.UseCompatibleStateImageBehavior = False
        Me.lvwSubDocuments.View = System.Windows.Forms.View.Details
        '
        '_lvwSubDocuments_ColumnHeader_1
        '
        Me._lvwSubDocuments_ColumnHeader_1.Tag = ""
        Me._lvwSubDocuments_ColumnHeader_1.Text = "code"
        Me._lvwSubDocuments_ColumnHeader_1.Width = 54
        '
        '_lvwSubDocuments_ColumnHeader_2
        '
        Me._lvwSubDocuments_ColumnHeader_2.Tag = ""
        Me._lvwSubDocuments_ColumnHeader_2.Text = "description"
        Me._lvwSubDocuments_ColumnHeader_2.Width = 147
        '
        '_lvwSubDocuments_ColumnHeader_3
        '
        Me._lvwSubDocuments_ColumnHeader_3.Tag = ""
        Me._lvwSubDocuments_ColumnHeader_3.Text = "ID"
        Me._lvwSubDocuments_ColumnHeader_3.Width = 97
        '
        'dlgFileOpen
        '
        Me.dlgFileOpen.Filter = "Documents (*.doc;*.txt)|*.doc;*.txt|All Files|*.*"
        '
        'Ctx_mnuClauses
        '
        Me.Ctx_mnuClauses.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.Ctx_mnuClauses.Name = "Ctx_mnuClauses"
        Me.Ctx_mnuClauses.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuField
        '
        Me.Ctx_mnuField.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.Ctx_mnuField.Name = "Ctx_mnuField"
        Me.Ctx_mnuField.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuSubDocuments
        '
        Me.Ctx_mnuSubDocuments.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.Ctx_mnuSubDocuments.Name = "Ctx_mnuSubDocuments"
        Me.Ctx_mnuSubDocuments.Size = New System.Drawing.Size(61, 4)
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(268, 549)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabFields)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Location = New System.Drawing.Point(663, 132)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Field Manager"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabFields.ResumeLayout(False)
        Me._tabFields_TabPage0.ResumeLayout(False)
        Me._tabFields_TabPage1.ResumeLayout(False)
        Me.frmDocumentBr.ResumeLayout(False)
        Me.frmDocumentBr.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        Me.fraRiskLoop.ResumeLayout(False)
        Me.fraRiskLoop.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.fraQuestion.ResumeLayout(False)
        Me.fraQuestion.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me._tabFields_TabPage2.ResumeLayout(False)
        Me._tabFields_TabPage2.PerformLayout()
        Me._tabFields_TabPage3.ResumeLayout(False)
        Me._tabFields_TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializeoptRiskType()
        Me.optRiskType(3) = _optRiskType_3
        Me.optRiskType(2) = _optRiskType_2
        Me.optRiskType(1) = _optRiskType_1
        Me.optRiskType(0) = _optRiskType_0
    End Sub
    Sub InitializemnuInsert()
        Me.mnuInsert(0) = _mnuInsert_0
        Me.mnuInsert(1) = _mnuInsert_1
        Me.mnuInsert(2) = _mnuInsert_2
    End Sub
    Sub InitializemnuClause()
        Me.mnuClause(0) = _mnuClause_0
        Me.mnuClause(1) = _mnuClause_1
        Me.mnuClause(2) = _mnuClause_2
        Me.mnuClause(3) = _mnuClause_3
    End Sub
    Sub lvwSubDocuments_InitializeColumnKeys()
        Me._lvwSubDocuments_ColumnHeader_1.Name = ""
        Me._lvwSubDocuments_ColumnHeader_2.Name = ""
        Me._lvwSubDocuments_ColumnHeader_3.Name = ""
    End Sub
    Sub lvwClauses_InitializeColumnKeys()
        Me._lvwClauses_ColumnHeader_1.Name = ""
        Me._lvwClauses_ColumnHeader_2.Name = ""
    End Sub
    Private WithEvents optStandardWordingDescription As System.Windows.Forms.RadioButton
    Private WithEvents optStandardWordingCodes As System.Windows.Forms.RadioButton
    Private WithEvents optStandardWordingText As System.Windows.Forms.RadioButton
    Public WithEvents frmDocumentBr As GroupBox
    Public WithEvents cmdInsertDocBreak As Button
    Public WithEvents Label2 As Label
    Public WithEvents txtDocumentSplit As TextBox
#End Region
End Class