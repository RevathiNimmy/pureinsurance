<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserControl1
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
	Friend WithEvents Timer1 As System.Windows.Forms.Timer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserControl1))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
		Me.Timer1 = New System.Windows.Forms.Timer(components)
		Me.SuspendLayout()
		' 
		' ProgressBar1
		' 
		Me.ProgressBar1.Location = New System.Drawing.Point(8, 4)
		Me.ProgressBar1.Maximum = 30
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(240, 17)
		Me.ProgressBar1.TabIndex = 0
		' 
		' Timer1
		' 
		Me.Timer1.Enabled = False
		Me.Timer1.Interval = 500
		' 
		' UserControl1
		' 
		Me.ClientSize = New System.Drawing.Size(255, 24)
		Me.Controls.Add(Me.ProgressBar1)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "UserControl1"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class