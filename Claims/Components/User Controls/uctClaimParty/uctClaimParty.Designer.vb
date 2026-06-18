<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctClaimParty
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents txtDate As System.Windows.Forms.TextBox
	Friend WithEvents cmdDeleteParty As System.Windows.Forms.Button
	Friend WithEvents cmdEditParty As System.Windows.Forms.Button
	Friend WithEvents cmdAddParty As System.Windows.Forms.Button
	Friend WithEvents lvwParty As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctClaimParty))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtDate = New System.Windows.Forms.TextBox
		Me.cmdDeleteParty = New System.Windows.Forms.Button
		Me.cmdEditParty = New System.Windows.Forms.Button
		Me.cmdAddParty = New System.Windows.Forms.Button
		Me.lvwParty = New System.Windows.Forms.ListView
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' txtDate
		' 
		Me.txtDate.AcceptsReturn = True
		Me.txtDate.AutoSize = False
		Me.txtDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDate.CausesValidation = True
		Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDate.Enabled = True
		Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDate.HideSelection = True
		Me.txtDate.Location = New System.Drawing.Point(280, 152)
		Me.txtDate.MaxLength = 0
		Me.txtDate.Multiline = False
		Me.txtDate.Name = "txtDate"
		Me.txtDate.ReadOnly = False
		Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDate.Size = New System.Drawing.Size(49, 19)
		Me.txtDate.TabIndex = 4
		Me.txtDate.TabStop = True
		Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDate.Visible = False
		' 
		' cmdDeleteParty
		' 
		Me.cmdDeleteParty.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeleteParty.CausesValidation = True
		Me.cmdDeleteParty.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeleteParty.Enabled = False
		Me.cmdDeleteParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDeleteParty.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeleteParty.Location = New System.Drawing.Point(266, 68)
		Me.cmdDeleteParty.Name = "cmdDeleteParty"
		Me.cmdDeleteParty.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeleteParty.Size = New System.Drawing.Size(73, 23)
		Me.cmdDeleteParty.TabIndex = 3
		Me.cmdDeleteParty.TabStop = True
		Me.cmdDeleteParty.Text = "&Delete"
		Me.cmdDeleteParty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeleteParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEditParty
		' 
		Me.cmdEditParty.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEditParty.CausesValidation = True
		Me.cmdEditParty.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEditParty.Enabled = False
		Me.cmdEditParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEditParty.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEditParty.Location = New System.Drawing.Point(266, 34)
		Me.cmdEditParty.Name = "cmdEditParty"
		Me.cmdEditParty.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEditParty.Size = New System.Drawing.Size(73, 23)
		Me.cmdEditParty.TabIndex = 2
		Me.cmdEditParty.TabStop = True
		Me.cmdEditParty.Text = "&Edit"
		Me.cmdEditParty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEditParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddParty
		' 
		Me.cmdAddParty.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddParty.CausesValidation = True
		Me.cmdAddParty.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddParty.Enabled = True
		Me.cmdAddParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddParty.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddParty.Location = New System.Drawing.Point(266, 0)
		Me.cmdAddParty.Name = "cmdAddParty"
		Me.cmdAddParty.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddParty.Size = New System.Drawing.Size(73, 23)
		Me.cmdAddParty.TabIndex = 1
		Me.cmdAddParty.TabStop = True
		Me.cmdAddParty.Text = "&Add"
		Me.cmdAddParty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwParty
		' 
		Me.lvwParty.BackColor = System.Drawing.SystemColors.Window
		Me.lvwParty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwParty.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwParty.HideSelection = False
		Me.lvwParty.LabelEdit = False
		Me.lvwParty.LabelWrap = True
		Me.lvwParty.Location = New System.Drawing.Point(2, 2)
		Me.lvwParty.Name = "lvwParty"
		Me.lvwParty.Size = New System.Drawing.Size(255, 235)
		Me.lvwParty.TabIndex = 0
		Me.lvwParty.View = System.Windows.Forms.View.Details
		' 
		' uctClaimParty
		' 
		Me.ClientSize = New System.Drawing.Size(344, 240)
		Me.Controls.Add(Me.txtDate)
		Me.Controls.Add(Me.cmdDeleteParty)
		Me.Controls.Add(Me.cmdEditParty)
		Me.Controls.Add(Me.cmdAddParty)
		Me.Controls.Add(Me.lvwParty)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctClaimParty"
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwParty, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
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