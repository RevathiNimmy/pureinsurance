<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptSinglePolicy()
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
    Private WithEvents _optSinglePolicy_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optSinglePolicy_0 As System.Windows.Forms.RadioButton
    Private WithEvents _optSinglePolicy_2 As System.Windows.Forms.RadioButton
    Public WithEvents tabPolicyVersion As System.Windows.Forms.TabControl
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
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
        Me._optSinglePolicy_1 = New System.Windows.Forms.RadioButton()
        Me._optSinglePolicy_0 = New System.Windows.Forms.RadioButton()
        Me._optSinglePolicy_2 = New System.Windows.Forms.RadioButton()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabPolicyVersion = New System.Windows.Forms.TabControl()
        Me._tabPolicyVersion_TabPage0 = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSqlQuery = New System.Windows.Forms.TextBox()
        Me.lvwPolicyVersion = New System.Windows.Forms.ListView()
        Me._lvwPolicyVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._optSinglePolicy_4 = New System.Windows.Forms.RadioButton()
        Me.cmdGetPolicyVersion = New System.Windows.Forms.Button()
        Me.stbMain = New System.Windows.Forms.StatusStrip()
        Me.MESSAGE = New System.Windows.Forms.ToolStripStatusLabel()
        Me.COUNT = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Ctx_mnuPopUp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lblPolicyStatus = New System.Windows.Forms.Label()
        Me.cboPolicyStatus = New System.Windows.Forms.ComboBox()
        Me.uctAnchor = New uSIRCommonControls.uctAnchor()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPMNumber = New System.Windows.Forms.TextBox()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        Me.tabPolicyVersion.SuspendLayout()
        Me._tabPolicyVersion_TabPage0.SuspendLayout()
        Me.stbMain.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_optSinglePolicy_1
        '
        Me._optSinglePolicy_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_1.Location = New System.Drawing.Point(836, 136)
        Me._optSinglePolicy_1.Name = "_optSinglePolicy_1"
        Me._optSinglePolicy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_1.Size = New System.Drawing.Size(116, 17)
        Me._optSinglePolicy_1.TabIndex = 20
        Me._optSinglePolicy_1.TabStop = True
        Me._optSinglePolicy_1.Text = "Delete This Policy Version"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_1, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_1.UseVisualStyleBackColor = False
        Me._optSinglePolicy_1.Visible = False
        '
        '_optSinglePolicy_0
        '
        Me._optSinglePolicy_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_0.Checked = True
        Me._optSinglePolicy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_0.Location = New System.Drawing.Point(836, 112)
        Me._optSinglePolicy_0.Name = "_optSinglePolicy_0"
        Me._optSinglePolicy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_0.Size = New System.Drawing.Size(169, 22)
        Me._optSinglePolicy_0.TabIndex = 21
        Me._optSinglePolicy_0.TabStop = True
        Me._optSinglePolicy_0.Text = "Repost Transaction"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_0, "Recreate stats folder, details and export folders and repost with option to delet" & _
        "e existing document and recalculate reinsurance")
        Me._optSinglePolicy_0.UseVisualStyleBackColor = False
        Me._optSinglePolicy_0.Visible = False
        '
        '_optSinglePolicy_2
        '
        Me._optSinglePolicy_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSinglePolicy_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSinglePolicy_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSinglePolicy_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSinglePolicy_2.Location = New System.Drawing.Point(836, 159)
        Me._optSinglePolicy_2.Name = "_optSinglePolicy_2"
        Me._optSinglePolicy_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSinglePolicy_2.Size = New System.Drawing.Size(185, 17)
        Me._optSinglePolicy_2.TabIndex = 40
        Me._optSinglePolicy_2.TabStop = True
        Me._optSinglePolicy_2.Text = "Set Policy Status"
        Me.ToolTip1.SetToolTip(Me._optSinglePolicy_2, "Delete this version of policy and all its allocation details")
        Me._optSinglePolicy_2.UseVisualStyleBackColor = False
        Me._optSinglePolicy_2.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(195, 18)
        Me.tabMain.Location = New System.Drawing.Point(12, 37)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(807, 544)
        Me.tabMain.TabIndex = 4
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.cmdExit)
        Me._tabMain_TabPage1.Controls.Add(Me.cmdOK)
        Me._tabMain_TabPage1.Controls.Add(Me.tabPolicyVersion)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(799, 518)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "Policy"
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(694, 474)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(70, 22)
        Me.cmdExit.TabIndex = 21
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
        Me.cmdOK.Location = New System.Drawing.Point(620, 474)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(70, 22)
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabPolicyVersion
        '
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage0)
        Me.tabPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPolicyVersion.ItemSize = New System.Drawing.Size(259, 18)
        Me.tabPolicyVersion.Location = New System.Drawing.Point(0, 3)
        Me.tabPolicyVersion.Multiline = True
        Me.tabPolicyVersion.Name = "tabPolicyVersion"
        Me.tabPolicyVersion.SelectedIndex = 0
        Me.tabPolicyVersion.Size = New System.Drawing.Size(796, 468)
        Me.tabPolicyVersion.TabIndex = 19
        '
        '_tabPolicyVersion_TabPage0
        '
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.Label2)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.txtSqlQuery)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lvwPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me._optSinglePolicy_4)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdGetPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage0.Name = "_tabPolicyVersion_TabPage0"
        Me._tabPolicyVersion_TabPage0.Size = New System.Drawing.Size(788, 442)
        Me._tabPolicyVersion_TabPage0.TabIndex = 0
        Me._tabPolicyVersion_TabPage0.Text = "Policy Version"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(372, 13)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "Copy Sql Here ( In SQL type column in same sequence as show in below List)"
        '
        'txtSqlQuery
        '
        Me.txtSqlQuery.Location = New System.Drawing.Point(10, 38)
        Me.txtSqlQuery.Multiline = True
        Me.txtSqlQuery.Name = "txtSqlQuery"
        Me.txtSqlQuery.Size = New System.Drawing.Size(489, 88)
        Me.txtSqlQuery.TabIndex = 44
        '
        'lvwPolicyVersion
        '
        Me.lvwPolicyVersion.AllowColumnReorder = True
        Me.lvwPolicyVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicyVersion.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyVersion_ColumnHeader_1, Me._lvwPolicyVersion_ColumnHeader_2, Me._lvwPolicyVersion_ColumnHeader_7, Me._lvwPolicyVersion_ColumnHeader_3, Me._lvwPolicyVersion_ColumnHeader_4, Me._lvwPolicyVersion_ColumnHeader_5, Me._lvwPolicyVersion_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicyVersion, False)
        Me.lvwPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicyVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicyVersion.FullRowSelect = True
        Me.lvwPolicyVersion.GridLines = True
        Me.lvwPolicyVersion.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Location = New System.Drawing.Point(3, 154)
        Me.lvwPolicyVersion.Name = "lvwPolicyVersion"
        Me.lvwPolicyVersion.Size = New System.Drawing.Size(778, 329)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicyVersion, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicyVersion, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicyVersion, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicyVersion.TabIndex = 43
        Me.lvwPolicyVersion.UseCompatibleStateImageBehavior = False
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
        '_lvwPolicyVersion_ColumnHeader_5
        '
        Me._lvwPolicyVersion_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_5.Text = "Cover Start Date"
        Me._lvwPolicyVersion_ColumnHeader_5.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_6
        '
        Me._lvwPolicyVersion_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_6.Text = "Document Date"
        Me._lvwPolicyVersion_ColumnHeader_6.Width = 97
        '
        '_optSinglePolicy_4
        '
        Me._optSinglePolicy_4.AutoSize = True
        Me._optSinglePolicy_4.Checked = True
        Me._optSinglePolicy_4.Location = New System.Drawing.Point(526, 38)
        Me._optSinglePolicy_4.Name = "_optSinglePolicy_4"
        Me._optSinglePolicy_4.Size = New System.Drawing.Size(204, 17)
        Me._optSinglePolicy_4.TabIndex = 42
        Me._optSinglePolicy_4.TabStop = True
        Me._optSinglePolicy_4.Text = "Reverse and Regenerate Transaction"
        Me._optSinglePolicy_4.UseVisualStyleBackColor = True
        '
        'cmdGetPolicyVersion
        '
        Me.cmdGetPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetPolicyVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGetPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetPolicyVersion.Location = New System.Drawing.Point(540, 101)
        Me.cmdGetPolicyVersion.Name = "cmdGetPolicyVersion"
        Me.cmdGetPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetPolicyVersion.Size = New System.Drawing.Size(166, 25)
        Me.cmdGetPolicyVersion.TabIndex = 22
        Me.cmdGetPolicyVersion.Text = "Get Corrupted Policy Versions"
        Me.cmdGetPolicyVersion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetPolicyVersion.UseVisualStyleBackColor = False
        '
        'stbMain
        '
        Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MESSAGE, Me.COUNT})
        Me.stbMain.Location = New System.Drawing.Point(0, 617)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = True
        Me.stbMain.Size = New System.Drawing.Size(817, 22)
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
        'lblPolicyStatus
        '
        Me.lblPolicyStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStatus.Enabled = False
        Me.lblPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStatus.Location = New System.Drawing.Point(826, 189)
        Me.lblPolicyStatus.Name = "lblPolicyStatus"
        Me.lblPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStatus.Size = New System.Drawing.Size(73, 19)
        Me.lblPolicyStatus.TabIndex = 42
        Me.lblPolicyStatus.Text = "Policy Status :"
        Me.lblPolicyStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblPolicyStatus.Visible = False
        '
        'cboPolicyStatus
        '
        Me.cboPolicyStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPolicyStatus.Enabled = False
        Me.cboPolicyStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPolicyStatus.Location = New System.Drawing.Point(926, 187)
        Me.cboPolicyStatus.Name = "cboPolicyStatus"
        Me.cboPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyStatus.Size = New System.Drawing.Size(277, 21)
        Me.cboPolicyStatus.TabIndex = 41
        Me.cboPolicyStatus.Visible = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(0, 598)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 43
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(545, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(114, 13)
        Me.Label6.TabIndex = 54
        Me.Label6.Text = "IM/PM Ref Number"
        '
        'txtPMNumber
        '
        Me.txtPMNumber.Location = New System.Drawing.Point(660, 21)
        Me.txtPMNumber.Name = "txtPMNumber"
        Me.txtPMNumber.Size = New System.Drawing.Size(140, 21)
        Me.txtPMNumber.TabIndex = 55
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(817, 639)
        Me.Controls.Add(Me.txtPMNumber)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblPolicyStatus)
        Me.Controls.Add(Me.cboPolicyStatus)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me._optSinglePolicy_1)
        Me.Controls.Add(Me._optSinglePolicy_0)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me._optSinglePolicy_2)
        Me.Controls.Add(Me.uctAnchor)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Incorrect VAT on Commission to GLCredit Utility"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage1.ResumeLayout(False)
        Me.tabPolicyVersion.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.PerformLayout()
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
        'Me.optSinglePolicy(0) = _optSinglePolicy_3
        Me.optSinglePolicy(1) = _optSinglePolicy_4
    End Sub
   
    

    
    Private WithEvents _tabPolicyVersion_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lvwPolicyVersion As System.Windows.Forms.ListView
    Private WithEvents _lvwPolicyVersion_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _optSinglePolicy_4 As System.Windows.Forms.RadioButton
    Public WithEvents cmdGetPolicyVersion As System.Windows.Forms.Button
    Public WithEvents lblPolicyStatus As System.Windows.Forms.Label
    Public WithEvents cboPolicyStatus As System.Windows.Forms.ComboBox
    Friend WithEvents txtSqlQuery As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents _lvwPolicyVersion_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPMNumber As System.Windows.Forms.TextBox
#End Region
End Class