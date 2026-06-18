<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMUPolicySummary
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
	Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
	Friend WithEvents txtAccountHandler As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicyStatus As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtCurrency As System.Windows.Forms.TextBox
	Friend WithEvents txtCoverTo As System.Windows.Forms.TextBox
	Friend WithEvents txtCoverFrom As System.Windows.Forms.TextBox
	Friend WithEvents txtPremiumInc As System.Windows.Forms.TextBox
	Friend WithEvents txtLeadAgent As System.Windows.Forms.TextBox
	Friend WithEvents txtProduct As System.Windows.Forms.TextBox
	Friend WithEvents lblBranch As System.Windows.Forms.Label
	Friend WithEvents lblProduct As System.Windows.Forms.Label
	Friend WithEvents lblLeadAgent As System.Windows.Forms.Label
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
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlghelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlghelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlghelpFont = New System.Windows.Forms.FontDialog
        Me.dlghelpColor = New System.Windows.Forms.ColorDialog
        Me.dlghelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraSummary = New System.Windows.Forms.GroupBox
        Me.txtBranchName = New System.Windows.Forms.TextBox
        Me.txtAccountHandler = New System.Windows.Forms.TextBox
        Me.txtPolicyStatus = New System.Windows.Forms.TextBox
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtCurrency = New System.Windows.Forms.TextBox
        Me.txtCoverTo = New System.Windows.Forms.TextBox
        Me.txtCoverFrom = New System.Windows.Forms.TextBox
        Me.txtPremiumInc = New System.Windows.Forms.TextBox
        Me.txtLeadAgent = New System.Windows.Forms.TextBox
        Me.txtProduct = New System.Windows.Forms.TextBox
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblLeadAgent = New System.Windows.Forms.Label
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
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(582, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(587, 336)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraSummary)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(579, 310)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Summary"
        '
        'fraSummary
        '
        Me.fraSummary.Controls.Add(Me.txtBranchName)
        Me.fraSummary.Controls.Add(Me.txtAccountHandler)
        Me.fraSummary.Controls.Add(Me.txtPolicyStatus)
        Me.fraSummary.Controls.Add(Me.txtPolicyNumber)
        Me.fraSummary.Controls.Add(Me.txtCurrency)
        Me.fraSummary.Controls.Add(Me.txtCoverTo)
        Me.fraSummary.Controls.Add(Me.txtCoverFrom)
        Me.fraSummary.Controls.Add(Me.txtPremiumInc)
        Me.fraSummary.Controls.Add(Me.txtLeadAgent)
        Me.fraSummary.Controls.Add(Me.txtProduct)
        Me.fraSummary.Controls.Add(Me.lblBranch)
        Me.fraSummary.Controls.Add(Me.lblProduct)
        Me.fraSummary.Controls.Add(Me.lblLeadAgent)
        Me.fraSummary.Controls.Add(Me.lblCurrency)
        Me.fraSummary.Controls.Add(Me.lblStatus)
        Me.fraSummary.Controls.Add(Me.lblPolicyNo)
        Me.fraSummary.Controls.Add(Me.lblCoverTo)
        Me.fraSummary.Controls.Add(Me.lblcoverFrom)
        Me.fraSummary.Controls.Add(Me.lblAccountHandler)
        Me.fraSummary.Controls.Add(Me.lblPremiumPayable)
        Me.fraSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSummary.Location = New System.Drawing.Point(8, 12)
        Me.fraSummary.Name = "fraSummary"
        Me.fraSummary.Size = New System.Drawing.Size(561, 209)
        Me.fraSummary.TabIndex = 1
        Me.fraSummary.TabStop = False
        '
        'txtBranchName
        '
        Me.txtBranchName.AcceptsReturn = True
        Me.txtBranchName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranchName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranchName.Enabled = False
        Me.txtBranchName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranchName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranchName.Location = New System.Drawing.Point(392, 32)
        Me.txtBranchName.MaxLength = 0
        Me.txtBranchName.Name = "txtBranchName"
        Me.txtBranchName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranchName.Size = New System.Drawing.Size(161, 21)
        Me.txtBranchName.TabIndex = 20
        Me.txtBranchName.Text = " "
        '
        'txtAccountHandler
        '
        Me.txtAccountHandler.AcceptsReturn = True
        Me.txtAccountHandler.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountHandler.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountHandler.Enabled = False
        Me.txtAccountHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountHandler.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountHandler.Location = New System.Drawing.Point(104, 128)
        Me.txtAccountHandler.MaxLength = 0
        Me.txtAccountHandler.Name = "txtAccountHandler"
        Me.txtAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountHandler.Size = New System.Drawing.Size(161, 21)
        Me.txtAccountHandler.TabIndex = 10
        '
        'txtPolicyStatus
        '
        Me.txtPolicyStatus.AcceptsReturn = True
        Me.txtPolicyStatus.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyStatus.Enabled = False
        Me.txtPolicyStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyStatus.Location = New System.Drawing.Point(392, 64)
        Me.txtPolicyStatus.MaxLength = 0
        Me.txtPolicyStatus.Name = "txtPolicyStatus"
        Me.txtPolicyStatus.ReadOnly = True
        Me.txtPolicyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyStatus.Size = New System.Drawing.Size(161, 21)
        Me.txtPolicyStatus.TabIndex = 9
        Me.txtPolicyStatus.Tag = "policy.policy_status/d"
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.HighlightText
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Enabled = False
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(104, 32)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(161, 21)
        Me.txtPolicyNumber.TabIndex = 8
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Enabled = False
        Me.txtCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(392, 96)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(161, 21)
        Me.txtCurrency.TabIndex = 7
        '
        'txtCoverTo
        '
        Me.txtCoverTo.AcceptsReturn = True
        Me.txtCoverTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverTo.Enabled = False
        Me.txtCoverTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverTo.Location = New System.Drawing.Point(392, 160)
        Me.txtCoverTo.MaxLength = 0
        Me.txtCoverTo.Name = "txtCoverTo"
        Me.txtCoverTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverTo.Size = New System.Drawing.Size(161, 21)
        Me.txtCoverTo.TabIndex = 6
        '
        'txtCoverFrom
        '
        Me.txtCoverFrom.AcceptsReturn = True
        Me.txtCoverFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtCoverFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverFrom.Enabled = False
        Me.txtCoverFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverFrom.Location = New System.Drawing.Point(104, 160)
        Me.txtCoverFrom.MaxLength = 0
        Me.txtCoverFrom.Name = "txtCoverFrom"
        Me.txtCoverFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverFrom.Size = New System.Drawing.Size(161, 21)
        Me.txtCoverFrom.TabIndex = 5
        '
        'txtPremiumInc
        '
        Me.txtPremiumInc.AcceptsReturn = True
        Me.txtPremiumInc.BackColor = System.Drawing.SystemColors.Window
        Me.txtPremiumInc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremiumInc.Enabled = False
        Me.txtPremiumInc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremiumInc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremiumInc.Location = New System.Drawing.Point(392, 128)
        Me.txtPremiumInc.MaxLength = 0
        Me.txtPremiumInc.Name = "txtPremiumInc"
        Me.txtPremiumInc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremiumInc.Size = New System.Drawing.Size(161, 21)
        Me.txtPremiumInc.TabIndex = 4
        Me.txtPremiumInc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLeadAgent
        '
        Me.txtLeadAgent.AcceptsReturn = True
        Me.txtLeadAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtLeadAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLeadAgent.Enabled = False
        Me.txtLeadAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeadAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLeadAgent.Location = New System.Drawing.Point(104, 64)
        Me.txtLeadAgent.MaxLength = 0
        Me.txtLeadAgent.Name = "txtLeadAgent"
        Me.txtLeadAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLeadAgent.Size = New System.Drawing.Size(161, 21)
        Me.txtLeadAgent.TabIndex = 3
        '
        'txtProduct
        '
        Me.txtProduct.AcceptsReturn = True
        Me.txtProduct.BackColor = System.Drawing.SystemColors.Window
        Me.txtProduct.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProduct.Enabled = False
        Me.txtProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProduct.Location = New System.Drawing.Point(104, 96)
        Me.txtProduct.MaxLength = 0
        Me.txtProduct.Name = "txtProduct"
        Me.txtProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProduct.Size = New System.Drawing.Size(161, 21)
        Me.txtProduct.TabIndex = 2
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(280, 32)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(89, 17)
        Me.lblBranch.TabIndex = 21
        Me.lblBranch.Text = "Branch:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(8, 99)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(81, 17)
        Me.lblProduct.TabIndex = 19
        Me.lblProduct.Text = "Product:"
        '
        'lblLeadAgent
        '
        Me.lblLeadAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblLeadAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLeadAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeadAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLeadAgent.Location = New System.Drawing.Point(8, 67)
        Me.lblLeadAgent.Name = "lblLeadAgent"
        Me.lblLeadAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLeadAgent.Size = New System.Drawing.Size(81, 17)
        Me.lblLeadAgent.TabIndex = 18
        Me.lblLeadAgent.Text = "Lead agent:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(280, 99)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblCurrency.TabIndex = 17
        Me.lblCurrency.Text = "Currency:"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(280, 67)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(97, 17)
        Me.lblStatus.TabIndex = 16
        Me.lblStatus.Text = "Status:"
        '
        'lblPolicyNo
        '
        Me.lblPolicyNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNo.Location = New System.Drawing.Point(8, 35)
        Me.lblPolicyNo.Name = "lblPolicyNo"
        Me.lblPolicyNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNo.Size = New System.Drawing.Size(81, 32)
        Me.lblPolicyNo.TabIndex = 15
        Me.lblPolicyNo.Text = "Policy number:"
        '
        'lblCoverTo
        '
        Me.lblCoverTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverTo.Location = New System.Drawing.Point(280, 163)
        Me.lblCoverTo.Name = "lblCoverTo"
        Me.lblCoverTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverTo.Size = New System.Drawing.Size(107, 17)
        Me.lblCoverTo.TabIndex = 14
        Me.lblCoverTo.Text = "Cover to:"
        '
        'lblcoverFrom
        '
        Me.lblcoverFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblcoverFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblcoverFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcoverFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblcoverFrom.Location = New System.Drawing.Point(8, 163)
        Me.lblcoverFrom.Name = "lblcoverFrom"
        Me.lblcoverFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblcoverFrom.Size = New System.Drawing.Size(121, 17)
        Me.lblcoverFrom.TabIndex = 13
        Me.lblcoverFrom.Text = "Cover from:"
        '
        'lblAccountHandler
        '
        Me.lblAccountHandler.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountHandler.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountHandler.Location = New System.Drawing.Point(8, 131)
        Me.lblAccountHandler.Name = "lblAccountHandler"
        Me.lblAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountHandler.Size = New System.Drawing.Size(121, 17)
        Me.lblAccountHandler.TabIndex = 12
        Me.lblAccountHandler.Text = "Acc. handler:"
        '
        'lblPremiumPayable
        '
        Me.lblPremiumPayable.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumPayable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumPayable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumPayable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumPayable.Location = New System.Drawing.Point(280, 131)
        Me.lblPremiumPayable.Name = "lblPremiumPayable"
        Me.lblPremiumPayable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumPayable.Size = New System.Drawing.Size(117, 17)
        Me.lblPremiumPayable.TabIndex = 11
        Me.lblPremiumPayable.Text = "Premium payable:"
        '
        'uctPMUPolicySummary
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMUPolicySummary"
        Me.Size = New System.Drawing.Size(604, 335)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraSummary.ResumeLayout(False)
        Me.fraSummary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class