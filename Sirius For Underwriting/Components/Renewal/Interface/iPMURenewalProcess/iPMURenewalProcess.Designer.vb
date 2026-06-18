<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRenewalProcess
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
    Private Sub Ctx_mnuRenewalProcess_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuRenewalProcess.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuRenewalProcess.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuRenewalProcess.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuRenewalProcess.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuRenewalProcess_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuRenewalProcess.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuRenewalProcess.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuRenewalProcess.DropDownItems.Add(item)
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
    Public WithEvents mnuRenewalSearchRenewalPolicies As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalSearchPolicyList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessSearch As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessAmend As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessInvite As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessAccept As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessSetStatus As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessSep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuRenewalProcessDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessLapse As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessSep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuRenewalProcessTransfer As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessSep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuRenewalProcessSelectAll As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcessUnselectAll As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRenewalProcess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdAccept As System.Windows.Forms.Button
    Public WithEvents cmdStatus As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdLapse As System.Windows.Forms.Button
    Public WithEvents cmdAmend As System.Windows.Forms.Button
    Public WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Public WithEvents imgRenewalProcess As System.Windows.Forms.ImageList
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents _lvwRenewalProcess_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalProcess_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRenewalProcess As System.Windows.Forms.ListView
    Public WithEvents Message As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Counter As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbMain As System.Windows.Forms.StatusStrip
    Public WithEvents Ctx_mnuRenewalProcess As System.Windows.Forms.ContextMenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRenewalProcess))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuRenewalProcessSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalSearchRenewalPolicies = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalSearchPolicyList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessAmend = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessInvite = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessWrite = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessAccept = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessSetStatus = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRenewalProcessDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessLapse = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRenewalProcessTransfer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessSep3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRenewalProcessSelectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRenewalProcessUnselectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdAccept = New System.Windows.Forms.Button
        Me.cmdStatus = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdLapse = New System.Windows.Forms.Button
        Me.cmdAmend = New System.Windows.Forms.Button
        Me.uctAnchor = New uSIRCommonControls.uctAnchor
        Me.imgRenewalProcess = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lvwRenewalProcess = New System.Windows.Forms.ListView
        Me._lvwRenewalProcess_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewalProcess_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me.stbMain = New System.Windows.Forms.StatusStrip
        Me.Message = New System.Windows.Forms.ToolStripStatusLabel
        Me.Counter = New System.Windows.Forms.ToolStripStatusLabel
        Me.Ctx_mnuRenewalProcess = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.stbMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRenewalProcessSearch, Me.mnuRenewalProcess})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(843, 24)
        Me.MainMenu1.TabIndex = 9
        '
        'mnuRenewalProcessSearch
        '
        Me.mnuRenewalProcessSearch.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRenewalSearchRenewalPolicies, Me.mnuRenewalSearchPolicyList})
        Me.mnuRenewalProcessSearch.Name = "mnuRenewalProcessSearch"
        Me.mnuRenewalProcessSearch.Size = New System.Drawing.Size(52, 20)
        Me.mnuRenewalProcessSearch.Text = "Search"
        '
        'mnuRenewalSearchRenewalPolicies
        '
        Me.mnuRenewalSearchRenewalPolicies.Name = "mnuRenewalSearchRenewalPolicies"
        Me.mnuRenewalSearchRenewalPolicies.Size = New System.Drawing.Size(152, 22)
        Me.mnuRenewalSearchRenewalPolicies.Text = "Renewal Policies"
        '
        'mnuRenewalSearchPolicyList
        '
        Me.mnuRenewalSearchPolicyList.Name = "mnuRenewalSearchPolicyList"
        Me.mnuRenewalSearchPolicyList.Size = New System.Drawing.Size(152, 22)
        Me.mnuRenewalSearchPolicyList.Text = "Policy List"
        '
        'mnuRenewalProcess
        '
        Me.mnuRenewalProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRenewalProcessAmend, Me.mnuRenewalProcessInvite, Me.mnuRenewalProcessWrite, Me.mnuRenewalProcessAccept, Me.mnuRenewalProcessSetStatus, Me.mnuRenewalProcessSep1, Me.mnuRenewalProcessDelete, Me.mnuRenewalProcessLapse, Me.mnuRenewalProcessSep2, Me.mnuRenewalProcessTransfer, Me.mnuRenewalProcessSep3, Me.mnuRenewalProcessSelectAll, Me.mnuRenewalProcessUnselectAll})
        Me.mnuRenewalProcess.Name = "mnuRenewalProcess"
        Me.mnuRenewalProcess.Size = New System.Drawing.Size(100, 20)
        Me.mnuRenewalProcess.Text = "Renewal Process"
        '
        'mnuRenewalProcessAmend
        '
        Me.mnuRenewalProcessAmend.Name = "mnuRenewalProcessAmend"
        Me.mnuRenewalProcessAmend.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessAmend.Text = "Amend"
        '
        'mnuRenewalProcessInvite
        '
        Me.mnuRenewalProcessInvite.Name = "mnuRenewalProcessInvite"
        Me.mnuRenewalProcessInvite.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessInvite.Text = "Invite"
        '
        'mnuRenewalProcessWrite
        '
        Me.mnuRenewalProcessWrite.Name = "mnuRenewalProcessWrite"
        Me.mnuRenewalProcessWrite.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessWrite.Text = "Written"
        '
        'mnuRenewalProcessAccept
        '
        Me.mnuRenewalProcessAccept.Name = "mnuRenewalProcessAccept"
        Me.mnuRenewalProcessAccept.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessAccept.Text = "Accept"
        '
        'mnuRenewalProcessSetStatus
        '
        Me.mnuRenewalProcessSetStatus.Name = "mnuRenewalProcessSetStatus"
        Me.mnuRenewalProcessSetStatus.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessSetStatus.Text = "Set Status"
        '
        'mnuRenewalProcessSep1
        '
        Me.mnuRenewalProcessSep1.Name = "mnuRenewalProcessSep1"
        Me.mnuRenewalProcessSep1.Size = New System.Drawing.Size(126, 6)
        '
        'mnuRenewalProcessDelete
        '
        Me.mnuRenewalProcessDelete.Name = "mnuRenewalProcessDelete"
        Me.mnuRenewalProcessDelete.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessDelete.Text = "Delete"
        '
        'mnuRenewalProcessLapse
        '
        Me.mnuRenewalProcessLapse.Name = "mnuRenewalProcessLapse"
        Me.mnuRenewalProcessLapse.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessLapse.Text = "Lapse"
        '
        'mnuRenewalProcessSep2
        '
        Me.mnuRenewalProcessSep2.Name = "mnuRenewalProcessSep2"
        Me.mnuRenewalProcessSep2.Size = New System.Drawing.Size(126, 6)
        '
        'mnuRenewalProcessTransfer
        '
        Me.mnuRenewalProcessTransfer.Name = "mnuRenewalProcessTransfer"
        Me.mnuRenewalProcessTransfer.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessTransfer.Text = "Transfer"
        '
        'mnuRenewalProcessSep3
        '
        Me.mnuRenewalProcessSep3.Name = "mnuRenewalProcessSep3"
        Me.mnuRenewalProcessSep3.Size = New System.Drawing.Size(126, 6)
        '
        'mnuRenewalProcessSelectAll
        '
        Me.mnuRenewalProcessSelectAll.Name = "mnuRenewalProcessSelectAll"
        Me.mnuRenewalProcessSelectAll.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessSelectAll.Text = "Select All"
        '
        'mnuRenewalProcessUnselectAll
        '
        Me.mnuRenewalProcessUnselectAll.Name = "mnuRenewalProcessUnselectAll"
        Me.mnuRenewalProcessUnselectAll.Size = New System.Drawing.Size(129, 22)
        Me.mnuRenewalProcessUnselectAll.Text = "Unselect All"
        '
        'cmdAccept
        '
        Me.cmdAccept.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccept.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccept.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccept.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccept.Location = New System.Drawing.Point(330, 477)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccept.Size = New System.Drawing.Size(73, 22)
        Me.cmdAccept.TabIndex = 7
        Me.cmdAccept.Text = "&Accept"
        Me.cmdAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccept.UseVisualStyleBackColor = False
        Me.cmdAccept.Visible = False
        '
        'cmdStatus
        '
        Me.cmdStatus.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStatus.Location = New System.Drawing.Point(249, 477)
        Me.cmdStatus.Name = "cmdStatus"
        Me.cmdStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStatus.Size = New System.Drawing.Size(73, 22)
        Me.cmdStatus.TabIndex = 6
        Me.cmdStatus.Text = "Stat&us"
        Me.cmdStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdStatus.UseVisualStyleBackColor = False
        Me.cmdStatus.Visible = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(168, 477)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 5
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        Me.cmdDelete.Visible = False
        '
        'cmdLapse
        '
        Me.cmdLapse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLapse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLapse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLapse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLapse.Location = New System.Drawing.Point(87, 477)
        Me.cmdLapse.Name = "cmdLapse"
        Me.cmdLapse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLapse.Size = New System.Drawing.Size(73, 22)
        Me.cmdLapse.TabIndex = 4
        Me.cmdLapse.Text = "&Lapse"
        Me.cmdLapse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLapse.UseVisualStyleBackColor = False
        Me.cmdLapse.Visible = False
        '
        'cmdAmend
        '
        Me.cmdAmend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAmend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAmend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAmend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAmend.Location = New System.Drawing.Point(6, 477)
        Me.cmdAmend.Name = "cmdAmend"
        Me.cmdAmend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAmend.Size = New System.Drawing.Size(73, 22)
        Me.cmdAmend.TabIndex = 3
        Me.cmdAmend.Text = "A&mend"
        Me.cmdAmend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAmend.UseVisualStyleBackColor = False
        Me.cmdAmend.Visible = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(39, 492)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 8
        Me.uctAnchor.Visible = False
        '
        'imgRenewalProcess
        '
        Me.imgRenewalProcess.ImageStream = CType(resources.GetObject("imgRenewalProcess.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgRenewalProcess.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgRenewalProcess.Images.SetKeyName(0, "Amend")
        Me.imgRenewalProcess.Images.SetKeyName(1, "Accept")
        Me.imgRenewalProcess.Images.SetKeyName(2, "Invite")
        Me.imgRenewalProcess.Images.SetKeyName(3, "Transfer")
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(762, 477)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lvwRenewalProcess
        '
        Me.lvwRenewalProcess.AllowColumnReorder = True
        Me.lvwRenewalProcess.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRenewalProcess.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRenewalProcess.CheckBoxes = True
        Me.lvwRenewalProcess.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRenewalProcess_ColumnHeader_1, Me._lvwRenewalProcess_ColumnHeader_2, Me._lvwRenewalProcess_ColumnHeader_3, Me._lvwRenewalProcess_ColumnHeader_4, Me._lvwRenewalProcess_ColumnHeader_5, Me._lvwRenewalProcess_ColumnHeader_6, Me._lvwRenewalProcess_ColumnHeader_7, Me._lvwRenewalProcess_ColumnHeader_8, Me._lvwRenewalProcess_ColumnHeader_9, Me._lvwRenewalProcess_ColumnHeader_10, Me._lvwRenewalProcess_ColumnHeader_11, Me._lvwRenewalProcess_ColumnHeader_12})
        Me.lvwRenewalProcess.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRenewalProcess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRenewalProcess.FullRowSelect = True
        Me.lvwRenewalProcess.GridLines = True
        Me.lvwRenewalProcess.HideSelection = False
        Me.lvwRenewalProcess.Location = New System.Drawing.Point(6, 27)
        Me.lvwRenewalProcess.Name = "lvwRenewalProcess"
        Me.lvwRenewalProcess.Size = New System.Drawing.Size(829, 439)
        Me.lvwRenewalProcess.SmallImageList = Me.imgRenewalProcess
        Me.lvwRenewalProcess.TabIndex = 0
        Me.lvwRenewalProcess.UseCompatibleStateImageBehavior = False
        Me.lvwRenewalProcess.View = System.Windows.Forms.View.Details
        '
        '_lvwRenewalProcess_ColumnHeader_1
        '
        Me._lvwRenewalProcess_ColumnHeader_1.Text = "Branch"
        Me._lvwRenewalProcess_ColumnHeader_1.Width = 100
        '
        '_lvwRenewalProcess_ColumnHeader_2
        '
        Me._lvwRenewalProcess_ColumnHeader_2.Text = "Client Code"
        Me._lvwRenewalProcess_ColumnHeader_2.Width = 97
        '
        '_lvwRenewalProcess_ColumnHeader_3
        '
        Me._lvwRenewalProcess_ColumnHeader_3.Text = "Client Name"
        Me._lvwRenewalProcess_ColumnHeader_3.Width = 97
        '
        '_lvwRenewalProcess_ColumnHeader_4
        '
        Me._lvwRenewalProcess_ColumnHeader_4.Text = "Policy Number"
        Me._lvwRenewalProcess_ColumnHeader_4.Width = 134
        '
        '_lvwRenewalProcess_ColumnHeader_5
        '
        Me._lvwRenewalProcess_ColumnHeader_5.Text = "Agent"
        Me._lvwRenewalProcess_ColumnHeader_5.Width = 97
        '
        '_lvwRenewalProcess_ColumnHeader_6
        '
        Me._lvwRenewalProcess_ColumnHeader_6.Text = "Agent Name"
        Me._lvwRenewalProcess_ColumnHeader_6.Width = 97
        '
        '_lvwRenewalProcess_ColumnHeader_7
        '
        Me._lvwRenewalProcess_ColumnHeader_7.Text = "Acc. Handler"
        Me._lvwRenewalProcess_ColumnHeader_7.Width = 97
        '
        '_lvwRenewalProcess_ColumnHeader_8
        '
        Me._lvwRenewalProcess_ColumnHeader_8.Tag = "DateColumn"
        Me._lvwRenewalProcess_ColumnHeader_8.Text = "Renewal Date"
        Me._lvwRenewalProcess_ColumnHeader_8.Width = 108
        '
        '_lvwRenewalProcess_ColumnHeader_9
        '
        Me._lvwRenewalProcess_ColumnHeader_9.Text = "Status"
        Me._lvwRenewalProcess_ColumnHeader_9.Width = 167
        '
        '_lvwRenewalProcess_ColumnHeader_10
        '
        Me._lvwRenewalProcess_ColumnHeader_10.Text = "Product"
        Me._lvwRenewalProcess_ColumnHeader_10.Width = 201
        '
        '_lvwRenewalProcess_ColumnHeader_11
        '
        Me._lvwRenewalProcess_ColumnHeader_11.Text = "Claims Indicator"
        Me._lvwRenewalProcess_ColumnHeader_11.Width = 101
        '
        '_lvwRenewalProcess_ColumnHeader_12
        '
        Me._lvwRenewalProcess_ColumnHeader_12.Text = "Transfer To"
        Me._lvwRenewalProcess_ColumnHeader_12.Width = 97
        '
        'stbMain
        '
        Me.stbMain.AllowMerge = False
        Me.stbMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.stbMain.AutoSize = False
        Me.stbMain.Dock = System.Windows.Forms.DockStyle.None
        Me.stbMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Message, Me.Counter})
        Me.stbMain.Location = New System.Drawing.Point(0, 505)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = True
        Me.stbMain.Size = New System.Drawing.Size(843, 25)
        Me.stbMain.SizingGrip = False
        Me.stbMain.Stretch = False
        Me.stbMain.TabIndex = 2
        '
        'Message
        '
        Me.Message.AutoSize = False
        Me.Message.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Message.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Message.DoubleClickEnabled = True
        Me.Message.Margin = New System.Windows.Forms.Padding(0)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(300, 25)
        Me.Message.Text = "Ready"
        Me.Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Counter
        '
        Me.Counter.AutoSize = False
        Me.Counter.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Counter.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Counter.DoubleClickEnabled = True
        Me.Counter.Margin = New System.Windows.Forms.Padding(0)
        Me.Counter.Name = "Counter"
        Me.Counter.Size = New System.Drawing.Size(96, 25)
        Me.Counter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Ctx_mnuRenewalProcess
        '
        Me.Ctx_mnuRenewalProcess.Name = "Ctx_mnuRenewalProcess"
        Me.Ctx_mnuRenewalProcess.Size = New System.Drawing.Size(61, 4)
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(409, 477)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(73, 22)
        Me.cmdWrite.TabIndex = 10
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdWrite.UseVisualStyleBackColor = False
        Me.cmdWrite.Visible = False
        '
        'frmRenewalProcess
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(843, 530)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdAccept)
        Me.Controls.Add(Me.cmdStatus)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdLapse)
        Me.Controls.Add(Me.cmdAmend)
        Me.Controls.Add(Me.uctAnchor)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lvwRenewalProcess)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(11, 30)
        Me.Name = "frmRenewalProcess"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Renewal Process"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.stbMain.ResumeLayout(False)
        Me.stbMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdWrite As System.Windows.Forms.Button
    Friend WithEvents mnuRenewalProcessWrite As System.Windows.Forms.ToolStripMenuItem
#End Region
End Class