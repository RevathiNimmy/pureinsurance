<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccountsGeneral
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelblOption5061()
		InitializelblOption15()
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
    Public WithEvents txtRoundOffZero As System.Windows.Forms.TextBox
    Public WithEvents cboOption5069 As System.Windows.Forms.ComboBox
    Public WithEvents chkPaymentTypeEditedonParty As System.Windows.Forms.CheckBox
    Public WithEvents txtCancelPolicyWriteOffAcCode As System.Windows.Forms.TextBox
    Public WithEvents txtCurGainLossAllocationLimit As System.Windows.Forms.TextBox
    Public WithEvents chkOption5059 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5058 As System.Windows.Forms.CheckBox
    Public WithEvents txtAgentSuspenseAccount As System.Windows.Forms.TextBox
    Public WithEvents chkOption5038 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5037 As System.Windows.Forms.CheckBox
    Public WithEvents Text5 As System.Windows.Forms.TextBox
    Public WithEvents chkOption5016 As System.Windows.Forms.CheckBox
    Public WithEvents cmdClearPaymentLetter As System.Windows.Forms.Button
    Public WithEvents cmdClearReceiptLetter As System.Windows.Forms.Button
    Public WithEvents cmdPaymentLetter As System.Windows.Forms.Button
    Public WithEvents cmdReceiptLetter As System.Windows.Forms.Button
    Public WithEvents Text4 As System.Windows.Forms.TextBox
    Public WithEvents Text3 As System.Windows.Forms.TextBox
    Public WithEvents Text2 As System.Windows.Forms.TextBox
    Public WithEvents Check1 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption1031 As System.Windows.Forms.CheckBox
    Public WithEvents cboOption60 As System.Windows.Forms.ComboBox
    Public WithEvents chkOption51 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption50 As System.Windows.Forms.CheckBox
    Public WithEvents txtPaymentLetter As System.Windows.Forms.TextBox
    Public WithEvents txtReceiptLetter As System.Windows.Forms.TextBox
    Public WithEvents chkOption5001 As System.Windows.Forms.CheckBox
    Public WithEvents Text1 As System.Windows.Forms.TextBox
    Public WithEvents txtChequeLetter As System.Windows.Forms.TextBox
    Public WithEvents ChkOption66 As System.Windows.Forms.CheckBox
    Public WithEvents cmdChequeLetter As System.Windows.Forms.Button
    Public WithEvents cmdClearChequeLetter As System.Windows.Forms.Button
    Public WithEvents txtChequeExportPath As System.Windows.Forms.TextBox
    Public WithEvents lblRoundOffZero5080 As System.Windows.Forms.Label
    Public WithEvents lblOption5069 As System.Windows.Forms.Label
    Private WithEvents _lblOption5061_7 As System.Windows.Forms.Label
    Public WithEvents lblPercentageMark As System.Windows.Forms.Label
    Public WithEvents lblOption5060 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_6 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_4 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_3 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_2 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_1 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_5 As System.Windows.Forms.Label
    Public WithEvents Label26 As System.Windows.Forms.Label
    Private WithEvents _lblOption15_0 As System.Windows.Forms.Label
    Public WithEvents lblChequeExportPath As System.Windows.Forms.Label
    Public lblOption15(6) As System.Windows.Forms.Label
    Public lblOption5061(7) As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtRoundOffZero = New System.Windows.Forms.TextBox()
        Me.cboOption5069 = New System.Windows.Forms.ComboBox()
        Me.chkPaymentTypeEditedonParty = New System.Windows.Forms.CheckBox()
        Me.txtCancelPolicyWriteOffAcCode = New System.Windows.Forms.TextBox()
        Me.txtCurGainLossAllocationLimit = New System.Windows.Forms.TextBox()
        Me.chkOption5059 = New System.Windows.Forms.CheckBox()
        Me.chkOption5058 = New System.Windows.Forms.CheckBox()
        Me.txtAgentSuspenseAccount = New System.Windows.Forms.TextBox()
        Me.chkOption5038 = New System.Windows.Forms.CheckBox()
        Me.chkOption5037 = New System.Windows.Forms.CheckBox()
        Me.Text5 = New System.Windows.Forms.TextBox()
        Me.chkOption5016 = New System.Windows.Forms.CheckBox()
        Me.cmdClearPaymentLetter = New System.Windows.Forms.Button()
        Me.cmdClearReceiptLetter = New System.Windows.Forms.Button()
        Me.cmdPaymentLetter = New System.Windows.Forms.Button()
        Me.cmdReceiptLetter = New System.Windows.Forms.Button()
        Me.Text4 = New System.Windows.Forms.TextBox()
        Me.Text3 = New System.Windows.Forms.TextBox()
        Me.Text2 = New System.Windows.Forms.TextBox()
        Me.Check1 = New System.Windows.Forms.CheckBox()
        Me.chkOption1031 = New System.Windows.Forms.CheckBox()
        Me.cboOption60 = New System.Windows.Forms.ComboBox()
        Me.chkOption51 = New System.Windows.Forms.CheckBox()
        Me.chkOption50 = New System.Windows.Forms.CheckBox()
        Me.txtPaymentLetter = New System.Windows.Forms.TextBox()
        Me.txtReceiptLetter = New System.Windows.Forms.TextBox()
        Me.chkOption5001 = New System.Windows.Forms.CheckBox()
        Me.Text1 = New System.Windows.Forms.TextBox()
        Me.txtChequeLetter = New System.Windows.Forms.TextBox()
        Me.ChkOption66 = New System.Windows.Forms.CheckBox()
        Me.cmdChequeLetter = New System.Windows.Forms.Button()
        Me.cmdClearChequeLetter = New System.Windows.Forms.Button()
        Me.txtChequeExportPath = New System.Windows.Forms.TextBox()
        Me.lblRoundOffZero5080 = New System.Windows.Forms.Label()
        Me.lblOption5069 = New System.Windows.Forms.Label()
        Me._lblOption5061_7 = New System.Windows.Forms.Label()
        Me.lblPercentageMark = New System.Windows.Forms.Label()
        Me.lblOption5060 = New System.Windows.Forms.Label()
        Me._lblOption15_6 = New System.Windows.Forms.Label()
        Me._lblOption15_4 = New System.Windows.Forms.Label()
        Me._lblOption15_3 = New System.Windows.Forms.Label()
        Me._lblOption15_2 = New System.Windows.Forms.Label()
        Me._lblOption15_1 = New System.Windows.Forms.Label()
        Me._lblOption15_5 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me._lblOption15_0 = New System.Windows.Forms.Label()
        Me.lblChequeExportPath = New System.Windows.Forms.Label()
        Me.chkSplitReceipt5091 = New System.Windows.Forms.CheckBox()
        Me.chkOption5095 = New System.Windows.Forms.CheckBox()
        Me.chkSingleCashListRecieptPerAllocation = New System.Windows.Forms.CheckBox()
        Me.chk5143 = New System.Windows.Forms.CheckBox()
        Me.Chk_accountaccesslimited = New System.Windows.Forms.CheckBox()
        Me.chk5208 = New System.Windows.Forms.CheckBox()
        Me.lblRSTolerance = New System.Windows.Forms.Label()
        Me.txtRSTolerance = New System.Windows.Forms.TextBox()
        Me.lblRSCurrency = New System.Windows.Forms.Label()
        Me.cboOption5243 = New System.Windows.Forms.ComboBox()
        Me.txtOption5247 = New System.Windows.Forms.TextBox()
        Me.lblOption5247 = New System.Windows.Forms.Label()
        Me.chkOption5246 = New System.Windows.Forms.CheckBox()
        Me.txtOption5248 = New System.Windows.Forms.TextBox()
        Me.lblOption5248 = New System.Windows.Forms.Label()
        Me.chkOption5264 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'txtRoundOffZero
        '
        Me.txtRoundOffZero.AcceptsReturn = True
        Me.txtRoundOffZero.AccessibleDescription = "Gross Total(Premium) Round Off Account code:"
        Me.txtRoundOffZero.BackColor = System.Drawing.SystemColors.Window
        Me.txtRoundOffZero.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRoundOffZero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRoundOffZero.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRoundOffZero.Location = New System.Drawing.Point(468, 519)
        Me.txtRoundOffZero.MaxLength = 255
        Me.txtRoundOffZero.Name = "txtRoundOffZero"
        Me.txtRoundOffZero.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRoundOffZero.Size = New System.Drawing.Size(113, 20)
        Me.txtRoundOffZero.TabIndex = 47
        Me.txtRoundOffZero.Tag = "5080"
        '
        'cboOption5069
        '
        Me.cboOption5069.AccessibleDescription = "Credit Card Processing Method:"
        Me.cboOption5069.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5069.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5069.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5069.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5069.Location = New System.Drawing.Point(456, 470)
        Me.cboOption5069.Name = "cboOption5069"
        Me.cboOption5069.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5069.Size = New System.Drawing.Size(125, 21)
        Me.cboOption5069.TabIndex = 44
        Me.cboOption5069.Tag = "5069"
        Me.cboOption5069.Text = "cboOption5069"
        '
        'chkPaymentTypeEditedonParty
        '
        Me.chkPaymentTypeEditedonParty.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaymentTypeEditedonParty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPaymentTypeEditedonParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaymentTypeEditedonParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaymentTypeEditedonParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaymentTypeEditedonParty.Location = New System.Drawing.Point(6, 553)
        Me.chkPaymentTypeEditedonParty.Name = "chkPaymentTypeEditedonParty"
        Me.chkPaymentTypeEditedonParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaymentTypeEditedonParty.Size = New System.Drawing.Size(287, 17)
        Me.chkPaymentTypeEditedonParty.TabIndex = 26
        Me.chkPaymentTypeEditedonParty.Tag = "5062"
        Me.chkPaymentTypeEditedonParty.Text = "Payment Type can only be edited on Party"
        Me.chkPaymentTypeEditedonParty.UseVisualStyleBackColor = False
        '
        'txtCancelPolicyWriteOffAcCode
        '
        Me.txtCancelPolicyWriteOffAcCode.AcceptsReturn = True
        Me.txtCancelPolicyWriteOffAcCode.AccessibleDescription = "Cancel Policy Write Off Account Code:"
        Me.txtCancelPolicyWriteOffAcCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCancelPolicyWriteOffAcCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCancelPolicyWriteOffAcCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCancelPolicyWriteOffAcCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCancelPolicyWriteOffAcCode.Location = New System.Drawing.Point(233, 179)
        Me.txtCancelPolicyWriteOffAcCode.MaxLength = 0
        Me.txtCancelPolicyWriteOffAcCode.Name = "txtCancelPolicyWriteOffAcCode"
        Me.txtCancelPolicyWriteOffAcCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCancelPolicyWriteOffAcCode.Size = New System.Drawing.Size(145, 20)
        Me.txtCancelPolicyWriteOffAcCode.TabIndex = 42
        Me.txtCancelPolicyWriteOffAcCode.Tag = "5061"
        '
        'txtCurGainLossAllocationLimit
        '
        Me.txtCurGainLossAllocationLimit.AcceptsReturn = True
        Me.txtCurGainLossAllocationLimit.AccessibleDescription = "Currency Gain/Loss Auto Allocation Limit:"
        Me.txtCurGainLossAllocationLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurGainLossAllocationLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurGainLossAllocationLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurGainLossAllocationLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurGainLossAllocationLimit.Location = New System.Drawing.Point(551, 495)
        Me.txtCurGainLossAllocationLimit.MaxLength = 0
        Me.txtCurGainLossAllocationLimit.Name = "txtCurGainLossAllocationLimit"
        Me.txtCurGainLossAllocationLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurGainLossAllocationLimit.Size = New System.Drawing.Size(30, 20)
        Me.txtCurGainLossAllocationLimit.TabIndex = 27
        Me.txtCurGainLossAllocationLimit.Tag = "5060"
        '
        'chkOption5059
        '
        Me.chkOption5059.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5059.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5059.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5059.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5059.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5059.Location = New System.Drawing.Point(6, 533)
        Me.chkOption5059.Name = "chkOption5059"
        Me.chkOption5059.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5059.Size = New System.Drawing.Size(287, 17)
        Me.chkOption5059.TabIndex = 25
        Me.chkOption5059.Tag = "5059"
        Me.chkOption5059.Text = "Auto Allocation If Able"
        Me.chkOption5059.UseVisualStyleBackColor = False
        Me.chkOption5059.Visible = False
        '
        'chkOption5058
        '
        Me.chkOption5058.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5058.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5058.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5058.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5058.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5058.Location = New System.Drawing.Point(6, 513)
        Me.chkOption5058.Name = "chkOption5058"
        Me.chkOption5058.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5058.Size = New System.Drawing.Size(287, 17)
        Me.chkOption5058.TabIndex = 24
        Me.chkOption5058.Tag = "5058"
        Me.chkOption5058.Text = "Multi Currency Banking Option"
        Me.chkOption5058.UseVisualStyleBackColor = False
        Me.chkOption5058.Visible = False
        '
        'txtAgentSuspenseAccount
        '
        Me.txtAgentSuspenseAccount.AcceptsReturn = True
        Me.txtAgentSuspenseAccount.AccessibleDescription = "Agent Suspense Account:"
        Me.txtAgentSuspenseAccount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgentSuspenseAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgentSuspenseAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgentSuspenseAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgentSuspenseAccount.Location = New System.Drawing.Point(456, 445)
        Me.txtAgentSuspenseAccount.MaxLength = 0
        Me.txtAgentSuspenseAccount.Name = "txtAgentSuspenseAccount"
        Me.txtAgentSuspenseAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgentSuspenseAccount.Size = New System.Drawing.Size(125, 20)
        Me.txtAgentSuspenseAccount.TabIndex = 22
        Me.txtAgentSuspenseAccount.Tag = "5039"
        '
        'chkOption5038
        '
        Me.chkOption5038.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5038.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5038.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5038.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5038.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5038.Location = New System.Drawing.Point(6, 485)
        Me.chkOption5038.Name = "chkOption5038"
        Me.chkOption5038.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5038.Size = New System.Drawing.Size(287, 23)
        Me.chkOption5038.TabIndex = 23
        Me.chkOption5038.Tag = "5038"
        Me.chkOption5038.Text = "Posting by Policy Effective and Transaction Date:"
        Me.chkOption5038.UseVisualStyleBackColor = False
        Me.chkOption5038.Visible = False
        '
        'chkOption5037
        '
        Me.chkOption5037.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5037.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5037.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5037.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5037.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5037.Location = New System.Drawing.Point(6, 462)
        Me.chkOption5037.Name = "chkOption5037"
        Me.chkOption5037.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5037.Size = New System.Drawing.Size(287, 17)
        Me.chkOption5037.TabIndex = 21
        Me.chkOption5037.Tag = "5037"
        Me.chkOption5037.Text = "Agent Commission Suspended Postings:"
        Me.chkOption5037.UseVisualStyleBackColor = False
        Me.chkOption5037.Visible = False
        '
        'Text5
        '
        Me.Text5.AcceptsReturn = True
        Me.Text5.AccessibleDescription = "Intermediary Write Off Account Code"
        Me.Text5.BackColor = System.Drawing.SystemColors.Window
        Me.Text5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text5.Location = New System.Drawing.Point(233, 155)
        Me.Text5.MaxLength = 0
        Me.Text5.Name = "Text5"
        Me.Text5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text5.Size = New System.Drawing.Size(145, 20)
        Me.Text5.TabIndex = 7
        Me.Text5.Tag = "5028"
        '
        'chkOption5016
        '
        Me.chkOption5016.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5016.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5016.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5016.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5016.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5016.Location = New System.Drawing.Point(6, 440)
        Me.chkOption5016.Name = "chkOption5016"
        Me.chkOption5016.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5016.Size = New System.Drawing.Size(287, 19)
        Me.chkOption5016.TabIndex = 20
        Me.chkOption5016.Tag = "5016"
        Me.chkOption5016.Text = "Tax posted by Tax Band to Client Account:"
        Me.chkOption5016.UseVisualStyleBackColor = False
        Me.chkOption5016.Visible = False
        '
        'cmdClearPaymentLetter
        '
        Me.cmdClearPaymentLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearPaymentLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearPaymentLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearPaymentLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearPaymentLetter.Location = New System.Drawing.Point(344, 289)
        Me.cmdClearPaymentLetter.Name = "cmdClearPaymentLetter"
        Me.cmdClearPaymentLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearPaymentLetter.Size = New System.Drawing.Size(57, 19)
        Me.cmdClearPaymentLetter.TabIndex = 14
        Me.cmdClearPaymentLetter.Tag = "63,ClearDocument"
        Me.cmdClearPaymentLetter.Text = "Clear"
        Me.cmdClearPaymentLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearPaymentLetter.UseVisualStyleBackColor = False
        '
        'cmdClearReceiptLetter
        '
        Me.cmdClearReceiptLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearReceiptLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearReceiptLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearReceiptLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearReceiptLetter.Location = New System.Drawing.Point(344, 267)
        Me.cmdClearReceiptLetter.Name = "cmdClearReceiptLetter"
        Me.cmdClearReceiptLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearReceiptLetter.Size = New System.Drawing.Size(57, 19)
        Me.cmdClearReceiptLetter.TabIndex = 11
        Me.cmdClearReceiptLetter.Tag = "61,ClearDocument"
        Me.cmdClearReceiptLetter.Text = "Clear"
        Me.cmdClearReceiptLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearReceiptLetter.UseVisualStyleBackColor = False
        '
        'cmdPaymentLetter
        '
        Me.cmdPaymentLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPaymentLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPaymentLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPaymentLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPaymentLetter.Location = New System.Drawing.Point(6, 290)
        Me.cmdPaymentLetter.Name = "cmdPaymentLetter"
        Me.cmdPaymentLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPaymentLetter.Size = New System.Drawing.Size(169, 19)
        Me.cmdPaymentLetter.TabIndex = 13
        Me.cmdPaymentLetter.Tag = "63,GetDocument"
        Me.cmdPaymentLetter.Text = "Default Payment Letter"
        Me.cmdPaymentLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPaymentLetter.UseVisualStyleBackColor = False
        '
        'cmdReceiptLetter
        '
        Me.cmdReceiptLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReceiptLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReceiptLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReceiptLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReceiptLetter.Location = New System.Drawing.Point(6, 266)
        Me.cmdReceiptLetter.Name = "cmdReceiptLetter"
        Me.cmdReceiptLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReceiptLetter.Size = New System.Drawing.Size(169, 19)
        Me.cmdReceiptLetter.TabIndex = 10
        Me.cmdReceiptLetter.Tag = "61,GetDocument"
        Me.cmdReceiptLetter.Text = "Default Receipt Letter"
        Me.cmdReceiptLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReceiptLetter.UseVisualStyleBackColor = False
        '
        'Text4
        '
        Me.Text4.AcceptsReturn = True
        Me.Text4.AccessibleDescription = "Creditor Write Off Account Code:"
        Me.Text4.BackColor = System.Drawing.SystemColors.Window
        Me.Text4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text4.Location = New System.Drawing.Point(233, 130)
        Me.Text4.MaxLength = 0
        Me.Text4.Name = "Text4"
        Me.Text4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text4.Size = New System.Drawing.Size(145, 20)
        Me.Text4.TabIndex = 6
        Me.Text4.Tag = "153"
        '
        'Text3
        '
        Me.Text3.AcceptsReturn = True
        Me.Text3.AccessibleDescription = "Debtor Write Off Account Code:"
        Me.Text3.BackColor = System.Drawing.SystemColors.Window
        Me.Text3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text3.Location = New System.Drawing.Point(233, 107)
        Me.Text3.MaxLength = 0
        Me.Text3.Name = "Text3"
        Me.Text3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text3.Size = New System.Drawing.Size(145, 20)
        Me.Text3.TabIndex = 5
        Me.Text3.Tag = "152"
        '
        'Text2
        '
        Me.Text2.AcceptsReturn = True
        Me.Text2.AccessibleDescription = "Currency Loss Account Code:"
        Me.Text2.BackColor = System.Drawing.SystemColors.Window
        Me.Text2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text2.Location = New System.Drawing.Point(233, 83)
        Me.Text2.MaxLength = 0
        Me.Text2.Name = "Text2"
        Me.Text2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text2.Size = New System.Drawing.Size(145, 20)
        Me.Text2.TabIndex = 4
        Me.Text2.Tag = "151"
        '
        'Check1
        '
        Me.Check1.BackColor = System.Drawing.SystemColors.Control
        Me.Check1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Check1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check1.Location = New System.Drawing.Point(7, 201)
        Me.Check1.Name = "Check1"
        Me.Check1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check1.Size = New System.Drawing.Size(240, 24)
        Me.Check1.TabIndex = 8
        Me.Check1.Tag = "154"
        Me.Check1.Text = "Currency Rates Per Branch:"
        Me.Check1.UseVisualStyleBackColor = False
        Me.Check1.Visible = False
        '
        'chkOption1031
        '
        Me.chkOption1031.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1031.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1031.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1031.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1031.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1031.Location = New System.Drawing.Point(8, 222)
        Me.chkOption1031.Name = "chkOption1031"
        Me.chkOption1031.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1031.Size = New System.Drawing.Size(239, 28)
        Me.chkOption1031.TabIndex = 9
        Me.chkOption1031.Tag = "1031"
        Me.chkOption1031.Text = "Debt Roll-up Enabled:"
        Me.chkOption1031.UseVisualStyleBackColor = False
        Me.chkOption1031.Visible = False
        '
        'cboOption60
        '
        Me.cboOption60.AccessibleDescription = "Cheque Production:"
        Me.cboOption60.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption60.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption60.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption60.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption60.Location = New System.Drawing.Point(233, 32)
        Me.cboOption60.Name = "cboOption60"
        Me.cboOption60.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption60.Size = New System.Drawing.Size(257, 21)
        Me.cboOption60.TabIndex = 1
        Me.cboOption60.Tag = "60,M"
        Me.cboOption60.Text = "cboOption60"
        '
        'chkOption51
        '
        Me.chkOption51.AccessibleDescription = "Default Payment Letter: Use Default Printer Settings:"
        Me.chkOption51.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption51.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption51.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption51.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption51.Location = New System.Drawing.Point(418, 291)
        Me.chkOption51.Name = "chkOption51"
        Me.chkOption51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption51.Size = New System.Drawing.Size(17, 17)
        Me.chkOption51.TabIndex = 15
        Me.chkOption51.Tag = "64"
        Me.chkOption51.UseVisualStyleBackColor = False
        '
        'chkOption50
        '
        Me.chkOption50.AccessibleDescription = "Default Receipt Letter: Use Default Printer Settings:"
        Me.chkOption50.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption50.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption50.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption50.Location = New System.Drawing.Point(418, 268)
        Me.chkOption50.Name = "chkOption50"
        Me.chkOption50.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption50.Size = New System.Drawing.Size(17, 17)
        Me.chkOption50.TabIndex = 12
        Me.chkOption50.Tag = "62"
        Me.chkOption50.UseVisualStyleBackColor = False
        '
        'txtPaymentLetter
        '
        Me.txtPaymentLetter.AcceptsReturn = True
        Me.txtPaymentLetter.AccessibleDescription = "Default Payment Letter"
        Me.txtPaymentLetter.BackColor = System.Drawing.SystemColors.Control
        Me.txtPaymentLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentLetter.Location = New System.Drawing.Point(182, 291)
        Me.txtPaymentLetter.MaxLength = 0
        Me.txtPaymentLetter.Name = "txtPaymentLetter"
        Me.txtPaymentLetter.ReadOnly = True
        Me.txtPaymentLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentLetter.Size = New System.Drawing.Size(153, 21)
        Me.txtPaymentLetter.TabIndex = 30
        Me.txtPaymentLetter.Tag = "63,ShowTemplateCode,D"
        Me.txtPaymentLetter.Text = "(none)"
        '
        'txtReceiptLetter
        '
        Me.txtReceiptLetter.AcceptsReturn = True
        Me.txtReceiptLetter.AccessibleDescription = "Default Receipt Letter"
        Me.txtReceiptLetter.BackColor = System.Drawing.SystemColors.Control
        Me.txtReceiptLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReceiptLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReceiptLetter.Location = New System.Drawing.Point(183, 267)
        Me.txtReceiptLetter.MaxLength = 0
        Me.txtReceiptLetter.Name = "txtReceiptLetter"
        Me.txtReceiptLetter.ReadOnly = True
        Me.txtReceiptLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReceiptLetter.Size = New System.Drawing.Size(153, 21)
        Me.txtReceiptLetter.TabIndex = 29
        Me.txtReceiptLetter.Tag = "61,ShowTemplateCode,D"
        Me.txtReceiptLetter.Text = "(none)"
        '
        'chkOption5001
        '
        Me.chkOption5001.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5001.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5001.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5001.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5001.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5001.Location = New System.Drawing.Point(10, 8)
        Me.chkOption5001.Name = "chkOption5001"
        Me.chkOption5001.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5001.Size = New System.Drawing.Size(237, 18)
        Me.chkOption5001.TabIndex = 0
        Me.chkOption5001.Tag = "5001,M"
        Me.chkOption5001.Text = "Credit Control Enabled:"
        Me.chkOption5001.UseVisualStyleBackColor = False
        Me.chkOption5001.Visible = False
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
        Me.Text1.AccessibleDescription = "Currency Gains Account Code:"
        Me.Text1.BackColor = System.Drawing.SystemColors.Window
        Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text1.Location = New System.Drawing.Point(233, 60)
        Me.Text1.MaxLength = 0
        Me.Text1.Name = "Text1"
        Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text1.Size = New System.Drawing.Size(145, 20)
        Me.Text1.TabIndex = 3
        Me.Text1.Tag = "150"
        '
        'txtChequeLetter
        '
        Me.txtChequeLetter.AcceptsReturn = True
        Me.txtChequeLetter.AccessibleDescription = "Default Cheque Letter"
        Me.txtChequeLetter.BackColor = System.Drawing.SystemColors.Control
        Me.txtChequeLetter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChequeLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChequeLetter.Location = New System.Drawing.Point(182, 315)
        Me.txtChequeLetter.MaxLength = 0
        Me.txtChequeLetter.Name = "txtChequeLetter"
        Me.txtChequeLetter.ReadOnly = True
        Me.txtChequeLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChequeLetter.Size = New System.Drawing.Size(153, 21)
        Me.txtChequeLetter.TabIndex = 28
        Me.txtChequeLetter.Tag = "5003,ShowTemplateCode,D"
        Me.txtChequeLetter.Text = "(none)"
        '
        'ChkOption66
        '
        Me.ChkOption66.AccessibleDescription = "Default Cheque Letter: Use Default Printer Settings:"
        Me.ChkOption66.BackColor = System.Drawing.SystemColors.Control
        Me.ChkOption66.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkOption66.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkOption66.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkOption66.Location = New System.Drawing.Point(418, 315)
        Me.ChkOption66.Name = "ChkOption66"
        Me.ChkOption66.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkOption66.Size = New System.Drawing.Size(17, 17)
        Me.ChkOption66.TabIndex = 18
        Me.ChkOption66.Tag = "5006"
        Me.ChkOption66.UseVisualStyleBackColor = False
        '
        'cmdChequeLetter
        '
        Me.cmdChequeLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChequeLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChequeLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChequeLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChequeLetter.Location = New System.Drawing.Point(6, 314)
        Me.cmdChequeLetter.Name = "cmdChequeLetter"
        Me.cmdChequeLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChequeLetter.Size = New System.Drawing.Size(169, 19)
        Me.cmdChequeLetter.TabIndex = 16
        Me.cmdChequeLetter.Tag = "5003,GetDocument"
        Me.cmdChequeLetter.Text = "Default Cheque Letter"
        Me.cmdChequeLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChequeLetter.UseVisualStyleBackColor = False
        '
        'cmdClearChequeLetter
        '
        Me.cmdClearChequeLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearChequeLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearChequeLetter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearChequeLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearChequeLetter.Location = New System.Drawing.Point(344, 314)
        Me.cmdClearChequeLetter.Name = "cmdClearChequeLetter"
        Me.cmdClearChequeLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearChequeLetter.Size = New System.Drawing.Size(57, 19)
        Me.cmdClearChequeLetter.TabIndex = 17
        Me.cmdClearChequeLetter.Tag = "5003,ClearDocument"
        Me.cmdClearChequeLetter.Text = "Clear"
        Me.cmdClearChequeLetter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearChequeLetter.UseVisualStyleBackColor = False
        '
        'txtChequeExportPath
        '
        Me.txtChequeExportPath.AcceptsReturn = True
        Me.txtChequeExportPath.AccessibleDescription = "Cheque Export Path:"
        Me.txtChequeExportPath.BackColor = System.Drawing.SystemColors.Window
        Me.txtChequeExportPath.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChequeExportPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeExportPath.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChequeExportPath.Location = New System.Drawing.Point(180, 339)
        Me.txtChequeExportPath.MaxLength = 255
        Me.txtChequeExportPath.Name = "txtChequeExportPath"
        Me.txtChequeExportPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChequeExportPath.Size = New System.Drawing.Size(399, 20)
        Me.txtChequeExportPath.TabIndex = 19
        Me.txtChequeExportPath.Tag = "158"
        '
        'lblRoundOffZero5080
        '
        Me.lblRoundOffZero5080.BackColor = System.Drawing.SystemColors.Control
        Me.lblRoundOffZero5080.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRoundOffZero5080.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRoundOffZero5080.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRoundOffZero5080.Location = New System.Drawing.Point(299, 522)
        Me.lblRoundOffZero5080.Name = "lblRoundOffZero5080"
        Me.lblRoundOffZero5080.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRoundOffZero5080.Size = New System.Drawing.Size(169, 25)
        Me.lblRoundOffZero5080.TabIndex = 46
        Me.lblRoundOffZero5080.Tag = "5080"
        Me.lblRoundOffZero5080.Text = "Gross Total(Premium) Round Off Account code"
        '
        'lblOption5069
        '
        Me.lblOption5069.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption5069.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption5069.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption5069.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption5069.Location = New System.Drawing.Point(302, 471)
        Me.lblOption5069.Name = "lblOption5069"
        Me.lblOption5069.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption5069.Size = New System.Drawing.Size(151, 25)
        Me.lblOption5069.TabIndex = 45
        Me.lblOption5069.Tag = "5069"
        Me.lblOption5069.Text = "Credit Card Processing Method"
        '
        '_lblOption5061_7
        '
        Me._lblOption5061_7.AutoSize = True
        Me._lblOption5061_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption5061_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption5061_7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption5061_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption5061_7.Location = New System.Drawing.Point(10, 185)
        Me._lblOption5061_7.Name = "_lblOption5061_7"
        Me._lblOption5061_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption5061_7.Size = New System.Drawing.Size(226, 13)
        Me._lblOption5061_7.TabIndex = 43
        Me._lblOption5061_7.Tag = "5027"
        Me._lblOption5061_7.Text = "Cancel Policy Write Off Account Code:"
        '
        'lblPercentageMark
        '
        Me.lblPercentageMark.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentageMark.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentageMark.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentageMark.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentageMark.Location = New System.Drawing.Point(578, 497)
        Me.lblPercentageMark.Name = "lblPercentageMark"
        Me.lblPercentageMark.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentageMark.Size = New System.Drawing.Size(20, 17)
        Me.lblPercentageMark.TabIndex = 41
        Me.lblPercentageMark.Text = "%"
        '
        'lblOption5060
        '
        Me.lblOption5060.AutoSize = True
        Me.lblOption5060.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption5060.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption5060.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption5060.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption5060.Location = New System.Drawing.Point(300, 497)
        Me.lblOption5060.Name = "lblOption5060"
        Me.lblOption5060.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption5060.Size = New System.Drawing.Size(245, 13)
        Me.lblOption5060.TabIndex = 40
        Me.lblOption5060.Tag = "5027"
        Me.lblOption5060.Text = "Currency Gain/Loss Auto Allocation Limit:"
        '
        '_lblOption15_6
        '
        Me._lblOption15_6.AutoSize = True
        Me._lblOption15_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_6.Location = New System.Drawing.Point(300, 446)
        Me._lblOption15_6.Name = "_lblOption15_6"
        Me._lblOption15_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_6.Size = New System.Drawing.Size(153, 13)
        Me._lblOption15_6.TabIndex = 39
        Me._lblOption15_6.Tag = "5027"
        Me._lblOption15_6.Text = "Agent Suspense Account:"
        '
        '_lblOption15_4
        '
        Me._lblOption15_4.AutoSize = True
        Me._lblOption15_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_4.Location = New System.Drawing.Point(10, 159)
        Me._lblOption15_4.Name = "_lblOption15_4"
        Me._lblOption15_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_4.Size = New System.Drawing.Size(225, 13)
        Me._lblOption15_4.TabIndex = 38
        Me._lblOption15_4.Tag = "5027"
        Me._lblOption15_4.Text = "Intermediary Write Off Account Code:"
        '
        '_lblOption15_3
        '
        Me._lblOption15_3.AutoSize = True
        Me._lblOption15_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_3.Location = New System.Drawing.Point(10, 135)
        Me._lblOption15_3.Name = "_lblOption15_3"
        Me._lblOption15_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_3.Size = New System.Drawing.Size(197, 13)
        Me._lblOption15_3.TabIndex = 37
        Me._lblOption15_3.Tag = "153"
        Me._lblOption15_3.Text = "Creditor Write Off Account Code:"
        '
        '_lblOption15_2
        '
        Me._lblOption15_2.AutoSize = True
        Me._lblOption15_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_2.Location = New System.Drawing.Point(10, 110)
        Me._lblOption15_2.Name = "_lblOption15_2"
        Me._lblOption15_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_2.Size = New System.Drawing.Size(189, 13)
        Me._lblOption15_2.TabIndex = 36
        Me._lblOption15_2.Tag = "152"
        Me._lblOption15_2.Text = "Debtor Write Off Account Code:"
        '
        '_lblOption15_1
        '
        Me._lblOption15_1.AutoSize = True
        Me._lblOption15_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_1.Location = New System.Drawing.Point(10, 86)
        Me._lblOption15_1.Name = "_lblOption15_1"
        Me._lblOption15_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_1.Size = New System.Drawing.Size(177, 13)
        Me._lblOption15_1.TabIndex = 35
        Me._lblOption15_1.Tag = "151"
        Me._lblOption15_1.Text = "Currency Loss Account Code:"
        '
        '_lblOption15_5
        '
        Me._lblOption15_5.AutoSize = True
        Me._lblOption15_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_5.Location = New System.Drawing.Point(12, 35)
        Me._lblOption15_5.Name = "_lblOption15_5"
        Me._lblOption15_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_5.Size = New System.Drawing.Size(120, 13)
        Me._lblOption15_5.TabIndex = 34
        Me._lblOption15_5.Tag = "60,M"
        Me._lblOption15_5.Text = "Cheque Production:"
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.SystemColors.Control
        Me.Label26.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label26.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label26.Location = New System.Drawing.Point(375, 236)
        Me.Label26.Name = "Label26"
        Me.Label26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label26.Size = New System.Drawing.Size(105, 28)
        Me.Label26.TabIndex = 33
        Me.Label26.Text = "Use Default Printer Settings."
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblOption15_0
        '
        Me._lblOption15_0.AutoSize = True
        Me._lblOption15_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_0.Location = New System.Drawing.Point(10, 63)
        Me._lblOption15_0.Name = "_lblOption15_0"
        Me._lblOption15_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_0.Size = New System.Drawing.Size(184, 13)
        Me._lblOption15_0.TabIndex = 32
        Me._lblOption15_0.Tag = "150"
        Me._lblOption15_0.Text = "Currency Gains Account Code:"
        '
        'lblChequeExportPath
        '
        Me.lblChequeExportPath.AutoSize = True
        Me.lblChequeExportPath.BackColor = System.Drawing.SystemColors.Control
        Me.lblChequeExportPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChequeExportPath.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChequeExportPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChequeExportPath.Location = New System.Drawing.Point(5, 339)
        Me.lblChequeExportPath.Name = "lblChequeExportPath"
        Me.lblChequeExportPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChequeExportPath.Size = New System.Drawing.Size(126, 13)
        Me.lblChequeExportPath.TabIndex = 31
        Me.lblChequeExportPath.Tag = "158"
        Me.lblChequeExportPath.Text = "Cheque Export Path:"
        '
        'chkSplitReceipt5091
        '
        Me.chkSplitReceipt5091.AutoSize = True
        Me.chkSplitReceipt5091.Location = New System.Drawing.Point(441, 269)
        Me.chkSplitReceipt5091.Name = "chkSplitReceipt5091"
        Me.chkSplitReceipt5091.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkSplitReceipt5091.Size = New System.Drawing.Size(86, 17)
        Me.chkSplitReceipt5091.TabIndex = 48
        Me.chkSplitReceipt5091.Tag = "5091"
        Me.chkSplitReceipt5091.Text = "Split Receipt"
        Me.chkSplitReceipt5091.UseVisualStyleBackColor = True
        '
        'chkOption5095
        '
        Me.chkOption5095.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5095.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5095.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5095.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5095.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5095.Location = New System.Drawing.Point(299, 396)
        Me.chkOption5095.Name = "chkOption5095"
        Me.chkOption5095.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5095.Size = New System.Drawing.Size(311, 35)
        Me.chkOption5095.TabIndex = 49
        Me.chkOption5095.Tag = "5095"
        Me.chkOption5095.Text = "Single Commission and Tax Posting to Sub-Agent Account"
        Me.chkOption5095.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkOption5095.UseVisualStyleBackColor = False
        Me.chkOption5095.Visible = False
        '
        'chkSingleCashListRecieptPerAllocation
        '
        Me.chkSingleCashListRecieptPerAllocation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSingleCashListRecieptPerAllocation.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkSingleCashListRecieptPerAllocation.Location = New System.Drawing.Point(6, 571)
        Me.chkSingleCashListRecieptPerAllocation.Name = "chkSingleCashListRecieptPerAllocation"
        Me.chkSingleCashListRecieptPerAllocation.Size = New System.Drawing.Size(287, 37)
        Me.chkSingleCashListRecieptPerAllocation.TabIndex = 50
        Me.chkSingleCashListRecieptPerAllocation.Tag = "5087"
        Me.chkSingleCashListRecieptPerAllocation.Text = "Single Cash List Reciept/Payment Per Allocation:"
        Me.chkSingleCashListRecieptPerAllocation.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkSingleCashListRecieptPerAllocation.UseVisualStyleBackColor = False
        '
        'chk5143
        '
        Me.chk5143.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk5143.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chk5143.Location = New System.Drawing.Point(299, 543)
        Me.chk5143.Name = "chk5143"
        Me.chk5143.Size = New System.Drawing.Size(287, 40)
        Me.chk5143.TabIndex = 51
        Me.chk5143.Tag = "5143"
        Me.chk5143.Text = "Include Insurer Agent Payment as part of Multi Step Approval"
        Me.chk5143.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chk5143.UseVisualStyleBackColor = False
        '
        'Chk_accountaccesslimited
        '
        Me.Chk_accountaccesslimited.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Chk_accountaccesslimited.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Chk_accountaccesslimited.Location = New System.Drawing.Point(6, 406)
        Me.Chk_accountaccesslimited.Name = "Chk_accountaccesslimited"
        Me.Chk_accountaccesslimited.Size = New System.Drawing.Size(287, 30)
        Me.Chk_accountaccesslimited.TabIndex = 55
        Me.Chk_accountaccesslimited.Tag = "5152"
        Me.Chk_accountaccesslimited.Text = "Accounts access limited to user's branch access:"
        Me.Chk_accountaccesslimited.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.Chk_accountaccesslimited.UseVisualStyleBackColor = False
        '
        'chk5208
        '
        Me.chk5208.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk5208.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chk5208.Location = New System.Drawing.Point(299, 583)
        Me.chk5208.Name = "chk5208"
        Me.chk5208.Size = New System.Drawing.Size(287, 21)
        Me.chk5208.TabIndex = 56
        Me.chk5208.Tag = "5208"
        Me.chk5208.Text = "Collect further instalments if 'Failed' status" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.chk5208.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chk5208.UseVisualStyleBackColor = False
        '
        'lblRSTolerance
        '
        Me.lblRSTolerance.AutoSize = True
        Me.lblRSTolerance.BackColor = System.Drawing.SystemColors.Control
        Me.lblRSTolerance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRSTolerance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRSTolerance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRSTolerance.Location = New System.Drawing.Point(7, 376)
        Me.lblRSTolerance.Name = "lblRSTolerance"
        Me.lblRSTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRSTolerance.Size = New System.Drawing.Size(220, 13)
        Me.lblRSTolerance.TabIndex = 57
        Me.lblRSTolerance.Tag = "5242"
        Me.lblRSTolerance.Text = "Agent Auto Reconciliation Tolerance: "
        '
        'txtRSTolerance
        '
        Me.txtRSTolerance.AcceptsReturn = True
        Me.txtRSTolerance.AccessibleDescription = "Agent Auto Reconciliation Tolerance:"
        Me.txtRSTolerance.BackColor = System.Drawing.SystemColors.Window
        Me.txtRSTolerance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRSTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRSTolerance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRSTolerance.Location = New System.Drawing.Point(229, 374)
        Me.txtRSTolerance.MaxLength = 0
        Me.txtRSTolerance.Name = "txtRSTolerance"
        Me.txtRSTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRSTolerance.Size = New System.Drawing.Size(125, 20)
        Me.txtRSTolerance.TabIndex = 58
        Me.txtRSTolerance.Tag = "5242,ValidateNumeric"
        '
        'lblRSCurrency
        '
        Me.lblRSCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblRSCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRSCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRSCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRSCurrency.Location = New System.Drawing.Point(376, 376)
        Me.lblRSCurrency.Name = "lblRSCurrency"
        Me.lblRSCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRSCurrency.Size = New System.Drawing.Size(96, 25)
        Me.lblRSCurrency.TabIndex = 59
        Me.lblRSCurrency.Tag = "5243"
        Me.lblRSCurrency.Text = "Currency"
        '
        'cboOption5243
        '
        Me.cboOption5243.AccessibleDescription = "Agent Auto Reconciliation Tolerance Currency"
        Me.cboOption5243.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5243.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5243.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5243.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOption5243.Location = New System.Drawing.Point(442, 373)
        Me.cboOption5243.Name = "cboOption5243"
        Me.cboOption5243.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5243.Size = New System.Drawing.Size(125, 21)
        Me.cboOption5243.TabIndex = 60
        Me.cboOption5243.Tag = "5243"
        Me.cboOption5243.Text = "cboOption5243"
        '
        'txtOption5247
        '
        Me.txtOption5247.AcceptsReturn = True
        Me.txtOption5247.AccessibleDescription = "Tolerance Days:"
        Me.txtOption5247.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5247.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5247.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5247.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5247.Location = New System.Drawing.Point(566, 182)
        Me.txtOption5247.MaxLength = 0
        Me.txtOption5247.Name = "txtOption5247"
        Me.txtOption5247.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5247.Size = New System.Drawing.Size(59, 20)
        Me.txtOption5247.TabIndex = 62
        Me.txtOption5247.Tag = "5247,ValidateNumeric"
        '
        'lblOption5247
        '
        Me.lblOption5247.AutoSize = True
        Me.lblOption5247.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption5247.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption5247.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption5247.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption5247.Location = New System.Drawing.Point(460, 186)
        Me.lblOption5247.Name = "lblOption5247"
        Me.lblOption5247.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption5247.Size = New System.Drawing.Size(100, 13)
        Me.lblOption5247.TabIndex = 61
        Me.lblOption5247.Tag = "5247"
        Me.lblOption5247.Text = "Tolerance Days:"
        '
        'chkOption5246
        '
        Me.chkOption5246.AutoSize = True
        Me.chkOption5246.Location = New System.Drawing.Point(441, 158)
        Me.chkOption5246.Name = "chkOption5246"
        Me.chkOption5246.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5246.Size = New System.Drawing.Size(194, 17)
        Me.chkOption5246.TabIndex = 63
        Me.chkOption5246.Tag = "5246"
        Me.chkOption5246.Text = "Auto Reconciliation on Cancellation"
        Me.chkOption5246.UseVisualStyleBackColor = True
        '
        'txtOption5248
        '
        Me.txtOption5248.AcceptsReturn = True
        Me.txtOption5248.AccessibleDescription = "Tolerance Amount:"
        Me.txtOption5248.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5248.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5248.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5248.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5248.Location = New System.Drawing.Point(566, 208)
        Me.txtOption5248.MaxLength = 0
        Me.txtOption5248.Name = "txtOption5248"
        Me.txtOption5248.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5248.Size = New System.Drawing.Size(59, 20)
        Me.txtOption5248.TabIndex = 65
        Me.txtOption5248.Tag = "5248,ValidateNumeric"
        '
        'lblOption5248
        '
        Me.lblOption5248.AutoSize = True
        Me.lblOption5248.BackColor = System.Drawing.SystemColors.Control
        Me.lblOption5248.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOption5248.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOption5248.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOption5248.Location = New System.Drawing.Point(444, 212)
        Me.lblOption5248.Name = "lblOption5248"
        Me.lblOption5248.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOption5248.Size = New System.Drawing.Size(115, 13)
        Me.lblOption5248.TabIndex = 64
        Me.lblOption5248.Tag = "5248"
        Me.lblOption5248.Text = "Tolerance Amount:"
        '
        'chkOption5264
        '
        Me.chkOption5264.AutoSize = True
        Me.chkOption5264.Location = New System.Drawing.Point(299, 420)
        Me.chkOption5264.Name = "chkOption5264"
        Me.chkOption5264.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOption5264.Size = New System.Drawing.Size(311, 17)
        Me.chkOption5264.TabIndex = 66
        Me.chkOption5264.Tag = "5264"
        Me.chkOption5264.Text = "Display Commission at Commission Band Level"
        Me.chkOption5264.UseVisualStyleBackColor = True
        '
        'frmAccountsGeneral
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(652, 615)
        Me.Controls.Add(Me.txtOption5248)
        Me.Controls.Add(Me.lblOption5248)
        Me.Controls.Add(Me.chkOption5264)
        Me.Controls.Add(Me.chkOption5246)
        Me.Controls.Add(Me.txtOption5247)
        Me.Controls.Add(Me.lblOption5247)
        Me.Controls.Add(Me.cboOption5243)
        Me.Controls.Add(Me.lblRSCurrency)
        Me.Controls.Add(Me.txtRSTolerance)
        Me.Controls.Add(Me.lblRSTolerance)
        Me.Controls.Add(Me.chk5208)
        Me.Controls.Add(Me.Chk_accountaccesslimited)
        Me.Controls.Add(Me.chk5143)
        Me.Controls.Add(Me.chkSingleCashListRecieptPerAllocation)
        Me.Controls.Add(Me.chkOption5095)
        Me.Controls.Add(Me.chkSplitReceipt5091)
        Me.Controls.Add(Me.txtRoundOffZero)
        Me.Controls.Add(Me.cboOption5069)
        Me.Controls.Add(Me.chkPaymentTypeEditedonParty)
        Me.Controls.Add(Me.txtCancelPolicyWriteOffAcCode)
        Me.Controls.Add(Me.txtCurGainLossAllocationLimit)
        Me.Controls.Add(Me.chkOption5059)
        Me.Controls.Add(Me.chkOption5058)
        Me.Controls.Add(Me.txtAgentSuspenseAccount)
        Me.Controls.Add(Me.chkOption5038)
        Me.Controls.Add(Me.chkOption5037)
        Me.Controls.Add(Me.Text5)
        Me.Controls.Add(Me.chkOption5016)
        Me.Controls.Add(Me.cmdClearPaymentLetter)
        Me.Controls.Add(Me.cmdClearReceiptLetter)
        Me.Controls.Add(Me.cmdPaymentLetter)
        Me.Controls.Add(Me.cmdReceiptLetter)
        Me.Controls.Add(Me.Text4)
        Me.Controls.Add(Me.Text3)
        Me.Controls.Add(Me.Text2)
        Me.Controls.Add(Me.Check1)
        Me.Controls.Add(Me.chkOption1031)
        Me.Controls.Add(Me.cboOption60)
        Me.Controls.Add(Me.chkOption51)
        Me.Controls.Add(Me.chkOption50)
        Me.Controls.Add(Me.txtPaymentLetter)
        Me.Controls.Add(Me.txtReceiptLetter)
        Me.Controls.Add(Me.chkOption5001)
        Me.Controls.Add(Me.Text1)
        Me.Controls.Add(Me.txtChequeLetter)
        Me.Controls.Add(Me.ChkOption66)
        Me.Controls.Add(Me.cmdChequeLetter)
        Me.Controls.Add(Me.cmdClearChequeLetter)
        Me.Controls.Add(Me.txtChequeExportPath)
        Me.Controls.Add(Me.lblRoundOffZero5080)
        Me.Controls.Add(Me.lblOption5069)
        Me.Controls.Add(Me._lblOption5061_7)
        Me.Controls.Add(Me.lblPercentageMark)
        Me.Controls.Add(Me.lblOption5060)
        Me.Controls.Add(Me._lblOption15_6)
        Me.Controls.Add(Me._lblOption15_4)
        Me.Controls.Add(Me._lblOption15_3)
        Me.Controls.Add(Me._lblOption15_2)
        Me.Controls.Add(Me._lblOption15_1)
        Me.Controls.Add(Me._lblOption15_5)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me._lblOption15_0)
        Me.Controls.Add(Me.lblChequeExportPath)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmAccountsGeneral"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblOption5061()
        Me.lblOption5061(7) = _lblOption5061_7
    End Sub
    Sub InitializelblOption15()
        Me.lblOption15(6) = _lblOption15_6
        Me.lblOption15(4) = _lblOption15_4
        Me.lblOption15(3) = _lblOption15_3
        Me.lblOption15(2) = _lblOption15_2
        Me.lblOption15(1) = _lblOption15_1
        Me.lblOption15(5) = _lblOption15_5
        Me.lblOption15(0) = _lblOption15_0
    End Sub
    Friend WithEvents chkSplitReceipt5091 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5095 As System.Windows.Forms.CheckBox
    Friend WithEvents chkSingleCashListRecieptPerAllocation As System.Windows.Forms.CheckBox
    Friend WithEvents chk5143 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk_accountaccesslimited As System.Windows.Forms.CheckBox
    Friend WithEvents chk5208 As CheckBox
    Private WithEvents lblRSTolerance As Label
    Public WithEvents txtRSTolerance As TextBox
    Public WithEvents lblRSCurrency As Label
    Public WithEvents cboOption5243 As ComboBox
    Public WithEvents txtOption5247 As TextBox
    Private WithEvents lblOption5247 As Label
    Friend WithEvents chkOption5246 As CheckBox
    Public WithEvents txtOption5248 As TextBox
    Private WithEvents lblOption5248 As Label
    Friend WithEvents chkOption5264 As System.Windows.Forms.CheckBox
#End Region
End Class
