<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMResizer
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Friend WithEvents Image1 As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPMResizer))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.SuspendLayout()
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(0, 0)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 30)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.Image1.Visible = True
		' 
		' uctPMResizer
		' 
		Me.ClientSize = New System.Drawing.Size(33, 32)
		Me.Controls.Add(Me.Image1)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctPMResizer"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class