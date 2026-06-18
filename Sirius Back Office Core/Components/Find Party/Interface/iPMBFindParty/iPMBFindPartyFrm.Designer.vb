<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializemnuRecentFile()
		lvwSearchDetails_InitializeColumnKeys()
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
	Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuRecentFiles As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdUsers As System.Windows.Forms.Button
	Public WithEvents chkPM As System.Windows.Forms.CheckBox
	Public WithEvents chkSirius As System.Windows.Forms.CheckBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblShortName As System.Windows.Forms.Label
	Public WithEvents lblLongName As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblAgentType As System.Windows.Forms.Label
	Public WithEvents lblFileCode As System.Windows.Forms.Label
	Public WithEvents lblDPARequired As System.Windows.Forms.Label
	Public WithEvents cmbBranch As System.Windows.Forms.ComboBox
	Public WithEvents txtShortName As System.Windows.Forms.TextBox
	Public WithEvents txtLongName As System.Windows.Forms.TextBox
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
	Public WithEvents txtFileCode As System.Windows.Forms.TextBox
	Public WithEvents cmbOtherPartyType As System.Windows.Forms.ComboBox
	Public WithEvents cmbAgentType As System.Windows.Forms.ComboBox
	Public WithEvents cmbActiveStatus As System.Windows.Forms.ComboBox
	Public WithEvents chkDPARequired As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeClosedBranches As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblPostalCode As System.Windows.Forms.Label
	Public WithEvents lblAddress1 As System.Windows.Forms.Label
	Public WithEvents lblTelephone As System.Windows.Forms.Label
	Public WithEvents lblDOB As System.Windows.Forms.Label
	Public WithEvents txtPostalCode As System.Windows.Forms.TextBox
	Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Public WithEvents txtTelephoneCode As System.Windows.Forms.TextBox
	Public WithEvents txtTelephone As System.Windows.Forms.TextBox
	Public WithEvents txtDOB As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents txtInsReference As System.Windows.Forms.TextBox
	Public WithEvents cmdPolicyRefFind As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents lblRiskIndex As System.Windows.Forms.Label
	Public WithEvents lblClaimNo As System.Windows.Forms.Label
	Public WithEvents txtRiskIndex As System.Windows.Forms.TextBox
	Public WithEvents txtClaimNumber As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
    'Deepak_todo: commented code related to listViewHelper1, as this was conflicting with the image icon of Listview control
	'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuRecentFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripSeparator()
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdUsers = New System.Windows.Forms.Button()
        Me.chkPM = New System.Windows.Forms.CheckBox()
        Me.chkSirius = New System.Windows.Forms.CheckBox()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdNew = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmbAgentType = New System.Windows.Forms.ComboBox()
        Me.lblAgentType = New System.Windows.Forms.Label()
        Me.lblShortName = New System.Windows.Forms.Label()
        Me.lblLongName = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblFileCode = New System.Windows.Forms.Label()
        Me.lblDPARequired = New System.Windows.Forms.Label()
        Me.txtShortName = New System.Windows.Forms.TextBox()
        Me.txtLongName = New System.Windows.Forms.TextBox()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.txtFileCode = New System.Windows.Forms.TextBox()
        Me.cmbOtherPartyType = New System.Windows.Forms.ComboBox()
        Me.cmbActiveStatus = New System.Windows.Forms.ComboBox()
        Me.chkDPARequired = New System.Windows.Forms.CheckBox()
        Me.chkIncludeClosedBranches = New System.Windows.Forms.CheckBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.cmbBranch = New System.Windows.Forms.ComboBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblPostalCode = New System.Windows.Forms.Label()
        Me.lblAddress1 = New System.Windows.Forms.Label()
        Me.lblTelephone = New System.Windows.Forms.Label()
        Me.lblDOB = New System.Windows.Forms.Label()
        Me.txtPostalCode = New System.Windows.Forms.TextBox()
        Me.txtAddress1 = New System.Windows.Forms.TextBox()
        Me.txtTelephoneCode = New System.Windows.Forms.TextBox()
        Me.txtTelephone = New System.Windows.Forms.TextBox()
        Me.txtDOB = New System.Windows.Forms.TextBox()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtInsReference = New System.Windows.Forms.TextBox()
        Me.cmdPolicyRefFind = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.lblCaseNumber = New System.Windows.Forms.Label()
        Me.txtCaseNumber = New System.Windows.Forms.TextBox()
        Me.lblRiskIndex = New System.Windows.Forms.Label()
        Me.lblClaimNo = New System.Windows.Forms.Label()
        Me.txtRiskIndex = New System.Windows.Forms.TextBox()
        Me.txtClaimNumber = New System.Windows.Forms.TextBox()
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lvwSearchDetails = New System.Windows.Forms.ListView()
        Me._lvwSearchDetails_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSearchDetails_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.cmbAgentGroup = New System.Windows.Forms.ComboBox()
        Me.lblAgentGroup = New System.Windows.Forms.Label()
        Me.MainMenu1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.BackColor = System.Drawing.SystemColors.Control
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRecentFiles})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(568, 24)
        Me.MainMenu1.TabIndex = 37
        '
        'mnuRecentFiles
        '
        Me.mnuRecentFiles.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator})
        Me.mnuRecentFiles.Name = "mnuRecentFiles"
        Me.mnuRecentFiles.Size = New System.Drawing.Size(78, 20)
        Me.mnuRecentFiles.Text = "&RecentFiles"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(74, 6)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(77, 22)
        Me._mnuRecentFile_1.Text = " "
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(77, 22)
        Me._mnuRecentFile_2.Text = " "
        Me._mnuRecentFile_2.Visible = False
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(77, 22)
        Me._mnuRecentFile_3.Text = " "
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(77, 22)
        Me._mnuRecentFile_4.Text = " "
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(77, 22)
        Me._mnuRecentFile_5.Text = " "
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(74, 6)
        '
        'cmdUsers
        '
        Me.cmdUsers.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUsers.Enabled = False
        Me.cmdUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUsers.Location = New System.Drawing.Point(168, 411)
        Me.cmdUsers.Name = "cmdUsers"
        Me.cmdUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUsers.Size = New System.Drawing.Size(73, 22)
        Me.cmdUsers.TabIndex = 22
        Me.cmdUsers.Text = "&Users"
        Me.cmdUsers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUsers.UseVisualStyleBackColor = False
        Me.cmdUsers.Visible = False
        '
        'chkPM
        '
        Me.chkPM.BackColor = System.Drawing.SystemColors.Control
        Me.chkPM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPM.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPM.Location = New System.Drawing.Point(482, 160)
        Me.chkPM.Name = "chkPM"
        Me.chkPM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPM.Size = New System.Drawing.Size(81, 41)
        Me.chkPM.TabIndex = 36
        Me.chkPM.Text = "Search Policy Master"
        Me.chkPM.UseVisualStyleBackColor = False
        '
        'chkSirius
        '
        Me.chkSirius.BackColor = System.Drawing.SystemColors.Control
        Me.chkSirius.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSirius.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSirius.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSirius.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSirius.Location = New System.Drawing.Point(482, 128)
        Me.chkSirius.Name = "chkSirius"
        Me.chkSirius.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSirius.Size = New System.Drawing.Size(81, 25)
        Me.chkSirius.TabIndex = 35
        Me.chkSirius.Text = "Search Sirius"
        Me.chkSirius.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(241, 411)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 26
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(88, 411)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 21
        Me.cmdEdit.Text = "E&dit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(8, 411)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 20
        Me.cmdNew.Text = "N&ew"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(482, 96)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 19
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(482, 64)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 16
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(480, 411)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 25
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
        Me.cmdCancel.Location = New System.Drawing.Point(400, 411)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(320, 411)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 23
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(113, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 29)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(465, 207)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbAgentGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAgentGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbAgentType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAgentType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblShortName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLongName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFileCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtShortName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtLongName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFileCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbOtherPartyType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbActiveStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIncludeClosedBranches)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbBranch)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(457, 181)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Client"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'cmbAgentType
        '
        Me.cmbAgentType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbAgentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbAgentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAgentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAgentType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbAgentType, New Integer(-1) {})
        Me.cmbAgentType.Location = New System.Drawing.Point(168, 110)
        Me.cmbAgentType.Name = "cmbAgentType"
        Me.cmbAgentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbAgentType.Size = New System.Drawing.Size(153, 21)
        Me.cmbAgentType.TabIndex = 4
        Me.cmbAgentType.Visible = False
        '
        'lblAgentType
        '
        Me.lblAgentType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentType.Location = New System.Drawing.Point(16, 109)
        Me.lblAgentType.Name = "lblAgentType"
        Me.lblAgentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentType.Size = New System.Drawing.Size(137, 17)
        Me.lblAgentType.TabIndex = 38
        Me.lblAgentType.Text = "Agent Type:"
        Me.lblAgentType.Visible = False
        '
        'lblShortName
        '
        Me.lblShortName.AutoSize = True
        Me.lblShortName.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortName.Location = New System.Drawing.Point(16, 38)
        Me.lblShortName.Name = "lblShortName"
        Me.lblShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortName.Size = New System.Drawing.Size(63, 13)
        Me.lblShortName.TabIndex = 31
        Me.lblShortName.Text = "Client code:"
        '
        'lblLongName
        '
        Me.lblLongName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLongName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLongName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLongName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLongName.Location = New System.Drawing.Point(16, 63)
        Me.lblLongName.Name = "lblLongName"
        Me.lblLongName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLongName.Size = New System.Drawing.Size(145, 17)
        Me.lblLongName.TabIndex = 32
        Me.lblLongName.Text = "Name:"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(16, 138)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(89, 17)
        Me.lblStatus.TabIndex = 34
        Me.lblStatus.Text = "Status:"
        '
        'lblFileCode
        '
        Me.lblFileCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileCode.Location = New System.Drawing.Point(16, 87)
        Me.lblFileCode.Name = "lblFileCode"
        Me.lblFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileCode.Size = New System.Drawing.Size(121, 17)
        Me.lblFileCode.TabIndex = 39
        Me.lblFileCode.Text = "File Code:"
        '
        'lblDPARequired
        '
        Me.lblDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.lblDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDPARequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDPARequired.Location = New System.Drawing.Point(425, 18)
        Me.lblDPARequired.Name = "lblDPARequired"
        Me.lblDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDPARequired.Size = New System.Drawing.Size(23, 18)
        Me.lblDPARequired.TabIndex = 8
        Me.lblDPARequired.Text = "No"
        '
        'txtShortName
        '
        Me.txtShortName.AcceptsReturn = True
        Me.txtShortName.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortName.Location = New System.Drawing.Point(168, 34)
        Me.txtShortName.MaxLength = 0
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortName.Size = New System.Drawing.Size(153, 20)
        Me.txtShortName.TabIndex = 1
        '
        'txtLongName
        '
        Me.txtLongName.AcceptsReturn = True
        Me.txtLongName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLongName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLongName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLongName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLongName.Location = New System.Drawing.Point(168, 60)
        Me.txtLongName.MaxLength = 0
        Me.txtLongName.Name = "txtLongName"
        Me.txtLongName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLongName.Size = New System.Drawing.Size(249, 20)
        Me.txtLongName.TabIndex = 2
        '
        'cmbStatus
        '
        Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbStatus, New Integer(-1) {})
        Me.cmbStatus.Location = New System.Drawing.Point(168, 136)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStatus.Size = New System.Drawing.Size(153, 21)
        Me.cmbStatus.TabIndex = 10
        Me.cmbStatus.Visible = False
        '
        'txtFileCode
        '
        Me.txtFileCode.AcceptsReturn = True
        Me.txtFileCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileCode.Location = New System.Drawing.Point(168, 84)
        Me.txtFileCode.MaxLength = 0
        Me.txtFileCode.Name = "txtFileCode"
        Me.txtFileCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileCode.Size = New System.Drawing.Size(153, 20)
        Me.txtFileCode.TabIndex = 3
        '
        'cmbOtherPartyType
        '
        Me.cmbOtherPartyType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbOtherPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbOtherPartyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOtherPartyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOtherPartyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbOtherPartyType, New Integer() {0})
        Me.cmbOtherPartyType.Items.AddRange(New Object() {"cmbOtherPartyType"})
        Me.cmbOtherPartyType.Location = New System.Drawing.Point(168, 136)
        Me.cmbOtherPartyType.Name = "cmbOtherPartyType"
        Me.cmbOtherPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbOtherPartyType.Size = New System.Drawing.Size(153, 21)
        Me.cmbOtherPartyType.Sorted = True
        Me.cmbOtherPartyType.TabIndex = 41
        Me.cmbOtherPartyType.Visible = False
        '
        'cmbActiveStatus
        '
        Me.cmbActiveStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbActiveStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbActiveStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActiveStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbActiveStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbActiveStatus, New Integer(-1) {})
        Me.cmbActiveStatus.Location = New System.Drawing.Point(168, 136)
        Me.cmbActiveStatus.Name = "cmbActiveStatus"
        Me.cmbActiveStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbActiveStatus.Size = New System.Drawing.Size(153, 21)
        Me.cmbActiveStatus.TabIndex = 5
        Me.cmbActiveStatus.Visible = False
        '
        'chkDPARequired
        '
        Me.chkDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.chkDPARequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDPARequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDPARequired.Location = New System.Drawing.Point(328, 5)
        Me.chkDPARequired.Name = "chkDPARequired"
        Me.chkDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDPARequired.Size = New System.Drawing.Size(91, 46)
        Me.chkDPARequired.TabIndex = 7
        Me.chkDPARequired.Text = "DPA Info Required?"
        Me.chkDPARequired.UseVisualStyleBackColor = False
        '
        'chkIncludeClosedBranches
        '
        Me.chkIncludeClosedBranches.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeClosedBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeClosedBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeClosedBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeClosedBranches.Location = New System.Drawing.Point(328, 116)
        Me.chkIncludeClosedBranches.Name = "chkIncludeClosedBranches"
        Me.chkIncludeClosedBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeClosedBranches.Size = New System.Drawing.Size(105, 33)
        Me.chkIncludeClosedBranches.TabIndex = 6
        Me.chkIncludeClosedBranches.Text = "Include Closed     Branches"
        Me.chkIncludeClosedBranches.UseVisualStyleBackColor = False
        Me.chkIncludeClosedBranches.Visible = False
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(16, 111)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(89, 17)
        Me.lblType.TabIndex = 33
        Me.lblType.Text = "Type:"
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbType, New Integer(-1) {})
        Me.cmbType.Location = New System.Drawing.Point(169, 110)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(153, 21)
        Me.cmbType.TabIndex = 9
        '
        'cmbBranch
        '
        Me.cmbBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbBranch, New Integer(-1) {})
        Me.cmbBranch.Location = New System.Drawing.Point(168, 109)
        Me.cmbBranch.Name = "cmbBranch"
        Me.cmbBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBranch.Size = New System.Drawing.Size(153, 21)
        Me.cmbBranch.TabIndex = 46
        Me.cmbBranch.Text = "cmbBranch"
        Me.cmbBranch.Visible = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblPostalCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblTelephone)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblDOB)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPostalCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtTelephoneCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtTelephone)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtDOB)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(457, 181)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Address && Telephone"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'lblPostalCode
        '
        Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostalCode.Location = New System.Drawing.Point(16, 75)
        Me.lblPostalCode.Name = "lblPostalCode"
        Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostalCode.Size = New System.Drawing.Size(89, 17)
        Me.lblPostalCode.TabIndex = 28
        Me.lblPostalCode.Text = "Postcode:"
        '
        'lblAddress1
        '
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(16, 43)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(89, 17)
        Me.lblAddress1.TabIndex = 29
        Me.lblAddress1.Text = "Address line 1:"
        '
        'lblTelephone
        '
        Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephone.Location = New System.Drawing.Point(16, 107)
        Me.lblTelephone.Name = "lblTelephone"
        Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephone.Size = New System.Drawing.Size(89, 17)
        Me.lblTelephone.TabIndex = 30
        Me.lblTelephone.Text = "Telephone:"
        '
        'lblDOB
        '
        Me.lblDOB.BackColor = System.Drawing.SystemColors.Control
        Me.lblDOB.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDOB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDOB.Location = New System.Drawing.Point(16, 139)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDOB.Size = New System.Drawing.Size(85, 13)
        Me.lblDOB.TabIndex = 40
        Me.lblDOB.Text = "Date of Birth:"
        '
        'txtPostalCode
        '
        Me.txtPostalCode.AcceptsReturn = True
        Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostalCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostalCode.Location = New System.Drawing.Point(112, 72)
        Me.txtPostalCode.MaxLength = 0
        Me.txtPostalCode.Name = "txtPostalCode"
        Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostalCode.Size = New System.Drawing.Size(137, 20)
        Me.txtPostalCode.TabIndex = 12
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(112, 40)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(289, 20)
        Me.txtAddress1.TabIndex = 11
        '
        'txtTelephoneCode
        '
        Me.txtTelephoneCode.AcceptsReturn = True
        Me.txtTelephoneCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtTelephoneCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTelephoneCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTelephoneCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTelephoneCode.Location = New System.Drawing.Point(112, 104)
        Me.txtTelephoneCode.MaxLength = 0
        Me.txtTelephoneCode.Name = "txtTelephoneCode"
        Me.txtTelephoneCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTelephoneCode.Size = New System.Drawing.Size(89, 20)
        Me.txtTelephoneCode.TabIndex = 13
        '
        'txtTelephone
        '
        Me.txtTelephone.AcceptsReturn = True
        Me.txtTelephone.BackColor = System.Drawing.SystemColors.Window
        Me.txtTelephone.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTelephone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTelephone.Location = New System.Drawing.Point(208, 104)
        Me.txtTelephone.MaxLength = 0
        Me.txtTelephone.Name = "txtTelephone"
        Me.txtTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTelephone.Size = New System.Drawing.Size(193, 20)
        Me.txtTelephone.TabIndex = 14
        '
        'txtDOB
        '
        Me.txtDOB.AcceptsReturn = True
        Me.txtDOB.BackColor = System.Drawing.SystemColors.Window
        Me.txtDOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDOB.Location = New System.Drawing.Point(112, 136)
        Me.txtDOB.MaxLength = 0
        Me.txtDOB.Name = "txtDOB"
        Me.txtDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDOB.Size = New System.Drawing.Size(153, 20)
        Me.txtDOB.TabIndex = 15
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.txtInsReference)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdPolicyRefFind)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(457, 181)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Insurance"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'txtInsReference
        '
        Me.txtInsReference.AcceptsReturn = True
        Me.txtInsReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsReference.Location = New System.Drawing.Point(168, 40)
        Me.txtInsReference.MaxLength = 0
        Me.txtInsReference.Name = "txtInsReference"
        Me.txtInsReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsReference.Size = New System.Drawing.Size(201, 20)
        Me.txtInsReference.TabIndex = 16
        '
        'cmdPolicyRefFind
        '
        Me.cmdPolicyRefFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicyRefFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicyRefFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicyRefFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicyRefFind.Location = New System.Drawing.Point(16, 40)
        Me.cmdPolicyRefFind.Name = "cmdPolicyRefFind"
        Me.cmdPolicyRefFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicyRefFind.Size = New System.Drawing.Size(144, 20)
        Me.cmdPolicyRefFind.TabIndex = 37
        Me.cmdPolicyRefFind.Text = "Insurance reference"
        Me.cmdPolicyRefFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicyRefFind.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblCaseNumber)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtCaseNumber)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblRiskIndex)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblClaimNo)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtRiskIndex)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtClaimNumber)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(457, 181)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Claim"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'lblCaseNumber
        '
        Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseNumber.Location = New System.Drawing.Point(16, 82)
        Me.lblCaseNumber.Name = "lblCaseNumber"
        Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseNumber.Size = New System.Drawing.Size(145, 17)
        Me.lblCaseNumber.TabIndex = 47
        Me.lblCaseNumber.Text = "Case Number:"
        Me.lblCaseNumber.Visible = False
        '
        'txtCaseNumber
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(168, 79)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(153, 20)
        Me.txtCaseNumber.TabIndex = 46
        Me.txtCaseNumber.Visible = False
        '
        'lblRiskIndex
        '
        Me.lblRiskIndex.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskIndex.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskIndex.Location = New System.Drawing.Point(16, 55)
        Me.lblRiskIndex.Name = "lblRiskIndex"
        Me.lblRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskIndex.Size = New System.Drawing.Size(145, 17)
        Me.lblRiskIndex.TabIndex = 44
        Me.lblRiskIndex.Text = "Risk Index:"
        '
        'lblClaimNo
        '
        Me.lblClaimNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimNo.Location = New System.Drawing.Point(16, 32)
        Me.lblClaimNo.Name = "lblClaimNo"
        Me.lblClaimNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimNo.Size = New System.Drawing.Size(95, 16)
        Me.lblClaimNo.TabIndex = 45
        Me.lblClaimNo.Text = "Claim Number:"
        '
        'txtRiskIndex
        '
        Me.txtRiskIndex.AcceptsReturn = True
        Me.txtRiskIndex.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskIndex.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskIndex.Location = New System.Drawing.Point(168, 52)
        Me.txtRiskIndex.MaxLength = 0
        Me.txtRiskIndex.Name = "txtRiskIndex"
        Me.txtRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskIndex.Size = New System.Drawing.Size(153, 20)
        Me.txtRiskIndex.TabIndex = 42
        '
        'txtClaimNumber
        '
        Me.txtClaimNumber.AcceptsReturn = True
        Me.txtClaimNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimNumber.Location = New System.Drawing.Point(168, 28)
        Me.txtClaimNumber.MaxLength = 0
        Me.txtClaimNumber.Name = "txtClaimNumber"
        Me.txtClaimNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimNumber.Size = New System.Drawing.Size(153, 20)
        Me.txtClaimNumber.TabIndex = 43
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 442)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(568, 22)
        Me.stbStatus.TabIndex = 27
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
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(542, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8, Me._lvwSearchDetails_ColumnHeader_9, Me._lvwSearchDetails_ColumnHeader_10, Me._lvwSearchDetails_ColumnHeader_11, Me._lvwSearchDetails_ColumnHeader_12, Me._lvwSearchDetails_ColumnHeader_13, Me._lvwSearchDetails_ColumnHeader_14, Me._lvwSearchDetails_ColumnHeader_15, Me._lvwSearchDetails_ColumnHeader_16, Me._lvwSearchDetails_ColumnHeader_17})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 242)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(560, 162)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 17
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "1"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "2"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "3"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "4"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "5"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "6"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "7"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = "8"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_9
        '
        Me._lvwSearchDetails_ColumnHeader_9.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_9.Text = "9"
        Me._lvwSearchDetails_ColumnHeader_9.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_10
        '
        Me._lvwSearchDetails_ColumnHeader_10.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_10.Text = "10"
        Me._lvwSearchDetails_ColumnHeader_10.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_11
        '
        Me._lvwSearchDetails_ColumnHeader_11.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_11.Text = "11"
        Me._lvwSearchDetails_ColumnHeader_11.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_12
        '
        Me._lvwSearchDetails_ColumnHeader_12.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_12.Text = "12"
        Me._lvwSearchDetails_ColumnHeader_12.Width = 0
        '
        '_lvwSearchDetails_ColumnHeader_13
        '
        Me._lvwSearchDetails_ColumnHeader_13.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_13.Text = "13"
        Me._lvwSearchDetails_ColumnHeader_13.Width = 0
        '
        '_lvwSearchDetails_ColumnHeader_14
        '
        Me._lvwSearchDetails_ColumnHeader_14.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_14.Text = "14"
        Me._lvwSearchDetails_ColumnHeader_14.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_15
        '
        Me._lvwSearchDetails_ColumnHeader_15.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_15.Text = "15"
        Me._lvwSearchDetails_ColumnHeader_15.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_16
        '
        Me._lvwSearchDetails_ColumnHeader_16.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_16.Text = "16"
        Me._lvwSearchDetails_ColumnHeader_16.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_17
        '
        Me._lvwSearchDetails_ColumnHeader_17.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_17.Text = "17"
        Me._lvwSearchDetails_ColumnHeader_17.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        Me.imglImages.Images.SetKeyName(1, "FindImage2")
        '
        'cmbAgentGroup
        '
        Me.cmbAgentGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cmbAgentGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbAgentGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAgentGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAgentGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cmbAgentGroup, New Integer(-1) {})
        Me.cmbAgentGroup.Location = New System.Drawing.Point(168, 8)
        Me.cmbAgentGroup.Name = "cmbAgentGroup"
        Me.cmbAgentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbAgentGroup.Size = New System.Drawing.Size(153, 21)
        Me.cmbAgentGroup.TabIndex = 47
        Me.cmbAgentGroup.Visible = False
        '
        'lblAgentGroup
        '
        Me.lblAgentGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentGroup.Location = New System.Drawing.Point(16, 10)
        Me.lblAgentGroup.Name = "lblAgentGroup"
        Me.lblAgentGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentGroup.Size = New System.Drawing.Size(137, 17)
        Me.lblAgentGroup.TabIndex = 48
        Me.lblAgentGroup.Text = "Agent Group:"
        Me.lblAgentGroup.Visible = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(568, 464)
        Me.Controls.Add(Me.cmdUsers)
        Me.Controls.Add(Me.chkPM)
        Me.Controls.Add(Me.chkSirius)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchDetails)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(159, 167)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find: Client"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage2.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage3.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializemnuRecentFile()
        'Me.mnuRecentFile(0) = _mnuRecentFile_0
        Me.mnuRecentFile(1) = _mnuRecentFile_1
        Me.mnuRecentFile(2) = _mnuRecentFile_2
        Me.mnuRecentFile(3) = _mnuRecentFile_3
        Me.mnuRecentFile(4) = _mnuRecentFile_4
        Me.mnuRecentFile(5) = _mnuRecentFile_5
    End Sub
    Sub lvwSearchDetails_InitializeColumnKeys()
        Me._lvwSearchDetails_ColumnHeader_1.Name = ""
        Me._lvwSearchDetails_ColumnHeader_2.Name = ""
        Me._lvwSearchDetails_ColumnHeader_3.Name = ""
        Me._lvwSearchDetails_ColumnHeader_4.Name = ""
        Me._lvwSearchDetails_ColumnHeader_5.Name = ""
        Me._lvwSearchDetails_ColumnHeader_6.Name = ""
        Me._lvwSearchDetails_ColumnHeader_7.Name = ""
        Me._lvwSearchDetails_ColumnHeader_8.Name = ""
        Me._lvwSearchDetails_ColumnHeader_9.Name = ""
        Me._lvwSearchDetails_ColumnHeader_10.Name = ""
        Me._lvwSearchDetails_ColumnHeader_11.Name = ""
        Me._lvwSearchDetails_ColumnHeader_12.Name = ""
        Me._lvwSearchDetails_ColumnHeader_13.Name = ""
        Me._lvwSearchDetails_ColumnHeader_14.Name = ""
        Me._lvwSearchDetails_ColumnHeader_15.Name = ""
        Me._lvwSearchDetails_ColumnHeader_16.Name = ""
        Me._lvwSearchDetails_ColumnHeader_17.Name = ""
    End Sub
    Public WithEvents lblCaseNumber As System.Windows.Forms.Label
    Public WithEvents txtCaseNumber As System.Windows.Forms.TextBox
    Public WithEvents cmbAgentGroup As System.Windows.Forms.ComboBox
    Public WithEvents lblAgentGroup As System.Windows.Forms.Label
#End Region 
End Class