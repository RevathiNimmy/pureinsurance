<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctSumInsured
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSumInsured_InitializeColumnKeys()
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
	Friend WithEvents pnlPremium As System.Windows.Forms.Panel
	Friend WithEvents txtRate As System.Windows.Forms.TextBox
	Friend WithEvents pnlTotalSumInsured As System.Windows.Forms.Panel
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents _lvwSumInsured_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSumInsured_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwSumInsured As System.Windows.Forms.ListView
	Friend WithEvents cmdAdd As System.Windows.Forms.Button
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents lblPremium As System.Windows.Forms.Label
	Friend WithEvents lblRate As System.Windows.Forms.Label
	Friend WithEvents lblTotal As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctSumInsured))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pnlPremium = New System.Windows.Forms.Panel
		Me.txtRate = New System.Windows.Forms.TextBox
		Me.pnlTotalSumInsured = New System.Windows.Forms.Panel
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.lvwSumInsured = New System.Windows.Forms.ListView
		Me._lvwSumInsured_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwSumInsured_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.lblPremium = New System.Windows.Forms.Label
		Me.lblRate = New System.Windows.Forms.Label
		Me.lblTotal = New System.Windows.Forms.Label
		Me.lvwSumInsured.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pnlPremium
		' 
		Me.pnlPremium.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pnlPremium.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlPremium.Location = New System.Drawing.Point(376, 224)
		Me.pnlPremium.Name = "pnlPremium"
		Me.pnlPremium.Size = New System.Drawing.Size(177, 17)
		Me.pnlPremium.TabIndex = 9
		' 
		' txtRate
		' 
		Me.txtRate.AcceptsReturn = True
		Me.txtRate.AutoSize = False
		Me.txtRate.BackColor = System.Drawing.SystemColors.Window
		Me.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRate.CausesValidation = True
		Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRate.Enabled = False
		Me.txtRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRate.HideSelection = True
		Me.txtRate.Location = New System.Drawing.Point(96, 224)
		Me.txtRate.MaxLength = 0
		Me.txtRate.Multiline = False
		Me.txtRate.Name = "txtRate"
		Me.txtRate.ReadOnly = False
		Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRate.Size = New System.Drawing.Size(105, 19)
		Me.txtRate.TabIndex = 8
		Me.txtRate.TabStop = True
		Me.txtRate.Text = "0.2%"
		Me.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRate.Visible = True
		' 
		' pnlTotalSumInsured
		' 
		Me.pnlTotalSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pnlTotalSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlTotalSumInsured.Location = New System.Drawing.Point(376, 192)
		Me.pnlTotalSumInsured.Name = "pnlTotalSumInsured"
		Me.pnlTotalSumInsured.Size = New System.Drawing.Size(177, 17)
		Me.pnlTotalSumInsured.TabIndex = 5
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(80, 192)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 3
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwSumInsured
		' 
		Me.lvwSumInsured.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSumInsured.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSumInsured.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSumInsured.HideSelection = True
		Me.lvwSumInsured.LabelEdit = False
		Me.lvwSumInsured.LabelWrap = True
		Me.lvwSumInsured.Location = New System.Drawing.Point(0, 0)
		Me.lvwSumInsured.Name = "lvwSumInsured"
		Me.lvwSumInsured.Size = New System.Drawing.Size(553, 185)
		Me.lvwSumInsured.TabIndex = 2
		Me.lvwSumInsured.View = System.Windows.Forms.View.Details
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_1)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_2)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_3)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_4)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_5)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_6)
		Me.lvwSumInsured.Columns.Add(Me._lvwSumInsured_ColumnHeader_7)
		' 
		' _lvwSumInsured_ColumnHeader_1
		' 
		Me._lvwSumInsured_ColumnHeader_1.Tag = ""
		Me._lvwSumInsured_ColumnHeader_1.Text = "Description"
		Me._lvwSumInsured_ColumnHeader_1.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_2
		' 
		Me._lvwSumInsured_ColumnHeader_2.Tag = ""
		Me._lvwSumInsured_ColumnHeader_2.Text = "Reference"
		Me._lvwSumInsured_ColumnHeader_2.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_3
		' 
		Me._lvwSumInsured_ColumnHeader_3.Tag = ""
		Me._lvwSumInsured_ColumnHeader_3.Text = "Sum insured"
		Me._lvwSumInsured_ColumnHeader_3.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_4
		' 
		Me._lvwSumInsured_ColumnHeader_4.Tag = ""
		Me._lvwSumInsured_ColumnHeader_4.Text = "Date added"
		Me._lvwSumInsured_ColumnHeader_4.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_5
		' 
		Me._lvwSumInsured_ColumnHeader_5.Tag = ""
		Me._lvwSumInsured_ColumnHeader_5.Text = "Date deleted"
		Me._lvwSumInsured_ColumnHeader_5.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_6
		' 
		Me._lvwSumInsured_ColumnHeader_6.Tag = ""
		Me._lvwSumInsured_ColumnHeader_6.Text = "Valuation Required"
		Me._lvwSumInsured_ColumnHeader_6.Width = 97
		' 
		' _lvwSumInsured_ColumnHeader_7
		' 
		Me._lvwSumInsured_ColumnHeader_7.Tag = ""
		Me._lvwSumInsured_ColumnHeader_7.Text = "Valuation Date"
		Me._lvwSumInsured_ColumnHeader_7.Width = 97
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(0, 192)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 1
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "Add"
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
		Me.cmdDelete.Location = New System.Drawing.Point(160, 192)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
		Me.cmdDelete.TabIndex = 0
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblPremium
		' 
		Me.lblPremium.AutoSize = False
		Me.lblPremium.BackColor = System.Drawing.SystemColors.Control
		Me.lblPremium.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPremium.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPremium.Enabled = True
		Me.lblPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPremium.Location = New System.Drawing.Point(256, 224)
		Me.lblPremium.Name = "lblPremium"
		Me.lblPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPremium.Size = New System.Drawing.Size(73, 17)
		Me.lblPremium.TabIndex = 7
		Me.lblPremium.Text = "Premium:"
		Me.lblPremium.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPremium.UseMnemonic = True
		Me.lblPremium.Visible = True
		' 
		' lblRate
		' 
		Me.lblRate.AutoSize = False
		Me.lblRate.BackColor = System.Drawing.SystemColors.Control
		Me.lblRate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRate.Enabled = True
		Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRate.Location = New System.Drawing.Point(0, 224)
		Me.lblRate.Name = "lblRate"
		Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRate.Size = New System.Drawing.Size(73, 17)
		Me.lblRate.TabIndex = 6
		Me.lblRate.Text = "Rate:"
		Me.lblRate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRate.UseMnemonic = True
		Me.lblRate.Visible = True
		' 
		' lblTotal
		' 
		Me.lblTotal.AutoSize = False
		Me.lblTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotal.Enabled = True
		Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotal.Location = New System.Drawing.Point(256, 192)
		Me.lblTotal.Name = "lblTotal"
		Me.lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotal.Size = New System.Drawing.Size(121, 17)
		Me.lblTotal.TabIndex = 4
		Me.lblTotal.Text = "Total sum insured:"
		Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotal.UseMnemonic = True
		Me.lblTotal.Visible = True
		' 
		' uctSumInsured
		' 
		Me.ClientSize = New System.Drawing.Size(557, 249)
		Me.Controls.Add(Me.pnlPremium)
		Me.Controls.Add(Me.txtRate)
		Me.Controls.Add(Me.pnlTotalSumInsured)
		Me.Controls.Add(Me.cmdEdit)
		Me.Controls.Add(Me.lvwSumInsured)
		Me.Controls.Add(Me.cmdAdd)
		Me.Controls.Add(Me.cmdDelete)
		Me.Controls.Add(Me.lblPremium)
		Me.Controls.Add(Me.lblRate)
		Me.Controls.Add(Me.lblTotal)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctSumInsured"
		Me.lvwSumInsured.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwSumInsured_InitializeColumnKeys()
		Me._lvwSumInsured_ColumnHeader_1.Name = ""
		Me._lvwSumInsured_ColumnHeader_2.Name = ""
		Me._lvwSumInsured_ColumnHeader_3.Name = ""
		Me._lvwSumInsured_ColumnHeader_4.Name = ""
		Me._lvwSumInsured_ColumnHeader_5.Name = ""
		Me._lvwSumInsured_ColumnHeader_6.Name = ""
		Me._lvwSumInsured_ColumnHeader_7.Name = ""
	End Sub
#End Region 
End Class