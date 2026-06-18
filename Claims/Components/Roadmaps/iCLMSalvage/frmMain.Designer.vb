<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
	Private Sub Ctx_mnuSummary_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSummary.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuSummary.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuSummary.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuSummary.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuSummary_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSummary.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSummary.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuSummary.DropDownItems.Add(item)
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
	Public WithEvents mnuProcessRestart As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuProcessRetry As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuProcessExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuProcess As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpContents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSummaryCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSummary As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents tmrStart As System.Windows.Forms.Timer
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdContinue As System.Windows.Forms.Button
	Public WithEvents imlIcons As System.Windows.Forms.ImageList
    Public WithEvents Status1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents navtype As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSummary_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSummary_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSummary As System.Windows.Forms.ListView
	Public WithEvents tvwTree As System.Windows.Forms.TreeView
	Public WithEvents lnBanner2 As System.Windows.Forms.Label
	Public WithEvents lnBanner1 As System.Windows.Forms.Label
	Public WithEvents lblBanner As System.Windows.Forms.Label
	Public WithEvents picLogo As System.Windows.Forms.PictureBox
	Public WithEvents Ctx_mnuSummary As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuProcess = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuProcessRestart = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuProcessRetry = New System.Windows.Forms.ToolStripMenuItem
		Me.sep2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuProcessExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelpContents = New System.Windows.Forms.ToolStripMenuItem
		Me.sep1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSummary = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSummaryCopy = New System.Windows.Forms.ToolStripMenuItem
		Me.tmrStart = New System.Windows.Forms.Timer(components)
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdContinue = New System.Windows.Forms.Button
		Me.imlIcons = New System.Windows.Forms.ImageList
		Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me.Status1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.navtype = New System.Windows.Forms.ToolStripStatusLabel
		Me.lvwSummary = New System.Windows.Forms.ListView
		Me._lvwSummary_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSummary_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.tvwTree = New System.Windows.Forms.TreeView
		Me.lnBanner2 = New System.Windows.Forms.Label
		Me.lnBanner1 = New System.Windows.Forms.Label
		Me.lblBanner = New System.Windows.Forms.Label
		Me.picLogo = New System.Windows.Forms.PictureBox
		Me.stbStatus.SuspendLayout()
		Me.lvwSummary.SuspendLayout()
		Me.SuspendLayout()
		'Ctx_mnuSummary
		Me.Ctx_mnuSummary = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuSummary.Size = New System.Drawing.Size(153, 26)
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuProcess, Me.mnuHelp, Me.mnuSummary})
		' 
		' mnuProcess
		' 
		Me.mnuProcess.Available = True
		Me.mnuProcess.Checked = False
		Me.mnuProcess.Enabled = True
		Me.mnuProcess.Name = "mnuProcess"
		Me.mnuProcess.Text = "&Process"
		Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuProcessRestart, Me.mnuProcessRetry, Me.sep2, Me.mnuProcessExit})
		' 
		' mnuProcessRestart
		' 
		Me.mnuProcessRestart.Available = True
		Me.mnuProcessRestart.Checked = False
		Me.mnuProcessRestart.Enabled = True
		Me.mnuProcessRestart.Name = "mnuProcessRestart"
		Me.mnuProcessRestart.Text = "&Restart"
		' 
		' mnuProcessRetry
		' 
		Me.mnuProcessRetry.Available = True
		Me.mnuProcessRetry.Checked = False
		Me.mnuProcessRetry.Enabled = True
		Me.mnuProcessRetry.Name = "mnuProcessRetry"
		Me.mnuProcessRetry.Text = "Re&try"
		' 
		' sep2
		' 
		Me.sep2.Available = True
		Me.sep2.Enabled = True
		Me.sep2.Name = "sep2"
		' 
		' mnuProcessExit
		' 
		Me.mnuProcessExit.Available = True
		Me.mnuProcessExit.Checked = False
		Me.mnuProcessExit.Enabled = True
		Me.mnuProcessExit.Name = "mnuProcessExit"
		Me.mnuProcessExit.Text = "E&xit"
		' 
		' mnuHelp
		' 
		Me.mnuHelp.Available = True
		Me.mnuHelp.Checked = False
		Me.mnuHelp.Enabled = True
		Me.mnuHelp.Name = "mnuHelp"
		Me.mnuHelp.Text = "&Help"
		Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuHelpContents, Me.sep1, Me.mnuHelpAbout})
		' 
		' mnuHelpContents
		' 
		Me.mnuHelpContents.Available = True
		Me.mnuHelpContents.Checked = False
		Me.mnuHelpContents.Enabled = True
		Me.mnuHelpContents.Name = "mnuHelpContents"
		Me.mnuHelpContents.ShortcutKeys = CType(System.Windows.Forms.Keys.F1, System.Windows.Forms.Keys)
		Me.mnuHelpContents.Text = "&Contents"
		' 
		' sep1
		' 
		Me.sep1.Available = True
		Me.sep1.Enabled = True
		Me.sep1.Name = "sep1"
		' 
		' mnuHelpAbout
		' 
		Me.mnuHelpAbout.Available = True
		Me.mnuHelpAbout.Checked = False
		Me.mnuHelpAbout.Enabled = True
		Me.mnuHelpAbout.Name = "mnuHelpAbout"
		Me.mnuHelpAbout.Text = "&About"
		' 
		' mnuSummary
		' 
		Me.mnuSummary.Available = False
		Me.mnuSummary.Checked = False
		Me.mnuSummary.Enabled = True
		Me.mnuSummary.Name = "mnuSummary"
		Me.mnuSummary.Text = "&Summary"
		Me.mnuSummary.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuSummaryCopy})
		' 
		' mnuSummaryCopy
		' 
		Me.mnuSummaryCopy.Available = True
		Me.mnuSummaryCopy.Checked = False
		Me.mnuSummaryCopy.Enabled = True
		Me.mnuSummaryCopy.Name = "mnuSummaryCopy"
		Me.mnuSummaryCopy.Text = "&Copy to Clipboard"
		' 
		' tmrStart
		' 
		Me.tmrStart.Enabled = False
		Me.tmrStart.Interval = 1
		' 
		' cmdClose
		' 
		Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClose.CausesValidation = True
		Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClose.Enabled = True
		Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClose.Location = New System.Drawing.Point(424, 424)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClose.Size = New System.Drawing.Size(73, 22)
		Me.cmdClose.TabIndex = 4
		Me.cmdClose.TabStop = True
		Me.cmdClose.Text = "&Close"
		Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdContinue
		' 
		Me.cmdContinue.BackColor = System.Drawing.SystemColors.Control
		Me.cmdContinue.CausesValidation = True
		Me.cmdContinue.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdContinue.Enabled = False
		Me.cmdContinue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdContinue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdContinue.Location = New System.Drawing.Point(0, 424)
		Me.cmdContinue.Name = "cmdContinue"
		Me.cmdContinue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdContinue.Size = New System.Drawing.Size(73, 22)
		Me.cmdContinue.TabIndex = 3
		Me.cmdContinue.TabStop = True
		Me.cmdContinue.Text = "&Continue"
		Me.cmdContinue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdContinue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' imlIcons
		' 
		Me.imlIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlIcons.Images.SetKeyName(0, "question")
		Me.imlIcons.Images.SetKeyName(1, "findform")
		Me.imlIcons.Images.SetKeyName(2, "print")
		Me.imlIcons.Images.SetKeyName(3, "navigate")
		Me.imlIcons.Images.SetKeyName(4, "dataform")
		Me.imlIcons.Images.SetKeyName(5, "business")
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 453)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(499, 18)
		Me.stbStatus.TabIndex = 2
		Me.stbStatus.Text = "In Progress..."
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Status1})
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.navtype})
		' 
        ' Status1
		' 
        Me.Status1.Alignment = 0
        Me.Status1.AutoSize = 1
        Me.Status1.AutoSize = False
        'Developer Guide No.20
        'Me.Status1.Bevel = 1
        Me.Status1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Status1.DoubleClickEnabled = True
        'Developer Guide No.20
        'Me.Status1.Key = "Status1"
        Me.Status1.Margin = New System.Windows.Forms.Padding(0)
        Me.Status1.Size = New System.Drawing.Size(384, 18)
		' 
		' navtype
		' 
		Me.navtype.AutoSize = True
		Me.navtype.AutoSize = False
		Me.navtype.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me.navtype.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me.navtype.DoubleClickEnabled = True
		Me.navtype.Margin = New System.Windows.Forms.Padding(0)
		Me.navtype.Name = "navtype"
		Me.navtype.Size = New System.Drawing.Size(96, 18)
		Me.navtype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.navtype.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' lvwSummary
		' 
		Me.lvwSummary.Alignment = System.Windows.Forms.ListViewAlignment.Left
		Me.lvwSummary.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSummary.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSummary.FullRowSelect = True
		Me.lvwSummary.GridLines = True
		Me.lvwSummary.HideSelection = True
		Me.lvwSummary.LabelEdit = False
		Me.lvwSummary.LabelWrap = True
		Me.lvwSummary.Location = New System.Drawing.Point(240, 176)
		Me.lvwSummary.Name = "lvwSummary"
		Me.lvwSummary.Size = New System.Drawing.Size(257, 241)
		Me.lvwSummary.TabIndex = 1
		Me.lvwSummary.View = System.Windows.Forms.View.Details
		Me.lvwSummary.Columns.Add(Me._lvwSummary_ColumnHeader_1)
		Me.lvwSummary.Columns.Add(Me._lvwSummary_ColumnHeader_2)
		' 
		' _lvwSummary_ColumnHeader_1
		' 
		Me._lvwSummary_ColumnHeader_1.Text = "Description"
		Me._lvwSummary_ColumnHeader_1.Width = 97
		' 
		' _lvwSummary_ColumnHeader_2
		' 
		Me._lvwSummary_ColumnHeader_2.Text = "Value"
		Me._lvwSummary_ColumnHeader_2.Width = 97
		' 
		' tvwTree
		' 
		Me.tvwTree.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvwTree.CausesValidation = True
		Me.tvwTree.CheckBoxes = True
		Me.tvwTree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvwTree.ImageList = imlIcons
		Me.tvwTree.Indent = 30
		Me.tvwTree.LabelEdit = False
		Me.tvwTree.LabelEdit = True
		Me.tvwTree.Location = New System.Drawing.Point(0, 56)
		Me.tvwTree.Name = "tvwTree"
		Me.tvwTree.Size = New System.Drawing.Size(233, 361)
		Me.tvwTree.TabIndex = 0
		' 
		' lnBanner2
		' 
		Me.lnBanner2.BackColor = System.Drawing.SystemColors.HighlightText
		Me.lnBanner2.Location = New System.Drawing.Point(0, 24)
		Me.lnBanner2.Name = "lnBanner2"
		Me.lnBanner2.Size = New System.Drawing.Size(496, 1)
		Me.lnBanner2.Visible = True
		' 
		' lnBanner1
		' 
		Me.lnBanner1.BackColor = System.Drawing.SystemColors.WindowText
		Me.lnBanner1.Location = New System.Drawing.Point(0, 24)
		Me.lnBanner1.Name = "lnBanner1"
		Me.lnBanner1.Size = New System.Drawing.Size(496, 1)
		Me.lnBanner1.Visible = True
		' 
		' lblBanner
		' 
		Me.lblBanner.AutoSize = False
		Me.lblBanner.BackColor = System.Drawing.Color.Gray
		Me.lblBanner.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBanner.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBanner.Enabled = True
		Me.lblBanner.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBanner.ForeColor = System.Drawing.Color.White
		Me.lblBanner.Location = New System.Drawing.Point(0, 26)
		Me.lblBanner.Name = "lblBanner"
		Me.lblBanner.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBanner.Size = New System.Drawing.Size(497, 25)
		Me.lblBanner.TabIndex = 5
		Me.lblBanner.Text = " Process Navigation"
		Me.lblBanner.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBanner.UseMnemonic = True
		Me.lblBanner.Visible = True
		' 
		' picLogo
		' 
		Me.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picLogo.Cursor = System.Windows.Forms.Cursors.Default
		Me.picLogo.Enabled = True
		Me.picLogo.Image = CType(resources.GetObject("picLogo.Image"), System.Drawing.Image)
		Me.picLogo.Location = New System.Drawing.Point(240, 56)
		Me.picLogo.Name = "picLogo"
		Me.picLogo.Size = New System.Drawing.Size(257, 113)
		Me.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picLogo.Visible = True
		' 
		' frmMain
		' 
		Me.BackColor = System.Drawing.SystemColors.Menu
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.cmdContinue)
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.lvwSummary)
		Me.Controls.Add(Me.tvwTree)
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(499, 471)
		Me.ControlBox = True
		Me.Controls.Add(Me.lnBanner2)
		Me.Controls.Add(Me.lnBanner1)
		Me.Controls.Add(Me.lblBanner)
		Me.Controls.Add(Me.picLogo)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(252, 139)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = " "
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.stbStatus.ResumeLayout(False)
		Me.lvwSummary.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class