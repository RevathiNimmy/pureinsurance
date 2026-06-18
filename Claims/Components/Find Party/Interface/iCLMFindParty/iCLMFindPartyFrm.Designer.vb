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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblPartyClaimID As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents txtPartyClaimID As System.Windows.Forms.TextBox
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtPhoneNumber As System.Windows.Forms.TextBox
	Public WithEvents txtAddress As System.Windows.Forms.TextBox
	Public WithEvents lblPhoneNumber As System.Windows.Forms.Label
	Public WithEvents lblAddress As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.lvwSearchDetails = New System.Windows.Forms.ListView
		Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdNew = New System.Windows.Forms.Button
		Me.cmdNewSearch = New System.Windows.Forms.Button
		Me.cmdFindNow = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblPartyClaimID = New System.Windows.Forms.Label
		Me.lblName = New System.Windows.Forms.Label
		Me.lblType = New System.Windows.Forms.Label
		Me.txtPartyClaimID = New System.Windows.Forms.TextBox
		Me.txtName = New System.Windows.Forms.TextBox
		Me.cmbType = New System.Windows.Forms.ComboBox
		Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
		Me.txtPhoneNumber = New System.Windows.Forms.TextBox
		Me.txtAddress = New System.Windows.Forms.TextBox
		Me.lblPhoneNumber = New System.Windows.Forms.Label
		Me.lblAddress = New System.Windows.Forms.Label
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.stbStatus.SuspendLayout()
		Me.lvwSearchDetails.SuspendLayout()
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me._tabMainTab_TabPage1.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 402)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(559, 17)
		Me.stbStatus.TabIndex = 20
		Me.stbStatus.Text = ""
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbStatus_Panel1})
		' 
		' _stbStatus_Panel1
		' 
		Me._stbStatus_Panel1.AutoSize = False
		Me._stbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbStatus_Panel1.DoubleClickEnabled = True
		Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(96, 17)
		Me._stbStatus_Panel1.Tag = ""
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' lvwSearchDetails
		' 
		Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSearchDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDetails.HideSelection = True
		Me.lvwSearchDetails.LabelEdit = False
		Me.lvwSearchDetails.LabelWrap = True
		Me.lvwSearchDetails.LargeImageList = imglImages
		Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 144)
		Me.lvwSearchDetails.Name = "lvwSearchDetails"
		Me.lvwSearchDetails.Size = New System.Drawing.Size(545, 225)
		Me.lvwSearchDetails.SmallImageList = imglImages
		Me.lvwSearchDetails.TabIndex = 19
		Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_1)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_2)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_3)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_4)
		' 
		' _lvwSearchDetails_ColumnHeader_1
		' 
		Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_1.Text = "Name"
		Me._lvwSearchDetails_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_2
		' 
		Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_2.Text = "Address"
		Me._lvwSearchDetails_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_3
		' 
		Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_3.Text = "Phone Number"
		Me._lvwSearchDetails_ColumnHeader_3.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_4
		' 
		Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_4.Text = "Type"
		Me._lvwSearchDetails_ColumnHeader_4.Width = 97
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(168, 376)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 12
		Me.cmdNavigate.TabStop = False
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = False
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(88, 376)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 11
		Me.cmdEdit.TabStop = False
		Me.cmdEdit.Text = "E&dit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNew
		' 
		Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNew.CausesValidation = True
		Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNew.Enabled = False
		Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNew.Location = New System.Drawing.Point(8, 376)
		Me.cmdNew.Name = "cmdNew"
		Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNew.Size = New System.Drawing.Size(73, 22)
		Me.cmdNew.TabIndex = 10
		Me.cmdNew.TabStop = False
		Me.cmdNew.Text = "N&ew"
		Me.cmdNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNewSearch
		' 
		Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewSearch.CausesValidation = True
		Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewSearch.Enabled = True
		Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewSearch.Location = New System.Drawing.Point(472, 72)
		Me.cmdNewSearch.Name = "cmdNewSearch"
		Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
		Me.cmdNewSearch.TabIndex = 6
		Me.cmdNewSearch.TabStop = False
		Me.cmdNewSearch.Text = "Ne&w Search"
		Me.cmdNewSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdFindNow
		' 
		Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindNow.CausesValidation = True
		Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindNow.Enabled = False
		Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindNow.Location = New System.Drawing.Point(472, 40)
		Me.cmdFindNow.Name = "cmdFindNow"
		Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
		Me.cmdFindNow.TabIndex = 5
		Me.cmdFindNow.TabStop = False
		Me.cmdFindNow.Text = "F&ind Now"
		Me.cmdFindNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(480, 376)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 9
		Me.cmdHelp.TabStop = False
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(400, 376)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 8
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(320, 376)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 7
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(151, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(461, 133)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 13
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblPartyClaimID)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtPartyClaimID)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbType)
		Me._tabMainTab_TabPage0.Text = "&1 - Client"
		' 
		' lblPartyClaimID
		' 
		Me.lblPartyClaimID.AutoSize = False
		Me.lblPartyClaimID.BackColor = System.Drawing.SystemColors.Control
		Me.lblPartyClaimID.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPartyClaimID.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPartyClaimID.Enabled = True
		Me.lblPartyClaimID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPartyClaimID.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPartyClaimID.Location = New System.Drawing.Point(16, 20)
		Me.lblPartyClaimID.Name = "lblPartyClaimID"
		Me.lblPartyClaimID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPartyClaimID.Size = New System.Drawing.Size(145, 17)
		Me.lblPartyClaimID.TabIndex = 16
		Me.lblPartyClaimID.Text = "Client code:"
		Me.lblPartyClaimID.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPartyClaimID.UseMnemonic = True
		Me.lblPartyClaimID.Visible = True
		' 
		' lblName
		' 
		Me.lblName.AutoSize = False
		Me.lblName.BackColor = System.Drawing.SystemColors.Control
		Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblName.Enabled = True
		Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblName.Location = New System.Drawing.Point(16, 47)
		Me.lblName.Name = "lblName"
		Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblName.Size = New System.Drawing.Size(145, 17)
		Me.lblName.TabIndex = 17
		Me.lblName.Text = "Name:"
		Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblName.UseMnemonic = True
		Me.lblName.Visible = True
		' 
		' lblType
		' 
		Me.lblType.AutoSize = False
		Me.lblType.BackColor = System.Drawing.SystemColors.Control
		Me.lblType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblType.Enabled = True
		Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblType.Location = New System.Drawing.Point(16, 75)
		Me.lblType.Name = "lblType"
		Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblType.Size = New System.Drawing.Size(145, 17)
		Me.lblType.TabIndex = 18
		Me.lblType.Text = "Type:"
		Me.lblType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblType.UseMnemonic = True
		Me.lblType.Visible = True
		' 
		' txtPartyClaimID
		' 
		Me.txtPartyClaimID.AcceptsReturn = True
		Me.txtPartyClaimID.AutoSize = False
		Me.txtPartyClaimID.BackColor = System.Drawing.SystemColors.Window
		Me.txtPartyClaimID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPartyClaimID.CausesValidation = True
		Me.txtPartyClaimID.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPartyClaimID.Enabled = True
		Me.txtPartyClaimID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPartyClaimID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPartyClaimID.HideSelection = True
		Me.txtPartyClaimID.Location = New System.Drawing.Point(160, 20)
		Me.txtPartyClaimID.MaxLength = 0
		Me.txtPartyClaimID.Multiline = False
		Me.txtPartyClaimID.Name = "txtPartyClaimID"
		Me.txtPartyClaimID.ReadOnly = False
		Me.txtPartyClaimID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPartyClaimID.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPartyClaimID.Size = New System.Drawing.Size(153, 19)
		Me.txtPartyClaimID.TabIndex = 0
		Me.txtPartyClaimID.TabStop = True
		Me.txtPartyClaimID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPartyClaimID.Visible = True
		' 
		' txtName
		' 
		Me.txtName.AcceptsReturn = True
		Me.txtName.AutoSize = False
		Me.txtName.BackColor = System.Drawing.SystemColors.Window
		Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtName.CausesValidation = True
		Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtName.Enabled = True
		Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtName.HideSelection = True
		Me.txtName.Location = New System.Drawing.Point(159, 44)
		Me.txtName.MaxLength = 0
		Me.txtName.Multiline = False
		Me.txtName.Name = "txtName"
		Me.txtName.ReadOnly = False
		Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtName.Size = New System.Drawing.Size(249, 19)
		Me.txtName.TabIndex = 1
		Me.txtName.TabStop = True
		Me.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtName.Visible = True
		' 
		' cmbType
		' 
		Me.cmbType.BackColor = System.Drawing.SystemColors.Window
		Me.cmbType.CausesValidation = True
		Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbType.Enabled = True
		Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbType.IntegralHeight = True
		Me.cmbType.Location = New System.Drawing.Point(160, 72)
		Me.cmbType.Name = "cmbType"
		Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbType.Size = New System.Drawing.Size(153, 21)
		Me.cmbType.Sorted = False
		Me.cmbType.TabIndex = 2
		Me.cmbType.TabStop = True
		Me.cmbType.Visible = True
		' 
		' _tabMainTab_TabPage1
		' 
		Me._tabMainTab_TabPage1.Controls.Add(Me.txtPhoneNumber)
		Me._tabMainTab_TabPage1.Controls.Add(Me.txtAddress)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblPhoneNumber)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblAddress)
		Me._tabMainTab_TabPage1.Text = "&2 - Address && Telephone"
		' 
		' txtPhoneNumber
		' 
		Me.txtPhoneNumber.AcceptsReturn = True
		Me.txtPhoneNumber.AutoSize = False
		Me.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtPhoneNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPhoneNumber.CausesValidation = True
		Me.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPhoneNumber.Enabled = True
		Me.txtPhoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPhoneNumber.HideSelection = True
		Me.txtPhoneNumber.Location = New System.Drawing.Point(120, 52)
		Me.txtPhoneNumber.MaxLength = 0
		Me.txtPhoneNumber.Multiline = False
		Me.txtPhoneNumber.Name = "txtPhoneNumber"
		Me.txtPhoneNumber.ReadOnly = False
		Me.txtPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPhoneNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPhoneNumber.Size = New System.Drawing.Size(281, 19)
		Me.txtPhoneNumber.TabIndex = 4
		Me.txtPhoneNumber.TabStop = True
		Me.txtPhoneNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPhoneNumber.Visible = True
		' 
		' txtAddress
		' 
		Me.txtAddress.AcceptsReturn = True
		Me.txtAddress.AutoSize = False
		Me.txtAddress.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress.CausesValidation = True
		Me.txtAddress.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress.Enabled = True
		Me.txtAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAddress.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress.HideSelection = True
		Me.txtAddress.Location = New System.Drawing.Point(120, 20)
		Me.txtAddress.MaxLength = 0
		Me.txtAddress.Multiline = False
		Me.txtAddress.Name = "txtAddress"
		Me.txtAddress.ReadOnly = False
		Me.txtAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress.Size = New System.Drawing.Size(281, 19)
		Me.txtAddress.TabIndex = 3
		Me.txtAddress.TabStop = True
		Me.txtAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress.Visible = True
		' 
		' lblPhoneNumber
		' 
		Me.lblPhoneNumber.AutoSize = False
		Me.lblPhoneNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblPhoneNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPhoneNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPhoneNumber.Enabled = True
		Me.lblPhoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPhoneNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPhoneNumber.Location = New System.Drawing.Point(16, 52)
		Me.lblPhoneNumber.Name = "lblPhoneNumber"
		Me.lblPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPhoneNumber.Size = New System.Drawing.Size(89, 17)
		Me.lblPhoneNumber.TabIndex = 15
		Me.lblPhoneNumber.Text = "Phone Number:"
		Me.lblPhoneNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPhoneNumber.UseMnemonic = True
		Me.lblPhoneNumber.Visible = True
		' 
		' lblAddress
		' 
		Me.lblAddress.AutoSize = False
		Me.lblAddress.BackColor = System.Drawing.SystemColors.Control
		Me.lblAddress.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAddress.Enabled = True
		Me.lblAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAddress.Location = New System.Drawing.Point(16, 23)
		Me.lblAddress.Name = "lblAddress"
		Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAddress.Size = New System.Drawing.Size(89, 17)
		Me.lblAddress.TabIndex = 14
		Me.lblAddress.Text = "Address :"
		Me.lblAddress.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAddress.UseMnemonic = True
		Me.lblAddress.Visible = True
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imglImages.Images.SetKeyName(0, "FindImage")
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdFindNow
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(559, 419)
		Me.ControlBox = True
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.lvwSearchDetails)
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdEdit)
		Me.Controls.Add(Me.cmdNew)
		Me.Controls.Add(Me.cmdNewSearch)
		Me.Controls.Add(Me.cmdFindNow)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(159, 148)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Find: Client"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 2)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.stbStatus.ResumeLayout(False)
		Me.lvwSearchDetails.ResumeLayout(False)
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me._tabMainTab_TabPage1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwSearchDetails_InitializeColumnKeys()
		Me._lvwSearchDetails_ColumnHeader_1.Name = ""
		Me._lvwSearchDetails_ColumnHeader_2.Name = ""
		Me._lvwSearchDetails_ColumnHeader_3.Name = ""
		Me._lvwSearchDetails_ColumnHeader_4.Name = ""
	End Sub
#End Region 
End Class