<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDeferredRIAuto
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtClientName As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public WithEvents lblClientName As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents fmeCurrentPolicy As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdStart As System.Windows.Forms.Button
	Private WithEvents _sbrStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents sbrStatus As System.Windows.Forms.StatusStrip
	Public WithEvents lblDesc As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDeferredRIAuto))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fmeCurrentPolicy = New System.Windows.Forms.GroupBox
        Me.txtClientName = New System.Windows.Forms.TextBox
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.lblClientName = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblClientCode = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdStart = New System.Windows.Forms.Button
        Me.sbrStatus = New System.Windows.Forms.StatusStrip
        Me._sbrStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblDesc = New System.Windows.Forms.Label
        Me.fmeCurrentPolicy.SuspendLayout()
        Me.sbrStatus.SuspendLayout()
        Me.SuspendLayout()
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
        Me.fmeCurrentPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.lblClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.lblClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(310, 186)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(76, 23)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(230, 186)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(73, 23)
        Me.cmdStart.TabIndex = 7
        Me.cmdStart.Text = "&Start"
        Me.cmdStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'sbrStatus
        '
        Me.sbrStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sbrStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._sbrStatus_Panel1})
        Me.sbrStatus.Location = New System.Drawing.Point(0, 212)
        Me.sbrStatus.Name = "sbrStatus"
        Me.sbrStatus.ShowItemToolTips = True
        Me.sbrStatus.Size = New System.Drawing.Size(395, 22)
        Me.sbrStatus.SizingGrip = False
        Me.sbrStatus.TabIndex = 9
        '
        '_sbrStatus_Panel1
        '
        Me._sbrStatus_Panel1.AutoSize = False
        Me._sbrStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._sbrStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._sbrStatus_Panel1.DoubleClickEnabled = True
        Me._sbrStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._sbrStatus_Panel1.Name = "_sbrStatus_Panel1"
        Me._sbrStatus_Panel1.Size = New System.Drawing.Size(395, 22)
        Me._sbrStatus_Panel1.Text = "No risks found for processing"
        Me._sbrStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDesc
        '
        Me.lblDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc.Location = New System.Drawing.Point(10, 10)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc.Size = New System.Drawing.Size(360, 37)
        Me.lblDesc.TabIndex = 10
        Me.lblDesc.Text = "Click 'Start' to begin processing of policies with risks that have deferred reins" & _
            "urance."
        '
        'frmDeferredRIAuto
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(395, 234)
        Me.Controls.Add(Me.fmeCurrentPolicy)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.sbrStatus)
        Me.Controls.Add(Me.lblDesc)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(15, 28)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDeferredRIAuto"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Deferred Reinsurance Processing"
        Me.fmeCurrentPolicy.ResumeLayout(False)
        Me.fmeCurrentPolicy.PerformLayout()
        Me.sbrStatus.ResumeLayout(False)
        Me.sbrStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class