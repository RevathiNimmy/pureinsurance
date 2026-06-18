<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInterface
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdQuit As System.Windows.Forms.Button
	Public WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
	Public WithEvents cmdStart As System.Windows.Forms.Button
	Public WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
	Public WithEvents LVList As System.Windows.Forms.ListView
	Public WithEvents lblHeadiing As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.cmdStart = New System.Windows.Forms.Button
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.LVList = New System.Windows.Forms.ListView
        Me.lblHeadiing = New System.Windows.Forms.Label
        Me.StatusBar1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(480, 272)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(73, 25)
        Me.cmdQuit.TabIndex = 5
        Me.cmdQuit.Text = "&Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(0, 304)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(97, 26)
        Me.ProgressBar1.TabIndex = 4
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(400, 272)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(73, 25)
        Me.cmdStart.TabIndex = 3
        Me.cmdStart.Text = "&Start"
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 305)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(568, 25)
        Me.StatusBar1.TabIndex = 2
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.AutoSize = False
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(96, 25)
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LVList
        '
        Me.LVList.BackColor = System.Drawing.SystemColors.Window
        Me.LVList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LVList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LVList.LabelEdit = True
        Me.LVList.Location = New System.Drawing.Point(8, 40)
        Me.LVList.Name = "LVList"
        Me.LVList.Size = New System.Drawing.Size(553, 225)
        Me.LVList.TabIndex = 1
        Me.LVList.UseCompatibleStateImageBehavior = False
        '
        'lblHeadiing
        '
        Me.lblHeadiing.BackColor = System.Drawing.SystemColors.Control
        Me.lblHeadiing.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHeadiing.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadiing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHeadiing.Location = New System.Drawing.Point(8, 8)
        Me.lblHeadiing.Name = "lblHeadiing"
        Me.lblHeadiing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHeadiing.Size = New System.Drawing.Size(545, 17)
        Me.lblHeadiing.TabIndex = 0
        Me.lblHeadiing.Text = "Converting existing HTM documents into XML"
        Me.lblHeadiing.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmInterface
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(568, 330)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.LVList)
        Me.Controls.Add(Me.lblHeadiing)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HTM To XML Convertor"
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class