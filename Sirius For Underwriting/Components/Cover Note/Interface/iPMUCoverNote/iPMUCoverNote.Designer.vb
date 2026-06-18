<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSheets_InitializeColumnKeys()
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
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblCreatedDate As System.Windows.Forms.Label
    Public WithEvents lblEndNumber As System.Windows.Forms.Label
    Public WithEvents lblStartNumber As System.Windows.Forms.Label
    Public WithEvents lblAgent As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblCoverNoteBookStatus As System.Windows.Forms.Label
    Public WithEvents lblBookNumber As System.Windows.Forms.Label
    Public WithEvents lblCoverNoteSheet As System.Windows.Forms.Label
    Public WithEvents cboCreatedDate As System.Windows.Forms.DateTimePicker
	Public WithEvents cboEffectiveDate As System.Windows.Forms.DateTimePicker
	Public WithEvents cboCoverNoteBookStatus As System.Windows.Forms.ComboBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents txtEndNumber As System.Windows.Forms.TextBox
	Public WithEvents cmdAgentLookup As System.Windows.Forms.Button
	Public WithEvents txtBookNumber As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents txtStartNumber As System.Windows.Forms.TextBox
	Public WithEvents uctPickListProducts As uctPickList.PickList
	Public WithEvents fraProducts As System.Windows.Forms.GroupBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwSheets = New System.Windows.Forms.ListView
        Me._lvwSheets_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSheets_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblCreatedDate = New System.Windows.Forms.Label
        Me.lblEndNumber = New System.Windows.Forms.Label
        Me.lblStartNumber = New System.Windows.Forms.Label
        Me.lblAgent = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblCoverNoteBookStatus = New System.Windows.Forms.Label
        Me.lblBookNumber = New System.Windows.Forms.Label
        Me.lblCoverNoteSheet = New System.Windows.Forms.Label
        Me.cboCreatedDate = New System.Windows.Forms.DateTimePicker
        Me.cboEffectiveDate = New System.Windows.Forms.DateTimePicker
        Me.cboCoverNoteBookStatus = New System.Windows.Forms.ComboBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.txtEndNumber = New System.Windows.Forms.TextBox
        Me.cmdAgentLookup = New System.Windows.Forms.Button
        Me.txtBookNumber = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtStartNumber = New System.Windows.Forms.TextBox
        Me.fraProducts = New System.Windows.Forms.GroupBox
        Me.uctPickListProducts = New uctPickList.PickList
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraProducts.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(254, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(5, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(767, 486)
        Me.tabMainTab.TabIndex = 27
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSheets)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCreatedDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEndNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStartNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCoverNoteBookStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBookNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCoverNoteSheet)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCreatedDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCoverNoteBookStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEndNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAgentLookup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBookNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStartNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraProducts)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(759, 460)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Cover Note Book Details"
        '
        'lvwSheets
        '
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSheets, "")
        Me.lvwSheets.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSheets_ColumnHeader_1, Me._lvwSheets_ColumnHeader_2, Me._lvwSheets_ColumnHeader_3, Me._lvwSheets_ColumnHeader_4, Me._lvwSheets_ColumnHeader_5, Me._lvwSheets_ColumnHeader_6, Me._lvwSheets_ColumnHeader_7, Me._lvwSheets_ColumnHeader_8})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSheets, False)
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSheets, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSheets, "")
        Me.lvwSheets.Location = New System.Drawing.Point(8, 252)
        Me.lvwSheets.Name = "lvwSheets"
        Me.lvwSheets.Size = New System.Drawing.Size(748, 176)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSheets, "")
        Me.listViewHelper1.SetSorted(Me.lvwSheets, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSheets, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSheets, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSheets.TabIndex = 20
        Me.lvwSheets.UseCompatibleStateImageBehavior = False
        Me.lvwSheets.View = System.Windows.Forms.View.Details
        '
        '_lvwSheets_ColumnHeader_1
        '
        Me._lvwSheets_ColumnHeader_1.Text = "1"
        Me._lvwSheets_ColumnHeader_1.Width = 0
        '
        '_lvwSheets_ColumnHeader_2
        '
        Me._lvwSheets_ColumnHeader_2.Text = "2"
        Me._lvwSheets_ColumnHeader_2.Width = 116
        '
        '_lvwSheets_ColumnHeader_3
        '
        Me._lvwSheets_ColumnHeader_3.Text = "3"
        Me._lvwSheets_ColumnHeader_3.Width = 115
        '
        '_lvwSheets_ColumnHeader_4
        '
        Me._lvwSheets_ColumnHeader_4.Text = "4"
        Me._lvwSheets_ColumnHeader_4.Width = 104
        '
        '_lvwSheets_ColumnHeader_5
        '
        Me._lvwSheets_ColumnHeader_5.Text = "5"
        Me._lvwSheets_ColumnHeader_5.Width = 104
        '
        '_lvwSheets_ColumnHeader_6
        '
        Me._lvwSheets_ColumnHeader_6.Text = "6"
        Me._lvwSheets_ColumnHeader_6.Width = 104
        '
        '_lvwSheets_ColumnHeader_7
        '
        Me._lvwSheets_ColumnHeader_7.Text = "7"
        Me._lvwSheets_ColumnHeader_7.Width = 104
        '
        '_lvwSheets_ColumnHeader_8
        '
        Me._lvwSheets_ColumnHeader_8.Text = "8"
        Me._lvwSheets_ColumnHeader_8.Width = 104
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(8, 91)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(94, 21)
        Me.lblEffectiveDate.TabIndex = 6
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblCreatedDate
        '
        Me.lblCreatedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreatedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreatedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreatedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreatedDate.Location = New System.Drawing.Point(8, 202)
        Me.lblCreatedDate.Name = "lblCreatedDate"
        Me.lblCreatedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreatedDate.Size = New System.Drawing.Size(94, 21)
        Me.lblCreatedDate.TabIndex = 15
        Me.lblCreatedDate.Text = "Created Date:"
        '
        'lblEndNumber
        '
        Me.lblEndNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndNumber.Location = New System.Drawing.Point(8, 63)
        Me.lblEndNumber.Name = "lblEndNumber"
        Me.lblEndNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndNumber.Size = New System.Drawing.Size(94, 21)
        Me.lblEndNumber.TabIndex = 4
        Me.lblEndNumber.Text = "End Number:"
        '
        'lblStartNumber
        '
        Me.lblStartNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartNumber.Location = New System.Drawing.Point(8, 34)
        Me.lblStartNumber.Name = "lblStartNumber"
        Me.lblStartNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartNumber.Size = New System.Drawing.Size(94, 21)
        Me.lblStartNumber.TabIndex = 2
        Me.lblStartNumber.Text = "Start Number:"
        '
        'lblAgent
        '
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(8, 118)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(94, 21)
        Me.lblAgent.TabIndex = 8
        Me.lblAgent.Text = "Agent:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 147)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(94, 21)
        Me.lblBranch.TabIndex = 11
        Me.lblBranch.Text = "Branch:"
        '
        'lblCoverNoteBookStatus
        '
        Me.lblCoverNoteBookStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteBookStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteBookStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteBookStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteBookStatus.Location = New System.Drawing.Point(8, 175)
        Me.lblCoverNoteBookStatus.Name = "lblCoverNoteBookStatus"
        Me.lblCoverNoteBookStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteBookStatus.Size = New System.Drawing.Size(94, 21)
        Me.lblCoverNoteBookStatus.TabIndex = 13
        Me.lblCoverNoteBookStatus.Text = "Book Status:"
        '
        'lblBookNumber
        '
        Me.lblBookNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblBookNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBookNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBookNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBookNumber.Location = New System.Drawing.Point(8, 7)
        Me.lblBookNumber.Name = "lblBookNumber"
        Me.lblBookNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBookNumber.Size = New System.Drawing.Size(94, 21)
        Me.lblBookNumber.TabIndex = 0
        Me.lblBookNumber.Text = "Book Number:"
        '
        'lblCoverNoteSheet
        '
        Me.lblCoverNoteSheet.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteSheet.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteSheet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteSheet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteSheet.Location = New System.Drawing.Point(8, 231)
        Me.lblCoverNoteSheet.Name = "lblCoverNoteSheet"
        Me.lblCoverNoteSheet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteSheet.Size = New System.Drawing.Size(119, 21)
        Me.lblCoverNoteSheet.TabIndex = 19
        Me.lblCoverNoteSheet.Text = "Cover Note Sheets"
        '
        'cboCreatedDate
        '
        Me.cboCreatedDate.Enabled = False
        Me.cboCreatedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboCreatedDate.Location = New System.Drawing.Point(107, 200)
        Me.cboCreatedDate.Name = "cboCreatedDate"
        Me.cboCreatedDate.Size = New System.Drawing.Size(153, 21)
        Me.cboCreatedDate.TabIndex = 16
        '
        'cboEffectiveDate
        '
        Me.cboEffectiveDate.Checked = False
        Me.cboEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboEffectiveDate.Location = New System.Drawing.Point(107, 88)
        Me.cboEffectiveDate.Name = "cboEffectiveDate"
        Me.cboEffectiveDate.Size = New System.Drawing.Size(153, 21)
        Me.cboEffectiveDate.TabIndex = 7
        '
        'cboCoverNoteBookStatus
        '
        Me.cboCoverNoteBookStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboCoverNoteBookStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCoverNoteBookStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCoverNoteBookStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCoverNoteBookStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCoverNoteBookStatus.Location = New System.Drawing.Point(107, 172)
        Me.cboCoverNoteBookStatus.Name = "cboCoverNoteBookStatus"
        Me.cboCoverNoteBookStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCoverNoteBookStatus.Size = New System.Drawing.Size(185, 21)
        Me.cboCoverNoteBookStatus.TabIndex = 14
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(107, 144)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(185, 21)
        Me.cboBranch.TabIndex = 12
        '
        'txtEndNumber
        '
        Me.txtEndNumber.AcceptsReturn = True
        Me.txtEndNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndNumber.Location = New System.Drawing.Point(107, 62)
        Me.txtEndNumber.MaxLength = 0
        Me.txtEndNumber.Name = "txtEndNumber"
        Me.txtEndNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndNumber.Size = New System.Drawing.Size(122, 20)
        Me.txtEndNumber.TabIndex = 5
        '
        'cmdAgentLookup
        '
        Me.cmdAgentLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgentLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgentLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgentLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgentLookup.Location = New System.Drawing.Point(317, 116)
        Me.cmdAgentLookup.Name = "cmdAgentLookup"
        Me.cmdAgentLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgentLookup.Size = New System.Drawing.Size(23, 22)
        Me.cmdAgentLookup.TabIndex = 10
        Me.cmdAgentLookup.Text = "..."
        Me.cmdAgentLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgentLookup.UseVisualStyleBackColor = False
        '
        'txtBookNumber
        '
        Me.txtBookNumber.AcceptsReturn = True
        Me.txtBookNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtBookNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBookNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBookNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBookNumber.Location = New System.Drawing.Point(107, 6)
        Me.txtBookNumber.MaxLength = 50
        Me.txtBookNumber.Name = "txtBookNumber"
        Me.txtBookNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBookNumber.Size = New System.Drawing.Size(231, 20)
        Me.txtBookNumber.TabIndex = 1
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Enabled = False
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(107, 116)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.ReadOnly = True
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(210, 20)
        Me.txtAgent.TabIndex = 9
        '
        'txtStartNumber
        '
        Me.txtStartNumber.AcceptsReturn = True
        Me.txtStartNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartNumber.Location = New System.Drawing.Point(107, 34)
        Me.txtStartNumber.MaxLength = 0
        Me.txtStartNumber.Name = "txtStartNumber"
        Me.txtStartNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartNumber.Size = New System.Drawing.Size(122, 20)
        Me.txtStartNumber.TabIndex = 3
        '
        'fraProducts
        '
        Me.fraProducts.BackColor = System.Drawing.SystemColors.Control
        Me.fraProducts.Controls.Add(Me.uctPickListProducts)
        Me.fraProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProducts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProducts.Location = New System.Drawing.Point(344, 4)
        Me.fraProducts.Name = "fraProducts"
        Me.fraProducts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraProducts.Size = New System.Drawing.Size(417, 230)
        Me.fraProducts.TabIndex = 17
        Me.fraProducts.TabStop = False
        Me.fraProducts.Text = "Products"
        '
        'uctPickListProducts
        '
        Me.uctPickListProducts.AvailableCaption = ""
        Me.uctPickListProducts.BusinessObject = "bSIRCoverNote.Business"
        Me.uctPickListProducts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListProducts.ForeignKeys = CType(resources.GetObject("uctPickListProducts.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListProducts.IsSearchable = False
        Me.uctPickListProducts.Location = New System.Drawing.Point(3, 16)
        Me.uctPickListProducts.Name = "uctPickListProducts"
        Me.uctPickListProducts.PickListType = "Products"
        Me.uctPickListProducts.Size = New System.Drawing.Size(408, 213)
        Me.uctPickListProducts.TabIndex = 18
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(523, 434)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 21
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(683, 434)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 23
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(603, 434)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 22
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(539, 494)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 24
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(699, 494)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(619, 494)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 25
        Me.cmdApply.Text = "A&pply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(775, 516)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdApply)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(158, 147)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Cover Note Book"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.fraProducts.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwSheets_InitializeColumnKeys()

        Me._lvwSheets_ColumnHeader_1.Name = ""
        Me._lvwSheets_ColumnHeader_2.Name = ""
        Me._lvwSheets_ColumnHeader_3.Name = ""
        Me._lvwSheets_ColumnHeader_4.Name = ""
        Me._lvwSheets_ColumnHeader_5.Name = ""
        Me._lvwSheets_ColumnHeader_6.Name = ""
        Me._lvwSheets_ColumnHeader_7.Name = ""
        Me._lvwSheets_ColumnHeader_8.Name = ""
    End Sub
    Friend WithEvents lvwSheets As System.Windows.Forms.ListView
    Friend WithEvents _lvwSheets_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSheets_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
#End Region 
End Class