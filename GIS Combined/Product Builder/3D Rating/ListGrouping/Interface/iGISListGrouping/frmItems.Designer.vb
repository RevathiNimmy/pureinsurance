<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItems
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents imlImage As System.Windows.Forms.ImageList
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblSelected As System.Windows.Forms.Label
	Public WithEvents lblAvailable As System.Windows.Forms.Label
	Private WithEvents _lvwSource_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSource_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSource As System.Windows.Forms.ListView
	Private WithEvents _lvwList_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwList_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwList As System.Windows.Forms.ListView
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents fraDetails As System.Windows.Forms.GroupBox
	Public WithEvents cmdRemoveAll As System.Windows.Forms.Button
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents cmdAddAll As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItems))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.imlImage = New System.Windows.Forms.ImageList
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblSelected = New System.Windows.Forms.Label
		Me.lblAvailable = New System.Windows.Forms.Label
		Me.lvwSource = New System.Windows.Forms.ListView
		Me._lvwSource_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSource_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.lvwList = New System.Windows.Forms.ListView
		Me._lvwList_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwList_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.fraDetails = New System.Windows.Forms.GroupBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.txtCode = New System.Windows.Forms.TextBox
		Me.lblDescription = New System.Windows.Forms.Label
		Me.lblCode = New System.Windows.Forms.Label
		Me.cmdRemoveAll = New System.Windows.Forms.Button
		Me.cmdRemove = New System.Windows.Forms.Button
		Me.cmdAddAll = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.lvwSource.SuspendLayout()
		Me.lvwList.SuspendLayout()
		Me.fraDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' imlImage
		' 
		Me.imlImage.ImageSize = New System.Drawing.Size(16, 16)
		Me.imlImage.ImageStream = CType(resources.GetObject("imlImage.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imlImage.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imlImage.Images.SetKeyName(0, "cross")
		Me.imlImage.Images.SetKeyName(1, "normal")
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(376, 416)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 9
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(456, 416)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 10
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(296, 416)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 8
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(172, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(525, 405)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 11
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblSelected)
		Me._tabMain_TabPage0.Controls.Add(Me.lblAvailable)
		Me._tabMain_TabPage0.Controls.Add(Me.lvwSource)
		Me._tabMain_TabPage0.Controls.Add(Me.lvwList)
		Me._tabMain_TabPage0.Controls.Add(Me.fraDetails)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdRemoveAll)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdRemove)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAddAll)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMain_TabPage0.Text = "&1 - Items"
		' 
		' lblSelected
		' 
		Me.lblSelected.AutoSize = False
		Me.lblSelected.BackColor = System.Drawing.SystemColors.Control
		Me.lblSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSelected.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSelected.Enabled = True
		Me.lblSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSelected.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSelected.Location = New System.Drawing.Point(304, 108)
		Me.lblSelected.Name = "lblSelected"
		Me.lblSelected.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSelected.Size = New System.Drawing.Size(129, 17)
		Me.lblSelected.TabIndex = 15
		Me.lblSelected.Text = "Selected Items"
		Me.lblSelected.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSelected.UseMnemonic = True
		Me.lblSelected.Visible = True
		' 
		' lblAvailable
		' 
		Me.lblAvailable.AutoSize = True
		Me.lblAvailable.BackColor = System.Drawing.SystemColors.Control
		Me.lblAvailable.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAvailable.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAvailable.Enabled = True
		Me.lblAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAvailable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAvailable.Location = New System.Drawing.Point(8, 108)
		Me.lblAvailable.Name = "lblAvailable"
		Me.lblAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAvailable.Size = New System.Drawing.Size(71, 13)
		Me.lblAvailable.TabIndex = 16
		Me.lblAvailable.Text = "Available Items"
		Me.lblAvailable.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAvailable.UseMnemonic = True
		Me.lblAvailable.Visible = True
		' 
		' lvwSource
		' 
		Me.lvwSource.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSource.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSource.FullRowSelect = True
		Me.lvwSource.HideSelection = True
		Me.lvwSource.LabelEdit = False
		Me.lvwSource.LabelWrap = True
		Me.lvwSource.LargeImageList = imlImage
		Me.lvwSource.Location = New System.Drawing.Point(8, 124)
		Me.lvwSource.MultiSelect = True
		Me.lvwSource.Name = "lvwSource"
		Me.lvwSource.Size = New System.Drawing.Size(209, 241)
		Me.lvwSource.SmallImageList = imlImage
		Me.lvwSource.TabIndex = 2
		Me.lvwSource.View = System.Windows.Forms.View.Details
		Me.lvwSource.Columns.Add(Me._lvwSource_ColumnHeader_1)
		Me.lvwSource.Columns.Add(Me._lvwSource_ColumnHeader_2)
		' 
		' _lvwSource_ColumnHeader_1
		' 
		Me._lvwSource_ColumnHeader_1.Text = "Code"
		Me._lvwSource_ColumnHeader_1.Width = 67
		' 
		' _lvwSource_ColumnHeader_2
		' 
		Me._lvwSource_ColumnHeader_2.Text = "Description"
		Me._lvwSource_ColumnHeader_2.Width = 134
		' 
		' lvwList
		' 
		Me.lvwList.BackColor = System.Drawing.SystemColors.Window
		Me.lvwList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwList.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwList.FullRowSelect = True
		Me.lvwList.HideSelection = True
		Me.lvwList.LabelEdit = False
		Me.lvwList.LabelWrap = True
		Me.lvwList.LargeImageList = imlImage
		Me.lvwList.Location = New System.Drawing.Point(304, 124)
		Me.lvwList.MultiSelect = True
		Me.lvwList.Name = "lvwList"
		Me.lvwList.Size = New System.Drawing.Size(209, 241)
		Me.lvwList.SmallImageList = imlImage
		Me.lvwList.TabIndex = 7
		Me.lvwList.View = System.Windows.Forms.View.Details
		Me.lvwList.Columns.Add(Me._lvwList_ColumnHeader_1)
		Me.lvwList.Columns.Add(Me._lvwList_ColumnHeader_2)
		' 
		' _lvwList_ColumnHeader_1
		' 
		Me._lvwList_ColumnHeader_1.Text = "Code"
		Me._lvwList_ColumnHeader_1.Width = 67
		' 
		' _lvwList_ColumnHeader_2
		' 
		Me._lvwList_ColumnHeader_2.Text = "Description"
		Me._lvwList_ColumnHeader_2.Width = 134
		' 
		' fraDetails
		' 
		Me.fraDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraDetails.Controls.Add(Me.txtDescription)
		Me.fraDetails.Controls.Add(Me.txtCode)
		Me.fraDetails.Controls.Add(Me.lblDescription)
		Me.fraDetails.Controls.Add(Me.lblCode)
		Me.fraDetails.Enabled = True
		Me.fraDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDetails.Location = New System.Drawing.Point(8, 12)
		Me.fraDetails.Name = "fraDetails"
		Me.fraDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDetails.Size = New System.Drawing.Size(497, 81)
		Me.fraDetails.TabIndex = 12
		Me.fraDetails.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(88, 48)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(401, 19)
		Me.txtDescription.TabIndex = 1
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' txtCode
		' 
		Me.txtCode.AcceptsReturn = True
		Me.txtCode.AutoSize = False
		Me.txtCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCode.CausesValidation = True
		Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCode.Enabled = True
		Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCode.HideSelection = True
		Me.txtCode.Location = New System.Drawing.Point(88, 16)
		Me.txtCode.MaxLength = 0
		Me.txtCode.Multiline = False
		Me.txtCode.Name = "txtCode"
		Me.txtCode.ReadOnly = False
		Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCode.Size = New System.Drawing.Size(145, 19)
		Me.txtCode.TabIndex = 0
		Me.txtCode.TabStop = True
		Me.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCode.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = True
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(8, 51)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblDescription.TabIndex = 14
		Me.lblDescription.Text = "Description:"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' lblCode
		' 
		Me.lblCode.AutoSize = True
		Me.lblCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCode.Enabled = True
		Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCode.Location = New System.Drawing.Point(8, 19)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(34, 13)
		Me.lblCode.TabIndex = 13
		Me.lblCode.Text = "Code:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' cmdRemoveAll
		' 
		Me.cmdRemoveAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemoveAll.CausesValidation = True
		Me.cmdRemoveAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemoveAll.Enabled = True
		Me.cmdRemoveAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemoveAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemoveAll.Location = New System.Drawing.Point(224, 340)
		Me.cmdRemoveAll.Name = "cmdRemoveAll"
		Me.cmdRemoveAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemoveAll.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemoveAll.TabIndex = 6
		Me.cmdRemoveAll.TabStop = True
		Me.cmdRemoveAll.Text = "<< Remo&ve"
		Me.cmdRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemoveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRemove
		' 
		Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemove.CausesValidation = True
		Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemove.Enabled = True
		Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemove.Location = New System.Drawing.Point(224, 308)
		Me.cmdRemove.Name = "cmdRemove"
		Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemove.TabIndex = 5
		Me.cmdRemove.TabStop = True
		Me.cmdRemove.Text = "< &Remove"
		Me.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddAll
		' 
		Me.cmdAddAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddAll.CausesValidation = True
		Me.cmdAddAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddAll.Enabled = True
		Me.cmdAddAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddAll.Location = New System.Drawing.Point(224, 156)
		Me.cmdAddAll.Name = "cmdAddAll"
		Me.cmdAddAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddAll.Size = New System.Drawing.Size(73, 22)
		Me.cmdAddAll.TabIndex = 4
		Me.cmdAddAll.TabStop = True
		Me.cmdAddAll.Text = "A&dd >>"
		Me.cmdAddAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(224, 124)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 3
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add >"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmItems
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(534, 443)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmItems.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmItems"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "List Items"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSource, True)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwList, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.lvwSource.ResumeLayout(False)
		Me.lvwList.ResumeLayout(False)
		Me.fraDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class