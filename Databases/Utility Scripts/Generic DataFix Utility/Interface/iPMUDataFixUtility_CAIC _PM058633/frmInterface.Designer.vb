<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
    Public WithEvents MESSAGE As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents COUNT As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbMain As System.Windows.Forms.StatusStrip
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabPolicyVersionPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.stbMain = New System.Windows.Forms.StatusStrip()
        Me.MESSAGE = New System.Windows.Forms.ToolStripStatusLabel()
        Me.COUNT = New System.Windows.Forms.ToolStripStatusLabel()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lvwPolicyVersion = New System.Windows.Forms.ListView()
        Me._lvwPolicyVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicyVersion_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwIncorrectCommissionPostingList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblPolicyStatus = New System.Windows.Forms.Label()
        Me.cboPolicyStatus = New System.Windows.Forms.ComboBox()
        Me.tabPolicyVersion = New System.Windows.Forms.TabControl()
        Me._tabPolicyVersion_TabPage0 = New System.Windows.Forms.TabPage()
        Me.CmdSelectAllPolicy = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSqlQuery = New System.Windows.Forms.TextBox()
        Me.cmdGetPolicyVersion = New System.Windows.Forms.Button()
        Me._tabPolicyVersion_TabPage1 = New System.Windows.Forms.TabPage()
        Me.cmdselectall1 = New System.Windows.Forms.Button()
        Me.cmdexit1 = New System.Windows.Forms.Button()
        Me.cmdok1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSqlQuery1 = New System.Windows.Forms.TextBox()
        Me.cmdGetRecords = New System.Windows.Forms.Button()
        Me._lvwPolicyVersion_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.stbMain.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPolicyVersion.SuspendLayout()
        Me._tabPolicyVersion_TabPage0.SuspendLayout()
        Me._tabPolicyVersion_TabPage1.SuspendLayout()
        Me.SuspendLayout()
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
        'lvwPolicyVersion
        '
        Me.lvwPolicyVersion.AllowColumnReorder = True
        Me.lvwPolicyVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicyVersion.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicyVersion_ColumnHeader_1, Me._lvwPolicyVersion_ColumnHeader_2, Me._lvwPolicyVersion_ColumnHeader_3, Me._lvwPolicyVersion_ColumnHeader_4, Me._lvwPolicyVersion_ColumnHeader_5, Me._lvwPolicyVersion_ColumnHeader_6, Me._lvwPolicyVersion_ColumnHeader_7, Me._lvwPolicyVersion_ColumnHeader_8, Me._lvwPolicyVersion_ColumnHeader_9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicyVersion, False)
        Me.lvwPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicyVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicyVersion.FullRowSelect = True
        Me.lvwPolicyVersion.GridLines = True
        Me.lvwPolicyVersion.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicyVersion, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicyVersion, "")
        Me.lvwPolicyVersion.Location = New System.Drawing.Point(10, 122)
        Me.lvwPolicyVersion.Name = "lvwPolicyVersion"
        Me.lvwPolicyVersion.Size = New System.Drawing.Size(791, 411)
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
        Me._lvwPolicyVersion_ColumnHeader_1.Width = 71
        '
        '_lvwPolicyVersion_ColumnHeader_2
        '
        Me._lvwPolicyVersion_ColumnHeader_2.Text = "Policy Ref"
        Me._lvwPolicyVersion_ColumnHeader_2.Width = 124
        '
        '_lvwPolicyVersion_ColumnHeader_3
        '
        Me._lvwPolicyVersion_ColumnHeader_3.Text = "Document ID"
        Me._lvwPolicyVersion_ColumnHeader_3.Width = 84
        '
        '_lvwPolicyVersion_ColumnHeader_4
        '
        Me._lvwPolicyVersion_ColumnHeader_4.Text = "Document Ref"
        Me._lvwPolicyVersion_ColumnHeader_4.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_5
        '
        Me._lvwPolicyVersion_ColumnHeader_5.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_5.Text = "Spare"
        Me._lvwPolicyVersion_ColumnHeader_5.Width = 58
        '
        '_lvwPolicyVersion_ColumnHeader_6
        '
        Me._lvwPolicyVersion_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwPolicyVersion_ColumnHeader_6.Text = "Amount"
        Me._lvwPolicyVersion_ColumnHeader_6.Width = 97
        '
        '_lvwPolicyVersion_ColumnHeader_7
        '
        Me._lvwPolicyVersion_ColumnHeader_7.Text = "Transaction_Type_ID"
        Me._lvwPolicyVersion_ColumnHeader_7.Width = 0
        '
        '_lvwPolicyVersion_ColumnHeader_8
        '
        Me._lvwPolicyVersion_ColumnHeader_8.Text = "Transaction Type"
        Me._lvwPolicyVersion_ColumnHeader_8.Width = 103
        '
        'lvwIncorrectCommissionPostingList
        '
        Me.lvwIncorrectCommissionPostingList.AllowColumnReorder = True
        Me.lvwIncorrectCommissionPostingList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwIncorrectCommissionPostingList.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwIncorrectCommissionPostingList, "")
        Me.lvwIncorrectCommissionPostingList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader12, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader13})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwIncorrectCommissionPostingList, False)
        Me.lvwIncorrectCommissionPostingList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwIncorrectCommissionPostingList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwIncorrectCommissionPostingList.FullRowSelect = True
        Me.lvwIncorrectCommissionPostingList.GridLines = True
        Me.lvwIncorrectCommissionPostingList.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwIncorrectCommissionPostingList, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwIncorrectCommissionPostingList, "")
        Me.lvwIncorrectCommissionPostingList.Location = New System.Drawing.Point(10, 124)
        Me.lvwIncorrectCommissionPostingList.Name = "lvwIncorrectCommissionPostingList"
        Me.lvwIncorrectCommissionPostingList.Size = New System.Drawing.Size(791, 411)
        Me.listViewHelper1.SetSmallIcons(Me.lvwIncorrectCommissionPostingList, "")
        Me.listViewHelper1.SetSorted(Me.lvwIncorrectCommissionPostingList, False)
        Me.listViewHelper1.SetSortKey(Me.lvwIncorrectCommissionPostingList, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwIncorrectCommissionPostingList, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwIncorrectCommissionPostingList.TabIndex = 47
        Me.lvwIncorrectCommissionPostingList.UseCompatibleStateImageBehavior = False
        Me.lvwIncorrectCommissionPostingList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Tag = "VALUESORT"
        Me.ColumnHeader1.Text = "Policy ID"
        Me.ColumnHeader1.Width = 54
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Policy Ref"
        Me.ColumnHeader2.Width = 112
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Premium Finance Cnt"
        Me.ColumnHeader3.Width = 0
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Premium Finance Version"
        Me.ColumnHeader4.Width = 0
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Tag = "DATESORT"
        Me.ColumnHeader5.Text = "Account ID"
        Me.ColumnHeader5.Width = 0
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Tag = "DATESORT"
        Me.ColumnHeader6.Text = "Instalment ID"
        Me.ColumnHeader6.Width = 0
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Instalment Amount"
        Me.ColumnHeader7.Width = 101
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Total Financed Amount"
        Me.ColumnHeader8.Width = 127
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Total Commission"
        Me.ColumnHeader12.Width = 95
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Commission Paid"
        Me.ColumnHeader9.Width = 92
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Correct Commission"
        Me.ColumnHeader10.Width = 108
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Correction Amount"
        Me.ColumnHeader11.Width = 106
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Transaction Date"
        Me.ColumnHeader13.Width = 100
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
        'tabPolicyVersion
        '
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage0)
        Me.tabPolicyVersion.Controls.Add(Me._tabPolicyVersion_TabPage1)
        Me.tabPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPolicyVersion.ItemSize = New System.Drawing.Size(259, 18)
        Me.tabPolicyVersion.Location = New System.Drawing.Point(0, 1)
        Me.tabPolicyVersion.Multiline = True
        Me.tabPolicyVersion.Name = "tabPolicyVersion"
        Me.tabPolicyVersion.SelectedIndex = 0
        Me.tabPolicyVersion.Size = New System.Drawing.Size(817, 613)
        Me.tabPolicyVersion.TabIndex = 44
        '
        '_tabPolicyVersion_TabPage0
        '
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.CmdSelectAllPolicy)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdExit)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdOk)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.Label2)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.txtSqlQuery)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.lvwPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Controls.Add(Me.cmdGetPolicyVersion)
        Me._tabPolicyVersion_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage0.Name = "_tabPolicyVersion_TabPage0"
        Me._tabPolicyVersion_TabPage0.Size = New System.Drawing.Size(809, 587)
        Me._tabPolicyVersion_TabPage0.TabIndex = 0
        Me._tabPolicyVersion_TabPage0.Text = "Sub-Agent Suspense Posting Fix"
        '
        'CmdSelectAllPolicy
        '
        Me.CmdSelectAllPolicy.Location = New System.Drawing.Point(10, 549)
        Me.CmdSelectAllPolicy.Name = "CmdSelectAllPolicy"
        Me.CmdSelectAllPolicy.Size = New System.Drawing.Size(75, 23)
        Me.CmdSelectAllPolicy.TabIndex = 49
        Me.CmdSelectAllPolicy.Text = "Select All"
        Me.CmdSelectAllPolicy.UseVisualStyleBackColor = True
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(699, 549)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(75, 23)
        Me.cmdExit.TabIndex = 48
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(608, 549)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 47
        Me.cmdOk.Text = "OK"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(372, 13)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "Copy Sql Here ( In SQL type column in same sequence as show in below List)"
        '
        'txtSqlQuery
        '
        Me.txtSqlQuery.Location = New System.Drawing.Point(10, 24)
        Me.txtSqlQuery.Multiline = True
        Me.txtSqlQuery.Name = "txtSqlQuery"
        Me.txtSqlQuery.Size = New System.Drawing.Size(618, 88)
        Me.txtSqlQuery.TabIndex = 44
        '
        'cmdGetPolicyVersion
        '
        Me.cmdGetPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetPolicyVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGetPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetPolicyVersion.Location = New System.Drawing.Point(634, 91)
        Me.cmdGetPolicyVersion.Name = "cmdGetPolicyVersion"
        Me.cmdGetPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetPolicyVersion.Size = New System.Drawing.Size(167, 25)
        Me.cmdGetPolicyVersion.TabIndex = 22
        Me.cmdGetPolicyVersion.Text = "Get Corrupted Entries"
        Me.cmdGetPolicyVersion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetPolicyVersion.UseVisualStyleBackColor = False
        '
        '_tabPolicyVersion_TabPage1
        '
        Me._tabPolicyVersion_TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdselectall1)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdexit1)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdok1)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.Label1)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.txtSqlQuery1)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.lvwIncorrectCommissionPostingList)
        Me._tabPolicyVersion_TabPage1.Controls.Add(Me.cmdGetRecords)
        Me._tabPolicyVersion_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabPolicyVersion_TabPage1.Name = "_tabPolicyVersion_TabPage1"
        Me._tabPolicyVersion_TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me._tabPolicyVersion_TabPage1.Size = New System.Drawing.Size(809, 587)
        Me._tabPolicyVersion_TabPage1.TabIndex = 1
        Me._tabPolicyVersion_TabPage1.Text = "Incorrect Commission Posting Fix"
        '
        'cmdselectall1
        '
        Me.cmdselectall1.Location = New System.Drawing.Point(19, 546)
        Me.cmdselectall1.Name = "cmdselectall1"
        Me.cmdselectall1.Size = New System.Drawing.Size(75, 23)
        Me.cmdselectall1.TabIndex = 52
        Me.cmdselectall1.Text = "Select All"
        Me.cmdselectall1.UseVisualStyleBackColor = True
        '
        'cmdexit1
        '
        Me.cmdexit1.Location = New System.Drawing.Point(708, 548)
        Me.cmdexit1.Name = "cmdexit1"
        Me.cmdexit1.Size = New System.Drawing.Size(75, 23)
        Me.cmdexit1.TabIndex = 51
        Me.cmdexit1.Text = "Exit"
        Me.cmdexit1.UseVisualStyleBackColor = True
        '
        'cmdok1
        '
        Me.cmdok1.Location = New System.Drawing.Point(617, 547)
        Me.cmdok1.Name = "cmdok1"
        Me.cmdok1.Size = New System.Drawing.Size(75, 23)
        Me.cmdok1.TabIndex = 50
        Me.cmdok1.Text = "OK"
        Me.cmdok1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(372, 13)
        Me.Label1.TabIndex = 49
        Me.Label1.Text = "Copy Sql Here ( In SQL type column in same sequence as show in below List)"
        '
        'txtSqlQuery1
        '
        Me.txtSqlQuery1.Location = New System.Drawing.Point(10, 26)
        Me.txtSqlQuery1.Multiline = True
        Me.txtSqlQuery1.Name = "txtSqlQuery1"
        Me.txtSqlQuery1.Size = New System.Drawing.Size(618, 88)
        Me.txtSqlQuery1.TabIndex = 48
        '
        'cmdGetRecords
        '
        Me.cmdGetRecords.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGetRecords.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGetRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGetRecords.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGetRecords.Location = New System.Drawing.Point(634, 93)
        Me.cmdGetRecords.Name = "cmdGetRecords"
        Me.cmdGetRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGetRecords.Size = New System.Drawing.Size(167, 25)
        Me.cmdGetRecords.TabIndex = 46
        Me.cmdGetRecords.Text = "Get Corrupted Entries"
        Me.cmdGetRecords.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGetRecords.UseVisualStyleBackColor = False
        '
        '_lvwPolicyVersion_ColumnHeader_9
        '
        Me._lvwPolicyVersion_ColumnHeader_9.Text = "Transaction Date"
        Me._lvwPolicyVersion_ColumnHeader_9.Width = 100
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(817, 639)
        Me.Controls.Add(Me.tabPolicyVersion)
        Me.Controls.Add(Me.lblPolicyStatus)
        Me.Controls.Add(Me.cboPolicyStatus)
        Me.Controls.Add(Me.stbMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Fix Utility"
        Me.stbMain.ResumeLayout(False)
        Me.stbMain.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPolicyVersion.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage0.PerformLayout()
        Me._tabPolicyVersion_TabPage1.ResumeLayout(False)
        Me._tabPolicyVersion_TabPage1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents lblPolicyStatus As System.Windows.Forms.Label
    Public WithEvents cboPolicyStatus As System.Windows.Forms.ComboBox
    Public WithEvents tabPolicyVersion As TabControl
    Private WithEvents _tabPolicyVersion_TabPage0 As TabPage
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSqlQuery As TextBox
    Public WithEvents lvwPolicyVersion As ListView
    Private WithEvents _lvwPolicyVersion_ColumnHeader_1 As ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_2 As ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_3 As ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_4 As ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_5 As ColumnHeader
    Private WithEvents _lvwPolicyVersion_ColumnHeader_6 As ColumnHeader
    Public WithEvents cmdGetPolicyVersion As Button
    Friend WithEvents CmdSelectAllPolicy As Button
    Friend WithEvents cmdExit As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents _lvwPolicyVersion_ColumnHeader_7 As ColumnHeader
    Friend WithEvents _lvwPolicyVersion_ColumnHeader_8 As ColumnHeader
    Friend WithEvents _tabPolicyVersion_TabPage1 As TabPage
    Friend WithEvents cmdselectall1 As Button
    Friend WithEvents cmdexit1 As Button
    Friend WithEvents cmdok1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSqlQuery1 As TextBox
    Public WithEvents lvwIncorrectCommissionPostingList As ListView
    Private WithEvents ColumnHeader1 As ColumnHeader
    Private WithEvents ColumnHeader2 As ColumnHeader
    Private WithEvents ColumnHeader3 As ColumnHeader
    Private WithEvents ColumnHeader4 As ColumnHeader
    Private WithEvents ColumnHeader5 As ColumnHeader
    Private WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents ColumnHeader7 As ColumnHeader
    Friend WithEvents ColumnHeader8 As ColumnHeader
    Friend WithEvents ColumnHeader9 As ColumnHeader
    Friend WithEvents ColumnHeader10 As ColumnHeader
    Friend WithEvents ColumnHeader11 As ColumnHeader
    Public WithEvents cmdGetRecords As Button
    Friend WithEvents ColumnHeader12 As ColumnHeader
    Friend WithEvents ColumnHeader13 As ColumnHeader
    Friend WithEvents _lvwPolicyVersion_ColumnHeader_9 As ColumnHeader
#End Region
End Class