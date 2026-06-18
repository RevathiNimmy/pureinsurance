<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents mnuFileExpandAll As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileRefresh As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuViewPage As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentSep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuDocumentDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentDeleteAll As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentSep2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuDocumentCommit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocument As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuContents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _tlbMain_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button3 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button4 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button5 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button6 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _tlbMain_Button7 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tlbMain_Button8 As System.Windows.Forms.ToolStripButton
	Public WithEvents tlbMain As System.Windows.Forms.ToolStrip
	Public WithEvents picSplitBar As System.Windows.Forms.PictureBox
	Public WithEvents picBorder As System.Windows.Forms.PictureBox
	Public WithEvents tvwDocuments As System.Windows.Forms.TreeView
	Private WithEvents _stbMain_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _stbMain_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _stbMain_Panel3 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbMain As System.Windows.Forms.StatusStrip
	Public WithEvents imlToolbar As System.Windows.Forms.ImageList
	Public WithEvents imlTree As System.Windows.Forms.ImageList
	Public WithEvents imgSplitBar As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileExpandAll = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileRefresh = New System.Windows.Forms.ToolStripMenuItem
		Me.sep1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDocument = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuViewPage = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDocumentSep1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuDocumentDelete = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDocumentDeleteAll = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDocumentSep2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuDocumentCommit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuContents = New System.Windows.Forms.ToolStripMenuItem
		Me.sep2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
		Me.tlbMain = New System.Windows.Forms.ToolStrip
		Me._tlbMain_Button1 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button2 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button3 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button4 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button5 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button6 = New System.Windows.Forms.ToolStripSeparator
		Me._tlbMain_Button7 = New System.Windows.Forms.ToolStripButton
		Me._tlbMain_Button8 = New System.Windows.Forms.ToolStripButton
		Me.picSplitBar = New System.Windows.Forms.PictureBox
		Me.picBorder = New System.Windows.Forms.PictureBox
		Me.tvwDocuments = New System.Windows.Forms.TreeView
		Me.stbMain = New System.Windows.Forms.StatusStrip
		Me._stbMain_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me._stbMain_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
		Me._stbMain_Panel3 = New System.Windows.Forms.ToolStripStatusLabel
		Me.imlToolbar = New System.Windows.Forms.ImageList
		Me.imlTree = New System.Windows.Forms.ImageList
		Me.imgSplitBar = New System.Windows.Forms.PictureBox
		Me.tlbMain.SuspendLayout()
		Me.stbMain.SuspendLayout()
		Me.SuspendLayout()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile, Me.mnuDocument, Me.mnuHelp})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileExpandAll, Me.mnuFileRefresh, Me.sep1, Me.mnuExit})
		' 
		' mnuFileExpandAll
		' 
		Me.mnuFileExpandAll.Available = True
		Me.mnuFileExpandAll.Checked = False
		Me.mnuFileExpandAll.Enabled = True
		Me.mnuFileExpandAll.Name = "mnuFileExpandAll"
		Me.mnuFileExpandAll.Text = "&Expand All"
		' 
		' mnuFileRefresh
		' 
		Me.mnuFileRefresh.Available = True
		Me.mnuFileRefresh.Checked = False
		Me.mnuFileRefresh.Enabled = True
		Me.mnuFileRefresh.Name = "mnuFileRefresh"
		Me.mnuFileRefresh.ShortcutKeys = CType(System.Windows.Forms.Keys.F5, System.Windows.Forms.Keys)
		Me.mnuFileRefresh.Text = "&Refresh"
		' 
		' sep1
		' 
		Me.sep1.Available = True
		Me.sep1.Enabled = True
		Me.sep1.Name = "sep1"
		' 
		' mnuExit
		' 
		Me.mnuExit.Available = True
		Me.mnuExit.Checked = False
		Me.mnuExit.Enabled = True
		Me.mnuExit.Name = "mnuExit"
		Me.mnuExit.Text = "E&xit"
		' 
		' mnuDocument
		' 
		Me.mnuDocument.Available = True
		Me.mnuDocument.Checked = False
		Me.mnuDocument.Enabled = True
		Me.mnuDocument.Name = "mnuDocument"
		Me.mnuDocument.Text = "&Document"
		Me.mnuDocument.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuViewPage, Me.mnuDocumentSep1, Me.mnuDocumentDelete, Me.mnuDocumentDeleteAll, Me.mnuDocumentSep2, Me.mnuDocumentCommit})
		' 
		' mnuViewPage
		' 
		Me.mnuViewPage.Available = True
		Me.mnuViewPage.Checked = False
		Me.mnuViewPage.Enabled = True
		Me.mnuViewPage.Name = "mnuViewPage"
		Me.mnuViewPage.Text = "&View Page"
		' 
		' mnuDocumentSep1
		' 
		Me.mnuDocumentSep1.Available = True
		Me.mnuDocumentSep1.Enabled = True
		Me.mnuDocumentSep1.Name = "mnuDocumentSep1"
		' 
		' mnuDocumentDelete
		' 
		Me.mnuDocumentDelete.Available = True
		Me.mnuDocumentDelete.Checked = False
		Me.mnuDocumentDelete.Enabled = True
		Me.mnuDocumentDelete.Name = "mnuDocumentDelete"
		Me.mnuDocumentDelete.Text = "&Delete"
		' 
		' mnuDocumentDeleteAll
		' 
		Me.mnuDocumentDeleteAll.Available = True
		Me.mnuDocumentDeleteAll.Checked = False
		Me.mnuDocumentDeleteAll.Enabled = True
		Me.mnuDocumentDeleteAll.Name = "mnuDocumentDeleteAll"
		Me.mnuDocumentDeleteAll.Text = "Delete &All"
		' 
		' mnuDocumentSep2
		' 
		Me.mnuDocumentSep2.Available = True
		Me.mnuDocumentSep2.Enabled = True
		Me.mnuDocumentSep2.Name = "mnuDocumentSep2"
		' 
		' mnuDocumentCommit
		' 
		Me.mnuDocumentCommit.Available = True
		Me.mnuDocumentCommit.Checked = False
		Me.mnuDocumentCommit.Enabled = True
		Me.mnuDocumentCommit.Name = "mnuDocumentCommit"
		Me.mnuDocumentCommit.Text = "&Commit"
		' 
		' mnuHelp
		' 
		Me.mnuHelp.Available = True
		Me.mnuHelp.Checked = False
		Me.mnuHelp.Enabled = True
		Me.mnuHelp.Name = "mnuHelp"
		Me.mnuHelp.Text = "&Help"
		Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuContents, Me.sep2, Me.mnuHelpAbout})
		' 
		' mnuContents
		' 
		Me.mnuContents.Available = False
		Me.mnuContents.Checked = False
		Me.mnuContents.Enabled = True
		Me.mnuContents.Name = "mnuContents"
		Me.mnuContents.ShortcutKeys = CType(System.Windows.Forms.Keys.F1, System.Windows.Forms.Keys)
		Me.mnuContents.Text = "&Contents"
		' 
		' sep2
		' 
		Me.sep2.Available = False
		Me.sep2.Enabled = True
		Me.sep2.Name = "sep2"
		' 
		' mnuHelpAbout
		' 
		Me.mnuHelpAbout.Available = True
		Me.mnuHelpAbout.Checked = False
		Me.mnuHelpAbout.Enabled = True
		Me.mnuHelpAbout.Name = "mnuHelpAbout"
		Me.mnuHelpAbout.Text = "A&bout"
		' 
		' tlbMain
		' 
		Me.tlbMain.Dock = System.Windows.Forms.DockStyle.Top
		Me.tlbMain.ImageList = imlToolbar
		Me.tlbMain.Location = New System.Drawing.Point(0, 24)
		Me.tlbMain.Name = "tlbMain"
		Me.tlbMain.ShowItemToolTips = True
		Me.tlbMain.Size = New System.Drawing.Size(619, 28)
		Me.tlbMain.TabIndex = 4
		Me.tlbMain.Items.Add(Me._tlbMain_Button1)
		Me.tlbMain.Items.Add(Me._tlbMain_Button2)
		Me.tlbMain.Items.Add(Me._tlbMain_Button3)
		Me.tlbMain.Items.Add(Me._tlbMain_Button4)
		Me.tlbMain.Items.Add(Me._tlbMain_Button5)
		Me.tlbMain.Items.Add(Me._tlbMain_Button7)
		Me.tlbMain.Items.Add(Me._tlbMain_Button8)
		' 
		' _tlbMain_Button1
		' 
		Me._tlbMain_Button1.AutoSize = False
		Me._tlbMain_Button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button1.ImageIndex = 6
		Me._tlbMain_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button1.Name = "expandall"
		Me._tlbMain_Button1.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button1.Tag = ""
		Me._tlbMain_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button1.ToolTipText = "Expand all"
		' 
		' _tlbMain_Button2
		' 
		Me._tlbMain_Button2.AutoSize = False
		Me._tlbMain_Button2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button2.ImageIndex = 5
		Me._tlbMain_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button2.Name = "viewpage"
		Me._tlbMain_Button2.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button2.Tag = ""
		Me._tlbMain_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button2.ToolTipText = "View page"
		' 
		' _tlbMain_Button3
		' 
		Me._tlbMain_Button3.AutoSize = False
		Me._tlbMain_Button3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button3.ImageIndex = 4
		Me._tlbMain_Button3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button3.Name = "delete"
		Me._tlbMain_Button3.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button3.Tag = ""
		Me._tlbMain_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button3.ToolTipText = "Delete "
		' 
		' _tlbMain_Button4
		' 
		Me._tlbMain_Button4.AutoSize = False
		Me._tlbMain_Button4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button4.ImageIndex = 1
		Me._tlbMain_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button4.Name = "clear"
		Me._tlbMain_Button4.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button4.Tag = ""
		Me._tlbMain_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button4.ToolTipText = "Delete all"
		' 
		' _tlbMain_Button5
		' 
		Me._tlbMain_Button5.AutoSize = False
		Me._tlbMain_Button5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button5.ImageIndex = 0
		Me._tlbMain_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button5.Name = "commit"
		Me._tlbMain_Button5.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button5.Tag = ""
		Me._tlbMain_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button5.ToolTipText = "Commit documents"
		' 
		' _tlbMain_Button6
		' 
		Me._tlbMain_Button6.AutoSize = False
		Me._tlbMain_Button6.Name = "blank"
		Me._tlbMain_Button6.Size = New System.Drawing.Size(6, 22)
		Me._tlbMain_Button6.Tag = ""
		Me._tlbMain_Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' _tlbMain_Button7
		' 
		Me._tlbMain_Button7.AutoSize = False
		Me._tlbMain_Button7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button7.ImageIndex = 3
		Me._tlbMain_Button7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button7.Name = "pageprevious"
		Me._tlbMain_Button7.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button7.Tag = ""
		Me._tlbMain_Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button7.ToolTipText = "Previous page"
		' 
		' _tlbMain_Button8
		' 
		Me._tlbMain_Button8.AutoSize = False
		Me._tlbMain_Button8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._tlbMain_Button8.ImageIndex = 2
		Me._tlbMain_Button8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._tlbMain_Button8.Name = "pagenext"
		Me._tlbMain_Button8.Size = New System.Drawing.Size(24, 22)
		Me._tlbMain_Button8.Tag = ""
		Me._tlbMain_Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._tlbMain_Button8.ToolTipText = "Next page"
		' 
		' picSplitBar
		' 
		Me.picSplitBar.BackColor = System.Drawing.Color.Black
		Me.picSplitBar.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.picSplitBar.CausesValidation = True
		Me.picSplitBar.Cursor = System.Windows.Forms.Cursors.SizeWE
		Me.picSplitBar.Dock = System.Windows.Forms.DockStyle.None
		Me.picSplitBar.Enabled = True
		Me.picSplitBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picSplitBar.Location = New System.Drawing.Point(224, 56)
		Me.picSplitBar.Name = "picSplitBar"
		Me.picSplitBar.Size = New System.Drawing.Size(4, 328)
		Me.picSplitBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.picSplitBar.TabIndex = 2
		Me.picSplitBar.TabStop = True
		Me.picSplitBar.Visible = False
		' 
		' picBorder
		' 
		Me.picBorder.BackColor = System.Drawing.SystemColors.Control
		Me.picBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picBorder.CausesValidation = True
		Me.picBorder.Cursor = System.Windows.Forms.Cursors.Default
		Me.picBorder.Dock = System.Windows.Forms.DockStyle.None
		Me.picBorder.Enabled = True
		Me.picBorder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picBorder.Location = New System.Drawing.Point(240, 56)
		Me.picBorder.Name = "picBorder"
		Me.picBorder.Size = New System.Drawing.Size(281, 329)
		Me.picBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.picBorder.TabIndex = 3
		Me.picBorder.TabStop = True
		Me.picBorder.Visible = True
		' 
		' tvwDocuments
		' 
		Me.tvwDocuments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvwDocuments.CausesValidation = True
		Me.tvwDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvwDocuments.ImageList = imlTree
		Me.tvwDocuments.Indent = 24
		Me.tvwDocuments.LabelEdit = False
		Me.tvwDocuments.LabelEdit = True
		Me.tvwDocuments.Location = New System.Drawing.Point(0, 56)
		Me.tvwDocuments.Name = "tvwDocuments"
		Me.tvwDocuments.Size = New System.Drawing.Size(201, 329)
		Me.tvwDocuments.TabIndex = 1
		' 
		' stbMain
		' 
		Me.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbMain.Location = New System.Drawing.Point(0, 401)
		Me.stbMain.Name = "stbMain"
		Me.stbMain.ShowItemToolTips = True
		Me.stbMain.Size = New System.Drawing.Size(619, 20)
		Me.stbMain.TabIndex = 0
		Me.stbMain.Text = ""
		Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbMain_Panel1})
		Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbMain_Panel2})
		Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbMain_Panel3})
		' 
		' _stbMain_Panel1
		' 
		Me._stbMain_Panel1.AutoSize = False
		Me._stbMain_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbMain_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbMain_Panel1.DoubleClickEnabled = True
		Me._stbMain_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbMain_Panel1.Name = ""
		Me._stbMain_Panel1.Size = New System.Drawing.Size(167, 20)
		Me._stbMain_Panel1.Tag = ""
		Me._stbMain_Panel1.Text = "Total Documents "
		Me._stbMain_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbMain_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' _stbMain_Panel2
		' 
		Me._stbMain_Panel2.AutoSize = True
		Me._stbMain_Panel2.AutoSize = False
		Me._stbMain_Panel2.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbMain_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbMain_Panel2.DoubleClickEnabled = True
		Me._stbMain_Panel2.Margin = New System.Windows.Forms.Padding(0)
		Me._stbMain_Panel2.Name = ""
		Me._stbMain_Panel2.Size = New System.Drawing.Size(314, 20)
		Me._stbMain_Panel2.Tag = ""
		Me._stbMain_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbMain_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' _stbMain_Panel3
		' 
		Me._stbMain_Panel3.AutoSize = False
		Me._stbMain_Panel3.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbMain_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbMain_Panel3.DoubleClickEnabled = True
		Me._stbMain_Panel3.Margin = New System.Windows.Forms.Padding(0)
		Me._stbMain_Panel3.Name = ""
		Me._stbMain_Panel3.Size = New System.Drawing.Size(117, 20)
		Me._stbMain_Panel3.Tag = ""
		Me._stbMain_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbMain_Panel3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' imlToolbar
		' 
		Me.imlToolbar.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlToolbar.ImageStream = CType(resources.GetObject("imlToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlToolbar.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlToolbar.Images.SetKeyName(0, "")
		Me.imlToolbar.Images.SetKeyName(1, "")
		Me.imlToolbar.Images.SetKeyName(2, "")
		Me.imlToolbar.Images.SetKeyName(3, "")
		Me.imlToolbar.Images.SetKeyName(4, "")
		Me.imlToolbar.Images.SetKeyName(5, "")
		Me.imlToolbar.Images.SetKeyName(6, "")
		' 
		' imlTree
		' 
		Me.imlTree.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlTree.ImageStream = CType(resources.GetObject("imlTree.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlTree.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlTree.Images.SetKeyName(0, "open")
		Me.imlTree.Images.SetKeyName(1, "document")
		Me.imlTree.Images.SetKeyName(2, "page")
		Me.imlTree.Images.SetKeyName(3, "closed")
		' 
		' imgSplitBar
		' 
		Me.imgSplitBar.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgSplitBar.Cursor = System.Windows.Forms.Cursors.SizeWE
		Me.imgSplitBar.Enabled = True
		Me.imgSplitBar.Location = New System.Drawing.Point(208, 56)
		Me.imgSplitBar.Name = "imgSplitBar"
		Me.imgSplitBar.Size = New System.Drawing.Size(3, 327)
		Me.imgSplitBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgSplitBar.Visible = True
		' 
		' frmInterface
		' 
		Me.Text = "DocuMaster Enterprise - ViewBatch"
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(619, 421)
		Me.ControlBox = True
		Me.Controls.Add(Me.tlbMain)
		Me.Controls.Add(Me.picSplitBar)
		Me.Controls.Add(Me.picBorder)
		Me.Controls.Add(Me.tvwDocuments)
		Me.Controls.Add(Me.stbMain)
		Me.Controls.Add(Me.imgSplitBar)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 42)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.tlbMain.ResumeLayout(False)
		Me.stbMain.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class