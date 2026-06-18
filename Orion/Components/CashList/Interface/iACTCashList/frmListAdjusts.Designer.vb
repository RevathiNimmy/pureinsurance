<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListAdjusts
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
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdOpen As System.Windows.Forms.Button
	Private WithEvents _lvwAdjustments_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAdjustments_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAdjustments_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAdjustments_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAdjustments_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAdjustments_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAdjustments As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmListAdjusts))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdOpen = New System.Windows.Forms.Button
		Me.lvwAdjustments = New System.Windows.Forms.ListView
		Me._lvwAdjustments_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwAdjustments_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwAdjustments_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwAdjustments_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwAdjustments_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwAdjustments_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me.lvwAdjustments.SuspendLayout()
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
		Me.cmdNavigate.TabIndex = 3
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Tag = "CAP;203"
		Me.cmdNavigate.Text = "*{Navigate}"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
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
		Me.cmdClose.TabIndex = 2
		Me.cmdClose.TabStop = True
		Me.cmdClose.Tag = "CAP;711"
		Me.cmdClose.Text = "*{Close}"
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
		Me.cmdOpen.Location = New System.Drawing.Point(8, 300)
		Me.cmdOpen.Name = "cmdOpen"
		Me.cmdOpen.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOpen.Size = New System.Drawing.Size(73, 22)
		Me.cmdOpen.TabIndex = 1
		Me.cmdOpen.TabStop = True
		Me.cmdOpen.Tag = "CAP;710"
		Me.cmdOpen.Text = "*{View}"
		Me.cmdOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwAdjustments
		' 
		Me.lvwAdjustments.BackColor = System.Drawing.SystemColors.Window
		Me.lvwAdjustments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwAdjustments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwAdjustments.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwAdjustments.FullRowSelect = True
		Me.lvwAdjustments.HideSelection = False
		Me.lvwAdjustments.LabelEdit = False
		Me.lvwAdjustments.LabelWrap = True
		Me.lvwAdjustments.Location = New System.Drawing.Point(8, 8)
		Me.lvwAdjustments.Name = "lvwAdjustments"
		Me.lvwAdjustments.Size = New System.Drawing.Size(305, 285)
		Me.lvwAdjustments.TabIndex = 0
		Me.lvwAdjustments.Tag = "CAP;720"
		Me.lvwAdjustments.View = System.Windows.Forms.View.Details
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_1)
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_2)
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_3)
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_4)
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_5)
		Me.lvwAdjustments.Columns.Add(Me._lvwAdjustments_ColumnHeader_6)
		' 
		' _lvwAdjustments_ColumnHeader_1
		' 
		Me._lvwAdjustments_ColumnHeader_1.Text = "*{User}"
		Me._lvwAdjustments_ColumnHeader_1.Width = 44
		' 
		' _lvwAdjustments_ColumnHeader_2
		' 
		Me._lvwAdjustments_ColumnHeader_2.Text = "*{Amount}"
		Me._lvwAdjustments_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwAdjustments_ColumnHeader_2.Width = 51
		' 
		' _lvwAdjustments_ColumnHeader_3
		' 
		Me._lvwAdjustments_ColumnHeader_3.Text = "*{Method}"
		Me._lvwAdjustments_ColumnHeader_3.Width = 147
		' 
		' _lvwAdjustments_ColumnHeader_4
		' 
		Me._lvwAdjustments_ColumnHeader_4.Tag = "HIDDEN"
		Me._lvwAdjustments_ColumnHeader_4.Text = "*{Adjustment Date}"
		Me._lvwAdjustments_ColumnHeader_4.Width = 0
		' 
		' _lvwAdjustments_ColumnHeader_5
		' 
		Me._lvwAdjustments_ColumnHeader_5.Tag = "HIDDEN"
		Me._lvwAdjustments_ColumnHeader_5.Text = "*{Comments}"
		Me._lvwAdjustments_ColumnHeader_5.Width = 0
		' 
		' _lvwAdjustments_ColumnHeader_6
		' 
		Me._lvwAdjustments_ColumnHeader_6.Tag = "HIDDEN"
		Me._lvwAdjustments_ColumnHeader_6.Text = "*{Cash Drawer}"
		Me._lvwAdjustments_ColumnHeader_6.Width = 0
		' 
		' frmListAdjusts
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(323, 328)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.cmdOpen)
		Me.Controls.Add(Me.lvwAdjustments)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmListAdjusts.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmListAdjusts"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "CAP;700"
		Me.Text = "*{InterfaceTitle}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwAdjustments.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class