<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdButton()
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
	Private WithEvents _cmdButton_0 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_2 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_3 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_1 As System.Windows.Forms.Button
	Public cmdButton(3) As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdButton_0 = New System.Windows.Forms.Button
        Me._cmdButton_2 = New System.Windows.Forms.Button
        Me._cmdButton_3 = New System.Windows.Forms.Button
        Me._cmdButton_1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        '_cmdButton_0
        '
        Me._cmdButton_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_0.Location = New System.Drawing.Point(169, 8)
        Me._cmdButton_0.Name = "_cmdButton_0"
        Me._cmdButton_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_0.TabIndex = 2
        Me._cmdButton_0.Text = "&Transfer"
        Me._cmdButton_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_0.UseVisualStyleBackColor = False
        '
        '_cmdButton_2
        '
        Me._cmdButton_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_2.Location = New System.Drawing.Point(89, 8)
        Me._cmdButton_2.Name = "_cmdButton_2"
        Me._cmdButton_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_2.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_2.TabIndex = 1
        Me._cmdButton_2.Text = "&Accept"
        Me._cmdButton_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_2.UseVisualStyleBackColor = False
        '
        '_cmdButton_3
        '
        Me._cmdButton_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_3.Location = New System.Drawing.Point(249, 8)
        Me._cmdButton_3.Name = "_cmdButton_3"
        Me._cmdButton_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_3.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_3.TabIndex = 3
        Me._cmdButton_3.Text = "&Cancel"
        Me._cmdButton_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_3.UseVisualStyleBackColor = False
        '
        '_cmdButton_1
        '
        Me._cmdButton_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_1.Location = New System.Drawing.Point(9, 8)
        Me._cmdButton_1.Name = "_cmdButton_1"
        Me._cmdButton_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_1.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_1.TabIndex = 0
        Me._cmdButton_1.Text = "A&mend"
        Me._cmdButton_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_1.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(331, 44)
        Me.ControlBox = False
        Me.Controls.Add(Me._cmdButton_0)
        Me.Controls.Add(Me._cmdButton_2)
        Me.Controls.Add(Me._cmdButton_3)
        Me.Controls.Add(Me._cmdButton_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Renewal Amendment"
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdButton()
		Me.cmdButton(0) = _cmdButton_0
		Me.cmdButton(2) = _cmdButton_2
		Me.cmdButton(3) = _cmdButton_3
		Me.cmdButton(1) = _cmdButton_1
	End Sub
#End Region 
End Class