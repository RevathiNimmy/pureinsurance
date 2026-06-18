<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PBFindDesign
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
	Friend WithEvents btnClear As System.Windows.Forms.Button
	Friend WithEvents btnFind As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PBFindDesign))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.btnClear = New System.Windows.Forms.Button
		Me.btnFind = New System.Windows.Forms.Button
		Me.SuspendLayout()
		' 
		' btnClear
		' 
		Me.btnClear.BackColor = System.Drawing.SystemColors.Control
		Me.btnClear.CausesValidation = True
		Me.btnClear.Cursor = System.Windows.Forms.Cursors.Default
		Me.btnClear.Enabled = True
		Me.btnClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.btnClear.ForeColor = System.Drawing.SystemColors.ControlText
		Me.btnClear.Location = New System.Drawing.Point(88, 8)
		Me.btnClear.Name = "btnClear"
		Me.btnClear.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.btnClear.Size = New System.Drawing.Size(73, 22)
		Me.btnClear.TabIndex = 1
		Me.btnClear.TabStop = True
		Me.btnClear.Text = "Clear"
		Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' btnFind
		' 
		Me.btnFind.BackColor = System.Drawing.SystemColors.Control
		Me.btnFind.CausesValidation = True
		Me.btnFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.btnFind.Enabled = True
		Me.btnFind.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.btnFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.btnFind.Location = New System.Drawing.Point(8, 8)
		Me.btnFind.Name = "btnFind"
		Me.btnFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.btnFind.Size = New System.Drawing.Size(73, 22)
		Me.btnFind.TabIndex = 0
		Me.btnFind.TabStop = True
		Me.btnFind.Text = "Find"
		Me.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' PBFindDesign
		' 
		Me.ClientSize = New System.Drawing.Size(170, 37)
		Me.Controls.Add(Me.btnClear)
		Me.Controls.Add(Me.btnFind)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "PBFindDesign"
		ToolTip1.SetToolTip(Me.btnClear, "Clear Search Values")
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
    '<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
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
    '<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
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
    '<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
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
#End Region 
End Class