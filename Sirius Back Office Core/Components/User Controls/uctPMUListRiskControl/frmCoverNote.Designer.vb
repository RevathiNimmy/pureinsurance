<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCoverNote
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtCNTimeTo As System.Windows.Forms.TextBox
	Public WithEvents txtCNDateTo As System.Windows.Forms.TextBox
	Public WithEvents txtCNTimeFrom As System.Windows.Forms.TextBox
	Public WithEvents txtCNDateFrom As System.Windows.Forms.TextBox
	Public WithEvents txtCNNumber As System.Windows.Forms.TextBox
	Public WithEvents txtRiskDescription As System.Windows.Forms.TextBox
	Public WithEvents txtRiskNo As System.Windows.Forms.TextBox
	Public WithEvents lblCNTimeTo As System.Windows.Forms.Label
	Public WithEvents lblCNTimeFrom As System.Windows.Forms.Label
	Public WithEvents lblCNDateTo As System.Windows.Forms.Label
	Public WithEvents lblCNDateFrom As System.Windows.Forms.Label
	Public WithEvents lblCNNumber As System.Windows.Forms.Label
	Public WithEvents lblRiskDescription As System.Windows.Forms.Label
	Public WithEvents lblRiskNo As System.Windows.Forms.Label
	Public WithEvents fraAttachCoverNote As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraAttachCoverNote = New System.Windows.Forms.GroupBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.txtCNTimeTo = New System.Windows.Forms.TextBox
        Me.txtCNDateTo = New System.Windows.Forms.TextBox
        Me.txtCNTimeFrom = New System.Windows.Forms.TextBox
        Me.txtCNDateFrom = New System.Windows.Forms.TextBox
        Me.txtCNNumber = New System.Windows.Forms.TextBox
        Me.txtRiskDescription = New System.Windows.Forms.TextBox
        Me.txtRiskNo = New System.Windows.Forms.TextBox
        Me.lblCNTimeTo = New System.Windows.Forms.Label
        Me.lblCNTimeFrom = New System.Windows.Forms.Label
        Me.lblCNDateTo = New System.Windows.Forms.Label
        Me.lblCNDateFrom = New System.Windows.Forms.Label
        Me.lblCNNumber = New System.Windows.Forms.Label
        Me.lblRiskDescription = New System.Windows.Forms.Label
        Me.lblRiskNo = New System.Windows.Forms.Label
        Me.fraAttachCoverNote.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraAttachCoverNote
        '
        Me.fraAttachCoverNote.BackColor = System.Drawing.SystemColors.Control
        Me.fraAttachCoverNote.Controls.Add(Me.cmdCancel)
        Me.fraAttachCoverNote.Controls.Add(Me.cmdOK)
        Me.fraAttachCoverNote.Controls.Add(Me.txtCNTimeTo)
        Me.fraAttachCoverNote.Controls.Add(Me.txtCNDateTo)
        Me.fraAttachCoverNote.Controls.Add(Me.txtCNTimeFrom)
        Me.fraAttachCoverNote.Controls.Add(Me.txtCNDateFrom)
        Me.fraAttachCoverNote.Controls.Add(Me.txtCNNumber)
        Me.fraAttachCoverNote.Controls.Add(Me.txtRiskDescription)
        Me.fraAttachCoverNote.Controls.Add(Me.txtRiskNo)
        Me.fraAttachCoverNote.Controls.Add(Me.lblCNTimeTo)
        Me.fraAttachCoverNote.Controls.Add(Me.lblCNTimeFrom)
        Me.fraAttachCoverNote.Controls.Add(Me.lblCNDateTo)
        Me.fraAttachCoverNote.Controls.Add(Me.lblCNDateFrom)
        Me.fraAttachCoverNote.Controls.Add(Me.lblCNNumber)
        Me.fraAttachCoverNote.Controls.Add(Me.lblRiskDescription)
        Me.fraAttachCoverNote.Controls.Add(Me.lblRiskNo)
        Me.fraAttachCoverNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAttachCoverNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAttachCoverNote.Location = New System.Drawing.Point(0, -2)
        Me.fraAttachCoverNote.Name = "fraAttachCoverNote"
        Me.fraAttachCoverNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAttachCoverNote.Size = New System.Drawing.Size(404, 201)
        Me.fraAttachCoverNote.TabIndex = 0
        Me.fraAttachCoverNote.TabStop = False
        Me.fraAttachCoverNote.Text = "Attach Cover Notes"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(286, 150)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(79, 25)
        Me.cmdCancel.TabIndex = 16
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(200, 150)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(79, 25)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "Ok"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'txtCNTimeTo
        '
        Me.txtCNTimeTo.AcceptsReturn = True
        Me.txtCNTimeTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNTimeTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNTimeTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNTimeTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNTimeTo.Location = New System.Drawing.Point(296, 114)
        Me.txtCNTimeTo.MaxLength = 0
        Me.txtCNTimeTo.Name = "txtCNTimeTo"
        Me.txtCNTimeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNTimeTo.Size = New System.Drawing.Size(81, 20)
        Me.txtCNTimeTo.TabIndex = 14
        Me.txtCNTimeTo.Text = "  "
        '
        'txtCNDateTo
        '
        Me.txtCNDateTo.AcceptsReturn = True
        Me.txtCNDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNDateTo.Location = New System.Drawing.Point(152, 114)
        Me.txtCNDateTo.MaxLength = 0
        Me.txtCNDateTo.Name = "txtCNDateTo"
        Me.txtCNDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNDateTo.Size = New System.Drawing.Size(105, 20)
        Me.txtCNDateTo.TabIndex = 12
        '
        'txtCNTimeFrom
        '
        Me.txtCNTimeFrom.AcceptsReturn = True
        Me.txtCNTimeFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNTimeFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNTimeFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNTimeFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNTimeFrom.Location = New System.Drawing.Point(296, 92)
        Me.txtCNTimeFrom.MaxLength = 0
        Me.txtCNTimeFrom.Name = "txtCNTimeFrom"
        Me.txtCNTimeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNTimeFrom.Size = New System.Drawing.Size(81, 20)
        Me.txtCNTimeFrom.TabIndex = 11
        Me.txtCNTimeFrom.Text = "  "
        '
        'txtCNDateFrom
        '
        Me.txtCNDateFrom.AcceptsReturn = True
        Me.txtCNDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNDateFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNDateFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNDateFrom.Location = New System.Drawing.Point(152, 92)
        Me.txtCNDateFrom.MaxLength = 0
        Me.txtCNDateFrom.Name = "txtCNDateFrom"
        Me.txtCNDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNDateFrom.Size = New System.Drawing.Size(105, 20)
        Me.txtCNDateFrom.TabIndex = 9
        '
        'txtCNNumber
        '
        Me.txtCNNumber.AcceptsReturn = True
        Me.txtCNNumber.BackColor = System.Drawing.SystemColors.Menu
        Me.txtCNNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNNumber.Enabled = False
        Me.txtCNNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNNumber.Location = New System.Drawing.Point(152, 70)
        Me.txtCNNumber.MaxLength = 0
        Me.txtCNNumber.Name = "txtCNNumber"
        Me.txtCNNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNNumber.Size = New System.Drawing.Size(225, 20)
        Me.txtCNNumber.TabIndex = 8
        '
        'txtRiskDescription
        '
        Me.txtRiskDescription.AcceptsReturn = True
        Me.txtRiskDescription.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRiskDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskDescription.Enabled = False
        Me.txtRiskDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskDescription.Location = New System.Drawing.Point(152, 48)
        Me.txtRiskDescription.MaxLength = 0
        Me.txtRiskDescription.Name = "txtRiskDescription"
        Me.txtRiskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskDescription.Size = New System.Drawing.Size(225, 20)
        Me.txtRiskDescription.TabIndex = 7
        '
        'txtRiskNo
        '
        Me.txtRiskNo.AcceptsReturn = True
        Me.txtRiskNo.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRiskNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRiskNo.Enabled = False
        Me.txtRiskNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRiskNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRiskNo.Location = New System.Drawing.Point(152, 26)
        Me.txtRiskNo.MaxLength = 0
        Me.txtRiskNo.Name = "txtRiskNo"
        Me.txtRiskNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRiskNo.Size = New System.Drawing.Size(87, 20)
        Me.txtRiskNo.TabIndex = 6
        '
        'lblCNTimeTo
        '
        Me.lblCNTimeTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNTimeTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNTimeTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNTimeTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNTimeTo.Location = New System.Drawing.Point(260, 116)
        Me.lblCNTimeTo.Name = "lblCNTimeTo"
        Me.lblCNTimeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNTimeTo.Size = New System.Drawing.Size(33, 17)
        Me.lblCNTimeTo.TabIndex = 13
        Me.lblCNTimeTo.Text = "Time"
        '
        'lblCNTimeFrom
        '
        Me.lblCNTimeFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNTimeFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNTimeFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNTimeFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNTimeFrom.Location = New System.Drawing.Point(260, 94)
        Me.lblCNTimeFrom.Name = "lblCNTimeFrom"
        Me.lblCNTimeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNTimeFrom.Size = New System.Drawing.Size(33, 17)
        Me.lblCNTimeFrom.TabIndex = 10
        Me.lblCNTimeFrom.Text = "Time"
        '
        'lblCNDateTo
        '
        Me.lblCNDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNDateTo.Location = New System.Drawing.Point(12, 116)
        Me.lblCNDateTo.Name = "lblCNDateTo"
        Me.lblCNDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNDateTo.Size = New System.Drawing.Size(134, 17)
        Me.lblCNDateTo.TabIndex = 5
        Me.lblCNDateTo.Text = "Date To"
        '
        'lblCNDateFrom
        '
        Me.lblCNDateFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNDateFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNDateFrom.Location = New System.Drawing.Point(12, 94)
        Me.lblCNDateFrom.Name = "lblCNDateFrom"
        Me.lblCNDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNDateFrom.Size = New System.Drawing.Size(134, 17)
        Me.lblCNDateFrom.TabIndex = 4
        Me.lblCNDateFrom.Text = "Date From"
        '
        'lblCNNumber
        '
        Me.lblCNNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNNumber.Location = New System.Drawing.Point(12, 72)
        Me.lblCNNumber.Name = "lblCNNumber"
        Me.lblCNNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNNumber.Size = New System.Drawing.Size(134, 17)
        Me.lblCNNumber.TabIndex = 3
        Me.lblCNNumber.Text = "Cover Note Number"
        '
        'lblRiskDescription
        '
        Me.lblRiskDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskDescription.Location = New System.Drawing.Point(12, 50)
        Me.lblRiskDescription.Name = "lblRiskDescription"
        Me.lblRiskDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskDescription.Size = New System.Drawing.Size(134, 17)
        Me.lblRiskDescription.TabIndex = 2
        Me.lblRiskDescription.Text = "Risk Description"
        '
        'lblRiskNo
        '
        Me.lblRiskNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskNo.Location = New System.Drawing.Point(12, 28)
        Me.lblRiskNo.Name = "lblRiskNo"
        Me.lblRiskNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskNo.Size = New System.Drawing.Size(134, 17)
        Me.lblRiskNo.TabIndex = 1
        Me.lblRiskNo.Text = "Risk No"
        '
        'frmCoverNote
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(408, 199)
        Me.Controls.Add(Me.fraAttachCoverNote)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCoverNote"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cover Note"
        Me.fraAttachCoverNote.ResumeLayout(False)
        Me.fraAttachCoverNote.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class