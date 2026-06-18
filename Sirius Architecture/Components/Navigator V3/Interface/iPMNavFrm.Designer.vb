<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lstwSummaryDetails_InitializeColumnKeys()
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
	Public WithEvents mnuHelpContents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpSearch As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpSep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdRestart As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents tmrStartProcess As System.Windows.Forms.Timer
	Public WithEvents cmdContinue As System.Windows.Forms.Button
	Public WithEvents LinSliderBar As System.Windows.Forms.Label
	Public WithEvents panSliderBar As System.Windows.Forms.GroupBox
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents panProcessTitle As System.Windows.Forms.Panel
	Public WithEvents panSummaryTitle As System.Windows.Forms.Panel
	Private WithEvents _staStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _staStatus_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _staStatus_Panel3 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents staStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lstwSummaryDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lstwSummaryDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lstwSummaryDetails As System.Windows.Forms.ListView
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public WithEvents panSummaryDetails As System.Windows.Forms.Panel
	Public WithEvents treMainData As System.Windows.Forms.TreeView
	Public WithEvents treDummy As System.Windows.Forms.TreeView
	Public WithEvents imgSummImages As System.Windows.Forms.ImageList
	Public WithEvents imgImages As System.Windows.Forms.ImageList
	Public WithEvents linMenuLine2 As System.Windows.Forms.Label
	Public WithEvents linMenuLine1 As System.Windows.Forms.Label
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
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpContents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdRestart = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tmrStartProcess = New System.Windows.Forms.Timer(Me.components)
        Me.cmdContinue = New System.Windows.Forms.Button
        Me.panSliderBar = New System.Windows.Forms.GroupBox
        Me.LinSliderBar = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.panProcessTitle = New System.Windows.Forms.Panel
        Me.panProcessTitlelbl = New System.Windows.Forms.Label
        Me.panSummaryTitle = New System.Windows.Forms.Panel
        Me.panSummaryTitlelbl = New System.Windows.Forms.Label
        Me.staStatus = New System.Windows.Forms.StatusStrip
        Me._staStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._staStatus_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me._staStatus_Panel3 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lstwSummaryDetails = New System.Windows.Forms.ListView
        Me._lstwSummaryDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lstwSummaryDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.panSummaryDetails = New System.Windows.Forms.Panel
        Me.treMainData = New System.Windows.Forms.TreeView
        Me.imgImages = New System.Windows.Forms.ImageList(Me.components)
        Me.treDummy = New System.Windows.Forms.TreeView
        Me.imgSummImages = New System.Windows.Forms.ImageList(Me.components)
        Me.linMenuLine2 = New System.Windows.Forms.Label
        Me.linMenuLine1 = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.panSliderBar.SuspendLayout()
        Me.panProcessTitle.SuspendLayout()
        Me.panSummaryTitle.SuspendLayout()
        Me.staStatus.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(595, 24)
        Me.MainMenu1.TabIndex = 15
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpContents, Me.mnuHelpSearch, Me.mnuHelpSep1, Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpContents
        '
        Me.mnuHelpContents.Name = "mnuHelpContents"
        Me.mnuHelpContents.Size = New System.Drawing.Size(179, 22)
        Me.mnuHelpContents.Text = "&Contents"
        '
        'mnuHelpSearch
        '
        Me.mnuHelpSearch.Name = "mnuHelpSearch"
        Me.mnuHelpSearch.Size = New System.Drawing.Size(179, 22)
        Me.mnuHelpSearch.Text = "&Search For Help On..."
        '
        'mnuHelpSep1
        '
        Me.mnuHelpSep1.Name = "mnuHelpSep1"
        Me.mnuHelpSep1.Size = New System.Drawing.Size(176, 6)
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(179, 22)
        Me.mnuHelpAbout.Text = "&About PMNavigator..."
        '
        'cmdRestart
        '
        Me.cmdRestart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRestart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRestart.Enabled = False
        Me.cmdRestart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRestart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRestart.Location = New System.Drawing.Point(84, 392)
        Me.cmdRestart.Name = "cmdRestart"
        Me.cmdRestart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRestart.Size = New System.Drawing.Size(73, 22)
        Me.cmdRestart.TabIndex = 12
        Me.cmdRestart.Text = "&Restart"
        Me.cmdRestart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRestart.UseVisualStyleBackColor = False
        Me.cmdRestart.Visible = False
        '
        'tmrStartProcess
        '
        Me.tmrStartProcess.Interval = 10
        '
        'cmdContinue
        '
        Me.cmdContinue.BackColor = System.Drawing.SystemColors.Control
        Me.cmdContinue.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdContinue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdContinue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdContinue.Location = New System.Drawing.Point(1, 392)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdContinue.Size = New System.Drawing.Size(73, 22)
        Me.cmdContinue.TabIndex = 10
        Me.cmdContinue.Text = "&Continue"
        Me.cmdContinue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdContinue.UseVisualStyleBackColor = False
        '
        'panSliderBar
        '
        Me.panSliderBar.BackColor = System.Drawing.SystemColors.Control
        Me.panSliderBar.Controls.Add(Me.LinSliderBar)
        Me.panSliderBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panSliderBar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.panSliderBar.Location = New System.Drawing.Point(246, 56)
        Me.panSliderBar.Name = "panSliderBar"
        Me.panSliderBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panSliderBar.Size = New System.Drawing.Size(3, 312)
        Me.panSliderBar.TabIndex = 6
        Me.panSliderBar.TabStop = False
        '
        'LinSliderBar
        '
        Me.LinSliderBar.BackColor = System.Drawing.SystemColors.Control
        Me.LinSliderBar.Location = New System.Drawing.Point(0, 0)
        Me.LinSliderBar.Name = "LinSliderBar"
        Me.LinSliderBar.Size = New System.Drawing.Size(1, 392)
        Me.LinSliderBar.TabIndex = 0
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(440, 392)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 0
        Me.cmdClose.Text = "Cl&ose"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(520, 392)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'panProcessTitle
        '
        Me.panProcessTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panProcessTitle.Controls.Add(Me.panProcessTitlelbl)
        Me.panProcessTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panProcessTitle.Location = New System.Drawing.Point(1, 32)
        Me.panProcessTitle.Name = "panProcessTitle"
        Me.panProcessTitle.Size = New System.Drawing.Size(245, 17)
        Me.panProcessTitle.TabIndex = 4
        '
        'panProcessTitlelbl
        '
        Me.panProcessTitlelbl.AutoSize = True
        Me.panProcessTitlelbl.Location = New System.Drawing.Point(3, 0)
        Me.panProcessTitlelbl.Name = "panProcessTitlelbl"
        Me.panProcessTitlelbl.Size = New System.Drawing.Size(0, 13)
        Me.panProcessTitlelbl.TabIndex = 0
        '
        'panSummaryTitle
        '
        Me.panSummaryTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panSummaryTitle.Controls.Add(Me.panSummaryTitlelbl)
        Me.panSummaryTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panSummaryTitle.Location = New System.Drawing.Point(249, 32)
        Me.panSummaryTitle.Name = "panSummaryTitle"
        Me.panSummaryTitle.Size = New System.Drawing.Size(343, 17)
        Me.panSummaryTitle.TabIndex = 5
        '
        'panSummaryTitlelbl
        '
        Me.panSummaryTitlelbl.Location = New System.Drawing.Point(3, 0)
        Me.panSummaryTitlelbl.Name = "panSummaryTitlelbl"
        Me.panSummaryTitlelbl.Size = New System.Drawing.Size(163, 13)
        Me.panSummaryTitlelbl.TabIndex = 0
        '
        'staStatus
        '
        Me.staStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.staStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._staStatus_Panel1, Me._staStatus_Panel2, Me._staStatus_Panel3})
        Me.staStatus.Location = New System.Drawing.Point(0, 417)
        Me.staStatus.Name = "staStatus"
        Me.staStatus.ShowItemToolTips = True
        Me.staStatus.Size = New System.Drawing.Size(595, 22)
        Me.staStatus.TabIndex = 2
        '
        '_staStatus_Panel1
        '
        Me._staStatus_Panel1.AutoSize = False
        Me._staStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staStatus_Panel1.DoubleClickEnabled = True
        Me._staStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._staStatus_Panel1.Name = "_staStatus_Panel1"
        Me._staStatus_Panel1.Size = New System.Drawing.Size(134, 22)
        Me._staStatus_Panel1.Tag = ""
        Me._staStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_staStatus_Panel2
        '
        Me._staStatus_Panel2.AutoSize = False
        Me._staStatus_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staStatus_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staStatus_Panel2.DoubleClickEnabled = True
        Me._staStatus_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._staStatus_Panel2.Name = "_staStatus_Panel2"
        Me._staStatus_Panel2.Size = New System.Drawing.Size(134, 22)
        Me._staStatus_Panel2.Tag = ""
        Me._staStatus_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_staStatus_Panel3
        '
        Me._staStatus_Panel3.AutoSize = False
        Me._staStatus_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staStatus_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staStatus_Panel3.DoubleClickEnabled = True
        Me._staStatus_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me._staStatus_Panel3.Name = "_staStatus_Panel3"
        Me._staStatus_Panel3.Size = New System.Drawing.Size(309, 22)
        Me._staStatus_Panel3.Tag = ""
        Me._staStatus_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._staStatus_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'lstwSummaryDetails
        '
        Me.lstwSummaryDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lstwSummaryDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lstwSummaryDetails, "")
        Me.lstwSummaryDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstwSummaryDetails_ColumnHeader_1, Me._lstwSummaryDetails_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lstwSummaryDetails, False)
        Me.lstwSummaryDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstwSummaryDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstwSummaryDetails.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lstwSummaryDetails, "")
        Me.lstwSummaryDetails.LabelEdit = True
        Me.lstwSummaryDetails.LabelWrap = False
        Me.listViewHelper1.SetLargeIcons(Me.lstwSummaryDetails, "")
        Me.lstwSummaryDetails.Location = New System.Drawing.Point(248, 55)
        Me.lstwSummaryDetails.Name = "lstwSummaryDetails"
        Me.lstwSummaryDetails.Size = New System.Drawing.Size(344, 328)
        Me.listViewHelper1.SetSmallIcons(Me.lstwSummaryDetails, "")
        Me.listViewHelper1.SetSorted(Me.lstwSummaryDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lstwSummaryDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lstwSummaryDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lstwSummaryDetails.TabIndex = 7
        Me.lstwSummaryDetails.UseCompatibleStateImageBehavior = False
        Me.lstwSummaryDetails.View = System.Windows.Forms.View.Details
        '
        '_lstwSummaryDetails_ColumnHeader_1
        '
        Me._lstwSummaryDetails_ColumnHeader_1.Tag = ""
        Me._lstwSummaryDetails_ColumnHeader_1.Text = "Description"
        Me._lstwSummaryDetails_ColumnHeader_1.Width = 167
        '
        '_lstwSummaryDetails_ColumnHeader_2
        '
        Me._lstwSummaryDetails_ColumnHeader_2.Tag = ""
        Me._lstwSummaryDetails_ColumnHeader_2.Text = "Value"
        Me._lstwSummaryDetails_ColumnHeader_2.Width = 114
        '
        'ListView1
        '
        Me.ListView1.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.ListView1, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.ListView1, False)
        Me.ListView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.ListView1, "")
        Me.ListView1.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.ListView1, "")
        Me.ListView1.Location = New System.Drawing.Point(248, 76)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(345, 309)
        Me.listViewHelper1.SetSmallIcons(Me.ListView1, "")
        Me.listViewHelper1.SetSorted(Me.ListView1, False)
        Me.listViewHelper1.SetSortKey(Me.ListView1, 0)
        Me.listViewHelper1.SetSortOrder(Me.ListView1, System.Windows.Forms.SortOrder.Ascending)
        Me.ListView1.TabIndex = 9
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.Visible = False
        '
        'panSummaryDetails
        '
        Me.panSummaryDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(165, Byte), Integer))
        Me.panSummaryDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panSummaryDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panSummaryDetails.Location = New System.Drawing.Point(249, 248)
        Me.panSummaryDetails.Name = "panSummaryDetails"
        Me.panSummaryDetails.Size = New System.Drawing.Size(344, 17)
        Me.panSummaryDetails.TabIndex = 8
        Me.panSummaryDetails.Visible = False
        '
        'treMainData
        '
        Me.treMainData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treMainData.HideSelection = False
        Me.treMainData.ImageIndex = 0
        Me.treMainData.ImageList = Me.imgImages
        Me.treMainData.Indent = 20
        Me.treMainData.LabelEdit = True
        Me.treMainData.Location = New System.Drawing.Point(0, 55)
        Me.treMainData.Name = "treMainData"
        Me.treMainData.SelectedImageIndex = 0
        Me.treMainData.ShowPlusMinus = False
        Me.treMainData.ShowRootLines = False
        Me.treMainData.Size = New System.Drawing.Size(246, 328)
        Me.treMainData.TabIndex = 3
        '
        'imgImages
        '
        Me.imgImages.ImageStream = CType(resources.GetObject("imgImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgImages.TransparentColor = System.Drawing.Color.Transparent
        Me.imgImages.Images.SetKeyName(0, "")
        Me.imgImages.Images.SetKeyName(1, "Process")
        Me.imgImages.Images.SetKeyName(2, "StepFind")
        Me.imgImages.Images.SetKeyName(3, "StepFindCross")
        Me.imgImages.Images.SetKeyName(4, "StepFindTick")
        Me.imgImages.Images.SetKeyName(5, "StepFindGrey")
        Me.imgImages.Images.SetKeyName(6, "StepNoForm")
        Me.imgImages.Images.SetKeyName(7, "StepNoFormCross")
        Me.imgImages.Images.SetKeyName(8, "StepNoFormGrey")
        Me.imgImages.Images.SetKeyName(9, "StepNoFormTick")
        Me.imgImages.Images.SetKeyName(10, "SubMap")
        Me.imgImages.Images.SetKeyName(11, "SubMapGrey")
        Me.imgImages.Images.SetKeyName(12, "StepDataForm")
        Me.imgImages.Images.SetKeyName(13, "StepDataFormCross")
        Me.imgImages.Images.SetKeyName(14, "StepDataFormGrey")
        Me.imgImages.Images.SetKeyName(15, "StepDataFormTick")
        Me.imgImages.Images.SetKeyName(16, "StepDecision")
        Me.imgImages.Images.SetKeyName(17, "StepDecisionTick")
        Me.imgImages.Images.SetKeyName(18, "StepDecisionCross")
        Me.imgImages.Images.SetKeyName(19, "StepDecisionGrey")
        Me.imgImages.Images.SetKeyName(20, "")
        '
        'treDummy
        '
        Me.treDummy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treDummy.LabelEdit = True
        Me.treDummy.Location = New System.Drawing.Point(0, 55)
        Me.treDummy.Name = "treDummy"
        Me.treDummy.Size = New System.Drawing.Size(246, 328)
        Me.treDummy.TabIndex = 11
        Me.treDummy.Visible = False
        '
        'imgSummImages
        '
        Me.imgSummImages.ImageStream = CType(resources.GetObject("imgSummImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgSummImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgSummImages.Images.SetKeyName(0, "SummMainDetails")
        Me.imgSummImages.Images.SetKeyName(1, "SummProcessSummary")
        Me.imgSummImages.Images.SetKeyName(2, "SummMapInstances")
        Me.imgSummImages.Images.SetKeyName(3, "")
        Me.imgSummImages.Images.SetKeyName(4, "SummDetails")
        '
        'linMenuLine2
        '
        Me.linMenuLine2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.linMenuLine2.Location = New System.Drawing.Point(0, 24)
        Me.linMenuLine2.Name = "linMenuLine2"
        Me.linMenuLine2.Size = New System.Drawing.Size(1334, 1)
        Me.linMenuLine2.TabIndex = 13
        '
        'linMenuLine1
        '
        Me.linMenuLine1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.linMenuLine1.Location = New System.Drawing.Point(0, 24)
        Me.linMenuLine1.Name = "linMenuLine1"
        Me.linMenuLine1.Size = New System.Drawing.Size(1334, 1)
        Me.linMenuLine1.TabIndex = 14
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdContinue
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(595, 439)
        Me.Controls.Add(Me.cmdRestart)
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.panSliderBar)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.panProcessTitle)
        Me.Controls.Add(Me.panSummaryTitle)
        Me.Controls.Add(Me.staStatus)
        Me.Controls.Add(Me.lstwSummaryDetails)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.panSummaryDetails)
        Me.Controls.Add(Me.treMainData)
        Me.Controls.Add(Me.treDummy)
        Me.Controls.Add(Me.linMenuLine2)
        Me.Controls.Add(Me.linMenuLine1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(231, 212)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Policy Master Navigator"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.panSliderBar.ResumeLayout(False)
        Me.panProcessTitle.ResumeLayout(False)
        Me.panProcessTitle.PerformLayout()
        Me.panSummaryTitle.ResumeLayout(False)
        Me.staStatus.ResumeLayout(False)
        Me.staStatus.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lstwSummaryDetails_InitializeColumnKeys()
        Me._lstwSummaryDetails_ColumnHeader_1.Name = ""
        Me._lstwSummaryDetails_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents panProcessTitlelbl As System.Windows.Forms.Label
    Friend WithEvents panSummaryTitlelbl As System.Windows.Forms.Label
#End Region 
End Class