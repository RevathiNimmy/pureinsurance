<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
    Public WithEvents uctInstalmentsControl As uctInstalmentsControl.uctInstalments
	Public WithEvents cmdOverride As System.Windows.Forms.Button
	Public WithEvents cmdRequote As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOverride = New System.Windows.Forms.Button()
        Me.cmdRequote = New System.Windows.Forms.Button()
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.uctInstalmentsControl = New uctInstalmentsControl.uctInstalments()
        Me.SuspendLayout()
        '
        'cmdOverride
        '
        Me.cmdOverride.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOverride.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOverride.Location = New System.Drawing.Point(88, 344)
        Me.cmdOverride.Name = "cmdOverride"
        Me.cmdOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOverride.Size = New System.Drawing.Size(73, 22)
        Me.cmdOverride.TabIndex = 5
        Me.cmdOverride.Text = "&Override"
        Me.cmdOverride.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOverride.UseVisualStyleBackColor = False
        '
        'cmdRequote
        '
        Me.cmdRequote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRequote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRequote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRequote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRequote.Location = New System.Drawing.Point(168, 344)
        Me.cmdRequote.Name = "cmdRequote"
        Me.cmdRequote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRequote.Size = New System.Drawing.Size(73, 22)
        Me.cmdRequote.TabIndex = 1
        Me.cmdRequote.Tag = "CAP;204"
        Me.cmdRequote.Text = "&ReQuote"
        Me.cmdRequote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRequote.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(248, 344)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 2
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Tag = "CAP;205"
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Location = New System.Drawing.Point(8, 344)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelect.TabIndex = 0
        Me.cmdSelect.TabStop = False
        Me.cmdSelect.Tag = "CAP;203"
        Me.cmdSelect.Text = "&Select"
        Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(646, 344)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Tag = "CAP;201"
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(566, 344)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Tag = "CAP;202"
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'uctInstalmentsControl
        '
        Me.uctInstalmentsControl.BaseCurrency = ""
        Me.uctInstalmentsControl.BaseCurrencyID = 0
        Me.uctInstalmentsControl.BaseISOCode = Nothing
        Me.uctInstalmentsControl.FeeDeposit = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalmentsControl.FeeExcluded = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalmentsControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctInstalmentsControl.GrossDue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalmentsControl.IsFinanceAmountNetPremium = False
        Me.uctInstalmentsControl.IsPlanSelected = False
        Me.uctInstalmentsControl.IsTrueMonthlypolicyandNextInstalmentRenewal = False
        Me.uctInstalmentsControl.Location = New System.Drawing.Point(8, -1)
        Me.uctInstalmentsControl.MTAType = 0
        Me.uctInstalmentsControl.Name = "uctInstalmentsControl"
        Me.uctInstalmentsControl.PremiumFinanceCnt = 0
        Me.uctInstalmentsControl.PremiumFinanceTransactions = Nothing
        Me.uctInstalmentsControl.PremiumFinanceVersion = 0
        Me.uctInstalmentsControl.Size = New System.Drawing.Size(728, 339)
        Me.uctInstalmentsControl.TabIndex = 6
        Me.uctInstalmentsControl.Task = 1
        Me.uctInstalmentsControl.TaxDeposit = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalmentsControl.TaxExcluded = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalmentsControl.TransactionType = ""
        Me.uctInstalmentsControl.TransCurrencyID = 0
        Me.uctInstalmentsControl.TransISOCode = Nothing
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(740, 385)
        Me.Controls.Add(Me.uctInstalmentsControl)
        Me.Controls.Add(Me.cmdOverride)
        Me.Controls.Add(Me.cmdRequote)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(158, 147)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Premium Finance Quote"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class