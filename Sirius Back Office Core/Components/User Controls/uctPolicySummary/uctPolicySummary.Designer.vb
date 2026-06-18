<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPolicySummControl
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public dlghelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlghelpSave As System.Windows.Forms.SaveFileDialog
	Public dlghelpFont As System.Windows.Forms.FontDialog
	Public dlghelpColor As System.Windows.Forms.ColorDialog
	Public dlghelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents txtAccountExecutive As System.Windows.Forms.TextBox
	Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
	Friend WithEvents txtAccountHandler As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicyStatus As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtCoverTo As System.Windows.Forms.TextBox
	Friend WithEvents txtCoverFrom As System.Windows.Forms.TextBox
	Friend WithEvents txtPremiumInc As System.Windows.Forms.TextBox
	Friend WithEvents txtInsurer As System.Windows.Forms.TextBox
	Friend WithEvents txtRisk As System.Windows.Forms.TextBox
	Friend WithEvents lblAccountExecutive As System.Windows.Forms.Label
	Friend WithEvents lblBranch As System.Windows.Forms.Label
	Friend WithEvents lblRisk As System.Windows.Forms.Label
	Friend WithEvents lblInsurer As System.Windows.Forms.Label
	Friend WithEvents lblCurrency As System.Windows.Forms.Label
	Friend WithEvents lblStatus As System.Windows.Forms.Label
	Friend WithEvents lblPolicyNo As System.Windows.Forms.Label
	Friend WithEvents lblCoverTo As System.Windows.Forms.Label
	Friend WithEvents lblcoverFrom As System.Windows.Forms.Label
	Friend WithEvents lblAccountHandler As System.Windows.Forms.Label
	Friend WithEvents lblPremiumPayable As System.Windows.Forms.Label
	Friend WithEvents fraSummary As System.Windows.Forms.GroupBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPolicySummControl))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlghelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlghelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlghelpFont = New System.Windows.Forms.FontDialog
		Me.dlghelpColor = New System.Windows.Forms.ColorDialog
		Me.dlghelpPrint = New System.Windows.Forms.PrintDialog
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.fraSummary = New System.Windows.Forms.GroupBox
		Me.txtAccountExecutive = New System.Windows.Forms.TextBox
		Me.txtBranchName = New System.Windows.Forms.TextBox
		Me.txtAccountHandler = New System.Windows.Forms.TextBox
		Me.txtPolicyStatus = New System.Windows.Forms.TextBox
		Me.txtPolicyNumber = New System.Windows.Forms.TextBox
		Me.txtCurrency = New System.Windows.Forms.TextBox
		Me.txtCoverTo = New System.Windows.Forms.TextBox
		Me.txtCoverFrom = New System.Windows.Forms.TextBox
		Me.txtPremiumInc = New System.Windows.Forms.TextBox
		Me.txtInsurer = New System.Windows.Forms.TextBox
		Me.txtRisk = New System.Windows.Forms.TextBox
		Me.lblAccountExecutive = New System.Windows.Forms.Label
		Me.lblBranch = New System.Windows.Forms.Label
		Me.lblRisk = New System.Windows.Forms.Label
		Me.lblInsurer = New System.Windows.Forms.Label
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblStatus = New System.Windows.Forms.Label
		Me.lblPolicyNo = New System.Windows.Forms.Label
		Me.lblCoverTo = New System.Windows.Forms.Label
		Me.lblcoverFrom = New System.Windows.Forms.Label
		Me.lblAccountHandler = New System.Windows.Forms.Label
		Me.lblPremiumPayable = New System.Windows.Forms.Label
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.fraSummary.SuspendLayout()
		Me.SuspendLayout()
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(605, 305)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 0
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraSummary)
		Me._tabMainTab_TabPage0.Text = "&1 - Summary"
		' 
		' fraSummary
		' 
		Me.fraSummary.Controls.Add(Me.txtAccountExecutive)
		Me.fraSummary.Controls.Add(Me.txtBranchName)
		Me.fraSummary.Controls.Add(Me.txtAccountHandler)
		Me.fraSummary.Controls.Add(Me.txtPolicyStatus)
		Me.fraSummary.Controls.Add(Me.txtPolicyNumber)
		Me.fraSummary.Controls.Add(Me.txtCurrency)
		Me.fraSummary.Controls.Add(Me.txtCoverTo)
		Me.fraSummary.Controls.Add(Me.txtCoverFrom)
		Me.fraSummary.Controls.Add(Me.txtPremiumInc)
		Me.fraSummary.Controls.Add(Me.txtInsurer)
		Me.fraSummary.Controls.Add(Me.txtRisk)
		Me.fraSummary.Controls.Add(Me.lblAccountExecutive)
		Me.fraSummary.Controls.Add(Me.lblBranch)
		Me.fraSummary.Controls.Add(Me.lblRisk)
		Me.fraSummary.Controls.Add(Me.lblInsurer)
		Me.fraSummary.Controls.Add(Me.lblCurrency)
		Me.fraSummary.Controls.Add(Me.lblStatus)
		Me.fraSummary.Controls.Add(Me.lblPolicyNo)
		Me.fraSummary.Controls.Add(Me.lblCoverTo)
		Me.fraSummary.Controls.Add(Me.lblcoverFrom)
		Me.fraSummary.Controls.Add(Me.lblAccountHandler)
		Me.fraSummary.Controls.Add(Me.lblPremiumPayable)
		Me.fraSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraSummary.Location = New System.Drawing.Point(16, 12)
		Me.fraSummary.Name = "fraSummary"
		Me.fraSummary.Size = New System.Drawing.Size(569, 233)
		Me.fraSummary.TabIndex = 1
		' 
		' txtAccountExecutive
		' 
		Me.txtAccountExecutive.AcceptsReturn = True
		Me.txtAccountExecutive.AutoSize = False
		Me.txtAccountExecutive.BackColor = System.Drawing.SystemColors.Window
		Me.txtAccountExecutive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAccountExecutive.CausesValidation = True
		Me.txtAccountExecutive.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAccountExecutive.Enabled = False
		Me.txtAccountExecutive.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAccountExecutive.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAccountExecutive.HideSelection = True
		Me.txtAccountExecutive.Location = New System.Drawing.Point(104, 156)
		Me.txtAccountExecutive.MaxLength = 0
		Me.txtAccountExecutive.Multiline = False
		Me.txtAccountExecutive.Name = "txtAccountExecutive"
		Me.txtAccountExecutive.ReadOnly = False
		Me.txtAccountExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAccountExecutive.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAccountExecutive.Size = New System.Drawing.Size(449, 19)
		Me.txtAccountExecutive.TabIndex = 23
		Me.txtAccountExecutive.TabStop = True
		Me.txtAccountExecutive.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAccountExecutive.Visible = True
		' 
		' txtBranchName
		' 
		Me.txtBranchName.AcceptsReturn = True
		Me.txtBranchName.AutoSize = False
		Me.txtBranchName.BackColor = System.Drawing.SystemColors.Window
		Me.txtBranchName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBranchName.CausesValidation = True
		Me.txtBranchName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBranchName.Enabled = False
		Me.txtBranchName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBranchName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBranchName.HideSelection = True
		Me.txtBranchName.Location = New System.Drawing.Point(392, 32)
		Me.txtBranchName.MaxLength = 0
		Me.txtBranchName.Multiline = False
		Me.txtBranchName.Name = "txtBranchName"
		Me.txtBranchName.ReadOnly = False
		Me.txtBranchName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBranchName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBranchName.Size = New System.Drawing.Size(161, 19)
		Me.txtBranchName.TabIndex = 20
		Me.txtBranchName.TabStop = True
		Me.txtBranchName.Text = " "
		Me.txtBranchName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBranchName.Visible = True
		' 
		' txtAccountHandler
		' 
		Me.txtAccountHandler.AcceptsReturn = True
		Me.txtAccountHandler.AutoSize = False
		Me.txtAccountHandler.BackColor = System.Drawing.SystemColors.Window
		Me.txtAccountHandler.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAccountHandler.CausesValidation = True
		Me.txtAccountHandler.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAccountHandler.Enabled = False
		Me.txtAccountHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAccountHandler.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAccountHandler.HideSelection = True
		Me.txtAccountHandler.Location = New System.Drawing.Point(104, 128)
		Me.txtAccountHandler.MaxLength = 0
		Me.txtAccountHandler.Multiline = False
		Me.txtAccountHandler.Name = "txtAccountHandler"
		Me.txtAccountHandler.ReadOnly = False
		Me.txtAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAccountHandler.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAccountHandler.Size = New System.Drawing.Size(161, 19)
		Me.txtAccountHandler.TabIndex = 10
		Me.txtAccountHandler.TabStop = True
		Me.txtAccountHandler.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAccountHandler.Visible = True
		' 
		' txtPolicyStatus
		' 
		Me.txtPolicyStatus.AcceptsReturn = True
		Me.txtPolicyStatus.AutoSize = False
		Me.txtPolicyStatus.BackColor = System.Drawing.SystemColors.Window
		Me.txtPolicyStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPolicyStatus.CausesValidation = True
		Me.txtPolicyStatus.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPolicyStatus.Enabled = False
		Me.txtPolicyStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPolicyStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPolicyStatus.HideSelection = True
		Me.txtPolicyStatus.Location = New System.Drawing.Point(392, 64)
		Me.txtPolicyStatus.MaxLength = 0
		Me.txtPolicyStatus.Multiline = False
		Me.txtPolicyStatus.Name = "txtPolicyStatus"
		Me.txtPolicyStatus.ReadOnly = True
		Me.txtPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPolicyStatus.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPolicyStatus.Size = New System.Drawing.Size(161, 19)
		Me.txtPolicyStatus.TabIndex = 9
		Me.txtPolicyStatus.TabStop = True
		Me.txtPolicyStatus.Tag = "policy.policy_status/d"
		Me.txtPolicyStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPolicyStatus.Visible = True
		' 
		' txtPolicyNumber
		' 
		Me.txtPolicyNumber.AcceptsReturn = True
		Me.txtPolicyNumber.AutoSize = False
		Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.HighlightText
		Me.txtPolicyNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPolicyNumber.CausesValidation = True
		Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPolicyNumber.Enabled = False
		Me.txtPolicyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPolicyNumber.HideSelection = True
		Me.txtPolicyNumber.Location = New System.Drawing.Point(104, 32)
		Me.txtPolicyNumber.MaxLength = 0
		Me.txtPolicyNumber.Multiline = False
		Me.txtPolicyNumber.Name = "txtPolicyNumber"
		Me.txtPolicyNumber.ReadOnly = False
		Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPolicyNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPolicyNumber.Size = New System.Drawing.Size(161, 19)
		Me.txtPolicyNumber.TabIndex = 8
		Me.txtPolicyNumber.TabStop = True
		Me.txtPolicyNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPolicyNumber.Visible = True
		' 
		' txtCurrency
		' 
		Me.txtCurrency.AcceptsReturn = True
		Me.txtCurrency.AutoSize = False
		Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrency.CausesValidation = True
		Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrency.Enabled = False
		Me.txtCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrency.HideSelection = True
		Me.txtCurrency.Location = New System.Drawing.Point(392, 96)
		Me.txtCurrency.MaxLength = 0
		Me.txtCurrency.Multiline = False
		Me.txtCurrency.Name = "txtCurrency"
		Me.txtCurrency.ReadOnly = False
		Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrency.Size = New System.Drawing.Size(161, 19)
		Me.txtCurrency.TabIndex = 7
		Me.txtCurrency.TabStop = True
		Me.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrency.Visible = True
		' 
		' txtCoverTo
		' 
		Me.txtCoverTo.AcceptsReturn = True
		Me.txtCoverTo.AutoSize = False
		Me.txtCoverTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtCoverTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCoverTo.CausesValidation = True
		Me.txtCoverTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCoverTo.Enabled = False
		Me.txtCoverTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCoverTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCoverTo.HideSelection = True
		Me.txtCoverTo.Location = New System.Drawing.Point(392, 184)
		Me.txtCoverTo.MaxLength = 0
		Me.txtCoverTo.Multiline = False
		Me.txtCoverTo.Name = "txtCoverTo"
		Me.txtCoverTo.ReadOnly = False
		Me.txtCoverTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCoverTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCoverTo.Size = New System.Drawing.Size(161, 19)
		Me.txtCoverTo.TabIndex = 6
		Me.txtCoverTo.TabStop = True
		Me.txtCoverTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCoverTo.Visible = True
		' 
		' txtCoverFrom
		' 
		Me.txtCoverFrom.AcceptsReturn = True
		Me.txtCoverFrom.AutoSize = False
		Me.txtCoverFrom.BackColor = System.Drawing.SystemColors.Window
		Me.txtCoverFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCoverFrom.CausesValidation = True
		Me.txtCoverFrom.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCoverFrom.Enabled = False
		Me.txtCoverFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCoverFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCoverFrom.HideSelection = True
		Me.txtCoverFrom.Location = New System.Drawing.Point(104, 184)
		Me.txtCoverFrom.MaxLength = 0
		Me.txtCoverFrom.Multiline = False
		Me.txtCoverFrom.Name = "txtCoverFrom"
		Me.txtCoverFrom.ReadOnly = False
		Me.txtCoverFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCoverFrom.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCoverFrom.Size = New System.Drawing.Size(161, 19)
		Me.txtCoverFrom.TabIndex = 5
		Me.txtCoverFrom.TabStop = True
		Me.txtCoverFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCoverFrom.Visible = True
		' 
		' txtPremiumInc
		' 
		Me.txtPremiumInc.AcceptsReturn = True
		Me.txtPremiumInc.AutoSize = False
		Me.txtPremiumInc.BackColor = System.Drawing.SystemColors.Window
		Me.txtPremiumInc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPremiumInc.CausesValidation = True
		Me.txtPremiumInc.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPremiumInc.Enabled = False
		Me.txtPremiumInc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPremiumInc.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPremiumInc.HideSelection = True
		Me.txtPremiumInc.Location = New System.Drawing.Point(392, 128)
		Me.txtPremiumInc.MaxLength = 0
		Me.txtPremiumInc.Multiline = False
		Me.txtPremiumInc.Name = "txtPremiumInc"
		Me.txtPremiumInc.ReadOnly = False
		Me.txtPremiumInc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPremiumInc.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPremiumInc.Size = New System.Drawing.Size(161, 19)
		Me.txtPremiumInc.TabIndex = 4
		Me.txtPremiumInc.TabStop = True
		Me.txtPremiumInc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtPremiumInc.Visible = True
		' 
		' txtInsurer
		' 
		Me.txtInsurer.AcceptsReturn = True
		Me.txtInsurer.AutoSize = False
		Me.txtInsurer.BackColor = System.Drawing.SystemColors.Window
		Me.txtInsurer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInsurer.CausesValidation = True
		Me.txtInsurer.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInsurer.Enabled = False
		Me.txtInsurer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInsurer.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInsurer.HideSelection = True
		Me.txtInsurer.Location = New System.Drawing.Point(104, 64)
		Me.txtInsurer.MaxLength = 0
		Me.txtInsurer.Multiline = False
		Me.txtInsurer.Name = "txtInsurer"
		Me.txtInsurer.ReadOnly = False
		Me.txtInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInsurer.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInsurer.Size = New System.Drawing.Size(161, 19)
		Me.txtInsurer.TabIndex = 3
		Me.txtInsurer.TabStop = True
		Me.txtInsurer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInsurer.Visible = True
		' 
		' txtRisk
		' 
		Me.txtRisk.AcceptsReturn = True
		Me.txtRisk.AutoSize = False
		Me.txtRisk.BackColor = System.Drawing.SystemColors.Window
		Me.txtRisk.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRisk.CausesValidation = True
		Me.txtRisk.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRisk.Enabled = False
		Me.txtRisk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRisk.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRisk.HideSelection = True
		Me.txtRisk.Location = New System.Drawing.Point(104, 96)
		Me.txtRisk.MaxLength = 0
		Me.txtRisk.Multiline = False
		Me.txtRisk.Name = "txtRisk"
		Me.txtRisk.ReadOnly = False
		Me.txtRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRisk.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRisk.Size = New System.Drawing.Size(161, 19)
		Me.txtRisk.TabIndex = 2
		Me.txtRisk.TabStop = True
		Me.txtRisk.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRisk.Visible = True
		' 
		' lblAccountExecutive
		' 
		Me.lblAccountExecutive.AutoSize = False
		Me.lblAccountExecutive.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccountExecutive.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccountExecutive.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccountExecutive.Enabled = True
		Me.lblAccountExecutive.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccountExecutive.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccountExecutive.Location = New System.Drawing.Point(8, 159)
		Me.lblAccountExecutive.Name = "lblAccountExecutive"
		Me.lblAccountExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccountExecutive.Size = New System.Drawing.Size(89, 17)
		Me.lblAccountExecutive.TabIndex = 22
		Me.lblAccountExecutive.Text = "Acc. Executive"
		Me.lblAccountExecutive.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccountExecutive.UseMnemonic = True
		Me.lblAccountExecutive.Visible = True
		' 
		' lblBranch
		' 
		Me.lblBranch.AutoSize = False
		Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
		Me.lblBranch.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBranch.Enabled = True
		Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBranch.Location = New System.Drawing.Point(280, 32)
		Me.lblBranch.Name = "lblBranch"
		Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBranch.Size = New System.Drawing.Size(89, 17)
		Me.lblBranch.TabIndex = 21
		Me.lblBranch.Text = "Branch:"
		Me.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBranch.UseMnemonic = True
		Me.lblBranch.Visible = True
		' 
		' lblRisk
		' 
		Me.lblRisk.AutoSize = False
		Me.lblRisk.BackColor = System.Drawing.SystemColors.Control
		Me.lblRisk.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRisk.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRisk.Enabled = True
		Me.lblRisk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRisk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRisk.Location = New System.Drawing.Point(8, 99)
		Me.lblRisk.Name = "lblRisk"
		Me.lblRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRisk.Size = New System.Drawing.Size(81, 17)
		Me.lblRisk.TabIndex = 19
		Me.lblRisk.Text = "Risk:"
		Me.lblRisk.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRisk.UseMnemonic = True
		Me.lblRisk.Visible = True
		' 
		' lblInsurer
		' 
		Me.lblInsurer.AutoSize = False
		Me.lblInsurer.BackColor = System.Drawing.SystemColors.Control
		Me.lblInsurer.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInsurer.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInsurer.Enabled = True
		Me.lblInsurer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInsurer.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInsurer.Location = New System.Drawing.Point(8, 67)
		Me.lblInsurer.Name = "lblInsurer"
		Me.lblInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInsurer.Size = New System.Drawing.Size(81, 17)
		Me.lblInsurer.TabIndex = 18
		Me.lblInsurer.Text = "Insurer:"
		Me.lblInsurer.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInsurer.UseMnemonic = True
		Me.lblInsurer.Visible = True
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(280, 99)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(73, 17)
		Me.lblCurrency.TabIndex = 17
		Me.lblCurrency.Text = "Currency:"
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(280, 67)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(97, 17)
		Me.lblStatus.TabIndex = 16
		Me.lblStatus.Text = "Status:"
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' lblPolicyNo
		' 
		Me.lblPolicyNo.AutoSize = False
		Me.lblPolicyNo.BackColor = System.Drawing.SystemColors.Control
		Me.lblPolicyNo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPolicyNo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPolicyNo.Enabled = True
		Me.lblPolicyNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPolicyNo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPolicyNo.Location = New System.Drawing.Point(8, 35)
		Me.lblPolicyNo.Name = "lblPolicyNo"
		Me.lblPolicyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPolicyNo.Size = New System.Drawing.Size(89, 17)
		Me.lblPolicyNo.TabIndex = 15
		Me.lblPolicyNo.Text = "Policy number:"
		Me.lblPolicyNo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPolicyNo.UseMnemonic = True
		Me.lblPolicyNo.Visible = True
		' 
		' lblCoverTo
		' 
		Me.lblCoverTo.AutoSize = False
		Me.lblCoverTo.BackColor = System.Drawing.SystemColors.Control
		Me.lblCoverTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCoverTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCoverTo.Enabled = True
		Me.lblCoverTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCoverTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCoverTo.Location = New System.Drawing.Point(280, 187)
		Me.lblCoverTo.Name = "lblCoverTo"
		Me.lblCoverTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCoverTo.Size = New System.Drawing.Size(73, 17)
		Me.lblCoverTo.TabIndex = 14
		Me.lblCoverTo.Text = "Cover to:"
		Me.lblCoverTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCoverTo.UseMnemonic = True
		Me.lblCoverTo.Visible = True
		' 
		' lblcoverFrom
		' 
		Me.lblcoverFrom.AutoSize = False
		Me.lblcoverFrom.BackColor = System.Drawing.SystemColors.Control
		Me.lblcoverFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblcoverFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblcoverFrom.Enabled = True
		Me.lblcoverFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblcoverFrom.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblcoverFrom.Location = New System.Drawing.Point(8, 187)
		Me.lblcoverFrom.Name = "lblcoverFrom"
		Me.lblcoverFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblcoverFrom.Size = New System.Drawing.Size(121, 17)
		Me.lblcoverFrom.TabIndex = 13
		Me.lblcoverFrom.Text = "Cover from:"
		Me.lblcoverFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblcoverFrom.UseMnemonic = True
		Me.lblcoverFrom.Visible = True
		' 
		' lblAccountHandler
		' 
		Me.lblAccountHandler.AutoSize = False
		Me.lblAccountHandler.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccountHandler.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccountHandler.Enabled = True
		Me.lblAccountHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccountHandler.Location = New System.Drawing.Point(8, 131)
		Me.lblAccountHandler.Name = "lblAccountHandler"
		Me.lblAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccountHandler.Size = New System.Drawing.Size(121, 17)
		Me.lblAccountHandler.TabIndex = 12
		Me.lblAccountHandler.Text = "Acc. Handler:"
		Me.lblAccountHandler.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccountHandler.UseMnemonic = True
		Me.lblAccountHandler.Visible = True
		' 
		' lblPremiumPayable
		' 
		Me.lblPremiumPayable.AutoSize = False
		Me.lblPremiumPayable.BackColor = System.Drawing.SystemColors.Control
		Me.lblPremiumPayable.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPremiumPayable.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPremiumPayable.Enabled = True
		Me.lblPremiumPayable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPremiumPayable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPremiumPayable.Location = New System.Drawing.Point(280, 131)
		Me.lblPremiumPayable.Name = "lblPremiumPayable"
		Me.lblPremiumPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPremiumPayable.Size = New System.Drawing.Size(117, 17)
		Me.lblPremiumPayable.TabIndex = 11
		Me.lblPremiumPayable.Text = "Premium payable:"
		Me.lblPremiumPayable.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPremiumPayable.UseMnemonic = True
		Me.lblPremiumPayable.Visible = True
		' 
		' uctPolicySummControl
		' 
		Me.ClientSize = New System.Drawing.Size(604, 305)
		Me.Controls.Add(Me.tabMainTab)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctPolicySummControl"
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.fraSummary.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class