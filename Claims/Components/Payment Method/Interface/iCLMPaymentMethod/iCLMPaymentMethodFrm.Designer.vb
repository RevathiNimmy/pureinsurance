<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdAuthorise As System.Windows.Forms.Button
	Public WithEvents cmdReject As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblParty As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblComments As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents cmdParty As System.Windows.Forms.Button
	Public WithEvents OptClientAccount As System.Windows.Forms.RadioButton
	Public WithEvents OptAgentAccount As System.Windows.Forms.RadioButton
	Public WithEvents OptPartyAccount As System.Windows.Forms.RadioButton
	Public WithEvents OptClaimAccount As System.Windows.Forms.RadioButton
	Public WithEvents FraSelectMethod As System.Windows.Forms.GroupBox
	Public WithEvents txtParty As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtComments As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As PMLookupControl.cboPMLookup
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cboMediaType As PMLookupControl.cboPMLookup
	Public WithEvents lblMediaType As System.Windows.Forms.Label
	Public WithEvents fraPaymentInformation As System.Windows.Forms.GroupBox
	Public WithEvents txtPayeeComments As System.Windows.Forms.TextBox
	Public WithEvents txtAccountNo As System.Windows.Forms.TextBox
	Public WithEvents txtSortCode As System.Windows.Forms.TextBox
	Public WithEvents txtBankName As System.Windows.Forms.TextBox
	Public WithEvents txtPayeeName As System.Windows.Forms.TextBox
	Public WithEvents cboCountry As PMLookupControl.cboPMLookup
	Public WithEvents lblPayeeComments As System.Windows.Forms.Label
	Public WithEvents lblCountry As System.Windows.Forms.Label
	Public WithEvents lblAccountNo As System.Windows.Forms.Label
	Public WithEvents lblSortCode As System.Windows.Forms.Label
	Public WithEvents lblBankName As System.Windows.Forms.Label
	Public WithEvents lblPayeeName As System.Windows.Forms.Label
	Public WithEvents fraPayee As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAuthorise = New System.Windows.Forms.Button
        Me.cmdReject = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblParty = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblComments = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.cmdParty = New System.Windows.Forms.Button
        Me.FraSelectMethod = New System.Windows.Forms.GroupBox
        Me.OptClientAccount = New System.Windows.Forms.RadioButton
        Me.OptAgentAccount = New System.Windows.Forms.RadioButton
        Me.OptPartyAccount = New System.Windows.Forms.RadioButton
        Me.OptClaimAccount = New System.Windows.Forms.RadioButton
        Me.txtParty = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.cboCurrency = New PMLookupControl.cboPMLookup
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraPaymentInformation = New System.Windows.Forms.GroupBox
        Me.cboMediaType = New PMLookupControl.cboPMLookup
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.fraPayee = New System.Windows.Forms.GroupBox
        Me.txtPayeeComments = New System.Windows.Forms.TextBox
        Me.txtAccountNo = New System.Windows.Forms.TextBox
        Me.txtSortCode = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtPayeeName = New System.Windows.Forms.TextBox
        Me.cboCountry = New PMLookupControl.cboPMLookup
        Me.lblPayeeComments = New System.Windows.Forms.Label
        Me.lblCountry = New System.Windows.Forms.Label
        Me.lblAccountNo = New System.Windows.Forms.Label
        Me.lblSortCode = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblPayeeName = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.FraSelectMethod.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraPaymentInformation.SuspendLayout()
        Me.fraPayee.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAuthorise
        '
        Me.cmdAuthorise.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAuthorise.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAuthorise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAuthorise.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAuthorise.Location = New System.Drawing.Point(8, 344)
        Me.cmdAuthorise.Name = "cmdAuthorise"
        Me.cmdAuthorise.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAuthorise.Size = New System.Drawing.Size(73, 22)
        Me.cmdAuthorise.TabIndex = 32
        Me.cmdAuthorise.TabStop = False
        Me.cmdAuthorise.Text = "&Authorise"
        Me.cmdAuthorise.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAuthorise.UseVisualStyleBackColor = False
        '
        'cmdReject
        '
        Me.cmdReject.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReject.Location = New System.Drawing.Point(86, 344)
        Me.cmdReject.Name = "cmdReject"
        Me.cmdReject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReject.Size = New System.Drawing.Size(73, 22)
        Me.cmdReject.TabIndex = 33
        Me.cmdReject.TabStop = False
        Me.cmdReject.Text = "&Reject"
        Me.cmdReject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReject.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(416, 344)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(336, 344)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.TabStop = False
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
        Me.cmdOK.Location = New System.Drawing.Point(256, 344)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(52, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(485, 333)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 4
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblParty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblComments)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdParty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.FraSelectMethod)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtParty)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtComments)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(477, 307)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblParty
        '
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(16, 163)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(41, 17)
        Me.lblParty.TabIndex = 10
        Me.lblParty.Text = "Party"
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(250, 163)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(54, 17)
        Me.lblAmount.TabIndex = 12
        Me.lblAmount.Text = "Amount"
        '
        'lblComments
        '
        Me.lblComments.BackColor = System.Drawing.SystemColors.Control
        Me.lblComments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComments.Location = New System.Drawing.Point(16, 187)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComments.Size = New System.Drawing.Size(81, 17)
        Me.lblComments.TabIndex = 14
        Me.lblComments.Text = "Comments"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(250, 186)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(53, 17)
        Me.lblCurrency.TabIndex = 34
        Me.lblCurrency.Text = "Currency"
        '
        'cmdParty
        '
        Me.cmdParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParty.Location = New System.Drawing.Point(218, 160)
        Me.cmdParty.Name = "cmdParty"
        Me.cmdParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParty.Size = New System.Drawing.Size(16, 19)
        Me.cmdParty.TabIndex = 0
        Me.cmdParty.TabStop = False
        Me.cmdParty.Text = ".."
        Me.cmdParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdParty.UseVisualStyleBackColor = False
        '
        'FraSelectMethod
        '
        Me.FraSelectMethod.BackColor = System.Drawing.SystemColors.Control
        Me.FraSelectMethod.Controls.Add(Me.OptClientAccount)
        Me.FraSelectMethod.Controls.Add(Me.OptAgentAccount)
        Me.FraSelectMethod.Controls.Add(Me.OptPartyAccount)
        Me.FraSelectMethod.Controls.Add(Me.OptClaimAccount)
        Me.FraSelectMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FraSelectMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FraSelectMethod.Location = New System.Drawing.Point(8, 12)
        Me.FraSelectMethod.Name = "FraSelectMethod"
        Me.FraSelectMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FraSelectMethod.Size = New System.Drawing.Size(465, 137)
        Me.FraSelectMethod.TabIndex = 5
        Me.FraSelectMethod.TabStop = False
        Me.FraSelectMethod.Text = "Select Payment Method"
        '
        'OptClientAccount
        '
        Me.OptClientAccount.BackColor = System.Drawing.SystemColors.Control
        Me.OptClientAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClientAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClientAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClientAccount.Location = New System.Drawing.Point(16, 96)
        Me.OptClientAccount.Name = "OptClientAccount"
        Me.OptClientAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClientAccount.Size = New System.Drawing.Size(234, 24)
        Me.OptClientAccount.TabIndex = 9
        Me.OptClientAccount.TabStop = True
        Me.OptClientAccount.Text = "Client Payable Account"
        Me.OptClientAccount.UseVisualStyleBackColor = False
        '
        'OptAgentAccount
        '
        Me.OptAgentAccount.BackColor = System.Drawing.SystemColors.Control
        Me.OptAgentAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptAgentAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptAgentAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptAgentAccount.Location = New System.Drawing.Point(16, 72)
        Me.OptAgentAccount.Name = "OptAgentAccount"
        Me.OptAgentAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAgentAccount.Size = New System.Drawing.Size(393, 24)
        Me.OptAgentAccount.TabIndex = 8
        Me.OptAgentAccount.TabStop = True
        Me.OptAgentAccount.Text = "Agent Payable Account"
        Me.OptAgentAccount.UseVisualStyleBackColor = False
        '
        'OptPartyAccount
        '
        Me.OptPartyAccount.BackColor = System.Drawing.SystemColors.Control
        Me.OptPartyAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptPartyAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPartyAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptPartyAccount.Location = New System.Drawing.Point(16, 48)
        Me.OptPartyAccount.Name = "OptPartyAccount"
        Me.OptPartyAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptPartyAccount.Size = New System.Drawing.Size(234, 24)
        Me.OptPartyAccount.TabIndex = 7
        Me.OptPartyAccount.TabStop = True
        Me.OptPartyAccount.Text = "Party Payable Account"
        Me.OptPartyAccount.UseVisualStyleBackColor = False
        '
        'OptClaimAccount
        '
        Me.OptClaimAccount.BackColor = System.Drawing.SystemColors.Control
        Me.OptClaimAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptClaimAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptClaimAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptClaimAccount.Location = New System.Drawing.Point(16, 24)
        Me.OptClaimAccount.Name = "OptClaimAccount"
        Me.OptClaimAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptClaimAccount.Size = New System.Drawing.Size(234, 24)
        Me.OptClaimAccount.TabIndex = 6
        Me.OptClaimAccount.TabStop = True
        Me.OptClaimAccount.Text = "Claim Payment Account"
        Me.OptClaimAccount.UseVisualStyleBackColor = False
        '
        'txtParty
        '
        Me.txtParty.AcceptsReturn = True
        Me.txtParty.BackColor = System.Drawing.SystemColors.Window
        Me.txtParty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParty.Location = New System.Drawing.Point(67, 160)
        Me.txtParty.MaxLength = 0
        Me.txtParty.Name = "txtParty"
        Me.txtParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParty.Size = New System.Drawing.Size(153, 20)
        Me.txtParty.TabIndex = 11
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(320, 160)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(145, 20)
        Me.txtAmount.TabIndex = 13
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(16, 212)
        Me.txtComments.MaxLength = 255
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(449, 49)
        Me.txtComments.TabIndex = 15
        '
        'cboCurrency
        '
        Me.cboCurrency.DefaultItemId = 0
        Me.cboCurrency.Enabled = False
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ItemId = 0
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(320, 184)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.PMLookupProductFamily = 1
        Me.cboCurrency.SingleItemId = 0
        Me.cboCurrency.Size = New System.Drawing.Size(145, 21)
        Me.cboCurrency.Sorted = True
        Me.cboCurrency.TabIndex = 35
        Me.cboCurrency.TableName = "Currency"
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhereClause = ""
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraPaymentInformation)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraPayee)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(477, 307)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Tab 1"
        '
        'fraPaymentInformation
        '
        Me.fraPaymentInformation.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentInformation.Controls.Add(Me.cboMediaType)
        Me.fraPaymentInformation.Controls.Add(Me.lblMediaType)
        Me.fraPaymentInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentInformation.Location = New System.Drawing.Point(8, 12)
        Me.fraPaymentInformation.Name = "fraPaymentInformation"
        Me.fraPaymentInformation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentInformation.Size = New System.Drawing.Size(465, 57)
        Me.fraPaymentInformation.TabIndex = 16
        Me.fraPaymentInformation.TabStop = False
        Me.fraPaymentInformation.Text = "Payment Information"
        '
        'cboMediaType
        '
        Me.cboMediaType.DefaultItemId = 0
        Me.cboMediaType.FirstItem = ""
        Me.cboMediaType.ItemId = 0
        Me.cboMediaType.ListIndex = -1
        Me.cboMediaType.Location = New System.Drawing.Point(152, 24)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.PMLookupProductFamily = 1
        Me.cboMediaType.SingleItemId = 0
        Me.cboMediaType.Size = New System.Drawing.Size(193, 21)
        Me.cboMediaType.Sorted = True
        Me.cboMediaType.TabIndex = 18
        Me.cboMediaType.TableName = "MediaType"
        Me.cboMediaType.ToolTipText = ""
        Me.cboMediaType.WhereClause = ""
        '
        'lblMediaType
        '
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(16, 24)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(97, 17)
        Me.lblMediaType.TabIndex = 25
        Me.lblMediaType.Text = "Media Type"
        '
        'fraPayee
        '
        Me.fraPayee.BackColor = System.Drawing.SystemColors.Control
        Me.fraPayee.Controls.Add(Me.txtPayeeComments)
        Me.fraPayee.Controls.Add(Me.txtAccountNo)
        Me.fraPayee.Controls.Add(Me.txtSortCode)
        Me.fraPayee.Controls.Add(Me.txtBankName)
        Me.fraPayee.Controls.Add(Me.txtPayeeName)
        Me.fraPayee.Controls.Add(Me.cboCountry)
        Me.fraPayee.Controls.Add(Me.lblPayeeComments)
        Me.fraPayee.Controls.Add(Me.lblCountry)
        Me.fraPayee.Controls.Add(Me.lblAccountNo)
        Me.fraPayee.Controls.Add(Me.lblSortCode)
        Me.fraPayee.Controls.Add(Me.lblBankName)
        Me.fraPayee.Controls.Add(Me.lblPayeeName)
        Me.fraPayee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPayee.Location = New System.Drawing.Point(8, 76)
        Me.fraPayee.Name = "fraPayee"
        Me.fraPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPayee.Size = New System.Drawing.Size(465, 225)
        Me.fraPayee.TabIndex = 17
        Me.fraPayee.TabStop = False
        Me.fraPayee.Text = "Payee"
        '
        'txtPayeeComments
        '
        Me.txtPayeeComments.AcceptsReturn = True
        Me.txtPayeeComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeComments.Location = New System.Drawing.Point(16, 160)
        Me.txtPayeeComments.MaxLength = 255
        Me.txtPayeeComments.Multiline = True
        Me.txtPayeeComments.Name = "txtPayeeComments"
        Me.txtPayeeComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPayeeComments.Size = New System.Drawing.Size(433, 49)
        Me.txtPayeeComments.TabIndex = 24
        '
        'txtAccountNo
        '
        Me.txtAccountNo.AcceptsReturn = True
        Me.txtAccountNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountNo.Location = New System.Drawing.Point(152, 96)
        Me.txtAccountNo.MaxLength = 30
        Me.txtAccountNo.Name = "txtAccountNo"
        Me.txtAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountNo.Size = New System.Drawing.Size(185, 20)
        Me.txtAccountNo.TabIndex = 22
        '
        'txtSortCode
        '
        Me.txtSortCode.AcceptsReturn = True
        Me.txtSortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtSortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSortCode.Location = New System.Drawing.Point(152, 72)
        Me.txtSortCode.MaxLength = 8
        Me.txtSortCode.Name = "txtSortCode"
        Me.txtSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSortCode.Size = New System.Drawing.Size(73, 20)
        Me.txtSortCode.TabIndex = 21
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(152, 48)
        Me.txtBankName.MaxLength = 255
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(297, 20)
        Me.txtBankName.TabIndex = 20
        '
        'txtPayeeName
        '
        Me.txtPayeeName.AcceptsReturn = True
        Me.txtPayeeName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayeeName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayeeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayeeName.Location = New System.Drawing.Point(152, 24)
        Me.txtPayeeName.MaxLength = 255
        Me.txtPayeeName.Name = "txtPayeeName"
        Me.txtPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayeeName.Size = New System.Drawing.Size(297, 20)
        Me.txtPayeeName.TabIndex = 19
        '
        'cboCountry
        '
        Me.cboCountry.DefaultItemId = 0
        Me.cboCountry.FirstItem = ""
        Me.cboCountry.ItemId = 0
        Me.cboCountry.ListIndex = -1
        Me.cboCountry.Location = New System.Drawing.Point(152, 120)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.PMLookupProductFamily = 1
        Me.cboCountry.SingleItemId = 0
        Me.cboCountry.Size = New System.Drawing.Size(297, 21)
        Me.cboCountry.Sorted = True
        Me.cboCountry.TabIndex = 23
        Me.cboCountry.TableName = "Country"
        Me.cboCountry.ToolTipText = ""
        Me.cboCountry.WhereClause = ""
        '
        'lblPayeeComments
        '
        Me.lblPayeeComments.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeComments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeComments.Location = New System.Drawing.Point(16, 144)
        Me.lblPayeeComments.Name = "lblPayeeComments"
        Me.lblPayeeComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeComments.Size = New System.Drawing.Size(129, 17)
        Me.lblPayeeComments.TabIndex = 31
        Me.lblPayeeComments.Text = "Comments"
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Location = New System.Drawing.Point(16, 120)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountry.Size = New System.Drawing.Size(129, 17)
        Me.lblCountry.TabIndex = 30
        Me.lblCountry.Text = "Country"
        '
        'lblAccountNo
        '
        Me.lblAccountNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountNo.Location = New System.Drawing.Point(16, 96)
        Me.lblAccountNo.Name = "lblAccountNo"
        Me.lblAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountNo.Size = New System.Drawing.Size(129, 17)
        Me.lblAccountNo.TabIndex = 29
        Me.lblAccountNo.Text = "Account No."
        '
        'lblSortCode
        '
        Me.lblSortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblSortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSortCode.Location = New System.Drawing.Point(16, 72)
        Me.lblSortCode.Name = "lblSortCode"
        Me.lblSortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSortCode.Size = New System.Drawing.Size(129, 17)
        Me.lblSortCode.TabIndex = 28
        Me.lblSortCode.Text = "Sort Code"
        '
        'lblBankName
        '
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(16, 48)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(129, 17)
        Me.lblBankName.TabIndex = 27
        Me.lblBankName.Text = "Bank Name"
        '
        'lblPayeeName
        '
        Me.lblPayeeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayeeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayeeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayeeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayeeName.Location = New System.Drawing.Point(16, 24)
        Me.lblPayeeName.Name = "lblPayeeName"
        Me.lblPayeeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayeeName.Size = New System.Drawing.Size(129, 17)
        Me.lblPayeeName.TabIndex = 26
        Me.lblPayeeName.Text = "Payee Name"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(498, 374)
        Me.Controls.Add(Me.cmdAuthorise)
        Me.Controls.Add(Me.cmdReject)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Payment Method"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.FraSelectMethod.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraPaymentInformation.ResumeLayout(False)
        Me.fraPayee.ResumeLayout(False)
        Me.fraPayee.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class