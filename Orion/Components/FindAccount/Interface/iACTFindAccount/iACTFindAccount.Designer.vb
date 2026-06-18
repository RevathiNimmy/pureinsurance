<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializepnlMain()
		lvwSearchResults_InitializeColumnKeys()
		tabMainPreviousTab = tabMain.SelectedIndex
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdAccountLookup As System.Windows.Forms.Button
	Public WithEvents chkShowDeleted As System.Windows.Forms.CheckBox
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtFullKey As System.Windows.Forms.TextBox
	Public WithEvents cboLedger As System.Windows.Forms.ComboBox
	Public WithEvents cboAccountType As System.Windows.Forms.ComboBox
	Public WithEvents txtShortCode As System.Windows.Forms.TextBox
	Public WithEvents lblShortCode As System.Windows.Forms.Label
	Public WithEvents lblLedger As System.Windows.Forms.Label
	Public WithEvents lblAccountType As System.Windows.Forms.Label
	Public WithEvents lblFullKey As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Private WithEvents _pnlMain_0 As System.Windows.Forms.Panel
	Public WithEvents chkShowBalance As System.Windows.Forms.CheckBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtPurchaseInvoiceNo As System.Windows.Forms.TextBox
	Public WithEvents cboPMUser As PMUserLookupControl.cboPMUserLookup
	Public WithEvents txtInsuranceRef As System.Windows.Forms.TextBox
	Public WithEvents txtPurchaseOrderNo As System.Windows.Forms.TextBox
	Public WithEvents lblPurchaseOrderNo As System.Windows.Forms.Label
	Public WithEvents lblOperatorID As System.Windows.Forms.Label
	Public WithEvents lblPurchaseInvoiceNo As System.Windows.Forms.Label
	Public WithEvents lblInsuranceRef As System.Windows.Forms.Label
	Private WithEvents _pnlMain_1 As System.Windows.Forms.Panel
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchResults As System.Windows.Forms.ListView
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    Public pnlMain(1) As System.Windows.Forms.Panel
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.chkShowBalance = New System.Windows.Forms.CheckBox
        Me._pnlMain_0 = New System.Windows.Forms.Panel
        Me.cmdAccountLookup = New System.Windows.Forms.Button
        Me.chkShowDeleted = New System.Windows.Forms.CheckBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtFullKey = New System.Windows.Forms.TextBox
        Me.cboLedger = New System.Windows.Forms.ComboBox
        Me.cboAccountType = New System.Windows.Forms.ComboBox
        Me.txtShortCode = New System.Windows.Forms.TextBox
        Me.lblShortCode = New System.Windows.Forms.Label
        Me.lblLedger = New System.Windows.Forms.Label
        Me.lblAccountType = New System.Windows.Forms.Label
        Me.lblFullKey = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage
        Me._pnlMain_1 = New System.Windows.Forms.Panel
        Me.txtPurchaseInvoiceNo = New System.Windows.Forms.TextBox
        Me.cboPMUser = New PMUserLookupControl.cboPMUserLookup
        Me.txtInsuranceRef = New System.Windows.Forms.TextBox
        Me.txtPurchaseOrderNo = New System.Windows.Forms.TextBox
        Me.lblPurchaseOrderNo = New System.Windows.Forms.Label
        Me.lblOperatorID = New System.Windows.Forms.Label
        Me.lblPurchaseInvoiceNo = New System.Windows.Forms.Label
        Me.lblInsuranceRef = New System.Windows.Forms.Label
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchResults = New System.Windows.Forms.ListView
        Me._lvwSearchResults_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me._pnlMain_0.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        Me._pnlMain_1.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(168, 360)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 16
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(88, 360)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 15
        Me.cmdNew.TabStop = False
        Me.cmdNew.Text = "N&ew"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        Me.cmdNew.Visible = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(472, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(87, 22)
        Me.cmdNewSearch.TabIndex = 8
        Me.cmdNewSearch.TabStop = False
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
        Me.cmdFindNow.Location = New System.Drawing.Point(472, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(87, 22)
        Me.cmdFindNow.TabIndex = 7
        Me.cmdFindNow.TabStop = False
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
        Me.cmdHelp.Location = New System.Drawing.Point(486, 360)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(75, 22)
        Me.cmdHelp.TabIndex = 19
        Me.cmdHelp.TabStop = False
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
        Me.cmdCancel.Location = New System.Drawing.Point(406, 360)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(75, 22)
        Me.cmdCancel.TabIndex = 18
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
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(326, 360)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(75, 22)
        Me.cmdOK.TabIndex = 17
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.tabMain.ItemSize = New System.Drawing.Size(227, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(461, 166)
        Me.tabMain.TabIndex = 20
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.chkShowBalance)
        Me._tabMain_TabPage0.Controls.Add(Me._pnlMain_0)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(453, 140)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "&1 - Details"
        '
        'chkShowBalance
        '
        Me.chkShowBalance.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowBalance.Location = New System.Drawing.Point(284, 86)
        Me.chkShowBalance.Name = "chkShowBalance"
        Me.chkShowBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowBalance.Size = New System.Drawing.Size(167, 19)
        Me.chkShowBalance.TabIndex = 33
        Me.chkShowBalance.Text = "Show Account Balance"
        Me.chkShowBalance.UseVisualStyleBackColor = False
        '
        '_pnlMain_0
        '
        Me._pnlMain_0.Controls.Add(Me.cmdAccountLookup)
        Me._pnlMain_0.Controls.Add(Me.chkShowDeleted)
        Me._pnlMain_0.Controls.Add(Me.txtName)
        Me._pnlMain_0.Controls.Add(Me.txtFullKey)
        Me._pnlMain_0.Controls.Add(Me.cboLedger)
        Me._pnlMain_0.Controls.Add(Me.cboAccountType)
        Me._pnlMain_0.Controls.Add(Me.txtShortCode)
        Me._pnlMain_0.Controls.Add(Me.lblShortCode)
        Me._pnlMain_0.Controls.Add(Me.lblLedger)
        Me._pnlMain_0.Controls.Add(Me.lblAccountType)
        Me._pnlMain_0.Controls.Add(Me.lblFullKey)
        Me._pnlMain_0.Controls.Add(Me.lblName)
        Me._pnlMain_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_0.Location = New System.Drawing.Point(8, 4)
        Me._pnlMain_0.Name = "_pnlMain_0"
        Me._pnlMain_0.Size = New System.Drawing.Size(446, 131)
        Me._pnlMain_0.TabIndex = 26
        '
        'cmdAccountLookup
        '
        Me.cmdAccountLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccountLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccountLookup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccountLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccountLookup.Location = New System.Drawing.Point(414, 56)
        Me.cmdAccountLookup.Name = "cmdAccountLookup"
        Me.cmdAccountLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccountLookup.Size = New System.Drawing.Size(23, 21)
        Me.cmdAccountLookup.TabIndex = 3
        Me.cmdAccountLookup.Text = "..."
        Me.cmdAccountLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccountLookup.UseVisualStyleBackColor = False
        '
        'chkShowDeleted
        '
        Me.chkShowDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowDeleted.Location = New System.Drawing.Point(276, 104)
        Me.chkShowDeleted.Name = "chkShowDeleted"
        Me.chkShowDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowDeleted.Size = New System.Drawing.Size(167, 19)
        Me.chkShowDeleted.TabIndex = 6
        Me.chkShowDeleted.Text = "Show deleted accounts"
        Me.chkShowDeleted.UseVisualStyleBackColor = False
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(104, 32)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(249, 20)
        Me.txtName.TabIndex = 1
        '
        'txtFullKey
        '
        Me.txtFullKey.AcceptsReturn = True
        Me.txtFullKey.BackColor = System.Drawing.SystemColors.Window
        Me.txtFullKey.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFullKey.Enabled = False
        Me.txtFullKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFullKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFullKey.Location = New System.Drawing.Point(104, 56)
        Me.txtFullKey.MaxLength = 0
        Me.txtFullKey.Name = "txtFullKey"
        Me.txtFullKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFullKey.Size = New System.Drawing.Size(307, 20)
        Me.txtFullKey.TabIndex = 2
        '
        'cboLedger
        '
        Me.cboLedger.BackColor = System.Drawing.SystemColors.Window
        Me.cboLedger.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLedger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLedger.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLedger.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLedger.Location = New System.Drawing.Point(104, 104)
        Me.cboLedger.Name = "cboLedger"
        Me.cboLedger.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLedger.Size = New System.Drawing.Size(153, 21)
        Me.cboLedger.TabIndex = 5
        '
        'cboAccountType
        '
        Me.cboAccountType.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccountType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAccountType.Location = New System.Drawing.Point(104, 80)
        Me.cboAccountType.Name = "cboAccountType"
        Me.cboAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccountType.Size = New System.Drawing.Size(153, 21)
        Me.cboAccountType.TabIndex = 4
        '
        'txtShortCode
        '
        Me.txtShortCode.AcceptsReturn = True
        Me.txtShortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortCode.Location = New System.Drawing.Point(104, 8)
        Me.txtShortCode.MaxLength = 0
        Me.txtShortCode.Name = "txtShortCode"
        Me.txtShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortCode.Size = New System.Drawing.Size(153, 20)
        Me.txtShortCode.TabIndex = 0
        '
        'lblShortCode
        '
        Me.lblShortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortCode.Location = New System.Drawing.Point(16, 10)
        Me.lblShortCode.Name = "lblShortCode"
        Me.lblShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortCode.Size = New System.Drawing.Size(81, 19)
        Me.lblShortCode.TabIndex = 21
        Me.lblShortCode.Text = "Short Name:"
        '
        'lblLedger
        '
        Me.lblLedger.BackColor = System.Drawing.SystemColors.Control
        Me.lblLedger.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLedger.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLedger.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLedger.Location = New System.Drawing.Point(16, 108)
        Me.lblLedger.Name = "lblLedger"
        Me.lblLedger.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLedger.Size = New System.Drawing.Size(81, 19)
        Me.lblLedger.TabIndex = 25
        Me.lblLedger.Text = "Ledger:"
        '
        'lblAccountType
        '
        Me.lblAccountType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountType.Location = New System.Drawing.Point(16, 84)
        Me.lblAccountType.Name = "lblAccountType"
        Me.lblAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountType.Size = New System.Drawing.Size(93, 19)
        Me.lblAccountType.TabIndex = 24
        Me.lblAccountType.Text = "Account Type:"
        '
        'lblFullKey
        '
        Me.lblFullKey.BackColor = System.Drawing.SystemColors.Control
        Me.lblFullKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFullKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFullKey.Location = New System.Drawing.Point(16, 58)
        Me.lblFullKey.Name = "lblFullKey"
        Me.lblFullKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFullKey.Size = New System.Drawing.Size(81, 19)
        Me.lblFullKey.TabIndex = 23
        Me.lblFullKey.Text = "Code:"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(16, 34)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(81, 19)
        Me.lblName.TabIndex = 22
        Me.lblName.Text = "Name:"
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me._pnlMain_1)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(453, 140)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "&2 - Reference"
        '
        '_pnlMain_1
        '
        Me._pnlMain_1.Controls.Add(Me.txtPurchaseInvoiceNo)
        Me._pnlMain_1.Controls.Add(Me.cboPMUser)
        Me._pnlMain_1.Controls.Add(Me.txtInsuranceRef)
        Me._pnlMain_1.Controls.Add(Me.txtPurchaseOrderNo)
        Me._pnlMain_1.Controls.Add(Me.lblPurchaseOrderNo)
        Me._pnlMain_1.Controls.Add(Me.lblOperatorID)
        Me._pnlMain_1.Controls.Add(Me.lblPurchaseInvoiceNo)
        Me._pnlMain_1.Controls.Add(Me.lblInsuranceRef)
        Me._pnlMain_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_1.Location = New System.Drawing.Point(8, 4)
        Me._pnlMain_1.Name = "_pnlMain_1"
        Me._pnlMain_1.Size = New System.Drawing.Size(446, 131)
        Me._pnlMain_1.TabIndex = 28
        '
        'txtPurchaseInvoiceNo
        '
        Me.txtPurchaseInvoiceNo.AcceptsReturn = True
        Me.txtPurchaseInvoiceNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPurchaseInvoiceNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPurchaseInvoiceNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurchaseInvoiceNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPurchaseInvoiceNo.Location = New System.Drawing.Point(216, 64)
        Me.txtPurchaseInvoiceNo.MaxLength = 0
        Me.txtPurchaseInvoiceNo.Name = "txtPurchaseInvoiceNo"
        Me.txtPurchaseInvoiceNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPurchaseInvoiceNo.Size = New System.Drawing.Size(153, 20)
        Me.txtPurchaseInvoiceNo.TabIndex = 13
        '
        'cboPMUser
        '
        Me.cboPMUser.DefaultUserID = 0
        Me.cboPMUser.FirstItem = "(all)"
        Me.cboPMUser.ListIndex = -1
        Me.cboPMUser.Location = New System.Drawing.Point(216, 24)
        Me.cboPMUser.Name = "cboPMUser"
        Me.cboPMUser.PMUserGroupID = 0
        Me.cboPMUser.SingleUserID = 0
        Me.cboPMUser.Size = New System.Drawing.Size(153, 21)
        Me.cboPMUser.Sorted = True
        Me.cboPMUser.TabIndex = 11
        Me.cboPMUser.ToolTipText = ""
        Me.cboPMUser.UserID = 0
        '
        'txtInsuranceRef
        '
        Me.txtInsuranceRef.AcceptsReturn = True
        Me.txtInsuranceRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsuranceRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsuranceRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsuranceRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsuranceRef.Location = New System.Drawing.Point(16, 24)
        Me.txtInsuranceRef.MaxLength = 0
        Me.txtInsuranceRef.Name = "txtInsuranceRef"
        Me.txtInsuranceRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsuranceRef.Size = New System.Drawing.Size(153, 20)
        Me.txtInsuranceRef.TabIndex = 10
        '
        'txtPurchaseOrderNo
        '
        Me.txtPurchaseOrderNo.AcceptsReturn = True
        Me.txtPurchaseOrderNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPurchaseOrderNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPurchaseOrderNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurchaseOrderNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPurchaseOrderNo.Location = New System.Drawing.Point(16, 64)
        Me.txtPurchaseOrderNo.MaxLength = 0
        Me.txtPurchaseOrderNo.Name = "txtPurchaseOrderNo"
        Me.txtPurchaseOrderNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPurchaseOrderNo.Size = New System.Drawing.Size(153, 20)
        Me.txtPurchaseOrderNo.TabIndex = 12
        '
        'lblPurchaseOrderNo
        '
        Me.lblPurchaseOrderNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurchaseOrderNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurchaseOrderNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseOrderNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseOrderNo.Location = New System.Drawing.Point(16, 48)
        Me.lblPurchaseOrderNo.Name = "lblPurchaseOrderNo"
        Me.lblPurchaseOrderNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchaseOrderNo.Size = New System.Drawing.Size(153, 17)
        Me.lblPurchaseOrderNo.TabIndex = 32
        Me.lblPurchaseOrderNo.Text = "Purchase Order No:"
        '
        'lblOperatorID
        '
        Me.lblOperatorID.BackColor = System.Drawing.SystemColors.Control
        Me.lblOperatorID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOperatorID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperatorID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOperatorID.Location = New System.Drawing.Point(216, 8)
        Me.lblOperatorID.Name = "lblOperatorID"
        Me.lblOperatorID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOperatorID.Size = New System.Drawing.Size(153, 17)
        Me.lblOperatorID.TabIndex = 31
        Me.lblOperatorID.Text = "Operator:"
        '
        'lblPurchaseInvoiceNo
        '
        Me.lblPurchaseInvoiceNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurchaseInvoiceNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurchaseInvoiceNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseInvoiceNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseInvoiceNo.Location = New System.Drawing.Point(216, 48)
        Me.lblPurchaseInvoiceNo.Name = "lblPurchaseInvoiceNo"
        Me.lblPurchaseInvoiceNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchaseInvoiceNo.Size = New System.Drawing.Size(155, 17)
        Me.lblPurchaseInvoiceNo.TabIndex = 30
        Me.lblPurchaseInvoiceNo.Text = "Purchase Invoice No:"
        '
        'lblInsuranceRef
        '
        Me.lblInsuranceRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsuranceRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsuranceRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsuranceRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsuranceRef.Location = New System.Drawing.Point(16, 8)
        Me.lblInsuranceRef.Name = "lblInsuranceRef"
        Me.lblInsuranceRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsuranceRef.Size = New System.Drawing.Size(155, 17)
        Me.lblInsuranceRef.TabIndex = 29
        Me.lblInsuranceRef.Text = "Insurance Ref:"
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 383)
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
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(552, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchResults
        '
        Me.lvwSearchResults.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchResults_ColumnHeader_1, Me._lvwSearchResults_ColumnHeader_2, Me._lvwSearchResults_ColumnHeader_3, Me._lvwSearchResults_ColumnHeader_4, Me._lvwSearchResults_ColumnHeader_5, Me._lvwSearchResults_ColumnHeader_6, Me._lvwSearchResults_ColumnHeader_7, Me._lvwSearchResults_ColumnHeader_8, Me._lvwSearchResults_ColumnHeader_9})
        Me.lvwSearchResults.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchResults.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchResults.FullRowSelect = True
        Me.lvwSearchResults.HideSelection = False
        Me.lvwSearchResults.LargeImageList = Me.imglImages
        Me.lvwSearchResults.Location = New System.Drawing.Point(8, 174)
        Me.lvwSearchResults.MultiSelect = False
        Me.lvwSearchResults.Name = "lvwSearchResults"
        Me.lvwSearchResults.Size = New System.Drawing.Size(551, 179)
        Me.lvwSearchResults.SmallImageList = Me.imglImages
        Me.lvwSearchResults.TabIndex = 9
        Me.lvwSearchResults.TabStop = False
        Me.lvwSearchResults.UseCompatibleStateImageBehavior = False
        Me.lvwSearchResults.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchResults_ColumnHeader_1
        '
        Me._lvwSearchResults_ColumnHeader_1.Tag = ""
        Me._lvwSearchResults_ColumnHeader_1.Text = "1"
        Me._lvwSearchResults_ColumnHeader_1.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_2
        '
        Me._lvwSearchResults_ColumnHeader_2.Tag = ""
        Me._lvwSearchResults_ColumnHeader_2.Text = "2"
        Me._lvwSearchResults_ColumnHeader_2.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_3
        '
        Me._lvwSearchResults_ColumnHeader_3.Tag = ""
        Me._lvwSearchResults_ColumnHeader_3.Text = "3"
        Me._lvwSearchResults_ColumnHeader_3.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_4
        '
        Me._lvwSearchResults_ColumnHeader_4.Tag = ""
        Me._lvwSearchResults_ColumnHeader_4.Text = "4"
        Me._lvwSearchResults_ColumnHeader_4.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_5
        '
        Me._lvwSearchResults_ColumnHeader_5.Tag = ""
        Me._lvwSearchResults_ColumnHeader_5.Text = "5"
        Me._lvwSearchResults_ColumnHeader_5.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_6
        '
        Me._lvwSearchResults_ColumnHeader_6.Tag = ""
        Me._lvwSearchResults_ColumnHeader_6.Text = "6"
        Me._lvwSearchResults_ColumnHeader_6.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_7
        '
        Me._lvwSearchResults_ColumnHeader_7.Tag = ""
        Me._lvwSearchResults_ColumnHeader_7.Text = "7"
        Me._lvwSearchResults_ColumnHeader_7.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_8
        '
        Me._lvwSearchResults_ColumnHeader_8.Tag = ""
        Me._lvwSearchResults_ColumnHeader_8.Text = "8"
        Me._lvwSearchResults_ColumnHeader_8.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_9
        '
        Me._lvwSearchResults_ColumnHeader_9.Tag = ""
        Me._lvwSearchResults_ColumnHeader_9.Text = "9"
        Me._lvwSearchResults_ColumnHeader_9.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(8, 360)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 14
        Me.cmdEdit.TabStop = False
        Me.cmdEdit.Text = "E&dit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(496, 104)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 28
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(568, 405)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchResults)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(159, 148)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Find: Account"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._pnlMain_0.ResumeLayout(False)
        Me._pnlMain_0.PerformLayout()
        Me._tabMain_TabPage1.ResumeLayout(False)
        Me._pnlMain_1.ResumeLayout(False)
        Me._pnlMain_1.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializepnlMain()
		Me.pnlMain(0) = _pnlMain_0
		Me.pnlMain(1) = _pnlMain_1
	End Sub
	Sub lvwSearchResults_InitializeColumnKeys()
		Me._lvwSearchResults_ColumnHeader_1.Name = ""
		Me._lvwSearchResults_ColumnHeader_2.Name = ""
		Me._lvwSearchResults_ColumnHeader_3.Name = ""
		Me._lvwSearchResults_ColumnHeader_4.Name = ""
		Me._lvwSearchResults_ColumnHeader_5.Name = ""
		Me._lvwSearchResults_ColumnHeader_6.Name = ""
		Me._lvwSearchResults_ColumnHeader_7.Name = ""
		Me._lvwSearchResults_ColumnHeader_8.Name = ""
		Me._lvwSearchResults_ColumnHeader_9.Name = ""
	End Sub
#End Region 
End Class