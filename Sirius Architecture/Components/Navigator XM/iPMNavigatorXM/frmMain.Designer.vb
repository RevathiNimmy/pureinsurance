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
	Public WithEvents imlIconsold As System.Windows.Forms.ImageList
    Public WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents navtype As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSummary_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSummary_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSummary As System.Windows.Forms.ListView
	Public WithEvents tvwTree As System.Windows.Forms.TreeView
	Public WithEvents imlIcons As System.Windows.Forms.ImageList
	Public WithEvents lblSummaryDetails As System.Windows.Forms.Label
	Public WithEvents lnBanner2 As System.Windows.Forms.Label
	Public WithEvents lnBanner1 As System.Windows.Forms.Label
	Public WithEvents lblBanner As System.Windows.Forms.Label
	Public WithEvents picLogo As System.Windows.Forms.PictureBox
    Public WithEvents Ctx_mnuSummary As System.Windows.Forms.ContextMenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
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
        Me.tmrStart = New System.Windows.Forms.Timer(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdContinue = New System.Windows.Forms.Button
        Me.imlIconsold = New System.Windows.Forms.ImageList(Me.components)
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.navtype = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSummary = New System.Windows.Forms.ListView
        Me._lvwSummary_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSummary_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.tvwTree = New System.Windows.Forms.TreeView
        Me.imlIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.lblSummaryDetails = New System.Windows.Forms.Label
        Me.lnBanner2 = New System.Windows.Forms.Label
        Me.lnBanner1 = New System.Windows.Forms.Label
        Me.lblBanner = New System.Windows.Forms.Label
        Me.picLogo = New System.Windows.Forms.PictureBox
        Me.Ctx_mnuSummary = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProcess, Me.mnuHelp, Me.mnuSummary})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Padding = New System.Windows.Forms.Padding(0)
        Me.MainMenu1.Size = New System.Drawing.Size(547, 24)
        Me.MainMenu1.TabIndex = 10
        '
        'mnuProcess
        '
        Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProcessRestart, Me.mnuProcessRetry, Me.sep2, Me.mnuProcessExit})
        Me.mnuProcess.Name = "mnuProcess"
        Me.mnuProcess.Padding = New System.Windows.Forms.Padding(0)
        Me.mnuProcess.Size = New System.Drawing.Size(48, 24)
        Me.mnuProcess.Text = "&Process"
        '
        'mnuProcessRestart
        '
        Me.mnuProcessRestart.Name = "mnuProcessRestart"
        Me.mnuProcessRestart.Size = New System.Drawing.Size(110, 22)
        Me.mnuProcessRestart.Text = "&Restart"
        '
        'mnuProcessRetry
        '
        Me.mnuProcessRetry.Name = "mnuProcessRetry"
        Me.mnuProcessRetry.Size = New System.Drawing.Size(110, 22)
        Me.mnuProcessRetry.Text = "Re&try"
        '
        'sep2
        '
        Me.sep2.Name = "sep2"
        Me.sep2.Size = New System.Drawing.Size(107, 6)
        '
        'mnuProcessExit
        '
        Me.mnuProcessExit.Name = "mnuProcessExit"
        Me.mnuProcessExit.Size = New System.Drawing.Size(110, 22)
        Me.mnuProcessExit.Text = "E&xit"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpContents, Me.sep1, Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Padding = New System.Windows.Forms.Padding(0)
        Me.mnuHelp.Size = New System.Drawing.Size(32, 24)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpContents
        '
        Me.mnuHelpContents.Name = "mnuHelpContents"
        Me.mnuHelpContents.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuHelpContents.Size = New System.Drawing.Size(137, 22)
        Me.mnuHelpContents.Text = "&Contents"
        '
        'sep1
        '
        Me.sep1.Name = "sep1"
        Me.sep1.Size = New System.Drawing.Size(134, 6)
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(137, 22)
        Me.mnuHelpAbout.Text = "&About"
        '
        'mnuSummary
        '
        Me.mnuSummary.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSummaryCopy})
        Me.mnuSummary.Name = "mnuSummary"
        Me.mnuSummary.Padding = New System.Windows.Forms.Padding(0)
        Me.mnuSummary.Size = New System.Drawing.Size(55, 24)
        Me.mnuSummary.Text = "&Summary"
        Me.mnuSummary.Visible = False
        '
        'mnuSummaryCopy
        '
        Me.mnuSummaryCopy.Name = "mnuSummaryCopy"
        Me.mnuSummaryCopy.Size = New System.Drawing.Size(160, 22)
        Me.mnuSummaryCopy.Text = "&Copy to Clipboard"
        '
        'tmrStart
        '
        Me.tmrStart.Interval = 1
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Enabled = False
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(472, 424)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 4
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdContinue
        '
        Me.cmdContinue.BackColor = System.Drawing.SystemColors.Control
        Me.cmdContinue.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdContinue.Enabled = False
        Me.cmdContinue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdContinue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdContinue.Location = New System.Drawing.Point(0, 424)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdContinue.Size = New System.Drawing.Size(73, 22)
        Me.cmdContinue.TabIndex = 3
        Me.cmdContinue.Text = "&Continue"
        Me.cmdContinue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdContinue.UseVisualStyleBackColor = False
        '
        'imlIconsold
        '
        Me.imlIconsold.ImageStream = CType(resources.GetObject("imlIconsold.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIconsold.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlIconsold.Images.SetKeyName(0, "")
        Me.imlIconsold.Images.SetKeyName(1, "Process")
        Me.imlIconsold.Images.SetKeyName(2, "StepFind")
        Me.imlIconsold.Images.SetKeyName(3, "StepFindCross")
        Me.imlIconsold.Images.SetKeyName(4, "StepFindTick")
        Me.imlIconsold.Images.SetKeyName(5, "StepFindGrey")
        Me.imlIconsold.Images.SetKeyName(6, "StepNoForm")
        Me.imlIconsold.Images.SetKeyName(7, "StepNoFormCross")
        Me.imlIconsold.Images.SetKeyName(8, "StepNoFormGrey")
        Me.imlIconsold.Images.SetKeyName(9, "StepNoFormTick")
        Me.imlIconsold.Images.SetKeyName(10, "SubMap")
        Me.imlIconsold.Images.SetKeyName(11, "SubMapGrey")
        Me.imlIconsold.Images.SetKeyName(12, "StepDataForm")
        Me.imlIconsold.Images.SetKeyName(13, "StepDataFormCross")
        Me.imlIconsold.Images.SetKeyName(14, "StepDataFormGrey")
        Me.imlIconsold.Images.SetKeyName(15, "StepDataFormTick")
        Me.imlIconsold.Images.SetKeyName(16, "StepDecision")
        Me.imlIconsold.Images.SetKeyName(17, "StepDecisionTick")
        Me.imlIconsold.Images.SetKeyName(18, "StepDecisionCross")
        Me.imlIconsold.Images.SetKeyName(19, "StepDecisionGrey")
        Me.imlIconsold.Images.SetKeyName(20, "")
        Me.imlIconsold.Images.SetKeyName(21, "StepPrint")
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.navtype})
        Me.stbStatus.Location = New System.Drawing.Point(0, 450)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(547, 22)
        Me.stbStatus.TabIndex = 2
        Me.stbStatus.Text = "In Progress..."
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = False
        Me.lblStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.lblStatus.DoubleClickEnabled = True
        Me.lblStatus.Margin = New System.Windows.Forms.Padding(0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(432, 22)
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'navtype
        '
        Me.navtype.AutoSize = False
        Me.navtype.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.navtype.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.navtype.DoubleClickEnabled = True
        Me.navtype.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.navtype.Margin = New System.Windows.Forms.Padding(0)
        Me.navtype.Name = "navtype"
        Me.navtype.Size = New System.Drawing.Size(100, 22)
        '
        'lvwSummary
        '
        Me.lvwSummary.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.lvwSummary.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSummary.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSummary_ColumnHeader_1, Me._lvwSummary_ColumnHeader_2})
        Me.lvwSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSummary.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSummary.FullRowSelect = True
        Me.lvwSummary.GridLines = True
        Me.lvwSummary.Location = New System.Drawing.Point(288, 192)
        Me.lvwSummary.Name = "lvwSummary"
        Me.lvwSummary.Size = New System.Drawing.Size(257, 225)
        Me.lvwSummary.TabIndex = 1
        Me.lvwSummary.UseCompatibleStateImageBehavior = False
        Me.lvwSummary.View = System.Windows.Forms.View.Details
        '
        '_lvwSummary_ColumnHeader_1
        '
        Me._lvwSummary_ColumnHeader_1.Text = "Description"
        Me._lvwSummary_ColumnHeader_1.Width = 97
        '
        '_lvwSummary_ColumnHeader_2
        '
        Me._lvwSummary_ColumnHeader_2.Text = "Value"
        Me._lvwSummary_ColumnHeader_2.Width = 97
        '
        'tvwTree
        '
        Me.tvwTree.CheckBoxes = True
        Me.tvwTree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwTree.ImageIndex = 0
        Me.tvwTree.ImageList = Me.imlIcons
        Me.tvwTree.Indent = 20
        Me.tvwTree.Location = New System.Drawing.Point(0, 56)
        Me.tvwTree.Name = "tvwTree"
        Me.tvwTree.SelectedImageIndex = 0
        Me.tvwTree.Size = New System.Drawing.Size(281, 361)
        Me.tvwTree.TabIndex = 0
        '
        'imlIcons
        '
        Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlIcons.Images.SetKeyName(0, "")
        Me.imlIcons.Images.SetKeyName(1, "Diary")
        Me.imlIcons.Images.SetKeyName(2, "EditText")
        Me.imlIcons.Images.SetKeyName(3, "StandardLetter")
        Me.imlIcons.Images.SetKeyName(4, "RaiseEvent")
        Me.imlIcons.Images.SetKeyName(5, "LaunchEXE")
        Me.imlIcons.Images.SetKeyName(6, "UserComponent")
        Me.imlIcons.Images.SetKeyName(7, "Roadmap")
        Me.imlIcons.Images.SetKeyName(8, "Process")
        Me.imlIcons.Images.SetKeyName(9, "StepFind")
        Me.imlIcons.Images.SetKeyName(10, "StepFindCross")
        Me.imlIcons.Images.SetKeyName(11, "StepFindTick")
        Me.imlIcons.Images.SetKeyName(12, "Key")
        Me.imlIcons.Images.SetKeyName(13, "StepFindGrey")
        Me.imlIcons.Images.SetKeyName(14, "StepNoForm")
        Me.imlIcons.Images.SetKeyName(15, "StepNoFormCross")
        Me.imlIcons.Images.SetKeyName(16, "StepNoFormGrey")
        Me.imlIcons.Images.SetKeyName(17, "StepNoFormTick")
        Me.imlIcons.Images.SetKeyName(18, "SubMap")
        Me.imlIcons.Images.SetKeyName(19, "SubMapGrey")
        Me.imlIcons.Images.SetKeyName(20, "StepDataForm")
        Me.imlIcons.Images.SetKeyName(21, "StepDataFormCross")
        Me.imlIcons.Images.SetKeyName(22, "StepDataFormGrey")
        Me.imlIcons.Images.SetKeyName(23, "StepDataFormTick")
        Me.imlIcons.Images.SetKeyName(24, "StepDecision")
        Me.imlIcons.Images.SetKeyName(25, "StepDecisionTick")
        Me.imlIcons.Images.SetKeyName(26, "StepDecisionCross")
        Me.imlIcons.Images.SetKeyName(27, "StepDecisionGrey")
        Me.imlIcons.Images.SetKeyName(28, "")
        Me.imlIcons.Images.SetKeyName(29, "StepPrint")
        '
        'lblSummaryDetails
        '
        Me.lblSummaryDetails.BackColor = System.Drawing.Color.Gray
        Me.lblSummaryDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSummaryDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSummaryDetails.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblSummaryDetails.Location = New System.Drawing.Point(288, 176)
        Me.lblSummaryDetails.Name = "lblSummaryDetails"
        Me.lblSummaryDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSummaryDetails.Size = New System.Drawing.Size(257, 17)
        Me.lblSummaryDetails.TabIndex = 6
        Me.lblSummaryDetails.Text = " Summary Details"
        '
        'lnBanner2
        '
        Me.lnBanner2.BackColor = System.Drawing.SystemColors.HighlightText
        Me.lnBanner2.Location = New System.Drawing.Point(0, 25)
        Me.lnBanner2.Name = "lnBanner2"
        Me.lnBanner2.Size = New System.Drawing.Size(544, 1)
        Me.lnBanner2.TabIndex = 7
        '
        'lnBanner1
        '
        Me.lnBanner1.BackColor = System.Drawing.SystemColors.WindowText
        Me.lnBanner1.Location = New System.Drawing.Point(0, 24)
        Me.lnBanner1.Name = "lnBanner1"
        Me.lnBanner1.Size = New System.Drawing.Size(544, 1)
        Me.lnBanner1.TabIndex = 8
        '
        'lblBanner
        '
        Me.lblBanner.BackColor = System.Drawing.Color.Gray
        Me.lblBanner.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBanner.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBanner.ForeColor = System.Drawing.Color.White
        Me.lblBanner.Location = New System.Drawing.Point(0, 27)
        Me.lblBanner.Name = "lblBanner"
        Me.lblBanner.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBanner.Size = New System.Drawing.Size(545, 25)
        Me.lblBanner.TabIndex = 5
        Me.lblBanner.Text = " Process Navigation"
        '
        'picLogo
        '
        Me.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picLogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.picLogo.Image = CType(resources.GetObject("picLogo.Image"), System.Drawing.Image)
        Me.picLogo.Location = New System.Drawing.Point(288, 56)
        Me.picLogo.Name = "picLogo"
        Me.picLogo.Size = New System.Drawing.Size(257, 113)
        Me.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLogo.TabIndex = 9
        Me.picLogo.TabStop = False
        '
        'Ctx_mnuSummary
        '
        Me.Ctx_mnuSummary.Name = "Ctx_mnuSummary"
        Me.Ctx_mnuSummary.Size = New System.Drawing.Size(61, 4)
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(547, 472)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSummary)
        Me.Controls.Add(Me.tvwTree)
        Me.Controls.Add(Me.lblSummaryDetails)
        Me.Controls.Add(Me.lnBanner2)
        Me.Controls.Add(Me.lnBanner1)
        Me.Controls.Add(Me.lblBanner)
        Me.Controls.Add(Me.picLogo)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(252, 131)
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class