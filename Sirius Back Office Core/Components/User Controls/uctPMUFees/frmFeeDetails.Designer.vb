<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFeeDetail
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
	Public WithEvents OptAmount As System.Windows.Forms.RadioButton
	Public WithEvents OptPercentage As System.Windows.Forms.RadioButton
	Public WithEvents txtRate As System.Windows.Forms.TextBox
	Public WithEvents lblCurrencyValue As System.Windows.Forms.Label
	Public WithEvents lblRate As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraFeeAmount As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblTaxGroupValue As System.Windows.Forms.Label
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents fraTax As System.Windows.Forms.GroupBox
	Private WithEvents _TabStrip1_Tab1 As System.Windows.Forms.TabPage
	Public WithEvents TabStrip1_Tabs As System.Windows.Forms.TabControl.TabPageCollection
	Public WithEvents TabStrip1 As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraFeeAmount = New System.Windows.Forms.GroupBox()
        Me.txtProRateAmount = New System.Windows.Forms.TextBox()
        Me.lblProrataAmount = New System.Windows.Forms.Label()
        Me.OptAmount = New System.Windows.Forms.RadioButton()
        Me.OptPercentage = New System.Windows.Forms.RadioButton()
        Me.txtRate = New System.Windows.Forms.TextBox()
        Me.lblCurrencyValue = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.fraTax = New System.Windows.Forms.GroupBox()
        Me.lblTaxGroupValue = New System.Windows.Forms.Label()
        Me.lblTaxGroup = New System.Windows.Forms.Label()
        Me.TabStrip1 = New System.Windows.Forms.TabControl()
        Me._TabStrip1_Tab1 = New System.Windows.Forms.TabPage()
        Me.fraFeeAmount.SuspendLayout()
        Me.fraTax.SuspendLayout()
        Me.TabStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraFeeAmount
        '
        Me.fraFeeAmount.BackColor = System.Drawing.SystemColors.Control
        Me.fraFeeAmount.Controls.Add(Me.txtProRateAmount)
        Me.fraFeeAmount.Controls.Add(Me.lblProrataAmount)
        Me.fraFeeAmount.Controls.Add(Me.OptAmount)
        Me.fraFeeAmount.Controls.Add(Me.OptPercentage)
        Me.fraFeeAmount.Controls.Add(Me.txtRate)
        Me.fraFeeAmount.Controls.Add(Me.lblCurrencyValue)
        Me.fraFeeAmount.Controls.Add(Me.lblRate)
        Me.fraFeeAmount.Controls.Add(Me.lblCurrency)
        Me.fraFeeAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFeeAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFeeAmount.Location = New System.Drawing.Point(16, 32)
        Me.fraFeeAmount.Name = "fraFeeAmount"
        Me.fraFeeAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFeeAmount.Size = New System.Drawing.Size(457, 81)
        Me.fraFeeAmount.TabIndex = 5
        Me.fraFeeAmount.TabStop = False
        Me.fraFeeAmount.Text = "Fee amount"
        '
        'txtProRateAmount
        '
        Me.txtProRateAmount.Enabled = False
        Me.txtProRateAmount.Location = New System.Drawing.Point(314, 25)
        Me.txtProRateAmount.Name = "txtProRateAmount"
        Me.txtProRateAmount.Size = New System.Drawing.Size(100, 20)
        Me.txtProRateAmount.TabIndex = 14
        '
        'lblProrataAmount
        '
        Me.lblProrataAmount.AutoSize = True
        Me.lblProrataAmount.Location = New System.Drawing.Point(217, 28)
        Me.lblProrataAmount.Name = "lblProrataAmount"
        Me.lblProrataAmount.Size = New System.Drawing.Size(91, 13)
        Me.lblProrataAmount.TabIndex = 13
        Me.lblProrataAmount.Text = "Pro-Rata Amount:"
        '
        'OptAmount
        '
        Me.OptAmount.BackColor = System.Drawing.SystemColors.Control
        Me.OptAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptAmount.Location = New System.Drawing.Point(136, 24)
        Me.OptAmount.Name = "OptAmount"
        Me.OptAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptAmount.Size = New System.Drawing.Size(65, 21)
        Me.OptAmount.TabIndex = 8
        Me.OptAmount.TabStop = True
        Me.OptAmount.Text = "Value"
        Me.OptAmount.UseVisualStyleBackColor = False
        '
        'OptPercentage
        '
        Me.OptPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.OptPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptPercentage.Location = New System.Drawing.Point(24, 24)
        Me.OptPercentage.Name = "OptPercentage"
        Me.OptPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptPercentage.Size = New System.Drawing.Size(105, 21)
        Me.OptPercentage.TabIndex = 7
        Me.OptPercentage.TabStop = True
        Me.OptPercentage.Text = "Percentage"
        Me.OptPercentage.UseVisualStyleBackColor = False
        '
        'txtRate
        '
        Me.txtRate.AcceptsReturn = True
        Me.txtRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate.Location = New System.Drawing.Point(64, 48)
        Me.txtRate.MaxLength = 9
        Me.txtRate.Name = "txtRate"
        Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate.Size = New System.Drawing.Size(81, 20)
        Me.txtRate.TabIndex = 6
        '
        'lblCurrencyValue
        '
        Me.lblCurrencyValue.AutoSize = True
        Me.lblCurrencyValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyValue.Location = New System.Drawing.Point(240, 52)
        Me.lblCurrencyValue.Name = "lblCurrencyValue"
        Me.lblCurrencyValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyValue.Size = New System.Drawing.Size(79, 13)
        Me.lblCurrencyValue.TabIndex = 12
        Me.lblCurrencyValue.Text = "Currency Value"
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(24, 52)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(40, 13)
        Me.lblRate.TabIndex = 10
        Me.lblRate.Text = "Rate:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(168, 52)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(70, 13)
        Me.lblCurrency.TabIndex = 9
        Me.lblCurrency.Text = "Currency:"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(320, 180)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(404, 180)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'fraTax
        '
        Me.fraTax.BackColor = System.Drawing.SystemColors.Control
        Me.fraTax.Controls.Add(Me.lblTaxGroupValue)
        Me.fraTax.Controls.Add(Me.lblTaxGroup)
        Me.fraTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTax.Location = New System.Drawing.Point(16, 112)
        Me.fraTax.Name = "fraTax"
        Me.fraTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTax.Size = New System.Drawing.Size(457, 57)
        Me.fraTax.TabIndex = 1
        Me.fraTax.TabStop = False
        Me.fraTax.Text = "Tax"
        '
        'lblTaxGroupValue
        '
        Me.lblTaxGroupValue.AutoSize = True
        Me.lblTaxGroupValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroupValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroupValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroupValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroupValue.Location = New System.Drawing.Point(104, 24)
        Me.lblTaxGroupValue.Name = "lblTaxGroupValue"
        Me.lblTaxGroupValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroupValue.Size = New System.Drawing.Size(87, 13)
        Me.lblTaxGroupValue.TabIndex = 11
        Me.lblTaxGroupValue.Text = "Tax Group Value"
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(24, 24)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(81, 17)
        Me.lblTaxGroup.TabIndex = 2
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'TabStrip1
        '
        Me.TabStrip1.Controls.Add(Me._TabStrip1_Tab1)
        Me.TabStrip1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabStrip1.Location = New System.Drawing.Point(8, 8)
        Me.TabStrip1.Name = "TabStrip1"
        Me.TabStrip1.SelectedIndex = 0
        Me.TabStrip1.Size = New System.Drawing.Size(473, 167)
        Me.TabStrip1.TabIndex = 0
        '
        '_TabStrip1_Tab1
        '
        Me._TabStrip1_Tab1.Location = New System.Drawing.Point(4, 23)
        Me._TabStrip1_Tab1.Name = "_TabStrip1_Tab1"
        Me._TabStrip1_Tab1.Size = New System.Drawing.Size(465, 140)
        Me._TabStrip1_Tab1.TabIndex = 0
        Me._TabStrip1_Tab1.Text = "1 - Fee Edit"
        '
        'frmFeeDetail
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(482, 205)
        Me.Controls.Add(Me.fraFeeAmount)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraTax)
        Me.Controls.Add(Me.TabStrip1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "frmFeeDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Fees"
        Me.fraFeeAmount.ResumeLayout(False)
        Me.fraFeeAmount.PerformLayout()
        Me.fraTax.ResumeLayout(False)
        Me.fraTax.PerformLayout()
        Me.TabStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtProRateAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblProrataAmount As System.Windows.Forms.Label
#End Region 
End Class