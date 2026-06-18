<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblReference As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblDateFrom As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents uctType As UserControls.TypeTable
	Public WithEvents uctStatus As UserControls.TypeTable
	Public WithEvents txtReference As System.Windows.Forms.TextBox
	Public WithEvents txtDateFrom As System.Windows.Forms.TextBox
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblBank As System.Windows.Forms.Label
	Public WithEvents lblTotalAmount As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblTotalItems As System.Windows.Forms.Label
	Public WithEvents uctBankAccount As UserControls.BankAccount
	Public WithEvents uctCurrency As UserControls.CurrencyLookup
	Public WithEvents txtTotalAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTotalItems As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
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
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
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
		Me.lblReference = New System.Windows.Forms.Label
		Me.lblType = New System.Windows.Forms.Label
		Me.lblStatus = New System.Windows.Forms.Label
		Me.lblDateFrom = New System.Windows.Forms.Label
		Me.lblDateTo = New System.Windows.Forms.Label
		Me.uctType = New UserControls.TypeTable
		Me.uctStatus = New UserControls.TypeTable
		Me.txtReference = New System.Windows.Forms.TextBox
		Me.txtDateFrom = New System.Windows.Forms.TextBox
		Me.txtDateTo = New System.Windows.Forms.TextBox
		Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
		Me.lblBank = New System.Windows.Forms.Label
		Me.lblTotalAmount = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblTotalItems = New System.Windows.Forms.Label
		Me.uctBankAccount = New UserControls.BankAccount
		Me.uctCurrency = New UserControls.CurrencyLookup
		Me.txtTotalAmount = New System.Windows.Forms.TextBox
		Me.txtTotalItems = New System.Windows.Forms.TextBox
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.lvwSearchDetails = New System.Windows.Forms.ListView
		Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwSearchDetails_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.ImgImage = New System.Windows.Forms.PictureBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me._tabMainTab_TabPage1.SuspendLayout()
		Me.stbStatus.SuspendLayout()
		Me.lvwSearchDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(168, 360)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 27
		Me.cmdNavigate.TabStop = True
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
		Me.cmdEdit.Location = New System.Drawing.Point(88, 360)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 21
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNew
		' 
		Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNew.CausesValidation = True
		Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNew.Enabled = True
		Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNew.Location = New System.Drawing.Point(8, 360)
		Me.cmdNew.Name = "cmdNew"
		Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNew.Size = New System.Drawing.Size(73, 22)
		Me.cmdNew.TabIndex = 20
		Me.cmdNew.TabStop = True
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
		Me.cmdNewSearch.Location = New System.Drawing.Point(460, 56)
		Me.cmdNewSearch.Name = "cmdNewSearch"
		Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewSearch.Size = New System.Drawing.Size(73, 22)
		Me.cmdNewSearch.TabIndex = 19
		Me.cmdNewSearch.TabStop = True
		Me.cmdNewSearch.Text = "Ne&w Search"
		Me.cmdNewSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdFindNow
		' 
		Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindNow.CausesValidation = True
		Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindNow.Enabled = True
		Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindNow.Location = New System.Drawing.Point(460, 28)
		Me.cmdFindNow.Name = "cmdFindNow"
		Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindNow.Size = New System.Drawing.Size(73, 22)
		Me.cmdFindNow.TabIndex = 18
		Me.cmdFindNow.TabStop = True
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
		Me.cmdHelp.Location = New System.Drawing.Point(464, 360)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 24
		Me.cmdHelp.TabStop = True
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
		Me.cmdCancel.Location = New System.Drawing.Point(384, 360)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 23
		Me.cmdCancel.TabStop = True
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
		Me.cmdOK.Location = New System.Drawing.Point(304, 360)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 22
		Me.cmdOK.TabStop = True
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
		Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(146, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(445, 166)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 25
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblReference)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateFrom)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateTo)
		Me._tabMainTab_TabPage0.Controls.Add(Me.uctType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.uctStatus)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtReference)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateFrom)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateTo)
		Me._tabMainTab_TabPage0.Text = " &1 - Details"
		' 
		' lblReference
		' 
		Me.lblReference.AutoSize = False
		Me.lblReference.BackColor = System.Drawing.SystemColors.Control
		Me.lblReference.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReference.Enabled = True
		Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReference.Location = New System.Drawing.Point(16, 14)
		Me.lblReference.Name = "lblReference"
		Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReference.Size = New System.Drawing.Size(73, 17)
		Me.lblReference.TabIndex = 5
		Me.lblReference.Text = "&Reference:"
		Me.lblReference.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReference.UseMnemonic = True
		Me.lblReference.Visible = True
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
		Me.lblType.Location = New System.Drawing.Point(16, 46)
		Me.lblType.Name = "lblType"
		Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblType.Size = New System.Drawing.Size(73, 17)
		Me.lblType.TabIndex = 6
		Me.lblType.Text = "T&ype:"
		Me.lblType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblType.UseMnemonic = True
		Me.lblType.Visible = True
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(16, 78)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(73, 17)
		Me.lblStatus.TabIndex = 7
		Me.lblStatus.Text = "&Status:"
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' lblDateFrom
		' 
		Me.lblDateFrom.AutoSize = False
		Me.lblDateFrom.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateFrom.Enabled = True
		Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateFrom.Location = New System.Drawing.Point(16, 110)
		Me.lblDateFrom.Name = "lblDateFrom"
		Me.lblDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateFrom.Size = New System.Drawing.Size(73, 17)
		Me.lblDateFrom.TabIndex = 8
		Me.lblDateFrom.Text = "Date &From:"
		Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateFrom.UseMnemonic = True
		Me.lblDateFrom.Visible = True
		' 
		' lblDateTo
		' 
		Me.lblDateTo.AutoSize = False
		Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblDateTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDateTo.Enabled = True
		Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDateTo.Location = New System.Drawing.Point(248, 110)
		Me.lblDateTo.Name = "lblDateTo"
		Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDateTo.Size = New System.Drawing.Size(65, 17)
		Me.lblDateTo.TabIndex = 9
		Me.lblDateTo.Text = "Date &To:"
		Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDateTo.UseMnemonic = True
		Me.lblDateTo.Visible = True
		' 
		' uctType
		' 
		Me.uctType.Location = New System.Drawing.Point(120, 44)
		Me.uctType.Name = "uctType"
		Me.uctType.Size = New System.Drawing.Size(153, 21)
		Me.uctType.Sorted = True
		Me.uctType.TabIndex = 1
		Me.uctType.Table = UserControls.TypeTable.actTable.actCashListType
		Me.uctType.WhatsThisHelpID = 51002
		' 
		' uctStatus
		' 
		Me.uctStatus.Location = New System.Drawing.Point(120, 76)
		Me.uctStatus.Name = "uctStatus"
		Me.uctStatus.Size = New System.Drawing.Size(153, 21)
		Me.uctStatus.Sorted = True
		Me.uctStatus.TabIndex = 2
		Me.uctStatus.Table = UserControls.TypeTable.actTable.actCashListStatus
		Me.uctStatus.WhatsThisHelpID = 51003
		' 
		' txtReference
		' 
		Me.txtReference.AcceptsReturn = True
		Me.txtReference.AutoSize = False
		Me.txtReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReference.CausesValidation = True
		Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReference.Enabled = True
		Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReference.HideSelection = True
		Me.txtReference.Location = New System.Drawing.Point(120, 12)
		Me.txtReference.MaxLength = 0
		Me.txtReference.Multiline = False
		Me.txtReference.Name = "txtReference"
		Me.txtReference.ReadOnly = False
		Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReference.Size = New System.Drawing.Size(153, 19)
		Me.txtReference.TabIndex = 0
		Me.txtReference.TabStop = True
		Me.txtReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReference.Visible = True
		' 
		' txtDateFrom
		' 
		Me.txtDateFrom.AcceptsReturn = True
		Me.txtDateFrom.AutoSize = False
		Me.txtDateFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateFrom.CausesValidation = True
		Me.txtDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateFrom.Enabled = True
		Me.txtDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateFrom.HideSelection = True
		Me.txtDateFrom.Location = New System.Drawing.Point(120, 108)
		Me.txtDateFrom.MaxLength = 0
		Me.txtDateFrom.Multiline = False
		Me.txtDateFrom.Name = "txtDateFrom"
		Me.txtDateFrom.ReadOnly = False
		Me.txtDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateFrom.Size = New System.Drawing.Size(105, 19)
		Me.txtDateFrom.TabIndex = 3
		Me.txtDateFrom.TabStop = True
		Me.txtDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateFrom.Visible = True
		' 
		' txtDateTo
		' 
		Me.txtDateTo.AcceptsReturn = True
		Me.txtDateTo.AutoSize = False
		Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtDateTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDateTo.CausesValidation = True
		Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDateTo.Enabled = True
		Me.txtDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDateTo.HideSelection = True
		Me.txtDateTo.Location = New System.Drawing.Point(320, 108)
		Me.txtDateTo.MaxLength = 0
		Me.txtDateTo.Multiline = False
		Me.txtDateTo.Name = "txtDateTo"
		Me.txtDateTo.ReadOnly = False
		Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDateTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDateTo.Size = New System.Drawing.Size(105, 19)
		Me.txtDateTo.TabIndex = 4
		Me.txtDateTo.TabStop = True
		Me.txtDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDateTo.Visible = True
		' 
		' _tabMainTab_TabPage1
		' 
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblBank)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblTotalAmount)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblCurrency)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblTotalItems)
		Me._tabMainTab_TabPage1.Controls.Add(Me.uctBankAccount)
		Me._tabMainTab_TabPage1.Controls.Add(Me.uctCurrency)
		Me._tabMainTab_TabPage1.Controls.Add(Me.txtTotalAmount)
		Me._tabMainTab_TabPage1.Controls.Add(Me.txtTotalItems)
		Me._tabMainTab_TabPage1.Text = "&2 - Bank && Totals"
		' 
		' lblBank
		' 
		Me.lblBank.AutoSize = False
		Me.lblBank.BackColor = System.Drawing.SystemColors.Control
		Me.lblBank.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBank.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBank.Enabled = True
		Me.lblBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBank.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBank.Location = New System.Drawing.Point(16, 15)
		Me.lblBank.Name = "lblBank"
		Me.lblBank.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBank.Size = New System.Drawing.Size(73, 17)
		Me.lblBank.TabIndex = 13
		Me.lblBank.Text = "&Bank Account:"
		Me.lblBank.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBank.UseMnemonic = True
		Me.lblBank.Visible = True
		' 
		' lblTotalAmount
		' 
		Me.lblTotalAmount.AutoSize = False
		Me.lblTotalAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalAmount.Enabled = True
		Me.lblTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalAmount.Location = New System.Drawing.Point(16, 79)
		Me.lblTotalAmount.Name = "lblTotalAmount"
		Me.lblTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalAmount.Size = New System.Drawing.Size(73, 17)
		Me.lblTotalAmount.TabIndex = 15
		Me.lblTotalAmount.Text = "&Amounts Total"
		Me.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalAmount.UseMnemonic = True
		Me.lblTotalAmount.Visible = True
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(16, 47)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(73, 17)
		Me.lblCurrency.TabIndex = 14
		Me.lblCurrency.Text = "&Currency"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblTotalItems
		' 
		Me.lblTotalItems.AutoSize = False
		Me.lblTotalItems.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalItems.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalItems.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalItems.Enabled = True
		Me.lblTotalItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalItems.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalItems.Location = New System.Drawing.Point(16, 111)
		Me.lblTotalItems.Name = "lblTotalItems"
		Me.lblTotalItems.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalItems.Size = New System.Drawing.Size(73, 17)
		Me.lblTotalItems.TabIndex = 16
		Me.lblTotalItems.Text = "&Items Total"
		Me.lblTotalItems.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalItems.UseMnemonic = True
		Me.lblTotalItems.Visible = True
		' 
		' uctBankAccount
		' 
		Me.uctBankAccount.Location = New System.Drawing.Point(120, 12)
		Me.uctBankAccount.Name = "uctBankAccount"
		Me.uctBankAccount.Size = New System.Drawing.Size(153, 21)
		Me.uctBankAccount.TabIndex = 28
		Me.uctBankAccount.WhatsThisHelpID = 51006
		' 
		' uctCurrency
		' 
		Me.uctCurrency.Location = New System.Drawing.Point(120, 44)
		Me.uctCurrency.Name = "uctCurrency"
		Me.uctCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
		Me.uctCurrency.Size = New System.Drawing.Size(153, 21)
		Me.uctCurrency.TabIndex = 10
		Me.uctCurrency.WhatsThisHelpID = 51007
		' 
		' txtTotalAmount
		' 
		Me.txtTotalAmount.AcceptsReturn = True
		Me.txtTotalAmount.AutoSize = False
		Me.txtTotalAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotalAmount.CausesValidation = True
		Me.txtTotalAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotalAmount.Enabled = True
		Me.txtTotalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotalAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotalAmount.HideSelection = True
		Me.txtTotalAmount.Location = New System.Drawing.Point(120, 76)
		Me.txtTotalAmount.MaxLength = 0
		Me.txtTotalAmount.Multiline = False
		Me.txtTotalAmount.Name = "txtTotalAmount"
		Me.txtTotalAmount.ReadOnly = False
		Me.txtTotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalAmount.Size = New System.Drawing.Size(113, 19)
		Me.txtTotalAmount.TabIndex = 11
		Me.txtTotalAmount.TabStop = True
		Me.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTotalAmount.Visible = True
		' 
		' txtTotalItems
		' 
		Me.txtTotalItems.AcceptsReturn = True
		Me.txtTotalItems.AutoSize = False
		Me.txtTotalItems.BackColor = System.Drawing.SystemColors.Window
		Me.txtTotalItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotalItems.CausesValidation = True
		Me.txtTotalItems.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotalItems.Enabled = True
		Me.txtTotalItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotalItems.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotalItems.HideSelection = True
		Me.txtTotalItems.Location = New System.Drawing.Point(120, 108)
		Me.txtTotalItems.MaxLength = 0
		Me.txtTotalItems.Multiline = False
		Me.txtTotalItems.Name = "txtTotalItems"
		Me.txtTotalItems.ReadOnly = False
		Me.txtTotalItems.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotalItems.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotalItems.Size = New System.Drawing.Size(49, 19)
		Me.txtTotalItems.TabIndex = 12
		Me.txtTotalItems.TabStop = True
		Me.txtTotalItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTotalItems.Visible = True
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 391)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(545, 18)
		Me.stbStatus.TabIndex = 26
		Me.stbStatus.Text = ""
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbStatus_Panel1})
		' 
		' _stbStatus_Panel1
		' 
		Me._stbStatus_Panel1.AutoSize = True
		Me._stbStatus_Panel1.AutoSize = False
		Me._stbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbStatus_Panel1.DoubleClickEnabled = True
		Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbStatus_Panel1.Name = ""
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(545, 18)
		Me._stbStatus_Panel1.Tag = ""
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' lvwSearchDetails
		' 
		Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSearchDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSearchDetails.HideSelection = False
		Me.lvwSearchDetails.LabelEdit = False
		Me.lvwSearchDetails.LabelWrap = True
		Me.lvwSearchDetails.LargeImageList = imglImages
		Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 184)
		Me.lvwSearchDetails.Name = "lvwSearchDetails"
		Me.lvwSearchDetails.Size = New System.Drawing.Size(529, 169)
		Me.lvwSearchDetails.SmallImageList = imglImages
		Me.lvwSearchDetails.TabIndex = 17
		Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_1)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_2)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_3)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_4)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_5)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_6)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_7)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_8)
		Me.lvwSearchDetails.Columns.Add(Me._lvwSearchDetails_ColumnHeader_9)
		' 
		' _lvwSearchDetails_ColumnHeader_1
		' 
		Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_1.Text = "Reference"
		Me._lvwSearchDetails_ColumnHeader_1.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_2
		' 
		Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_2.Text = "Type"
		Me._lvwSearchDetails_ColumnHeader_2.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_3
		' 
		Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_3.Text = "Status"
		Me._lvwSearchDetails_ColumnHeader_3.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_4
		' 
		Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_4.Text = "Date"
		Me._lvwSearchDetails_ColumnHeader_4.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_5
		' 
		Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_5.Text = "Bank Account"
		Me._lvwSearchDetails_ColumnHeader_5.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_6
		' 
		Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_6.Text = "Currency"
		Me._lvwSearchDetails_ColumnHeader_6.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_7
		' 
		Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_7.Text = "Amounts Total"
		Me._lvwSearchDetails_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwSearchDetails_ColumnHeader_7.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_8
		' 
		Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_8.Text = "Items"
		Me._lvwSearchDetails_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwSearchDetails_ColumnHeader_8.Width = 97
		' 
		' _lvwSearchDetails_ColumnHeader_9
		' 
		Me._lvwSearchDetails_ColumnHeader_9.Tag = ""
		Me._lvwSearchDetails_ColumnHeader_9.Text = "Date Sort"
		Me._lvwSearchDetails_ColumnHeader_9.Width = 97
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imglImages.Images.SetKeyName(0, "FindImage")
		' 
		' ImgImage
		' 
		Me.ImgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.ImgImage.Enabled = True
		Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
		Me.ImgImage.Location = New System.Drawing.Point(480, 104)
		Me.ImgImage.Name = "ImgImage"
		Me.ImgImage.Size = New System.Drawing.Size(32, 32)
		Me.ImgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.ImgImage.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(545, 409)
		Me.ControlBox = True
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
		Me.Controls.Add(Me.ImgImage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(190, 279)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Find: Cash List"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 2)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me._tabMainTab_TabPage1.ResumeLayout(False)
		Me.stbStatus.ResumeLayout(False)
		Me.lvwSearchDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
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
	End Sub
#End Region 
End Class