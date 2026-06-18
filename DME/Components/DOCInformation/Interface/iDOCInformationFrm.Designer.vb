<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
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
	Public WithEvents cmdAnnotations As System.Windows.Forms.Button
	Public WithEvents CmdPassword As System.Windows.Forms.Button
	Public WithEvents CmdKeywords As System.Windows.Forms.Button
	Public WithEvents CmdHelp As System.Windows.Forms.Button
	Public WithEvents CmdCancel As System.Windows.Forms.Button
	Public WithEvents CmdOK As System.Windows.Forms.Button
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents CmbDocName As System.Windows.Forms.ComboBox
	Public WithEvents TxtFolderName As System.Windows.Forms.TextBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents optExtNo As System.Windows.Forms.RadioButton
	Public WithEvents optExtYes As System.Windows.Forms.RadioButton
	Public WithEvents TxtCreateUser As System.Windows.Forms.TextBox
	Public WithEvents TxtAccLevel As System.Windows.Forms.TextBox
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents TxtCreateDate As System.Windows.Forms.TextBox
	Public WithEvents TxtExpDate As System.Windows.Forms.TextBox
	Public WithEvents TxtDocDate As System.Windows.Forms.TextBox
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAnnotations = New System.Windows.Forms.Button
        Me.CmdPassword = New System.Windows.Forms.Button
        Me.CmdKeywords = New System.Windows.Forms.Button
        Me.CmdHelp = New System.Windows.Forms.Button
        Me.CmdCancel = New System.Windows.Forms.Button
        Me.CmdOK = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.CmbDocName = New System.Windows.Forms.ComboBox
        Me.TxtFolderName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.optExtNo = New System.Windows.Forms.RadioButton
        Me.optExtYes = New System.Windows.Forms.RadioButton
        Me.TxtCreateUser = New System.Windows.Forms.TextBox
        Me.TxtAccLevel = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.TxtCreateDate = New System.Windows.Forms.TextBox
        Me.TxtExpDate = New System.Windows.Forms.TextBox
        Me.TxtDocDate = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAnnotations
        '
        Me.cmdAnnotations.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAnnotations.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAnnotations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAnnotations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAnnotations.Location = New System.Drawing.Point(184, 392)
        Me.cmdAnnotations.Name = "cmdAnnotations"
        Me.cmdAnnotations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAnnotations.Size = New System.Drawing.Size(73, 22)
        Me.cmdAnnotations.TabIndex = 27
        Me.cmdAnnotations.Text = "&Annotation"
        Me.cmdAnnotations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAnnotations.UseVisualStyleBackColor = False
        '
        'CmdPassword
        '
        Me.CmdPassword.BackColor = System.Drawing.SystemColors.Control
        Me.CmdPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdPassword.Location = New System.Drawing.Point(96, 392)
        Me.CmdPassword.Name = "CmdPassword"
        Me.CmdPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdPassword.Size = New System.Drawing.Size(73, 22)
        Me.CmdPassword.TabIndex = 5
        Me.CmdPassword.Text = "&Password"
        Me.CmdPassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdPassword.UseVisualStyleBackColor = False
        '
        'CmdKeywords
        '
        Me.CmdKeywords.BackColor = System.Drawing.SystemColors.Control
        Me.CmdKeywords.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdKeywords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdKeywords.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdKeywords.Location = New System.Drawing.Point(8, 392)
        Me.CmdKeywords.Name = "CmdKeywords"
        Me.CmdKeywords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdKeywords.Size = New System.Drawing.Size(73, 22)
        Me.CmdKeywords.TabIndex = 4
        Me.CmdKeywords.Text = "&Keywords"
        Me.CmdKeywords.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdKeywords.UseVisualStyleBackColor = False
        '
        'CmdHelp
        '
        Me.CmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.CmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdHelp.Location = New System.Drawing.Point(536, 392)
        Me.CmdHelp.Name = "CmdHelp"
        Me.CmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.CmdHelp.TabIndex = 2
        Me.CmdHelp.Text = "&Help"
        Me.CmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdHelp.UseVisualStyleBackColor = False
        '
        'CmdCancel
        '
        Me.CmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.CmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCancel.Location = New System.Drawing.Point(448, 392)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.CmdCancel.TabIndex = 1
        Me.CmdCancel.Text = "&Cancel"
        Me.CmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdCancel.UseVisualStyleBackColor = False
        '
        'CmdOK
        '
        Me.CmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.CmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdOK.Location = New System.Drawing.Point(360, 392)
        Me.CmdOK.Name = "CmdOK"
        Me.CmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdOK.Size = New System.Drawing.Size(73, 22)
        Me.CmdOK.TabIndex = 0
        Me.CmdOK.Text = "&OK"
        Me.CmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdOK.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(199, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(605, 381)
        Me.SSTab1.TabIndex = 3
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Image1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame2)
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame3)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(597, 355)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - General"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(544, 26)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 0
        Me.Image1.TabStop = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.CmbDocName)
        Me.Frame1.Controls.Add(Me.TxtFolderName)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 20)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(473, 113)
        Me.Frame1.TabIndex = 6
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Location"
        '
        'CmbDocName
        '
        Me.CmbDocName.BackColor = System.Drawing.SystemColors.Window
        Me.CmbDocName.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmbDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbDocName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CmbDocName.Location = New System.Drawing.Point(112, 72)
        Me.CmbDocName.Name = "CmbDocName"
        Me.CmbDocName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmbDocName.Size = New System.Drawing.Size(345, 21)
        Me.CmbDocName.TabIndex = 8
        '
        'TxtFolderName
        '
        Me.TxtFolderName.AcceptsReturn = True
        Me.TxtFolderName.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtFolderName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtFolderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFolderName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtFolderName.Location = New System.Drawing.Point(112, 32)
        Me.TxtFolderName.MaxLength = 0
        Me.TxtFolderName.Name = "TxtFolderName"
        Me.TxtFolderName.ReadOnly = True
        Me.TxtFolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtFolderName.Size = New System.Drawing.Size(345, 20)
        Me.TxtFolderName.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(113, 17)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Document Name"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(105, 17)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Folder Name"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.optExtNo)
        Me.Frame2.Controls.Add(Me.optExtYes)
        Me.Frame2.Controls.Add(Me.TxtCreateUser)
        Me.Frame2.Controls.Add(Me.TxtAccLevel)
        Me.Frame2.Controls.Add(Me.Label9)
        Me.Frame2.Controls.Add(Me.Label10)
        Me.Frame2.Controls.Add(Me.Label8)
        Me.Frame2.Controls.Add(Me.Label7)
        Me.Frame2.Controls.Add(Me.Label4)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(328, 164)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(257, 153)
        Me.Frame2.TabIndex = 11
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Details"
        '
        'optExtNo
        '
        Me.optExtNo.BackColor = System.Drawing.SystemColors.Control
        Me.optExtNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.optExtNo.Enabled = False
        Me.optExtNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optExtNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optExtNo.Location = New System.Drawing.Point(192, 112)
        Me.optExtNo.Name = "optExtNo"
        Me.optExtNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optExtNo.Size = New System.Drawing.Size(17, 19)
        Me.optExtNo.TabIndex = 25
        Me.optExtNo.TabStop = True
        Me.optExtNo.UseVisualStyleBackColor = False
        '
        'optExtYes
        '
        Me.optExtYes.BackColor = System.Drawing.SystemColors.Control
        Me.optExtYes.Cursor = System.Windows.Forms.Cursors.Default
        Me.optExtYes.Enabled = False
        Me.optExtYes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optExtYes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optExtYes.Location = New System.Drawing.Point(112, 112)
        Me.optExtYes.Name = "optExtYes"
        Me.optExtYes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optExtYes.Size = New System.Drawing.Size(17, 19)
        Me.optExtYes.TabIndex = 24
        Me.optExtYes.TabStop = True
        Me.optExtYes.UseVisualStyleBackColor = False
        '
        'TxtCreateUser
        '
        Me.TxtCreateUser.AcceptsReturn = True
        Me.TxtCreateUser.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtCreateUser.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtCreateUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCreateUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtCreateUser.Location = New System.Drawing.Point(112, 72)
        Me.TxtCreateUser.MaxLength = 0
        Me.TxtCreateUser.Name = "TxtCreateUser"
        Me.TxtCreateUser.ReadOnly = True
        Me.TxtCreateUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtCreateUser.Size = New System.Drawing.Size(113, 20)
        Me.TxtCreateUser.TabIndex = 13
        '
        'TxtAccLevel
        '
        Me.TxtAccLevel.AcceptsReturn = True
        Me.TxtAccLevel.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtAccLevel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtAccLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAccLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtAccLevel.Location = New System.Drawing.Point(112, 32)
        Me.TxtAccLevel.MaxLength = 0
        Me.TxtAccLevel.Name = "TxtAccLevel"
        Me.TxtAccLevel.ReadOnly = True
        Me.TxtAccLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtAccLevel.Size = New System.Drawing.Size(25, 20)
        Me.TxtAccLevel.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(136, 115)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(25, 13)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Yes"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(216, 115)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(25, 17)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "No"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(16, 115)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(105, 25)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "External"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(16, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(113, 17)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Create User"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(113, 17)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Access Level"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.TxtCreateDate)
        Me.Frame3.Controls.Add(Me.TxtExpDate)
        Me.Frame3.Controls.Add(Me.TxtDocDate)
        Me.Frame3.Controls.Add(Me.Label6)
        Me.Frame3.Controls.Add(Me.Label5)
        Me.Frame3.Controls.Add(Me.Label3)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(16, 164)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(297, 153)
        Me.Frame3.TabIndex = 16
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Dates"
        '
        'TxtCreateDate
        '
        Me.TxtCreateDate.AcceptsReturn = True
        Me.TxtCreateDate.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtCreateDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtCreateDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCreateDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtCreateDate.Location = New System.Drawing.Point(112, 112)
        Me.TxtCreateDate.MaxLength = 0
        Me.TxtCreateDate.Name = "TxtCreateDate"
        Me.TxtCreateDate.ReadOnly = True
        Me.TxtCreateDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtCreateDate.Size = New System.Drawing.Size(169, 20)
        Me.TxtCreateDate.TabIndex = 19
        '
        'TxtExpDate
        '
        Me.TxtExpDate.AcceptsReturn = True
        Me.TxtExpDate.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtExpDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtExpDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtExpDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtExpDate.Location = New System.Drawing.Point(112, 72)
        Me.TxtExpDate.MaxLength = 0
        Me.TxtExpDate.Name = "TxtExpDate"
        Me.TxtExpDate.ReadOnly = True
        Me.TxtExpDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtExpDate.Size = New System.Drawing.Size(169, 20)
        Me.TxtExpDate.TabIndex = 18
        '
        'TxtDocDate
        '
        Me.TxtDocDate.AcceptsReturn = True
        Me.TxtDocDate.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtDocDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDocDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtDocDate.Location = New System.Drawing.Point(112, 32)
        Me.TxtDocDate.MaxLength = 0
        Me.TxtDocDate.Name = "TxtDocDate"
        Me.TxtDocDate.ReadOnly = True
        Me.TxtDocDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtDocDate.Size = New System.Drawing.Size(169, 20)
        Me.TxtDocDate.TabIndex = 17
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(16, 115)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(97, 17)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Create Date"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(16, 75)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(113, 17)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Expiry Date"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(16, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(113, 17)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Document Date"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.CmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(621, 424)
        Me.Controls.Add(Me.cmdAnnotations)
        Me.Controls.Add(Me.CmdPassword)
        Me.Controls.Add(Me.CmdKeywords)
        Me.Controls.Add(Me.CmdHelp)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.CmdOK)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Document Information"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class