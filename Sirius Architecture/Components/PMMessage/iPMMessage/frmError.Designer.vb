<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmError
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializetxtError()
		InitializelblCapion()
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
	Public WithEvents cmdMore As System.Windows.Forms.Button
	Public WithEvents cmdLog As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgSimple As System.Windows.Forms.PictureBox
	Public WithEvents lblSimpleError As System.Windows.Forms.Label
	Public WithEvents fraSimple As System.Windows.Forms.GroupBox
	Private WithEvents _txtError_4 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_5 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_7 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_6 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_3 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_2 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_1 As System.Windows.Forms.TextBox
	Private WithEvents _txtError_0 As System.Windows.Forms.TextBox
    Private WithEvents _txtError_8 As System.Windows.Forms.TextBox
    Public WithEvents imgMain As System.Windows.Forms.PictureBox
    Private WithEvents _lblCapion_5 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_7 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_0 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_6 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_4 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_3 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_2 As System.Windows.Forms.Label
    Private WithEvents _lblCapion_1 As System.Windows.Forms.Label
    Public WithEvents fraMain As System.Windows.Forms.GroupBox
    Public WithEvents imlIcons As System.Windows.Forms.ImageList
    Public lblCapion(7) As System.Windows.Forms.Label
    Public txtError(8) As System.Windows.Forms.TextBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmError))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdMore = New System.Windows.Forms.Button
        Me.cmdLog = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.fraSimple = New System.Windows.Forms.GroupBox
        Me.imgSimple = New System.Windows.Forms.PictureBox
        Me.lblSimpleError = New System.Windows.Forms.Label
        Me.fraMain = New System.Windows.Forms.GroupBox
        Me._txtError_8 = New System.Windows.Forms.TextBox
        Me._txtError_4 = New System.Windows.Forms.TextBox
        Me._txtError_5 = New System.Windows.Forms.TextBox
        Me._txtError_7 = New System.Windows.Forms.TextBox
        Me._txtError_6 = New System.Windows.Forms.TextBox
        Me._txtError_3 = New System.Windows.Forms.TextBox
        Me._txtError_2 = New System.Windows.Forms.TextBox
        Me._txtError_1 = New System.Windows.Forms.TextBox
        Me._txtError_0 = New System.Windows.Forms.TextBox
        Me.imgMain = New System.Windows.Forms.PictureBox
        Me._lblCapion_5 = New System.Windows.Forms.Label
        Me._lblCapion_7 = New System.Windows.Forms.Label
        Me._lblCapion_0 = New System.Windows.Forms.Label
        Me._lblCapion_6 = New System.Windows.Forms.Label
        Me._lblCapion_4 = New System.Windows.Forms.Label
        Me._lblCapion_3 = New System.Windows.Forms.Label
        Me._lblCapion_2 = New System.Windows.Forms.Label
        Me._lblCapion_1 = New System.Windows.Forms.Label
        Me.imlIcons = New System.Windows.Forms.ImageList
        Me.fraSimple.SuspendLayout()
        Me.fraMain.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' cmdMore
        ' 
        Me.cmdMore.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMore.CausesValidation = True
        Me.cmdMore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMore.Enabled = True
        Me.cmdMore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdMore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMore.Location = New System.Drawing.Point(390, 394)
        Me.cmdMore.Name = "cmdMore"
        Me.cmdMore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMore.Size = New System.Drawing.Size(73, 22)
        Me.cmdMore.TabIndex = 19
        Me.cmdMore.TabStop = True
        Me.cmdMore.Text = "&More..."
        Me.cmdMore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdMore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' cmdLog
        ' 
        Me.cmdLog.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLog.CausesValidation = True
        Me.cmdLog.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLog.Enabled = True
        Me.cmdLog.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdLog.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLog.Location = New System.Drawing.Point(8, 394)
        Me.cmdLog.Name = "cmdLog"
        Me.cmdLog.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLog.Size = New System.Drawing.Size(73, 22)
        Me.cmdLog.TabIndex = 14
        Me.cmdLog.TabStop = True
        Me.cmdLog.Text = "&Email"
        Me.cmdLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' cmdOK
        ' 
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.CausesValidation = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = True
        Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(470, 394)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.TabStop = True
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        ' 
        ' fraSimple
        ' 
        Me.fraSimple.BackColor = System.Drawing.SystemColors.Control
        Me.fraSimple.Controls.Add(Me.imgSimple)
        Me.fraSimple.Controls.Add(Me.lblSimpleError)
        Me.fraSimple.Enabled = True
        Me.fraSimple.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.fraSimple.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSimple.Location = New System.Drawing.Point(6, 2)
        Me.fraSimple.Name = "fraSimple"
        Me.fraSimple.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSimple.Size = New System.Drawing.Size(537, 125)
        Me.fraSimple.TabIndex = 20
        Me.fraSimple.Visible = True
        ' 
        ' imgSimple
        ' 
        Me.imgSimple.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.imgSimple.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgSimple.Enabled = True
        Me.imgSimple.Location = New System.Drawing.Point(16, 32)
        Me.imgSimple.Name = "imgSimple"
        Me.imgSimple.Size = New System.Drawing.Size(32, 32)
        Me.imgSimple.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
        Me.imgSimple.Visible = True
        ' 
        ' lblSimpleError
        ' 
        Me.lblSimpleError.AutoSize = False
        Me.lblSimpleError.BackColor = System.Drawing.SystemColors.Control
        Me.lblSimpleError.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblSimpleError.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSimpleError.Enabled = True
        Me.lblSimpleError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.lblSimpleError.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSimpleError.Location = New System.Drawing.Point(64, 32)
        Me.lblSimpleError.Name = "lblSimpleError"
        Me.lblSimpleError.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSimpleError.Size = New System.Drawing.Size(457, 83)
        Me.lblSimpleError.TabIndex = 21
        Me.lblSimpleError.Text = "Simple Message"
        Me.lblSimpleError.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblSimpleError.UseMnemonic = True
        Me.lblSimpleError.Visible = True
        ' 
        ' fraMain
        ' 
        Me.fraMain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMain.Controls.Add(Me._txtError_8)
        Me.fraMain.Controls.Add(Me._txtError_4)
        Me.fraMain.Controls.Add(Me._txtError_5)
        Me.fraMain.Controls.Add(Me._txtError_7)
        Me.fraMain.Controls.Add(Me._txtError_6)
        Me.fraMain.Controls.Add(Me._txtError_3)
        Me.fraMain.Controls.Add(Me._txtError_2)
        Me.fraMain.Controls.Add(Me._txtError_1)
        Me.fraMain.Controls.Add(Me._txtError_0)
        Me.fraMain.Controls.Add(Me.imgMain)
        Me.fraMain.Controls.Add(Me._lblCapion_5)
        Me.fraMain.Controls.Add(Me._lblCapion_7)
        Me.fraMain.Controls.Add(Me._lblCapion_0)
        Me.fraMain.Controls.Add(Me._lblCapion_6)
        Me.fraMain.Controls.Add(Me._lblCapion_4)
        Me.fraMain.Controls.Add(Me._lblCapion_3)
        Me.fraMain.Controls.Add(Me._lblCapion_2)
        Me.fraMain.Controls.Add(Me._lblCapion_1)
        Me.fraMain.Enabled = True
        Me.fraMain.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.fraMain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMain.Location = New System.Drawing.Point(6, 2)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMain.Size = New System.Drawing.Size(537, 385)
        Me.fraMain.TabIndex = 0
        Me.fraMain.Visible = True
        ' 
        ' _txtError_4
        ' 
        Me._txtError_4.AcceptsReturn = True
        Me._txtError_4.AutoSize = False
        Me._txtError_4.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_4.CausesValidation = True
        Me._txtError_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_4.Enabled = True
        Me._txtError_4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_4.HideSelection = True
        Me._txtError_4.Location = New System.Drawing.Point(208, 184)
        Me._txtError_4.MaxLength = 0
        Me._txtError_4.Multiline = True
        Me._txtError_4.Name = "_txtError_4"
        Me._txtError_4.ReadOnly = True
        Me._txtError_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_4.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_4.Size = New System.Drawing.Size(313, 63)
        Me._txtError_4.TabIndex = 12
        Me._txtError_4.TabStop = True
        Me._txtError_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_4.Visible = True
        ' 
        ' _txtError_5
        ' 
        Me._txtError_5.AcceptsReturn = True
        Me._txtError_5.AutoSize = False
        Me._txtError_5.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_5.CausesValidation = True
        Me._txtError_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_5.Enabled = True
        Me._txtError_5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_5.HideSelection = True
        Me._txtError_5.Location = New System.Drawing.Point(208, 250)
        Me._txtError_5.MaxLength = 0
        Me._txtError_5.Multiline = False
        Me._txtError_5.Name = "_txtError_5"
        Me._txtError_5.ReadOnly = True
        Me._txtError_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_5.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_5.Size = New System.Drawing.Size(313, 19)
        Me._txtError_5.TabIndex = 17
        Me._txtError_5.TabStop = True
        Me._txtError_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_5.Visible = True
        ' 
        ' _txtError_7
        ' 
        Me._txtError_7.AcceptsReturn = True
        Me._txtError_7.AutoSize = False
        Me._txtError_7.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_7.CausesValidation = True
        Me._txtError_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_7.Enabled = True
        Me._txtError_7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_7.HideSelection = True
        Me._txtError_7.Location = New System.Drawing.Point(208, 32)
        Me._txtError_7.MaxLength = 0
        Me._txtError_7.Multiline = False
        Me._txtError_7.Name = "_txtError_7"
        Me._txtError_7.ReadOnly = True
        Me._txtError_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_7.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_7.Size = New System.Drawing.Size(241, 19)
        Me._txtError_7.TabIndex = 15
        Me._txtError_7.TabStop = True
        Me._txtError_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_7.Visible = True
        ' 
        ' _txtError_6
        ' 
        Me._txtError_6.AcceptsReturn = True
        Me._txtError_6.AutoSize = False
        Me._txtError_6.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_6.CausesValidation = True
        Me._txtError_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_6.Enabled = True
        Me._txtError_6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_6.HideSelection = True
        Me._txtError_6.Location = New System.Drawing.Point(208, 270)
        Me._txtError_6.MaxLength = 0
        Me._txtError_6.Multiline = True
        Me._txtError_6.Name = "_txtError_6"
        Me._txtError_6.ReadOnly = True
        Me._txtError_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_6.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_6.Size = New System.Drawing.Size(313, 110)
        Me._txtError_6.TabIndex = 13
        Me._txtError_6.TabStop = True
        Me._txtError_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_6.Visible = True
        ' 
        ' _txtError_3
        ' 
        Me._txtError_3.AcceptsReturn = True
        Me._txtError_3.AutoSize = False
        Me._txtError_3.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_3.CausesValidation = True
        Me._txtError_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_3.Enabled = True
        Me._txtError_3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_3.HideSelection = True
        Me._txtError_3.Location = New System.Drawing.Point(208, 160)
        Me._txtError_3.MaxLength = 0
        Me._txtError_3.Multiline = True
        Me._txtError_3.Name = "_txtError_3"
        Me._txtError_3.ReadOnly = True
        Me._txtError_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_3.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_3.Size = New System.Drawing.Size(313, 19)
        Me._txtError_3.TabIndex = 11
        Me._txtError_3.TabStop = True
        Me._txtError_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_3.Visible = True
        ' 
        ' _txtError_2
        ' 
        Me._txtError_2.AcceptsReturn = True
        Me._txtError_2.AutoSize = False
        Me._txtError_2.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_2.CausesValidation = True
        Me._txtError_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_2.Enabled = True
        Me._txtError_2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_2.HideSelection = True
        Me._txtError_2.Location = New System.Drawing.Point(208, 136)
        Me._txtError_2.MaxLength = 0
        Me._txtError_2.Multiline = True
        Me._txtError_2.Name = "_txtError_2"
        Me._txtError_2.ReadOnly = True
        Me._txtError_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_2.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_2.Size = New System.Drawing.Size(313, 19)
        Me._txtError_2.TabIndex = 10
        Me._txtError_2.TabStop = True
        Me._txtError_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_2.Visible = True
        ' 
        ' _txtError_1
        ' 
        Me._txtError_1.AcceptsReturn = True
        Me._txtError_1.AutoSize = False
        Me._txtError_1.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_1.CausesValidation = True
        Me._txtError_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_1.Enabled = True
        Me._txtError_1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_1.HideSelection = True
        Me._txtError_1.Location = New System.Drawing.Point(208, 80)
        Me._txtError_1.MaxLength = 0
        Me._txtError_1.Multiline = True
        Me._txtError_1.Name = "_txtError_1"
        Me._txtError_1.ReadOnly = True
        Me._txtError_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_1.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_1.Size = New System.Drawing.Size(241, 19)
        Me._txtError_1.TabIndex = 9
        Me._txtError_1.TabStop = True
        Me._txtError_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_1.Visible = True
        ' 
        ' _txtError_0
        ' 
        Me._txtError_0.AcceptsReturn = True
        Me._txtError_0.AutoSize = False
        Me._txtError_0.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_0.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_0.CausesValidation = True
        Me._txtError_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_0.Enabled = True
        Me._txtError_0.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_0.HideSelection = True
        Me._txtError_0.Location = New System.Drawing.Point(208, 56)
        Me._txtError_0.MaxLength = 0
        Me._txtError_0.Multiline = False
        Me._txtError_0.Name = "_txtError_0"
        Me._txtError_0.ReadOnly = True
        Me._txtError_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_0.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_0.Size = New System.Drawing.Size(241, 19)
        Me._txtError_0.TabIndex = 7
        Me._txtError_0.TabStop = True
        Me._txtError_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_0.Visible = True
        ' 
        ' _txtError_8
        ' 
        Me._txtError_8.AcceptsReturn = True
        Me._txtError_8.AutoSize = False
        Me._txtError_8.BackColor = System.Drawing.SystemColors.Menu
        Me._txtError_8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtError_8.CausesValidation = True
        Me._txtError_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtError_8.Enabled = True
        Me._txtError_8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._txtError_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtError_8.HideSelection = True
        Me._txtError_8.Location = New System.Drawing.Point(67, 258)
        Me._txtError_8.MaxLength = 0
        Me._txtError_8.Multiline = False
        Me._txtError_8.Name = "_txtError_8"
        Me._txtError_8.ReadOnly = True
        Me._txtError_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtError_8.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me._txtError_8.Size = New System.Drawing.Size(454, 14)
        Me._txtError_8.TabIndex = 19
        Me._txtError_8.TabStop = True
        Me._txtError_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me._txtError_8.Visible = False
        ' 
        ' imgMain
        ' 
        Me.imgMain.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.imgMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgMain.Enabled = True
        Me.imgMain.Location = New System.Drawing.Point(16, 32)
        Me.imgMain.Name = "imgMain"
        Me.imgMain.Size = New System.Drawing.Size(32, 32)
        Me.imgMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
        Me.imgMain.Visible = True
        ' 
        ' _lblCapion_5
        ' 
        Me._lblCapion_5.AutoSize = True
        Me._lblCapion_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_5.Enabled = True
        Me._lblCapion_5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_5.Location = New System.Drawing.Point(64, 250)
        Me._lblCapion_5.Name = "_lblCapion_5"
        Me._lblCapion_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_5.Size = New System.Drawing.Size(90, 13)
        Me._lblCapion_5.TabIndex = 18
        Me._lblCapion_5.Text = "Error Number:"
        Me._lblCapion_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_5.UseMnemonic = True
        Me._lblCapion_5.Visible = True
        ' 
        ' _lblCapion_7
        ' 
        Me._lblCapion_7.AutoSize = True
        Me._lblCapion_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_7.Enabled = True
        Me._lblCapion_7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_7.Location = New System.Drawing.Point(64, 32)
        Me._lblCapion_7.Name = "_lblCapion_7"
        Me._lblCapion_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_7.Size = New System.Drawing.Size(33, 13)
        Me._lblCapion_7.TabIndex = 16
        Me._lblCapion_7.Text = "User:"
        Me._lblCapion_7.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_7.UseMnemonic = True
        Me._lblCapion_7.Visible = True
        ' 
        ' _lblCapion_0
        ' 
        Me._lblCapion_0.AutoSize = True
        Me._lblCapion_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_0.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_0.Enabled = True
        Me._lblCapion_0.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_0.Location = New System.Drawing.Point(64, 56)
        Me._lblCapion_0.Name = "_lblCapion_0"
        Me._lblCapion_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_0.Size = New System.Drawing.Size(69, 13)
        Me._lblCapion_0.TabIndex = 8
        Me._lblCapion_0.Text = "Date / Time :"
        Me._lblCapion_0.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_0.UseMnemonic = True
        Me._lblCapion_0.Visible = True
        ' 
        ' _lblCapion_6
        ' 
        Me._lblCapion_6.AutoSize = True
        Me._lblCapion_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_6.Enabled = True
        Me._lblCapion_6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_6.Location = New System.Drawing.Point(64, 270)
        Me._lblCapion_6.Name = "_lblCapion_6"
        Me._lblCapion_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_6.Size = New System.Drawing.Size(95, 13)
        Me._lblCapion_6.TabIndex = 6
        Me._lblCapion_6.Text = "Error Message:"
        Me._lblCapion_6.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_6.UseMnemonic = True
        Me._lblCapion_6.Visible = True
        ' 
        ' _lblCapion_4
        ' 
        Me._lblCapion_4.AutoSize = True
        Me._lblCapion_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_4.Enabled = True
        Me._lblCapion_4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_4.Location = New System.Drawing.Point(64, 184)
        Me._lblCapion_4.Name = "_lblCapion_4"
        Me._lblCapion_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_4.Size = New System.Drawing.Size(135, 13)
        Me._lblCapion_4.TabIndex = 5
        Me._lblCapion_4.Text = "Application Error Message:"
        Me._lblCapion_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_4.UseMnemonic = True
        Me._lblCapion_4.Visible = True
        ' 
        ' _lblCapion_3
        ' 
        Me._lblCapion_3.AutoSize = True
        Me._lblCapion_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_3.Enabled = True
        Me._lblCapion_3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_3.Location = New System.Drawing.Point(64, 160)
        Me._lblCapion_3.Name = "_lblCapion_3"
        Me._lblCapion_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_3.Size = New System.Drawing.Size(47, 13)
        Me._lblCapion_3.TabIndex = 4
        Me._lblCapion_3.Text = "Method:"
        Me._lblCapion_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_3.UseMnemonic = True
        Me._lblCapion_3.Visible = True
        ' 
        ' _lblCapion_2
        ' 
        Me._lblCapion_2.AutoSize = True
        Me._lblCapion_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_2.Enabled = True
        Me._lblCapion_2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_2.Location = New System.Drawing.Point(64, 136)
        Me._lblCapion_2.Name = "_lblCapion_2"
        Me._lblCapion_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_2.Size = New System.Drawing.Size(36, 13)
        Me._lblCapion_2.TabIndex = 3
        Me._lblCapion_2.Text = "Class:"
        Me._lblCapion_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_2.UseMnemonic = True
        Me._lblCapion_2.Visible = True
        ' 
        ' _lblCapion_1
        ' 
        Me._lblCapion_1.AutoSize = True
        Me._lblCapion_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblCapion_1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._lblCapion_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCapion_1.Enabled = True
        Me._lblCapion_1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me._lblCapion_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCapion_1.Location = New System.Drawing.Point(64, 80)
        Me._lblCapion_1.Name = "_lblCapion_1"
        Me._lblCapion_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCapion_1.Size = New System.Drawing.Size(63, 13)
        Me._lblCapion_1.TabIndex = 2
        Me._lblCapion_1.Text = "Application:"
        Me._lblCapion_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me._lblCapion_1.UseMnemonic = True
        Me._lblCapion_1.Visible = True
        ' 
        ' imlIcons
        ' 
        Me.imlIcons.ImageSize = New System.Drawing.Size(32, 32)
        Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
        Me.imlIcons.Images.SetKeyName(0, "information")
        Me.imlIcons.Images.SetKeyName(1, "question")
        Me.imlIcons.Images.SetKeyName(2, "stop")
        Me.imlIcons.Images.SetKeyName(3, "exclamation")
        ' 
        ' frmError
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(551, 422)
        Me.ControlBox = True
        Me.Controls.Add(Me.cmdMore)
        Me.Controls.Add(Me.cmdLog)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraSimple)
        Me.Controls.Add(Me.fraMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Enabled = True
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = False
        Me.Icon = CType(resources.GetObject("frmError.Icon"), System.Drawing.Icon)
        Me.KeyPreview = False
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmError"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "There has been an error"
        Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        Me.fraSimple.ResumeLayout(False)
        Me.fraMain.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Sub InitializetxtError()
        Me.txtError(4) = _txtError_4
        Me.txtError(5) = _txtError_5
        Me.txtError(7) = _txtError_7
        Me.txtError(6) = _txtError_6
        Me.txtError(3) = _txtError_3
        Me.txtError(2) = _txtError_2
        Me.txtError(1) = _txtError_1
        Me.txtError(0) = _txtError_0
        Me.txtError(8) = _txtError_8
    End Sub
	Sub InitializelblCapion()
		Me.lblCapion(5) = _lblCapion_5
		Me.lblCapion(7) = _lblCapion_7
		Me.lblCapion(0) = _lblCapion_0
		Me.lblCapion(6) = _lblCapion_6
		Me.lblCapion(4) = _lblCapion_4
		Me.lblCapion(3) = _lblCapion_3
		Me.lblCapion(2) = _lblCapion_2
		Me.lblCapion(1) = _lblCapion_1
	End Sub
#End Region 
End Class