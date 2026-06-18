<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Lookup
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
			UserControl_Terminate()
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents cboTypeTable As System.Windows.Forms.ComboBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Lookup))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cboTypeTable = New System.Windows.Forms.ComboBox
		Me.SuspendLayout()
		' 
		' cboTypeTable
		' 
		Me.cboTypeTable.BackColor = System.Drawing.SystemColors.Window
		Me.cboTypeTable.CausesValidation = True
		Me.cboTypeTable.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboTypeTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboTypeTable.Enabled = True
		Me.cboTypeTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboTypeTable.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboTypeTable.IntegralHeight = True
		Me.cboTypeTable.Location = New System.Drawing.Point(0, 0)
		Me.cboTypeTable.Name = "cboTypeTable"
		Me.cboTypeTable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboTypeTable.Size = New System.Drawing.Size(153, 21)
		Me.cboTypeTable.Sorted = False
		Me.cboTypeTable.TabIndex = 0
		Me.cboTypeTable.TabStop = True
		Me.cboTypeTable.Visible = True
		' 
		' Lookup
		' 
		Me.ClientSize = New System.Drawing.Size(153, 22)
		Me.Controls.Add(Me.cboTypeTable)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "Lookup"
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
	Public NotInheritable Class MouseUpEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
	Public NotInheritable Class MouseMoveEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
	Public NotInheritable Class MouseDownEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public X As Single
		Public Y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef X As Single, ByRef Y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.X = X
			Me.Y = Y
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