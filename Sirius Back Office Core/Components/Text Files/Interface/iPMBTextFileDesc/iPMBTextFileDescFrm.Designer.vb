<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwClaim_InitializeColumnKeys()
		lvwPolicy_InitializeColumnKeys()
		lvwClient_InitializeColumnKeys()
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lvwClient_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClient_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClient As System.Windows.Forms.ListView
	Public WithEvents cmdClientEdit As System.Windows.Forms.Button
	Public WithEvents cmdClientAdd As System.Windows.Forms.Button
	Public WithEvents fraClient As System.Windows.Forms.GroupBox
	Public WithEvents cmdPolicyAdd As System.Windows.Forms.Button
	Public WithEvents cmdPolicyEdit As System.Windows.Forms.Button
	Private WithEvents _lvwPolicy_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPolicy_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPolicy As System.Windows.Forms.ListView
	Public WithEvents fraPolicy As System.Windows.Forms.GroupBox
	Public WithEvents cmdClaimEdit As System.Windows.Forms.Button
	Public WithEvents cmdClaimAdd As System.Windows.Forms.Button
	Private WithEvents _lvwClaim_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClaim_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClaim As System.Windows.Forms.ListView
	Public WithEvents fraClaim As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraClient = New System.Windows.Forms.GroupBox
        Me.lvwClient = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._lvwClient_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwClient_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.cmdClientEdit = New System.Windows.Forms.Button
        Me.cmdClientAdd = New System.Windows.Forms.Button
        Me.fraPolicy = New System.Windows.Forms.GroupBox
        Me.cmdPolicyAdd = New System.Windows.Forms.Button
        Me.cmdPolicyEdit = New System.Windows.Forms.Button
        Me.lvwPolicy = New System.Windows.Forms.ListView
        Me._lvwPolicy_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPolicy_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.fraClaim = New System.Windows.Forms.GroupBox
        Me.cmdClaimEdit = New System.Windows.Forms.Button
        Me.cmdClaimAdd = New System.Windows.Forms.Button
        Me.lvwClaim = New System.Windows.Forms.ListView
        Me._lvwClaim_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwClaim_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraClient.SuspendLayout()
        Me.fraPolicy.SuspendLayout()
        Me.fraClaim.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 440)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 0
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(632, 440)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(552, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(472, 440)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(138, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(701, 437)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraClaim)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(693, 411)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Text Files"
        '
        'fraClient
        '
        Me.fraClient.BackColor = System.Drawing.SystemColors.Control
        Me.fraClient.Controls.Add(Me.lvwClient)
        Me.fraClient.Controls.Add(Me.cmdClientEdit)
        Me.fraClient.Controls.Add(Me.cmdClientAdd)
        Me.fraClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClient.Location = New System.Drawing.Point(8, 52)
        Me.fraClient.Name = "fraClient"
        Me.fraClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClient.Size = New System.Drawing.Size(681, 113)
        Me.fraClient.TabIndex = 5
        Me.fraClient.TabStop = False
        Me.fraClient.Text = "Client Text Files"
        '
        'lvwClient
        '
        Me.lvwClient.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClient.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClient, "")
        Me.lvwClient.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClient_ColumnHeader_1, Me._lvwClient_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClient, False)
        Me.lvwClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClient.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClient, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClient, "")
        Me.lvwClient.LargeImageList = Me.ImageList1
        Me.lvwClient.Location = New System.Drawing.Point(16, 24)
        Me.lvwClient.Name = "lvwClient"
        Me.lvwClient.Size = New System.Drawing.Size(569, 81)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClient, "")
        Me.lvwClient.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwClient, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClient, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClient, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClient.TabIndex = 6
        Me.lvwClient.UseCompatibleStateImageBehavior = False
        Me.lvwClient.View = System.Windows.Forms.View.Details
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.SystemColors.Window
        Me.ImageList1.Images.SetKeyName(0, "Textfile")
        '
        '_lvwClient_ColumnHeader_1
        '
        Me._lvwClient_ColumnHeader_1.Tag = ""
        Me._lvwClient_ColumnHeader_1.Text = "Slot"
        Me._lvwClient_ColumnHeader_1.Width = 97
        '
        '_lvwClient_ColumnHeader_2
        '
        Me._lvwClient_ColumnHeader_2.Tag = ""
        Me._lvwClient_ColumnHeader_2.Text = "Description"
        Me._lvwClient_ColumnHeader_2.Width = 385
        '
        'cmdClientEdit
        '
        Me.cmdClientEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClientEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClientEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClientEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClientEdit.Location = New System.Drawing.Point(600, 72)
        Me.cmdClientEdit.Name = "cmdClientEdit"
        Me.cmdClientEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClientEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdClientEdit.TabIndex = 8
        Me.cmdClientEdit.Text = "Edit"
        Me.cmdClientEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClientEdit.UseVisualStyleBackColor = False
        '
        'cmdClientAdd
        '
        Me.cmdClientAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClientAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClientAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClientAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClientAdd.Location = New System.Drawing.Point(600, 40)
        Me.cmdClientAdd.Name = "cmdClientAdd"
        Me.cmdClientAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClientAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdClientAdd.TabIndex = 7
        Me.cmdClientAdd.Text = "Add"
        Me.cmdClientAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClientAdd.UseVisualStyleBackColor = False
        '
        'fraPolicy
        '
        Me.fraPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicy.Controls.Add(Me.cmdPolicyAdd)
        Me.fraPolicy.Controls.Add(Me.cmdPolicyEdit)
        Me.fraPolicy.Controls.Add(Me.lvwPolicy)
        Me.fraPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicy.Location = New System.Drawing.Point(8, 172)
        Me.fraPolicy.Name = "fraPolicy"
        Me.fraPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicy.Size = New System.Drawing.Size(681, 113)
        Me.fraPolicy.TabIndex = 9
        Me.fraPolicy.TabStop = False
        Me.fraPolicy.Text = "Policy Text Files"
        '
        'cmdPolicyAdd
        '
        Me.cmdPolicyAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicyAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicyAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicyAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicyAdd.Location = New System.Drawing.Point(600, 40)
        Me.cmdPolicyAdd.Name = "cmdPolicyAdd"
        Me.cmdPolicyAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicyAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdPolicyAdd.TabIndex = 11
        Me.cmdPolicyAdd.Text = "Add"
        Me.cmdPolicyAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicyAdd.UseVisualStyleBackColor = False
        '
        'cmdPolicyEdit
        '
        Me.cmdPolicyEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicyEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicyEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicyEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicyEdit.Location = New System.Drawing.Point(600, 72)
        Me.cmdPolicyEdit.Name = "cmdPolicyEdit"
        Me.cmdPolicyEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicyEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdPolicyEdit.TabIndex = 10
        Me.cmdPolicyEdit.Text = "Edit"
        Me.cmdPolicyEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicyEdit.UseVisualStyleBackColor = False
        '
        'lvwPolicy
        '
        Me.lvwPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicy.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicy, "")
        Me.lvwPolicy.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicy_ColumnHeader_1, Me._lvwPolicy_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicy, False)
        Me.lvwPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicy.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicy, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicy, "")
        Me.lvwPolicy.LargeImageList = Me.ImageList1
        Me.lvwPolicy.Location = New System.Drawing.Point(16, 24)
        Me.lvwPolicy.Name = "lvwPolicy"
        Me.lvwPolicy.Size = New System.Drawing.Size(569, 81)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicy, "")
        Me.lvwPolicy.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwPolicy, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicy, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicy, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicy.TabIndex = 15
        Me.lvwPolicy.UseCompatibleStateImageBehavior = False
        Me.lvwPolicy.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicy_ColumnHeader_1
        '
        Me._lvwPolicy_ColumnHeader_1.Tag = ""
        Me._lvwPolicy_ColumnHeader_1.Text = "Slot"
        Me._lvwPolicy_ColumnHeader_1.Width = 97
        '
        '_lvwPolicy_ColumnHeader_2
        '
        Me._lvwPolicy_ColumnHeader_2.Tag = ""
        Me._lvwPolicy_ColumnHeader_2.Text = "Description"
        Me._lvwPolicy_ColumnHeader_2.Width = 385
        '
        'fraClaim
        '
        Me.fraClaim.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaim.Controls.Add(Me.cmdClaimEdit)
        Me.fraClaim.Controls.Add(Me.cmdClaimAdd)
        Me.fraClaim.Controls.Add(Me.lvwClaim)
        Me.fraClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaim.Location = New System.Drawing.Point(8, 292)
        Me.fraClaim.Name = "fraClaim"
        Me.fraClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaim.Size = New System.Drawing.Size(681, 113)
        Me.fraClaim.TabIndex = 12
        Me.fraClaim.TabStop = False
        Me.fraClaim.Text = "Claim Text Files"
        '
        'cmdClaimEdit
        '
        Me.cmdClaimEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClaimEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClaimEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClaimEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClaimEdit.Location = New System.Drawing.Point(600, 72)
        Me.cmdClaimEdit.Name = "cmdClaimEdit"
        Me.cmdClaimEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClaimEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdClaimEdit.TabIndex = 14
        Me.cmdClaimEdit.Text = "Edit"
        Me.cmdClaimEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClaimEdit.UseVisualStyleBackColor = False
        '
        'cmdClaimAdd
        '
        Me.cmdClaimAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClaimAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClaimAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClaimAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClaimAdd.Location = New System.Drawing.Point(600, 40)
        Me.cmdClaimAdd.Name = "cmdClaimAdd"
        Me.cmdClaimAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClaimAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdClaimAdd.TabIndex = 13
        Me.cmdClaimAdd.Text = "Add"
        Me.cmdClaimAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClaimAdd.UseVisualStyleBackColor = False
        '
        'lvwClaim
        '
        Me.lvwClaim.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClaim.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClaim, "")
        Me.lvwClaim.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClaim_ColumnHeader_1, Me._lvwClaim_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClaim, False)
        Me.lvwClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClaim.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClaim, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClaim, "")
        Me.lvwClaim.LargeImageList = Me.ImageList1
        Me.lvwClaim.Location = New System.Drawing.Point(16, 24)
        Me.lvwClaim.Name = "lvwClaim"
        Me.lvwClaim.Size = New System.Drawing.Size(569, 81)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClaim, "")
        Me.lvwClaim.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwClaim, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClaim, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClaim, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClaim.TabIndex = 16
        Me.lvwClaim.UseCompatibleStateImageBehavior = False
        Me.lvwClaim.View = System.Windows.Forms.View.Details
        '
        '_lvwClaim_ColumnHeader_1
        '
        Me._lvwClaim_ColumnHeader_1.Tag = ""
        Me._lvwClaim_ColumnHeader_1.Text = "Slot"
        Me._lvwClaim_ColumnHeader_1.Width = 97
        '
        '_lvwClaim_ColumnHeader_2
        '
        Me._lvwClaim_ColumnHeader_2.Tag = ""
        Me._lvwClaim_ColumnHeader_2.Text = "Description"
        Me._lvwClaim_ColumnHeader_2.Width = 385
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(713, 469)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Text Files"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraClient.ResumeLayout(False)
        Me.fraPolicy.ResumeLayout(False)
        Me.fraClaim.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwClaim_InitializeColumnKeys()
		Me._lvwClaim_ColumnHeader_1.Name = ""
		Me._lvwClaim_ColumnHeader_2.Name = ""
	End Sub
	Sub lvwPolicy_InitializeColumnKeys()
		Me._lvwPolicy_ColumnHeader_1.Name = ""
		Me._lvwPolicy_ColumnHeader_2.Name = ""
	End Sub
	Sub lvwClient_InitializeColumnKeys()
		Me._lvwClient_ColumnHeader_1.Name = ""
		Me._lvwClient_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class