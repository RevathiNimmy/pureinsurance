<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVolumeName
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents txtVolName As System.Windows.Forms.TextBox
	Public WithEvents Panel3D3 As System.Windows.Forms.Panel
	Public WithEvents txtRoot As System.Windows.Forms.TextBox
	Public WithEvents Panel3D2 As System.Windows.Forms.Panel
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame3D1 As System.Windows.Forms.GroupBox
	Public WithEvents chk3rewrite As System.Windows.Forms.CheckBox
	Public WithEvents Panel3D1 As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVolumeName))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Panel3D1 = New System.Windows.Forms.Panel
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Panel3D3 = New System.Windows.Forms.Panel
		Me.txtVolName = New System.Windows.Forms.TextBox
		Me.Frame3D1 = New System.Windows.Forms.GroupBox
		Me.Panel3D2 = New System.Windows.Forms.Panel
		Me.txtRoot = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.chk3rewrite = New System.Windows.Forms.CheckBox
		Me.Panel3D1.SuspendLayout()
		Me.Panel3D3.SuspendLayout()
		Me.Frame3D1.SuspendLayout()
		Me.Panel3D2.SuspendLayout()
		Me.SuspendLayout()
		' 
		' Panel3D1
		' 
		Me.Panel3D1.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel3D1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel3D1.Controls.Add(Me.cmdCancel)
		Me.Panel3D1.Controls.Add(Me.cmdOK)
		Me.Panel3D1.Controls.Add(Me.Panel3D3)
		Me.Panel3D1.Controls.Add(Me.Frame3D1)
		Me.Panel3D1.Controls.Add(Me.chk3rewrite)
		Me.Panel3D1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Panel3D1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D1.Location = New System.Drawing.Point(0, 0)
		Me.Panel3D1.Name = "Panel3D1"
		Me.Panel3D1.Size = New System.Drawing.Size(367, 157)
		Me.Panel3D1.TabIndex = 0
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(264, 116)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(85, 25)
		Me.cmdCancel.TabIndex = 5
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
		Me.cmdOK.Location = New System.Drawing.Point(264, 84)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(85, 25)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Panel3D3
		' 
		Me.Panel3D3.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D3.Controls.Add(Me.txtVolName)
		Me.Panel3D3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D3.Location = New System.Drawing.Point(40, 36)
		Me.Panel3D3.Name = "Panel3D3"
		Me.Panel3D3.Size = New System.Drawing.Size(189, 24)
		Me.Panel3D3.TabIndex = 6
		' 
		' txtVolName
		' 
		Me.txtVolName.AcceptsReturn = True
		Me.txtVolName.AutoSize = False
		Me.txtVolName.BackColor = System.Drawing.SystemColors.Window
		Me.txtVolName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtVolName.CausesValidation = True
		Me.txtVolName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtVolName.Enabled = True
		Me.txtVolName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtVolName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtVolName.HideSelection = True
		Me.txtVolName.Location = New System.Drawing.Point(1, 1)
		Me.txtVolName.MaxLength = 0
		Me.txtVolName.Multiline = False
		Me.txtVolName.Name = "txtVolName"
		Me.txtVolName.ReadOnly = False
		Me.txtVolName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtVolName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtVolName.Size = New System.Drawing.Size(187, 22)
		Me.txtVolName.TabIndex = 1
		Me.txtVolName.TabStop = True
		Me.txtVolName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtVolName.Visible = True
		' 
		' Frame3D1
		' 
		Me.Frame3D1.Controls.Add(Me.Panel3D2)
		Me.Frame3D1.Controls.Add(Me.Label1)
		Me.Frame3D1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'to do
        'Me.Frame3D1.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.Frame3D1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.Frame3D1.Location = New System.Drawing.Point(20, 12)
		Me.Frame3D1.Name = "Frame3D1"
		Me.Frame3D1.Size = New System.Drawing.Size(229, 129)
		Me.Frame3D1.TabIndex = 7
		Me.Frame3D1.Text = "Volume Name"
		' 
		' Panel3D2
		' 
		Me.Panel3D2.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D2.Controls.Add(Me.txtRoot)
		Me.Panel3D2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D2.Location = New System.Drawing.Point(20, 72)
		Me.Panel3D2.Name = "Panel3D2"
		Me.Panel3D2.Size = New System.Drawing.Size(189, 24)
		Me.Panel3D2.TabIndex = 9
		' 
		' txtRoot
		' 
		Me.txtRoot.AcceptsReturn = True
		Me.txtRoot.AutoSize = False
		Me.txtRoot.BackColor = System.Drawing.SystemColors.Window
		Me.txtRoot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtRoot.CausesValidation = True
		Me.txtRoot.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRoot.Enabled = True
		Me.txtRoot.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRoot.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRoot.HideSelection = True
		Me.txtRoot.Location = New System.Drawing.Point(1, 1)
		Me.txtRoot.MaxLength = 0
		Me.txtRoot.Multiline = False
		Me.txtRoot.Name = "txtRoot"
		Me.txtRoot.ReadOnly = False
		Me.txtRoot.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRoot.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRoot.Size = New System.Drawing.Size(187, 22)
		Me.txtRoot.TabIndex = 2
		Me.txtRoot.TabStop = True
		Me.txtRoot.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRoot.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.Color.Transparent
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label1.Location = New System.Drawing.Point(20, 56)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(89, 13)
		Me.Label1.TabIndex = 8
		Me.Label1.Text = "Data Root"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' chk3rewrite
		' 
		Me.chk3rewrite.Checked = True
		Me.chk3rewrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chk3rewrite.Location = New System.Drawing.Point(40, 116)
		Me.chk3rewrite.Name = "chk3rewrite"
		Me.chk3rewrite.Size = New System.Drawing.Size(93, 17)
		Me.chk3rewrite.TabIndex = 3
		Me.chk3rewrite.Text = "Rewriteable"
		' 
		' frmVolumeName
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(367, 157)
		Me.ControlBox = False
		Me.Controls.Add(Me.Panel3D1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(138, 230)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmVolumeName"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Volume Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Panel3D1.ResumeLayout(False)
		Me.Panel3D3.ResumeLayout(False)
		Me.Frame3D1.ResumeLayout(False)
		Me.Panel3D2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class