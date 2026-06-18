<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyTax
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		UserControl_InitProperties()
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
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraPartyTax = New System.Windows.Forms.Panel
        Me.txtTaxPercentage = New System.Windows.Forms.TextBox
        Me.chkIsDomiciledForTax = New System.Windows.Forms.CheckBox
        Me.chkTaxExempt = New System.Windows.Forms.CheckBox
        Me.txtTaxNumber = New System.Windows.Forms.TextBox
        Me.lblTaxPercentage = New System.Windows.Forms.Label
        Me.lblTaxExempt = New System.Windows.Forms.Label
        Me.lblIsDomiciledForTax = New System.Windows.Forms.Label
        Me.lblTaxNumber = New System.Windows.Forms.Label
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraPartyTax.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraPartyTax
        '
        Me.fraPartyTax.Controls.Add(Me.txtTaxPercentage)
        Me.fraPartyTax.Controls.Add(Me.chkIsDomiciledForTax)
        Me.fraPartyTax.Controls.Add(Me.chkTaxExempt)
        Me.fraPartyTax.Controls.Add(Me.txtTaxNumber)
        Me.fraPartyTax.Controls.Add(Me.lblTaxPercentage)
        Me.fraPartyTax.Controls.Add(Me.lblTaxExempt)
        Me.fraPartyTax.Controls.Add(Me.lblIsDomiciledForTax)
        Me.fraPartyTax.Controls.Add(Me.lblTaxNumber)
        Me.fraPartyTax.Location = New System.Drawing.Point(3, 3)
        Me.fraPartyTax.Name = "fraPartyTax"
        Me.fraPartyTax.Size = New System.Drawing.Size(542, 278)
        Me.fraPartyTax.TabIndex = 0
        '
        'txtTaxPercentage
        '
        Me.txtTaxPercentage.AcceptsReturn = True
        Me.txtTaxPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxPercentage.Location = New System.Drawing.Point(161, 91)
        Me.txtTaxPercentage.MaxLength = 0
        Me.txtTaxPercentage.Name = "txtTaxPercentage"
        Me.txtTaxPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxPercentage.Size = New System.Drawing.Size(49, 20)
        Me.txtTaxPercentage.TabIndex = 12
        '
        'chkIsDomiciledForTax
        '
        Me.chkIsDomiciledForTax.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDomiciledForTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDomiciledForTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDomiciledForTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDomiciledForTax.Location = New System.Drawing.Point(161, 43)
        Me.chkIsDomiciledForTax.Name = "chkIsDomiciledForTax"
        Me.chkIsDomiciledForTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDomiciledForTax.Size = New System.Drawing.Size(17, 21)
        Me.chkIsDomiciledForTax.TabIndex = 11
        Me.chkIsDomiciledForTax.Text = "Check1"
        Me.chkIsDomiciledForTax.UseVisualStyleBackColor = False
        '
        'chkTaxExempt
        '
        Me.chkTaxExempt.BackColor = System.Drawing.SystemColors.Control
        Me.chkTaxExempt.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTaxExempt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTaxExempt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTaxExempt.Location = New System.Drawing.Point(161, 67)
        Me.chkTaxExempt.Name = "chkTaxExempt"
        Me.chkTaxExempt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTaxExempt.Size = New System.Drawing.Size(17, 21)
        Me.chkTaxExempt.TabIndex = 10
        Me.chkTaxExempt.Text = "Check2"
        Me.chkTaxExempt.UseVisualStyleBackColor = False
        '
        'txtTaxNumber
        '
        Me.txtTaxNumber.AcceptsReturn = True
        Me.txtTaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxNumber.Location = New System.Drawing.Point(161, 11)
        Me.txtTaxNumber.MaxLength = 0
        Me.txtTaxNumber.Name = "txtTaxNumber"
        Me.txtTaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxNumber.Size = New System.Drawing.Size(289, 20)
        Me.txtTaxNumber.TabIndex = 9
        '
        'lblTaxPercentage
        '
        Me.lblTaxPercentage.AutoSize = True
        Me.lblTaxPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxPercentage.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxPercentage.Location = New System.Drawing.Point(54, 91)
        Me.lblTaxPercentage.Name = "lblTaxPercentage"
        Me.lblTaxPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxPercentage.Size = New System.Drawing.Size(101, 13)
        Me.lblTaxPercentage.TabIndex = 16
        Me.lblTaxPercentage.Text = "Tax Percentage:"
        '
        'lblTaxExempt
        '
        Me.lblTaxExempt.AutoSize = True
        Me.lblTaxExempt.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxExempt.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxExempt.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxExempt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxExempt.Location = New System.Drawing.Point(75, 67)
        Me.lblTaxExempt.Name = "lblTaxExempt"
        Me.lblTaxExempt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxExempt.Size = New System.Drawing.Size(80, 13)
        Me.lblTaxExempt.TabIndex = 15
        Me.lblTaxExempt.Text = "Tax Exempt:"
        '
        'lblIsDomiciledForTax
        '
        Me.lblIsDomiciledForTax.AutoSize = True
        Me.lblIsDomiciledForTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsDomiciledForTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsDomiciledForTax.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsDomiciledForTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsDomiciledForTax.Location = New System.Drawing.Point(25, 43)
        Me.lblIsDomiciledForTax.Name = "lblIsDomiciledForTax"
        Me.lblIsDomiciledForTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsDomiciledForTax.Size = New System.Drawing.Size(130, 13)
        Me.lblIsDomiciledForTax.TabIndex = 14
        Me.lblIsDomiciledForTax.Text = "Is Domiciled For Tax:"
        '
        'lblTaxNumber
        '
        Me.lblTaxNumber.AutoSize = True
        Me.lblTaxNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxNumber.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxNumber.Location = New System.Drawing.Point(73, 19)
        Me.lblTaxNumber.Name = "lblTaxNumber"
        Me.lblTaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxNumber.Size = New System.Drawing.Size(82, 13)
        Me.lblTaxNumber.TabIndex = 13
        Me.lblTaxNumber.Text = "Tax Number:"
        '
        'uctPartyTax
        '
        Me.Controls.Add(Me.fraPartyTax)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPartyTax"
        Me.Size = New System.Drawing.Size(548, 284)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraPartyTax.ResumeLayout(False)
        Me.fraPartyTax.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraPartyTax As System.Windows.Forms.Panel
    Friend WithEvents txtTaxPercentage As System.Windows.Forms.TextBox
    Friend WithEvents chkIsDomiciledForTax As System.Windows.Forms.CheckBox
    Friend WithEvents chkTaxExempt As System.Windows.Forms.CheckBox
    Friend WithEvents txtTaxNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblTaxPercentage As System.Windows.Forms.Label
    Friend WithEvents lblTaxExempt As System.Windows.Forms.Label
    Friend WithEvents lblIsDomiciledForTax As System.Windows.Forms.Label
    Friend WithEvents lblTaxNumber As System.Windows.Forms.Label
#End Region 
End Class