<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
	Public WithEvents imlList As System.Windows.Forms.ImageList
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Private WithEvents _lvwListType_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwListType_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwListType_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwListType As System.Windows.Forms.ListView
	Public WithEvents cmdAuto As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.imlList = New System.Windows.Forms.ImageList
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lvwListType = New System.Windows.Forms.ListView
		Me._lvwListType_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwListType_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwListType_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.cmdAuto = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.lvwListType.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' imlList
		' 
		Me.imlList.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlList.ImageStream = CType(resources.GetObject("imlList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlList.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlList.Images.SetKeyName(0, "cross")
		Me.imlList.Images.SetKeyName(1, "normal")
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(8, 328)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(424, 336)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 1
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(344, 336)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(73, 22)
		Me.cmdExit.TabIndex = 0
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "&Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.HotTrack = False
		Me.tabMainTab.ItemSize = New System.Drawing.Size(162, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(493, 325)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 2
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwListType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAuto)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lvwListType
		' 
		Me.lvwListType.BackColor = System.Drawing.SystemColors.Window
		Me.lvwListType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwListType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwListType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwListType.FullRowSelect = True
		Me.lvwListType.GridLines = True
		Me.lvwListType.HideSelection = True
		Me.lvwListType.LabelEdit = False
		Me.lvwListType.LabelWrap = True
		Me.lvwListType.LargeImageList = imlList
		Me.lvwListType.Location = New System.Drawing.Point(8, 12)
		Me.lvwListType.Name = "lvwListType"
		Me.lvwListType.Size = New System.Drawing.Size(473, 249)
		Me.lvwListType.SmallImageList = imlList
		Me.lvwListType.TabIndex = 5
		Me.lvwListType.View = System.Windows.Forms.View.Details
		Me.lvwListType.Columns.Add(Me._lvwListType_ColumnHeader_1)
		Me.lvwListType.Columns.Add(Me._lvwListType_ColumnHeader_2)
		Me.lvwListType.Columns.Add(Me._lvwListType_ColumnHeader_3)
		' 
		' _lvwListType_ColumnHeader_1
		' 
		Me._lvwListType_ColumnHeader_1.Text = "List Code"
		Me._lvwListType_ColumnHeader_1.Width = 67
		' 
		' _lvwListType_ColumnHeader_2
		' 
		Me._lvwListType_ColumnHeader_2.Text = "Description"
		Me._lvwListType_ColumnHeader_2.Width = 333
		' 
		' _lvwListType_ColumnHeader_3
		' 
		Me._lvwListType_ColumnHeader_3.Text = "# Groups"
		Me._lvwListType_ColumnHeader_3.Width = 67
		' 
		' cmdAuto
		' 
		Me.cmdAuto.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAuto.CausesValidation = True
		Me.cmdAuto.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAuto.Enabled = True
		Me.cmdAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAuto.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAuto.Location = New System.Drawing.Point(8, 268)
		Me.cmdAuto.Name = "cmdAuto"
		Me.cmdAuto.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAuto.Size = New System.Drawing.Size(73, 22)
		Me.cmdAuto.TabIndex = 3
		Me.cmdAuto.TabStop = True
		Me.cmdAuto.Text = "&Auto Group"
		Me.cmdAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(408, 268)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 4
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmMain
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(503, 363)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(-111, 103)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "List Grouping Maintenance"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwListType, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.lvwListType.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class