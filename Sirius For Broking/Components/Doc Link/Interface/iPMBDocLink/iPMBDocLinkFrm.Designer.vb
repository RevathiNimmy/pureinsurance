<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblSchemeVersion As System.Windows.Forms.Label
	Public WithEvents lblGISScheme As System.Windows.Forms.Label
	Public WithEvents imgLogo As System.Windows.Forms.PictureBox
	Private WithEvents _lvwDocLink_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDocLink_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDocLink_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDocLink_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDocLink_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDocLink_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwDocLink As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cboSchemeVersion As System.Windows.Forms.ComboBox
	Public WithEvents cboGISScheme As System.Windows.Forms.ComboBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblSchemeVersion = New System.Windows.Forms.Label
		Me.lblGISScheme = New System.Windows.Forms.Label
		Me.imgLogo = New System.Windows.Forms.PictureBox
		Me.lvwDocLink = New System.Windows.Forms.ListView
		Me._lvwDocLink_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwDocLink_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwDocLink_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwDocLink_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwDocLink_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwDocLink_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cboSchemeVersion = New System.Windows.Forms.ComboBox
		Me.cboGISScheme = New System.Windows.Forms.ComboBox
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.lvwDocLink.SuspendLayout()
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
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(9, 320)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 9
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(583, 320)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 12
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "&Apply"
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
		Me.cmdCancel.Location = New System.Drawing.Point(503, 320)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 11
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
		Me.cmdOK.Location = New System.Drawing.Point(423, 320)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 10
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
		Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.HotTrack = False
		Me.tabMain.ItemSize = New System.Drawing.Size(215, 18)
		Me.tabMain.Location = New System.Drawing.Point(9, 0)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(652, 317)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 0
		Me.tabMain.TabStop = False
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblSchemeVersion)
		Me._tabMain_TabPage0.Controls.Add(Me.lblGISScheme)
		Me._tabMain_TabPage0.Controls.Add(Me.imgLogo)
		Me._tabMain_TabPage0.Controls.Add(Me.lvwDocLink)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMain_TabPage0.Controls.Add(Me.cboSchemeVersion)
		Me._tabMain_TabPage0.Controls.Add(Me.cboGISScheme)
		Me._tabMain_TabPage0.Text = "&1 - General"
		' 
		' lblSchemeVersion
		' 
		Me.lblSchemeVersion.AutoSize = False
		Me.lblSchemeVersion.BackColor = System.Drawing.SystemColors.Control
		Me.lblSchemeVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSchemeVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSchemeVersion.Enabled = True
		Me.lblSchemeVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSchemeVersion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSchemeVersion.Location = New System.Drawing.Point(16, 40)
		Me.lblSchemeVersion.Name = "lblSchemeVersion"
		Me.lblSchemeVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSchemeVersion.Size = New System.Drawing.Size(57, 17)
		Me.lblSchemeVersion.TabIndex = 3
		Me.lblSchemeVersion.Text = "Version"
		Me.lblSchemeVersion.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSchemeVersion.UseMnemonic = True
		Me.lblSchemeVersion.Visible = True
		' 
		' lblGISScheme
		' 
		Me.lblGISScheme.AutoSize = False
		Me.lblGISScheme.BackColor = System.Drawing.SystemColors.Control
		Me.lblGISScheme.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGISScheme.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGISScheme.Enabled = True
		Me.lblGISScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGISScheme.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGISScheme.Location = New System.Drawing.Point(16, 12)
		Me.lblGISScheme.Name = "lblGISScheme"
		Me.lblGISScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGISScheme.Size = New System.Drawing.Size(113, 17)
		Me.lblGISScheme.TabIndex = 1
		Me.lblGISScheme.Text = "Scheme "
		Me.lblGISScheme.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblGISScheme.UseMnemonic = True
		Me.lblGISScheme.Visible = True
		' 
		' imgLogo
		' 
		Me.imgLogo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgLogo.Enabled = True
		Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
		Me.imgLogo.Location = New System.Drawing.Point(522, 16)
		Me.imgLogo.Name = "imgLogo"
		Me.imgLogo.Size = New System.Drawing.Size(32, 32)
		Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgLogo.Visible = True
		' 
		' lvwDocLink
		' 
		Me.lvwDocLink.BackColor = System.Drawing.SystemColors.Window
		Me.lvwDocLink.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwDocLink.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwDocLink.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwDocLink.FullRowSelect = True
		Me.lvwDocLink.GridLines = True
		Me.lvwDocLink.HideSelection = True
		Me.lvwDocLink.LabelEdit = False
		Me.lvwDocLink.LabelWrap = True
		Me.lvwDocLink.Location = New System.Drawing.Point(8, 68)
		Me.lvwDocLink.Name = "lvwDocLink"
		Me.lvwDocLink.Size = New System.Drawing.Size(633, 188)
		Me.lvwDocLink.TabIndex = 5
		Me.lvwDocLink.View = System.Windows.Forms.View.Details
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_1)
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_2)
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_3)
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_4)
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_5)
		Me.lvwDocLink.Columns.Add(Me._lvwDocLink_ColumnHeader_6)
		' 
		' _lvwDocLink_ColumnHeader_1
		' 
		Me._lvwDocLink_ColumnHeader_1.Text = "Process"
		Me._lvwDocLink_ColumnHeader_1.Width = 116
		' 
		' _lvwDocLink_ColumnHeader_2
		' 
		Me._lvwDocLink_ColumnHeader_2.Text = "Type"
		Me._lvwDocLink_ColumnHeader_2.Width = 97
		' 
		' _lvwDocLink_ColumnHeader_3
		' 
		Me._lvwDocLink_ColumnHeader_3.Text = "Template Name"
		Me._lvwDocLink_ColumnHeader_3.Width = 166
		' 
		' _lvwDocLink_ColumnHeader_4
		' 
		Me._lvwDocLink_ColumnHeader_4.Text = "Agent"
		Me._lvwDocLink_ColumnHeader_4.Width = 83
		' 
		' _lvwDocLink_ColumnHeader_5
		' 
		Me._lvwDocLink_ColumnHeader_5.Text = "Print or Spool"
		Me._lvwDocLink_ColumnHeader_5.Width = 81
		' 
		' _lvwDocLink_ColumnHeader_6
		' 
		Me._lvwDocLink_ColumnHeader_6.Text = "Auto Archive"
		Me._lvwDocLink_ColumnHeader_6.Width = 81
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(8, 260)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 6
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(168, 260)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 8
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(88, 260)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 7
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboSchemeVersion
		' 
		Me.cboSchemeVersion.BackColor = System.Drawing.SystemColors.Window
		Me.cboSchemeVersion.CausesValidation = True
		Me.cboSchemeVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboSchemeVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboSchemeVersion.Enabled = True
		Me.cboSchemeVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboSchemeVersion.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboSchemeVersion.IntegralHeight = True
		Me.cboSchemeVersion.Location = New System.Drawing.Point(80, 38)
		Me.cboSchemeVersion.Name = "cboSchemeVersion"
		Me.cboSchemeVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboSchemeVersion.Size = New System.Drawing.Size(80, 21)
		Me.cboSchemeVersion.Sorted = False
		Me.cboSchemeVersion.TabIndex = 4
		Me.cboSchemeVersion.TabStop = True
		Me.cboSchemeVersion.Visible = True
		' 
		' cboGISScheme
		' 
		Me.cboGISScheme.BackColor = System.Drawing.SystemColors.Window
		Me.cboGISScheme.CausesValidation = True
		Me.cboGISScheme.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboGISScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboGISScheme.Enabled = True
		Me.cboGISScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboGISScheme.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboGISScheme.IntegralHeight = True
		Me.cboGISScheme.Location = New System.Drawing.Point(80, 12)
		Me.cboGISScheme.Name = "cboGISScheme"
		Me.cboGISScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboGISScheme.Size = New System.Drawing.Size(417, 21)
		Me.cboGISScheme.Sorted = False
		Me.cboGISScheme.TabIndex = 2
		Me.cboGISScheme.TabStop = True
		Me.cboGISScheme.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(661, 350)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(241, 85)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Document Link"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwDocLink, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.lvwDocLink.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class