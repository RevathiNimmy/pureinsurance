<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewAllocation
#Region "Windows Form Designer generated code "
	Friend Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdNext()
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
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdReverse As System.Windows.Forms.Button
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Private WithEvents _lvwCredits_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCredits_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCredits_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCredits_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCredits As System.Windows.Forms.ListView
	Public WithEvents fraCredit As System.Windows.Forms.GroupBox
	Private WithEvents _lvwDebits_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDebits_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwDebits_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwDebits_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwDebits As System.Windows.Forms.ListView
	Public WithEvents fraDebit As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public cmdNext(0) As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewAllocation))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdReverse = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.fraCredit = New System.Windows.Forms.GroupBox
        Me.lvwCredits = New System.Windows.Forms.ListView
        Me._lvwCredits_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
        Me._lvwCredits_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.fraDebit = New System.Windows.Forms.GroupBox
        Me.lvwDebits = New System.Windows.Forms.ListView
        Me._lvwDebits_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
        Me._lvwDebits_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraCredit.SuspendLayout()
        Me.fraDebit.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(692, 340)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(81, 22)
        Me.cmdClose.TabIndex = 7
        Me.cmdClose.Tag = "CAP;214"
        Me.cmdClose.Text = "*{Close}"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdReverse
        '
        Me.cmdReverse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReverse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReverse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReverse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReverse.Location = New System.Drawing.Point(12, 340)
        Me.cmdReverse.Name = "cmdReverse"
        Me.cmdReverse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReverse.Size = New System.Drawing.Size(81, 22)
        Me.cmdReverse.TabIndex = 6
        Me.cmdReverse.Tag = "CAP;213"
        Me.cmdReverse.Text = "*{Reverse}"
        Me.cmdReverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReverse.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(151, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(765, 329)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraCredit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDebit)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(757, 303)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Allocations"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(740, 288)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(22, 19)
        Me._cmdNext_0.TabIndex = 8
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        Me._cmdNext_0.Visible = False
        '
        'fraCredit
        '
        Me.fraCredit.BackColor = System.Drawing.SystemColors.Control
        Me.fraCredit.Controls.Add(Me.lvwCredits)
        Me.fraCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCredit.Location = New System.Drawing.Point(8, 8)
        Me.fraCredit.Name = "fraCredit"
        Me.fraCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCredit.Size = New System.Drawing.Size(745, 137)
        Me.fraCredit.TabIndex = 1
        Me.fraCredit.TabStop = False
        Me.fraCredit.Tag = "CAP;160"
        Me.fraCredit.Text = "*{Credits}"
        '
        'lvwCredits
        '
        Me.lvwCredits.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCredits, "")
        Me.lvwCredits.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCredits_ColumnHeader_1, Me._lvwCredits_ColumnHeader_2, Me._lvwCredits_ColumnHeader_3, Me._lvwCredits_ColumnHeader_4, Me._lvwCredits_ColumnHeader_5, Me._lvwCredits_ColumnHeader_6, Me._lvwCredits_ColumnHeader_7, Me._lvwCredits_ColumnHeader_8, Me._lvwCredits_ColumnHeader_9, Me._lvwCredits_ColumnHeader_10, Me._lvwCredits_ColumnHeader_11, Me._lvwCredits_ColumnHeader_12, Me._lvwCredits_ColumnHeader_13, Me._lvwCredits_ColumnHeader_14, Me._lvwCredits_ColumnHeader_15})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCredits, True)
        Me.lvwCredits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCredits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCredits.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCredits, "lvwCredits_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCredits, "")
        Me.lvwCredits.Location = New System.Drawing.Point(8, 16)
        Me.lvwCredits.Name = "lvwCredits"
        Me.lvwCredits.Size = New System.Drawing.Size(729, 113)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCredits, "")
        Me.lvwCredits.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwCredits, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCredits, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCredits, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCredits.TabIndex = 2
        Me.lvwCredits.Tag = "CAP;165"
        Me.lvwCredits.UseCompatibleStateImageBehavior = False
        Me.lvwCredits.View = System.Windows.Forms.View.Details
        '
        '_lvwCredits_ColumnHeader_1
        '
        Me._lvwCredits_ColumnHeader_1.Text = "*{Document Ref}"
        Me._lvwCredits_ColumnHeader_1.Width = 97
        '
        '_lvwCredits_ColumnHeader_2
        '
        Me._lvwCredits_ColumnHeader_2.Tag = "HIDDEN"
        Me._lvwCredits_ColumnHeader_2.Text = "*{TransDetailId}"
        Me._lvwCredits_ColumnHeader_2.Width = 0
        '
        '_lvwCredits_ColumnHeader_3
        '
        Me._lvwCredits_ColumnHeader_3.Text = "*{Trans. Date}"
        Me._lvwCredits_ColumnHeader_3.Width = 97
        '
        '_lvwCredits_ColumnHeader_4
        '
        Me._lvwCredits_ColumnHeader_4.Text = "*{Allocated Date}"
        Me._lvwCredits_ColumnHeader_4.Width = 97
        '
        '_lvwCredits_ColumnHeader_5
        '
        Me._lvwCredits_ColumnHeader_5.Text = "*{Allocated Amount}"
        Me._lvwCredits_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCredits_ColumnHeader_5.Width = 97
        '
        '_lvwCredits_ColumnHeader_6
        '
        Me._lvwCredits_ColumnHeader_6.Text = "*{Original Amount}"
        Me._lvwCredits_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCredits_ColumnHeader_6.Width = 97
        '
        '_lvwCredits_ColumnHeader_7
        '
        Me._lvwCredits_ColumnHeader_7.Text = "*{Write Off Amount}"
        Me._lvwCredits_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCredits_ColumnHeader_7.Width = 97
        '
        '_lvwCredits_ColumnHeader_8
        '
        Me._lvwCredits_ColumnHeader_8.Text = "*{Doc Type}"
        Me._lvwCredits_ColumnHeader_8.Width = 97
        '
        '_lvwCredits_ColumnHeader_9
        '
        Me._lvwCredits_ColumnHeader_9.Text = "*{Insurance Ref.}"
        Me._lvwCredits_ColumnHeader_9.Width = 97
        '
        '_lvwCredits_ColumnHeader_10
        '
        Me._lvwCredits_ColumnHeader_10.Text = "*{Account}"
        Me._lvwCredits_ColumnHeader_10.Width = 97
        '
        '_lvwCredits_ColumnHeader_11
        '
        Me._lvwCredits_ColumnHeader_11.Text = "*{User}"
        Me._lvwCredits_ColumnHeader_11.Width = 97
        '
        '_lvwCredits_ColumnHeader_12
        '
        Me._lvwCredits_ColumnHeader_12.Tag = "HIDDEN"
        Me._lvwCredits_ColumnHeader_12.Text = "*{CashListItem)"
        Me._lvwCredits_ColumnHeader_12.Width = 0
        '
        '_lvwCredits_ColumnHeader_13
        '
        Me._lvwCredits_ColumnHeader_13.Tag = "HIDDEN"
        Me._lvwCredits_ColumnHeader_13.Text = "*{Allocation Id}"
        Me._lvwCredits_ColumnHeader_13.Width = 0
        '
        '_lvwCredits_ColumnHeader_14
        '
        Me._lvwCredits_ColumnHeader_14.Tag = "HIDDEN"
        Me._lvwCredits_ColumnHeader_14.Text = "*{AllocationDetail Id}"
        Me._lvwCredits_ColumnHeader_14.Width = 0
        '
        ' _lvwCredits_ColumnHeader_15
        ' 
        Me._lvwCredits_ColumnHeader_15.Text = "*{Round Off Amount}"
        Me._lvwCredits_ColumnHeader_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwCredits_ColumnHeader_15.Width = 97
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.Tag = "Sirius For Broking Rules"
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "check")
        '
        'fraDebit
        '
        Me.fraDebit.BackColor = System.Drawing.SystemColors.Control
        Me.fraDebit.Controls.Add(Me.lvwDebits)
        Me.fraDebit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDebit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDebit.Location = New System.Drawing.Point(8, 156)
        Me.fraDebit.Name = "fraDebit"
        Me.fraDebit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDebit.Size = New System.Drawing.Size(745, 137)
        Me.fraDebit.TabIndex = 3
        Me.fraDebit.TabStop = False
        Me.fraDebit.Tag = "CAP;161"
        Me.fraDebit.Text = "*{Debits}"
        '
        'lvwDebits
        '
        Me.lvwDebits.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwDebits, "")
        Me.lvwDebits.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwDebits_ColumnHeader_1, Me._lvwDebits_ColumnHeader_2, Me._lvwDebits_ColumnHeader_3, Me._lvwDebits_ColumnHeader_4, Me._lvwDebits_ColumnHeader_5, Me._lvwDebits_ColumnHeader_6, Me._lvwDebits_ColumnHeader_7, Me._lvwDebits_ColumnHeader_8, Me._lvwDebits_ColumnHeader_9, Me._lvwDebits_ColumnHeader_10, Me._lvwDebits_ColumnHeader_11, Me._lvwDebits_ColumnHeader_12, Me._lvwDebits_ColumnHeader_13, Me._lvwDebits_ColumnHeader_14, Me._lvwDebits_ColumnHeader_15})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwDebits, True)
        Me.lvwDebits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDebits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwDebits.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwDebits, "lvwDebits_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwDebits, "")
        Me.lvwDebits.Location = New System.Drawing.Point(8, 18)
        Me.lvwDebits.Name = "lvwDebits"
        Me.lvwDebits.Size = New System.Drawing.Size(729, 113)
        Me.listViewHelper1.SetSmallIcons(Me.lvwDebits, "")
        Me.lvwDebits.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwDebits, False)
        Me.listViewHelper1.SetSortKey(Me.lvwDebits, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwDebits, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwDebits.TabIndex = 4
        Me.lvwDebits.Tag = "CAP;165"
        Me.lvwDebits.UseCompatibleStateImageBehavior = False
        Me.lvwDebits.View = System.Windows.Forms.View.Details
        '
        '_lvwDebits_ColumnHeader_1
        '
        Me._lvwDebits_ColumnHeader_1.Text = "*{Document Ref}"
        Me._lvwDebits_ColumnHeader_1.Width = 97
        '
        '_lvwDebits_ColumnHeader_2
        '
        Me._lvwDebits_ColumnHeader_2.Tag = "HIDDEN"
        Me._lvwDebits_ColumnHeader_2.Text = "*{TransDetailId}"
        Me._lvwDebits_ColumnHeader_2.Width = 0
        '
        '_lvwDebits_ColumnHeader_3
        '
        Me._lvwDebits_ColumnHeader_3.Text = "*{Trans. Date}"
        Me._lvwDebits_ColumnHeader_3.Width = 97
        '
        '_lvwDebits_ColumnHeader_4
        '
        Me._lvwDebits_ColumnHeader_4.Text = "*{Allocated Date}"
        Me._lvwDebits_ColumnHeader_4.Width = 97
        '
        '_lvwDebits_ColumnHeader_5
        '
        Me._lvwDebits_ColumnHeader_5.Text = "*{Allocated Amount}"
        Me._lvwDebits_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwDebits_ColumnHeader_5.Width = 97
        '
        '_lvwDebits_ColumnHeader_6
        '
        Me._lvwDebits_ColumnHeader_6.Text = "*{Original Amount}"
        Me._lvwDebits_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwDebits_ColumnHeader_6.Width = 97
        '
        '_lvwDebits_ColumnHeader_7
        '
        Me._lvwDebits_ColumnHeader_7.Text = "*{Write Off Amount}"
        Me._lvwDebits_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwDebits_ColumnHeader_7.Width = 97
        '
        '_lvwDebits_ColumnHeader_8
        '
        Me._lvwDebits_ColumnHeader_8.Text = "*{Doc Type}"
        Me._lvwDebits_ColumnHeader_8.Width = 97
        '
        '_lvwDebits_ColumnHeader_9
        '
        Me._lvwDebits_ColumnHeader_9.Text = "*{Insurance Ref.}"
        Me._lvwDebits_ColumnHeader_9.Width = 97
        '
        '_lvwDebits_ColumnHeader_10
        '
        Me._lvwDebits_ColumnHeader_10.Text = "*{Account)"
        Me._lvwDebits_ColumnHeader_10.Width = 97
        '
        '_lvwDebits_ColumnHeader_11
        '
        Me._lvwDebits_ColumnHeader_11.Text = "*{User}"
        Me._lvwDebits_ColumnHeader_11.Width = 97
        '
        '_lvwDebits_ColumnHeader_12
        '
        Me._lvwDebits_ColumnHeader_12.Tag = "HIDDEN"
        Me._lvwDebits_ColumnHeader_12.Text = "*{CashListItem}"
        Me._lvwDebits_ColumnHeader_12.Width = 0
        '
        '_lvwDebits_ColumnHeader_13
        '
        Me._lvwDebits_ColumnHeader_13.Tag = "HIDDEN"
        Me._lvwDebits_ColumnHeader_13.Text = "*{Allocation Id}"
        Me._lvwDebits_ColumnHeader_13.Width = 0
        '
        '_lvwDebits_ColumnHeader_14
        '
        Me._lvwDebits_ColumnHeader_14.Tag = "HIDDEN"
        Me._lvwDebits_ColumnHeader_14.Text = "*{AllocationDetail Id}"
        Me._lvwDebits_ColumnHeader_14.Width = 0
        '_lvwDebits_ColumnHeader_15
        '
        Me._lvwDebits_ColumnHeader_15.Text = "*{Round Off Amount}"
        Me._lvwDebits_ColumnHeader_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwDebits_ColumnHeader_15.Width = 169
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(-8, 328)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 5
        Me.cmdNavigate.Tag = "CAP;203"
        Me.cmdNavigate.Text = "*{Navigate}"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'frmViewAllocation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(783, 372)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdReverse)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmViewAllocation"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "View Allocation"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraCredit.ResumeLayout(False)
        Me.fraDebit.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdNext()
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class