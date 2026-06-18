<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEdit
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
	Public WithEvents txtAllocatedAmount As System.Windows.Forms.TextBox
	Public WithEvents txtOSAmount As System.Windows.Forms.TextBox
	Public WithEvents txtOriginalAmount As System.Windows.Forms.TextBox
	Public WithEvents lblOSAmount As System.Windows.Forms.Label
	Public WithEvents lblAllocatedAmount As System.Windows.Forms.Label
	Public WithEvents lblOriginalAmount As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEdit))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.txtAllocatedAmount = New System.Windows.Forms.TextBox
		Me.txtOSAmount = New System.Windows.Forms.TextBox
		Me.txtOriginalAmount = New System.Windows.Forms.TextBox
		Me.lblOSAmount = New System.Windows.Forms.Label
		Me.lblAllocatedAmount = New System.Windows.Forms.Label
		Me.lblOriginalAmount = New System.Windows.Forms.Label
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
		Me.cmdCancel.Location = New System.Drawing.Point(184, 112)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
		Me.cmdCancel.TabIndex = 7
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
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
		Me.cmdOK.Location = New System.Drawing.Point(104, 112)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 25)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtAllocatedAmount
		' 
		Me.txtAllocatedAmount.AcceptsReturn = True
		Me.txtAllocatedAmount.AutoSize = False
		Me.txtAllocatedAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtAllocatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAllocatedAmount.CausesValidation = True
		Me.txtAllocatedAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAllocatedAmount.Enabled = True
		Me.txtAllocatedAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAllocatedAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAllocatedAmount.HideSelection = True
		Me.txtAllocatedAmount.Location = New System.Drawing.Point(136, 48)
		Me.txtAllocatedAmount.MaxLength = 0
		Me.txtAllocatedAmount.Multiline = False
		Me.txtAllocatedAmount.Name = "txtAllocatedAmount"
		Me.txtAllocatedAmount.ReadOnly = False
		Me.txtAllocatedAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAllocatedAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAllocatedAmount.Size = New System.Drawing.Size(121, 19)
		Me.txtAllocatedAmount.TabIndex = 5
		Me.txtAllocatedAmount.TabStop = True
		Me.txtAllocatedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAllocatedAmount.Visible = True
		' 
		' txtOSAmount
		' 
		Me.txtOSAmount.AcceptsReturn = True
		Me.txtOSAmount.AutoSize = False
		Me.txtOSAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtOSAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOSAmount.CausesValidation = True
		Me.txtOSAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOSAmount.Enabled = False
		Me.txtOSAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOSAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOSAmount.HideSelection = True
		Me.txtOSAmount.Location = New System.Drawing.Point(136, 80)
		Me.txtOSAmount.MaxLength = 0
		Me.txtOSAmount.Multiline = False
		Me.txtOSAmount.Name = "txtOSAmount"
		Me.txtOSAmount.ReadOnly = False
		Me.txtOSAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOSAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOSAmount.Size = New System.Drawing.Size(121, 19)
		Me.txtOSAmount.TabIndex = 4
		Me.txtOSAmount.TabStop = True
		Me.txtOSAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOSAmount.Visible = True
		' 
		' txtOriginalAmount
		' 
		Me.txtOriginalAmount.AcceptsReturn = True
		Me.txtOriginalAmount.AutoSize = False
		Me.txtOriginalAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtOriginalAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOriginalAmount.CausesValidation = True
		Me.txtOriginalAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOriginalAmount.Enabled = False
		Me.txtOriginalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOriginalAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOriginalAmount.HideSelection = True
		Me.txtOriginalAmount.Location = New System.Drawing.Point(136, 16)
		Me.txtOriginalAmount.MaxLength = 0
		Me.txtOriginalAmount.Multiline = False
		Me.txtOriginalAmount.Name = "txtOriginalAmount"
		Me.txtOriginalAmount.ReadOnly = False
		Me.txtOriginalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOriginalAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOriginalAmount.Size = New System.Drawing.Size(121, 19)
		Me.txtOriginalAmount.TabIndex = 1
		Me.txtOriginalAmount.TabStop = True
		Me.txtOriginalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOriginalAmount.Visible = True
		' 
		' lblOSAmount
		' 
		Me.lblOSAmount.AutoSize = False
		Me.lblOSAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblOSAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOSAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOSAmount.Enabled = True
		Me.lblOSAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOSAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOSAmount.Location = New System.Drawing.Point(8, 80)
		Me.lblOSAmount.Name = "lblOSAmount"
		Me.lblOSAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOSAmount.Size = New System.Drawing.Size(121, 17)
		Me.lblOSAmount.TabIndex = 3
		Me.lblOSAmount.Text = "Outstanding Amount"
		Me.lblOSAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOSAmount.UseMnemonic = True
		Me.lblOSAmount.Visible = True
		' 
		' lblAllocatedAmount
		' 
		Me.lblAllocatedAmount.AutoSize = False
		Me.lblAllocatedAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblAllocatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAllocatedAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAllocatedAmount.Enabled = True
		Me.lblAllocatedAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAllocatedAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAllocatedAmount.Location = New System.Drawing.Point(8, 48)
		Me.lblAllocatedAmount.Name = "lblAllocatedAmount"
		Me.lblAllocatedAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocatedAmount.Size = New System.Drawing.Size(121, 17)
		Me.lblAllocatedAmount.TabIndex = 2
		Me.lblAllocatedAmount.Text = "Allocated Amount"
		Me.lblAllocatedAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAllocatedAmount.UseMnemonic = True
		Me.lblAllocatedAmount.Visible = True
		' 
		' lblOriginalAmount
		' 
		Me.lblOriginalAmount.AutoSize = False
		Me.lblOriginalAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblOriginalAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOriginalAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOriginalAmount.Enabled = True
		Me.lblOriginalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOriginalAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOriginalAmount.Location = New System.Drawing.Point(8, 16)
		Me.lblOriginalAmount.Name = "lblOriginalAmount"
		Me.lblOriginalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOriginalAmount.Size = New System.Drawing.Size(121, 17)
		Me.lblOriginalAmount.TabIndex = 0
		Me.lblOriginalAmount.Text = "Original Amount"
		Me.lblOriginalAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOriginalAmount.UseMnemonic = True
		Me.lblOriginalAmount.Visible = True
		' 
		' frmEdit
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(263, 144)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.txtAllocatedAmount)
		Me.Controls.Add(Me.txtOSAmount)
		Me.Controls.Add(Me.txtOriginalAmount)
		Me.Controls.Add(Me.lblOSAmount)
		Me.Controls.Add(Me.lblAllocatedAmount)
		Me.Controls.Add(Me.lblOriginalAmount)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmEdit"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Edit Allocation"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class