<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUInterface
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblIsTaxable As System.Windows.Forms.Label
	Public WithEvents lblTransType As System.Windows.Forms.Label
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblPercentage As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents txtPercentage As System.Windows.Forms.TextBox
	Public WithEvents cboTransType As System.Windows.Forms.ComboBox
	Public WithEvents chkIsTaxable As System.Windows.Forms.CheckBox
	Public WithEvents cboProduct As System.Windows.Forms.ComboBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblIsTaxable = New System.Windows.Forms.Label
        Me.lblTransType = New System.Windows.Forms.Label
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblPercentage = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me.cboTransType = New System.Windows.Forms.ComboBox
        Me.chkIsTaxable = New System.Windows.Forms.CheckBox
        Me.cboProduct = New System.Windows.Forms.ComboBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(176, 256)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(260, 256)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(340, 256)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 10
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(80, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(409, 243)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblIsTaxable)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTransType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPercentage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPercentage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTransType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsTaxable)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAmount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(401, 217)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Fee Charges"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(24, 155)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(105, 17)
        Me.lblCurrency.TabIndex = 17
        Me.lblCurrency.Text = "Currency:"
        '
        'lblIsTaxable
        '
        Me.lblIsTaxable.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsTaxable.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsTaxable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsTaxable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsTaxable.Location = New System.Drawing.Point(24, 182)
        Me.lblIsTaxable.Name = "lblIsTaxable"
        Me.lblIsTaxable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsTaxable.Size = New System.Drawing.Size(93, 19)
        Me.lblIsTaxable.TabIndex = 11
        Me.lblIsTaxable.Text = "Is Taxable?:"
        '
        'lblTransType
        '
        Me.lblTransType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransType.Location = New System.Drawing.Point(24, 102)
        Me.lblTransType.Name = "lblTransType"
        Me.lblTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransType.Size = New System.Drawing.Size(117, 19)
        Me.lblTransType.TabIndex = 12
        Me.lblTransType.Text = "Transaction Type:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(24, 22)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(93, 19)
        Me.lblProduct.TabIndex = 13
        Me.lblProduct.Text = "Product:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(24, 129)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(105, 19)
        Me.lblEffectiveDate.TabIndex = 14
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblPercentage
        '
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(24, 49)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(101, 19)
        Me.lblPercentage.TabIndex = 15
        Me.lblPercentage.Text = "Percentage:"
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(24, 75)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(93, 19)
        Me.lblAmount.TabIndex = 16
        Me.lblAmount.Text = "Amount:"
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(150, 47)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(240, 20)
        Me.txtPercentage.TabIndex = 2
        '
        'cboTransType
        '
        Me.cboTransType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTransType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTransType.Location = New System.Drawing.Point(150, 100)
        Me.cboTransType.Name = "cboTransType"
        Me.cboTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransType.Size = New System.Drawing.Size(240, 21)
        Me.cboTransType.TabIndex = 4
        '
        'chkIsTaxable
        '
        Me.chkIsTaxable.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsTaxable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsTaxable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsTaxable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsTaxable.Location = New System.Drawing.Point(150, 180)
        Me.chkIsTaxable.Name = "chkIsTaxable"
        Me.chkIsTaxable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsTaxable.Size = New System.Drawing.Size(25, 21)
        Me.chkIsTaxable.TabIndex = 7
        Me.chkIsTaxable.UseVisualStyleBackColor = False
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(150, 20)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(240, 21)
        Me.cboProduct.TabIndex = 1
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(150, 127)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(112, 20)
        Me.txtEffectiveDate.TabIndex = 5
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(150, 73)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(240, 20)
        Me.txtAmount.TabIndex = 3
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(150, 153)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 6
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'frmUInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(418, 283)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(332, 269)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Charges"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class