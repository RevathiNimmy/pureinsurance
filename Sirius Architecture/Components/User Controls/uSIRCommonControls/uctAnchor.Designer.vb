<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctAnchor
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_Initialize()
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
	Friend WithEvents lblCaption As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctAnchor))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lblCaption = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' lblCaption
		' 
		Me.lblCaption.AutoSize = True
		Me.lblCaption.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblCaption.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaption.Enabled = True
		Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaption.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaption.Location = New System.Drawing.Point(0, -1)
		Me.lblCaption.Name = "lblCaption"
		Me.lblCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaption.Size = New System.Drawing.Size(44, 17)
		Me.lblCaption.TabIndex = 0
		Me.lblCaption.Text = " Anchor "
		Me.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaption.UseMnemonic = True
		Me.lblCaption.Visible = True
		' 
		' uctAnchor
		' 
		Me.ClientSize = New System.Drawing.Size(44, 16)
		Me.Controls.Add(Me.lblCaption)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctAnchor"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class