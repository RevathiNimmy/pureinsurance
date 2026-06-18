<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmList
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdBanking As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdOpen As System.Windows.Forms.Button
	Private WithEvents _lvwCashListDrawers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCashListDrawers_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCashListDrawers As System.Windows.Forms.ListView
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
		Me.cmdBanking = New System.Windows.Forms.Button
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdOpen = New System.Windows.Forms.Button
		Me.lvwCashListDrawers = New System.Windows.Forms.ListView
		Me._lvwCashListDrawers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwCashListDrawers_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me.lvwCashListDrawers.SuspendLayout()
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
		Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 324)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 4
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Tag = "CAP;203"
		Me.cmdNavigate.Text = "*{Navigate}"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdBanking
		' 
		Me.cmdBanking.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBanking.CausesValidation = True
		Me.cmdBanking.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBanking.Enabled = True
		Me.cmdBanking.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdBanking.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBanking.Location = New System.Drawing.Point(88, 300)
		Me.cmdBanking.Name = "cmdBanking"
		Me.cmdBanking.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdBanking.Size = New System.Drawing.Size(73, 22)
		Me.cmdBanking.TabIndex = 2
		Me.cmdBanking.TabStop = True
		Me.cmdBanking.Tag = "CAP;411"
		Me.cmdBanking.Text = "*{Banking}"
		Me.cmdBanking.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdBanking.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdClose
		' 
		Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClose.CausesValidation = True
		Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClose.Enabled = True
		Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClose.Location = New System.Drawing.Point(236, 300)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClose.Size = New System.Drawing.Size(73, 22)
		Me.cmdClose.TabIndex = 3
		Me.cmdClose.TabStop = True
		Me.cmdClose.Tag = "CAP;202"
		Me.cmdClose.Text = "*{Cancel}"
		Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOpen
		' 
		Me.cmdOpen.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOpen.CausesValidation = True
		Me.cmdOpen.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOpen.Enabled = True
		Me.cmdOpen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOpen.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOpen.Location = New System.Drawing.Point(8, 301)
		Me.cmdOpen.Name = "cmdOpen"
		Me.cmdOpen.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOpen.Size = New System.Drawing.Size(73, 22)
		Me.cmdOpen.TabIndex = 1
		Me.cmdOpen.TabStop = True
		Me.cmdOpen.Tag = "CAP;410"
		Me.cmdOpen.Text = "*{Open}"
		Me.cmdOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwCashListDrawers
		' 
		Me.lvwCashListDrawers.BackColor = System.Drawing.SystemColors.Window
		Me.lvwCashListDrawers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwCashListDrawers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwCashListDrawers.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwCashListDrawers.FullRowSelect = True
		Me.lvwCashListDrawers.HideSelection = False
		Me.lvwCashListDrawers.LabelEdit = False
		Me.lvwCashListDrawers.LabelWrap = True
		Me.lvwCashListDrawers.Location = New System.Drawing.Point(8, 8)
		Me.lvwCashListDrawers.Name = "lvwCashListDrawers"
		Me.lvwCashListDrawers.Size = New System.Drawing.Size(305, 285)
		Me.lvwCashListDrawers.TabIndex = 0
		Me.lvwCashListDrawers.Tag = "CAP;401"
		Me.lvwCashListDrawers.View = System.Windows.Forms.View.Details
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_1)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_2)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_3)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_4)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_5)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_6)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_7)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_8)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_9)
		Me.lvwCashListDrawers.Columns.Add(Me._lvwCashListDrawers_ColumnHeader_10)
		' 
		' _lvwCashListDrawers_ColumnHeader_1
		' 
		Me._lvwCashListDrawers_ColumnHeader_1.Text = "*{Drawer Name}"
		Me._lvwCashListDrawers_ColumnHeader_1.Width = 147
		' 
		' _lvwCashListDrawers_ColumnHeader_2
		' 
		Me._lvwCashListDrawers_ColumnHeader_2.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_2.Text = "*{Drawer Id}"
		Me._lvwCashListDrawers_ColumnHeader_2.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_3
		' 
		Me._lvwCashListDrawers_ColumnHeader_3.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_3.Text = "*{Cash List Id}"
		Me._lvwCashListDrawers_ColumnHeader_3.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_4
		' 
		Me._lvwCashListDrawers_ColumnHeader_4.Text = "*{Open}"
		Me._lvwCashListDrawers_ColumnHeader_4.Width = 28
		' 
		' _lvwCashListDrawers_ColumnHeader_5
		' 
		Me._lvwCashListDrawers_ColumnHeader_5.Text = "*{Multi-User}"
		Me._lvwCashListDrawers_ColumnHeader_5.Width = 67
		' 
		' _lvwCashListDrawers_ColumnHeader_6
		' 
		Me._lvwCashListDrawers_ColumnHeader_6.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_6.Text = "*{Bank Account Id}"
		Me._lvwCashListDrawers_ColumnHeader_6.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_7
		' 
		Me._lvwCashListDrawers_ColumnHeader_7.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_7.Text = "*{Cash Float Amount}"
		Me._lvwCashListDrawers_ColumnHeader_7.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_8
		' 
		Me._lvwCashListDrawers_ColumnHeader_8.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_8.Text = "*{Auto Close}"
		Me._lvwCashListDrawers_ColumnHeader_8.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_9
		' 
		Me._lvwCashListDrawers_ColumnHeader_9.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_9.Text = "*{Branch ID}"
		Me._lvwCashListDrawers_ColumnHeader_9.Width = 0
		' 
		' _lvwCashListDrawers_ColumnHeader_10
		' 
		Me._lvwCashListDrawers_ColumnHeader_10.Tag = "HIDDEN"
		Me._lvwCashListDrawers_ColumnHeader_10.Text = "*{Sub Branch ID}"
		Me._lvwCashListDrawers_ColumnHeader_10.Width = 0
		' 
		' frmList
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(323, 328)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdBanking)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.cmdOpen)
		Me.Controls.Add(Me.lvwCashListDrawers)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmList.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmList"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "CAP;400"
		Me.Text = "*{InterfaceTitle}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetItemClickMethod(Me.lvwCashListDrawers, "lvwCashListDrawers_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCashListDrawers, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwCashListDrawers.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class