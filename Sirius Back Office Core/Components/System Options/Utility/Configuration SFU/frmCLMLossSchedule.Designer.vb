<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClaimLossSchedule
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
	Public WithEvents cmdClaimHelpfile As System.Windows.Forms.Button
	Public WithEvents txtClaimHelpFile As System.Windows.Forms.TextBox
	Public WithEvents lblOption2015 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClaimLossSchedule))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdClaimHelpfile = New System.Windows.Forms.Button
		Me.txtClaimHelpFile = New System.Windows.Forms.TextBox
		Me.lblOption2015 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdClaimHelpfile
		' 
		Me.cmdClaimHelpfile.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClaimHelpfile.CausesValidation = True
		Me.cmdClaimHelpfile.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClaimHelpfile.Enabled = True
		Me.cmdClaimHelpfile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClaimHelpfile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClaimHelpfile.Location = New System.Drawing.Point(392, 8)
		Me.cmdClaimHelpfile.Name = "cmdClaimHelpfile"
		Me.cmdClaimHelpfile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClaimHelpfile.Size = New System.Drawing.Size(57, 19)
		Me.cmdClaimHelpfile.TabIndex = 1
		Me.cmdClaimHelpfile.TabStop = False
		Me.cmdClaimHelpfile.Tag = "2015,ClaimHelpFile"
		Me.cmdClaimHelpfile.Text = "Browse"
		Me.cmdClaimHelpfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClaimHelpfile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtClaimHelpFile
		' 
		Me.txtClaimHelpFile.AcceptsReturn = True
		Me.txtClaimHelpFile.AutoSize = False
		Me.txtClaimHelpFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtClaimHelpFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtClaimHelpFile.CausesValidation = True
		Me.txtClaimHelpFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtClaimHelpFile.Enabled = True
		Me.txtClaimHelpFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtClaimHelpFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtClaimHelpFile.HideSelection = True
		Me.txtClaimHelpFile.Location = New System.Drawing.Point(136, 9)
		Me.txtClaimHelpFile.MaxLength = 0
		Me.txtClaimHelpFile.Multiline = False
		Me.txtClaimHelpFile.Name = "txtClaimHelpFile"
		Me.txtClaimHelpFile.ReadOnly = False
		Me.txtClaimHelpFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtClaimHelpFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtClaimHelpFile.Size = New System.Drawing.Size(249, 19)
		Me.txtClaimHelpFile.TabIndex = 0
		Me.txtClaimHelpFile.TabStop = True
		Me.txtClaimHelpFile.Tag = "2015"
		Me.txtClaimHelpFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtClaimHelpFile.Visible = True
		' 
		' lblOption2015
		' 
		Me.lblOption2015.AutoSize = True
		Me.lblOption2015.BackColor = System.Drawing.SystemColors.Control
		Me.lblOption2015.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOption2015.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOption2015.Enabled = True
		Me.lblOption2015.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOption2015.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOption2015.Location = New System.Drawing.Point(8, 12)
		Me.lblOption2015.Name = "lblOption2015"
		Me.lblOption2015.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOption2015.Size = New System.Drawing.Size(104, 13)
		Me.lblOption2015.TabIndex = 2
		Me.lblOption2015.Tag = "2015"
		Me.lblOption2015.Text = "Help File Location:"
		Me.lblOption2015.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOption2015.UseMnemonic = True
		Me.lblOption2015.Visible = True
		' 
		' frmClaimLossSchedule
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(553, 394)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdClaimHelpfile)
		Me.Controls.Add(Me.txtClaimHelpFile)
		Me.Controls.Add(Me.lblOption2015)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmClaimLossSchedule"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class