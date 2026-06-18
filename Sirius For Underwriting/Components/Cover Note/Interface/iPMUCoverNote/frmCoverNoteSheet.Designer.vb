<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCoverNoteSheet
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
	Public WithEvents cboSheetStatus As System.Windows.Forms.ComboBox
	Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtSheetNumber As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtComments As System.Windows.Forms.TextBox
	Public WithEvents cboAssignedDate As System.Windows.Forms.DateTimePicker
	Public WithEvents lblComments As System.Windows.Forms.Label
	Public WithEvents lblSheetStatus As System.Windows.Forms.Label
	Public WithEvents lblAssignedDate As System.Windows.Forms.Label
	Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
	Public WithEvents lblSheetNumber As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboSheetStatus = New System.Windows.Forms.ComboBox
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox
        Me.txtSheetNumber = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.cboAssignedDate = New System.Windows.Forms.DateTimePicker
        Me.lblComments = New System.Windows.Forms.Label
        Me.lblSheetStatus = New System.Windows.Forms.Label
        Me.lblAssignedDate = New System.Windows.Forms.Label
        Me.lblPolicyNumber = New System.Windows.Forms.Label
        Me.lblSheetNumber = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cboSheetStatus
        '
        Me.cboSheetStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboSheetStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSheetStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSheetStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSheetStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSheetStatus.Location = New System.Drawing.Point(111, 94)
        Me.cboSheetStatus.Name = "cboSheetStatus"
        Me.cboSheetStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSheetStatus.Size = New System.Drawing.Size(167, 21)
        Me.cboSheetStatus.TabIndex = 7
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.AcceptsReturn = True
        Me.txtPolicyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyNumber.Enabled = False
        Me.txtPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyNumber.Location = New System.Drawing.Point(111, 36)
        Me.txtPolicyNumber.MaxLength = 0
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyNumber.Size = New System.Drawing.Size(167, 21)
        Me.txtPolicyNumber.TabIndex = 3
        '
        'txtSheetNumber
        '
        Me.txtSheetNumber.AcceptsReturn = True
        Me.txtSheetNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtSheetNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSheetNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSheetNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSheetNumber.Location = New System.Drawing.Point(111, 7)
        Me.txtSheetNumber.MaxLength = 0
        Me.txtSheetNumber.Name = "txtSheetNumber"
        Me.txtSheetNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSheetNumber.Size = New System.Drawing.Size(144, 21)
        Me.txtSheetNumber.TabIndex = 1
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(384, 302)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(300, 302)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 10
        Me.cmdOk.TabStop = False
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(8, 152)
        Me.txtComments.MaxLength = 1024
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(451, 143)
        Me.txtComments.TabIndex = 9
        '
        'cboAssignedDate
        '
        Me.cboAssignedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.cboAssignedDate.Location = New System.Drawing.Point(111, 65)
        Me.cboAssignedDate.Name = "cboAssignedDate"
        Me.cboAssignedDate.Size = New System.Drawing.Size(167, 21)
        Me.cboAssignedDate.TabIndex = 5
        '
        'lblComments
        '
        Me.lblComments.BackColor = System.Drawing.SystemColors.Control
        Me.lblComments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComments.Location = New System.Drawing.Point(8, 127)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComments.Size = New System.Drawing.Size(95, 21)
        Me.lblComments.TabIndex = 8
        Me.lblComments.Text = "Comments"
        '
        'lblSheetStatus
        '
        Me.lblSheetStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblSheetStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSheetStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSheetStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSheetStatus.Location = New System.Drawing.Point(8, 97)
        Me.lblSheetStatus.Name = "lblSheetStatus"
        Me.lblSheetStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSheetStatus.Size = New System.Drawing.Size(104, 21)
        Me.lblSheetStatus.TabIndex = 6
        Me.lblSheetStatus.Text = "Sheet Status:"
        '
        'lblAssignedDate
        '
        Me.lblAssignedDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAssignedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAssignedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAssignedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAssignedDate.Location = New System.Drawing.Point(8, 65)
        Me.lblAssignedDate.Name = "lblAssignedDate"
        Me.lblAssignedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAssignedDate.Size = New System.Drawing.Size(104, 21)
        Me.lblAssignedDate.TabIndex = 4
        Me.lblAssignedDate.Text = "Assigned Date:"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumber.Location = New System.Drawing.Point(8, 37)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumber.Size = New System.Drawing.Size(104, 21)
        Me.lblPolicyNumber.TabIndex = 2
        Me.lblPolicyNumber.Text = "Policy Number:"
        '
        'lblSheetNumber
        '
        Me.lblSheetNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblSheetNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSheetNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSheetNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSheetNumber.Location = New System.Drawing.Point(8, 8)
        Me.lblSheetNumber.Name = "lblSheetNumber"
        Me.lblSheetNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSheetNumber.Size = New System.Drawing.Size(104, 21)
        Me.lblSheetNumber.TabIndex = 0
        Me.lblSheetNumber.Text = "Sheet Number:"
        '
        'frmCoverNoteSheet
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(465, 329)
        Me.Controls.Add(Me.cboSheetStatus)
        Me.Controls.Add(Me.txtPolicyNumber)
        Me.Controls.Add(Me.txtSheetNumber)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.cboAssignedDate)
        Me.Controls.Add(Me.lblComments)
        Me.Controls.Add(Me.lblSheetStatus)
        Me.Controls.Add(Me.lblAssignedDate)
        Me.Controls.Add(Me.lblPolicyNumber)
        Me.Controls.Add(Me.lblSheetNumber)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCoverNoteSheet"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cover Note Sheet"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class