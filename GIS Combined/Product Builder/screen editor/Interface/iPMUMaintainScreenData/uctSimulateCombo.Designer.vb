<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctSimulateCombo
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
	Friend WithEvents Combo1 As System.Windows.Forms.ComboBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctSimulateCombo))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Combo1 = New System.Windows.Forms.ComboBox
		Me.SuspendLayout()
		' 
		' Combo1
		' 
        Me.Combo1.BackColor = Color.White ' System.Drawing.SystemColors.Window
		Me.Combo1.CausesValidation = True
		Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Combo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.Combo1.Enabled = False
		Me.Combo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Combo1.IntegralHeight = True
		Me.Combo1.Location = New System.Drawing.Point(0, 0)
		Me.Combo1.Name = "Combo1"
		Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Combo1.Size = New System.Drawing.Size(162, 21)
		Me.Combo1.Sorted = False
		Me.Combo1.TabIndex = 0
		Me.Combo1.TabStop = True
		Me.Combo1.Visible = True
		' 
		' uctSimulateCombo
		' 
		Me.ClientSize = New System.Drawing.Size(208, 21)
		Me.Controls.Add(Me.Combo1)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctSimulateCombo"
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
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
#End Region 
End Class