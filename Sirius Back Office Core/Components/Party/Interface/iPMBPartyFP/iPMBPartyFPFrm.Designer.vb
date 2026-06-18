<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrevious()
		InitializecmdNext()
		lvwContacts_InitializeColumnKeys()
		lvwAddresses_InitializeColumnKeys()
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
    Private WithEvents FINANCIAL As System.Windows.Forms.ToolStripButton
    Private WithEvents COMMISSION As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents NOTE As System.Windows.Forms.ToolStripButton
    Private WithEvents LETTER As System.Windows.Forms.ToolStripButton
    Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents EMAIL As System.Windows.Forms.ToolStripButton
    Private WithEvents WEB As System.Windows.Forms.ToolStripButton
    Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
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
    Public WithEvents chkCompanyReg As System.Windows.Forms.CheckBox
    Public WithEvents chkDOB As System.Windows.Forms.CheckBox
    Public WithEvents framExtras As System.Windows.Forms.GroupBox
    Public WithEvents txtAgencyNumber As System.Windows.Forms.TextBox
    Public WithEvents txtMailbox As System.Windows.Forms.TextBox
    Public WithEvents cboPFEDIDefinition As PMLookupControl.cboPMLookup
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblAgencyNumber As System.Windows.Forms.Label
    Public WithEvents lblMailbox As System.Windows.Forms.Label
    Public WithEvents framEDI As System.Windows.Forms.GroupBox
    Public WithEvents cboSource As PMLookupControl.cboPMLookup
    Public WithEvents txtNumber As System.Windows.Forms.TextBox
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents txtIDReference As System.Windows.Forms.TextBox
    Public WithEvents cboSubBranch As PMLookupControl.cboPMLookup
    Public WithEvents cboCurrency As UserControls.CurrencyLookup
    Public WithEvents lblSubBranch As System.Windows.Forms.Label
    Public WithEvents lblSource As System.Windows.Forms.Label
    Public WithEvents lblNumber As System.Windows.Forms.Label
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents lblName As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents fraFinanceProvider As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAddresses As System.Windows.Forms.ListView
    Public WithEvents cmdAddAd As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
    Public WithEvents cmdEditAd As System.Windows.Forms.Button
    Public WithEvents fraAddress As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents cmdEditCon As System.Windows.Forms.Button
    Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
    Public WithEvents cmdAddCon As System.Windows.Forms.Button
    Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwContacts As System.Windows.Forms.ListView
    Public WithEvents fraContact As System.Windows.Forms.GroupBox
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents ImageList2 As System.Windows.Forms.ImageList
    Public cmdNext(2) As System.Windows.Forms.Button
    Public cmdPrevious(2) As System.Windows.Forms.Button
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
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
        Me.FINANCIAL = New System.Windows.Forms.ToolStripButton
        Me.COMMISSION = New System.Windows.Forms.ToolStripButton
        Me.NOTE = New System.Windows.Forms.ToolStripButton
        Me.LETTER = New System.Windows.Forms.ToolStripButton
        Me.EMAIL = New System.Windows.Forms.ToolStripButton
        Me.WEB = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripSeparator
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
        Me.fraFinanceProvider = New System.Windows.Forms.GroupBox
        Me.framExtras = New System.Windows.Forms.GroupBox
        Me.chkCompanyReg = New System.Windows.Forms.CheckBox
        Me.chkDOB = New System.Windows.Forms.CheckBox
        Me.framEDI = New System.Windows.Forms.GroupBox
        Me.txtAgencyNumber = New System.Windows.Forms.TextBox
        Me.txtMailbox = New System.Windows.Forms.TextBox
        Me.cboPFEDIDefinition = New PMLookupControl.cboPMLookup
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAgencyNumber = New System.Windows.Forms.Label
        Me.lblMailbox = New System.Windows.Forms.Label
        Me.cboSource = New PMLookupControl.cboPMLookup
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.cboSubBranch = New PMLookupControl.cboPMLookup
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblSource = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.lvwAddresses = New System.Windows.Forms.ListView
        Me._lvwAddresses_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraFinanceProvider.SuspendLayout()
        Me.framExtras.SuspendLayout()
        Me.framEDI.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFind, Me.mnuRelatedDocuments})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(608, 24)
        Me.MainMenu1.TabIndex = 42
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
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FINANCIAL, Me.COMMISSION, Me.NOTE, Me.LETTER, Me.EMAIL, Me.WEB})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 24)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(608, 25)
        Me.Toolbar1.TabIndex = 41
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
        Me.ImageList2.Images.SetKeyName(5, "AddressImage")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "ContactImage")
        '
        'FINANCIAL
        '
        Me.FINANCIAL.AutoSize = False
        Me.FINANCIAL.ImageIndex = 0
        Me.FINANCIAL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FINANCIAL.Name = "FINANCIAL"
        Me.FINANCIAL.Size = New System.Drawing.Size(24, 22)
        Me.FINANCIAL.Tag = ""
        Me.FINANCIAL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.FINANCIAL.ToolTipText = "Financial"
        '
        'COMMISSION
        '
        Me.COMMISSION.AutoSize = False
        Me.COMMISSION.ImageIndex = 1
        Me.COMMISSION.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.COMMISSION.Name = "COMMISSION"
        Me.COMMISSION.Size = New System.Drawing.Size(24, 22)
        Me.COMMISSION.Tag = ""
        Me.COMMISSION.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.COMMISSION.ToolTipText = "Commission"
        '
        'NOTE
        '
        Me.NOTE.AutoSize = False
        Me.NOTE.ImageIndex = 2
        Me.NOTE.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.NOTE.Name = "NOTE"
        Me.NOTE.Size = New System.Drawing.Size(24, 22)
        Me.NOTE.Tag = ""
        Me.NOTE.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.NOTE.ToolTipText = "Notes"
        '
        'LETTER
        '
        Me.LETTER.AutoSize = False
        Me.LETTER.ImageIndex = 3
        Me.LETTER.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.LETTER.Name = "LETTER"
        Me.LETTER.Size = New System.Drawing.Size(24, 22)
        Me.LETTER.Tag = ""
        Me.LETTER.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.LETTER.ToolTipText = "Letter"
        '
        'EMAIL
        '
        Me.EMAIL.AutoSize = False
        Me.EMAIL.ImageIndex = 5
        Me.EMAIL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.EMAIL.Name = "EMAIL"
        Me.EMAIL.Size = New System.Drawing.Size(24, 22)
        Me.EMAIL.Tag = ""
        Me.EMAIL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.EMAIL.ToolTipText = "E-Mail"
        '
        'WEB
        '
        Me.WEB.AutoSize = False
        Me.WEB.ImageIndex = 6
        Me.WEB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.WEB.Name = "WEB"
        Me.WEB.Size = New System.Drawing.Size(24, 22)
        Me.WEB.Tag = ""
        Me.WEB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.WEB.ToolTipText = "Web "
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button3.Tag = ""
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(6, 22)
        Me._Toolbar1_Button6.Tag = ""
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(528, 388)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 26
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
        Me.cmdCancel.Location = New System.Drawing.Point(448, 388)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 25
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 388)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 24
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(117, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 58)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(597, 327)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFinanceProvider)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(589, 301)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Identity"
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
        Me._cmdNext_0.Location = New System.Drawing.Point(540, 276)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 24
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraFinanceProvider
        '
        Me.fraFinanceProvider.BackColor = System.Drawing.SystemColors.Control
        Me.fraFinanceProvider.Controls.Add(Me.framExtras)
        Me.fraFinanceProvider.Controls.Add(Me.framEDI)
        Me.fraFinanceProvider.Controls.Add(Me.cboSource)
        Me.fraFinanceProvider.Controls.Add(Me.txtNumber)
        Me.fraFinanceProvider.Controls.Add(Me.txtName)
        Me.fraFinanceProvider.Controls.Add(Me.txtIDReference)
        Me.fraFinanceProvider.Controls.Add(Me.cboSubBranch)
        Me.fraFinanceProvider.Controls.Add(Me.cboCurrency)
        Me.fraFinanceProvider.Controls.Add(Me.lblSubBranch)
        Me.fraFinanceProvider.Controls.Add(Me.lblSource)
        Me.fraFinanceProvider.Controls.Add(Me.lblNumber)
        Me.fraFinanceProvider.Controls.Add(Me.lblCurrency)
        Me.fraFinanceProvider.Controls.Add(Me.lblName)
        Me.fraFinanceProvider.Controls.Add(Me.lblCode)
        Me.fraFinanceProvider.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFinanceProvider.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFinanceProvider.Location = New System.Drawing.Point(12, 7)
        Me.fraFinanceProvider.Name = "fraFinanceProvider"
        Me.fraFinanceProvider.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFinanceProvider.Size = New System.Drawing.Size(567, 262)
        Me.fraFinanceProvider.TabIndex = 1
        Me.fraFinanceProvider.TabStop = False
        '
        'framExtras
        '
        Me.framExtras.BackColor = System.Drawing.SystemColors.Control
        Me.framExtras.Controls.Add(Me.chkCompanyReg)
        Me.framExtras.Controls.Add(Me.chkDOB)
        Me.framExtras.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.framExtras.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framExtras.Location = New System.Drawing.Point(300, 164)
        Me.framExtras.Name = "framExtras"
        Me.framExtras.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framExtras.Size = New System.Drawing.Size(241, 71)
        Me.framExtras.TabIndex = 21
        Me.framExtras.TabStop = False
        Me.framExtras.Text = "Additional Fields"
        '
        'chkCompanyReg
        '
        Me.chkCompanyReg.BackColor = System.Drawing.SystemColors.Control
        Me.chkCompanyReg.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCompanyReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCompanyReg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCompanyReg.Location = New System.Drawing.Point(10, 40)
        Me.chkCompanyReg.Name = "chkCompanyReg"
        Me.chkCompanyReg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCompanyReg.Size = New System.Drawing.Size(165, 19)
        Me.chkCompanyReg.TabIndex = 23
        Me.chkCompanyReg.Text = "Company Reg. Number"
        Me.chkCompanyReg.UseVisualStyleBackColor = False
        '
        'chkDOB
        '
        Me.chkDOB.BackColor = System.Drawing.SystemColors.Control
        Me.chkDOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDOB.Location = New System.Drawing.Point(10, 20)
        Me.chkDOB.Name = "chkDOB"
        Me.chkDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDOB.Size = New System.Drawing.Size(105, 19)
        Me.chkDOB.TabIndex = 22
        Me.chkDOB.Text = "Date of Birth"
        Me.chkDOB.UseVisualStyleBackColor = False
        '
        'framEDI
        '
        Me.framEDI.BackColor = System.Drawing.SystemColors.Control
        Me.framEDI.Controls.Add(Me.txtAgencyNumber)
        Me.framEDI.Controls.Add(Me.txtMailbox)
        Me.framEDI.Controls.Add(Me.cboPFEDIDefinition)
        Me.framEDI.Controls.Add(Me.Label1)
        Me.framEDI.Controls.Add(Me.lblAgencyNumber)
        Me.framEDI.Controls.Add(Me.lblMailbox)
        Me.framEDI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.framEDI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framEDI.Location = New System.Drawing.Point(22, 130)
        Me.framEDI.Name = "framEDI"
        Me.framEDI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framEDI.Size = New System.Drawing.Size(253, 105)
        Me.framEDI.TabIndex = 14
        Me.framEDI.TabStop = False
        Me.framEDI.Text = "EDI"
        '
        'txtAgencyNumber
        '
        Me.txtAgencyNumber.AcceptsReturn = True
        Me.txtAgencyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgencyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgencyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgencyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgencyNumber.Location = New System.Drawing.Point(116, 46)
        Me.txtAgencyNumber.MaxLength = 0
        Me.txtAgencyNumber.Name = "txtAgencyNumber"
        Me.txtAgencyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgencyNumber.Size = New System.Drawing.Size(121, 20)
        Me.txtAgencyNumber.TabIndex = 18
        '
        'txtMailbox
        '
        Me.txtMailbox.AcceptsReturn = True
        Me.txtMailbox.BackColor = System.Drawing.SystemColors.Window
        Me.txtMailbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMailbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailbox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMailbox.Location = New System.Drawing.Point(116, 72)
        Me.txtMailbox.MaxLength = 0
        Me.txtMailbox.Name = "txtMailbox"
        Me.txtMailbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMailbox.Size = New System.Drawing.Size(121, 20)
        Me.txtMailbox.TabIndex = 20
        '
        'cboPFEDIDefinition
        '
        Me.cboPFEDIDefinition.DefaultItemId = 0
        Me.cboPFEDIDefinition.FirstItem = ""
        Me.cboPFEDIDefinition.ItemId = 0
        Me.cboPFEDIDefinition.ListIndex = -1
        Me.cboPFEDIDefinition.Location = New System.Drawing.Point(116, 18)
        Me.cboPFEDIDefinition.Name = "cboPFEDIDefinition"
        Me.cboPFEDIDefinition.PMLookupProductFamily = 1
        Me.cboPFEDIDefinition.SingleItemId = 0
        Me.cboPFEDIDefinition.Size = New System.Drawing.Size(123, 21)
        Me.cboPFEDIDefinition.Sorted = True
        Me.cboPFEDIDefinition.TabIndex = 16
        Me.cboPFEDIDefinition.TableName = "PFEDIDefinition"
        Me.cboPFEDIDefinition.ToolTipText = ""
        Me.cboPFEDIDefinition.WhereClause = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(10, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "EDI Format:"
        '
        'lblAgencyNumber
        '
        Me.lblAgencyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgencyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgencyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgencyNumber.Location = New System.Drawing.Point(10, 48)
        Me.lblAgencyNumber.Name = "lblAgencyNumber"
        Me.lblAgencyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgencyNumber.Size = New System.Drawing.Size(121, 17)
        Me.lblAgencyNumber.TabIndex = 17
        Me.lblAgencyNumber.Text = "A&gency number:"
        '
        'lblMailbox
        '
        Me.lblMailbox.BackColor = System.Drawing.SystemColors.Control
        Me.lblMailbox.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMailbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMailbox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMailbox.Location = New System.Drawing.Point(10, 74)
        Me.lblMailbox.Name = "lblMailbox"
        Me.lblMailbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMailbox.Size = New System.Drawing.Size(121, 17)
        Me.lblMailbox.TabIndex = 19
        Me.lblMailbox.Text = "&Mailbox number:"
        '
        'cboSource
        '
        Me.cboSource.DefaultItemId = 0
        Me.cboSource.FirstItem = ""
        Me.cboSource.ItemId = 0
        Me.cboSource.ListIndex = -1
        Me.cboSource.Location = New System.Drawing.Point(98, 90)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.PMLookupProductFamily = 1
        Me.cboSource.SingleItemId = 0
        Me.cboSource.Size = New System.Drawing.Size(201, 21)
        Me.cboSource.Sorted = True
        Me.cboSource.TabIndex = 9
        Me.cboSource.TableName = "Source"
        Me.cboSource.ToolTipText = ""
        Me.cboSource.WhereClause = ""
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(388, 32)
        Me.txtNumber.MaxLength = 0
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(151, 20)
        Me.txtNumber.TabIndex = 5
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(98, 62)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(441, 20)
        Me.txtName.TabIndex = 7
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(98, 33)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(81, 20)
        Me.txtIDReference.TabIndex = 3
        '
        'cboSubBranch
        '
        Me.cboSubBranch.DefaultItemId = 0
        Me.cboSubBranch.FirstItem = ""
        Me.cboSubBranch.ItemId = 0
        Me.cboSubBranch.ListIndex = -1
        Me.cboSubBranch.Location = New System.Drawing.Point(388, 90)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.PMLookupProductFamily = 1
        Me.cboSubBranch.SingleItemId = 0
        Me.cboSubBranch.Size = New System.Drawing.Size(153, 21)
        Me.cboSubBranch.Sorted = True
        Me.cboSubBranch.TabIndex = 11
        Me.cboSubBranch.TableName = "Sub_Branch"
        Me.cboSubBranch.ToolTipText = ""
        Me.cboSubBranch.WhereClause = ""
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = -1
        Me.cboCurrency.FirstItem = "(none)"
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(388, 118)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 13
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(304, 94)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(66, 13)
        Me.lblSubBranch.TabIndex = 10
        Me.lblSubBranch.Text = "Sub-Branch:"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(24, 94)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(44, 13)
        Me.lblSource.TabIndex = 8
        Me.lblSource.Text = "Branch:"
        '
        'lblNumber
        '
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(304, 34)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(121, 17)
        Me.lblNumber.TabIndex = 4
        Me.lblNumber.Text = "N&umber:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(304, 122)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(121, 17)
        Me.lblCurrency.TabIndex = 12
        Me.lblCurrency.Text = "Cu&rrency:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(24, 64)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 6
        Me.lblName.Text = "N&ame:"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(24, 35)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(43, 13)
        Me.lblCode.TabIndex = 2
        Me.lblCode.Text = "Co&de:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(589, 301)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Address"
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(12, 7)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(569, 262)
        Me.fraAddress.TabIndex = 25
        Me.fraAddress.TabStop = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAddresses, False)
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwAddresses, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.LargeImageList = Me.ImageList2
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(553, 206)
        Me.listViewHelper1.SetSmallIcons(Me.lvwAddresses, "")
        Me.lvwAddresses.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwAddresses, False)
        Me.listViewHelper1.SetSortKey(Me.lvwAddresses, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwAddresses, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAddresses.TabIndex = 27
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Tag = ""
        Me._lvwAddresses_ColumnHeader_1.Text = "Post Code"
        Me._lvwAddresses_ColumnHeader_1.Width = 67
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Tag = ""
        Me._lvwAddresses_ColumnHeader_2.Text = "Address Usage"
        Me._lvwAddresses_ColumnHeader_2.Width = 167
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Tag = ""
        Me._lvwAddresses_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Tag = ""
        Me._lvwAddresses_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Tag = ""
        Me._lvwAddresses_ColumnHeader_5.Text = "Address Line 3"
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Tag = ""
        Me._lvwAddresses_ColumnHeader_6.Text = "Address Line 4"
        Me._lvwAddresses_ColumnHeader_6.Width = 97
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 229)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 28
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(88, 229)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 29
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(168, 229)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 30
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(12, 276)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 31
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(540, 276)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 30
        Me._cmdNext_1.TabStop = False
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(589, 301)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "&3 - Contacts"
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(12, 7)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(569, 262)
        Me.fraContact.TabIndex = 32
        Me.fraContact.TabStop = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 229)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 36
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 229)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 35
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 229)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 34
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContacts, "")
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContacts, False)
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwContacts, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwContacts, "")
        Me.lvwContacts.LargeImageList = Me.ImageList2
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(553, 206)
        Me.listViewHelper1.SetSmallIcons(Me.lvwContacts, "")
        Me.lvwContacts.SmallImageList = Me.ImageList2
        Me.listViewHelper1.SetSorted(Me.lvwContacts, False)
        Me.listViewHelper1.SetSortKey(Me.lvwContacts, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwContacts, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContacts.TabIndex = 33
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Tag = ""
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 67
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Tag = ""
        Me._lvwContacts_ColumnHeader_2.Text = "Number"
        Me._lvwContacts_ColumnHeader_2.Width = 67
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Tag = ""
        Me._lvwContacts_ColumnHeader_3.Text = "Extension"
        Me._lvwContacts_ColumnHeader_3.Width = 67
        '
        '_lvwContacts_ColumnHeader_4
        '
        Me._lvwContacts_ColumnHeader_4.Tag = ""
        Me._lvwContacts_ColumnHeader_4.Text = "Type"
        Me._lvwContacts_ColumnHeader_4.Width = 97
        '
        '_lvwContacts_ColumnHeader_5
        '
        Me._lvwContacts_ColumnHeader_5.Tag = ""
        Me._lvwContacts_ColumnHeader_5.Text = "Description"
        Me._lvwContacts_ColumnHeader_5.Width = 134
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(12, 276)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 37
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(540, 276)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 44
        Me._cmdNext_2.TabStop = False
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.uctPartyTax1)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(589, 301)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = True
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(12, 7)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(569, 145)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 42
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(12, 276)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 43
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(608, 418)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
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
        Me.Text = "Finance Provider"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraFinanceProvider.ResumeLayout(False)
        Me.fraFinanceProvider.PerformLayout()
        Me.framExtras.ResumeLayout(False)
        Me.framEDI.ResumeLayout(False)
        Me.framEDI.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializecmdPrevious()
		Me.cmdPrevious(2) = _cmdPrevious_2
		Me.cmdPrevious(1) = _cmdPrevious_1
		Me.cmdPrevious(0) = _cmdPrevious_0
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(2) = _cmdNext_2
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(0) = _cmdNext_0
	End Sub
	Sub lvwContacts_InitializeColumnKeys()
		Me._lvwContacts_ColumnHeader_1.Name = ""
		Me._lvwContacts_ColumnHeader_2.Name = ""
		Me._lvwContacts_ColumnHeader_3.Name = ""
		Me._lvwContacts_ColumnHeader_4.Name = ""
		Me._lvwContacts_ColumnHeader_5.Name = ""
	End Sub
	Sub lvwAddresses_InitializeColumnKeys()
		Me._lvwAddresses_ColumnHeader_1.Name = ""
		Me._lvwAddresses_ColumnHeader_2.Name = ""
		Me._lvwAddresses_ColumnHeader_3.Name = ""
		Me._lvwAddresses_ColumnHeader_4.Name = ""
		Me._lvwAddresses_ColumnHeader_5.Name = ""
		Me._lvwAddresses_ColumnHeader_6.Name = ""
	End Sub
#End Region 
End Class