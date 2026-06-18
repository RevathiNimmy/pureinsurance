<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGroupSummary
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwGroups_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwGroups As System.Windows.Forms.ListView
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents chkDeleted As System.Windows.Forms.CheckBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGroupSummary))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.imlList = New System.Windows.Forms.ImageList
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lvwGroups = New System.Windows.Forms.ListView
		Me._lvwGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwGroups_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwGroups_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.chkDeleted = New System.Windows.Forms.CheckBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.lvwGroups.SuspendLayout()
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
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(360, 352)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 8
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(8, 344)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(280, 352)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
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
		Me.cmdHelp.Location = New System.Drawing.Point(440, 352)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 0
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(167, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(509, 341)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 2
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwGroups)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Controls.Add(Me.chkDeleted)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lvwGroups
		' 
		Me.lvwGroups.BackColor = System.Drawing.SystemColors.Window
		Me.lvwGroups.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwGroups.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwGroups.FullRowSelect = True
		Me.lvwGroups.GridLines = True
		Me.lvwGroups.HideSelection = True
		Me.lvwGroups.LabelEdit = False
		Me.lvwGroups.LabelWrap = True
		Me.lvwGroups.LargeImageList = imlList
		Me.lvwGroups.Location = New System.Drawing.Point(8, 12)
		Me.lvwGroups.Name = "lvwGroups"
		Me.lvwGroups.Size = New System.Drawing.Size(489, 265)
		Me.lvwGroups.SmallImageList = imlList
		Me.lvwGroups.TabIndex = 6
		Me.lvwGroups.View = System.Windows.Forms.View.Details
		Me.lvwGroups.Columns.Add(Me._lvwGroups_ColumnHeader_1)
		Me.lvwGroups.Columns.Add(Me._lvwGroups_ColumnHeader_2)
		Me.lvwGroups.Columns.Add(Me._lvwGroups_ColumnHeader_3)
		' 
		' _lvwGroups_ColumnHeader_1
		' 
		Me._lvwGroups_ColumnHeader_1.Text = "Code"
		Me._lvwGroups_ColumnHeader_1.Width = 97
		' 
		' _lvwGroups_ColumnHeader_2
		' 
		Me._lvwGroups_ColumnHeader_2.Text = "Description"
		Me._lvwGroups_ColumnHeader_2.Width = 292
		' 
		' _lvwGroups_ColumnHeader_3
		' 
		Me._lvwGroups_ColumnHeader_3.Text = "# Items"
		Me._lvwGroups_ColumnHeader_3.Width = 97
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(424, 284)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(264, 284)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 4
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(344, 284)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 5
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' chkDeleted
		' 
		Me.chkDeleted.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDeleted.BackColor = System.Drawing.SystemColors.Control
		Me.chkDeleted.CausesValidation = True
		Me.chkDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkDeleted.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkDeleted.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDeleted.Enabled = True
		Me.chkDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDeleted.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDeleted.Location = New System.Drawing.Point(8, 284)
		Me.chkDeleted.Name = "chkDeleted"
		Me.chkDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDeleted.Size = New System.Drawing.Size(113, 17)
		Me.chkDeleted.TabIndex = 7
		Me.chkDeleted.TabStop = True
		Me.chkDeleted.Text = "Show deleted items"
		Me.chkDeleted.Visible = False
		' 
		' frmGroupSummary
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(518, 378)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmGroupSummary.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(349, -34)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmGroupSummary"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Groups"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwGroups, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.lvwGroups.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class