<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectDir
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
	Public WithEvents Drive1 As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
	Public WithEvents Panel3D3 As System.Windows.Forms.Panel
	Public WithEvents Dir1 As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
	Public WithEvents Panel3D2 As System.Windows.Forms.Panel
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents pan3Path As System.Windows.Forms.Panel
    Public WithEvents lbl3Path As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Panel3D1 As System.Windows.Forms.Panel
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectDir))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Panel3D1 = New System.Windows.Forms.Panel
		Me.Panel3D3 = New System.Windows.Forms.Panel
		Me.Drive1 = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
		Me.Panel3D2 = New System.Windows.Forms.Panel
		Me.Dir1 = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.pan3Path = New System.Windows.Forms.Panel
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.Panel3D1.SuspendLayout()
		Me.Panel3D3.SuspendLayout()
		Me.Panel3D2.SuspendLayout()
		Me.SuspendLayout()
		' 
		' Panel3D1
		' 
		Me.Panel3D1.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel3D1.Controls.Add(Me.Panel3D3)
		Me.Panel3D1.Controls.Add(Me.Panel3D2)
		Me.Panel3D1.Controls.Add(Me.cmdCancel)
		Me.Panel3D1.Controls.Add(Me.cmdOK)
		Me.Panel3D1.Controls.Add(Me.pan3Path)
		Me.Panel3D1.Controls.Add(Me.Label2)
		Me.Panel3D1.Controls.Add(Me.Label1)
		Me.Panel3D1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Panel3D1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D1.Location = New System.Drawing.Point(0, 0)
		Me.Panel3D1.Name = "Panel3D1"
		Me.Panel3D1.Size = New System.Drawing.Size(369, 229)
		Me.Panel3D1.TabIndex = 0
		' 
		' Panel3D3
		' 
		Me.Panel3D3.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D3.Controls.Add(Me.Drive1)
		Me.Panel3D3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D3.Location = New System.Drawing.Point(8, 192)
		Me.Panel3D3.Name = "Panel3D3"
		Me.Panel3D3.Size = New System.Drawing.Size(229, 23)
		Me.Panel3D3.TabIndex = 8
		' 
		' Drive1
		' 
		Me.Drive1.BackColor = System.Drawing.SystemColors.Window
		Me.Drive1.CausesValidation = True
		Me.Drive1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Drive1.Enabled = True
		Me.Drive1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Drive1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Drive1.Location = New System.Drawing.Point(1, 1)
		Me.Drive1.Name = "Drive1"
		Me.Drive1.Size = New System.Drawing.Size(227, 21)
		Me.Drive1.TabIndex = 9
		Me.Drive1.TabStop = True
		Me.Drive1.Visible = True
		' 
		' Panel3D2
		' 
		Me.Panel3D2.BackColor = System.Drawing.SystemColors.Control
		Me.Panel3D2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Panel3D2.Controls.Add(Me.Dir1)
		Me.Panel3D2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Panel3D2.Location = New System.Drawing.Point(8, 48)
		Me.Panel3D2.Name = "Panel3D2"
		Me.Panel3D2.Size = New System.Drawing.Size(229, 139)
		Me.Panel3D2.TabIndex = 6
		' 
		' Dir1
		' 
		Me.Dir1.BackColor = System.Drawing.SystemColors.Window
		Me.Dir1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Dir1.CausesValidation = True
		Me.Dir1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Dir1.Enabled = True
		Me.Dir1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Dir1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Dir1.Location = New System.Drawing.Point(1, 1)
		Me.Dir1.Name = "Dir1"
		Me.Dir1.Size = New System.Drawing.Size(227, 137)
		Me.Dir1.TabIndex = 7
		Me.Dir1.TabStop = True
		Me.Dir1.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(260, 60)
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
		Me.cmdOK.Location = New System.Drawing.Point(260, 28)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(85, 25)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' pan3Path
		' 
		Me.pan3Path.BackColor = System.Drawing.SystemColors.Control
		Me.pan3Path.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pan3Path.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3Path.Location = New System.Drawing.Point(8, 28)
		Me.pan3Path.Name = "pan3Path"
		Me.pan3Path.Size = New System.Drawing.Size(229, 17)
        Me.pan3Path.TabIndex = 3
        Me.pan3Path.Controls.Add(lbl3Path)
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.Color.Transparent
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label2.Location = New System.Drawing.Point(8, 176)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(113, 17)
		Me.Label2.TabIndex = 2
		Me.Label2.Text = "Drives:"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(8, 12)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(121, 17)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Directories:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmSelectDir
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(369, 229)
		Me.ControlBox = True
		Me.Controls.Add(Me.Panel3D1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(173, 150)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSelectDir"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Select Remote Directory"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Panel3D1.ResumeLayout(False)
		Me.Panel3D3.ResumeLayout(False)
		Me.Panel3D2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class