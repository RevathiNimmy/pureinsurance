<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboReason As System.Windows.Forms.ComboBox
	Public WithEvents txtOther As System.Windows.Forms.TextBox
	Public WithEvents lblOther As System.Windows.Forms.Label
	Public WithEvents lblReason As System.Windows.Forms.Label
	Public WithEvents fraReason As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.fraReason = New System.Windows.Forms.GroupBox
        Me.cboReason = New System.Windows.Forms.ComboBox
        Me.txtOther = New System.Windows.Forms.TextBox
        Me.lblOther = New System.Windows.Forms.Label
        Me.lblReason = New System.Windows.Forms.Label
        Me.fraReason.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(352, 104)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 104)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 25)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'fraReason
        '
        Me.fraReason.BackColor = System.Drawing.SystemColors.Control
        Me.fraReason.Controls.Add(Me.cboReason)
        Me.fraReason.Controls.Add(Me.txtOther)
        Me.fraReason.Controls.Add(Me.lblOther)
        Me.fraReason.Controls.Add(Me.lblReason)
        Me.fraReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReason.Location = New System.Drawing.Point(8, 8)
        Me.fraReason.Name = "fraReason"
        Me.fraReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReason.Size = New System.Drawing.Size(417, 89)
        Me.fraReason.TabIndex = 0
        Me.fraReason.TabStop = False
        Me.fraReason.Text = "Reason for the Change"
        '
        'cboReason
        '
        Me.cboReason.BackColor = System.Drawing.SystemColors.Window
        Me.cboReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReason.Location = New System.Drawing.Point(64, 24)
        Me.cboReason.Name = "cboReason"
        Me.cboReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReason.Size = New System.Drawing.Size(337, 21)
        Me.cboReason.TabIndex = 1
        '
        'txtOther
        '
        Me.txtOther.AcceptsReturn = True
        Me.txtOther.BackColor = System.Drawing.SystemColors.Window
        Me.txtOther.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOther.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOther.Location = New System.Drawing.Point(64, 54)
        Me.txtOther.MaxLength = 0
        Me.txtOther.Name = "txtOther"
        Me.txtOther.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOther.Size = New System.Drawing.Size(337, 21)
        Me.txtOther.TabIndex = 2
        Me.txtOther.Tag = "F;"
        '
        'lblOther
        '
        Me.lblOther.BackColor = System.Drawing.SystemColors.Control
        Me.lblOther.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOther.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOther.Location = New System.Drawing.Point(8, 56)
        Me.lblOther.Name = "lblOther"
        Me.lblOther.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOther.Size = New System.Drawing.Size(65, 17)
        Me.lblOther.TabIndex = 6
        Me.lblOther.Text = "If Other"
        '
        'lblReason
        '
        Me.lblReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReason.Location = New System.Drawing.Point(8, 24)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReason.Size = New System.Drawing.Size(65, 17)
        Me.lblReason.TabIndex = 5
        Me.lblReason.Text = "Reason"
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(438, 134)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraReason)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Policy"
        Me.fraReason.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class