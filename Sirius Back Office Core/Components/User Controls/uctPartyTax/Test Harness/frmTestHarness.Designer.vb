<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
	Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
	Public WithEvents cmdload As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
		Me.cmdload = New System.Windows.Forms.Button
		Me.SuspendLayout()
		' 
		' uctPartyTax1
		' 
		Me.uctPartyTax1.FrameOn = True
		Me.uctPartyTax1.Location = New System.Drawing.Point(16, 40)
		Me.uctPartyTax1.Name = "uctPartyTax1"
		Me.uctPartyTax1.Size = New System.Drawing.Size(841, 113)
		Me.uctPartyTax1.TabIndex = 1
		' 
		' cmdload
		' 
		Me.cmdload.BackColor = System.Drawing.SystemColors.Control
		Me.cmdload.CausesValidation = True
		Me.cmdload.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdload.Enabled = True
		Me.cmdload.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdload.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdload.Location = New System.Drawing.Point(72, 264)
		Me.cmdload.Name = "cmdload"
		Me.cmdload.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdload.Size = New System.Drawing.Size(169, 65)
		Me.cmdload.TabIndex = 0
		Me.cmdload.TabStop = True
		Me.cmdload.Text = "  "
		Me.cmdload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Form1
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(900, 506)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctPartyTax1)
		Me.Controls.Add(Me.cmdload)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "Form1"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class