<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccountsAge
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelblStart()
		InitializelblBand()
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
	Public WithEvents Text23 As System.Windows.Forms.TextBox
	Public WithEvents Text22 As System.Windows.Forms.TextBox
	Public WithEvents Text21 As System.Windows.Forms.TextBox
	Public WithEvents Text20 As System.Windows.Forms.TextBox
	Public WithEvents Text19 As System.Windows.Forms.TextBox
	Public WithEvents Text18 As System.Windows.Forms.TextBox
	Public WithEvents Text17 As System.Windows.Forms.TextBox
	Public WithEvents Text16 As System.Windows.Forms.TextBox
	Public WithEvents Text15 As System.Windows.Forms.TextBox
	Public WithEvents Text14 As System.Windows.Forms.TextBox
	Public WithEvents Text13 As System.Windows.Forms.TextBox
	Public WithEvents Text12 As System.Windows.Forms.TextBox
	Public WithEvents Text11 As System.Windows.Forms.TextBox
	Private WithEvents _lblBand_11 As System.Windows.Forms.Label
	Private WithEvents _lblBand_10 As System.Windows.Forms.Label
	Private WithEvents _lblBand_9 As System.Windows.Forms.Label
	Private WithEvents _lblBand_8 As System.Windows.Forms.Label
	Private WithEvents _lblBand_7 As System.Windows.Forms.Label
	Private WithEvents _lblBand_6 As System.Windows.Forms.Label
	Private WithEvents _lblStart_3 As System.Windows.Forms.Label
	Private WithEvents _lblStart_2 As System.Windows.Forms.Label
	Private WithEvents _lblBand_5 As System.Windows.Forms.Label
	Public lblBand(11) As System.Windows.Forms.Label
	Public lblStart(3) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Text23 = New System.Windows.Forms.TextBox()
        Me.Text22 = New System.Windows.Forms.TextBox()
        Me.Text21 = New System.Windows.Forms.TextBox()
        Me.Text20 = New System.Windows.Forms.TextBox()
        Me.Text19 = New System.Windows.Forms.TextBox()
        Me.Text18 = New System.Windows.Forms.TextBox()
        Me.Text17 = New System.Windows.Forms.TextBox()
        Me.Text16 = New System.Windows.Forms.TextBox()
        Me.Text15 = New System.Windows.Forms.TextBox()
        Me.Text14 = New System.Windows.Forms.TextBox()
        Me.Text13 = New System.Windows.Forms.TextBox()
        Me.Text12 = New System.Windows.Forms.TextBox()
        Me.Text11 = New System.Windows.Forms.TextBox()
        Me._lblBand_11 = New System.Windows.Forms.Label()
        Me._lblBand_10 = New System.Windows.Forms.Label()
        Me._lblBand_9 = New System.Windows.Forms.Label()
        Me._lblBand_8 = New System.Windows.Forms.Label()
        Me._lblBand_7 = New System.Windows.Forms.Label()
        Me._lblBand_6 = New System.Windows.Forms.Label()
        Me._lblStart_3 = New System.Windows.Forms.Label()
        Me._lblStart_2 = New System.Windows.Forms.Label()
        Me._lblBand_5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Text23
        '
        Me.Text23.AcceptsReturn = True
        Me.Text23.AccessibleDescription = "Age Analysis Bands: Band #7: Start Range:"
        Me.Text23.BackColor = System.Drawing.SystemColors.Window
        Me.Text23.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text23.Location = New System.Drawing.Point(69, 199)
        Me.Text23.MaxLength = 0
        Me.Text23.Name = "Text23"
        Me.Text23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text23.Size = New System.Drawing.Size(80, 20)
        Me.Text23.TabIndex = 12
        Me.Text23.Tag = "3022,ValidateNumeric,M"
        Me.Text23.Text = "366"
        Me.Text23.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text22
        '
        Me.Text22.AcceptsReturn = True
        Me.Text22.AccessibleDescription = "Age Analysis Bands: Band #6: End Range:"
        Me.Text22.BackColor = System.Drawing.SystemColors.Window
        Me.Text22.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text22.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text22.Location = New System.Drawing.Point(163, 171)
        Me.Text22.MaxLength = 0
        Me.Text22.Name = "Text22"
        Me.Text22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text22.Size = New System.Drawing.Size(80, 20)
        Me.Text22.TabIndex = 11
        Me.Text22.Tag = "3021,ValidateNumeric,M"
        Me.Text22.Text = "365"
        Me.Text22.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text21
        '
        Me.Text21.AcceptsReturn = True
        Me.Text21.AccessibleDescription = "Age Analysis Bands: Band #6: Start Range:"
        Me.Text21.BackColor = System.Drawing.SystemColors.Window
        Me.Text21.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text21.Location = New System.Drawing.Point(69, 171)
        Me.Text21.MaxLength = 0
        Me.Text21.Name = "Text21"
        Me.Text21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text21.Size = New System.Drawing.Size(80, 20)
        Me.Text21.TabIndex = 10
        Me.Text21.Tag = "3020,ValidateNumeric,M"
        Me.Text21.Text = "181"
        Me.Text21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text20
        '
        Me.Text20.AcceptsReturn = True
        Me.Text20.AccessibleDescription = "Age Analysis Bands: Band #5: End Range:"
        Me.Text20.BackColor = System.Drawing.SystemColors.Window
        Me.Text20.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text20.Location = New System.Drawing.Point(163, 143)
        Me.Text20.MaxLength = 0
        Me.Text20.Name = "Text20"
        Me.Text20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text20.Size = New System.Drawing.Size(80, 20)
        Me.Text20.TabIndex = 9
        Me.Text20.Tag = "3019,ValidateNumeric,M"
        Me.Text20.Text = "180"
        Me.Text20.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text19
        '
        Me.Text19.AcceptsReturn = True
        Me.Text19.AccessibleDescription = "Age Analysis Bands: Band #5: Start Range:"
        Me.Text19.BackColor = System.Drawing.SystemColors.Window
        Me.Text19.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text19.Location = New System.Drawing.Point(69, 143)
        Me.Text19.MaxLength = 0
        Me.Text19.Name = "Text19"
        Me.Text19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text19.Size = New System.Drawing.Size(80, 20)
        Me.Text19.TabIndex = 8
        Me.Text19.Tag = "3018,ValidateNumeric,M"
        Me.Text19.Text = "121"
        Me.Text19.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text18
        '
        Me.Text18.AcceptsReturn = True
        Me.Text18.AccessibleDescription = "Age Analysis Bands: Band #4: End Range:"
        Me.Text18.BackColor = System.Drawing.SystemColors.Window
        Me.Text18.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text18.Location = New System.Drawing.Point(163, 115)
        Me.Text18.MaxLength = 0
        Me.Text18.Name = "Text18"
        Me.Text18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text18.Size = New System.Drawing.Size(80, 20)
        Me.Text18.TabIndex = 7
        Me.Text18.Tag = "3017,ValidateNumeric,M"
        Me.Text18.Text = "120"
        Me.Text18.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text17
        '
        Me.Text17.AcceptsReturn = True
        Me.Text17.AccessibleDescription = "Age Analysis Bands: Band #4: Start Range:"
        Me.Text17.BackColor = System.Drawing.SystemColors.Window
        Me.Text17.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text17.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text17.Location = New System.Drawing.Point(69, 115)
        Me.Text17.MaxLength = 0
        Me.Text17.Name = "Text17"
        Me.Text17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text17.Size = New System.Drawing.Size(80, 20)
        Me.Text17.TabIndex = 6
        Me.Text17.Tag = "3016,ValidateNumeric,M"
        Me.Text17.Text = "91"
        Me.Text17.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text16
        '
        Me.Text16.AcceptsReturn = True
        Me.Text16.AccessibleDescription = "Age Analysis Bands: Band #3: End Range:"
        Me.Text16.BackColor = System.Drawing.SystemColors.Window
        Me.Text16.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text16.Location = New System.Drawing.Point(163, 87)
        Me.Text16.MaxLength = 0
        Me.Text16.Name = "Text16"
        Me.Text16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text16.Size = New System.Drawing.Size(80, 20)
        Me.Text16.TabIndex = 5
        Me.Text16.Tag = "3015,ValidateNumeric,M"
        Me.Text16.Text = "90"
        Me.Text16.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text15
        '
        Me.Text15.AcceptsReturn = True
        Me.Text15.AccessibleDescription = "Age Analysis Bands: Band #3: Start Range:"
        Me.Text15.BackColor = System.Drawing.SystemColors.Window
        Me.Text15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text15.Location = New System.Drawing.Point(69, 87)
        Me.Text15.MaxLength = 0
        Me.Text15.Name = "Text15"
        Me.Text15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text15.Size = New System.Drawing.Size(80, 20)
        Me.Text15.TabIndex = 4
        Me.Text15.Tag = "3014,ValidateNumeric,M"
        Me.Text15.Text = "61"
        Me.Text15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text14
        '
        Me.Text14.AcceptsReturn = True
        Me.Text14.AccessibleDescription = "Age Analysis Bands: Band #2: End Range:"
        Me.Text14.BackColor = System.Drawing.SystemColors.Window
        Me.Text14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text14.Location = New System.Drawing.Point(163, 59)
        Me.Text14.MaxLength = 0
        Me.Text14.Name = "Text14"
        Me.Text14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text14.Size = New System.Drawing.Size(80, 20)
        Me.Text14.TabIndex = 3
        Me.Text14.Tag = "3013,ValidateNumeric,M"
        Me.Text14.Text = "60"
        Me.Text14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text13
        '
        Me.Text13.AcceptsReturn = True
        Me.Text13.AccessibleDescription = "Age Analysis Bands: Band #2: Start Range:"
        Me.Text13.BackColor = System.Drawing.SystemColors.Window
        Me.Text13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text13.Location = New System.Drawing.Point(69, 59)
        Me.Text13.MaxLength = 0
        Me.Text13.Name = "Text13"
        Me.Text13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text13.Size = New System.Drawing.Size(80, 20)
        Me.Text13.TabIndex = 2
        Me.Text13.Tag = "3012,ValidateNumeric,M"
        Me.Text13.Text = "31"
        Me.Text13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text12
        '
        Me.Text12.AcceptsReturn = True
        Me.Text12.AccessibleDescription = "Age Analysis Bands: Band #1: End Range:"
        Me.Text12.BackColor = System.Drawing.SystemColors.Window
        Me.Text12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text12.Location = New System.Drawing.Point(163, 31)
        Me.Text12.MaxLength = 0
        Me.Text12.Name = "Text12"
        Me.Text12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text12.Size = New System.Drawing.Size(80, 20)
        Me.Text12.TabIndex = 1
        Me.Text12.Tag = "3011,ValidateNumeric,M"
        Me.Text12.Text = "30"
        Me.Text12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text11
        '
        Me.Text11.AcceptsReturn = True
        Me.Text11.AccessibleDescription = "Age Analysis Bands: Band #1: Start Range:"
        Me.Text11.BackColor = System.Drawing.SystemColors.Window
        Me.Text11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text11.Location = New System.Drawing.Point(69, 31)
        Me.Text11.MaxLength = 0
        Me.Text11.Name = "Text11"
        Me.Text11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text11.Size = New System.Drawing.Size(80, 20)
        Me.Text11.TabIndex = 0
        Me.Text11.Tag = "3010,ValidateNumeric,M"
        Me.Text11.Text = "0"
        Me.Text11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_lblBand_11
        '
        Me._lblBand_11.AutoSize = True
        Me._lblBand_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_11.Location = New System.Drawing.Point(7, 203)
        Me._lblBand_11.Name = "_lblBand_11"
        Me._lblBand_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_11.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_11.TabIndex = 21
        Me._lblBand_11.Tag = "60,M"
        Me._lblBand_11.Text = "Band #7:"
        '
        '_lblBand_10
        '
        Me._lblBand_10.AutoSize = True
        Me._lblBand_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_10.Location = New System.Drawing.Point(7, 175)
        Me._lblBand_10.Name = "_lblBand_10"
        Me._lblBand_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_10.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_10.TabIndex = 20
        Me._lblBand_10.Tag = "60,M"
        Me._lblBand_10.Text = "Band #6:"
        '
        '_lblBand_9
        '
        Me._lblBand_9.AutoSize = True
        Me._lblBand_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_9.Location = New System.Drawing.Point(7, 147)
        Me._lblBand_9.Name = "_lblBand_9"
        Me._lblBand_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_9.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_9.TabIndex = 19
        Me._lblBand_9.Tag = "60,M"
        Me._lblBand_9.Text = "Band #5:"
        '
        '_lblBand_8
        '
        Me._lblBand_8.AutoSize = True
        Me._lblBand_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_8.Location = New System.Drawing.Point(7, 119)
        Me._lblBand_8.Name = "_lblBand_8"
        Me._lblBand_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_8.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_8.TabIndex = 18
        Me._lblBand_8.Tag = "60,M"
        Me._lblBand_8.Text = "Band #4:"
        '
        '_lblBand_7
        '
        Me._lblBand_7.AutoSize = True
        Me._lblBand_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_7.Location = New System.Drawing.Point(7, 91)
        Me._lblBand_7.Name = "_lblBand_7"
        Me._lblBand_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_7.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_7.TabIndex = 17
        Me._lblBand_7.Tag = "60,M"
        Me._lblBand_7.Text = "Band #3:"
        '
        '_lblBand_6
        '
        Me._lblBand_6.AutoSize = True
        Me._lblBand_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_6.Location = New System.Drawing.Point(7, 63)
        Me._lblBand_6.Name = "_lblBand_6"
        Me._lblBand_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_6.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_6.TabIndex = 16
        Me._lblBand_6.Tag = "60,M"
        Me._lblBand_6.Text = "Band #2:"
        '
        '_lblStart_3
        '
        Me._lblStart_3.AutoSize = True
        Me._lblStart_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblStart_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStart_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblStart_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStart_3.Location = New System.Drawing.Point(160, 9)
        Me._lblStart_3.Name = "_lblStart_3"
        Me._lblStart_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStart_3.Size = New System.Drawing.Size(68, 13)
        Me._lblStart_3.TabIndex = 15
        Me._lblStart_3.Tag = "60,M"
        Me._lblStart_3.Text = "End Range"
        '
        '_lblStart_2
        '
        Me._lblStart_2.AutoSize = True
        Me._lblStart_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblStart_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStart_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblStart_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStart_2.Location = New System.Drawing.Point(66, 9)
        Me._lblStart_2.Name = "_lblStart_2"
        Me._lblStart_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStart_2.Size = New System.Drawing.Size(75, 13)
        Me._lblStart_2.TabIndex = 14
        Me._lblStart_2.Tag = "60,M"
        Me._lblStart_2.Text = "Start Range"
        '
        '_lblBand_5
        '
        Me._lblBand_5.AutoSize = True
        Me._lblBand_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_5.Location = New System.Drawing.Point(7, 35)
        Me._lblBand_5.Name = "_lblBand_5"
        Me._lblBand_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_5.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_5.TabIndex = 13
        Me._lblBand_5.Tag = "60,M"
        Me._lblBand_5.Text = "Band #1:"
        '
        'frmAccountsAge
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(578, 395)
        Me.Controls.Add(Me.Text23)
        Me.Controls.Add(Me.Text22)
        Me.Controls.Add(Me.Text21)
        Me.Controls.Add(Me.Text20)
        Me.Controls.Add(Me.Text19)
        Me.Controls.Add(Me.Text18)
        Me.Controls.Add(Me.Text17)
        Me.Controls.Add(Me.Text16)
        Me.Controls.Add(Me.Text15)
        Me.Controls.Add(Me.Text14)
        Me.Controls.Add(Me.Text13)
        Me.Controls.Add(Me.Text12)
        Me.Controls.Add(Me.Text11)
        Me.Controls.Add(Me._lblBand_11)
        Me.Controls.Add(Me._lblBand_10)
        Me.Controls.Add(Me._lblBand_9)
        Me.Controls.Add(Me._lblBand_8)
        Me.Controls.Add(Me._lblBand_7)
        Me.Controls.Add(Me._lblBand_6)
        Me.Controls.Add(Me._lblStart_3)
        Me.Controls.Add(Me._lblStart_2)
        Me.Controls.Add(Me._lblBand_5)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmAccountsAge"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblStart()
		Me.lblStart(3) = _lblStart_3
		Me.lblStart(2) = _lblStart_2
	End Sub
	Sub InitializelblBand()
		Me.lblBand(11) = _lblBand_11
		Me.lblBand(10) = _lblBand_10
		Me.lblBand(9) = _lblBand_9
		Me.lblBand(8) = _lblBand_8
		Me.lblBand(7) = _lblBand_7
		Me.lblBand(6) = _lblBand_6
		Me.lblBand(5) = _lblBand_5
	End Sub
#End Region 
End Class