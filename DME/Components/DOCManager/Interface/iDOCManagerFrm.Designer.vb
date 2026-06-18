<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializelblTitleMain()
		InitializelblTitleFind()
		InitializelblTitleFav()
		InitializelblTitleBC()
		lvwBCDocs_InitializeColumnKeys()
		lvwDocList_InitializeColumnKeys()
		lvwKeyWords_InitializeColumnKeys()
		lvwAnnotations_InitializeColumnKeys()
		Form_Initialize_Renamed()
	End Sub
    Private Sub Ctx_mnuPop_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuPop.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuPop.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuPop.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuPop.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuPop_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuPop.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuPop.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuPop.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub Ctx_mnuPop2_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuPop2.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuPop2.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuPop2.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuPop2.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuPop2_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuPop2.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuPop2.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuPop2.DropDownItems.Add(item)
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
    Public WithEvents mnuFileOpenDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileFilter As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSelect As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileNewFolder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents fsep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileNewTXT As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileNewDOC As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileNew As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileImport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileEmail As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileArchive As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSaveAs As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents fsep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileRename As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileInformation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents fsep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFilePrint As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents fsep4 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileScan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents fsep5 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEditCut As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEditCopy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEditPaste As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents esep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuEditSelectAll As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewMain As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewFavourites As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewFindResults As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewBC As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewExtrasKeywords As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewExtrasAnnotations As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewExtrasPages As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents vsep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuViewLargeIcons As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewSmallIcons As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents vsep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuViewExpand As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents vsep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuViewRefresh As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUtilitiesKeywords As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUtilitiesDocNames As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUtilitiesUserAccess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUtilitiesEditAccess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuUtilities As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsFindClear As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents tsep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuToolsPassword As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsAccess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsClearCache As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents tsep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuToolsAddAnn As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsAddKeyword As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents tsep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuToolsSetHome As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuToolsGoHome As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents tsep4 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuViewOptions As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopFilter As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopSelect As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopOpenDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopEmail As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopArchive As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopSubFolder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopSetHome As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopAddKeyword As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopAddAnn As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopPassword As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopAccess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep4 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopCut As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopCopy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopPaste As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep5 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopRename As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopInformation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents psep6 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopScan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPop As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopNewFolder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPop2 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Private WithEvents _tlbMain_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button7 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button10 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button15 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button17 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button20 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button23 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _tlbMain_Button28 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents txtAcrobatDDE As System.Windows.Forms.TextBox
    'Public WithEvents IkFile1 As AxIMGKIT6Lib.AxIkFile
    'Public WithEvents IkCommon1 As AxIMGKIT6Lib.AxIkCommon
    'Public WithEvents IkPrint1 As AxIMGKIT6Lib.AxIkPrint
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    Public WithEvents rtbHidden As System.Windows.Forms.RichTextBox
    Public WithEvents picSplitter As System.Windows.Forms.PictureBox
    Public WithEvents lvwDocsOnly As System.Windows.Forms.ListView
    Public WithEvents lvwBCDocsOnly As System.Windows.Forms.ListView
    Public WithEvents tvwFav As System.Windows.Forms.TreeView
    Public WithEvents lvwPages As System.Windows.Forms.ListView
    Private WithEvents _lvwAnnotations_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAnnotations_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAnnotations_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAnnotations As System.Windows.Forms.ListView
    Private WithEvents _lvwKeyWords_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwKeyWords_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwKeyWords_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwKeyWords As System.Windows.Forms.ListView
    Public dlgMainOpen As System.Windows.Forms.OpenFileDialog
    Public dlgMainSave As System.Windows.Forms.SaveFileDialog
    Public dlgMainPrint As System.Windows.Forms.PrintDialog
    Private WithEvents _lblTitleBC_0 As System.Windows.Forms.Label
    Private WithEvents _lblTitleFind_0 As System.Windows.Forms.Label
    Private WithEvents _lblTitleMain_0 As System.Windows.Forms.Label
    Public WithEvents imgMain As System.Windows.Forms.ImageList
    Private WithEvents _staContents_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _staContents_Panel2 As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _staContents_Panel3 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents staContents As System.Windows.Forms.StatusStrip
    Public WithEvents tvwMain As System.Windows.Forms.TreeView
    Public WithEvents tvwFind As System.Windows.Forms.TreeView
    Public WithEvents tvwBCMain As System.Windows.Forms.TreeView
    Private WithEvents _lvwDocList_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwDocList_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwDocList_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwDocList_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwDocList As System.Windows.Forms.ListView
    Private WithEvents _tlbBCDocsButtons_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbBCDocsButtons_Button2 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbBCDocsButtons_Button3 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbBCDocsButtons_Button4 As System.Windows.Forms.ToolStripButton
    Public WithEvents tlbBCDocsButtons As System.Windows.Forms.ToolStrip
    Private WithEvents _lvwBCDocs_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBCDocs_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwBCDocs_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwBCDocs As System.Windows.Forms.ListView
    Public WithEvents lblBCDocs As System.Windows.Forms.Label
    Public WithEvents frBCDocs As System.Windows.Forms.Panel
    Public WithEvents imgBCSplitterH As System.Windows.Forms.PictureBox
    Public WithEvents imlMainLarge As System.Windows.Forms.ImageList
    Public WithEvents imgSplitterV As System.Windows.Forms.PictureBox
    Public WithEvents imgSplitterH As System.Windows.Forms.PictureBox
    Public lblTitleBC(1) As System.Windows.Forms.Label
    Public lblTitleFav(1) As System.Windows.Forms.Label
    Public lblTitleFind(1) As System.Windows.Forms.Label
    Public lblTitleMain(1) As System.Windows.Forms.Label
    Public WithEvents Ctx_mnuPop As System.Windows.Forms.ContextMenuStrip
    Public WithEvents Ctx_mnuPop2 As System.Windows.Forms.ContextMenuStrip

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileOpenDocument = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileNewFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.fsep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileNewTXT = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileNewDOC = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileEmail = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileArchive = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.fsep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.fsep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFilePrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.fsep4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.fsep5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.esep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuEditSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewMain = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewFavourites = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewFindResults = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewBC = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewExtrasKeywords = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewExtrasAnnotations = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewExtrasPages = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewLargeIcons = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewSmallIcons = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewExpand = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUtilities = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUtilitiesKeywords = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUtilitiesDocNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUtilitiesUserAccess = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUtilitiesEditAccess = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsFind = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsFindClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuToolsPassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsAccess = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsClearCache = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuToolsAddAnn = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsAddKeyword = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuToolsSetHome = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolsGoHome = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsep4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopOpenDocument = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopEmail = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopArchive = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopSubFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopFind = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopSetHome = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopAddKeyword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopAddAnn = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopPassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopAccess = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.psep6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPopScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopToSharePoint = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPop2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPopNewFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.imgMain = New System.Windows.Forms.ImageList(Me.components)
        Me._tlbMain_Button3 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button7 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button10 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button15 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button17 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button20 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button23 = New System.Windows.Forms.ToolStripSeparator()
        Me._tlbMain_Button28 = New System.Windows.Forms.ToolStripSeparator()
        Me.txtAcrobatDDE = New System.Windows.Forms.TextBox()
        Me.rtbHidden = New System.Windows.Forms.RichTextBox()
        Me.picSplitter = New System.Windows.Forms.PictureBox()
        Me.lvwDocsOnly = New System.Windows.Forms.ListView()
        Me.lvwBCDocsOnly = New System.Windows.Forms.ListView()
        Me.tvwFav = New System.Windows.Forms.TreeView()
        Me.lvwPages = New System.Windows.Forms.ListView()
        Me.lvwAnnotations = New System.Windows.Forms.ListView()
        Me._lvwAnnotations_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAnnotations_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAnnotations_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwKeyWords = New System.Windows.Forms.ListView()
        Me._lvwKeyWords_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwKeyWords_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwKeyWords_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lblTitleFind_0 = New System.Windows.Forms.Label()
        Me._lblTitleBC_0 = New System.Windows.Forms.Label()
        Me._lblTitleMain_0 = New System.Windows.Forms.Label()
        Me.dlgMainOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgMainSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgMainPrint = New System.Windows.Forms.PrintDialog()
        Me.staContents = New System.Windows.Forms.StatusStrip()
        Me._staContents_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._staContents_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._staContents_Panel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tvwMain = New System.Windows.Forms.TreeView()
        Me.tvwFind = New System.Windows.Forms.TreeView()
        Me.tvwBCMain = New System.Windows.Forms.TreeView()
        Me.lvwDocList = New System.Windows.Forms.ListView()
        Me._lvwDocList_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwDocList_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwDocList_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwDocList_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imlMainLarge = New System.Windows.Forms.ImageList(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.frBCDocs = New System.Windows.Forms.Panel()
        Me.tlbBCDocsButtons = New System.Windows.Forms.ToolStrip()
        Me._tlbBCDocsButtons_Button1 = New System.Windows.Forms.ToolStripButton()
        Me._tlbBCDocsButtons_Button2 = New System.Windows.Forms.ToolStripButton()
        Me._tlbBCDocsButtons_Button3 = New System.Windows.Forms.ToolStripButton()
        Me._tlbBCDocsButtons_Button4 = New System.Windows.Forms.ToolStripButton()
        Me.lvwBCDocs = New System.Windows.Forms.ListView()
        Me._lvwBCDocs_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBCDocs_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBCDocs_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblBCDocs = New System.Windows.Forms.Label()
        Me.imgBCSplitterH = New System.Windows.Forms.PictureBox()
        Me.imgSplitterV = New System.Windows.Forms.PictureBox()
        Me.imgSplitterH = New System.Windows.Forms.PictureBox()
        Me.Ctx_mnuPop = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuPop2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me._tlbMain_Button1 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button2 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button4 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button5 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button6 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button8 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button9 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button11 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button12 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button13 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button14 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button16 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button18 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button19 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button21 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button22 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button24 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button25 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button26 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button27 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button29 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button30 = New System.Windows.Forms.ToolStripButton()
        Me._tlbMain_Button31 = New System.Windows.Forms.ToolStripButton()
        Me.tlbMain = New System.Windows.Forms.ToolStrip()
        Me._lblTitleFav_0 = New System.Windows.Forms.Label()
        Me._lblTitleFav_1 = New System.Windows.Forms.Label()
        Me._lblTitleMain_1 = New System.Windows.Forms.Label()
        Me._lblTitleBC_1 = New System.Windows.Forms.Label()
        Me._lblTitleFind_1 = New System.Windows.Forms.Label()
        Me.picTitles = New System.Windows.Forms.PictureBox()
        Me.MainMenu1.SuspendLayout()
        CType(Me.picSplitter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.staContents.SuspendLayout()
        Me.frBCDocs.SuspendLayout()
        Me.tlbBCDocsButtons.SuspendLayout()
        CType(Me.imgBCSplitterH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSplitterV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSplitterH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlbMain.SuspendLayout()
        CType(Me.picTitles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picTitles.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuUtilities, Me.mnuTools, Me.mnuHelp, Me.mnuPop, Me.mnuPop2})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(751, 24)
        Me.MainMenu1.TabIndex = 31
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileOpenDocument, Me.mnuFileFilter, Me.mnuFileSelect, Me.mnuFileNew, Me.mnuFileImport, Me.mnuFileEmail, Me.mnuFileArchive, Me.mnuFileSaveAs, Me.fsep2, Me.mnuFileDelete, Me.mnuFileRename, Me.mnuFileInformation, Me.fsep3, Me.mnuFilePrint, Me.fsep4, Me.mnuFileScan, Me.fsep5, Me.mnuFileExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.ShortcutKeyDisplayString = ""
        Me.mnuFile.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileOpenDocument
        '
        Me.mnuFileOpenDocument.Name = "mnuFileOpenDocument"
        Me.mnuFileOpenDocument.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuFileOpenDocument.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileOpenDocument.Text = "&Open Document"
        '
        'mnuFileFilter
        '
        Me.mnuFileFilter.Name = "mnuFileFilter"
        Me.mnuFileFilter.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuFileFilter.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileFilter.Text = "&Filter"
        '
        'mnuFileSelect
        '
        Me.mnuFileSelect.Name = "mnuFileSelect"
        Me.mnuFileSelect.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuFileSelect.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileSelect.Text = "&Add Folders to View..."
        '
        'mnuFileNew
        '
        Me.mnuFileNew.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileNewFolder, Me.fsep1, Me.mnuFileNewTXT, Me.mnuFileNewDOC})
        Me.mnuFileNew.Name = "mnuFileNew"
        Me.mnuFileNew.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuFileNew.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileNew.Text = "&New"
        '
        'mnuFileNewFolder
        '
        Me.mnuFileNewFolder.Name = "mnuFileNewFolder"
        Me.mnuFileNewFolder.Size = New System.Drawing.Size(151, 22)
        Me.mnuFileNewFolder.Text = "Folder"
        '
        'fsep1
        '
        Me.fsep1.Name = "fsep1"
        Me.fsep1.Size = New System.Drawing.Size(148, 6)
        '
        'mnuFileNewTXT
        '
        Me.mnuFileNewTXT.Name = "mnuFileNewTXT"
        Me.mnuFileNewTXT.Size = New System.Drawing.Size(151, 22)
        Me.mnuFileNewTXT.Text = "Text Document"
        Me.mnuFileNewTXT.Visible = False
        '
        'mnuFileNewDOC
        '
        Me.mnuFileNewDOC.Name = "mnuFileNewDOC"
        Me.mnuFileNewDOC.Size = New System.Drawing.Size(151, 22)
        Me.mnuFileNewDOC.Text = "Word Document"
        Me.mnuFileNewDOC.Visible = False
        '
        'mnuFileImport
        '
        Me.mnuFileImport.Name = "mnuFileImport"
        Me.mnuFileImport.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.mnuFileImport.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileImport.Text = "I&mport Document..."
        '
        'mnuFileEmail
        '
        Me.mnuFileEmail.Name = "mnuFileEmail"
        Me.mnuFileEmail.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuFileEmail.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileEmail.Text = "&Email Document"
        '
        'mnuFileArchive
        '
        Me.mnuFileArchive.Name = "mnuFileArchive"
        Me.mnuFileArchive.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuFileArchive.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileArchive.Text = "Archi&ve Document"
        '
        'mnuFileSaveAs
        '
        Me.mnuFileSaveAs.Name = "mnuFileSaveAs"
        Me.mnuFileSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuFileSaveAs.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileSaveAs.Text = "&Save As..."
        '
        'fsep2
        '
        Me.fsep2.Name = "fsep2"
        Me.fsep2.Size = New System.Drawing.Size(238, 6)
        '
        'mnuFileDelete
        '
        Me.mnuFileDelete.Name = "mnuFileDelete"
        Me.mnuFileDelete.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuFileDelete.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileDelete.Text = "&Delete"
        '
        'mnuFileRename
        '
        Me.mnuFileRename.Name = "mnuFileRename"
        Me.mnuFileRename.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.mnuFileRename.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileRename.Text = "&Rename"
        '
        'mnuFileInformation
        '
        Me.mnuFileInformation.Name = "mnuFileInformation"
        Me.mnuFileInformation.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuFileInformation.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileInformation.Text = "&Information..."
        '
        'fsep3
        '
        Me.fsep3.Name = "fsep3"
        Me.fsep3.Size = New System.Drawing.Size(238, 6)
        '
        'mnuFilePrint
        '
        Me.mnuFilePrint.Name = "mnuFilePrint"
        Me.mnuFilePrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuFilePrint.Size = New System.Drawing.Size(241, 22)
        Me.mnuFilePrint.Text = "&Print..."
        '
        'fsep4
        '
        Me.fsep4.Name = "fsep4"
        Me.fsep4.Size = New System.Drawing.Size(238, 6)
        '
        'mnuFileScan
        '
        Me.mnuFileScan.Name = "mnuFileScan"
        Me.mnuFileScan.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileScan.Text = "S&can..."
        '
        'fsep5
        '
        Me.fsep5.Name = "fsep5"
        Me.fsep5.Size = New System.Drawing.Size(238, 6)
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuFileExit.Size = New System.Drawing.Size(241, 22)
        Me.mnuFileExit.Text = "E&xit"
        '
        'mnuEdit
        '
        Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEditCut, Me.mnuEditCopy, Me.mnuEditPaste, Me.esep1, Me.mnuEditSelectAll})
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEdit.Size = New System.Drawing.Size(37, 20)
        Me.mnuEdit.Text = "&Edit"
        '
        'mnuEditCut
        '
        Me.mnuEditCut.Name = "mnuEditCut"
        Me.mnuEditCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuEditCut.Size = New System.Drawing.Size(156, 22)
        Me.mnuEditCut.Text = "Cu&t"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Name = "mnuEditCopy"
        Me.mnuEditCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuEditCopy.Size = New System.Drawing.Size(156, 22)
        Me.mnuEditCopy.Text = "&Copy"
        '
        'mnuEditPaste
        '
        Me.mnuEditPaste.Name = "mnuEditPaste"
        Me.mnuEditPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuEditPaste.Size = New System.Drawing.Size(156, 22)
        Me.mnuEditPaste.Text = "&Paste"
        '
        'esep1
        '
        Me.esep1.Name = "esep1"
        Me.esep1.Size = New System.Drawing.Size(153, 6)
        '
        'mnuEditSelectAll
        '
        Me.mnuEditSelectAll.Name = "mnuEditSelectAll"
        Me.mnuEditSelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuEditSelectAll.Size = New System.Drawing.Size(156, 22)
        Me.mnuEditSelectAll.Text = "Select &All"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewMain, Me.mnuViewFavourites, Me.mnuViewFindResults, Me.mnuViewBC, Me.mnuViewExtrasKeywords, Me.mnuViewExtrasAnnotations, Me.mnuViewExtrasPages, Me.vsep1, Me.mnuViewLargeIcons, Me.mnuViewSmallIcons, Me.mnuViewList, Me.mnuViewDetails, Me.vsep2, Me.mnuViewExpand, Me.vsep3, Me.mnuViewRefresh})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuView.Size = New System.Drawing.Size(41, 20)
        Me.mnuView.Text = "&View"
        '
        'mnuViewMain
        '
        Me.mnuViewMain.Name = "mnuViewMain"
        Me.mnuViewMain.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.mnuViewMain.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewMain.Text = "&Main"
        '
        'mnuViewFavourites
        '
        Me.mnuViewFavourites.Name = "mnuViewFavourites"
        Me.mnuViewFavourites.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewFavourites.Text = "&Favourites"
        Me.mnuViewFavourites.Visible = False
        '
        'mnuViewFindResults
        '
        Me.mnuViewFindResults.Name = "mnuViewFindResults"
        Me.mnuViewFindResults.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuViewFindResults.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewFindResults.Text = "Fin&d Results"
        '
        'mnuViewBC
        '
        Me.mnuViewBC.Name = "mnuViewBC"
        Me.mnuViewBC.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewBC.Text = "&Briefcase"
        Me.mnuViewBC.Visible = False
        '
        'mnuViewExtrasKeywords
        '
        Me.mnuViewExtrasKeywords.Name = "mnuViewExtrasKeywords"
        Me.mnuViewExtrasKeywords.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.mnuViewExtrasKeywords.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewExtrasKeywords.Text = "&Keywords"
        '
        'mnuViewExtrasAnnotations
        '
        Me.mnuViewExtrasAnnotations.Name = "mnuViewExtrasAnnotations"
        Me.mnuViewExtrasAnnotations.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuViewExtrasAnnotations.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewExtrasAnnotations.Text = "&Annotations"
        '
        'mnuViewExtrasPages
        '
        Me.mnuViewExtrasPages.Name = "mnuViewExtrasPages"
        Me.mnuViewExtrasPages.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewExtrasPages.Text = "&Pages"
        Me.mnuViewExtrasPages.Visible = False
        '
        'vsep1
        '
        Me.vsep1.Name = "vsep1"
        Me.vsep1.Size = New System.Drawing.Size(213, 6)
        '
        'mnuViewLargeIcons
        '
        Me.mnuViewLargeIcons.Name = "mnuViewLargeIcons"
        Me.mnuViewLargeIcons.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.mnuViewLargeIcons.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewLargeIcons.Text = "Lar&ge Icons"
        '
        'mnuViewSmallIcons
        '
        Me.mnuViewSmallIcons.Name = "mnuViewSmallIcons"
        Me.mnuViewSmallIcons.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuViewSmallIcons.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewSmallIcons.Text = "S&mall Icons"
        '
        'mnuViewList
        '
        Me.mnuViewList.Name = "mnuViewList"
        Me.mnuViewList.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.mnuViewList.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewList.Text = "&List"
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Checked = True
        Me.mnuViewDetails.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuViewDetails.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewDetails.Text = "&Details"
        '
        'vsep2
        '
        Me.vsep2.Name = "vsep2"
        Me.vsep2.Size = New System.Drawing.Size(213, 6)
        '
        'mnuViewExpand
        '
        Me.mnuViewExpand.Name = "mnuViewExpand"
        Me.mnuViewExpand.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuViewExpand.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewExpand.Text = "&Expand Client Folders"
        '
        'vsep3
        '
        Me.vsep3.Name = "vsep3"
        Me.vsep3.Size = New System.Drawing.Size(213, 6)
        '
        'mnuViewRefresh
        '
        Me.mnuViewRefresh.Name = "mnuViewRefresh"
        Me.mnuViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuViewRefresh.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewRefresh.Text = "&Refresh"
        '
        'mnuUtilities
        '
        Me.mnuUtilities.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuUtilitiesKeywords, Me.mnuUtilitiesDocNames, Me.mnuUtilitiesUserAccess, Me.mnuUtilitiesEditAccess})
        Me.mnuUtilities.Name = "mnuUtilities"
        Me.mnuUtilities.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.U), System.Windows.Forms.Keys)
        Me.mnuUtilities.Size = New System.Drawing.Size(53, 20)
        Me.mnuUtilities.Text = "&Utilities"
        '
        'mnuUtilitiesKeywords
        '
        Me.mnuUtilitiesKeywords.Name = "mnuUtilitiesKeywords"
        Me.mnuUtilitiesKeywords.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.mnuUtilitiesKeywords.Size = New System.Drawing.Size(288, 22)
        Me.mnuUtilitiesKeywords.Text = "&Keyword Maintenance..."
        '
        'mnuUtilitiesDocNames
        '
        Me.mnuUtilitiesDocNames.Name = "mnuUtilitiesDocNames"
        Me.mnuUtilitiesDocNames.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuUtilitiesDocNames.Size = New System.Drawing.Size(288, 22)
        Me.mnuUtilitiesDocNames.Text = "&Document Name Maintenance..."
        '
        'mnuUtilitiesUserAccess
        '
        Me.mnuUtilitiesUserAccess.Name = "mnuUtilitiesUserAccess"
        Me.mnuUtilitiesUserAccess.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.U), System.Windows.Forms.Keys)
        Me.mnuUtilitiesUserAccess.Size = New System.Drawing.Size(288, 22)
        Me.mnuUtilitiesUserAccess.Text = "&User Access Administration..."
        '
        'mnuUtilitiesEditAccess
        '
        Me.mnuUtilitiesEditAccess.Name = "mnuUtilitiesEditAccess"
        Me.mnuUtilitiesEditAccess.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuUtilitiesEditAccess.Size = New System.Drawing.Size(288, 22)
        Me.mnuUtilitiesEditAccess.Text = "&Edit Access Levels..."
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuToolsFind, Me.mnuToolsFindClear, Me.tsep1, Me.mnuToolsPassword, Me.mnuToolsAccess, Me.mnuToolsClearCache, Me.tsep3, Me.mnuToolsAddAnn, Me.mnuToolsAddKeyword, Me.tsep2, Me.mnuToolsSetHome, Me.mnuToolsGoHome, Me.tsep4, Me.mnuViewOptions})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.mnuTools.Size = New System.Drawing.Size(44, 20)
        Me.mnuTools.Text = "&Tools"
        '
        'mnuToolsFind
        '
        Me.mnuToolsFind.Name = "mnuToolsFind"
        Me.mnuToolsFind.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuToolsFind.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsFind.Text = "&Find..."
        '
        'mnuToolsFindClear
        '
        Me.mnuToolsFindClear.Name = "mnuToolsFindClear"
        Me.mnuToolsFindClear.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuToolsFindClear.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsFindClear.Text = "&Clear Find Results"
        '
        'tsep1
        '
        Me.tsep1.Name = "tsep1"
        Me.tsep1.Size = New System.Drawing.Size(226, 6)
        '
        'mnuToolsPassword
        '
        Me.mnuToolsPassword.Name = "mnuToolsPassword"
        Me.mnuToolsPassword.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuToolsPassword.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsPassword.Text = "&Password Protect..."
        '
        'mnuToolsAccess
        '
        Me.mnuToolsAccess.Name = "mnuToolsAccess"
        Me.mnuToolsAccess.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuToolsAccess.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsAccess.Text = "&Set Access Level..."
        '
        'mnuToolsClearCache
        '
        Me.mnuToolsClearCache.Name = "mnuToolsClearCache"
        Me.mnuToolsClearCache.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuToolsClearCache.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsClearCache.Text = "C&lear Cache"
        '
        'tsep3
        '
        Me.tsep3.Name = "tsep3"
        Me.tsep3.Size = New System.Drawing.Size(226, 6)
        '
        'mnuToolsAddAnn
        '
        Me.mnuToolsAddAnn.Name = "mnuToolsAddAnn"
        Me.mnuToolsAddAnn.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuToolsAddAnn.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsAddAnn.Text = "Add &Annotation..."
        '
        'mnuToolsAddKeyword
        '
        Me.mnuToolsAddKeyword.Name = "mnuToolsAddKeyword"
        Me.mnuToolsAddKeyword.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.mnuToolsAddKeyword.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsAddKeyword.Text = "Add &Keyword..."
        '
        'tsep2
        '
        Me.tsep2.Name = "tsep2"
        Me.tsep2.Size = New System.Drawing.Size(226, 6)
        '
        'mnuToolsSetHome
        '
        Me.mnuToolsSetHome.Name = "mnuToolsSetHome"
        Me.mnuToolsSetHome.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.mnuToolsSetHome.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsSetHome.Text = "Set as &Home"
        '
        'mnuToolsGoHome
        '
        Me.mnuToolsGoHome.Name = "mnuToolsGoHome"
        Me.mnuToolsGoHome.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuToolsGoHome.Size = New System.Drawing.Size(229, 22)
        Me.mnuToolsGoHome.Text = "&Go Home"
        '
        'tsep4
        '
        Me.tsep4.Name = "tsep4"
        Me.tsep4.Size = New System.Drawing.Size(226, 6)
        '
        'mnuViewOptions
        '
        Me.mnuViewOptions.Name = "mnuViewOptions"
        Me.mnuViewOptions.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuViewOptions.Size = New System.Drawing.Size(229, 22)
        Me.mnuViewOptions.Text = "&Options ..."
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(215, 22)
        Me.mnuHelpAbout.Text = "&About DocuMaster Enterprise"
        '
        'mnuPop
        '
        Me.mnuPop.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopFilter, Me.mnuPopSelect, Me.mnuPopOpenDocument, Me.mnuPopEmail, Me.mnuPopArchive, Me.mnuPopSubFolder, Me.psep1, Me.mnuPopFind, Me.psep2, Me.mnuPopSetHome, Me.psep3, Me.mnuPopAddKeyword, Me.mnuPopAddAnn, Me.mnuPopPassword, Me.mnuPopAccess, Me.psep4, Me.mnuPopCut, Me.mnuPopCopy, Me.mnuPopPaste, Me.psep5, Me.mnuPopDelete, Me.mnuPopRename, Me.mnuPopInformation, Me.psep6, Me.mnuPopScan, Me.mnuPopToSharePoint})
        Me.mnuPop.Name = "mnuPop"
        Me.mnuPop.Size = New System.Drawing.Size(50, 20)
        Me.mnuPop.Text = "PopUp"
        Me.mnuPop.Visible = False
        '
        'mnuPopFilter
        '
        Me.mnuPopFilter.Name = "mnuPopFilter"
        Me.mnuPopFilter.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopFilter.Text = "Filter..."
        '
        'mnuPopSelect
        '
        Me.mnuPopSelect.Name = "mnuPopSelect"
        Me.mnuPopSelect.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopSelect.Text = "Add Folders to View..."
        '
        'mnuPopOpenDocument
        '
        Me.mnuPopOpenDocument.Name = "mnuPopOpenDocument"
        Me.mnuPopOpenDocument.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopOpenDocument.Text = "Open Document"
        '
        'mnuPopEmail
        '
        Me.mnuPopEmail.Name = "mnuPopEmail"
        Me.mnuPopEmail.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopEmail.Text = "Email Document"
        '
        'mnuPopArchive
        '
        Me.mnuPopArchive.Name = "mnuPopArchive"
        Me.mnuPopArchive.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopArchive.Text = "Archive Document"
        '
        'mnuPopSubFolder
        '
        Me.mnuPopSubFolder.Name = "mnuPopSubFolder"
        Me.mnuPopSubFolder.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopSubFolder.Text = "New Folder"
        '
        'psep1
        '
        Me.psep1.Name = "psep1"
        Me.psep1.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopFind
        '
        Me.mnuPopFind.Name = "mnuPopFind"
        Me.mnuPopFind.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopFind.Text = "Find..."
        '
        'psep2
        '
        Me.psep2.Name = "psep2"
        Me.psep2.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopSetHome
        '
        Me.mnuPopSetHome.Name = "mnuPopSetHome"
        Me.mnuPopSetHome.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopSetHome.Text = "Set as Home Folder"
        '
        'psep3
        '
        Me.psep3.Name = "psep3"
        Me.psep3.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopAddKeyword
        '
        Me.mnuPopAddKeyword.Name = "mnuPopAddKeyword"
        Me.mnuPopAddKeyword.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopAddKeyword.Text = "Add Keyword..."
        '
        'mnuPopAddAnn
        '
        Me.mnuPopAddAnn.Name = "mnuPopAddAnn"
        Me.mnuPopAddAnn.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopAddAnn.Text = "Add Annotation..."
        '
        'mnuPopPassword
        '
        Me.mnuPopPassword.Name = "mnuPopPassword"
        Me.mnuPopPassword.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopPassword.Text = "Password Protect..."
        '
        'mnuPopAccess
        '
        Me.mnuPopAccess.Name = "mnuPopAccess"
        Me.mnuPopAccess.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopAccess.Text = "Set Access Level..."
        '
        'psep4
        '
        Me.psep4.Name = "psep4"
        Me.psep4.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopCut
        '
        Me.mnuPopCut.Name = "mnuPopCut"
        Me.mnuPopCut.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopCut.Text = "Cut"
        '
        'mnuPopCopy
        '
        Me.mnuPopCopy.Name = "mnuPopCopy"
        Me.mnuPopCopy.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopCopy.Text = "Copy"
        '
        'mnuPopPaste
        '
        Me.mnuPopPaste.Name = "mnuPopPaste"
        Me.mnuPopPaste.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopPaste.Text = "Paste"
        '
        'psep5
        '
        Me.psep5.Name = "psep5"
        Me.psep5.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopDelete
        '
        Me.mnuPopDelete.Name = "mnuPopDelete"
        Me.mnuPopDelete.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopDelete.Text = "Delete"
        '
        'mnuPopRename
        '
        Me.mnuPopRename.Name = "mnuPopRename"
        Me.mnuPopRename.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopRename.Text = "Rename"
        '
        'mnuPopInformation
        '
        Me.mnuPopInformation.Name = "mnuPopInformation"
        Me.mnuPopInformation.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopInformation.Text = "Information..."
        '
        'psep6
        '
        Me.psep6.Name = "psep6"
        Me.psep6.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPopScan
        '
        Me.mnuPopScan.Name = "mnuPopScan"
        Me.mnuPopScan.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopScan.Text = "Scan..."
        '
        'mnuPopToSharePoint
        '
        Me.mnuPopToSharePoint.Name = "mnuPopToSharePoint"
        Me.mnuPopToSharePoint.Size = New System.Drawing.Size(181, 22)
        Me.mnuPopToSharePoint.Text = "Send to Sharepoint"
        '
        'mnuPop2
        '
        Me.mnuPop2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopNewFolder})
        Me.mnuPop2.Name = "mnuPop2"
        Me.mnuPop2.Size = New System.Drawing.Size(50, 20)
        Me.mnuPop2.Text = "PopUp"
        Me.mnuPop2.Visible = False
        '
        'mnuPopNewFolder
        '
        Me.mnuPopNewFolder.Name = "mnuPopNewFolder"
        Me.mnuPopNewFolder.Size = New System.Drawing.Size(128, 22)
        Me.mnuPopNewFolder.Text = "New Folder"
        '
        'imgMain
        '
        Me.imgMain.ImageStream = CType(resources.GetObject("imgMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgMain.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgMain.Images.SetKeyName(0, "")
        Me.imgMain.Images.SetKeyName(1, "CASE")
        Me.imgMain.Images.SetKeyName(2, "COPY")
        Me.imgMain.Images.SetKeyName(3, "CUT")
        Me.imgMain.Images.SetKeyName(4, "DELETE")
        Me.imgMain.Images.SetKeyName(5, "SCAN")
        Me.imgMain.Images.SetKeyName(6, "FAV")
        Me.imgMain.Images.SetKeyName(7, "FIND")
        Me.imgMain.Images.SetKeyName(8, "IMGCLOSEDFOLDER")
        Me.imgMain.Images.SetKeyName(9, "IMGCLOSEDFOLDERMULTI")
        Me.imgMain.Images.SetKeyName(10, "WORLD")
        Me.imgMain.Images.SetKeyName(11, "HEART")
        Me.imgMain.Images.SetKeyName(12, "HOME")
        Me.imgMain.Images.SetKeyName(13, "IMGTIFF")
        Me.imgMain.Images.SetKeyName(14, "IMGTIFFMULTI")
        Me.imgMain.Images.SetKeyName(15, "INFO")
        Me.imgMain.Images.SetKeyName(16, "KEYWORD")
        Me.imgMain.Images.SetKeyName(17, "")
        Me.imgMain.Images.SetKeyName(18, "IMGOPENFOLDER")
        Me.imgMain.Images.SetKeyName(19, "PASTE")
        Me.imgMain.Images.SetKeyName(20, "VIEW")
        Me.imgMain.Images.SetKeyName(21, "PRINT")
        Me.imgMain.Images.SetKeyName(22, "IMGRTF")
        Me.imgMain.Images.SetKeyName(23, "IMGRTFMULTI")
        Me.imgMain.Images.SetKeyName(24, "IMGTEXT")
        Me.imgMain.Images.SetKeyName(25, "IMGTEXTMULTI")
        Me.imgMain.Images.SetKeyName(26, "IMGUNKNOWN")
        Me.imgMain.Images.SetKeyName(27, "DETAIL")
        Me.imgMain.Images.SetKeyName(28, "LIST")
        Me.imgMain.Images.SetKeyName(29, "LARGE")
        Me.imgMain.Images.SetKeyName(30, "SMALL")
        Me.imgMain.Images.SetKeyName(31, "ANNOTATION")
        Me.imgMain.Images.SetKeyName(32, "IMGNODROP")
        Me.imgMain.Images.SetKeyName(33, "IMGADDFOLDER")
        Me.imgMain.Images.SetKeyName(34, "IMGWORD")
        Me.imgMain.Images.SetKeyName(35, "IMGEXCEL")
        Me.imgMain.Images.SetKeyName(36, "IMGPOWERPNT")
        Me.imgMain.Images.SetKeyName(37, "IMGACCESS")
        Me.imgMain.Images.SetKeyName(38, "IMGIEXPLORER")
        Me.imgMain.Images.SetKeyName(39, "IMGGIF")
        Me.imgMain.Images.SetKeyName(40, "IMGJPEG")
        Me.imgMain.Images.SetKeyName(41, "IMGOUTLOOK")
        Me.imgMain.Images.SetKeyName(42, "IMGADOBE")
        Me.imgMain.Images.SetKeyName(43, "IMGHELP")
        Me.imgMain.Images.SetKeyName(44, "IMGZIP")
        Me.imgMain.Images.SetKeyName(45, "IMGARCHIVE")
        Me.imgMain.Images.SetKeyName(46, "IMGEXPAND")
        '
        '_tlbMain_Button3
        '
        Me._tlbMain_Button3.AutoSize = False
        Me._tlbMain_Button3.Name = "_tlbMain_Button3"
        Me._tlbMain_Button3.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button3.Tag = ""
        '
        '_tlbMain_Button7
        '
        Me._tlbMain_Button7.AutoSize = False
        Me._tlbMain_Button7.Name = "_tlbMain_Button7"
        Me._tlbMain_Button7.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button7.Tag = ""
        '
        '_tlbMain_Button10
        '
        Me._tlbMain_Button10.AutoSize = False
        Me._tlbMain_Button10.Name = "_tlbMain_Button10"
        Me._tlbMain_Button10.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button10.Tag = ""
        '
        '_tlbMain_Button15
        '
        Me._tlbMain_Button15.AutoSize = False
        Me._tlbMain_Button15.Name = "_tlbMain_Button15"
        Me._tlbMain_Button15.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button15.Tag = ""
        '
        '_tlbMain_Button17
        '
        Me._tlbMain_Button17.AutoSize = False
        Me._tlbMain_Button17.Name = "_tlbMain_Button17"
        Me._tlbMain_Button17.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button17.Tag = ""
        '
        '_tlbMain_Button20
        '
        Me._tlbMain_Button20.AutoSize = False
        Me._tlbMain_Button20.Name = "_tlbMain_Button20"
        Me._tlbMain_Button20.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button20.Tag = ""
        '
        '_tlbMain_Button23
        '
        Me._tlbMain_Button23.AutoSize = False
        Me._tlbMain_Button23.Name = "_tlbMain_Button23"
        Me._tlbMain_Button23.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button23.Tag = ""
        '
        '_tlbMain_Button28
        '
        Me._tlbMain_Button28.AutoSize = False
        Me._tlbMain_Button28.Name = "_tlbMain_Button28"
        Me._tlbMain_Button28.Size = New System.Drawing.Size(6, 22)
        Me._tlbMain_Button28.Tag = ""
        '
        'txtAcrobatDDE
        '
        Me.txtAcrobatDDE.AcceptsReturn = True
        Me.txtAcrobatDDE.BackColor = System.Drawing.SystemColors.Window
        Me.txtAcrobatDDE.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAcrobatDDE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAcrobatDDE.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAcrobatDDE.Location = New System.Drawing.Point(336, 440)
        Me.txtAcrobatDDE.MaxLength = 0
        Me.txtAcrobatDDE.Name = "txtAcrobatDDE"
        Me.txtAcrobatDDE.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAcrobatDDE.Size = New System.Drawing.Size(82, 20)
        Me.txtAcrobatDDE.TabIndex = 26
        Me.txtAcrobatDDE.Text = "txtAcrobatDDE (hidden during runtime)"
        Me.txtAcrobatDDE.Visible = False
        '
        'rtbHidden
        '
        Me.rtbHidden.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbHidden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbHidden.Location = New System.Drawing.Point(88, 368)
        Me.rtbHidden.Name = "rtbHidden"
        Me.rtbHidden.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.rtbHidden.Size = New System.Drawing.Size(73, 65)
        Me.rtbHidden.TabIndex = 22
        Me.rtbHidden.Text = ""
        Me.rtbHidden.Visible = False
        '
        'picSplitter
        '
        Me.picSplitter.BackColor = System.Drawing.Color.Gray
        Me.picSplitter.Cursor = System.Windows.Forms.Cursors.Default
        Me.picSplitter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSplitter.Location = New System.Drawing.Point(448, 256)
        Me.picSplitter.Name = "picSplitter"
        Me.picSplitter.Size = New System.Drawing.Size(8, 64)
        Me.picSplitter.TabIndex = 4
        Me.picSplitter.TabStop = False
        Me.picSplitter.Visible = False
        '
        'lvwDocsOnly
        '
        Me.lvwDocsOnly.BackColor = System.Drawing.SystemColors.Window
        Me.lvwDocsOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDocsOnly.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwDocsOnly.LabelEdit = True
        Me.lvwDocsOnly.Location = New System.Drawing.Point(16, 248)
        Me.lvwDocsOnly.Name = "lvwDocsOnly"
        Me.lvwDocsOnly.Size = New System.Drawing.Size(321, 57)
        Me.lvwDocsOnly.TabIndex = 13
        Me.lvwDocsOnly.UseCompatibleStateImageBehavior = False
        '
        'lvwBCDocsOnly
        '
        Me.lvwBCDocsOnly.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBCDocsOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBCDocsOnly.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBCDocsOnly.LabelEdit = True
        Me.lvwBCDocsOnly.Location = New System.Drawing.Point(368, 200)
        Me.lvwBCDocsOnly.Name = "lvwBCDocsOnly"
        Me.lvwBCDocsOnly.Size = New System.Drawing.Size(249, 41)
        Me.lvwBCDocsOnly.TabIndex = 12
        Me.lvwBCDocsOnly.UseCompatibleStateImageBehavior = False
        '
        'tvwFav
        '
        Me.tvwFav.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwFav.HideSelection = False
        Me.tvwFav.LabelEdit = True
        Me.tvwFav.Location = New System.Drawing.Point(16, 136)
        Me.tvwFav.Name = "tvwFav"
        Me.tvwFav.Size = New System.Drawing.Size(337, 49)
        Me.tvwFav.TabIndex = 11
        '
        'lvwPages
        '
        Me.lvwPages.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPages.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPages.Location = New System.Drawing.Point(536, 344)
        Me.lvwPages.Name = "lvwPages"
        Me.lvwPages.Size = New System.Drawing.Size(81, 73)
        Me.lvwPages.TabIndex = 8
        Me.lvwPages.UseCompatibleStateImageBehavior = False
        '
        'lvwAnnotations
        '
        Me.lvwAnnotations.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAnnotations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAnnotations_ColumnHeader_1, Me._lvwAnnotations_ColumnHeader_2, Me._lvwAnnotations_ColumnHeader_3})
        Me.lvwAnnotations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAnnotations.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAnnotations.LargeImageList = Me.imgMain
        Me.lvwAnnotations.Location = New System.Drawing.Point(448, 344)
        Me.lvwAnnotations.Name = "lvwAnnotations"
        Me.lvwAnnotations.Size = New System.Drawing.Size(81, 73)
        Me.lvwAnnotations.SmallImageList = Me.imgMain
        Me.lvwAnnotations.TabIndex = 7
        Me.lvwAnnotations.UseCompatibleStateImageBehavior = False
        Me.lvwAnnotations.View = System.Windows.Forms.View.Details
        '
        '_lvwAnnotations_ColumnHeader_1
        '
        Me._lvwAnnotations_ColumnHeader_1.Tag = ""
        Me._lvwAnnotations_ColumnHeader_1.Text = "Annotation"
        Me._lvwAnnotations_ColumnHeader_1.Width = 134
        '
        '_lvwAnnotations_ColumnHeader_2
        '
        Me._lvwAnnotations_ColumnHeader_2.Tag = ""
        Me._lvwAnnotations_ColumnHeader_2.Text = "User"
        Me._lvwAnnotations_ColumnHeader_2.Width = 48
        '
        '_lvwAnnotations_ColumnHeader_3
        '
        Me._lvwAnnotations_ColumnHeader_3.Tag = ""
        Me._lvwAnnotations_ColumnHeader_3.Text = "Date Added"
        Me._lvwAnnotations_ColumnHeader_3.Width = 97
        '
        'lvwKeyWords
        '
        Me.lvwKeyWords.BackColor = System.Drawing.SystemColors.Window
        Me.lvwKeyWords.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwKeyWords_ColumnHeader_1, Me._lvwKeyWords_ColumnHeader_2, Me._lvwKeyWords_ColumnHeader_3})
        Me.lvwKeyWords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwKeyWords.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwKeyWords.LargeImageList = Me.imgMain
        Me.lvwKeyWords.Location = New System.Drawing.Point(368, 344)
        Me.lvwKeyWords.Name = "lvwKeyWords"
        Me.lvwKeyWords.Size = New System.Drawing.Size(73, 73)
        Me.lvwKeyWords.SmallImageList = Me.imgMain
        Me.lvwKeyWords.TabIndex = 6
        Me.lvwKeyWords.UseCompatibleStateImageBehavior = False
        Me.lvwKeyWords.View = System.Windows.Forms.View.Details
        '
        '_lvwKeyWords_ColumnHeader_1
        '
        Me._lvwKeyWords_ColumnHeader_1.Tag = ""
        Me._lvwKeyWords_ColumnHeader_1.Text = "Keyword"
        Me._lvwKeyWords_ColumnHeader_1.Width = 82
        '
        '_lvwKeyWords_ColumnHeader_2
        '
        Me._lvwKeyWords_ColumnHeader_2.Tag = ""
        Me._lvwKeyWords_ColumnHeader_2.Text = "User"
        Me._lvwKeyWords_ColumnHeader_2.Width = 49
        '
        '_lvwKeyWords_ColumnHeader_3
        '
        Me._lvwKeyWords_ColumnHeader_3.Tag = ""
        Me._lvwKeyWords_ColumnHeader_3.Text = "Date Added"
        Me._lvwKeyWords_ColumnHeader_3.Width = 97
        '
        '_lblTitleFind_0
        '
        Me._lblTitleFind_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleFind_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleFind_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleFind_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleFind_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleFind_0.Location = New System.Drawing.Point(0, 52)
        Me._lblTitleFind_0.Name = "_lblTitleFind_0"
        Me._lblTitleFind_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleFind_0.Size = New System.Drawing.Size(335, 17)
        Me._lblTitleFind_0.TabIndex = 18
        Me._lblTitleFind_0.Text = "Find Results"
        '
        '_lblTitleBC_0
        '
        Me._lblTitleBC_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleBC_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleBC_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleBC_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleBC_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleBC_0.Location = New System.Drawing.Point(241, 52)
        Me._lblTitleBC_0.Name = "_lblTitleBC_0"
        Me._lblTitleBC_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleBC_0.Size = New System.Drawing.Size(129, 17)
        Me._lblTitleBC_0.TabIndex = 20
        Me._lblTitleBC_0.Text = "Briefcase Mode"
        '
        '_lblTitleMain_0
        '
        Me._lblTitleMain_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleMain_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleMain_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleMain_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleMain_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleMain_0.Location = New System.Drawing.Point(0, 52)
        Me._lblTitleMain_0.Name = "_lblTitleMain_0"
        Me._lblTitleMain_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleMain_0.Size = New System.Drawing.Size(335, 17)
        Me._lblTitleMain_0.TabIndex = 14
        Me._lblTitleMain_0.Text = "Main View"
        '
        'staContents
        '
        Me.staContents.AutoSize = False
        Me.staContents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.staContents.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._staContents_Panel1, Me._staContents_Panel2, Me._staContents_Panel3})
        Me.staContents.Location = New System.Drawing.Point(0, 481)
        Me.staContents.Name = "staContents"
        Me.staContents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.staContents.ShowItemToolTips = True
        Me.staContents.Size = New System.Drawing.Size(751, 22)
        Me.staContents.TabIndex = 1
        '
        '_staContents_Panel1
        '
        Me._staContents_Panel1.AutoSize = False
        Me._staContents_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staContents_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staContents_Panel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._staContents_Panel1.DoubleClickEnabled = True
        Me._staContents_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._staContents_Panel1.Name = "_staContents_Panel1"
        Me._staContents_Panel1.Size = New System.Drawing.Size(145, 22)
        Me._staContents_Panel1.Tag = ""
        Me._staContents_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_staContents_Panel2
        '
        Me._staContents_Panel2.AutoSize = False
        Me._staContents_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staContents_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staContents_Panel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._staContents_Panel2.DoubleClickEnabled = True
        Me._staContents_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._staContents_Panel2.Name = "_staContents_Panel2"
        Me._staContents_Panel2.Size = New System.Drawing.Size(200, 22)
        Me._staContents_Panel2.Tag = ""
        '
        '_staContents_Panel3
        '
        Me._staContents_Panel3.AutoSize = False
        Me._staContents_Panel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._staContents_Panel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._staContents_Panel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._staContents_Panel3.DoubleClickEnabled = True
        Me._staContents_Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me._staContents_Panel3.Name = "_staContents_Panel3"
        Me._staContents_Panel3.Size = New System.Drawing.Size(145, 22)
        Me._staContents_Panel3.Tag = ""
        Me._staContents_Panel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tvwMain
        '
        Me.tvwMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwMain.HideSelection = False
        Me.tvwMain.ImageIndex = 8
        Me.tvwMain.ImageList = Me.imgMain
        Me.tvwMain.Indent = 12
        Me.tvwMain.LabelEdit = True
        Me.tvwMain.Location = New System.Drawing.Point(16, 80)
        Me.tvwMain.Name = "tvwMain"
        Me.tvwMain.SelectedImageIndex = 18
        Me.tvwMain.Size = New System.Drawing.Size(337, 40)
        Me.tvwMain.TabIndex = 2
        '
        'tvwFind
        '
        Me.tvwFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwFind.HideSelection = False
        Me.tvwFind.ImageIndex = 8
        Me.tvwFind.ImageList = Me.imgMain
        Me.tvwFind.Indent = 12
        Me.tvwFind.LabelEdit = True
        Me.tvwFind.Location = New System.Drawing.Point(16, 192)
        Me.tvwFind.Name = "tvwFind"
        Me.tvwFind.SelectedImageIndex = 18
        Me.tvwFind.ShowRootLines = False
        Me.tvwFind.Size = New System.Drawing.Size(337, 46)
        Me.tvwFind.TabIndex = 9
        '
        'tvwBCMain
        '
        Me.tvwBCMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwBCMain.HideSelection = False
        Me.tvwBCMain.LabelEdit = True
        Me.tvwBCMain.Location = New System.Drawing.Point(368, 136)
        Me.tvwBCMain.Name = "tvwBCMain"
        Me.tvwBCMain.Size = New System.Drawing.Size(248, 53)
        Me.tvwBCMain.TabIndex = 10
        '
        'lvwDocList
        '
        Me.lvwDocList.AllowDrop = True
        Me.lvwDocList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwDocList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwDocList_ColumnHeader_1, Me._lvwDocList_ColumnHeader_2, Me._lvwDocList_ColumnHeader_3, Me._lvwDocList_ColumnHeader_4})
        Me.lvwDocList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDocList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwDocList.LabelEdit = True
        Me.lvwDocList.LargeImageList = Me.imlMainLarge
        Me.lvwDocList.Location = New System.Drawing.Point(368, 80)
        Me.lvwDocList.Name = "lvwDocList"
        Me.lvwDocList.Size = New System.Drawing.Size(248, 40)
        Me.lvwDocList.SmallImageList = Me.ImageList1
        Me.lvwDocList.TabIndex = 3
        Me.lvwDocList.UseCompatibleStateImageBehavior = False
        Me.lvwDocList.View = System.Windows.Forms.View.Details
        '
        '_lvwDocList_ColumnHeader_1
        '
        Me._lvwDocList_ColumnHeader_1.Tag = ""
        Me._lvwDocList_ColumnHeader_1.Text = "Name"
        Me._lvwDocList_ColumnHeader_1.Width = 140
        '
        '_lvwDocList_ColumnHeader_2
        '
        Me._lvwDocList_ColumnHeader_2.Tag = ""
        Me._lvwDocList_ColumnHeader_2.Text = "Create Date"
        Me._lvwDocList_ColumnHeader_2.Width = 140
        '
        '_lvwDocList_ColumnHeader_3
        '
        Me._lvwDocList_ColumnHeader_3.Tag = ""
        Me._lvwDocList_ColumnHeader_3.Text = "Document Type"
        Me._lvwDocList_ColumnHeader_3.Width = 140
        '
        '_lvwDocList_ColumnHeader_4
        '
        Me._lvwDocList_ColumnHeader_4.Tag = ""
        Me._lvwDocList_ColumnHeader_4.Text = "Hidden Date"
        Me._lvwDocList_ColumnHeader_4.Width = 0
        '
        'imlMainLarge
        '
        Me.imlMainLarge.ImageStream = CType(resources.GetObject("imlMainLarge.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlMainLarge.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imlMainLarge.Images.SetKeyName(0, "IMGUNKNOWN")
        Me.imlMainLarge.Images.SetKeyName(1, "IMGTIFF")
        Me.imlMainLarge.Images.SetKeyName(2, "IMGRTF")
        Me.imlMainLarge.Images.SetKeyName(3, "IMGCLOSEDFOLDER")
        Me.imlMainLarge.Images.SetKeyName(4, "IMGTEXT")
        Me.imlMainLarge.Images.SetKeyName(5, "IMGADDFOLDER")
        Me.imlMainLarge.Images.SetKeyName(6, "IMGWORD")
        Me.imlMainLarge.Images.SetKeyName(7, "IMGEXCEL")
        Me.imlMainLarge.Images.SetKeyName(8, "IMGPOWERPNT")
        Me.imlMainLarge.Images.SetKeyName(9, "IMGACCESS")
        Me.imlMainLarge.Images.SetKeyName(10, "IMGIEXPLORER")
        Me.imlMainLarge.Images.SetKeyName(11, "IMGGIF")
        Me.imlMainLarge.Images.SetKeyName(12, "IMGJPEG")
        Me.imlMainLarge.Images.SetKeyName(13, "IMGOUTLOOK")
        Me.imlMainLarge.Images.SetKeyName(14, "IMGADOBE")
        Me.imlMainLarge.Images.SetKeyName(15, "IMGHELP")
        Me.imlMainLarge.Images.SetKeyName(16, "IMGZIP")
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "IMGUNKNOWN")
        Me.ImageList1.Images.SetKeyName(1, "IMGTIFF")
        Me.ImageList1.Images.SetKeyName(2, "IMGRTF")
        Me.ImageList1.Images.SetKeyName(3, "IMGCLOSEDFOLDER")
        Me.ImageList1.Images.SetKeyName(4, "IMGTEXT")
        Me.ImageList1.Images.SetKeyName(5, "IMGADDFOLDER")
        Me.ImageList1.Images.SetKeyName(6, "IMGWORD")
        Me.ImageList1.Images.SetKeyName(7, "IMGEXCEL")
        Me.ImageList1.Images.SetKeyName(8, "IMGPOWERPNT")
        Me.ImageList1.Images.SetKeyName(9, "IMGACCESS")
        Me.ImageList1.Images.SetKeyName(10, "IMGIEXPLORER")
        Me.ImageList1.Images.SetKeyName(11, "IMGGIF")
        Me.ImageList1.Images.SetKeyName(12, "IMGJPEG")
        Me.ImageList1.Images.SetKeyName(13, "IMGOUTLOOK")
        Me.ImageList1.Images.SetKeyName(14, "IMGADOBE")
        Me.ImageList1.Images.SetKeyName(15, "IMGHELP")
        Me.ImageList1.Images.SetKeyName(16, "IMGZIP")
        Me.ImageList1.Images.SetKeyName(17, "IMGFAILED")
        Me.ImageList1.Images.SetKeyName(18, "IMGMIGRATED")
        Me.ImageList1.Images.SetKeyName(19, "IMGWIP")
        '
        'frBCDocs
        '
        Me.frBCDocs.BackColor = System.Drawing.SystemColors.Control
        Me.frBCDocs.Controls.Add(Me.tlbBCDocsButtons)
        Me.frBCDocs.Controls.Add(Me.lvwBCDocs)
        Me.frBCDocs.Controls.Add(Me.lblBCDocs)
        Me.frBCDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.frBCDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frBCDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frBCDocs.Location = New System.Drawing.Point(400, 256)
        Me.frBCDocs.Name = "frBCDocs"
        Me.frBCDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frBCDocs.Size = New System.Drawing.Size(313, 193)
        Me.frBCDocs.TabIndex = 27
        '
        'tlbBCDocsButtons
        '
        Me.tlbBCDocsButtons.Dock = System.Windows.Forms.DockStyle.None
        Me.tlbBCDocsButtons.ImageList = Me.imgMain
        Me.tlbBCDocsButtons.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._tlbBCDocsButtons_Button1, Me._tlbBCDocsButtons_Button2, Me._tlbBCDocsButtons_Button3, Me._tlbBCDocsButtons_Button4})
        Me.tlbBCDocsButtons.Location = New System.Drawing.Point(0, 24)
        Me.tlbBCDocsButtons.Name = "tlbBCDocsButtons"
        Me.tlbBCDocsButtons.Size = New System.Drawing.Size(106, 25)
        Me.tlbBCDocsButtons.TabIndex = 28
        '
        '_tlbBCDocsButtons_Button1
        '
        Me._tlbBCDocsButtons_Button1.AutoSize = False
        Me._tlbBCDocsButtons_Button1.Enabled = False
        Me._tlbBCDocsButtons_Button1.ImageKey = "IMGOUTLOOK"
        Me._tlbBCDocsButtons_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbBCDocsButtons_Button1.Name = "_tlbBCDocsButtons_Button1"
        Me._tlbBCDocsButtons_Button1.Size = New System.Drawing.Size(24, 22)
        Me._tlbBCDocsButtons_Button1.Tag = "EMAIL"
        Me._tlbBCDocsButtons_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbBCDocsButtons_Button1.ToolTipText = "Email Documents"
        '
        '_tlbBCDocsButtons_Button2
        '
        Me._tlbBCDocsButtons_Button2.AutoSize = False
        Me._tlbBCDocsButtons_Button2.Enabled = False
        Me._tlbBCDocsButtons_Button2.ImageKey = "IMGARCHIVE"
        Me._tlbBCDocsButtons_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbBCDocsButtons_Button2.Name = "_tlbBCDocsButtons_Button2"
        Me._tlbBCDocsButtons_Button2.Size = New System.Drawing.Size(24, 22)
        Me._tlbBCDocsButtons_Button2.Tag = "ARCHIVE"
        Me._tlbBCDocsButtons_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbBCDocsButtons_Button2.ToolTipText = "Archive Documents"
        '
        '_tlbBCDocsButtons_Button3
        '
        Me._tlbBCDocsButtons_Button3.AutoSize = False
        Me._tlbBCDocsButtons_Button3.Enabled = False
        Me._tlbBCDocsButtons_Button3.ImageKey = "IMGOPENFOLDER"
        Me._tlbBCDocsButtons_Button3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbBCDocsButtons_Button3.Name = "_tlbBCDocsButtons_Button3"
        Me._tlbBCDocsButtons_Button3.Size = New System.Drawing.Size(24, 22)
        Me._tlbBCDocsButtons_Button3.Tag = "EXPORT"
        Me._tlbBCDocsButtons_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbBCDocsButtons_Button3.ToolTipText = "Export Documents"
        '
        '_tlbBCDocsButtons_Button4
        '
        Me._tlbBCDocsButtons_Button4.AutoSize = False
        Me._tlbBCDocsButtons_Button4.Enabled = False
        Me._tlbBCDocsButtons_Button4.ImageKey = "DELETE"
        Me._tlbBCDocsButtons_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbBCDocsButtons_Button4.Name = "_tlbBCDocsButtons_Button4"
        Me._tlbBCDocsButtons_Button4.Size = New System.Drawing.Size(24, 22)
        Me._tlbBCDocsButtons_Button4.Tag = "REMOVE"
        Me._tlbBCDocsButtons_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbBCDocsButtons_Button4.ToolTipText = "Clear"
        '
        'lvwBCDocs
        '
        Me.lvwBCDocs.AllowDrop = True
        Me.lvwBCDocs.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBCDocs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBCDocs_ColumnHeader_1, Me._lvwBCDocs_ColumnHeader_2, Me._lvwBCDocs_ColumnHeader_3})
        Me.lvwBCDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBCDocs.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBCDocs.Location = New System.Drawing.Point(0, 56)
        Me.lvwBCDocs.Name = "lvwBCDocs"
        Me.lvwBCDocs.Size = New System.Drawing.Size(305, 129)
        Me.lvwBCDocs.TabIndex = 30
        Me.lvwBCDocs.UseCompatibleStateImageBehavior = False
        Me.lvwBCDocs.View = System.Windows.Forms.View.Details
        '
        '_lvwBCDocs_ColumnHeader_1
        '
        Me._lvwBCDocs_ColumnHeader_1.Tag = ""
        Me._lvwBCDocs_ColumnHeader_1.Text = "Path Name"
        Me._lvwBCDocs_ColumnHeader_1.Width = 101
        '
        '_lvwBCDocs_ColumnHeader_2
        '
        Me._lvwBCDocs_ColumnHeader_2.Tag = ""
        Me._lvwBCDocs_ColumnHeader_2.Text = "File Name"
        Me._lvwBCDocs_ColumnHeader_2.Width = 101
        '
        '_lvwBCDocs_ColumnHeader_3
        '
        Me._lvwBCDocs_ColumnHeader_3.Tag = ""
        Me._lvwBCDocs_ColumnHeader_3.Text = "Size"
        Me._lvwBCDocs_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwBCDocs_ColumnHeader_3.Width = 67
        '
        'lblBCDocs
        '
        Me.lblBCDocs.BackColor = System.Drawing.SystemColors.Control
        Me.lblBCDocs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBCDocs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBCDocs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBCDocs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBCDocs.Location = New System.Drawing.Point(0, 8)
        Me.lblBCDocs.Name = "lblBCDocs"
        Me.lblBCDocs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBCDocs.Size = New System.Drawing.Size(296, 17)
        Me.lblBCDocs.TabIndex = 29
        Me.lblBCDocs.Text = "Briefcase"
        '
        'imgBCSplitterH
        '
        Me.imgBCSplitterH.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.imgBCSplitterH.Location = New System.Drawing.Point(88, 320)
        Me.imgBCSplitterH.Name = "imgBCSplitterH"
        Me.imgBCSplitterH.Size = New System.Drawing.Size(113, 9)
        Me.imgBCSplitterH.TabIndex = 28
        Me.imgBCSplitterH.TabStop = False
        '
        'imgSplitterV
        '
        Me.imgSplitterV.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.imgSplitterV.Location = New System.Drawing.Point(328, 344)
        Me.imgSplitterV.Name = "imgSplitterV"
        Me.imgSplitterV.Size = New System.Drawing.Size(9, 73)
        Me.imgSplitterV.TabIndex = 29
        Me.imgSplitterV.TabStop = False
        '
        'imgSplitterH
        '
        Me.imgSplitterH.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.imgSplitterH.Location = New System.Drawing.Point(504, 288)
        Me.imgSplitterH.Name = "imgSplitterH"
        Me.imgSplitterH.Size = New System.Drawing.Size(113, 9)
        Me.imgSplitterH.TabIndex = 30
        Me.imgSplitterH.TabStop = False
        '
        'Ctx_mnuPop
        '
        Me.Ctx_mnuPop.Name = "Ctx_mnuPop"
        Me.Ctx_mnuPop.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuPop2
        '
        Me.Ctx_mnuPop2.Name = "Ctx_mnuPop2"
        Me.Ctx_mnuPop2.Size = New System.Drawing.Size(61, 4)
        '
        '_tlbMain_Button1
        '
        Me._tlbMain_Button1.AutoSize = False
        Me._tlbMain_Button1.ImageKey = "VIEW"
        Me._tlbMain_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button1.Name = "_tlbMain_Button1"
        Me._tlbMain_Button1.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button1.Tag = "VIEWDOC"
        Me._tlbMain_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button1.ToolTipText = "View Document"
        '
        '_tlbMain_Button2
        '
        Me._tlbMain_Button2.AutoSize = False
        Me._tlbMain_Button2.ImageKey = "SCAN"
        Me._tlbMain_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button2.Name = "_tlbMain_Button2"
        Me._tlbMain_Button2.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button2.Tag = "SCAN"
        Me._tlbMain_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button2.ToolTipText = "Scan"
        '
        '_tlbMain_Button4
        '
        Me._tlbMain_Button4.AutoSize = False
        Me._tlbMain_Button4.ImageKey = "CUT"
        Me._tlbMain_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button4.Name = "_tlbMain_Button4"
        Me._tlbMain_Button4.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button4.Tag = "CUT"
        Me._tlbMain_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button4.ToolTipText = "Cut"
        '
        '_tlbMain_Button5
        '
        Me._tlbMain_Button5.AutoSize = False
        Me._tlbMain_Button5.ImageKey = "COPY"
        Me._tlbMain_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button5.Name = "_tlbMain_Button5"
        Me._tlbMain_Button5.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button5.Tag = "COPY"
        Me._tlbMain_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button5.ToolTipText = "Copy"
        '
        '_tlbMain_Button6
        '
        Me._tlbMain_Button6.AutoSize = False
        Me._tlbMain_Button6.ImageKey = "PASTE"
        Me._tlbMain_Button6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button6.Name = "_tlbMain_Button6"
        Me._tlbMain_Button6.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button6.Tag = "PASTE"
        Me._tlbMain_Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button6.ToolTipText = "Paste"
        '
        '_tlbMain_Button8
        '
        Me._tlbMain_Button8.AutoSize = False
        Me._tlbMain_Button8.ImageKey = "DELETE"
        Me._tlbMain_Button8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button8.Name = "_tlbMain_Button8"
        Me._tlbMain_Button8.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button8.Tag = "DELETE"
        Me._tlbMain_Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button8.ToolTipText = "Delete"
        '
        '_tlbMain_Button9
        '
        Me._tlbMain_Button9.AutoSize = False
        Me._tlbMain_Button9.ImageKey = "INFO"
        Me._tlbMain_Button9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button9.Name = "_tlbMain_Button9"
        Me._tlbMain_Button9.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button9.Tag = "DOCINFO"
        Me._tlbMain_Button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button9.ToolTipText = "Document Information"
        '
        '_tlbMain_Button11
        '
        Me._tlbMain_Button11.AutoSize = False
        Me._tlbMain_Button11.ImageKey = "LARGE"
        Me._tlbMain_Button11.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button11.Name = "_tlbMain_Button11"
        Me._tlbMain_Button11.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button11.Tag = "LARGE"
        Me._tlbMain_Button11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button11.ToolTipText = "Large Icons"
        '
        '_tlbMain_Button12
        '
        Me._tlbMain_Button12.AutoSize = False
        Me._tlbMain_Button12.ImageKey = "SMALL"
        Me._tlbMain_Button12.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button12.Name = "_tlbMain_Button12"
        Me._tlbMain_Button12.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button12.Tag = "SMALL"
        Me._tlbMain_Button12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button12.ToolTipText = "Small Icons"
        '
        '_tlbMain_Button13
        '
        Me._tlbMain_Button13.AutoSize = False
        Me._tlbMain_Button13.ImageKey = "LIST"
        Me._tlbMain_Button13.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button13.Name = "_tlbMain_Button13"
        Me._tlbMain_Button13.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button13.Tag = "LIST"
        Me._tlbMain_Button13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button13.ToolTipText = "List"
        '
        '_tlbMain_Button14
        '
        Me._tlbMain_Button14.AutoSize = False
        Me._tlbMain_Button14.Checked = True
        Me._tlbMain_Button14.CheckState = System.Windows.Forms.CheckState.Checked
        Me._tlbMain_Button14.ImageKey = "DETAIL"
        Me._tlbMain_Button14.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button14.Name = "_tlbMain_Button14"
        Me._tlbMain_Button14.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button14.Tag = "LISTDET"
        Me._tlbMain_Button14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button14.ToolTipText = "List Details"
        '
        '_tlbMain_Button16
        '
        Me._tlbMain_Button16.AutoSize = False
        Me._tlbMain_Button16.ImageKey = "IMGEXPAND"
        Me._tlbMain_Button16.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button16.Name = "_tlbMain_Button16"
        Me._tlbMain_Button16.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button16.Tag = "EXPAND"
        Me._tlbMain_Button16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button16.ToolTipText = "Expand Client Folders"
        '
        '_tlbMain_Button18
        '
        Me._tlbMain_Button18.AutoSize = False
        Me._tlbMain_Button18.ImageKey = "HOME"
        Me._tlbMain_Button18.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button18.Name = "_tlbMain_Button18"
        Me._tlbMain_Button18.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button18.Tag = "HOME"
        Me._tlbMain_Button18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button18.ToolTipText = "Go Home"
        '
        '_tlbMain_Button19
        '
        Me._tlbMain_Button19.AutoSize = False
        Me._tlbMain_Button19.ImageKey = "HEART"
        Me._tlbMain_Button19.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button19.Name = "_tlbMain_Button19"
        Me._tlbMain_Button19.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button19.Tag = "HOTKEY"
        Me._tlbMain_Button19.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button19.ToolTipText = "Cycle Through Favourites"
        '
        '_tlbMain_Button21
        '
        Me._tlbMain_Button21.AutoSize = False
        Me._tlbMain_Button21.ImageKey = "KEYWORD"
        Me._tlbMain_Button21.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button21.Name = "_tlbMain_Button21"
        Me._tlbMain_Button21.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button21.Tag = "KEYWORD"
        Me._tlbMain_Button21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button21.ToolTipText = "Display Keywords"
        '
        '_tlbMain_Button22
        '
        Me._tlbMain_Button22.AutoSize = False
        Me._tlbMain_Button22.ImageKey = "ANNOTATION"
        Me._tlbMain_Button22.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button22.Name = "_tlbMain_Button22"
        Me._tlbMain_Button22.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button22.Tag = "ANNOTATION"
        Me._tlbMain_Button22.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button22.ToolTipText = "Display Annotations"
        '
        '_tlbMain_Button24
        '
        Me._tlbMain_Button24.AutoSize = False
        Me._tlbMain_Button24.Checked = True
        Me._tlbMain_Button24.CheckState = System.Windows.Forms.CheckState.Checked
        Me._tlbMain_Button24.ImageKey = "WORLD"
        Me._tlbMain_Button24.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button24.Name = "_tlbMain_Button24"
        Me._tlbMain_Button24.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button24.Tag = "MAIN"
        Me._tlbMain_Button24.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button24.ToolTipText = "Main View"
        '
        '_tlbMain_Button25
        '
        Me._tlbMain_Button25.AutoSize = False
        Me._tlbMain_Button25.ImageKey = "FAV"
        Me._tlbMain_Button25.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button25.Name = "_tlbMain_Button25"
        Me._tlbMain_Button25.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button25.Tag = "FAV"
        Me._tlbMain_Button25.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button25.ToolTipText = "View Favourites"
        Me._tlbMain_Button25.Visible = False
        '
        '_tlbMain_Button26
        '
        Me._tlbMain_Button26.AutoSize = False
        Me._tlbMain_Button26.ImageKey = "CASE"
        Me._tlbMain_Button26.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button26.Name = "_tlbMain_Button26"
        Me._tlbMain_Button26.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button26.Tag = "BC"
        Me._tlbMain_Button26.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button26.ToolTipText = "Briefcase View"
        Me._tlbMain_Button26.Visible = False
        '
        '_tlbMain_Button27
        '
        Me._tlbMain_Button27.AutoSize = False
        Me._tlbMain_Button27.ImageKey = "FIND"
        Me._tlbMain_Button27.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button27.Name = "_tlbMain_Button27"
        Me._tlbMain_Button27.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button27.Tag = "FIND"
        Me._tlbMain_Button27.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button27.ToolTipText = "Find Results"
        '
        '_tlbMain_Button29
        '
        Me._tlbMain_Button29.AutoSize = False
        Me._tlbMain_Button29.ImageKey = "PRINT"
        Me._tlbMain_Button29.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button29.Name = "_tlbMain_Button29"
        Me._tlbMain_Button29.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button29.Tag = "PRINT"
        Me._tlbMain_Button29.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button29.ToolTipText = "Print"
        '
        '_tlbMain_Button30
        '
        Me._tlbMain_Button30.AutoSize = False
        Me._tlbMain_Button30.ImageKey = "IMGOUTLOOK"
        Me._tlbMain_Button30.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button30.Name = "_tlbMain_Button30"
        Me._tlbMain_Button30.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button30.Tag = "EMAIL"
        Me._tlbMain_Button30.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button30.ToolTipText = "Email Document"
        '
        '_tlbMain_Button31
        '
        Me._tlbMain_Button31.AutoSize = False
        Me._tlbMain_Button31.ImageKey = "IMGARCHIVE"
        Me._tlbMain_Button31.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._tlbMain_Button31.Name = "_tlbMain_Button31"
        Me._tlbMain_Button31.Size = New System.Drawing.Size(24, 22)
        Me._tlbMain_Button31.Tag = "ARCHIVE"
        Me._tlbMain_Button31.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._tlbMain_Button31.ToolTipText = "Archive Document"
        '
        'tlbMain
        '
        Me.tlbMain.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar
        Me.tlbMain.ImageList = Me.imgMain
        Me.tlbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._tlbMain_Button1, Me._tlbMain_Button2, Me._tlbMain_Button4, Me._tlbMain_Button5, Me._tlbMain_Button6, Me._tlbMain_Button8, Me._tlbMain_Button9, Me._tlbMain_Button11, Me._tlbMain_Button12, Me._tlbMain_Button13, Me._tlbMain_Button14, Me._tlbMain_Button16, Me._tlbMain_Button18, Me._tlbMain_Button19, Me._tlbMain_Button21, Me._tlbMain_Button22, Me._tlbMain_Button24, Me._tlbMain_Button25, Me._tlbMain_Button26, Me._tlbMain_Button27, Me._tlbMain_Button29, Me._tlbMain_Button30, Me._tlbMain_Button31})
        Me.tlbMain.Location = New System.Drawing.Point(0, 24)
        Me.tlbMain.Name = "tlbMain"
        Me.tlbMain.Size = New System.Drawing.Size(751, 25)
        Me.tlbMain.TabIndex = 0
        '
        '_lblTitleFav_0
        '
        Me._lblTitleFav_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleFav_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleFav_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleFav_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleFav_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleFav_0.Location = New System.Drawing.Point(364, 49)
        Me._lblTitleFav_0.Name = "_lblTitleFav_0"
        Me._lblTitleFav_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleFav_0.Size = New System.Drawing.Size(142, 19)
        Me._lblTitleFav_0.TabIndex = 32
        Me._lblTitleFav_0.Text = "Favourites"
        '
        '_lblTitleFav_1
        '
        Me._lblTitleFav_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleFav_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleFav_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleFav_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleFav_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleFav_1.Location = New System.Drawing.Point(448, 49)
        Me._lblTitleFav_1.Name = "_lblTitleFav_1"
        Me._lblTitleFav_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleFav_1.Size = New System.Drawing.Size(201, 21)
        Me._lblTitleFav_1.TabIndex = 33
        '
        '_lblTitleMain_1
        '
        Me._lblTitleMain_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleMain_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleMain_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleMain_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleMain_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleMain_1.Location = New System.Drawing.Point(330, 0)
        Me._lblTitleMain_1.Name = "_lblTitleMain_1"
        Me._lblTitleMain_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleMain_1.Size = New System.Drawing.Size(250, 17)
        Me._lblTitleMain_1.TabIndex = 15
        '
        '_lblTitleBC_1
        '
        Me._lblTitleBC_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleBC_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleBC_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleBC_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleBC_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleBC_1.Location = New System.Drawing.Point(440, 0)
        Me._lblTitleBC_1.Name = "_lblTitleBC_1"
        Me._lblTitleBC_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleBC_1.Size = New System.Drawing.Size(33, 17)
        Me._lblTitleBC_1.TabIndex = 21
        '
        '_lblTitleFind_1
        '
        Me._lblTitleFind_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblTitleFind_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblTitleFind_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitleFind_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitleFind_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTitleFind_1.Location = New System.Drawing.Point(330, 0)
        Me._lblTitleFind_1.Name = "_lblTitleFind_1"
        Me._lblTitleFind_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitleFind_1.Size = New System.Drawing.Size(240, 17)
        Me._lblTitleFind_1.TabIndex = 19
        '
        'picTitles
        '
        Me.picTitles.BackColor = System.Drawing.SystemColors.Control
        Me.picTitles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picTitles.Controls.Add(Me._lblTitleFind_1)
        Me.picTitles.Controls.Add(Me._lblTitleMain_1)
        Me.picTitles.Controls.Add(Me._lblTitleBC_1)
        Me.picTitles.Cursor = System.Windows.Forms.Cursors.Default
        Me.picTitles.Dock = System.Windows.Forms.DockStyle.Top
        Me.picTitles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picTitles.Location = New System.Drawing.Point(0, 49)
        Me.picTitles.Name = "picTitles"
        Me.picTitles.Size = New System.Drawing.Size(751, 21)
        Me.picTitles.TabIndex = 5
        Me.picTitles.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(751, 503)
        Me.Controls.Add(Me._lblTitleBC_0)
        Me.Controls.Add(Me._lblTitleMain_0)
        Me.Controls.Add(Me.picTitles)
        Me.Controls.Add(Me._lblTitleFav_1)
        Me.Controls.Add(Me._lblTitleFav_0)
        Me.Controls.Add(Me._lblTitleFind_0)
        Me.Controls.Add(Me.lvwDocList)
        Me.Controls.Add(Me.tvwMain)
        Me.Controls.Add(Me.tlbMain)
        Me.Controls.Add(Me.txtAcrobatDDE)
        Me.Controls.Add(Me.rtbHidden)
        Me.Controls.Add(Me.picSplitter)
        Me.Controls.Add(Me.lvwBCDocsOnly)
        Me.Controls.Add(Me.lvwDocsOnly)
        Me.Controls.Add(Me.tvwFav)
        Me.Controls.Add(Me.lvwPages)
        Me.Controls.Add(Me.lvwAnnotations)
        Me.Controls.Add(Me.lvwKeyWords)
        Me.Controls.Add(Me.staContents)
        Me.Controls.Add(Me.tvwFind)
        Me.Controls.Add(Me.tvwBCMain)
        Me.Controls.Add(Me.frBCDocs)
        Me.Controls.Add(Me.imgBCSplitterH)
        Me.Controls.Add(Me.imgSplitterV)
        Me.Controls.Add(Me.imgSplitterH)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(255, 175)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "DocuMaster Enterprise  "
        Me.Text = "DocuMaster Enterprise  "
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.picSplitter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.staContents.ResumeLayout(False)
        Me.staContents.PerformLayout()
        Me.frBCDocs.ResumeLayout(False)
        Me.frBCDocs.PerformLayout()
        Me.tlbBCDocsButtons.ResumeLayout(False)
        Me.tlbBCDocsButtons.PerformLayout()
        CType(Me.imgBCSplitterH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSplitterV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSplitterH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tlbMain.ResumeLayout(False)
        Me.tlbMain.PerformLayout()
        CType(Me.picTitles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picTitles.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblTitleMain()
        Me.lblTitleMain(1) = _lblTitleMain_1
        Me.lblTitleMain(0) = _lblTitleMain_0
    End Sub
    Sub InitializelblTitleFind()
        Me.lblTitleFind(1) = _lblTitleFind_1
        Me.lblTitleFind(0) = _lblTitleFind_0
    End Sub
    Sub InitializelblTitleFav()
        Me.lblTitleFav(1) = _lblTitleFav_1
        Me.lblTitleFav(0) = _lblTitleFav_0
    End Sub
    Sub InitializelblTitleBC()
        Me.lblTitleBC(1) = _lblTitleBC_1
        Me.lblTitleBC(0) = _lblTitleBC_0
    End Sub
    Sub lvwBCDocs_InitializeColumnKeys()
        Me._lvwBCDocs_ColumnHeader_1.Name = ""
        Me._lvwBCDocs_ColumnHeader_2.Name = ""
        Me._lvwBCDocs_ColumnHeader_3.Name = ""
    End Sub
    Sub lvwDocList_InitializeColumnKeys()
        Me._lvwDocList_ColumnHeader_1.Name = ""
        Me._lvwDocList_ColumnHeader_2.Name = ""
        Me._lvwDocList_ColumnHeader_3.Name = ""
        Me._lvwDocList_ColumnHeader_4.Name = ""
    End Sub
    Sub lvwKeyWords_InitializeColumnKeys()
        Me._lvwKeyWords_ColumnHeader_1.Name = ""
        Me._lvwKeyWords_ColumnHeader_2.Name = ""
        Me._lvwKeyWords_ColumnHeader_3.Name = ""
    End Sub
    Sub lvwAnnotations_InitializeColumnKeys()
        Me._lvwAnnotations_ColumnHeader_1.Name = ""
        Me._lvwAnnotations_ColumnHeader_2.Name = ""
        Me._lvwAnnotations_ColumnHeader_3.Name = ""
    End Sub
    Private WithEvents _tlbMain_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button2 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button4 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button5 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button6 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button8 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button9 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button11 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button12 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button13 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button14 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button16 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button18 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button19 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button21 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button22 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button24 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button25 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button26 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button27 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button29 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button30 As System.Windows.Forms.ToolStripButton
    Private WithEvents _tlbMain_Button31 As System.Windows.Forms.ToolStripButton
    Public WithEvents tlbMain As System.Windows.Forms.ToolStrip
    Private WithEvents _lblTitleFav_0 As System.Windows.Forms.Label
    Private WithEvents _lblTitleFav_1 As System.Windows.Forms.Label
    Private WithEvents _lblTitleMain_1 As System.Windows.Forms.Label
    Private WithEvents _lblTitleBC_1 As System.Windows.Forms.Label
    Private WithEvents _lblTitleFind_1 As System.Windows.Forms.Label
    Public WithEvents picTitles As System.Windows.Forms.PictureBox
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents mnuPopToSharePoint As System.Windows.Forms.ToolStripMenuItem

#End Region
End Class
