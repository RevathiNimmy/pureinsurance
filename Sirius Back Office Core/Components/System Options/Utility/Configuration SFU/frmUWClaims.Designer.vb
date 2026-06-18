<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUWClaims
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents chkOption5071 As System.Windows.Forms.CheckBox
	Public WithEvents chkOption5072 As System.Windows.Forms.CheckBox
	Public WithEvents chkRestrictNonZeroClosure As System.Windows.Forms.CheckBox
	Public WithEvents chkTotalIncurredLessReceipts As System.Windows.Forms.CheckBox
	Public WithEvents chkOption5067 As System.Windows.Forms.CheckBox
	Public WithEvents chkClaimPaymentTaxGroupIsMandatory5063 As System.Windows.Forms.CheckBox
	Public WithEvents cboOption5035 As System.Windows.Forms.ComboBox
	Public WithEvents cmdClearCloseCaseDocument As System.Windows.Forms.Button
	Public WithEvents cmdCloseCaseDocument As System.Windows.Forms.Button
	Public WithEvents txtCloseCaseDocument As System.Windows.Forms.TextBox
	Public WithEvents txtOpenCaseDocument As System.Windows.Forms.TextBox
	Public WithEvents txtEditCaseDocument As System.Windows.Forms.TextBox
	Public WithEvents cmdOpenCaseDocument As System.Windows.Forms.Button
	Public WithEvents cmdEditCaseDocument As System.Windows.Forms.Button
	Public WithEvents cmdClearOpenCaseDocument As System.Windows.Forms.Button
	Public WithEvents cmdCleaEditCaseDocument As System.Windows.Forms.Button
    Public WithEvents cboOption5030 As System.Windows.Forms.ComboBox
    Public WithEvents lblCaseScreen5035 As System.Windows.Forms.Label
    Public WithEvents lblOption5031 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents chkOptionEnableLossDateduringClaim As System.Windows.Forms.CheckBox
    Public WithEvents chkAutomaticRecieptGenerationForSalvageTP As System.Windows.Forms.CheckBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkOption5071 = New System.Windows.Forms.CheckBox()
        Me.chkOption5072 = New System.Windows.Forms.CheckBox()
        Me.chkRestrictNonZeroClosure = New System.Windows.Forms.CheckBox()
        Me.chkTotalIncurredLessReceipts = New System.Windows.Forms.CheckBox()
        Me.chkOption5067 = New System.Windows.Forms.CheckBox()
        Me.chkClaimPaymentTaxGroupIsMandatory5063 = New System.Windows.Forms.CheckBox()
        Me.cboOption5035 = New System.Windows.Forms.ComboBox()
        Me.cmdClearCloseCaseDocument = New System.Windows.Forms.Button()
        Me.cmdCloseCaseDocument = New System.Windows.Forms.Button()
        Me.txtCloseCaseDocument = New System.Windows.Forms.TextBox()
        Me.txtOpenCaseDocument = New System.Windows.Forms.TextBox()
        Me.txtEditCaseDocument = New System.Windows.Forms.TextBox()
        Me.cmdOpenCaseDocument = New System.Windows.Forms.Button()
        Me.cmdEditCaseDocument = New System.Windows.Forms.Button()
        Me.cmdClearOpenCaseDocument = New System.Windows.Forms.Button()
        Me.cmdCleaEditCaseDocument = New System.Windows.Forms.Button()
        Me.cboOption5030 = New System.Windows.Forms.ComboBox()
        Me.lblCaseScreen5035 = New System.Windows.Forms.Label()
        Me.lblOption5031 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkOptionEnableLossDateduringClaim = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtClaimPayExGratiaAccount = New System.Windows.Forms.TextBox()
        Me.chkEnhanceCaseSearching = New System.Windows.Forms.CheckBox()
        Me.chkAutomaticRecieptGenerationForSalvageTP = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkClaimsReserveGross = New System.Windows.Forms.CheckBox()
        Me.cboOption5240 = New System.Windows.Forms.ComboBox()
        Me.cboOption5031 = New System.Windows.Forms.ComboBox()
        Me.chkSkipCashlistProcessForNegativeClaimPayment = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'chkOption5071
        '
        Me.chkOption5071.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5071.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5071.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5071.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5071.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5071.Location = New System.Drawing.Point(4, 323)
        Me.chkOption5071.Name = "chkOption5071"
        Me.chkOption5071.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5071.Size = New System.Drawing.Size(385, 26)
        Me.chkOption5071.TabIndex = 19
        Me.chkOption5071.Tag = "5071"
        Me.chkOption5071.Text = "ATS Settlement Check box:"
        Me.chkOption5071.UseVisualStyleBackColor = False
        '
        'chkOption5072
        '
        Me.chkOption5072.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5072.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5072.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5072.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5072.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5072.Location = New System.Drawing.Point(4, 346)
        Me.chkOption5072.Name = "chkOption5072"
        Me.chkOption5072.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5072.Size = New System.Drawing.Size(385, 25)
        Me.chkOption5072.TabIndex = 18
        Me.chkOption5072.Tag = "5072"
        Me.chkOption5072.Text = "Payment ATS safe harbour:"
        Me.chkOption5072.UseVisualStyleBackColor = False
        '
        'chkRestrictNonZeroClosure
        '
        Me.chkRestrictNonZeroClosure.BackColor = System.Drawing.SystemColors.Control
        Me.chkRestrictNonZeroClosure.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRestrictNonZeroClosure.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRestrictNonZeroClosure.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRestrictNonZeroClosure.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRestrictNonZeroClosure.Location = New System.Drawing.Point(4, 300)
        Me.chkRestrictNonZeroClosure.Name = "chkRestrictNonZeroClosure"
        Me.chkRestrictNonZeroClosure.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRestrictNonZeroClosure.Size = New System.Drawing.Size(384, 30)
        Me.chkRestrictNonZeroClosure.TabIndex = 17
        Me.chkRestrictNonZeroClosure.Tag = "5073"
        Me.chkRestrictNonZeroClosure.Text = "Only Allow Claim Closure when Reserves are Zero:"
        Me.chkRestrictNonZeroClosure.UseVisualStyleBackColor = False
        '
        'chkTotalIncurredLessReceipts
        '
        Me.chkTotalIncurredLessReceipts.BackColor = System.Drawing.SystemColors.Control
        Me.chkTotalIncurredLessReceipts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTotalIncurredLessReceipts.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTotalIncurredLessReceipts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTotalIncurredLessReceipts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTotalIncurredLessReceipts.Location = New System.Drawing.Point(4, 277)
        Me.chkTotalIncurredLessReceipts.Name = "chkTotalIncurredLessReceipts"
        Me.chkTotalIncurredLessReceipts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTotalIncurredLessReceipts.Size = New System.Drawing.Size(384, 29)
        Me.chkTotalIncurredLessReceipts.TabIndex = 30
        Me.chkTotalIncurredLessReceipts.Tag = "5263"
        Me.chkTotalIncurredLessReceipts.Text = "Total Incurred as Incurred less Receipts:"
        Me.chkTotalIncurredLessReceipts.UseVisualStyleBackColor = False
        '
        'chkOption5067
        '
        Me.chkOption5067.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5067.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5067.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5067.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5067.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5067.Location = New System.Drawing.Point(4, 254)
        Me.chkOption5067.Name = "chkOption5067"
        Me.chkOption5067.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5067.Size = New System.Drawing.Size(384, 30)
        Me.chkOption5067.TabIndex = 16
        Me.chkOption5067.Tag = "5067"
        Me.chkOption5067.Text = "Salvage And TP Recovery reserves exclude tax :"
        Me.chkOption5067.UseVisualStyleBackColor = False
        '
        'chkClaimPaymentTaxGroupIsMandatory5063
        '
        Me.chkClaimPaymentTaxGroupIsMandatory5063.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimPaymentTaxGroupIsMandatory5063.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimPaymentTaxGroupIsMandatory5063.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Location = New System.Drawing.Point(4, 231)
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Name = "chkClaimPaymentTaxGroupIsMandatory5063"
        Me.chkClaimPaymentTaxGroupIsMandatory5063.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Size = New System.Drawing.Size(384, 23)
        Me.chkClaimPaymentTaxGroupIsMandatory5063.TabIndex = 15
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Tag = "5063"
        Me.chkClaimPaymentTaxGroupIsMandatory5063.Text = "Claim Payment Tax Group Is Mandatory:"
        Me.chkClaimPaymentTaxGroupIsMandatory5063.UseVisualStyleBackColor = False
        '
        'cboOption5035
        '
        Me.cboOption5035.AccessibleDescription = "Case Screen:"
        Me.cboOption5035.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5035.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5035.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5035.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5035.Location = New System.Drawing.Point(286, 64)
        Me.cboOption5035.Name = "cboOption5035"
        Me.cboOption5035.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5035.Size = New System.Drawing.Size(308, 25)
        Me.cboOption5035.TabIndex = 1
        Me.cboOption5035.Tag = "5035"
        Me.cboOption5035.Text = "cboOption5035"
        '
        'cmdClearCloseCaseDocument
        '
        Me.cmdClearCloseCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearCloseCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearCloseCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearCloseCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearCloseCaseDocument.Location = New System.Drawing.Point(482, 187)
        Me.cmdClearCloseCaseDocument.Name = "cmdClearCloseCaseDocument"
        Me.cmdClearCloseCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearCloseCaseDocument.Size = New System.Drawing.Size(69, 23)
        Me.cmdClearCloseCaseDocument.TabIndex = 9
        Me.cmdClearCloseCaseDocument.Tag = "5034,ClearDocument"
        Me.cmdClearCloseCaseDocument.Text = "Clear"
        Me.cmdClearCloseCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearCloseCaseDocument.UseVisualStyleBackColor = False
        '
        'cmdCloseCaseDocument
        '
        Me.cmdCloseCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCloseCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseCaseDocument.Location = New System.Drawing.Point(77, 185)
        Me.cmdCloseCaseDocument.Name = "cmdCloseCaseDocument"
        Me.cmdCloseCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCloseCaseDocument.Size = New System.Drawing.Size(203, 23)
        Me.cmdCloseCaseDocument.TabIndex = 8
        Me.cmdCloseCaseDocument.Tag = "5034,GetDocument"
        Me.cmdCloseCaseDocument.Text = "Close Case Document"
        Me.cmdCloseCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCloseCaseDocument.UseVisualStyleBackColor = False
        '
        'txtCloseCaseDocument
        '
        Me.txtCloseCaseDocument.AcceptsReturn = True
        Me.txtCloseCaseDocument.AccessibleDescription = "Close Case Document"
        Me.txtCloseCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.txtCloseCaseDocument.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCloseCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCloseCaseDocument.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCloseCaseDocument.Location = New System.Drawing.Point(288, 187)
        Me.txtCloseCaseDocument.MaxLength = 0
        Me.txtCloseCaseDocument.Name = "txtCloseCaseDocument"
        Me.txtCloseCaseDocument.ReadOnly = True
        Me.txtCloseCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCloseCaseDocument.Size = New System.Drawing.Size(184, 24)
        Me.txtCloseCaseDocument.TabIndex = 13
        Me.txtCloseCaseDocument.Tag = "5034,ShowTemplateCode,D"
        Me.txtCloseCaseDocument.Text = "(none)"
        '
        'txtOpenCaseDocument
        '
        Me.txtOpenCaseDocument.AcceptsReturn = True
        Me.txtOpenCaseDocument.AccessibleDescription = "Open Case Document"
        Me.txtOpenCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.txtOpenCaseDocument.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOpenCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOpenCaseDocument.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOpenCaseDocument.Location = New System.Drawing.Point(288, 129)
        Me.txtOpenCaseDocument.MaxLength = 0
        Me.txtOpenCaseDocument.Name = "txtOpenCaseDocument"
        Me.txtOpenCaseDocument.ReadOnly = True
        Me.txtOpenCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOpenCaseDocument.Size = New System.Drawing.Size(184, 24)
        Me.txtOpenCaseDocument.TabIndex = 4
        Me.txtOpenCaseDocument.Tag = "5032,ShowTemplateCode,D"
        Me.txtOpenCaseDocument.Text = "(none)"
        '
        'txtEditCaseDocument
        '
        Me.txtEditCaseDocument.AcceptsReturn = True
        Me.txtEditCaseDocument.AccessibleDescription = "Edit Case Document"
        Me.txtEditCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.txtEditCaseDocument.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEditCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEditCaseDocument.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEditCaseDocument.Location = New System.Drawing.Point(288, 158)
        Me.txtEditCaseDocument.MaxLength = 0
        Me.txtEditCaseDocument.Name = "txtEditCaseDocument"
        Me.txtEditCaseDocument.ReadOnly = True
        Me.txtEditCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEditCaseDocument.Size = New System.Drawing.Size(184, 24)
        Me.txtEditCaseDocument.TabIndex = 12
        Me.txtEditCaseDocument.Tag = "5033,ShowTemplateCode,D"
        Me.txtEditCaseDocument.Text = "(none)"
        '
        'cmdOpenCaseDocument
        '
        Me.cmdOpenCaseDocument.AccessibleDescription = ""
        Me.cmdOpenCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOpenCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOpenCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOpenCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOpenCaseDocument.Location = New System.Drawing.Point(77, 128)
        Me.cmdOpenCaseDocument.Name = "cmdOpenCaseDocument"
        Me.cmdOpenCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOpenCaseDocument.Size = New System.Drawing.Size(203, 23)
        Me.cmdOpenCaseDocument.TabIndex = 3
        Me.cmdOpenCaseDocument.Tag = "5032,GetDocument"
        Me.cmdOpenCaseDocument.Text = "Open Case Document"
        Me.cmdOpenCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOpenCaseDocument.UseVisualStyleBackColor = False
        '
        'cmdEditCaseDocument
        '
        Me.cmdEditCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCaseDocument.Location = New System.Drawing.Point(77, 158)
        Me.cmdEditCaseDocument.Name = "cmdEditCaseDocument"
        Me.cmdEditCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCaseDocument.Size = New System.Drawing.Size(203, 23)
        Me.cmdEditCaseDocument.TabIndex = 6
        Me.cmdEditCaseDocument.Tag = "5033,GetDocument"
        Me.cmdEditCaseDocument.Text = "Edit Case Document"
        Me.cmdEditCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCaseDocument.UseVisualStyleBackColor = False
        '
        'cmdClearOpenCaseDocument
        '
        Me.cmdClearOpenCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearOpenCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearOpenCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearOpenCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearOpenCaseDocument.Location = New System.Drawing.Point(482, 129)
        Me.cmdClearOpenCaseDocument.Name = "cmdClearOpenCaseDocument"
        Me.cmdClearOpenCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearOpenCaseDocument.Size = New System.Drawing.Size(69, 24)
        Me.cmdClearOpenCaseDocument.TabIndex = 5
        Me.cmdClearOpenCaseDocument.Tag = "5032,ClearDocument"
        Me.cmdClearOpenCaseDocument.Text = "Clear"
        Me.cmdClearOpenCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearOpenCaseDocument.UseVisualStyleBackColor = False
        '
        'cmdCleaEditCaseDocument
        '
        Me.cmdCleaEditCaseDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCleaEditCaseDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCleaEditCaseDocument.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCleaEditCaseDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCleaEditCaseDocument.Location = New System.Drawing.Point(482, 159)
        Me.cmdCleaEditCaseDocument.Name = "cmdCleaEditCaseDocument"
        Me.cmdCleaEditCaseDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCleaEditCaseDocument.Size = New System.Drawing.Size(69, 23)
        Me.cmdCleaEditCaseDocument.TabIndex = 7
        Me.cmdCleaEditCaseDocument.Tag = "5033,ClearDocument"
        Me.cmdCleaEditCaseDocument.Text = "Clear"
        Me.cmdCleaEditCaseDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCleaEditCaseDocument.UseVisualStyleBackColor = False
        '
        'cboOption5030
        '
        Me.cboOption5030.AccessibleDescription = "Claim Documents chosen by:"
        Me.cboOption5030.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5030.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5030.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5030.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5030.Location = New System.Drawing.Point(286, 34)
        Me.cboOption5030.Name = "cboOption5030"
        Me.cboOption5030.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5030.Size = New System.Drawing.Size(308, 25)
        Me.cboOption5030.TabIndex = 0
        Me.cboOption5030.Tag = "5030"
        Me.cboOption5030.Text = "cboOption5030"
        '
        'lblCaseScreen5035
        '
        Me.lblCaseScreen5035.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaseScreen5035.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaseScreen5035.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseScreen5035.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaseScreen5035.Location = New System.Drawing.Point(4, 64)
        Me.lblCaseScreen5035.Name = "lblCaseScreen5035"
        Me.lblCaseScreen5035.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaseScreen5035.Size = New System.Drawing.Size(270, 21)
        Me.lblCaseScreen5035.TabIndex = 14
        Me.lblCaseScreen5035.Text = "Case Screen:"
        '
        'lblOption5031
        '
        Me.lblOption5031.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption5031.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption5031.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption5031.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption5031.Location = New System.Drawing.Point(4, 94)
        Me.lblOption5031.Name = "lblOption5031"
        Me.lblOption5031.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption5031.Size = New System.Drawing.Size(270, 20)
        Me.lblOption5031.TabIndex = 11
        Me.lblOption5031.Text = "Case Numbering Scheme:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(4, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(274, 21)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Claim Documents chosen by"
        '
        'chkOptionEnableLossDateduringClaim
        '
        Me.chkOptionEnableLossDateduringClaim.BackColor = System.Drawing.SystemColors.Control
        Me.chkOptionEnableLossDateduringClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOptionEnableLossDateduringClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOptionEnableLossDateduringClaim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOptionEnableLossDateduringClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOptionEnableLossDateduringClaim.Location = New System.Drawing.Point(4, 369)
        Me.chkOptionEnableLossDateduringClaim.Name = "chkOptionEnableLossDateduringClaim"
        Me.chkOptionEnableLossDateduringClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOptionEnableLossDateduringClaim.Size = New System.Drawing.Size(385, 26)
        Me.chkOptionEnableLossDateduringClaim.TabIndex = 19
        Me.chkOptionEnableLossDateduringClaim.Tag = "5176"
        Me.chkOptionEnableLossDateduringClaim.Text = "Disable loss dates for Maintain Claims task"
        Me.chkOptionEnableLossDateduringClaim.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 421)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(244, 17)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "Claim Payment Ex-Gratia Account"
        '
        'TxtClaimPayExGratiaAccount
        '
        Me.TxtClaimPayExGratiaAccount.AcceptsReturn = True
        Me.TxtClaimPayExGratiaAccount.AccessibleDescription = "Claim Payment Ex-Gratia Account"
        Me.TxtClaimPayExGratiaAccount.BackColor = System.Drawing.SystemColors.Window
        Me.TxtClaimPayExGratiaAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TxtClaimPayExGratiaAccount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtClaimPayExGratiaAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TxtClaimPayExGratiaAccount.Location = New System.Drawing.Point(287, 418)
        Me.TxtClaimPayExGratiaAccount.MaxLength = 0
        Me.TxtClaimPayExGratiaAccount.Name = "TxtClaimPayExGratiaAccount"
        Me.TxtClaimPayExGratiaAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TxtClaimPayExGratiaAccount.Size = New System.Drawing.Size(183, 24)
        Me.TxtClaimPayExGratiaAccount.TabIndex = 24
        Me.TxtClaimPayExGratiaAccount.Tag = "5114"
        '
        'chkEnhanceCaseSearching
        '
        Me.chkEnhanceCaseSearching.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnhanceCaseSearching.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEnhanceCaseSearching.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnhanceCaseSearching.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnhanceCaseSearching.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnhanceCaseSearching.Location = New System.Drawing.Point(4, 392)
        Me.chkEnhanceCaseSearching.Name = "chkEnhanceCaseSearching"
        Me.chkEnhanceCaseSearching.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnhanceCaseSearching.Size = New System.Drawing.Size(385, 26)
        Me.chkEnhanceCaseSearching.TabIndex = 23
        Me.chkEnhanceCaseSearching.Tag = "5099"
        Me.chkEnhanceCaseSearching.Text = "Enhanced Case Searching"
        Me.chkEnhanceCaseSearching.UseVisualStyleBackColor = False
        '
        'chkAutomaticRecieptGenerationForSalvageTP
        '
        Me.chkAutomaticRecieptGenerationForSalvageTP.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutomaticRecieptGenerationForSalvageTP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAutomaticRecieptGenerationForSalvageTP.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutomaticRecieptGenerationForSalvageTP.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutomaticRecieptGenerationForSalvageTP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutomaticRecieptGenerationForSalvageTP.Location = New System.Drawing.Point(4, 444)
        Me.chkAutomaticRecieptGenerationForSalvageTP.Name = "chkAutomaticRecieptGenerationForSalvageTP"
        Me.chkAutomaticRecieptGenerationForSalvageTP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutomaticRecieptGenerationForSalvageTP.Size = New System.Drawing.Size(384, 30)
        Me.chkAutomaticRecieptGenerationForSalvageTP.TabIndex = 24
        Me.chkAutomaticRecieptGenerationForSalvageTP.Tag = "5117"
        Me.chkAutomaticRecieptGenerationForSalvageTP.Text = "Automate Receipt Generation for Salvage/Third Party receipt:"
        Me.chkAutomaticRecieptGenerationForSalvageTP.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 519)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 17)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "Tax Group"
        '
        'chkClaimsReserveGross
        '
        Me.chkClaimsReserveGross.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimsReserveGross.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkClaimsReserveGross.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimsReserveGross.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimsReserveGross.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimsReserveGross.Location = New System.Drawing.Point(4, 490)
        Me.chkClaimsReserveGross.Name = "chkClaimsReserveGross"
        Me.chkClaimsReserveGross.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimsReserveGross.Size = New System.Drawing.Size(384, 26)
        Me.chkClaimsReserveGross.TabIndex = 26
        Me.chkClaimsReserveGross.Tag = "5239"
        Me.chkClaimsReserveGross.Text = "Claims Reserves are Gross"
        Me.chkClaimsReserveGross.UseVisualStyleBackColor = False
        '
        'cboOption5240
        '
        Me.cboOption5240.AccessibleDescription = "Tax Group:"
        Me.cboOption5240.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5240.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5240.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5240.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5240.Location = New System.Drawing.Point(288, 516)
        Me.cboOption5240.Name = "cboOption5240"
        Me.cboOption5240.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5240.Size = New System.Drawing.Size(182, 25)
        Me.cboOption5240.TabIndex = 29
        Me.cboOption5240.Tag = "5240"
        Me.cboOption5240.Text = "cboOption5240"
        '
        'cboOption5031
        '
        Me.cboOption5031.AccessibleDescription = "Case Numbering Scheme:"
        Me.cboOption5031.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5031.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5031.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5031.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5031.Location = New System.Drawing.Point(286, 94)
        Me.cboOption5031.Name = "cboOption5031"
        Me.cboOption5031.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5031.Size = New System.Drawing.Size(308, 25)
        Me.cboOption5031.TabIndex = 2
        Me.cboOption5031.Tag = "5031"
        Me.cboOption5031.Text = "cboOption5031"
        '
        'chkSkipCashlistProcessForNegativeClaimPayment
        '
        Me.chkSkipCashlistProcessForNegativeClaimPayment.BackColor = System.Drawing.SystemColors.Control
        Me.chkSkipCashlistProcessForNegativeClaimPayment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSkipCashlistProcessForNegativeClaimPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Location = New System.Drawing.Point(4, 467)
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Name = "chkSkipCashlistProcessForNegativeClaimPayment"
        Me.chkSkipCashlistProcessForNegativeClaimPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Size = New System.Drawing.Size(384, 29)
        Me.chkSkipCashlistProcessForNegativeClaimPayment.TabIndex = 26
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Tag = "5115"
        Me.chkSkipCashlistProcessForNegativeClaimPayment.Text = "Skip Cash List process for negative Claim Payment :"
        Me.chkSkipCashlistProcessForNegativeClaimPayment.UseVisualStyleBackColor = False
        '
        'frmUWClaims
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 16)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(575, 560)
        Me.Controls.Add(Me.chkSkipCashlistProcessForNegativeClaimPayment)
        Me.Controls.Add(Me.cboOption5240)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkClaimsReserveGross)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtClaimPayExGratiaAccount)
        Me.Controls.Add(Me.chkEnhanceCaseSearching)
        Me.Controls.Add(Me.chkOption5071)
        Me.Controls.Add(Me.chkOption5072)
        Me.Controls.Add(Me.chkRestrictNonZeroClosure)
        Me.Controls.Add(Me.chkTotalIncurredLessReceipts)
        Me.Controls.Add(Me.chkOption5067)
        Me.Controls.Add(Me.chkClaimPaymentTaxGroupIsMandatory5063)
        Me.Controls.Add(Me.cboOption5035)
        Me.Controls.Add(Me.cmdClearCloseCaseDocument)
        Me.Controls.Add(Me.cmdCloseCaseDocument)
        Me.Controls.Add(Me.txtCloseCaseDocument)
        Me.Controls.Add(Me.txtOpenCaseDocument)
        Me.Controls.Add(Me.txtEditCaseDocument)
        Me.Controls.Add(Me.cmdOpenCaseDocument)
        Me.Controls.Add(Me.cmdEditCaseDocument)
        Me.Controls.Add(Me.cmdClearOpenCaseDocument)
        Me.Controls.Add(Me.cmdCleaEditCaseDocument)
        Me.Controls.Add(Me.cboOption5031)
        Me.Controls.Add(Me.cboOption5030)
        Me.Controls.Add(Me.lblCaseScreen5035)
        Me.Controls.Add(Me.lblOption5031)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkOptionEnableLossDateduringClaim)
        Me.Controls.Add(Me.chkAutomaticRecieptGenerationForSalvageTP)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmUWClaims"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtClaimPayExGratiaAccount As System.Windows.Forms.TextBox
    Public WithEvents chkEnhanceCaseSearching As System.Windows.Forms.CheckBox
    Public WithEvents chkSkipCashlistProcessForNegativeClaimPayment As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As Label
    Public WithEvents chkClaimsReserveGross As CheckBox
    Public WithEvents cboOption5240 As ComboBox
    Public WithEvents cboOption5031 As ComboBox
#End Region
End Class
