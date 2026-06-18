<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgress
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
	Public WithEvents lblDrawer As System.Windows.Forms.Label
	Public WithEvents lblCabinet As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgress))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lblDrawer = New System.Windows.Forms.Label
		Me.lblCabinet = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' lblDrawer
		' 
		Me.lblDrawer.AutoSize = False
		Me.lblDrawer.BackColor = System.Drawing.SystemColors.Control
		Me.lblDrawer.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDrawer.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDrawer.Enabled = True
		Me.lblDrawer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDrawer.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDrawer.Location = New System.Drawing.Point(8, 32)
		Me.lblDrawer.Name = "lblDrawer"
		Me.lblDrawer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDrawer.Size = New System.Drawing.Size(289, 17)
		Me.lblDrawer.TabIndex = 1
		Me.lblDrawer.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblDrawer.UseMnemonic = True
		Me.lblDrawer.Visible = True
		' 
		' lblCabinet
		' 
		Me.lblCabinet.AutoSize = False
		Me.lblCabinet.BackColor = System.Drawing.SystemColors.Control
		Me.lblCabinet.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCabinet.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCabinet.Enabled = True
		Me.lblCabinet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCabinet.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCabinet.Location = New System.Drawing.Point(8, 8)
		Me.lblCabinet.Name = "lblCabinet"
		Me.lblCabinet.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCabinet.Size = New System.Drawing.Size(289, 17)
		Me.lblCabinet.TabIndex = 0
		Me.lblCabinet.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblCabinet.UseMnemonic = True
		Me.lblCabinet.Visible = True
		' 
		' frmProgress
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(307, 61)
		Me.ControlBox = True
		Me.Controls.Add(Me.lblDrawer)
		Me.Controls.Add(Me.lblCabinet)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmProgress"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "API Daemon - Rebuilding DME Remote History"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class