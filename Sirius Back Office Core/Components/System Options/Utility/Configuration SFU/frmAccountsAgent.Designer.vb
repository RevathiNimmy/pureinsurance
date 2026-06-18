<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccountsAgent
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
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents Text2 As System.Windows.Forms.TextBox
	Public WithEvents Text3 As System.Windows.Forms.TextBox
	Public WithEvents Text4 As System.Windows.Forms.TextBox
	Public WithEvents Text5 As System.Windows.Forms.TextBox
	Public WithEvents Text6 As System.Windows.Forms.TextBox
	Public WithEvents Text7 As System.Windows.Forms.TextBox
	Public WithEvents Text8 As System.Windows.Forms.TextBox
	Public WithEvents Text9 As System.Windows.Forms.TextBox
	Private WithEvents _lblBand_0 As System.Windows.Forms.Label
	Private WithEvents _lblStart_0 As System.Windows.Forms.Label
	Private WithEvents _lblStart_1 As System.Windows.Forms.Label
	Private WithEvents _lblBand_1 As System.Windows.Forms.Label
	Private WithEvents _lblBand_2 As System.Windows.Forms.Label
	Private WithEvents _lblBand_3 As System.Windows.Forms.Label
	Private WithEvents _lblBand_4 As System.Windows.Forms.Label
	Public lblBand(4) As System.Windows.Forms.Label
	Public lblStart(1) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Text1 = New System.Windows.Forms.TextBox()
        Me.Text2 = New System.Windows.Forms.TextBox()
        Me.Text3 = New System.Windows.Forms.TextBox()
        Me.Text4 = New System.Windows.Forms.TextBox()
        Me.Text5 = New System.Windows.Forms.TextBox()
        Me.Text6 = New System.Windows.Forms.TextBox()
        Me.Text7 = New System.Windows.Forms.TextBox()
        Me.Text8 = New System.Windows.Forms.TextBox()
        Me.Text9 = New System.Windows.Forms.TextBox()
        Me._lblBand_0 = New System.Windows.Forms.Label()
        Me._lblStart_0 = New System.Windows.Forms.Label()
        Me._lblStart_1 = New System.Windows.Forms.Label()
        Me._lblBand_1 = New System.Windows.Forms.Label()
        Me._lblBand_2 = New System.Windows.Forms.Label()
        Me._lblBand_3 = New System.Windows.Forms.Label()
        Me._lblBand_4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
        Me.Text1.AccessibleDescription = "Agent Analysis Bands: Band #1: Start Range:"
        Me.Text1.BackColor = System.Drawing.SystemColors.Window
        Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text1.Location = New System.Drawing.Point(73, 32)
        Me.Text1.MaxLength = 3
        Me.Text1.Name = "Text1"
        Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text1.Size = New System.Drawing.Size(80, 20)
        Me.Text1.TabIndex = 0
        Me.Text1.Tag = "3001,ValidateNumeric,M"
        Me.Text1.Text = "0"
        Me.Text1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text2
        '
        Me.Text2.AcceptsReturn = True
        Me.Text2.AccessibleDescription = "Agent Analysis Bands: Band #1: End Range:"
        Me.Text2.BackColor = System.Drawing.SystemColors.Window
        Me.Text2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text2.Location = New System.Drawing.Point(163, 32)
        Me.Text2.MaxLength = 0
        Me.Text2.Name = "Text2"
        Me.Text2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text2.Size = New System.Drawing.Size(80, 20)
        Me.Text2.TabIndex = 1
        Me.Text2.Tag = "3002,ValidateNumeric,M"
        Me.Text2.Text = "30"
        Me.Text2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text3
        '
        Me.Text3.AcceptsReturn = True
        Me.Text3.AccessibleDescription = "Agent Analysis Bands: Band #2: Start Range:"
        Me.Text3.BackColor = System.Drawing.SystemColors.Window
        Me.Text3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text3.Location = New System.Drawing.Point(73, 60)
        Me.Text3.MaxLength = 0
        Me.Text3.Name = "Text3"
        Me.Text3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text3.Size = New System.Drawing.Size(80, 20)
        Me.Text3.TabIndex = 2
        Me.Text3.Tag = "3003,ValidateNumeric,M"
        Me.Text3.Text = "31"
        Me.Text3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text4
        '
        Me.Text4.AcceptsReturn = True
        Me.Text4.AccessibleDescription = "Agent Analysis Bands: Band #2: End Range:"
        Me.Text4.BackColor = System.Drawing.SystemColors.Window
        Me.Text4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text4.Location = New System.Drawing.Point(163, 60)
        Me.Text4.MaxLength = 0
        Me.Text4.Name = "Text4"
        Me.Text4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text4.Size = New System.Drawing.Size(80, 20)
        Me.Text4.TabIndex = 3
        Me.Text4.Tag = "3004,ValidateNumeric,M"
        Me.Text4.Text = "60"
        Me.Text4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text5
        '
        Me.Text5.AcceptsReturn = True
        Me.Text5.AccessibleDescription = "Agent Analysis Bands: Band #3: Start Range:"
        Me.Text5.BackColor = System.Drawing.SystemColors.Window
        Me.Text5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text5.Location = New System.Drawing.Point(73, 88)
        Me.Text5.MaxLength = 0
        Me.Text5.Name = "Text5"
        Me.Text5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text5.Size = New System.Drawing.Size(80, 20)
        Me.Text5.TabIndex = 4
        Me.Text5.Tag = "3005,ValidateNumeric,M"
        Me.Text5.Text = "61"
        Me.Text5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text6
        '
        Me.Text6.AcceptsReturn = True
        Me.Text6.AccessibleDescription = "Agent Analysis Bands: Band #3: End Range:"
        Me.Text6.BackColor = System.Drawing.SystemColors.Window
        Me.Text6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text6.Location = New System.Drawing.Point(163, 88)
        Me.Text6.MaxLength = 0
        Me.Text6.Name = "Text6"
        Me.Text6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text6.Size = New System.Drawing.Size(80, 20)
        Me.Text6.TabIndex = 5
        Me.Text6.Tag = "3006,ValidateNumeric,M"
        Me.Text6.Text = "90"
        Me.Text6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text7
        '
        Me.Text7.AcceptsReturn = True
        Me.Text7.AccessibleDescription = "Agent Analysis Bands: Band #4: Start Range:"
        Me.Text7.BackColor = System.Drawing.SystemColors.Window
        Me.Text7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text7.Location = New System.Drawing.Point(73, 116)
        Me.Text7.MaxLength = 0
        Me.Text7.Name = "Text7"
        Me.Text7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text7.Size = New System.Drawing.Size(80, 20)
        Me.Text7.TabIndex = 6
        Me.Text7.Tag = "3007,ValidateNumeric,M"
        Me.Text7.Text = "91"
        Me.Text7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text8
        '
        Me.Text8.AcceptsReturn = True
        Me.Text8.AccessibleDescription = "Agent Analysis Bands: Band #4: End Range:"
        Me.Text8.BackColor = System.Drawing.SystemColors.Window
        Me.Text8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text8.Location = New System.Drawing.Point(163, 116)
        Me.Text8.MaxLength = 0
        Me.Text8.Name = "Text8"
        Me.Text8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text8.Size = New System.Drawing.Size(80, 20)
        Me.Text8.TabIndex = 7
        Me.Text8.Tag = "3008,ValidateNumeric,M"
        Me.Text8.Text = "120"
        Me.Text8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Text9
        '
        Me.Text9.AcceptsReturn = True
        Me.Text9.AccessibleDescription = "Agent Analysis Bands: Band #5: Start Range:"
        Me.Text9.BackColor = System.Drawing.SystemColors.Window
        Me.Text9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text9.Location = New System.Drawing.Point(73, 144)
        Me.Text9.MaxLength = 0
        Me.Text9.Name = "Text9"
        Me.Text9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text9.Size = New System.Drawing.Size(80, 20)
        Me.Text9.TabIndex = 8
        Me.Text9.Tag = "3009,ValidateNumeric,M"
        Me.Text9.Text = "121"
        Me.Text9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_lblBand_0
        '
        Me._lblBand_0.AutoSize = True
        Me._lblBand_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_0.Location = New System.Drawing.Point(11, 36)
        Me._lblBand_0.Name = "_lblBand_0"
        Me._lblBand_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_0.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_0.TabIndex = 15
        Me._lblBand_0.Tag = "60,M"
        Me._lblBand_0.Text = "Band #1:"
        '
        '_lblStart_0
        '
        Me._lblStart_0.AutoSize = True
        Me._lblStart_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblStart_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStart_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblStart_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStart_0.Location = New System.Drawing.Point(70, 8)
        Me._lblStart_0.Name = "_lblStart_0"
        Me._lblStart_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStart_0.Size = New System.Drawing.Size(75, 13)
        Me._lblStart_0.TabIndex = 14
        Me._lblStart_0.Tag = "60,M"
        Me._lblStart_0.Text = "Start Range"
        '
        '_lblStart_1
        '
        Me._lblStart_1.AutoSize = True
        Me._lblStart_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblStart_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStart_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblStart_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStart_1.Location = New System.Drawing.Point(160, 8)
        Me._lblStart_1.Name = "_lblStart_1"
        Me._lblStart_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStart_1.Size = New System.Drawing.Size(68, 13)
        Me._lblStart_1.TabIndex = 13
        Me._lblStart_1.Tag = "60,M"
        Me._lblStart_1.Text = "End Range"
        '
        '_lblBand_1
        '
        Me._lblBand_1.AutoSize = True
        Me._lblBand_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_1.Location = New System.Drawing.Point(11, 64)
        Me._lblBand_1.Name = "_lblBand_1"
        Me._lblBand_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_1.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_1.TabIndex = 12
        Me._lblBand_1.Tag = "60,M"
        Me._lblBand_1.Text = "Band #2:"
        '
        '_lblBand_2
        '
        Me._lblBand_2.AutoSize = True
        Me._lblBand_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_2.Location = New System.Drawing.Point(11, 92)
        Me._lblBand_2.Name = "_lblBand_2"
        Me._lblBand_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_2.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_2.TabIndex = 11
        Me._lblBand_2.Tag = "60,M"
        Me._lblBand_2.Text = "Band #3:"
        '
        '_lblBand_3
        '
        Me._lblBand_3.AutoSize = True
        Me._lblBand_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_3.Location = New System.Drawing.Point(11, 118)
        Me._lblBand_3.Name = "_lblBand_3"
        Me._lblBand_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_3.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_3.TabIndex = 10
        Me._lblBand_3.Tag = "60,M"
        Me._lblBand_3.Text = "Band #4:"
        '
        '_lblBand_4
        '
        Me._lblBand_4.AutoSize = True
        Me._lblBand_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblBand_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBand_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBand_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBand_4.Location = New System.Drawing.Point(11, 146)
        Me._lblBand_4.Name = "_lblBand_4"
        Me._lblBand_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBand_4.Size = New System.Drawing.Size(61, 13)
        Me._lblBand_4.TabIndex = 9
        Me._lblBand_4.Tag = "60,M"
        Me._lblBand_4.Text = "Band #5:"
        '
        'frmAccountsAgent
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(573, 376)
        Me.Controls.Add(Me.Text1)
        Me.Controls.Add(Me.Text2)
        Me.Controls.Add(Me.Text3)
        Me.Controls.Add(Me.Text4)
        Me.Controls.Add(Me.Text5)
        Me.Controls.Add(Me.Text6)
        Me.Controls.Add(Me.Text7)
        Me.Controls.Add(Me.Text8)
        Me.Controls.Add(Me.Text9)
        Me.Controls.Add(Me._lblBand_0)
        Me.Controls.Add(Me._lblStart_0)
        Me.Controls.Add(Me._lblStart_1)
        Me.Controls.Add(Me._lblBand_1)
        Me.Controls.Add(Me._lblBand_2)
        Me.Controls.Add(Me._lblBand_3)
        Me.Controls.Add(Me._lblBand_4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmAccountsAgent"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblStart()
		Me.lblStart(0) = _lblStart_0
		Me.lblStart(1) = _lblStart_1
	End Sub
	Sub InitializelblBand()
		Me.lblBand(0) = _lblBand_0
		Me.lblBand(1) = _lblBand_1
		Me.lblBand(2) = _lblBand_2
		Me.lblBand(3) = _lblBand_3
		Me.lblBand(4) = _lblBand_4
	End Sub
#End Region 
End Class