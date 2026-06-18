<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwDetails_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Private WithEvents _lvwDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwDetails As System.Windows.Forms.ListView
	Public WithEvents txtDate As System.Windows.Forms.TextBox
	Public WithEvents txtPercentage As System.Windows.Forms.TextBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.lvwDetails = New System.Windows.Forms.ListView
		Me._lvwDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.txtDate = New System.Windows.Forms.TextBox
		Me.txtPercentage = New System.Windows.Forms.TextBox
		Me.txtCurrency = New System.Windows.Forms.TextBox
		Me.ImageList1 = New System.Windows.Forms.ImageList
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.lvwDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = False
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(232, 312)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 12
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "&Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 312)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 4
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
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(472, 312)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 7
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
		Me.cmdCancel.Location = New System.Drawing.Point(392, 312)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 6
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
		Me.cmdOK.Location = New System.Drawing.Point(312, 312)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 5
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
		Me.tabMainTab.ItemSize = New System.Drawing.Size(106, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(541, 301)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 8
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwDetails)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtPercentage)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCurrency)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(8, 244)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 1
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
		Me.cmdEdit.Location = New System.Drawing.Point(88, 244)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 2
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
		Me.cmdDelete.Location = New System.Drawing.Point(168, 244)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwDetails
		' 
		Me.lvwDetails.BackColor = System.Drawing.SystemColors.Window
		Me.lvwDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwDetails.HideSelection = True
		Me.lvwDetails.LabelEdit = False
		Me.lvwDetails.LabelWrap = True
		Me.lvwDetails.LargeImageList = ImageList1
		Me.lvwDetails.Location = New System.Drawing.Point(8, 12)
		Me.lvwDetails.Name = "lvwDetails"
		Me.lvwDetails.Size = New System.Drawing.Size(513, 225)
		Me.lvwDetails.SmallImageList = ImageList1
		Me.lvwDetails.TabIndex = 0
		Me.lvwDetails.View = System.Windows.Forms.View.Details
		Me.lvwDetails.Columns.Add(Me._lvwDetails_ColumnHeader_1)
		Me.lvwDetails.Columns.Add(Me._lvwDetails_ColumnHeader_2)
		' 
		' _lvwDetails_ColumnHeader_1
		' 
		Me._lvwDetails_ColumnHeader_1.Tag = ""
		Me._lvwDetails_ColumnHeader_1.Text = ""
		Me._lvwDetails_ColumnHeader_1.Width = 97
		' 
		' _lvwDetails_ColumnHeader_2
		' 
		Me._lvwDetails_ColumnHeader_2.Tag = ""
		Me._lvwDetails_ColumnHeader_2.Text = ""
		Me._lvwDetails_ColumnHeader_2.Width = 97
		' 
		' txtDate
		' 
		Me.txtDate.AcceptsReturn = True
		Me.txtDate.AutoSize = False
		Me.txtDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDate.CausesValidation = True
		Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDate.Enabled = True
		Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDate.HideSelection = True
		Me.txtDate.Location = New System.Drawing.Point(272, 244)
		Me.txtDate.MaxLength = 0
		Me.txtDate.Multiline = False
		Me.txtDate.Name = "txtDate"
		Me.txtDate.ReadOnly = False
		Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDate.Size = New System.Drawing.Size(105, 19)
		Me.txtDate.TabIndex = 9
		Me.txtDate.TabStop = True
		Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDate.Visible = False
		' 
		' txtPercentage
		' 
		Me.txtPercentage.AcceptsReturn = True
		Me.txtPercentage.AutoSize = False
		Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
		Me.txtPercentage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPercentage.CausesValidation = True
		Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPercentage.Enabled = True
		Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPercentage.HideSelection = True
		Me.txtPercentage.Location = New System.Drawing.Point(352, 244)
		Me.txtPercentage.MaxLength = 0
		Me.txtPercentage.Multiline = False
		Me.txtPercentage.Name = "txtPercentage"
		Me.txtPercentage.ReadOnly = False
		Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPercentage.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPercentage.Size = New System.Drawing.Size(105, 19)
		Me.txtPercentage.TabIndex = 10
		Me.txtPercentage.TabStop = True
		Me.txtPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPercentage.Visible = False
		' 
		' txtCurrency
		' 
		Me.txtCurrency.AcceptsReturn = True
		Me.txtCurrency.AutoSize = False
		Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
		Me.txtCurrency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCurrency.CausesValidation = True
		Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCurrency.Enabled = True
		Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCurrency.HideSelection = True
		Me.txtCurrency.Location = New System.Drawing.Point(392, 244)
		Me.txtCurrency.MaxLength = 0
		Me.txtCurrency.Multiline = False
		Me.txtCurrency.Name = "txtCurrency"
		Me.txtCurrency.ReadOnly = False
		Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCurrency.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCurrency.Size = New System.Drawing.Size(105, 19)
		Me.txtCurrency.TabIndex = 11
		Me.txtCurrency.TabStop = True
		Me.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCurrency.Visible = False
		' 
		' ImageList1
		' 
		Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList1.Images.SetKeyName(0, "Main")
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(553, 341)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Lookup Details"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwDetails, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.lvwDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwDetails_InitializeColumnKeys()
		Me._lvwDetails_ColumnHeader_1.Name = ""
		Me._lvwDetails_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class