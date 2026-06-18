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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboFSATypeOfSale As PMLookupControl.cboPMLookup
	Public WithEvents cboPMCommissionTax As PMLookupControl.cboPMLookup
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtPercentage As System.Windows.Forms.TextBox
	Public WithEvents txtCommissionPercentage As System.Windows.Forms.TextBox
	Public WithEvents txtCommissionAmount As System.Windows.Forms.TextBox
	Public WithEvents cboRiskGroup As System.Windows.Forms.ComboBox
	Public WithEvents chkDisplayOnQuotes As System.Windows.Forms.CheckBox
	Public WithEvents cboScheme As System.Windows.Forms.ComboBox
	Public WithEvents cboPMTransType As PMLookupControl.cboPMLookup
	Public WithEvents cboPMTax As PMLookupControl.cboPMLookup
	Public WithEvents lblFSATypeOfSale As System.Windows.Forms.Label
	Public WithEvents lblCommissionTax As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblPercentage As System.Windows.Forms.Label
	Public WithEvents lblCommissionPercentage As System.Windows.Forms.Label
	Public WithEvents lblCommissionAmount As System.Windows.Forms.Label
	Public WithEvents lblDisplayOnQuotes As System.Windows.Forms.Label
	Public WithEvents lblPMTransType As System.Windows.Forms.Label
	Public WithEvents lblTax As System.Windows.Forms.Label
	Public WithEvents lblRiskGroup As System.Windows.Forms.Label
	Public WithEvents lblScheme As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraGeneral = New System.Windows.Forms.GroupBox
        Me.cboFSATypeOfSale = New PMLookupControl.cboPMLookup
        Me.cboPMCommissionTax = New PMLookupControl.cboPMLookup
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me.txtCommissionPercentage = New System.Windows.Forms.TextBox
        Me.txtCommissionAmount = New System.Windows.Forms.TextBox
        Me.cboRiskGroup = New System.Windows.Forms.ComboBox
        Me.chkDisplayOnQuotes = New System.Windows.Forms.CheckBox
        Me.cboScheme = New System.Windows.Forms.ComboBox
        Me.cboPMTransType = New PMLookupControl.cboPMLookup
        Me.cboPMTax = New PMLookupControl.cboPMLookup
        Me.lblFSATypeOfSale = New System.Windows.Forms.Label
        Me.lblCommissionTax = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblPercentage = New System.Windows.Forms.Label
        Me.lblCommissionPercentage = New System.Windows.Forms.Label
        Me.lblCommissionAmount = New System.Windows.Forms.Label
        Me.lblDisplayOnQuotes = New System.Windows.Forms.Label
        Me.lblPMTransType = New System.Windows.Forms.Label
        Me.lblTax = New System.Windows.Forms.Label
        Me.lblRiskGroup = New System.Windows.Forms.Label
        Me.lblScheme = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(400, 416)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 20
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(0, 416)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 21
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(320, 416)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 19
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(240, 416)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 18
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(92, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(473, 395)
        Me.tabMainTab.TabIndex = 22
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraGeneral)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(465, 369)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Party Fees"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraGeneral
        '
        Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.fraGeneral.Controls.Add(Me.cboFSATypeOfSale)
        Me.fraGeneral.Controls.Add(Me.cboPMCommissionTax)
        Me.fraGeneral.Controls.Add(Me.cboCurrency)
        Me.fraGeneral.Controls.Add(Me.txtAmount)
        Me.fraGeneral.Controls.Add(Me.txtPercentage)
        Me.fraGeneral.Controls.Add(Me.txtCommissionPercentage)
        Me.fraGeneral.Controls.Add(Me.txtCommissionAmount)
        Me.fraGeneral.Controls.Add(Me.cboRiskGroup)
        Me.fraGeneral.Controls.Add(Me.chkDisplayOnQuotes)
        Me.fraGeneral.Controls.Add(Me.cboScheme)
        Me.fraGeneral.Controls.Add(Me.cboPMTransType)
        Me.fraGeneral.Controls.Add(Me.cboPMTax)
        Me.fraGeneral.Controls.Add(Me.lblFSATypeOfSale)
        Me.fraGeneral.Controls.Add(Me.lblCommissionTax)
        Me.fraGeneral.Controls.Add(Me.lblCurrency)
        Me.fraGeneral.Controls.Add(Me.lblAmount)
        Me.fraGeneral.Controls.Add(Me.lblPercentage)
        Me.fraGeneral.Controls.Add(Me.lblCommissionPercentage)
        Me.fraGeneral.Controls.Add(Me.lblCommissionAmount)
        Me.fraGeneral.Controls.Add(Me.lblDisplayOnQuotes)
        Me.fraGeneral.Controls.Add(Me.lblPMTransType)
        Me.fraGeneral.Controls.Add(Me.lblTax)
        Me.fraGeneral.Controls.Add(Me.lblRiskGroup)
        Me.fraGeneral.Controls.Add(Me.lblScheme)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(16, 12)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(444, 342)
        Me.fraGeneral.TabIndex = 23
        Me.fraGeneral.TabStop = False
        '
        'cboFSATypeOfSale
        '
        Me.cboFSATypeOfSale.DefaultItemId = 0
        Me.cboFSATypeOfSale.FirstItem = "(Select Type Of Sale)"
        Me.cboFSATypeOfSale.ItemId = 0
        Me.cboFSATypeOfSale.ListIndex = -1
        Me.cboFSATypeOfSale.Location = New System.Drawing.Point(190, 344)
        Me.cboFSATypeOfSale.Name = "cboFSATypeOfSale"
        Me.cboFSATypeOfSale.PMLookupProductFamily = 1
        Me.cboFSATypeOfSale.SingleItemId = 0
        Me.cboFSATypeOfSale.Size = New System.Drawing.Size(241, 21)
        Me.cboFSATypeOfSale.Sorted = True
        Me.cboFSATypeOfSale.TabIndex = 29
        Me.cboFSATypeOfSale.TableName = "FSA_Type_Of_Sale"
        Me.cboFSATypeOfSale.ToolTipText = ""
        Me.cboFSATypeOfSale.WhereClause = ""
        '
        'cboPMCommissionTax
        '
        Me.cboPMCommissionTax.DefaultItemId = 0
        Me.cboPMCommissionTax.FirstItem = "(none)"
        Me.cboPMCommissionTax.ItemId = 0
        Me.cboPMCommissionTax.ListIndex = -1
        Me.cboPMCommissionTax.Location = New System.Drawing.Point(190, 304)
        Me.cboPMCommissionTax.Name = "cboPMCommissionTax"
        Me.cboPMCommissionTax.PMLookupProductFamily = 1
        Me.cboPMCommissionTax.SingleItemId = 0
        Me.cboPMCommissionTax.Size = New System.Drawing.Size(240, 21)
        Me.cboPMCommissionTax.Sorted = True
        Me.cboPMCommissionTax.TabIndex = 26
        Me.cboPMCommissionTax.TableName = "tax_group"
        Me.cboPMCommissionTax.ToolTipText = ""
        Me.cboPMCommissionTax.WhereClause = ""
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(190, 160)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(240, 21)
        Me.cboCurrency.TabIndex = 25
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(190, 218)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(160, 21)
        Me.txtAmount.TabIndex = 13
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(190, 190)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(80, 21)
        Me.txtPercentage.TabIndex = 11
        '
        'txtCommissionPercentage
        '
        Me.txtCommissionPercentage.AcceptsReturn = True
        Me.txtCommissionPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionPercentage.Location = New System.Drawing.Point(190, 247)
        Me.txtCommissionPercentage.MaxLength = 0
        Me.txtCommissionPercentage.Name = "txtCommissionPercentage"
        Me.txtCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionPercentage.Size = New System.Drawing.Size(80, 21)
        Me.txtCommissionPercentage.TabIndex = 15
        '
        'txtCommissionAmount
        '
        Me.txtCommissionAmount.AcceptsReturn = True
        Me.txtCommissionAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommissionAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommissionAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommissionAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommissionAmount.Location = New System.Drawing.Point(190, 275)
        Me.txtCommissionAmount.MaxLength = 0
        Me.txtCommissionAmount.Name = "txtCommissionAmount"
        Me.txtCommissionAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommissionAmount.Size = New System.Drawing.Size(160, 21)
        Me.txtCommissionAmount.TabIndex = 17
        '
        'cboRiskGroup
        '
        Me.cboRiskGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboRiskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRiskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRiskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRiskGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRiskGroup.Location = New System.Drawing.Point(190, 19)
        Me.cboRiskGroup.Name = "cboRiskGroup"
        Me.cboRiskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRiskGroup.Size = New System.Drawing.Size(240, 21)
        Me.cboRiskGroup.TabIndex = 1
        '
        'chkDisplayOnQuotes
        '
        Me.chkDisplayOnQuotes.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayOnQuotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayOnQuotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayOnQuotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayOnQuotes.Location = New System.Drawing.Point(190, 76)
        Me.chkDisplayOnQuotes.Name = "chkDisplayOnQuotes"
        Me.chkDisplayOnQuotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayOnQuotes.Size = New System.Drawing.Size(81, 21)
        Me.chkDisplayOnQuotes.TabIndex = 5
        Me.chkDisplayOnQuotes.Text = "     "
        Me.chkDisplayOnQuotes.UseVisualStyleBackColor = False
        '
        'cboScheme
        '
        Me.cboScheme.BackColor = System.Drawing.SystemColors.Window
        Me.cboScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboScheme.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboScheme.Location = New System.Drawing.Point(190, 48)
        Me.cboScheme.Name = "cboScheme"
        Me.cboScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboScheme.Size = New System.Drawing.Size(240, 21)
        Me.cboScheme.TabIndex = 3
        '
        'cboPMTransType
        '
        Me.cboPMTransType.DefaultItemId = 0
        Me.cboPMTransType.FirstItem = "(none)"
        Me.cboPMTransType.ItemId = 0
        Me.cboPMTransType.ListIndex = -1
        Me.cboPMTransType.Location = New System.Drawing.Point(190, 105)
        Me.cboPMTransType.Name = "cboPMTransType"
        Me.cboPMTransType.PMLookupProductFamily = 9
        Me.cboPMTransType.SingleItemId = 0
        Me.cboPMTransType.Size = New System.Drawing.Size(240, 21)
        Me.cboPMTransType.Sorted = True
        Me.cboPMTransType.TabIndex = 7
        Me.cboPMTransType.TableName = "transaction_type"
        Me.cboPMTransType.ToolTipText = ""
        Me.cboPMTransType.WhereClause = ""
        '
        'cboPMTax
        '
        Me.cboPMTax.DefaultItemId = 0
        Me.cboPMTax.FirstItem = "(none)"
        Me.cboPMTax.ItemId = 0
        Me.cboPMTax.ListIndex = -1
        Me.cboPMTax.Location = New System.Drawing.Point(190, 133)
        Me.cboPMTax.Name = "cboPMTax"
        Me.cboPMTax.PMLookupProductFamily = 9
        Me.cboPMTax.SingleItemId = 0
        Me.cboPMTax.Size = New System.Drawing.Size(240, 21)
        Me.cboPMTax.Sorted = True
        Me.cboPMTax.TabIndex = 9
        Me.cboPMTax.TableName = "tax_group"
        Me.cboPMTax.ToolTipText = ""
        Me.cboPMTax.WhereClause = ""
        '
        'lblFSATypeOfSale
        '
        Me.lblFSATypeOfSale.BackColor = System.Drawing.SystemColors.Control
        Me.lblFSATypeOfSale.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFSATypeOfSale.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFSATypeOfSale.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFSATypeOfSale.Location = New System.Drawing.Point(8, 344)
        Me.lblFSATypeOfSale.Name = "lblFSATypeOfSale"
        Me.lblFSATypeOfSale.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFSATypeOfSale.Size = New System.Drawing.Size(129, 17)
        Me.lblFSATypeOfSale.TabIndex = 28
        Me.lblFSATypeOfSale.Text = "Type Of Sale:"
        '
        'lblCommissionTax
        '
        Me.lblCommissionTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionTax.Location = New System.Drawing.Point(16, 304)
        Me.lblCommissionTax.Name = "lblCommissionTax"
        Me.lblCommissionTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionTax.Size = New System.Drawing.Size(153, 19)
        Me.lblCommissionTax.TabIndex = 27
        Me.lblCommissionTax.Text = "Tax on Commission:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 163)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(133, 19)
        Me.lblCurrency.TabIndex = 24
        Me.lblCurrency.Text = "Currency:"
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(16, 220)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(133, 19)
        Me.lblAmount.TabIndex = 12
        Me.lblAmount.Text = "Amount:"
        '
        'lblPercentage
        '
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(16, 192)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(133, 19)
        Me.lblPercentage.TabIndex = 10
        Me.lblPercentage.Text = "Percentage:"
        '
        'lblCommissionPercentage
        '
        Me.lblCommissionPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionPercentage.Location = New System.Drawing.Point(16, 249)
        Me.lblCommissionPercentage.Name = "lblCommissionPercentage"
        Me.lblCommissionPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionPercentage.Size = New System.Drawing.Size(185, 19)
        Me.lblCommissionPercentage.TabIndex = 14
        Me.lblCommissionPercentage.Text = "Commission Percentage:"
        '
        'lblCommissionAmount
        '
        Me.lblCommissionAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionAmount.Location = New System.Drawing.Point(16, 277)
        Me.lblCommissionAmount.Name = "lblCommissionAmount"
        Me.lblCommissionAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionAmount.Size = New System.Drawing.Size(181, 19)
        Me.lblCommissionAmount.TabIndex = 16
        Me.lblCommissionAmount.Text = "Commission Amount:"
        '
        'lblDisplayOnQuotes
        '
        Me.lblDisplayOnQuotes.BackColor = System.Drawing.SystemColors.Control
        Me.lblDisplayOnQuotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisplayOnQuotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisplayOnQuotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDisplayOnQuotes.Location = New System.Drawing.Point(16, 78)
        Me.lblDisplayOnQuotes.Name = "lblDisplayOnQuotes"
        Me.lblDisplayOnQuotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDisplayOnQuotes.Size = New System.Drawing.Size(153, 19)
        Me.lblDisplayOnQuotes.TabIndex = 4
        Me.lblDisplayOnQuotes.Text = "Display on Quotes:"
        '
        'lblPMTransType
        '
        Me.lblPMTransType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMTransType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMTransType.Location = New System.Drawing.Point(16, 107)
        Me.lblPMTransType.Name = "lblPMTransType"
        Me.lblPMTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMTransType.Size = New System.Drawing.Size(171, 19)
        Me.lblPMTransType.TabIndex = 6
        Me.lblPMTransType.Text = "Mandatory on:"
        '
        'lblTax
        '
        Me.lblTax.AutoSize = True
        Me.lblTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTax.Location = New System.Drawing.Point(16, 135)
        Me.lblTax.Name = "lblTax"
        Me.lblTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTax.Size = New System.Drawing.Size(69, 13)
        Me.lblTax.TabIndex = 8
        Me.lblTax.Text = "Tax to Apply:"
        '
        'lblRiskGroup
        '
        Me.lblRiskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskGroup.Location = New System.Drawing.Point(16, 21)
        Me.lblRiskGroup.Name = "lblRiskGroup"
        Me.lblRiskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskGroup.Size = New System.Drawing.Size(117, 19)
        Me.lblRiskGroup.TabIndex = 0
        Me.lblRiskGroup.Text = "Risk Group:"
        '
        'lblScheme
        '
        Me.lblScheme.BackColor = System.Drawing.SystemColors.Control
        Me.lblScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScheme.Location = New System.Drawing.Point(16, 50)
        Me.lblScheme.Name = "lblScheme"
        Me.lblScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScheme.Size = New System.Drawing.Size(117, 19)
        Me.lblScheme.TabIndex = 2
        Me.lblScheme.Text = "Scheme:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(487, 450)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Rates"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.fraGeneral.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class