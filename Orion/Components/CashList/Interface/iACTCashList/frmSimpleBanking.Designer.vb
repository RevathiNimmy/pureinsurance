<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSimpleBanking
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdBanking As System.Windows.Forms.Button
	Public WithEvents txtBankReference As System.Windows.Forms.TextBox
	Public WithEvents fraReference As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSimpleBanking))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdBanking = New System.Windows.Forms.Button
		Me.fraReference = New System.Windows.Forms.GroupBox
		Me.txtBankReference = New System.Windows.Forms.TextBox
		Me.fraReference.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(230, 78)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdBanking
		' 
		Me.cmdBanking.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBanking.CausesValidation = True
		Me.cmdBanking.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBanking.Enabled = True
		Me.cmdBanking.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdBanking.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBanking.Location = New System.Drawing.Point(152, 78)
		Me.cmdBanking.Name = "cmdBanking"
		Me.cmdBanking.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdBanking.Size = New System.Drawing.Size(73, 22)
		Me.cmdBanking.TabIndex = 2
		Me.cmdBanking.TabStop = True
		Me.cmdBanking.Text = "Banking"
		Me.cmdBanking.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdBanking.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraReference
		' 
		Me.fraReference.BackColor = System.Drawing.SystemColors.Control
		Me.fraReference.Controls.Add(Me.txtBankReference)
		Me.fraReference.Enabled = True
		Me.fraReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraReference.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraReference.Location = New System.Drawing.Point(6, 8)
		Me.fraReference.Name = "fraReference"
		Me.fraReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraReference.Size = New System.Drawing.Size(297, 61)
		Me.fraReference.TabIndex = 0
		Me.fraReference.Text = "Bank Reference"
		Me.fraReference.Visible = True
		' 
		' txtBankReference
		' 
		Me.txtBankReference.AcceptsReturn = True
		Me.txtBankReference.AutoSize = False
		Me.txtBankReference.BackColor = System.Drawing.SystemColors.Window
		Me.txtBankReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBankReference.CausesValidation = True
		Me.txtBankReference.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBankReference.Enabled = True
		Me.txtBankReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtBankReference.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBankReference.HideSelection = True
		Me.txtBankReference.Location = New System.Drawing.Point(20, 22)
		Me.txtBankReference.MaxLength = 0
		Me.txtBankReference.Multiline = False
		Me.txtBankReference.Name = "txtBankReference"
		Me.txtBankReference.ReadOnly = False
		Me.txtBankReference.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBankReference.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBankReference.Size = New System.Drawing.Size(259, 21)
		Me.txtBankReference.TabIndex = 1
		Me.txtBankReference.TabStop = True
		Me.txtBankReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBankReference.Visible = True
		' 
		' frmSimpleBanking
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(312, 111)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdBanking)
		Me.Controls.Add(Me.fraReference)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSimpleBanking"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "End of Day Banking"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraReference.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class