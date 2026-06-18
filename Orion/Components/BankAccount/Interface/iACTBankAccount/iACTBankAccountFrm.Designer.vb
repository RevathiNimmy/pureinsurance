<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblAccountNumber As System.Windows.Forms.Label
	Public WithEvents lblAccountName As System.Windows.Forms.Label
	Public WithEvents lblAccountCode As System.Windows.Forms.Label
	Public WithEvents lblAccountDescription As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblnextChequeNumber As System.Windows.Forms.Label
	Public WithEvents lblReconciledDate As System.Windows.Forms.Label
	Public WithEvents lblDefaultBankAccount As System.Windows.Forms.Label
	Public WithEvents lblReceiveCashIntoThisCurr As System.Windows.Forms.Label
	Public WithEvents lblStartChequeNumber As System.Windows.Forms.Label
	Public WithEvents pnlDefaultBankAccount As System.Windows.Forms.Panel
	Public WithEvents pnlReconciledDate As System.Windows.Forms.Panel
	Public WithEvents txtAccountNumber As System.Windows.Forms.TextBox
	Public WithEvents txtAccountName As System.Windows.Forms.TextBox
	Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
	Public WithEvents txtAccountDescription As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As System.Windows.Forms.ComboBox
	Public WithEvents cmdAccountHolder As System.Windows.Forms.Button
	Public WithEvents pnlAccountHolder As System.Windows.Forms.Panel
	Public WithEvents txtnextChequeNumber As System.Windows.Forms.TextBox
	Public WithEvents cmdFindAccount As System.Windows.Forms.Button
	Public WithEvents chkReceiveCashIntoThisCurr As System.Windows.Forms.CheckBox
	Public WithEvents txtStartChequeNumber As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cmdViewRule As System.Windows.Forms.Button
	Public WithEvents cmdAddRule As System.Windows.Forms.Button
	Public WithEvents cmdDeleteRule As System.Windows.Forms.Button
	Public WithEvents cmdEditRule As System.Windows.Forms.Button
	Private WithEvents _lvwRules_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRules_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRules As System.Windows.Forms.ListView
	Public WithEvents fraAccounts As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _lvwBankAccountDelay_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankAccountDelay_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankAccountDelay_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankAccountDelay_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwBankAccountDelay As System.Windows.Forms.ListView
	Public WithEvents cmdAddDelay As System.Windows.Forms.Button
	Public WithEvents cmdDeleteDelay As System.Windows.Forms.Button
	Public WithEvents cmdEditDelay As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents uctPickListBranches As uctPickList.PickList
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents txtProcessingDays As System.Windows.Forms.TextBox
	Public WithEvents txtFinancialInstitutionCode As System.Windows.Forms.TextBox
	Public WithEvents txtDirectDebitSupplierName As System.Windows.Forms.TextBox
	Public WithEvents txtDirectDebitSupplierID As System.Windows.Forms.TextBox
	Public WithEvents txtRemitter As System.Windows.Forms.TextBox
	Public WithEvents lblFinancialInstitutionCode As System.Windows.Forms.Label
	Public WithEvents lblSupplierName As System.Windows.Forms.Label
	Public WithEvents lblProcessingDays As System.Windows.Forms.Label
	Public WithEvents lblRemitter As System.Windows.Forms.Label
	Public WithEvents lblSupplierID As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblIBAN = New System.Windows.Forms.Label()
        Me.txtIBAN = New System.Windows.Forms.TextBox()
        Me.lblBIC = New System.Windows.Forms.Label()
        Me.txtBIC = New System.Windows.Forms.TextBox()
        Me.lblAccountNumber = New System.Windows.Forms.Label()
        Me.lblAccountName = New System.Windows.Forms.Label()
        Me.lblAccountCode = New System.Windows.Forms.Label()
        Me.lblAccountDescription = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblnextChequeNumber = New System.Windows.Forms.Label()
        Me.lblReconciledDate = New System.Windows.Forms.Label()
        Me.lblDefaultBankAccount = New System.Windows.Forms.Label()
        Me.lblReceiveCashIntoThisCurr = New System.Windows.Forms.Label()
        Me.lblStartChequeNumber = New System.Windows.Forms.Label()
        Me.pnlDefaultBankAccount = New System.Windows.Forms.Panel()
        Me.pnlReconciledDate = New System.Windows.Forms.Panel()
        Me.txtAccountNumber = New System.Windows.Forms.TextBox()
        Me.txtAccountName = New System.Windows.Forms.TextBox()
        Me.txtAccountCode = New System.Windows.Forms.TextBox()
        Me.txtAccountDescription = New System.Windows.Forms.TextBox()
        Me.cboCurrency = New System.Windows.Forms.ComboBox()
        Me.cmdAccountHolder = New System.Windows.Forms.Button()
        Me.pnlAccountHolder = New System.Windows.Forms.Panel()
        Me.lblAccountHolder = New System.Windows.Forms.Label()
        Me.txtnextChequeNumber = New System.Windows.Forms.TextBox()
        Me.cmdFindAccount = New System.Windows.Forms.Button()
        Me.chkReceiveCashIntoThisCurr = New System.Windows.Forms.CheckBox()
        Me.txtStartChequeNumber = New System.Windows.Forms.TextBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraAccounts = New System.Windows.Forms.GroupBox()
        Me.cmdViewRule = New System.Windows.Forms.Button()
        Me.cmdAddRule = New System.Windows.Forms.Button()
        Me.cmdDeleteRule = New System.Windows.Forms.Button()
        Me.cmdEditRule = New System.Windows.Forms.Button()
        Me.lvwRules = New System.Windows.Forms.ListView()
        Me._lvwRules_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRules_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.lvwBankAccountDelay = New System.Windows.Forms.ListView()
        Me._lvwBankAccountDelay_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankAccountDelay_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankAccountDelay_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankAccountDelay_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdAddDelay = New System.Windows.Forms.Button()
        Me.cmdDeleteDelay = New System.Windows.Forms.Button()
        Me.cmdEditDelay = New System.Windows.Forms.Button()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.uctPickListBranches = New uctPickList.PickList()
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.txtProcessingDays = New System.Windows.Forms.TextBox()
        Me.txtFinancialInstitutionCode = New System.Windows.Forms.TextBox()
        Me.txtDirectDebitSupplierName = New System.Windows.Forms.TextBox()
        Me.txtDirectDebitSupplierID = New System.Windows.Forms.TextBox()
        Me.txtRemitter = New System.Windows.Forms.TextBox()
        Me.lblFinancialInstitutionCode = New System.Windows.Forms.Label()
        Me.lblSupplierName = New System.Windows.Forms.Label()
        Me.lblProcessingDays = New System.Windows.Forms.Label()
        Me.lblRemitter = New System.Windows.Forms.Label()
        Me.lblSupplierID = New System.Windows.Forms.Label()
        Me.imgIcon = New System.Windows.Forms.PictureBox()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlAccountHolder.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAccounts.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(338, 522)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 26
        Me.cmdApply.Text = "A&pply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(258, 522)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 25
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(18, 522)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 28
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(418, 522)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 27
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(178, 522)
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
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(105, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(537, 510)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblIBAN)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtIBAN)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBIC)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBIC)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblnextChequeNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReconciledDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDefaultBankAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReceiveCashIntoThisCurr)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStartChequeNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlDefaultBankAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlReconciledDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccountNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccountName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccountCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccountDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAccountHolder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlAccountHolder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtnextChequeNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdFindAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkReceiveCashIntoThisCurr)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStartChequeNumber)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(529, 484)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Bank Account Details"
        '
        'lblIBAN
        '
        Me.lblIBAN.BackColor = System.Drawing.SystemColors.Control
        Me.lblIBAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIBAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIBAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIBAN.Location = New System.Drawing.Point(16, 328)
        Me.lblIBAN.Name = "lblIBAN"
        Me.lblIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIBAN.Size = New System.Drawing.Size(137, 17)
        Me.lblIBAN.TabIndex = 43
        Me.lblIBAN.Text = "IBAN"
        '
        'txtIBAN
        '
        Me.txtIBAN.AcceptsReturn = True
        Me.txtIBAN.BackColor = System.Drawing.SystemColors.Window
        Me.txtIBAN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIBAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIBAN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIBAN.Location = New System.Drawing.Point(152, 328)
        Me.txtIBAN.MaxLength = 50
        Me.txtIBAN.Name = "txtIBAN"
        Me.txtIBAN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIBAN.Size = New System.Drawing.Size(217, 21)
        Me.txtIBAN.TabIndex = 42
        '
        'lblBIC
        '
        Me.lblBIC.BackColor = System.Drawing.SystemColors.Control
        Me.lblBIC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBIC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBIC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBIC.Location = New System.Drawing.Point(16, 301)
        Me.lblBIC.Name = "lblBIC"
        Me.lblBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBIC.Size = New System.Drawing.Size(137, 17)
        Me.lblBIC.TabIndex = 41
        Me.lblBIC.Text = "BIC"
        '
        'txtBIC
        '
        Me.txtBIC.AcceptsReturn = True
        Me.txtBIC.BackColor = System.Drawing.SystemColors.Window
        Me.txtBIC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBIC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBIC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBIC.Location = New System.Drawing.Point(152, 301)
        Me.txtBIC.MaxLength = 50
        Me.txtBIC.Name = "txtBIC"
        Me.txtBIC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBIC.Size = New System.Drawing.Size(217, 21)
        Me.txtBIC.TabIndex = 40
        '
        'lblAccountNumber
        '
        Me.lblAccountNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountNumber.Location = New System.Drawing.Point(16, 18)
        Me.lblAccountNumber.Name = "lblAccountNumber"
        Me.lblAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountNumber.Size = New System.Drawing.Size(121, 17)
        Me.lblAccountNumber.TabIndex = 7
        Me.lblAccountNumber.Text = "Account Number:"
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(16, 186)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(94, 13)
        Me.lblAccountName.TabIndex = 15
        Me.lblAccountName.Text = "Account Name:"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.AutoSize = True
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(16, 58)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(91, 13)
        Me.lblAccountCode.TabIndex = 9
        Me.lblAccountCode.Text = "Account Code:"
        '
        'lblAccountDescription
        '
        Me.lblAccountDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountDescription.Location = New System.Drawing.Point(16, 218)
        Me.lblAccountDescription.Name = "lblAccountDescription"
        Me.lblAccountDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountDescription.Size = New System.Drawing.Size(97, 17)
        Me.lblAccountDescription.TabIndex = 17
        Me.lblAccountDescription.Text = "Description:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 98)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(81, 17)
        Me.lblCurrency.TabIndex = 12
        Me.lblCurrency.Text = "Currency:"
        '
        'lblnextChequeNumber
        '
        Me.lblnextChequeNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblnextChequeNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblnextChequeNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblnextChequeNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblnextChequeNumber.Location = New System.Drawing.Point(16, 276)
        Me.lblnextChequeNumber.Name = "lblnextChequeNumber"
        Me.lblnextChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblnextChequeNumber.Size = New System.Drawing.Size(137, 17)
        Me.lblnextChequeNumber.TabIndex = 29
        Me.lblnextChequeNumber.Text = "Next Cheque Number:"
        Me.lblnextChequeNumber.Visible = False
        '
        'lblReconciledDate
        '
        Me.lblReconciledDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblReconciledDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReconciledDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReconciledDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReconciledDate.Location = New System.Drawing.Point(16, 376)
        Me.lblReconciledDate.Name = "lblReconciledDate"
        Me.lblReconciledDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReconciledDate.Size = New System.Drawing.Size(137, 17)
        Me.lblReconciledDate.TabIndex = 31
        Me.lblReconciledDate.Text = "Reconciled Date:"
        Me.lblReconciledDate.Visible = False
        '
        'lblDefaultBankAccount
        '
        Me.lblDefaultBankAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefaultBankAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefaultBankAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefaultBankAccount.Location = New System.Drawing.Point(16, 404)
        Me.lblDefaultBankAccount.Name = "lblDefaultBankAccount"
        Me.lblDefaultBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefaultBankAccount.Size = New System.Drawing.Size(137, 17)
        Me.lblDefaultBankAccount.TabIndex = 35
        Me.lblDefaultBankAccount.Text = "Default Bank Account:"
        Me.lblDefaultBankAccount.Visible = False
        '
        'lblReceiveCashIntoThisCurr
        '
        Me.lblReceiveCashIntoThisCurr.BackColor = System.Drawing.SystemColors.Control
        Me.lblReceiveCashIntoThisCurr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReceiveCashIntoThisCurr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReceiveCashIntoThisCurr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReceiveCashIntoThisCurr.Location = New System.Drawing.Point(313, 94)
        Me.lblReceiveCashIntoThisCurr.Name = "lblReceiveCashIntoThisCurr"
        Me.lblReceiveCashIntoThisCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReceiveCashIntoThisCurr.Size = New System.Drawing.Size(164, 32)
        Me.lblReceiveCashIntoThisCurr.TabIndex = 37
        Me.lblReceiveCashIntoThisCurr.Text = "Only receive cash into this account in this currency"
        '
        'lblStartChequeNumber
        '
        Me.lblStartChequeNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartChequeNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartChequeNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartChequeNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartChequeNumber.Location = New System.Drawing.Point(16, 250)
        Me.lblStartChequeNumber.Name = "lblStartChequeNumber"
        Me.lblStartChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartChequeNumber.Size = New System.Drawing.Size(137, 17)
        Me.lblStartChequeNumber.TabIndex = 39
        Me.lblStartChequeNumber.Text = "Start Cheque Number:"
        '
        'pnlDefaultBankAccount
        '
        Me.pnlDefaultBankAccount.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlDefaultBankAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlDefaultBankAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDefaultBankAccount.Location = New System.Drawing.Point(152, 406)
        Me.pnlDefaultBankAccount.Name = "pnlDefaultBankAccount"
        Me.pnlDefaultBankAccount.Size = New System.Drawing.Size(190, 19)
        Me.pnlDefaultBankAccount.TabIndex = 23
        Me.pnlDefaultBankAccount.Visible = False
        '
        'pnlReconciledDate
        '
        Me.pnlReconciledDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlReconciledDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlReconciledDate.Enabled = False
        Me.pnlReconciledDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlReconciledDate.Location = New System.Drawing.Point(152, 376)
        Me.pnlReconciledDate.Name = "pnlReconciledDate"
        Me.pnlReconciledDate.Size = New System.Drawing.Size(217, 19)
        Me.pnlReconciledDate.TabIndex = 21
        Me.pnlReconciledDate.Visible = False
        '
        'txtAccountNumber
        '
        Me.txtAccountNumber.AcceptsReturn = True
        Me.txtAccountNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountNumber.Location = New System.Drawing.Point(152, 18)
        Me.txtAccountNumber.MaxLength = 0
        Me.txtAccountNumber.Name = "txtAccountNumber"
        Me.txtAccountNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountNumber.Size = New System.Drawing.Size(129, 21)
        Me.txtAccountNumber.TabIndex = 8
        '
        'txtAccountName
        '
        Me.txtAccountName.AcceptsReturn = True
        Me.txtAccountName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountName.Location = New System.Drawing.Point(152, 186)
        Me.txtAccountName.MaxLength = 0
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountName.Size = New System.Drawing.Size(217, 21)
        Me.txtAccountName.TabIndex = 16
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(152, 58)
        Me.txtAccountCode.MaxLength = 0
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(129, 21)
        Me.txtAccountCode.TabIndex = 10
        '
        'txtAccountDescription
        '
        Me.txtAccountDescription.AcceptsReturn = True
        Me.txtAccountDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountDescription.Location = New System.Drawing.Point(152, 218)
        Me.txtAccountDescription.MaxLength = 0
        Me.txtAccountDescription.Name = "txtAccountDescription"
        Me.txtAccountDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountDescription.Size = New System.Drawing.Size(217, 21)
        Me.txtAccountDescription.TabIndex = 18
        '
        'cboCurrency
        '
        Me.cboCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCurrency.Location = New System.Drawing.Point(152, 94)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCurrency.Size = New System.Drawing.Size(129, 21)
        Me.cboCurrency.TabIndex = 11
        '
        'cmdAccountHolder
        '
        Me.cmdAccountHolder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccountHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccountHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccountHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccountHolder.Location = New System.Drawing.Point(14, 134)
        Me.cmdAccountHolder.Name = "cmdAccountHolder"
        Me.cmdAccountHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccountHolder.Size = New System.Drawing.Size(121, 26)
        Me.cmdAccountHolder.TabIndex = 13
        Me.cmdAccountHolder.Text = "Account Holder ..."
        Me.cmdAccountHolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAccountHolder.UseVisualStyleBackColor = False
        '
        'pnlAccountHolder
        '
        Me.pnlAccountHolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlAccountHolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAccountHolder.Controls.Add(Me.lblAccountHolder)
        Me.pnlAccountHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAccountHolder.Location = New System.Drawing.Point(152, 138)
        Me.pnlAccountHolder.Name = "pnlAccountHolder"
        Me.pnlAccountHolder.Size = New System.Drawing.Size(217, 19)
        Me.pnlAccountHolder.TabIndex = 14
        '
        'lblAccountHolder
        '
        Me.lblAccountHolder.AutoSize = True
        Me.lblAccountHolder.Location = New System.Drawing.Point(-2, 3)
        Me.lblAccountHolder.Name = "lblAccountHolder"
        Me.lblAccountHolder.Size = New System.Drawing.Size(0, 13)
        Me.lblAccountHolder.TabIndex = 0
        '
        'txtnextChequeNumber
        '
        Me.txtnextChequeNumber.AcceptsReturn = True
        Me.txtnextChequeNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtnextChequeNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtnextChequeNumber.Enabled = False
        Me.txtnextChequeNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtnextChequeNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtnextChequeNumber.Location = New System.Drawing.Point(152, 274)
        Me.txtnextChequeNumber.MaxLength = 10
        Me.txtnextChequeNumber.Name = "txtnextChequeNumber"
        Me.txtnextChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtnextChequeNumber.Size = New System.Drawing.Size(217, 21)
        Me.txtnextChequeNumber.TabIndex = 20
        Me.txtnextChequeNumber.Visible = False
        '
        'cmdFindAccount
        '
        Me.cmdFindAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindAccount.Location = New System.Drawing.Point(346, 406)
        Me.cmdFindAccount.Name = "cmdFindAccount"
        Me.cmdFindAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindAccount.Size = New System.Drawing.Size(25, 19)
        Me.cmdFindAccount.TabIndex = 22
        Me.cmdFindAccount.Text = "..."
        Me.cmdFindAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindAccount.UseVisualStyleBackColor = False
        Me.cmdFindAccount.Visible = False
        '
        'chkReceiveCashIntoThisCurr
        '
        Me.chkReceiveCashIntoThisCurr.BackColor = System.Drawing.SystemColors.Control
        Me.chkReceiveCashIntoThisCurr.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReceiveCashIntoThisCurr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReceiveCashIntoThisCurr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReceiveCashIntoThisCurr.Location = New System.Drawing.Point(291, 94)
        Me.chkReceiveCashIntoThisCurr.Name = "chkReceiveCashIntoThisCurr"
        Me.chkReceiveCashIntoThisCurr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReceiveCashIntoThisCurr.Size = New System.Drawing.Size(18, 17)
        Me.chkReceiveCashIntoThisCurr.TabIndex = 36
        Me.chkReceiveCashIntoThisCurr.Text = "Check1"
        Me.chkReceiveCashIntoThisCurr.UseVisualStyleBackColor = False
        '
        'txtStartChequeNumber
        '
        Me.txtStartChequeNumber.AcceptsReturn = True
        Me.txtStartChequeNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartChequeNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartChequeNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartChequeNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartChequeNumber.Location = New System.Drawing.Point(152, 248)
        Me.txtStartChequeNumber.MaxLength = 10
        Me.txtStartChequeNumber.Name = "txtStartChequeNumber"
        Me.txtStartChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartChequeNumber.Size = New System.Drawing.Size(217, 21)
        Me.txtStartChequeNumber.TabIndex = 19
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAccounts)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(529, 484)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "3 - Rules"
        '
        'fraAccounts
        '
        Me.fraAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.fraAccounts.Controls.Add(Me.cmdViewRule)
        Me.fraAccounts.Controls.Add(Me.cmdAddRule)
        Me.fraAccounts.Controls.Add(Me.cmdDeleteRule)
        Me.fraAccounts.Controls.Add(Me.cmdEditRule)
        Me.fraAccounts.Controls.Add(Me.lvwRules)
        Me.fraAccounts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAccounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAccounts.Location = New System.Drawing.Point(14, 10)
        Me.fraAccounts.Name = "fraAccounts"
        Me.fraAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAccounts.Size = New System.Drawing.Size(506, 369)
        Me.fraAccounts.TabIndex = 1
        Me.fraAccounts.TabStop = False
        '
        'cmdViewRule
        '
        Me.cmdViewRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewRule.Location = New System.Drawing.Point(170, 334)
        Me.cmdViewRule.Name = "cmdViewRule"
        Me.cmdViewRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewRule.Size = New System.Drawing.Size(73, 22)
        Me.cmdViewRule.TabIndex = 5
        Me.cmdViewRule.Text = "&View"
        Me.cmdViewRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdViewRule.UseVisualStyleBackColor = False
        '
        'cmdAddRule
        '
        Me.cmdAddRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRule.Location = New System.Drawing.Point(14, 334)
        Me.cmdAddRule.Name = "cmdAddRule"
        Me.cmdAddRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddRule.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddRule.TabIndex = 3
        Me.cmdAddRule.Text = "&Add"
        Me.cmdAddRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddRule.UseVisualStyleBackColor = False
        '
        'cmdDeleteRule
        '
        Me.cmdDeleteRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRule.Location = New System.Drawing.Point(248, 334)
        Me.cmdDeleteRule.Name = "cmdDeleteRule"
        Me.cmdDeleteRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRule.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteRule.TabIndex = 6
        Me.cmdDeleteRule.Text = "&Delete"
        Me.cmdDeleteRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteRule.UseVisualStyleBackColor = False
        '
        'cmdEditRule
        '
        Me.cmdEditRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRule.Location = New System.Drawing.Point(92, 334)
        Me.cmdEditRule.Name = "cmdEditRule"
        Me.cmdEditRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRule.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditRule.TabIndex = 4
        Me.cmdEditRule.Text = "&Edit"
        Me.cmdEditRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRule.UseVisualStyleBackColor = False
        '
        'lvwRules
        '
        Me.lvwRules.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRules.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRules_ColumnHeader_1, Me._lvwRules_ColumnHeader_2})
        Me.lvwRules.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRules.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRules.Location = New System.Drawing.Point(8, 14)
        Me.lvwRules.Name = "lvwRules"
        Me.lvwRules.Size = New System.Drawing.Size(491, 315)
        Me.lvwRules.TabIndex = 2
        Me.lvwRules.UseCompatibleStateImageBehavior = False
        Me.lvwRules.View = System.Windows.Forms.View.Details
        '
        '_lvwRules_ColumnHeader_1
        '
        Me._lvwRules_ColumnHeader_1.Text = "Media Type"
        Me._lvwRules_ColumnHeader_1.Width = 167
        '
        '_lvwRules_ColumnHeader_2
        '
        Me._lvwRules_ColumnHeader_2.Text = "Rule Type"
        Me._lvwRules_ColumnHeader_2.Width = 167
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.lvwBankAccountDelay)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdAddDelay)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdDeleteDelay)
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdEditDelay)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(529, 484)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "2 - Receipt Delay"
        '
        'lvwBankAccountDelay
        '
        Me.lvwBankAccountDelay.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBankAccountDelay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBankAccountDelay_ColumnHeader_1, Me._lvwBankAccountDelay_ColumnHeader_2, Me._lvwBankAccountDelay_ColumnHeader_3, Me._lvwBankAccountDelay_ColumnHeader_4})
        Me.lvwBankAccountDelay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBankAccountDelay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBankAccountDelay.FullRowSelect = True
        Me.lvwBankAccountDelay.GridLines = True
        Me.lvwBankAccountDelay.HideSelection = False
        Me.lvwBankAccountDelay.Location = New System.Drawing.Point(14, 16)
        Me.lvwBankAccountDelay.Name = "lvwBankAccountDelay"
        Me.lvwBankAccountDelay.Size = New System.Drawing.Size(505, 341)
        Me.lvwBankAccountDelay.TabIndex = 30
        Me.lvwBankAccountDelay.UseCompatibleStateImageBehavior = False
        Me.lvwBankAccountDelay.View = System.Windows.Forms.View.Details
        '
        '_lvwBankAccountDelay_ColumnHeader_1
        '
        Me._lvwBankAccountDelay_ColumnHeader_1.Text = "bankaccountdelayid"
        Me._lvwBankAccountDelay_ColumnHeader_1.Width = 0
        '
        '_lvwBankAccountDelay_ColumnHeader_2
        '
        Me._lvwBankAccountDelay_ColumnHeader_2.Text = "medatypeid"
        Me._lvwBankAccountDelay_ColumnHeader_2.Width = 0
        '
        '_lvwBankAccountDelay_ColumnHeader_3
        '
        Me._lvwBankAccountDelay_ColumnHeader_3.Text = "Media Type"
        Me._lvwBankAccountDelay_ColumnHeader_3.Width = 134
        '
        '_lvwBankAccountDelay_ColumnHeader_4
        '
        Me._lvwBankAccountDelay_ColumnHeader_4.Text = "Days Delay"
        Me._lvwBankAccountDelay_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._lvwBankAccountDelay_ColumnHeader_4.Width = 101
        '
        'cmdAddDelay
        '
        Me.cmdAddDelay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddDelay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddDelay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddDelay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddDelay.Location = New System.Drawing.Point(20, 362)
        Me.cmdAddDelay.Name = "cmdAddDelay"
        Me.cmdAddDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddDelay.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddDelay.TabIndex = 32
        Me.cmdAddDelay.Text = "&Add"
        Me.cmdAddDelay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddDelay.UseVisualStyleBackColor = False
        '
        'cmdDeleteDelay
        '
        Me.cmdDeleteDelay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteDelay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteDelay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteDelay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteDelay.Location = New System.Drawing.Point(176, 362)
        Me.cmdDeleteDelay.Name = "cmdDeleteDelay"
        Me.cmdDeleteDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteDelay.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteDelay.TabIndex = 34
        Me.cmdDeleteDelay.Text = "&Delete"
        Me.cmdDeleteDelay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteDelay.UseVisualStyleBackColor = False
        '
        'cmdEditDelay
        '
        Me.cmdEditDelay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditDelay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditDelay.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditDelay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditDelay.Location = New System.Drawing.Point(98, 362)
        Me.cmdEditDelay.Name = "cmdEditDelay"
        Me.cmdEditDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditDelay.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditDelay.TabIndex = 33
        Me.cmdEditDelay.Text = "&Edit"
        Me.cmdEditDelay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditDelay.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.uctPickListBranches)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(529, 484)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Branches"
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Branches/Companies"
        Me.uctPickListBranches.BusinessObject = "bACTBankAccount.Form"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(3, 7)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "BankAccount_Source"
        Me.uctPickListBranches.Size = New System.Drawing.Size(524, 381)
        Me.uctPickListBranches.TabIndex = 38
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(529, 484)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Connectivity"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtProcessingDays)
        Me.Frame1.Controls.Add(Me.txtFinancialInstitutionCode)
        Me.Frame1.Controls.Add(Me.txtDirectDebitSupplierName)
        Me.Frame1.Controls.Add(Me.txtDirectDebitSupplierID)
        Me.Frame1.Controls.Add(Me.txtRemitter)
        Me.Frame1.Controls.Add(Me.lblFinancialInstitutionCode)
        Me.Frame1.Controls.Add(Me.lblSupplierName)
        Me.Frame1.Controls.Add(Me.lblProcessingDays)
        Me.Frame1.Controls.Add(Me.lblRemitter)
        Me.Frame1.Controls.Add(Me.lblSupplierID)
        Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(10, 20)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(511, 367)
        Me.Frame1.TabIndex = 40
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Addition fields for XML Export"
        '
        'txtProcessingDays
        '
        Me.txtProcessingDays.AcceptsReturn = True
        Me.txtProcessingDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtProcessingDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProcessingDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProcessingDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProcessingDays.Location = New System.Drawing.Point(200, 130)
        Me.txtProcessingDays.MaxLength = 0
        Me.txtProcessingDays.Name = "txtProcessingDays"
        Me.txtProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProcessingDays.Size = New System.Drawing.Size(129, 21)
        Me.txtProcessingDays.TabIndex = 45
        '
        'txtFinancialInstitutionCode
        '
        Me.txtFinancialInstitutionCode.AcceptsReturn = True
        Me.txtFinancialInstitutionCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFinancialInstitutionCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFinancialInstitutionCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinancialInstitutionCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFinancialInstitutionCode.Location = New System.Drawing.Point(200, 26)
        Me.txtFinancialInstitutionCode.MaxLength = 0
        Me.txtFinancialInstitutionCode.Name = "txtFinancialInstitutionCode"
        Me.txtFinancialInstitutionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFinancialInstitutionCode.Size = New System.Drawing.Size(129, 21)
        Me.txtFinancialInstitutionCode.TabIndex = 44
        '
        'txtDirectDebitSupplierName
        '
        Me.txtDirectDebitSupplierName.AcceptsReturn = True
        Me.txtDirectDebitSupplierName.BackColor = System.Drawing.SystemColors.Window
        Me.txtDirectDebitSupplierName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDirectDebitSupplierName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDirectDebitSupplierName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDirectDebitSupplierName.Location = New System.Drawing.Point(200, 52)
        Me.txtDirectDebitSupplierName.MaxLength = 0
        Me.txtDirectDebitSupplierName.Name = "txtDirectDebitSupplierName"
        Me.txtDirectDebitSupplierName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDirectDebitSupplierName.Size = New System.Drawing.Size(129, 21)
        Me.txtDirectDebitSupplierName.TabIndex = 43
        '
        'txtDirectDebitSupplierID
        '
        Me.txtDirectDebitSupplierID.AcceptsReturn = True
        Me.txtDirectDebitSupplierID.BackColor = System.Drawing.SystemColors.Window
        Me.txtDirectDebitSupplierID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDirectDebitSupplierID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDirectDebitSupplierID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDirectDebitSupplierID.Location = New System.Drawing.Point(200, 78)
        Me.txtDirectDebitSupplierID.MaxLength = 0
        Me.txtDirectDebitSupplierID.Name = "txtDirectDebitSupplierID"
        Me.txtDirectDebitSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDirectDebitSupplierID.Size = New System.Drawing.Size(129, 21)
        Me.txtDirectDebitSupplierID.TabIndex = 42
        '
        'txtRemitter
        '
        Me.txtRemitter.AcceptsReturn = True
        Me.txtRemitter.BackColor = System.Drawing.SystemColors.Window
        Me.txtRemitter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRemitter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemitter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRemitter.Location = New System.Drawing.Point(200, 104)
        Me.txtRemitter.MaxLength = 0
        Me.txtRemitter.Name = "txtRemitter"
        Me.txtRemitter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRemitter.Size = New System.Drawing.Size(129, 21)
        Me.txtRemitter.TabIndex = 41
        '
        'lblFinancialInstitutionCode
        '
        Me.lblFinancialInstitutionCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinancialInstitutionCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinancialInstitutionCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinancialInstitutionCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinancialInstitutionCode.Location = New System.Drawing.Point(10, 26)
        Me.lblFinancialInstitutionCode.Name = "lblFinancialInstitutionCode"
        Me.lblFinancialInstitutionCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinancialInstitutionCode.Size = New System.Drawing.Size(159, 17)
        Me.lblFinancialInstitutionCode.TabIndex = 50
        Me.lblFinancialInstitutionCode.Text = "Financial Institution Code:"
        '
        'lblSupplierName
        '
        Me.lblSupplierName.BackColor = System.Drawing.SystemColors.Control
        Me.lblSupplierName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSupplierName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSupplierName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSupplierName.Location = New System.Drawing.Point(10, 52)
        Me.lblSupplierName.Name = "lblSupplierName"
        Me.lblSupplierName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSupplierName.Size = New System.Drawing.Size(177, 17)
        Me.lblSupplierName.TabIndex = 49
        Me.lblSupplierName.Text = "Direct Debit Supplier Name:"
        '
        'lblProcessingDays
        '
        Me.lblProcessingDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblProcessingDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProcessingDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProcessingDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProcessingDays.Location = New System.Drawing.Point(10, 130)
        Me.lblProcessingDays.Name = "lblProcessingDays"
        Me.lblProcessingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProcessingDays.Size = New System.Drawing.Size(121, 17)
        Me.lblProcessingDays.TabIndex = 48
        Me.lblProcessingDays.Text = "Processing Days:"
        '
        'lblRemitter
        '
        Me.lblRemitter.BackColor = System.Drawing.SystemColors.Control
        Me.lblRemitter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRemitter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemitter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRemitter.Location = New System.Drawing.Point(10, 104)
        Me.lblRemitter.Name = "lblRemitter"
        Me.lblRemitter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRemitter.Size = New System.Drawing.Size(121, 17)
        Me.lblRemitter.TabIndex = 47
        Me.lblRemitter.Text = "Remitter:"
        '
        'lblSupplierID
        '
        Me.lblSupplierID.BackColor = System.Drawing.SystemColors.Control
        Me.lblSupplierID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSupplierID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSupplierID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSupplierID.Location = New System.Drawing.Point(10, 78)
        Me.lblSupplierID.Name = "lblSupplierID"
        Me.lblSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSupplierID.Size = New System.Drawing.Size(165, 17)
        Me.lblSupplierID.TabIndex = 46
        Me.lblSupplierID.Text = "Direct Debit Supplier ID:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(627, -36)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdApply
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(548, 547)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Bank Account"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pnlAccountHolder.ResumeLayout(False)
        Me.pnlAccountHolder.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAccounts.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblAccountHolder As System.Windows.Forms.Label
    Public WithEvents lblIBAN As System.Windows.Forms.Label
    Public WithEvents txtIBAN As System.Windows.Forms.TextBox
    Public WithEvents lblBIC As System.Windows.Forms.Label
    Public WithEvents txtBIC As System.Windows.Forms.TextBox
#End Region 
End Class