<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTaskLogEntry
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents lblTaskLogText As System.Windows.Forms.Label
	Public WithEvents lblDateTime As System.Windows.Forms.Label
	Public WithEvents lblCreatedBy As System.Windows.Forms.Label
	Public WithEvents cboPMUserLookup1 As PMUserLookupControl.cboPMUserLookup
	Public WithEvents txtTaskLogTime As System.Windows.Forms.TextBox
	Public WithEvents txtTaskLogText As System.Windows.Forms.TextBox
	Public WithEvents txtTaskLogDate As System.Windows.Forms.TextBox
    Public WithEvents pan3CreatedBy As System.Windows.Forms.Panel
    'developers guide no 26
    Public WithEvents lbl3CreatedBy As System.Windows.Forms.Label
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTaskLogEntry))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblTaskLogText = New System.Windows.Forms.Label
        Me.lblDateTime = New System.Windows.Forms.Label
        Me.lblCreatedBy = New System.Windows.Forms.Label
        Me.cboPMUserLookup1 = New PMUserLookupControl.cboPMUserLookup
        Me.txtTaskLogTime = New System.Windows.Forms.TextBox
        Me.txtTaskLogText = New System.Windows.Forms.TextBox
        Me.txtTaskLogDate = New System.Windows.Forms.TextBox
        Me.pan3CreatedBy = New System.Windows.Forms.Panel
        Me.lbl3CreatedBy = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pan3CreatedBy.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(400, 216)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 5
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(320, 216)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
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
        Me.cmdOK.Location = New System.Drawing.Point(240, 216)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(8, 224)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 6
        Me.uctPMResizer1.Visible = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(154, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(469, 205)
        Me.tabMainTab.TabIndex = 6
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaskLogText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateTime)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCreatedBy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMUserLookup1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTaskLogTime)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTaskLogText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTaskLogDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pan3CreatedBy)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(461, 179)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Log Entry"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblTaskLogText
        '
        Me.lblTaskLogText.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskLogText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskLogText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskLogText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskLogText.Location = New System.Drawing.Point(16, 87)
        Me.lblTaskLogText.Name = "lblTaskLogText"
        Me.lblTaskLogText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskLogText.Size = New System.Drawing.Size(57, 17)
        Me.lblTaskLogText.TabIndex = 7
        Me.lblTaskLogText.Text = "Entry:"
        '
        'lblDateTime
        '
        Me.lblDateTime.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateTime.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateTime.Location = New System.Drawing.Point(16, 55)
        Me.lblDateTime.Name = "lblDateTime"
        Me.lblDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateTime.Size = New System.Drawing.Size(73, 17)
        Me.lblDateTime.TabIndex = 8
        Me.lblDateTime.Text = "Date/Time:"
        '
        'lblCreatedBy
        '
        Me.lblCreatedBy.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreatedBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreatedBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreatedBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCreatedBy.Location = New System.Drawing.Point(16, 23)
        Me.lblCreatedBy.Name = "lblCreatedBy"
        Me.lblCreatedBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreatedBy.Size = New System.Drawing.Size(62, 13)
        Me.lblCreatedBy.TabIndex = 10
        Me.lblCreatedBy.Text = "Created By:"
        '
        'cboPMUserLookup1
        '
        Me.cboPMUserLookup1.DefaultUserID = 0
        Me.cboPMUserLookup1.FirstItem = "()"
        Me.cboPMUserLookup1.ListIndex = -1
        Me.cboPMUserLookup1.Location = New System.Drawing.Point(96, 20)
        Me.cboPMUserLookup1.Name = "cboPMUserLookup1"
        Me.cboPMUserLookup1.PMUserGroupID = 0
        Me.cboPMUserLookup1.SingleUserID = 0
        Me.cboPMUserLookup1.Size = New System.Drawing.Size(153, 21)
        Me.cboPMUserLookup1.Sorted = True
        Me.cboPMUserLookup1.TabIndex = 11
        Me.cboPMUserLookup1.ToolTipText = ""
        Me.cboPMUserLookup1.UserID = 0
        Me.cboPMUserLookup1.Visible = False
        '
        'txtTaskLogTime
        '
        Me.txtTaskLogTime.AcceptsReturn = True
        Me.txtTaskLogTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaskLogTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaskLogTime.Enabled = False
        Me.txtTaskLogTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskLogTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaskLogTime.Location = New System.Drawing.Point(232, 52)
        Me.txtTaskLogTime.MaxLength = 0
        Me.txtTaskLogTime.Name = "txtTaskLogTime"
        Me.txtTaskLogTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaskLogTime.Size = New System.Drawing.Size(65, 20)
        Me.txtTaskLogTime.TabIndex = 1
        '
        'txtTaskLogText
        '
        Me.txtTaskLogText.AcceptsReturn = True
        Me.txtTaskLogText.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaskLogText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaskLogText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskLogText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaskLogText.Location = New System.Drawing.Point(96, 84)
        Me.txtTaskLogText.MaxLength = 255
        Me.txtTaskLogText.Multiline = True
        Me.txtTaskLogText.Name = "txtTaskLogText"
        Me.txtTaskLogText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaskLogText.Size = New System.Drawing.Size(361, 83)
        Me.txtTaskLogText.TabIndex = 2
        '
        'txtTaskLogDate
        '
        Me.txtTaskLogDate.AcceptsReturn = True
        Me.txtTaskLogDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaskLogDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaskLogDate.Enabled = False
        Me.txtTaskLogDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskLogDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaskLogDate.Location = New System.Drawing.Point(96, 52)
        Me.txtTaskLogDate.MaxLength = 0
        Me.txtTaskLogDate.Name = "txtTaskLogDate"
        Me.txtTaskLogDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaskLogDate.Size = New System.Drawing.Size(113, 20)
        Me.txtTaskLogDate.TabIndex = 0
        '
        'pan3CreatedBy
        '
        Me.pan3CreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pan3CreatedBy.Controls.Add(Me.lbl3CreatedBy)
        Me.pan3CreatedBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pan3CreatedBy.Location = New System.Drawing.Point(96, 20)
        Me.pan3CreatedBy.Name = "pan3CreatedBy"
        Me.pan3CreatedBy.Size = New System.Drawing.Size(153, 21)
        Me.pan3CreatedBy.TabIndex = 9
        '
        'lbl3CreatedBy
        '
        Me.lbl3CreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl3CreatedBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3CreatedBy.Location = New System.Drawing.Point(0, 0)
        Me.lbl3CreatedBy.Name = "lbl3CreatedBy"
        Me.lbl3CreatedBy.Size = New System.Drawing.Size(153, 21)
        Me.lbl3CreatedBy.TabIndex = 9
        '
        'frmTaskLogEntry
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(478, 245)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTaskLogEntry"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "TaskLog"
        Me.Text = "Task Log Entry"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.pan3CreatedBy.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class