<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectRoadmap
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents tvwRoadmaps As System.Windows.Forms.TreeView
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents imlIcons As System.Windows.Forms.ImageList
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectRoadmap))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.tvwRoadmaps = New System.Windows.Forms.TreeView
		Me.imlIcons = New System.Windows.Forms.ImageList
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(216, 280)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(69, 25)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(124, 280)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(69, 25)
		Me.cmdOk.TabIndex = 2
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "Ok"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(78, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(241, 261)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 0
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.tvwRoadmaps)
		Me._tabMain_TabPage0.Text = "Available roadmaps"
		' 
		' tvwRoadmaps
		' 
		Me.tvwRoadmaps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvwRoadmaps.CausesValidation = True
		Me.tvwRoadmaps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvwRoadmaps.ImageList = imlIcons
		Me.tvwRoadmaps.Indent = 36
		Me.tvwRoadmaps.LabelEdit = True
		Me.tvwRoadmaps.Location = New System.Drawing.Point(8, 8)
		Me.tvwRoadmaps.Name = "tvwRoadmaps"
		Me.tvwRoadmaps.Size = New System.Drawing.Size(157, 169)
		Me.tvwRoadmaps.TabIndex = 1
		' 
		' imlIcons
		' 
		Me.imlIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlIcons.ImageStream = CType(resources.GetObject("imlIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlIcons.Images.SetKeyName(0, "")
		Me.imlIcons.Images.SetKeyName(1, "Delete")
		Me.imlIcons.Images.SetKeyName(2, "MoveDown")
		Me.imlIcons.Images.SetKeyName(3, "MoveUp")
		Me.imlIcons.Images.SetKeyName(4, "Open")
		Me.imlIcons.Images.SetKeyName(5, "Save")
		Me.imlIcons.Images.SetKeyName(6, "Diary")
		Me.imlIcons.Images.SetKeyName(7, "EditText")
		Me.imlIcons.Images.SetKeyName(8, "StandardLetter")
		Me.imlIcons.Images.SetKeyName(9, "RaiseEvent")
		Me.imlIcons.Images.SetKeyName(10, "LaunchEXE")
		Me.imlIcons.Images.SetKeyName(11, "UserComponent")
		Me.imlIcons.Images.SetKeyName(12, "Roadmap")
		Me.imlIcons.Images.SetKeyName(13, "Process")
		Me.imlIcons.Images.SetKeyName(14, "StepFind")
		Me.imlIcons.Images.SetKeyName(15, "StepFindCross")
		Me.imlIcons.Images.SetKeyName(16, "StepFindTick")
		Me.imlIcons.Images.SetKeyName(17, "Key")
		Me.imlIcons.Images.SetKeyName(18, "StepFindGrey")
		Me.imlIcons.Images.SetKeyName(19, "StepNoForm")
		Me.imlIcons.Images.SetKeyName(20, "StepNoFormCross")
		Me.imlIcons.Images.SetKeyName(21, "StepNoFormGrey")
		Me.imlIcons.Images.SetKeyName(22, "StepNoFormTick")
		Me.imlIcons.Images.SetKeyName(23, "SubMap")
		Me.imlIcons.Images.SetKeyName(24, "SubMapGrey")
		Me.imlIcons.Images.SetKeyName(25, "StepDataForm")
		Me.imlIcons.Images.SetKeyName(26, "StepDataFormCross")
		Me.imlIcons.Images.SetKeyName(27, "StepDataFormGrey")
		Me.imlIcons.Images.SetKeyName(28, "StepDataFormTick")
		Me.imlIcons.Images.SetKeyName(29, "StepDecision")
		Me.imlIcons.Images.SetKeyName(30, "StepDecisionTick")
		Me.imlIcons.Images.SetKeyName(31, "StepDecisionCross")
		Me.imlIcons.Images.SetKeyName(32, "StepDecisionGrey")
		Me.imlIcons.Images.SetKeyName(33, "")
		Me.imlIcons.Images.SetKeyName(34, "StepPrint")
		' 
		' frmSelectRoadmap
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(302, 342)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmSelectRoadmap.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSelectRoadmap"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "PMNavXMEditor - select roadmap"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class