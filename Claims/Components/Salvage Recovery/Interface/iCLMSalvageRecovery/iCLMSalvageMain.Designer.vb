<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtclaimNumber()
		InitializetxtPerilType()
		InitializelblPerilType()
		InitializelblClaimNumber()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lblClaimNumber_0 As System.Windows.Forms.Label
	Private WithEvents _lblPerilType_0 As System.Windows.Forms.Label
	Public WithEvents lblExchangeRate As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Private WithEvents _txtclaimNumber_0 As System.Windows.Forms.TextBox
	Private WithEvents _txtPerilType_0 As System.Windows.Forms.TextBox
	Public WithEvents txtExchangeRate As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents lvwRecovery As System.Windows.Forms.ListView
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblCoinsuranceTreatment As System.Windows.Forms.Label
	Private WithEvents _lblPerilType_1 As System.Windows.Forms.Label
	Private WithEvents _lblClaimNumber_1 As System.Windows.Forms.Label
	Public WithEvents lvwCoInsurance As System.Windows.Forms.ListView
	Public WithEvents cboCoinsuranceTreatment As System.Windows.Forms.ComboBox
	Private WithEvents _txtPerilType_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtclaimNumber_1 As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _txtclaimNumber_2 As System.Windows.Forms.TextBox
	Private WithEvents _txtPerilType_2 As System.Windows.Forms.TextBox
	Public WithEvents lvwReinsurance As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents _lblClaimNumber_2 As System.Windows.Forms.Label
	Private WithEvents _lblPerilType_2 As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents stbStatus As System.Windows.Forms.PictureBox
	Public lblClaimNumber(2) As System.Windows.Forms.Label
	Public lblPerilType(2) As System.Windows.Forms.Label
	Public txtPerilType(2) As System.Windows.Forms.TextBox
	Public txtclaimNumber(2) As System.Windows.Forms.TextBox
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
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me._lblClaimNumber_0 = New System.Windows.Forms.Label
		Me._lblPerilType_0 = New System.Windows.Forms.Label
		Me.lblExchangeRate = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me._txtclaimNumber_0 = New System.Windows.Forms.TextBox
		Me._txtPerilType_0 = New System.Windows.Forms.TextBox
		Me.txtExchangeRate = New System.Windows.Forms.TextBox
		Me.cboCurrency = New System.Windows.Forms.ComboBox
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.lvwRecovery = New System.Windows.Forms.ListView
		Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
		Me.lblCoinsuranceTreatment = New System.Windows.Forms.Label
		Me._lblPerilType_1 = New System.Windows.Forms.Label
		Me._lblClaimNumber_1 = New System.Windows.Forms.Label
		Me.lvwCoInsurance = New System.Windows.Forms.ListView
		Me.cboCoinsuranceTreatment = New System.Windows.Forms.ComboBox
		Me._txtPerilType_1 = New System.Windows.Forms.TextBox
		Me._txtclaimNumber_1 = New System.Windows.Forms.TextBox
		Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
		Me._txtclaimNumber_2 = New System.Windows.Forms.TextBox
		Me._txtPerilType_2 = New System.Windows.Forms.TextBox
		Me.lvwReinsurance = New System.Windows.Forms.ListView
		Me.imglImages = New System.Windows.Forms.ImageList
		Me._lblClaimNumber_2 = New System.Windows.Forms.Label
		Me._lblPerilType_2 = New System.Windows.Forms.Label
		Me.stbStatus = New System.Windows.Forms.PictureBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me._tabMainTab_TabPage1.SuspendLayout()
		Me._tabMainTab_TabPage2.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileExit})
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.ShortcutKeys = CType(System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q, System.Windows.Forms.Keys)
		Me.mnuFileExit.Text = "E&xit"
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(536, 376)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 14
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
		Me.cmdCancel.Location = New System.Drawing.Point(456, 376)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 12
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
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(376, 376)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 11
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
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(199, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 32)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(605, 341)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 13
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me._lblClaimNumber_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._lblPerilType_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblExchangeRate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me._txtclaimNumber_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me._txtPerilType_0)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtExchangeRate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRecovery)
		Me._tabMainTab_TabPage0.Text = "&1 - Recovery Amounts"
		' 
		' _lblClaimNumber_0
		' 
		Me._lblClaimNumber_0.AutoSize = True
		Me._lblClaimNumber_0.BackColor = System.Drawing.SystemColors.Control
		Me._lblClaimNumber_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblClaimNumber_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblClaimNumber_0.Enabled = True
		Me._lblClaimNumber_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblClaimNumber_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblClaimNumber_0.Location = New System.Drawing.Point(16, 19)
		Me._lblClaimNumber_0.Name = "_lblClaimNumber_0"
		Me._lblClaimNumber_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblClaimNumber_0.Size = New System.Drawing.Size(86, 17)
		Me._lblClaimNumber_0.TabIndex = 0
		Me._lblClaimNumber_0.Text = "Claim number:"
		Me._lblClaimNumber_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblClaimNumber_0.UseMnemonic = True
		Me._lblClaimNumber_0.Visible = True
		' 
		' _lblPerilType_0
		' 
		Me._lblPerilType_0.AutoSize = True
		Me._lblPerilType_0.BackColor = System.Drawing.SystemColors.Control
		Me._lblPerilType_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblPerilType_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblPerilType_0.Enabled = True
		Me._lblPerilType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblPerilType_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblPerilType_0.Location = New System.Drawing.Point(288, 19)
		Me._lblPerilType_0.Name = "_lblPerilType_0"
		Me._lblPerilType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblPerilType_0.Size = New System.Drawing.Size(66, 17)
		Me._lblPerilType_0.TabIndex = 2
		Me._lblPerilType_0.Text = "Peril Type :"
		Me._lblPerilType_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblPerilType_0.UseMnemonic = True
		Me._lblPerilType_0.Visible = True
		' 
		' lblExchangeRate
		' 
		Me.lblExchangeRate.AutoSize = True
		Me.lblExchangeRate.BackColor = System.Drawing.SystemColors.Control
		Me.lblExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExchangeRate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExchangeRate.Enabled = True
		Me.lblExchangeRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExchangeRate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExchangeRate.Location = New System.Drawing.Point(288, 43)
		Me.lblExchangeRate.Name = "lblExchangeRate"
		Me.lblExchangeRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExchangeRate.Size = New System.Drawing.Size(100, 17)
		Me.lblExchangeRate.TabIndex = 6
		Me.lblExchangeRate.Text = "&Exchange rate:"
		Me.lblExchangeRate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblExchangeRate.UseMnemonic = True
		Me.lblExchangeRate.Visible = False
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = True
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(16, 43)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(87, 13)
		Me.lblCurrency.TabIndex = 4
		Me.lblCurrency.Text = "Loss C&urrency:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' _txtclaimNumber_0
		' 
		Me._txtclaimNumber_0.AcceptsReturn = True
		Me._txtclaimNumber_0.AutoSize = False
		Me._txtclaimNumber_0.BackColor = System.Drawing.SystemColors.Control
		Me._txtclaimNumber_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtclaimNumber_0.CausesValidation = True
		Me._txtclaimNumber_0.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtclaimNumber_0.Enabled = False
		Me._txtclaimNumber_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtclaimNumber_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtclaimNumber_0.HideSelection = True
		Me._txtclaimNumber_0.Location = New System.Drawing.Point(127, 16)
		Me._txtclaimNumber_0.MaxLength = 0
		Me._txtclaimNumber_0.Multiline = False
		Me._txtclaimNumber_0.Name = "_txtclaimNumber_0"
		Me._txtclaimNumber_0.ReadOnly = True
		Me._txtclaimNumber_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtclaimNumber_0.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtclaimNumber_0.Size = New System.Drawing.Size(153, 19)
		Me._txtclaimNumber_0.TabIndex = 1
		Me._txtclaimNumber_0.TabStop = True
		Me._txtclaimNumber_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtclaimNumber_0.Visible = True
		' 
		' _txtPerilType_0
		' 
		Me._txtPerilType_0.AcceptsReturn = True
		Me._txtPerilType_0.AutoSize = False
		Me._txtPerilType_0.BackColor = System.Drawing.SystemColors.Control
		Me._txtPerilType_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPerilType_0.CausesValidation = True
		Me._txtPerilType_0.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPerilType_0.Enabled = False
		Me._txtPerilType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtPerilType_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPerilType_0.HideSelection = True
		Me._txtPerilType_0.Location = New System.Drawing.Point(408, 16)
		Me._txtPerilType_0.MaxLength = 0
		Me._txtPerilType_0.Multiline = False
		Me._txtPerilType_0.Name = "_txtPerilType_0"
		Me._txtPerilType_0.ReadOnly = False
		Me._txtPerilType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPerilType_0.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPerilType_0.Size = New System.Drawing.Size(153, 19)
		Me._txtPerilType_0.TabIndex = 3
		Me._txtPerilType_0.TabStop = True
		Me._txtPerilType_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPerilType_0.Visible = True
		' 
		' txtExchangeRate
		' 
		Me.txtExchangeRate.AcceptsReturn = True
		Me.txtExchangeRate.AutoSize = False
		Me.txtExchangeRate.BackColor = System.Drawing.SystemColors.Window
		Me.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExchangeRate.CausesValidation = True
		Me.txtExchangeRate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExchangeRate.Enabled = True
		Me.txtExchangeRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExchangeRate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExchangeRate.HideSelection = True
		Me.txtExchangeRate.Location = New System.Drawing.Point(408, 40)
		Me.txtExchangeRate.MaxLength = 0
		Me.txtExchangeRate.Multiline = False
		Me.txtExchangeRate.Name = "txtExchangeRate"
		Me.txtExchangeRate.ReadOnly = False
		Me.txtExchangeRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExchangeRate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExchangeRate.Size = New System.Drawing.Size(153, 19)
		Me.txtExchangeRate.TabIndex = 7
		Me.txtExchangeRate.TabStop = True
		Me.txtExchangeRate.Text = "0.00"
		Me.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtExchangeRate.Visible = False
		' 
		' cboCurrency
		' 
		Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
		Me.cboCurrency.CausesValidation = True
		Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCurrency.Enabled = True
		Me.cboCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCurrency.IntegralHeight = True
		Me.cboCurrency.Location = New System.Drawing.Point(127, 40)
		Me.cboCurrency.Name = "cboCurrency"
		Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
		Me.cboCurrency.Sorted = False
		Me.cboCurrency.TabIndex = 5
		Me.cboCurrency.TabStop = True
		Me.cboCurrency.Visible = True
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(512, 108)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 8
		Me.cmdAdd.TabStop = False
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(512, 148)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 9
		Me.cmdEdit.TabStop = False
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(512, 188)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 10
		Me.cmdDelete.TabStop = False
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwRecovery
		' 
		Me.lvwRecovery.BackColor = System.Drawing.SystemColors.Window
		Me.lvwRecovery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwRecovery.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwRecovery.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwRecovery.HideSelection = True
		Me.lvwRecovery.LabelEdit = False
		Me.lvwRecovery.LabelWrap = True
		Me.lvwRecovery.LargeImageList = imglImages
		Me.lvwRecovery.Location = New System.Drawing.Point(16, 68)
		Me.lvwRecovery.Name = "lvwRecovery"
		Me.lvwRecovery.Size = New System.Drawing.Size(481, 233)
		Me.lvwRecovery.SmallImageList = imglImages
		Me.lvwRecovery.TabIndex = 28
		Me.lvwRecovery.View = System.Windows.Forms.View.Details
		' 
		' _tabMainTab_TabPage1
		' 
		Me._tabMainTab_TabPage1.Controls.Add(Me.lblCoinsuranceTreatment)
		Me._tabMainTab_TabPage1.Controls.Add(Me._lblPerilType_1)
		Me._tabMainTab_TabPage1.Controls.Add(Me._lblClaimNumber_1)
		Me._tabMainTab_TabPage1.Controls.Add(Me.lvwCoInsurance)
		Me._tabMainTab_TabPage1.Controls.Add(Me.cboCoinsuranceTreatment)
		Me._tabMainTab_TabPage1.Controls.Add(Me._txtPerilType_1)
		Me._tabMainTab_TabPage1.Controls.Add(Me._txtclaimNumber_1)
		Me._tabMainTab_TabPage1.Text = "&2 - Coinsurance Recovery"
		' 
		' lblCoinsuranceTreatment
		' 
		Me.lblCoinsuranceTreatment.AutoSize = True
		Me.lblCoinsuranceTreatment.BackColor = System.Drawing.SystemColors.Control
		Me.lblCoinsuranceTreatment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCoinsuranceTreatment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCoinsuranceTreatment.Enabled = True
		Me.lblCoinsuranceTreatment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCoinsuranceTreatment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCoinsuranceTreatment.Location = New System.Drawing.Point(16, 67)
		Me.lblCoinsuranceTreatment.Name = "lblCoinsuranceTreatment"
		Me.lblCoinsuranceTreatment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCoinsuranceTreatment.Size = New System.Drawing.Size(139, 17)
		Me.lblCoinsuranceTreatment.TabIndex = 20
		Me.lblCoinsuranceTreatment.Text = "Coinsurance Treatment:"
		Me.lblCoinsuranceTreatment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCoinsuranceTreatment.UseMnemonic = True
		Me.lblCoinsuranceTreatment.Visible = True
		' 
		' _lblPerilType_1
		' 
		Me._lblPerilType_1.AutoSize = True
		Me._lblPerilType_1.BackColor = System.Drawing.SystemColors.Control
		Me._lblPerilType_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblPerilType_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblPerilType_1.Enabled = True
		Me._lblPerilType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblPerilType_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblPerilType_1.Location = New System.Drawing.Point(16, 43)
		Me._lblPerilType_1.Name = "_lblPerilType_1"
		Me._lblPerilType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblPerilType_1.Size = New System.Drawing.Size(62, 17)
		Me._lblPerilType_1.TabIndex = 18
		Me._lblPerilType_1.Text = "Peril Type:"
		Me._lblPerilType_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblPerilType_1.UseMnemonic = True
		Me._lblPerilType_1.Visible = True
		' 
		' _lblClaimNumber_1
		' 
		Me._lblClaimNumber_1.AutoSize = True
		Me._lblClaimNumber_1.BackColor = System.Drawing.SystemColors.Control
		Me._lblClaimNumber_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblClaimNumber_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblClaimNumber_1.Enabled = True
		Me._lblClaimNumber_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblClaimNumber_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblClaimNumber_1.Location = New System.Drawing.Point(16, 19)
		Me._lblClaimNumber_1.Name = "_lblClaimNumber_1"
		Me._lblClaimNumber_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblClaimNumber_1.Size = New System.Drawing.Size(86, 17)
		Me._lblClaimNumber_1.TabIndex = 16
		Me._lblClaimNumber_1.Text = "Claim number:"
		Me._lblClaimNumber_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblClaimNumber_1.UseMnemonic = True
		Me._lblClaimNumber_1.Visible = True
		' 
		' lvwCoInsurance
		' 
		Me.lvwCoInsurance.BackColor = System.Drawing.SystemColors.Window
		Me.lvwCoInsurance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwCoInsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwCoInsurance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwCoInsurance.HideSelection = True
		Me.lvwCoInsurance.LabelEdit = False
		Me.lvwCoInsurance.LabelWrap = True
		Me.lvwCoInsurance.LargeImageList = imglImages
		Me.lvwCoInsurance.Location = New System.Drawing.Point(16, 92)
		Me.lvwCoInsurance.Name = "lvwCoInsurance"
		Me.lvwCoInsurance.Size = New System.Drawing.Size(569, 209)
		Me.lvwCoInsurance.SmallImageList = imglImages
		Me.lvwCoInsurance.TabIndex = 26
		Me.lvwCoInsurance.View = System.Windows.Forms.View.Details
		' 
		' cboCoinsuranceTreatment
		' 
		Me.cboCoinsuranceTreatment.BackColor = System.Drawing.SystemColors.Menu
		Me.cboCoinsuranceTreatment.CausesValidation = True
		Me.cboCoinsuranceTreatment.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCoinsuranceTreatment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCoinsuranceTreatment.Enabled = False
		Me.cboCoinsuranceTreatment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCoinsuranceTreatment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCoinsuranceTreatment.IntegralHeight = True
		Me.cboCoinsuranceTreatment.Location = New System.Drawing.Point(160, 64)
		Me.cboCoinsuranceTreatment.Name = "cboCoinsuranceTreatment"
		Me.cboCoinsuranceTreatment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCoinsuranceTreatment.Size = New System.Drawing.Size(153, 21)
		Me.cboCoinsuranceTreatment.Sorted = False
		Me.cboCoinsuranceTreatment.TabIndex = 21
		Me.cboCoinsuranceTreatment.TabStop = True
		Me.cboCoinsuranceTreatment.Visible = True
		' 
		' _txtPerilType_1
		' 
		Me._txtPerilType_1.AcceptsReturn = True
		Me._txtPerilType_1.AutoSize = False
		Me._txtPerilType_1.BackColor = System.Drawing.SystemColors.Control
		Me._txtPerilType_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPerilType_1.CausesValidation = True
		Me._txtPerilType_1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPerilType_1.Enabled = False
		Me._txtPerilType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtPerilType_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPerilType_1.HideSelection = True
		Me._txtPerilType_1.Location = New System.Drawing.Point(160, 40)
		Me._txtPerilType_1.MaxLength = 0
		Me._txtPerilType_1.Multiline = False
		Me._txtPerilType_1.Name = "_txtPerilType_1"
		Me._txtPerilType_1.ReadOnly = False
		Me._txtPerilType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPerilType_1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPerilType_1.Size = New System.Drawing.Size(153, 19)
		Me._txtPerilType_1.TabIndex = 19
		Me._txtPerilType_1.TabStop = True
		Me._txtPerilType_1.Text = "Fire"
		Me._txtPerilType_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPerilType_1.Visible = True
		' 
		' _txtclaimNumber_1
		' 
		Me._txtclaimNumber_1.AcceptsReturn = True
		Me._txtclaimNumber_1.AutoSize = False
		Me._txtclaimNumber_1.BackColor = System.Drawing.SystemColors.Control
		Me._txtclaimNumber_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtclaimNumber_1.CausesValidation = True
		Me._txtclaimNumber_1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtclaimNumber_1.Enabled = False
		Me._txtclaimNumber_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtclaimNumber_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtclaimNumber_1.HideSelection = True
		Me._txtclaimNumber_1.Location = New System.Drawing.Point(160, 16)
		Me._txtclaimNumber_1.MaxLength = 0
		Me._txtclaimNumber_1.Multiline = False
		Me._txtclaimNumber_1.Name = "_txtclaimNumber_1"
		Me._txtclaimNumber_1.ReadOnly = True
		Me._txtclaimNumber_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtclaimNumber_1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtclaimNumber_1.Size = New System.Drawing.Size(153, 19)
		Me._txtclaimNumber_1.TabIndex = 17
		Me._txtclaimNumber_1.TabStop = True
		Me._txtclaimNumber_1.Text = "XYZ789"
		Me._txtclaimNumber_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtclaimNumber_1.Visible = True
		' 
		' _tabMainTab_TabPage2
		' 
		Me._tabMainTab_TabPage2.Controls.Add(Me._txtclaimNumber_2)
		Me._tabMainTab_TabPage2.Controls.Add(Me._txtPerilType_2)
		Me._tabMainTab_TabPage2.Controls.Add(Me.lvwReinsurance)
		Me._tabMainTab_TabPage2.Controls.Add(Me._lblClaimNumber_2)
		Me._tabMainTab_TabPage2.Controls.Add(Me._lblPerilType_2)
		Me._tabMainTab_TabPage2.Text = "&3 - Reinsurance Recovery"
		' 
		' _txtclaimNumber_2
		' 
		Me._txtclaimNumber_2.AcceptsReturn = True
		Me._txtclaimNumber_2.AutoSize = False
		Me._txtclaimNumber_2.BackColor = System.Drawing.SystemColors.Control
		Me._txtclaimNumber_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtclaimNumber_2.CausesValidation = True
		Me._txtclaimNumber_2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtclaimNumber_2.Enabled = False
		Me._txtclaimNumber_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtclaimNumber_2.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtclaimNumber_2.HideSelection = True
		Me._txtclaimNumber_2.Location = New System.Drawing.Point(136, 16)
		Me._txtclaimNumber_2.MaxLength = 0
		Me._txtclaimNumber_2.Multiline = False
		Me._txtclaimNumber_2.Name = "_txtclaimNumber_2"
		Me._txtclaimNumber_2.ReadOnly = True
		Me._txtclaimNumber_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtclaimNumber_2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtclaimNumber_2.Size = New System.Drawing.Size(153, 19)
		Me._txtclaimNumber_2.TabIndex = 23
		Me._txtclaimNumber_2.TabStop = True
		Me._txtclaimNumber_2.Text = "XYZ781"
		Me._txtclaimNumber_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtclaimNumber_2.Visible = True
		' 
		' _txtPerilType_2
		' 
		Me._txtPerilType_2.AcceptsReturn = True
		Me._txtPerilType_2.AutoSize = False
		Me._txtPerilType_2.BackColor = System.Drawing.SystemColors.Control
		Me._txtPerilType_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPerilType_2.CausesValidation = True
		Me._txtPerilType_2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPerilType_2.Enabled = False
		Me._txtPerilType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._txtPerilType_2.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPerilType_2.HideSelection = True
		Me._txtPerilType_2.Location = New System.Drawing.Point(136, 40)
		Me._txtPerilType_2.MaxLength = 0
		Me._txtPerilType_2.Multiline = False
		Me._txtPerilType_2.Name = "_txtPerilType_2"
		Me._txtPerilType_2.ReadOnly = False
		Me._txtPerilType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPerilType_2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPerilType_2.Size = New System.Drawing.Size(153, 19)
		Me._txtPerilType_2.TabIndex = 25
		Me._txtPerilType_2.TabStop = True
		Me._txtPerilType_2.Text = "Fire"
		Me._txtPerilType_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPerilType_2.Visible = True
		' 
		' lvwReinsurance
		' 
		Me.lvwReinsurance.BackColor = System.Drawing.SystemColors.Window
		Me.lvwReinsurance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwReinsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwReinsurance.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwReinsurance.HideSelection = True
		Me.lvwReinsurance.LabelEdit = False
		Me.lvwReinsurance.LabelWrap = True
		Me.lvwReinsurance.LargeImageList = imglImages
		Me.lvwReinsurance.Location = New System.Drawing.Point(16, 68)
		Me.lvwReinsurance.Name = "lvwReinsurance"
		Me.lvwReinsurance.Size = New System.Drawing.Size(569, 233)
		Me.lvwReinsurance.SmallImageList = imglImages
		Me.lvwReinsurance.TabIndex = 27
		Me.lvwReinsurance.View = System.Windows.Forms.View.Details
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        'Me.imglImages.Key_0 = "FindImage"
        Me.imglImages.Images.SetKeyName(0, "FindImage")
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		' 
		' _lblClaimNumber_2
		' 
		Me._lblClaimNumber_2.AutoSize = True
		Me._lblClaimNumber_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblClaimNumber_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblClaimNumber_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblClaimNumber_2.Enabled = True
		Me._lblClaimNumber_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblClaimNumber_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblClaimNumber_2.Location = New System.Drawing.Point(16, 19)
		Me._lblClaimNumber_2.Name = "_lblClaimNumber_2"
		Me._lblClaimNumber_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblClaimNumber_2.Size = New System.Drawing.Size(87, 17)
		Me._lblClaimNumber_2.TabIndex = 22
		Me._lblClaimNumber_2.Text = "Claim Number:"
		Me._lblClaimNumber_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblClaimNumber_2.UseMnemonic = True
		Me._lblClaimNumber_2.Visible = True
		' 
		' _lblPerilType_2
		' 
		Me._lblPerilType_2.AutoSize = True
		Me._lblPerilType_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblPerilType_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblPerilType_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblPerilType_2.Enabled = True
		Me._lblPerilType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._lblPerilType_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblPerilType_2.Location = New System.Drawing.Point(16, 43)
		Me._lblPerilType_2.Name = "_lblPerilType_2"
		Me._lblPerilType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblPerilType_2.Size = New System.Drawing.Size(66, 17)
		Me._lblPerilType_2.TabIndex = 24
		Me._lblPerilType_2.Text = "Peril Type :"
		Me._lblPerilType_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblPerilType_2.UseMnemonic = True
		Me._lblPerilType_2.Visible = True
		' 
		' stbStatus
		' 
		Me.stbStatus.BackColor = System.Drawing.SystemColors.Control
		Me.stbStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.stbStatus.CausesValidation = True
		Me.stbStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Enabled = True
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 403)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.Size = New System.Drawing.Size(615, 18)
		Me.stbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.stbStatus.TabIndex = 15
		Me.stbStatus.TabStop = True
		Me.stbStatus.Visible = True
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(615, 421)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = True
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(333, 149)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Salvage Receipt [Paul Hayes Q199710061516]"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRecovery, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me._tabMainTab_TabPage1.ResumeLayout(False)
		Me._tabMainTab_TabPage2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializetxtclaimNumber()
		Me.txtclaimNumber(2) = _txtclaimNumber_2
		Me.txtclaimNumber(1) = _txtclaimNumber_1
		Me.txtclaimNumber(0) = _txtclaimNumber_0
	End Sub
	Sub InitializetxtPerilType()
		Me.txtPerilType(2) = _txtPerilType_2
		Me.txtPerilType(1) = _txtPerilType_1
		Me.txtPerilType(0) = _txtPerilType_0
	End Sub
	Sub InitializelblPerilType()
		Me.lblPerilType(2) = _lblPerilType_2
		Me.lblPerilType(1) = _lblPerilType_1
		Me.lblPerilType(0) = _lblPerilType_0
	End Sub
	Sub InitializelblClaimNumber()
		Me.lblClaimNumber(2) = _lblClaimNumber_2
		Me.lblClaimNumber(1) = _lblClaimNumber_1
		Me.lblClaimNumber(0) = _lblClaimNumber_0
	End Sub
#End Region 
End Class