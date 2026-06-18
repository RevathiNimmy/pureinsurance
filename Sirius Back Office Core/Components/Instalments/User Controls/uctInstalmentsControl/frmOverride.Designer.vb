<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOverride
#Region "Windows Form Designer generated code "
	Friend Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents chkPaymentProtection As System.Windows.Forms.CheckBox
	Public WithEvents fraAdditionalOptions As System.Windows.Forms.GroupBox
	Public WithEvents chkOverrideInterestRate As System.Windows.Forms.CheckBox
	Public WithEvents txtNewRate As System.Windows.Forms.TextBox
	Public WithEvents chkCommissionOverride As System.Windows.Forms.CheckBox
	Public WithEvents txtOverrideReference As System.Windows.Forms.TextBox
	Public WithEvents chkDepositOverride As System.Windows.Forms.CheckBox
	Public WithEvents txtOverrideDeposit As System.Windows.Forms.TextBox
	Public WithEvents lblNewInterestRate As System.Windows.Forms.Label
	Public WithEvents lblReference As System.Windows.Forms.Label
	Public WithEvents lblDepositOverride As System.Windows.Forms.Label
	Public WithEvents fraOverrideOptions As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.fraAdditionalOptions = New System.Windows.Forms.GroupBox()
        Me.chkPaymentProtection = New System.Windows.Forms.CheckBox()
        Me.fraOverrideOptions = New System.Windows.Forms.GroupBox()
        Me.chkOverrideInterestRate = New System.Windows.Forms.CheckBox()
        Me.txtNewRate = New System.Windows.Forms.TextBox()
        Me.chkCommissionOverride = New System.Windows.Forms.CheckBox()
        Me.txtOverrideReference = New System.Windows.Forms.TextBox()
        Me.chkDepositOverride = New System.Windows.Forms.CheckBox()
        Me.txtOverrideDeposit = New System.Windows.Forms.TextBox()
        Me.lblNewInterestRate = New System.Windows.Forms.Label()
        Me.lblReference = New System.Windows.Forms.Label()
        Me.lblDepositOverride = New System.Windows.Forms.Label()
        Me.fraAdditionalOptions.SuspendLayout()
        Me.fraOverrideOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(128, 304)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(89, 25)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(224, 304)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'fraAdditionalOptions
        '
        Me.fraAdditionalOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAdditionalOptions.Controls.Add(Me.chkPaymentProtection)
        Me.fraAdditionalOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAdditionalOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAdditionalOptions.Location = New System.Drawing.Point(8, 232)
        Me.fraAdditionalOptions.Name = "fraAdditionalOptions"
        Me.fraAdditionalOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAdditionalOptions.Size = New System.Drawing.Size(305, 57)
        Me.fraAdditionalOptions.TabIndex = 13
        Me.fraAdditionalOptions.TabStop = False
        Me.fraAdditionalOptions.Tag = "CAP;323"
        Me.fraAdditionalOptions.Text = "Additional Options"
        '
        'chkPaymentProtection
        '
        Me.chkPaymentProtection.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaymentProtection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaymentProtection.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaymentProtection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaymentProtection.Location = New System.Drawing.Point(16, 24)
        Me.chkPaymentProtection.Name = "chkPaymentProtection"
        Me.chkPaymentProtection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaymentProtection.Size = New System.Drawing.Size(169, 17)
        Me.chkPaymentProtection.TabIndex = 7
        Me.chkPaymentProtection.Tag = "CAP;328"
        Me.chkPaymentProtection.Text = "Payment Protection"
        Me.chkPaymentProtection.UseVisualStyleBackColor = False
        '
        'fraOverrideOptions
        '
        Me.fraOverrideOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraOverrideOptions.Controls.Add(Me.chkOverrideInterestRate)
        Me.fraOverrideOptions.Controls.Add(Me.txtNewRate)
        Me.fraOverrideOptions.Controls.Add(Me.chkCommissionOverride)
        Me.fraOverrideOptions.Controls.Add(Me.txtOverrideReference)
        Me.fraOverrideOptions.Controls.Add(Me.chkDepositOverride)
        Me.fraOverrideOptions.Controls.Add(Me.txtOverrideDeposit)
        Me.fraOverrideOptions.Controls.Add(Me.lblNewInterestRate)
        Me.fraOverrideOptions.Controls.Add(Me.lblReference)
        Me.fraOverrideOptions.Controls.Add(Me.lblDepositOverride)
        Me.fraOverrideOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOverrideOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOverrideOptions.Location = New System.Drawing.Point(8, 8)
        Me.fraOverrideOptions.Name = "fraOverrideOptions"
        Me.fraOverrideOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOverrideOptions.Size = New System.Drawing.Size(305, 217)
        Me.fraOverrideOptions.TabIndex = 0
        Me.fraOverrideOptions.TabStop = False
        Me.fraOverrideOptions.Tag = "CAP;322"
        Me.fraOverrideOptions.Text = "Override Options"
        '
        'chkOverrideInterestRate
        '
        Me.chkOverrideInterestRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideInterestRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideInterestRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideInterestRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideInterestRate.Location = New System.Drawing.Point(16, 24)
        Me.chkOverrideInterestRate.Name = "chkOverrideInterestRate"
        Me.chkOverrideInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideInterestRate.Size = New System.Drawing.Size(193, 26)
        Me.chkOverrideInterestRate.TabIndex = 1
        Me.chkOverrideInterestRate.Tag = "CAP;324"
        Me.chkOverrideInterestRate.Text = "Interest Rate Override (%)"
        Me.chkOverrideInterestRate.UseVisualStyleBackColor = False
        '
        'txtNewRate
        '
        Me.txtNewRate.AcceptsReturn = True
        Me.txtNewRate.BackColor = System.Drawing.SystemColors.Control
        Me.txtNewRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewRate.Enabled = False
        Me.txtNewRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewRate.Location = New System.Drawing.Point(112, 56)
        Me.txtNewRate.MaxLength = 0
        Me.txtNewRate.Name = "txtNewRate"
        Me.txtNewRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewRate.Size = New System.Drawing.Size(89, 19)
        Me.txtNewRate.TabIndex = 2
        Me.txtNewRate.Tag = "F;"
        Me.txtNewRate.Text = " "
        '
        'chkCommissionOverride
        '
        Me.chkCommissionOverride.BackColor = System.Drawing.SystemColors.Control
        Me.chkCommissionOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCommissionOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCommissionOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCommissionOverride.Location = New System.Drawing.Point(16, 88)
        Me.chkCommissionOverride.Name = "chkCommissionOverride"
        Me.chkCommissionOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCommissionOverride.Size = New System.Drawing.Size(169, 17)
        Me.chkCommissionOverride.TabIndex = 3
        Me.chkCommissionOverride.Tag = "CAP;326"
        Me.chkCommissionOverride.Text = "Commission Override"
        Me.chkCommissionOverride.UseVisualStyleBackColor = False
        '
        'txtOverrideReference
        '
        Me.txtOverrideReference.AcceptsReturn = True
        Me.txtOverrideReference.BackColor = System.Drawing.SystemColors.Control
        Me.txtOverrideReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideReference.Enabled = False
        Me.txtOverrideReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideReference.Location = New System.Drawing.Point(112, 120)
        Me.txtOverrideReference.MaxLength = 0
        Me.txtOverrideReference.Name = "txtOverrideReference"
        Me.txtOverrideReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideReference.Size = New System.Drawing.Size(89, 19)
        Me.txtOverrideReference.TabIndex = 4
        Me.txtOverrideReference.Tag = "F;"
        Me.txtOverrideReference.Text = " "
        '
        'chkDepositOverride
        '
        Me.chkDepositOverride.BackColor = System.Drawing.SystemColors.Control
        Me.chkDepositOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDepositOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDepositOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDepositOverride.Location = New System.Drawing.Point(16, 152)
        Me.chkDepositOverride.Name = "chkDepositOverride"
        Me.chkDepositOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDepositOverride.Size = New System.Drawing.Size(153, 17)
        Me.chkDepositOverride.TabIndex = 5
        Me.chkDepositOverride.Text = "Deposit Override"
        Me.chkDepositOverride.UseVisualStyleBackColor = False
        '
        'txtOverrideDeposit
        '
        Me.txtOverrideDeposit.AcceptsReturn = True
        Me.txtOverrideDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.txtOverrideDeposit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverrideDeposit.Enabled = False
        Me.txtOverrideDeposit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOverrideDeposit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOverrideDeposit.Location = New System.Drawing.Point(112, 176)
        Me.txtOverrideDeposit.MaxLength = 0
        Me.txtOverrideDeposit.Name = "txtOverrideDeposit"
        Me.txtOverrideDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOverrideDeposit.Size = New System.Drawing.Size(89, 19)
        Me.txtOverrideDeposit.TabIndex = 6
        Me.txtOverrideDeposit.Text = " "
        '
        'lblNewInterestRate
        '
        Me.lblNewInterestRate.AutoSize = True
        Me.lblNewInterestRate.BackColor = System.Drawing.Color.Transparent
        Me.lblNewInterestRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewInterestRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewInterestRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewInterestRate.Location = New System.Drawing.Point(40, 56)
        Me.lblNewInterestRate.Name = "lblNewInterestRate"
        Me.lblNewInterestRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewInterestRate.Size = New System.Drawing.Size(66, 13)
        Me.lblNewInterestRate.TabIndex = 12
        Me.lblNewInterestRate.Tag = "CAP;325"
        Me.lblNewInterestRate.Text = "New Rate:"
        '
        'lblReference
        '
        Me.lblReference.AutoSize = True
        Me.lblReference.BackColor = System.Drawing.Color.Transparent
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(40, 120)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(70, 13)
        Me.lblReference.TabIndex = 11
        Me.lblReference.Tag = "CAP;327"
        Me.lblReference.Text = "Reference:"
        '
        'lblDepositOverride
        '
        Me.lblDepositOverride.AutoSize = True
        Me.lblDepositOverride.BackColor = System.Drawing.Color.Transparent
        Me.lblDepositOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepositOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepositOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepositOverride.Location = New System.Drawing.Point(40, 176)
        Me.lblDepositOverride.Name = "lblDepositOverride"
        Me.lblDepositOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepositOverride.Size = New System.Drawing.Size(59, 13)
        Me.lblDepositOverride.TabIndex = 10
        Me.lblDepositOverride.Text = "Deposit: "
        '
        'frmOverride
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(322, 338)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraAdditionalOptions)
        Me.Controls.Add(Me.fraOverrideOptions)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 24)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOverride"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Override Quote"
        Me.fraAdditionalOptions.ResumeLayout(False)
        Me.fraOverrideOptions.ResumeLayout(False)
        Me.fraOverrideOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class