<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaskedInputBox
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtMaskedInput As System.Windows.Forms.TextBox
	Public WithEvents lblCaption As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMaskedInputBox))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.txtMaskedInput = New System.Windows.Forms.TextBox
		Me.lblCaption = New System.Windows.Forms.Label
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
		Me.cmdCancel.Location = New System.Drawing.Point(290, 42)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(57, 23)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(290, 14)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(57, 23)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtMaskedInput
		' 
		Me.txtMaskedInput.AcceptsReturn = True
		Me.txtMaskedInput.AutoSize = False
		Me.txtMaskedInput.BackColor = System.Drawing.SystemColors.Window
		Me.txtMaskedInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMaskedInput.CausesValidation = True
		Me.txtMaskedInput.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMaskedInput.Enabled = True
		Me.txtMaskedInput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMaskedInput.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMaskedInput.HideSelection = True
		Me.txtMaskedInput.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtMaskedInput.Location = New System.Drawing.Point(8, 94)
		Me.txtMaskedInput.MaxLength = 0
		Me.txtMaskedInput.Multiline = False
		Me.txtMaskedInput.Name = "txtMaskedInput"
		Me.txtMaskedInput.PasswordChar = ChrW(42)
		Me.txtMaskedInput.ReadOnly = False
		Me.txtMaskedInput.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMaskedInput.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMaskedInput.Size = New System.Drawing.Size(339, 19)
		Me.txtMaskedInput.TabIndex = 0
		Me.txtMaskedInput.TabStop = True
		Me.txtMaskedInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMaskedInput.Visible = True
		' 
		' lblCaption
		' 
		Me.lblCaption.AutoSize = False
		Me.lblCaption.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaption.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaption.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaption.Enabled = True
		Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaption.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaption.Location = New System.Drawing.Point(8, 8)
		Me.lblCaption.Name = "lblCaption"
		Me.lblCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaption.Size = New System.Drawing.Size(273, 65)
		Me.lblCaption.TabIndex = 3
		Me.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaption.UseMnemonic = True
		Me.lblCaption.Visible = True
		' 
		' frmMaskedInputBox
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(356, 122)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.txtMaskedInput)
		Me.Controls.Add(Me.lblCaption)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMaskedInputBox"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class