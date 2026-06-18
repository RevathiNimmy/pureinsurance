<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWorkflowPackages
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwPackages_InitializeColumnKeys()
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
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwPackages_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPackages_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPackages_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPackages As System.Windows.Forms.ListView
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents imgIcons As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWorkflowPackages))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.lvwPackages = New System.Windows.Forms.ListView
		Me._lvwPackages_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwPackages_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwPackages_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.imgIcons = New System.Windows.Forms.ImageList
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.lvwPackages.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(9, 354)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = False
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(492, 357)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 23)
		Me.cmdApply.TabIndex = 4
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
		Me.cmdCancel.Location = New System.Drawing.Point(412, 357)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
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
		Me.cmdOK.Location = New System.Drawing.Point(332, 357)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 23)
		Me.cmdOK.TabIndex = 5
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
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(184, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(561, 344)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 7
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdDelete)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMain_TabPage0.Controls.Add(Me.lvwPackages)
		Me._tabMain_TabPage0.Text = "&1 - Packages"
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = False
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(472, 39)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 23)
		Me.cmdEdit.TabIndex = 2
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit..."
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = False
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(472, 69)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 23)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "Dele&te"
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
		Me.cmdAdd.Location = New System.Drawing.Point(472, 9)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 23)
		Me.cmdAdd.TabIndex = 1
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "A&dd..."
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwPackages
		' 
		Me.lvwPackages.BackColor = System.Drawing.SystemColors.Window
		Me.lvwPackages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwPackages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwPackages.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwPackages.FullRowSelect = True
		Me.lvwPackages.HideSelection = False
		Me.lvwPackages.LabelEdit = False
		Me.lvwPackages.LabelWrap = True
		Me.lvwPackages.Location = New System.Drawing.Point(8, 9)
		Me.lvwPackages.Name = "lvwPackages"
		Me.lvwPackages.Size = New System.Drawing.Size(455, 301)
		Me.lvwPackages.SmallImageList = imgIcons
		Me.lvwPackages.TabIndex = 0
		Me.lvwPackages.View = System.Windows.Forms.View.Details
		Me.lvwPackages.Columns.Add(Me._lvwPackages_ColumnHeader_1)
		Me.lvwPackages.Columns.Add(Me._lvwPackages_ColumnHeader_2)
		Me.lvwPackages.Columns.Add(Me._lvwPackages_ColumnHeader_3)
		' 
		' _lvwPackages_ColumnHeader_1
		' 
		Me._lvwPackages_ColumnHeader_1.Text = "Code"
		Me._lvwPackages_ColumnHeader_1.Width = 114
		' 
		' _lvwPackages_ColumnHeader_2
		' 
		Me._lvwPackages_ColumnHeader_2.Text = "Description"
		Me._lvwPackages_ColumnHeader_2.Width = 194
		' 
		' _lvwPackages_ColumnHeader_3
		' 
		Me._lvwPackages_ColumnHeader_3.Text = "Effective Date"
		Me._lvwPackages_ColumnHeader_3.Width = 121
		' 
		' imgIcons
		' 
		Me.imgIcons.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imgIcons.Images.SetKeyName(0, "Folder")
		' 
		' frmWorkflowPackages
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(573, 387)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = False
		Me.Name = "frmWorkflowPackages"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Workflow Packages"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetSorted(Me.lvwPackages, True)
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwPackages, "lvwPackages_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPackages, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.lvwPackages.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwPackages_InitializeColumnKeys()
		Me._lvwPackages_ColumnHeader_1.Name = "Code"
		Me._lvwPackages_ColumnHeader_2.Name = "Description"
		Me._lvwPackages_ColumnHeader_3.Name = "EffectiveDate"
	End Sub
#End Region 
End Class