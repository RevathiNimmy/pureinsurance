<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cboAccumulation
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cboAccumulation_cboAccumulation As System.Windows.Forms.ComboBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(cboAccumulation))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cboAccumulation_cboAccumulation = New System.Windows.Forms.ComboBox
		Me.SuspendLayout()
		' 
		' cboAccumulation_cboAccumulation
		' 
		Me.cboAccumulation_cboAccumulation.BackColor = System.Drawing.SystemColors.Window
		Me.cboAccumulation_cboAccumulation.CausesValidation = True
		Me.cboAccumulation_cboAccumulation.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboAccumulation_cboAccumulation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboAccumulation_cboAccumulation.Enabled = True
		Me.cboAccumulation_cboAccumulation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAccumulation_cboAccumulation.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboAccumulation_cboAccumulation.IntegralHeight = True
		Me.cboAccumulation_cboAccumulation.Location = New System.Drawing.Point(0, 0)
		Me.cboAccumulation_cboAccumulation.Name = "cboAccumulation_cboAccumulation"
		Me.cboAccumulation_cboAccumulation.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboAccumulation_cboAccumulation.Size = New System.Drawing.Size(153, 21)
		Me.cboAccumulation_cboAccumulation.Sorted = True
		Me.cboAccumulation_cboAccumulation.TabIndex = 0
		Me.cboAccumulation_cboAccumulation.TabStop = True
		Me.cboAccumulation_cboAccumulation.Visible = True
		' 
		' cboAccumulation
		' 
		Me.ClientSize = New System.Drawing.Size(153, 21)
		Me.Controls.Add(Me.cboAccumulation_cboAccumulation)
		Me.Location = New System.Drawing.Point(0, 0)
		Me.Name = "cboAccumulation"
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
	Public NotInheritable Class MouseUpEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
	Public NotInheritable Class MouseMoveEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
	Public NotInheritable Class MouseDownEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
	Public NotInheritable Class KeyUpEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
	Public NotInheritable Class KeyPressEventArgs
		Inherits System.EventArgs
		Public KeyAscii As Integer
		Public Sub New(ByRef KeyAscii As Integer)
			MyBase.New()
			Me.KeyAscii = KeyAscii
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
	Public NotInheritable Class KeyDownEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
#End Region 
End Class