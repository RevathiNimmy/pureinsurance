<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctDivider
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
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
	Friend WithEvents fraDivider As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctDivider))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraDivider = New System.Windows.Forms.GroupBox
		Me.SuspendLayout()
		' 
		' fraDivider
		' 
		Me.fraDivider.BackColor = System.Drawing.SystemColors.Control
		Me.fraDivider.Enabled = True
		Me.fraDivider.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDivider.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDivider.Location = New System.Drawing.Point(-2, 1)
		Me.fraDivider.Name = "fraDivider"
		Me.fraDivider.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDivider.Size = New System.Drawing.Size(474, 33)
		Me.fraDivider.TabIndex = 0
		Me.fraDivider.Text = "Frame1"
		Me.fraDivider.Visible = True
		' 
		' uctDivider
		' 
		Me.ClientSize = New System.Drawing.Size(470, 19)
		Me.Controls.Add(Me.fraDivider)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctDivider"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class