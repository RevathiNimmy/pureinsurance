<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwUsers_InitializeColumnKeys()
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
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents cmdChange As System.Windows.Forms.Button
	Public WithEvents cboAdminLevel As System.Windows.Forms.ComboBox
	Private WithEvents _lvwUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwUsers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwUsers As System.Windows.Forms.ListView
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents lblAdminLevel As System.Windows.Forms.Label
	Public WithEvents fraAdmin As System.Windows.Forms.GroupBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraAdmin = New System.Windows.Forms.GroupBox
		Me.cmdRemove = New System.Windows.Forms.Button
		Me.cmdChange = New System.Windows.Forms.Button
		Me.cboAdminLevel = New System.Windows.Forms.ComboBox
		Me.lvwUsers = New System.Windows.Forms.ListView
		Me._lvwUsers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwUsers_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.lblAdminLevel = New System.Windows.Forms.Label
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraAdmin.SuspendLayout()
		Me.lvwUsers.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraAdmin
		' 
		Me.fraAdmin.BackColor = System.Drawing.SystemColors.Control
		Me.fraAdmin.Controls.Add(Me.cmdRemove)
		Me.fraAdmin.Controls.Add(Me.cmdChange)
		Me.fraAdmin.Controls.Add(Me.cboAdminLevel)
		Me.fraAdmin.Controls.Add(Me.lvwUsers)
		Me.fraAdmin.Controls.Add(Me.Image1)
		Me.fraAdmin.Controls.Add(Me.lblAdminLevel)
		Me.fraAdmin.Enabled = True
		Me.fraAdmin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraAdmin.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAdmin.Location = New System.Drawing.Point(4, 0)
		Me.fraAdmin.Name = "fraAdmin"
		Me.fraAdmin.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAdmin.Size = New System.Drawing.Size(440, 305)
		Me.fraAdmin.TabIndex = 3
		Me.fraAdmin.Visible = True
		' 
		' cmdRemove
		' 
		Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemove.CausesValidation = True
		Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemove.Enabled = True
		Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemove.Location = New System.Drawing.Point(356, 232)
		Me.cmdRemove.Name = "cmdRemove"
		Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemove.TabIndex = 8
		Me.cmdRemove.TabStop = True
		Me.cmdRemove.Text = "&Remove"
		Me.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdChange
		' 
		Me.cmdChange.BackColor = System.Drawing.SystemColors.Control
		Me.cmdChange.CausesValidation = True
		Me.cmdChange.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdChange.Enabled = True
		Me.cmdChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdChange.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdChange.Location = New System.Drawing.Point(356, 264)
		Me.cmdChange.Name = "cmdChange"
		Me.cmdChange.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdChange.Size = New System.Drawing.Size(73, 22)
		Me.cmdChange.TabIndex = 7
		Me.cmdChange.TabStop = True
		Me.cmdChange.Text = "&Change"
		Me.cmdChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboAdminLevel
		' 
		Me.cboAdminLevel.BackColor = System.Drawing.SystemColors.Window
		Me.cboAdminLevel.CausesValidation = True
		Me.cboAdminLevel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboAdminLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboAdminLevel.Enabled = True
		Me.cboAdminLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAdminLevel.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboAdminLevel.IntegralHeight = True
		Me.cboAdminLevel.Location = New System.Drawing.Point(356, 88)
		Me.cboAdminLevel.Name = "cboAdminLevel"
		Me.cboAdminLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboAdminLevel.Size = New System.Drawing.Size(73, 21)
		Me.cboAdminLevel.Sorted = False
		Me.cboAdminLevel.TabIndex = 6
		Me.cboAdminLevel.TabStop = True
		Me.cboAdminLevel.Visible = True
		Me.cboAdminLevel.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' lvwUsers
		' 
		Me.lvwUsers.Alignment = System.Windows.Forms.ListViewAlignment.Left
		Me.lvwUsers.BackColor = System.Drawing.SystemColors.Window
		Me.lvwUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwUsers.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwUsers.HideSelection = False
		Me.lvwUsers.LabelEdit = False
		Me.lvwUsers.LabelWrap = True
		Me.lvwUsers.Location = New System.Drawing.Point(8, 16)
		Me.lvwUsers.Name = "lvwUsers"
		Me.lvwUsers.Size = New System.Drawing.Size(337, 281)
		Me.lvwUsers.TabIndex = 4
		Me.lvwUsers.View = System.Windows.Forms.View.Details
		Me.lvwUsers.Columns.Add(Me._lvwUsers_ColumnHeader_1)
		Me.lvwUsers.Columns.Add(Me._lvwUsers_ColumnHeader_2)
		' 
		' _lvwUsers_ColumnHeader_1
		' 
		Me._lvwUsers_ColumnHeader_1.Tag = ""
		Me._lvwUsers_ColumnHeader_1.Text = "User Name"
		Me._lvwUsers_ColumnHeader_1.Width = 234
		' 
		' _lvwUsers_ColumnHeader_2
		' 
		Me._lvwUsers_ColumnHeader_2.Tag = ""
		Me._lvwUsers_ColumnHeader_2.Text = "Access Level"
		Me._lvwUsers_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwUsers_ColumnHeader_2.Width = 61
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(377, 24)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' lblAdminLevel
		' 
		Me.lblAdminLevel.AutoSize = True
		Me.lblAdminLevel.BackColor = System.Drawing.SystemColors.Control
		Me.lblAdminLevel.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAdminLevel.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAdminLevel.Enabled = True
		Me.lblAdminLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAdminLevel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAdminLevel.Location = New System.Drawing.Point(364, 72)
		Me.lblAdminLevel.Name = "lblAdminLevel"
		Me.lblAdminLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAdminLevel.Size = New System.Drawing.Size(58, 13)
		Me.lblAdminLevel.TabIndex = 5
		Me.lblAdminLevel.Text = "Admin Level"
		Me.lblAdminLevel.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAdminLevel.UseMnemonic = True
		Me.lblAdminLevel.Visible = True
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(360, 312)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 2
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
		Me.cmdCancel.Location = New System.Drawing.Point(280, 312)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
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
		Me.cmdOK.Location = New System.Drawing.Point(200, 312)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.BackgroundImage = CType(resources.GetObject("frmInterface.BackgroundImage"), System.Drawing.Image)
		Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(447, 342)
		Me.ControlBox = True
		Me.Controls.Add(Me.fraAdmin)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "User Access Administration"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboAdminLevel, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listViewHelper1.SetSorted(Me.lvwUsers, True)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwUsers, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraAdmin.ResumeLayout(False)
		Me.lvwUsers.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwUsers_InitializeColumnKeys()
		Me._lvwUsers_ColumnHeader_1.Name = ""
		Me._lvwUsers_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class