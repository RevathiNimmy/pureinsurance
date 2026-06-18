<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwKeywords_InitializeColumnKeys()
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
	Public WithEvents mnuFileNew As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileRemove As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewRefresh As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _lvwKeywords_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwKeywords_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwKeywords As System.Windows.Forms.ListView
	Public WithEvents cmdDetach As System.Windows.Forms.Button
	Public WithEvents cmdAttach As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents imgKeywords As System.Windows.Forms.PictureBox
	Public WithEvents fraKeywords As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileRemove = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.fraKeywords = New System.Windows.Forms.GroupBox
        Me.lvwKeywords = New System.Windows.Forms.ListView
        Me._lvwKeywords_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwKeywords_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.cmdDetach = New System.Windows.Forms.Button
        Me.cmdAttach = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.imgKeywords = New System.Windows.Forms.PictureBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.fraKeywords.SuspendLayout()
        CType(Me.imgKeywords, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuView})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(449, 24)
        Me.MainMenu1.TabIndex = 9
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileNew, Me.mnuFileRemove})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(32, 19)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileNew
        '
        Me.mnuFileNew.Name = "mnuFileNew"
        Me.mnuFileNew.Size = New System.Drawing.Size(113, 22)
        Me.mnuFileNew.Text = "&New"
        '
        'mnuFileRemove
        '
        Me.mnuFileRemove.Name = "mnuFileRemove"
        Me.mnuFileRemove.Size = New System.Drawing.Size(113, 22)
        Me.mnuFileRemove.Text = "&Remove"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewRefresh})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(32, 19)
        Me.mnuView.Text = "&View"
        '
        'mnuViewRefresh
        '
        Me.mnuViewRefresh.Name = "mnuViewRefresh"
        Me.mnuViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuViewRefresh.Size = New System.Drawing.Size(131, 22)
        Me.mnuViewRefresh.Text = "&Refresh"
        '
        'fraKeywords
        '
        Me.fraKeywords.BackColor = System.Drawing.SystemColors.Control
        Me.fraKeywords.Controls.Add(Me.lvwKeywords)
        Me.fraKeywords.Controls.Add(Me.cmdDetach)
        Me.fraKeywords.Controls.Add(Me.cmdAttach)
        Me.fraKeywords.Controls.Add(Me.cmdNew)
        Me.fraKeywords.Controls.Add(Me.cmdRemove)
        Me.fraKeywords.Controls.Add(Me.imgKeywords)
        Me.fraKeywords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraKeywords.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraKeywords.Location = New System.Drawing.Point(8, 32)
        Me.fraKeywords.Name = "fraKeywords"
        Me.fraKeywords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraKeywords.Size = New System.Drawing.Size(433, 313)
        Me.fraKeywords.TabIndex = 8
        Me.fraKeywords.TabStop = False
        '
        'lvwKeywords
        '
        Me.lvwKeywords.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwKeywords, "")
        Me.lvwKeywords.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwKeywords_ColumnHeader_1, Me._lvwKeywords_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwKeywords, True)
        Me.lvwKeywords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwKeywords.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwKeywords.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwKeywords, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwKeywords, "")
        Me.lvwKeywords.Location = New System.Drawing.Point(8, 16)
        Me.lvwKeywords.Name = "lvwKeywords"
        Me.lvwKeywords.Size = New System.Drawing.Size(337, 289)
        Me.listViewHelper1.SetSmallIcons(Me.lvwKeywords, "")
        Me.listViewHelper1.SetSorted(Me.lvwKeywords, True)
        Me.lvwKeywords.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me.lvwKeywords, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwKeywords, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwKeywords.TabIndex = 7
        Me.lvwKeywords.UseCompatibleStateImageBehavior = False
        Me.lvwKeywords.View = System.Windows.Forms.View.List
        '
        '_lvwKeywords_ColumnHeader_1
        '
        Me._lvwKeywords_ColumnHeader_1.Tag = ""
        Me._lvwKeywords_ColumnHeader_1.Text = "Keyword"
        Me._lvwKeywords_ColumnHeader_1.Width = 241
        '
        '_lvwKeywords_ColumnHeader_2
        '
        Me._lvwKeywords_ColumnHeader_2.Tag = ""
        Me._lvwKeywords_ColumnHeader_2.Text = "Attached"
        Me._lvwKeywords_ColumnHeader_2.Width = 54
        '
        'cmdDetach
        '
        Me.cmdDetach.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetach.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetach.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetach.Location = New System.Drawing.Point(352, 160)
        Me.cmdDetach.Name = "cmdDetach"
        Me.cmdDetach.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetach.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetach.TabIndex = 3
        Me.cmdDetach.Text = "&Detach"
        Me.cmdDetach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetach.UseVisualStyleBackColor = False
        Me.cmdDetach.Visible = False
        '
        'cmdAttach
        '
        Me.cmdAttach.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAttach.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAttach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAttach.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAttach.Location = New System.Drawing.Point(352, 192)
        Me.cmdAttach.Name = "cmdAttach"
        Me.cmdAttach.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAttach.Size = New System.Drawing.Size(73, 22)
        Me.cmdAttach.TabIndex = 4
        Me.cmdAttach.Text = "&Attach"
        Me.cmdAttach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAttach.UseVisualStyleBackColor = False
        Me.cmdAttach.Visible = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(352, 248)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 5
        Me.cmdNew.Text = "&New"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(352, 280)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
        Me.cmdRemove.TabIndex = 6
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'imgKeywords
        '
        Me.imgKeywords.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgKeywords.Image = CType(resources.GetObject("imgKeywords.Image"), System.Drawing.Image)
        Me.imgKeywords.Location = New System.Drawing.Point(368, 24)
        Me.imgKeywords.Name = "imgKeywords"
        Me.imgKeywords.Size = New System.Drawing.Size(32, 32)
        Me.imgKeywords.TabIndex = 8
        Me.imgKeywords.TabStop = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(200, 352)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(360, 352)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
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
        Me.cmdCancel.Location = New System.Drawing.Point(280, 352)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(449, 380)
        Me.Controls.Add(Me.fraKeywords)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Attach Keywords"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.fraKeywords.ResumeLayout(False)
        CType(Me.imgKeywords, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwKeywords_InitializeColumnKeys()
		Me._lvwKeywords_ColumnHeader_1.Name = ""
		Me._lvwKeywords_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class