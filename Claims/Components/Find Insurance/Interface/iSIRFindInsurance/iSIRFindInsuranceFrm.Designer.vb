<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Private WithEvents _stbstatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbstatus As System.Windows.Forms.StatusStrip
	Public WithEvents StatusBar1 As System.Windows.Forms.PictureBox
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblCoverNoteSheetNo As System.Windows.Forms.Label
	Public WithEvents lblClientName As System.Windows.Forms.Label
	Public WithEvents lblDPARequired As System.Windows.Forms.Label
	Public WithEvents lblExcLapsed As System.Windows.Forms.Label
	Public WithEvents lblclaimdate As System.Windows.Forms.Label
	Public WithEvents lblRiskIndex As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents txtCoverNoteSheetNo As System.Windows.Forms.TextBox
	Public WithEvents txtClientName As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents chkDPARequired As System.Windows.Forms.CheckBox
	Public WithEvents chkExcLapsed As System.Windows.Forms.CheckBox
	Public WithEvents txtShortname2 As System.Windows.Forms.TextBox
	Public WithEvents cmdRelatedPartyFind2 As System.Windows.Forms.Button
	Public WithEvents txtClaimDate As System.Windows.Forms.TextBox
	Public WithEvents txtRiskIndex As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cmdRelatedPartyFind As System.Windows.Forms.Button
	Public WithEvents txtFromDate As System.Windows.Forms.TextBox
	Public WithEvents txtToDate As System.Windows.Forms.TextBox
	Public WithEvents txtPostcode As System.Windows.Forms.TextBox
	Public WithEvents txtShortName As System.Windows.Forms.TextBox
	Public WithEvents lblPostCode As System.Windows.Forms.Label
	Public WithEvents lblInForceFromDate As System.Windows.Forms.Label
	Public WithEvents lblInForceTodate As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon displai in listview)
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.stbstatus = New System.Windows.Forms.StatusStrip
        Me._stbstatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.StatusBar1 = New System.Windows.Forms.PictureBox
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblCoverNoteSheetNo = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblClientName = New System.Windows.Forms.Label
        Me.lblDPARequired = New System.Windows.Forms.Label
        Me.lblExcLapsed = New System.Windows.Forms.Label
        Me.lblclaimdate = New System.Windows.Forms.Label
        Me.lblRiskIndex = New System.Windows.Forms.Label
        Me.cmdRelatedPartyFind2 = New System.Windows.Forms.Button
        Me.txtClientName = New System.Windows.Forms.TextBox
        Me.chkDPARequired = New System.Windows.Forms.CheckBox
        Me.chkExcLapsed = New System.Windows.Forms.CheckBox
        Me.txtClaimDate = New System.Windows.Forms.TextBox
        Me.txtCoverNoteSheetNo = New System.Windows.Forms.TextBox
        Me.txtShortname2 = New System.Windows.Forms.TextBox
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtRiskIndex = New System.Windows.Forms.TextBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.cmdRelatedPartyFind = New System.Windows.Forms.Button
        Me.txtFromDate = New System.Windows.Forms.TextBox
        Me.txtToDate = New System.Windows.Forms.TextBox
        Me.txtPostcode = New System.Windows.Forms.TextBox
        Me.txtShortName = New System.Windows.Forms.TextBox
        Me.lblPostCode = New System.Windows.Forms.Label
        Me.lblInForceFromDate = New System.Windows.Forms.Label
        Me.lblInForceTodate = New System.Windows.Forms.Label
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.stbstatus.SuspendLayout()
        CType(Me.StatusBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(9, 181)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(545, 175)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 0
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = ""
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'stbstatus
        '
        Me.stbstatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbstatus_Panel1})
        Me.stbstatus.Location = New System.Drawing.Point(0, 389)
        Me.stbstatus.Name = "stbstatus"
        Me.stbstatus.ShowItemToolTips = True
        Me.stbstatus.Size = New System.Drawing.Size(562, 22)
        Me.stbstatus.TabIndex = 6
        '
        '_stbstatus_Panel1
        '
        Me._stbstatus_Panel1.AutoSize = False
        Me._stbstatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbstatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbstatus_Panel1.DoubleClickEnabled = True
        Me._stbstatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbstatus_Panel1.Name = "_stbstatus_Panel1"
        Me._stbstatus_Panel1.Size = New System.Drawing.Size(545, 22)
        Me._stbstatus_Panel1.Tag = ""
        Me._stbstatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusBar1
        '
        Me.StatusBar1.BackColor = System.Drawing.SystemColors.Control
        Me.StatusBar1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.StatusBar1.Cursor = System.Windows.Forms.Cursors.Default
        Me.StatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.StatusBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Location = New System.Drawing.Point(0, 411)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(562, 2)
        Me.StatusBar1.TabIndex = 7
        Me.StatusBar1.TabStop = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(474, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(80, 22)
        Me.cmdNewSearch.TabIndex = 2
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "&New Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(474, 32)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(80, 22)
        Me.cmdFindNow.TabIndex = 1
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "&Find Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Enabled = False
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(481, 362)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 5
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(396, 362)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(311, 362)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(227, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(461, 166)
        Me.tabMainTab.TabIndex = 8
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCoverNoteSheetNo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPolicyNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClientName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblExcLapsed)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblclaimdate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskIndex)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRelatedPartyFind2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClientName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkDPARequired)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkExcLapsed)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaimDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCoverNoteSheetNo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtShortname2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicyNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRiskIndex)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(453, 140)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblCoverNoteSheetNo
        '
        Me.lblCoverNoteSheetNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverNoteSheetNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverNoteSheetNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverNoteSheetNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverNoteSheetNo.Location = New System.Drawing.Point(7, 44)
        Me.lblCoverNoteSheetNo.Name = "lblCoverNoteSheetNo"
        Me.lblCoverNoteSheetNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverNoteSheetNo.Size = New System.Drawing.Size(171, 17)
        Me.lblCoverNoteSheetNo.TabIndex = 23
        Me.lblCoverNoteSheetNo.Text = "Cover Note Sheet Number:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(8, 44)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(112, 20)
        Me.lblPolicyNumber.TabIndex = 29
        Me.lblPolicyNumber.Text = "Policy number:"
        '
        'lblClientName
        '
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(8, 92)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(89, 17)
        Me.lblClientName.TabIndex = 24
        Me.lblClientName.Text = "Client na&me:"
        '
        'lblDPARequired
        '
        Me.lblDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.lblDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDPARequired.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDPARequired.Location = New System.Drawing.Point(200, 18)
        Me.lblDPARequired.Name = "lblDPARequired"
        Me.lblDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDPARequired.Size = New System.Drawing.Size(45, 21)
        Me.lblDPARequired.TabIndex = 25
        Me.lblDPARequired.Text = "No"
        '
        'lblExcLapsed
        '
        Me.lblExcLapsed.BackColor = System.Drawing.SystemColors.Control
        Me.lblExcLapsed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExcLapsed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExcLapsed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExcLapsed.Location = New System.Drawing.Point(336, 12)
        Me.lblExcLapsed.Name = "lblExcLapsed"
        Me.lblExcLapsed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExcLapsed.Size = New System.Drawing.Size(97, 28)
        Me.lblExcLapsed.TabIndex = 26
        Me.lblExcLapsed.Text = "Exclude Lapsed Policies:"
        '
        'lblclaimdate
        '
        Me.lblclaimdate.BackColor = System.Drawing.SystemColors.Control
        Me.lblclaimdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblclaimdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblclaimdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblclaimdate.Location = New System.Drawing.Point(8, 116)
        Me.lblclaimdate.Name = "lblclaimdate"
        Me.lblclaimdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblclaimdate.Size = New System.Drawing.Size(89, 17)
        Me.lblclaimdate.TabIndex = 27
        Me.lblclaimdate.Text = "Claim date:"
        '
        'lblRiskIndex
        '
        Me.lblRiskIndex.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskIndex.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskIndex.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskIndex.Location = New System.Drawing.Point(8, 68)
        Me.lblRiskIndex.Name = "lblRiskIndex"
        Me.lblRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskIndex.Size = New System.Drawing.Size(89, 17)
        Me.lblRiskIndex.TabIndex = 28
        Me.lblRiskIndex.Text = "Risk index:"
        '
        'cmdRelatedPartyFind2
        '
        Me.cmdRelatedPartyFind2.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRelatedPartyFind2.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRelatedPartyFind2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRelatedPartyFind2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelatedPartyFind2.Location = New System.Drawing.Point(11, 64)
        Me.cmdRelatedPartyFind2.Name = "cmdRelatedPartyFind2"
        Me.cmdRelatedPartyFind2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelatedPartyFind2.Size = New System.Drawing.Size(109, 19)
        Me.cmdRelatedPartyFind2.TabIndex = 15
        Me.cmdRelatedPartyFind2.Text = "Party"
        Me.cmdRelatedPartyFind2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelatedPartyFind2.UseVisualStyleBackColor = False
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = True
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(185, 92)
        Me.txtClientName.MaxLength = 255
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(217, 21)
        Me.txtClientName.TabIndex = 10
        '
        'chkDPARequired
        '
        Me.chkDPARequired.BackColor = System.Drawing.SystemColors.Control
        Me.chkDPARequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDPARequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDPARequired.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDPARequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDPARequired.Location = New System.Drawing.Point(6, 12)
        Me.chkDPARequired.Name = "chkDPARequired"
        Me.chkDPARequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDPARequired.Size = New System.Drawing.Size(175, 25)
        Me.chkDPARequired.TabIndex = 12
        Me.chkDPARequired.Text = "DPA Info Required?"
        Me.chkDPARequired.UseVisualStyleBackColor = False
        '
        'chkExcLapsed
        '
        Me.chkExcLapsed.BackColor = System.Drawing.SystemColors.Control
        Me.chkExcLapsed.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExcLapsed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExcLapsed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExcLapsed.Location = New System.Drawing.Point(432, 4)
        Me.chkExcLapsed.Name = "chkExcLapsed"
        Me.chkExcLapsed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExcLapsed.Size = New System.Drawing.Size(17, 33)
        Me.chkExcLapsed.TabIndex = 13
        Me.chkExcLapsed.UseVisualStyleBackColor = False
        '
        'txtClaimDate
        '
        Me.txtClaimDate.AcceptsReturn = True
        Me.txtClaimDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimDate.Location = New System.Drawing.Point(185, 116)
        Me.txtClaimDate.MaxLength = 0
        Me.txtClaimDate.Name = "txtClaimDate"
        Me.txtClaimDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimDate.Size = New System.Drawing.Size(137, 21)
        Me.txtClaimDate.TabIndex = 17
        '
        'txtCoverNoteSheetNo
        '
        Me.txtCoverNoteSheetNo.AcceptsReturn = True
        Me.txtCoverNoteSheetNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverNoteSheetNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverNoteSheetNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverNoteSheetNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverNoteSheetNo.Location = New System.Drawing.Point(185, 42)
        Me.txtCoverNoteSheetNo.MaxLength = 50
        Me.txtCoverNoteSheetNo.Name = "txtCoverNoteSheetNo"
        Me.txtCoverNoteSheetNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverNoteSheetNo.Size = New System.Drawing.Size(137, 21)
        Me.txtCoverNoteSheetNo.TabIndex = 9
        '
        'txtShortname2
        '
        Me.txtShortname2.AcceptsReturn = True
        Me.txtShortname2.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortname2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortname2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortname2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortname2.Location = New System.Drawing.Point(185, 68)
        Me.txtShortname2.MaxLength = 20
        Me.txtShortname2.Name = "txtShortname2"
        Me.txtShortname2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortname2.Size = New System.Drawing.Size(137, 21)
        Me.txtShortname2.TabIndex = 14
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(185, 42)
        Me.txtPolicyNumber.MaxLength = 50
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(217, 21)
        Me.txtPolicyNumber.TabIndex = 11
        '
        'txtRiskIndex
        '
        Me.txtRiskIndex.AcceptsReturn = True
        Me.txtRiskIndex.BackColor = System.Drawing.SystemColors.Window
        Me.txtRiskIndex.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskIndex.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskIndex.Location = New System.Drawing.Point(185, 68)
        Me.txtRiskIndex.MaxLength = 10
        Me.txtRiskIndex.Name = "txtRiskIndex"
        Me.txtRiskIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskIndex.Size = New System.Drawing.Size(217, 21)
        Me.txtRiskIndex.TabIndex = 18
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdRelatedPartyFind)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtFromDate)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtToDate)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtPostcode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtShortName)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblPostCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblInForceFromDate)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblInForceTodate)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(453, 140)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Tab 1"
        '
        'cmdRelatedPartyFind
        '
        Me.cmdRelatedPartyFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRelatedPartyFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRelatedPartyFind.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRelatedPartyFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRelatedPartyFind.Location = New System.Drawing.Point(8, 12)
        Me.cmdRelatedPartyFind.Name = "cmdRelatedPartyFind"
        Me.cmdRelatedPartyFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRelatedPartyFind.Size = New System.Drawing.Size(94, 19)
        Me.cmdRelatedPartyFind.TabIndex = 16
        Me.cmdRelatedPartyFind.Text = "Party"
        Me.cmdRelatedPartyFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRelatedPartyFind.UseVisualStyleBackColor = False
        '
        'txtFromDate
        '
        Me.txtFromDate.AcceptsReturn = True
        Me.txtFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFromDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFromDate.Location = New System.Drawing.Point(128, 74)
        Me.txtFromDate.MaxLength = 0
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFromDate.Size = New System.Drawing.Size(129, 21)
        Me.txtFromDate.TabIndex = 19
        '
        'txtToDate
        '
        Me.txtToDate.AcceptsReturn = True
        Me.txtToDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtToDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtToDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtToDate.Location = New System.Drawing.Point(128, 104)
        Me.txtToDate.MaxLength = 0
        Me.txtToDate.Name = "txtToDate"
        Me.txtToDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtToDate.Size = New System.Drawing.Size(129, 21)
        Me.txtToDate.TabIndex = 20
        '
        'txtPostcode
        '
        Me.txtPostcode.AcceptsReturn = True
        Me.txtPostcode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostcode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostcode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostcode.Location = New System.Drawing.Point(128, 43)
        Me.txtPostcode.MaxLength = 20
        Me.txtPostcode.Name = "txtPostcode"
        Me.txtPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostcode.Size = New System.Drawing.Size(81, 21)
        Me.txtPostcode.TabIndex = 21
        '
        'txtShortName
        '
        Me.txtShortName.AcceptsReturn = True
        Me.txtShortName.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortName.Location = New System.Drawing.Point(128, 12)
        Me.txtShortName.MaxLength = 20
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortName.Size = New System.Drawing.Size(217, 21)
        Me.txtShortName.TabIndex = 22
        '
        'lblPostCode
        '
        Me.lblPostCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostCode.Location = New System.Drawing.Point(8, 46)
        Me.lblPostCode.Name = "lblPostCode"
        Me.lblPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostCode.Size = New System.Drawing.Size(89, 17)
        Me.lblPostCode.TabIndex = 30
        Me.lblPostCode.Text = "Post code:"
        '
        'lblInForceFromDate
        '
        Me.lblInForceFromDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInForceFromDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInForceFromDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInForceFromDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInForceFromDate.Location = New System.Drawing.Point(8, 77)
        Me.lblInForceFromDate.Name = "lblInForceFromDate"
        Me.lblInForceFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInForceFromDate.Size = New System.Drawing.Size(107, 17)
        Me.lblInForceFromDate.TabIndex = 31
        Me.lblInForceFromDate.Text = "In force from:"
        '
        'lblInForceTodate
        '
        Me.lblInForceTodate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInForceTodate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInForceTodate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInForceTodate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInForceTodate.Location = New System.Drawing.Point(8, 107)
        Me.lblInForceTodate.Name = "lblInForceTodate"
        Me.lblInForceTodate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInForceTodate.Size = New System.Drawing.Size(107, 17)
        Me.lblInForceTodate.TabIndex = 32
        Me.lblInForceTodate.Text = "In force to:"
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(496, 104)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 9
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(562, 413)
        Me.Controls.Add(Me.lvwSearchDetails)
        Me.Controls.Add(Me.stbstatus)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find: Insurance File"
        Me.stbstatus.ResumeLayout(False)
        Me.stbstatus.PerformLayout()
        CType(Me.StatusBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwSearchDetails_InitializeColumnKeys()
		Me._lvwSearchDetails_ColumnHeader_1.Name = ""
	End Sub
#End Region 
End Class