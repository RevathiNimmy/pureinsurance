<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowFailures
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwFailures_InitializeColumnKeys()
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Private WithEvents _lvwFailures_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwFailures_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwFailures As System.Windows.Forms.ListView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.lvwFailures = New System.Windows.Forms.ListView
        Me._lvwFailures_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwFailures_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(8, 520)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 25)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.Text = "E&xit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lvwFailures
        '
        Me.lvwFailures.BackColor = System.Drawing.SystemColors.Window
        Me.lvwFailures.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwFailures_ColumnHeader_1, Me._lvwFailures_ColumnHeader_2})
        Me.lvwFailures.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFailures.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwFailures.Location = New System.Drawing.Point(8, 8)
        Me.lvwFailures.Name = "lvwFailures"
        Me.lvwFailures.Size = New System.Drawing.Size(825, 505)
        Me.lvwFailures.TabIndex = 0
        Me.lvwFailures.UseCompatibleStateImageBehavior = False
        Me.lvwFailures.View = System.Windows.Forms.View.Details
        '
        '_lvwFailures_ColumnHeader_1
        '
        Me._lvwFailures_ColumnHeader_1.Tag = ""
        Me._lvwFailures_ColumnHeader_1.Text = "Client Code"
        Me._lvwFailures_ColumnHeader_1.Width = 167
        '
        '_lvwFailures_ColumnHeader_2
        '
        Me._lvwFailures_ColumnHeader_2.Tag = ""
        Me._lvwFailures_ColumnHeader_2.Text = "Reason"
        Me._lvwFailures_ColumnHeader_2.Width = 601
        '
        'frmShowFailures
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(840, 551)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lvwFailures)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShowFailures"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Failures"
        Me.ResumeLayout(False)

    End Sub
	Sub lvwFailures_InitializeColumnKeys()
		Me._lvwFailures_ColumnHeader_1.Name = ""
		Me._lvwFailures_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class