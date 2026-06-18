<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmList
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwListDetails_InitializeColumnKeys()
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents lblSubBranch As System.Windows.Forms.Label
	Private WithEvents _lvwListDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwListDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwListDetails As System.Windows.Forms.ListView
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmList))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.imgIcon = New System.Windows.Forms.PictureBox
		Me.imglImages = New System.Windows.Forms.ImageList
		Me.lblSubBranch = New System.Windows.Forms.Label
		Me.lvwListDetails = New System.Windows.Forms.ListView
		Me._lvwListDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwListDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdRemove = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cboSubBranch = New System.Windows.Forms.ComboBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.lvwListDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 400)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 7
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(280, 400)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 6
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
		Me.cmdCancel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(200, 400)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 5
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
		Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(120, 400)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 4
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
		Me.tabMainTab.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(171, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(349, 389)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 8
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblSubBranch)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwListDetails)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRemove)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboSubBranch)
		Me._tabMainTab_TabPage0.Text = "&1 - Ledgers"
		' 
		' imgIcon
		' 
		Me.imgIcon.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgIcon.Enabled = True
		Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
		Me.imgIcon.Location = New System.Drawing.Point(304, 4)
		Me.imgIcon.Name = "imgIcon"
		Me.imgIcon.Size = New System.Drawing.Size(32, 32)
		Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgIcon.Visible = True
		' 
		' imglImages
		' 
		Me.imglImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        'developer guide no.48
        'Me.imglImages.Key_0 = "LedgerImage"
        Me.imglImages.Images.SetKeyName(0, "LedgerImage")
		Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		' 
		' lblSubBranch
		' 
		Me.lblSubBranch.AutoSize = True
		Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubBranch.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubBranch.Enabled = True
		Me.lblSubBranch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubBranch.Location = New System.Drawing.Point(20, 16)
		Me.lblSubBranch.Name = "lblSubBranch"
		Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubBranch.Size = New System.Drawing.Size(255, 13)
		Me.lblSubBranch.TabIndex = 10
		Me.lblSubBranch.Text = "Current Sub-Branch:"
		Me.lblSubBranch.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSubBranch.UseMnemonic = True
		Me.lblSubBranch.Visible = True
		' 
		' lvwListDetails
		' 
		Me.lvwListDetails.BackColor = System.Drawing.SystemColors.Window
		Me.lvwListDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwListDetails.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwListDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwListDetails.HideSelection = False
		Me.lvwListDetails.LabelEdit = False
		Me.lvwListDetails.LabelWrap = True
		Me.lvwListDetails.Location = New System.Drawing.Point(16, 62)
		Me.lvwListDetails.Name = "lvwListDetails"
		Me.lvwListDetails.Size = New System.Drawing.Size(313, 249)
		Me.lvwListDetails.SmallImageList = imglImages
		Me.lvwListDetails.TabIndex = 0
		Me.lvwListDetails.View = System.Windows.Forms.View.Details
		Me.lvwListDetails.Columns.Add(Me._lvwListDetails_ColumnHeader_1)
		Me.lvwListDetails.Columns.Add(Me._lvwListDetails_ColumnHeader_2)
		' 
		' _lvwListDetails_ColumnHeader_1
		' 
		Me._lvwListDetails_ColumnHeader_1.Tag = ""
		Me._lvwListDetails_ColumnHeader_1.Text = "Name"
		Me._lvwListDetails_ColumnHeader_1.Width = 291
		' 
		' _lvwListDetails_ColumnHeader_2
		' 
		Me._lvwListDetails_ColumnHeader_2.Tag = ""
		Me._lvwListDetails_ColumnHeader_2.Text = "Type"
		Me._lvwListDetails_ColumnHeader_2.Width = 97
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(94, 318)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 3
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRemove
		' 
		Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemove.CausesValidation = True
		Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemove.Enabled = True
		Me.cmdRemove.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemove.Location = New System.Drawing.Point(172, 318)
		Me.cmdRemove.Name = "cmdRemove"
		Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemove.TabIndex = 2
		Me.cmdRemove.TabStop = True
		Me.cmdRemove.Text = "&Remove"
		Me.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(16, 318)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 1
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboSubBranch
		' 
		Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
		Me.cboSubBranch.CausesValidation = True
		Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboSubBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboSubBranch.Enabled = True
		Me.cboSubBranch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboSubBranch.IntegralHeight = True
		Me.cboSubBranch.Location = New System.Drawing.Point(18, 32)
		Me.cboSubBranch.Name = "cboSubBranch"
		Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboSubBranch.Size = New System.Drawing.Size(257, 21)
		Me.cboSubBranch.Sorted = False
		Me.cboSubBranch.TabIndex = 9
		Me.cboSubBranch.TabStop = True
		Me.cboSubBranch.Visible = True
		' 
		' frmList
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(362, 428)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmList.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(190, 279)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmList"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Ledger"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwListDetails, "lvwListDetails_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwListDetails, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.lvwListDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwListDetails_InitializeColumnKeys()
		Me._lvwListDetails_ColumnHeader_1.Name = ""
		Me._lvwListDetails_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class