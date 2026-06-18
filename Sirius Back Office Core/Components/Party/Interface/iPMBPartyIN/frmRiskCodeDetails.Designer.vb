<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRiskCodeDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
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
	Public WithEvents chkDelegatedAuthority As System.Windows.Forms.CheckBox
	Public WithEvents chkRiskTransferAgreement As System.Windows.Forms.CheckBox
	Public WithEvents fraOptions As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRiskCodeDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraOptions = New System.Windows.Forms.GroupBox
		Me.chkDelegatedAuthority = New System.Windows.Forms.CheckBox
		Me.chkRiskTransferAgreement = New System.Windows.Forms.CheckBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.fraOptions.SuspendLayout()
		Me.SuspendLayout()
		' 
		' fraOptions
		' 
		Me.fraOptions.BackColor = System.Drawing.SystemColors.Control
		Me.fraOptions.Controls.Add(Me.chkDelegatedAuthority)
		Me.fraOptions.Controls.Add(Me.chkRiskTransferAgreement)
		Me.fraOptions.Enabled = True
		Me.fraOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraOptions.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraOptions.Location = New System.Drawing.Point(0, 8)
		Me.fraOptions.Name = "fraOptions"
		Me.fraOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraOptions.Size = New System.Drawing.Size(313, 97)
		Me.fraOptions.TabIndex = 2
		Me.fraOptions.Text = "Options"
		Me.fraOptions.Visible = True
		' 
		' chkDelegatedAuthority
		' 
		Me.chkDelegatedAuthority.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDelegatedAuthority.BackColor = System.Drawing.SystemColors.Control
		Me.chkDelegatedAuthority.CausesValidation = True
		Me.chkDelegatedAuthority.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkDelegatedAuthority.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkDelegatedAuthority.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDelegatedAuthority.Enabled = True
		Me.chkDelegatedAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDelegatedAuthority.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDelegatedAuthority.Location = New System.Drawing.Point(16, 56)
		Me.chkDelegatedAuthority.Name = "chkDelegatedAuthority"
		Me.chkDelegatedAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDelegatedAuthority.Size = New System.Drawing.Size(265, 25)
		Me.chkDelegatedAuthority.TabIndex = 4
		Me.chkDelegatedAuthority.TabStop = True
		Me.chkDelegatedAuthority.Text = "Delegated Authority:"
		Me.chkDelegatedAuthority.Visible = True
		' 
		' chkRiskTransferAgreement
		' 
		Me.chkRiskTransferAgreement.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkRiskTransferAgreement.BackColor = System.Drawing.SystemColors.Control
		Me.chkRiskTransferAgreement.CausesValidation = True
		Me.chkRiskTransferAgreement.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkRiskTransferAgreement.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkRiskTransferAgreement.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkRiskTransferAgreement.Enabled = True
		Me.chkRiskTransferAgreement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkRiskTransferAgreement.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkRiskTransferAgreement.Location = New System.Drawing.Point(16, 24)
		Me.chkRiskTransferAgreement.Name = "chkRiskTransferAgreement"
		Me.chkRiskTransferAgreement.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkRiskTransferAgreement.Size = New System.Drawing.Size(265, 25)
		Me.chkRiskTransferAgreement.TabIndex = 3
		Me.chkRiskTransferAgreement.TabStop = True
		Me.chkRiskTransferAgreement.Text = "Risk Transfer Agreement:"
		Me.chkRiskTransferAgreement.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(160, 112)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 24)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(240, 112)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 24)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmRiskCodeDetails
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(317, 142)
		Me.ControlBox = True
		Me.Controls.Add(Me.fraOptions)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmRiskCodeDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Risk Code Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraOptions.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class