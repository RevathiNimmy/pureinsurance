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
	Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not fTerminateCalled_Form_Terminate_Renamed Then
				fTerminateCalled_Form_Terminate_Renamed = True
				Form_Terminate_Renamed()
			End If
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdCheckList As System.Windows.Forms.Button
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents uctRiskScreen1 As uctRiskScreenControl.RiskScreen
	Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button7 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button8 As System.Windows.Forms.ToolStripButton
	Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
	Public WithEvents cmdLossSchedule As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents imgToolbar As System.Windows.Forms.ImageList
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCheckList = New System.Windows.Forms.Button
		Me.cmdAddTask = New System.Windows.Forms.Button
		Me.uctRiskScreen1 = New uctRiskScreenControl.RiskScreen
		Me.Toolbar1 = New System.Windows.Forms.ToolStrip
		Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button7 = New System.Windows.Forms.ToolStripButton
		Me._Toolbar1_Button8 = New System.Windows.Forms.ToolStripButton
		Me.cmdLossSchedule = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.imgToolbar = New System.Windows.Forms.ImageList
		Me.Toolbar1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCheckList
		' 
		Me.cmdCheckList.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCheckList.CausesValidation = True
		Me.cmdCheckList.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCheckList.Enabled = True
		Me.cmdCheckList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCheckList.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCheckList.Location = New System.Drawing.Point(111, 449)
		Me.cmdCheckList.Name = "cmdCheckList"
		Me.cmdCheckList.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCheckList.Size = New System.Drawing.Size(97, 22)
		Me.cmdCheckList.TabIndex = 5
		Me.cmdCheckList.TabStop = True
		Me.cmdCheckList.Text = "Check List"
		Me.cmdCheckList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCheckList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddTask
		' 
		Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddTask.CausesValidation = True
		Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddTask.Enabled = True
		Me.cmdAddTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddTask.Location = New System.Drawing.Point(216, 448)
		Me.cmdAddTask.Name = "cmdAddTask"
		Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddTask.Size = New System.Drawing.Size(81, 20)
		Me.cmdAddTask.TabIndex = 7
		Me.cmdAddTask.TabStop = True
		Me.cmdAddTask.Text = "Add &Task"
		Me.cmdAddTask.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdAddTask.Visible = False
		' 
		' uctRiskScreen1
		' 
        ' Me.uctRiskScreen1.Font = New System.Drawing.Font("Arial", 8.25, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.uctRiskScreen1.Location = New System.Drawing.Point(8, 32)
		Me.uctRiskScreen1.Name = "uctRiskScreen1"
		Me.uctRiskScreen1.Size = New System.Drawing.Size(601, 409)
		Me.uctRiskScreen1.TabIndex = 6
		' 
		' Toolbar1
		' 
		Me.Toolbar1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Toolbar1.ImageList = imgToolbar
		Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
		Me.Toolbar1.Name = "Toolbar1"
		Me.Toolbar1.ShowItemToolTips = True
		Me.Toolbar1.Size = New System.Drawing.Size(616, 28)
		Me.Toolbar1.TabIndex = 4
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button1)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button2)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button3)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button4)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button5)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button6)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button7)
		Me.Toolbar1.Items.Add(Me._Toolbar1_Button8)
		' 
		' _Toolbar1_Button1
		' 
		Me._Toolbar1_Button1.AutoSize = False
		Me._Toolbar1_Button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button1.ImageIndex = 10
		Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button1.Name = "Financial"
		Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button1.ToolTipText = "Financial Details"
		' 
		' _Toolbar1_Button2
		' 
		Me._Toolbar1_Button2.AutoSize = False
		Me._Toolbar1_Button2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button2.ImageIndex = 11
		Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button2.Name = "Event"
		Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button2.ToolTipText = "Event Logs"
		' 
		' _Toolbar1_Button3
		' 
		Me._Toolbar1_Button3.AutoSize = False
		Me._Toolbar1_Button3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button3.ImageIndex = 9
		Me._Toolbar1_Button3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button3.Name = "Party"
		Me._Toolbar1_Button3.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button3.ToolTipText = "Party Details"
		' 
		' _Toolbar1_Button4
		' 
		Me._Toolbar1_Button4.AutoSize = False
		Me._Toolbar1_Button4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button4.ImageIndex = 7
		Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button4.Name = "Policy"
		Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button4.ToolTipText = "Policy Details"
		' 
		' _Toolbar1_Button5
		' 
		Me._Toolbar1_Button5.AutoSize = False
		Me._Toolbar1_Button5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button5.ImageIndex = 8
		Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button5.Name = "Risk"
		Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button5.ToolTipText = "Risk Details"
		' 
		' _Toolbar1_Button6
		' 
		Me._Toolbar1_Button6.AutoSize = False
		Me._Toolbar1_Button6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button6.ImageIndex = 10
		Me._Toolbar1_Button6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button6.Name = "Notes"
		Me._Toolbar1_Button6.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button6.ToolTipText = "Notes"
		' 
		' _Toolbar1_Button7
		' 
		Me._Toolbar1_Button7.AutoSize = False
		Me._Toolbar1_Button7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button7.ImageIndex = 0
		Me._Toolbar1_Button7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button7.Name = "History"
		Me._Toolbar1_Button7.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button7.ToolTipText = "Case History"
		Me._Toolbar1_Button7.Visible = False
		' 
		' _Toolbar1_Button8
		' 
		Me._Toolbar1_Button8.AutoSize = False
		Me._Toolbar1_Button8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
		Me._Toolbar1_Button8.ImageIndex = 12
		Me._Toolbar1_Button8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me._Toolbar1_Button8.Name = "DocArchive"
		Me._Toolbar1_Button8.Size = New System.Drawing.Size(24, 22)
		Me._Toolbar1_Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me._Toolbar1_Button8.ToolTipText = "Doc Archive"
		' 
		' cmdLossSchedule
		' 
		Me.cmdLossSchedule.BackColor = System.Drawing.SystemColors.Control
		Me.cmdLossSchedule.CausesValidation = True
		Me.cmdLossSchedule.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdLossSchedule.Enabled = True
		Me.cmdLossSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdLossSchedule.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdLossSchedule.Location = New System.Drawing.Point(8, 448)
		Me.cmdLossSchedule.Name = "cmdLossSchedule"
		Me.cmdLossSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdLossSchedule.Size = New System.Drawing.Size(97, 22)
		Me.cmdLossSchedule.TabIndex = 2
		Me.cmdLossSchedule.TabStop = True
		Me.cmdLossSchedule.Text = "&Loss Schedule"
		Me.cmdLossSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdLossSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(375, 448)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(536, 448)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 3
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(456, 448)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' imgToolbar
		' 
		Me.imgToolbar.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgToolbar.ImageStream = CType(resources.GetObject("imgToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgToolbar.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imgToolbar.Images.SetKeyName(0, "")
		Me.imgToolbar.Images.SetKeyName(1, "Party")
		Me.imgToolbar.Images.SetKeyName(2, "Policy")
		Me.imgToolbar.Images.SetKeyName(3, "Event")
		Me.imgToolbar.Images.SetKeyName(4, "")
		Me.imgToolbar.Images.SetKeyName(5, "")
		Me.imgToolbar.Images.SetKeyName(6, "AdhocDocs")
		Me.imgToolbar.Images.SetKeyName(7, "Risk")
		Me.imgToolbar.Images.SetKeyName(8, "")
		Me.imgToolbar.Images.SetKeyName(9, "")
		Me.imgToolbar.Images.SetKeyName(10, "")
		Me.imgToolbar.Images.SetKeyName(11, "")
		Me.imgToolbar.Images.SetKeyName(12, "")
		' 
		' frmInterface
		' 
		Me.KeyPreview = True
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(616, 478)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCheckList)
		Me.Controls.Add(Me.cmdAddTask)
		Me.Controls.Add(Me.uctRiskScreen1)
		Me.Controls.Add(Me.Toolbar1)
		Me.Controls.Add(Me.cmdLossSchedule)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.HelpButton = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Policy"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Toolbar1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class