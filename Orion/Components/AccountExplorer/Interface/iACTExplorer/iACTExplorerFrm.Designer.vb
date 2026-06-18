<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwExplorer_InitializeColumnKeys()
		Form_Initialize_Renamed()
	End Sub
	Private Sub Ctx_mnuExpT_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuExpT.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'TODO:MILAN::
        'Ctx_mnuExpT.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuExpT.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuExpT.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuExpT_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuExpT.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuExpT.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuExpT.DropDownItems.Add(item)
		Next item
	End Sub
	Private Sub Ctx_mnuListV_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuListV.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'TODO:MILAN::
        'Ctx_mnuListV.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuListV.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuListV.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuListV_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuListV.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuListV.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuListV.DropDownItems.Add(item)
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
	Public WithEvents mnuExplorerExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExplorer As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTCut As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTPaste As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeparator1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExpTMapFolder As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTUnmapFolder As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeparator2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExpTCreateFolder As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTCreateAccount As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpTRename As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator3 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExpTSecurity As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator4 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuExpGroupForGlExport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpT As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpExplorerHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpHelpTopics As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExpHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListVProperties As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListVExtras As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListVDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListV As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Private WithEvents _tbarExplorer_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbarExplorer_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbarExplorer_Button3 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _tbarExplorer_Button4 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbarExplorer_Button5 As System.Windows.Forms.ToolStripButton
	Private WithEvents _tbarExplorer_Button6 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _tbarExplorer_Button7 As System.Windows.Forms.ToolStripButton
	Public WithEvents tbarExplorer As System.Windows.Forms.ToolStrip
	Private WithEvents _sbarMain_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents _sbarMain_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sbarMain As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwExplorer_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwExplorer_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwExplorer_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwExplorer_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwExplorer As System.Windows.Forms.ListView
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents tvwExplorer As System.Windows.Forms.TreeView
	Public WithEvents picSplit As System.Windows.Forms.PictureBox
	Public WithEvents lblExplorerTree As System.Windows.Forms.Label
	Public WithEvents lblExplorerList As System.Windows.Forms.Label
	Public WithEvents imgExplorer As System.Windows.Forms.ImageList
	Public WithEvents imgToolbar As System.Windows.Forms.ImageList
	Public WithEvents Ctx_mnuExpT As System.Windows.Forms.ContextMenuStrip
	Public WithEvents Ctx_mnuListV As System.Windows.Forms.ContextMenuStrip
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuExplorer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExplorerExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpT = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTMapFolder = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTUnmapFolder = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExpTCreateFolder = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTCreateAccount = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTRename = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExpTSecurity = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpExplorerHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpHelpTopics = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuListV = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuListVProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuListVExtras = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuListVDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSeperator4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExpGroupForGlExport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTCut = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExpTPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.tbarExplorer = New System.Windows.Forms.ToolStrip
        Me.imgToolbar = New System.Windows.Forms.ImageList(Me.components)
        Me._tbarExplorer_Button1 = New System.Windows.Forms.ToolStripButton
        Me._tbarExplorer_Button2 = New System.Windows.Forms.ToolStripButton
        Me._tbarExplorer_Button4 = New System.Windows.Forms.ToolStripButton
        Me._tbarExplorer_Button5 = New System.Windows.Forms.ToolStripButton
        Me._tbarExplorer_Button7 = New System.Windows.Forms.ToolStripButton
        Me._tbarExplorer_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me._tbarExplorer_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me.sbarMain = New System.Windows.Forms.StatusStrip
        Me._sbarMain_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me._sbarMain_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwExplorer = New System.Windows.Forms.ListView
        Me._lvwExplorer_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwExplorer_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwExplorer_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwExplorer_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.imgExplorer = New System.Windows.Forms.ImageList(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tvwExplorer = New System.Windows.Forms.TreeView
        Me.picSplit = New System.Windows.Forms.PictureBox
        Me.lblExplorerTree = New System.Windows.Forms.Label
        Me.lblExplorerList = New System.Windows.Forms.Label
        Me.Ctx_mnuExpT = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuListV = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.tbarExplorer.SuspendLayout()
        Me.sbarMain.SuspendLayout()
        CType(Me.picSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExplorer, Me.mnuExpT, Me.mnuExpHelp, Me.mnuListV})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(748, 24)
        Me.MainMenu1.TabIndex = 7
        '
        'mnuExplorer
        '
        Me.mnuExplorer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExplorerExit})
        Me.mnuExplorer.Name = "mnuExplorer"
        Me.mnuExplorer.Size = New System.Drawing.Size(59, 20)
        Me.mnuExplorer.Text = "&Explorer"
        '
        'mnuExplorerExit
        '
        Me.mnuExplorerExit.Name = "mnuExplorerExit"
        Me.mnuExplorerExit.Size = New System.Drawing.Size(92, 22)
        Me.mnuExplorerExit.Text = "E&xit"
        '
        'mnuExpT
        '
        Me.mnuExpT.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExpTMapFolder, Me.mnuExpTUnmapFolder, Me.mnuSeparator2, Me.mnuExpTCreateFolder, Me.mnuExpTCreateAccount, Me.mnuExpTDelete, Me.mnuExpTRename, Me.mnuSeperator3, Me.mnuExpTSecurity})
        Me.mnuExpT.Name = "mnuExpT"
        Me.mnuExpT.Size = New System.Drawing.Size(37, 20)
        Me.mnuExpT.Text = "&Edit"
        '
        'mnuExpTMapFolder
        '
        Me.mnuExpTMapFolder.Name = "mnuExpTMapFolder"
        Me.mnuExpTMapFolder.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTMapFolder.Text = "Map F&older"
        '
        'mnuExpTUnmapFolder
        '
        Me.mnuExpTUnmapFolder.Name = "mnuExpTUnmapFolder"
        Me.mnuExpTUnmapFolder.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTUnmapFolder.Text = "&Unmap Folder"
        '
        'mnuSeparator2
        '
        Me.mnuSeparator2.Name = "mnuSeparator2"
        Me.mnuSeparator2.Size = New System.Drawing.Size(146, 6)
        '
        'mnuExpTCreateFolder
        '
        Me.mnuExpTCreateFolder.Name = "mnuExpTCreateFolder"
        Me.mnuExpTCreateFolder.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTCreateFolder.Text = "Create &Folder"
        '
        'mnuExpTCreateAccount
        '
        Me.mnuExpTCreateAccount.Name = "mnuExpTCreateAccount"
        Me.mnuExpTCreateAccount.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTCreateAccount.Text = "Create &Account"
        '
        'mnuExpTDelete
        '
        Me.mnuExpTDelete.Name = "mnuExpTDelete"
        Me.mnuExpTDelete.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTDelete.Text = "&Delete"
        '
        'mnuExpTRename
        '
        Me.mnuExpTRename.Name = "mnuExpTRename"
        Me.mnuExpTRename.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTRename.Text = "Rena&me"
        '
        'mnuSeperator3
        '
        Me.mnuSeperator3.Name = "mnuSeperator3"
        Me.mnuSeperator3.Size = New System.Drawing.Size(146, 6)
        '
        'mnuExpTSecurity
        '
        Me.mnuExpTSecurity.Name = "mnuExpTSecurity"
        Me.mnuExpTSecurity.Size = New System.Drawing.Size(149, 22)
        Me.mnuExpTSecurity.Text = "Security"
        '
        'mnuExpHelp
        '
        Me.mnuExpHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExpExplorerHelp, Me.mnuExpHelpTopics})
        Me.mnuExpHelp.Name = "mnuExpHelp"
        Me.mnuExpHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuExpHelp.Text = "&Help"
        '
        'mnuExpExplorerHelp
        '
        Me.mnuExpExplorerHelp.Name = "mnuExpExplorerHelp"
        Me.mnuExpExplorerHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuExpExplorerHelp.Size = New System.Drawing.Size(186, 22)
        Me.mnuExpExplorerHelp.Text = "Orion Explorer &Help"
        '
        'mnuExpHelpTopics
        '
        Me.mnuExpHelpTopics.Name = "mnuExpHelpTopics"
        Me.mnuExpHelpTopics.Size = New System.Drawing.Size(186, 22)
        Me.mnuExpHelpTopics.Text = "&Orion Help Topics"
        '
        'mnuListV
        '
        Me.mnuListV.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuListVProperties, Me.mnuListVExtras, Me.mnuListVDelete})
        Me.mnuListV.Enabled = False
        Me.mnuListV.Name = "mnuListV"
        Me.mnuListV.Size = New System.Drawing.Size(60, 20)
        Me.mnuListV.Text = "List View"
        Me.mnuListV.Visible = False
        '
        'mnuListVProperties
        '
        Me.mnuListVProperties.Name = "mnuListVProperties"
        Me.mnuListVProperties.Size = New System.Drawing.Size(123, 22)
        Me.mnuListVProperties.Text = "&Properties"
        '
        'mnuListVExtras
        '
        Me.mnuListVExtras.Name = "mnuListVExtras"
        Me.mnuListVExtras.Size = New System.Drawing.Size(123, 22)
        Me.mnuListVExtras.Text = "&Extras"
        '
        'mnuListVDelete
        '
        Me.mnuListVDelete.Name = "mnuListVDelete"
        Me.mnuListVDelete.Size = New System.Drawing.Size(123, 22)
        Me.mnuListVDelete.Text = "&Delete"
        '
        'mnuSeparator1
        '
        Me.mnuSeparator1.Name = "mnuSeparator1"
        Me.mnuSeparator1.Size = New System.Drawing.Size(166, 6)
        '
        'mnuSeperator4
        '
        Me.mnuSeperator4.Name = "mnuSeperator4"
        Me.mnuSeperator4.Size = New System.Drawing.Size(166, 6)
        '
        'mnuExpGroupForGlExport
        '
        Me.mnuExpGroupForGlExport.Name = "mnuExpGroupForGlExport"
        Me.mnuExpGroupForGlExport.Size = New System.Drawing.Size(169, 22)
        Me.mnuExpGroupForGlExport.Text = "Group For Gl Export"
        '
        'mnuExpTCut
        '
        Me.mnuExpTCut.Name = "mnuExpTCut"
        Me.mnuExpTCut.Size = New System.Drawing.Size(169, 22)
        Me.mnuExpTCut.Text = "Cu&t"
        '
        'mnuExpTCopy
        '
        Me.mnuExpTCopy.Name = "mnuExpTCopy"
        Me.mnuExpTCopy.Size = New System.Drawing.Size(169, 22)
        Me.mnuExpTCopy.Text = "&Copy"
        '
        'mnuExpTPaste
        '
        Me.mnuExpTPaste.Name = "mnuExpTPaste"
        Me.mnuExpTPaste.Size = New System.Drawing.Size(169, 22)
        Me.mnuExpTPaste.Text = "&Paste"
        '
        'tbarExplorer
        '
        Me.tbarExplorer.ImageList = Me.imgToolbar
        Me.tbarExplorer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._tbarExplorer_Button1, Me._tbarExplorer_Button2, Me._tbarExplorer_Button4, Me._tbarExplorer_Button5, Me._tbarExplorer_Button7})
        Me.tbarExplorer.Location = New System.Drawing.Point(0, 24)
        Me.tbarExplorer.Name = "tbarExplorer"
        Me.tbarExplorer.Size = New System.Drawing.Size(748, 25)
        Me.tbarExplorer.TabIndex = 0
        '
        'imgToolbar
        '
        Me.imgToolbar.ImageStream = CType(resources.GetObject("imgToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgToolbar.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgToolbar.Images.SetKeyName(0, "")
        Me.imgToolbar.Images.SetKeyName(1, "")
        Me.imgToolbar.Images.SetKeyName(2, "")
        Me.imgToolbar.Images.SetKeyName(3, "")
        Me.imgToolbar.Images.SetKeyName(4, "")
        Me.imgToolbar.Images.SetKeyName(5, "")
        Me.imgToolbar.Images.SetKeyName(6, "")
        Me.imgToolbar.Images.SetKeyName(7, "")
        Me.imgToolbar.Images.SetKeyName(8, "")
        Me.imgToolbar.Images.SetKeyName(9, "")
        Me.imgToolbar.Images.SetKeyName(10, "")
        '
        '_tbarExplorer_Button1
        '
        Me._tbarExplorer_Button1.AutoSize = False
        Me._tbarExplorer_Button1.ImageIndex = 1
        Me._tbarExplorer_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tbarExplorer_Button1.Name = "_tbarExplorer_Button1"
        Me._tbarExplorer_Button1.Size = New System.Drawing.Size(24, 22)
        Me._tbarExplorer_Button1.Tag = ""
        Me._tbarExplorer_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tbarExplorer_Button1.ToolTipText = "Map Folder"
        '
        '_tbarExplorer_Button2
        '
        Me._tbarExplorer_Button2.AutoSize = False
        Me._tbarExplorer_Button2.ImageIndex = 2
        Me._tbarExplorer_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tbarExplorer_Button2.Name = "_tbarExplorer_Button2"
        Me._tbarExplorer_Button2.Size = New System.Drawing.Size(24, 22)
        Me._tbarExplorer_Button2.Tag = ""
        Me._tbarExplorer_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tbarExplorer_Button2.ToolTipText = "Unmap Folder"
        '
        '_tbarExplorer_Button4
        '
        Me._tbarExplorer_Button4.AutoSize = False
        Me._tbarExplorer_Button4.ImageIndex = 9
        Me._tbarExplorer_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tbarExplorer_Button4.Name = "_tbarExplorer_Button4"
        Me._tbarExplorer_Button4.Size = New System.Drawing.Size(24, 22)
        Me._tbarExplorer_Button4.Tag = ""
        Me._tbarExplorer_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tbarExplorer_Button4.ToolTipText = "Create Folder"
        '
        '_tbarExplorer_Button5
        '
        Me._tbarExplorer_Button5.AutoSize = False
        Me._tbarExplorer_Button5.ImageIndex = 10
        Me._tbarExplorer_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tbarExplorer_Button5.Name = "_tbarExplorer_Button5"
        Me._tbarExplorer_Button5.Size = New System.Drawing.Size(24, 22)
        Me._tbarExplorer_Button5.Tag = ""
        Me._tbarExplorer_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tbarExplorer_Button5.ToolTipText = "Create Account"
        '
        '_tbarExplorer_Button7
        '
        Me._tbarExplorer_Button7.AutoSize = False
        Me._tbarExplorer_Button7.ImageIndex = 4
        Me._tbarExplorer_Button7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tbarExplorer_Button7.Name = "_tbarExplorer_Button7"
        Me._tbarExplorer_Button7.Size = New System.Drawing.Size(24, 22)
        Me._tbarExplorer_Button7.Tag = ""
        Me._tbarExplorer_Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tbarExplorer_Button7.ToolTipText = "Delete"
        Me._tbarExplorer_Button7.Visible = False
        '
        '_tbarExplorer_Button3
        '
        Me._tbarExplorer_Button3.AutoSize = False
        Me._tbarExplorer_Button3.Name = "_tbarExplorer_Button3"
        Me._tbarExplorer_Button3.Size = New System.Drawing.Size(6, 22)
        Me._tbarExplorer_Button3.Tag = ""
        '
        '_tbarExplorer_Button6
        '
        Me._tbarExplorer_Button6.AutoSize = False
        Me._tbarExplorer_Button6.Name = "_tbarExplorer_Button6"
        Me._tbarExplorer_Button6.Size = New System.Drawing.Size(6, 22)
        Me._tbarExplorer_Button6.Tag = ""
        '
        'sbarMain
        '
        Me.sbarMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sbarMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._sbarMain_Panel1, Me._sbarMain_Panel2})
        Me.sbarMain.Location = New System.Drawing.Point(0, 590)
        Me.sbarMain.Name = "sbarMain"
        Me.sbarMain.ShowItemToolTips = True
        Me.sbarMain.Size = New System.Drawing.Size(748, 22)
        Me.sbarMain.TabIndex = 1
        '
        '_sbarMain_Panel1
        '
        Me._sbarMain_Panel1.AutoSize = False
        Me._sbarMain_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._sbarMain_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._sbarMain_Panel1.DoubleClickEnabled = True
        Me._sbarMain_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._sbarMain_Panel1.Name = "_sbarMain_Panel1"
        Me._sbarMain_Panel1.Size = New System.Drawing.Size(193, 22)
        Me._sbarMain_Panel1.Tag = ""
        Me._sbarMain_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_sbarMain_Panel2
        '
        Me._sbarMain_Panel2.AutoSize = False
        Me._sbarMain_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._sbarMain_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._sbarMain_Panel2.DoubleClickEnabled = True
        Me._sbarMain_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._sbarMain_Panel2.Name = "_sbarMain_Panel2"
        Me._sbarMain_Panel2.Size = New System.Drawing.Size(578, 21)
        Me._sbarMain_Panel2.Tag = ""
        Me._sbarMain_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwExplorer
        '
        Me.lvwExplorer.AllowDrop = True
        Me.lvwExplorer.BackColor = System.Drawing.SystemColors.Window
        Me.lvwExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwExplorer.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwExplorer_ColumnHeader_1, Me._lvwExplorer_ColumnHeader_2, Me._lvwExplorer_ColumnHeader_3, Me._lvwExplorer_ColumnHeader_4})
        Me.lvwExplorer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwExplorer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwExplorer.Location = New System.Drawing.Point(218, 78)
        Me.lvwExplorer.Name = "lvwExplorer"
        Me.lvwExplorer.Size = New System.Drawing.Size(530, 509)
        Me.lvwExplorer.SmallImageList = Me.imgExplorer
        Me.lvwExplorer.TabIndex = 4
        Me.lvwExplorer.UseCompatibleStateImageBehavior = False
        Me.lvwExplorer.View = System.Windows.Forms.View.Details
        '
        '_lvwExplorer_ColumnHeader_1
        '
        Me._lvwExplorer_ColumnHeader_1.Tag = ""
        Me._lvwExplorer_ColumnHeader_1.Text = "Short Name"
        Me._lvwExplorer_ColumnHeader_1.Width = 97
        '
        '_lvwExplorer_ColumnHeader_2
        '
        Me._lvwExplorer_ColumnHeader_2.Tag = ""
        Me._lvwExplorer_ColumnHeader_2.Text = "Type"
        Me._lvwExplorer_ColumnHeader_2.Width = 97
        '
        '_lvwExplorer_ColumnHeader_3
        '
        Me._lvwExplorer_ColumnHeader_3.Tag = ""
        Me._lvwExplorer_ColumnHeader_3.Text = "Name"
        Me._lvwExplorer_ColumnHeader_3.Width = 97
        '
        '_lvwExplorer_ColumnHeader_4
        '
        Me._lvwExplorer_ColumnHeader_4.Tag = ""
        Me._lvwExplorer_ColumnHeader_4.Text = "Sub Branch"
        Me._lvwExplorer_ColumnHeader_4.Width = 97
        '
        'imgExplorer
        '
        Me.imgExplorer.ImageStream = CType(resources.GetObject("imgExplorer.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgExplorer.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.imgExplorer.Images.SetKeyName(0, "FolderClosed")
        Me.imgExplorer.Images.SetKeyName(1, "FolderOpen")
        Me.imgExplorer.Images.SetKeyName(2, "")
        Me.imgExplorer.Images.SetKeyName(3, "")
        Me.imgExplorer.Images.SetKeyName(4, "")
        Me.imgExplorer.Images.SetKeyName(5, "")
        Me.imgExplorer.Images.SetKeyName(6, "")
        Me.imgExplorer.Images.SetKeyName(7, "")
        Me.imgExplorer.Images.SetKeyName(8, "")
        '
        'tvwExplorer
        '
        Me.tvwExplorer.AllowDrop = True
        Me.tvwExplorer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwExplorer.HideSelection = False
        Me.tvwExplorer.ImageIndex = 0
        Me.tvwExplorer.ImageList = Me.imgExplorer
        Me.tvwExplorer.Indent = 30
        Me.tvwExplorer.Location = New System.Drawing.Point(2, 78)
        Me.tvwExplorer.Name = "tvwExplorer"
        Me.tvwExplorer.SelectedImageIndex = 0
        Me.tvwExplorer.Size = New System.Drawing.Size(211, 509)
        Me.tvwExplorer.TabIndex = 2
        '
        'picSplit
        '
        Me.picSplit.BackColor = System.Drawing.SystemColors.Control
        Me.picSplit.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.picSplit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSplit.Location = New System.Drawing.Point(214, 56)
        Me.picSplit.Name = "picSplit"
        Me.picSplit.Size = New System.Drawing.Size(3, 281)
        Me.picSplit.TabIndex = 3
        Me.picSplit.TabStop = False
        '
        'lblExplorerTree
        '
        Me.lblExplorerTree.BackColor = System.Drawing.SystemColors.Control
        Me.lblExplorerTree.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblExplorerTree.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExplorerTree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExplorerTree.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExplorerTree.Location = New System.Drawing.Point(3, 58)
        Me.lblExplorerTree.Name = "lblExplorerTree"
        Me.lblExplorerTree.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExplorerTree.Size = New System.Drawing.Size(209, 17)
        Me.lblExplorerTree.TabIndex = 6
        Me.lblExplorerTree.Text = "All Folders"
        '
        'lblExplorerList
        '
        Me.lblExplorerList.BackColor = System.Drawing.SystemColors.Control
        Me.lblExplorerList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblExplorerList.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExplorerList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExplorerList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExplorerList.Location = New System.Drawing.Point(219, 58)
        Me.lblExplorerList.Name = "lblExplorerList"
        Me.lblExplorerList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExplorerList.Size = New System.Drawing.Size(293, 17)
        Me.lblExplorerList.TabIndex = 5
        Me.lblExplorerList.Text = "Contents of "
        '
        'Ctx_mnuExpT
        '
        Me.Ctx_mnuExpT.Name = "Ctx_mnuExpT"
        Me.Ctx_mnuExpT.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuListV
        '
        Me.Ctx_mnuListV.Name = "Ctx_mnuListV"
        Me.Ctx_mnuListV.Size = New System.Drawing.Size(61, 4)
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(748, 612)
        Me.Controls.Add(Me.tbarExplorer)
        Me.Controls.Add(Me.sbarMain)
        Me.Controls.Add(Me.lvwExplorer)
        Me.Controls.Add(Me.tvwExplorer)
        Me.Controls.Add(Me.picSplit)
        Me.Controls.Add(Me.lblExplorerTree)
        Me.Controls.Add(Me.lblExplorerList)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(11, 49)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Account Structure Tree"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tbarExplorer.ResumeLayout(False)
        Me.tbarExplorer.PerformLayout()
        Me.sbarMain.ResumeLayout(False)
        Me.sbarMain.PerformLayout()
        CType(Me.picSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwExplorer_InitializeColumnKeys()
		Me._lvwExplorer_ColumnHeader_1.Name = ""
		Me._lvwExplorer_ColumnHeader_2.Name = ""
		Me._lvwExplorer_ColumnHeader_3.Name = ""
		Me._lvwExplorer_ColumnHeader_4.Name = ""
	End Sub
#End Region 
End Class