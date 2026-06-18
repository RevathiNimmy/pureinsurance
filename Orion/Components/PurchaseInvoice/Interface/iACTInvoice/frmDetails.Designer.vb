<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblNominalAccount As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblCostCentre As System.Windows.Forms.Label
	Public WithEvents lblDeptAmount As System.Windows.Forms.Label
	Public WithEvents lblTotalIncVat As System.Windows.Forms.Label
	Public WithEvents lblTax As System.Windows.Forms.Label
	Public WithEvents txtTaxGroup As System.Windows.Forms.Label
	Public WithEvents cboPMTax As PMLookupControl.cboPMLookup
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents uctNominalAccount As UserControls.AccountLookup
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents cboCostCentre As PMLookupControl.cboPMLookup
	Public WithEvents txtDeptAmount As System.Windows.Forms.TextBox
	Public WithEvents cboValType As System.Windows.Forms.ComboBox
	Public WithEvents txtVATAmount As System.Windows.Forms.TextBox
	Public WithEvents txtTotalIncVat As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblNominalAccount = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblCostCentre = New System.Windows.Forms.Label
        Me.lblDeptAmount = New System.Windows.Forms.Label
        Me.lblTotalIncVat = New System.Windows.Forms.Label
        Me.lblTax = New System.Windows.Forms.Label
        Me.txtTaxGroup = New System.Windows.Forms.Label
        Me.cboPMTax = New PMLookupControl.cboPMLookup
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.uctNominalAccount = New UserControls.AccountLookup
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.cboCostCentre = New PMLookupControl.cboPMLookup
        Me.txtDeptAmount = New System.Windows.Forms.TextBox
        Me.cboValType = New System.Windows.Forms.ComboBox
        Me.txtVATAmount = New System.Windows.Forms.TextBox
        Me.txtTotalIncVat = New System.Windows.Forms.TextBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(360, 336)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(280, 336)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(140, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(429, 321)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMainTab.TabIndex = 11
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNominalAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCostCentre)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDeptAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalIncVat)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTax)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTaxGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMTax)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctNominalAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCostCentre)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDeptAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboValType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtVATAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtTotalIncVat)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(421, 295)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "General"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(24, 24)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 12
        Me.lblDescription.Text = "Description:"
        '
        'lblNominalAccount
        '
        Me.lblNominalAccount.AutoSize = True
        Me.lblNominalAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblNominalAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNominalAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNominalAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNominalAccount.Location = New System.Drawing.Point(24, 56)
        Me.lblNominalAccount.Name = "lblNominalAccount"
        Me.lblNominalAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNominalAccount.Size = New System.Drawing.Size(91, 13)
        Me.lblNominalAccount.TabIndex = 13
        Me.lblNominalAccount.Text = "Nominal Account:"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(24, 88)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblAmount.TabIndex = 14
        Me.lblAmount.Text = "Amount:"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.BackColor = System.Drawing.SystemColors.Control
        Me.lblCostCentre.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCostCentre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostCentre.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCostCentre.Location = New System.Drawing.Point(24, 220)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCostCentre.Size = New System.Drawing.Size(65, 13)
        Me.lblCostCentre.TabIndex = 15
        Me.lblCostCentre.Text = "Cost Centre:"
        '
        'lblDeptAmount
        '
        Me.lblDeptAmount.AutoSize = True
        Me.lblDeptAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeptAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeptAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeptAmount.Location = New System.Drawing.Point(24, 260)
        Me.lblDeptAmount.Name = "lblDeptAmount"
        Me.lblDeptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeptAmount.Size = New System.Drawing.Size(104, 13)
        Me.lblDeptAmount.TabIndex = 16
        Me.lblDeptAmount.Text = "Cost Centre Amount:"
        '
        'lblTotalIncVat
        '
        Me.lblTotalIncVat.AutoSize = True
        Me.lblTotalIncVat.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalIncVat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalIncVat.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalIncVat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalIncVat.Location = New System.Drawing.Point(24, 180)
        Me.lblTotalIncVat.Name = "lblTotalIncVat"
        Me.lblTotalIncVat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalIncVat.Size = New System.Drawing.Size(84, 13)
        Me.lblTotalIncVat.TabIndex = 17
        Me.lblTotalIncVat.Text = "Total (inc. TAX):"
        '
        'lblTax
        '
        Me.lblTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTax.Location = New System.Drawing.Point(24, 148)
        Me.lblTax.Name = "lblTax"
        Me.lblTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTax.Size = New System.Drawing.Size(89, 13)
        Me.lblTax.TabIndex = 18
        Me.lblTax.Text = "Tax:"
        '
        'txtTaxGroup
        '
        Me.txtTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtTaxGroup.Location = New System.Drawing.Point(24, 116)
        Me.txtTaxGroup.Name = "txtTaxGroup"
        Me.txtTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxGroup.Size = New System.Drawing.Size(89, 17)
        Me.txtTaxGroup.TabIndex = 19
        Me.txtTaxGroup.Text = "Tax to Apply:"
        '
        'cboPMTax
        '
        Me.cboPMTax.DefaultItemId = 0
        Me.cboPMTax.FirstItem = ""
        Me.cboPMTax.ItemId = 0
        Me.cboPMTax.ListIndex = -1
        Me.cboPMTax.Location = New System.Drawing.Point(160, 116)
        Me.cboPMTax.Name = "cboPMTax"
        Me.cboPMTax.PMLookupProductFamily = 9
        Me.cboPMTax.SingleItemId = 0
        Me.cboPMTax.Size = New System.Drawing.Size(240, 21)
        Me.cboPMTax.Sorted = True
        Me.cboPMTax.TabIndex = 3
        Me.cboPMTax.TableName = "tax_group"
        Me.cboPMTax.ToolTipText = ""
        Me.cboPMTax.WhereClause = ""
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(160, 20)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(241, 20)
        Me.txtDescription.TabIndex = 0
        '
        'uctNominalAccount
        '
        Me.uctNominalAccount.AccountId = 0
        Me.uctNominalAccount.AllowStoppedAccounts = False
        Me.uctNominalAccount.BackStyle = 0
        Me.uctNominalAccount.CompanyId = 0
        Me.uctNominalAccount.Default_Renamed = False
        Me.uctNominalAccount.Location = New System.Drawing.Point(160, 56)
        Me.uctNominalAccount.LookupCaption = "..."
        Me.uctNominalAccount.LookupHeight = 285
        Me.uctNominalAccount.LookupLeft = 2910
        Me.uctNominalAccount.LookupTextLeft = 0
        Me.uctNominalAccount.LookupTextWidth = 2910
        Me.uctNominalAccount.LookupWidth = 360
        Me.uctNominalAccount.Name = "uctNominalAccount"
        Me.uctNominalAccount.OnlyUpdatableAccounts = False
        Me.uctNominalAccount.SelLength = 0
        Me.uctNominalAccount.SelStart = 0
        Me.uctNominalAccount.SelText = ""
        Me.uctNominalAccount.ShowEditOnFindAccount = False
        Me.uctNominalAccount.Size = New System.Drawing.Size(218, 19)
        Me.uctNominalAccount.TabIndex = 20
        Me.uctNominalAccount.ToolTipText = ""
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(160, 84)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtAmount.TabIndex = 2
        '
        'cboCostCentre
        '
        Me.cboCostCentre.DefaultItemId = 0
        Me.cboCostCentre.FirstItem = ""
        Me.cboCostCentre.ItemId = 0
        Me.cboCostCentre.ListIndex = -1
        Me.cboCostCentre.Location = New System.Drawing.Point(160, 220)
        Me.cboCostCentre.Name = "cboCostCentre"
        Me.cboCostCentre.PMLookupProductFamily = 3
        Me.cboCostCentre.SingleItemId = 0
        Me.cboCostCentre.Size = New System.Drawing.Size(153, 21)
        Me.cboCostCentre.Sorted = True
        Me.cboCostCentre.TabIndex = 6
        Me.cboCostCentre.TableName = "CostCentre"
        Me.cboCostCentre.ToolTipText = ""
        Me.cboCostCentre.WhereClause = ""
        '
        'txtDeptAmount
        '
        Me.txtDeptAmount.AcceptsReturn = True
        Me.txtDeptAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtDeptAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeptAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeptAmount.Location = New System.Drawing.Point(160, 260)
        Me.txtDeptAmount.MaxLength = 0
        Me.txtDeptAmount.Name = "txtDeptAmount"
        Me.txtDeptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeptAmount.Size = New System.Drawing.Size(135, 20)
        Me.txtDeptAmount.TabIndex = 7
        '
        'cboValType
        '
        Me.cboValType.BackColor = System.Drawing.SystemColors.Window
        Me.cboValType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboValType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboValType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboValType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboValType.Location = New System.Drawing.Point(304, 260)
        Me.cboValType.Name = "cboValType"
        Me.cboValType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboValType.Size = New System.Drawing.Size(97, 21)
        Me.cboValType.TabIndex = 8
        '
        'txtVATAmount
        '
        Me.txtVATAmount.AcceptsReturn = True
        Me.txtVATAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtVATAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVATAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVATAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVATAmount.Location = New System.Drawing.Point(160, 148)
        Me.txtVATAmount.MaxLength = 0
        Me.txtVATAmount.Name = "txtVATAmount"
        Me.txtVATAmount.ReadOnly = True
        Me.txtVATAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVATAmount.Size = New System.Drawing.Size(137, 20)
        Me.txtVATAmount.TabIndex = 4
        '
        'txtTotalIncVat
        '
        Me.txtTotalIncVat.AcceptsReturn = True
        Me.txtTotalIncVat.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalIncVat.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalIncVat.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalIncVat.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalIncVat.Location = New System.Drawing.Point(160, 180)
        Me.txtTotalIncVat.MaxLength = 0
        Me.txtTotalIncVat.Name = "txtTotalIncVat"
        Me.txtTotalIncVat.ReadOnly = True
        Me.txtTotalIncVat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalIncVat.Size = New System.Drawing.Size(137, 20)
        Me.txtTotalIncVat.TabIndex = 5
        '
        'frmDetails
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(441, 364)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(593, 435)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetails"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Invoice Details"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class