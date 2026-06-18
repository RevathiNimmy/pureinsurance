<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YesNoCheck
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
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
	Friend WithEvents picTickCross As System.Windows.Forms.PictureBox
	Friend WithEvents imgCrossGrey As System.Windows.Forms.PictureBox
	Friend WithEvents imgTickGrey As System.Windows.Forms.PictureBox
	Friend WithEvents lblDisabled As System.Windows.Forms.Label
	Friend WithEvents lblEnabled As System.Windows.Forms.Label
	Friend WithEvents imgCrossBlack As System.Windows.Forms.PictureBox
	Friend WithEvents imgTickBlack As System.Windows.Forms.PictureBox
	Friend WithEvents shpFocus As Artinsoft.VB6.Gui.ShapeHelper
	Friend WithEvents lblCaption As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(YesNoCheck))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.picTickCross = New System.Windows.Forms.PictureBox
		Me.imgCrossGrey = New System.Windows.Forms.PictureBox
		Me.imgTickGrey = New System.Windows.Forms.PictureBox
		Me.lblDisabled = New System.Windows.Forms.Label
		Me.lblEnabled = New System.Windows.Forms.Label
		Me.imgCrossBlack = New System.Windows.Forms.PictureBox
		Me.imgTickBlack = New System.Windows.Forms.PictureBox
		Me.shpFocus = New Artinsoft.VB6.Gui.ShapeHelper
		Me.lblCaption = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' picTickCross
		' 
		Me.picTickCross.BackColor = System.Drawing.Color.White
		Me.picTickCross.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.picTickCross.CausesValidation = True
		Me.picTickCross.Cursor = System.Windows.Forms.Cursors.Default
		Me.picTickCross.Dock = System.Windows.Forms.DockStyle.None
		Me.picTickCross.Enabled = True
		Me.picTickCross.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picTickCross.Location = New System.Drawing.Point(24, 16)
		Me.picTickCross.Name = "picTickCross"
		Me.picTickCross.Size = New System.Drawing.Size(13, 13)
		Me.picTickCross.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.picTickCross.TabIndex = 0
		Me.picTickCross.TabStop = True
		Me.picTickCross.Visible = True
		' 
		' imgCrossGrey
		' 
		Me.imgCrossGrey.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgCrossGrey.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgCrossGrey.Enabled = True
		Me.imgCrossGrey.Image = CType(resources.GetObject("imgCrossGrey.Image"), System.Drawing.Image)
		Me.imgCrossGrey.Location = New System.Drawing.Point(40, 55)
		Me.imgCrossGrey.Name = "imgCrossGrey"
		Me.imgCrossGrey.Size = New System.Drawing.Size(9, 9)
		Me.imgCrossGrey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgCrossGrey.Visible = False
		' 
		' imgTickGrey
		' 
		Me.imgTickGrey.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgTickGrey.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgTickGrey.Enabled = True
		Me.imgTickGrey.Image = CType(resources.GetObject("imgTickGrey.Image"), System.Drawing.Image)
		Me.imgTickGrey.Location = New System.Drawing.Point(25, 55)
		Me.imgTickGrey.Name = "imgTickGrey"
		Me.imgTickGrey.Size = New System.Drawing.Size(9, 9)
		Me.imgTickGrey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgTickGrey.Visible = False
		' 
		' lblDisabled
		' 
		Me.lblDisabled.AutoSize = False
		Me.lblDisabled.BackColor = System.Drawing.SystemColors.Control
		Me.lblDisabled.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDisabled.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDisabled.Enabled = True
		Me.lblDisabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDisabled.ForeColor = System.Drawing.SystemColors.GrayText
		Me.lblDisabled.Location = New System.Drawing.Point(212, 28)
		Me.lblDisabled.Name = "lblDisabled"
		Me.lblDisabled.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDisabled.Size = New System.Drawing.Size(69, 17)
		Me.lblDisabled.TabIndex = 3
		Me.lblDisabled.Text = "Disabled"
		Me.lblDisabled.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDisabled.UseMnemonic = True
		Me.lblDisabled.Visible = False
		' 
		' lblEnabled
		' 
		Me.lblEnabled.AutoSize = False
		Me.lblEnabled.BackColor = System.Drawing.SystemColors.Control
		Me.lblEnabled.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEnabled.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEnabled.Enabled = True
		Me.lblEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEnabled.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEnabled.Location = New System.Drawing.Point(212, 4)
		Me.lblEnabled.Name = "lblEnabled"
		Me.lblEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEnabled.Size = New System.Drawing.Size(69, 17)
		Me.lblEnabled.TabIndex = 2
		Me.lblEnabled.Text = "Enabled"
		Me.lblEnabled.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEnabled.UseMnemonic = True
		Me.lblEnabled.Visible = False
		' 
		' imgCrossBlack
		' 
		Me.imgCrossBlack.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgCrossBlack.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgCrossBlack.Enabled = True
		Me.imgCrossBlack.Image = CType(resources.GetObject("imgCrossBlack.Image"), System.Drawing.Image)
		Me.imgCrossBlack.Location = New System.Drawing.Point(40, 40)
		Me.imgCrossBlack.Name = "imgCrossBlack"
		Me.imgCrossBlack.Size = New System.Drawing.Size(9, 9)
		Me.imgCrossBlack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgCrossBlack.Visible = False
		' 
		' imgTickBlack
		' 
		Me.imgTickBlack.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgTickBlack.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgTickBlack.Enabled = True
		Me.imgTickBlack.Image = CType(resources.GetObject("imgTickBlack.Image"), System.Drawing.Image)
		Me.imgTickBlack.Location = New System.Drawing.Point(25, 40)
		Me.imgTickBlack.Name = "imgTickBlack"
		Me.imgTickBlack.Size = New System.Drawing.Size(9, 9)
		Me.imgTickBlack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgTickBlack.Visible = False
		' 
		' shpFocus
		' 
		Me.shpFocus.BackColor = System.Drawing.SystemColors.Window
		Me.shpFocus.BackStyle = 0
		Me.shpFocus.BorderColor = System.Drawing.SystemColors.ControlDark
		Me.shpFocus.BorderStyle = 3
		Me.shpFocus.FillColor = System.Drawing.Color.Black
		Me.shpFocus.FillStyle = 1
		Me.shpFocus.Location = New System.Drawing.Point(108, 36)
		Me.shpFocus.Name = "shpFocus"
		Me.shpFocus.Size = New System.Drawing.Size(93, 17)
		Me.shpFocus.Visible = False
		' 
		' lblCaption
		' 
		Me.lblCaption.AutoSize = False
		Me.lblCaption.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaption.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaption.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaption.Enabled = True
		Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaption.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaption.Location = New System.Drawing.Point(52, 16)
		Me.lblCaption.Name = "lblCaption"
		Me.lblCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaption.Size = New System.Drawing.Size(25, 13)
		Me.lblCaption.TabIndex = 1
		Me.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaption.UseMnemonic = True
		Me.lblCaption.Visible = True
		' 
		' YesNoCheck
		' 
		Me.ClientSize = New System.Drawing.Size(327, 106)
		Me.Controls.Add(Me.picTickCross)
		Me.Controls.Add(Me.imgCrossGrey)
		Me.Controls.Add(Me.imgTickGrey)
		Me.Controls.Add(Me.lblDisabled)
		Me.Controls.Add(Me.lblEnabled)
		Me.Controls.Add(Me.imgCrossBlack)
		Me.Controls.Add(Me.imgTickBlack)
		Me.Controls.Add(Me.shpFocus)
		Me.Controls.Add(Me.lblCaption)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "YesNoCheck"
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