<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportCashBook
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

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents Timer1 As System.Windows.Forms.Timer
	Public CommonDialog1Save As System.Windows.Forms.SaveFileDialog
	Public WithEvents txtBranch As System.Windows.Forms.TextBox
	Public WithEvents cmdClear As System.Windows.Forms.Button
	Public WithEvents cmdSaveSettings As System.Windows.Forms.Button
	Public WithEvents cmdRunExport As System.Windows.Forms.Button
	Public WithEvents txtExportLocation As System.Windows.Forms.TextBox
	Public WithEvents cmdExportLocation As System.Windows.Forms.Button
	Public WithEvents cboMediaType As System.Windows.Forms.ComboBox
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblMediaType As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CommonDialog1Save = New System.Windows.Forms.SaveFileDialog
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtBranch = New System.Windows.Forms.TextBox
        Me.cmdClear = New System.Windows.Forms.Button
        Me.cmdSaveSettings = New System.Windows.Forms.Button
        Me.cmdRunExport = New System.Windows.Forms.Button
        Me.txtExportLocation = New System.Windows.Forms.TextBox
        Me.cmdExportLocation = New System.Windows.Forms.Button
        Me.cboMediaType = New System.Windows.Forms.ComboBox
        Me.lblBranch = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblMediaType = New System.Windows.Forms.Label
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(200, 172)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(93, 25)
        Me.cmdOk.TabIndex = 9
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(300, 172)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(85, 25)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 10
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtBranch)
        Me.Frame1.Controls.Add(Me.cmdClear)
        Me.Frame1.Controls.Add(Me.cmdSaveSettings)
        Me.Frame1.Controls.Add(Me.cmdRunExport)
        Me.Frame1.Controls.Add(Me.txtExportLocation)
        Me.Frame1.Controls.Add(Me.cmdExportLocation)
        Me.Frame1.Controls.Add(Me.cboMediaType)
        Me.Frame1.Controls.Add(Me.lblBranch)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.lblMediaType)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(389, 157)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        '
        'txtBranch
        '
        Me.txtBranch.AcceptsReturn = True
        Me.txtBranch.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtBranch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranch.Enabled = False
        Me.txtBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranch.Location = New System.Drawing.Point(88, 48)
        Me.txtBranch.MaxLength = 0
        Me.txtBranch.Name = "txtBranch"
        Me.txtBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranch.Size = New System.Drawing.Size(177, 21)
        Me.txtBranch.TabIndex = 12
        '
        'cmdClear
        '
        Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClear.Location = New System.Drawing.Point(356, 20)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClear.Size = New System.Drawing.Size(21, 19)
        Me.cmdClear.TabIndex = 10
        Me.cmdClear.Text = "X"
        Me.cmdClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClear.UseVisualStyleBackColor = False
        '
        'cmdSaveSettings
        '
        Me.cmdSaveSettings.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveSettings.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveSettings.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveSettings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveSettings.Location = New System.Drawing.Point(192, 120)
        Me.cmdSaveSettings.Name = "cmdSaveSettings"
        Me.cmdSaveSettings.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveSettings.Size = New System.Drawing.Size(93, 25)
        Me.cmdSaveSettings.TabIndex = 7
        Me.cmdSaveSettings.Text = "&Save settings"
        Me.cmdSaveSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSaveSettings.UseVisualStyleBackColor = False
        '
        'cmdRunExport
        '
        Me.cmdRunExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRunExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRunExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRunExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRunExport.Location = New System.Drawing.Point(292, 120)
        Me.cmdRunExport.Name = "cmdRunExport"
        Me.cmdRunExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRunExport.Size = New System.Drawing.Size(85, 25)
        Me.cmdRunExport.TabIndex = 6
        Me.cmdRunExport.Text = "&Run export"
        Me.cmdRunExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRunExport.UseVisualStyleBackColor = False
        '
        'txtExportLocation
        '
        Me.txtExportLocation.AcceptsReturn = True
        Me.txtExportLocation.BackColor = System.Drawing.SystemColors.Window
        Me.txtExportLocation.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExportLocation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportLocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExportLocation.Location = New System.Drawing.Point(88, 20)
        Me.txtExportLocation.MaxLength = 0
        Me.txtExportLocation.Name = "txtExportLocation"
        Me.txtExportLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExportLocation.Size = New System.Drawing.Size(249, 21)
        Me.txtExportLocation.TabIndex = 3
        '
        'cmdExportLocation
        '
        Me.cmdExportLocation.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExportLocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExportLocation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExportLocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExportLocation.Location = New System.Drawing.Point(336, 20)
        Me.cmdExportLocation.Name = "cmdExportLocation"
        Me.cmdExportLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExportLocation.Size = New System.Drawing.Size(21, 19)
        Me.cmdExportLocation.TabIndex = 2
        Me.cmdExportLocation.Text = "..."
        Me.cmdExportLocation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExportLocation.UseVisualStyleBackColor = False
        '
        'cboMediaType
        '
        Me.cboMediaType.BackColor = System.Drawing.SystemColors.Window
        Me.cboMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMediaType.Enabled = False
        Me.cboMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMediaType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMediaType.Location = New System.Drawing.Point(88, 76)
        Me.cboMediaType.Name = "cboMediaType"
        Me.cboMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMediaType.Size = New System.Drawing.Size(181, 21)
        Me.cboMediaType.TabIndex = 1
        Me.cboMediaType.Tag = "F;M;"
        Me.cboMediaType.Visible = False
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Enabled = False
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(8, 48)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(57, 13)
        Me.lblBranch.TabIndex = 11
        Me.lblBranch.Text = "Branch"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Export location"
        '
        'lblMediaType
        '
        Me.lblMediaType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMediaType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMediaType.Enabled = False
        Me.lblMediaType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMediaType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMediaType.Location = New System.Drawing.Point(8, 76)
        Me.lblMediaType.Name = "lblMediaType"
        Me.lblMediaType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMediaType.Size = New System.Drawing.Size(57, 13)
        Me.lblMediaType.TabIndex = 4
        Me.lblMediaType.Text = "Media type"
        Me.lblMediaType.Visible = False
        '
        'frmExportCashBook
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(405, 205)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExportCashBook"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Export cash book"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class