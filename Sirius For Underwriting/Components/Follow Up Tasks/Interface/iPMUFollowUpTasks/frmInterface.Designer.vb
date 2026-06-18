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
	Public WithEvents lblUserGroup As System.Windows.Forms.Label
	Public WithEvents lblRunDate As System.Windows.Forms.Label
	Public WithEvents txtRunDate As System.Windows.Forms.TextBox
	Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdRun As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblUserGroup = New System.Windows.Forms.Label
        Me.lblRunDate = New System.Windows.Forms.Label
        Me.txtRunDate = New System.Windows.Forms.TextBox
        Me.cboUserGroup = New System.Windows.Forms.ComboBox
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdRun = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(426, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(431, 114)
        Me.tabMainTab.TabIndex = 4
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUserGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRunDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtRunDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboUserGroup)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(423, 88)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1-Follow Up Tasks"
        '
        'lblUserGroup
        '
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(24, 52)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(94, 17)
        Me.lblUserGroup.TabIndex = 5
        Me.lblUserGroup.Text = "User Group:"
        '
        'lblRunDate
        '
        Me.lblRunDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRunDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRunDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRunDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRunDate.Location = New System.Drawing.Point(24, 19)
        Me.lblRunDate.Name = "lblRunDate"
        Me.lblRunDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRunDate.Size = New System.Drawing.Size(94, 17)
        Me.lblRunDate.TabIndex = 6
        Me.lblRunDate.Text = "Run Date:"
        '
        'txtRunDate
        '
        Me.txtRunDate.AcceptsReturn = True
        Me.txtRunDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRunDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRunDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRunDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRunDate.Location = New System.Drawing.Point(120, 16)
        Me.txtRunDate.MaxLength = 0
        Me.txtRunDate.Name = "txtRunDate"
        Me.txtRunDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRunDate.Size = New System.Drawing.Size(149, 19)
        Me.txtRunDate.TabIndex = 0
        '
        'cboUserGroup
        '
        Me.cboUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUserGroup.Location = New System.Drawing.Point(120, 48)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUserGroup.Size = New System.Drawing.Size(289, 21)
        Me.cboUserGroup.TabIndex = 1
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(364, 127)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdRun
        '
        Me.cmdRun.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRun.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRun.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRun.Location = New System.Drawing.Point(284, 127)
        Me.cmdRun.Name = "cmdRun"
        Me.cmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRun.Size = New System.Drawing.Size(73, 22)
        Me.cmdRun.TabIndex = 2
        Me.cmdRun.Text = "&Run"
        Me.cmdRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRun.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(444, 158)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdRun)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Follow Up Tasks"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class