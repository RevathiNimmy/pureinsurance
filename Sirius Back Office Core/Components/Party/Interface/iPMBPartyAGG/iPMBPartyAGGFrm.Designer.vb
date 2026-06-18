<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializecmdPrevious()
		InitializecmdNext()
		lvwContact_InitializeColumnKeys()
		lvwAddress_InitializeColumnKeys()
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
    Public WithEvents mnuFinancial As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuCommission As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuLetter As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRelatedDocuments As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents uctBranch As PMLookupControl.cboPMLookup
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents txtIDReference As System.Windows.Forms.TextBox
    Public WithEvents lblActive As System.Windows.Forms.Label
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblIDReference As System.Windows.Forms.Label
    Public WithEvents fraAgentGroup As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents cmdAddAd As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
    Public WithEvents cmdEditAd As System.Windows.Forms.Button
    Private WithEvents _lvwAddress_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddress_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddress As System.Windows.Forms.ListView
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents cmdAddCon As System.Windows.Forms.Button
    Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
    Public WithEvents cmdEditCon As System.Windows.Forms.Button
    Private WithEvents _lvwContact_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContact_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwContact As System.Windows.Forms.ListView
    Public WithEvents fraContact As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList2 As System.Windows.Forms.ImageList
    Public cmdNext(3) As System.Windows.Forms.Button
    Public cmdPrevious(1) As System.Windows.Forms.Button
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFinancial = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCommission = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRelatedDocuments = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuLetter = New System.Windows.Forms.ToolStripMenuItem
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me.cmdApply = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.fraAgentGroup = New System.Windows.Forms.GroupBox
        Me.chkActive = New System.Windows.Forms.CheckBox
        Me.uctBranch = New PMLookupControl.cboPMLookup
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.lblActive = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblIDReference = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me.lvwAddress = New System.Windows.Forms.ListView
        Me._lvwAddress_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.lvwContact = New System.Windows.Forms.ListView
        Me._lvwContact_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContact_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContact_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContact_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContact_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraAgentGroup.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFind, Me.mnuRelatedDocuments})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(618, 24)
        Me.MainMenu1.TabIndex = 29
        '
        'mnuFind
        '
        Me.mnuFind.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFinancial, Me.mnuCommission})
        Me.mnuFind.Name = "mnuFind"
        Me.mnuFind.Size = New System.Drawing.Size(39, 20)
        Me.mnuFind.Text = "&Find"
        '
        'mnuFinancial
        '
        Me.mnuFinancial.Name = "mnuFinancial"
        Me.mnuFinancial.Size = New System.Drawing.Size(129, 22)
        Me.mnuFinancial.Text = "Financial"
        '
        'mnuCommission
        '
        Me.mnuCommission.Name = "mnuCommission"
        Me.mnuCommission.Size = New System.Drawing.Size(129, 22)
        Me.mnuCommission.Text = "Commission"
        '
        'mnuRelatedDocuments
        '
        Me.mnuRelatedDocuments.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNotes, Me.mnuLetter})
        Me.mnuRelatedDocuments.Name = "mnuRelatedDocuments"
        Me.mnuRelatedDocuments.Size = New System.Drawing.Size(112, 20)
        Me.mnuRelatedDocuments.Text = "&Related Documents"
        '
        'mnuNotes
        '
        Me.mnuNotes.Name = "mnuNotes"
        Me.mnuNotes.Size = New System.Drawing.Size(103, 22)
        Me.mnuNotes.Text = "Notes"
        '
        'mnuLetter
        '
        Me.mnuLetter.Name = "mnuLetter"
        Me.mnuLetter.Size = New System.Drawing.Size(103, 22)
        Me.mnuLetter.Text = "Letter"
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList2
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Toolbar1_Button1, Me._Toolbar1_Button2, Me.ToolStripButton1, Me._Toolbar1_Button4, Me._Toolbar1_Button5})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 24)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(618, 25)
        Me.Toolbar1.TabIndex = 20
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "NOTE")
        Me.ImageList2.Images.SetKeyName(3, "LETTER")
        Me.ImageList2.Images.SetKeyName(4, "COMMISSION")
        Me.ImageList2.Images.SetKeyName(5, "ADDRESS")
        Me.ImageList2.Images.SetKeyName(6, "")
        '
        '_Toolbar1_Button1
        '
        Me._Toolbar1_Button1.AutoSize = False
        Me._Toolbar1_Button1.ImageKey = "FINANCIAL"
        Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button1.Name = "_Toolbar1_Button1"
        Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button1.Tag = ""
        Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button1.ToolTipText = "Financial"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.ImageKey = "COMMISSION"
        Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button2.Tag = ""
        Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button2.ToolTipText = "Commission"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.ImageKey = "NOTE"
        Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button4.Tag = ""
        Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button4.ToolTipText = "Notes"
        '
        '_Toolbar1_Button5
        '
        Me._Toolbar1_Button5.AutoSize = False
        Me._Toolbar1_Button5.ImageKey = "LETTER"
        Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button5.Name = "_Toolbar1_Button5"
        Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button5.Tag = ""
        Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button5.ToolTipText = "Letter"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button3.Tag = ""
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(296, 400)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 28
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 400)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 19
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 400)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 18
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
        Me.cmdOK.Location = New System.Drawing.Point(376, 400)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 17
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(119, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 56)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 337)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAgentGroup)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 311)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Agent Group"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(544, 284)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 5
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraAgentGroup
        '
        Me.fraAgentGroup.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgentGroup.Controls.Add(Me.chkActive)
        Me.fraAgentGroup.Controls.Add(Me.uctBranch)
        Me.fraAgentGroup.Controls.Add(Me.txtName)
        Me.fraAgentGroup.Controls.Add(Me.txtIDReference)
        Me.fraAgentGroup.Controls.Add(Me.lblActive)
        Me.fraAgentGroup.Controls.Add(Me.lblBranch)
        Me.fraAgentGroup.Controls.Add(Me.lblName)
        Me.fraAgentGroup.Controls.Add(Me.lblIDReference)
        Me.fraAgentGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgentGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgentGroup.Location = New System.Drawing.Point(8, 4)
        Me.fraAgentGroup.Name = "fraAgentGroup"
        Me.fraAgentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgentGroup.Size = New System.Drawing.Size(577, 269)
        Me.fraAgentGroup.TabIndex = 21
        Me.fraAgentGroup.TabStop = False
        '
        'chkActive
        '
        Me.chkActive.BackColor = System.Drawing.SystemColors.Control
        Me.chkActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkActive.Location = New System.Drawing.Point(248, 176)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkActive.Size = New System.Drawing.Size(17, 17)
        Me.chkActive.TabIndex = 3
        Me.chkActive.UseVisualStyleBackColor = False
        '
        'uctBranch
        '
        Me.uctBranch.DefaultItemId = 0
        Me.uctBranch.FirstItem = ""
        Me.uctBranch.ItemId = 0
        Me.uctBranch.ListIndex = -1
        Me.uctBranch.Location = New System.Drawing.Point(248, 144)
        Me.uctBranch.Name = "uctBranch"
        Me.uctBranch.PMLookupProductFamily = 9
        Me.uctBranch.SingleItemId = 0
        Me.uctBranch.Size = New System.Drawing.Size(209, 21)
        Me.uctBranch.Sorted = True
        Me.uctBranch.TabIndex = 2
        Me.uctBranch.TableName = "source"
        Me.uctBranch.ToolTipText = ""
        Me.uctBranch.WhereClause = ""
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(248, 112)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(209, 20)
        Me.txtName.TabIndex = 1
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(248, 83)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(209, 20)
        Me.txtIDReference.TabIndex = 0
        '
        'lblActive
        '
        Me.lblActive.BackColor = System.Drawing.SystemColors.Control
        Me.lblActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActive.Location = New System.Drawing.Point(120, 178)
        Me.lblActive.Name = "lblActive"
        Me.lblActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActive.Size = New System.Drawing.Size(97, 17)
        Me.lblActive.TabIndex = 27
        Me.lblActive.Text = "Active ?"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(120, 147)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(105, 17)
        Me.lblBranch.TabIndex = 26
        Me.lblBranch.Text = "Branch:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(120, 115)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 25
        Me.lblName.Text = "Name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(120, 86)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(98, 13)
        Me.lblIDReference.TabIndex = 22
        Me.lblIDReference.Text = "Agent Group Code:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(597, 311)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.lvwAddress)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(577, 269)
        Me.fraAddress.TabIndex = 23
        Me.fraAddress.TabStop = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 232)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 7
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Enabled = False
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 232)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 8
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Enabled = False
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 232)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 9
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'lvwAddress
        '
        Me.lvwAddress.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddress.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddress_ColumnHeader_1, Me._lvwAddress_ColumnHeader_2, Me._lvwAddress_ColumnHeader_3, Me._lvwAddress_ColumnHeader_4, Me._lvwAddress_ColumnHeader_5, Me._lvwAddress_ColumnHeader_6})
        Me.lvwAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddress.LargeImageList = Me.ImageList2
        Me.lvwAddress.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddress.Name = "lvwAddress"
        Me.lvwAddress.Size = New System.Drawing.Size(561, 209)
        Me.lvwAddress.SmallImageList = Me.ImageList2
        Me.lvwAddress.TabIndex = 6
        Me.lvwAddress.UseCompatibleStateImageBehavior = False
        Me.lvwAddress.View = System.Windows.Forms.View.Details
        '
        '_lvwAddress_ColumnHeader_1
        '
        Me._lvwAddress_ColumnHeader_1.Tag = ""
        Me._lvwAddress_ColumnHeader_1.Text = ""
        Me._lvwAddress_ColumnHeader_1.Width = 97
        '
        '_lvwAddress_ColumnHeader_2
        '
        Me._lvwAddress_ColumnHeader_2.Tag = ""
        Me._lvwAddress_ColumnHeader_2.Text = ""
        Me._lvwAddress_ColumnHeader_2.Width = 97
        '
        '_lvwAddress_ColumnHeader_3
        '
        Me._lvwAddress_ColumnHeader_3.Tag = ""
        Me._lvwAddress_ColumnHeader_3.Text = ""
        Me._lvwAddress_ColumnHeader_3.Width = 97
        '
        '_lvwAddress_ColumnHeader_4
        '
        Me._lvwAddress_ColumnHeader_4.Tag = ""
        Me._lvwAddress_ColumnHeader_4.Text = ""
        Me._lvwAddress_ColumnHeader_4.Width = 97
        '
        '_lvwAddress_ColumnHeader_5
        '
        Me._lvwAddress_ColumnHeader_5.Tag = ""
        Me._lvwAddress_ColumnHeader_5.Text = ""
        Me._lvwAddress_ColumnHeader_5.Width = 97
        '
        '_lvwAddress_ColumnHeader_6
        '
        Me._lvwAddress_ColumnHeader_6.Tag = ""
        Me._lvwAddress_ColumnHeader_6.Text = ""
        Me._lvwAddress_ColumnHeader_6.Width = 97
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(544, 284)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 11
        Me._cmdNext_3.TabStop = False
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 284)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 10
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(597, 311)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Contacts"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.lvwContact)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 4)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(577, 269)
        Me.fraContact.TabIndex = 24
        Me.fraContact.TabStop = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 232)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 13
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Enabled = False
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 232)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 14
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Enabled = False
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 232)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 15
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'lvwContact
        '
        Me.lvwContact.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContact.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContact_ColumnHeader_1, Me._lvwContact_ColumnHeader_2, Me._lvwContact_ColumnHeader_3, Me._lvwContact_ColumnHeader_4, Me._lvwContact_ColumnHeader_5})
        Me.lvwContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContact.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContact.LargeImageList = Me.ImageList2
        Me.lvwContact.Location = New System.Drawing.Point(8, 16)
        Me.lvwContact.Name = "lvwContact"
        Me.lvwContact.Size = New System.Drawing.Size(561, 209)
        Me.lvwContact.TabIndex = 12
        Me.lvwContact.UseCompatibleStateImageBehavior = False
        Me.lvwContact.View = System.Windows.Forms.View.Details
        '
        '_lvwContact_ColumnHeader_1
        '
        Me._lvwContact_ColumnHeader_1.Tag = ""
        Me._lvwContact_ColumnHeader_1.Text = ""
        Me._lvwContact_ColumnHeader_1.Width = 97
        '
        '_lvwContact_ColumnHeader_2
        '
        Me._lvwContact_ColumnHeader_2.Tag = ""
        Me._lvwContact_ColumnHeader_2.Text = ""
        Me._lvwContact_ColumnHeader_2.Width = 97
        '
        '_lvwContact_ColumnHeader_3
        '
        Me._lvwContact_ColumnHeader_3.Tag = ""
        Me._lvwContact_ColumnHeader_3.Text = ""
        Me._lvwContact_ColumnHeader_3.Width = 97
        '
        '_lvwContact_ColumnHeader_4
        '
        Me._lvwContact_ColumnHeader_4.Tag = ""
        Me._lvwContact_ColumnHeader_4.Text = ""
        Me._lvwContact_ColumnHeader_4.Width = 97
        '
        '_lvwContact_ColumnHeader_5
        '
        Me._lvwContact_ColumnHeader_5.Tag = ""
        Me._lvwContact_ColumnHeader_5.Text = ""
        Me._lvwContact_ColumnHeader_5.Width = 97
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 284)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 16
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(618, 428)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 182)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Agent Group"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraAgentGroup.ResumeLayout(False)
        Me.fraAgentGroup.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Sub lvwContact_InitializeColumnKeys()
        Me._lvwContact_ColumnHeader_1.Name = ""
        Me._lvwContact_ColumnHeader_2.Name = ""
        Me._lvwContact_ColumnHeader_3.Name = ""
        Me._lvwContact_ColumnHeader_4.Name = ""
        Me._lvwContact_ColumnHeader_5.Name = ""
    End Sub
    Sub lvwAddress_InitializeColumnKeys()
        Me._lvwAddress_ColumnHeader_1.Name = ""
        Me._lvwAddress_ColumnHeader_2.Name = ""
        Me._lvwAddress_ColumnHeader_3.Name = ""
        Me._lvwAddress_ColumnHeader_4.Name = ""
        Me._lvwAddress_ColumnHeader_5.Name = ""
        Me._lvwAddress_ColumnHeader_6.Name = ""
    End Sub
    Private WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
#End Region
End Class