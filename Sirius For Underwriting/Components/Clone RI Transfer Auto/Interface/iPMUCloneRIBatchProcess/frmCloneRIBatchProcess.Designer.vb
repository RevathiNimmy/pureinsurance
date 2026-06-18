<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCloneRIBatchProcess
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_renamed()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents chkClaims As System.Windows.Forms.CheckBox
	Public WithEvents chkPolicies As System.Windows.Forms.CheckBox
	Public WithEvents txtClientName As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public WithEvents lblClientName As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents fmeCurrentPolicy As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdStart As System.Windows.Forms.Button
    Public WithEvents lblDesc As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCloneRIBatchProcess))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkClaims = New System.Windows.Forms.CheckBox()
        Me.chkPolicies = New System.Windows.Forms.CheckBox()
        Me.fmeCurrentPolicy = New System.Windows.Forms.GroupBox()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.txtClientCode = New System.Windows.Forms.TextBox()
        Me.lblClientName = New System.Windows.Forms.Label()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.lblClientCode = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.sbrStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.fmeCurrentPolicy.SuspendLayout()
        Me.sbrStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkClaims
        '
        Me.chkClaims.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaims.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaims.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaims.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaims.Location = New System.Drawing.Point(12, 202)
        Me.chkClaims.Name = "chkClaims"
        Me.chkClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaims.Size = New System.Drawing.Size(111, 35)
        Me.chkClaims.TabIndex = 12
        Me.chkClaims.Text = "Process Claims"
        Me.chkClaims.UseVisualStyleBackColor = False
        '
        'chkPolicies
        '
        Me.chkPolicies.BackColor = System.Drawing.SystemColors.Control
        Me.chkPolicies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPolicies.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPolicies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPolicies.Location = New System.Drawing.Point(12, 174)
        Me.chkPolicies.Name = "chkPolicies"
        Me.chkPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPolicies.Size = New System.Drawing.Size(111, 35)
        Me.chkPolicies.TabIndex = 11
        Me.chkPolicies.Text = "Process Policies"
        Me.chkPolicies.UseVisualStyleBackColor = False
        '
        'fmeCurrentPolicy
        '
        Me.fmeCurrentPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.fmeCurrentPolicy.Controls.Add(Me.txtClientName)
        Me.fmeCurrentPolicy.Controls.Add(Me.txtPolicyNumber)
        Me.fmeCurrentPolicy.Controls.Add(Me.txtClientCode)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblClientName)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblPolicyNumber)
        Me.fmeCurrentPolicy.Controls.Add(Me.lblClientCode)
        Me.fmeCurrentPolicy.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeCurrentPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeCurrentPolicy.Location = New System.Drawing.Point(10, 51)
        Me.fmeCurrentPolicy.Name = "fmeCurrentPolicy"
        Me.fmeCurrentPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeCurrentPolicy.Size = New System.Drawing.Size(376, 121)
        Me.fmeCurrentPolicy.TabIndex = 6
        Me.fmeCurrentPolicy.TabStop = False
        Me.fmeCurrentPolicy.Text = "Current Policy"
        '
        'txtClientName
        '
        Me.txtClientName.AcceptsReturn = True
        Me.txtClientName.BackColor = System.Drawing.SystemColors.Control
        Me.txtClientName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientName.Enabled = False
        Me.txtClientName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientName.Location = New System.Drawing.Point(111, 78)
        Me.txtClientName.MaxLength = 0
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.ReadOnly = True
        Me.txtClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientName.Size = New System.Drawing.Size(249, 20)
        Me.txtClientName.TabIndex = 5
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Enabled = False
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(111, 24)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.ReadOnly = True
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(249, 20)
        Me.txtPolicyNumber.TabIndex = 1
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(111, 51)
        Me.txtClientCode.MaxLength = 0
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.ReadOnly = True
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(249, 20)
        Me.txtClientCode.TabIndex = 3
        '
        'lblClientName
        '
        Me.lblClientName.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientName.Location = New System.Drawing.Point(12, 80)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientName.Size = New System.Drawing.Size(100, 19)
        Me.lblClientName.TabIndex = 4
        Me.lblClientName.Text = "Client Name:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(12, 26)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(100, 19)
        Me.lblPolicyNumber.TabIndex = 0
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblClientCode
        '
        Me.lblClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientCode.Location = New System.Drawing.Point(12, 53)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientCode.Size = New System.Drawing.Size(100, 19)
        Me.lblClientCode.TabIndex = 2
        Me.lblClientCode.Text = "Client Code:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(310, 208)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(76, 23)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(230, 208)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(73, 23)
        Me.cmdStart.TabIndex = 7
        Me.cmdStart.Text = "&Start"
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'lblDesc
        '
        Me.lblDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc.Location = New System.Drawing.Point(10, 10)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc.Size = New System.Drawing.Size(360, 37)
        Me.lblDesc.TabIndex = 10
        Me.lblDesc.Text = "Click 'Start' to begin processing of policies with risks and claims that have Clo" & _
    "ned reinsurance."
        '
        'sbrStatus
        '
        Me.sbrStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1, Me.ToolStripStatusLabel1})
        Me.sbrStatus.Location = New System.Drawing.Point(0, 238)
        Me.sbrStatus.Name = "sbrStatus"
        Me.sbrStatus.Size = New System.Drawing.Size(395, 22)
        Me.sbrStatus.TabIndex = 13
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(0, 17)
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'frmCloneRIBatchProcess
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(395, 260)
        Me.Controls.Add(Me.sbrStatus)
        Me.Controls.Add(Me.chkClaims)
        Me.Controls.Add(Me.chkPolicies)
        Me.Controls.Add(Me.fmeCurrentPolicy)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.lblDesc)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(15, 28)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCloneRIBatchProcess"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Cloned Reinsurance Processing"
        Me.fmeCurrentPolicy.ResumeLayout(False)
        Me.fmeCurrentPolicy.PerformLayout()
        Me.sbrStatus.ResumeLayout(False)
        Me.sbrStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sbrStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
#End Region 
End Class