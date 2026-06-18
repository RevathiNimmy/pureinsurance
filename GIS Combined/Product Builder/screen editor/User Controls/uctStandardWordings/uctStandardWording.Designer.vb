<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctStandardWording
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwStandardWording_InitializeColumnKeys()
		UserControl_Initialize()
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
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents cmdAdd As System.Windows.Forms.Button
	Friend WithEvents cmdUp As System.Windows.Forms.Button
	Friend WithEvents cmdDown As System.Windows.Forms.Button
	Friend WithEvents _lvwStandardWording_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwStandardWording_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwStandardWording As System.Windows.Forms.ListView
	Friend WithEvents lblMove As System.Windows.Forms.Label
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctStandardWording))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdUp = New System.Windows.Forms.Button
		Me.cmdDown = New System.Windows.Forms.Button
		Me.lvwStandardWording = New System.Windows.Forms.ListView
		Me._lvwStandardWording_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwStandardWording_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.lblMove = New System.Windows.Forms.Label
		Me.lvwStandardWording.SuspendLayout()
		Me.SuspendLayout()
		Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(80, 200)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "Delete"
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
		Me.cmdAdd.Location = New System.Drawing.Point(0, 200)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 2
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdUp
		' 
		Me.cmdUp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdUp.CausesValidation = True
		Me.cmdUp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdUp.Enabled = True
		Me.cmdUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdUp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdUp.Image = CType(resources.GetObject("cmdUp.Image"), System.Drawing.Image)
		Me.cmdUp.Location = New System.Drawing.Point(544, 16)
		Me.cmdUp.Name = "cmdUp"
		Me.cmdUp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdUp.Size = New System.Drawing.Size(33, 33)
		Me.cmdUp.TabIndex = 1
		Me.cmdUp.TabStop = True
		Me.cmdUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.cmdUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDown
		' 
		Me.cmdDown.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDown.CausesValidation = True
		Me.cmdDown.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDown.Enabled = True
		Me.cmdDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDown.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDown.Image = CType(resources.GetObject("cmdDown.Image"), System.Drawing.Image)
		Me.cmdDown.Location = New System.Drawing.Point(544, 152)
		Me.cmdDown.Name = "cmdDown"
		Me.cmdDown.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDown.Size = New System.Drawing.Size(33, 33)
		Me.cmdDown.TabIndex = 0
		Me.cmdDown.TabStop = True
		Me.cmdDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.cmdDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwStandardWording
		' 
		Me.lvwStandardWording.BackColor = System.Drawing.SystemColors.Window
		Me.lvwStandardWording.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwStandardWording.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwStandardWording.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwStandardWording.HideSelection = True
		Me.lvwStandardWording.LabelEdit = False
		Me.lvwStandardWording.LabelWrap = True
		Me.lvwStandardWording.Location = New System.Drawing.Point(0, 0)
		Me.lvwStandardWording.Name = "lvwStandardWording"
		Me.lvwStandardWording.Size = New System.Drawing.Size(529, 193)
		Me.lvwStandardWording.TabIndex = 4
		Me.lvwStandardWording.View = System.Windows.Forms.View.Details
		Me.lvwStandardWording.Columns.Add(Me._lvwStandardWording_ColumnHeader_1)
		Me.lvwStandardWording.Columns.Add(Me._lvwStandardWording_ColumnHeader_2)
		' 
		' _lvwStandardWording_ColumnHeader_1
		' 
		Me._lvwStandardWording_ColumnHeader_1.Tag = ""
		Me._lvwStandardWording_ColumnHeader_1.Text = "Code"
		Me._lvwStandardWording_ColumnHeader_1.Width = 97
		' 
		' _lvwStandardWording_ColumnHeader_2
		' 
		Me._lvwStandardWording_ColumnHeader_2.Tag = ""
		Me._lvwStandardWording_ColumnHeader_2.Text = "Description"
		Me._lvwStandardWording_ColumnHeader_2.Width = 385
		' 
		' lblMove
		' 
		Me.lblMove.AutoSize = False
		Me.lblMove.BackColor = System.Drawing.SystemColors.Control
		Me.lblMove.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMove.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMove.Enabled = True
		Me.lblMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMove.Location = New System.Drawing.Point(544, 80)
		Me.lblMove.Name = "lblMove"
		Me.lblMove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMove.Size = New System.Drawing.Size(33, 17)
		Me.lblMove.TabIndex = 5
		Me.lblMove.Text = "Move"
		Me.lblMove.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblMove.UseMnemonic = True
		Me.lblMove.Visible = True
		' 
		' uctStandardWording
		' 
		Me.ClientSize = New System.Drawing.Size(617, 269)
		Me.Controls.Add(Me.cmdDelete)
		Me.Controls.Add(Me.cmdAdd)
		Me.Controls.Add(Me.cmdUp)
		Me.Controls.Add(Me.cmdDown)
		Me.Controls.Add(Me.lvwStandardWording)
		Me.Controls.Add(Me.lblMove)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctStandardWording"
		Me.commandButtonHelper1.SetStyle(Me.cmdUp, 1)
		Me.commandButtonHelper1.SetStyle(Me.cmdDown, 1)
		Me.lvwStandardWording.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwStandardWording_InitializeColumnKeys()
		Me._lvwStandardWording_ColumnHeader_1.Name = ""
		Me._lvwStandardWording_ColumnHeader_2.Name = ""
	End Sub
#End Region 
End Class