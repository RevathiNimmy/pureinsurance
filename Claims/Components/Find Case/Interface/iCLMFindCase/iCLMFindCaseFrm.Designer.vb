<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwsearchdetails_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
	Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not fTerminateCalled_Form_Terminate_Renamed Then
				fTerminateCalled_Form_Terminate_Renamed = True
				Form_Terminate_Renamed()
			End If
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdCloseCase As System.Windows.Forms.Button
	Public WithEvents cmdEditCase As System.Windows.Forms.Button
	Public WithEvents cmdNewcase As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Private WithEvents _stbstatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbstatus As System.Windows.Forms.StatusStrip
	Public WithEvents lblProgressStatus As System.Windows.Forms.Label
	Public WithEvents lblCaseNumber As System.Windows.Forms.Label
	Public WithEvents lblClaimNumber As System.Windows.Forms.Label
	Public WithEvents lblCaseOpenDate As System.Windows.Forms.Label
	Public WithEvents lblRiskType As System.Windows.Forms.Label
	Public WithEvents cboCaseOpenDate As System.Windows.Forms.DateTimePicker
	Public WithEvents txtCaseNumber As System.Windows.Forms.TextBox
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Public WithEvents cboRiskType As PMLookupControl.cboPMLookup
	Public WithEvents cboProgressStatus As PMLookupControl.cboPMLookup
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents uctPBSearchField1 As uctPBSearchFields.uctPBSearchField
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents _lvwsearchdetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwsearchdetails_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwsearchdetails As System.Windows.Forms.ListView
    Public WithEvents imgImage As System.Windows.Forms.ImageList
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCloseCase = New System.Windows.Forms.Button()
        Me.cmdEditCase = New System.Windows.Forms.Button()
        Me.cmdNewcase = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.stbstatus = New System.Windows.Forms.StatusStrip()
        Me._stbstatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblProgressStatus = New System.Windows.Forms.Label()
        Me.lblCaseNumber = New System.Windows.Forms.Label()
        Me.lblClaimNumber = New System.Windows.Forms.Label()
        Me.lblCaseOpenDate = New System.Windows.Forms.Label()
        Me.lblRiskType = New System.Windows.Forms.Label()
        Me.cboCaseOpenDate = New System.Windows.Forms.DateTimePicker()
        Me.txtCaseNumber = New System.Windows.Forms.TextBox()
        Me.txtClaimNumber = New System.Windows.Forms.TextBox()
        Me.cboRiskType = New PMLookupControl.cboPMLookup()
        Me.cboProgressStatus = New PMLookupControl.cboPMLookup()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.uctPBSearchField1 = New uctPBSearchFields.uctPBSearchField()
        Me.lvwsearchdetails = New System.Windows.Forms.ListView()
        Me._lvwsearchdetails_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imgImage = New System.Windows.Forms.ImageList(Me.components)
        Me.stbstatus.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCloseCase
        '
        Me.cmdCloseCase.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseCase.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseCase.Enabled = False
        Me.cmdCloseCase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCloseCase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseCase.Location = New System.Drawing.Point(184, 387)
        Me.cmdCloseCase.Name = "cmdCloseCase"
        Me.cmdCloseCase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCloseCase.Size = New System.Drawing.Size(81, 25)
        Me.cmdCloseCase.TabIndex = 19
        Me.cmdCloseCase.Text = "Close Case"
        Me.cmdCloseCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCloseCase.UseVisualStyleBackColor = False
        '
        'cmdEditCase
        '
        Me.cmdEditCase.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCase.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCase.Enabled = False
        Me.cmdEditCase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCase.Location = New System.Drawing.Point(96, 387)
        Me.cmdEditCase.Name = "cmdEditCase"
        Me.cmdEditCase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCase.Size = New System.Drawing.Size(81, 25)
        Me.cmdEditCase.TabIndex = 18
        Me.cmdEditCase.Text = "Edit Case"
        Me.cmdEditCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCase.UseVisualStyleBackColor = False
        '
        'cmdNewcase
        '
        Me.cmdNewcase.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewcase.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewcase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewcase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewcase.Location = New System.Drawing.Point(8, 387)
        Me.cmdNewcase.Name = "cmdNewcase"
        Me.cmdNewcase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewcase.Size = New System.Drawing.Size(81, 25)
        Me.cmdNewcase.TabIndex = 17
        Me.cmdNewcase.Text = "New Case"
        Me.cmdNewcase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewcase.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(648, 387)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(81, 25)
        Me.cmdClose.TabIndex = 7
        Me.cmdClose.Text = "Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(648, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 6
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(648, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 5
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'stbstatus
        '
        Me.stbstatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbstatus_Panel1})
        Me.stbstatus.Location = New System.Drawing.Point(0, 412)
        Me.stbstatus.Name = "stbstatus"
        Me.stbstatus.ShowItemToolTips = True
        Me.stbstatus.Size = New System.Drawing.Size(734, 22)
        Me.stbstatus.TabIndex = 8
        '
        '_stbstatus_Panel1
        '
        Me._stbstatus_Panel1.AutoSize = False
        Me._stbstatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbstatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbstatus_Panel1.DoubleClickEnabled = True
        Me._stbstatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbstatus_Panel1.Name = "_stbstatus_Panel1"
        Me._stbstatus_Panel1.Size = New System.Drawing.Size(700, 22)
        Me._stbstatus_Panel1.Tag = ""
        Me._stbstatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(210, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(639, 165)
        Me.tabMainTab.TabIndex = 9
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProgressStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCaseNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCaseOpenDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCaseOpenDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCaseNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProgressStatus)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(631, 139)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Details"
        '
        'lblProgressStatus
        '
        Me.lblProgressStatus.AutoSize = True
        Me.lblProgressStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProgressStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProgressStatus.Location = New System.Drawing.Point(320, 20)
        Me.lblProgressStatus.Name = "lblProgressStatus"
        Me.lblProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProgressStatus.Size = New System.Drawing.Size(102, 13)
        Me.lblProgressStatus.TabIndex = 10
        Me.lblProgressStatus.Text = "Progress Status:"
        '
        'lblCaseNumber
        '
        Me.lblCaseNumber.AutoSize = True
        Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseNumber.Location = New System.Drawing.Point(8, 20)
        Me.lblCaseNumber.Name = "lblCaseNumber"
        Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseNumber.Size = New System.Drawing.Size(90, 13)
        Me.lblCaseNumber.TabIndex = 11
        Me.lblCaseNumber.Text = "Case Number:"
        '
        'lblClaimNumber
        '
        Me.lblClaimNumber.AutoSize = True
        Me.lblClaimNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNumber.Location = New System.Drawing.Point(8, 88)
        Me.lblClaimNumber.Name = "lblClaimNumber"
        Me.lblClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNumber.Size = New System.Drawing.Size(98, 13)
        Me.lblClaimNumber.TabIndex = 12
        Me.lblClaimNumber.Text = "Claim Number :"
        '
        'lblCaseOpenDate
        '
        Me.lblCaseOpenDate.AutoSize = True
        Me.lblCaseOpenDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseOpenDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseOpenDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseOpenDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseOpenDate.Location = New System.Drawing.Point(8, 52)
        Me.lblCaseOpenDate.Name = "lblCaseOpenDate"
        Me.lblCaseOpenDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseOpenDate.Size = New System.Drawing.Size(106, 13)
        Me.lblCaseOpenDate.TabIndex = 13
        Me.lblCaseOpenDate.Text = "Case Open Date:"
        '
        'lblRiskType
        '
        Me.lblRiskType.AutoSize = True
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(320, 88)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(67, 13)
        Me.lblRiskType.TabIndex = 14
        Me.lblRiskType.Text = "Risk Type:"
        '
        'cboCaseOpenDate
        '
        Me.cboCaseOpenDate.Checked = False
        Me.cboCaseOpenDate.CustomFormat = ""
        Me.cboCaseOpenDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right
        Me.cboCaseOpenDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.cboCaseOpenDate.Location = New System.Drawing.Point(120, 49)
        Me.cboCaseOpenDate.Name = "cboCaseOpenDate"
        Me.cboCaseOpenDate.ShowCheckBox = True
        Me.cboCaseOpenDate.Size = New System.Drawing.Size(177, 21)
        Me.cboCaseOpenDate.TabIndex = 2
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(120, 17)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(177, 21)
        Me.txtCaseNumber.TabIndex = 0
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(120, 84)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(177, 21)
        Me.txtClaimNumber.TabIndex = 3
        '
        'cboRiskType
        '
        Me.cboRiskType.DefaultItemId = 0
        Me.cboRiskType.FirstItem = ""
        Me.cboRiskType.ItemId = 0
        Me.cboRiskType.ListIndex = -1
        Me.cboRiskType.Location = New System.Drawing.Point(424, 84)
        Me.cboRiskType.Name = "cboRiskType"
        Me.cboRiskType.PMLookupProductFamily = 1
        Me.cboRiskType.SingleItemId = 0
        Me.cboRiskType.Size = New System.Drawing.Size(201, 21)
        Me.cboRiskType.Sorted = True
        Me.cboRiskType.TabIndex = 4
        Me.cboRiskType.TableName = "risk_type"
        Me.cboRiskType.ToolTipText = ""
        Me.cboRiskType.WhereClause = ""
        '
        'cboProgressStatus
        '
        Me.cboProgressStatus.DefaultItemId = 0
        Me.cboProgressStatus.FirstItem = ""
        Me.cboProgressStatus.ItemId = 0
        Me.cboProgressStatus.ListIndex = -1
        Me.cboProgressStatus.Location = New System.Drawing.Point(424, 20)
        Me.cboProgressStatus.Name = "cboProgressStatus"
        Me.cboProgressStatus.PMLookupProductFamily = 1
        Me.cboProgressStatus.SingleItemId = 0
        Me.cboProgressStatus.Size = New System.Drawing.Size(201, 21)
        Me.cboProgressStatus.Sorted = True
        Me.cboProgressStatus.TabIndex = 1
        Me.cboProgressStatus.TableName = "case_progress"
        Me.cboProgressStatus.ToolTipText = ""
        Me.cboProgressStatus.WhereClause = ""
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.uctPBSearchField1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(631, 139)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Case Builder Fields"
        '
        'uctPBSearchField1
        '
        Me.uctPBSearchField1.DataModelType = 5
        Me.uctPBSearchField1.Dock = System.Windows.Forms.DockStyle.Top
        Me.uctPBSearchField1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPBSearchField1.GISModelCode = ""
        Me.uctPBSearchField1.Location = New System.Drawing.Point(0, 0)
        Me.uctPBSearchField1.Name = "uctPBSearchField1"
        Me.uctPBSearchField1.Size = New System.Drawing.Size(631, 130)
        Me.uctPBSearchField1.TabIndex = 15
        '
        'lvwsearchdetails
        '
        Me.lvwsearchdetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwsearchdetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwsearchdetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwsearchdetails_ColumnHeader_1, Me._lvwsearchdetails_ColumnHeader_2, Me._lvwsearchdetails_ColumnHeader_3, Me._lvwsearchdetails_ColumnHeader_4, Me._lvwsearchdetails_ColumnHeader_5, Me._lvwsearchdetails_ColumnHeader_6, Me._lvwsearchdetails_ColumnHeader_7, Me._lvwsearchdetails_ColumnHeader_8, Me._lvwsearchdetails_ColumnHeader_9, Me._lvwsearchdetails_ColumnHeader_10})
        Me.lvwsearchdetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwsearchdetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwsearchdetails.LargeImageList = Me.imgImage
        Me.lvwsearchdetails.Location = New System.Drawing.Point(8, 176)
        Me.lvwsearchdetails.Name = "lvwsearchdetails"
        Me.lvwsearchdetails.Size = New System.Drawing.Size(725, 206)
        Me.lvwsearchdetails.SmallImageList = Me.imgImage
        Me.lvwsearchdetails.TabIndex = 16
        Me.lvwsearchdetails.UseCompatibleStateImageBehavior = False
        Me.lvwsearchdetails.View = System.Windows.Forms.View.Details
        '
        '_lvwsearchdetails_ColumnHeader_1
        '
        Me._lvwsearchdetails_ColumnHeader_1.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_1.Text = ""
        Me._lvwsearchdetails_ColumnHeader_1.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_2
        '
        Me._lvwsearchdetails_ColumnHeader_2.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_2.Text = ""
        Me._lvwsearchdetails_ColumnHeader_2.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_3
        '
        Me._lvwsearchdetails_ColumnHeader_3.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_3.Text = ""
        Me._lvwsearchdetails_ColumnHeader_3.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_4
        '
        Me._lvwsearchdetails_ColumnHeader_4.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_4.Text = ""
        Me._lvwsearchdetails_ColumnHeader_4.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_5
        '
        Me._lvwsearchdetails_ColumnHeader_5.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_5.Text = ""
        Me._lvwsearchdetails_ColumnHeader_5.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_6
        '
        Me._lvwsearchdetails_ColumnHeader_6.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_6.Text = ""
        Me._lvwsearchdetails_ColumnHeader_6.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_7
        '
        Me._lvwsearchdetails_ColumnHeader_7.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_7.Text = ""
        Me._lvwsearchdetails_ColumnHeader_7.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_8
        '
        Me._lvwsearchdetails_ColumnHeader_8.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_8.Text = ""
        Me._lvwsearchdetails_ColumnHeader_8.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_9
        '
        Me._lvwsearchdetails_ColumnHeader_9.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_9.Text = ""
        Me._lvwsearchdetails_ColumnHeader_9.Width = 97
        '
        '_lvwsearchdetails_ColumnHeader_10
        '
        Me._lvwsearchdetails_ColumnHeader_10.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_10.Text = "Party_cnt"
        Me._lvwsearchdetails_ColumnHeader_10.Width = 0
        '
        'imgImage
        '
        Me.imgImage.ImageStream = CType(resources.GetObject("imgImage.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgImage.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imgImage.Images.SetKeyName(0, "FindImage")
        Me.imgImage.Images.SetKeyName(1, "FindImage2")
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(734, 434)
        Me.Controls.Add(Me.cmdCloseCase)
        Me.Controls.Add(Me.cmdEditCase)
        Me.Controls.Add(Me.cmdNewcase)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.stbstatus)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.lvwsearchdetails)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find Case"
        Me.stbstatus.ResumeLayout(False)
        Me.stbstatus.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwsearchdetails_InitializeColumnKeys()
		Me._lvwsearchdetails_ColumnHeader_1.Name = ""
		Me._lvwsearchdetails_ColumnHeader_2.Name = ""
		Me._lvwsearchdetails_ColumnHeader_3.Name = ""
		Me._lvwsearchdetails_ColumnHeader_4.Name = ""
		Me._lvwsearchdetails_ColumnHeader_5.Name = ""
		Me._lvwsearchdetails_ColumnHeader_6.Name = ""
		Me._lvwsearchdetails_ColumnHeader_7.Name = ""
		Me._lvwsearchdetails_ColumnHeader_8.Name = ""
		Me._lvwsearchdetails_ColumnHeader_9.Name = ""
		Me._lvwsearchdetails_ColumnHeader_10.Name = ""
	End Sub
#End Region 
End Class
