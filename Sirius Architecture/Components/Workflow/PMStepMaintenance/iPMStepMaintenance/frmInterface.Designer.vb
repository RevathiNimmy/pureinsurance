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
	Public dlgMainOpen As System.Windows.Forms.OpenFileDialog
	Public dlgMainSave As System.Windows.Forms.SaveFileDialog
	Public dlgMainFont As System.Windows.Forms.FontDialog
	Public dlgMainColor As System.Windows.Forms.ColorDialog
	Public dlgMainPrint As System.Windows.Forms.PrintDialog
	Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblMove As System.Windows.Forms.Label
	Public WithEvents lvwTable As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdMoveUp As System.Windows.Forms.Button
	Public WithEvents cmdMoveDown As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents imlTable As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgMainOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgMainSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgMainFont = New System.Windows.Forms.FontDialog
		Me.dlgMainColor = New System.Windows.Forms.ColorDialog
		Me.dlgMainPrint = New System.Windows.Forms.PrintDialog
		Me.uctPMResizer = New PMResizerControl.uctPMResizer
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.imgIcon = New System.Windows.Forms.PictureBox
		Me.lblMove = New System.Windows.Forms.Label
		Me.lvwTable = New System.Windows.Forms.ListView
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdView = New System.Windows.Forms.Button
		Me.cmdMoveUp = New System.Windows.Forms.Button
		Me.cmdMoveDown = New System.Windows.Forms.Button
		Me.imlTable = New System.Windows.Forms.ImageList
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
		' 
		' uctPMResizer
		' 
		Me.uctPMResizer.Location = New System.Drawing.Point(200, 320)
		Me.uctPMResizer.Name = "uctPMResizer"
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = False
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(480, 320)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 3
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "App&ly"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(400, 320)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(320, 320)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(109, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(2, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(557, 309)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 4
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblMove)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwTable)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdView)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdMoveUp)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdMoveDown)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' imgIcon
		' 
		Me.imgIcon.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgIcon.Enabled = True
		Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
		Me.imgIcon.Location = New System.Drawing.Point(496, 4)
		Me.imgIcon.Name = "imgIcon"
		Me.imgIcon.Size = New System.Drawing.Size(32, 32)
		Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgIcon.Visible = True
		' 
		' lblMove
		' 
		Me.lblMove.AutoSize = False
		Me.lblMove.BackColor = System.Drawing.SystemColors.Control
		Me.lblMove.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMove.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMove.Enabled = False
		Me.lblMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMove.Location = New System.Drawing.Point(472, 208)
		Me.lblMove.Name = "lblMove"
		Me.lblMove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMove.Size = New System.Drawing.Size(40, 19)
		Me.lblMove.TabIndex = 11
		Me.lblMove.Text = "Move"
		Me.lblMove.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMove.UseMnemonic = True
		Me.lblMove.Visible = True
		' 
		' lvwTable
		' 
		Me.lvwTable.BackColor = System.Drawing.SystemColors.Window
		Me.lvwTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwTable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwTable.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwTable.HideSelection = False
		Me.lvwTable.LabelEdit = False
		Me.lvwTable.LabelWrap = True
		Me.lvwTable.LargeImageList = imlTable
		Me.lvwTable.Location = New System.Drawing.Point(8, 12)
		Me.lvwTable.Name = "lvwTable"
		Me.lvwTable.Size = New System.Drawing.Size(457, 265)
		Me.lvwTable.SmallImageList = imlTable
		Me.lvwTable.TabIndex = 0
		Me.lvwTable.View = System.Windows.Forms.View.Details
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(472, 52)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 5
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
		Me.cmdEdit.Location = New System.Drawing.Point(472, 84)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 6
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(472, 148)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 8
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdView
		' 
		Me.cmdView.BackColor = System.Drawing.SystemColors.Control
		Me.cmdView.CausesValidation = True
		Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdView.Enabled = True
		Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdView.Location = New System.Drawing.Point(472, 116)
		Me.cmdView.Name = "cmdView"
		Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdView.Size = New System.Drawing.Size(73, 22)
		Me.cmdView.TabIndex = 7
		Me.cmdView.TabStop = True
		Me.cmdView.Text = "&View"
		Me.cmdView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdMoveUp
		' 
		Me.cmdMoveUp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMoveUp.CausesValidation = True
		Me.cmdMoveUp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMoveUp.Enabled = False
		Me.cmdMoveUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMoveUp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMoveUp.Image = CType(resources.GetObject("cmdMoveUp.Image"), System.Drawing.Image)
		Me.cmdMoveUp.Location = New System.Drawing.Point(472, 180)
		Me.cmdMoveUp.Name = "cmdMoveUp"
		Me.cmdMoveUp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMoveUp.Size = New System.Drawing.Size(24, 23)
		Me.cmdMoveUp.TabIndex = 9
		Me.cmdMoveUp.TabStop = True
		Me.cmdMoveUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.cmdMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdMoveDown
		' 
		Me.cmdMoveDown.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMoveDown.CausesValidation = True
		Me.cmdMoveDown.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMoveDown.Enabled = False
		Me.cmdMoveDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMoveDown.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMoveDown.Image = CType(resources.GetObject("cmdMoveDown.Image"), System.Drawing.Image)
		Me.cmdMoveDown.Location = New System.Drawing.Point(472, 229)
		Me.cmdMoveDown.Name = "cmdMoveDown"
		Me.cmdMoveDown.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMoveDown.Size = New System.Drawing.Size(24, 23)
		Me.cmdMoveDown.TabIndex = 10
		Me.cmdMoveDown.TabStop = True
		Me.cmdMoveDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.cmdMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' imlTable
		' 
		Me.imlTable.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlTable.ImageStream = CType(resources.GetObject("imlTable.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlTable.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlTable.Images.SetKeyName(0, "find")
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(559, 346)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctPMResizer)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.dlgMainOpen.DefaultExt = "csv"
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(101, 84)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Action Type Maintenance"
		Me.dlgMainOpen.Title = "Save As..."
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.commandButtonHelper1.SetStyle(Me.cmdMoveUp, 1)
		Me.commandButtonHelper1.SetStyle(Me.cmdMoveDown, 1)
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.ToolTip1.SetToolTip(Me.cmdMoveUp, "Move Up")
		Me.ToolTip1.SetToolTip(Me.cmdMoveDown, "Move Down")
		Me.listViewHelper1.SetItemClickMethod(Me.lvwTable, "lvwTable_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTable, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class