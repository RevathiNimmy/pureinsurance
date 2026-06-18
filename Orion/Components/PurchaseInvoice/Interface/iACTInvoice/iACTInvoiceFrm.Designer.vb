<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializecmdNext()
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
	Public WithEvents txtInvVal As System.Windows.Forms.TextBox
	Public WithEvents txtInvNum As System.Windows.Forms.TextBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblDetTotal As System.Windows.Forms.Label
	Public WithEvents lblInvDes As System.Windows.Forms.Label
	Public WithEvents lblRef As System.Windows.Forms.Label
	Public WithEvents lblPoNum As System.Windows.Forms.Label
	Public WithEvents lblSupplierAccount As System.Windows.Forms.Label
	Public WithEvents lblInvVal As System.Windows.Forms.Label
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblInvNum As System.Windows.Forms.Label
	Public WithEvents lblInvDate As System.Windows.Forms.Label
	Public WithEvents lblNominalAccount As System.Windows.Forms.Label
	Public WithEvents lblBranches As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents uctNominalAccount As UserControls.AccountLookup
	Public WithEvents txtPONum As System.Windows.Forms.TextBox
	Public WithEvents txtInvDes As System.Windows.Forms.TextBox
	Public WithEvents txtRef As System.Windows.Forms.TextBox
	Public WithEvents txtInvDate As System.Windows.Forms.TextBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Public WithEvents uctSupplierAccount As UserControls.AccountLookup
	Public WithEvents cboBranches As System.Windows.Forms.ComboBox
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblInvNo2 As System.Windows.Forms.Label
	Public WithEvents lblInvVal2 As System.Windows.Forms.Label
	Public WithEvents imgIcon2 As System.Windows.Forms.PictureBox
	Public WithEvents txtTotal As System.Windows.Forms.TextBox
	Private WithEvents _lvwItems_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwItems_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwItems_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwItems_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwItems_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwItems_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwItems As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdPrevious As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(0) As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtInvVal = New System.Windows.Forms.TextBox
        Me.txtInvNum = New System.Windows.Forms.TextBox
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
        Me.lblInvDes = New System.Windows.Forms.Label
        Me.lblRef = New System.Windows.Forms.Label
        Me.lblPoNum = New System.Windows.Forms.Label
        Me.lblSupplierAccount = New System.Windows.Forms.Label
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblInvDate = New System.Windows.Forms.Label
        Me.lblNominalAccount = New System.Windows.Forms.Label
        Me.lblBranches = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.uctNominalAccount = New UserControls.AccountLookup
        Me.txtPONum = New System.Windows.Forms.TextBox
        Me.txtInvDes = New System.Windows.Forms.TextBox
        Me.txtRef = New System.Windows.Forms.TextBox
        Me.txtInvDate = New System.Windows.Forms.TextBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.uctSupplierAccount = New UserControls.AccountLookup
        Me.cboBranches = New System.Windows.Forms.ComboBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.lblInvNo2 = New System.Windows.Forms.Label
        Me.lblInvVal2 = New System.Windows.Forms.Label
        Me.imgIcon2 = New System.Windows.Forms.PictureBox
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.lvwItems = New System.Windows.Forms.ListView
        Me._lvwItems_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdPrevious = New System.Windows.Forms.Button
        Me.lblInvVal = New System.Windows.Forms.Label
        Me.lblInvNum = New System.Windows.Forms.Label
        Me.lblDetTotal = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        CType(Me.imgIcon2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInvVal
        '
        Me.txtInvVal.AcceptsReturn = True
        Me.txtInvVal.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvVal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInvVal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvVal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvVal.Location = New System.Drawing.Point(184, 80)
        Me.txtInvVal.MaxLength = 0
        Me.txtInvVal.Name = "txtInvVal"
        Me.txtInvVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInvVal.Size = New System.Drawing.Size(153, 21)
        Me.txtInvVal.TabIndex = 1
        '
        'txtInvNum
        '
        Me.txtInvNum.AcceptsReturn = True
        Me.txtInvNum.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInvNum.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvNum.Location = New System.Drawing.Point(184, 48)
        Me.txtInvNum.MaxLength = 0
        Me.txtInvNum.Name = "txtInvNum"
        Me.txtInvNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInvNum.Size = New System.Drawing.Size(153, 21)
        Me.txtInvNum.TabIndex = 0
        Me.txtInvNum.Tag = "Reference"
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(440, 392)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 11
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
        Me.cmdCancel.Location = New System.Drawing.Point(360, 392)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(280, 392)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 9
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(112, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(509, 381)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 8
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInvDes)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPoNum)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSupplierAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInvDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNominalAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranches)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctNominalAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPONum)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtInvDes)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtInvDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctSupplierAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBranches)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(501, 355)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Invoice"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblInvDes
        '
        Me.lblInvDes.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvDes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvDes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvDes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvDes.Location = New System.Drawing.Point(24, 156)
        Me.lblInvDes.Name = "lblInvDes"
        Me.lblInvDes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvDes.Size = New System.Drawing.Size(137, 17)
        Me.lblInvDes.TabIndex = 12
        Me.lblInvDes.Text = "Invoice Description:"
        '
        'lblRef
        '
        Me.lblRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRef.Location = New System.Drawing.Point(24, 298)
        Me.lblRef.Name = "lblRef"
        Me.lblRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRef.Size = New System.Drawing.Size(121, 17)
        Me.lblRef.TabIndex = 13
        Me.lblRef.Text = "Reference:"
        '
        'lblPoNum
        '
        Me.lblPoNum.BackColor = System.Drawing.SystemColors.Control
        Me.lblPoNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPoNum.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPoNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPoNum.Location = New System.Drawing.Point(24, 118)
        Me.lblPoNum.Name = "lblPoNum"
        Me.lblPoNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPoNum.Size = New System.Drawing.Size(129, 17)
        Me.lblPoNum.TabIndex = 14
        Me.lblPoNum.Text = "Purchase Order No:"
        '
        'lblSupplierAccount
        '
        Me.lblSupplierAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblSupplierAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSupplierAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSupplierAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSupplierAccount.Location = New System.Drawing.Point(24, 238)
        Me.lblSupplierAccount.Name = "lblSupplierAccount"
        Me.lblSupplierAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSupplierAccount.Size = New System.Drawing.Size(121, 17)
        Me.lblSupplierAccount.TabIndex = 15
        Me.lblSupplierAccount.Text = "Supplier Account:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(456, 12)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(36, 32)
        Me.imgIcon.TabIndex = 18
        Me.imgIcon.TabStop = False
        '
        'lblInvDate
        '
        Me.lblInvDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvDate.Location = New System.Drawing.Point(24, 86)
        Me.lblInvDate.Name = "lblInvDate"
        Me.lblInvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvDate.Size = New System.Drawing.Size(129, 17)
        Me.lblInvDate.TabIndex = 22
        Me.lblInvDate.Text = "Invoice Date:"
        '
        'lblNominalAccount
        '
        Me.lblNominalAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblNominalAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNominalAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNominalAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNominalAccount.Location = New System.Drawing.Point(24, 270)
        Me.lblNominalAccount.Name = "lblNominalAccount"
        Me.lblNominalAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNominalAccount.Size = New System.Drawing.Size(121, 17)
        Me.lblNominalAccount.TabIndex = 28
        Me.lblNominalAccount.Text = "Nominal Account:"
        '
        'lblBranches
        '
        Me.lblBranches.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranches.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranches.Location = New System.Drawing.Point(24, 328)
        Me.lblBranches.Name = "lblBranches"
        Me.lblBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranches.Size = New System.Drawing.Size(73, 17)
        Me.lblBranches.TabIndex = 29
        Me.lblBranches.Text = "Branch:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(24, 208)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(65, 13)
        Me.lblCurrency.TabIndex = 31
        Me.lblCurrency.Text = "Currency:"
        '
        'uctNominalAccount
        '
        Me.uctNominalAccount.AccountId = 0
        Me.uctNominalAccount.AllowStoppedAccounts = False
        Me.uctNominalAccount.BackStyle = 0
        Me.uctNominalAccount.CompanyId = 0
        Me.uctNominalAccount.Default_Renamed = False
        Me.uctNominalAccount.Location = New System.Drawing.Point(176, 268)
        Me.uctNominalAccount.LookupCaption = "..."
        Me.uctNominalAccount.LookupHeight = 285
        Me.uctNominalAccount.LookupLeft = 2175
        Me.uctNominalAccount.LookupTextLeft = 0
        Me.uctNominalAccount.LookupTextWidth = 2175
        Me.uctNominalAccount.LookupWidth = 360
        Me.uctNominalAccount.Name = "uctNominalAccount"
        Me.uctNominalAccount.OnlyUpdatableAccounts = False
        Me.uctNominalAccount.SelLength = 0
        Me.uctNominalAccount.SelStart = 0
        Me.uctNominalAccount.SelText = ""
        Me.uctNominalAccount.ShowEditOnFindAccount = False
        Me.uctNominalAccount.Size = New System.Drawing.Size(169, 19)
        Me.uctNominalAccount.TabIndex = 6
        Me.uctNominalAccount.ToolTipText = ""
        '
        'txtPONum
        '
        Me.txtPONum.AcceptsReturn = True
        Me.txtPONum.BackColor = System.Drawing.SystemColors.Window
        Me.txtPONum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPONum.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPONum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPONum.Location = New System.Drawing.Point(176, 116)
        Me.txtPONum.MaxLength = 0
        Me.txtPONum.Name = "txtPONum"
        Me.txtPONum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPONum.Size = New System.Drawing.Size(153, 21)
        Me.txtPONum.TabIndex = 3
        '
        'txtInvDes
        '
        Me.txtInvDes.AcceptsReturn = True
        Me.txtInvDes.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvDes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInvDes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvDes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvDes.Location = New System.Drawing.Point(176, 140)
        Me.txtInvDes.MaxLength = 0
        Me.txtInvDes.Multiline = True
        Me.txtInvDes.Name = "txtInvDes"
        Me.txtInvDes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInvDes.Size = New System.Drawing.Size(289, 51)
        Me.txtInvDes.TabIndex = 4
        '
        'txtRef
        '
        Me.txtRef.AcceptsReturn = True
        Me.txtRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRef.Location = New System.Drawing.Point(176, 296)
        Me.txtRef.MaxLength = 0
        Me.txtRef.Name = "txtRef"
        Me.txtRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRef.Size = New System.Drawing.Size(145, 21)
        Me.txtRef.TabIndex = 7
        '
        'txtInvDate
        '
        Me.txtInvDate.AcceptsReturn = True
        Me.txtInvDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInvDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvDate.Location = New System.Drawing.Point(176, 84)
        Me.txtInvDate.MaxLength = 0
        Me.txtInvDate.Name = "txtInvDate"
        Me.txtInvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInvDate.Size = New System.Drawing.Size(153, 21)
        Me.txtInvDate.TabIndex = 2
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(462, 324)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(32, 19)
        Me._cmdNext_0.TabIndex = 23
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'uctSupplierAccount
        '
        Me.uctSupplierAccount.AccountId = 0
        Me.uctSupplierAccount.AllowStoppedAccounts = False
        Me.uctSupplierAccount.BackStyle = 0
        Me.uctSupplierAccount.CompanyId = 0
        Me.uctSupplierAccount.Default_Renamed = False
        Me.uctSupplierAccount.Location = New System.Drawing.Point(176, 236)
        Me.uctSupplierAccount.LookupCaption = "..."
        Me.uctSupplierAccount.LookupHeight = 285
        Me.uctSupplierAccount.LookupLeft = 2175
        Me.uctSupplierAccount.LookupTextLeft = 0
        Me.uctSupplierAccount.LookupTextWidth = 2175
        Me.uctSupplierAccount.LookupWidth = 360
        Me.uctSupplierAccount.Name = "uctSupplierAccount"
        Me.uctSupplierAccount.OnlyUpdatableAccounts = False
        Me.uctSupplierAccount.SelLength = 0
        Me.uctSupplierAccount.SelStart = 0
        Me.uctSupplierAccount.SelText = ""
        Me.uctSupplierAccount.ShowEditOnFindAccount = False
        Me.uctSupplierAccount.Size = New System.Drawing.Size(169, 19)
        Me.uctSupplierAccount.TabIndex = 5
        Me.uctSupplierAccount.ToolTipText = ""
        '
        'cboBranches
        '
        Me.cboBranches.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranches.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranches.Location = New System.Drawing.Point(176, 324)
        Me.cboBranches.Name = "cboBranches"
        Me.cboBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranches.Size = New System.Drawing.Size(161, 21)
        Me.cboBranches.TabIndex = 30
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(176, 204)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 32
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblInvNo2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblInvVal2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.imgIcon2)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtTotal)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lvwItems)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cmdPrevious)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(501, 355)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Invoice Details"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'lblInvNo2
        '
        Me.lblInvNo2.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvNo2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvNo2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvNo2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvNo2.Location = New System.Drawing.Point(24, 22)
        Me.lblInvNo2.Name = "lblInvNo2"
        Me.lblInvNo2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvNo2.Size = New System.Drawing.Size(145, 17)
        Me.lblInvNo2.TabIndex = 20
        Me.lblInvNo2.Text = "Invoice No:"
        '
        'lblInvVal2
        '
        Me.lblInvVal2.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvVal2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvVal2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvVal2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvVal2.Location = New System.Drawing.Point(24, 54)
        Me.lblInvVal2.Name = "lblInvVal2"
        Me.lblInvVal2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvVal2.Size = New System.Drawing.Size(145, 17)
        Me.lblInvVal2.TabIndex = 21
        Me.lblInvVal2.Text = "Invoice Value:"
        '
        'imgIcon2
        '
        Me.imgIcon2.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon2.Image = CType(resources.GetObject("imgIcon2.Image"), System.Drawing.Image)
        Me.imgIcon2.Location = New System.Drawing.Point(456, 12)
        Me.imgIcon2.Name = "imgIcon2"
        Me.imgIcon2.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon2.TabIndex = 22
        Me.imgIcon2.TabStop = False
        '
        'txtTotal
        '
        Me.txtTotal.AcceptsReturn = True
        Me.txtTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotal.Location = New System.Drawing.Point(390, 284)
        Me.txtTotal.MaxLength = 0
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.ReadOnly = True
        Me.txtTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotal.Size = New System.Drawing.Size(99, 21)
        Me.txtTotal.TabIndex = 19
        Me.txtTotal.TabStop = False
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwItems
        '
        Me.lvwItems.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwItems, "")
        Me.lvwItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwItems_ColumnHeader_1, Me._lvwItems_ColumnHeader_2, Me._lvwItems_ColumnHeader_3, Me._lvwItems_ColumnHeader_4, Me._lvwItems_ColumnHeader_5, Me._lvwItems_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwItems, False)
        Me.lvwItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwItems.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwItems.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwItems, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwItems, "")
        Me.lvwItems.Location = New System.Drawing.Point(16, 92)
        Me.lvwItems.Name = "lvwItems"
        Me.lvwItems.Size = New System.Drawing.Size(473, 185)
        Me.listViewHelper1.SetSmallIcons(Me.lvwItems, "")
        Me.listViewHelper1.SetSorted(Me.lvwItems, False)
        Me.listViewHelper1.SetSortKey(Me.lvwItems, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwItems, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwItems.TabIndex = 24
        Me.lvwItems.UseCompatibleStateImageBehavior = False
        Me.lvwItems.View = System.Windows.Forms.View.Details
        '
        '_lvwItems_ColumnHeader_1
        '
        Me._lvwItems_ColumnHeader_1.Text = "Item Description"
        Me._lvwItems_ColumnHeader_1.Width = 97
        '
        '_lvwItems_ColumnHeader_2
        '
        Me._lvwItems_ColumnHeader_2.Text = "Nominal Account"
        Me._lvwItems_ColumnHeader_2.Width = 97
        '
        '_lvwItems_ColumnHeader_3
        '
        Me._lvwItems_ColumnHeader_3.Text = "Amount"
        Me._lvwItems_ColumnHeader_3.Width = 97
        '
        '_lvwItems_ColumnHeader_4
        '
        Me._lvwItems_ColumnHeader_4.Text = "Amount (Inc Tax)"
        Me._lvwItems_ColumnHeader_4.Width = 97
        '
        '_lvwItems_ColumnHeader_5
        '
        Me._lvwItems_ColumnHeader_5.Text = "Cost Centre"
        Me._lvwItems_ColumnHeader_5.Width = 97
        '
        '_lvwItems_ColumnHeader_6
        '
        Me._lvwItems_ColumnHeader_6.Text = "Dept Amount"
        Me._lvwItems_ColumnHeader_6.Width = 97
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(256, 316)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 25
        Me.cmdAdd.TabStop = False
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(336, 316)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 26
        Me.cmdEdit.TabStop = False
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(416, 316)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 27
        Me.cmdDelete.TabStop = False
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdPrevious
        '
        Me.cmdPrevious.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrevious.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrevious.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrevious.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrevious.Location = New System.Drawing.Point(20, 324)
        Me.cmdPrevious.Name = "cmdPrevious"
        Me.cmdPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrevious.Size = New System.Drawing.Size(32, 19)
        Me.cmdPrevious.TabIndex = 33
        Me.cmdPrevious.TabStop = False
        Me.cmdPrevious.Text = "<<"
        Me.cmdPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrevious.UseVisualStyleBackColor = False
        '
        'lblInvVal
        '
        Me.lblInvVal.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvVal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvVal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvVal.Location = New System.Drawing.Point(38, 84)
        Me.lblInvVal.Name = "lblInvVal"
        Me.lblInvVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvVal.Size = New System.Drawing.Size(145, 17)
        Me.lblInvVal.TabIndex = 17
        Me.lblInvVal.Text = "Invoice Value:"
        '
        'lblInvNum
        '
        Me.lblInvNum.BackColor = System.Drawing.SystemColors.Control
        Me.lblInvNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvNum.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvNum.Location = New System.Drawing.Point(38, 52)
        Me.lblInvNum.Name = "lblInvNum"
        Me.lblInvNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvNum.Size = New System.Drawing.Size(145, 17)
        Me.lblInvNum.TabIndex = 16
        Me.lblInvNum.Text = "Invoice No:"
        '
        'lblDetTotal
        '
        Me.lblDetTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblDetTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDetTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDetTotal.Location = New System.Drawing.Point(-4688, 312)
        Me.lblDetTotal.Name = "lblDetTotal"
        Me.lblDetTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDetTotal.Size = New System.Drawing.Size(81, 17)
        Me.lblDetTotal.TabIndex = 18
        Me.lblDetTotal.Text = "Detail Total:"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdPrevious
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(521, 420)
        Me.Controls.Add(Me.lblInvVal)
        Me.Controls.Add(Me.txtInvVal)
        Me.Controls.Add(Me.txtInvNum)
        Me.Controls.Add(Me.lblInvNum)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(308, 243)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Purchase Invoice"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        CType(Me.imgIcon2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializecmdNext()
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class