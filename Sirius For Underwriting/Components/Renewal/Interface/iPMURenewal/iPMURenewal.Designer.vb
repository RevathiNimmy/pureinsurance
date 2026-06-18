<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRenewal
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwRenewals_InitializeColumnKeys()
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
    Public WithEvents mnuSearch As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents cmdTransfer As System.Windows.Forms.Button
    Public WithEvents cmdChangeStatus As System.Windows.Forms.Button
    Public WithEvents cmdFilter As System.Windows.Forms.Button
    Public WithEvents cmdSelectAll As System.Windows.Forms.Button
    Public WithEvents cmdAccept As System.Windows.Forms.Button
    Private WithEvents _lvwRenewals_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewals_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRenewals As System.Windows.Forms.ListView
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdLapse As System.Windows.Forms.Button
    Public WithEvents cmdAmmend As System.Windows.Forms.Button
    Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRenewal))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdTransfer = New System.Windows.Forms.Button
        Me.cmdChangeStatus = New System.Windows.Forms.Button
        Me.cmdFilter = New System.Windows.Forms.Button
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.cmdAccept = New System.Windows.Forms.Button
        Me.lvwRenewals = New System.Windows.Forms.ListView
        Me._lvwRenewals_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwRenewals_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdLapse = New System.Windows.Forms.Button
        Me.cmdAmmend = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me.MainMenu1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.stbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSearch})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(839, 24)
        Me.MainMenu1.TabIndex = 11
        '
        'mnuSearch
        '
        Me.mnuSearch.Name = "mnuSearch"
        Me.mnuSearch.Size = New System.Drawing.Size(52, 20)
        Me.mnuSearch.Text = "Search"
        '
        'cmdTransfer
        '
        Me.cmdTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransfer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransfer.Location = New System.Drawing.Point(6, 448)
        Me.cmdTransfer.Name = "cmdTransfer"
        Me.cmdTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransfer.Size = New System.Drawing.Size(73, 22)
        Me.cmdTransfer.TabIndex = 10
        Me.cmdTransfer.Text = "&Transfer"
        Me.cmdTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTransfer.UseVisualStyleBackColor = False
        Me.cmdTransfer.Visible = False
        '
        'cmdChangeStatus
        '
        Me.cmdChangeStatus.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangeStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangeStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangeStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangeStatus.Location = New System.Drawing.Point(329, 448)
        Me.cmdChangeStatus.Name = "cmdChangeStatus"
        Me.cmdChangeStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangeStatus.Size = New System.Drawing.Size(73, 22)
        Me.cmdChangeStatus.TabIndex = 9
        Me.cmdChangeStatus.Text = "Stat&us"
        Me.cmdChangeStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangeStatus.UseVisualStyleBackColor = False
        Me.cmdChangeStatus.Visible = False
        '
        'cmdFilter
        '
        Me.cmdFilter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFilter.Location = New System.Drawing.Point(249, 474)
        Me.cmdFilter.Name = "cmdFilter"
        Me.cmdFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFilter.Size = New System.Drawing.Size(73, 22)
        Me.cmdFilter.TabIndex = 8
        Me.cmdFilter.Text = "&Filter"
        Me.cmdFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFilter.UseVisualStyleBackColor = False
        Me.cmdFilter.Visible = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(167, 474)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelectAll.TabIndex = 7
        Me.cmdSelectAll.Text = "&Select All"
        Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        Me.cmdSelectAll.Visible = False
        '
        'cmdAccept
        '
        Me.cmdAccept.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccept.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccept.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccept.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccept.Location = New System.Drawing.Point(86, 471)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccept.Size = New System.Drawing.Size(73, 22)
        Me.cmdAccept.TabIndex = 6
        Me.cmdAccept.Text = "&Accept"
        Me.cmdAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccept.UseVisualStyleBackColor = False
        Me.cmdAccept.Visible = False
        '
        'lvwRenewals
        '
        Me.lvwRenewals.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRenewals.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRenewals, "")
        Me.lvwRenewals.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRenewals_ColumnHeader_1, Me._lvwRenewals_ColumnHeader_2, Me._lvwRenewals_ColumnHeader_3, Me._lvwRenewals_ColumnHeader_4, Me._lvwRenewals_ColumnHeader_5, Me._lvwRenewals_ColumnHeader_6, Me._lvwRenewals_ColumnHeader_7, Me._lvwRenewals_ColumnHeader_8, Me._lvwRenewals_ColumnHeader_9, Me._lvwRenewals_ColumnHeader_10, Me._lvwRenewals_ColumnHeader_11, Me._lvwRenewals_ColumnHeader_12})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRenewals, True)
        Me.lvwRenewals.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRenewals.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRenewals.FullRowSelect = True
        Me.lvwRenewals.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRenewals, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRenewals, "")
        Me.lvwRenewals.LargeImageList = Me.ImageList1
        Me.lvwRenewals.Location = New System.Drawing.Point(6, 34)
        Me.lvwRenewals.Name = "lvwRenewals"
        Me.lvwRenewals.Size = New System.Drawing.Size(827, 407)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRenewals, "")
        Me.lvwRenewals.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwRenewals, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRenewals, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRenewals, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRenewals.TabIndex = 0
        Me.lvwRenewals.UseCompatibleStateImageBehavior = False
        Me.lvwRenewals.View = System.Windows.Forms.View.Details
        '
        '_lvwRenewals_ColumnHeader_1
        '
        Me._lvwRenewals_ColumnHeader_1.Tag = ""
        Me._lvwRenewals_ColumnHeader_1.Text = "Branch"
        Me._lvwRenewals_ColumnHeader_1.Width = 67
        '
        '_lvwRenewals_ColumnHeader_2
        '
        Me._lvwRenewals_ColumnHeader_2.Tag = ""
        Me._lvwRenewals_ColumnHeader_2.Text = "Client Code"
        Me._lvwRenewals_ColumnHeader_2.Width = 97
        '
        '_lvwRenewals_ColumnHeader_3
        '
        Me._lvwRenewals_ColumnHeader_3.Tag = ""
        Me._lvwRenewals_ColumnHeader_3.Text = "Policy Number"
        Me._lvwRenewals_ColumnHeader_3.Width = 134
        '
        '_lvwRenewals_ColumnHeader_4
        '
        Me._lvwRenewals_ColumnHeader_4.Tag = ""
        Me._lvwRenewals_ColumnHeader_4.Text = "Agent"
        Me._lvwRenewals_ColumnHeader_4.Width = 97
        '
        '_lvwRenewals_ColumnHeader_5
        '
        Me._lvwRenewals_ColumnHeader_5.Tag = ""
        Me._lvwRenewals_ColumnHeader_5.Text = "Acc. Handler"
        Me._lvwRenewals_ColumnHeader_5.Width = 97
        '
        '_lvwRenewals_ColumnHeader_6
        '
        Me._lvwRenewals_ColumnHeader_6.Tag = ""
        Me._lvwRenewals_ColumnHeader_6.Text = "Renewal Date"
        Me._lvwRenewals_ColumnHeader_6.Width = 108
        '
        '_lvwRenewals_ColumnHeader_7
        '
        Me._lvwRenewals_ColumnHeader_7.Tag = ""
        Me._lvwRenewals_ColumnHeader_7.Text = "Status"
        Me._lvwRenewals_ColumnHeader_7.Width = 167
        '
        '_lvwRenewals_ColumnHeader_8
        '
        Me._lvwRenewals_ColumnHeader_8.Tag = ""
        Me._lvwRenewals_ColumnHeader_8.Text = "Exception Reason"
        Me._lvwRenewals_ColumnHeader_8.Width = 170
        '
        '_lvwRenewals_ColumnHeader_9
        '
        Me._lvwRenewals_ColumnHeader_9.Tag = ""
        Me._lvwRenewals_ColumnHeader_9.Text = "Product"
        Me._lvwRenewals_ColumnHeader_9.Width = 201
        '
        '_lvwRenewals_ColumnHeader_10
        '
        Me._lvwRenewals_ColumnHeader_10.Tag = ""
        Me._lvwRenewals_ColumnHeader_10.Text = "Claims Indicator"
        Me._lvwRenewals_ColumnHeader_10.Width = 101
        '
        '_lvwRenewals_ColumnHeader_11
        '
        Me._lvwRenewals_ColumnHeader_11.Tag = ""
        Me._lvwRenewals_ColumnHeader_11.Text = "Closed Branch"
        Me._lvwRenewals_ColumnHeader_11.Width = 0
        '
        '_lvwRenewals_ColumnHeader_12
        '
        Me._lvwRenewals_ColumnHeader_12.Tag = ""
        Me._lvwRenewals_ColumnHeader_12.Text = "Transfer To"
        Me._lvwRenewals_ColumnHeader_12.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Anniversary")
        Me.ImageList1.Images.SetKeyName(1, "Policy")
        '
        'lblStatus
        '
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(3, 480)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(837, 22)
        Me.lblStatus.TabIndex = 5
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(760, 448)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(249, 448)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 3
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        Me.cmdDelete.Visible = False
        '
        'cmdLapse
        '
        Me.cmdLapse.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLapse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLapse.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLapse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLapse.Location = New System.Drawing.Point(167, 448)
        Me.cmdLapse.Name = "cmdLapse"
        Me.cmdLapse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLapse.Size = New System.Drawing.Size(73, 22)
        Me.cmdLapse.TabIndex = 2
        Me.cmdLapse.Text = "&Lapse"
        Me.cmdLapse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdLapse.UseVisualStyleBackColor = False
        Me.cmdLapse.Visible = False
        '
        'cmdAmmend
        '
        Me.cmdAmmend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAmmend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAmmend.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAmmend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAmmend.Location = New System.Drawing.Point(86, 448)
        Me.cmdAmmend.Name = "cmdAmmend"
        Me.cmdAmmend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAmmend.Size = New System.Drawing.Size(73, 22)
        Me.cmdAmmend.TabIndex = 1
        Me.cmdAmmend.Text = "A&mend"
        Me.cmdAmmend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAmmend.UseVisualStyleBackColor = False
        Me.cmdAmmend.Visible = False
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(837, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'stbStatus
        '
        Me.stbStatus.Dock = System.Windows.Forms.DockStyle.None
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 480)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(854, 22)
        Me.stbStatus.TabIndex = 5
        '
        'frmRenewal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(839, 498)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmdTransfer)
        Me.Controls.Add(Me.cmdChangeStatus)
        Me.Controls.Add(Me.cmdFilter)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.cmdAccept)
        Me.Controls.Add(Me.lvwRenewals)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdLapse)
        Me.Controls.Add(Me.cmdAmmend)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(10, 48)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRenewal"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Renewals"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwRenewals_InitializeColumnKeys()
		Me._lvwRenewals_ColumnHeader_1.Name = ""
		Me._lvwRenewals_ColumnHeader_2.Name = ""
		Me._lvwRenewals_ColumnHeader_3.Name = ""
		Me._lvwRenewals_ColumnHeader_4.Name = ""
		Me._lvwRenewals_ColumnHeader_5.Name = ""
		Me._lvwRenewals_ColumnHeader_6.Name = ""
		Me._lvwRenewals_ColumnHeader_7.Name = ""
		Me._lvwRenewals_ColumnHeader_8.Name = ""
		Me._lvwRenewals_ColumnHeader_9.Name = ""
		Me._lvwRenewals_ColumnHeader_10.Name = ""
		Me._lvwRenewals_ColumnHeader_11.Name = ""
		Me._lvwRenewals_ColumnHeader_12.Name = ""
    End Sub
    Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
#End Region 
End Class